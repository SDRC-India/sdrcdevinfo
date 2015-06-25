// JScript File
var ErrorMessage = '';
var SubscriptionCallBackPageName = "Callback.aspx";
var SubscriptionSelectedCatGIDAndScheme = "";
var txtRegistryURNCtlId = "txtRegistryURN";
var txtNotificationMailCtlId = "txtNotificationMail";
var txtNotificationHTTPCtlId = "txtNotificationHTTP";
var chkIsSOAPCtlId = "chkIsSOAP";
var txtStartDateCtlId = "txtStartDate";
var txtEndDateCtlId = "txtEndDate";

var MsgNotificationMailHTTP = "Atleast one field should contain value Notification Mail or Notification HTTP.";
var MsgStartDate = "Please Select Start Date.";
var MsgEndDate = "Please Select End Date.";
var MsgInvalidEmail = "Please enter valid Email Id.";
var MsgCategory = "Select atleast one category.";
var MsgMFD = "Select atleast one Metadata Flow from the list.";
var PivotColumnSeparator = "|";
var ValueSeparator = "~~";
var CommaSeparator = ",";

function onPageLoad(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hLoggedInUserNId, hLoggedInUserName, hOriginaldbnid) {
    var hsgcount = 10;
    // ************************************************
    // create Form Tag with hidden input boxes
    // ************************************************
    createFormHiddenInputs("frm_sdmxWSDemo", "POST");
    SetCommonLinksHref("frm_sdmxWSDemo", "POST");
    SetOriginaldbnidInForm("frm_sdmxWSDemo", hOriginaldbnid);

    // ************************************************1
    // set page level variables based on selections or defaults
    // ************************************************1
    setPostedData(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hsgcount, hLoggedInUserNId, hLoggedInUserName);

    // ************************************************1
    // Load Language Component
    // ************************************************1
    ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', hlngcode);

    SetCommonRegistryPageDetails('RegWebServiceDemo.aspx', "frm_sdmxWSDemo", "POST");
}

function GenerateAPICall() 
{
    var InputParam;
    z('aAPIURLValue').href = "";
    z('aAPIURLValue').innerHTML = "";
    z('spanMethodValue').innerHTML = "";
    z('textRequest').innerHTML = "";
    z('textResponse').innerHTML = "";
    ApplyMaskingDiv();

    if (ValidateInputs()) 
    {
        ShowLoadingDiv();
        InputParam = z('hdbnid').value;
        InputParam += ParamDelimiter + z("selWebServiceFunction").options[z("selWebServiceFunction").selectedIndex].value;
        if ((z('selWebServiceFunction').value == 'QuerySubscription') || (z('selWebServiceFunction').value == 'QueryRegistration'))
        {
            InputParam += ParamDelimiter + document.getElementById('hLoggedInUserNId').value;
        }
        else if (z('selWebServiceFunction').value == 'SubmitSubscription')
        {
            InputParam += ParamDelimiter + document.getElementById('hLoggedInUserNId').value;
            InputParam += ParamDelimiter + ($$(chkIsSOAPCtlId, false).checked == true ? 1 : 0);
            InputParam += ParamDelimiter + $$(txtNotificationMailCtlId, false).value;
            InputParam += ParamDelimiter + $$(txtNotificationHTTPCtlId, false).value;
            InputParam += ParamDelimiter + $$(txtStartDateCtlId, false).value;
            InputParam += ParamDelimiter + $$(txtEndDateCtlId, false).value;
            InputParam += ParamDelimiter + z("selEventSelector").options[z("selEventSelector").selectedIndex].text;
            if (z("selEventSelector").options[z("selEventSelector").selectedIndex].text == "Data Registration") {
                InputParam += ParamDelimiter + SubscriptionSelectedCatGIDAndScheme;
                InputParam += ParamDelimiter + "0";
            }
            else if (z("selEventSelector").options[z("selEventSelector").selectedIndex].text == "Metadata Registration") {
                InputParam += ParamDelimiter + "0";
                InputParam += ParamDelimiter + z("selMFD").options[z("selMFD").selectedIndex].value;
            }
            else if (z("selEventSelector").options[z("selEventSelector").selectedIndex].text == "Structural Metadata Registration") {
                InputParam += ParamDelimiter + "0";
                InputParam += ParamDelimiter + "0";
            }
            // Adding language preference
            InputParam += ParamDelimiter + z("selLang").options[z("selLang").selectedIndex].value;
        }
        else if (z('selWebServiceFunction').value == 'SubmitRegistration') 
        {
            InputParam += ParamDelimiter + document.getElementById('hLoggedInUserNId').value;
            InputParam += ParamDelimiter + z("selDFDMFD").options[z("selDFDMFD").selectedIndex].value;
            InputParam += ParamDelimiter + z('txtFileURL').value;
        }
        else 
        {
            InputParam += ParamDelimiter + z('txtId').value;
            InputParam += ParamDelimiter + z('txtAgencyId').value;
            InputParam += ParamDelimiter + z('txtVersion').value;
        }

        try 
        {
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: CallBackPageName,
                data: { 'callback': '119', 'param1': InputParam },
                async: true,
                success: function (data) {
                    try {
                        var Output = data.split(ParamDelimiter);
                        z('aAPIURLValue').href = Output[0];
                        z('aAPIURLValue').innerHTML = Output[0];
                        z('spanMethodValue').innerHTML = Output[1];
                        z('textRequest').innerHTML = Output[2];
                        z('textResponse').innerHTML = Output[3];
                        z('aViewRequest').href = Output[4];
                        z('aViewResponse').href = Output[5];

                        HideLoadingDiv();
                        RemoveMaskingDiv();
                    }
                    catch (ex) {
                        alert("message:" + ex.message);

                        HideLoadingDiv();
                        RemoveMaskingDiv();
                    }
                },
                error: function () {
                    
                    HideLoadingDiv();
                    RemoveMaskingDiv();
                },
                cache: false
            });
        }
        catch (ex) {
            HideLoadingDiv();
            RemoveMaskingDiv();
        }
    }
    else 
    {
        RemoveMaskingDiv();
    }

}


