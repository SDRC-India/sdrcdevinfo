// JScript File
var MsgErrorOccurred = "Error Occured";
var MsgPleaseBrowseDSD = "Please browse a DSD to Compare against Master DSD.";
var MsgPleaseBrowseDSD1AndDSD2 = "Please browse DSD1 and DSD2 to Compare with each other.";
var MsgGeneratingReport = "Generating  Report....";
var MsgReportSuccessfullyGenerated = "Report generated sucessfully.Please download it by clicking on Download Report button.";
var ValidationCallBackPageName = "Callback.aspx";
var MsgErrorOccurred = "Error Occured";
var MsgValidateSDMXML = "Please browse a SDMX-ML file and then click on Validate SDMX-ML button.";
var MsgValidateDSD = "Please browse a DSD file and then click on Validate DSD button.";
var MsgValidateMetadata = "Please browse a Metadata Report file and then click on Validate Metadata Report button.";
var MsgSDMXMLValidationScenarios = "SDMX-ML Validation Scenarios";
var MsgSDMXMLValidationStatus = "SDMX-ML Validation Status";
var MsgDSDValidationScenarios = "DSD Validation Scenarios";
var MsgDSDValidationStatus = "DSD Validation Status";
var MsgMetadataReportValidationScenarios = "Metadata Report Validation Scenarios";
var MsgMetadataReportValidationStatus = "Metadata Report Validation Status";
var MsgErrorDetailsNotFound = "Error Details Not Found";
function onPageLoad(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hLoggedInUserNId, hLoggedInUserName, hOriginaldbnid) {
    var hsgcount = 10;
    // ************************************************
    // create Form Tag with hidden input boxes
    // ************************************************
    createFormHiddenInputs("frm_sdmxTools", "POST");
    SetCommonLinksHref("frm_sdmxTools", "POST");
    SetOriginaldbnidInForm("frm_sdmxTools", hOriginaldbnid);

    // ************************************************1
    // set page level variables based on selections or defaults
    // ************************************************1
    setPostedData(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hsgcount, hLoggedInUserNId, hLoggedInUserName);

    // ************************************************1
    // Load Language Component
    // ************************************************1
    ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', hlngcode);

    SetCommonRegistryPageDetails('RegTools.aspx', "frm_sdmxTools", "POST");
    document.getElementById('divStructuralComparison').style.display = "none";
    document.getElementById('divCodelistComparison').style.display = "none";

    document.getElementById('divGridContainer').style.display = "none";
    if (hIsUploadedDSD == "True") {
        document.getElementById('divValidateDSD').style.display = "none";
    }
    else {
        document.getElementById('divValidateDSD').style.display = "";
    }
}

function LanguageHandlingForAlerts() {
    MsgErrorOccurred = document.getElementById('hErrorOccurred').value;
    MsgPleaseBrowseDSD = document.getElementById('hMsgPleaseBrowseDSD').value;
    MsgPleaseBrowseDSD1AndDSD2 = document.getElementById('hMsgPleaseBrowseDSD1AndDSD2').value;
    MsgGeneratingReport = document.getElementById('hMsgGeneratingReport').value;
    MsgReportSuccessfullyGenerated = document.getElementById('hMsgReportSuccessfullyGenerated').value;
}

function CompareUserDSDAgainstDevInfoDSD() {

    try {

        document.getElementById('divCodelistComparison').innerHTML = "";
        document.getElementById('divCodelistComparison').style.display = "none";
        if (document.getElementById('hdnDSDAgainstDevInfoDSD').value) {
            ApplyMaskingDiv();
            ShowLoadingDiv();
            InputParam = document.getElementById('hdbnid').value;
            InputParam += ParamDelimiter + document.getElementById('hlngcodedb').value;
            InputParam += ParamDelimiter + document.getElementById('hdnDSDAgainstDevInfoDSD').value;


            var htmlResp = di_jq.ajax
                    ({
                        type: "POST",
                        url: CallBackPageName,
                        data: "callback=78&param1=" + InputParam,
                        async: false,
                        success: function (data) {
                            try {
                                HideLoadingDiv();
                                RemoveMaskingDiv();
                                if (data) {
                                    document.getElementById('divStructuralComparison').style.display = "";
                                    document.getElementById('divStructuralComparison').innerHTML = data;
                                }

                            }
                            catch (ex) {
                                HideLoadingDiv();
                                RemoveMaskingDiv();
                                //                                alert("message:" + ex.message);
                               
                            }
                        },
                        error: function () {
                            HideLoadingDiv();
                            RemoveMaskingDiv();
                                                   },
                        cache: false
                    });
        }
        else {

            alert(MsgPleaseBrowseDSD);

        }


    }
    catch (ex) {
        //        alert("message:" + ex.message);
       
    }


}

