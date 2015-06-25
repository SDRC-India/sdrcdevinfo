var ErrorMessage = '';

function onPageLoad(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hLoggedInUserNId, hLoggedInUserName) {
    var hsgcount = 10;

    createFormHiddenInputs("frm_DataQuery", "POST");
    SetCommonLinksHref("frm_DataQuery", "POST");
    setPostedData(hdsby, hdbnid, '', '', hlngcode, hlngcodedb, '', '', hsgcount, hLoggedInUserNId, hLoggedInUserName);

   

    // ************************************************1
    // Load Language Component
    // ************************************************1
    ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', hlngcode);
//    if (GetLanguageCounts() > 1) {
//        z("LanguagePipeLi").style.display = "";        
//        z('lngcode_div').style.display = "none";
//    }

    FillLanguageDropDown();

    if (getQueryVariable("lang") == null) {
        LoadLanguage("en");
    }
    else {
        LoadLanguage(getQueryVariable("lang"));
    }
}

function GetRequests() {
    var InputParam;

    ApplyMaskingDiv();
    ShowLoadingDiv();

    if (ValidateInputs()) {
        try {
            InputParam = z('hdbnid').value;
            InputParam += ParamDelimiter + z('selectLanguage').value;

            if (z('hselind').value != null) {
                InputParam += ParamDelimiter + z('hselind').value;
            }
            else {
                InputParam += ParamDelimiter + '';
            }

            if (z('hselarea').value != null) {
                InputParam += ParamDelimiter + z('hselarea').value;
            }
            else {
                InputParam += ParamDelimiter + '';
            }

            InputParam += ParamDelimiter + z('chkMRD').checked;
            InputParam += ParamDelimiter + z('hdnTimeRange').value;

            if (z('hdnSource').value != null) {
                InputParam += ParamDelimiter + z('hdnSource').value;
            }
            else {
                InputParam += ParamDelimiter + '';
            }
            InputParam += ParamDelimiter + z('selectResponseFormat').value;
            InputParam += ParamDelimiter + z('selectSDMXFormat').value;
            if (z('selectRequestFormat').value == "0")//SOAP
            {
                GetSOAPRequest(InputParam, "xml");
            }
            else if (z('selectRequestFormat').value == "1")//REST
            {
                GetRESTRequest(InputParam, "txt");
            }
            else if (z('selectRequestFormat').value == "2")//HTTP
            {                
                if (z('chkboxTitle').checked) {
                    InputParam += ParamDelimiter + z('txtboxTitle').value;
                }
                else {
                    InputParam += ParamDelimiter + "";
                }
                if (z('chkboxFootnote').checked) {
                    InputParam += ParamDelimiter + z('txtboxFootnote').value;
                }
                else {
                    InputParam += ParamDelimiter + "";
                }
                InputParam += ParamDelimiter + z('chkboxGbyi').checked;
                GetHTTPRequest(InputParam, "txt");
            }
        }
        catch (ex) {
            z('tblRequestResponse').style.display = "none";
            HideLoadingDiv();
            RemoveMaskingDiv();
        }
    }
    else {
        z('tblRequestResponse').style.display = "none";
        alert(ErrorMessage);
        HideLoadingDiv();
        RemoveMaskingDiv();
    }
}

function ArrangeResponseOptions(selVal) {
    var responseDdl = z('selectResponseFormat');
    if (selVal == 2) {
        var item = document.createElement("option");
        item.value = 3;
        item.innerHTML = "Table";
        responseDdl.appendChild(item);
        if (di_jq("#selectResponseFormat").val() == "3") {
            di_jq("#tableFormatOptions").show();
        }
    }
    else {
        if (responseDdl.options[3]) {
            responseDdl.removeChild(responseDdl.options[3]);
            di_jq("#tableFormatOptions").hide();
            di_jq("#selectSDMXFormat").show();            
        }
    }
}

function GetResponses(Request) {
    var InputParam;

    InputParam = z('hdbnid').value
    InputParam += ParamDelimiter + z('selectRequestFormat').value;
    InputParam += ParamDelimiter + Request;
    InputParam += ParamDelimiter + z('selectSDMXFormat').value;

    if (z('selectResponseFormat').value == "0")//SDMX
    {
        GetSDMXMLResponse(InputParam, "xml");
    }
    else if (z('selectResponseFormat').value == "1")//JSON
    {
        GetJSONResponse(InputParam, "txt");
    }
    else if (z('selectResponseFormat').value == "2")//XML
    {
        GetXMLResponse(InputParam, "xml");
    }
    else if (z('selectResponseFormat').value == "3")//TABLE
    {
        GetXMLResponse(InputParam, "txt");
    }
}

