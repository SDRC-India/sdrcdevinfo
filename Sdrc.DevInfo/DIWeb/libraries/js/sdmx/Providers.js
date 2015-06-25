// JScript File

var CallbackStatus = '';
var ArtifactNotFound = '';
function onPageLoad(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hLoggedInUserNId, hLoggedInUserName, hOriginaldbnid)
{
    var hsgcount = 10;
	// ************************************************
	// create Form Tag with hidden input boxes
	// ************************************************
	createFormHiddenInputs("frm_sdmxProvisioningMetadata", "POST");
	SetCommonLinksHref("frm_sdmxProvisioningMetadata", "POST");
	SetOriginaldbnidInForm("frm_sdmxProvisioningMetadata", hOriginaldbnid);

	// ************************************************1
	// set page level variables based on selections or defaults
	// ************************************************1
    setPostedData(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hsgcount, hLoggedInUserNId, hLoggedInUserName);

	// ************************************************1
	// Load Language Component
	// ************************************************1
	ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', hlngcode);

	SetCommonRegistryPageDetails('RegProviders.aspx', "frm_sdmxProvisioningMetadata", "POST");
	
	 //Binding Data Provider Scheme and Data Flow Definiton by reading from corresponding xmls
	//BindDPSchemeAndDFD();
}

function LanguageHandlingForAlerts() {
    CallbackStatus = document.getElementById('hUpdatedSuccess').value;
    ArtifactNotFound = document.getElementById('hArtifactNotFound').value;
   
}

function SetHiddenFieldValues() {
    if (CheckIfSiteAdminLoggedInShowSpecificTabs() == 'true') {

        if (z('spanEmail') != null) {
            z('spanEmail').innerHTML = z('hEmail').value;
        }

        if (z('spanName') != null) {
            z('spanName').innerHTML = z('hName').value;
        }

        if (z('spanCountry') != null) {
            z('spanCountry').innerHTML = z('hCountry').value;
        }

        if (z('spanProvider') != null) {
            z('spanProvider').innerHTML = z('hProvider').value;
        }
    }
}

function SetHiddenFieldValuesForOAT() {
    for (var i = 0; i < document.getElementsByName('lang_View').length; i++) {
        document.getElementsByName('lang_View')[i].innerHTML = document.getElementById('hView').value;
    }

    for (var i = 0; i < document.getElementsByName('lang_Download').length; i++) {
        document.getElementsByName('lang_Download')[i].innerHTML = document.getElementById('hDownload').value;
    }
}


// BindDPSchemeAndDFD function
//function BindDPSchemeAndDFD() 
//{
//    ApplyMaskingDiv();
//    ShowLoadingDiv();
//    
//	var InputParam  = document.getElementById('hdbnid').value;	
//	InputParam  += ParamDelimiter + document.getElementById('hlngcodedb').value;	
//    var htmlResp = di_jq.ajax({
//            type: "POST",
//            url: "Callback.aspx",
//            data: "callback=64&param1=" + InputParam,
//            async:false,
//            success: function(data){                              
//                try
//                {
//                document.getElementById('divDownloads').innerHTML=data;
//                HideLoadingDiv();
//                RemoveMaskingDiv();
//										
//                }
//                catch(ex){
//                    alert("Error : " + ex.message);
//                }
//            },
//            error:function(){
//                
//            },
//            cache: false
//    });
//}


// BindDataProviders function
function BindDataProviders() 
{

    ApplyMaskingDiv();
    ShowLoadingDiv();
    var InputParam = document.getElementById('hdbnid').value;
    InputParam += ParamDelimiter + document.getElementById('hlngcodedb').value;
    InputParam += ParamDelimiter + document.getElementById('hlngcode').value;	
    var htmlResp = di_jq.ajax({
            type: "POST",
            url: "Callback.aspx",
             data: "callback=65&param1=" + InputParam,
            async:false,
            success: function(data){                              
                try
                {
                  if(data)
                  {  
                      RenderingProviderDetailsInOATGrid(data);
                      
                  }
                 
										
                }
                catch(ex){
                    alert("Error : " + ex.message);
                }
            },
            error:function(){
                
            },
            cache: false
    });
     HideLoadingDiv();
     RemoveMaskingDiv();
}


