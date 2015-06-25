// JScript File
var CommaSeparator = ",";
var MsgErrorOccurred = "Error Occured";
var MsgFileNotFound = "File Not Found";
var DiscoverRegistrationsCallBackPageName = "Callback.aspx";
function onPageLoad(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hLoggedInUserNId, hLoggedInUserName, hOriginaldbnid) {
    var hsgcount = 10;
    // ************************************************
    // create Form Tag with hidden input boxes
    //    // ************************************************
    createFormHiddenInputs("frm_sdmxStructure", "POST");
    SetCommonLinksHref("frm_sdmxStructure", "POST");

    SetOriginaldbnidInForm("frm_sdmxStructure", hOriginaldbnid);

    // ************************************************1
    // set page level variables based on selections or defaults
    // ************************************************1
    setPostedData(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hsgcount, hLoggedInUserNId, hLoggedInUserName);

    // ************************************************1
    // Load Language Component
    // ************************************************1
    ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', hlngcode);
    //    if (GetLanguageCounts() > 1) {
    //        z("LanguagePipeLi").style.display = "";
    //        z('lngcode_div').style.display = "none";
    //    }

    SetCommonRegistryPageDetails('RegStructure.aspx', "frm_sdmxStructure", "POST");

    document.getElementById("lnkDownloadAll").href = "Download.aspx?fileId=stock/data/" + hdbnid + "/sdmx/Complete.xml";
    document.getElementById("lnkDownloadRpt").href = "Download.aspx?fileId=stock/data/" + hdbnid + "/sdmx/Report.xls";

    BindMSDList();
    BindRegistrations();
    BindDPSchemeAndDFD();
    BindMASchemeAndDFD();
}
function ViewCodelist(CodelistPath) {
    // calling callback
    CallBack(CodelistPath);
}
//function SetHiddenFieldValues() {
//    for (var i = 0; i < document.getElementsByName('lang_View').length; i++) {
//        document.getElementsByName('lang_View')[i].innerHTML = document.getElementById('hView').value;
//    }

//    for (var i = 0; i < document.getElementsByName('lang_Download').length; i++) {
//        document.getElementsByName('lang_Download')[i].innerHTML = document.getElementById('hDownload').value;
//    }

//    for (var i = 0; i < document.getElementsByTagName("span").length; i++) {
//        if (document.getElementsByTagName("span")[i].id.indexOf("lang_Attachment_Level") != -1) {
//            document.getElementsByTagName("span")[i].innerHTML = document.getElementById('hAttachmentLevel').value;
//        }
//        else if (document.getElementsByTagName("span")[i].id.indexOf("lang_Mandatory") != -1) {
//            document.getElementsByTagName("span")[i].innerHTML = document.getElementById('hMandatory').value;
//        }
//        else if (document.getElementsByTagName("span")[i].id.indexOf("lang_Yes") != -1) {
//            document.getElementsByTagName("span")[i].innerHTML = document.getElementById('hYes').value;
//        }
//        else if (document.getElementsByTagName("span")[i].id.indexOf("lang_No") != -1) {
//            document.getElementsByTagName("span")[i].innerHTML = document.getElementById('hNo').value;
//        }
//    }
//    LanguageHandlingForAlerts();
//}

function LanguageHandlingForAlerts() {
    MsgErrorOccurred = document.getElementById('hErrorOccurred').value;
    MsgFileNotFound = document.getElementById('hFileNotFound').value;
}

// callback function
function CallBack(CodelistPath) {

    ApplyMaskingDiv();
    ShowLoadingDiv();

    var InputParam = CodelistPath;
    InputParam += ParamDelimiter + document.getElementById('hlngcode').value;
    InputParam += ParamDelimiter + document.getElementById('hdbnid').value;
    var htmlResp = di_jq.ajax({
        type: "POST",
        url: "Callback.aspx",
        data: "callback=20&param1=" + InputParam,
        async: false,
        success: function (data) {
            try {
                ShowCodelist(data);

            }
            catch (ex) {
                //                    alert("Error : " + ex.message);
                
            }
        },
        error: function () {
            
        },
        cache: false
    });
}

