var ErrorMessage = '';
//var MsgErrorOccurred = "Error Occurred";
//var MsgDeleteRegistrationConfirmation = "Do you really want to delete this registration?";
var MsgWADLRequired = "For a REST Web Service WADL URL is required!";
var MsgWSDLRequired = "For a SOAP Web Service WSDL URL is required!";
var MsgSelectDataMetadataFlow = "Select Data/Metadata Flows!";
var MsgProvideWSUrlOrSDMXMLFileUrl = "Atleast provide one of Web Service URL or SDMX-ML File URL!";
var MsgRegistrationWithId = "Registration With Id : ";
//var MsgDeletedSuccessfully = " deleted Successfully";
var CallbackPageName = "Callback.aspx";
var selectedDBorDSD = '';
var SelectedDefaultDBorDSDText = '';
var AreaSelections = {};
var TimeSelections = '';
var SourceSelections = '';
var AreaGenSelections = '';
var ErrorLogInPublishData = '';
var OriginalDBNid = '';
var ErrorNDFIndicatorGIDS = '';
function onPageLoad(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hLoggedInUserNId, hLoggedInUserName, hOriginaldbnid) {
    var hsgcount = 10;
    // ************************************************
    // create Form Tag with hidden input boxes
    // ************************************************
    createFormHiddenInputs("frm_sdmxPublishData", "POST");
    SetCommonLinksHref("frm_sdmxPublishData", "POST");
    SetOriginaldbnidInForm("frm_sdmxPublishData", hOriginaldbnid);

    // ************************************************1
    // set page level variables based on selections or defaults
    // ************************************************1
    setPostedData(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hsgcount, hLoggedInUserNId, hLoggedInUserName);

    // ************************************************1
    // Load Language Component
    // ************************************************1
    ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', hlngcode);

    SetCommonRegistryPageDetails('RegPublishData.aspx', "frm_sdmxPublishData", "POST");

    OriginalDBNid = hOriginaldbnid;
    selectedDBorDSD = di_jq("option:selected", '#cphMainContent_selectDSDInUse').attr("class")
    SelectedDefaultDBorDSDText = di_jq("option:selected", '#cphMainContent_selectDSDInUse').text().split('-');
    SelectDefaultAction();

    if (selectedDBorDSD == "Database") {
        if (SelectedDefaultDBorDSDText != null) {
            z('hdnSelectedDatabase').value = di_jq("option:selected", '#cphMainContent_selectDSDInUse').val();
        }
    }
    else {
        if (SelectedDefaultDBorDSDText != null) {
            z('hdnSelectedDSD').value = di_jq("option:selected", '#cphMainContent_selectDSDInUse').val();
        }
    }
    LanguageHandlingForAlerts();
}

function LanguageHandlingForAlerts() {
    MsgErrorOccurred = document.getElementById('hErrorOccurred').value;
    MsgDeleteRegistrationConfirmation = document.getElementById('hDeleteRegistrationConfirmation').value;
    MsgWADLRequired = document.getElementById('hWADLRequired').value;
    MsgWSDLRequired = document.getElementById('hWSDLRequired').value;
    MsgSelectDataMetadataFlow = document.getElementById('hSelectDataMetadataFlow').value;
    MsgProvideWSUrlOrSDMXMLFileUrl = document.getElementById('hProvideWSUrlOrSDMXMLFileUrl').value;
    MsgRegistrationWithId = document.getElementById('hRegistrationWithId').value;
    MsgDeletedSuccessfully = document.getElementById('hDeletedSuccessfully').value;
  
}

function GetNextPageRegistrations() {
    var StartIndex, NumberPagingRows, TotalRows;

    StartIndex = parseInt(z('hStartIndex').value);
    NumberPagingRows = parseInt(z('hNumberPagingRows').value);
    TotalRows = parseInt(z('hTotalRows').value);

    z('hStartIndex').value = StartIndex + NumberPagingRows;
    GetRegistrationsSummary(z('hdbnid').value, z('hLoggedInUserNId').value.split('|')[0], z('hlngcodedb').value);
}

function GetPreviousPageRegistrations() {
    var StartIndex, NumberPagingRows, TotalRows;

    StartIndex = parseInt(z('hStartIndex').value);
    NumberPagingRows = parseInt(z('hNumberPagingRows').value);
    TotalRows = parseInt(z('hTotalRows').value);

    z('hStartIndex').value = StartIndex - NumberPagingRows;
    GetRegistrationsSummary(z('hdbnid').value, z('hLoggedInUserNId').value.split('|')[0], z('hlngcodedb').value);
}

