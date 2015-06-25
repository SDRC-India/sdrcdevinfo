

PageName = {
    News: "news",
    Innovations: "Innovations",
    FAQ: "Innovations"
}
function CheckIfSiteAdminLoggedInCMS(hLoggedInUserNId) {
    var RetVal;
    if (hLoggedInUserNId != undefined && hLoggedInUserNId != null) {
        var InputParam = hLoggedInUserNId;
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: "../libraries/aspx/Callback.aspx",
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
        RetVal = 'false';
    }
    return RetVal;
}

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
                if (CheckIfSiteAdminLoggedInCMS(hLoggedInUserNIdPart) == 'true') {
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
        // di_jq("#aHome").attr("class", "navActive")

        // Hide background color div
        di_jq("#apDiv3").css("display", "none");

        // Set default area count of selected database
        //z("lblDefalutAreaCount").innerHTML = dbareacount;
        //LoadLanguage(hlngcode);


        // ************************************************
        // create Form Tag with hidden input boxes
        // ************************************************
        createFormHiddenInputs("frmArticles", "POST");
        SetCommonLinksHref("frmArticles", "POST");

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

        if (readCookie('SlideStartIndex') == null) {
            createCookie('SlideStartIndex', 0);
        }
        // start for home page slider
        HideLoadingDiv();

        z('hselindo').value = "";
        z('hselareao').value = "";

        z('hselind').value = "";
        z('hselarea').value = "";
        z('hdsby').value = "qds";
    }
    catch (err) { }
}


var CloseImgSrc = "../../stock/themes/default/images/close.png";




// set the variables and the page events


//----------------------------------------Method For retreving articles from database Tags-------------------------------------------------


var TagsArray = [];
var SelectedMenuCat;

function GetArticlesByTags(Tag) {
    try {
        // fill tag array
        InputSelection(Tag);
        // check if tag array is having at least 1 element, then call method to get data fotr articles
        if (TagsArray.length > 0) {
            GetArticlesByMenuAndTags(TagsArray, 1);
            ShowTopOptions(false);
        }
        else {
            GetArticleByMenuCategory(1);
            /// ShowTopOptions(true);
        }
        ManageDefaults();
    }
    catch (err) {
        HandleJsError(err, "GetArticlesByTags");
    }
}


// this method is used for assigning selectd tags to tags array
function InputSelection(val) {
    var index = -1;
    try {
        index = GetArrayIndex(TagsArray, val);
        if (index == -1) {
            TagsArray.push(val);
        }
        else {
            TagsArray.splice(index, 1);
        }
    }
    catch (err) {
        HandleJsError(err, "InputSelection");
    }
}

// get index of array
function GetArrayIndex(arr, val) {
    var index = -1;
    for (var i = 0; i < arr.length; i++)
        if (arr[i] == val) {
            index = i;
            break;
        }
    return index;
}


function ShowArticleByPage(PageNo) {
    GetArticlesByMenuAndTags(TagsArray, PageNo);
}


function GetArticleByMenuCategory(PageNo) {
    var MenuCategory = SelectedMenuCat;
    var Tags = '';
    var IsHiddenArticlesVisible = IsArticlesVisible();
    var CallbackPageName = "../libraries/aspx/Callback.aspx";
    InputParam = MenuCategory;
    InputParam += ParamDelimiter + Tags;
    InputParam += ParamDelimiter + PageNo;
    InputParam += ParamDelimiter + IsHiddenArticlesVisible;
    di_jq.ajax({
        type: "POST",
        url: CallbackPageName,
        data: { 'callback': '1024', 'param1': InputParam },
        async: true,
        beforeSend: function () {
            ApplyMaskingDiv();
        },
        success: function (data) {
            try {
                z('cphMainContent_div_content').innerHTML = '';
                if (data != "") {
                    z('cphMainContent_div_content').innerHTML = data;
                    ManageDefaults();
                }
            }
            catch (ex) {
                alert("Error : " + ex.message);
            }
        },
        error: function () {
            
        },
        complete: function () {
            RemoveMaskingDiv();
        },
        cache: false
    });
}

function GetAllArticlesByMenuCategory(MenuCategory) {
    var Tags = '';
    var IsHiddenArticlesVisible = IsArticlesVisible();
    var CallbackPageName = "../libraries/aspx/Callback.aspx";
    InputParam = MenuCategory;
    InputParam += ParamDelimiter + Tags;
    InputParam += ParamDelimiter + 1;
    InputParam += ParamDelimiter + IsHiddenArticlesVisible;
    di_jq.ajax({
        type: "POST",
        url: CallbackPageName,
        data: { 'callback': '1024', 'param1': InputParam },
        async: true,
        beforeSend: function () {
            ApplyMaskingDiv();
        },
        success: function (data) {
            try {
                z('cphMainContent_div_content').innerHTML = '';
                if (data != "") {
                    z('cphMainContent_div_content').innerHTML = data;

                }
                GetTagsListByMenuCategory(MenuCategory, '');
                ClearArticleMenuSelection();
                SelectArticleMenuOption(MenuCategory, '');
                ManageDefaults();
            }
            catch (ex) {
                alert("Error : " + ex.message);
            }
        },
        error: function () {
            
        },
        complete: function () {
            RemoveMaskingDiv();
        },
        cache: false
    });
}

function GetArticlesByMenuAndTags(Tags, PageNo) {
    var IsHiddenArticlesVisible = IsArticlesVisible();
    MenuCategory = SelectedMenuCat;
    var CallbackPageName = "../libraries/aspx/Callback.aspx";
    InputParam = MenuCategory;
    InputParam += ParamDelimiter + Tags;
    InputParam += ParamDelimiter + PageNo;
    InputParam += ParamDelimiter + IsHiddenArticlesVisible;
    di_jq.ajax({
        type: "POST",
        url: CallbackPageName,
        data: { 'callback': '1024', 'param1': InputParam },
        async: true,
        beforeSend: function () {
            ApplyMaskingDiv();
        },
        success: function (data) {
            try {
                z('cphMainContent_div_content').innerHTML = '';
                if (data != "") {
                    z('cphMainContent_div_content').innerHTML = data;
                    RecheckTagsCheckBoxes(Tags);
                    z('cphMainContent_HdnTagIds').value = FormatArrayInputCommaSeperatedToList(Tags);
                    ManageDefaults();

                    if (MenuCategory != "") {
                        for (var i = 0; i < ObjMenuCategory.length; i++) {
                            if (MenuCategory.toLowerCase() == ObjMenuCategory[i].MenuCategory.toLowerCase()) {
                                var HeaderText = ObjMenuCategory[i].HeaderText;
                                var HeaderDesc = ObjMenuCategory[i].HeaderDesc;
                                SetMaxchar(document.getElementById("cphMainContent_SpanMenuCategoryHeading"), HeaderText);
                                SetMaxchar(document.getElementById("cphMainContent_SpanHeaderDescription"), HeaderDesc);
                            }
                        }
                    }


                }
                else {
                    z('cphMainContent_HdnTagIds').value = '';
                }
            }
            catch (ex) {
                alert("Error : " + ex.message);
            }
        },
        error: function () {
            
        },
        complete: function () {
            RemoveMaskingDiv();
        },
        cache: false
    });
}


