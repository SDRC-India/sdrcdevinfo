function onPageLoad(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hLoggedInUserNId, hLoggedInUserName)
{	
    var hsgcount = 10;
    // Set the active selection to Data Search Navigation link    
    di_jq("#aGalleries").attr("class","navActive")
    
    // Hide background color div
	di_jq("#apDiv2").css("display", "none");
	
	
	createFormHiddenInputs("frmGallery", "POST");
	SetCommonLinksHref("frmGallery", "POST");

    setPostedData(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hsgcount, hLoggedInUserNId, hLoggedInUserName);
    
    if(hdsby != "share")
    {
        InitializeDatabaseComponent(getAbsURL('stock'),'database_div',hdbnid, hlngcodedb, 550, 460, true, "OkDbComponent", "CloseDbComponent");
        ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', hlngcode);
        ShowQuickDSComponent(getAbsURL('stock'), 'quick_data_search_div',hdbnid, hlngcodedb, '100%', '100px', 'GetQDSResult', "", "", "", "", false);
        DrawDatabaseDetailsComponent(hdbnid, 'db_details_div');
    }
}

function SearchGallery(GalleryItemType)
{
    var DbNIds;
    var aGalleryAll, aGalleryTables, aGalleryGraphs, aGalleryMaps;
    
    aGalleryAll = z('aGalleryAll');
    aGalleryTables = z('aGalleryTables');
    aGalleryGraphs = z('aGalleryGraphs');
    aGalleryMaps = z('aGalleryMaps');
    
    DbNIds = di_db_getAllSelDbNIds();
    
    if (DbNIds.split(',').length == 1)
    {
        GetQDSResultForGalleryItemType(GalleryItemType);
    }
    else
    {
        GetASResultForGalleryItemType(GalleryItemType);
    }
    
    if (GalleryItemType == 'A')
    {
        aGalleryAll.className = "navActive";
        aGalleryTables.className = "nav";
        aGalleryGraphs.className = "nav";
        aGalleryMaps.className = "nav";
    }
    else if (GalleryItemType == 'T')
    {
        aGalleryAll.className = "nav";
        aGalleryTables.className = "navActive";
        aGalleryGraphs.className = "nav";
        aGalleryMaps.className = "nav";
    }
    else if (GalleryItemType == 'G')
    {
        aGalleryAll.className = "nav";
        aGalleryTables.className = "nav";
        aGalleryGraphs.className = "navActive";
        aGalleryMaps.className = "nav";
    }
    else if (GalleryItemType == 'M')
    {
        aGalleryAll.className = "nav";
        aGalleryTables.className = "nav";
        aGalleryGraphs.className = "nav";
        aGalleryMaps.className = "navActive";
    }
}

function GetQDSResult()
{
    GetQDSResultForGalleryItemType("A");
}

function GetQDSResultForGalleryItemType(GalleryItemType)
{
    var InputParam, IndicatorIC, Areas;
    var spanLoading, spanLoadingOuter, tblQDSResults, divQDSResults, spanTimeCounter;
    
    IndicatorIC = di_get_selected_indicators_values();
    Areas = di_get_selected_areas_values();
    
    spanLoading = z('spanLoading');
    spanLoadingOuter = z('spanLoadingOuter');
    divQDSResults = z('divQDSResults');
    tblQDSResults = z('tblQDSResults');
    spanTimeCounter = z('spanTimeCounter');
    
    if((IndicatorIC != null && IndicatorIC != "") || (Areas != null && Areas != ""))
    {
        spanLoadingOuter.style.display = "inline";
        spanLoading.innerHTML = "Searching the database...";
        divQDSResults.innerHTML = "";
        spanTimeCounter.innerHTML = "";
        
        InputParam = Get_Indicators_from_components_IndicatorIC(IndicatorIC);
        InputParam += ParamDelimiter + Get_IC_from_components_IndicatorIC(IndicatorIC);
        InputParam += ParamDelimiter + Areas;
        InputParam += ParamDelimiter + z('hlngcode').value;
        InputParam += ParamDelimiter + z('hdbnid').value;
        InputParam += ParamDelimiter + GalleryItemType;
        
        try
        {
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: CallBackPageName,
                data: {'callback': '30', 'param1': InputParam},
                async: true,
                success: function(data){                              
                    try
                    {
                        if (data.split(ParamDelimiter)[0] != null && data.split(ParamDelimiter)[0] != '')
                        {
                            tblQDSResults.style.display = "";
                            if (GalleryItemType == 'A')
                            {
                                z('aGalleryAll').className = "navActive";
                            }
					    }
					    
					    divQDSResults.innerHTML = data.split(ParamDelimiter)[0];	
					    spanTimeCounter.innerHTML = data.split(ParamDelimiter)[1];	
					    
					    di_qds_resize_ind_blocks();
					    di_qds_resize_area_blocks();
					    
					    spanLoadingOuter.style.display = "none";
		                spanLoading.innerHTML = "";
                    }
                    catch(ex){
                        alert("message:" + ex.message);
                    }
                },
                error:function(){
                    
                },
                cache: false
            });
        }
        catch(ex){        
        }
    }
    else
    {
        tblQDSResults.style.display = "none";
        divQDSResults.innerHTML = "";	
		spanTimeCounter.innerHTML = "";
		spanLoadingOuter.style.display = "none";
		spanLoading.innerHTML = "";
    }
}

