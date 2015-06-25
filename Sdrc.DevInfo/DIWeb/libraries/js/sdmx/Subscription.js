// JScript File

var SubscriptionCallBackPageName = "Callback.aspx";
var SelectedAction = "";
var SubscriptionSelectedCatGIDAndScheme = "";

var btnGoCtlId = "btnAdd";
var txtRegistryURNCtlId = "txtRegistryURN";

var txtNotificationMailCtlId = "txtNotificationMail";
var txtNotificationHTTPCtlId = "txtNotificationHTTP";
var chkIsSOAPCtlId = "chkIsSOAP";
var txtStartDateCtlId = "txtStartDate";
var txtEndDateCtlId = "txtEndDate";

var message = "message:";
var MsgErrorOccurred = "Error Occured";
var MsgRegistryURN = "Enter value in Registry URN.";
var MsgNotificationMailHTTP = "Atleast one field should contain value Notification Mail or Notification HTTP.";
var MsgSubscriberAssignedId = "Enter value in Subscriber Assigned Id.";
var MsgStartDate = "Please Select Start Date.";
var MsgEndDate = "Please Select End Date.";
var MsgInvalidEmail = "Please enter valid Email Id.";
var MsgCategory = "Select atleast one category.";
var MsgMFD = "Select atleast one Metadata Flow from the list.";
var MsgSubcriptionDetailsNotRetrieved = "Subscription Details cannot be retrieved.";
var MsgDeleteSubscription = "Do you really want to delete this subscription?";
var PivotColumnSeparator = "|";
var ValueSeparator = "~~";
var CommaSeparator = ",";
function onPageLoad(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hLoggedInUserNId, hLoggedInUserName, hOriginaldbnid) {
    var hsgcount = 10;
    // ************************************************
    // create Form Tag with hidden input boxes
    // ************************************************
    createFormHiddenInputs("frm_sdmxSubscription", "POST");
    SetCommonLinksHref("frm_sdmxSubscription", "POST");
    SetOriginaldbnidInForm("frm_sdmxSubscription", hOriginaldbnid);

    // ************************************************1
    // set page level variables based on selections or defaults
    // ************************************************1
    setPostedData(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hsgcount, hLoggedInUserNId, hLoggedInUserName);

    // ************************************************1
    // Load Language Component
    // ************************************************1
    ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', hlngcode);

    SetCommonRegistryPageDetails('RegSubscription.aspx', "frm_sdmxSubscription", "POST");
}

function LanguageHandlingForAlerts() {
    message = document.getElementById('hMessage').value;
    MsgErrorOccurred = document.getElementById('hErrorOccurred').value;
    MsgRegistryURN = document.getElementById('hMsgRegistryURN').value;
    MsgNotificationMailHTTP = document.getElementById('hMsgNotificationMailHTTP').value;
    MsgSubscriberAssignedId = document.getElementById('hMsgSubscriberAssignedId').value;
    MsgStartDate = document.getElementById('hMsgStartDate').value;
    MsgEndDate = document.getElementById('hMsgEndDate').value;
    MsgInvalidEmail = document.getElementById('hMsgInvalidEmail').value;
    MsgCategory = document.getElementById('hMsgCategory').value;
    MsgMFD = document.getElementById('hMsgMFD').value;
}

function SetHiddenFieldValues() {
    for (var i = 0; i < document.getElementsByName('lang_View').length; i++) {
        document.getElementsByName('lang_View')[i].innerHTML = document.getElementById('hView').value;
    }

    for (var i = 0; i < document.getElementsByName('lang_Edit').length; i++) {
        document.getElementsByName('lang_Edit')[i].innerHTML = document.getElementById('hEdit').value;
    }

    for (var i = 0; i < document.getElementsByName('lang_Delete').length; i++) {
        document.getElementsByName('lang_Delete')[i].innerHTML = document.getElementById('hDelete').value;
    }
    LanguageHandlingForAlerts();
}

