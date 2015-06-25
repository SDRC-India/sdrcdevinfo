// JScript File

var DiscoverRegistrationsCallBackPageName = "Callback.aspx";

function onPageLoad(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hLoggedInUserNId, hLoggedInUserName, hOriginaldbnid) {
    var hsgcount = 10;
    // ************************************************
    // create Form Tag with hidden input boxes
    // ************************************************
    createFormHiddenInputs("frm_sdmxDiscoverRegistrations", "POST");
    SetCommonLinksHref("frm_sdmxDiscoverRegistrations", "POST");
    SetOriginaldbnidInForm("frm_sdmxDiscoverRegistrations", hOriginaldbnid);

    // ************************************************1
    // set page level variables based on selections or defaults
    // ************************************************1
    setPostedData(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hsgcount, hLoggedInUserNId, hLoggedInUserName);

    // ************************************************1
    // Load Language Component
    // ************************************************1
    ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', hlngcode);

    SetCommonRegistryPageDetails('RegDiscoverRegistrations.aspx', "frm_sdmxDiscoverRegistrations", "POST");
    //document.getElementById('selViewRegistrationsBy').selectedIndex = 3;
    BindRegistrations();
  
}

function SetHiddenFieldValues() {
    for (var i = 0; i < document.getElementsByName('lang_View').length; i++) 
    {
        document.getElementsByName('lang_View')[i].innerHTML = document.getElementById('hView').value;
    }

    for (var i = 0; i < document.getElementsByName('lang_Download').length; i++) 
    {
        document.getElementsByName('lang_Download')[i].innerHTML = document.getElementById('hDownload').value;
    }

    for (var i = 0; i < document.getElementsByTagName('span').length; i++) 
    {
        if (document.getElementsByTagName("span")[i].id.indexOf("lang_Id") != -1)
        {
            document.getElementsByTagName("span")[i].innerHTML = z('hId').value;
        }

        if (document.getElementsByTagName("span")[i].id.indexOf("lang_Queryable_Data") != -1) 
        {
            document.getElementsByTagName("span")[i].innerHTML = z('hQueryableData').value;
        }

        if (document.getElementsByTagName("span")[i].id.indexOf("lang_WADL") != -1) 
        {
            document.getElementsByTagName("span")[i].innerHTML = z('hWADL').value;
        }

        if (document.getElementsByTagName("span")[i].id.indexOf("lang_WSDL") != -1) 
        {
            document.getElementsByTagName("span")[i].innerHTML = z('hWSDL').value;
        }

        if (document.getElementsByTagName("span")[i].id.indexOf("lang_Simple_Data") != -1) 
        {
            document.getElementsByTagName("span")[i].innerHTML = z('hSimpleData').value;
        }

        if (document.getElementsByTagName("span")[i].id.indexOf("lang_Data_Metadata") != -1) 
        {
            document.getElementsByTagName("span")[i].innerHTML = z('hDataMetadata').value;
        }

        if (document.getElementsByTagName("span")[i].id.indexOf("lang_Constraints") != -1) 
        {
            document.getElementsByTagName("span")[i].innerHTML = z('hConstraints').value;
        }

        if (document.getElementsByTagName("span")[i].id.indexOf("lang_Provision_Agreement") != -1) 
        {
            document.getElementsByTagName("span")[i].innerHTML = z('hProvisionAgreement').value;
        }

        if (document.getElementsByTagName("span")[i].id.indexOf("lang_Data_Provider") != -1) 
        {
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
    }
    
}

function BindRegistrations() 
{
    var ArtefactType;
    var InputParam;
    ApplyMaskingDiv();
    ShowLoadingDiv();
    if (document.getElementById('selViewRegistrationsBy').selectedIndex == 0) {
        document.getElementById('divRegistrationsByDFD').style.display = "";
      
        document.getElementById('divRegistrationsByMFD').style.display = "none";
        document.getElementById('divRegistrationsByPA').style.display = "none";
        document.getElementById('divRegistrationsByDP').style.display = "none";
        ArtefactType = "DFD";
    }
    else if (document.getElementById('selViewRegistrationsBy').selectedIndex == 1) {
        document.getElementById('divRegistrationsByDFD').style.display = "none";
        document.getElementById('divRegistrationsByMFD').style.display = "";
        document.getElementById('divRegistrationsByPA').style.display = "none";
        document.getElementById('divRegistrationsByDP').style.display = "none";
        ArtefactType = "MFD";
    }
    else if (document.getElementById('selViewRegistrationsBy').selectedIndex == 2) {
        document.getElementById('divRegistrationsByDFD').style.display = "none";
        document.getElementById('divRegistrationsByMFD').style.display = "none";
        document.getElementById('divRegistrationsByPA').style.display = "";
        document.getElementById('divRegistrationsByDP').style.display = "none";
        ArtefactType = "PA";
    }
    else if (document.getElementById('selViewRegistrationsBy').selectedIndex == 3) {
        document.getElementById('divRegistrationsByDFD').style.display = "none";
        document.getElementById('divRegistrationsByMFD').style.display = "none";
        document.getElementById('divRegistrationsByPA').style.display = "none";
        document.getElementById('divRegistrationsByDP').style.display = "";
        ArtefactType = "DP";
    }

    InputParam =  document.getElementById('hdbnid').value;
    InputParam += ParamDelimiter +  document.getElementById('hlngcodedb').value;
    InputParam += ParamDelimiter + ArtefactType;

    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: { 'callback': '141', 'param1': InputParam },
            async: true,
            success: function (data) {
                try {
                    if (data) {
                        if (ArtefactType == "DFD") {
                            document.getElementById('divRegistrationsByDFD').innerHTML = data;
                          //  document.getElementById('divDFD_DF_DevInfo').style.display = "";
                        }
                        else if (ArtefactType == "MFD") {
                            document.getElementById('divRegistrationsByMFD').innerHTML = data;
                        }
                        else if (ArtefactType == "PA") {
                            document.getElementById('divRegistrationsByPA').innerHTML = data;
                        }
                        else if (ArtefactType == "DP") {
                            document.getElementById('divRegistrationsByDP').innerHTML = data;
                        }

                        SetHiddenFieldValues();
                        MakeStaticHeaderForAllTheTablesInPage();
                        RemoveLoaderNMask();
                    }
                }
                catch (ex) {
                    alert("message:" + ex.message);
                    RemoveLoaderNMask();
                }
            },
            error: function () {
             //   
                RemoveLoaderNMask();
            },
            cache: false
        });
    }
    catch (ex) {
    }

}

function MakeStaticHeaderForAllTheTablesInPage() 
{
    var name = "";
    for (var i = 0; i < document.getElementsByTagName('table').length; i++) 
    {
        if (document.getElementsByTagName("table")[i].id.indexOf("tblMainContent_") != -1) {

            name = document.getElementsByTagName("table")[i].id.split('tblMainContent_')[1];
            MakeStaticHeader(document.getElementsByTagName("table")[i].id, "DivHeaderRow_" + name, "DivFooterRow_" + name , 300, 400, 52, true);
        }
    }
}









