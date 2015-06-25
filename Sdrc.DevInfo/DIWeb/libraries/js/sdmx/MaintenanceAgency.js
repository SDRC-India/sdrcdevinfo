// JScript File

var CallbackPageName = "Callback.aspx";
var CommaSeparator = ",";

function onPageLoad(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hLoggedInUserNId, hLoggedInUserName, hOriginaldbnid) {
    //,hdsdnid
    var hsgcount = 10;
    // ************************************************
    // create Form Tag with hidden input boxes
    // ************************************************
    createFormHiddenInputs("frm_sdmxMaintenanceAgency", "POST");
    SetCommonLinksHref("frm_sdmxMaintenanceAgency", "POST");
    SetOriginaldbnidInForm("frm_sdmxMaintenanceAgency", hOriginaldbnid);

    // ************************************************1
    // set page level variables based on selections or defaults
    // ************************************************1
    setPostedData(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hsgcount, hLoggedInUserNId, hLoggedInUserName);

    // ************************************************1
    // Load Language Component
    // ************************************************1
    ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', hlngcode);

    SetCommonRegistryPageDetails('RegMaintenanceAgency.aspx', "frm_sdmxMaintenanceAgency", "POST");

}

function SetLanguageFromHiddenFieldValuesForDatabase() {

    if (document.getElementById('lang_Connection_Name')) 
    {
        document.getElementById('lang_Connection_Name').innerHTML = document.getElementById('hConnectionName').value;
    }
    if (document.getElementById('lang_Database_Type')) {
        document.getElementById('lang_Database_Type').innerHTML = document.getElementById('hDatabaseType').value;    
    }
    if (document.getElementById('lang_Created_On')) 
    {
        document.getElementById('lang_Created_On').innerHTML = document.getElementById('hCreatedOn').value;
    }
    if (document.getElementById('lang_Publisher')) 
    {
        document.getElementById('lang_Publisher').innerHTML = document.getElementById('hPublisher').value;
    }
    
}

function SetLanguageFromHiddenFieldValuesForUploadedDSD() {

    if (document.getElementById('lang_SNo')) 
    {
        document.getElementById('lang_SNo').innerHTML = document.getElementById('hSNo').value;
    }
    if (document.getElementById('lang_Id')) 
    {
        document.getElementById('lang_Id').innerHTML = document.getElementById('hId').value;
    }
    if (document.getElementById('lang_AgencyId')) 
    {
        document.getElementById('lang_AgencyId').innerHTML = document.getElementById('hAgencyId').value;
    }
    if (document.getElementById('lang_Version')) 
    {
        document.getElementById('lang_Version').innerHTML = document.getElementById('hVersion').value; 
    }
    if (document.getElementById('lang_Assosciated_Database')) 
    {
        document.getElementById('lang_Assosciated_Database').innerHTML = document.getElementById('hAssosciatedDatabase').value;
    }
    if (document.getElementById('lang_Publisher_DSD')) 
    {
        document.getElementById('lang_Publisher_DSD').innerHTML = document.getElementById('hPublisher').value;    
    }
    if (document.getElementById('lang_Details')) 
    {
        document.getElementById('lang_Details').innerHTML = document.getElementById('hDetails').value;
    }
    if (document.getElementById('lang_Action')) 
    {
        document.getElementById('lang_Action').innerHTML = document.getElementById('hAction').value;
    }
    for (var i = 0; i < document.getElementsByName('lang_View').length; i++) 
    {
        document.getElementsByName('lang_View')[i].innerHTML = document.getElementById('hView').value;
    }

    for (var i = 0; i < document.getElementsByName('lang_Download').length; i++) {
        document.getElementsByName('lang_Download')[i].innerHTML = document.getElementById('hDownload').value;
    }

    for (var i = 0; i < document.getElementsByName('lang_Edit').length; i++) {
        document.getElementsByName('lang_Edit')[i].innerHTML = document.getElementById('hEdit').value;
    }

    for (var i = 0; i < document.getElementsByName('lang_Delete').length; i++) {
        document.getElementsByName('lang_Delete')[i].innerHTML = document.getElementById('hDelete').value;
    }
}

