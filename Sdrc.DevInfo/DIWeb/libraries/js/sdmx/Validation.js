// JScript File

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
function onPageLoad(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hLoggedInUserNId, hLoggedInUserName, hOriginaldbnid)
{	
    var hsgcount = 10;
	// ************************************************
	// create Form Tag with hidden input boxes
	// ************************************************
	createFormHiddenInputs("frm_sdmxValidate", "POST");
	SetCommonLinksHref("frm_sdmxValidate", "POST");
	SetOriginaldbnidInForm("frm_sdmxValidate", hOriginaldbnid);

	// ************************************************1
	// set page level variables based on selections or defaults
	// ************************************************1
    setPostedData(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hsgcount, hLoggedInUserNId, hLoggedInUserName);

	// ************************************************1
	// Load Language Component
	// ************************************************1
	ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', hlngcode);

	SetCommonRegistryPageDetails('RegValidation.aspx', "frm_sdmxValidate", "POST");
	document.getElementById('divGridContainer').style.display = "none";
	if (hIsUploadedDSD == "True") 
    {
        document.getElementById('divValidateDSD').style.display = "none";
    }
    else 
    {
        document.getElementById('divValidateDSD').style.display = "";
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

function ValidateSDMXML()
{
    try
    {      
               if(document.getElementById('hdnSdmxMlFile').value)
               {  
                    ApplyMaskingDiv();
                    ShowLoadingDiv();
                    InputParam  = document.getElementById('hdbnid').value;
                    InputParam  += ParamDelimiter + document.getElementById('hdnSdmxMlFile').value;	
                   
                    var htmlResp = di_jq.ajax
                    ({
                        type: "POST",
                        url: ValidationCallBackPageName,
                        data: "callback=71&param1=" + InputParam,                   
                        async:false,                          
                        success: function(data)
                        {                              
                            try
                            {                       
                                    
                                if(data)
                                {
                                    document.getElementById('divGridContainer').style.display = "";
                                    RenderingSDMXMLValidationResponseToOATGrid(data);
                                    HideLoadingDiv();
                                    RemoveMaskingDiv();    
                                }  
                               
                            }
                            catch(ex)
                            {
                                //                                alert("message:" + ex.message);
                               
                                HideLoadingDiv();
                                RemoveMaskingDiv();
                               
                            }
                        },
                        error:function()
                        {
                           
                            HideLoadingDiv();
                            RemoveMaskingDiv();
                          
                        },
                        cache: false
                    });   
                }
                else
                {
                    alert(MsgValidateSDMXML);
                }

                 
    }
    catch(ex)
    {
        //        alert("message:" + ex.message);
       
    }


}


function ValidateDSD()
{
    try
    {       
               if(document.getElementById('hdnDSD').value)
               {
                    ApplyMaskingDiv();
                    ShowLoadingDiv();
                    InputParam  = document.getElementById('hdbnid').value;
                    InputParam  += ParamDelimiter + document.getElementById('hdnDSD').value;	
                   
                    var htmlResp = di_jq.ajax
                    ({
                        type: "POST",
                        url: ValidationCallBackPageName,
                        data: "callback=72&param1=" + InputParam,                   
                        async:false,                          
                        success: function(data)
                        {                              
                            try
                            {                       
                                if(data)
                                {
                                    document.getElementById('divGridContainer').style.display = "";
                                    RenderingDSDValidationResponseToOATGrid(data);
                                    HideLoadingDiv();
                                    RemoveMaskingDiv();     
                                }  
                                                
                            }
                            catch(ex)
                            {
                                //                                alert("message:" + ex.message);
                               
                                HideLoadingDiv();
                                RemoveMaskingDiv();
                            }
                        },
                        error:function()
                        {
                           
                            HideLoadingDiv();
                            RemoveMaskingDiv();
                        },
                        cache: false
                    });   
                }
                else
                {
                    alert(MsgValidateDSD);
                }
           
                 
    }
    catch(ex)
    {
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

function RenderingSDMXMLValidationResponseToOATGrid(data)
{
    var header = new Array();
    var OATdata = [];
    
    header.push(MsgSDMXMLValidationScenarios);
    header.push(MsgSDMXMLValidationStatus);
    header.push('');
   
    
    var distinctValidationRows=data.split('#');
	for (var i=0 ; i < distinctValidationRows.length;i++) 
	{
	    var tmpRow = distinctValidationRows[i].split("~");
		OATdata.push(tmpRow);
	}
    var mainlist = [];
    mainlist.push(0);
    mainlist.push(1);
    
    
    var sublist = [];

    var pivot = new OAT.Pivot("pivot_content", "", "pivot_page", header, OATdata, mainlist, sublist, [], header.length - 1, { showChart: 0, showEmpty: 0, externalSort: true });
	
}

function RenderingDSDValidationResponseToOATGrid(data)
{
    var header = new Array();
    var OATdata = [];
    
    header.push(MsgDSDValidationScenarios);
    header.push(MsgDSDValidationStatus);
    header.push('');
   
    
    var distinctValidationRows=data.split('#');
	for (var i=0 ; i < distinctValidationRows.length;i++) 
	{
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

function ViewErrorDetails(ErrorDetails)
{
  
    if(ErrorDetails!='')
    {
        ShowLoadingDiv();
        ApplyMaskingDiv();

        document.getElementById('txtError').value = ErrorDetails;
        
        //Set close button at right corner of popup div
        SetCloseButtonInPopupDiv(di_jq('#divErrorDetails'),'HideErrorDetails');
        di_jq("#divErrorDetails").show('slow');    
        GetWindowCentered(document.getElementById('divErrorDetails'), 702, 490);
        HideLoadingDiv();        
    }
    else
    {
        alert(MsgErrorDetailsNotFound);
        HideLoadingDiv();
        RemoveMaskingDiv();
    }
}

function HideErrorDetails()
{
    di_jq('#divErrorDetails').hide('slow');	
    HideLoadingDiv();
    RemoveMaskingDiv();
}

function CallBack()
{}





