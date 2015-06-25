var ErrorMessage = '';

function onPageLoad(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hLoggedInUserNId, hLoggedInUserName, hOriginaldbnid) {
    var hsgcount = 10;
    // ************************************************
    // create Form Tag with hidden input boxes
    // ************************************************
    createFormHiddenInputs("frm_sdmxDiscover", "POST");
    SetCommonLinksHref("frm_sdmxDiscover", "POST");
    SetOriginaldbnidInForm("frm_sdmxDiscover", hOriginaldbnid);

    // ************************************************1
    // set page level variables based on selections or defaults
    // ************************************************1
    setPostedData(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hsgcount, hLoggedInUserNId, hLoggedInUserName);

    // ************************************************1
    // Load Language Component
    // ************************************************1
    ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', hlngcode);

    SetCommonRegistryPageDetails('RegDiscover.aspx', "frm_sdmxDiscover", "POST");

    Reset(true);
}

function Reset(DFDOrMFDFlag) {
    var rowIUSSeparator, rowIUS, rowIndicatorSeparator, rowIndicator, rowAreaSeparator, rowArea, rowTPSeparator, rowTP, rowSourceSeparator, rowSource,
        rowDiscoverSeparator, rowDiscover;
    var radioData, radioMetadata;

    rowIUSSeparator = z('rowIUSSeparator');
    rowIUS = z('rowIUS');
    rowIndicatorSeparator = z('rowIndicatorSeparator');
    rowIndicator = z('rowIndicator');
    rowAreaSeparator = z('rowAreaSeparator');
    rowArea = z('rowArea');
    rowTPSeparator = z('rowTPSeparator');
    rowTP = z('rowTP');
    rowSourceSeparator = z('rowSourceSeparator');
    rowSource = z('rowSource');
    rowDiscoverSeparator = z('rowDiscoverSeparator');
    rowDiscover = z('rowDiscover');

    radioData = z('radioData');
    radioMetadata = z('radioMetadata');

    rowIUSSeparator.style.display = "none";
    rowIUS.style.display = "none";
    rowIndicatorSeparator.style.display = "none";
    rowIndicator.style.display = "none";
    rowAreaSeparator.style.display = "none";
    rowArea.style.display = "none";
    rowTPSeparator.style.display = "none";
    rowTP.style.display = "none";
    rowSourceSeparator.style.display = "none";
    rowSource.style.display = "none";
    rowDiscoverSeparator.style.display = "none";
    rowDiscover.style.display = "none";

    if (DFDOrMFDFlag == true) {
        radioData.checked = true;
        radioMetadata.checked = false;
    }
    else {
        radioData.checked = false;
        radioMetadata.checked = true;
    }

    PopulateDFDMFDSelect(DFDOrMFDFlag);
}