function SetLanguageFromHiddenFieldValuesForUploadedMSD() {

    if (document.getElementById('lang_SNo_MSD')) {
        document.getElementById('lang_SNo_MSD').innerHTML = document.getElementById('hSNo').value;
    }
    if (document.getElementById('lang_Id_MSD')) {
        document.getElementById('lang_Id_MSD').innerHTML = document.getElementById('hId').value;
    }
    if (document.getElementById('lang_Agency_Id_MSD')) {
        document.getElementById('lang_Agency_Id_MSD').innerHTML = document.getElementById('hAgencyId').value;
    }
    if (document.getElementById('lang_Version_MSD')) {
        document.getElementById('lang_Version_MSD').innerHTML = document.getElementById('hVersion').value;
    }
    if (document.getElementById('lang_Assosciated_DSD_MSD')) 
    {
        document.getElementById('lang_Assosciated_DSD_MSD').innerHTML = document.getElementById('hAssosciatedDSD').value;
    }
    if (document.getElementById('lang_Details_MSD')) {
        document.getElementById('lang_Details_MSD').innerHTML = document.getElementById('hDetails').value;
    }
    for (var i = 0; i < document.getElementsByName('lang_View').length; i++) {
        document.getElementsByName('lang_View')[i].innerHTML = document.getElementById('hView').value;
    }

    for (var i = 0; i < document.getElementsByName('lang_Download').length; i++) {
        document.getElementsByName('lang_Download')[i].innerHTML = document.getElementById('hDownload').value;
    }
}


function GenerateSDMXArtefacts() {
    try {

        var DbId = "";

        if (di_jq("#divListofDatabases :checked").length > 0) {
            di_jq("#divListofDatabases :checked").each(function () {
                DbId = di_jq(this).context.id.substr(3);
            });

            InputParam = DbId;
            InputParam += ParamDelimiter + document.getElementById('hLoggedInUserNId').value.split('|')[0];

            ApplyMaskNLoader();
            di_jq.ajax({
                type: "POST",
                url: CallbackPageName,
                data: { 'callback': '47', 'param1': InputParam },
                async: true,
                success: function (data) {
                    try {

                        if (data == "true") {
                            HideLoadingDiv();
                            alert("SDMX Artefacts Generated successfully for selected database.");
                            BindDatabasesInTheTable();
                            var NowURL = location.href;
                            document.location = NowURL;

                            RemoveMaskingDiv();
                        }
                        else {

                            alert(data);
                            RemoveLoaderNMask();
                        }
                    }
                    catch (ex) {

                        alert("Error : " + ex.message);
                        RemoveLoaderNMask();
                    }
                },
                error: function () {

                    alert("Error occured");
                    RemoveLoaderNMask();
                },
                cache: false
            });
        }
        else {
            alert("Select a connection.");
        }


    }
    catch (err) { }
}

function CreateArtefactsForUploadedDSD(FileNameWPath) {

    var InputParam;
    try {

        ApplyMaskNLoader(2000);

        InputParam = FileNameWPath;
        InputParam += ParamDelimiter + z("selAssosciatedDatabase").options[z("selAssosciatedDatabase").selectedIndex].value;
        InputParam += ParamDelimiter + document.getElementById('hLoggedInUserNId').value.split('|')[0];
        InputParam += ParamDelimiter + document.getElementById('chkDefaultDSDUpload').checked;

        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '93', 'param1': InputParam },
            async: true,
            success: function (data) {
                try {
                    if (data == "True") {
                        HideLoadingDiv();
                        alert("DSD and MSD uploaded successfully.");
                        BindUploadedDSDsInTheTable();
                        BindUploadedMSDsInTheTable();
                        var NowURL = location.href;
                        document.location = NowURL;

                        RemoveMaskingDiv();

                    }
                    else {
                        alert("Error while uploading DSD:" + data);
                    }
                    HideUploadDSDPopup();
                    RemoveLoaderNMask();
                }
                catch (ex) {
                    alert("Error : " + ex.message);
                    HideUploadDSDPopup();
                    RemoveLoaderNMask();
                }
            },
            error: function () {
                alert("Error occured");
                HideUploadDSDPopup();
                RemoveLoaderNMask();
            },
            cache: false
        });

    }
    catch (err) {
    }
}

function BindDatabasesInTheTable() {

    try {
        InputParam = document.getElementById('hLoggedInUserNId').value.split('|')[0];

        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '106', 'param1': InputParam },
            async: false,
            success: function (data) {
                try {
                    document.getElementById('divListofDatabases').innerHTML = data;
                    SetLanguageFromHiddenFieldValuesForDatabase();
                }
                catch (ex) {
                    alert("Error : " + ex.message);

                }
            },
            error: function () {
                alert("Error occured");

            },
            cache: false
        });
    }
    catch (err) { }

}

