// JScript File

var DiscoverRegistrationsCallBackPageName = "Callback.aspx";

function onPageLoad(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hLoggedInUserNId, hLoggedInUserName, hOriginaldbnid) {
    var hsgcount = 10;
    // ************************************************
    // create Form Tag with hidden input boxes
    // ************************************************
    createFormHiddenInputs("frm_sdmxData", "POST");
    SetCommonLinksHref("frm_sdmxData", "POST");
    SetOriginaldbnidInForm("frm_sdmxData", hOriginaldbnid);

    // ************************************************1
    // set page level variables based on selections or defaults
    // ************************************************1
    setPostedData(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hsgcount, hLoggedInUserNId, hLoggedInUserName);

    // ************************************************1
    // Load Language Component
    // ************************************************1
    ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', hlngcode);

    SetCommonRegistryPageDetails('RegData.aspx', "frm_sdmxData", "POST");
    //document.getElementById('selViewRegistrationsBy').selectedIndex = 3;
    BindRegistrations();

}


function SetHiddenFieldValues() {
    for (var i = 0; i < document.getElementsByName('lang_View').length; i++) {
        document.getElementsByName('lang_View')[i].innerHTML = document.getElementById('hView').value;
    }

    for (var i = 0; i < document.getElementsByName('lang_Download').length; i++) {
        document.getElementsByName('lang_Download')[i].innerHTML = document.getElementById('hDownload').value;
    }

    for (var i = 0; i < document.getElementsByTagName('span').length; i++) {
        if (document.getElementsByTagName("span")[i].id.indexOf("langReg_Id") != -1) {
            document.getElementsByTagName("span")[i].innerHTML = z('hId').value;
        }

        if (document.getElementsByTagName("span")[i].id.indexOf("lang_Queryable_Data") != -1) {
            document.getElementsByTagName("span")[i].innerHTML = z('hQueryableData').value;
        }

        if (document.getElementsByTagName("span")[i].id.indexOf("lang_WADL") != -1) {
            document.getElementsByTagName("span")[i].innerHTML = z('hWADL').value;
        }

        if (document.getElementsByTagName("span")[i].id.indexOf("lang_WSDL") != -1) {
            document.getElementsByTagName("span")[i].innerHTML = z('hWSDL').value;
        }

        if (document.getElementsByTagName("span")[i].id.indexOf("lang_Simple_Data") != -1) {
            document.getElementsByTagName("span")[i].innerHTML = z('hSimpleData').value;
        }

        if (document.getElementsByTagName("span")[i].id.indexOf("lang_Data") != -1) {
            document.getElementsByTagName("span")[i].innerHTML = z('hData').value;
        }
        if (document.getElementsByTagName("span")[i].id.indexOf("lang_Metadata") != -1) {
            document.getElementsByTagName("span")[i].innerHTML = z('hMetadata').value;
        }

        if (document.getElementsByTagName("span")[i].id.indexOf("lang_Constraints") != -1) {
            document.getElementsByTagName("span")[i].innerHTML = z('hConstraints').value;
        }

        if (document.getElementsByTagName("span")[i].id.indexOf("lang_Provision_Agreement") != -1) {
            document.getElementsByTagName("span")[i].innerHTML = z('hProvisionAgreement').value;
        }

        if (document.getElementsByTagName("span")[i].id.indexOf("lang_Data_Provider") != -1) {
            document.getElementsByTagName("span")[i].innerHTML = z('hDataProvider').value;
        }

        if (document.getElementsByTagName("span")[i].id.indexOf("lang_Provider") != -1) {
            document.getElementsByTagName("span")[i].innerHTML = z('hProvider').value;
        }

        if (document.getElementsByTagName("span")[i].id.indexOf("lang_DFD_MFD") != -1) {
            document.getElementsByTagName("span")[i].innerHTML = z('hDFDMFD').value;
        }

        if (document.getElementsByTagName("span")[i].id.indexOf("lang_DFD_MFD_Short_Form") != -1) {
            document.getElementsByTagName("span")[i].innerHTML = z('hDFDMFDShortForm').value;
        }

        if (document.getElementsByTagName("span")[i].id.indexOf("lang_No_Registration_Found") != -1) {
            document.getElementsByTagName("span")[i].innerHTML = z('hNoRegistrationFound').value;
        }
        if (document.getElementsByTagName("span")[i].id.indexOf("lang_data_Registration") != -1) {
            document.getElementsByTagName("span")[i].innerHTML = z('hDataRegistration').value;
        }
        if (document.getElementsByTagName("span")[i].id.indexOf("lang_metadata_Registration") != -1) {
            document.getElementsByTagName("span")[i].innerHTML = z('hMetadataRegistration').value;
        }
        if (document.getElementsByTagName("span")[i].id.indexOf("lang_metadata_Registration_Area") != -1) {
            document.getElementsByTagName("span")[i].innerHTML = z('hMetadataRegistrationArea').value;
        }
        if (document.getElementsByTagName("span")[i].id.indexOf("lang_metadata_Registration_Indicator") != -1) {
            document.getElementsByTagName("span")[i].innerHTML = z('hMetadataRegistrationIndicator').value;
        }
        if (document.getElementsByTagName("span")[i].id.indexOf("lang_metadata_Registration_Source") != -1) {
            document.getElementsByTagName("span")[i].innerHTML = z('hMetadataRegistrationSource').value;
        }


    }

}

function BindRegistrations() {
    var ArtefactType;
    var InputParam;
    ApplyMaskingDiv();
    ShowLoadingDiv();
    document.getElementById('divRegistrationsByDFD').style.display = "";
    // document.getElementById('divRegistrationsByMFD').style.display = "";
    ArtefactType = "DFD";
    InputParam = document.getElementById('hdbnid').value;
    InputParam += ParamDelimiter + document.getElementById('hlngcodedb').value;
    InputParam += ParamDelimiter + ArtefactType;

    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: { 'callback': '1044', 'param1': InputParam },
            async: true,
            success: function (data) {
                try {

                    if (data != '') {
                        if (data) {
                            if (ArtefactType == "DFD") {
                                document.getElementById('divRegistrationsByDFD').innerHTML = data;
                                //  document.getElementById('divRegistrationsByMFD').innerHTML = data;
                            }

                            SetHiddenFieldValues();
                            MakeStaticHeaderForAllTheTablesInPage();
                            RemoveLoaderNMask();
                        }
                    }
                    else {
                        alert("Artifacts are not generated");
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


function MakeStaticHeaderForAllTheTablesInPage() {
    var name = "";
    for (var i = 0; i < document.getElementsByTagName('table').length; i++) {
        if (document.getElementsByTagName("table")[i].id.indexOf("tblMainContent_") != -1) {

            name = document.getElementsByTagName("table")[i].id.split('tblMainContent_')[1];
            MakeStaticHeader(document.getElementsByTagName("table")[i].id, "DivHeaderRow_" + name, "DivFooterRow_" + name, 300, 400, 52, true);
        }
    }
}