function PopulateDFDMFDSelect(DFDOrMFDFlag) {
    var selectDFDMFD, InputParam;
    var dataRowSpilts;

    selectDFDMFD = z('selectDFDMFD');

    InputParam = z('hdbnid').value;
    InputParam += ParamDelimiter + z('hlngcodedb').value;
    InputParam += ParamDelimiter + DFDOrMFDFlag;

    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: { 'callback': '118', 'param1': InputParam },
            async: false,
            success: function (data) {
                try {
                    selectDFDMFD.options.length = 0;
                    selectDFDMFD.options.add(new Option("---- Select One ----", "-1"));

                    if (data != null && data != '') {
                        dataRowSpilts = data.split('#');

                        for (i = 0; i < dataRowSpilts.length; i++) {
                            selectDFDMFD.options.add(new Option(dataRowSpilts[i].split("|")[1], dataRowSpilts[i].split("|")[0]));
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

function ShowHideRowsForSelection() {
    var radioData, radioMetadata, selectDFDMFD;
    var rowIUSSeparator, rowIUS, rowIndicatorSeparator, rowIndicator, rowAreaSeparator, rowArea, rowTPSeparator, rowTP, rowSourceSeparator, rowSource,
        rowDiscoverSeparator, rowDiscover;

    radioData = z('radioData');
    radioMetadata = z('radioMetadata');
    selectDFDMFD = z('selectDFDMFD');

    rowIUSSeparator = z('rowIUSSeparator');
    rowIUS = z('rowIUS');
    rowIndicatorSeparator = z('rowIndicatorSeparator');
    rowIndicator = z('rowIndicator');
    rowAreaSeparator = z('rowAreaSeparator');
    rowArea = z('rowArea');
    rowTPSeparator = z('rowTPSeparator');
    rowTP = z('rowTP');
    rowSourceSeparator = z('rowSourceSeparator');
    rowSource = z('rowSource');
    rowDiscoverSeparator = z('rowDiscoverSeparator');
    rowDiscover = z('rowDiscover');

    if (radioData.checked == true && radioMetadata.checked == false) {
        if (selectDFDMFD.value == "DF_DevInfo") {
            rowIUSSeparator.style.display = "";
            rowIUS.style.display = "";
            rowIndicatorSeparator.style.display = "none";
            rowIndicator.style.display = "none";
            rowAreaSeparator.style.display = "";
            rowArea.style.display = "";
            rowTPSeparator.style.display = "";
            rowTP.style.display = "";
            rowSourceSeparator.style.display = "none";
            rowSource.style.display = "none";
            rowDiscoverSeparator.style.display = "";
            rowDiscover.style.display = "";
        }
        else {
            rowIUSSeparator.style.display = "none";
            rowIUS.style.display = "none";
            rowIndicatorSeparator.style.display = "none";
            rowIndicator.style.display = "none";
            rowAreaSeparator.style.display = "none";
            rowArea.style.display = "none";
            rowTPSeparator.style.display = "none";
            rowTP.style.display = "none";
            rowSourceSeparator.style.display = "none";
            rowSource.style.display = "none";
            rowDiscoverSeparator.style.display = "none";
            rowDiscover.style.display = "none";
        }
    }
    else {
        if (IsUploadedDSD() == false) {
            if (selectDFDMFD.value == "MF_Area_DevInfo") {
                rowIUSSeparator.style.display = "none";
                rowIUS.style.display = "none";
                rowIndicatorSeparator.style.display = "none";
                rowIndicator.style.display = "none";
                rowAreaSeparator.style.display = "";
                rowArea.style.display = "";
                rowTPSeparator.style.display = "none";
                rowTP.style.display = "none";
                rowSourceSeparator.style.display = "none";
                rowSource.style.display = "none";
                rowDiscoverSeparator.style.display = "";
                rowDiscover.style.display = "";
            }
            else if (selectDFDMFD.value == "MF_Indicator_DevInfo") {
                rowIUSSeparator.style.display = "none";
                rowIUS.style.display = "none";
                rowIndicatorSeparator.style.display = "";
                rowIndicator.style.display = "";
                rowAreaSeparator.style.display = "none";
                rowArea.style.display = "none";
                rowTPSeparator.style.display = "none";
                rowTP.style.display = "none";
                rowSourceSeparator.style.display = "none";
                rowSource.style.display = "none";
                rowDiscoverSeparator.style.display = "";
                rowDiscover.style.display = "";
            }
            else if (selectDFDMFD.value == "MF_Source_DevInfo") {
                rowIUSSeparator.style.display = "none";
                rowIUS.style.display = "none";
                rowIndicatorSeparator.style.display = "none";
                rowIndicator.style.display = "none";
                rowAreaSeparator.style.display = "none";
                rowArea.style.display = "none";
                rowTPSeparator.style.display = "none";
                rowTP.style.display = "none";
                rowSourceSeparator.style.display = "";
                rowSource.style.display = "";
                rowDiscoverSeparator.style.display = "";
                rowDiscover.style.display = "";
            }
            else {
                rowIUSSeparator.style.display = "none";
                rowIUS.style.display = "none";
                rowIndicatorSeparator.style.display = "none";
                rowIndicator.style.display = "none";
                rowAreaSeparator.style.display = "none";
                rowArea.style.display = "none";
                rowTPSeparator.style.display = "none";
                rowTP.style.display = "none";
                rowSourceSeparator.style.display = "none";
                rowSource.style.display = "none";
                rowDiscoverSeparator.style.display = "none";
                rowDiscover.style.display = "none";
            }
        }
        else {
            if (selectDFDMFD.value != "-1") {
                rowIUSSeparator.style.display = "none";
                rowIUS.style.display = "none";
                rowIndicatorSeparator.style.display = "";
                rowIndicator.style.display = "";
                rowAreaSeparator.style.display = "none";
                rowArea.style.display = "none";
                rowTPSeparator.style.display = "none";
                rowTP.style.display = "none";
                rowSourceSeparator.style.display = "none";
                rowSource.style.display = "none";
                rowDiscoverSeparator.style.display = "";
                rowDiscover.style.display = "";
            }
            else {
                rowIUSSeparator.style.display = "none";
                rowIUS.style.display = "none";
                rowIndicatorSeparator.style.display = "none";
                rowIndicator.style.display = "none";
                rowAreaSeparator.style.display = "none";
                rowArea.style.display = "none";
                rowTPSeparator.style.display = "none";
                rowTP.style.display = "none";
                rowSourceSeparator.style.display = "none";
                rowSource.style.display = "none";
                rowDiscoverSeparator.style.display = "none";
                rowDiscover.style.display = "none";
            }
        }
    }
}

function OpenTPPopup() {
    ApplyMaskingDiv();
    ShowLoadingDiv();

    if (z('hdbnid').value != '') {
        SetCloseButtonInPopupDiv(di_jq('#TPPopup'), 'TPPopupCancel', '../../stock/themes/default/images/close.png');
        di_jq('#TPPopup').show('slow');

        GetTPDivInnerHTML(z('hdbnid').value);

        GetWindowCentered(z("TPPopup"), 710, 520);
    }
    else {
        alert('Select a Database or a DSD');
        HideLoadingDiv();
        RemoveMaskingDiv();
    }
}

function TPPopupOk() {
    var SelectedText = "";
    var tblTP = z('tblTP');
    z('hdnSelectedTPs').value = '';

    for (i = 1; i < tblTP.rows.length; i++) {
        if (tblTP.rows[i].cells[0].childNodes[0].checked == true) {
            z('hdnSelectedTPs').value += tblTP.rows[i].cells[0].childNodes[0].value + ',';
            SelectedText += tblTP.rows[i].cells[1].childNodes[0].innerText + ",";
        }
    }

    if (z('hdnSelectedTPs').value != null && z('hdnSelectedTPs').value != '') {
        z('hdnSelectedTPs').value = z('hdnSelectedTPs').value.substr(0, z('hdnSelectedTPs').value.length - 1);
    }

    if (SelectedText != null && SelectedText != '') {
        SelectedText = SelectedText.substr(0, SelectedText.length - 1);
    }

    SetSelectedText(SelectedText, 'spanSelectedTPs');
    TPPopupCancel();
}

function TPPopupCancel() {
    HideLoadingDiv();
    RemoveMaskingDiv();

    di_jq('#TPPopup').hide('slow');
}

function ClearTPSelections() {
    z('hdnSelectedTPs').value = '';
    z('spanSelectedTPs').innerHTML = '';
    z('spanSelectedTPs').title = '';
}

function GetTPDivInnerHTML(DBNId) {
    var InputParam;

    InputParam = DBNId;

    if (InputParam != '') {
        try {
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: CallBackPageName,
                data: { 'callback': '120', 'param1': InputParam },
                async: false,
                success: function (data) {
                    try {
                        if (data != null && data != '') {
                            z('divTP').innerHTML = data;
                        }

                        for (i = 0; i < z('hdnSelectedTPs').value.split(',').length; i++) {
                            if (z('chkTP_' + z('hdnSelectedTPs').value.split(',')[i]) != null) {
                                z('chkTP_' + z('hdnSelectedTPs').value.split(',')[i]).checked = true;
                            }
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
        alert('Select a Database');
    }
}

function SelectUnselectAllTPs() {
    var tblTP = z('tblTP');

    for (i = 1; i < tblTP.rows.length; i++) {
        if (tblTP.rows[0].cells[0].childNodes[0].checked == true) {
            tblTP.rows[i].cells[0].childNodes[0].checked = true;
        }
        else {
            tblTP.rows[i].cells[0].childNodes[0].checked = false;
        }
    }
}

function OpenIndicatorPopup() {
    ApplyMaskingDiv();
    ShowLoadingDiv();

    if (z('hdbnid').value != '') {
        SetCloseButtonInPopupDiv(di_jq('#IndicatorPopup'), 'IndicatorPopupCancel', '../../stock/themes/default/images/close.png');
        di_jq('#IndicatorPopup').show('slow');

        GetIndicatorDivInnerHTML(z('hdbnid').value);

        GetWindowCentered(z("IndicatorPopup"), 710, 520);
    }
    else {
        alert('Select a Database or a DSD');
        HideLoadingDiv();
        RemoveMaskingDiv();
    }
}

function IndicatorPopupOk() {
    var SelectedText = "";
    var tblIndicator = z('tblIndicator');
    z('hdnSelectedIndicators').value = '';

    for (i = 1; i < tblIndicator.rows.length; i++) {
        if (tblIndicator.rows[i].cells[0].childNodes[0].checked == true) {
            z('hdnSelectedIndicators').value += tblIndicator.rows[i].cells[0].childNodes[0].value + ',';
            SelectedText += tblIndicator.rows[i].cells[1].childNodes[0].innerText + ",";
        }
    }

    if (z('hdnSelectedIndicators').value != null && z('hdnSelectedIndicators').value != '') {
        z('hdnSelectedIndicators').value = z('hdnSelectedIndicators').value.substr(0, z('hdnSelectedIndicators').value.length - 1);
    }

    if (SelectedText != null && SelectedText != '') {
        SelectedText = SelectedText.substr(0, SelectedText.length - 1);
    }

    SetSelectedText(SelectedText, 'spanSelectedIndicators');
    IndicatorPopupCancel();
}

function IndicatorPopupCancel() {
    HideLoadingDiv();
    RemoveMaskingDiv();

    di_jq('#IndicatorPopup').hide('slow');
}

function ClearIndicatorSelections() {
    z('hdnSelectedIndicators').value = '';
    z('spanSelectedIndicators').innerHTML = '';
    z('spanSelectedIndicators').title = '';
}

function GetIndicatorDivInnerHTML(DBNId) {
    var InputParam;

    InputParam = DBNId;

    if (InputParam != '') {
        try {
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: CallBackPageName,
                data: { 'callback': '121', 'param1': InputParam },
                async: false,
                success: function (data) {
                    try {
                        if (data != null && data != '') {
                            z('divIndicator').innerHTML = data;
                        }

                        for (i = 0; i < z('hdnSelectedIndicators').value.split(',').length; i++) {
                            if (z('chkIndicator_' + z('hdnSelectedIndicators').value.split(',')[i]) != null) {
                                z('chkIndicator_' + z('hdnSelectedIndicators').value.split(',')[i]).checked = true;
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
    else {
        alert('Select a Database or a DSD');
    }
}

function SelectUnselectAllIndicators() {
    var tblIndicator = z('tblIndicator');

    for (i = 1; i < tblIndicator.rows.length; i++) {
        if (tblIndicator.rows[0].cells[0].childNodes[0].checked == true) {
            tblIndicator.rows[i].cells[0].childNodes[0].checked = true;
        }
        else {
            tblIndicator.rows[i].cells[0].childNodes[0].checked = false;
        }
    }
}

function OpenAreaPopup() {
    ApplyMaskingDiv();
    ShowLoadingDiv();

    if (z('hdbnid').value != '') {
        SetCloseButtonInPopupDiv(di_jq('#AreaPopup'), 'AreaPopupCancel', '../../stock/themes/default/images/close.png');
        di_jq('#AreaPopup').show('slow');

        GetAreaDivInnerHTML(z('hdbnid').value);

        GetWindowCentered(z("AreaPopup"), 710, 520);
    }
    else {
        alert('Select a Database or a DSD');
        HideLoadingDiv();
        RemoveMaskingDiv();
    }
}

function AreaPopupOk() {
    var SelectedText = "";
    var tblArea = z('tblArea');
    z('hdnSelectedAreas').value = '';

    for (i = 1; i < tblArea.rows.length; i++) {
        if (tblArea.rows[i].cells[0].childNodes[0].checked == true) {
            z('hdnSelectedAreas').value += tblArea.rows[i].cells[0].childNodes[0].value + ',';
            SelectedText += tblArea.rows[i].cells[1].childNodes[0].innerText + ",";
        }
    }

    if (z('hdnSelectedAreas').value != null && z('hdnSelectedAreas').value != '') {
        z('hdnSelectedAreas').value = z('hdnSelectedAreas').value.substr(0, z('hdnSelectedAreas').value.length - 1);
    }

    if (SelectedText != null && SelectedText != '') {
        SelectedText = SelectedText.substr(0, SelectedText.length - 1);
    }

    SetSelectedText(SelectedText, 'spanSelectedAreas');
    AreaPopupCancel();
}

function AreaPopupCancel() {
    HideLoadingDiv();
    RemoveMaskingDiv();

    di_jq('#AreaPopup').hide('slow');
}

function ClearAreaSelections() {
    z('hdnSelectedAreas').value = '';
    z('spanSelectedAreas').innerHTML = '';
    z('spanSelectedAreas').title = '';
}

function GetAreaDivInnerHTML(DBNId) {
    var InputParam;

    InputParam = DBNId;

    if (InputParam != '') {
        try {
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: CallBackPageName,
                data: { 'callback': '122', 'param1': InputParam },
                async: false,
                success: function (data) {
                    try {
                        if (data != null && data != '') {
                            z('divArea').innerHTML = data;
                        }

                        for (i = 0; i < z('hdnSelectedAreas').value.split(',').length; i++) {
                            if (z('chkArea_' + z('hdnSelectedAreas').value.split(',')[i]) != null) {
                                z('chkArea_' + z('hdnSelectedAreas').value.split(',')[i]).checked = true;
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
    else {
        alert('Select a Database or a DSD');
    }
}

function SelectUnselectAllAreas() {
    var tblArea = z('tblArea');

    for (i = 1; i < tblArea.rows.length; i++) {
        if (tblArea.rows[0].cells[0].childNodes[0].checked == true) {
            tblArea.rows[i].cells[0].childNodes[0].checked = true;
        }
        else {
            tblArea.rows[i].cells[0].childNodes[0].checked = false;
        }
    }
}

function OpenSourcePopup() {
    ApplyMaskingDiv();
    ShowLoadingDiv();

    if (z('hdbnid').value != '') {
        SetCloseButtonInPopupDiv(di_jq('#SourcePopup'), 'SourcePopupCancel', '../../stock/themes/default/images/close.png');
        di_jq('#SourcePopup').show('slow');

        GetSourceDivInnerHTML(z('hdbnid').value);

        GetWindowCentered(z("SourcePopup"), 710, 520);
    }
    else {
        alert('Select a Database or a DSD');
        HideLoadingDiv();
        RemoveMaskingDiv();
    }
}

function SourcePopupOk() {
    var SelectedText = "";
    var tblSource = z('tblSource');
    z('hdnSelectedSources').value = '';

    for (i = 1; i < tblSource.rows.length; i++) {
        if (tblSource.rows[i].cells[0].childNodes[0].checked == true) {
            z('hdnSelectedSources').value += tblSource.rows[i].cells[0].childNodes[0].value + ',';
            SelectedText += tblSource.rows[i].cells[1].childNodes[0].innerText + ",";
        }
    }

    if (z('hdnSelectedSources').value != null && z('hdnSelectedSources').value != '') {
        z('hdnSelectedSources').value = z('hdnSelectedSources').value.substr(0, z('hdnSelectedSources').value.length - 1);
    }

    if (SelectedText != null && SelectedText != '') {
        SelectedText = SelectedText.substr(0, SelectedText.length - 1);
    }

    SetSelectedText(SelectedText, 'spanSelectedAreas');
    SourcePopupCancel();
}

function SourcePopupCancel() {
    HideLoadingDiv();
    RemoveMaskingDiv();

    di_jq('#SourcePopup').hide('slow');
}

function ClearSourceSelections() {
    z('hdnSelectedSources').value = '';
    z('hdnSelectedSources').innerHTML = '';
    z('hdnSelectedSources').title = '';
}

function GetSourceDivInnerHTML(DBNId) {
    var InputParam;

    InputParam = DBNId;

    if (InputParam != '') {
        try {
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: CallBackPageName,
                data: { 'callback': '123', 'param1': InputParam },
                async: false,
                success: function (data) {
                    try {
                        if (data != null && data != '') {
                            z('divSource').innerHTML = data;
                        }

                        for (i = 0; i < z('hdnSelectedSources').value.split(',').length; i++) {
                            if (z('chkSource_' + z('hdnSelectedSources').value.split(',')[i]) != null) {
                                z('chkSource_' + z('hdnSelectedSources').value.split(',')[i]).checked = true;
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
    else {
        alert('Select a Database or a DSD');
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

function SetSelectedText(text, spanId) {
    z(spanId).innerHTML = '';
    z(spanId).title = '';

    if (text.length > 100) {
        z(spanId).innerHTML = text.substr(0, 100) + "...";
        z(spanId).title = text;
    }
    else {
        z(spanId).innerHTML = text;
        z(spanId).title = text;
    }
}

