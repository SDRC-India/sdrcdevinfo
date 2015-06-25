function SetCommonRegistryPageDetails(pageName, formID, method)
{
    var aStructure,aData,aStructuralMetadata, aStructuralMetadataMetadata, aSubscription, aPublishData, aProviders, aValidate, aCompare, aWSDemo, aDiscover, aMapping, aTools;
    var aMaintenanceAgency, aDataProvider, aDiscoverRegistrations, aDatabaseManagement,aUpload;
    var spanPageTitle, pPageDescription;

    aStructure = z('aStructure');
    aData = z('aData');
    aStructuralMetadata = z('aStructuralMetadata');
    aStructuralMetadataMetadata = z('aStructuralMetadataMetadata');
    aSubscription = z('aSubscription');
    aPublishData = z('aPublishData');
    aProviders = z('aProviders');
    aValidate = z('aValidate');
    aCompare = z('aCompare');
    aWSDemo = z('aWSDemo');
    aDiscover = z('aDiscover');
    aMapping = z('aMapping');
    aMaintenanceAgency = z('aMaintenanceAgency');
    aDatabaseManagement = z('aDatabaseManagement');
    aUpload = z('aUpload');
    aDataProvider = z('aDataProvider');
    aDiscoverRegistrations = z('aDiscoverRegistrations');
    aTools = z('aTools');
    spanPageTitle = z('spanPageTitle');
    pPageDescription = z('pPageDescription');


    if (pageName == 'RegStructure.aspx') {

        di_jq("#aStructure").attr("class", "reg_nav_tab_active");
    }
     if (pageName == 'RegData.aspx') {
        di_jq("#aData").attr("class", "reg_nav_tab_active");
    }
//    else if (pageName == 'RegStructuralMD.aspx')
//    {
//        di_jq("#aStructuralMetadata").attr("class","reg_nav_tab_active");
//    }
    else if (pageName == 'RegStructuralMD-Metadata.aspx') {
        di_jq("#aStructuralMetadataMetadata").attr("class", "reg_nav_tab_active");
    }
    else if (pageName == 'RegSubscription.aspx')
    {
        z('aNotLoggedIn').href = z('aLogin').href;
        di_jq("#aSubscription").attr("class","reg_nav_tab_active");
    }
    else if (pageName == 'RegPublishData.aspx')
    {
        z('aNotLoggedIn').href = z('aLogin').href;
        di_jq("#aPublishData").attr("class","reg_nav_tab_active");
    }
    else if (pageName == 'RegProviders.aspx')
    {
        di_jq("#aProviders").attr("class","reg_nav_tab_active");
    }
    else if (pageName == 'RegValidation.aspx')
    {
        di_jq("#aValidate").attr("class","reg_nav_tab_active");
    }
    else if (pageName == 'RegComparison.aspx')
    {
        di_jq("#aCompare").attr("class","reg_nav_tab_active");
    }
    else if (pageName == 'RegWebServiceDemo.aspx') {
        di_jq("#aWSDemo").attr("class", "reg_nav_tab_active");
    }
    else if (pageName == 'RegDiscover.aspx') {
        spanPageTitle.innerHTML = 'Discover';
        pPageDescription.innerHTML = 'Discover data and metadata available from various providers against a selection of dimension values.';
        di_jq("#aDiscover").attr("class", "reg_nav_tab_active");
    }
    else if (pageName == 'RegMapping.aspx') {
        z('aNotLoggedIn').href = z('aLogin').href;
        z('aNotanAdmin').href = z('aLogin').href;
        di_jq("#aMapping").attr("class", "reg_nav_tab_active");
    }
    else if (pageName == 'RegMaintenanceAgency.aspx') {
        z('aNotLoggedIn').href = z('aLogin').href;
        z('aNotanAdmin').href = z('aLogin').href;
        di_jq("#aMaintenanceAgency").attr("class", "reg_nav_tab_active");
    }
    else if (pageName == 'RegDatabaseManagement.aspx') {
        z('aNotLoggedIn').href = z('aLogin').href;
        z('aNotanAdmin').href = z('aLogin').href;
        di_jq("#aDatabaseManagement").attr("class", "reg_nav_tab_active");
    }
    else if (pageName == 'RegUpload.aspx') {
        z('aNotLoggedIn').href = z('aLogin').href;
        z('aNotanAdmin').href = z('aLogin').href;
        di_jq("#aUpload").attr("class", "reg_nav_tab_active");
    }
    else if (pageName == 'RegDataProvider.aspx') {
        z('aNotLoggedIn').href = z('aLogin').href;
        z('aNotanAdmin').href = z('aLogin').href;
        di_jq("#aDataProvider").attr("class", "reg_nav_tab_active");
    }
    else if (pageName == 'RegDiscoverRegistrations.aspx') {
        di_jq("#aDiscoverRegistrations").attr("class", "reg_nav_tab_active");
    }
    else if (pageName == 'RegTools.aspx') {
        di_jq("#aTools").attr("class", "reg_nav_tab_active");
    }

    aStructure.href = "javascript:PostData('" + formID + "','RegStructure.aspx','" + method + "');";
    aData.href = "javascript:PostData('" + formID + "','RegData.aspx','" + method + "');";
   // aStructuralMetadata.href = "javascript:PostData('" + formID + "','RegStructuralMD.aspx','" + method + "');";
    aStructuralMetadataMetadata.href = "javascript:PostData('" + formID + "','RegStructuralMD-Metadata.aspx','" + method + "');";
    aSubscription.href = "javascript:PostData('" + formID + "','RegSubscription.aspx','" + method + "');";
    aPublishData.href = "javascript:PostData('" + formID + "','RegPublishData.aspx','" + method + "');";
    aProviders.href = "javascript:PostData('" + formID + "','RegProviders.aspx','" + method + "');";
    aValidate.href = "javascript:PostData('" + formID + "','RegValidation.aspx','" + method + "');";
    aCompare.href = "javascript:PostData('" + formID + "','RegComparison.aspx','" + method + "');";
    aWSDemo.href = "javascript:PostData('" + formID + "','RegWebServiceDemo.aspx','" + method + "');";
    aDiscover.href = "javascript:PostData('" + formID + "','RegDiscover.aspx','" + method + "');";
    aMapping.href = "javascript:PostData('" + formID + "','RegMapping.aspx','" + method + "');";
    aMaintenanceAgency.href = "javascript:PostData('" + formID + "','RegMaintenanceAgency.aspx','" + method + "');";
    aDatabaseManagement.href = "javascript:PostData('" + formID + "','RegDatabaseManagement.aspx','" + method + "');";
    aUpload.href = "javascript:PostData('" + formID + "','RegUpload.aspx','" + method + "');";
    aDataProvider.href = "javascript:PostData('" + formID + "','RegDataProvider.aspx','" + method + "');";
    aDiscoverRegistrations.href = "javascript:PostData('" + formID + "','RegDiscoverRegistrations.aspx','" + method + "');";
    aTools.href = "javascript:PostData('" + formID + "','RegTools.aspx','" + method + "');";
}