function GetTagsListByMenuCategory(MenuCategory, TagListToSetCheckBoxChecked) {
    var IsHiddenArticlesVisible = IsArticlesVisible();
    var CallbackPageName = "../libraries/aspx/Callback.aspx";
    InputParam = MenuCategory;
    InputParam += ParamDelimiter + IsHiddenArticlesVisible;
    di_jq.ajax({
        type: "POST",
        url: CallbackPageName,
        data: { 'callback': '1025', 'param1': InputParam },
        async: true,
        beforeSend: function () {
            ApplyMaskingDiv();
        },
        success: function (data) {
            try {
                z('cphMainContent_DVTagsMenu').innerHTML = '';
                z('cphMainContent_DVTags').innerHTML = '';
                if (data != "") {
                    z('cphMainContent_DVTags').style.display = "block";
                    z('cphMainContent_DVTags').innerHTML = "<span style='font-weight:bold'>Tags</span>";
                    z('cphMainContent_DVTagsMenu').innerHTML = data;
                    if (TagListToSetCheckBoxChecked != '' && TagListToSetCheckBoxChecked != undefined) {
                        var TagsArray = [];
                        TagsArray = TagListToSetCheckBoxChecked.split(',');
                        RecheckTagsCheckBoxes(TagsArray);
                        z('cphMainContent_HdnTagIds').value = TagListToSetCheckBoxChecked;
                        for (var ICount = 0; ICount < TagsArray.length; ICount++) {
                            InputSelection(TagsArray[ICount]);
                        }
                    }
                    else {
                        z('cphMainContent_DVTags').style.display = "block";
                    }
                }
            }
            catch (ex) {
                alert("Error : " + ex.message);
            }
        },
        error: function () {
            
        },
        complete: function () {
            RemoveMaskingDiv();
        },
        cache: false
    });
}



function DeleteArticlebyContentId(ContentId, CurrentPageNo) {
    if (ConfirmToDelete('Delete this Article?')) {
        var CallbackPageName = "../libraries/aspx/Callback.aspx";
        InputParam = ContentId;
        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '1026', 'param1': InputParam },
            async: true,
            beforeSend: function () {
                ApplyMaskingDiv();
            },
            success: function (data) {
                try {
                    if (data == "true") {
                        ShowArticleByPage(CurrentPageNo);
                        SetHideAndEditButtonVisiblity(false);
                        ManageDefaults();
                    }
                }
                catch (ex) {
                    alert("Error : " + ex.message);
                }
            },
            error: function () {
                
            },
            complete: function () {
                RemoveMaskingDiv();
            },
            cache: false
        });
    }
}

function ShowHideArticlebyContentId(ContentId, IsHidden, CurrentPageNo) {
    var ConfirmMsg = '';
    if (IsHidden == 'True') {
        ConfirmMsg = 'Hide this Article ?';
    }
    else {
        ConfirmMsg = 'Unhide this Article ?';
    }
    if (ConfirmToDelete(ConfirmMsg)) {
        var CallbackPageName = "../libraries/aspx/Callback.aspx";
        InputParam = ContentId;
        InputParam += ParamDelimiter + IsHidden;
        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '1027', 'param1': InputParam },
            async: true,
            beforeSend: function () {
                ApplyMaskingDiv();
            },
            success: function (data) {
                try {
                    if (data == "true") {
                        ShowArticleByPage(CurrentPageNo);
                        ManageDefaults();
                    }
                }
                catch (ex) {
                    alert("Error : " + ex.message);
                }
            },
            error: function () {
                
            },
            complete: function () {
                RemoveMaskingDiv();
            },
            cache: false
        });
    }
}

function SetHideAndEditButtonVisiblity(status) {
    if (status == false) {
        z("cphMainContent_EditPage").style.display = "none";
        z("cphMainContent_DeleteNews").style.display = "none";
    }
    else {
        z("cphMainContent_EditPage").style.display = "block";
        z("cphMainContent_DeleteNews").style.display = "block";
    }
}
function GetMenuCategoryByUrl() {
    var Url = document.URL;
    var MenuCategory = '';
    if (Url.indexOf("/articles/") != -1) {
        if (Url.split('/articles/').length > 1) {
            MenuCategory = Url.split('/articles/')[1];
            return MenuCategory;
        }
    }
}

function MenuCategoryIsInUrl() {
    for (var i = 0; i < ObjMenuCategory.length; i++) {
        if (GetMenuCategoryByUrl().toLowerCase() == ObjMenuCategory[i].MenuCategory.toLowerCase()) {
            return true;
            break;
        }
    }
    return false;
}

function CheckIsNumericValue(InputValue) {
    var ValueForTest;
    var MenuCategory = '';
    if (InputValue != '') {
        ValueForTest = InputValue.substr(InputValue.length - 1, 1);
        if (ValueForTest / 1) {
            return true;
        }
        else {
            return false;
        }
    }
}

function RecheckTagsCheckBoxes(Tags) {
    var HiddenCheckBox = '';
    for (var Icount = 0; Icount < Tags.length; Icount++) {
        if (z('Chk' + Tags[Icount]) != undefined && z('Chk' + Tags[Icount]) != null && z('Chk' + Tags[Icount]).value != undefined && z('Chk' + Tags[Icount]).value != null) {
            if (z('Chk' + Tags[Icount]).value == Tags[Icount])
                z('Chk' + Tags[Icount]).checked = 'true';
            HiddenCheckBox = Tags[Icount];
        }
    }
}


