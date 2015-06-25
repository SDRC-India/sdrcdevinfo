// JScript File

var CommaSeparator = ",";
var MsgErrorOccurred = "Error Occured";
function onPageLoad(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hLoggedInUserNId, hLoggedInUserName, hOriginaldbnid)
{
    var hsgcount = 10;
	// ************************************************
	// create Form Tag with hidden input boxes
	// ************************************************
    createFormHiddenInputs("frm_sdmxStructureMetadata-Metadata", "POST");
    SetCommonLinksHref("frm_sdmxStructureMetadata-Metadata", "POST");
    SetOriginaldbnidInForm("frm_sdmxStructureMetadata-Metadata", hOriginaldbnid);

	// ************************************************1
	// set page level variables based on selections or defaults
	// ************************************************1
    setPostedData(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hsgcount, hLoggedInUserNId, hLoggedInUserName);

	// ************************************************1
	// Load Language Component
	// ************************************************1
	ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', hlngcode);

	SetCommonRegistryPageDetails('RegStructuralMD-Metadata.aspx', "frm_sdmxStructureMetadata-Metadata", "POST");
	BindMSDList();

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
    for (var i = 0; i < document.getElementsByTagName("h3").length; i++) 
    {
        if (document.getElementsByTagName("h3")[i].id.indexOf("lang_Metadata_Structure_Definition") != -1) {
            document.getElementsByTagName("h3")[i].innerHTML = document.getElementById('hMSD').value;
        }
        else if (document.getElementsByTagName("h3")[i].id.indexOf("lang_Metadata_Flow_Definition") != -1) {
            document.getElementsByTagName("h3")[i].innerHTML = document.getElementById('hMFD').value;
        }
        else if (document.getElementsByTagName("h3")[i].id.indexOf("lang_Concept_Scheme") != -1) {
            document.getElementsByTagName("h3")[i].innerHTML = document.getElementById('hCS').value;
        }
    }

    LanguageHandlingForAlerts();
}

function LanguageHandlingForAlerts() {
    MsgErrorOccurred = document.getElementById('hErrorOccurred').value;
}

function BindMSDList()
{
    try 
    {

        InputParam = z('hdbnid').value;

        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: "callback=95&param1=" + InputParam,
            async: false,
            success: function (data) {
                try {
                    var MSDs = data.split(CommaSeparator);
                    var i;
                    for (i = 0; i < MSDs.length; i++) {
                        z("selectMSD").options[z("selectMSD").length] = new Option(MSDs[i], i + 1);
                    }

                    if (z("selectMSD").length == 2) {
                            z("selectMSD").options[1].selected = "selected";
                   }
                   else
                   {
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
    catch (ex) 
    {

    }

}

function BindMSDArtefacts() 
{
    if (z("selectMSD").selectedIndex > 0) {
        z("reg_rgt_sec").style.display = "";
        z("lnkDownloadAll").href = "Download.aspx?fileId=stock/data/" + z('hdbnid').value + "/sdmx/Complete.xml";
        BindMSDAttributes();
        SetHiddenFieldValues();
    }
    else 
    {
        z("reg_rgt_sec").style.display = "none";
        z("divAttributes").innerHTML = "";
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

                    z("divAttributes").innerHTML = data;
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




