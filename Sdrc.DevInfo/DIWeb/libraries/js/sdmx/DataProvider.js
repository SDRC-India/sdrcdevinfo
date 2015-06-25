var CallbackPageName = "Callback.aspx";
var selectedDBorDSD = '';
var SelectedDefaultDBorDSDText = '';
function onPageLoad(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hLoggedInUserNId, hLoggedInUserName, hOriginaldbnid) {
    var hsgcount = 10;
  
    // ************************************************
    // create Form Tag with hidden input boxes
    // ************************************************
    createFormHiddenInputs("frm_sdmxDataProvider", "POST");
    SetCommonLinksHref("frm_sdmxDataProvider", "POST");
    SetOriginaldbnidInForm("frm_sdmxDataProvider", hOriginaldbnid);

    // ************************************************1
    // set page level variables based on selections or defaults
    // ************************************************1
    setPostedData(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hsgcount, hLoggedInUserNId, hLoggedInUserName);

    // ************************************************1
    // Load Language Component
    // ************************************************1
    ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', hlngcode);

    SetCommonRegistryPageDetails('RegDataProvider.aspx', "frm_sdmxDataProvider", "POST");
    selectedDBorDSD = di_jq("option:selected", '#cphMainContent_selectDSDInUse').attr("class")
    SelectedDefaultDBorDSDText = di_jq("option:selected", '#cphMainContent_selectDSDInUse').text().split('-');
   
   if (selectedDBorDSD == "Database") {
        document.getElementById('segmentDatabase').style.display = '';
        document.getElementById('segmentOR').style.display = 'none';
        document.getElementById('segmentDSD').style.display = 'none';
        if (SelectedDefaultDBorDSDText != null) {
            SetSelectedText(SelectedDefaultDBorDSDText[1], 'spanSelectedDatabase');
            z('hdnSelectedDatabase').value = di_jq("option:selected", '#cphMainContent_selectDSDInUse').val();
        }
    }
    else {
        document.getElementById('segmentDatabase').style.display = 'none';
        document.getElementById('segmentOR').style.display = 'none';
        document.getElementById('segmentDSD').style.display = '';
        if (SelectedDefaultDBorDSDText != null) {
            SetSelectedText(SelectedDefaultDBorDSDText[0], 'spanSelectedDSD');
            z('hdnSelectedDSD').value = di_jq("option:selected", '#cphMainContent_selectDSDInUse').val();
        }
    }
}

function OpenDatabasePopup() {
    ApplyMaskingDiv();
    ShowLoadingDiv();

    SetCloseButtonInPopupDiv(di_jq('#DatabasePopup'), 'DatabasePopupCancel', '../../stock/themes/default/images/close.png');
    di_jq('#DatabasePopup').show('slow');

    GetDatabaseDivInnerHTML();
    GetWindowCentered(z("DatabasePopup"), 710, 520);
}

function DatabasePopupOk() {
    var SelectedText = "";
    var tblDatabase = z('tblDatabase');
    z('hdnSelectedDatabase').value = '';

    for (i = 1; i < tblDatabase.rows.length; i++) {
        if (tblDatabase.rows[i].cells[0].childNodes.length > 0 && tblDatabase.rows[i].cells[0].childNodes[0].checked == true) {
            z('hdnSelectedDatabase').value = tblDatabase.rows[i].cells[0].childNodes[0].value;
            SelectedText = tblDatabase.rows[i].cells[1].childNodes[0].innerHTML;

            EnableDisableDSDLinks(false);
            break;
        }
    }

    SetSelectedText(SelectedText, 'spanSelectedDatabase');
    DatabasePopupCancel();
    SelectDefaultAction();
}

function DatabasePopupCancel() {
    HideLoadingDiv();
    RemoveMaskingDiv();

    di_jq('#DatabasePopup').hide('slow');
}

function ClearDatabaseSelections() {
    z('hdnSelectedDatabase').value = '';
    z('spanSelectedDatabase').innerHTML = '';
    z('spanSelectedDatabase').title = '';

    EnableDisableDSDLinks(true);
    SelectDefaultAction();
}

