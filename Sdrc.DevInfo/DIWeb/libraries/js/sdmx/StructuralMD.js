// JScript File
var MsgErrorOccurred = "Error Occured";
var MsgFileNotFound = "File Not Found";
function onPageLoad(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hLoggedInUserNId, hLoggedInUserName, hOriginaldbnid)
{
    var hsgcount = 10;
	// ************************************************
	// create Form Tag with hidden input boxes
	// ************************************************
	createFormHiddenInputs("frm_sdmxStructureMetadata", "POST");
	SetCommonLinksHref("frm_sdmxStructureMetadata", "POST");

	SetOriginaldbnidInForm("frm_sdmxStructureMetadata", hOriginaldbnid);

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

	SetCommonRegistryPageDetails('RegStructuralMD.aspx', "frm_sdmxStructureMetadata", "POST");
	
	document.getElementById("lnkDownloadAll").href = "Download.aspx?fileId=stock/data/"+hdbnid+"/sdmx/Complete.xml";
	document.getElementById("lnkDownloadRpt").href = "Download.aspx?fileId=stock/data/"+hdbnid+"/sdmx/Report.xls";
	
}
function ViewCodelist(CodelistPath)
{
    // calling callback
	CallBack(CodelistPath);
}
function SetHiddenFieldValues() 
{
    for (var i = 0; i < document.getElementsByName('lang_View').length; i++) 
    {
        document.getElementsByName('lang_View')[i].innerHTML = document.getElementById('hView').value;
    }

    for (var i = 0; i < document.getElementsByName('lang_Download').length; i++) 
    {
        document.getElementsByName('lang_Download')[i].innerHTML = document.getElementById('hDownload').value;
    }

    for (var i = 0; i < document.getElementsByTagName("span").length; i++) 
    {
        if (document.getElementsByTagName("span")[i].id.indexOf("lang_Attachment_Level") != -1)
        {
           document.getElementsByTagName("span")[i].innerHTML = document.getElementById('hAttachmentLevel').value;
       }
       else if (document.getElementsByTagName("span")[i].id.indexOf("lang_Mandatory") != -1) {
           document.getElementsByTagName("span")[i].innerHTML = document.getElementById('hMandatory').value;
       }
       else if (document.getElementsByTagName("span")[i].id.indexOf("lang_Yes") != -1) {
           document.getElementsByTagName("span")[i].innerHTML = document.getElementById('hYes').value;
       }
       else if (document.getElementsByTagName("span")[i].id.indexOf("lang_No") != -1) {
           document.getElementsByTagName("span")[i].innerHTML = document.getElementById('hNo').value;
       }
    }
    LanguageHandlingForAlerts();
}

function LanguageHandlingForAlerts() {
    MsgErrorOccurred = document.getElementById('hErrorOccurred').value;
    MsgFileNotFound = document.getElementById('hFileNotFound').value;
}

// callback function
function CallBack(CodelistPath) 
{

    ApplyMaskingDiv();
    ShowLoadingDiv();

    var InputParam = CodelistPath;
    InputParam += ParamDelimiter + document.getElementById('hlngcode').value;
    InputParam += ParamDelimiter + document.getElementById('hdbnid').value;				                         
    var htmlResp = di_jq.ajax({
            type: "POST",
            url: "Callback.aspx",
            data: "callback=20&param1=" + InputParam,
            async:false,
            success: function(data){                              
                try
                {
                ShowCodelist(data);
										
                }
                catch(ex){
//                    alert("Error : " + ex.message);
                
                }
            },
            error:function(){
                
            },
            cache: false
    });
}

function ShowCodelist(ResponseText)
{
    if(ResponseText!='')
    {
        document.getElementById('divCodelistXmlData').innerHTML = ResponseText;        
        
        //Set close button at right corner of popup div
        SetCloseButtonInPopupDiv(di_jq('#divCodelistXml'),'HideCodelist'); 
        
        di_jq('#divCodelistXml').show('slow');	
        
        GetWindowCentered(document.getElementById('divCodelistXml'), 702, 475);
        
        HideLoadingDiv();
    }
    else
    {
        alert(MsgFileNotFound);
        HideLoadingDiv();
        RemoveMaskingDiv();
    }
}

function HideCodelist()
{
    di_jq('#divCodelistXml').hide('slow');	
    RemoveMaskingDiv();
}