function SelectedDSDChange() {
    z('hdbnid').value = z('cphMainContent_selectDSDInUse').value;
   
    GoToRegistry();
}

function HandleLoginForRegistry(flag) {
    var aStructure,aData, aStructuralMetadata, aStructuralMetadataMetadata, aSubscription, aPublishData, aProviders, aValidate, aCompare, aWSDemo, aDiscover, aMapping;
    var aMaintenanceAgency, aDataProvider, aDiscoverRegistrations, aTools, aDatabaseManagement, aUpload;

    aStructure = z('aStructure');
    aData = z('aData');
    aStructuralMetadata = z('aStructuralMetadata');
    aStructuralMetadataMetadata = z('aStructuralMetadataMetadata');
    aSubscription = z('aSubscription');
    aPublishData = z('aPublishData');
    aProviders = z('aProviders');
    aValidate = z('aValidate');
    aCompare = z('aCompare');
    aWSDemo = z('aWSDemo');
    aDiscover = z('aDiscover');
    aMapping = z('aMapping');
    aMaintenanceAgency = z('aMaintenanceAgency');
    aDatabaseManagement = z('aDatabaseManagement');
    aUpload =  z('aUpload');
    aDataProvider = z('aDataProvider');
    aDiscoverRegistrations = z('aDiscoverRegistrations');
    aTools = z('aTools');
    if (flag == true) {
        if (z('hLoggedInUserNId').value.split('|')[1] == 'True') {
            if (CheckIfSiteAdminLoggedInShowSpecificTabs() == 'true') {
                aStructure.style.display = "";
                aData.style.display = "";
                aStructuralMetadata.style.display = "";
                aStructuralMetadataMetadata.style.display = "";
                aSubscription.style.display = "";
                aPublishData.style.display = "";
                aProviders.style.display = "";
                aValidate.style.display = "";
                aCompare.style.display = "";
                aWSDemo.style.display = "";
                aDiscover.style.display = "";
                aMapping.style.display = "";
                aMaintenanceAgency.style.display = "";
                aDatabaseManagement.style.display = "";
                aUpload.style.display = "";
                aDataProvider.style.display = "";
                aDiscoverRegistrations.style.display = "";

                if (z('hiddenFrmId').value == 'frm_sdmxProvisioningMetadata') {
                    z('divUsers').style.display = "";
                    GetAllUsersHTMLForAdmin();
                }
            }
            else {
                aStructure.style.display = "";
                aData.style.display = "";
                aStructuralMetadata.style.display = "";
                aStructuralMetadataMetadata.style.display = "";
                aSubscription.style.display = "";
                aPublishData.style.display = "";
                aProviders.style.display = "none";
                aValidate.style.display = "";
                aCompare.style.display = "";
                aWSDemo.style.display = "";
                aDiscover.style.display = "";
                aMapping.style.display = "none";
                aMaintenanceAgency.style.display = "none";
                aDatabaseManagement.style.display = "none";
                aUpload.style.display = "none";
                aDataProvider.style.display = "none";
                aDiscoverRegistrations.style.display = "";

                if (z('hiddenFrmId').value == 'frm_sdmxProvisioningMetadata') {
                    z('divUsers').style.display = "none";
                }
            }
        }
        else {
            aStructure.style.display = "";
            aData.style.display = "";
            aStructuralMetadata.style.display = "";
            aStructuralMetadataMetadata.style.display = "";
            aSubscription.style.display = "";
            aPublishData.style.display = "none";
            aProviders.style.display = "none";
            aValidate.style.display = "";
            aCompare.style.display = "";
            aWSDemo.style.display = "";
            aDiscover.style.display = "";
            aMapping.style.display = "none";
            aMaintenanceAgency.style.display = "none";
            aDatabaseManagement.style.display = "none";
            aUpload.style.display = "none";
            aDataProvider.style.display = "none";
            aDiscoverRegistrations.style.display = "";

            if (z('hiddenFrmId').value == 'frm_sdmxProvisioningMetadata') {
                z('divUsers').style.display = "none";
            }
        }
    }
    else {
        aStructure.style.display = "";
        aData.style.display = "";
        aStructuralMetadata.style.display = "";
        aStructuralMetadataMetadata.style.display = "";
        aSubscription.style.display = "none";
        aPublishData.style.display = "none";
        aProviders.style.display = "none";
        aValidate.style.display = "";
        aCompare.style.display = "";
        aWSDemo.style.display = "";
        aDiscover.style.display = "";
        aMapping.style.display = "none";
        aMaintenanceAgency.style.display = "none";
        aDatabaseManagement.style.display = "none";
        aUpload.style.display = "none";
        aDataProvider.style.display = "none";
        aDiscoverRegistrations.style.display = "";

        if (z('hiddenFrmId').value == 'frm_sdmxProvisioningMetadata') {
            z('divUsers').style.display = "none";
        }

        if (z('hiddenFrmId').value == 'frm_sdmxSubscription' || z('hiddenFrmId').value == 'frm_sdmxPublishData' ||
                z('hiddenFrmId').value == 'frm_sdmxDataProvider' || z('hiddenFrmId').value == 'frm_sdmxUpload' || z('hiddenFrmId').value == 'frm_sdmxMaintenanceAgency' || z('hiddenFrmId').value == 'frm_sdmxDatabaseManagement' ||
                z('hiddenFrmId').value == 'frm_sdmxMapping' ) {
            z('hdbnid').value = z('cphMainContent_selectDSDInUse').value;
            GoToRegistry();
        }
    }

    z('divMainContentContainer').style.display = "";
    z('divNotanAdmin').style.display = "none";
    z('divNotaProvider').style.display = "none";
    z('divNotLoggedIn').style.display = "none";

    // Hide Tabs according to version - Begin

    if (hIsUploadedDSD == "True") {
        aCompare.style.display = "none";
        z('DivUpperRight').style.display = "none";
    }
    else {
        aMapping.style.display = "none";
    }
    aDiscover.style.display = "none";

}