function ValidateInputs() {
    var RetVal;

    RetVal = true;

    if (z('selWebServiceFunction').value == 'SubmitSubscription') 
    {
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
        else if (z("selEventSelector").options[z("selEventSelector").selectedIndex].text == "Data Registration") {
            if (SubscriptionSelectedCatGIDAndScheme == '') {
                RetVal = false;
                ShowMessage(MsgCategory);
            }
        }
        else if (z("selEventSelector").options[z("selEventSelector").selectedIndex].text == "Metadata Registration") {
            if (z("selMFD").selectedIndex <= 0) {
                RetVal = false;
                ShowMessage(MsgMFD);
                z("selMFD").focus();
            }
        } 
    }
    else if (z('selWebServiceFunction').value == 'SubmitRegistration') 
    {
        if (z('selDFDMFD').selectedIndex <= 0) {
            RetVal = false;
            ErrorMessage = "Select Data/Metadata Flows!";
            z('selDFDMFD').focus();
        }
        else if ((z('txtFileURL').value == null) || (z('txtFileURL').value == "")) {
            RetVal = false;
            ErrorMessage = "Please enter SDMX-ML /Metadata Report File URL!";
            z('txtFileURL').focus();
        }
        else if (z('txtFileURL').value != "" && z('txtFileURL').value != null) 
        {
            InputParam = z('hdbnid').value;
            InputParam += ParamDelimiter + z('txtFileURL').value;
            InputParam += ParamDelimiter + z('chkIsMetadata').checked;
            InputParam += ParamDelimiter + z("selDFDMFD").options[z("selDFDMFD").selectedIndex].value;

            try {
                var htmlResp = di_jq.ajax({
                    type: "POST",
                    url: CallBackPageName,
                    data: { 'callback': '85', 'param1': InputParam },
                    async: false,
                    success: function (data) {
                        if (data == "Valid") {
                            RetVal = true;
                            ErrorMessage = '';
                        }
                        else {
                            RetVal = false;
                            if (data == "") {
                                ErrorMessage = "Incorrect URL of SDMX-ML /Metadata Report File to be registered.";
                            }
                            else {
                                ErrorMessage = data;
                            }

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
      
    }
    else if ((z('selWebServiceFunction').value != 'QuerySubscription') && (z('selWebServiceFunction').value != 'QueryRegistration')) 
    {
        if (z('txtId').value == null || z('txtId').value == '') {
            ErrorMessage = "Please enter some value in the Id Field.!";
            alert(ErrorMessage);
            z('txtId').focus();
            RetVal = false;
        }

        else if (z('txtAgencyId').value == null || z('txtAgencyId').value == '') {
            ErrorMessage = "Please enter some value in the Agency Id Field.!";
            alert(ErrorMessage);
            z('txtAgencyId').focus();
            RetVal = false;
        }

        else if (z('txtVersion').value == null || z('txtVersion').value == '') {
            ErrorMessage = "Please enter some value in the Version Field.!";
            alert(ErrorMessage);
            z('txtVersion').focus();
            RetVal = false;
        }
    }
   

    return RetVal;
}

function DownloadContent(preElement) {
    var form = document.createElement("form");
    form.setAttribute("method", "post");
    form.setAttribute("action", "Download.aspx");

    var hiddenField = document.createElement("input");
    hiddenField.setAttribute("style", "display:none;");
    hiddenField.setAttribute("name", "hDownload");
    hiddenField.setAttribute("value", z(preElement).innerText);
    form.appendChild(hiddenField);

    hiddenField = document.createElement("input");
    hiddenField.setAttribute("style", "display:none;");
    hiddenField.setAttribute("name", "hFileName");
    hiddenField.setAttribute("value", preElement.replace("text", ""));
    form.appendChild(hiddenField);

    hiddenField = document.createElement("input");
    hiddenField.setAttribute("style", "display:none;");
    hiddenField.setAttribute("name", "hFileType");

    if (preElement.replace("text", "") == "Response") {
        hiddenField.setAttribute("value", "xml");
    }
    else if (preElement.replace("text", "") == "Request") {
        hiddenField.setAttribute("value", "xml");
    }

    form.appendChild(hiddenField);

    document.body.appendChild(form);

    form.submit();
}

function CopyContent(preElement) 
{
    var windowClipboard = window.open('', '', 'width=600,height=500,top=0,left=0');
    windowClipboard.document.write(z(preElement).innerHTML);
    windowClipboard.focus();
}

function ClearSelection() 
{
    z('txtId').value = "";
    z('txtAgencyId').value = "";
    z('txtVersion').value = "";
    z('iId').innerHTML = "";
    z('iAgencyId').innerHTML = "";
    z('iVersion').innerHTML = "";
    z('aAPIURLValue').href = "";
    z('aAPIURLValue').innerHTML = "";
    z('spanMethodValue').innerHTML = "";
    z('textRequest').innerHTML = "";
    z('textResponse').innerHTML = "";

    z('aViewRequest').href = "";
    z('aViewResponse').href = "";
}

function GetIndicativeIdAgencyIdVersionValues() {
    var InputParam;

    if (z('selWebServiceFunction').value != '0') 
    {
        ClearSelection();
        if ((z('selWebServiceFunction').value == 'QuerySubscription') || (z('selWebServiceFunction').value == 'QueryRegistration'))
        {
            z('divIdAgencyIdVersionEntry').style.display = "none";
            z('divSubmitSubscription').style.display = "none";
            z('divSubmitRegistration').style.display = "none";
            z('spanSampleValues').style.display = "none";
            if ((z('hLoggedInUserNId').value == null) || (z('hLoggedInUserNId').value == '')) 
            {
                alert('Please login to view the working of this functions!');
                z('selWebServiceFunction').selectedIndex = 0;
            }
        }
        else if (z('selWebServiceFunction').value == 'SubmitSubscription') {

            z('spanSampleValues').style.display = "none";
            if ((z('hLoggedInUserNId').value == null) || (z('hLoggedInUserNId').value == '')) {
                alert('Please login to view the working of this functions!');
                z('selWebServiceFunction').selectedIndex = 0;
            }
            else 
            {
                z('divIdAgencyIdVersionEntry').style.display = "none";
                z('divSubmitRegistration').style.display = "none";
                z('divSubmitSubscription').style.display = "";
                PopulateLangPref();
                ClearSubscriptionCtrlValues();
            }
        }
        else if (z('selWebServiceFunction').value == 'SubmitRegistration') 
        {
            z('spanSampleValues').style.display = "none";
            if ((z('hLoggedInUserNId').value == null) || (z('hLoggedInUserNId').value == '')) 
            {
                alert('Please login to view the working of this functions!');
                z('selWebServiceFunction').selectedIndex = 0;
            }
            else {
                z('divIdAgencyIdVersionEntry').style.display = "none";
                z('divSubmitSubscription').style.display = "none";
                z('divSubmitRegistration').style.display = "";
                ClearRegistrationCtrlValues();
            }
        }
        else 
        {
            z('divIdAgencyIdVersionEntry').style.display = "";
            z('divSubmitSubscription').style.display = "none";
            z('divSubmitRegistration').style.display = "none";
            z('spanSampleValues').style.display = "";
            
            ShowLoadingDiv();
            ApplyMaskingDiv();

            InputParam = z('hdbnid').value;
            InputParam += ParamDelimiter + z('selWebServiceFunction').value;

            try {
                var htmlResp = di_jq.ajax({
                    type: "POST",
                    url: CallBackPageName,
                    data: { 'callback': '134', 'param1': InputParam },
                    async: true,
                    success: function (data) {
                        try {
                            ClearSelection();

                            z('iId').innerHTML = data.split(',')[0];
                            z('iAgencyId').innerHTML = data.split(',')[1];
                            z('iVersion').innerHTML = data.split(',')[2];

                            HideLoadingDiv();
                            RemoveMaskingDiv();
                        }
                        catch (ex) {
                            alert("message:" + ex.message);

                            HideLoadingDiv();
                            RemoveMaskingDiv();
                        }
                    },
                    error: function () {
                        

                        HideLoadingDiv();
                        RemoveMaskingDiv();
                    },
                    cache: false
                });
            }
            catch (ex) {
                alert("message:" + ex.message);

                HideLoadingDiv();
                RemoveMaskingDiv();
            }
        }
        
    }

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


function ClearRegistrationCtrlValues() {
    try {
        z('chkIsMetadata').checked = false;
        BindDFDAndMFDList();
//        z('txtWebServiceURL').value = "";

//        z('chkIsREST').checked = false;
//        z('txtWADLURL').value = "";

//        z('chkIsSOAPRegistration').checked = false;
//        z('txtWSDLURL').value = "";

        z('txtFileURL').value = "";
    }
    catch (err) { }
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
        if ($$(ctrl, true).value.trim() == "") {
            RetVal = true;
            FocusCtrl($$(ctrl, true));
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
                    alert("message:" + ex.message);
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

function BindICTypes(SubscriptionSelectedCatGIDAndScheme) {
    try {

        InputParam = document.getElementById('hdbnid').value;
        InputParam += ParamDelimiter + SubscriptionSelectedCatGIDAndScheme;

        var htmlResp = di_jq.ajax({
            type: "GET",
            url: "Callback.aspx",
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
                    alert("message:" + ex.message);
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
            url: "Callback.aspx",
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
                    alert("message:" + ex.message);
                }
            },
            error: function () {
                HideLoadingDiv();
                
            },
            cache: false
        });

        di_jq('#' + ICType).checkboxTree({
            trueCheck: {
                val: 'yes'
            },
            onCheck: {
                ancestors: '',
                descendants: 'check'
            },
            onUncheck: {
                ancestors: '',
                descendants: 'uncheck'
            },
            initializeChecked: 'collapsed',
            initializeUnchecked: 'collapsed'
        });
    }
    catch (ex) {

    }

}

function BindRegCategoryOrDataFlows() {
    if (z("selEventSelector").options[z("selEventSelector").selectedIndex].text == "Data Registration") {
        document.getElementById('divCategory').style.display = "";
        document.getElementById('divMetadataFlows').style.display = "none";
    }
    else if (z("selEventSelector").options[z("selEventSelector").selectedIndex].text == "Metadata Registration") {
        document.getElementById('divCategory').style.display = "none";
        document.getElementById('divMetadataFlows').style.display = "";
        BindMFDList();
    }
    else if (z("selEventSelector").options[z("selEventSelector").selectedIndex].text == "Structural Metadata Registration") {
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
            url: "Callback.aspx",
            data: "callback=97&param1=" + InputParam,
            async: false,
            success: function (data) {
                try {
                    z("selMFD").length = 1;
                    var MFDs = data.split(CommaSeparator);
                    var i;
                    for (i = 0; i < MFDs.length; i++) 
                    {
                        z("selMFD").options[z("selMFD").length] = new Option(MFDs[i].split("~~")[0], MFDs[i].split("~~")[1]);
                    }

                    HideLoadingDiv();
                }
                catch (ex) {
                    HideLoadingDiv();
                    alert("message:" + ex.message);
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

function EnableDisableAssociatedTextBox(txtBoxId) {
    var srcElement;

    srcElement = this.event.srcElement || this.event.target;

    if (srcElement.checked == true) {
        z(txtBoxId).disabled = "";
    }
    else {
        z(txtBoxId).disabled = "disabled";
    }

    z(txtBoxId).value = '';
}

function BindDFDAndMFDList() {
    ClearFileUrl();
    try {
        if (z('chkIsMetadata').checked == true) {
            InputParam = z('hdbnid').value;
            InputParam += ParamDelimiter + z('hlngcodedb').value;
            InputParam += ParamDelimiter + "MFD";
        }
        else {
            InputParam = z('hdbnid').value;
            InputParam += ParamDelimiter + z('hlngcodedb').value;
            InputParam += ParamDelimiter + "DFD";
        }


        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: "callback=98&param1=" + InputParam,
            async: false,
            success: function (data) {
                try {
                    z("selDFDMFD").length = 1;
                    var MFDs = data.split(",");
                    var i;
                    for (i = 0; i < MFDs.length; i++) {
                        z("selDFDMFD").options[z("selDFDMFD").length] = new Option(MFDs[i].split("~~")[0], MFDs[i].split("~~")[1]);
                    }

                    HideLoadingDiv();
                }
                catch (ex) {
                    HideLoadingDiv();
                    alert("message:" + ex.message);
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
                                                ddlOption = new Option(dict.Value.split("_")[1], dict.Key);
                                                z("selLang").options.add(ddlOption);
                                                z("selLang").options[key].selected = true;
                                            }
                                            else {
                                                ddlOption = new Option(dict.Value.split("_")[1], dict.Key);
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

function ClearFileUrl() 
{
    z('txtFileURL').value = "";
}