function FormatArrayInputCommaSeperatedToList(InputArr) {
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


//----------------------------------------------------------------------------------------------------
var ObjMenuCategory;
function CreateLeftLink(MenuCategory, IsArticlesVisible, SelectedPageName) {
    var HTMLStr = "";
    var CallbackPageName = "../libraries/aspx/Callback.aspx";
    var InputParam = SelectedPageName;
    di_jq.ajax({
        type: "Get",
        url: CallbackPageName,
        data: { 'callback': '1029', 'param1': InputParam },
        async: true,
        beforeSend: function () {
            ApplyMaskingDiv();
        },
        success: function (data) {
            try {
                var url = document.URL;
                if (url.indexOf("articles/") != -1) {
                    url = url.substring(0, url.indexOf("articles/"));
                    url = url + "articles/";
                }
                if (data != "") {
                    ObjMenuCategory = di_jq.parseJSON(data);
                    for (var i = 0; i < ObjMenuCategory.length; i++) {
                        if (SelectedPageName.toLowerCase() == ObjMenuCategory[i].PageName.toLowerCase()) {
                            var CategoryName = ObjMenuCategory[i].MenuCategory;
                            var LinkText = ObjMenuCategory[i].LinkText;
                            HTMLStr += "<li><a id=aLft" + CategoryName + " " + "Class=nav" + " " + "href=" + url + CategoryName + ">" + LinkText + "</a></li>";

                        }
                    }
                    z('LeftcatMenuLink').innerHTML = HTMLStr;
                    SelectArticleMenuOption(MenuCategory, SelectedPageName);

                    if (MenuCategoryIsInUrl()) {
                        ShowTopOptions(false);

                    }
                    else {
                        ShowTopOptions(true);
                    }
                }
            }
            catch (ex) {
                alert("Error : " + ex.message);
            }
        },
        error: function () {
            
        },
        complete: function () {
            RemoveMaskingDiv();
        },
        cache: false
    });
}

function SelectArticleMenuOption(MenuCategory, SelectedPageName) {
    var IsArtile = true;
    try {
        SelectedMenuCat = MenuCategory;
        ClearArticleMenuSelection();
        if (SelectedPageName != null && SelectedPageName != undefined) {
            switch (SelectedPageName.toLowerCase()) {
                case PageName.News.toLowerCase():
                    CtrlName = "aNews";
                    break;
                case PageName.Innovations.toLowerCase():
                    CtrlName = "aInnovations";
                    break;
            }
        }

        if (MenuCategory != "") {
            for (var i = 0; i < ObjMenuCategory.length; i++) {
                var CategoryName = ObjMenuCategory[i].MenuCategory;
                var HeaderText = ObjMenuCategory[i].HeaderText;
                var HeaderDesc = ObjMenuCategory[i].HeaderDesc;

                //  SetMaxchar(document.getElementById("cphMainContent_SpanMenuCategoryHeading"), HeaderText);
                if (CategoryName.toLowerCase() == MenuCategory.toLowerCase()) {

                    if (MenuCategoryIsInUrl()) {
                        IsArtile = false;

                        if (document.getElementById("cphMainContent_SpanMenuCategoryHeading") != null && document.getElementById("cphMainContent_SpanMenuCategoryHeading") != undefined) {
                            SetMaxchar(document.getElementById("cphMainContent_SpanMenuCategoryHeading"), HeaderText);
                        }
                        if (document.getElementById("cphMainContent_SpanHeaderDescription") != null && document.getElementById("cphMainContent_SpanHeaderDescription") != undefined) {
                            SetMaxchar(document.getElementById("cphMainContent_SpanHeaderDescription"), HeaderDesc);
                        }
                    }
                    break;
                }
            }
            if (IsArtile) {
                SetMaxchar(document.getElementById("cphMainContent_SpanMenuCategoryHeading"), document.getElementById("cphMainContent_SpanMenuCategoryHeading").innerHTML);
            }
            if (z('aLft' + CategoryName) != null && z('aLft' + CategoryName).className != null) {
                z('aLft' + CategoryName).className = "adm_lft_nav_seld";
            }

            if (z(CtrlName) != null && z(CtrlName).className != null && z(CtrlName).className != undefined) {
                z(CtrlName).className = "navActive";
            }
        }
    }
    catch (err) { }
}

function ClearArticleMenuSelection() {
    try {
        for (var i = 0; i < ObjMenuCategory.length; i++) {
            var CategoryName = ObjMenuCategory[i].MenuCategory;
            if (z('aLft' + CategoryName) != null && z('aLft' + CategoryName).className != null) {
                z('aLft' + CategoryName).className = "nav";
            }
        }
    }
    catch (err) { }
}


function CreateMenuCategoryOptionsList(OptionTag, SelectedMenuCat, SelectedPageName) {
    var HTMLStr = "";
    var CallbackPageName = "Callback.aspx";
    // var SelectedPageName = GetUrlVars()['MC'];
    var InputParam = SelectedPageName;
    di_jq.ajax({
        type: "Get",
        url: CallbackPageName,
        data: { 'callback': '1029', 'param1': InputParam },
        async: true,
        beforeSend: function () {
            ApplyMaskingDiv();
        },
        success: function (data) {
            try {
                HTMLStr += "<select id='PageCategory' style='width:50%' runat=server>";
                if (data != "") {

                    ObjMenuCategory = di_jq.parseJSON(data);
                    for (var i = 0; i < ObjMenuCategory.length; i++) {
                        var CategoryName = ObjMenuCategory[i].MenuCategory;
                        var PageName = ObjMenuCategory[i].PageName;
                        if (PageName.toLowerCase() == SelectedPageName.toLowerCase()) {
                            if (CategoryName.toLowerCase() == SelectedMenuCat.toLowerCase()) {
                                HTMLStr += "<option" + " " + "id=Opt" + CategoryName + " " + "value=" + CategoryName + " " + "Selected=true >" + CategoryName + "</option>";
                            }
                            else {
                                HTMLStr += "<option" + " " + "id=Opt" + CategoryName + " " + "value=" + CategoryName + ">" + CategoryName + "</option>";
                            }
                        }
                    }
                    HTMLStr += "</<select>";
                    OptionTag.innerHTML = HTMLStr;
                }
            }
            catch (ex) {
                alert("Error : " + ex.message);
            }
        },
        error: function () {
            
        },
        complete: function () {
            RemoveMaskingDiv();
        },
        cache: false
    });
}


//---------------------------------------------Start Manage MenuCategories List Methods---------------------------------------------------------------

function AddNewCategoryPopUp(isOpen) {

    var ConfirmAlertMsg = '';
    var CategorySelected = false;
    try {
        if (isOpen) {
            ApplyMaskingDiv();
            SetCloseButtonInPopupDiv(di_jq('#AddNEditCategoryPopup'), 'EditCategoryPopUp', CloseImgSrc);
            di_jq('#AddNEditCategoryPopup').show('slow');
            di_jq('#txtMenuCategory').attr("disabled", "");
            GetWindowCentered(z("AddNEditCategoryPopup"), 710, 450);
            di_jq('#txtMenuCategory').val('');
            di_jq('#txtLeftLinkText').val('');
            di_jq('#txtHeaderText').val('');
            di_jq('#txtHeaderDescText').val('');
            di_jq('#btnEditCategory').css({ display: "none" });
            di_jq('#btnAddCategory').css({ display: "block" });
            di_jq('#Lang_HddAdd').css({ display: "block" });
            di_jq('#Lang_HddEdit').css({ display: "none" });
        }
        else {
            di_jq('#AddNEditCategoryPopup').hide('slow');
            RemoveMaskingDiv();
        }
    }
    catch (err) { }
}


function EditCategoryPopUp(isOpen) {
    var CategorySelected = false;
    try {
        var SelectedPageName = GetUrlVars()['MC'];
        if (isOpen) {
            for (var i = 0; i < ObjMenuCategory.length; i++) {
                if (SelectedPageName.toLowerCase() == ObjMenuCategory[i].PageName.toLowerCase()) {
                    if (z("MenuCategory_" + i).checked == true) {
                        var CategoryName = ObjMenuCategory[i].MenuCategory;
                        var LinkText = ObjMenuCategory[i].LinkText;
                        var HeaderText = ObjMenuCategory[i].HeaderText;
                        var HeaderDesc = ObjMenuCategory[i].HeaderDesc;
                        ApplyMaskingDiv();
                        SetCloseButtonInPopupDiv(di_jq('#AddNEditCategoryPopup'), 'EditCategoryPopUp', CloseImgSrc);
                        di_jq('#AddNEditCategoryPopup').show('slow');
                        GetWindowCentered(z("AddNEditCategoryPopup"), 710, 450);
                        di_jq('#txtMenuCategory').attr("disabled", "disabled");
                        di_jq('#txtMenuCategory').val(CategoryName);
                        di_jq('#txtLeftLinkText').val(LinkText);
                        di_jq('#txtHeaderText').val(HeaderText);
                        di_jq('#txtHeaderDescText').val(HeaderDesc);
                        di_jq('#btnEditCategory').css({ display: "block" });
                        di_jq('#btnAddCategory').css({ display: "none" });
                        di_jq('#Lang_HddAdd').css({ display: "none" });
                        di_jq('#Lang_HddEdit').css({ display: "block" });
                        CategorySelected = true;
                        break;
                    }

                }
            }
            if (!CategorySelected) {
                alert("Select a category to edit.");
            }
        }
        else {
            di_jq('#AddNEditCategoryPopup').hide('slow');
            RemoveMaskingDiv();
        }
    }
    catch (err) { }
}

function DeleteCategoryPopUp() {
    var ConfirmAlertMsg = '';
    var CategorySelected = false;
    try {
        var SelectedPageName = GetUrlVars()['MC'];
        for (var i = 0; i < ObjMenuCategory.length; i++) {
            if (SelectedPageName.toLowerCase() == ObjMenuCategory[i].PageName.toLowerCase()) {
                if (z("MenuCategory_" + i).checked == true) {
                    var CategoryName = ObjMenuCategory[i].MenuCategory;
                    var LinkText = ObjMenuCategory[i].LinkText;
                    var HeaderText = ObjMenuCategory[i].HeaderText;
                    var HeaderDesc = ObjMenuCategory[i].HeaderDesc;
                    ConfirmAlertMsg = 'Deleting this category will delete all the articles associated with it. Do you wish to proceed';
                    if (ConfirmAlertReturn(ConfirmAlertMsg)) {
                        DeleteMenuCategory(SelectedPageName, CategoryName);
                    }
                    CategorySelected = true;
                    break;
                }
            }
        }
        if (!CategorySelected) {
            alert("Select a category to edit.");
        }
    }
    catch (err) { }
}

function ConfirmAlertReturn(msg) {
    return confirm(msg);
}

function DeleteMenuCategory(MenuCategoryToDelete) {
    try {
        var SelectedPageName = GetUrlVars()['MC'];
        if (MenuCategoryToDelete != '') {
            InputParam = MenuCategoryToDelete;
            di_jq.ajax({
                type: "POST",
                url: "Callback.aspx",
                data: { 'callback': '1030', 'param1': InputParam },
                async: true,
                beforeSend: function () {
                    ApplyMaskingDiv();
                },
                success: function (data) {
                    try {
                        if (data == "True") {
                            CreateMenuCategoryGrid('');
                        }
                    }
                    catch (ex) {
                        alert("Error : " + ex.message);
                    }
                },
                error: function () {
                    
                },
                complete: function () {
                    RemoveMaskingDiv();
                },
                cache: false
            });
        }
    }
    catch (err) { }
}
function AddNewCategory() {
    try {
        var SelectedPageName = GetUrlVars()['MC'];
        var CategoryName = txtMenuCategory.value;
        var LeftLinkText = txtLeftLinkText.value;
        var HeaderText = txtHeaderText.value;
        var HeaderDesc = txtHeaderDescText.value;
        var CatAlreadyExist = false;
        for (var i = 0; i < ObjMenuCategory.length; i++) {
            if (ObjMenuCategory[i].MenuCategory.toLowerCase() == txtMenuCategory.value.toLowerCase()) {
                CatAlreadyExist = true;
            }
        }

        if (CategoryName == '') {

            if (z('Lang_MenuCategory_Alert').innerHTML != '' && z('Lang_MenuCategory_Alert').innerHTML != undefined) {
                alert(z('Lang_MenuCategory_Alert').innerHTML);
            }
            else if (z('Lang_MenuCategory_Alert').innerText != '' && z('Lang_MenuCategory_Alert').innerText != undefined) {
                alert(z('Lang_MenuCategory_Alert').innerText);
            }
            else {
                alert("Category name is empty");
            }
        }
        else if (LeftLinkText == '') {
            if (z('Lang_MenuLinkText_Alert').innerHTML != '' && z('Lang_MenuLinkText_Alert').innerHTML != undefined) {
                alert(z('Lang_MenuLinkText_Alert').innerHTML);
            }
            else if (z('Lang_MenuLinkText_Alert').innerText != '' && z('Lang_MenuLinkText_Alert').innerText != undefined) {
                alert(z('Lang_MenuLinkText_Alert').innerText);
            }
            else {
                alert("Link text is empty.");
            }
        }
        else if (HeaderText == '') {
            if (z('Lang_MenuHeader_Alert').innerHTML != '' && z('Lang_MenuHeader_Alert').innerHTML != undefined) {
                alert(z('Lang_MenuHeader_Alert').innerHTML);
            }
            else if (z('Lang_MenuHeader_Alert').innerText != '' && z('Lang_MenuHeader_Alert').innerText != undefined) {
                alert(z('Lang_MenuHeader_Alert').innerText);
            }
            else {
                alert("Header text is empty.");
            }
        }
        //        else if (HeaderDesc == '') {

        //            if (z('Lang_MenuHeaderDesc_Alert').innerHTML != '' && z('Lang_MenuHeaderDesc_Alert').innerHTML != undefined) {
        //                alert(z('Lang_MenuHeaderDesc_Alert').innerHTML);
        //            }
        //            else if (z('Lang_MenuHeaderDesc_Alert').innerText != '' && z('Lang_MenuHeaderDesc_Alert').innerText != undefined) {
        //                alert(z('Lang_MenuHeaderDesc_Alert').innerText);
        //            }
        //            else {
        //                alert("Description is empty.");
        //            }
        //        }
        else if (!ValidateAlphaInput(CategoryName)) {
            alert("Numeric values and special charaters are not allowed.");
        }

        else if (CatAlreadyExist) {
            alert("This menu category already exists. Kindly make another selection.");
        }
        else {
            if (CategoryName.indexOf(' ') != -1) {
                CategoryName = CategoryName.replace(/ /g, '-');
            }
            var InputParam = CategoryName;
            InputParam += ParamDelimiter + LeftLinkText;
            InputParam += ParamDelimiter + HeaderText;
            InputParam += ParamDelimiter + HeaderDesc;
            InputParam += ParamDelimiter + SelectedPageName;
            di_jq.ajax({
                type: "POST",
                url: "Callback.aspx",
                data: { 'callback': '1031', 'param1': InputParam },
                async: true,
                beforeSend: function () {
                    ApplyMaskingDiv();
                },
                success: function (data) {
                    try {
                        if (data == "True") {

                            alert("Category added sucessfully.");
                            di_jq('#AddNEditCategoryPopup').hide('slow');
                            CreateMenuCategoryGrid('');
                        }
                        else {
                            alert("Error while adding category.");
                            di_jq('#AddNEditCategoryPopup').hide('slow');
                        }
                    }
                    catch (ex) {
                        alert("Error : " + ex.message);
                        di_jq('#AddNEditCategoryPopup').hide('slow');
                    }
                },
                error: function () {
                    
                    di_jq('#AddNEditCategoryPopup').hide('slow');
                },
                complete: function () {
                    RemoveMaskingDiv();
                },
                cache: false
            });
        }
    }
    catch (err) { }
}

function EditCategory() {
    var SelectedPageName = GetUrlVars()['MC'];
    var ConfirmAlertMsg = '';
    try {
        ConfirmAlertMsg = 'Are you sure to edit Category';
        if (ConfirmAlertReturn(ConfirmAlertMsg)) {
            var CategoryName = txtMenuCategory.value;
            var LeftLinkText = txtLeftLinkText.value;
            var HeaderText = txtHeaderText.value;
            var HeaderDesc = txtHeaderDescText.value;
            // InputParam += InputParam + SelectedPageName;
            if (LeftLinkText == '') {
                if (z('Lang_MenuLinkText_Alert').innerHTML != '' && z('Lang_MenuLinkText_Alert').innerHTML != undefined) {
                    alert(z('Lang_MenuLinkText_Alert').innerHTML);
                }
                else if (z('Lang_MenuLinkText_Alert').innerText != '' && z('Lang_MenuLinkText_Alert').innerText != undefined) {
                    alert(z('Lang_MenuLinkText_Alert').innerText);
                }
                else {
                    alert("Link text is empty");
                }
            }
            else if (HeaderText == '') {
                if (z('Lang_MenuHeader_Alert').innerHTML != '' && z('Lang_MenuHeader_Alert').innerHTML != undefined) {
                    alert(z('Lang_MenuHeader_Alert').innerHTML);
                }
                else if (z('Lang_MenuHeader_Alert').innerText != '' && z('Lang_MenuHeader_Alert').innerText != undefined) {
                    alert(z('Lang_MenuHeader_Alert').innerText);
                }
                else {
                    alert("Header text is empty");
                }
            }
            //            else if (HeaderDesc == '') {

            //                if (z('Lang_MenuHeaderDesc_Alert').innerHTML != '' && z('Lang_MenuHeaderDesc_Alert').innerHTML != undefined) {
            //                    alert(z('Lang_MenuHeaderDesc_Alert').innerHTML);
            //                }
            //                else if (z('Lang_MenuHeaderDesc_Alert').innerText != '' && z('Lang_MenuHeaderDesc_Alert').innerText != undefined) {
            //                    alert(z('Lang_MenuHeaderDesc_Alert').innerText);
            //                }
            //                else {
            //                    alert("Description is empty");
            //                }
            //            }
            else {
                var InputParam = CategoryName;
                InputParam += ParamDelimiter + LeftLinkText;
                InputParam += ParamDelimiter + HeaderText;
                InputParam += ParamDelimiter + HeaderDesc;
                InputParam += ParamDelimiter + SelectedPageName;
                di_jq.ajax({
                    type: "POST",
                    url: "Callback.aspx",
                    data: { 'callback': '1032', 'param1': InputParam },
                    async: true,
                    beforeSend: function () {
                        ApplyMaskingDiv();
                    },
                    success: function (data) {
                        try {
                            if (data == "True") {
                                alert("Category edited sucessfully.");
                                di_jq('#AddNEditCategoryPopup').hide('slow');
                                CreateMenuCategoryGrid(CategoryName);
                            }
                            else {
                                alert("Error while editing category.");
                                di_jq('#AddNEditCategoryPopup').hide('slow');
                            }
                        }
                        catch (ex) {
                            alert("Error : " + ex.message);
                            di_jq('#AddNEditCategoryPopup').hide('slow');
                        }
                    },
                    error: function () {
                        
                        di_jq('#AddNEditCategoryPopup').hide('slow');
                    },
                    complete: function () {
                        RemoveMaskingDiv();
                    },
                    cache: false
                });
            }
        }
    }
    catch (err) { }
}

function CreateMenuCategoryGrid(CategoryNameToMaintainState) {
    var SelectedPageName = GetUrlVars()['MC'];
    var InputParam = SelectedPageName;
    var HTMLStr = "";
    var CallbackPageName = "Callback.aspx";
    di_jq.ajax({
        type: "Get",
        url: CallbackPageName,
        data: { 'callback': '1029', 'param1': InputParam },
        async: false,
        beforeSend: function () {
            ApplyMaskingDiv();
        },
        success: function (data) {
            try {
                HTMLStr += ' <table border="0" id="AdapUser_Table" cellspacing="0" cellpadding="0" style="width:100%;">';
                HTMLStr += ' <tbody id="tbody">';
                HTMLStr += ' <tr class="HeaderRowStyle"><td class="CheckBoxColumnStyle_crmpd">&nbsp;</td>';
                HTMLStr += '<td class="HeaderColumnStyle_crmpd HeaderColumnStyle_crmpd_Wid"><span class="flt_lft" id="Head_TabMenuCategory"></span></td>';
                HTMLStr += ' <td class="HeaderColumnStyle_crmpd HeaderColumnStyle_crmpd_Wid"><span class="flt_lft" id="Head_TabMenuLinkText"></span></td>';
                HTMLStr += ' <td class="HeaderColumnStyle_crmpd HeaderColumnStyle_crmpd_Wid"><span class="flt_lft" id="Head_TabMenuHeader"></span></td>';
                HTMLStr += '<td class="HeaderColumnStyle_crmpd HeaderColumnStyle_crmpd_Wid">';
                HTMLStr += '<span class="flt_lft" id="Head_TabMenuHeaderDesc"></span></td>';

                if (data != "") {
                    ObjMenuCategory = di_jq.parseJSON(data);
                    for (var i = 0; i < ObjMenuCategory.length; i++) {
                        if (SelectedPageName.toLowerCase() == ObjMenuCategory[i].PageName.toLowerCase()) {
                            var CategoryName = ObjMenuCategory[i].MenuCategory;
                            if (CategoryNameToMaintainState == CategoryName && CategoryNameToMaintainState != '' && CategoryNameToMaintainState != undefined) {
                                HTMLStr += '<tr class="DataRowStyle"><td class="CheckBoxDataColumnStyle_crmpd"><input type="Radio" id=MenuCategory_' + i + ' checked="true" name=radMenuCat></td><td class="DataColumnStyle_crmpd">' + ObjMenuCategory[i].MenuCategory + '</td>';
                            }
                            else {
                                HTMLStr += '<tr class="DataRowStyle"><td class="CheckBoxDataColumnStyle_crmpd"><input type="Radio" id=MenuCategory_' + i + ' name=radMenuCat></td><td class="DataColumnStyle_crmpd">' + ObjMenuCategory[i].MenuCategory + '</td>';
                            }
                            HTMLStr += '<td class="DataColumnStyle_crmpd">' + ObjMenuCategory[i].LinkText + '</td>';
                            HTMLStr += '<td class="DataColumnStyle_crmpd">' + ObjMenuCategory[i].HeaderText + '</td>';
                            HTMLStr += '<td class="DataColumnStyle_crmpd">' + ObjMenuCategory[i].HeaderDesc + '</td>';
                            HTMLStr += '</tr>';
                        }
                    }
                    HTMLStr += '</tbody>';
                    HTMLStr += '</table>'; //  
                    z('cphMainContent_divCategoriesList').innerHTML = HTMLStr;

                    //show move up and down button
                    if (ObjMenuCategory.length > 1) {
                        di_jq('#LiMoveUpAndDown').css({ display: "block" });
                        di_jq('#PipeMoveUpNdwn').css({ display: "block" });
                    }
                    else {
                        di_jq('#LiMoveUpAndDown').css({ display: "none" });
                        di_jq('#PipeMoveUpNdwn').css({ display: "none" });
                    }

                }
            }
            catch (ex) {
                alert("Error : " + ex.message);
            }
        },
        error: function () {
            
        },
        complete: function () {
            RemoveMaskingDiv();
        },
        cache: false
    });
}

function CallToShowCmsToolTip(elementID) {
    var toolTipMessage;
    if (elementID.id == "ImgUp") {
        if (z('lang_ToolTipMovUp').innerHTML != '' && z('lang_ToolTipMovUp').innerHTML != undefined) {
            toolTipMessage = z("lang_ToolTipMovUp").innerHTML;
        }
        else if (z('lang_ToolTipMovUp').innerText != '' && z('lang_ToolTipMovUp').innerText != undefined) {
            toolTipMessage = z("lang_ToolTipMovUp").innerText;
        }
    }
    else {
        if (z('lang_ToolTipMovDown').innerHTML != '' && z('lang_ToolTipMovDown').innerHTML != undefined) {
            toolTipMessage = z("lang_ToolTipMovDown").innerHTML;
        }
        else if (z('lang_ToolTipMovDown').innerText != '' && z('lang_ToolTipMovDown').innerText != undefined) {
            toolTipMessage = z("lang_ToolTipMovDown").innerText;
        }
    }
    ShowCallout('divCallout', toolTipMessage, event);
}

function ShowCommonToolTip(elementID) {
    var toolTipMessage = elementID.textContent;
    if (toolTipMessage.indexOf("...") != -1) {
        toolTipMessage = toolTipMessage.replace("...", "");
        ShowCallout('divCallout', toolTipMessage, elementID);
    }
}
function HideToolTip() {
    HideCallout("divCallout");
}
function MoveUpNDownMenuCat(MenuCategory, MoveUp, MoveDown) {
    try {
        var SelectedPageName = GetUrlVars()['MC'];
        var InputParam = MenuCategory;
        InputParam += ParamDelimiter + MoveUp;
        InputParam += ParamDelimiter + MoveDown;
        InputParam += ParamDelimiter + SelectedPageName;
        di_jq.ajax({
            type: "POST",
            url: "Callback.aspx",
            data: { 'callback': '1033', 'param1': InputParam },
            async: true,
            beforeSend: function () {
                ApplyMaskingDiv();
            },
            success: function (data) {
                try {
                    if (data == "True") {
                        CreateMenuCategoryGrid(MenuCategory);
                    }
                }
                catch (ex) {
                    //  alert("Error : " + ex.message);
                }
            },
            error: function () {
                //  
            },
            complete: function () {
                RemoveMaskingDiv();
            },
            cache: false
        });

    }
    catch (err) { }
}

function MoveUp() {
    var SelectedPageName = GetUrlVars()['MC'];
    var CategorySelected = false;
    var ConfirmAlertMsg = '';
    try {
        for (var i = 0; i < ObjMenuCategory.length; i++) {
            if (SelectedPageName.toLowerCase() == ObjMenuCategory[i].PageName.toLowerCase()) {
                if (z("MenuCategory_" + i).checked == true) {
                    CategorySelected = true;
                    if (i == 0) {
                        alert("Can not move up, since this is already at top position");
                    }
                    else {
                        var CategoryName = ObjMenuCategory[i].MenuCategory;
                        var MoveUp = true;
                        var MoveDown = false;
                        MoveUpNDownMenuCat(CategoryName, MoveUp, MoveDown, SelectedPageName);
                    }
                    break;
                }
            }
        }
        if (!CategorySelected) {
            alert("Select a category to move up.");
        }
    }
    catch (err) { }
}

function MoveDown() {
    var SelectedPageName = GetUrlVars()['MC'];
    var ConfirmAlertMsg = '';
    var CategorySelected = false;
    try {
        for (var i = 0; i < ObjMenuCategory.length; i++) {
            if (SelectedPageName.toLowerCase() == ObjMenuCategory[i].PageName.toLowerCase()) {
                if (z("MenuCategory_" + i).checked == true) {
                    CategorySelected = true;
                    if (i == ObjMenuCategory.length - 1) {
                        alert("Can not move down, since this is already at bottom position");
                    }
                    else {
                        var CategoryName = ObjMenuCategory[i].MenuCategory;
                        var MoveUp = false;
                        var MoveDown = true;
                        MoveUpNDownMenuCat(CategoryName, MoveUp, MoveDown, SelectedPageName);
                    }
                    break;
                }
            }
        }
        if (!CategorySelected) {
            alert("Select a category to move down.");
        }
    }
    catch (err) { }
}

//---------------------------------------------End Manage MenuCategories List Methods-----------------------------------------------------------------
function CreateTopNFooterArticleLinks(FooterNewsHeaderName, TopNewsElmentToSetUrl, TopInnoElmentToSetUrl) {
    var SelectedPageName = PageName.News;
    //    if (GetUrlVars()['MC'] != undefined) {
    //        SelectedPageName = GetUrlVars()['MC'];
    //    }
    //    else {
    //        SelectedPageName = PageName.News;
    //    }
    var CallbackPageName = "Callback.aspx";
    var url = document.URL;
    if (url.indexOf("articles/") != -1) {
        url = url.substring(0, url.indexOf("articles/"))
        url = url + "articles/";
        CallbackPageName = "../libraries/aspx/Callback.aspx";
    }
    else if (url.indexOf("libraries/") != -1) {
        url = url.substring(0, url.indexOf("libraries/"));
        url = url + "articles/";
    }
    var HTMLStr = "";
    var InputParam = SelectedPageName;
    di_jq.ajax({
        type: "Get",
        url: CallbackPageName,
        data: { 'callback': '1029', 'param1': InputParam },
        async: true,
        beforeSend: function () {
            ApplyMaskingDiv();
        },
        success: function (data) {
            try {
                if (data != "") {
                    var ObjMenuCategoryForNews = di_jq.parseJSON(data);
                    HTMLStr += '<ul><li id="langNews" class="fnt_sz_frten">' + FooterNewsHeaderName + '</li>';
                    var IsTopNewsLinkSet = false;
                    HTMLStr += '<li><a target="_blank" href="http://devinfo.us2.list-manage.com/subscribe/post?u=dd048496522c675fe306a2b87&id=a24023691d" id="NewsletterSubmitLink">Subscribe</a> </li>';
                    for (var i = 0; i < ObjMenuCategoryForNews.length; i++) {
                        if (PageName.News.toLowerCase() == ObjMenuCategoryForNews[i].PageName.toLowerCase()) {
                            var CategoryName = ObjMenuCategoryForNews[i].MenuCategory;
                            var LinkText = ObjMenuCategoryForNews[i].LinkText;
                            HTMLStr += "<li><a id=li" + CategoryName + " " + "Class=fnt_sz_frten" + " " + "href=" + url + CategoryName + ">" + LinkText + "</a></li>";
                            if (!IsTopNewsLinkSet) {
                                TopNewsElmentToSetUrl.href = url + CategoryName;
                                IsTopNewsLinkSet = true;
                            }
                        }
                        CreateTopLinksForInnovation(TopInnoElmentToSetUrl);
                    }
                    HTMLStr += '</ul>';
                    z('footerNews').innerHTML = HTMLStr;
                    try {
                        if (z('cphMainContent_sfooterNews') != null && z('cphMainContent_sfooterNews') != undefined) {
                            z('cphMainContent_sfooterNews').innerHTML = HTMLStr;
                        }
                    } catch (e) {

                    }
                }

            }
            catch (ex) {
                alert("Error : " + ex.message);
            }
        },
        error: function () {
            
        },
        complete: function () {
            RemoveMaskingDiv();
        },
        cache: false
    });
}

function CreateTopLinksForInnovation(TopInnoElmentToSetUrl) {
    var SelectedPageName = PageName.Innovations;
    var CallbackPageName = "Callback.aspx";
    var url = document.URL;
    if (url.indexOf("articles/") != -1) {
        url = url.substring(0, url.indexOf("articles/"))
        url = url + "articles/";
        CallbackPageName = "../libraries/aspx/Callback.aspx";
    }
    else if (url.indexOf("libraries/") != -1) {
        url = url.substring(0, url.indexOf("libraries/"));
        url = url + "articles/";
    }
    var HTMLStr = "";
    var InputParam = SelectedPageName;
    di_jq.ajax({
        type: "Get",
        url: CallbackPageName,
        data: { 'callback': '1029', 'param1': InputParam },
        async: true,
        beforeSend: function () {
            ApplyMaskingDiv();
        },
        success: function (data) {
            try {
                if (data != "") {
                    var ObjMenuCategoryForInno = di_jq.parseJSON(data);
                    var IsTopInnoLinkSet = false;
                    for (var i = 0; i < ObjMenuCategoryForInno.length; i++) {
                        if (!IsTopInnoLinkSet) {
                            TopInnoElmentToSetUrl.href = url + ObjMenuCategoryForInno[i].MenuCategory;
                            IsTopInnoLinkSet = true;
                        }
                    }
                }
            }
            catch (ex) {
                alert("Error : " + ex.message);
            }
        },
        error: function () {
            
        },
        complete: function () {
            RemoveMaskingDiv();
        },
        cache: false
    });
}


function SetMaxchar(ElementId, StrText) {
    var Limit;
    if (screen.width == 1024) {
        Limit = 75;
    }
    else {
        Limit = 90;
    }
    var TextToShow = '';
    var TextToHide = '';
    if (StrText.length > Limit) {
        TextToShow = StrText.substr(0, Limit) + "...";
        TextToHide = StrText.substr(Limit, StrText.length);
    }
    else {
        TextToShow = StrText;
        TextToHide = '';
    }
    ElementId.innerHTML = TextToShow + '<span style=display:none;>' + TextToHide + '</span>';
    ElementId.style.display = "block"
}

function ValidateAlphaInput(InputString) {
    var regex = /^[a-zA-Z- ]+$/;
    if (regex.test(InputString)) {
        return true;
    }
    else
        return false;
}

function ShowTopOptions(IsShow) {
    if (IsShow) {
        document.getElementById("DivTopOptions").style.display = "";
    }
    else {
        document.getElementById("DivTopOptions").style.display = "none";
    }
}

function CMSTweet() {
    var myTweet = z('txtTwitterShare').value + ' - ' + z('valShareLink').value;
    if (myTweet.indexOf("/articles/") != -1) {
        myTweet = myTweet.replace("/articles/", "/$articles$/");
    }
    window.open("../libraries/aspx/tweet/Default.aspx?tweet=" + myTweet, '_blank', '', '');
}

function CMSfbWallPost() {
    var description = z('valDesc').value;
    if (description.length >= 200) {
        description = description.substring(0, 199);
    }
    var parameters = "";
    var WallPost = z('txtFacebookShare').value + ' - ' + z('valShareLink').value;
    if (WallPost.indexOf("/articles/") != -1) {
        WallPost = WallPost.replace("/articles/", "/$articles$/");
    }
    parameters += WallPost;
    parameters += "&picture_url=" + z('valPictureLink').value;
    parameters += "&title=" + z('valTitle').value;
    parameters += "&desc=" + description;
    parameters += "&valArticleUrl=" + z('valShareLink').value.replace("/articles/", "/$articles$/");
    window.open("../libraries/aspx/fb/Default.aspx?wall_post=" + parameters, '_blank', '', '');
}

function CMSShareByEmail() {
    if (z('txtEmailShare').value == '') {
        alert("Please enter E-Mail Id");
        return;
    }
    var ShareLink = z('valShareLink').value; // z('EmbeddLink').value.replace('<iframe src="','').replace('" style="width:100%; border:0px;"></iframe>','');
    var InputParam = z('txtEmailShare').value.trim() + ParamDelimiter + ShareLink;

    var htmlResp = di_jq.ajax({
        type: "POST",
        url: "../libraries/aspx/Callback.aspx",
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
function CMSHideSocialSharing() {
    try {
        RemoveMaskingDiv();
        di_jq('#CMSdivEmbeddLink').hide('slow');
    }
    catch (err) { }
}


function CMSSocialSharing(PreText, AppName, ShareLink, PictureLink, Title, Desc) {
    var PictureUrlPath = '';
    if (PictureLink != "") {
        var PictureUrlPath = getAbsURL('libraries') + PictureLink.split("libraries/")[1];
    }
    var EmailSent = z('lblEmailSent');
    EmailSent.style.display = "none";
    var EmbededShareLink = ShareLink.replace("/articles/", "/stock/shared/article/articleshare.html?id=");
    var EmbeddURL = '<iframe src="' + EmbededShareLink + '" style="width:100%; border:0px;"></iframe>';
    z('valShareLink').value = ShareLink;
    z('EmbeddLink').value = EmbeddURL;
    z('URL').value = ShareLink;
    z('valPictureLink').value = PictureUrlPath;
    z('valTitle').value = Title;
    z('valDesc').value = Desc.replace(/\<br[\/]*\>/g, "\n");
    z('txtTwitterShare').onkeypress = function () //Tweet 140 length check
    {
        var tmpLink = ShareLink;
        checkTweetLength(tmpLink);
    }
    z('txtTwitterShare').value = PreText + " - " + AppName; // + " - " + ShareLink;
    z('txtFacebookShare').value = PreText + " - " + AppName; // + " - " + ShareLink;

    ApplyMaskingDiv();
    GetWindowCentered(z("CMSdivEmbeddLink"), 600, 420);
    var imgSrc = "../stock/themes/default/images/close.png";
    //Set close button at right corner of popup div
    SetCloseButtonInPopupDiv(di_jq('#CMSdivEmbeddLink'), 'CMSHideSocialSharing', imgSrc);

    di_jq('#CMSdivEmbeddLink').show('slow');
}

function ManageDefaults() {

    di_jq("#TrArticleLink td").each(function () {
        var IsMoreLinkHidden = false;
        var IsDateLinkHidden = false;
        if (di_jq(this).find('tbody').find('tr').find('td').find("a[id='AncArticleUrlForTitle']").attr('href') != "javascript:void(0);" && di_jq(this).find('tbody').find('tr').find('td').find("a[id='AncArticleUrlForTitle']").attr('href') != undefined) {
            di_jq(this).find('tbody').find('tr').find('td').find("img[id='ImgMoreArrow']").css('display', 'block');
            di_jq(this).find('tbody').find('tr').find('td').find("a[id='AncArticleUrlForMore']").css('display', 'block');
            IsMoreLinkHidden = true;
        }
        else {
            di_jq(this).find('tbody').find('tr').find('td').find("a[id='AncArticleUrlForTitle']").addClass("news_lnk_disbled");
        }

        if (di_jq(this).find('tbody').find('tr').find('td').find('a').find("img[id='ImgThumbNail']").attr('src') != "" && di_jq(this).find('tbody').find('tr').find('td').find('a').find("img[id='ImgThumbNail']").attr('src') != undefined) {
            di_jq(this).find('tbody').find('tr').find('td').find('a').find("img[id='ImgThumbNail']").closest("td").css('display', 'block');
        }
        if (di_jq(this).find('tbody').find('tr').find('td').find("span[class='adaptation_italic']").html() != null) {
            if (di_jq(this).find('tbody').find('tr').find('td').find("span[class='adaptation_italic']").html().indexOf(1900) == -1) {
                di_jq(this).find('tbody').find('tr').find('td').find("span[class='adaptation_italic']").css('display', 'block');
                IsDateLinkHidden = true;
            }
        }
        if (di_jq(this).find('tbody').find('tr').find('td').find("a[id='AncArticleUrlForImg']").attr('href') == "javascript:void(0);") {
            di_jq(this).find('tbody').find('tr').find('td').find("img[id='ImgThumbNail']").addClass("cur_Default");
        }
        if (IsMoreLinkHidden == true || IsDateLinkHidden == true) {
            di_jq(this).find('tbody').find('tr').find('td').find("span[class='adaptation_italic']").closest("tr").css('display', 'block');
        }
        ManageHaderContent();
    });
}

function ManageHaderContent() {
    if (di_jq("#cphMainContent_SpanHeaderDescription").html().indexOf(1900) == -1) {
        di_jq("#cphMainContent_SpanHeaderDescription").css('display', 'block');
    }
    else {
        di_jq("#cphMainContent_SpanHeaderDescription").css('display', 'none');
    }
    if (di_jq("#cphMainContent_ArticlePdf").attr("href") != "javascript:void(0);") {
        di_jq("#SpnPdfContent").css('display', 'inline');
    }
    else {
        di_jq("#SpnPdfContent").css('display', 'none');
    }
}

function GetArticleByContentIdForSharing() {
    var CallbackPageName = "../../../libraries/aspx/Callback.aspx";
    InputParam = GetUrlVars()["id"];
    di_jq.ajax({
        type: "POST",
        url: CallbackPageName,
        data: { 'callback': '1039', 'param1': InputParam },
        async: true,
        beforeSend: function () {
            ApplyMaskingDiv();
        },
        success: function (data) {
            try {
                if (data != "") {
                    if (data != "") {
                        var RetData = data.split(ParamDelimiter);
                        z('DevSharedContent').innerHTML = RetData[0];
                        z('SpanMenuCategoryHeading').innerHTML = RetData[1];
                        z('SpanHeaderDate').innerHTML = RetData[2];
                        document.title = RetData[1];
                    }
                }
            }
            catch (ex) {
                alert("Error : " + ex.message);
            }
        },
        error: function () {
            
        },
        complete: function () {
            RemoveMaskingDiv();
        },
        cache: false
    });

}

function GetUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}

//function SetPaginationPosition(IsPaginationOnTop) {
////    if (IsPaginationOnTop) {
////        di_jq('#PagerTr').addClass('Pagination_Float');
////    }
////    else {
////        di_jq('#PagerTr').removeClass('Pagination_Float');
////    }
//}