function GetASResult()
{
    GetASResultForGalleryItemType("A");
}

function GetASResultForGalleryItemType(GalleryItemType)
{
    var IndicatorIC, Areas, LngCode, ASDatabaseIds;
    var IndicatorText, IndicatorBlockText, AreaText, AreaBlockText;
    var spanLoading, spanLoadingOuter, tblQDSResults, divQDSResults, spanTimeCounter;
    
    IndicatorText = di_qds_get_ind_search_data();
    IndicatorBlockText = di_qds_get_all_ind_block_data();
    AreaText = di_qds_get_area_search_data();
    AreaBlockText = di_qds_get_all_area_block_data();
    
    spanLoading = z('spanLoading');
    spanLoadingOuter = z('spanLoadingOuter');
    spanTimeCounter = z('spanTimeCounter');
    divQDSResults = z('divQDSResults');
    tblQDSResults = z('tblQDSResults');
    
    IndicatorIC = GetTextAndBlockTextCombinedValue(IndicatorText, IndicatorBlockText);
    Areas = GetTextAndBlockTextCombinedValue(AreaText, AreaBlockText);
    LngCode = z('hlngcode').value;
    ASDatabaseIds = di_db_getAllSelDbNIds();
    
    if(((IndicatorIC != null && IndicatorIC != "") || (Areas != null && Areas != "")) && 
        (ASDatabaseIds != null && ASDatabaseIds != ""))
    {
        divQDSResults.innerHTML = "";
        spanTimeCounter.innerHTML = "";
        GetASResultForDBNumber(IndicatorIC, Areas, LngCode, ASDatabaseIds, 0, 0, 0, GalleryItemType);
    }
    else
    {
        tblQDSResults.style.display = "none";
        divQDSResults.innerHTML = "";	
		spanTimeCounter.innerHTML = "";
		spanLoadingOuter.style.display = "none";
		spanLoading.innerHTML = "";
    }
}

function ShareResult(IdUniquePart)
{
    var InputParam;
    
    InputParam = outerHTML(z("DIUA_" + IdUniquePart));
    InputParam += ParamDelimiter + "share";
    InputParam += ParamDelimiter + z('hdbnid').value;
    InputParam += ParamDelimiter + z('hselarea').value;
    InputParam += ParamDelimiter + z('hselind').value;
    InputParam += ParamDelimiter + z('hlngcode').value;
    InputParam += ParamDelimiter + z('hlngcodedb').value;
    InputParam += ParamDelimiter + z('hselindo').value;
    InputParam += ParamDelimiter + z('hselareao').value;
    
    try
    {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: {'callback': '32', 'param1': InputParam},
            async: true,
            success: function(data){                              
                try
                {
					SocialSharing("You should check this out!","DevInfo", getAbsURL('stock/shared/Gallery') + data + ".htm");
                }
                catch(ex){
                    alert("message:" + ex.message);
                }
            },
            error:function(){
                
            },
            cache: false
        });
    }
    catch(ex){        
    }
}