function BindSubscriptions() {
    try {
        if (document.getElementById('hLoggedInUserNId').value) {
            ShowLoadingDiv();
            InputParam = document.getElementById('hdbnid').value;
            InputParam += ParamDelimiter + document.getElementById('hLoggedInUserNId').value;
            InputParam += ParamDelimiter + document.getElementById('hlngcode').value;

            var htmlResp = di_jq.ajax
                ({
                    type: "POST",
                    url: SubscriptionCallBackPageName,
                    data: "callback=59&param1=" + InputParam,
                    async: false,
                    success: function (data) {
                        try {
                            if (data) {
                                document.getElementById('gridData').style.display = "";
                                RenderingSubscriptionDetailsInOATGrid(data);
                            }
                            else {
                                document.getElementById('gridData').style.display = "none";
                            }
                            HideLoadingDiv();
                            SetHiddenFieldValues();

                        }
                        catch (ex) {
                            HideLoadingDiv();
                            //                            alert(message + ex.message);
                            
                        }
                    },
                    error: function () {
                        HideLoadingDiv();
                        
                    },
                    cache: false
                });

        }

    }
    catch (ex) {

    }

}


function RenderingSubscriptionDetailsInOATGrid(data) {
    var header = new Array();
    var OATdata = [];

    header.push(document.getElementById('hSubscription').value);
    header.push(document.getElementById('hEmail').value);
    header.push(document.getElementById('hWebServiceAddress').value);
    header.push(document.getElementById('hStartDate').value);
    header.push(document.getElementById('hEndDate').value);
    header.push(document.getElementById('hEventType').value);
    
    header.push(document.getElementById('hLangPref').value);
    header.push(document.getElementById('hAction').value);
 

    var distinctSubscriptionRows = data.split('#');
    for (var i = 0; i < distinctSubscriptionRows.length; i++) {
        var tmpRow = distinctSubscriptionRows[i].split("~");
        OATdata.push(tmpRow);
    }
    var mainlist = [];
    mainlist.push(0);
    mainlist.push(1);
    mainlist.push(2);
    mainlist.push(3);
    mainlist.push(4);
    mainlist.push(5);
    mainlist.push(6);

    var sublist = [];

    var pivot = new OAT.Pivot("pivot_content", "", "pivot_page", header, OATdata, mainlist, sublist, [], header.length - 1, { showChart: 0, showEmpty: 0 });

}


function BindICTypes(SubscriptionSelectedCatGIDAndScheme) {
    try {

        InputParam = document.getElementById('hdbnid').value;
        InputParam += ParamDelimiter + SubscriptionSelectedCatGIDAndScheme;

        var htmlResp = di_jq.ajax({
            type: "GET",
            url: SubscriptionCallBackPageName,
            data: "callback=44&param1=" + InputParam,
            async: false,
            success: function (data) {
                try {
                    var ICTypes = data;
                    document.getElementById('divICTypes').innerHTML = ICTypes;

                    HideLoadingDiv();
                }
                catch (ex) {
                    HideLoadingDiv();
                    // alert(message + ex.message);
                    

                }
            },
            error: function () {
                HideLoadingDiv();
                
            },
            cache: false
        });

    }
    catch (ex) {

    }

}

function BindCatScheme(ICType, SelectedCatGIDsAndScheme) {

    try {

        InputParam = document.getElementById('hdbnid').value;
        InputParam += ParamDelimiter + ICType;
        InputParam += ParamDelimiter + SubscriptionSelectedCatGIDAndScheme;
        var htmlResp = di_jq.ajax({
            type: "GET",
            url: SubscriptionCallBackPageName,
            data: "callback=45&param1=" + InputParam,
            async: false,
            success: function (data) {
                try {
                    var CatScheme = data;
                    document.getElementById('divCatScheme').innerHTML = CatScheme;

                    HideLoadingDiv();
                }
                catch (ex) {
                    HideLoadingDiv();
                    //alert(message + ex.message);
                    
                }
            },
            error: function () {
                HideLoadingDiv();
                
            },
            cache: false
        });

        di_jq('#' + ICType).checkboxTree({
            trueCheck: {
                val: 'no'
            },
            onCheck: {
                ancestors: '',
                descendants: 'check'
            },
            onUncheck: {
                ancestors: '',
                descendants: 'uncheck'
            },
            initializeChecked: '',
            initializeUnchecked: 'collapsed'
        });
    }
    catch (ex) {

    }

}