function BindUploadedDSDsInTheTable() {

    try {

        InputParam = document.getElementById('hLoggedInUserNId').value.split('|')[0];

        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '99', 'param1': InputParam },
            async: false,
            success: function (data) {
                try {
                    document.getElementById('divListofUploadedDSDs').innerHTML = data;
                    SetLanguageFromHiddenFieldValuesForUploadedDSD();
                }
                catch (ex) {
                    alert("Error : " + ex.message);

                }
            },
            error: function () {
                alert("Error occured");

            },
            cache: false
        });
    }
    catch (err) { }

}

function BindUploadedMSDsInTheTable() {

    try {

        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '103' },
            async: false,
            success: function (data) {
                try {
                    document.getElementById('divListofUploadedMSDs').innerHTML = data;
                    SetLanguageFromHiddenFieldValuesForUploadedMSD();
                }
                catch (ex) {
                    alert("Error : " + ex.message);

                }
            },
            error: function () {
                alert("Error occured");

            },
            cache: false
        });
    }
    catch (err) { }

}

function BindUploadedDSDsList() {

    try {

        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '102' },
            async: false,
            success: function (data) {
                try {
                    z("selDSD").length = 1;
                    var DSDs = data.split(CommaSeparator);
                    var i;
                    for (i = 0; i < DSDs.length; i++) {
                        z("selDSD").options[z("selDSD").length] = new Option(DSDs[i].split("~~")[0], DSDs[i].split("~~")[1]);
                    }
                }
                catch (ex) {
                    alert("Error : " + ex.message);

                }
            },
            error: function () {
                alert("Error occured");

            },
            cache: false
        });
    }
    catch (err) { }

}

function BindAssosciatedDatabaseList(action) {

    try {

        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '107' },
            async: false,
            success: function (data) {
                try {
                    if (action == 'A') {
                        z("selAssosciatedDatabase").length = 1;
                        var AssosciatedDatabases = data.split(CommaSeparator);
                        var i;
                        for (i = 0; i < AssosciatedDatabases.length; i++) {
                            z("selAssosciatedDatabase").options[z("selAssosciatedDatabase").length] = new Option(AssosciatedDatabases[i].split("~~")[0], AssosciatedDatabases[i].split("~~")[1]);
                        }
                    }
                    else {
                        z("selAssosciatedDatabaseEditDelete").length = 1;
                        var AssosciatedDatabases = data.split(CommaSeparator);
                        var i;
                        for (i = 0; i < AssosciatedDatabases.length; i++) {
                            z("selAssosciatedDatabaseEditDelete").options[z("selAssosciatedDatabaseEditDelete").length] = new Option(AssosciatedDatabases[i].split("~~")[0], AssosciatedDatabases[i].split("~~")[1]);
                        }
                    }

                }
                catch (ex) {
                    alert("Error : " + ex.message);

                }
            },
            error: function () {
                alert("Error occured");

            },
            cache: false
        });
    }
    catch (err) { }

}

function BindDefaultDBCheckbox(DSDId) {
    try {
        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '142' },
            async: false,
            success: function (data) {
                try {
                    if (DSDId == data) {
                        z('chkDefaultDSDUpdate').checked = true;
                    }
                }
                catch (ex) {
                    alert("Error : " + ex.message);

                }
            },
            error: function () {
                alert("Error occured");

            },
            cache: false
        });
    }
    catch (err) { }
}

function UpdateArtefactsForUploadedDSD(FileNameWPath) {

    var InputParam;
    try {
        ApplyMaskNLoader(2000);
        InputParam = FileNameWPath;
        InputParam += ParamDelimiter + z("selDSD").options[z("selDSD").selectedIndex].value;
        InputParam += ParamDelimiter + z("selAssosciatedDatabaseEditDelete").options[z("selAssosciatedDatabaseEditDelete").selectedIndex].value;
        InputParam += ParamDelimiter + document.getElementById('hLoggedInUserNId').value.split('|')[0];
        InputParam += ParamDelimiter + document.getElementById('chkDefaultDSDUpdate').checked;

        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '100', 'param1': InputParam },
            async: true,
            success: function (data) {
                try {
                    if (data == "True") {
                        HideLoadingDiv();
                        alert("DSD and MSD updated successfully.");
                        BindUploadedDSDsInTheTable();
                        BindUploadedMSDsInTheTable();
                        var NowURL = location.href;
                        document.location = NowURL;

                        RemoveMaskingDiv();
                    }
                    else {
                        alert("Error while updating DSD:" + data);
                    }
                    HideUploadDSDPopup();
                    RemoveLoaderNMask();
                }
                catch (ex) {
                    alert("Error : " + ex.message);
                    HideUploadDSDPopup();
                    RemoveLoaderNMask();
                }
            },
            error: function () {
                alert("Error occured");
                HideUploadDSDPopup();
                RemoveLoaderNMask();

            },
            cache: false
        });

    }
    catch (err) {
    }
}