function RequestAdminForDataProviderRights() {
    var InputParam = z('hLoggedInUserNId').value.split('|')[0];
    InputParam += z('hlngcode').value;
    ShowLoadingDiv();
    ApplyMaskingDiv();

    var htmlResp = di_jq.ajax({
        type: "POST",
        url: "Callback.aspx",
        data: "callback=135&param1=" + InputParam,
        async: true,
        success: function (data) {
            try {
                if (data == 'true') {
                    alert('Mail has been sent to admin to give you data provider rights');
                }

                HideLoadingDiv();
                RemoveMaskingDiv();
            }
            catch (ex) {
                alert("Error : " + ex.message);

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

function NotaProviderRedirectToLogin()
{
    z('hLoggedInUserNId').value = '';
    z('hLoggedInUserName').value = '';
}

function NotanAdminRedirectToLogin() {
    z('hLoggedInUserNId').value = '';
    z('hLoggedInUserName').value = '';
}

function Close(divPopup) 
{
    divPopup.style.display = "none";
    RemoveMaskingDiv();
}

function IsUploadedDSD() {
    var RetVal;
    var selectDSDInUse;

    selectDSDInUse = z('cphMainContent_selectDSDInUse');

    if (selectDSDInUse.options[selectDSDInUse.selectedIndex].getAttribute("IsUploadedDSD") == "True") {
        RetVal = true;
    }
    else {
        RetVal = false;
    }

    return RetVal;
}

function ToggleCallout(divID, evt, helpDivId) {
    var helpDiv = z(helpDivId);
    var evnt = evt || window.event;
    if ((z(divID).style.display == "none") || (z(divID).style.display == ""))
    {
        for (var i = 0; i < z(divID + 'Text').children.length; i++) {
            z(divID + 'Text').children[i].style.display = "none";
        }
        helpDiv.style.display = "";

        SetCalloutPosition(evt, z(divID), 20, 0);

        z(divID).style.display = "block";
    }
    else {
        HideCallout(divID);
    }
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

function ApplyMaskNLoader(zIndex) {
    ApplyMaskingDiv(zIndex);
    ShowLoadingDiv();
}

function RemoveLoaderNMask() {
    HideLoadingDiv();
    RemoveMaskingDiv();
}