function AddSubscription() {
    try {
        ShowLoadingDiv();
        var SelectedAction = "Add";
        if (ValidateSubscription(SelectedAction)) {

            InputParam = SelectedAction;
            InputParam += ParamDelimiter + $$(txtNotificationMailCtlId, false).value;
            InputParam += ParamDelimiter + $$(txtNotificationHTTPCtlId, false).value;
            InputParam += ParamDelimiter + ($$(chkIsSOAPCtlId, false).checked == true ? 1 : 0);
            InputParam += ParamDelimiter + $$(txtStartDateCtlId, false).value;
            InputParam += ParamDelimiter + $$(txtEndDateCtlId, false).value;
            if (z("selEventSelector").selectedIndex == 0) {
                InputParam += ParamDelimiter + "Data Registration";
                InputParam += ParamDelimiter + SubscriptionSelectedCatGIDAndScheme;
                InputParam += ParamDelimiter + "0";
            }
            else if (z("selEventSelector").selectedIndex == 1) {
                InputParam += ParamDelimiter + "Metadata Registration";
                InputParam += ParamDelimiter + "0";
                InputParam += ParamDelimiter + z("selMFD").options[z("selMFD").selectedIndex].value;
            }
            else if (z("selEventSelector").selectedIndex == 2) {
                InputParam += ParamDelimiter + "Structural Metadata Registration";
                InputParam += ParamDelimiter + "0";
                InputParam += ParamDelimiter + "0";
            }
            InputParam += ParamDelimiter + document.getElementById('hdbnid').value;
            InputParam += ParamDelimiter + document.getElementById('hLoggedInUserNId').value;
            InputParam += ParamDelimiter + document.getElementById('hlngcode').value;

            // Adding lang preference to the end
            InputParam += ParamDelimiter + z("selLang").options[z("selLang").selectedIndex].value;
            InputParam += ParamDelimiter + document.getElementById('hOriginaldbnid').value;
            var htmlResp = di_jq.ajax({
                type: "GET",
                url: SubscriptionCallBackPageName,
                data: "callback=28&param1=" + InputParam,
                async: false,
                success: function (data) {
                    try {
                        var SubscriptionSuccess = data.split(ValueSeparator);
                        HideSubscriptionDetailsPopup();
                        BindSubscriptions();
                        HideLoadingDiv();
                    }
                    catch (ex) {
                        HideLoadingDiv();
                        //alert(message + ex.message);
                        
                    }
                },
                error: function () {
                    HideLoadingDiv();
                    
                },
                cache: false
            });
        }
    }
    catch (ex) {
        ShowErrorAlert(ex, "ManipulateSubscription()");
    }
    HideLoadingDiv();

}