function GetDatabaseDivInnerHTML() {
    var InputParam = z('hLoggedInUserNId').value.split('|')[0];

    z('divDatabase').innerHTML = '';

    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '111', 'param1': InputParam },
            async: false,
            success: function (data) {
                try {
                    if (data != null && data != '') {
                        z('divDatabase').innerHTML = data;
                        z('lang_Publisher').innerHTML = z('hPublisher').value;
                        z('lang_Database').innerHTML = z('hDatabase').value;
                    }
                    if (z('radio_' + z('hdnSelectedDatabase').value) != null) {
                        z('radio_' + z('hdnSelectedDatabase').value).checked = true;
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

function EnableDisableDatabaseLinks(enableFlag) {
    if (enableFlag == false) {
        EnableDisableLinks(z('aDatabaseSelect'), true);
        EnableDisableLinks(z('aDatabaseClear'), true);
    }
    else {
        EnableDisableLinks(z('aDatabaseSelect'), false);
        EnableDisableLinks(z('aDatabaseClear'), false);
    }
}

function OpenDSDPopup() {
    ApplyMaskingDiv();
    ShowLoadingDiv();

    SetCloseButtonInPopupDiv(di_jq('#DSDPopup'), 'DSDPopupCancel', '../../stock/themes/default/images/close.png');
    di_jq('#DSDPopup').show('slow');

    GetDSDDivInnerHTML();
    GetWindowCentered(z("DSDPopup"), 710, 520);
}

function DSDPopupOk() {
    var SelectedText = "";
    var tblDSD = z('tblDSD');
    z('hdnSelectedDSD').value = '';

    for (i = 1; i < tblDSD.rows.length; i++) {
        if (tblDSD.rows[i].cells[0].childNodes.length > 0 && tblDSD.rows[i].cells[0].childNodes[0].checked == true) {
            z('hdnSelectedDSD').value = tblDSD.rows[i].cells[0].childNodes[0].value;
            SelectedText = tblDSD.rows[i].cells[1].childNodes[0].innerHTML + " (" + tblDSD.rows[i].cells[3].childNodes[0].innerHTML + ", " +
                           tblDSD.rows[i].cells[2].childNodes[0].innerHTML + ")";

            EnableDisableDatabaseLinks(false);
            break;
        }
    }

    SetSelectedText(SelectedText, 'spanSelectedDSD');
    DSDPopupCancel();
    SelectDefaultAction();
}

function DSDPopupCancel() {
    HideLoadingDiv();
    RemoveMaskingDiv();

    di_jq('#DSDPopup').hide('slow');
}

function ClearDSDSelections() {
    z('hdnSelectedDSD').value = '';
    z('spanSelectedDSD').innerHTML = '';
    z('spanSelectedDSD').title = '';

    EnableDisableDatabaseLinks(true);
    SelectDefaultAction();
}

function GetDSDDivInnerHTML() {
    var InputParam = z('hLoggedInUserNId').value.split('|')[0];

    z('divDSD').innerHTML = '';

    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '112', 'param1': InputParam },
            async: false,
            success: function (data) {
                try {
                    if (data != null && data != '') {
                        z('divDSD').innerHTML = data;
                        z('lang_Id').innerHTML = z('hId').value;
                        z('lang_AgencyId').innerHTML = z('hAgencyId').value;
                        z('lang_Version').innerHTML = z('hVersion').value;
                        z('lang_Assosciated_Database').innerHTML = z('hAssosciatedDatabase').value;
                        z('lang_Publisher_DSD').innerHTML = z('hPublisher').value;
                    }
                    if (z('radio_' + z('hdnSelectedDSD').value) != null) {
                        z('radio_' + z('hdnSelectedDSD').value).checked = true;
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

function EnableDisableDSDLinks(enableFlag) {
    if (enableFlag == false) {
        EnableDisableLinks(z('aDSDSelect'), true);
        EnableDisableLinks(z('aDSDClear'), true);
    }
    else {
        EnableDisableLinks(z('aDSDSelect'), false);
        EnableDisableLinks(z('aDSDClear'), false);
    }
}

function OpenIndicatorPopup() {
    var DBOrDSDDBNId;
    ApplyMaskingDiv();
    ShowLoadingDiv();

    DBOrDSDDBNId = GetSelectedDBOrDSDDBNId();

    SetCloseButtonInPopupDiv(di_jq('#IndicatorPopup'), 'IndicatorPopupCancel', '../../stock/themes/default/images/close.png');
    di_jq('#IndicatorPopup').show('slow');

    GetIndicatorDivInnerHTML(DBOrDSDDBNId);
    GetWindowCentered(z("IndicatorPopup"), 710, 520);
}

function IndicatorPopupOk() {
    var SelectedText = "";
    var tblIndicator = z('tblIndicator');
    z('hdnSelectedIndicators').value = '';

    for (i = 1; i < tblIndicator.rows.length; i++) {
        if (tblIndicator.rows[i].cells[0].childNodes[0].checked == true) {
            z('hdnSelectedIndicators').value += tblIndicator.rows[i].cells[0].childNodes[0].value + ',';
            SelectedText += tblIndicator.rows[i].cells[1].childNodes[0].innerHTML + ",";
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

function GetIndicatorDivInnerHTML(DBOrDSDDBNId) {
    var InputParam;

    z('divIndicator').innerHTML = '';

    InputParam = DBOrDSDDBNId;
    InputParam += '|' + z('selectAction').value;

    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '110', 'param1': InputParam },
            async: false,
            success: function (data) {
                try {
                    if (data != null && data != '') {
                        z('divIndicator').innerHTML = data;
                        z('spanIndicator_0').innerHTML = z('hSelectAll').value;
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
                
            },
            cache: false
        });
    }
    catch (ex) {
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
    var DBOrDSDDBNId;
    ApplyMaskingDiv();
    ShowLoadingDiv();

    DBOrDSDDBNId = GetSelectedDBOrDSDDBNId();

    SetCloseButtonInPopupDiv(di_jq('#AreaPopup'), 'AreaPopupCancel', '../../stock/themes/default/images/close.png');
    di_jq('#AreaPopup').show('slow');

    GetAreaDivInnerHTML(DBOrDSDDBNId);
    GetWindowCentered(z("AreaPopup"), 710, 520);
}

function AreaPopupOk() {
    var SelectedText = "";
    var tblArea = z('tblArea');
    z('hdnSelectedAreas').value = '';

    for (i = 1; i < tblArea.rows.length; i++) {
        if (tblArea.rows[i].cells[0].childNodes[0].checked == true) {
            z('hdnSelectedAreas').value += tblArea.rows[i].cells[0].childNodes[0].value + ',';
            SelectedText += tblArea.rows[i].cells[1].childNodes[0].innerHTML + ",";
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

function GetAreaDivInnerHTML(DBOrDSDDBNId) {
    var InputParam;

    z('divArea').innerHTML = '';

    InputParam = DBOrDSDDBNId;
    InputParam += '|' + z('selectAction').value;

    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '109', 'param1': InputParam },
            async: false,
            success: function (data) {
                try {
                    if (data != null && data != '') {
                        z('divArea').innerHTML = data;
                        z('spanArea_0').innerHTML = z('hSelectAll').value;
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
                
            },
            cache: false
        });
    }
    catch (ex) {
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

function OpenTPPopup() {
    var DBOrDSDDBNId;
    ApplyMaskingDiv();
    ShowLoadingDiv();

    DBOrDSDDBNId = GetSelectedDBOrDSDDBNId();

    SetCloseButtonInPopupDiv(di_jq('#TPPopup'), 'TPPopupCancel', '../../stock/themes/default/images/close.png');
    di_jq('#TPPopup').show('slow');

    GetTPDivInnerHTML(DBOrDSDDBNId);
    GetWindowCentered(z("TPPopup"), 710, 520);
}

function TPPopupOk() {
    var SelectedText = "";
    var tblTP = z('tblTP');
    z('hdnSelectedTPs').value = '';

    for (i = 1; i < tblTP.rows.length; i++) {
        if (tblTP.rows[i].cells[0].childNodes[0].checked == true) {
            z('hdnSelectedTPs').value += tblTP.rows[i].cells[0].childNodes[0].value + ',';
            SelectedText += tblTP.rows[i].cells[1].childNodes[0].innerHTML + ",";
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

function GetTPDivInnerHTML(DBOrDSDDBNId) {
    var InputParam;

    z('divTP').innerHTML = '';

    InputParam = DBOrDSDDBNId;

    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '108', 'param1': InputParam },
            async: false,
            success: function (data) {
                try {
                    if (data != null && data != '') {
                        z('divTP').innerHTML = data;
                        z('spanTP_0').innerHTML = z('hSelectAll').value;
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

function OpenSourcePopup() {
    var DBOrDSDDBNId;
    ApplyMaskingDiv();
    ShowLoadingDiv();

    DBOrDSDDBNId = GetSelectedDBOrDSDDBNId();

    SetCloseButtonInPopupDiv(di_jq('#SourcePopup'), 'SourcePopupCancel', '../../stock/themes/default/images/close.png');
    di_jq('#SourcePopup').show('slow');

    GetSourceDivInnerHTML(DBOrDSDDBNId);
    GetWindowCentered(z("SourcePopup"), 710, 520);
}

function SourcePopupOk() {
    var SelectedText = "";
    var tblSource = z('tblSource');
    z('hdnSelectedSources').value = '';

    for (i = 1; i < tblSource.rows.length; i++) {
        if (tblSource.rows[i].cells[0].childNodes[0].checked == true) {
            z('hdnSelectedSources').value += tblSource.rows[i].cells[0].childNodes[0].value + ',';
            SelectedText += tblSource.rows[i].cells[1].childNodes[0].innerHTML + ",";
        }
    }

    if (z('hdnSelectedSources').value != null && z('hdnSelectedSources').value != '') {
        z('hdnSelectedSources').value = z('hdnSelectedSources').value.substr(0, z('hdnSelectedSources').value.length - 1);
    }

    if (SelectedText != null && SelectedText != '') {
        SelectedText = SelectedText.substr(0, SelectedText.length - 1);
    }

    SetSelectedText(SelectedText, 'spanSelectedSources');
    SourcePopupCancel();
}

function SourcePopupCancel() {
    HideLoadingDiv();
    RemoveMaskingDiv();

    di_jq('#SourcePopup').hide('slow');
}

function ClearSourceSelections() {
    z('hdnSelectedSources').value = '';
    z('spanSelectedSources').innerHTML = '';
    z('spanSelectedSources').title = '';
}

function GetSourceDivInnerHTML(DBOrDSDDBNId) {
    var InputParam;

    z('divSource').innerHTML = '';

    InputParam = DBOrDSDDBNId;
    InputParam += '|' + z('selectAction').value;

    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '125', 'param1': InputParam },
            async: false,
            success: function (data) {
                try {
                    if (data != null && data != '') {
                        z('divSource').innerHTML = data;
                        z('spanSource_0').innerHTML = z('hSelectAll').value;
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
                
            },
            cache: false
        });
    }
    catch (ex) {
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

function GenerateSDMXML() {
    var InputParam;
    var ArrMsgNFileCount;
    var Status;
    var FileCount;
    var ValidMsg;
    var DSDORDB;
    ApplyMaskingDiv();
    ShowLoadingDiv();

    if (z('hdnSelectedIndicators').value != null && z('hdnSelectedIndicators').value != '') {
        InputParam = GetSelectedDBOrDSDDBNId();
        InputParam += '|' + z('hdnSelectedIndicators').value;
        InputParam += '|' + z('hdnSelectedAreas').value;
        InputParam += '|' + z('hdnSelectedTPs').value;
        InputParam += '|' + z('hdnSelectedSources').value;
        InputParam += '|' + z('hdnDBAreaId').value;
        InputParam += '|' + z('hLoggedInUserNId').value.split('|')[0];

        try {
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: CallbackPageName,
                data: { 'callback': '113', 'param1': InputParam },
                async: true,
                success: function (data) {
                    try {
                        if (data.length > 0) {
                            DSDORDB = z('hdnDSDORDB').value;
                            ArrMsgNFileCount = data.split('[****]');
                            Status = ArrMsgNFileCount[0];
                            FileCount = ArrMsgNFileCount[1];
                            if (isNaN(FileCount)) {
                                ValidMsg = ArrMsgNFileCount[1];
                            }
                            else {
                                ValidMsg = ArrMsgNFileCount[2];
                            }
                            if (FileCount != 0 && Status == "true") {
                                alert(FileCount + " " + "SDMX-ML Message(s) generated successfully.");
                            }
                            else {
                                if (ValidMsg == "MNF") {
                                    alert(z('hdnMappingNotFound').value)
                                }
                                else if (ValidMsg == "NDF") {
                                    alert(z('hdnNoDataFound').value)
                                }
                                else {
                                    alert(ValidMsg);
                                }
                            }

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

        }
    }
    else {
        alert('Select one indicator atleast!');

        HideLoadingDiv();
        RemoveMaskingDiv();
    }
}

//function RegisterSDMXML() {
//    var InputParam;

//    ApplyMaskingDiv();
//    ShowLoadingDiv();

//   
//        InputParam = GetSelectedDBOrDSDDBNId();
//        InputParam += '|' + z('hdnSelectedIndicators').value;
//        InputParam += '|' + z('hLoggedInUserNId').value.split('|')[0];

//        try {
//            var htmlResp = di_jq.ajax({
//                type: "POST",
//                url: CallbackPageName,
//                data: { 'callback': '114', 'param1': InputParam },
//                async: true,
//                success: function (data) {
//                    try {
//                        alert('SDMX-ML registered successfully.');

//                        HideLoadingDiv();
//                        RemoveMaskingDiv();
//                    }
//                    catch (ex) {
//                        alert("message:" + ex.message);

//                        HideLoadingDiv();
//                        RemoveMaskingDiv();
//                    }
//                },
//                error: function () {
//                    

//                    HideLoadingDiv();
//                    RemoveMaskingDiv();
//                },
//                cache: false
//            });
//        }
//        catch (ex) {
//        }
//   
//   
//}


function RegisterSDMXML() {
    var InputParam;

    ApplyMaskingDiv();
    ShowLoadingDiv();

    InputParam = GetSelectedDBOrDSDDBNId();
    InputParam += '|' + z('hLoggedInUserNId').value.split('|')[0];

    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '114', 'param1': InputParam },
            async: true,
            success: function (data) {
                try {
                    alert('SDMX-ML registered successfully.');

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
    }
}


function GenerateMetadata() {
    var InputParam;

    ApplyMaskingDiv();
    ShowLoadingDiv();

    if (GetSelectedDBOrDSDDBNId().split('|')[1] == 'true') {
        if ((z('hdnSelectedIndicators').value != null && z('hdnSelectedIndicators').value != '') ||
        (z('hdnSelectedAreas').value != null && z('hdnSelectedAreas').value != '') ||
        (z('hdnSelectedSources').value != null && z('hdnSelectedSources').value != '')) {
            GenerateMetadataCallback(GetSelectedDBOrDSDDBNId(), z('hdnSelectedIndicators').value, z('hdnSelectedAreas').value, z('hdnSelectedSources').value, z('hLoggedInUserNId').value.split('|')[0]);
        }
        else {
            alert("Select one Indicator, Area or Source atleast!");

            HideLoadingDiv();
            RemoveMaskingDiv();
        }
    }
    else {
        if (z('hdnSelectedIndicators').value != null && z('hdnSelectedIndicators').value != '') {
            GenerateMetadataCallback(GetSelectedDBOrDSDDBNId(), z('hdnSelectedIndicators').value, '', '', z('hLoggedInUserNId').value.split('|')[0]);
        }
        else {
            alert("Select one Indicator atleast!");

            HideLoadingDiv();
            RemoveMaskingDiv();
        }
    }
}

//function RegisterMetadata() {
//    var InputParam;

//    ApplyMaskingDiv();
//    ShowLoadingDiv();

//    if (GetSelectedDBOrDSDDBNId().split('|')[1] == 'true') {
//        if ((z('hdnSelectedIndicators').value != null && z('hdnSelectedIndicators').value != '') ||
//        (z('hdnSelectedAreas').value != null && z('hdnSelectedAreas').value != '') ||
//        (z('hdnSelectedSources').value != null && z('hdnSelectedSources').value != '')) {
//            RegisterMetadataCallback(GetSelectedDBOrDSDDBNId(), z('hdnSelectedIndicators').value, z('hdnSelectedAreas').value, z('hdnSelectedSources').value, z('hLoggedInUserNId').value.split('|')[0]);
//        }
//        else {
//            alert("Select one Indicator, Area or Source atleast!");

//            HideLoadingDiv();
//            RemoveMaskingDiv();
//        }
//    }
//    else {
//        if (z('hdnSelectedIndicators').value != null && z('hdnSelectedIndicators').value != '') {
//            RegisterMetadataCallback(GetSelectedDBOrDSDDBNId(), z('hdnSelectedIndicators').value, '', '', z('hLoggedInUserNId').value.split('|')[0]);
//        }
//        else {
//            alert("Select one Indicator atleast!");

//            HideLoadingDiv();
//            RemoveMaskingDiv();
//        }
//    }
//}

//function GenerateMetadata() {
//    var InputParam;

//    ApplyMaskingDiv();
//    ShowLoadingDiv();

//    InputParam = GetSelectedDBOrDSDDBNId();
//    InputParam += '|' + z('hLoggedInUserNId').value.split('|')[0];

//    try {
//        var htmlResp = di_jq.ajax({
//            type: "POST",
//            url: CallbackPageName,
//            data: { 'callback': '115', 'param1': InputParam },
//            async: true,
//            success: function (data) {
//                try {
//                    alert('Metadata generated successfully.');

//                    HideLoadingDiv();
//                    RemoveMaskingDiv();
//                }
//                catch (ex) {
//                    alert("message:" + ex.message);

//                    HideLoadingDiv();
//                    RemoveMaskingDiv();
//                }
//            },
//            error: function () {
//                

//                HideLoadingDiv();
//                RemoveMaskingDiv();
//            },
//            cache: false
//        });
//    }
//    catch (ex) {
//    }
//}

function RegisterMetadata() {
    var InputParam;

    ApplyMaskingDiv();
    ShowLoadingDiv();

    InputParam = GetSelectedDBOrDSDDBNId();
    InputParam += '|' + z('hLoggedInUserNId').value.split('|')[0];

    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '116', 'param1': InputParam },
            async: true,
            success: function (data) {
                try {
                    alert('Metadata registered successfully.');

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
    }
}


function GenerateMetadataCallback(DBOrDSDDBNId, IndicatorNIds, AreaNIds, SourceNIds, UserNId) {
    var InputParam;
    var ArrData;
    var Status;
    var ErrMsg;
    InputParam = DBOrDSDDBNId;
    InputParam += '|' + IndicatorNIds;
    InputParam += '|' + AreaNIds;
    InputParam += '|' + SourceNIds;
    InputParam += '|' + UserNId;

    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '115', 'param1': InputParam },
            async: true,
            success: function (data) {
                try {
                    ArrData = data.split('[****]');
                    Status = ArrData[0];
                    ErrMsg = ArrData[1];
                    if (ErrMsg == "MNF" && Status == "false") {
                        alert(z('hdnMappingNotFound').value);
                    }
                    else if (Status == "true") {
                        alert('Metadata generated successfully.');
                    }
                    else {
                        alert('Metadata generated successfully.');
                    }
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
    }
}

function RegisterMetadataCallback(DBOrDSDDBNId, IndicatorNIds, AreaNIds, SourceNIds, UserNId) {
    var InputParam;

    InputParam = DBOrDSDDBNId;
    InputParam += '|' + IndicatorNIds;
    InputParam += '|' + AreaNIds;
    InputParam += '|' + SourceNIds;
    InputParam += '|' + UserNId;

    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '116', 'param1': InputParam },
            async: true,
            success: function (data) {
                try {
                    alert('Metadata registered successfully.');

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
    }
}

function SelectActionChange() {
    var selectAction = z('selectAction');
    var spanGenerateSDMXMLDescription = z('spanGenerateSDMXMLDescription');
    var spanRegisterSDMXMLDescription = z('spanRegisterSDMXMLDescription');
    var spanGenerateMetadataDescription = z('spanGenerateMetadataDescription');
    var spanRegisterMetadataDescription = z('spanRegisterMetadataDescription');
    var rowIndicatorSelection = z('rowIndicatorSelection');
    var rowAreaSelection = z('rowAreaSelection');
    var rowTPSelection = z('rowTPSelection');
    var rowSourceSelection = z('rowSourceSelection');
    var rowGenerateSDMXML = z('rowGenerateSDMXML');
    var rowRegisterSDMXML = z('rowRegisterSDMXML');
    var rowGenerateMetadata = z('rowGenerateMetadata');
    var rowRegisterMetadata = z('rowRegisterMetadata');
    var tblAction = z('tblAction');
    var SelectedDBOrDSDNId = GetSelectedDBOrDSDDBNId();

    ClearSelections();

    if (selectAction.value == "0") {
        // None selected
        spanGenerateSDMXMLDescription.style.display = "none";
        spanRegisterSDMXMLDescription.style.display = "none";
        spanGenerateMetadataDescription.style.display = "none";
        spanRegisterMetadataDescription.style.display = "none";

        rowGenerateSDMXML.style.display = "none";
        rowRegisterSDMXML.style.display = "none";
        rowGenerateMetadata.style.display = "none";
        rowRegisterMetadata.style.display = "none";

        rowIndicatorSelection.style.display = "none";
        rowAreaSelection.style.display = "none";
        rowTPSelection.style.display = "none";
        rowSourceSelection.style.display = "none";

        tblAction.style.display = "none";
    }
    else {
        if (GetSelectedDBOrDSDDBNId() != '') {
            if (selectAction.value == "1") {
                // Generate SDMX-ML
                spanGenerateSDMXMLDescription.style.display = "inline";
                spanRegisterSDMXMLDescription.style.display = "none";
                spanGenerateMetadataDescription.style.display = "none";
                spanRegisterMetadataDescription.style.display = "none";

                rowGenerateSDMXML.style.display = "inline";
                rowRegisterSDMXML.style.display = "none";
                rowGenerateMetadata.style.display = "none";
                rowRegisterMetadata.style.display = "none";

                if (SelectedDBOrDSDNId.split('|')[1] == 'true') {
                    rowIndicatorSelection.style.display = "block";
                    rowAreaSelection.style.display = "block";
                    rowTPSelection.style.display = "block";
                    rowSourceSelection.style.display = "block";
                }
                else {
                    rowIndicatorSelection.style.display = "block";
                    rowAreaSelection.style.display = "block";
                    rowTPSelection.style.display = "block";
                    rowSourceSelection.style.display = "block";
                }

                tblAction.style.display = "block";
            }
            else if (selectAction.value == "2") {
                // Register SDMX-ML
                spanGenerateSDMXMLDescription.style.display = "none";
                spanRegisterSDMXMLDescription.style.display = "inline";
                spanGenerateMetadataDescription.style.display = "none";
                spanRegisterMetadataDescription.style.display = "none";

                rowGenerateSDMXML.style.display = "none";
                rowRegisterSDMXML.style.display = "inline";
                rowGenerateMetadata.style.display = "none";
                rowRegisterMetadata.style.display = "none";

                if (SelectedDBOrDSDNId.split('|')[1] == 'true') {
                    rowIndicatorSelection.style.display = "none";
                    rowAreaSelection.style.display = "none";
                    rowTPSelection.style.display = "none";
                    rowSourceSelection.style.display = "none";
                }
                else {
                    rowIndicatorSelection.style.display = "none";
                    rowAreaSelection.style.display = "none";
                    rowTPSelection.style.display = "none";
                    rowSourceSelection.style.display = "none";
                }

                tblAction.style.display = "block";
            }
            else if (selectAction.value == "3") {
                // Generate Metadata Report
                spanGenerateSDMXMLDescription.style.display = "none";
                spanRegisterSDMXMLDescription.style.display = "none";
                spanGenerateMetadataDescription.style.display = "inline";
                spanRegisterMetadataDescription.style.display = "none";

                rowGenerateSDMXML.style.display = "none";
                rowRegisterSDMXML.style.display = "none";
                rowGenerateMetadata.style.display = "inline";
                rowRegisterMetadata.style.display = "none";

                if (SelectedDBOrDSDNId.split('|')[1] == 'true') {
                    rowIndicatorSelection.style.display = "block";
                    rowAreaSelection.style.display = "block";
                    rowTPSelection.style.display = "none";
                    rowSourceSelection.style.display = "block";
                }
                else {
                    rowIndicatorSelection.style.display = "block";
                    rowAreaSelection.style.display = "none";
                    rowTPSelection.style.display = "none";
                    rowSourceSelection.style.display = "none";
                }

                tblAction.style.display = "block";
            }
            else if (selectAction.value == "4") {
                // Register Metadata Report
                spanGenerateSDMXMLDescription.style.display = "none";
                spanRegisterSDMXMLDescription.style.display = "none";
                spanGenerateMetadataDescription.style.display = "none";
                spanRegisterMetadataDescription.style.display = "inline";

                rowGenerateSDMXML.style.display = "none";
                rowRegisterSDMXML.style.display = "none";
                rowGenerateMetadata.style.display = "none";
                rowRegisterMetadata.style.display = "inline";

                if (SelectedDBOrDSDNId.split('|')[1] == 'true') {
                    rowIndicatorSelection.style.display = "none";
                    rowAreaSelection.style.display = "none";
                    rowTPSelection.style.display = "none";
                    rowSourceSelection.style.display = "none";
                }
                else {
                    rowIndicatorSelection.style.display = "none";
                    rowAreaSelection.style.display = "none";
                    rowTPSelection.style.display = "none";
                    rowSourceSelection.style.display = "none";
                }

                tblAction.style.display = "block";
            }
        }
        else {
            ApplyMaskingDiv();
            ShowLoadingDiv();
            if (selectedDBorDSD == 'Database') {
                alert('Select a Database');
            }
            else {
                alert('Select a DSD');
            }
            selectAction.value = "0";

            HideLoadingDiv();
            RemoveMaskingDiv();
        }
    }
}

function GetSelectedDBOrDSDDBNId() {
    var RetVal;

    RetVal = '';
    z('hdnDSDORDB').value = '';
    if (z('hdnSelectedDatabase').value != null && z('hdnSelectedDatabase').value != '') {
        RetVal = z('hdnSelectedDatabase').value + '|' + true;
        z('hdnDSDORDB').value = 'DB';
    }
    else if (z('hdnSelectedDSD').value != null && z('hdnSelectedDSD').value != '') {
        RetVal = z('hdnSelectedDSD').value + '|' + false;
        z('hdnDSDORDB').value = 'DSB';
    }

    return RetVal;
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

function SelectDefaultAction() {
    z('selectAction').value = "0";
    SelectActionChange();
}

function ClearSelections() {
    ClearIndicatorSelections();
    ClearAreaSelections();
    ClearTPSelections();
    ClearSourceSelections();
}