function ShowCodelist(ResponseText) {
    if (ResponseText != '') {
        document.getElementById('divCodelistXmlData').innerHTML = ResponseText;

        //Set close button at right corner of popup div
        SetCloseButtonInPopupDiv(di_jq('#divCodelistXml'), 'HideCodelist');

        di_jq('#divCodelistXml').show('slow');

        GetWindowCentered(document.getElementById('divCodelistXml'), 702, 475);

        HideLoadingDiv();
    }
    else {
        alert(MsgFileNotFound);
        HideLoadingDiv();
        RemoveMaskingDiv();
    }
}

function HideCodelist() {
    di_jq('#divCodelistXml').hide('slow');
    RemoveMaskingDiv();
}

//********************************************MSD***************************************************************//
//Metadata Structure
//********************************************MSD***************************************************************//
function SetHiddenFieldValues() {
    for (var i = 0; i < document.getElementsByName('lang_View').length; i++) {
        document.getElementsByName('lang_View')[i].innerHTML = document.getElementById('hView').value;
    }

    for (var i = 0; i < document.getElementsByName('lang_Download').length; i++) {
        document.getElementsByName('lang_Download')[i].innerHTML = document.getElementById('hDownload').value;
    }

    for (var i = 0; i < document.getElementsByTagName("span").length; i++) {
        if (document.getElementsByTagName("span")[i].id.indexOf("lang_Object_Type") != -1) {
            document.getElementsByTagName("span")[i].innerHTML = document.getElementById('hObjectType').value;
        }
        else if (document.getElementsByTagName("span")[i].id.indexOf("lang_Presentational") != -1) {
            document.getElementsByTagName("span")[i].innerHTML = document.getElementById('hPresentational').value;
        }
        else if (document.getElementsByTagName("span")[i].id.indexOf("lang_Yes") != -1) {
            document.getElementsByTagName("span")[i].innerHTML = document.getElementById('hYes').value;
        }
        else if (document.getElementsByTagName("span")[i].id.indexOf("lang_No") != -1) {
            document.getElementsByTagName("span")[i].innerHTML = document.getElementById('hNo').value;
        }
    }
    for (var i = 0; i < document.getElementsByTagName("h4").length; i++) {
        if (document.getElementsByTagName("h4")[i].id.indexOf("lang_Target") != -1) {
            document.getElementsByTagName("h4")[i].innerHTML = document.getElementById('hTarget').value;
        }
        else if (document.getElementsByTagName("h4")[i].id.indexOf("lang_Report_Structure") != -1) {
            document.getElementsByTagName("h4")[i].innerHTML = document.getElementById('hReportStructure').value;
        }
    }
    for (var i = 0; i < document.getElementsByTagName("h3").length; i++) {
        if (document.getElementsByTagName("h3")[i].id.indexOf("lang_Metadata_Structure_Definition") != -1) {
            document.getElementsByTagName("h3")[i].innerHTML = document.getElementById('hMSD').value;
        }
        else if (document.getElementsByTagName("h3")[i].id.indexOf("lang_Metadata_Flow_Definition") != -1) {
            document.getElementsByTagName("h3")[i].innerHTML = document.getElementById('hMFD').value;
        }
        else if (document.getElementsByTagName("h3")[i].id.indexOf("lang_Concept_Scheme_MSD") != -1) {
            document.getElementsByTagName("h3")[i].innerHTML = document.getElementById('hCS').value;
        }
    }



    for (var i = 0; i < document.getElementsByTagName('span').length; i++) {
        if (document.getElementsByTagName("span")[i].id.indexOf("lang_Id") != -1) {
            document.getElementsByTagName("span")[i].innerHTML = z('hId').value;
        }


        if (document.getElementsByTagName("span")[i].id.indexOf("lang_Provision_Agreement") != -1) {
            document.getElementsByTagName("span")[i].innerHTML = z('hProvisionAgreement').value;
        }



    }

    for (var i = 0; i < document.getElementsByTagName("span").length; i++) {
        if (document.getElementsByTagName("span")[i].id.indexOf("lang_Attachment_Level") != -1) {
            document.getElementsByTagName("span")[i].innerHTML = document.getElementById('hAttachmentLevel').value;
        }
        else if (document.getElementsByTagName("span")[i].id.indexOf("lang_Mandatory") != -1) {
            document.getElementsByTagName("span")[i].innerHTML = document.getElementById('hMandatory').value;
        }

    }

    LanguageHandlingForAlerts();
}