function GetRegistrationsSummary(DBNId, UserNId, LanguageCode) {
    var InputParam;
    var StartIndex, NumberPagingRows, TotalRows;

    InputParam = DBNId;
    InputParam += ParamDelimiter + UserNId;
    InputParam += ParamDelimiter + LanguageCode;
    InputParam += ParamDelimiter + z('hStartIndex').value;
    InputParam += ParamDelimiter + OriginalDBNid;
    ApplyMaskingDiv();
    ShowLoadingDiv();

    if (UserNId) {
        try {
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: CallBackPageName,
                data: { 'callback': '54', 'param1': InputParam },
                async: true,
                success: function (data) {
                    try {
                        if (data != '') {
                            document.getElementById('gridData').style.display = "";
                            RenderingPublishDetailsInOATGrid(data.split(ParamDelimiter)[0]);

                            StartIndex = parseInt(data.split(ParamDelimiter)[1]);
                            NumberPagingRows = parseInt(data.split(ParamDelimiter)[2]);
                            TotalRows = parseInt(data.split(ParamDelimiter)[3]);

                            z('hStartIndex').value = StartIndex;
                            z('hNumberPagingRows').value = NumberPagingRows;
                            z('hTotalRows').value = TotalRows;

                            if (StartIndex + NumberPagingRows <= TotalRows) {
                                z('divResultsCount').style.display = "";
                                // z('divResultsCount').innerHTML = "( " + parseInt(StartIndex + 1) + " - " + parseInt(StartIndex + NumberPagingRows) + " ) / " + TotalRows;
                                z('divResultsCount').innerHTML = z('hdnLangTotalRows').value + TotalRows;
                            }
                            else {
                                z('divResultsCount').style.display = "";
                                z('divResultsCount').innerHTML = z('hdnLangTotalRows').value + TotalRows;
                                // z('divResultsCount').innerHTML = "( " + parseInt(StartIndex + 1) + " - " + TotalRows + " ) / " + TotalRows;
                            }

                            EnableDisableLinks(z('aPrevious'), false);
                            EnableDisableLinks(z('aNext'), false);

                            if (StartIndex == 0) {
                                EnableDisableLinks(z('aPrevious'), true);
                            }

                            if (StartIndex + NumberPagingRows >= TotalRows) {
                                EnableDisableLinks(z('aNext'), true);
                            }
                        }
                        else {
                            document.getElementById('gridData').style.display = "none";

                            StartIndex = 0;
                            TotalRows = 0;

                            z('hStartIndex').value = StartIndex;
                            z('hTotalRows').value = TotalRows;
                            z('divResultsCount').style.display = "";
                            z('divResultsCount').innerHTML = z('hNoRegistrationsFound').innerHTML;

                            EnableDisableLinks(z('aPrevious'), true);
                            EnableDisableLinks(z('aNext'), true);
                        }

                        LanguageHandling();
                        HideLoadingDiv();
                        RemoveMaskingDiv();
                    }
                    catch (ex) {
                        //                            alert("message:" + ex.message);
                        //
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
        }
    }
}

function GetRegistrationDetails(registrationId) {
    var InputParam;
    var WebserviceURL, IsREST, WADLURL, IsSOAP, WSDLURL, FileURL, IsMetadata, DFDOrMFDId;

    InputParam = z('hdbnid').value;
    InputParam += ParamDelimiter + z('hLoggedInUserNId').value.split('|')[0];
    InputParam += ParamDelimiter + registrationId;
    InputParam += ParamDelimiter + z('hlngcodedb').value;
    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: { 'callback': '55', 'param1': InputParam },
            async: false,
            success: function (data) {
                try {
                    registrationId = data.split(ParamDelimiter)[0];
                    WebserviceURL = data.split(ParamDelimiter)[1];
                    IsREST = data.split(ParamDelimiter)[2];
                    WADLURL = data.split(ParamDelimiter)[3];
                    IsSOAP = data.split(ParamDelimiter)[4];
                    WSDLURL = data.split(ParamDelimiter)[5];
                    FileURL = data.split(ParamDelimiter)[6];
                    IsMetadata = data.split(ParamDelimiter)[7];
                    DFDOrMFDId = data.split(ParamDelimiter)[8];

                    SetControlsWithValues(registrationId, WebserviceURL, IsREST, WADLURL, IsSOAP, WSDLURL, FileURL, IsMetadata, DFDOrMFDId);
                }
                catch (ex) {
                    //                    alert("message:" + ex.message);
                    //  
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

function OpenRegistrationDetailsPopup(action, registrationId) {
    ApplyMaskingDiv();
    ShowLoadingDiv();
    Reset();
    //Set close button at right corner of popup div
    SetCloseButtonInPopupDiv(di_jq('#divRegistrationDetails'), 'HideRegistrationDetailsPopup');
    di_jq("#divRegistrationDetails").show('slow');
    GetWindowCentered(z('divRegistrationDetails'), 702, 400);

    if (action == 'A')//Add
    {
        z('rowAddRegistration').style.display = "";
        z('rowUpdateRegistration').style.display = "none";
        z('rowDeleteRegistration').style.display = "none";
        z('rowViewRegistration').style.display = "none";
    }
    else if (action == 'U')//Update
    {
        z('rowAddRegistration').style.display = "none";
        z('rowUpdateRegistration').style.display = "";
        z('rowDeleteRegistration').style.display = "none";
        z('rowViewRegistration').style.display = "none";

        GetRegistrationDetails(registrationId);
    }
    else if (action == 'V')//View
    {
        z('rowAddRegistration').style.display = "none";
        z('rowUpdateRegistration').style.display = "none";
        z('rowDeleteRegistration').style.display = "none";
        z('rowViewRegistration').style.display = "";

        GetRegistrationDetails(registrationId);
    }
    else if (action == 'D')//Delete
    {
        z('rowAddRegistration').style.display = "none";
        z('rowUpdateRegistration').style.display = "none";
        z('rowDeleteRegistration').style.display = "";
        z('rowViewRegistration').style.display = "none";

        GetRegistrationDetails(registrationId);
    }

    HideLoadingDiv();
}

function AddRegistrationAndSendNotification() {
    var InputParam;

    if (ValidateInputs() == true) {
        InputParam = z('hdbnid').value;
        InputParam += ParamDelimiter + z('hLoggedInUserNId').value.split('|')[0];
        InputParam += ParamDelimiter + z('chkIsMetadata').checked;
        InputParam += ParamDelimiter + z("selDFDMFD").options[z("selDFDMFD").selectedIndex].value;
        InputParam += ParamDelimiter + z('txtWebServiceURL').value;
        InputParam += ParamDelimiter + z('chkIsREST').checked;
        InputParam += ParamDelimiter + z('txtWADLURL').value;
        InputParam += ParamDelimiter + z('chkIsSOAP').checked;
        InputParam += ParamDelimiter + z('txtWSDLURL').value;
        InputParam += ParamDelimiter + z('txtFileURL').value;
        InputParam += ParamDelimiter + z('hlngcode').value;
        InputParam += ParamDelimiter + OriginalDBNid;
        try {
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: CallBackPageName,
                data: { 'callback': '53', 'param1': InputParam },
                async: false,
                success: function (data) {
                    try {
                        if (data.split(ParamDelimiter)[0] == 'false') {
                            alert(data.split(ParamDelimiter)[1]);
                        }
                        else {
                            z('hdnRegistrationId').value = data.split(ParamDelimiter)[1];
                        }

                        HideLoadingDiv();
                        Close(z('divRegistrationDetails'));
                        GetRegistrationsSummary(z('hdbnid').value, z('hLoggedInUserNId').value.split('|')[0], z('hlngcodedb').value);
                    }
                    catch (ex) {
                        //                        alert("message:" + ex.message);
                        //   
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
        alert(ErrorMessage);
    }
}

function UpdateRegistrationAndSendNotification() {
    var InputParam;

    if (ValidateInputs() == true) {
        InputParam = z('hdbnid').value;
        InputParam += ParamDelimiter + z('hLoggedInUserNId').value.split('|')[0];
        InputParam += ParamDelimiter + z('chkIsMetadata').checked;
        InputParam += ParamDelimiter + z("selDFDMFD").options[z("selDFDMFD").selectedIndex].value;
        InputParam += ParamDelimiter + z('hdnRegistrationId').value;
        InputParam += ParamDelimiter + z('txtWebServiceURL').value;
        InputParam += ParamDelimiter + z('chkIsREST').checked;
        InputParam += ParamDelimiter + z('txtWADLURL').value;
        InputParam += ParamDelimiter + z('chkIsSOAP').checked;
        InputParam += ParamDelimiter + z('txtWSDLURL').value;
        InputParam += ParamDelimiter + z('txtFileURL').value;
        InputParam += ParamDelimiter + z('hlngcode').value;


        try {
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: CallBackPageName,
                data: { 'callback': '56', 'param1': InputParam },
                async: false,
                success: function (data) {
                    try {
                        if (data.split(ParamDelimiter)[0] == 'false') {
                            alert(data.split(ParamDelimiter)[1]);
                        }
                        else {
                            z('hdnRegistrationId').value = data.split(ParamDelimiter)[1];
                        }

                        HideLoadingDiv();
                        Close(z('divRegistrationDetails'));
                        GetRegistrationsSummary(z('hdbnid').value, z('hLoggedInUserNId').value.split('|')[0], z('hlngcodedb').value);
                    }
                    catch (ex) {
                        //                        alert("message:" + ex.message);
                        //  
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
        alert(ErrorMessage);
    }
}

function SendNotification() {
    var InputParam;
    var WebserviceURL, IsREST, WADLURL, IsSOAP, WSDLURL, FileURL;

    ShowLoadingDiv();
    InputParam = z('hdbnid').value;
    InputParam += ParamDelimiter + z('hLoggedInUserNId').value.split('|')[0];
    InputParam += ParamDelimiter + z('hdnRegistrationId').value;
    InputParam += ParamDelimiter + z('chkIsMetadata').checked;

    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: { 'callback': '58', 'param1': InputParam },
            async: false,
            success: function (data) {
                try {
                    if (data.split(ParamDelimiter)[0] == 'false') {
                        alert(data.split(ParamDelimiter)[1]);
                    }

                    HideLoadingDiv();
                    Close(z('divRegistrationDetails'));
                    GetRegistrationsSummary(z('hdbnid').value, z('hLoggedInUserNId').value.split('|')[0], z('hlngcodedb').value);
                }
                catch (ex) {
                    //                    alert("message:" + ex.message);
                    //   
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

function DeleteRegistration() {
    var InputParam;

    if (confirm(MsgDeleteRegistrationConfirmation)) {
        InputParam = z('hdbnid').value;
        InputParam += ParamDelimiter + z('hLoggedInUserNId').value.split('|')[0];
        InputParam += ParamDelimiter + z('hdnRegistrationId').value;
        InputParam += ParamDelimiter + z("selDFDMFD").value;
        InputParam += ParamDelimiter + z('hlngcode').value;

        try {
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: CallBackPageName,
                data: { 'callback': '57', 'param1': InputParam },
                async: false,
                success: function (data) {
                    try {
                        if (data.split(ParamDelimiter)[0] == 'false') {
                            alert("Registration Deleted Successfully"); //data.split(ParamDelimiter)[1]
                        }
                        else {
                            alert(MsgRegistrationWithId + data.split(ParamDelimiter)[1] + MsgDeletedSuccessfully);
                        }

                        Close(z('divRegistrationDetails'));

                        if (parseInt(z('hStartIndex').value) + 1 == parseInt(z('hTotalRows').value)) {
                            if (parseInt(z('hStartIndex').value) != 0) {
                                GetPreviousPageRegistrations();
                            }
                            else {
                                GetRegistrationsSummary(z('hdbnid').value, z('hLoggedInUserNId').value.split('|')[0], z('hlngcodedb').value);
                            }
                        }
                        else {
                            GetRegistrationsSummary(z('hdbnid').value, z('hLoggedInUserNId').value.split('|')[0], z('hlngcodedb').value);
                        }
                    }
                    catch (ex) {
                        alert("message:" + ex.message);
                        // 
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
}

function BindDFDAndMFDList() {
    var DFDMFDList;

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
                    var DFDMFDList = data.split(",");

                    for (var i = 0; i < DFDMFDList.length; i++) {
                        z("selDFDMFD").options[z("selDFDMFD").length] = new Option(DFDMFDList[i].split("~~")[0], DFDMFDList[i].split("~~")[1]);
                    }

                    HideLoadingDiv();
                }
                catch (ex) {
                    HideLoadingDiv();
                    //                    alert("message:" + ex.message);
                    // 
                }
            },
            error: function () {
                HideLoadingDiv();
                // 
            },
            cache: false
        });
    }
    catch (ex) {
    }
}

function SetControlsWithValues(registrationId, WebserviceURL, IsREST, WADLURL, IsSOAP, WSDLURL, FileURL, IsMetadata, DFDOrMFDId) {
    z('hdnRegistrationId').value = registrationId;
    z('txtWebServiceURL').value = WebserviceURL;

    if (IsREST == "True") {
        z('chkIsREST').checked = true;
        z('txtWADLURL').value = WADLURL;
        z('txtWADLURL').disabled = "";
    }
    else {
        z('chkIsREST').checked = false;
        z('txtWADLURL').value = "";
        z('txtWADLURL').disabled = "disabled";
    }

    if (IsSOAP == "True") {
        z('chkIsSOAP').checked = true;
        z('txtWSDLURL').value = WSDLURL;
        z('txtWSDLURL').disabled = "";
    }
    else {
        z('chkIsSOAP').checked = false;
        z('txtWSDLURL').value = "";
        z('txtWSDLURL').disabled = "disabled";
    }

    z('txtFileURL').value = FileURL;

    if (IsMetadata == "True") {
        z('chkIsMetadata').checked = true;
    }
    else {
        z('chkIsMetadata').checked = false;
    }

    BindDFDAndMFDList();

    z("selDFDMFD").value = DFDOrMFDId;
}

function ValidateInputs() {
    var RetVal;

    RetVal = true;

    if (z('chkIsREST').checked == true && (z('txtWADLURL').value == null || z('txtWADLURL').value == "")) {
        RetVal = false;
        ErrorMessage = MsgWADLRequired;
        z('txtWADLURL').focus();
    }
    else if (z('chkIsSOAP').checked == true && (z('txtWSDLURL').value == null || z('txtWSDLURL').value == "")) {
        RetVal = false;
        ErrorMessage = MsgWSDLRequired;
        z('txtWSDLURL').focus();
    }
    else if (z('selDFDMFD').selectedIndex <= 0) {
        RetVal = false;
        ErrorMessage = MsgSelectDataMetadataFlow;
        z('selDFDMFD').focus();
    }
    else if ((z('txtWebServiceURL').value == "" || z('txtWebServiceURL').value == null) &&
        (z('txtFileURL').value == "" || z('txtFileURL').value == null)) {
        RetVal = false;
        ErrorMessage = MsgProvideWSUrlOrSDMXMLFileUrl;
    }

    return RetVal;
}

function Reset() {
    z('chkIsMetadata').checked = false;
    BindDFDAndMFDList();
    z('txtWebServiceURL').value = "";

    z('chkIsREST').checked = false;
    z('txtWADLURL').value = "";

    z('chkIsSOAP').checked = false;
    z('txtWSDLURL').value = "";

    z('txtFileURL').value = "";
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

function HideRegistrationDetailsPopup() {
    di_jq('#divRegistrationDetails').hide('slow');
    RemoveMaskingDiv();
}

//mainlist.push(1);
//mainlist.push(2);
//mainlist.push(3);
//mainlist.push(4);
//mainlist.push(5);
//mainlist.push(6);
//mainlist.push(7);
//mainlist.push(8);
//mainlist.push(9);


function RenderingPublishDetailsInOATGrid(data) {
    var header = new Array();
    var OATdata = [];
  
    header.push(document.getElementById('hId').value);
    header.push(document.getElementById('hIndicator').value);
    //header.push(document.getElementById('hLanguage').value);
    header.push(document.getElementById('hQueryableData').value);
    header.push(document.getElementById('hWADL').value);
    header.push(document.getElementById('hWSDL').value);
    header.push(document.getElementById('hSimpleData').value);
    header.push(document.getElementById('hDataMetadata').value);
    header.push(document.getElementById('hDFDMFD').value);
    header.push(document.getElementById('hConstraints').value);
    header.push(document.getElementById('hAction').value);

    var distinctPublishRows = data.split('#');
  
    for (var i = 0; i < distinctPublishRows.length; i++) {
        var tmpRow = distinctPublishRows[i].split("~");
        OATdata.push(tmpRow);
      
    }
    var mainlist = [];
    //mainlist.push(0);
    mainlist.push(1);
    mainlist.push(2);
    mainlist.push(3);
    mainlist.push(4);
    mainlist.push(5);
    mainlist.push(6);
    mainlist.push(7);
    mainlist.push(8);
    // mainlist.push(9);
    // mainlist.push(10);
    var sublist = [];

    var pivot = new OAT.Pivot("pivot_content", "", "pivot_page", header, OATdata, mainlist, sublist, [], header.length - 1, { showChart: 0, showEmpty: 0, externalSort: true });
}

function LanguageHandling() {
    for (var i = 0; i < document.getElementsByTagName("a").length; i++) {
        if (document.getElementsByTagName("a")[i].id == "aView") {
            document.getElementsByTagName("a")[i].innerHTML = z('hView').value;
        }

        if (document.getElementsByTagName("a")[i].id == "aEdit") {
            document.getElementsByTagName("a")[i].innerHTML = z('hEdit').value;
        }

        if (document.getElementsByTagName("a")[i].id == "aDelete") {
            document.getElementsByTagName("a")[i].innerHTML = z('hDelete').value;
        }

        if (document.getElementsByTagName("a")[i].id == "aDownload") {
            document.getElementsByTagName("a")[i].innerHTML = z('hDownload').value;
        }
    }

    for (var i = 0; i < document.getElementsByTagName("span").length; i++) {
        if (document.getElementsByTagName("span")[i].id == "spanIndicator") {
            document.getElementsByTagName("span")[i].innerHTML = z('hIndicator').value;
        }

        //        if (document.getElementsByTagName("span")[i].id == "spanLanguageName") {
        //            document.getElementsByTagName("span")[i].innerHTML = z('hLanguage').value;
        //        }

        if (document.getElementsByTagName("span")[i].id == "spanData") {
            document.getElementsByTagName("span")[i].innerHTML = z('hData').value;
        }

        if (document.getElementsByTagName("span")[i].id == "spanMetadata") {
            document.getElementsByTagName("span")[i].innerHTML = z('hMetadata').value;
        }
    }
}

function CallBack()
{ }


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
    var DBOrDSDDBNId;

    ClearSelections();
    if (selectAction.value == "0") {
        // None selected
        spanGenerateSDMXMLDescription.style.display = "none";
        spanRegisterSDMXMLDescription.style.display = "none";
        spanGenerateMetadataDescription.style.display = "none";
        spanRegisterMetadataDescription.style.display = "none";

        rowGenerateSDMXML.style.display = "none";

        rowGenerateMetadata.style.display = "none";
        tblAction.style.display = "none";
        z('divIndicatorGrid').style.display = "none";
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
                rowGenerateMetadata.style.display = "none";
                tblAction.style.display = "block";
                if (document.getElementById('hdnDSDORDB').value != "DB") {
                    rowIndicatorSelection.style.display = "none";
                    rowAreaSelection.style.display = "none";
                    rowTPSelection.style.display = "none";
                    rowSourceSelection.style.display = "none";
                    DBOrDSDDBNId = SelectedDBOrDSDNId;
                    GetIndicatorDivInnerHTML(DBOrDSDDBNId);
                    z('divIndicatorGrid').style.width = "70%";
                    z('divIndicatorGrid').style.height = "200px";
                   // z('divIndicatorGrid').style.overflowY = "scroll";
                    z('btnSave').style.display = 'block';
                }
                else {
                    z('divIndicatorGrid').style.display = "none";
                }
            }
            else if (selectAction.value == "3") {
                // Generate Metadata Report
                spanGenerateSDMXMLDescription.style.display = "none";
                spanRegisterSDMXMLDescription.style.display = "none";
                spanGenerateMetadataDescription.style.display = "inline";
                spanRegisterMetadataDescription.style.display = "none";
                rowGenerateSDMXML.style.display = "none";
                rowGenerateMetadata.style.display = "inline";
                tblAction.style.display = "block";
                if (SelectedDBOrDSDNId.split('|')[1] == 'true') {
                    rowIndicatorSelection.style.display = "block";
                    rowAreaSelection.style.display = "block";
                    rowTPSelection.style.display = "none";
                    rowSourceSelection.style.display = "block";
                }
                else {
                    rowIndicatorSelection.style.display = "none";
                    rowAreaSelection.style.display = "none";
                    rowTPSelection.style.display = "none";
                    rowSourceSelection.style.display = "none";
                    z('btnSaveMetadata').style.display = 'none';
                    DBOrDSDDBNId = SelectedDBOrDSDNId;
                    GetIndicatorDivInnerHTML(DBOrDSDDBNId);
                    z('divIndicatorGrid').style.width = "60%";
                    z('divIndicatorGrid').style.height = "200px";
                    z('tblIATSGrid').style.width = '100%'
                }
            }

        }
    }
}

function OpenIndicatorPopup() {
    var DBOrDSDDBNId;
    ApplyMaskingDiv();
    ShowLoadingDiv();

    DBOrDSDDBNId = GetSelectedDBOrDSDDBNId();

    SetCloseButtonInPopupDiv(di_jq('#IndicatorPopup'), 'IndicatorPopupCancel', '../../stock/themes/default/images/close.png');
    di_jq('#IndicatorPopup').show('slow');

    if (document.getElementById('hdnDSDORDB').value == 'DB') {
        GetIndicatorDivInnerHTML(DBOrDSDDBNId);
    }
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
    z('divIndicatorGrid').innerHTML = '';

    InputParam = DBOrDSDDBNId;
    InputParam += '|' + z('selectAction').value;
    InputParam += '|' + z('hLoggedInUserNId').value.split('|')[0];

    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '110', 'param1': InputParam },
            async: false,
            success: function (data) {
                try {
                    if (data != null && data != '') {

                        if (document.getElementById('hdnDSDORDB').value == 'DB') {
                            z('divIndicatorGrid').style.display = "none";
                            z('divIndicator').style.display = "block";
                            z('divIndicator').innerHTML = data;
                            z('spanIndicator_0').innerHTML = z('hSelectAll').value;
                            z('divIndicatorGrid').style.display = "none";
                        }
                        else {
                            z('divIndicatorGrid').style.display = "block";
                            if (data.indexOf("[[**end**]]") != -1) {
                                var divData = data.split("[[**end**]]");
                                var parsedJSON = JSON.parse(divData[1]);
                                var selectCount = parsedJSON.Data;
                                for (i = 0; i < selectCount.length; i++) {

                                    CreateNUpdateSelections(selectCount[i].Ind, selectCount[i].areas, selectCount[i].timeperiods, selectCount[i].source);
                                }

                                z('divIndicatorGrid').innerHTML = divData[0];
                            }
                            else {
                                z('divIndicatorGrid').innerHTML = data;
                            }
                        }

                    }

                    for (i = 0; i < z('hdnSelectedIndicators').value.split(',').length; i++) {
                        if (z('chkIndicator_' + z('hdnSelectedIndicators').value.split(',')[i]) != null) {
                            //z('chkIndicator_' + z('hdnSelectedIndicators').value.split(',')[i]).checked = true;
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

function SelectUnselectAllIndicators() {
    if (document.getElementById('hdnDSDORDB') == "DB") {
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
    else {
        var tblIndicator = z('tblIATSGrid');
        for (i = 1; i < tblIndicator.rows.length; i++) {
            if (di_jq('#tblIATSGrid').find("#chkIndicator_0").is(':checked')) {
                tblIndicator.rows[i].cells[0].childNodes[0].checked = true;
            }
            else {
                tblIndicator.rows[i].cells[0].childNodes[0].checked = false;
            }
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


//Area

function OpenAreaPopup(object) {
    var DBOrDSDDBNId;
    var SelectedObjectId = '';
    var SelectedIndicatorNId = '';
    var SelectedAreaNIds = '';

    ApplyMaskingDiv();
    ShowLoadingDiv();

    DBOrDSDDBNId = GetSelectedDBOrDSDDBNId();
    if (document.getElementById('hdnDSDORDB').value != "DB") {
        z('hdnSelectedAreas').value = '';
        SelectedObjectId = object.id.split('_');
        SelectedIndicatorNId = SelectedObjectId[1];
        SelectedAreaNIds = di_jq(object).attr("rel");
        //        if (SelectedAreaNIds == false) {
        //            SelectedAreaNIds = z('hdnSelectedAreas').value;

        //        }
    }

    SetCloseButtonInPopupDiv(di_jq('#AreaPopup'), 'AreaPopupCancel', '../../stock/themes/default/images/close.png');
    di_jq('#AreaPopup').show('slow');
    di_jq('#AreaPopup').attr("rel", SelectedIndicatorNId);
    GetAreaDivInnerHTML(DBOrDSDDBNId, SelectedAreaNIds, SelectedIndicatorNId);
    GetWindowCentered(z("AreaPopup"), 710, 520);


}

function RemoveAreas(object) {
    var selectedValues = "";
    SelectedObjectId = object.id.split('_');
    SelectedIndicatorNId = SelectedObjectId[1];
    SaveFilters(SelectedIndicatorNId, "Areas", selectedValues);
    di_jq(object).parent().find('b').contents().unwrap().wrap('');
    DBOrDSDDBNId = GetSelectedDBOrDSDDBNId();
    GetIndicatorDivInnerHTML(DBOrDSDDBNId);
}

function RemoveTimeperiods(object) {
    var selectedValues = "";
    SelectedObjectId = object.id.split('_');
    SelectedIndicatorNId = SelectedObjectId[1];
    SaveFilters(SelectedIndicatorNId, "TimePeriods", selectedValues);
    di_jq(object).parent().find('b').contents().unwrap().wrap('');
    DBOrDSDDBNId = GetSelectedDBOrDSDDBNId();
    GetIndicatorDivInnerHTML(DBOrDSDDBNId);
}

function RemoveSource(object) {
    var selectedValues = "";
    SelectedObjectId = object.id.split('_');
    SelectedIndicatorNId = SelectedObjectId[1];
    SaveFilters(SelectedIndicatorNId, "Source", selectedValues);
    di_jq(object).parent().find('b').contents().unwrap().wrap('');
    DBOrDSDDBNId = GetSelectedDBOrDSDDBNId();
    GetIndicatorDivInnerHTML(DBOrDSDDBNId);
}


function AreaPopupOk() {
    var SelectedText = "";
    var tblArea = z('tblArea');
    var SelectedIndicatorNId = di_jq('#AreaPopup').attr("rel");
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
    if (document.getElementById('hdnDSDORDB').value == "DB") {
        if (SelectedText != null && SelectedText != '') {
            SelectedText = SelectedText.substr(0, SelectedText.length - 1);
        }

        SetSelectedText(SelectedText, 'spanSelectedAreas');
    } else {
        //CreateNUpdateSelections(SelectedIndicatorNId, z('hdnSelectedAreas').value, z('hdnSelectedTPs').value, z('hdnSelectedSources').value);
        SaveFilters(SelectedIndicatorNId, "Areas", z('hdnSelectedAreas').value);

        if (SelectedText != null && SelectedText != '') {
            SelectedText = SelectedText.substr(0, SelectedText.length - 1);
        }
        var spanId = 'spanArea_' + SelectedIndicatorNId;
        SetSelectedTextToolTip(SelectedText, spanId);
        di_jq(spanId).find('Select').contents().unwrap().wrap('<b>');
        AreaGenSelections += "[**[R]**]" + "Indicator_" + SelectedIndicatorNId + "[**[C]**]" + "Area_" + z('hdnSelectedAreas').value;
    }
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

function GetAreaDivInnerHTML(DBOrDSDDBNId, SelectedAreaNIds, SelectedIndicatorNId) {
    var InputParam; var currentIndId = '';
    z('divArea').innerHTML = '';
    currentIndId = di_jq('#AreaPopup').attr("rel");
    InputParam = DBOrDSDDBNId;
    InputParam += '|' + z('selectAction').value;
    InputParam += '|' + SelectedIndicatorNId;
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
                    if (document.getElementById('hdnDSDORDB').value != "DB") {
                        //                        z('hdnSelectedAreas').value = SelectedAreaNIds;
                        //                        if (SelectedAreaNIds.indexOf(",") == "-1" && currentIndId == SelectedIndicatorNId) {
                        //                            if (z('chkArea_' + parseInt(SelectedAreaNIds)) != null) {
                        //                                z('chkArea_' + parseInt(SelectedAreaNIds)).checked = true;
                        //                            }
                        //                        }
                        //                        else {
                        //                            for (i = 0; i < SelectedAreaNIds.split(',').length; i++) {
                        //                                if (z('chkArea_' + SelectedAreaNIds.split(',')[i]) != null) {
                        //                                    di_jq('#chkArea_' + SelectedAreaNIds.split(',')[i]).checked = true;
                        //                                }
                        //                            }
                        //                        }
                    }
                    else {
                        for (i = 0; i < z('hdnSelectedAreas').value.split(',').length; i++) {
                            if (z('chkArea_' + z('hdnSelectedAreas').value.split(',')[i]) != null) {
                                z('chkArea_' + z('hdnSelectedAreas').value.split(',')[i]).checked = true;
                            }
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

//Time

function OpenTPPopup(object) {
    var DBOrDSDDBNId;
    var SelectedObjectId = '';
    var SelectedIndicatorNId = '';
    var SelectedTimeNIds = '';
    ApplyMaskingDiv();
    ShowLoadingDiv();

    DBOrDSDDBNId = GetSelectedDBOrDSDDBNId();
    if (document.getElementById('hdnDSDORDB').value != "DB") {
        z('hdnSelectedTPs').value = '';
        SelectedObjectId = object.id.split('_');
        SelectedIndicatorNId = SelectedObjectId[1];
        SelectedTimeNIds = di_jq(object).attr("rel");
        //        if (SelectedTimeNIds == false) {
        //            SelectedTimeNIds = z('hdnSelectedTPs').value;

        //        }
        //        AreaSelections['Ind_' + SelectedIndicatorNId] = { "Data": [{ "@Ind": SelectedIndicatorNId, "@areas": z('hdnSelectedAreas').value, "@timeperiods": SelectedTimeNIds, "@source": z('hdnSelectedSources').value}] };


        //  CreateNUpdateSelections(SelectedIndicatorNId, z('hdnSelectedAreas').value, SelectedTimeNIds, z('hdnSelectedSources').value);
    }
    SetCloseButtonInPopupDiv(di_jq('#TPPopup'), 'TPPopupCancel', '../../stock/themes/default/images/close.png');
    di_jq('#TPPopup').show('slow');
    di_jq('#TPPopup').attr("rel", SelectedIndicatorNId);

    GetTPDivInnerHTML(DBOrDSDDBNId, SelectedTimeNIds, SelectedIndicatorNId);
    GetWindowCentered(z("TPPopup"), 710, 520);

}



function TPPopupOk() {
    var SelectedText = "";
    var tblTP = z('tblTP');

    z('hdnSelectedTPs').value = '';

    var SelectedIndicatorNId = di_jq('#TPPopup').attr("rel");

    for (i = 1; i < tblTP.rows.length; i++) {
        if (tblTP.rows[i].cells[0].childNodes[0].checked == true) {
            z('hdnSelectedTPs').value += tblTP.rows[i].cells[0].childNodes[0].value + ',';
            SelectedText += tblTP.rows[i].cells[1].childNodes[0].innerHTML + ",";
        }
    }

    if (z('hdnSelectedTPs').value != null && z('hdnSelectedTPs').value != '') {
        z('hdnSelectedTPs').value = z('hdnSelectedTPs').value.substr(0, z('hdnSelectedTPs').value.length - 1);

        //        AreaSelections['Ind_' + SelectedIndicatorNId] = { "Data": [{ "@Ind": SelectedIndicatorNId, "@areas": z('hdnSelectedAreas').value, "@timeperiods": z('hdnSelectedTPs').value, "@source": z('hdnSelectedSources').value}] };


        //        CreateNUpdateSelections(SelectedIndicatorNId, z('hdnSelectedAreas').value, z('hdnSelectedTPs').value, z('hdnSelectedSources').value);

    }
    if (document.getElementById('hdnDSDORDB').value == "DB") {
        if (SelectedText != null && SelectedText != '') {
            SelectedText = SelectedText.substr(0, SelectedText.length - 1);
        }

        SetSelectedText(SelectedText, 'spanSelectedTPs');
    } else {
        //CreateNUpdateSelections(SelectedIndicatorNId, z('hdnSelectedAreas').value, z('hdnSelectedTPs').value, z('hdnSelectedSources').value);
        SaveFilters(SelectedIndicatorNId, "TimePeriods", z('hdnSelectedTPs').value);

        if (SelectedText != null && SelectedText != '') {
            SelectedText = SelectedText.substr(0, SelectedText.length - 1);
        }
        var spanId = 'spanTime_' + SelectedIndicatorNId;
        SetSelectedTextToolTip(SelectedText, spanId);
        TimeSelections += "[**[R]**]" + "Indicator_" + SelectedIndicatorNId + "[**[C]**]" + "Time_" + z('hdnSelectedTPs').value;
    }
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


function GetTPDivInnerHTML(DBOrDSDDBNId, SelectedTimeNIds, SelectedIndicatorNId) {
    var InputParam;
    z('divTP').innerHTML = '';
    InputParam = DBOrDSDDBNId;
    InputParam += '|' + SelectedIndicatorNId;
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
                    if (document.getElementById('hdnDSDORDB').value != "DB") {
                        //                        z('hdnSelectedTPs').value = SelectedTimeNIds;
                        //                        if (SelectedTimeNIds.indexOf(",") == "-1") {
                        //                            if (z('chkTP_' + parseInt(SelectedTimeNIds)) != null) {
                        //                                z('chkTP_' + parseInt(SelectedTimeNIds)).checked = true;
                        //                            }
                        //                        }
                        //                        else {

                        //                            for (i = 0; i < SelectedTimeNIds.split(',').length; i++) {
                        //                                if (z('chkTP_' + parseInt(SelectedTimeNIds.split(',')[i])) != null) {
                        //                                    z('chkTP_' + parseInt(SelectedTimeNIds.split(',')[i])).checked = true;
                        //                                }
                        //                            }
                        //                        }
                    }
                    else {
                        for (i = 0; i < z('hdnSelectedTPs').value.split(',').length; i++) {
                            if (z('chkTP_' + z('hdnSelectedTPs').value.split(',')[i]) != null) {
                                z('chkTP_' + z('hdnSelectedTPs').value.split(',')[i]).checked = true;
                            }
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


//Source
function OpenSourcePopup(object) {
    var DBOrDSDDBNId;
    var SelectedObjectId = '';
    var SelectedIndicatorNId = '';
    var SelectedSourceNIds = '';
    ApplyMaskingDiv();
    ShowLoadingDiv();

    DBOrDSDDBNId = GetSelectedDBOrDSDDBNId();
    if (document.getElementById('hdnDSDORDB').value != "DB") {
        z('hdnSelectedSources').value = '';
        SelectedObjectId = object.id.split('_');
        SelectedIndicatorNId = SelectedObjectId[1];
        SelectedSourceNIds = di_jq(object).attr("rel");

        //        if (SelectedSourceNIds==false) {
        //            SelectedSourceNIds = z('hdnSelectedSources').value;
        //            
        //        }
        //      

        //        AreaSelections['Ind_' + SelectedIndicatorNId] = { "Data": [{ "@Ind": SelectedIndicatorNId, "@areas": z('hdnSelectedAreas').value, "@timeperiods": z('hdnSelectedTPs').value, "@source": SelectedSourceNIds}] };

        //CreateNUpdateSelections(SelectedIndicatorNId, z('hdnSelectedAreas').value, z('hdnSelectedTPs').value, SelectedSourceNIds);
    }
    SetCloseButtonInPopupDiv(di_jq('#SourcePopup'), 'SourcePopupCancel', '../../stock/themes/default/images/close.png');
    di_jq('#SourcePopup').show('slow');
    di_jq('#SourcePopup').attr("rel", SelectedIndicatorNId);

    GetSourceDivInnerHTML(DBOrDSDDBNId, SelectedSourceNIds, SelectedIndicatorNId);
    GetWindowCentered(z("SourcePopup"), 710, 520);
}

function SourcePopupOk() {
    var SelectedText = "";
    var tblSource = z('tblSource');
    z('hdnSelectedSources').value = '';
    var SelectedIndicatorNId = di_jq('#SourcePopup').attr("rel");
    for (i = 1; i < tblSource.rows.length; i++) {
        if (tblSource.rows[i].cells[0].childNodes[0].checked == true) {
            z('hdnSelectedSources').value += tblSource.rows[i].cells[0].childNodes[0].value + ',';
            SelectedText += tblSource.rows[i].cells[1].childNodes[0].innerHTML + ",";
        }
    }

    if (z('hdnSelectedSources').value != null && z('hdnSelectedSources').value != '') {
        z('hdnSelectedSources').value = z('hdnSelectedSources').value.substr(0, z('hdnSelectedSources').value.length - 1);

        //        AreaSelections['Ind_' + SelectedIndicatorNId] = { "Data": [{ "@Ind": SelectedIndicatorNId, "@areas": z('hdnSelectedAreas').value, "@timeperiods": z('hdnSelectedTPs').value, "@source": z('hdnSelectedSources').value}] };

    }
    if (document.getElementById('hdnDSDORDB').value == "DB") {
        if (SelectedText != null && SelectedText != '') {
            SelectedText = SelectedText.substr(0, SelectedText.length - 1);
        }

        SetSelectedText(SelectedText, 'spanSelectedSources');
    }
    else {
        // CreateNUpdateSelections(SelectedIndicatorNId, z('hdnSelectedAreas').value, z('hdnSelectedTPs').value, z('hdnSelectedSources').value);
        SaveFilters(SelectedIndicatorNId, "Source", z('hdnSelectedSources').value);

        if (SelectedText != null && SelectedText != '') {
            SelectedText = SelectedText.substr(0, SelectedText.length - 1);
        }
        var spanId = 'spanSource_' + SelectedIndicatorNId;
        SetSelectedTextToolTip(SelectedText, spanId);
        SourceSelections += "[**[R]**]" + "Indicator_" + SelectedIndicatorNId + "[**[C]**]" + "Source_" + z('hdnSelectedSources').value;

    }


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


function GetSourceDivInnerHTML(DBOrDSDDBNId, SelectedSourceNIds, SelectedIndicatorNId) {
    var InputParam;

    z('divSource').innerHTML = '';

    InputParam = DBOrDSDDBNId;
    InputParam += '|' + z('selectAction').value;
    InputParam += '|' + SelectedIndicatorNId;
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

                    if (document.getElementById('hdnDSDORDB').value != "DB") {
                        //                        z('hdnSelectedSources').value = SelectedSourceNIds;
                        //                        if (SelectedSourceNIds.indexOf(",") == "-1") {
                        //                            if (z('chkSource_' + parseInt(SelectedSourceNIds)) != null) {
                        //                                z('chkSource_' + parseInt(SelectedSourceNIds)).checked = true;
                        //                            }
                        //                        }
                        //                        else {
                        //                            for (i = 0; i < SelectedSourceNIds.split(',').length; i++) {
                        //                                if (z('chkSource_' + parseInt(SelectedSourceNIds.split(',')[i])) != null) {
                        //                                    z('chkSource_' + parseInt(SelectedSourceNIds.split(',')[i])).checked = true;
                        //                                }
                        //                            }
                        //                        }
                    }
                    else {
                        for (i = 0; i < z('hdnSelectedTPs').value.split(',').length; i++) {
                            if (z('chkTP_' + z('hdnSelectedSources').value.split(',')[i]) != null) {
                                z('chkTP_' + z('hdnSelectedSources').value.split(',')[i]).checked = true;
                            }
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


function GenerateMetadata() {
    var InputParam;

    ApplyMaskingDiv();
    ShowLoadingDiv();
    if (document.getElementById('hdnDSDORDB').value != "DB") {
        var tblIndicator = z('tblIATSGrid');
        z('hdnSelectedIndicators').value = '';

        for (i = 1; i < tblIndicator.rows.length; i++) {

            if (tblIndicator.rows[i].cells[0].childNodes[0].checked == true) {
                AreaSelections['Indicator' + i] = { "Data": [{ "@Ind": z('hdnSelectedIndicators').value}] };
                z('hdnSelectedIndicators').value += tblIndicator.rows[i].cells[0].childNodes[0].value + ',';
            }
        }

        if (z('hdnSelectedIndicators').value != null && z('hdnSelectedIndicators').value != '') {
            z('hdnSelectedIndicators').value = z('hdnSelectedIndicators').value.substr(0, z('hdnSelectedIndicators').value.length - 1);
        }

    }
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
                //  

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
                    else if (ErrMsg == "ANF" && Status == "false") {
                        //  alert(z('hdnArtifactNotFound').value);
                        alert("Artifact not found");
                    }
                    else if (Status == "true") {
                        alert('Metadata generated successfully.');
                        GetRegistrationsSummary(z('hdbnid').value, z('hLoggedInUserNId').value.split('|')[0], z('hlngcodedb').value);

                    }
                    else {
                        alert('Error in  generating.');
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
                //   

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
                //  

                HideLoadingDiv();
                RemoveMaskingDiv();
            },
            cache: false
        });
    }
    catch (ex) {
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
    var ErrorLog = z('lnkErrorLog');

    if (document.getElementById('hdnDSDORDB').value != "DB") {
        var tblIndicator = z('tblIATSGrid');
        z('hdnSelectedIndicators').value = '';

        for (i = 1; i < tblIndicator.rows.length; i++) {

            if (tblIndicator.rows[i].cells[0].childNodes[0].checked == true) {
                z('hdnSelectedIndicators').value += tblIndicator.rows[i].cells[0].childNodes[0].value + ',';
            }
        }

        if (z('hdnSelectedIndicators').value != null && z('hdnSelectedIndicators').value != '') {
            z('hdnSelectedIndicators').value = z('hdnSelectedIndicators').value.substr(0, z('hdnSelectedIndicators').value.length - 1);
        }

    }



    if (z('hdnSelectedIndicators').value != null && z('hdnSelectedIndicators').value != '') {
        InputParam = GetSelectedDBOrDSDDBNId();
        InputParam += '|' + z('hdnSelectedIndicators').value;
        InputParam += '|' + z('hdnSelectedAreas').value;
        InputParam += '|' + z('hdnSelectedTPs').value;
        InputParam += '|' + z('hdnSelectedSources').value;
        InputParam += '|' + z('hdnDBAreaId').value;
        InputParam += '|' + z('hLoggedInUserNId').value.split('|')[0];
        InputParam += '|' + z('hOriginaldbnid').value;
        InputParam += '|' + AreaGenSelections + TimeSelections + SourceSelections;
        try {
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: CallbackPageName,
                data: { 'callback': '113', 'param1': InputParam },
                async: true,
                success: function (data) {
                    try {
                        if (data.length > 0) {
                            ErrorLogInPublishData = '';
                            ErrorNDFIndicatorGIDS = '';
                            DSDORDB = z('hdnDSDORDB').value;

                            ArrMsgNFileCount = data.split('[****]');
                            Status = ArrMsgNFileCount[0];
                            FileCount = ArrMsgNFileCount[1];
                            if (isNaN(FileCount)) {
                                ValidMsg = ArrMsgNFileCount[1];
                            }


                            if (ArrMsgNFileCount.length > 2) {
                                for (i = 2; i < ArrMsgNFileCount.length; i += 2) {
                                    ValidMsg = ArrMsgNFileCount[i];
                                    ErrorLogMessage = ArrMsgNFileCount[i + 1];
                                    if (ValidMsg == "MNF") {
                                        alert(z('hdnMappingNotFound').value);
                                        ErrorLogInPublishData += "No mapping found for selected indicators";
                                    }
                                    else if (ValidMsg == "NDF") {
                                        if (ArrMsgNFileCount[i + 1] == "NDF" && ErrorLogInPublishData == '') {
                                            alert(z('hdnNoDataFound').value);
                                            ErrorLog.style.display = "none";
                                        }
                                        else {
                                            ErrorLog.style.display = "block";
                                            ErrorLogInPublishData += " " + "No Data found for selected indicators ";
                                            ErrorLogInPublishData += ":" + ArrMsgNFileCount[i + 1];
                                            ErrorLogInPublishData += "<br/>";
                                        }
                                    }
                                    else if (ValidMsg == "DK") {
                                        ErrorLog.style.display = "block";
                                        ErrorLogInPublishData += " " + "Multiple source found for ";
                                        var IndicatorTimePeriod = ArrMsgNFileCount[i + 1];
                                        var SplitGIDS = '';
                                        if (IndicatorTimePeriod.indexOf("__@@@@__") != -1) {
                                            var divData = IndicatorTimePeriod.split("__@@@@__");
                                            var Indicator = divData[0];
                                            var TimePeriod = divData[1];
                                            SplitGIDS = Indicator + " " + TimePeriod;
                                        }
                                        ErrorLogInPublishData += " " + SplitGIDS;
                                        ErrorLogInPublishData += "<br/>";
                                    }


                                }
                            }
                            if (FileCount != 0 && Status == "true") {
                                alert(FileCount + " " + "SDMX-ML Message(s) generated successfully.");
                                GetRegistrationsSummary(z('hdbnid').value, z('hLoggedInUserNId').value.split('|')[0], z('hlngcodedb').value);
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
                    //   

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
                //    

                HideLoadingDiv();
                RemoveMaskingDiv();
            },
            cache: false
        });
    }
    catch (ex) {
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

function SetSelectedTextToolTip(text, spanId) {
    // z(spanId).innerHTML = '';
    z(spanId).title = '';

    if (text.length > 100) {
        // z(spanId).innerHTML = '&nbsp;';
        z(spanId).title = text;
    }
    else {
        //  z(spanId).innerHTML = '&nbsp;';
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

function SaveSelection() {
    var selectedValues = '';
    var tblIndicator = z('tblIATSGrid');
    z('hdnSelectedIndicators').value = '';
    for (i = 1; i < tblIndicator.rows.length; i++) {
        if (tblIndicator.rows[i].cells[0].childNodes[0].checked == true) {
            z('hdnSelectedIndicators').value += tblIndicator.rows[i].cells[0].childNodes[0].value + ',';
        }
    }

    if (z('hdnSelectedIndicators').value != null && z('hdnSelectedIndicators').value != '') {
        z('hdnSelectedIndicators').value = z('hdnSelectedIndicators').value.substr(0, z('hdnSelectedIndicators').value.length - 1);
    }

    if (z('hdnSelectedIndicators').value != null && z('hdnSelectedIndicators').value != '') {
        InputParam = GetSelectedDBOrDSDDBNId();
        InputParam += '|' + z('hdnSelectedIndicators').value;
        InputParam += '|' + selectedValues;
        InputParam += '|' + z('hdnDBAreaId').value;
        InputParam += '|' + z('hLoggedInUserNId').value.split('|')[0];
        InputParam += '|' + "Indicators";
        try {
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: CallbackPageName,
                data: { 'callback': '1048', 'param1': InputParam },
                async: false,
                success: function (data) {
                    try {
                        if (data == "true") {
                            alert("Selection Saved");
                            DBOrDSDDBNId = GetSelectedDBOrDSDDBNId();
                            GetIndicatorDivInnerHTML(DBOrDSDDBNId);
                        }
                        else {
                            alert("There was an error while saving. Kindly try again.");
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
        alert('Select one indicator atleast!');
    }
}

function SaveFilters(selectedIndicator, selectedFilterType, selectedValues) {
    InputParam = GetSelectedDBOrDSDDBNId();
    InputParam += '|' + selectedIndicator;
    InputParam += '|' + selectedValues;
    InputParam += '|' + z('hdnDBAreaId').value;
    InputParam += '|' + z('hLoggedInUserNId').value.split('|')[0];
    InputParam += '|' + selectedFilterType;
    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '1048', 'param1': InputParam },
            async: false,
            success: function (data) {
                try {
                    if (data == "true") {

                    }
                    else {

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

function SaveMetadataSelection() {
    //Adding selected indicators to the grid
    var tblIndicator = z('tblIATSGrid');
    z('hdnSelectedIndicators').value = '';
    for (i = 1; i < tblIndicator.rows.length; i++) {

        if (tblIndicator.rows[i].cells[0].childNodes[0].checked == true) {
            //  AreaSelections['Ind' + i] = { "Data": [{ "@Ind": tblIndicator.rows[i].cells[0].childNodes[0].value}] };
            z('hdnSelectedIndicators').value += tblIndicator.rows[i].cells[0].childNodes[0].value + ',';

            CreateNUpdateSelections(tblIndicator.rows[i].cells[0].childNodes[0].value, '', '', '');
        }
    }
    // AreaSelections = { "?xml": { "@version": "1.0", "@standalone": "no" }, "root": AreaSelections };


    if (z('hdnSelectedIndicators').value != null && z('hdnSelectedIndicators').value != '') {
        z('hdnSelectedIndicators').value = z('hdnSelectedIndicators').value.substr(0, z('hdnSelectedIndicators').value.length - 1);
    }

    if (z('hdnSelectedIndicators').value != null && z('hdnSelectedIndicators').value != '') {
        InputParam = GetSelectedDBOrDSDDBNId();
        InputParam += '|' + z('hdnSelectedIndicators').value;
        InputParam += '|' + z('hdnDBAreaId').value;
        InputParam += '|' + z('hLoggedInUserNId').value.split('|')[0];
        InputParam += '|' + JSON.stringify(AreaSelections);
        try {
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: CallbackPageName,
                data: { 'callback': '1049', 'param1': InputParam },
                async: false,
                success: function (data) {
                    try {
                        if (data == "true") {
                            AreaSelections = {};
                            alert("Selection Saved");
                            DBOrDSDDBNId = GetSelectedDBOrDSDDBNId();
                            GetIndicatorDivInnerHTML(DBOrDSDDBNId);
                        }
                        else {
                            alert("There was an error while saving. Kindly try again.");
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
        alert('Select one indicator atleast!');
    }
}



function CreateNUpdateSelections(IndicatorNId, AreaNids, TimePeriodNids, SourceNids) {
    if (AreaSelections['Ind_' + IndicatorNId] != undefined) {
        AreaSelections['Ind_' + IndicatorNId].Data[0]["@Ind"] = IndicatorNId;
        AreaSelections['Ind_' + IndicatorNId].Data[0]["@areas"] = AreaNids;
        AreaSelections['Ind_' + IndicatorNId].Data[0]["@timeperiods"] = TimePeriodNids;
        AreaSelections['Ind_' + IndicatorNId].Data[0]["@source"] = SourceNids;
    }
    else {

        AreaSelections['Ind_' + IndicatorNId] = { "Data": [{ "@Ind": IndicatorNId, "@areas": AreaNids, "@timeperiods": TimePeriodNids, "@source": SourceNids}] };

    }
}

function OpenErrorLog() {
    var Message = '';
 
    SetCloseButtonInPopupDiv(di_jq('#ErrorPopup'), 'ErrorPopupCancel', '../../stock/themes/default/images/close.png');
    di_jq('#ErrorPopup').show('slow');

    GetWindowCentered(z("ErrorPopup"), 510, 320);
    z('divErrorLog').style.display = "block";
    di_jq('#divErrorLog').html(ErrorLogInPublishData);
}

function ErrorPopupCancel() {
    HideLoadingDiv();
    RemoveMaskingDiv();

    di_jq('#ErrorPopup').hide('slow');
}