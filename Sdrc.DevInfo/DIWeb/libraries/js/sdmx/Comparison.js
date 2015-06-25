// JScript File
var MsgErrorOccurred = "Error Occured";
var MsgPleaseBrowseDSD = "Please browse a DSD to Compare against Master DSD.";
var MsgPleaseBrowseDSD1AndDSD2 = "Please browse DSD1 and DSD2 to Compare with each other.";
var MsgGeneratingReport = "Generating  Report....";
var MsgReportSuccessfullyGenerated = "Report generated sucessfully.Please download it by clicking on Download Report button.";

function onPageLoad(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hLoggedInUserNId, hLoggedInUserName, hOriginaldbnid)
{	
    var hsgcount = 10;
	// ************************************************
	// create Form Tag with hidden input boxes
	// ************************************************
	createFormHiddenInputs("frm_sdmxCompare", "POST");
	SetCommonLinksHref("frm_sdmxCompare", "POST");
	SetOriginaldbnidInForm("frm_sdmxCompare", hOriginaldbnid);

	// ************************************************1
	// set page level variables based on selections or defaults
	// ************************************************1
    setPostedData(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hsgcount, hLoggedInUserNId, hLoggedInUserName);

	// ************************************************1
	// Load Language Component
	// ************************************************1
	ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', hlngcode);

	SetCommonRegistryPageDetails('RegComparison.aspx', "frm_sdmxCompare", "POST");
    document.getElementById('divStructuralComparison').style.display = "none";
    document.getElementById('divCodelistComparison').style.display = "none";
    LanguageHandlingForAlerts();
}

function LanguageHandlingForAlerts() {
    MsgErrorOccurred = document.getElementById('hErrorOccurred').value;
    MsgPleaseBrowseDSD = document.getElementById('hMsgPleaseBrowseDSD').value;
    MsgPleaseBrowseDSD1AndDSD2 = document.getElementById('hMsgPleaseBrowseDSD1AndDSD2').value;
    MsgGeneratingReport = document.getElementById('hMsgGeneratingReport').value;
    MsgReportSuccessfullyGenerated = document.getElementById('hMsgReportSuccessfullyGenerated').value;
}

function CompareUserDSDAgainstDevInfoDSD()
{
   
 try
    {      
               
               document.getElementById('divCodelistComparison').innerHTML="";
               document.getElementById('divCodelistComparison').style.display = "none";
               if(document.getElementById('hdnDSDAgainstDevInfoDSD').value)
               {
                    ApplyMaskingDiv();
                    ShowLoadingDiv();
                    InputParam  = document.getElementById('hdbnid').value;
                    InputParam  += ParamDelimiter + document.getElementById('hlngcodedb').value;	
                    InputParam  += ParamDelimiter + document.getElementById('hdnDSDAgainstDevInfoDSD').value;	
                    
                   
                    var htmlResp = di_jq.ajax
                    ({
                        type: "POST",
                        url: CallBackPageName,
                        data: "callback=78&param1=" + InputParam,                   
                        async:false,                          
                        success: function(data)
                        {                              
                            try
                            {                       
                                HideLoadingDiv();
                                RemoveMaskingDiv();          
                                if(data)
                                {
                                  document.getElementById('divStructuralComparison').style.display = ""; 
                                  document.getElementById('divStructuralComparison').innerHTML = data;
                                }  
                               
                            }
                            catch(ex)
                            {                
                                HideLoadingDiv();
                                RemoveMaskingDiv();
                                //                                alert("message:" + ex.message);
                             //   
                            }
                        },
                        error:function()
                        {
                             HideLoadingDiv();
                             RemoveMaskingDiv();          
                          //  
                        },
                        cache: false
                    });   
               }
               else
               {
               
                alert(MsgPleaseBrowseDSD);
               
               }
                
             
    }
    catch(ex)
    {
        //        alert("message:" + ex.message);
      //  
    }


}