function ValidateSubscription(SelectedAction) {
    var RetVal = true;

    try {
        if (SelectedAction == "Add") {
            if (CheckBlankCtrl(txtNotificationMailCtlId) && CheckBlankCtrl(txtNotificationHTTPCtlId)) {
                RetVal = false;
                ShowMessage(MsgNotificationMailHTTP);
                FocusCtrl($$(txtNotificationMailCtlId, false));
            }
            else if (!CheckBlankCtrl(txtNotificationMailCtlId) && !validateEmail($$(txtNotificationMailCtlId, false).value)) {
                RetVal = false;
                alert(MsgInvalidEmail);
                FocusCtrl($$(txtNotificationMailCtlId, false));
            }
            else if (CheckBlankCtrl(txtStartDateCtlId)) {
                RetVal = false;
                ShowMessage(MsgStartDate);
            }
            else if (CheckBlankCtrl(txtEndDateCtlId)) {
                RetVal = false;
                ShowMessage(MsgEndDate);
            }
            else if (z("selEventSelector").selectedIndex == 0) {
                if (SubscriptionSelectedCatGIDAndScheme == '') {
                    RetVal = false;
                    ShowMessage(MsgCategory);

                }

            }
            else if (z("selEventSelector").selectedIndex == 1) {
                if (z("selMFD").selectedIndex <= 0) {
                    RetVal = false;
                    ShowMessage(MsgMFD);
                    z("selMFD").focus();
                }
            }

        }
        else {
            if (SelectedAction == "Edit") {
                if (z("selEventSelector").selectedIndex == 0) {
                    if (SubscriptionSelectedCatGIDAndScheme == '') {
                        RetVal = false;
                        ShowMessage(MsgCategory);
                    }

                }
                else if (z("selEventSelector").selectedIndex == 1) {
                    if (z("selMFD").selectedIndex <= 0) {
                        RetVal = false;
                        ShowMessage(MsgMFD);
                        z("selMFD").focus();
                    }

                }

            }
        }
    }
    catch (err) { }

    return RetVal;
}




function ShowMessage(msgStr) {
    try {
        alert(msgStr);
    }
    catch (err) { }
}

function FocusCtrl(ctrl) {
    try {
        ctrl.focus();
    }
    catch (err) { }
}

function ShowErrorAlert(ex, methodName) {
    var ErrMessage = "";
    ErrMessage += "JS Error occured -  ";
    ErrMessage += "\n------------------------------------------------";
    ErrMessage += "\nMethod Name: " + methodName;
    ErrMessage += "\nMessage    : " + ex.message;
    ErrMessage += "\nDescription: " + ex.description;

    alert(ErrMessage);
}


function ClearSubscriptionCtrlValues() {
    try {
        var currentTime = new Date();
        var month = currentTime.getMonth() + 1;
        var day = currentTime.getDate();
        var year = currentTime.getFullYear();
        var EndDateYear = currentTime.getFullYear() + 5;
        if (day < 10) {
            day = '0' + day;
        }
        if (month < 10) {
            month = '0' + month;
        }

        document.getElementById('txtStartDate').value = day + "-" + month + "-" + year;
        document.getElementById('txtEndDate').value = day + "-" + month + "-" + EndDateYear;
        GetEmailIdOfLoggedInUser();
        $$(txtNotificationHTTPCtlId, false).value = "";
        $$(chkIsSOAPCtlId, false).checked = false;

        z("selEventSelector").selectedIndex = 0;
        BindRegCategoryOrDataFlows();
        SubscriptionSelectedCatGIDAndScheme = "";

        BindICTypes("");
        BindCatScheme("SC", "");

    }
    catch (err) { }
}
function GetEmailIdOfLoggedInUser() {
    try {

        InputParam = document.getElementById('hLoggedInUserNId').value;

        var htmlResp = di_jq.ajax({
            type: "GET",
            url: SubscriptionCallBackPageName,
            data: "callback=63&param1=" + InputParam,
            async: false,
            success: function (data) {
                try {
                    $$(txtNotificationMailCtlId, false).value = data;

                }
                catch (ex) {
                    HideLoadingDiv();
                    //alert(message + ex.message);
                    
                }
            },
            error: function () {
                HideLoadingDiv();
                
            },
            cache: false
        });

    }
    catch (ex) {

    }

}


function $$(ctlId, includeCPH) {
    var objCtl;
    if (ctlId != "") {
        if (includeCPH == true) {
            objCtl = document.getElementById("ctl00_BaseContentHolder_" + ctlId);
        }
        else {
            objCtl = document.getElementById(ctlId);
        }
    }
    return objCtl
}

function FocusCtrl(ctrl) {
    try {
        ctrl.focus();
    }
    catch (err) { }
}