function DeleteArtefactsForUploadedDSD() {

    var InputParam;
    if (!z('chkDefaultDSDUpdate').checked) {
        try {
            ApplyMaskNLoader(2000);

            InputParam = z("selDSD").options[z("selDSD").selectedIndex].value;
            InputParam += ParamDelimiter + document.getElementById('hLoggedInUserNId').value.split('|')[0];

            di_jq.ajax({
                type: "POST",
                url: CallbackPageName,
                data: { 'callback': '36', 'param1': InputParam },
                async: false,
                success: function (data) {
                    try {
                        if (data == "true") {
                            HideLoadingDiv();
                            alert("DSD and MSD deleted successfully.");
                            BindUploadedDSDsInTheTable();
                            BindUploadedMSDsInTheTable();
                            RemoveMaskingDiv();
                        }
                        else {
                            alert("Error while deleting DSD:" + data);
                        }
                        HideUploadDSDPopup();
                        RemoveLoaderNMask();
                    }
                    catch (ex) {
                        alert("Error : " + ex.message);
                        HideUploadDSDPopup();
                        RemoveLoaderNMask();
                    }
                },
                error: function () {
                    alert("Error occured");
                    HideUploadDSDPopup();
                    RemoveLoaderNMask();
                },
                cache: false
            });

        }
        catch (err) {
        }
    }
    else {
        alert('Default DSD can\'t be deleted!');
    }
}

function ValidateUploadDSD(action, FileNameWPath) {

    var RetVal = true;

    if (CheckIfDSDAlreadyUploaded(FileNameWPath)) {
        if (action == 'A')//Add
        {
            if (z("selAssosciatedDatabase").selectedIndex <= 0) {
                RetVal = false;
                alert('Please Select Assosciated Database.');
                z("selAssosciatedDatabase").focus();
            }
        }
        else {
            if (z("selAssosciatedDatabaseEditDelete").selectedIndex <= 0) {
                RetVal = false;
                alert('Please Select Assosciated Database.');
                z("selAssosciatedDatabaseEditDelete").focus();
            }

        }
    }
    else {
        RetVal = false;
        alert('A DSD with same Id, Agency Id, Version has been uploaded.Please upload different DSD.');
    }

    return RetVal;
}

function CheckIfDSDAlreadyUploaded(FileNameWPath) {
    var RetVal = false;
    ApplyMaskingDiv();
    ShowLoadingDiv();

    var InputParam = FileNameWPath;
    InputParam += ParamDelimiter + z("selDSD").options[z("selDSD").selectedIndex].value;

    var htmlResp = di_jq.ajax({
        type: "POST",
        url: CallbackPageName,
        data: "callback=105&param1=" + InputParam,
        async: false,
        success: function (data) {
            try {
                if (data == "True") {
                    RetVal = true;
                }
                else {
                    RetVal = false;
                }
                HideLoadingDiv();
            }
            catch (ex) {
                alert("Error : " + ex.message);
                RemoveMaskingDiv();
                HideLoadingDiv();
                HideUploadDSDPopup();
            }
        },
        error: function () {
            alert("Error occured");
            RemoveMaskingDiv();
            HideLoadingDiv();
            HideUploadDSDPopup();
        },
        cache: false
    });

    return RetVal;
}

