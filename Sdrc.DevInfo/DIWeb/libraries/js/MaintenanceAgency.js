var CallbackPageName = "../Callback.aspx";
var CommaSeparator = ",";


function GenerateSDMXArtefacts() {
    try {
       
        var DbId = "";

        if (di_jq("#divListofDatabases :checked").length > 0) {
            di_jq("#divListofDatabases :checked").each(function () {
                DbId = di_jq(this).context.id.substr(3);
            });

            ApplyMaskNLoader();
            di_jq.ajax({
                type: "POST",
                url: CallbackPageName,
                data: { 'callback': '47', 'param1': DbId },
                async: true,
                success: function (data) {
                    try {

                        if (data == "true") {
                            alert("SDMX Artefacts Generated successfully for selected database.");
                            BindDatabasesInTheTable();
                            RemoveLoaderNMask();
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

                    
                    RemoveLoaderNMask();
                },
                cache: false
            });
        }
        else
        {
            alert("Select a connection.");
        }


    }
    catch (err) { }
}

function CreateArtefactsForUploadedDSD(FileNameWPath) {

    var InputParam;
    try {
       
            ApplyMaskNLoader();
            InputParam = FileNameWPath;
            InputParam += ParamDelimiter + z("selAssosciatedDatabase").options[z("selAssosciatedDatabase").selectedIndex].value;
            di_jq.ajax({
                type: "POST",
                url: CallbackPageName,
                data: { 'callback': '93', 'param1': InputParam },
                async: true,
                success: function (data) {
                    try {
                        if (data == "True") {
                            alert("DSD and MSD uploaded successfully.");
                            BindUploadedDSDsInTheTable();
                            BindUploadedMSDsInTheTable();
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
        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '106'},
            async: false,
            success: function (data) {
                try {
                    document.getElementById('divListofDatabases').innerHTML = data;
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

function BindUploadedDSDsInTheTable() {

    try {

        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '99' },
            async: false,
            success: function (data) {
                try {
                    document.getElementById('divListofUploadedDSDs').innerHTML = data;
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
                    if (action == 'A') 
                    {
                        z("selAssosciatedDatabase").length = 1;
                        var AssosciatedDatabases = data.split(CommaSeparator);
                        var i;
                        for (i = 0; i < AssosciatedDatabases.length; i++) {
                            z("selAssosciatedDatabase").options[z("selAssosciatedDatabase").length] = new Option(AssosciatedDatabases[i].split("~~")[0], AssosciatedDatabases[i].split("~~")[1]);
                        }
                    }
                    else 
                    {
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
                

            },
            cache: false
        });
    }
    catch (err) { }

}


function UpdateArtefactsForUploadedDSD(FileNameWPath) {

    var InputParam;
    try 
    {
          ApplyMaskNLoader();
            InputParam = FileNameWPath;
            InputParam += ParamDelimiter + z("selDSD").options[z("selDSD").selectedIndex].value;
            InputParam += ParamDelimiter + z("selAssosciatedDatabaseEditDelete").options[z("selAssosciatedDatabaseEditDelete").selectedIndex].value;
            di_jq.ajax({
                type: "POST",
                url: CallbackPageName,
                data: { 'callback': '100', 'param1': InputParam },
                async: true,
                success: function (data) {
                    try {
                        if (data == "True") {
                            alert("DSD and MSD updated successfully.");
                            BindUploadedDSDsInTheTable();
                            BindUploadedMSDsInTheTable();
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
    try {
        ApplyMaskNLoader();
        InputParam = z("selDSD").options[z("selDSD").selectedIndex].value;
        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '36', 'param1': InputParam },
            async: false,
            success: function (data) {
                try {
                    if (data == "true") {
                        alert("DSD and MSD deleted successfully.");
                        BindUploadedDSDsInTheTable();
                        BindUploadedMSDsInTheTable();
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
                
                HideUploadDSDPopup();
                RemoveLoaderNMask();
            },
            cache: false
        });

    }
    catch (err) {
    }
}

function ValidateUploadDSD(action, FileNameWPath) {

    var RetVal = true;

    if (CheckIfDSDAlreadyUploaded(FileNameWPath)) 
    {
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
    else 
    {
        RetVal = false;
        alert('A DSD with same Id, Agency Id, Version has been uploaded.Please upload different DSD.');
    }
   
    return RetVal;
}

function CheckIfDSDAlreadyUploaded(FileNameWPath) 
{
    var RetVal=false;
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
            
            RemoveMaskingDiv();
            HideLoadingDiv();
            HideUploadDSDPopup();
        },
        cache: false
    });

    return RetVal;
}

function OpenUploadDSDPopup(action, DSDId, AssosciatedDbId) {
    ApplyMaskingDiv();
    ShowLoadingDiv();

    //Set close button at right corner of popup div
    SetCloseButtonInPopupDiv(di_jq('#divUploadDSD'), 'HideUploadDSDPopup', '../../../stock/themes/default/images/close.png');
    di_jq("#divUploadDSD").show('slow');
    GetWindowCentered(document.getElementById('divUploadDSD'), 502, 255);

    if (action == 'A')//Add
    {
        document.getElementById('divAddUploadDSD').style.display = "";
        document.getElementById('divEditDeleteUploadDSD').style.display = "none";
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
        BindAssosciatedDatabaseList(action);
        BindUploadedDSDsList();
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
        BindAssosciatedDatabaseList(action);
        BindUploadedDSDsList();
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
    ApplyMaskingDiv();
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
        SetCloseButtonInPopupDiv(di_jq('#divArtefacts'), 'HideArtefact', '../../../stock/themes/default/images/close.png');

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