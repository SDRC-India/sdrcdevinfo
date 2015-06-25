// JScript File

function onPageLoad(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao)
{
    var breadcrumStr = '';
	setTimeout(CallBack, 3500);
	// ************************************************
	// create Form Tag with hidden input boxes
	// ************************************************
	
	createFormHiddenInputs("frm_dv", "POST");
	SetCommonLinksHref("frm_dv", "POST");

	// ************************************************1
	// set page level variables based on selections or defaults
	// ************************************************1
    setPostedData(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao);

	// ************************************************1
	// Load Language Component
	// ************************************************1
	ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', hlngcode);

    if(hdsby=="topic")
    {
	    // ************************************************1
	    // Load Area Component
	    // ************************************************1
	    //ShowAreaComponent( getAbsURL('stock'), 'area_div', hdbnid, hlngcodedb,'960', '300');
	    
	    // Breadcrum string for area selection        
        breadcrumStr = "<a class='di_gui_link' onclick='BreadCrumPost(\"frm_dv\",\"home.aspx\",\"POST\");'>Search for data by Topic</a>&nbsp;&gt;&nbsp;<a class='di_gui_link' onclick='BreadCrumPost(\"frm_dv\",\"Selection.aspx\",\"POST\");'>Select area</a>&nbsp;&gt;&nbsp;Data View";
    }
    else if(hdsby=="area")
    {
	    // ************************************************1
	    // Load Indicator Component
	    // ************************************************1
	    //ShowIndicatorComponent(getAbsURL('stock'), 'indicator_div', hdbnid, hlngcodedb, hselindo,'960', '300');
	    
	    // Breadcrum string for indicator selection
	    breadcrumStr = "<a class='di_gui_link' onclick='BreadCrumPost(\"frm_dv\",\"home.aspx\",\"POST\");'>Search for data by Geographic area</a>&nbsp;&gt;&nbsp;<a class='di_gui_link' onclick='BreadCrumPost(\"frm_dv\",\"Selection.aspx\",\"POST\");'>Select Indicator</a>&nbsp;&gt;&nbsp;Data View";
        
    }
    else if(hdsby=="qds")
    {   
	    // Breadcrum string for indicator selection
	    breadcrumStr = "<a class='di_gui_link' onclick='BreadCrumPost(\"frm_dv\",\"home.aspx\",\"POST\");'>Home</a>&nbsp;&gt;&nbsp;Data View";        
    }
    
    document.getElementById('breadcrum_div').innerHTML = breadcrumStr;

	
} // end on load function 