function showHideComponent(comp) {
    if (comp == 'area') {
        ApplyMaskingDiv();
        ShowLoadingDiv();

        SetCloseButtonInPopupDiv(di_jq('#AreaOuterBox'), 'showHideComponent');
        di_jq('#AreaOuterBox').show('slow');

        GetWindowCentered(z("AreaOuterBox"), 720, 520);

        ShowAreaComponent(getAbsURL('stock'), 'area_div', z("hdbnid").value, z("hlngcodedb").value, z("hselareao").value, '700', '400');
    }
    else if (comp == 'ind') {
        ApplyMaskingDiv();
        ShowLoadingDiv();

        SetCloseButtonInPopupDiv(di_jq('#IndOuterBox'), 'showHideComponent');
        di_jq('#IndOuterBox').show('slow');

        GetWindowCentered(z("IndOuterBox"), 710, 520);

        ShowIndicatorComponent(getAbsURL('stock'), 'indicator_div', z("hdbnid").value, z("hlngcodedb").value, z("hselindo").value, '700', '400', z("hsgcount").value);
    }
    else if (comp == 'tp') {
        ApplyMaskingDiv();
        ShowLoadingDiv();

        SetCloseButtonInPopupDiv(di_jq('#TimeRangeOuterBox'), 'showHideComponent');
        di_jq('#TimeRangeOuterBox').show('slow');

        FillTimeRangeDiv();

        GetWindowCentered(z("TimeRangeOuterBox"), 710, 520);
    }
    else if (comp == 'source') {
        ApplyMaskingDiv();
        ShowLoadingDiv();

        SetCloseButtonInPopupDiv(di_jq('#SourceOuterBox'), 'showHideComponent');
        di_jq('#SourceOuterBox').show('slow');

        FillSourceDiv();

        GetWindowCentered(z("SourceOuterBox"), 710, 520);
    }
    else if (comp == 'close' || comp == null) {
        HideLoadingDiv();
        RemoveMaskingDiv();

        di_jq('#AreaOuterBox').hide('slow');
        di_jq('#IndOuterBox').hide('slow');
        di_jq('#TimeRangeOuterBox').hide('slow');
        di_jq('#SourceOuterBox').hide('slow');
    }
}

function onClickOkPopUpButton(comp) {
    var delimiter;
    var SelectedText, SelectedIndicators, SelectedAreas;

    delimiter = "||{~~}||";
    SelectedText = "";

    if (comp == 'area') {
        z('hselarea').value = AreaCompObj.getSelectedAreas().split(delimiter)[0];
        z('hselareao').value = AreaCompObj.getSelectedAreas().split(delimiter)[1];
        SelectedAreas = jQuery.parseJSON(z('hselareao').value); 
        SelectedText = '';

        if (SelectedAreas != null && SelectedAreas != undefined && SelectedAreas.area != null && SelectedAreas.area != undefined) {
            for (level in SelectedAreas.area) {
                if (level != null && SelectedAreas.area[level] != null) {
                    for (i = 0; i < SelectedAreas.area[level].length; i++) {
                        SelectedText += SelectedAreas.area[level][i].split('||')[1].split(' (')[0] + ",";
                    }
                }
            }
        }

        if (SelectedText != null && SelectedText != '') {
            SelectedText = SelectedText.substr(0, SelectedText.length - 1);
        }

        SetSelectedText(SelectedText, 'spanSelectedAreas');
    }
    else if (comp == 'ind') {
        z('hselind').value = IndCompObj.getSelectedData().split(delimiter)[0];
        z('hselindo').value = IndCompObj.getSelectedData().split(delimiter)[1];
        SelectedIndicators = jQuery.parseJSON(z('hselindo').value); 
        SelectedText = '';

        for (i = 0; i < SelectedIndicators.iu.length; i++) {
            SelectedText += SelectedIndicators.iu[i].split('||')[1].split('~')[0] + ",";
        }

        if (SelectedText != null && SelectedText != '') {
            SelectedText = SelectedText.substr(0, SelectedText.length - 1);
        }

        SetSelectedText(SelectedText, 'spanSelectedIndicators');
    }
    else if (comp == 'tp') {
        var tblTimeRange = z('tblTimeRange');
        z('hdnTimeRange').value = '-1';

        for (i = 1; i < tblTimeRange.rows.length; i++) {
            if (tblTimeRange.rows[i].cells[0].childNodes[0].checked == true) {
                z('hdnTimeRange').value += ',' + tblTimeRange.rows[i].cells[0].childNodes[0].value;
                SelectedText += tblTimeRange.rows[i].cells[1].childNodes[0].innerHTML + ",";
            }
        }

        if (SelectedText != null && SelectedText != '') {
            SelectedText = SelectedText.substr(0, SelectedText.length - 1);
        }

        SetSelectedText(SelectedText, 'spanSelectedTPs');
    }
    else if (comp == 'source') {
        var tblSource = z('tblSource');
        z('hdnSource').value = '-1';

        for (i = 1; i < tblSource.rows.length; i++) {
            if (tblSource.rows[i].cells[0].childNodes[0].checked == true) {
                z('hdnSource').value += ',' + tblSource.rows[i].cells[0].childNodes[0].value;
                SelectedText += tblSource.rows[i].cells[1].childNodes[0].innerHTML + ",";
            }
        }

        if (SelectedText != null && SelectedText != '') {
            SelectedText = SelectedText.substr(0, SelectedText.length - 1);
        }

        SetSelectedText(SelectedText, 'spanSelectedSources');
    }

    showHideComponent('close');
}