function CheckBlankCtrl(ctrl) {
    var RetVal = false;
    try {
        if ($$(ctrl, false).value.trim() == "") {
            RetVal = true;
            FocusCtrl($$(ctrl, true));
        }
    }
    catch (err) { }

    return RetVal;
}

function GetSubscriptionDetails(SubscriptionId) {
    try {
        document.getElementById('hdnSubscriptionId').value = SubscriptionId;
        if (SubscriptionId) {
            ShowLoadingDiv();

            InputParam = SubscriptionId;
            InputParam += ParamDelimiter + document.getElementById('hdbnid').value;
            InputParam += ParamDelimiter + document.getElementById('hLoggedInUserNId').value;
            InputParam += ParamDelimiter + document.getElementById('hlngcode').value;

            var htmlResp = di_jq.ajax({
                type: "GET",
                url: SubscriptionCallBackPageName,
                data: "callback=33&param1=" + InputParam,
                async: false,
                success: function (data) {
                    try {
                        var SubscriptionDetails = data.split(ValueSeparator);

                        if (SubscriptionDetails.length > 1) {
                            $$(txtNotificationMailCtlId, false).value = SubscriptionDetails[0];
                            $$(txtNotificationHTTPCtlId, false).value = SubscriptionDetails[1];
                            $$(chkIsSOAPCtlId, false).checked = (SubscriptionDetails[2] == "True" ? true : false);
                            $$(txtStartDateCtlId, false).value = SubscriptionDetails[3];
                            $$(txtEndDateCtlId, false).value = SubscriptionDetails[4];
                            if (SubscriptionDetails[5] == document.getElementById('lang_Data_Registration').innerHTML) {
                                z("selEventSelector").selectedIndex = 0;
                                BindRegCategoryOrDataFlows();
                                SubscriptionSelectedCatGIDAndScheme = SubscriptionDetails[6];
                                BindICTypes();
                                BindCatScheme("SC", SubscriptionSelectedCatGIDAndScheme);
                            }
                            else if (SubscriptionDetails[5] == document.getElementById('lang_Metadata_Registration').innerHTML) {
                                z("selEventSelector").selectedIndex = 1;
                                BindRegCategoryOrDataFlows();
                                z("selMFD").value = SubscriptionDetails[6];
                            }
                            else if (SubscriptionDetails[5] == document.getElementById('lang_Structural_Metadata_Registration').innerHTML) {
                                z("selEventSelector").selectedIndex = 2;
                                BindRegCategoryOrDataFlows();
                            }
                            // set preferred language
                            var selLang = z("selLang");
                            var i, j;
                            //j = selLang.options.length;
                            //alert(selLang.options.length);

                            for (i = 0; i < selLang.options.length; i++) {
                                //alert(i);
                                if (selLang.options[i].value == SubscriptionDetails[7]) {
                                    selLang.options[i].selected = true;
                                    break;
                                }
                            }

                        }
                        else {
                            alert(MsgSubcriptionDetailsNotRetrieved);
                        }

                        HideLoadingDiv();
                    }
                    catch (ex) {
                        HideLoadingDiv();
                        alert(message + ex.message);
                    }
                },
                error: function () {
                    HideLoadingDiv();
                    
                },
                cache: false
            });
        }
    }
    catch (ex) {
    }
}


function AddCategoryGId(CategoryGIdAndCatSchemeId, e) {

    var blank = "";
    var evnt = e || window.event;
    var element = evnt.srcElement || evnt.target;
    var SelCategoryGId = CategoryGIdAndCatSchemeId.split('|')[0];
    var SelCategorySchemeId = CategoryGIdAndCatSchemeId.split('|')[1];
    if (element.checked) {
        if (CategoryGIdAlreadyExists(SelCategoryGId) == false) {
            if (SubscriptionSelectedCatGIDAndScheme != "") {

                SubscriptionSelectedCatGIDAndScheme += CommaSeparator + CategoryGIdAndCatSchemeId;
            }
            else {
                SubscriptionSelectedCatGIDAndScheme += CategoryGIdAndCatSchemeId;
            }

        }

    }
    else {

        SubscriptionSelectedCatGIDAndScheme = SubscriptionSelectedCatGIDAndScheme.replace((CategoryGIdAndCatSchemeId + ","), blank);
        SubscriptionSelectedCatGIDAndScheme = SubscriptionSelectedCatGIDAndScheme.replace(("," + CategoryGIdAndCatSchemeId), blank);
        SubscriptionSelectedCatGIDAndScheme = SubscriptionSelectedCatGIDAndScheme.replace(CategoryGIdAndCatSchemeId, blank);

    }

}