function LanguageHandlingForAlerts() {
    MsgErrorOccurred = document.getElementById('hErrorOccurred').value;
}

function BindMSDList() {
    try {

        InputParam = z('hdbnid').value;
        InputParam += ParamDelimiter + getAbsURL('stock') + "themes/default/images/cust/logo.png";  
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: "callback=95&param1=" + InputParam,
            async: false,
            success: function (data) {
                try {
                    var MSDs = data.split(CommaSeparator);
                    if (MSDs[1] == "MSD_Area") {
                        MSDs.insert(1, 'MSD_Indicator');
                        MSDs.insert(2, 'MSD_Area');
                    }

                    var i;
                    for (i = 0; i < MSDs.length; i++) {

                        z("selectMSD").options[z("selectMSD").length] = new Option(MSDs[i], i + 1);


                    }
                    di_jq('#selectMSD>option:contains(MSD_Area)').insertAfter('#selectMSD>option:contains(MSD_Indicator)');

                    if (z("selectMSD").length == 2) {
                        z("selectMSD").options[1].selected = "selected";
                    }
                    else {
                        for (i = 0; i < MSDs.length; i++) {
                            if (z("selectMSD").options[i].text == "MSD_Indicator") {
                                z("selectMSD").options[i].selected = "selected";
                            }
                            
                        }
                    }

                  
                    BindMSDArtefacts();
                    HideLoadingDiv();
                }
                catch (ex) {
                    HideLoadingDiv();
                    //                    alert("message:" + ex.message);
                    
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

function BindMSDArtefacts() {
    if (z("selectMSD").selectedIndex > 0) {
        z("reg_rgt_sec").style.display = "";
        z("lnkDownloadAll").href = "Download.aspx?fileId=stock/data/" + z('hdbnid').value + "/sdmx/Complete.xml";
        BindMSDAttributes();
        SetHiddenFieldValues();
    }
    else {
        z("reg_rgt_sec").style.display = "none";
        z("divMetaAttributes").innerHTML = "";
    }

}



function BindMSDAttributes() {
    try {

        var InputParam = z('hdbnid').value;
        InputParam += ParamDelimiter + z('hlngcodedb').value;
        InputParam += ParamDelimiter + z("selectMSD").options[z("selectMSD").selectedIndex].text;


        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: "callback=96&param1=" + InputParam,
            async: false,
            success: function (data) {
                try {

                    z("divMetaAttributes").innerHTML = data;
                    HideLoadingDiv();
                }
                catch (ex) {
                    HideLoadingDiv();
                    //                    alert("message:" + ex.message);
                    
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


//********************************************MSD***************************************************************//
//Provision Agreement
//********************************************MSD***************************************************************//

//function SetHiddenFieldValues() {
//    for (var i = 0; i < document.getElementsByName('lang_View').length; i++) {
//        document.getElementsByName('lang_View')[i].innerHTML = document.getElementById('hView').value;
//    }

//    for (var i = 0; i < document.getElementsByName('lang_Download').length; i++) {
//        document.getElementsByName('lang_Download')[i].innerHTML = document.getElementById('hDownload').value;
//    }

//    for (var i = 0; i < document.getElementsByTagName('span').length; i++) {
//        if (document.getElementsByTagName("span")[i].id.indexOf("lang_Id") != -1) {
//            document.getElementsByTagName("span")[i].innerHTML = z('hId').value;
//        }


//        if (document.getElementsByTagName("span")[i].id.indexOf("lang_Provision_Agreement") != -1) {
//            document.getElementsByTagName("span")[i].innerHTML = z('hProvisionAgreement').value;
//        }



//    }

//}


function BindRegistrations() {
    var ArtefactType;
    var InputParam;
    ApplyMaskingDiv();
    ShowLoadingDiv();
    // document.getElementById('divRegistrationsByPA').style.display = "";
    ArtefactType = "PA";
    InputParam = document.getElementById('hdbnid').value;
    InputParam += ParamDelimiter + document.getElementById('hlngcodedb').value;
    InputParam += ParamDelimiter + ArtefactType;

    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: { 'callback': '1042', 'param1': InputParam },
            async: true,
            success: function (data) {
                try {
                    if (data) {
                        if (ArtefactType == "PA") {
                            document.getElementById('divRegistrationsByPA').innerHTML = data;
                        }
                        SetHiddenFieldValues();
                        // MakeStaticHeaderForAllTheTablesInPage();
                        RemoveLoaderNMask();
                    }
                }
                catch (ex) {
                    alert("message:" + ex.message);
                    RemoveLoaderNMask();
                }
            },
            error: function () {
                
                RemoveLoaderNMask();
            },
            cache: false
        });
    }
    catch (ex) {
    }

}


//********************************************MA/DP Scheme***************************************************************//

//********************************************MA/DP Scheme***************************************************************//

// BindDPSchemeAndDFD function
function BindDPSchemeAndDFD() {
    ApplyMaskingDiv();
    ShowLoadingDiv();

    var InputParam = document.getElementById('hdbnid').value;
    InputParam += ParamDelimiter + document.getElementById('hlngcodedb').value;
    var htmlResp = di_jq.ajax({
        type: "POST",
        url: "Callback.aspx",
        data: "callback=64&param1=" + InputParam,
        async: false,
        success: function (data) {
            try {
                document.getElementById('divDownloadsDP').innerHTML = data;
                HideLoadingDiv();
                RemoveMaskingDiv();

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



function BindMASchemeAndDFD() {
    ApplyMaskingDiv();
    ShowLoadingDiv();

    var InputParam = document.getElementById('hdbnid').value;
    InputParam += ParamDelimiter + document.getElementById('hlngcodedb').value;
    var htmlResp = di_jq.ajax({
        type: "POST",
        url: "Callback.aspx",
        data: "callback=1041&param1=" + InputParam,
        async: false,
        success: function (data) {
            try {
                document.getElementById('divDownloadsMA').innerHTML = data;
                HideLoadingDiv();
                RemoveMaskingDiv();

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
        url: "Callback.aspx",
        data: "callback=21&param1=" + InputParam,
        async: false,
        success: function (data) {
            try {
                ShowArtefact(ArtefactType, data);

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


function ShowArtefact(ArtefactType, ResponseText) {
    if (ResponseText != '') {

        document.getElementById('preArtefacts').innerHTML = ResponseText;

        //Set close button at right corner of popup div
        SetCloseButtonInPopupDiv(di_jq('#divArtefacts'), 'HideArtefact');

        di_jq("#divArtefacts").show('slow');
        GetWindowCentered(document.getElementById('divArtefacts'), 702, 490);
        if (ArtefactType == 'DFD') {
            document.getElementById('lblArtefactType').innerHTML = "DATA FLOW DEFINITION";
        }
        else if (ArtefactType == 'MFD') {
            document.getElementById('lblArtefactType').innerHTML = "METADATA FLOW DEFINITION";
        }
        else if (ArtefactType == 'PA') {
            document.getElementById('lblArtefactType').innerHTML = "PROVISION AGREEMENT";
        }
        else if (ArtefactType == 'DPScheme') {
            document.getElementById('lblArtefactType').innerHTML = "DATA PROVIDER SCHEME";
        }
        else if (ArtefactType == 'MAScheme') {
            document.getElementById('lblArtefactType').innerHTML = "MAINTENANCE AGENCY SCHEME";
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


function ExpandCollapseList(ImageToUse, ControlToToggle) {
    if ((z(ControlToToggle).style.display == "") || (z(ControlToToggle).style.display == "block")) {
        z(ImageToUse).src = "../../stock/themes/default/images/expand.png";
        di_jq('#' + ControlToToggle).hide('slow');
    }
    else {
        z(ImageToUse).src = "../../stock/themes/default/images/collapse.png";
        di_jq('#' + ControlToToggle).show('slow');
    }
}