function DownloadContent(preElement) {
    var downloadContent = z(preElement).innerHTML;
    var form = document.createElement("form");
    form.setAttribute("method", "post");
    form.setAttribute("action", "Download.aspx");

    var hiddenField = document.createElement("input");
    hiddenField.setAttribute("style", "display:none;");
    hiddenField.setAttribute("name", "hDownload");

    if (z('selectResponseFormat').value == 3) {
        downloadContent = "<iframe frameborder='0' style='width: 100%;height: 100%;' scrolling='auto' noresize='' marginwidth='0' marginheight='0' src='" + z('textRequest').innerHTML +
                 "'></iframe>";
    }
    hiddenField.setAttribute("value", downloadContent);

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
        hiddenField.setAttribute("value", z('hdnResponseContentType').value);
    }
    else if (preElement.replace("text", "") == "Request") {
        hiddenField.setAttribute("value", z('hdnRequestContentType').value);
    }

    form.appendChild(hiddenField);

    document.body.appendChild(form);

    form.submit();
}

function CopyContent(preElement) {
    var windowClipboard = window.open('', '', 'width=600,height=500,top=0,left=0');
    windowClipboard.document.write(z(preElement).innerHTML);
    windowClipboard.focus();
}

function SelectUnselectAllTPs() {
    var tblTimeRange = z('tblTimeRange');

    for (i = 1; i < tblTimeRange.rows.length; i++) {
        if (tblTimeRange.rows[0].cells[0].childNodes[0].checked == true) {
            tblTimeRange.rows[i].cells[0].childNodes[0].checked = true;
        }
        else {
            tblTimeRange.rows[i].cells[0].childNodes[0].checked = false;
        }
    }
}

function EnableDisableTimeRangeDropDowns() {
    if (z('chkMRD').checked) {
        EnableDisableLinks(z('aTimeRange'), true);
        //cleare  html of span spanSelectedTPs in case of check box is checked
        if (z('spanSelectedTPs').innerHTML != '') {
            SetSelectedText("", 'spanSelectedTPs');
        }
        if(z('hdnTimeRange').value != '') {
            z('hdnTimeRange').value = '';
        }

    }
    else {
        EnableDisableLinks(z('aTimeRange'), false);
    }
}

function SelectUnselectAllSources() {
    var tblSource = z('tblSource');

    for (i = 1; i < tblSource.rows.length; i++) {
        if (tblSource.rows[0].cells[0].childNodes[0].checked == true) {
            tblSource.rows[i].cells[0].childNodes[0].checked = true;
        }
        else {
            tblSource.rows[i].cells[0].childNodes[0].checked = false;
        }
    }
}