function RenderingProviderDetailsInOATGrid(data)
{
    var header = new Array();
    var OATdata = [];

    var RowsAndColumns = data.split('[****]');

    if (RowsAndColumns.length > 1) {
        var Columns = RowsAndColumns[0].split('~');
        for (var i = 0; i < Columns.length; i++) {
            header.push(Columns[i]);
        }

        var distinctProviderRows = RowsAndColumns[1].split('#');
        for (var i = 0; i < distinctProviderRows.length; i++) {
            var tmpRow = distinctProviderRows[i].split("~");
            OATdata.push(tmpRow);
        }

        var mainlist = [];
        for (var i = 0; i < Columns.length - 1; i++) {
            mainlist.push(i);
        }

        var sublist = [];

        var pivot = new OAT.Pivot("pivot_content", "", "pivot_page", header, OATdata, mainlist, sublist, [], header.length - 1, { showChart: 0, showEmpty: 0 });
    }
}


function ViewArtefact(ArtefactType,ArtefactPath)
{
    // calling callback
	ArtefactCallBack(ArtefactType,ArtefactPath);
}

// callback function
function ArtefactCallBack(ArtefactType,ArtefactPath) 
{
    ApplyMaskingDiv();
    ShowLoadingDiv();

    var InputParam = ArtefactPath;
    
    var htmlResp = di_jq.ajax({
            type: "POST",
            url: "Callback.aspx",
            data: "callback=21&param1=" + InputParam,
            async:false,
            success: function(data){                              
                try
                {
                ShowArtefact(ArtefactType,data);
										
                }
                catch(ex){
                    alert("Error : " + ex.message);
                }
            },
            error:function(){
                
            },
            cache: false
    });
}

function ShowArtefact(ArtefactType,ResponseText) {
    if(ResponseText!='')
    {

        document.getElementById('preArtefacts').innerHTML = ResponseText;
        
        //Set close button at right corner of popup div
        SetCloseButtonInPopupDiv(di_jq('#divArtefacts'),'HideArtefact');

        di_jq("#divArtefacts").show('slow');
        GetWindowCentered(document.getElementById('divArtefacts'), 702, 490);
        if(ArtefactType=='DFD')
        {
             document.getElementById('lblArtefactType').innerHTML="DATA FLOW DEFINITION";
        }
        else if (ArtefactType == 'MFD') 
        {
             document.getElementById('lblArtefactType').innerHTML = "METADATA FLOW DEFINITION";
        }
        else if(ArtefactType=='PA')
        {
             document.getElementById('lblArtefactType').innerHTML="PROVISION AGREEMENT";
        }
         else if (ArtefactType == 'DPScheme')
        {
             document.getElementById('lblArtefactType').innerHTML="DATA PROVIDER SCHEME";
         }
         else if (ArtefactType == 'MAScheme') {
             document.getElementById('lblArtefactType').innerHTML = "MAINTENANCE AGENCY SCHEME";
         }
    
       
       
        HideLoadingDiv();
    }
    else {
        alert(ArtifactNotFound);
        RemoveMaskingDiv();
        HideLoadingDiv();
    }
}

function HideArtefact()
{
    RemoveMaskingDiv();
    di_jq('#divArtefacts').hide('slow');	
}

function GetAllUsersHTMLForAdmin() {
    var InputParam = '';

    ApplyMaskingDiv();
    ShowLoadingDiv();

    var htmlResp = di_jq.ajax({
        type: "POST",
        url: "Callback.aspx",
        data: "callback=132&param1=" + InputParam,
        async: false,
        success: function (data) {
            try {
                z('divInnerUsers').innerHTML = data;
                
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

function UpdateUsers() {
    var InputParam = z('hlngcode').value + ParamDelimiter + GetUpdatedTableInformation();

    ApplyMaskingDiv();
    ShowLoadingDiv();

    var htmlResp = di_jq.ajax({
        type: "POST",
        url: "Callback.aspx",
        data: "callback=133&param1=" + InputParam,
        async: true,
        success: function (data) {
            try {
                if (data == 'true') {
                    // alert('Updated Successfully');
                    alert(CallbackStatus)
                    HideLoadingDiv();
                    RemoveMaskingDiv();

                    GetAllUsersHTMLForAdmin();
                    BindDataProviders();
                    SetHiddenFieldValues(); 
                    SetHiddenFieldValuesForOAT();
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

function GetUpdatedTableInformation() {
    var RetVal;
    var tblUsers;
    var chkUsers;

    RetVal = '';
    tblUsers = z('tblUsers');

    for (var rowCount = 1; rowCount < tblUsers.rows.length; rowCount++) {
        chkUsers = tblUsers.rows[rowCount].cells[tblUsers.rows[rowCount].cells.length - 1].children[0];

        RetVal += chkUsers.value;

        if (chkUsers.checked) {
            RetVal += '|True';
        }
        else {
            RetVal += '|False';
        }

        RetVal += ParamDelimiter;
    }

    return RetVal;
}

function CallBack()
{}

