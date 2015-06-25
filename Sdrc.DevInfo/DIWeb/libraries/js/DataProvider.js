var CallbackPageName = "../Callback.aspx";

function onPageLoad() {
    SelectLeftMenuItem("DataProvider");
}

function OpenDatabasePopup() {
    ApplyMaskingDiv();
    ShowLoadingDiv();

    SetCloseButtonInPopupDiv(di_jq('#DatabasePopup'), 'DatabasePopupCancel', '../../../stock/themes/default/images/close.png');
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
    ClearSelections();
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
    ClearSelections();
}

function GetDatabaseDivInnerHTML() {
    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '111', 'param1': '' },
            async: false,
            success: function (data) {
                try {
                    if (data != null && data != '') {
                        z('divDatabase').innerHTML = data;
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

    SetCloseButtonInPopupDiv(di_jq('#DSDPopup'), 'DSDPopupCancel', '../../../stock/themes/default/images/close.png');
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
    ClearSelections();
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
    ClearSelections();
}

function GetDSDDivInnerHTML() {
    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '112', 'param1': '' },
            async: false,
            success: function (data) {
                try {
                    if (data != null && data != '') {
                        z('divDSD').innerHTML = data;
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

    if (DBOrDSDDBNId != '') {
        SetCloseButtonInPopupDiv(di_jq('#IndicatorPopup'), 'IndicatorPopupCancel', '../../../stock/themes/default/images/close.png');
        di_jq('#IndicatorPopup').show('slow');

        GetIndicatorDivInnerHTML(DBOrDSDDBNId);

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

    InputParam = DBOrDSDDBNId;

    if (InputParam != '') {
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
                        }

                        for (i = 0; i < z('hdnSelectedIndicators').value.split(',').length; i++) {
                            if (z('chkIndicator_' + z('hdnSelectedIndicators').value.split(',')[i]) != null) {
                                z('chkIndicator_' + z('hdnSelectedIndicators').value.split(',')[i]).defaultChecked = true;
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
        alert('Select a Database or a DSD');
    }
}

function SelectUnselectAllIndicators() {
    var tblIndicator = z('tblIndicator');

    for (i = 1; i < tblIndicator.rows.length; i++) {
        if (tblIndicator.rows[0].cells[0].childNodes[0].checked == true) {
            tblIndicator.rows[i].cells[0].childNodes[0].defaultChecked = true;
        }
        else {
            tblIndicator.rows[i].cells[0].childNodes[0].defaultChecked = false;
        }
    }
}

function OpenAreaPopup() {
    var DBOrDSDDBNId;
    ApplyMaskingDiv();
    ShowLoadingDiv();

    DBOrDSDDBNId = GetSelectedDBOrDSDDBNId();

    if (DBOrDSDDBNId != '') {
        SetCloseButtonInPopupDiv(di_jq('#AreaPopup'), 'AreaPopupCancel', '../../../stock/themes/default/images/close.png');
        di_jq('#AreaPopup').show('slow');

        GetAreaDivInnerHTML(DBOrDSDDBNId);

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

    InputParam = DBOrDSDDBNId;

    if (InputParam != '') {
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
                        }

                        for (i = 0; i < z('hdnSelectedAreas').value.split(',').length; i++) {
                            if (z('chkArea_' + z('hdnSelectedAreas').value.split(',')[i]) != null) {
                                z('chkArea_' + z('hdnSelectedAreas').value.split(',')[i]).defaultChecked = true;
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
        alert('Select a Database or a DSD');
    }
}

function SelectUnselectAllAreas() {
    var tblArea = z('tblArea');

    for (i = 1; i < tblArea.rows.length; i++) {
        if (tblArea.rows[0].cells[0].childNodes[0].checked == true) {
            tblArea.rows[i].cells[0].childNodes[0].defaultChecked = true;
        }
        else {
            tblArea.rows[i].cells[0].childNodes[0].defaultChecked = false;
        }
    }
}

function OpenTPPopup() {
    var DBOrDSDDBNId;
    ApplyMaskingDiv();
    ShowLoadingDiv();

    DBOrDSDDBNId = GetSelectedDBOrDSDDBNId();

    if (DBOrDSDDBNId != '') {
        SetCloseButtonInPopupDiv(di_jq('#TPPopup'), 'TPPopupCancel', '../../../stock/themes/default/images/close.png');
        di_jq('#TPPopup').show('slow');

        GetTPDivInnerHTML(DBOrDSDDBNId);

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

    InputParam = DBOrDSDDBNId;

    if (InputParam != '') {
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
                        }

                        for (i = 0; i < z('hdnSelectedTPs').value.split(',').length; i++) {
                            if (z('chkTP_' + z('hdnSelectedTPs').value.split(',')[i]) != null) {
                                z('chkTP_' + z('hdnSelectedTPs').value.split(',')[i]).defaultChecked = true;
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
        alert('Select a Database or a DSD');
    }
}

function SelectUnselectAllTPs() {
    var tblTP = z('tblTP');

    for (i = 1; i < tblTP.rows.length; i++) {
        if (tblTP.rows[0].cells[0].childNodes[0].checked == true) {
            tblTP.rows[i].cells[0].childNodes[0].defaultChecked = true;
        }
        else {
            tblTP.rows[i].cells[0].childNodes[0].defaultChecked = false;
        }
    }
}

function OpenSourcePopup() {
    var DBOrDSDDBNId;
    ApplyMaskingDiv();
    ShowLoadingDiv();

    DBOrDSDDBNId = GetSelectedDBOrDSDDBNId();

    if (DBOrDSDDBNId != '') {
        SetCloseButtonInPopupDiv(di_jq('#SourcePopup'), 'SourcePopupCancel', '../../../stock/themes/default/images/close.png');
        di_jq('#SourcePopup').show('slow');

        GetSourceDivInnerHTML(DBOrDSDDBNId);

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

    InputParam = DBOrDSDDBNId;

    if (InputParam != '') {
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
                        }

                        for (i = 0; i < z('hdnSelectedSources').value.split(',').length; i++) {
                            if (z('chkSource_' + z('hdnSelectedSources').value.split(',')[i]) != null) {
                                z('chkSource_' + z('hdnSelectedSources').value.split(',')[i]).defaultChecked = true;
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
        alert('Select a Database or a DSD');
    }
}

function SelectUnselectAllSources() {
    var tblSource = z('tblSource');

    for (i = 1; i < tblSource.rows.length; i++) {
        if (tblSource.rows[0].cells[0].childNodes[0].checked == true) {
            tblSource.rows[i].cells[0].childNodes[0].defaultChecked = true;
        }
        else {
            tblSource.rows[i].cells[0].childNodes[0].defaultChecked = false;
        }
    }
}

function GenerateSDMXML() {
    var InputParam;

    ApplyMaskingDiv();
    ShowLoadingDiv();

    if (z('hdnSelectedIndicators').value != null && z('hdnSelectedIndicators').value != '') {
        InputParam = GetSelectedDBOrDSDDBNId();
        InputParam += '|' + z('hdnSelectedIndicators').value;
        InputParam += '|' + z('hdnSelectedAreas').value;
        InputParam += '|' + z('hdnSelectedTPs').value;
        InputParam += '|' + z('hdnSelectedSources').value;

        if (InputParam != '') {
            try {
                var htmlResp = di_jq.ajax({
                    type: "POST",
                    url: CallbackPageName,
                    data: { 'callback': '113', 'param1': InputParam },
                    async: false,
                    success: function (data) {
                        try {
                            alert('SDMX-ML generated successfully.');

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
        else {
            alert('Select a Database or a DSD');

            HideLoadingDiv();
            RemoveMaskingDiv();
        }
    }
    else {
        alert('Select one indicator atleast!');

        HideLoadingDiv();
        RemoveMaskingDiv();
    }
}

function RegisterSDMXML() {
    var InputParam;

    ApplyMaskingDiv();
    ShowLoadingDiv();

    InputParam = GetSelectedDBOrDSDDBNId();

    if (InputParam != '') {
        try {
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: CallbackPageName,
                data: { 'callback': '114', 'param1': InputParam },
                async: false,
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
    else {
        alert('Select a Database or a DSD');

        HideLoadingDiv();
        RemoveMaskingDiv();
    }
}

function GenerateMetadata() {
    var InputParam;

    ApplyMaskingDiv();
    ShowLoadingDiv();

    InputParam = GetSelectedDBOrDSDDBNId();

    if (InputParam != '') {
        try {
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: CallbackPageName,
                data: { 'callback': '115', 'param1': InputParam },
                async: false,
                success: function (data) {
                    try {
                        alert('Metadata generated successfully.');

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
    else {
        alert('Select a Database or a DSD');

        HideLoadingDiv();
        RemoveMaskingDiv();
    }
}

function RegisterMetadata() {
    var InputParam;

    ApplyMaskingDiv();
    ShowLoadingDiv();

    InputParam = GetSelectedDBOrDSDDBNId();

    if (InputParam != '') {
        try {
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: CallbackPageName,
                data: { 'callback': '116', 'param1': InputParam },
                async: false,
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
    else {
        alert('Select a Database or a DSD');

        HideLoadingDiv();
        RemoveMaskingDiv();
    }
}

function SelectActionChange() {
    var selectAction = z('selectAction');
    var spanGenerateSDMXMLDescription = z('spanGenerateSDMXMLDescription');
    var spanRegisterSDMXMLDescription = z('spanRegisterSDMXMLDescription');
    var spanGenerateMetadataDescription = z('spanGenerateMetadataDescription');
    var spanRegisterMetadataDescription = z('spanRegisterMetadataDescription');
    var tblGenerateSDMXML = z('tblGenerateSDMXML');
    var tblRegisterSDMXML = z('tblRegisterSDMXML');
    var tblGenerateMetadata = z('tblGenerateMetadata');
    var tblRegisterMetadata = z('tblRegisterMetadata');
    var rowIndicatorSelection = z('rowIndicatorSelection');
    var rowAreaSelection = z('rowAreaSelection');
    var rowTPSelection = z('rowTPSelection');
    var rowSourceSelection = z('rowSourceSelection');
    var SelectedDBOrDSDNId = GetSelectedDBOrDSDDBNId();

    if (selectAction.value == "0") {
        // None selected
        spanGenerateSDMXMLDescription.style.display = "none";
        spanRegisterSDMXMLDescription.style.display = "none";
        spanGenerateMetadataDescription.style.display = "none";
        spanRegisterMetadataDescription.style.display = "none";

        tblGenerateSDMXML.style.display = "none";
        tblRegisterSDMXML.style.display = "none";
        tblGenerateMetadata.style.display = "none";
        tblRegisterMetadata.style.display = "none";
    }
    else if (selectAction.value == "1") {
        // Generate SDMX-ML
        spanGenerateSDMXMLDescription.style.display = "inline";
        spanRegisterSDMXMLDescription.style.display = "none";
        spanGenerateMetadataDescription.style.display = "none";
        spanRegisterMetadataDescription.style.display = "none";

        tblGenerateSDMXML.style.display = "inline";
        tblRegisterSDMXML.style.display = "none";
        tblGenerateMetadata.style.display = "none";
        tblRegisterMetadata.style.display = "none";

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
    }
    else if (selectAction.value == "2") {
        // Register SDMX-ML
        spanGenerateSDMXMLDescription.style.display = "none";
        spanRegisterSDMXMLDescription.style.display = "inline";
        spanGenerateMetadataDescription.style.display = "none";
        spanRegisterMetadataDescription.style.display = "none";

        tblGenerateSDMXML.style.display = "none";
        tblRegisterSDMXML.style.display = "inline";
        tblGenerateMetadata.style.display = "none";
        tblRegisterMetadata.style.display = "none";
    }
    else if (selectAction.value == "3") {
        // Generate Metadata Report
        spanGenerateSDMXMLDescription.style.display = "none";
        spanRegisterSDMXMLDescription.style.display = "none";
        spanGenerateMetadataDescription.style.display = "inline";
        spanRegisterMetadataDescription.style.display = "none";

        tblGenerateSDMXML.style.display = "none";
        tblRegisterSDMXML.style.display = "none";
        tblGenerateMetadata.style.display = "inline";
        tblRegisterMetadata.style.display = "none";
    }
    else if (selectAction.value == "4") {
        // Register Metadata Report
        spanGenerateSDMXMLDescription.style.display = "none";
        spanRegisterSDMXMLDescription.style.display = "none";
        spanGenerateMetadataDescription.style.display = "none";
        spanRegisterMetadataDescription.style.display = "inline";

        tblGenerateSDMXML.style.display = "none";
        tblRegisterSDMXML.style.display = "none";
        tblGenerateMetadata.style.display = "none";
        tblRegisterMetadata.style.display = "inline";
    }
}

function GetSelectedDBOrDSDDBNId() {
    var RetVal;

    RetVal = '';

    if (z('hdnSelectedDatabase').value != null && z('hdnSelectedDatabase').value != '') {
        RetVal = z('hdnSelectedDatabase').value + '|' + true;
    }
    else if (z('hdnSelectedDSD').value != null && z('hdnSelectedDSD').value != '') {
        RetVal = z('hdnSelectedDSD').value + '|' + false;
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

function ClearSelections() {
    ClearIndicatorSelections();
    ClearAreaSelections();
    ClearTPSelections();
    ClearSourceSelections();

    z('selectAction').value = "0";
    SelectActionChange();
}