function CategoryGIdAlreadyExists(SelCategoryGId) {

    var SelectedCategoriesAndScheme = SubscriptionSelectedCatGIDAndScheme.split(CommaSeparator);
    var i;
    var c = 0;
    for (i = 0; i < SelectedCategoriesAndScheme.length; i++) {
        if (SelCategoryGId == SelectedCategoriesAndScheme[i].split('|')[0]) {
            c = c + 1;
        }
    }
    if (c > 0)
    { return true; }
    else
    { return false; }
}


function UpdateSubscription() {
    try {
        var SelectedAction = "Edit";
        if (ValidateSubscription(SelectedAction)) {
            ShowLoadingDiv();


            InputParam = SelectedAction;
            InputParam += ParamDelimiter + $$(txtNotificationMailCtlId, false).value;
            InputParam += ParamDelimiter + $$(txtNotificationHTTPCtlId, false).value;
            InputParam += ParamDelimiter + ($$(chkIsSOAPCtlId, false).checked == true ? 1 : 0);
            InputParam += ParamDelimiter + $$(txtStartDateCtlId, false).value;
            InputParam += ParamDelimiter + $$(txtEndDateCtlId, false).value;
            if (z("selEventSelector").selectedIndex == 0) {
                InputParam += ParamDelimiter + "Data Registration";
                InputParam += ParamDelimiter + SubscriptionSelectedCatGIDAndScheme;
                InputParam += ParamDelimiter + "0";
            }
            else if (z("selEventSelector").selectedIndex == 1) {
                InputParam += ParamDelimiter + "Metadata Registration";
                InputParam += ParamDelimiter + "0";
                InputParam += ParamDelimiter + z("selMFD").options[z("selMFD").selectedIndex].value;
            }
            else if (z("selEventSelector").selectedIndex == 2) {
                InputParam += ParamDelimiter + "Structural Metadata Registration";
                InputParam += ParamDelimiter + "0";
                InputParam += ParamDelimiter + "0";
            }

            InputParam += ParamDelimiter + document.getElementById('hdbnid').value;
            InputParam += ParamDelimiter + document.getElementById('hLoggedInUserNId').value;
            InputParam += ParamDelimiter + document.getElementById('hdnSubscriptionId').value;
            InputParam += ParamDelimiter + document.getElementById('hlngcode').value;
            // adding param for langPref
            InputParam += ParamDelimiter + z("selLang").options[z("selLang").selectedIndex].value;
            InputParam += ParamDelimiter + document.getElementById('hOriginaldbnid').value;
            var htmlResp = di_jq.ajax({
                type: "GET",
                url: SubscriptionCallBackPageName,
                data: "callback=60&param1=" + InputParam,
                async: false,
                success: function (data) {
                    try {
                        var SubscriptionSuccess = data.split(ValueSeparator);
                        HideSubscriptionDetailsPopup();
                        BindSubscriptions();
                        HideLoadingDiv();
                    }
                    catch (ex) {
                        HideLoadingDiv();
                        alert(message + ex.message);
                    }
                },
                error: function () {
                    HideLoadingDiv();
                    
                },
                cache: false
            });
        }
    }
    catch (ex) {
        //ShowErrorAlert(ex, "UpdateSubscription()");
        
    }
}