function CompareUserDSD1AgainstDSD2() {

    try {
        if ((document.getElementById('hdnDSD1').value) && (document.getElementById('hdnDSD2').value)) {

            ApplyMaskingDiv();
            ShowLoadingDiv();
            InputParam = document.getElementById('hdbnid').value;
            InputParam += ParamDelimiter + document.getElementById('hlngcodedb').value;
            InputParam += ParamDelimiter + document.getElementById('hdnDSD1').value;
            InputParam += ParamDelimiter + document.getElementById('hdnDSD2').value;

            var htmlResp = di_jq.ajax
                    ({
                        type: "POST",
                        url: CallBackPageName,
                        data: "callback=80&param1=" + InputParam,
                        async: false,
                        success: function (data) {
                            try {
                                HideLoadingDiv();
                                RemoveMaskingDiv();
                                if (data) {

                                    document.getElementById('divStructuralComparison').style.display = "";
                                    document.getElementById('divStructuralComparison').innerHTML = data;
                                }

                            }
                            catch (ex) {
                                HideLoadingDiv();
                                RemoveMaskingDiv();
                                //                                alert("message:" + ex.message);
                               
                            }
                        },
                        error: function () {
                            HideLoadingDiv();
                            RemoveMaskingDiv();
                           
                        },
                        cache: false
                    });
        }
        else {

            alert(MsgPleaseBrowseDSD1AndDSD2);

        }


    }
    catch (ex) {
        //        alert("message:" + ex.message);
       
    }

}
function CompareCodelists(ComparisonType) {
    try {

        ApplyMaskingDiv();
        ShowLoadingDiv();
        var strMissingDimensionsMapping = '';
        var strMissingAttributesMapping = '';
        var elem = document.getElementById('frmStructuralComparison').elements;
        for (var i = 0; i < elem.length; i++) {

            if (((elem[i].attributes[1].nodeValue.indexOf("ddlDimensionAlternative_") != -1) && (elem[i].selectedIndex > 0)) || ((elem[i].attributes[2].nodeValue.indexOf("ddlDimensionAlternative_") != -1) && (elem[i].selectedIndex > 0))) {
                if (elem[i].previousElementSibling.previousElementSibling) {
                    strMissingDimensionsMapping += elem[i].previousElementSibling.previousElementSibling.id + "," + elem[i].value + "#";
                }
                else if (elem[i].previousElementSibling) {
                    strMissingDimensionsMapping += elem[i].previousElementSibling.id + "," + elem[i].value + "#";
                }


            }


            else if (((elem[i].attributes[1].nodeValue.indexOf("ddlAttributeAlternative_") != -1) && (elem[i].selectedIndex > 0)) || ((elem[i].attributes[2].nodeValue.indexOf("ddlAttributeAlternative_") != -1) && (elem[i].selectedIndex > 0))) {
                if (elem[i].previousElementSibling.previousElementSibling) {
                    strMissingAttributesMapping += elem[i].previousElementSibling.previousElementSibling.id + "," + elem[i].value + "#";
                }
                else if (elem[i].previousElementSibling) {
                    strMissingAttributesMapping += elem[i].previousElementSibling.id + "," + elem[i].value + "#";
                }

            }
        }

        strMissingDimensionsMapping = strMissingDimensionsMapping.substring(strMissingDimensionsMapping, strMissingDimensionsMapping.length - 1);
        strMissingAttributesMapping = strMissingAttributesMapping.substring(strMissingAttributesMapping, strMissingAttributesMapping.length - 1);

        InputParam = document.getElementById('hdbnid').value;
        InputParam += ParamDelimiter + document.getElementById('hlngcodedb').value;
        InputParam += ParamDelimiter + ComparisonType;

        if (ComparisonType == "1") {
            InputParam += ParamDelimiter + document.getElementById('hdnDSD1').value + "#" + document.getElementById('hdnDSD2').value;
        }
        else {
            InputParam += ParamDelimiter + document.getElementById('hdnDSDAgainstDevInfoDSD').value;
        }

        InputParam += ParamDelimiter + strMissingDimensionsMapping;
        InputParam += ParamDelimiter + strMissingAttributesMapping;



        var htmlResp = di_jq.ajax
            ({
                type: "POST",
                url: CallBackPageName,
                data: "callback=79&param1=" + InputParam,
                async: false,
                success: function (data) {
                    try {
                        HideLoadingDiv();
                        RemoveMaskingDiv();
                        if (data) {

                            document.getElementById('divCodelistComparison').style.display = "";
                            document.getElementById('divCodelistComparison').innerHTML = data;

                        }

                    }
                    catch (ex) {
                        HideLoadingDiv();
                        RemoveMaskingDiv();
                        //                        alert("message:" + ex.message);
                       
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
        //        alert("message:" + ex.message);
       
    }
}


function ToggleExpandCollapse(event) {
    var eventRaiser;
    var eventTarget;


    if (event.srcElement != null) {
        eventRaiser = event.srcElement;
    }
    else if (event.target != null) {
        eventRaiser = event.target;
    }


    eventTarget = '#' + eventRaiser.id.replace("lnk_", "div_");

    if (eventRaiser.getAttribute('class') == 'expand') {
        eventRaiser.setAttribute('class', "collapse");
    }
    else if (eventRaiser.getAttribute('class') == 'collapse') {
        eventRaiser.setAttribute('class', "expand");
    }

    di_jq(eventTarget).slideToggle();
}

function GenerateReport(ComparisonType) {
    try {

        ApplyMaskingDiv();
        ShowLoadingDiv();
        var strMissingDimensionsMapping = '';
        var strMissingAttributesMapping = '';
        var elem = document.getElementById('frmStructuralComparison').elements;
        for (var i = 0; i < elem.length; i++) {


            if (((elem[i].attributes[1].nodeValue.indexOf("ddlDimensionAlternative_") != -1) && (elem[i].selectedIndex > 0)) || ((elem[i].attributes[2].nodeValue.indexOf("ddlDimensionAlternative_") != -1) && (elem[i].selectedIndex > 0))) {
                if (elem[i].previousElementSibling.previousElementSibling) {
                    strMissingDimensionsMapping += elem[i].previousElementSibling.previousElementSibling.id + "," + elem[i].value + "#";
                }
                else if (elem[i].previousElementSibling) {
                    strMissingDimensionsMapping += elem[i].previousElementSibling.id + "," + elem[i].value + "#";
                }


            }


            else if (((elem[i].attributes[1].nodeValue.indexOf("ddlAttributeAlternative_") != -1) && (elem[i].selectedIndex > 0)) || ((elem[i].attributes[2].nodeValue.indexOf("ddlAttributeAlternative_") != -1) && (elem[i].selectedIndex > 0))) {
                if (elem[i].previousElementSibling.previousElementSibling) {
                    strMissingAttributesMapping += elem[i].previousElementSibling.previousElementSibling.id + "," + elem[i].value + "#";
                }
                else if (elem[i].previousElementSibling) {
                    strMissingAttributesMapping += elem[i].previousElementSibling.id + "," + elem[i].value + "#";
                }

            }
        }

        strMissingDimensionsMapping = strMissingDimensionsMapping.substring(strMissingDimensionsMapping, strMissingDimensionsMapping.length - 1);
        strMissingAttributesMapping = strMissingAttributesMapping.substring(strMissingAttributesMapping, strMissingAttributesMapping.length - 1);

        InputParam = document.getElementById('hdbnid').value;
        InputParam += ParamDelimiter + document.getElementById('hlngcodedb').value;
        InputParam += ParamDelimiter + ComparisonType;

        if (ComparisonType == "1") {
            InputParam += ParamDelimiter + document.getElementById('hdnDSD1').value + "#" + document.getElementById('hdnDSD2').value;
        }
        else {
            InputParam += ParamDelimiter + document.getElementById('hdnDSDAgainstDevInfoDSD').value;
        }

        InputParam += ParamDelimiter + strMissingDimensionsMapping;
        InputParam += ParamDelimiter + strMissingAttributesMapping;

        alert(MsgGeneratingReport);

        var htmlResp = di_jq.ajax
            ({
                type: "POST",
                url: CallBackPageName,
                data: "callback=84&param1=" + InputParam,
                async: false,
                success: function (data) {
                    try {
                        HideLoadingDiv();
                        RemoveMaskingDiv();
                        if (data) {

                            document.getElementById("divDownloadReport").style.display = "";
                            document.getElementById("lnkReport").href = "Download.aspx?fileId=stock/tempSDMXFiles/" + data;
                            alert(MsgReportSuccessfullyGenerated);

                        }

                    }
                    catch (ex) {
                        HideLoadingDiv();
                        //                        alert("message:" + ex.message);
                       
                    }
                },
                error: function () {
                    HideLoadingDiv();
                   
                },
                cache: false
            });


    }
    catch (ex) {
        //        alert("message:" + ex.message);
       
    }

}

function LanguageHandlingForAlerts() {
    MsgErrorOccurred = document.getElementById('hErrorOccurred').value;
    MsgValidateSDMXML = document.getElementById('hMsgValidateSDMXML').value;
    MsgValidateDSD = document.getElementById('hMsgValidateDSD').value;
    MsgValidateMetadata = document.getElementById('hMsgValidateMetadata').value;
    MsgSDMXMLValidationScenarios = document.getElementById('hMsgSDMXMLValidationScenarios').value;
    MsgSDMXMLValidationStatus = document.getElementById('hMsgSDMXMLValidationStatus').value;
    MsgDSDValidationScenarios = document.getElementById('hMsgDSDValidationScenarios').value;
    MsgDSDValidationStatus = document.getElementById('hMsgDSDValidationStatus').value;
    MsgMetadataReportValidationScenarios = document.getElementById('hMsgMetadataReportValidationScenarios').value;
    MsgMetadataReportValidationStatus = document.getElementById('hMsgMetadataReportValidationStatus').value;
    MsgErrorDetailsNotFound = document.getElementById('hMsgErrorDetailsNotFound').value;
}

function ValidateSDMXML() {
    try {
        if (document.getElementById('hdnSdmxMlFile').value) {
            ApplyMaskingDiv();
            ShowLoadingDiv();
            InputParam = document.getElementById('hdbnid').value;
            InputParam += ParamDelimiter + document.getElementById('hdnSdmxMlFile').value;

            var htmlResp = di_jq.ajax
                    ({
                        type: "POST",
                        url: ValidationCallBackPageName,
                        data: "callback=71&param1=" + InputParam,
                        async: false,
                        success: function (data) {
                            try {

                                if (data) {
                                    document.getElementById('divGridContainer').style.display = "";
                                    RenderingSDMXMLValidationResponseToOATGrid(data);
                                    HideLoadingDiv();
                                    RemoveMaskingDiv();
                                }

                            }
                            catch (ex) {
                                //                                alert("message:" + ex.message);
                               
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
        else {
            alert(MsgValidateSDMXML);
        }


    }
    catch (ex) {
        //        alert("message:" + ex.message);
       
    }


}


function ValidateDSD() {
    try {
        if (document.getElementById('hdnDSD').value) {
            ApplyMaskingDiv();
            ShowLoadingDiv();
            InputParam = document.getElementById('hdbnid').value;
            InputParam += ParamDelimiter + document.getElementById('hdnDSD').value;

            var htmlResp = di_jq.ajax
                    ({
                        type: "POST",
                        url: ValidationCallBackPageName,
                        data: "callback=72&param1=" + InputParam,
                        async: false,
                        success: function (data) {
                            try {
                                if (data) {
                                    document.getElementById('divGridContainer').style.display = "";
                                    RenderingDSDValidationResponseToOATGrid(data);
                                    HideLoadingDiv();
                                    RemoveMaskingDiv();
                                }

                            }
                            catch (ex) {
                                //                                alert("message:" + ex.message);
                               
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
        else {
            alert(MsgValidateDSD);
        }


    }
    catch (ex) {
        //        alert("message:" + ex.message);
       
    }


}

function ValidateMetadataReport() {
    try {
        if (document.getElementById('hdnMetadataReport').value) {
            ApplyMaskingDiv();
            ShowLoadingDiv();
            InputParam = document.getElementById('hdbnid').value;
            InputParam += ParamDelimiter + document.getElementById('hdnMetadataReport').value;

            var htmlResp = di_jq.ajax
                    ({
                        type: "POST",
                        url: ValidationCallBackPageName,
                        data: "callback=117&param1=" + InputParam,
                        async: false,
                        success: function (data) {
                            try {

                                if (data) {
                                    document.getElementById('divGridContainer').style.display = "";
                                    RenderingMetadataReportValidationResponseToOATGrid(data);
                                    HideLoadingDiv();
                                    RemoveMaskingDiv();
                                }

                            }
                            catch (ex) {
                                //                                alert("message:" + ex.message);
                               
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
        else {
            alert(MsgValidateMetadata);
        }


    }
    catch (ex) {
        //        alert("message:" + ex.message);
       
    }


}

function RenderingSDMXMLValidationResponseToOATGrid(data) {
    var header = new Array();
    var OATdata = [];

    header.push(MsgSDMXMLValidationScenarios);
    header.push(MsgSDMXMLValidationStatus);
    header.push('');


    var distinctValidationRows = data.split('#');
    for (var i = 0; i < distinctValidationRows.length; i++) {
        var tmpRow = distinctValidationRows[i].split("~");
        OATdata.push(tmpRow);
    }
    var mainlist = [];
    mainlist.push(0);
    mainlist.push(1);


    var sublist = [];

    var pivot = new OAT.Pivot("pivot_content", "", "pivot_page", header, OATdata, mainlist, sublist, [], header.length - 1, { showChart: 0, showEmpty: 0, externalSort: true });

}

function RenderingDSDValidationResponseToOATGrid(data) {
    var header = new Array();
    var OATdata = [];

    header.push(MsgDSDValidationScenarios);
    header.push(MsgDSDValidationStatus);
    header.push('');


    var distinctValidationRows = data.split('#');
    for (var i = 0; i < distinctValidationRows.length; i++) {
        var tmpRow = distinctValidationRows[i].split("~");
        OATdata.push(tmpRow);
    }
    var mainlist = [];
    mainlist.push(0);
    mainlist.push(1);


    var sublist = [];

    var pivot = new OAT.Pivot("pivot_content", "", "pivot_page", header, OATdata, mainlist, sublist, [], header.length - 1, { showChart: 0, showEmpty: 0, externalSort: true });

}

function RenderingMetadataReportValidationResponseToOATGrid(data) {
    var header = new Array();
    var OATdata = [];

    header.push(MsgMetadataReportValidationScenarios);
    header.push(MsgMetadataReportValidationStatus);
    header.push('');


    var distinctValidationRows = data.split('#');
    for (var i = 0; i < distinctValidationRows.length; i++) {
        var tmpRow = distinctValidationRows[i].split("~");
        OATdata.push(tmpRow);
    }

    var mainlist = [];
    mainlist.push(0);
    mainlist.push(1);


    var sublist = [];

    var pivot = new OAT.Pivot("pivot_content", "", "pivot_page", header, OATdata, mainlist, sublist, [], header.length - 1, { showChart: 0, showEmpty: 0, externalSort: true });

}

function ViewErrorDetails(ErrorDetails) {

    if (ErrorDetails != '') {
        ShowLoadingDiv();
        ApplyMaskingDiv();

        document.getElementById('txtError').value = ErrorDetails;

        //Set close button at right corner of popup div
        SetCloseButtonInPopupDiv(di_jq('#divErrorDetails'), 'HideErrorDetails');
        di_jq("#divErrorDetails").show('slow');
        GetWindowCentered(document.getElementById('divErrorDetails'), 702, 490);
        HideLoadingDiv();
    }
    else {
        alert(MsgErrorDetailsNotFound);
        HideLoadingDiv();
        RemoveMaskingDiv();
    }
}

function HideErrorDetails() {
    di_jq('#divErrorDetails').hide('slow');
    HideLoadingDiv();
    RemoveMaskingDiv();
}

function CallBack()
{ }