function ShowHideTableSDMX() {
    if (z('selectResponseFormat').value == 0) {
        z('selectSDMXFormat').style.display = "";
    }
    else {
        z('selectSDMXFormat').style.display = "none";
    }
}

function ShowHideTableOptions(val) {
    if (val == 3) {
        ResetTableFormatOptions();
        di_jq("#tableFormatOptions").show();
    }
    else {
        di_jq("#tableFormatOptions").hide();
    }
}

function ResetTableFormatOptions() {
    di_jq("#tableFormatOptions input[type=checkbox]").attr("checked", false);
    di_jq("#tableFormatOptions input[type=text]").attr("disabled", true);
    di_jq("#tableFormatOptions input[type=text]").val('');
}

function EnableDiableTextbox(chkbox, txtboxId) {
    if (chkbox.checked) {
        di_jq("#" + txtboxId).attr("disabled", false);
    }
    else {
        di_jq("#" + txtboxId).attr("disabled", true);
    }
}


function GetSOAPRequest(InputParam, RequestContentType) {
    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: { 'callback': '76', 'param1': InputParam },
            async: true,
            success: function (data) {
                try {
                    HandleRequest(data, RequestContentType);
                }
                catch (ex) {
                    alert("message:" + ex.message);

                    HideLoadingDiv();
                    RemoveMaskingDiv();
                }
            },
            error: function () {
            //    

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

function GetRESTRequest(InputParam, RequestContentType) {
    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: { 'callback': '89', 'param1': InputParam },
            async: true,
            success: function (data) {
                try {
                    if (data.split(ParamDelimiter)[0].length < 2048) {
                        HandleRequest(data, RequestContentType);
                    }
                    else {
                        alert('Request URL length has exceeded 2048 characters limit.');
                        HideLoadingDiv();
                        RemoveMaskingDiv();
                    }
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

function GetHTTPRequest(InputParam, RequestContentType) {
    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: { 'callback': '88', 'param1': InputParam },
            async: true,
            success: function (data) {
                try {
                    if (data.length < 2048) {
                        HandleRequest(data, RequestContentType);
                    }
                    else {
                        alert('Request URL length has exceeded 2048 characters limit.');
                        HideLoadingDiv();
                        RemoveMaskingDiv();
                    }
                }
                catch (ex) {
                    alert("message:" + ex.message);

                    HideLoadingDiv();
                    RemoveMaskingDiv();
                }
            },
            error: function () {
             //   

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

function HandleRequest(data, RequestContentType) {
    if (data != null) {
        z('textRequest').innerHTML = ReplaceAll(data, "~~", "<br/><br/>"); //replace is done for rest requests.
        z('tblRequestResponse').style.display = "";
        z('hdnRequestContentType').value = RequestContentType;

        GetResponses(data.split(ParamDelimiter)[0]); //split is done for rest requests.
    }
}

function GetSDMXMLResponse(InputParam, ResponseContentType) {
    var RetVal;

    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: { 'callback': '77', 'param1': InputParam },
            async: true,
            success: function (data) {
                try {
                    HandleResponse(data, ResponseContentType);
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

    return RetVal;
}

function GetJSONResponse(InputParam, ResponseContentType) {
    var RetVal;

    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: { 'callback': '82', 'param1': InputParam },
            async: true,
            success: function (data) {
                try {
                    HandleResponse(data, ResponseContentType);
                }
                catch (ex) {
                    alert("message:" + ex.message);

                    HideLoadingDiv();
                    RemoveMaskingDiv();
                }
            },
            error: function () {
             //   

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

    return RetVal;
}

function GetXMLResponse(InputParam, ResponseContentType) {
    var RetVal;

    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: { 'callback': '83', 'param1': InputParam },
            async: true,
            success: function (data) {
                try {
                    HandleResponse(data, ResponseContentType);
                }
                catch (ex) {
                    alert("message:" + ex.message);
                    HideLoadingDiv();
                    RemoveMaskingDiv();
                }
            },
            error: function () {
             //   

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

    return RetVal;
}

function HandleResponse(data, ResponseContentType) {
    if (data != null) {
        if (z('selectRequestFormat').value == "0")//SOAP
        {
            z('textResponse').innerHTML = data.split(ParamDelimiter)[0];
            z('tblSoapAPI').style.display = "";

            z('aAPIURLValue').innerHTML = data.split(ParamDelimiter)[1];
            z('aAPIURLValue').href = data.split(ParamDelimiter)[1];
            z('spanSoapMethodValue').innerHTML = data.split(ParamDelimiter)[2];
        }
        else if (z('selectRequestFormat').value == "1")//REST
        {
            z('textResponse').innerHTML = data;
            z('tblSoapAPI').style.display = "none";
        }
        else if (z('selectRequestFormat').value == "2")//HTTP/TABLE
        {
            if (z('selectResponseFormat').value == "2" || z('selectResponseFormat').value == "1" || z('selectResponseFormat').value == "0") {//HTTP
                z('textResponse').innerHTML = data;
            }
            else if (z('selectResponseFormat').value == "3") { //TABLE                         
                var html = "<span>&lt;</span><span>iframe frameborder='0' style='width: 100%;height: 100%;' scrolling='auto' noresize='' marginwidth='0' marginheight='0'</span><span> src='" + z('textRequest').innerHTML +
                 "'&gt;&lt;</span><span>/iframe&gt;</span>";
                z('textResponse').innerHTML = html;     
            }
            z('tblSoapAPI').style.display = "none";
        }
        
        z('tblRequestResponse').style.display = "";
        z('hdnResponseContentType').value = ResponseContentType;
    }

    HideLoadingDiv();
    RemoveMaskingDiv();
}

function SetSelectedText(text, spanId) {
    z(spanId).innerHTML = '';
    z(spanId).title = '';

    if (text.length > 120) {
        z(spanId).innerHTML = text.substr(0, 120) + "...";
        z(spanId).title = text;
    }
    else {
        z(spanId).innerHTML = text;
        z(spanId).title = text;
    }
}

function FillLanguageDropDown() {
    var languageOption;

    if (z('lngcode_div') != null && z('lngcode_div').childNodes.length > 0) {
        for (i = 0; i < z('lngcode_div').childNodes[0].options.length; i++) {
            languageOption = new Option(z('lngcode_div').childNodes[0].options[i].text, z('lngcode_div').childNodes[0].options[i].value);
            z('selectLanguage').options.add(languageOption);

            if (z('lngcode_div').childNodes[0].options[i].selected == true) {
                z('selectLanguage').options[i].selected = true;
            }
        }
    }
}

function FillTimeRangeDiv() {
    var InputParam;
    var tpOption;

    InputParam = z('hdbnid').value;

    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: { 'callback': '81', 'param1': InputParam },
            async: false,
            success: function (data) {
                try {
                    if (data != null && data != '') {
                        z('divTimeRange').innerHTML = data;
                    }

                    for (i = 0; i < z('hdnTimeRange').value.split(',').length; i++) {
                        if (z('chkTimeRange_' + z('hdnTimeRange').value.split(',')[i]) != null) {
                            z('chkTimeRange_' + z('hdnTimeRange').value.split(',')[i]).checked = true;
                        }
                    }
                }
                catch (ex) {
                    alert("message:" + ex.message);
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

function FillSourceDiv() {
    var InputParam;
    var tpOption;

    InputParam = z('hdbnid').value;
    InputParam += ParamDelimiter + z('hlngcodedb').value;

    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: { 'callback': '124', 'param1': InputParam },
            async: false,
            success: function (data) {
                try {
                    if (data != null && data != '') {
                        z('divSource').innerHTML = data;
                    }

                    for (i = 0; i < z('hdnSource').value.split(',').length; i++) {
                        if (z('chkSource_' + z('hdnSource').value.split(',')[i]) != null) {
                            z('chkSource_' + z('hdnSource').value.split(',')[i]).checked = true;
                        }
                    }
                }
                catch (ex) {
                    alert("message:" + ex.message);
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


function ValidateInputs() {
    var RetVal;

    RetVal = true;

    if ((z('hselarea').value == null || z('hselarea').value == '') && (z('hselind').value == null || z('hselind').value == '')) {
        ErrorMessage = "Please select an area or an indicator!";
        RetVal = false;
    }

    return RetVal;
}

function getQueryVariable(variable) {
    var query = window.location.search.substring(1);
    var vars = query.split("&");

    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        if (pair[0] == variable) {
            return pair[1];
        }
    }
}

function abc() {
    return "Javascript Title";
}