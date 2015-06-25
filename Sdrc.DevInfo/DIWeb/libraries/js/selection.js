function onPageLoad(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao)
{	
	var hsgcount = 10;
    var breadcrumStr = '';
    
    // Set the active selection to Data Search Navigation link    
    di_jq("#aDS").attr("class","navActive")
    
    // Hide background color div
	di_jq("#apDiv2").css("display", "none");

	// ************************************************
	// create Form Tag with hidden input boxes
	// ************************************************
	createFormHiddenInputs("frmSelection", "POST");
	SetCommonLinksHref("frmSelection", "POST");

	// ************************************************1
	// set page level variables based on selections or defaults
	// ************************************************1
    setPostedData(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hsgcount);

	// ************************************************1
	// Load Language Component
	// ************************************************1
	ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', hlngcode);

        
    if(hdsby=="topic")
    {       
	    // ************************************************1
	    // Load Area Component
	    // ************************************************1
	    ShowAreaComponent( getAbsURL('stock'), 'area_div', hdbnid, hlngcodedb, hselareao, '100%', '300');
	    
	    // Breadcrum string for area selection        
        breadcrumStr = "<a class='di_gui_link' onclick='BreadCrumPost(\"frmSelection\",\"home.aspx\",\"POST\");'>Search for data by Topic</a>&nbsp;&gt;&nbsp;Selection"; 
    }
    else if(hdsby=="area")
    {    
	    // ************************************************1
	    // Load Indicator Component
	    // ************************************************1
	    ShowIndicatorComponent(getAbsURL('stock'), 'indicator_div', hdbnid, hlngcodedb, hselindo,'100%', '300', hsgcount);
	    
	    // Breadcrum string for indicator selection
	    breadcrumStr = "<a class='di_gui_link' onclick='BreadCrumPost(\"frmSelection\",\"home.aspx\", \"POST\");'>Search for data by Geographic area</a>&nbsp;&gt;&nbsp;Selection"; 
        
    }
    
    document.getElementById('breadcrum_div').innerHTML = breadcrumStr;
}

function onGotoDataViewPg()
{
	if (document.getElementById('hdsby').value=="area")
	{
	    // Set Indicator selections in the hidden text box
	    var sIndSel=di_ind_get_selections().split("||{~~}||");
	    document.getElementById('hselind').value = sIndSel[0];
	    document.getElementById('hselindo').value = sIndSel[1];
	}
	else
	{
	    // Set page selection data in hidden text boxes
	    //document.getElementById('hselarea').value = di_get_selected_areas();
	    
	    var sAreaSel=di_get_selected_areas().split("||{~~}||");
	    document.getElementById('hselarea').value = sAreaSel[0];
	    document.getElementById('hselareao').value = sAreaSel[1];
	}
	// Post the data
	PostData("frmSelection", "dataview.aspx", "POST")
}