/* function to show component (Area or Indicator) in pop up */
function showHideComponent(comp) {
	if(comp=='area') {
		di_jq('#GradOutDiv').show();
		di_jq('#IndOuterBox').hide('slow');
		di_jq('#AreaOuterBox').show('slow');

		if(di_jq('#area_div').html()=='') {
			ShowAreaComponent( getAbsURL('stock'), 'area_div', document.getElementById("hdbnid").value, document.getElementById("hlngcodedb").value, document.getElementById("hselareao").value, '700', '300');
			di_loading('hide');
		}
	}
	else if(comp=='ind') {
		di_jq('#GradOutDiv').show();
		di_jq('#AreaOuterBox').hide('slow');
		di_jq('#IndOuterBox').show('slow');
		
		if(di_jq('#indicator_div').html()=='') {
			ShowIndicatorComponent(getAbsURL('stock'), 'indicator_div', document.getElementById("hdbnid").value, document.getElementById("hlngcodedb").value, document.getElementById("hselindo").value, '700', '300');
		}
	}
	else{
		di_jq('#GradOutDiv').hide();
		di_jq('#AreaOuterBox').hide('slow');
		di_jq('#IndOuterBox').hide('slow');
	}
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

//********************* Start for Grid
function GenerateData()
{
	// calling callbak
	ShowProgressBar(true);
	setTimeout(CallBack, 3500);
	
} 

function onClickOkPopUpButton(type)
{
	var delemeter = "||{~~}||";
	if(type=='area') {
		var areaData = di_get_selected_areas().split(delemeter);
		document.getElementById('hselarea').value = areaData[0];
		document.getElementById('hselareao').value = areaData[1];
	}
	else if(type=='ind') {
		var indData = di_ind_get_selections().split(delemeter);
		document.getElementById('hselind').value = indData[0];
		document.getElementById('hselindo').value = indData[1];
	}
	showHideComponent('close');
	GenerateData();
	delete delemeter;

	
}

function get_data(ResponseText) { 

    document.getElementById('pivot').style.height = (document.body.offsetHeight - document.getElementById('GridContainer').offsetTop - 175) +  "px";// - 

	var DataContent = ResponseText.split("#"); 
	if(DataContent.length==1) {
	    ShowProgressBar(false);
		alert("No data found for selected area.");
		return false;
	}

	var data = [];
	var DataContent = ResponseText.split("#");
	var AllJSONs = DataContent[0].split("}");
	var Areas = [];
	var Indicators = [];
	var Units = [];
	var Sources = [];
	for(var i=0;i<AllJSONs.length;i++){
	    var currentJSON = AllJSONs[i];
	    switch (i){	    
	        case 0:
	            Areas = JSonToArray(currentJSON);
	            break;
	        case 1:
	            Indicators = JSonToArray(currentJSON);
	            break;
	        case 2:
	            Units = JSonToArray(currentJSON);
	            break;
	        case 3:
	            Sources = JSonToArray(currentJSON);
	            break;	            	    
	    }
	}
	
	updateTitles(DataContent[1]);
	var header = DataContent[2].split("|");
	header.splice(header.length-2,1);
	
	var Contents = new Array();
	for(var i=3; i<DataContent.length; i++) {
		Contents[i-3] = DataContent[i];		
	}
	for (var i=0;i<Contents.length;i++) {
	
	    var tmpRow = Contents[i].split("|");
		
		for (var j=0;j<tmpRow.length;j++){
		    
		    switch (j){
		    case header.indexOf("Area"):
		        tmpRow[j] = getStringFromNId(Areas,tmpRow[j]);
		        break; 		
		    case header.indexOf("Indicator"):
		        tmpRow[j] = getStringFromNId(Indicators,tmpRow[j]);
		        break;
		    case header.indexOf("Source"):
		        tmpRow[j] = getStringFromNId(Sources,tmpRow[j]);
		        break;    
		    case tmpRow.length - 2:
		        {
		            if(tmpRow[j] !='-1'){ // If footernote column is traversed
		                tmpRow[j+1] = "<a href='javascript:getFootNote(" + tmpRow[j] + ")'>" + tmpRow[j+1] + "</a>";
		            }
		        }

		    }
		}
		tmpRow.splice(tmpRow.length-2,1);
		data.push(tmpRow);
	}
	var totc="";
	var lastcol = header.length-1;
	var sublist = [];
	var mainlist = [];
	if(header.length > 2) {
		if(di_jq.inArray("Indicator", header)!='-1' && di_jq.inArray("Area", header)!='-1' && di_jq.inArray("Time Period", header)!='-1') { 
			mainlist.push(0);
			mainlist.push(1);
			mainlist.push(2);
			totc = 4;
		}
		/*if(di_jq.inArray("Source", header)!='-1' && di_jq.inArray("Area", header)!='-1') { 
			mainlist.push(0);
			mainlist.push(1);
			mainlist.push(2);
			totc = 4;
		}*/
		else if(di_jq.inArray("Indicator", header)!='-1') {
			mainlist.push(0);
			mainlist.push(1);
			totc = 3;
		}
		else if(di_jq.inArray("Area", header)!='-1') {
			mainlist.push(0);
			mainlist.push(1);
			totc = 3;
		}
		else if(di_jq.inArray("Time Period", header)!='-1') {
			mainlist.push(0);
			mainlist.push(1);
			totc = 3;
		}
		else {
			mainlist.push(0);
			totc = 2;
		}
	} else {
		mainlist.push(0);
		totc = 2;
	}
	
	var tota = totc-1;
	for(m=0; m<header.length-totc; m++) { 
		sublist.push(m+tota);
	} 
    //var pivot = new OAT.Pivot("pivot_content","","pivot_page",header,data,[0,1,2],[3,4,5],[],6,"");
	var pivot = new OAT.Pivot("pivot_content","","pivot_page",header,data,mainlist,sublist,[],lastcol,"");
	ShowProgressBar(false);
	
	
}
//end get_data function

function ShowProgressBar(TrueOrFalse){

var myProgressDiv = document.getElementById('GradOutDiv');

if(TrueOrFalse == true)
{   
    myProgressDiv.style.display = 'block';
}
else
{
    myProgressDiv.style.display = 'none';
}

}

// callback function
function CallBack() 
{    
	var area_nids = document.getElementById('hselarea').value;
	var ius_nid = document.getElementById('hselind').value;
    var InputParam  = area_nids;					                            // AreaNids
    InputParam += ParamDelimiter + ius_nid;		                                // IU_Nids
    InputParam += ParamDelimiter + document.getElementById('hdbnid').value;		// DbNid
    InputParam = '6105,6122,6118,6112,6120,6101,6128,6124,6125,6110,6126,6099,6103,6104,6130,6121,6108,6109,6127,6123,6116,6113,6115,6117,6098,6129,6106,6102,6097,6119,6107,6114,6096,6100,6111[****]1764[****]103';
    //InputParam = '6105,6122,6118,6112,6120,6101,6128[****]235,236,237,238,239,240,241,242,243[****]103';
    //InputParam = '6105,6122,6118,6112,6120,6101,6128[****]11,31[****]103';        
    var htmlResp = di_jq.ajax({
            type: "POST",
            url: "Callback.aspx",
            data: "callback=3&param1=" + InputParam,
            async:false,
            success: function(data){                              
                try
                {
                get_data(data);
										
                }
                catch(ex){
                    ShowProgressBar(false);
                    alert("Error : " + ex.message);
                }
            },
            error:function(){
                
            },
            cache: false
    });
}


function updateTitles(TitleContent)
{
    //alert(TitleContent);
    var tmpTitle = document.getElementById('dataTitle');
	var tmpSubTitle = document.getElementById('dataSubTitle');
	
	var tmpPairs = TitleContent.split("|");
	
	var strTitle = "";
	var strSubTitle = "";
	for(var i=0; i<tmpPairs.length; i++) {
	    var tmpKeyVal = tmpPairs[i].split(":");
		switch (tmpKeyVal[0]){
		    case "Area":
		        strTitle += "," + tmpKeyVal[1];
		        break; 		
		    case "Indicator":
		        strTitle += "," + tmpKeyVal[1];
		        break;
		    case "Unit":
		        strTitle += "-" + tmpKeyVal[1];
		        break;
		    case "Time Period":
		        strTitle += "," + tmpKeyVal[1];
		        break;
		   case "Sub Group":
		        if(tmpKeyVal[1] != "Total")
		        strSubTitle += "," + tmpKeyVal[1]; 
		        break;
		    case "Source":
		        strSubTitle += "," + tmpKeyVal[1]; 
		        break;
		}
		
		
		
	}
	if(strTitle.indexOf(",") == 0) strTitle = strTitle.substring(1);
	if(strSubTitle.indexOf(",") == 0) strSubTitle = strSubTitle.substring(1);
	tmpTitle.innerHTML = strTitle;
	tmpSubTitle.innerHTML = strSubTitle;

}


/* function to load footnotes for any FootNoteNid*/
function getFootNote(FootNoteNid) { 
    var Result = '';
    var FootPath = getAbsURL('stock') + 'data/' + document.getElementById('hdbnid').value + '/ds/en/footnotes/';
	var di_return_first_ic_type_val = '';
	di_jq.ajax({
		type: "GET",
		url: FootPath + FootNoteNid + ".xml",
		dataType: "xml",
		async:false,
		success: function(xml){                              
                try
                {
                var tmpFootNoteVal = xml.lastChild.nodeTypedValue;
                if(tmpFootNoteVal != ''){
                Result = tmpFootNoteVal.toString();
                }
                else{
                    //alert("Blank");
                }
										
                }
                catch(ex){
                    //ShowProgressBar(false);
                    //alert("Error : " + ex.message);
                }
            },
            error:function(){
                //ShowProgressBar(false);
                //
            },
            cache: false
    });

    alert(Result);
	
}



function JSonToArray(strJson){
    var Result = [];
    var startPos = strJson.indexOf("{");
    
    var CollonSeparatedKeyVal = strJson.substring(startPos + 1).split("|");
    
    for(i=0;i<CollonSeparatedKeyVal.length;i++){
        var arrKeyVal = CollonSeparatedKeyVal[i].split(":");
        Result.push(arrKeyVal[0]); //Pushed Nid
        Result.push(arrKeyVal[1]); //Pushed corresponding string value
    }
    return Result; 
}

function getStringFromNId(arrNidsStringPair,StrNId){
    var Result = "";
    for(var i=0; i<arrNidsStringPair.length; i = i + 2){
        if(arrNidsStringPair[i] == StrNId)
        {
            Result = arrNidsStringPair[i+1];
            break;
        }
    }
    return Result;

}

//function prepareFilterParameters(){
//    var Result;
//    if(document.getElementById("chkIsMRD").checked)
//        {
//            Result += ParamDelimiter + "isMRD:1";
//        }
//   else{
//            Result += ParamDelimiter + "isMRD:0";
//       }
//       
//       //alert(Result);
//            
//    return Result;
//    
//}

/*************************************/
/********************** Visualizer Functions ***************/
/*************************************/
function showVisualizer(chartType)
{
    var objOAT=OAT.getSelfInstance();   
    createVisualizer('divVis',chartType,'HighChart(Title)','Sub Title',objOAT);
}

/* Show visualizer component.*/
function createVisualizer(hc_ctrlId,hc_chartType,hc_title,hc_subtitle,objOAT)
{     
    var categoryArray = getCategory(objOAT);
    var seriesArray = getSeriesData(objOAT,categoryArray);
    var chartDataCollection;
    var yAxisTitle=getYaxisText(objOAT);
    
    // Chart input to create graph.
    chartDataCollection = getFinalInputForHighChart(hc_ctrlId,hc_chartType,hc_title,categoryArray,yAxisTitle,seriesArray);    
    var chart = new Highcharts.Chart(chartDataCollection);
}

/* Make series data for HighChart*/
function getSeriesData(objOAT,categoryArray)
{
    var seriesObj=[];
    var seriesColSource;
    var isColumn;
    if(objOAT.colConditions.length==0)
    {
        seriesColSource = objOAT.conditions[1].distinctValues;
        isColumn=false;
    }
    else
    {
        seriesColSource = objOAT.colStructure.items;
        isColumn=true;
    }
    var tempArray=[];
    var totalSeries;
    if(seriesColSource[0].items)
    {
        totalSeries = getSeriesNameCol(seriesColSource,tempArray,"");
    }
    else
    {
        totalSeries = getSeriesName(seriesColSource,tempArray,"");
    }
    // For all series
    for(var k=0;k<totalSeries.length;k++)
    {
        var seriesItem=new Object();            
        var seriesName=[];
        seriesItem.name = totalSeries[k];
        try
        {
            if(totalSeries[k].indexOf('/')>-1)
            {
                seriesName = totalSeries[k].split('/');
            }
            else
            {
                seriesName.push(totalSeries[k]);
            }
        }
        catch(err)
        {
            seriesItem.name = totalSeries[k].value;
            seriesName.push(totalSeries[k].value);
        }
        //seriesItem.stack = (objOAT.allData[i])[1];
        var dataArray = [];         
        var isExistAll;
        // Get data with filtering process
        var rowIndex = objOAT.rowConditions;
        for(var c=0;c<categoryArray.length;c++)
        {
            for(var i=0;i<objOAT.allData.length;i++)
            {
                if(isColumn)
                {
                    for(var j=0;j<seriesName.length;j++)
                    {
                        isExistAll = true;
                        var index =  objOAT.colConditions[j];
                        if(!isMatchAllCondition(seriesName[j],objOAT.allData[i],index,categoryArray[c],rowIndex))
                        {
                            isExistAll = false;
                            break;
                        }                        
                    }
                    if(isExistAll)
                    {                    
                        var temp = (objOAT.allData[i])[objOAT.dataColumnIndex];                                
                        dataArray.push(parseFloat(temp));                 
                    }                 
                }
                else
                {
                    if(seriesColSource[k]==(objOAT.allData[i])[1])
                    {                 
                        var temp = (objOAT.allData[i])[objOAT.dataColumnIndex];
                        dataArray.push(parseFloat(temp));                        
                    }                 
                }
            }            
            seriesItem.data = dataArray
            if(!isColumn)
            {
                break;
            }
        }
        seriesObj.push(seriesItem);
    }
    return seriesObj;
}

/*filterting data with seriesname and categories*/
function isMatchAllCondition(matchingString,sourceArray,colIndex,categoryLabel,rowIndexList)
{
    var retVal=false;
    if(categoryLabel.indexOf('/')==-1)
    {    
        if(matchingString == sourceArray[colIndex] && categoryLabel == sourceArray[rowIndexList[0]])
        {
            retVal = true;
        }     
    }
    else
    {
        var categories = categoryLabel.split('/');
        var isCategoryMatch=true;
        for(var i=0;i<categories.length;i++)
        {
            if(categories[i]!=sourceArray[rowIndexList[i]])
            {
                isCategoryMatch = false;
                break;
            }
        }
        if(isCategoryMatch)
        {
            if(matchingString == sourceArray[colIndex])
            {
                retVal = true;
            }
        }
    }
    
    return retVal;
}


/*Get series name list*/
function getSeriesNameCol(collection,actualSeries,str)
{
    for(var i=0;i<collection.length;i++)
    {
        str += "/" + collection[i].value;        
        if(collection[i].items.length>0)
        {
            getSeriesNameCol(collection[i].items,actualSeries,str);
            str = str.substr(0,str.lastIndexOf("/"));
        }
        else if(!collection[i].items)
        {
            str = str.substr(1);
            actualSeries.push(str);             
            return;
        }        
    }
    return actualSeries;
}

/*Get series name if only 1 series exist*/
function getSeriesName(collection,actualSeries,str)
{
    for(var i=0;i<collection.length;i++)
    {        
        str = collection[i];
        actualSeries.push(str);        
    }
    return actualSeries;
}

/* Get YAxis Title*/
function getYaxisText(objOAT)
{     
    var retVal="";
    var textSource;
    if(objOAT.colConditions.length==0) // if 1 category
    {         
        retVal +=  "/" +objOAT.headerRow[objOAT.dataColumnIndex];
    }
    else                                // if more than 1 category
    {
        textSource = objOAT.colStructure.items;
        for(var i=0;i<textSource.length;i++)
        {
            retVal += "/" + textSource[i].value;
        }
    }     
    retVal = retVal.substr(1);
    return retVal;
}

/* get Category */
function getCategory(objOAT)
{ 
     var catArray=[];
     if(objOAT.colConditions.length==0) // If 1 row
     {
        catArray = objOAT.conditions[objOAT.rowConditions[0]].distinctValues;        
     }
     else
     {                                  // More than 1 row
        var allCategory = objOAT.conditions;
        var indexes = objOAT.rowConditions;
        for(var i=0;i<indexes.length;i++)
        {
            var rows = allCategory[indexes[i]].distinctValues;
            catArray = getCategoryNameCol(rows,catArray);
        }
        
     }      
     return catArray;
}

/* get category list if more than 1 row*/
function getCategoryNameCol(rows,catArray)
{
    if(catArray.length==0)
    {
        for(var i=0;i<rows.length;i++)
        {
            catArray.push(rows[i]);
        }
    }
    else
    {
        var cat=[];
        for(var i=0;i<catArray.length;i++)
        {
            for(var j=0;j<rows.length;j++)
            {
               cat.push(catArray[i] +"/"+ rows[j]);
            }
        }
        catArray = cat;
    }
    return catArray;
}

/* return final input on the basis of chart type*/
function getFinalInputForHighChart(hc_ctrlId,hc_chartType,hc_title,categoryArray,yAxisTitle,seriesArray)
{
    var chartDataCollection;
    switch(hc_chartType.toLowerCase())
    {
        case "column":
            chartDataCollection = {chart:{renderTo:hc_ctrlId,defaultSeriesType:hc_chartType},
            title:{text:hc_title},
            xAxis:{categories:categoryArray,labels:{rotation: -90,align:'right',style:{font: 'normal 13px Verdana, sans-serif'}}},
            yAxis:{min:0,title:{text:yAxisTitle}},
            legend:{layout:'vertical',backgroundColor: '#FFFFFF',align: 'left',verticalAlign: 'top',x: 100,y: 70,floating: true,shadow: true},
            tooltip: {formatter: function() {return ''+this.x +': '+ this.y;}},
            plotOptions: {column: {pointPadding: 0.2,borderWidth: 0}},
            series: seriesArray};
            break;
        case "line":
            chartDataCollection = {chart:{renderTo:hc_ctrlId,defaultSeriesType:hc_chartType},
            title:{text:hc_title},
            xAxis:{categories:categoryArray,labels:{rotation: -90,align:'right',style:{font: 'normal 13px Verdana, sans-serif'}}},
            yAxis:{min:0,title:{text:yAxisTitle}},
            legend:{layout:'vertical',backgroundColor: '#FFFFFF',align: 'left',verticalAlign: 'top',x: 100,y: 70,floating: true,shadow: true},
            tooltip: {formatter: function() {return ''+this.x +': '+ this.y;}},
            plotOptions: {line: {pointPadding: 0.2,borderWidth: 0}},
            series: seriesArray};
            break;
        case "bar":
            chartDataCollection = {chart:{renderTo:hc_ctrlId,defaultSeriesType:hc_chartType},
            title:{text:hc_title},
            xAxis:{categories:categoryArray,labels:{rotation: -90,align:'right',style:{font: 'normal 13px Verdana, sans-serif'}}},
            yAxis:{min:0,title:{text:yAxisTitle}},
            legend:{layout:'vertical',backgroundColor: '#FFFFFF',align: 'left',verticalAlign: 'top',x: 100,y: 70,floating: true,shadow: true},
            tooltip: {formatter: function() {return ''+this.x +': '+ this.y;}},
            plotOptions: {bar: {pointPadding: 0.2,borderWidth: 0}},
            series: seriesArray};
            break;
        case "pie":
            chartDataCollection = {chart:{renderTo:hc_ctrlId,defaultSeriesType:hc_chartType},
            title:{text:hc_title},
            xAxis:{categories:categoryArray,labels:{rotation: -90,align:'right',style:{font: 'normal 13px Verdana, sans-serif'}}},
            yAxis:{min:0,title:{text:yAxisTitle}},
            legend:{layout:'vertical',backgroundColor: '#FFFFFF',align: 'left',verticalAlign: 'top',x: 100,y: 70,floating: true,shadow: true},
            tooltip: {formatter: function() {return ''+this.x +': '+ this.y;}},
            plotOptions: {pie: {pointPadding: 0.2,borderWidth: 0}},
            series: seriesArray};
            break;
    }
    return chartDataCollection;
}