function DeleteSubscription() {

    if (confirm(MsgDeleteSubscription)) {
        try {
            var SelectedAction = "Delete";
            ShowLoadingDiv();
            InputParam = SelectedAction;
            InputParam += ParamDelimiter + $$(txtNotificationMailCtlId, false).value;
            InputParam += ParamDelimiter + $$(txtNotificationHTTPCtlId, false).value;
            InputParam += ParamDelimiter + ($$(chkIsSOAPCtlId, false).checked == true ? 1 : 0);
            InputParam += ParamDelimiter + $$(txtStartDateCtlId, false).value;
            InputParam += ParamDelimiter + $$(txtEndDateCtlId, false).value;
            if (z("selEventSelector").selectedIndex == 0) {
                InputParam += ParamDelimiter + "Data Registration";
                InputParam += ParamDelimiter + SubscriptionSelectedCatGIDAndScheme;
                InputParam += ParamDelimiter + "0";
            }
            else if (z("selEventSelector").selectedIndex == 1) {
                InputParam += ParamDelimiter + "Metadata Registration";
                InputParam += ParamDelimiter + "0";
                InputParam += ParamDelimiter + z("selMFD").options[z("selMFD").selectedIndex].value;
            }
            else if (z("selEventSelector").selectedIndex == 2) {
                InputParam += ParamDelimiter + "Structural Metadata Registration";
                InputParam += ParamDelimiter + "0";
                InputParam += ParamDelimiter + "0";
            }
            InputParam += ParamDelimiter + document.getElementById('hdbnid').value;
            InputParam += ParamDelimiter + document.getElementById('hLoggedInUserNId').value;
            InputParam += ParamDelimiter + document.getElementById('hdnSubscriptionId').value;
            InputParam += ParamDelimiter + document.getElementById('hOriginaldbnid').value;
            var htmlResp = di_jq.ajax({
                type: "GET",
                url: SubscriptionCallBackPageName,
                data: "callback=61&param1=" + InputParam,
                async: false,
                success: function (data) {
                    try {
                        var SubscriptionSuccess = data.split(ValueSeparator);
                        HideSubscriptionDetailsPopup();
                        BindSubscriptions();
                        HideLoadingDiv();
                    }
                    catch (ex) {
                        HideLoadingDiv();
                                      alert(message + ex.message);
                        
                    }
                },
                error: function () {
                    HideLoadingDiv();
                    
                },
                cache: false
            });

        }
        catch (ex) {
            //            ShowErrorAlert(ex, "UpdateSubscription()");
            
        }
    }
}

function OpenSubscriptionDetailsPopup(action, SubscriptionId) {
    ApplyMaskingDiv();
    ShowLoadingDiv();

    // Populate language preference dropdown
    PopulateLangPref();

    //Set close button at right corner of popup div
    SetCloseButtonInPopupDiv(di_jq('#divSubscriptionDetails'), 'HideSubscriptionDetailsPopup');
    di_jq("#divSubscriptionDetails").show('slow');
    GetWindowCentered(document.getElementById('divSubscriptionDetails'), 702, 455);

    ClearSubscriptionCtrlValues();
    document.getElementById('hdnSubscriptionId').value = SubscriptionId;
    if (action == 'A')//Add
    {
        document.getElementById('rowAddSubscription').style.display = "";
        document.getElementById('rowUpdateSubscription').style.display = "none";
        document.getElementById('rowDeleteSubscription').style.display = "none";
        document.getElementById('rowViewSubscription').style.display = "none";
    }
    else if (action == 'U')//Update
    {
        document.getElementById('rowAddSubscription').style.display = "none";
        document.getElementById('rowUpdateSubscription').style.display = "";
        document.getElementById('rowDeleteSubscription').style.display = "none";
        document.getElementById('rowViewSubscription').style.display = "none";

        GetSubscriptionDetails(SubscriptionId);
    }
    else if (action == 'V')//View
    {
        document.getElementById('rowAddSubscription').style.display = "none";
        document.getElementById('rowUpdateSubscription').style.display = "none";
        document.getElementById('rowDeleteSubscription').style.display = "none";
        document.getElementById('rowViewSubscription').style.display = "";

        GetSubscriptionDetails(SubscriptionId);
    }
    else if (action == 'D')//Delete
    {
        document.getElementById('rowAddSubscription').style.display = "none";
        document.getElementById('rowUpdateSubscription').style.display = "none";
        document.getElementById('rowDeleteSubscription').style.display = "";
        document.getElementById('rowViewSubscription').style.display = "none";

        GetSubscriptionDetails(SubscriptionId);
    }

    HideLoadingDiv();
}