function OpenUploadDSDPopup(action, DSDId, AssosciatedDbId) {
    ApplyMaskingDiv(100);
    ShowLoadingDiv();

    //Set close button at right corner of popup div
    SetCloseButtonInPopupDiv(di_jq('#divUploadDSD'), 'HideUploadDSDPopup', '../../stock/themes/default/images/close.png');
    di_jq("#divUploadDSD").show('slow');
    GetWindowCentered(document.getElementById('divUploadDSD'), 502, 255);
    z('chkDefaultDSDUpdate').disabled = '';

    if (action == 'A')//Add
    {
        document.getElementById('divAddUploadDSD').style.display = "";
        document.getElementById('divEditDeleteUploadDSD').style.display = "none";
        document.getElementById('chkDefaultDSDUpload').checked = false;
      //  document.getElementById('chkDefaultDSDUpload').disabled=true;
        BindAssosciatedDatabaseList(action);
       
    }
    else if (action == 'U')//Update
    {
        document.getElementById('lang_Update_Your_DSD_MSD').innerHTML = "Update Your DSD And MSD";
        document.getElementById('divAddUploadDSD').style.display = "none";
        document.getElementById('divEditDeleteUploadDSD').style.display = "";
        document.getElementById('UpdateUplDSDFileFromAdmin').style.display = "";
        document.getElementById('btnUpdateDSDFromAdmin').style.display = "";
        document.getElementById('btnDeleteDSDFromAdmin').style.display = "none";
        z('chkDefaultDSDUpdate').checked = false;
        BindAssosciatedDatabaseList(action);
        BindUploadedDSDsList();
        BindDefaultDBCheckbox(DSDId);
        z("selDSD").value = DSDId;
        z("selDSD").disabled = true;
        z("selAssosciatedDatabaseEditDelete").value = AssosciatedDbId;
        z("selAssosciatedDatabaseEditDelete").disabled = false;

    }
    else if (action == 'D')//Delete
    {
        document.getElementById('lang_Update_Your_DSD_MSD').innerHTML = "Delete Your DSD And MSD";
        document.getElementById('divAddUploadDSD').style.display = "none";
        document.getElementById('divEditDeleteUploadDSD').style.display = "";
        document.getElementById('UpdateUplDSDFileFromAdmin').style.display = "none";
        document.getElementById('btnUpdateDSDFromAdmin').style.display = "none";
        document.getElementById('btnDeleteDSDFromAdmin').style.display = "";
        z('chkDefaultDSDUpdate').checked = false;
        z('chkDefaultDSDUpdate').disabled = 'disabled';
        BindAssosciatedDatabaseList(action);
        BindUploadedDSDsList();
        BindDefaultDBCheckbox(DSDId);
        z("selDSD").value = DSDId;
        z("selDSD").disabled = true;
        z("selAssosciatedDatabaseEditDelete").value = AssosciatedDbId;
        z("selAssosciatedDatabaseEditDelete").disabled = true;
    }
   
    HideLoadingDiv();
}

function HideUploadDSDPopup() {
    di_jq('#divUploadDSD').hide('slow');
    RemoveMaskingDiv();
}

function ViewArtefact(ArtefactType, ArtefactPath) {
    // calling callback
    ArtefactCallBack(ArtefactType, ArtefactPath);
}

// callback function
function ArtefactCallBack(ArtefactType, ArtefactPath) {
    ApplyMaskingDiv(100);
    ShowLoadingDiv();

    var InputParam = ArtefactPath;

    var htmlResp = di_jq.ajax({
        type: "POST",
        url: CallbackPageName,
        data: "callback=21&param1=" + InputParam,
        async: false,
        success: function (data) {
            try {
                ShowArtefact(ArtefactType, data);

            }
            catch (ex) {
                alert("Error : " + ex.message);
                RemoveMaskingDiv();
                HideLoadingDiv();
            }
        },
        error: function () {
            alert("Error occured");
            RemoveMaskingDiv();
            HideLoadingDiv();
        },
        cache: false
    });
}

function ShowArtefact(ArtefactType, ResponseText) {
    if (ResponseText != '') {

        document.getElementById('preArtefacts').innerHTML = ResponseText;

        //Set close button at right corner of popup div
        SetCloseButtonInPopupDiv(di_jq('#divArtefacts'), 'HideArtefact', '../../stock/themes/default/images/close.png');

        di_jq("#divArtefacts").show('slow');
        GetWindowCentered(document.getElementById('divArtefacts'), 702, 490);
        if (ArtefactType == 'DSD') {
            document.getElementById('lblArtefactType').innerHTML = "DATA STRUCTURE DEFINITION";
        }
        else if (ArtefactType == 'MSD') {
            document.getElementById('lblArtefactType').innerHTML = "METADATA STRUCTURE DEFINITION";
        }
        HideLoadingDiv();
    }
    else {
        alert('Artefact Not Found');
        RemoveMaskingDiv();
        HideLoadingDiv();
    }
}

function HideArtefact() {
    RemoveMaskingDiv();
    di_jq('#divArtefacts').hide('slow');
}