function CompareUserDSD1AgainstDSD2()
{
   
  try
    {      
               if((document.getElementById('hdnDSD1').value) && (document.getElementById('hdnDSD2').value))
               {
                    
                    ApplyMaskingDiv();
                    ShowLoadingDiv();
                    InputParam  = document.getElementById('hdbnid').value;
                    InputParam  += ParamDelimiter + document.getElementById('hlngcodedb').value;
                    InputParam  += ParamDelimiter + document.getElementById('hdnDSD1').value;	
                    InputParam  += ParamDelimiter + document.getElementById('hdnDSD2').value;		
                   
                    var htmlResp = di_jq.ajax
                    ({
                        type: "POST",
                        url: CallBackPageName,
                        data: "callback=80&param1=" + InputParam,                   
                        async:false,                          
                        success: function(data)
                        {                              
                            try
                            {                       
                                HideLoadingDiv();
                                RemoveMaskingDiv();          
                                if(data)
                                {
                                 
                                  document.getElementById('divStructuralComparison').style.display = ""; 
                                  document.getElementById('divStructuralComparison').innerHTML = data;
                                }  
                               
                            }
                            catch(ex)
                            {                
                                HideLoadingDiv();
                                RemoveMaskingDiv();
                                //                                alert("message:" + ex.message);
                             //   
                            }
                        },
                        error:function()
                        {
                            HideLoadingDiv();
                            RemoveMaskingDiv();          
                          //  
                        },
                        cache: false
                    });   
               }
               else
               {
               
                alert(MsgPleaseBrowseDSD1AndDSD2);
               
               }
                
           
    }
    catch(ex)
    {
        //        alert("message:" + ex.message);
        //
    }

}
function CompareCodelists(ComparisonType)
{
    try
    {      
               
             ApplyMaskingDiv();
             ShowLoadingDiv();
            var strMissingDimensionsMapping = '';
            var strMissingAttributesMapping = '';
            var elem = document.getElementById('frmStructuralComparison').elements;
            for(var i = 0; i < elem.length; i++)
            {

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

            strMissingDimensionsMapping = strMissingDimensionsMapping.substring(strMissingDimensionsMapping, strMissingDimensionsMapping.length-1);
            strMissingAttributesMapping = strMissingAttributesMapping.substring(strMissingAttributesMapping, strMissingAttributesMapping.length-1);
            
            InputParam  = document.getElementById('hdbnid').value;
            InputParam  += ParamDelimiter + document.getElementById('hlngcodedb').value;
            InputParam  += ParamDelimiter + ComparisonType;		
           
            if (ComparisonType == "1" )
            {
                InputParam  += ParamDelimiter + document.getElementById('hdnDSD1').value + "#" + document.getElementById('hdnDSD2').value;	
            }
            else
            {
                InputParam  += ParamDelimiter + document.getElementById('hdnDSDAgainstDevInfoDSD').value;		
            }
            
            InputParam  += ParamDelimiter + strMissingDimensionsMapping;
            InputParam  += ParamDelimiter + strMissingAttributesMapping;
            
           
           
            var htmlResp = di_jq.ajax
            ({
                type: "POST",
                url: CallBackPageName,
                data: "callback=79&param1=" + InputParam,                   
                async:false,                          
                success: function(data)
                {                              
                    try
                    {                       
                         HideLoadingDiv();
                         RemoveMaskingDiv();                   
                        if(data)
                        {
                        
                          document.getElementById('divCodelistComparison').style.display = "";
                          document.getElementById('divCodelistComparison').innerHTML = data;
                          
                        }  
                       
                    }
                    catch(ex)
                    {                
                         HideLoadingDiv();
                         RemoveMaskingDiv();
                         //                        alert("message:" + ex.message);
                      //   
                    }
                },
                error:function()
                {
                     HideLoadingDiv();
                     RemoveMaskingDiv();          
                    //
                },
                cache: false
            });   
              
                 
    }
    catch(ex)
    {
        //        alert("message:" + ex.message);
       // 
    }
}


function ToggleExpandCollapse(event)
{
    var eventRaiser;
    var eventTarget;

    
    if (event.srcElement != null)
    {
        eventRaiser = event.srcElement;
    }
    else if (event.target != null)
    {
        eventRaiser = event.target;
    }
    
    
    eventTarget = '#' + eventRaiser.id.replace("lnk_","div_");
    
    if (eventRaiser.getAttribute('class') == 'expand')
    {
        eventRaiser.setAttribute('class', "collapse");
    }
    else if (eventRaiser.getAttribute('class') == 'collapse')
    {
        eventRaiser.setAttribute('class', "expand");
    }
    
    di_jq(eventTarget).slideToggle();
}

function GenerateReport(ComparisonType)
{
        try
    {      
               
            ApplyMaskingDiv();
            ShowLoadingDiv();
            var strMissingDimensionsMapping = '';
            var strMissingAttributesMapping = '';
            var elem = document.getElementById('frmStructuralComparison').elements;
            for(var i = 0; i < elem.length; i++)
            {
              
             
                if(((elem[i].attributes[1].nodeValue.indexOf("ddlDimensionAlternative_") != -1) && (elem[i].selectedIndex > 0)) || ((elem[i].attributes[2].nodeValue.indexOf("ddlDimensionAlternative_") != -1) && (elem[i].selectedIndex > 0)))
                {
                    if (elem[i].previousElementSibling.previousElementSibling)
                    {
                        strMissingDimensionsMapping += elem[i].previousElementSibling.previousElementSibling.id + "," + elem[i].value + "#" ;
                    }
                    else if (elem[i].previousElementSibling)
                    {
                        strMissingDimensionsMapping += elem[i].previousElementSibling.id + "," + elem[i].value + "#" ;
                    }
                    
                    
                }
                
                
                else if(((elem[i].attributes[1].nodeValue.indexOf("ddlAttributeAlternative_") != -1) && (elem[i].selectedIndex > 0)) || ((elem[i].attributes[2].nodeValue.indexOf("ddlAttributeAlternative_") != -1) && (elem[i].selectedIndex > 0)))
                {
                   if (elem[i].previousElementSibling.previousElementSibling)
                    {
                         strMissingAttributesMapping +=  elem[i].previousElementSibling.previousElementSibling.id + "," + elem[i].value + "#" ;
                    }
                    else if (elem[i].previousElementSibling)
                    {
                         strMissingAttributesMapping +=  elem[i].previousElementSibling.id + "," + elem[i].value + "#" ;
                    }
                   
                }
            } 

            strMissingDimensionsMapping = strMissingDimensionsMapping.substring(strMissingDimensionsMapping, strMissingDimensionsMapping.length-1);
            strMissingAttributesMapping = strMissingAttributesMapping.substring(strMissingAttributesMapping, strMissingAttributesMapping.length-1);
            
            InputParam  = document.getElementById('hdbnid').value;
            InputParam  += ParamDelimiter + document.getElementById('hlngcodedb').value;
            InputParam  += ParamDelimiter + ComparisonType;		
           
            if (ComparisonType == "1" )
            {
                InputParam  += ParamDelimiter + document.getElementById('hdnDSD1').value + "#" + document.getElementById('hdnDSD2').value;	
            }
            else
            {
                InputParam  += ParamDelimiter + document.getElementById('hdnDSDAgainstDevInfoDSD').value;		
            }
            
            InputParam  += ParamDelimiter + strMissingDimensionsMapping;
            InputParam  += ParamDelimiter + strMissingAttributesMapping;
            
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
    catch(ex)
    {
        //        alert("message:" + ex.message);
     //   
    }

}