function HideSubscriptionDetailsPopup() {
    di_jq('#divSubscriptionDetails').hide('slow');
    RemoveMaskingDiv();
    HideLoadingDiv();
}

function BindRegCategoryOrDataFlows() {
    if (z("selEventSelector").selectedIndex == 0) {
        document.getElementById('divCategory').style.display = "";
        document.getElementById('divMetadataFlows').style.display = "none";
    }
    else if (z("selEventSelector").selectedIndex == 1) {
        document.getElementById('divCategory').style.display = "none";
        document.getElementById('divMetadataFlows').style.display = "";
        BindMFDList();
    }
    else if (z("selEventSelector").selectedIndex == 2) {
        document.getElementById('divCategory').style.display = "none";
        document.getElementById('divMetadataFlows').style.display = "none";
    }
}

function BindMFDList() {
    try {

        InputParam = z('hdbnid').value;
        InputParam += ParamDelimiter + document.getElementById('hlngcodedb').value;

        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: "callback=97&param1=" + InputParam,
            async: false,
            success: function (data) {
                try {
                    z("selMFD").length = 1;
                    var MFDs = data.split(CommaSeparator);
                    var i;
                    for (i = 0; i < MFDs.length; i++) {
                        z("selMFD").options[z("selMFD").length] = new Option(MFDs[i].split("~~")[0], MFDs[i].split("~~")[1]);
                    }

                    HideLoadingDiv();
                }
                catch (ex) {
                    HideLoadingDiv();
                    //                      alert(message + ex.message);
                    
                }
            },
            error: function () {
                HideLoadingDiv();
                
            },
            cache: false
        });

    }
    catch (ex) {
        
    }

}

// Populate language preference select list
function PopulateLangPref() {
    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: SubscriptionCallBackPageName,
            dataType: 'json',
            data: { 'callback': '302' },
            async: false,
            success: function (data) {
                try {
                    var lNid, lName, defaultLang, count = 1;
                    if (data.error) {
                        throw "Error in result"
                    }
                    else {
                        // clear if list exists
                        var selectList = z("selLang");
                        var optionCount = selectList.options.length;
                        if (optionCount > 0) {
                            selectList.options.length = 0;
                        }
                        di_jq.each(data, function (k, v) {      //DefaultLang has to be the first key
                            switch (k) {
                                case "DefaultLang":
                                    {
                                        defaultLang = v;
                                    }
                                    break;
                                case "LanguagesFromDB":
                                    {
                                        di_jq.each(v, function (key, dict) {
                                            if (dict.Value == defaultLang) {
                                                if (dict.Value.indexOf("_") != -1) {
                                                    ddlOption = new Option(dict.Value.split("_")[1], dict.Key);
                                                }
                                                else {
                                                    ddlOption = new Option(dict.Value, dict.Key);
                                                }
                                                // ddlOption = new Option(dict.Value.split("_")[1], dict.Key);
                                                z("selLang").options.add(ddlOption);
                                                z("selLang").options[key].selected = true;
                                            }
                                            else {
                                                if (dict.Value.indexOf("_") != -1) {
                                                    ddlOption = new Option(dict.Value.split("_")[1], dict.Key);
                                                }
                                                else {
                                                    ddlOption = new Option(dict.Value, dict.Key);
                                                }

                                                z("selLang").options.add(ddlOption);
                                            }
                                        });
                                    }
                                    break;
                                default: { }
                            }

                        });
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

    }
    catch (ex) {
        
    }
}

function CallBack()
{ }