function GetASResultForDBNumber(IndicatorIC, Areas, LngCode, ASDatabaseIds, ASDBNumber, NumResults, ElapsedTime, GalleryItemType)
{
    var InputParam;
    var DBId, DBName;
    var spanLoading, spanLoadingOuter, tblQDSResults, divQDSResults, spanTimeCounter;
    
    spanLoading = z('spanLoading');
    spanLoadingOuter = z('spanLoadingOuter');
    divQDSResults = z('divQDSResults');
    tblQDSResults = z('tblQDSResults');
    spanTimeCounter = z('spanTimeCounter');
    InputParam += ParamDelimiter + GalleryItemType;
    
    DBId = ASDatabaseIds.split(",")[ASDBNumber];
    DBName = di_db_getAllSelDbNames().split(",")[ASDBNumber];
    
    if((IndicatorIC != null && IndicatorIC != "") || (Areas != null && Areas != ""))
    {
        spanLoadingOuter.style.display = "inline";
        spanLoading.innerHTML = "Searching " + DBName + " database...";
    
        InputParam = IndicatorIC;
        InputParam += ParamDelimiter + Areas;
        InputParam += ParamDelimiter + LngCode;
        InputParam += ParamDelimiter + DBId;
        InputParam += ParamDelimiter + NumResults;
        InputParam += ParamDelimiter + ElapsedTime;
        
        try
        {
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: CallBackPageName,
                data: {'callback': '31', 'param1': InputParam},
                async: true,
                success: function(data){                              
                    try
                    {
                        if (data.split(ParamDelimiter)[0] != null && data.split(ParamDelimiter)[0] != '')
                        {
                            tblQDSResults.style.display = "";
                            if (GalleryItemType == 'A')
                            {
                                z('aGalleryAll').className = "navActive";
                            }
					    }
					    
					    divQDSResults.innerHTML = divQDSResults.innerHTML + data.split(ParamDelimiter)[0];	
				        spanTimeCounter.innerHTML = data.split(ParamDelimiter)[1];	
					    
				        di_qds_resize_ind_blocks();
				        di_qds_resize_area_blocks();
					        
				        NumResults = data.split(ParamDelimiter)[1].split(' ')[0];
				        ElapsedTime = data.split(ParamDelimiter)[1].split(' ')[data.split(ParamDelimiter)[1].split(' ').length - 2];
	                    
                        ASDBNumber = ASDBNumber + 1;
                        if (ASDBNumber < ASDatabaseIds.split(",").length)
                        {
				            GetASResultForDBNumber(IndicatorIC, Areas, LngCode, ASDatabaseIds, ASDBNumber, NumResults, ElapsedTime, GalleryItemType);
				        }	
				        else
				        {
				            spanLoadingOuter.style.display = "none";
	                        spanLoading.innerHTML = "";
				        }
                    }
                    catch(ex){
                        alert("message:" + ex.message);
                    }
                },
                error:function(){
                    
                },
                cache: false
            });
        }
        catch(ex){        
        }
    }
    else
    {
        tblQDSResults.style.display = "none";
        divQDSResults.innerHTML = "";	
		spanTimeCounter.innerHTML = "";
		spanLoadingOuter.style.display = "none";
        spanLoading.innerHTML = "";
    }
}

function GetTextAndBlockTextCombinedValue(Text, BlockText)
{
    var RetVal;
    
    RetVal = "";
    if (Text != null && Text != '')
    {
        Text = ReplaceAll(Text, ",", "||");
        RetVal = Text;
        
        if (BlockText != null && BlockText != '')
        {
            RetVal = RetVal + "||" + BlockText;
        }
    }
    else
    {
        if (BlockText != null && BlockText != '')
        {
            RetVal = BlockText;
        }
    }
    
    return RetVal;
}

function Get_Indicators_from_components_IndicatorIC(IndicatorIC)
{
    var Indicators;
    
    Indicators = '';
    for(var i = 0; i < IndicatorIC.length; i++)
    {
        if (IndicatorIC[i].split('_')[1] == 1)
        {
            Indicators = Indicators + IndicatorIC[i].split('_')[0] + ",";
        }
    }
        
    if (Indicators.length > 0)
    {
        Indicators = Indicators.substr(0, Indicators.length - 1);
    }
    
    return Indicators;
}

function Get_IC_from_components_IndicatorIC(IndicatorIC)
{
    var ICs;
    
    ICs = '';
    for(var i = 0; i < IndicatorIC.length; i++)
    {
        if (IndicatorIC[i].split('_')[1] == 0)
        {
            ICs = ICs + IndicatorIC[i].split('_')[0] + ",";
        }
    }
        
    if (ICs.length > 0)
    {
        ICs = ICs.substr(0, ICs.length - 1);
    }
    
    return ICs;
}

function GetCallbackPageLocation()
{
    var RetVal;
    
    if (z('hdsby').value == 'share')
    {
        RetVal = "../../../libraries/aspx/" + CallBackPageName;
    }
    else
    {
        RetVal = CallBackPageName;
    }
    
    return RetVal;
}

