/*************************************/
/********************** Visualizer Functions ***************/
/*************************************/

var legendFontcolorCode = "#000000";
var mapDataDelemeter = '[****]';
var mapOverlay;
var isDisputedBdriesLoaded = 'no';
var themePreviousSelVal = '';
function showVisualizer(chartType) {
    // show/hide map icons
    z("clickDownload").style.display = "block";
    z("clickDownloadLabel").style.display = "block";
    z("saveGallerySpan").style.display = "block";
    z("shareSpan").style.display = "block";
    z("arrangeRow").style.display = "none";
    z("pivotRow").style.display = "block";
    z("swapRow").style.display = "block";
    z("langShare").style.display = "block";
    //z("setting_tab").style.display = "block";
    if (chartType == "table" || z("tab-d").className == "current") {
        z("arrangeRow").style.display = "block";
        return;
    }
    z("settings_section1").style.display = z("settings_section2").style.display = z("settings_section4").style.display = "block";
    if (chartType == "map") {
        z("pivotRow").style.display = "none";
        z("pivotRow").style.display = "none";
        z("swapRow").style.display = "none";
    }
    if (chartType == "map2d" || chartType == "map3d") {
        z("clickDownload").style.display = "none";
        z("clickDownloadLabel").style.display = "none";
        z("saveGallerySpan").style.display = "none";
        z("shareSpan").style.display = "none";
        z("langShare").style.display = "none";
    }
    //applyControlSettings(chartType)
    storeTreemapHtml();
    z("visChartDiv").innerHTML = "";
    var objOAT = OAT.getSelfInstance();
    var chartObj = di_getChartObject(chartVisualizationDecided);
    if (chartObj == undefined) {
        var hc_chartTitle = z('dataTitle').innerHTML;
    }
    else {
        if (chartType != "radar" && chartType != "treemap") {
            var hc_chartTitle = chartObj.options.title.text;
        }
        else {
            var hc_chartTitle = z('dataTitle').innerHTML;
        }
    }

    hc_chartTitle = hc_chartTitle.replace(/&amp;/g, "&");

    if (chartObj) {
        if (chartType != "radar" && chartType != "scatter3d") {
            if (chartType == "treemap") { // Redraw treemap
                z("clickDownload").style.display = "none";
                z("clickDownloadLabel").style.display = "none";
                z("visChartDiv").innerHTML = createHTMLforTreeOrCloud(chartType);
                var treeRowNodes = getDataStructure(objOAT);
                var allSeriesName = [];
                if (objOAT.colConditions.length > 0)
                    allSeriesName = getSeriesNameList(objOAT);
                else
                    allSeriesName.push("Data Value");
                treeMapDataCollection = getTreeMapData(objOAT, treeRowNodes, allSeriesName);
                redrawTreeMap(chartVisualizationDecided, 'di_vc_treeContainer', treeMapDataCollection, hc_chartTitle);
                setTimeout("setTreemapHtml()", 1000);
            }
            else {
                var chartWidth = z("tab-vsection").style.width;
                if (chartWidth == "")
                    chartWidth = "898px";
                else {
                    //  chartWidth = parseInt(chartWidth.substr(0, chartWidth.length - 2));	
                }
                //alert(chartWidth);
                var categoryArray = getCategory(objOAT); // Get Category Data
                var seriesArray = getSeriesData(objOAT, categoryArray); // Get Series Data
                categoryArray = properCategoryText(categoryArray);
                var chartData = {
                    categoryCollection: categoryArray,
                    seriesCollection: seriesArray
                }
                chartData = FilterOutStringData(chartData);
                if (chartType == "pie" || chartType == "pyramid") { // Redraw pie and pyramid
                    chartData.categoryCollection = categoryArray;
                    chartData.seriesCollection = seriesArray;
                    if (chartType == "pie")
                        di_createCategoryList(chartData, 'di_vcSelSeries');
                    di_reDrawChart(chartVisualizationDecided, chartObj, chartData, chartType, hc_chartTitle, "", "", "");
                }
                else if (chartType == "cloud") { //Redraw Cloud
                    z("clickDownload").style.display = "none";
                    z("clickDownloadLabel").style.display = "none";
                    z("visChartDiv").innerHTML = createHTMLforTreeOrCloud(chartType);
                    chartData = FilterOutStringData(chartData);
                    di_redrawCloud(chartVisualizationDecided, 'visChartDiv', chartData, hc_chartTitle);
                    setTimeout("setTreemapHtml()", 1000);
                }
                else { // Redraw other visualizer
                    var xTitle = "";
                    var yTitle = "";
                    if (chartType == "scatter2d") {
                        xTitle = scatterXTitle(chartData);
                        yTitle = scatterYTitle(chartData);
                    }
                    chartData = FilterOutStringData(chartData);
                    di_reDrawChart(chartVisualizationDecided, chartObj, chartData, chartType, hc_chartTitle, xTitle, yTitle, "");
                }
            }
        }
        else // Redraw Radar visualizer
        {
            if (chartType == "pyramid") {
                z('visChartDiv').style.display = "none";
            }
            else {
                z('visChartDiv').style.display = "";
            }
            createVisualizer('visChartDiv', chartType, hc_chartTitle, '', objOAT, true);
        }
    }
    else // When 1st tym draw visualizer
    {
        if (chartType == "pyramid") {
            z('visChartDiv').style.display = "none";
        }
        else {
            z('visChartDiv').style.display = "";
        }
        createVisualizer('visChartDiv', chartType, hc_chartTitle, '', objOAT, false);
    }
    if (chartType != "radar") {
        setTimeout("applyControlsSettings()", 1000);
    }
    else if (chartObj != null && chartType == "radar") {
        setTimeout("applyControlsSettings()", 1000);
    }
}

/*This is used for refreshing chart object but not in use righ now*/
function refreshChart() {
    var objOAT = OAT.getSelfInstance();
    createVisualizer('visChartDiv', chartType, hc_chartTitle, '', objOAT, true);
}

/* function to apply settings */
function applyVisSetting(uid) {

    switch (chartVisualizationDecided) {
        case ("table"):
            {
                break;
            }
        default:
            {
                // Set Visualizer UI  mycircle = { area, no, id } mycircle.id
                di_setHideShowUI(uid);
                //di_setColumnDataLabelChkbox(uid);
                //di_setLegendUI(uid);
                //di_setChartAndPlotareaBgUI(uid);
                //di_setDropdownlist('di_vcTitlesList'+uid,title);
                //di_vcSelTitleVal(title,uid);
                //var titleListCtrl = z('di_vcTitlesList'+uid);
                //di_setChartTitleUI(uid, titleListCtrl);
                //di_setDropdownlist('di_vcSelLegend'+uid,'6');

                break;
            }
    }
}
/*  check for valid numeric strings	*/
function IsNumeric(strString) {
    var strValidChars = "0123456789.-";
    var strChar;
    var blnResult = true;

    if (strString.length == 0) return false;

    //  test strString consists of valid characters listed above
    for (i = 0; i < strString.length && blnResult == true; i++) {
        strChar = strString.charAt(i);
        if (strValidChars.indexOf(strChar) == -1) {
            blnResult = false;
        }
    }
    return blnResult;
}

/*If data is NaN or string, it puts null there*/
function FilterOutStringData(dataCol) {
    var seriesCol = dataCol.seriesCollection;
    for (var i = 0; i < seriesCol.length; i++) {
        var dataArray = seriesCol[i].data;
        for (var j = 0; j < dataArray.length; j++) {
            if (!IsNumeric(dataArray[j]) || isNaN(dataArray[j]))
                dataCol.seriesCollection[i].data[j] = null;
        }
    }
    return dataCol;
}

/* Show visualizer component.
Parameters: 
hc_ctrlId : div id where component is need to be drawn.
hc_chartType: chrat type
hc_title: chart title
hc_subtitle: chart subtitle
objOAT: OAT object
isRedraw: false if draw new component otherwise false.
*/
function createVisualizer(hc_ctrlId, hc_chartType, hc_title, hc_subtitle, objOAT, isRedraw) {
    // Variables
    var treeMapDataCollection, categoryArray, seriesArray, xAxisTitle, yAxisTitle;
    var chartData = {
        categoryCollection: null,
        seriesCollection: null
    }

    // conidtion for below code will not work for following array
    var arrayCodeNExecute = ['map', 'map2d', 'map3d'];
    if (jQuery.inArray(hc_chartType.toLowerCase(), arrayCodeNExecute) == -1) {

        // if treemap or cloud then create html markup for title, source and component
        if (hc_chartType.toLowerCase() == "treemap" || hc_chartType.toLowerCase() == "cloud")
            z("visChartDiv").innerHTML = createHTMLforTreeOrCloud(hc_chartType.toLowerCase());


        // Treemap
        if (hc_chartType.toLowerCase() == "treemap") {
            var treeRowNodes = getDataStructure(objOAT);
            var allSeriesName = [];
            if (objOAT.colConditions.length > 0)
                allSeriesName = getSeriesNameList(objOAT);
            else
                allSeriesName.push("Data Value");
            treeMapDataCollection = getTreeMapData(objOAT, treeRowNodes, allSeriesName);
        }
        else {// Other visualizer
            categoryArray = getCategory(objOAT);
            seriesArray = getSeriesData(objOAT, categoryArray);
            categoryArray = properCategoryText(categoryArray);
            chartData.categoryCollection = categoryArray;
            chartData.seriesCollection = seriesArray;
            if (chartData.categoryCollection.length == 0 || chartData.seriesCollection.length == 0)
                return;
            chartData = FilterOutStringData(chartData);
            xAxisTitle = getXaxisText(objOAT);
            xAxisTitle = "";
            yAxisTitle = getYaxisText(objOAT);
            yAxisTitle = "";
        }

        // If component is already exists or redraw component	
        if (isRedraw) {
            if (hc_chartType.toLowerCase() == "radar") {// Radar
                loadingVisualization(true);
                di_createCategoryList(chartData, 'di_vcSelSeries');
                chartData = di_prepairDataForRadar(chartData);
                //var seriesNameCollection = di_radarSeriesColor(chartData);				
                di_initChart(
					null,
					hc_title,
					'radar',
					hc_ctrlId,
					chartData,
					'j.obj',
					'',
					'',
					'',
					'radar',
					'',
					''
					);
                loadingVisualization(false);
            }
            else if (hc_chartType.toLowerCase() == "scatter3d") {// Radar
                loadingVisualization(true);
                di_createCategoryList(chartData, 'di_vcSelSeries');
                chartData = makeBubbleData(chartData);
                di_initChart(
					null,
					hc_title,
					'scatter3d',
					hc_ctrlId,
					chartData,
					'j.obj',
					'',
					'',
					'',
					'scatter3d',
					'',
					''
					);
                loadingVisualization(false);
            }
            else { // Other visualizers
                di_updateChartData(chartVisualizationDecided, chartData);
            }
            return;
        }

    } // end outer if

    switch (hc_chartType.toLowerCase()) {
        case "table":
            toggleVisBtmTab('tab-d');
            break;
        case "column":
            loadingVisualization(true);
            di_initChart(
					'../../stock/templates/column.txt',
					hc_title,
					'column',
					hc_ctrlId,
					chartData,
					'j.obj',
					'',
					'',
					'',
					'column',
					'normal',
					''
				);
            loadingVisualization(false);
            break;
        case "line":
            loadingVisualization(true);
            di_initChart(
					'../../stock/templates/line.txt',
					hc_title,
					'line',
					hc_ctrlId,
					chartData,
					'j.obj',
					'',
					'',
					'',
					'line',
					'normal',
					''
				);
            loadingVisualization(false);
            break;
        case "bar":
            loadingVisualization(true);
            di_initChart(
					'../../stock/templates/bar.txt',
					hc_title,
					'bar',
					hc_ctrlId,
					chartData,
					'j.obj',
					'',
					'',
					'',
					'bar',
					'normal',
					''
				);
            loadingVisualization(false);
            break;
        case "area":
            loadingVisualization(true);
            di_initChart(
					'../../stock/templates/area.txt',
					hc_title,
					'area',
					hc_ctrlId,
					chartData,
					'j.obj',
					'',
					'',
					'',
					'area',
					'normal',
					''
				);
            loadingVisualization(false);
            break;
        case "pie":
            loadingVisualization(true);
            //di_createSeriesList(chartData,"di_seriesPie",true);
            di_createCategoryList(chartData, 'di_vcSelSeries');
            di_initChart(
					'../../stock/templates/pie.txt',
					hc_title,
					'pie',
					hc_ctrlId,
					chartData,
					'j.obj',
					'',
					'',
					'',
					'pie',
					'',
					''
				);
            loadingVisualization(false);
            break;
        case "donut":
            loadingVisualization(true);
            di_draw_donut_chart(
					hc_title, 														// title of the chart
					'do', 																// Unique Id for reference
					'100%', 																// Width
					'600', 																// Height
					hc_ctrlId, 														// Div id of the div in which the component HTML will be drawn.
					chartData, 																// Data collection in form of JSON
					'j.obj', 															// Data collection type j.obj/j.str (Json Object/Json String)
					xAxisTitle, 															// Label for X-Axis
					yAxisTitle, 															// Label for Y-Axis
					['Settings', 'Series', 'Options', 'Save & Export'], 	// Allowed tab names array
					true, 																// Allow to save
					'', 																	// Url where data to be post for saving 			
					'',
					true,
					'closeChartPopup()',
					'shareVisualizer',                                           // Share Hot Selection Function
					'chart',
					'../../stock/templates/donut.txt',
					'source'
				);
            loadingVisualization(false);
            break;
        case "stackcolumn":
            loadingVisualization(true);
            di_initChart(
					'../../stock/templates/column.txt',
					hc_title,
					'stackcolumn',
					hc_ctrlId,
					chartData,
					'j.obj',
					'',
					'',
					'',
					'stackcolumn',
					'stacking',
					''
				);
            loadingVisualization(false);
            break;
        case "stackline":
            loadingVisualization(true);
            di_initChart(
					'../../stock/templates/line.txt',
					hc_title,
					'stackline',
					hc_ctrlId,
					chartData,
					'j.obj',
					'',
					'',
					'',
					'stackline',
					'stacking',
					''
				);
            loadingVisualization(false);
            break;
        case "stackbar":
            loadingVisualization(true);
            di_initChart(
					'../../stock/templates/bar.txt',
					hc_title,
					'stackbar',
					hc_ctrlId,
					chartData,
					'j.obj',
					'',
					'',
					'',
					'stackbar',
					'stacking',
					''
				);
            loadingVisualization(false);
            break;
        case "stackarea":
            loadingVisualization(true);
            di_initChart(
					'../../stock/templates/area.txt',
					hc_title,
					'stackarea',
					hc_ctrlId,
					chartData,
					'j.obj',
					'',
					'',
					'',
					'stackarea',
					'stacking',
					''
				);
            loadingVisualization(false);
            break;
        case "100stackcolumn":
            loadingVisualization(true);
            di_initChart(
					'../../stock/templates/column.txt',
					hc_title,
					'100stackcolumn',
					hc_ctrlId,
					chartData,
					'j.obj',
					'',
					'',
					'',
					'100stackcolumn',
					'percent',
					''
				);
            loadingVisualization(false);
            break;
        case "100stackline":
            loadingVisualization(true);
            di_initChart(
					'../../stock/templates/line.txt',
					hc_title,
					'100stackline',
					hc_ctrlId,
					chartData,
					'j.obj',
					'',
					'',
					'',
					'100stackline',
					'percent',
					''
				);
            loadingVisualization(false);
            break;
        case "100stackbar":
            loadingVisualization(true);
            di_initChart(
					'../../stock/templates/bar.txt',
					hc_title,
					'100stackbar',
					hc_ctrlId,
					chartData,
					'j.obj',
					'',
					'',
					'',
					'100stackbar',
					'percent',
					''
				);
            loadingVisualization(false);
            break;
        case "100stackarea":
            loadingVisualization(true);
            di_initChart(
					'../../stock/templates/area.txt',
					hc_title,
					'100stackarea',
					hc_ctrlId,
					chartData,
					'j.obj',
					'',
					'',
					'',
					'100stackarea',
					'percent',
					''
				);
            loadingVisualization(false);
            break;
        case "treemap":
            z("clickDownload").style.display = "none";
            z("clickDownloadLabel").style.display = "none";
            loadingVisualization(true);
            di_initChart(
					'',
					hc_title,
					'treemap',
					"di_vc_treeContainer",
					treeMapDataCollection,
					'j.obj',
					'',
					'',
					'',
					'treemap',
					'',
					''
				);
            loadingVisualization(false);
            setTimeout("setTreemapHtml()", 1000);
            break;
        case "scatter2d":
            loadingVisualization(true);
            var xTitle = scatterXTitle(chartData);
            var yTitle = scatterYTitle(chartData);
            di_initChart(
					            '../../stock/templates/scatter.txt',
					            hc_title,
					            'scatter2d',
					            hc_ctrlId,
					            chartData,
					            'j.obj',
					            xTitle,
					            yTitle,
					            '',
					            'scatter2d',
					            'normal',
					            ''
				            );
            loadingVisualization(false);
            break;
        case "scatter3d":
            loadingVisualization(true);
            //var xTitle = scatterXTitle(chartData);
            //var yTitle = scatterYTitle(chartData);
            var newDataScatter3d = makeBubbleData(chartData);
            di_initChart(
					            '../../stock/templates/scatter3d.txt',
					            hc_title,
					            'scatter3d',
					            hc_ctrlId,
					            newDataScatter3d,
					            'j.obj',
					            '',
					            '',
					            '',
					            'scatter3d',
					            'normal',
					            ''
				            );
            loadingVisualization(false);
            break;
        case "pyramid":
            loadingVisualization(true);
            di_initChart(
					'../../stock/templates/pyramid.txt',
					hc_title,
					'pyramid',
					hc_ctrlId,
					chartData,
					'j.obj',
					'',
					'',
					'',
					'pyramid',
					'normal',
					''
				);
            loadingVisualization(false);
            break;
        case "radar":
            loadingVisualization(true);
            setTimeout(function () {
                di_createCategoryList(chartData, 'di_vcSelSeries');
                chartData = di_prepairDataForRadar(chartData);
                di_initChart(
					            '../../stock/templates/radar.txt',
					            hc_title,
					            'radar',
					            hc_ctrlId,
					            chartData,
					            'j.obj',
					            '',
					            '',
					            '',
					            'radar',
					            '',
					            ''
				            );
                setTimeout("applyControlsSettings();", 1000);
            }, 1000);
            loadingVisualization(false);
            break;
        case "cloud":
            z("clickDownload").style.display = "none";
            z("clickDownloadLabel").style.display = "none";

            loadingVisualization(true);
            di_initChart(
					'',
					hc_title,
					'cloud',
					'di_vc_treeContainer',
					chartData,
					'j.obj',
					'',
					'',
					'',
					'cloud',
					'',
					''
				);
            loadingVisualization(false);
            setTimeout("setTreemapHtml()", 1000);
            break;
        case "map":
            resetMapMenuOptions('no');

            var imageHeight = 460;
            loadingVisualization(true);
            setTimeout(function () {
                var panelWidth = di_jq('#visChartDiv').width();
                if (mapviewtype == 'single') {
                    var textSelectSr = di_jq('#lngMapSingleThemeSelectIndTxt').html(); // normal case
                    if (OATfilePath != '') { // my data case
                        textSelectSr = di_jq('#lngMapSingleThemeSelectSrTxt').html();
                    }
                    imageHeight = 440;
                    di_jq('#visChartDiv').html('<div id="mapSeriesDropDwnCon" style="height:20px;text-align:center;padding-top:5px;display:none;font-size:14px;">' + textSelectSr + ' <select id="mapSeriesDropDwnbox" onchange="changeMapSingleTheme(this.value)"></select></div><div id="mapTitle" style="height:25px;text-align:center;padding-top:3px;"></div><div id="mapImage" style="height:440px;"></div>');
                }
                else {
                    di_jq('#visChartDiv').html('<div id="mapTitle" style="height:25px;text-align:center;padding-top:5px;"></div><div id="mapImage" style="height:460px;"></div>');
                }
                if (isResetCallback == 0 && mapviewtype == 'single') {
                    var themeCount = mapObj.getMapThemeCount();
                    if (themeCount > 1) isResetCallback = 1;
                }

                if (isResetCallback == 1) {
                    mapObj = new di_Map(); // create map object

                    mapObj.drawMapImage(
							panelWidth, 					// width
							imageHeight, 						// height
							'', 							// series data
							"", 							// area ids
							'mapImage', 					// div id
							z('hdbnid').value, 			// db nid
							z('hlngcodedb').value, 		// db lang code
							mapviewtype						// map type single/multiple
						);
                    // default settings
                    di_jq('#map_dtlabels').attr('checked', false);
                    di_jq('#map_borders').attr('checked', true);
                    di_jq('#map_nrsymbol').attr('checked', false);
                    di_jq('#map_scale').attr('checked', false);
                    resetModeType();

                    // loadingVisualization(false);

                    // create Series UI
                    createSeriesUi();

                    // creating theme info UI
                    //createThemeUi(); commented on 21 May (added in the function "changeMapViewMode()")

                    // set title details
                    var titleType = di_jq('#di_mapSelTitle').val();
                    getMapTitleDetails(titleType);

                    di_jq('#mapSrC_0').removeAttr('class');
                    di_jq('#mapSrC_0').addClass('map_color_palette_1');

                    isResetCallback = 0;
                } else {
                    //mapObj.refreshMap();
                    mapObj.setMapSize(panelWidth, imageHeight);
                    // set title details
                    var titleType = di_jq('#di_mapSelTitle').val();
                    getMapTitleDetails(titleType);

                    //loadingVisualization(false);
                }
                // calling function top change mode single/multiple theme
                changeMapViewMode();
                // hiding fileter - REQUIREMENT CHANGED - No need of hiding filters now, so commenting below code
                //                    if (di_jq('#example5').css('display') != 'none') {
                //                        toggleFiltersPanel();
                //                    }
                setUpTimeLineSlider();
                loadingVisualization(false);
            }, 1000);
            break;
        case "map2d":
            resetMapMenuOptions('no');

            loadingVisualization(true);
            setTimeout(function () {
                di_jq('#visual_icon_tristate_map').hide();
                var panelWidth = di_jq('#visChartDiv').width();
                di_jq('#visChartDiv').html('<div id="mapTitle" style="height:25px;text-align:center;padding-top:5px;"></div><div id="mapImage" style="height:460px;"></div>');
                // initialize map object if does not avaliable
                if (!mapObj) {
                    mapObj = new di_Map(); // create map object
                    mapObj.initMap(
						panelWidth, 					// width
						460, 						// height
						z('hdbnid').value, 			// db nid
						z('hlngcodedb').value			// db lang code
					);
                } // end if 

                // display title of the map
                getMapTitleDetails(1);
                var kmzURL = mapObj.getKMZ();

                // draw 2d map
                var divIdObj = document.getElementById("mapImage");
                //kmzURL = 'http://devinfolive.info/di7beta2/stock/map.kmz';
                initializeG2DMap(divIdObj, kmzURL);

                //loadingVisualization(false);
                // hiding fileter - REQUIREMENT CHANGED - No need of hiding filters now, so commenting below code
                //                    if (di_jq('#example5').css('display') != 'none') {
                //                        toggleFiltersPanel();
                //                    }
                loadingVisualization(false);
            }, 1000);
            //                // for deleting kmz file
            //                mapObj.deleteFile(kmzURL);

            break;
        case "map3d":
            resetMapMenuOptions('no');

            loadingVisualization(true);
            setTimeout(function () {
                di_jq('#visual_icon_tristate_map').hide();

                var panelWidth = di_jq('#visChartDiv').width();
                di_jq('#visChartDiv').html('<div id="mapTitle" style="height:25px;text-align:center;padding-top:5px;"></div><div id="mapImage" style="height:460px;"></div>');
                // initialize map object if does not avaliable
                if (!mapObj) {
                    mapObj = new di_Map(); // create map object
                    mapObj.initMap(
						panelWidth, 					// width
						460, 						// height
						z('hdbnid').value, 			// db nid
						z('hlngcodedb').value			// db lang code
					);
                } // end if 

                // display title of the map
                getMapTitleDetails(1);
                var kmzURL = mapObj.getKMZ();
                var kmzOpacity = 1;

                // draw 3d map
                //kmzURL = 'http://devinfolive.info/di7beta2/stock/map.kmz';
                var ge, networkLink;
                //google.load("earth", "1");

                function initGE() {
                    google.earth.createInstance('mapImage', initCB, failureCB);
                }

                function initCB(instance) {
                    ge = instance;
                    ge.getWindow().setVisibility(true);
                    ge.getNavigationControl().setVisibility(ge.VISIBILITY_SHOW);

                    // Earth is ready, we can add features to it
                    addKmlFromUrl(kmzURL);

                    //google.earth.addEventListener(ge.getWindow(), 'click', doSomething);
                    //google.earth.addEventListener(ge.getGlobe(), 'click', doSomething);
                }
                function failureCB(errorCode) {
                }

                //google.setOnLoadCallback(initGE);
                google.load("earth", "1", { "callback": initGE });

                function addKmlFromUrl(kmlUrl) {
                    var link = ge.createLink('');
                    link.setHref(kmlUrl);

                    networkLink = ge.createNetworkLink('');
                    networkLink.setLink(link);
                    networkLink.setFlyToView(true);

                    ge.getFeatures().appendChild(networkLink);
                    networkLink.setOpacity(kmzOpacity);
                }


                // enable slider
                di_jq("#diMap3DOpacity").slider({
                    orientation: 'vertical',
                    range: "max",
                    min: 1,
                    max: 100,
                    step: 1,
                    value: 100,
                    stop: function (event, ui) {
                        kmzOpacity = ui.value / 100;
                        networkLink.setOpacity(kmzOpacity);
                    }
                });

                //loadingVisualization(false);

                // hiding fileter - REQUIREMENT CHANGED - No need of hiding filters now, so commenting below code
                //                    if (di_jq('#example5').css('display') != 'none') {
                //                        toggleFiltersPanel();
                //                    }

                // for deleting kmz file
                mapObj.deleteFile(kmzURL);
                loadingVisualization(false);
            }, 1000);
            break;
    }
    /*try {

    setTimeout("di_toggleThousandSep("+chartVisualizationDecided+", 7, true, "+PointToDecimal(seriesArray)+", "+chartVisualizationDecided+")",1000);
    }
    catch (err)
    { }*/
}
/* function to show/hide map options for single/multiple theme */
function changeMapViewMode() {

    if (mapviewtype == 'single') {

        var srIdObj = z('mapSeriesDropDwnbox');
        var i = 0;
        di_jq('#di_mapSeries').children('option').each(function () {
            var opval = this.value;
            var optxt = di_jq(this).text();

            srIdObj.options[i] = new Option(optxt, opval);
            i++;
        }); //
        if (i > 1) {
            di_jq('#mapSeriesDropDwnCon').show();
        }
        else {
            di_jq('#mapSeriesDropDwnCon').hide();
        }

        di_jq('#shMapAction').hide();
        di_jq('#shMapAction2').hide();

    }
    else {
        di_jq('#shMapAction').show();
        di_jq('#shMapAction2').show();
    }

    createThemeUi(); // added on 21 May

}
var timeLineTimerClicked = false;
function ShowTimeLine() {
    if (di_jq('#mapTimeLine')[0].checked) {
        timeLineTimerClicked = true;
        TimeLineTimer.init();
        di_jq("#slider-timeline").slider({ disabled: false });
        di_jq('#playBtnImg').show();
        di_jq('#mapTimeLine').attr("title", z('enabledTimelineChkBoxTitle').value);
        var tparray = [];
        if (SliderthArrayData != null && SliderthArrayData.length > 0) {
            tparray = SliderthArrayData[themeID][23];

        }
        else {
            var themeID = di_jq('#di_mapTheme').val();
            var thArrayData = createArrColThemeData(false, themeID);
            SliderthArrayData = thArrayData;
            if (thArrayData != undefined) {
                tparray = thArrayData[themeID][23];
            }
            if (tparray.length > 1) {
                timeLineLegendsInitialized = false;
                previousTp = "a";
                updateMapThemeFromSlider(tparray[0].toString());
                timerTickCount = 0;
                MoveTimeLineSlider();
            }
        }
    }
    else {
        pauseBtnClick();
        timeLineTimerClicked = false;
        di_jq('#playBtnImg').hide();
        di_jq("#slider-timeline").slider({ disabled: true });
        di_jq('#mapTimeLine').attr("title", z('disabledTimelineChkBoxTitle').value);
        updateMapThemeToMrd();
    }
}


function updateMapThemeToMrd() {

    var delemeter = '[****]';
    var themeId = di_jq('#di_mapTheme').val();
    var themeName = SliderthArrayData[themeId][6];
    var themeType = SliderthArrayData[themeId][0];
    var seriesId = SliderthArrayData[themeId][1];
    var chartType = SliderthArrayData[themeId][7];
    var inputStr = themeName + delemeter + themeType + delemeter + seriesId + delemeter + chartType + delemeter;
    var isMrd = true;
    if (themeType == "Chart") {
        isMrd = false;
    }
    var res = mapObj.ediMapTheme(themeId, inputStr, isMrd, '');
    if (res != false) {
        di_jq('#di_mapTheme option[value="' + themeId + '"]').text(themeName);
        redrawThemeDetails(['theme', 'title']);

    }
}

function setUpTimeLineSlider() {
    timeLineTimerClicked = false;
    previousTp = '';
    timePeriodArray = null;
    timeLineToolTip = null;
    SliderthArrayData = null;
    var themeID = di_jq('#di_mapTheme').val();
    var thArrayData = createArrColThemeData(false, themeID);
    SliderthArrayData = thArrayData;
    if (thArrayData != undefined) {
        var tparray = thArrayData[themeID][23]; // theme time period array
        di_jq('#timeLine_cont').html("");
        if (tparray.length > 1 && di_jq('#mapTimeLine')[0] == undefined) {
            di_jq('#timeLine_cont').html(di_jq('#timeLine_cont').html() + '<div style="width:100%; height:100%;float:left; margin-left:0px;"><div style="width:100%; height:10%; float:left; margin-left:0px;"><input type="checkbox" id="mapTimeLine" style="width:16px; height:16px;float:left;" onclick="ShowTimeLine()" title="' + z('disabledTimelineChkBoxTitle').value + '"/><div style="width:200px; padding-left:10px; padding-top:4px; height:15px;float:left;">' + z('timeLineChkBoxValue').value + '</div></div><div  id="timeLineWrapper" style="width:100%; height:90%;margin-left: 0px; float: left;"><div id="timeLinePlay" style="float: left; height:25px; width:4%;"><img id="playBtnImg" title="' + z('playAnimatorBtnTitle').value + '" src="../../stock/themes/default/images/play_btn.png" width="24" height="24" border="0"/></div><div  style="float: left; width: 93.5%; height: 90%; margin-left: 2%; margin-top: 1%;"><div id="slider-timeline"></div><div id="timePeriodLables"  style="width:100%; margin-top:-0.5%; height:20%;float:left;"></div></div></div>');
            di_jq('#playBtnImg').hide();
            di_jq('#mapTimeLine').css("display", "block");
            timePeriodArray = tparray;
            addPlayImageMouseHandlers();
            var i = 0;
            var subMargin = tparray.length * 0.35;
            var percentWidth = 0;
            if (di_jq(window).width() <= 1300) {
                percentWidth = 98 - subMargin;
            }
            else {
                percentWidth = 99 - subMargin;
            }
            var tPMarkerSpacing = (percentWidth - (tparray.length * 0.08)) / (tparray.length - 1);
            var c = 0;
            for (i; i < tparray.length; i++) {
                c = 0;
                if (i != 0) {
                    c = tPMarkerSpacing;
                }
                di_jq('#timePeriodLables').html(di_jq('#timePeriodLables').html() + '<div id="divMarker' + i + '">|</div>');
                di_jq('#divMarker' + i).css({
                    float: 'left',
                    marginLeft: c + '%'
                });
            }
            di_jq('#timePeriodLables').html(di_jq('#timePeriodLables').html() + '<div id="divMarkerYear1">' + tparray[0] + '</div>');
            di_jq('#divMarkerYear1').css({
                float: 'left',
                marginLeft: -1 + '%',
                marginTop: 1 + '%'
            });

            di_jq('#timePeriodLables').html(di_jq('#timePeriodLables').html() + '<div id="divMarkerYear2">' + tparray[tparray.length - 1] + '</div>');
            di_jq('#divMarkerYear2').css({
                float: 'right',
                marginRight: -1 + '%',
                marginTop: 1 + '%'
            });

            timeLineToolTip = di_jq('<div id="timeLineToolTip"/>').css({
                position: 'absolute',
                width: 90,
                top: -25,
                left: -10
            }).hide();
            di_jq("#slider-timeline").slider({
                range: "max",
                min: 0,
                max: tparray.length - 1,
                value: 0,
                slide: timeLineSliderSlide,
                disabled: true,
                animate: false
            }).find(".ui-slider-handle").append(timeLineToolTip).hover(function () {
                timeLineToolTip.show()
            }, function () {
                timeLineToolTip.hide()
            })

        }
    }

}
function setTimeLineHeight() {
    var windowHeight = di_jq(window).height();
    var ij = 0;
    for (ij; ij < timePeriodArray.length; ij++) {
        var offTop = di_jq('#timeLine_cont')[0].offsetParent.offsetTop;
        offTop = offTop + 28;
        var p = (offTop / windowHeight) * 100;
        di_jq('#divMarkerYear' + ij).css({
            top: p + '%'
        });
    }
}
function addPlayImageMouseHandlers() {
    di_jq('#playBtnImg').bind('mouseover', playBtnMOvr);
    di_jq('#playBtnImg').bind('mouseout', playBtnMOut);
    var src = "../../stock/themes/default/images/play_btn.png";
    di_jq('#playBtnImg').attr("src", src);
    di_jq('#playBtnImg').unbind('click', pauseBtnClick);
    di_jq('#playBtnImg').bind('click', playBtnClick);
}
function playBtnMOvr() {
    var src = "../../stock/themes/default/images/play_btn_alt.png";
    di_jq('#playBtnImg').attr("src", src);
}
function playBtnMOut() {
    var src = "../../stock/themes/default/images/play_btn.png";
    di_jq('#playBtnImg').attr("src", src);

}
function playBtnClick() {
    di_jq('#playBtnImg').attr("title", z('stopAnimatorBtnTitle').value);
    removePlayImageMouseHandlers();
    timerTickCount = 0;
    MoveTimeLineSlider();
    TimeLineTimer.start();
}
function pauseBtnClick() {
    di_jq('#playBtnImg').attr("title", z('playAnimatorBtnTitle').value);
    addPlayImageMouseHandlers();
    TimeLineTimer.stop();
}
function removePlayImageMouseHandlers() {
    di_jq('#playBtnImg').unbind('mouseover', playBtnMOvr);
    di_jq('#playBtnImg').unbind('mouseout', playBtnMOut);
    di_jq('#playBtnImg').unbind('click', playBtnClick);
    di_jq('#playBtnImg').bind('click', pauseBtnClick);
    var src = "../../stock/themes/default/images/Pause_btn.png";
    di_jq('#playBtnImg').attr("src", src);
}
var previousTp = '';
var timePeriodArray = null;
var timeLineToolTip = null;
var SliderthArrayData = null;


function timeLineSliderSlide(event, ui) {
    timeLineToolTip.text(timePeriodArray[ui.value]);
    updateMapThemeFromSlider(timePeriodArray[ui.value].toString());
}
var timeLineLegendsInitialized = false;
function updateMapThemeFromSlider(selTP) {
    if (previousTp != selTP) {
        var delemeter = '[****]';
        var themeId = di_jq('#di_mapTheme').val();
        var themeName = SliderthArrayData[themeId][6];
        var themeType = SliderthArrayData[themeId][0];
        var seriesId = SliderthArrayData[themeId][1];
        var chartType = SliderthArrayData[themeId][7];
        var inputStr = themeName + delemeter + themeType + delemeter + seriesId + delemeter + chartType + delemeter;
        if (!timeLineLegendsInitialized) {
            mapObj.UpdateThemeLegends(themeId, inputStr, '', 'false');
            timeLineLegendsInitialized = true;
        }
        mapObj.areaInfoArray = [];
        var res = mapObj.UpdateThemeLegends(themeId, inputStr, selTP, 'true');
        if (res != false) {
            redrawThemeDetails(['detail', 'title']);
        }
        else {
            return res;
        }
        previousTp = selTP;
    }
}
function fadeInImg() {
    di_jq('#diMapImg').fadeIn(3000);
}
function diTimeLinetimer(func, time, autostart) {
    this.set = function (func, time, autostart) {
        this.init = true;
        if (typeof func == 'object') {
            var paramList = ['autostart', 'time'];
            for (var arg in paramList) { if (func[paramList[arg]] != undefined) { eval(paramList[arg] + " = func[paramList[arg]]"); } };
            func = func.action;
        }
        if (typeof func == 'function') { this.action = func; }
        if (!isNaN(time)) { this.intervalTime = time; }
        if (autostart && !this.isActive) {
            this.isActive = true;
            this.setTimer();
        }
        return this;
    };
    this.once = function (time) {
        var timer = this;
        if (isNaN(time)) { time = 0; }
        window.setTimeout(function () { timer.action(); }, time);
        return this;
    };
    this.play = function (reset) {
        if (!this.isActive) {
            if (reset) { this.setTimer(); }
            else { this.setTimer(this.remaining); }
            this.isActive = true;
        }
        return this;
    };
    this.pause = function () {
        if (this.isActive) {
            this.isActive = false;
            this.remaining -= new Date() - this.last;
            this.clearTimer();
        }
        return this;
    };
    this.stop = function () {
        this.isActive = false;
        this.remaining = this.intervalTime;
        this.clearTimer();
        return this;
    };
    this.toggle = function (reset) {
        if (this.isActive) { this.pause(); }
        else if (reset) { this.play(true); }
        else { this.play(); }
        return this;
    };
    this.reset = function () {
        this.isActive = false;
        this.play(true);
        return this;
    };
    this.clearTimer = function () {
        window.clearTimeout(this.timeoutObject);
    };
    this.setTimer = function (time) {
        var timer = this;
        if (typeof this.action != 'function') { return; }
        if (isNaN(time)) { time = this.intervalTime; }
        this.remaining = time;
        this.last = new Date();
        this.clearTimer();
        this.timeoutObject = window.setTimeout(function () { timer.go(); }, time);
    };
    this.go = function () {
        if (this.isActive) {
            this.action();
            this.setTimer();
        }
    };

    if (this.init) {
        return new diTimeLinetimer(func, time, autostart);
    } else {
        this.set(func, time, autostart);
        return this;
    }
};
var TimeLineTimer = new (function () {
    incrementTime = 500, // Timer speed in milliseconds      
        this.init = function () {
            TimeLineTimer.Timer = diTimeLinetimer(TimeLineTimerTick, incrementTime, false);
        };
    this.stop = function () {
        this.Timer.stop();
    };
    this.start = function () {
        this.Timer.play(true);
    };
    this.resetTimer = function () {
        this.Timer.stop().once();
    };
});

var timerTickCount = 0;
function TimeLineTimerTick() {
    MoveTimeLineSlider();
}
function MoveTimeLineSlider() {
    if (timerTickCount < timePeriodArray.length) {
        di_jq("#slider-timeline").slider("option", "value", timerTickCount);
        timeLineToolTip.text(timePeriodArray[timerTickCount]);
        updateMapThemeFromSlider(timePeriodArray[timerTickCount].toString());
        timerTickCount++;
    }
    else {
        TimeLineTimer.stop();
        addPlayImageMouseHandlers();
    }

}


/* function to change map according to the series change from drop down box */
function changeMapSingleTheme(val) {
    var maptype = di_jq('#mapThemeType').attr('title');
    if (maptype == 'Chart') {
        di_jq("#di_mapSeries").multiselect("destroy");
        var seriesLength = di_jq('#di_mapSeries option').length;
        if (seriesLength > 1) {
            di_jq('#di_mapSeries').attr('multiple', 'multiple');
            di_jq('#di_mapSeries option').each(function () { di_jq(this).attr("selected", false); });
            di_jq('#di_mapSeries option[value="' + val + '"]').attr("selected", "selected");
            di_jq("#di_mapSeries").multiselect({
                selectedText: "# of # selected",
                height: 'auto',
                minWidth: 200,
                header: false
            });
        }
        else {
            di_jq("#di_mapSeries").multiselect("destroy");
        }
    }
    else {
        di_jq('#di_mapSeries').val(val);
    }

    changeSeriesBreaks();
    setUpTimeLineSlider();
}
/* function to initialize Google 2D Map */
function initializeG2DMap(divIdObj, kmzURL) {
    kmzURL = addhttpIfNot(kmzURL);
    var myLatlng = new google.maps.LatLng(55.4312553, -10.9418736);
    var myOptions = {
        zoom: 12,
        center: myLatlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };


    /*var map = new google.maps.Map(divIdObj, myOptions);

    var nylayer = new google.maps.KmlLayer(kmzURL,
    { 
    suppressinfowindows: true,
    map: map,
    styles:[{polygonoptions:{fillopacity: 0.5}}]
    }); */

    var map = new google.maps.Map(divIdObj, myOptions);
    var nylayer = new google.maps.KmlLayer(kmzURL);
    nylayer.setMap(map);

    /*kmzURL='http://dev.openlayers.org/releases/OpenLayers-2.11/examples/kml/lines.kml'	  
    var imageBounds = new google.maps.LatLngBounds(
    new google.maps.LatLng(55.4312553, -10.9418736),
    new google.maps.LatLng(55.4312553, -10.9418736));

    mapOverlay = new google.maps.GroundOverlay( kmzURL, imageBounds);
    //set opacity here
    mapOverlay.setOpacity(1);
    mapOverlay.setMap(map);
    */
}
/* function to check if http:// not exist in url */
function addhttpIfNot(url) {
    var i = url.indexOf('http://');
    if (i < 0) {
        url = 'http://' + url;
    }
    return url;
}

/* function to show/hide loading visualization */
function loadingVisualization(show) {
    if (show) {
        di_jq('#divShowLoadingText').html(z('langLoadingVisualization').value);
        di_jq('#divShowLoading').show();
    }
    else {
        di_jq('#divShowLoading').hide();
    }
}
/* function to show/hide loading visualization */
function loadingMapDataProcess(show) {
    if (show) {
        di_jq('#divShowLoadingText').html("Loading Data...");
        di_jq('#divShowLoading').show();
    }
    else {
        di_jq('#divShowLoading').hide();
    }
}

function getSeriesNameList(objOAT) {
    var seriesObj = [];
    var seriesColSource;
    if (objOAT.colConditions.length == 0) {
        seriesColSource = null;
    }
    else {
        seriesColSource = objOAT.colStructure.items;
    }
    var tempArray = [];
    var totalSeries = [];
    if (seriesColSource == null) {
        totalSeries.push("Data Value");
    }
    else {
        if (seriesColSource[0] && seriesColSource[0].items) {
            totalSeries = getSeriesNameCol(seriesColSource, tempArray, "");
        }
        else {
            totalSeries = getSeriesName(seriesColSource, tempArray, "");
        }
    }
    return totalSeries;
}

function getTreeMapData(objOAT, treeRowNodes, allSeriesName) {
    var treeData = {
        id: "root",
        name: "",
        data: {
            $color: ''
        },
        children: new Array()
    };
    if (treeRowNodes == null || allSeriesName == null) {
        return treeData;
    }
    treeData = makeTreeNodes(objOAT.rowStructure, treeData, allSeriesName, "root");
    treeData = enterDataIntoStructure(treeData, objOAT, "");
    return treeData;
    //var temptreedata=JSON.stringify(treeData);
}

/*Make tree nodes*/
function makeTreeNodes(collection, rootNode, allSeriesName, parentNodeId) {
    if (!collection.length) {
        collection = collection.items;
    }
    var iteration = collection.length;
    for (var i = 0; i < iteration; i++) {
        var treeChildNode = {
            id: "",
            name: "",
            data: {
                playcount: 0,
                $area: 0,
                $color: ''
            },
            children: new Array()
        };
        var childNodeId = parentNodeId + "_" + collection[i].value;
        treeChildNode.name = collection[i].value;
        treeChildNode.id = childNodeId;
        rootNode.children.push(treeChildNode);
        if (collection[i].items.length > 0) {
            makeTreeNodes(collection[i].items, treeChildNode, allSeriesName, childNodeId);
        }
        else {
            for (var j = 0; j < allSeriesName.length; j++) {
                if (allSeriesName[j] != "Data Value") {
                    var lastNode = {
                        id: childNodeId + "_" + allSeriesName[j],
                        name: allSeriesName[j],
                        data: {
                            playcount: 0,
                            $area: 0,
                            $color: ''
                        },
                        children: new Array()
                    };
                    treeChildNode.children.push(lastNode);
                }
            }
        }
    }
    return rootNode;
}

/*enter data into tree map nodes
Parameters:
treeStructure:  all tree map without data
objOAT: OAT object
str: fields
*/
function enterDataIntoStructure(treeStructure, objOAT, str) {
    if (treeStructure.id != "root") {
        str += "{@@}" + treeStructure.name;
        var playCountValue = getSumOfMatchValue(str, objOAT.filteredData, objOAT.dataColumnIndex);
        treeStructure.data.playcount = playCountValue;
        treeStructure.data.$area = String(playCountValue);
    }
    treeStructure.data.$color = get_random_color();
    var childNodes = treeStructure.children;
    if (childNodes.length > 0) {
        for (var i = 0; i < childNodes.length; i++) {
            enterDataIntoStructure(childNodes[i], objOAT, str);
        }
    }
    return treeStructure;
}
/* Sum of matched values in record
Parameters:
str: '/' seperated strings
filterdDataCollection: filted data from OAT
dataIndex: data column index in record
*/
function getSumOfMatchValue(str, filterdDataCollection, dataIndex) {
    var sum = 0;
    var matchingStringArray = str.split("{@@}");
    if (filterdDataCollection.length > 0) {
        for (i = 0; i < filterdDataCollection.length; i++) {
            var record = filterdDataCollection[i];
            if (isMatchAllStrings(matchingStringArray, record)) {
                var dataValue = record[dataIndex];
                dataValue = extractDataVale(dataValue);
                if (dataValue.indexOf("<") != 0)
                    sum += parseFloat(dataValue);

                /*else if (dataValue.indexOf(">") == 0) {
                if (dataValue.indexOf(">") != 0)
                sum += parseFloat(dataValue);
                }*/
            }
        }
    }
    return sum.toFixed(2);
}
/*match all condition with record
*/
function isMatchAllStrings(matchingStringArray, record) {
    var isAllMatch = true;
    for (var i = 1; i < matchingStringArray.length; i++) {
        if (record.indexOf(matchingStringArray[i]) == -1) {
            isAllMatch = false;
        }
    }
    return isAllMatch;
}
/* Return random color*/
function get_random_color() {
    var letters = '0123456789ABCDEF'.split('');
    var color = '#';
    for (var i = 0; i < 6; i++) {
        color += letters[Math.round(Math.random() * 15)];
    }
    return color;
}
/* Make series data for HighChart*/
function getSeriesData(objOAT, categoryArray) {
    var seriesObj = [];
    var seriesColSource;
    var isColumn;
    if (objOAT.colConditions.length == 0) {
        seriesColSource = null;
        isColumn = false;
    }
    else {
        seriesColSource = objOAT.colStructure.items;
        isColumn = true;
    }
    var tempArray = [];
    var totalSeries = [];
    if (seriesColSource == null) {
        totalSeries.push("Data Value");
    }
    else {
        if (seriesColSource[0] && seriesColSource[0].items) {
            totalSeries = getSeriesNameCol(seriesColSource, tempArray, "");
        }
        else {
            totalSeries = getSeriesName(seriesColSource, tempArray, "");
        }
    }
    // For all series
    var data = objOAT.filteredData;
    if (data.length == 0) {
        data = objOAT.filteredData;
    }
    if (isColumn == false && objOAT.colConditions.length > 0) {
        isColumn = true;
    }
    for (var k = 0; k < totalSeries.length; k++) {
        var seriesItem = new Object();
        var seriesName = [];
        seriesItem.name = totalSeries[k];
        try {
            if (totalSeries[k].indexOf('{@@}') > -1) {
                seriesName = totalSeries[k].split('{@@}');
            }
            else {
                seriesName.push(totalSeries[k]);
            }
        }
        catch (err) {
            seriesItem.name = totalSeries[k].value;
            seriesName.push(totalSeries[k].value);
        }
        //seriesItem.stack = (objOAT.allData[i])[1];
        var dataArray = [];
        var isExistAll;
        // Get data with filtering process
        var rowIndex = objOAT.rowConditions;
        for (var c = 0; c < categoryArray.length; c++) {
            var isDataExist = false;
            for (var i = 0; i < data.length; i++) {
                if (isColumn) {
                    for (var j = 0; j < seriesName.length; j++) {
                        isExistAll = true;
                        var index = objOAT.colConditions[j];
                        if (objOAT.colConditions.length == 0) {
                            index = 1;
                        }
                        if (!isMatchAllCondition(seriesName[j], data[i], index, categoryArray[c], rowIndex)) {
                            isExistAll = false;
                            break;
                        }
                    }
                    if (isExistAll) {
                        isDataExist = true;
                        var dataValueString = (data[i])[objOAT.dataColumnIndex];
                        var dataValue = extractDataVale(dataValueString);
                        dataArray.push(parseFloat(dataValue));
                    }
                }
                else {
                    if (seriesColSource == null) // If there is only 2 column in grid
                    {
                        if (isMatchSpacialCondition(data[i], objOAT.dataColumnIndex, categoryArray[c], rowIndex)) {
                            isExistAll = true;
                            var dataValueString = (data[i])[objOAT.dataColumnIndex];
                            var dataValue = extractDataVale(dataValueString);
                            dataArray.push(parseFloat(dataValue));
                        }
                    }
                    else {
                        if (seriesColSource[k] == (data[i])[1] && categoryArray[c] == (data[i])[0]) {
                            isDataExist = true;
                            var dataValueString = (data[i])[objOAT.dataColumnIndex];
                            var dataValue = extractDataVale(dataValueString);
                            dataArray.push(parseFloat(dataValue));
                        }
                    }
                }
            }
            if (isDataExist == false && seriesColSource != null) {
                dataArray.push(null);
            }
            seriesItem.name = seriesItem.name.replace(/{@@}/g, " ");
            seriesItem.data = dataArray
            /*if(!isColumn && seriesColSource!=null)
            {
            break;
            }*/
        }
        seriesObj.push(seriesItem);
    }
    return seriesObj;
}

//  check for valid numeric strings	
function IsNumeric(strString) {
    var strValidChars = "0123456789.-";
    var strChar;
    var blnResult = true;

    if (strString == null || strString.length == 0) return false;
    //  test strString consists of valid characters listed above
    for (i = 0; i < strString.length && blnResult == true; i++) {
        strChar = strString.charAt(i);
        if (strValidChars.indexOf(strChar) == -1) {
            blnResult = false;
        }
    }
    return blnResult;
}

function extractDataVale(string) {
    var dataValue = string;
    if (string.indexOf("<a") > -1) {
        var dataValuePos = string.indexOf('>') + 1;
        var tempString = string.substr(dataValuePos);
        var anchorEngTagPos = tempString.indexOf("</a>");
        var dataValueString = tempString.substr(0, anchorEngTagPos);
        dataValue = dataValueString;
    }
    return dataValue;
}

/*filterting data with seriesname and categories(When there is 2 column only in grid)*/
function isMatchSpacialCondition(sourceArray, colIndex, categoryLabel, rowIndexList) {
    var retVal = false;
    if (categoryLabel.indexOf('{@@}') == -1) {
        if (categoryLabel == sourceArray[rowIndexList[0]]) {
            retVal = true;
        }
    }
    else {
        var categories = categoryLabel.split('{@@}');
        var isCategoryMatch = true;
        for (var i = 0; i < categories.length; i++) {
            if (categories[i] != sourceArray[rowIndexList[i]]) {
                isCategoryMatch = false;
                break;
            }
        }
        if (isCategoryMatch) {
            //if(matchingString == sourceArray[colIndex])
            //{
            retVal = true;
            //}
        }
    }
    return retVal;
}


/*filterting data with seriesname and categories*/
function isMatchAllCondition(matchingString, sourceArray, colIndex, categoryLabel, rowIndexList) {
    var retVal = false;
    if (categoryLabel.indexOf('{@@}') == -1) {
        if (matchingString == sourceArray[colIndex] && categoryLabel == sourceArray[rowIndexList[0]]) {
            retVal = true;
        }
    }
    else {
        var categories = categoryLabel.split('{@@}');
        var isCategoryMatch = true;
        for (var i = 0; i < categories.length; i++) {
            if (categories[i] != sourceArray[rowIndexList[i]]) {
                isCategoryMatch = false;
                break;
            }
        }
        if (isCategoryMatch) {
            if (matchingString == sourceArray[colIndex]) {
                retVal = true;
            }
        }
    }

    return retVal;
}


/*Get series name list*/
function getSeriesNameCol(collection, actualSeries, str) {
    for (var i = 0; i < collection.length; i++) {
        str += "{@@}" + collection[i].value;
        if (collection[i].items.length > 0) {
            getSeriesNameCol(collection[i].items, actualSeries, str);
            str = str.substr(0, str.lastIndexOf("{@@}"));
        }
        else if (!collection[i].items) {
            str = str.substr(4);
            /*if(str.indexOf("/")==0)
            {
            str = str.substr(1);
            }*/
            actualSeries.push(str);
            str = "{@@}" + str.substr(0, str.lastIndexOf("{@@}"));
            //return;
        }
    }
    return actualSeries;
}

/*Get series name if only 1 series exist*/
function getSeriesName(collection, actualSeries, str) {
    for (var i = 0; i < collection.length; i++) {
        str = collection[i].value;
        actualSeries.push(str);
    }
    return actualSeries;
}

/* Get YAxis Title*/
function getYaxisText(objOAT) {
    var retVal = "";
    var textSource;
    if (objOAT.colConditions.length == 0) // if 1 category
    {
        retVal += "/" + objOAT.headerRow[objOAT.dataColumnIndex];
    }
    else                                // if more than 1 category
    {
        textSource = objOAT.colStructure.items;
        for (var i = 0; i < textSource.length; i++) {
            retVal += "/" + textSource[i].value;
        }
    }
    retVal = retVal.substr(1);
    return retVal;
}


/* get data structure for tree map */
function getDataStructure(objOAT) {
    var dataStructure = [];
    var displayData = objOAT.filteredData;
    var rows = objOAT.rowConditions;
    for (var i = 0; i < rows.length; i++) {
        var allCategory = [];
        for (var j = 0; j < displayData.length; j++) {
            var dataPoint = displayData[j];
            allCategory.push(dataPoint[rows[i]]);
        }
        allCategory = allCategory.unique();
        //allCategory = allCategory.sort();
        dataStructure.push(allCategory);
    }
    return dataStructure;
}


/* get Category */
function getCategory(objOAT) {
    var catArray = [];
    var rows = objOAT.rowConditions;
    var rowSource;
    if (objOAT.rowConditions.length == 0) {
        rowSource = null;
        isColumn = false;
    }
    else {
        rowSource = objOAT.rowStructure.items;
        isColumn = true;
    }
    if (rowSource == null) {
    }
    else {
        if (rowSource.length > 0) {
            var lableStr = "";
            var tempArray = [];
            catArray = getSeriesNameCol(rowSource, tempArray, "");
            if (catArray.length > 0) {
                for (var i = 0; i < catArray.length; i++) {
                    lableStr = catArray[i];
                    if (lableStr.indexOf("{@@}") == 0) {
                        lableStr = lableStr.substr(4);
                        catArray[i] = lableStr;
                    }
                }
            }
        }
    }
    return catArray;
}

/* Get XAxis Title*/
function getXaxisText(objOAT) {
    var retVal = "";
    if (objOAT.colConditions.length == 0) // If 1 row
    {
        retVal = objOAT.headerRow[0];
    }
    else                                // if more than 1 category
    {
        var indexes = objOAT.rowConditions;
        for (var i = 0; i < indexes.length; i++) {
            retVal += "/" + objOAT.headerRow[i];
        }
        retVal = retVal.substr(1);
    }
    return retVal;
}

/*Filter Category*/
function filterCategory(blackList, catArray) {
    var tempCatArray = [];
    var isFound;
    for (var i = 0; i < catArray.length; i++) {
        isFound = false;
        for (var j = 0; j < blackList.length; j++) {
            if (catArray[i] == blackList[j]) {
                isFound = true;
                break;
            }
        }
        if (!isFound) {
            tempCatArray.push(catArray[i]);
        }
    }
    return tempCatArray;
}


// Return new array with duplicate values removed
Array.prototype.unique =
  function () {
      var a = [];
      var l = this.length;
      for (var i = 0; i < l; i++) {
          for (var j = i + 1; j < l; j++) {
              // If this[i] is found later in the array
              if (this[i] === this[j])
                  j = ++i;
          }
          a.push(this[i]);
      }
      return a;
  };

  function GetBaseURL() {
      var RetVal;
      var Pathname = window.location.pathname;
      var SiteName = Pathname.split("/")[1];
      RetVal = "http://" + window.location.hostname;
      if (window.location.href.indexOf(":") != -1) {
          RetVal += ":" + window.location.port;
      }
      RetVal += "/" + SiteName;
      return RetVal;
  }

  function shareVisualizer(chartType) {
      
    var SvgImg = getImageData(chartType);
    var chartData; //,imgData;
    var SourceValue = "";
    if (chartType == "radar")
        chartData = di_getRadarCurrentSettings(chartVisualizationDecided);
    else if (chartType == "scatter3d")
        chartData = di_getScatter3dCurrentSettings(chartVisualizationDecided);
    else {
        var chartObj = di_getChartObject(chartVisualizationDecided);
        var chartInput = chartObj.options;
        chartData = JSON.stringify(chartInput);
    }
    //Get the Source and save it to hidden value
    //try {SourceValue = di_getSourceText(chartType);} catch (e) {}    

    var url;
    var storagePath = "../../stock/shared/vc/";
    var htmlResp = di_jq.ajax({
        type: "POST",
        url: "Callback.aspx",
        data: { 'callback': 14, 'param1': chartData, 'param2': storagePath, 'param3': SvgImg, 'param4': chartType },
        async: false,
        success: function (data) {
            try {
                var imgUrl="";
                if (chartType == "radar" || chartType == "scatter3d")
                {
                    url = di_getBaseURL() + "stock/shared/vc/radar.html?id=" + data;
                    imgUrl = di_getBaseURL() + "stock/shared/vc/" + data +".jpg";
                }
                else
                {
                    url = di_getBaseURL() + "stock/shared/vc/vcshare.html?id=" + data;
                    imgUrl = di_getBaseURL() + "stock/shared/vc/" + data +".png";
                }
                SocialSharing("You should check this out!", "DevInfo", url, imgUrl, "Powered By DevInfo", "This is dummy description for testing the facebook sharing feature.");
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


function shareTreemapVisualizer(chartType) {
    var chartObj = di_getChartObject(chartVisualizationDecided);
    var chartInput = chartObj.op.viz.json;
    var url, imgurl;
    var chartData = JSON.stringify(chartInput);
    var storagePath = "../../stock/shared/vc/treemap/";
    var treemapHTML = createHTMLForSharedTreemap('shared');
    var htmlResp = di_jq.ajax({
        type: "POST",
        url: "Callback.aspx",
        data: { 'callback': 14, 'param1': chartData, 'param2': storagePath, 'param3': treemapHTML, 'param4': chartType },
        async: false,
        success: function (data) {
            try {
                url = di_getBaseURL() + "stock/shared/vc/treemap/" + data + ".html";
                imgurl = di_getBaseURL() + "stock/shared/vc/treemap/" + data + ".png";
                SocialSharing("You should check this out!", "DevInfo",url, imgurl, "Powered By DevInfo", "This is dummy description for testing the facebook sharing feature.");
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

function shareCloudVisualizer(chartType) {
    var cloudData = di_getChartObject(chartVisualizationDecided);
    var url;
    var chartData = JSON.stringify(cloudData);
    var storagePath = "../../stock/shared/vc/cloud/";
    var cloudHTML = createHTMLForSharedCloud('shared');
    var htmlResp = di_jq.ajax({
        type: "POST",
        url: "Callback.aspx",
        data: { 'callback': 14, 'param1': chartData, 'param2': storagePath, 'param3': cloudHTML, 'param4': chartType },
        async: false,
        success: function (data) {
            try {
                url = di_getBaseURL() + "stock/shared/vc/cloud/" + data + ".html";
                SocialSharing("You should check this out!", "DevInfo", url, "", "Powered By DevInfo", "This is dummy description for testing the facebook sharing feature.");
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

/*Return base url*/
function di_getBaseURL() {
    var url = window.location.href;
    var index = 0;
    var count = -1;
    var target = 3;
    url = url.substr(0, url.indexOf('libraries'));
    return url;
}

function getQueryStr(qSName) {
    var url = window.location.search.substring(1);
    var url = url.split("&");
    for (var i = 0; i < url.length; i++) {
        var strVal = url[i].split("=");
        if (strVal[0] == qSName) {
            return strVal[1];
        }
    }
}

/*Get data from server on pageload for sharedlink*/
function getChartInputFromServer(chartType) {
    var storagePath = "../../stock/shared/vc/";
    var qs = window.location.search.substring(1); //QueryString
    var qsArray = qs.split("&");
    //var InputParam = qs.split("=")[1]; // key
    var InputParam = qsArray[0].split("=")[1]; // key
    var height = "700";
    var width = "auto";
    var Source = "";

    var w = getQueryStr('w');
    if (w != null && w != '') {
        width = w;
    }
    var h = getQueryStr('h');
    if (h != null && h != '') {
        height = h;
    }

    di_jq(document.body).css("background-image", "none");
    var branding = getQueryStr('branding');
    if (branding != null && branding != '') {
        if (branding == "n") {
            height = window.innerHeight - 10;
        }
    }
    else {
        height = window.innerHeight - 50;
    }

    width = window.innerWidth - 2;
   // alert(height +"\n" + width)

//    if (window.parent.frames.length > 0) {
//        height = window.parent.frames[0].innerHeight - 50;
//        width = (window.parent.frames[0].innerWidth - 2);
//    }

    //var chartInputData;
    var htmlResp = di_jq.ajax({
        type: "POST",
        url: "../../../libraries/aspx/Callback.aspx",
        data: { "callback": 15, "param1": InputParam, "param2": storagePath },
        async: false,
        success: function (sharedata) {
            try {
                var data = sharedata.split("[****]")[0];
                document.title = sharedata.split("[****]")[1] + " - Share";
                if (sharedata.split("[****]")[2] != null && sharedata.split("[****]")[2] != undefined && sharedata.split("[****]")[2] != "undefined") {
                    Source = sharedata.split("[****]")[2];
                }                
                //                var chartInputData = window.JSON.parse(data);
                var chartInputData = di_jq.parseJSON(data);                

                /*try {
                document.title = chartInputData.title.text;
                }
                catch (err) { }*/
                if (chartType == "radar" || chartType == "scatter3d") {
                    SharedRadarSettings = chartInputData;
                    //height = height.replace("px", "");2
                    if (width == "auto") {
                        width = 1200;
                    }
                    else {
                        //width = width.replace("px", "");
                    }
                    LoadSWF("sharedradar", "visDiv", height, width);
                    di_jq("#visDiv").css("width", width + "px");
                    di_jq("#visDiv").css("height", height + "px");
                }
                else {
                    document.getElementById('visDiv').innerHTML = "<div id='" + chartInputData.chart.renderTo + "' style='width: " + width + "px; height: " + height + "px;'></div>";
                    var chart = new Highcharts.Chart(chartInputData);
//                  document.getElementById('visDiv').innerHTML += "<div style='padding-left:40px;padding-bottom:20px'>Source: " + Source + "</div>"
//                  var chart = new Highcharts.Chart(chartInputData, function (chartObj) {
//                       chartObj.renderer.text(Source,20 , height+5).add();
//                  });                    
                }
                var branding = getQueryStr('branding');
                if (branding != null && branding != '') {
                    if (branding == "n") {
                        //di_jq("#logo_lt_dvinf").hide();
                        document.getElementById('logo_lt_dvinf').style.display = 'none';
                    }
                }
            }
            catch (ex) {
                //ShowProgressBar(false);
                alert("Error : " + ex.message);
            }
        },
        error: function () {

        },
        cache: false
    });
}


function isUserAdmin(unid) {
    var isUser = "";
    var htmlResp = di_jq.ajax({
        type: "POST",
        url: "../../../../../libraries/aspx/Callback.aspx",
        data: { "callback": 261, "param1": unid },
        async: false,
        success: function (result) {
            try {
                isUser = result;
            }
            catch (ex) {
                //ShowProgressBar(false);
                alert("Error : " + ex.message);
            }
        },
        error: function () {
            
        },
        cache: false
    });
    return isUser;
}
/*Get data from server on pageload for sharedlink*/
function gettVisualizerFromGallery(chartType) {
    var storagePath = "";
    var qs = window.location.search.substring(1); //QueryString     
    var unid = qs.split("&")[1].split("=")[1]; // key     
    if (isUserAdmin(unid) == "true") {
        storagePath = "../../stock/gallery/public/admin/";
    }
    else {
        storagePath = "../../stock/gallery/private/" + unid + "/";
    }
    var InputParam = qs.split("&")[0].split("=")[1]; // key     
    storagePath += InputParam + "/";
    var htmlResp = di_jq.ajax({
        type: "POST",
        url: "../../../../../libraries/aspx/Callback.aspx",
        data: { "callback": 15, "param1": InputParam, "param2": storagePath },
        async: false,
        success: function (sharedata) {
            try {
                var data = sharedata.split("[****]")[0];
                document.title = sharedata.split("[****]")[1] + " - Share";
                //                 var chartInputData = window.JSON.parse(data);
                var chartInputData = di_jq.parseJSON(data);

                /*try {
                document.title = chartInputData.title.text;
                }
                catch (err) { }*/
                if (chartType == "radar") { // This is for radar & scatter3d both
                    SharedRadarSettings = chartInputData;
                    LoadSWF("sharedradar", "visDiv", 700, 1200);
                }
                else if (chartType == "treemap") {
                    var tm = new $jit.TM.Squarified({
                        //where to inject the visualization
                        injectInto: 'di_vc_treeContainer',
                        //parent box title heights
                        titleHeight: 15,
                        //enable animations
                        animate: animate,
                        // show level
                        levelsToShow: 1,
                        //box offsets
                        offset: 1,
                        //Attach left and right click events
                        Events: {
                            enable: true,
                            onClick: function (node) {
                                if (node) tm.enter(node);
                            },
                            onRightClick: function () {
                                tm.out();
                            }
                        },
                        duration: 1000,
                        //Enable tips
                        Tips: {
                            enable: true,
                            //add positioning offsets
                            offsetX: 20,
                            offsetY: 20,
                            //implement the onShow method to
                            //add content to the tooltip when a node
                            //is hovered
                            onShow: function (tip, node, isLeaf, domElement) {
                                var html = "<div class=\"tip-title\">" + node.name
          + "</div><div class=\"tip-text\">";
                                var data = node.data;
                                if (data.playcount) {
                                    html += "data value: " + data.playcount;
                                }
                                if (data.image) {
                                    html += "<img src=\"" + data.image + "\" class=\"album\" />";
                                }
                                tip.innerHTML = html;
                            }
                        },
                        //Add the name of the node in the correponding label
                        //This method is called once, on label creation.
                        onCreateLabel: function (domElement, node) {
                            domElement.innerHTML = node.name;
                            var style = domElement.style;
                            style.display = '';
                            style.border = '1px solid transparent';
                            domElement.onmouseover = function () {
                                style.border = '1px solid #9FD4FF';
                            };
                            domElement.onmouseout = function () {
                                style.border = '1px solid transparent';
                            };
                        }
                    });
                    tm.loadJSON(chartInputData);
                    tm.refresh();
                    //add event to the back button
                    var back = $jit.id('di_vc_GoRootbtn');
                    $jit.util.addEvent(back, 'click', function () {
                        tm.out();
                    });
                }
                else if (chartType == "cloud") {
                    di_jq("#di_vc_treeContainer").jQCloud(chartInputData);
                }
                else {                    
                    document.getElementById('visDiv').innerHTML = "<div id='" + chartInputData.chart.renderTo + "' style='width: auto; height: 700px;'></div>";
                    var chart = new Highcharts.Chart(chartInputData);
                }
            }
            catch (ex) {
                //ShowProgressBar(false);
                alert("Error : " + ex.message);
            }
        },
        error: function () {
            
        },
        cache: false
    });
}

/*Get data from server on pageload for sharedlink*/
function getTreeInputFromServer() {
    var storagePath = "../../stock/shared/vc/treemap/";
    var url = window.location.href;
    var index = url.lastIndexOf("/");
    var InputParam = url.substr(index + 1);
    InputParam = InputParam.substr(0, InputParam.indexOf("."))  // key
    var Height = 0, Width = 0;
    di_jq(document.body).css("background-image", "none");

    var branding = getQueryStr('branding');
    if (branding != null && branding != '') {
        if (branding == "n") {
            height = window.innerHeight - 10;
        }
    }
    else {
        height = window.innerHeight - 50;
    }

        di_jq("table").height(Height);
        Width = window.innerWidth - 4;
        di_jq("table").width(Width);

        var htmlResp = di_jq.ajax({
            type: "POST",
            url: "../../../../libraries/aspx/Callback.aspx",
            data: { "callback": 15, "param1": InputParam, "param2": storagePath },
            async: false,
            //dataType: "json",
            //contentType: "application/json",
            success: function (sharedata) {
                try {
                    var data = sharedata.split("[****]")[0];
                    document.title = sharedata.split("[****]")[1] + " - Share";
                    var treemapData = di_jq.parseJSON(data);
                    //var treemapData = window.JSON.parse(data);
                    var tm = new $jit.TM.Squarified({
                        //where to inject the visualization
                        injectInto: 'di_vc_treeContainer',
                        //parent box title heights
                        titleHeight: 15,
                        //enable animations
                        animate: animate,
                        // show level
                        levelsToShow: 1,
                        //box offsets
                        offset: 1,
                        //Attach left and right click events
                        Events: {
                            enable: true,
                            onClick: function (node) {
                                if (node) tm.enter(node);
                            },
                            onRightClick: function () {
                                tm.out();
                            }
                        },
                        duration: 1000,
                        //Enable tips
                        Tips: {
                            enable: true,
                            //add positioning offsets
                            offsetX: 20,
                            offsetY: 20,
                            //implement the onShow method to
                            //add content to the tooltip when a node
                            //is hovered
                            onShow: function (tip, node, isLeaf, domElement) {
                                var html = "<div class=\"tip-title\">" + node.name
          + "</div><div class=\"tip-text\">";
                                var data = node.data;
                                if (data.playcount) {
                                    html += "data value: " + data.playcount;
                                }
                                if (data.image) {
                                    html += "<img src=\"" + data.image + "\" class=\"album\" />";
                                }
                                tip.innerHTML = html;
                            }
                        },
                        //Add the name of the node in the correponding label
                        //This method is called once, on label creation.
                        onCreateLabel: function (domElement, node) {
                            domElement.innerHTML = node.name;
                            var style = domElement.style;
                            style.display = '';
                            style.border = '1px solid transparent';
                            domElement.onmouseover = function () {
                                style.border = '1px solid #9FD4FF';
                            };
                            domElement.onmouseout = function () {
                                style.border = '1px solid transparent';
                            };
                        }
                    });
                    tm.loadJSON(treemapData);
                    tm.refresh();
                    //add event to the back button
                    var back = $jit.id('di_vc_GoRootbtn');
                    $jit.util.addEvent(back, 'click', function () {
                        tm.out();
                    });
                    var branding = getQueryStr('branding');
                    if (branding != null && branding != '') {
                        if (branding == "n") {
                            //di_jq("#logo_lt_dvinf").hide();
                            document.getElementById('logo_lt_dvinf').style.display = 'none';
                        }
                    }
                }
                catch (ex) {
                    //ShowProgressBar(false);
                    alert("Error : " + ex.message);
                }
            },
            error: function () {

            },
            cache: false
        });
}



/*Get data from server on pageload for sharedlink*/
function getCloudInputFromServer() {
    var storagePath = "../../stock/shared/vc/cloud/";
    var url = window.location.href;
    var index = url.lastIndexOf("/");
    var InputParam = url.substr(index + 1);
    InputParam = InputParam.substr(0, InputParam.indexOf("."))  // key
    var htmlResp = di_jq.ajax({
        type: "POST",
        url: "../../../../libraries/aspx/Callback.aspx",
        data: { "callback": 15, "param1": InputParam, "param2": storagePath },
        async: false,
        success: function (data) {
            //var cloudDataStr = "[{ 'text': '1990', 'weight': 67, 'title': '67' }, { 'text': '2010', 'weight': 44, 'title': '44'}]";
            //                var cloudData = window.JSON.parse(data);
            var cloudData = di_jq.parseJSON(data);

            //alert(data);                
            di_jq("#di_vc_treeContainer").jQCloud(cloudData);
        },
        error: function () {
            
        },
        cache: false
    });
}

/* function to call chart properties */
function callChartProperty(action, flagChecked) {
   // alert(action + "nath" + flagChecked);
    switch (action) {
        case "border":
            if (chartVisualizationDecided == "radar") {
                di_RadarBorderVisible(chartVisualizationDecided, flagChecked);
                di_renderRadar(chartVisualizationDecided);
            }
            else {
                di_toggleChartBorder(chartVisualizationDecided, flagChecked);
            }
            break;
        case "gridX":
            if (chartVisualizationDecided == "radar") {
                di_RadarGridVisibility(chartVisualizationDecided, flagChecked);
                di_renderRadar(chartVisualizationDecided);
            }
            else {
                di_toggleMajorXGridLine(chartVisualizationDecided, flagChecked);
            }
            break;
        case "gridY":
            di_toggleMajorYGridLine(chartVisualizationDecided, flagChecked);
            break;
        case "dataLbl":
            if (z('di_vcTitlesList').value == "7") {
                z('di_showLabel').defaultChecked = flagChecked;
            }
            di_toggleDatalabels(chartVisualizationDecided, flagChecked, chartVisualizationDecided);
            break;
        case "legend":
            if (chartVisualizationDecided == "radar") {
                di_RadarLegendVisible(chartVisualizationDecided, flagChecked);
                di_renderRadar(chartVisualizationDecided);
            }
            else {
                di_toggleLegendVisibility(chartVisualizationDecided, flagChecked);
            }
            break;
        case "changeTfont":
            var itemValue = z('di_vcTitlesList').value;
            if (itemValue == "4" || itemValue == "6" || itemValue == "7") {
                di_applyAxisDataLabelFontSize(chartVisualizationDecided, flagChecked, itemValue, chartVisualizationDecided);
            }
            else {
                if (chartVisualizationDecided == "radar") {
                    di_RadarTextFontSize(chartVisualizationDecided, flagChecked, itemValue);
                }
                else if (chartVisualizationDecided == "scatter3d") {
                    di_Scatter3dTextFontSize(chartVisualizationDecided, flagChecked, itemValue);
                }
                else {
                    di_applyTitltFontSize(chartVisualizationDecided, flagChecked, itemValue);
                }
            }
            break;
        case "title":
            if (flagChecked == "-1") {
                setControlForSelect();
                return;
            }
            if (chartVisualizationDecided == "radar")
                TitlesSettingsForRadar(flagChecked);
            else if (chartVisualizationDecided == "scatter3d")
                TitlesSettingsForScatter3d(flagChecked);
            else
                TitlesSettingsExceptRadar(flagChecked);
            break;

        case "changeTitle":
            var itemValue = z('di_vcTitlesList').value;
            flagChecked = EncriptText(flagChecked);
            if (chartVisualizationDecided == "radar")
                di_chartRadarText(chartVisualizationDecided, flagChecked, itemValue);
            else if (chartVisualizationDecided == "scatter3d")
                di_chartScatter3dText(chartVisualizationDecided, flagChecked, itemValue);
            else
                di_changeTitleText(chartVisualizationDecided, flagChecked, itemValue);
            break;
        case "changeTFontOneByOne":
            var itemValue = z('di_vcTitlesList').value;
            var titleSettings, TfontSize;
            if (itemValue == "4" || itemValue == "6" || itemValue == "7") {
                di_applyAxisLabelFontSize(chartVisualizationDecided, flagChecked, itemValue, chartVisualizationDecided);
                titleSettings = di_getAllLabelSettings(chartVisualizationDecided, itemValue, chartVisualizationDecided);
                TfontSize = titleSettings[0];
                di_setDropdownlist('di_vctfontlist', TfontSize);
            }
            else {
                if (chartVisualizationDecided == "radar")
                    GetRadarTextGetFontSize(chartVisualizationDecided, flagChecked, itemValue);
                else if (chartVisualizationDecided == "scatter3d")
                    GetScatter3dTextGetFontSize(chartVisualizationDecided, flagChecked, itemValue);
                else {
                    di_applyTitleFontSize(chartVisualizationDecided, flagChecked, itemValue);
                    titleSettings = di_getAllTitleSettings(chartVisualizationDecided, itemValue);
                    TfontSize = titleSettings[0];
                    di_setDropdownlist('di_vctfontlist', TfontSize);
                }
            }
            break;
        case "changeTFontstyle":
            var size = z('di_vctfontlist').value;
            var itemValue = z('di_vcTitlesList').value;
            if (itemValue == "4" || itemValue == "6" || itemValue == "7") {
                di_applyAxisLabelFontStyle(chartVisualizationDecided, flagChecked, itemValue, chartVisualizationDecided, size);
            }
            else {
                if (chartVisualizationDecided == "radar")
                    di_RadarTextFontStyle(chartVisualizationDecided, flagChecked, itemValue);
                else if (chartVisualizationDecided == "scatter3d")
                    di_Scatter3dTextFontStyle(chartVisualizationDecided, flagChecked, itemValue);
                else
                    di_applyTitleFontStyle(chartVisualizationDecided, flagChecked, itemValue, size);
            }
            break;
        case "rotation":
            di_applyAxisDataLabelRotation(chartVisualizationDecided, flagChecked, z('di_vcTitlesList').value, chartVisualizationDecided);
            break;
        case "noLabel":
            var itemValue = z('di_vcTitlesList').value;
            if (itemValue == "7") {
                var ctrlObj = z('di_vcpdatalbl');
                ctrlObj.defaultChecked = flagChecked;
                callChartProperty("dataLbl", flagChecked);
            }
            {
                di_toggleLabel(chartVisualizationDecided, itemValue, flagChecked);
            }
            break;
        case "dataLabelPosition":
            var itemValue = z('di_vcTitlesList').value;
            if (itemValue == "7") {
                di_dataLabelsPosition(chartVisualizationDecided, parseInt(flagChecked), chartVisualizationDecided);
            }
            break;
        case "customLabelPos":
            var xPos = z('labelXPos').value;
            var yPos = z('labelYPos').value;
            if (xPos == "") {
                xPos = 0;
            }
            if (yPos == "") {
                yPos = 0;
            }
            var pos;
            if (chartVisualizationDecided == "radar" || chartVisualizationDecided == "scatter3d") {
                pos = { x: String(xPos), y: parseInt(yPos) };
                pos.y = 490 - pos.y;
                pos.y = String(pos.y);
            }
            else {
                pos = { x: parseInt(xPos), y: parseInt(yPos) };
                pos.y = 490 - pos.y;
            }
            if (z('di_vcTitlesList').value == "8") {
                if (chartVisualizationDecided == "radar") {
                    di_RadarSourceLabelPosition(chartVisualizationDecided, pos);
                    di_renderRadar(chartVisualizationDecided);
                }
                else if (chartVisualizationDecided == "scatter3d") {
                    di_Scatter3dSourceLabelPosition(chartVisualizationDecided, pos);
                }
                else {
                    di_setSourcePosition(chartVisualizationDecided, pos);
                    di_changeSourcePosition(chartVisualizationDecided);
                }
            }
            else if (z('di_vcTitlesList').value == "9") {
                if (chartVisualizationDecided == "radar") {
                    di_RadarCustomLabelPosition(chartVisualizationDecided, pos);
                    di_renderRadar(chartVisualizationDecided);
                }
                else if (chartVisualizationDecided == "scatter3d") {
                    di_Scatter3dCustomLabelPosition(chartVisualizationDecided, pos);
                }
                else {
                    di_setCustomLabelPosition(chartVisualizationDecided, pos);
                    di_customLabelPosition(chartVisualizationDecided);
                }
            }
            break;
        case "chartStyle":
            if (chartVisualizationDecided == "treemap") {
                di_changeTreeMapTheme(chartVisualizationDecided, "di_vc_treeContainer", flagChecked);

            }
            else if (chartVisualizationDecided == "radar") {
                di_setRadarTheme(chartVisualizationDecided, flagChecked);
                di_renderRadar(chartVisualizationDecided);
            }
            else {
                di_setChartStyle(chartVisualizationDecided, flagChecked);
            }
            break;

        case "seriesBorderWidth":
            onchangeSeriesBorderWidth(chartVisualizationDecided, flagChecked, chartVisualizationDecided);
            break;
        case "series":
            var colorCode = "#ffffff";
            if (flagChecked != "-1") {
                if (chartVisualizationDecided != "radar") {
                    colorCode = di_selectSeriesName(chartVisualizationDecided, parseInt(flagChecked) - 1);
                }
                else {
                    var colorCode = di_GetRadarSeriesColor(chartVisualizationDecided, di_jq('#di_vcSelSeries option:selected').text());
                    colorCode = colorCode.replace("0x", "#");
                    /*var radarSettingsObject = di_getRadarSettings(chartVisualizationDecided);
                    radarSettingsObject.colors[parseInt(flagChecked)-1]= colorCode;
                    di_setRadarSettings(chartVisualizationDecided,radarSettingsObject);*/
                }
            }
            di_setColorDialogBox("inp_dicolor7", colorCode);
            break;

        case "seriesBorderStyle":
            if (chartVisualizationDecided.toLowerCase() != "line") {
                onchangeSeriesStyle(chartVisualizationDecided, flagChecked, chartVisualizationDecided);
            }
            else {
                di_selectSeriesStyle(chartVisualizationDecided, flagChecked, z('di_vcSelSeries').selectedIndex)
            }

            break
        case "changePieData":
            di_changePieData(chartVisualizationDecided, flagChecked);
            break;

        case "export":
            if (chartVisualizationDecided == "radar") {
                var fileName = "radar";
                var imageResult = di_RadarImageBitString(chartVisualizationDecided);
                var htmlTable = getOATtableContent();
                var titleText = di_GetRadarTitle(chartVisualizationDecided);
                var subtitleText = di_GetRadarSubTitle(chartVisualizationDecided);
                if (titleText != "") {
                    fileName = removeInvalidCharacters(titleText);
                }
                var sourceText = getSourceList();
                var keywordsText = "";
                postSwfImage(imageResult, flagChecked, fileName, htmlTable, titleText, subtitleText, sourceText, keywordsText);
            }

            else if (chartVisualizationDecided == "scatter3d") {
                var imageResult = di_scatter3dImageBitString(chartVisualizationDecided);
                var htmlTable = getOATtableContent();
                var titleText = "Title";
                var subtitleText = "Subtitle";
                var fileName = "scatter3d"//di_removeInvalidCharacters(titleText);				
                var sourceText = getSourceList();
                var keywordsText = "";
                postSwfImage(imageResult, flagChecked, fileName, htmlTable, titleText, subtitleText, sourceText, keywordsText);
            }

            else {
                //alert("sourav");
               
                di_exportImage(chartVisualizationDecided, flagChecked, chartVisualizationDecided, z('di_vcpdatalbl').checked);
            }

            break;
        case "seperator":
            z('di_decimalTxt').disabled = !flagChecked;
            var itemValue = z('di_vcTitlesList').value;
            if (itemValue == "-1" && itemValue == "4")
                return;
            var decimalValue = 0;
            if (flagChecked) {
                decimalValue = z('di_decimalTxt').value;
                if (decimalValue == "") {
                    decimalValue = 0;
                }
                decimalValue = parseInt(decimalValue);
            }
            di_toggleThousandSep(chartVisualizationDecided, itemValue, flagChecked, decimalValue, chartVisualizationDecided);
            break;
        case "decimal":
            var itemValue = z('di_vcTitlesList').value;
            if (itemValue == "-1" && itemValue == "4")
                return;
            var decimalValue;
            if (flagChecked) {
                decimalValue = flagChecked;
                if (decimalValue == "") {
                    decimalValue = 0;
                }
                decimalValue = parseInt(decimalValue);
            }
            di_toggleThousandSep(chartVisualizationDecided, itemValue, true, decimalValue, chartVisualizationDecided);
            break;

        case "share":
            if (OAT.getSelfInstance() == null)
                return;
            if (chartVisualizationDecided == 'table')
                HtmlOAT();
            else if (chartVisualizationDecided == 'map')
                shareMap();
            else if (chartVisualizationDecided == 'treemap')
                shareTreemapVisualizer(chartVisualizationDecided);
            else if (chartVisualizationDecided == "cloud")
                shareCloudVisualizer(chartVisualizationDecided);
            else
                shareVisualizer(chartVisualizationDecided);
            break;
        case "print":
            di_printChart(chartVisualizationDecided);
            break;
        case "popAxis":
            if (flagChecked == "1") {
                z('popMajorGridChkbox').defaultChecked = z('di_vcpgridx').checked;
                z('popGridLineWidthTxt').value = di_getXAxisMajorGridWidth(chartVisualizationDecided);
                z('popGridLineColorTxt').value = di_getXAxisMajorGridColor(chartVisualizationDecided);
                z('popMinorXLineWidthTxt').value = di_getXAxisMinorGridWidth(chartVisualizationDecided);
                z('popMinorXLineColorTxt').value = di_getXAxisMinorGridColor(chartVisualizationDecided);
                if (di_getXAxisMinorGridWidth(chartVisualizationDecided) > 0) {
                    z('popXMinorLineChkbox').defaultChecked = true;
                }
                else {
                    z('popXMinorLineChkbox').defaultChecked = false;
                }
                di_setDropdownlist('popMajorLineStyle', Axis.xAxis.MajorAxis.style);
                di_setDropdownlist('popMinorLineStyle', Axis.xAxis.MinorAxis.style);
            }
            else {
                z('popMajorGridChkbox').defaultChecked = z('di_vcpgridy').checked;
                z('popGridLineWidthTxt').value = di_getYAxisMajorGridWidth(chartVisualizationDecided);
                z('popGridLineColorTxt').value = di_getYAxisMajorGridColor(chartVisualizationDecided);
                z('popMinorXLineWidthTxt').value = di_getYAxisMinorGridWidth(chartVisualizationDecided);
                z('popMinorXLineColorTxt').value = di_getYAxisMinorGridColor(chartVisualizationDecided);
                if (di_getYAxisMinorGridWidth(chartVisualizationDecided) > 0) {
                    z('popXMinorLineChkbox').defaultChecked = true;
                }
                else {
                    z('popXMinorLineChkbox').defaultChecked = false;
                }
                di_setDropdownlist('popMajorLineStyle', Axis.yAxis.MajorAxis.style);
                di_setDropdownlist('popMinorLineStyle', Axis.yAxis.MinorAxis.style);
            }
            di_jq('#popGridLineColorTxt').css("backgroundColor", z('popGridLineColorTxt').value);
            di_jq('#popGridLineColorTxt').css("color", z('popGridLineColorTxt').value);
            di_jq('#popMinorXLineColorTxt').css("backgroundColor", z('popMinorXLineColorTxt').value);
            di_jq('#popMinorXLineColorTxt').css("color", z('popMinorXLineColorTxt').value);
            break;
        case "popMajorGridLine":
            if (z('popAxisDdl').value == "1") {
                z('di_vcpgridx').defaultChecked = flagChecked;
            }
            else {
                z('di_vcpgridy').defaultChecked = flagChecked;
            }
            break;
        case "popupGLEnabled":
            if (z('popAxisDdl').value == "1") {
                setAxisSettingOnChange('xaxis', 'glEnabled', flagChecked);
                z('di_vcpgridx').defaultChecked = flagChecked;


            }
            else {
                setAxisSettingOnChange('yaxis', 'glEnabled', flagChecked);
                z('di_vcpgridy').defaultChecked = flagChecked;

            }
            break;
        case "popupGLWidth":
            if (z('popAxisDdl').value == "1") {
                setAxisSettingOnChange('xaxis', 'glWidth', flagChecked);
            }
            else {
                setAxisSettingOnChange('yaxis', 'glWidth', flagChecked);
            }
            break;
        case "popupGLColor":
            if (z('popAxisDdl').value == "1") {
                setAxisSettingOnChange('xaxis', 'glColor', flagChecked);
            }
            else {
                setAxisSettingOnChange('yaxis', 'glColor', flagChecked);
            }
            break;
        case "popupGLStyle":
            if (z('popAxisDdl').value == "1") {
                setAxisSettingOnChange('xaxis', 'glStyle', flagChecked);
            }
            else {
                setAxisSettingOnChange('yaxis', 'glStyle', flagChecked);
            }
            break;
        case "popupMGLEnabled":
            if (z('popAxisDdl').value == "1") {
                setAxisSettingOnChange('xaxis', 'mglEnabled', flagChecked);
            }
            else {
                setAxisSettingOnChange('yaxis', 'mglEnabled', flagChecked);
            }
            break;
        case "popupMGLWidth":
            if (z('popAxisDdl').value == "1") {
                setAxisSettingOnChange('xaxis', 'mglWidth', flagChecked);
            }
            else {
                setAxisSettingOnChange('yaxis', 'mglWidth', flagChecked);
            }
            break;
        case "popupMGLColor":
            if (z('popAxisDdl').value == "1") {
                setAxisSettingOnChange('xaxis', 'mglColor', flagChecked);
            }
            else {
                setAxisSettingOnChange('yaxis', 'mglColor', flagChecked);
            }
            break;
        case "popupMGLStyle":
            if (z('popAxisDdl').value == "1") {
                setAxisSettingOnChange('xaxis', 'mglStyle', flagChecked);
            }
            else {
                setAxisSettingOnChange('yaxis', 'mglStyle', flagChecked);
            }
            break;
        case "popupSeriesColor":
            if (z("popSeriesDdl").value == "-1") {
                return;
            }
            else {
                var index = parseInt(z("popSeriesDdl").value) - 1;
                Axis.series[index].color = flagChecked;
            }
            break;
        case "popupSeriesWidth":
            if (z("popSeriesDdl").value == "-1") {
                return;
            }
            else {
                var index = parseInt(z("popSeriesDdl").value) - 1;
                Axis.series[index].width = flagChecked;
            }
            break;
        case "popupSeriesBdrColor":
            if (z("popSeriesDdl").value == "-1") {
                return;
            }
            else {
                var index = parseInt(z("popSeriesDdl").value) - 1;
                Axis.series[index].borderColor = flagChecked;
            }
            break;
        case "popupSeriesStyle":
            if (z("popSeriesDdl").value == "-1") {
                return;
            }
            else {
                var index = parseInt(z("popSeriesDdl").value) - 1;
                Axis.series[index].style = flagChecked;
            }
            break;
        case "popupSeriesNoColor":
            if (z("popSeriesDdl").value == "-1") {
                return;
            }
            else {
                if (flagChecked) {
                    var index = parseInt(z("popSeriesDdl").value) - 1;
                    Axis.series[index].color = "rgba(255, 255, 255, 0.0)";
                }
            }
            break;
        case "popupSeriesGCStyle":
            if (z("popSeriesDdl").value == "-1") {
                return;
            }
            else {
                var index = parseInt(z("popSeriesDdl").value) - 1;
                Axis.series[index].gcStyle = flagChecked;
            }
            break
    }
}


function EncriptText(txt) {
    var RetVal = txt;
    if (txt.indexOf('<') > -1) {
        RetVal = ReplaceAll(txt, "<", "&lt;");
    }
    else if (txt.indexOf('>') > -1) {
        RetVal = ReplaceAll(RetVal, ">", "&gt;");
    }
    return RetVal;
}

function DecriptText(txt) {
    var RetVal = txt;
    if (txt.indexOf('&lt;') > -1) {
        RetVal = ReplaceAll(txt, "&lt;", "<");
    }
    else if (txt.indexOf('&gt;') > -1) {
        RetVal = ReplaceAll(RetVal, "&gt;", ">");
    }
    return RetVal;
}


function TitlesSettingsExceptRadar(flagChecked) {
    z('di_vctfontlist').disabled = false;
    z('di_vctitleval').disabled = false;
    var titleSettings;
    if (flagChecked == "4" || flagChecked == "6" || flagChecked == "7") {
        z('titles_fun_row1').style.display = "none";
        z('titles_fun_row2').style.display = "none";
        z('labels_fun_row1').style.display = "";
        z('labels_fun_row2').style.display = "";
        z('di_PosTxt').disabled = true;
        z('di_seperatorChkbox').disabled = false;
        z('di_decimalTxt').disabled = false;
        titleSettings = di_getAllLabelSettings(chartVisualizationDecided, flagChecked, chartVisualizationDecided);
        if (flagChecked == "7")// DataLabels
        {
            z('di_seperatorChkbox').defaultChecked = di_getDataLabelSeperator(chartVisualizationDecided);
            if (chartVisualizationDecided != "pyramid") {
                z('di_PosTxt').disabled = false;
            }
            z('di_showLabel').defaultChecked = z('di_vcpdatalbl').checked;
            z('di_decimalTxt').value = di_getDataLabelDecimal(chartVisualizationDecided);
            var posVal = di_getDataLabelsPosition(chartVisualizationDecided, chartVisualizationDecided);
            if (posVal) {
                z('di_PosTxt').value = posVal;
            }
            if (chartVisualizationDecided == "pie") {
                z('di_decimalTxt').disabled = z('di_seperatorChkbox').disabled = true;
            }
        }
        else // AxisLabels
        {
            var isTrue = di_getLabelStatus(chartVisualizationDecided, flagChecked);
            z('di_showLabel').defaultChecked = isTrue;
            if (flagChecked == "6") {
                z('di_seperatorChkbox').defaultChecked = di_getYAxisSeperator(chartVisualizationDecided);
                z('di_decimalTxt').value = di_getYAxisDecimal(chartVisualizationDecided);
            }
            else {
                z('di_decimalTxt').disabled = true;
                z('di_seperatorChkbox').disabled = true;
            }
        }
    }
    else {
        z('titles_fun_row1').style.display = "";
        z('titles_fun_row2').style.display = "";
        z('labels_fun_row1').style.display = "none";
        z('labels_fun_row2').style.display = "none";
        var titleVal = di_getChTitle(chartVisualizationDecided, flagChecked);
        titleVal = DecriptText(titleVal);
        z('di_vctitleval').value = titleVal;
        titleSettings = di_getAllTitleSettings(chartVisualizationDecided, flagChecked);
    }
    di_jq("#di_vctfontlist").val(titleSettings[0]);
    di_setColorDialogBox("inp_dicolor5", titleSettings[1]);
    if (titleSettings[2].toLowerCase() == "bold") {
        di_jq("#vc_Bold").addClass("active");
    }
    else {
        di_jq("#vc_Bold").removeClass("active");
    }
    if (titleSettings[3].toLowerCase() == "italic") {
        di_jq("#vc_Italic").addClass("active");
    }
    else {
        di_jq("#vc_Italic").removeClass("active");
    }
    if (titleSettings[4].toLowerCase() == "underline") {
        di_jq("#vc_Underline").addClass("active");
    }
    else {
        di_jq("#vc_Underline").removeClass("active");
    }
    var xPos = z('labelXPos');
    var yPos = z('labelYPos');
    if (flagChecked == "8" && chartVisualizationDecided != "treemap") {
        var pos = di_getSourcePosition(chartVisualizationDecided);
        xPos.value = pos.x;
        yPos.value = 500 - pos.y;
        xPos.disabled = yPos.disabled = false;
    }
    else if (flagChecked == "9") {
        var pos = di_getCustomLabelPosition(chartVisualizationDecided);
        xPos.value = pos.x;
        yPos.value = 480 - pos.y;
        xPos.disabled = yPos.disabled = false;
    }
    else {
        xPos.disabled = yPos.disabled = true;
    }
}


function TitlesSettingsForRadar(flagChecked) {
    z('di_vctfontlist').disabled = false;
    z('di_vctitleval').disabled = false;
    var xPos = z('labelXPos');
    var yPos = z('labelYPos');
    xPos.disabled = yPos.disabled = true;
    if (flagChecked == "8" || flagChecked == "9") {
        xPos.disabled = yPos.disabled = false;
        var pos = {};
        if (flagChecked == "8") {
            pos = di_GetRadarSourceLabelPosition(chartVisualizationDecided);
        }
        else {
            pos = di_GetRadarCustomLabelPosition(chartVisualizationDecided);
        }
        xPos.value = pos.x;
        yPos.value = pos.y;
    }
    // Settings
    var R_text = "", R_fontSize = "9px", R_bold = "n", R_Italice = "n", R_Underline = "n", R_color = "";
    switch (flagChecked) {
        case "1":
            R_text = di_GetRadarTitle(chartVisualizationDecided);
            R_fontSize = di_GetRadarTitleFontsize(chartVisualizationDecided);
            R_bold = di_GetRadarTitleWeight(chartVisualizationDecided);
            R_Italice = di_GetRadarTitleStyle(chartVisualizationDecided);
            R_Underline = di_GetRadarTitleDecoration(chartVisualizationDecided);
            R_color = di_GetRadarTitleColor(chartVisualizationDecided);
            break;
        case "2":
            R_text = di_GetRadarSubTitle(chartVisualizationDecided);
            R_fontSize = di_GetRadarSubTitleFontsize(chartVisualizationDecided);
            R_bold = di_GetRadarSubTitleWeight(chartVisualizationDecided);
            R_Italice = di_GetRadarSubTitleStyle(chartVisualizationDecided);
            R_Underline = di_GetRadarSubTitleDecoration(chartVisualizationDecided);
            R_color = di_GetRadarSubTitleColor(chartVisualizationDecided);
            break;
        case "8":
            R_text = di_GetRadarSourceText(chartVisualizationDecided);
            R_fontSize = di_GetRadarSourceLabelFontsize(chartVisualizationDecided);
            R_bold = di_GetRadarSourceLabelWeight(chartVisualizationDecided);
            R_Italice = di_GetRadarSourceLabelStyle(chartVisualizationDecided);
            R_Underline = di_GetRadarSourceLabelDecoration(chartVisualizationDecided);
            R_color = di_GetRadarSourceLabelColor(chartVisualizationDecided);
            break;
        case "9":
            R_text = di_GetRadarCustomText(chartVisualizationDecided);
            R_fontSize = di_GetRadarCustomLabelFontsize(chartVisualizationDecided);
            R_bold = di_GetRadarCustomLabelWeight(chartVisualizationDecided);
            R_Italice = di_GetRadarCustomLabelStyle(chartVisualizationDecided);
            R_Underline = di_GetRadarCustomLabelDecoration(chartVisualizationDecided);
            R_color = di_GetRadarCustomLabelColor(chartVisualizationDecided);
            break;
    }
    z("di_vctitleval").value = R_text;
    di_jq("#di_vctfontlist").val(R_fontSize + "px");
    if (R_bold.toLowerCase() == "b") {
        di_jq("#vc_Bold").addClass("active");
    }
    else {
        di_jq("#vc_Bold").removeClass("active");
    }
    if (R_Italice.toLowerCase() == "i") {
        di_jq("#vc_Italic").addClass("active");
    }
    else {
        di_jq("#vc_Italic").removeClass("active");
    }
    if (R_Underline.toLowerCase() == "u") {
        di_jq("#vc_Underline").addClass("active");
    }
    else {
        di_jq("#vc_Underline").removeClass("active");
    }
    if (R_color.toLowerCase() == "0x0") {
        R_color = "#000000";
    }
    else {
        R_color = R_color.replace("0x", "#");
    }
    di_setColorDialogBox("inp_dicolor5", R_color);
}

function TitlesSettingsForScatter3d(flagChecked) {
    z('di_vctfontlist').disabled = false;
    z('di_vctitleval').disabled = false;
    var xPos = z('labelXPos');
    var yPos = z('labelYPos');
    xPos.disabled = yPos.disabled = true;
    if (flagChecked == "8" || flagChecked == "9") {
        xPos.disabled = yPos.disabled = false;
        var pos = {};
        if (flagChecked == "8") {
            pos = di_GetScatter3dSourceLabelPosition(chartVisualizationDecided);
        }
        else {
            pos = di_GetScatter3dCustomLabelPosition(chartVisualizationDecided);
        }
        xPos.value = pos.x;
        yPos.value = pos.y;
    }
    // Settings
    var R_text = "", R_fontSize = "9px", R_bold = "n", R_Italice = "n", R_Underline = "n", R_color = "";
    switch (flagChecked) {
        case "1":
            R_text = di_GetScatter3dTitle(chartVisualizationDecided);
            R_fontSize = di_GetScatter3dTitleFontsize(chartVisualizationDecided);
            R_bold = di_GetScatter3dTitleWeight(chartVisualizationDecided);
            R_Italice = di_GetScatter3dTitleStyle(chartVisualizationDecided);
            R_Underline = di_GetScatter3dTitleDecoration(chartVisualizationDecided);
            R_color = di_GetScatter3dTitleColor(chartVisualizationDecided);
            break;
        case "2":
            R_text = di_GetScatter3dSubTitle(chartVisualizationDecided);
            R_fontSize = di_GetScatter3dSubTitleFontsize(chartVisualizationDecided);
            R_bold = di_GetScatter3dSubTitleWeight(chartVisualizationDecided);
            R_Italice = di_GetScatter3dSubTitleStyle(chartVisualizationDecided);
            R_Underline = di_GetScatter3dSubTitleDecoration(chartVisualizationDecided);
            R_color = di_GetScatter3dSubTitleColor(chartVisualizationDecided);
            break;
        case "8":
            R_text = di_GetScatter3dSourceText(chartVisualizationDecided);
            R_fontSize = di_GetScatter3dSourceLabelFontsize(chartVisualizationDecided);
            R_bold = di_GetScatter3dSourceLabelWeight(chartVisualizationDecided);
            R_Italice = di_GetScatter3dSourceLabelStyle(chartVisualizationDecided);
            R_Underline = di_GetScatter3dSourceLabelDecoration(chartVisualizationDecided);
            R_color = di_GetScatter3dSourceLabelColor(chartVisualizationDecided);
            break;
        case "9":
            R_text = di_GetScatter3dCustomText(chartVisualizationDecided);
            R_fontSize = di_GetScatter3dCustomLabelFontsize(chartVisualizationDecided);
            R_bold = di_GetScatter3dCustomLabelWeight(chartVisualizationDecided);
            R_Italice = di_GetSCatter3dCustomLabelStyle(chartVisualizationDecided);
            R_Underline = di_GetScatter3dCustomLabelDecoration(chartVisualizationDecided);
            R_color = di_GetScatter3dCustomLabelColor(chartVisualizationDecided);
            break;
    }
    z("di_vctitleval").value = R_text;
    di_jq("#di_vctfontlist").val(R_fontSize + "px");
    if (R_bold.toLowerCase() == "b") {
        di_jq("#vc_Bold").addClass("active");
    }
    else {
        di_jq("#vc_Bold").removeClass("active");
    }
    if (R_Italice.toLowerCase() == "i") {
        di_jq("#vc_Italic").addClass("active");
    }
    else {
        di_jq("#vc_Italic").removeClass("active");
    }
    if (R_Underline.toLowerCase() == "u") {
        di_jq("#vc_Underline").addClass("active");
    }
    else {
        di_jq("#vc_Underline").removeClass("active");
    }
    if (R_color.toLowerCase() == "0x0") {
        R_color = "#000000";
    }
    else {
        R_color = R_color.replace("0x", "#");
    }
    di_setColorDialogBox("inp_dicolor5", R_color);
}
function GetRadarTextGetFontSize(chartVisualizationDecided, flagChecked, itemValue) {
    var R_fontSize = z("di_vctfontlist").value;
    R_fontSize = R_fontSize.substr(0, R_fontSize.length - 2);
    R_fontSize = parseInt(R_fontSize);
    if (flagChecked)
        R_fontSize++;
    else
        R_fontSize--;
    di_setDropdownlist('di_vctfontlist', R_fontSize + "px");
    switch (itemValue) {
        case "1":
            R_fontSize = di_RadarTitleFontsize(chartVisualizationDecided, String(R_fontSize));
            break;
        case "2":
            R_fontSize = di_RadarSubTitleFontsize(chartVisualizationDecided, String(R_fontSize));
            break;
        case "8":
            R_fontSize = di_RadarSourceLabelFontsize(chartVisualizationDecided, String(R_fontSize));
            break;
        case "9":
            R_fontSize = di_RadarCustomLabelFontsize(chartVisualizationDecided, String(R_fontSize));
            break;
    }
    di_renderRadar(chartVisualizationDecided);
}
function di_chartRadarText(chartVisualizationDecided, flagChecked, itemValue) {
    if (itemValue == "1") {
        di_RadarTitle(chartVisualizationDecided, flagChecked);
    }
    else if (itemValue == "2") {
        di_RadarSubTitle(chartVisualizationDecided, flagChecked);
    }
    else if (itemValue == "8") {
        di_RadarSourceText(chartVisualizationDecided, flagChecked);
    }
    else if (itemValue == "9") {
        di_RadarCustomText(chartVisualizationDecided, flagChecked);
    }
    di_renderRadar(chartVisualizationDecided);
}

function di_RadarTextFontSize(chartVisualizationDecided, flagChecked, itemValue) {
    var fontSize;
    if (flagChecked.indexOf("px") > -1) {
        fontSize = flagChecked.substr(0, flagChecked.indexOf("px"));
    }
    if (itemValue == "1") {
        di_RadarTitleFontsize(chartVisualizationDecided, fontSize);
    }
    else if (itemValue == "2") {
        di_RadarSubTitleFontsize(chartVisualizationDecided, fontSize);
    }
    else if (itemValue == "8") {
        di_RadarSourceLabelFontsize(chartVisualizationDecided, fontSize);
    }
    else if (itemValue == "9") {
        di_RadarCustomLabelFontsize(chartVisualizationDecided, fontSize);
    }
    di_renderRadar(chartVisualizationDecided);
}

function di_RadarTextFontStyle(chartVisualizationDecided, flagChecked, itemValue) {
    switch (itemValue) {
        case "1":
            var fontStyle;
            if (flagChecked == "b") {
                fontStyle = di_GetRadarTitleWeight(chartVisualizationDecided);
                if (fontStyle == "b")
                    di_RadarTitleWeight(chartVisualizationDecided, "n");
                else
                    di_RadarTitleWeight(chartVisualizationDecided, "b");
            }
            else if (flagChecked == "i") {
                fontStyle = di_GetRadarTitleStyle(chartVisualizationDecided);
                if (fontStyle == "i")
                    di_RadarTitleStyle(chartVisualizationDecided, "n");
                else
                    di_RadarTitleStyle(chartVisualizationDecided, "i");
            }
            else if (flagChecked == "u") {
                fontStyle = di_GetRadarTitleDecoration(chartVisualizationDecided);
                if (fontStyle == "u")
                    di_RadarTitleDecoration(chartVisualizationDecided, "n");
                else
                    di_RadarTitleDecoration(chartVisualizationDecided, "u");
            }
            break;
        case "2":
            var fontStyle;
            if (flagChecked == "b") {
                fontStyle = di_GetRadarSubTitleWeight(chartVisualizationDecided);
                if (fontStyle == "b")
                    di_RadarSubTitleWeight(chartVisualizationDecided, "n");
                else
                    di_RadarSubTitleWeight(chartVisualizationDecided, "b");
            }
            else if (flagChecked == "i") {
                fontStyle = di_GetRadarSubTitleStyle(chartVisualizationDecided);
                if (fontStyle == "i")
                    di_RadarSubTitleStyle(chartVisualizationDecided, "n");
                else
                    di_RadarSubTitleStyle(chartVisualizationDecided, "i");
            }
            else if (flagChecked == "u") {
                fontStyle = di_GetRadarSubTitleDecoration(chartVisualizationDecided);
                if (fontStyle == "u")
                    di_RadarSubTitleDecoration(chartVisualizationDecided, flagChecked);
                else
                    di_RadarSubTitleDecoration(chartVisualizationDecided, "n");
            }
            break;
        case "8":
            var fontStyle;
            if (flagChecked == "b") {
                fontStyle = di_GetRadarSourceLabelWeight(chartVisualizationDecided);
                if (fontStyle == "b")
                    di_RadarSourceLabelWeight(chartVisualizationDecided, "n");
                else
                    di_RadarSourceLabelWeight(chartVisualizationDecided, "b");
            }
            else if (flagChecked == "i") {
                fontStyle = di_GetRadarSourceLabelStyle(chartVisualizationDecided);
                if (fontStyle == "i")
                    di_RadarSourceLabelStyle(chartVisualizationDecided, "n");
                else
                    di_RadarSourceLabelStyle(chartVisualizationDecided, "i");
            }
            else if (flagChecked == "u") {
                fontStyle = di_GetRadarSourceLabelDecoration(chartVisualizationDecided);
                if (fontStyle == "u")
                    di_RadarSourceLabelDecoration(chartVisualizationDecided, flagChecked);
                else
                    di_RadarSourceLabelDecoration(chartVisualizationDecided, "n");
            }
            break;
        case "9":
            var fontStyle;
            if (flagChecked == "b") {
                fontStyle = di_GetRadarCustomLabelWeight(chartVisualizationDecided);
                if (fontStyle == "b")
                    di_RadarCustomLabelWeight(chartVisualizationDecided, "n");
                else
                    di_RadarCustomLabelWeight(chartVisualizationDecided, "b");
            }
            else if (flagChecked == "i") {
                fontStyle = di_GetRadarCustomLabelStyle(chartVisualizationDecided);
                if (fontStyle == "i")
                    di_RadarCustomLabelStyle(chartVisualizationDecided, "n");
                else
                    di_RadarCustomLabelStyle(chartVisualizationDecided, "i");
            }
            else if (flagChecked == "u") {
                fontStyle = di_GetRadarCustomLabelDecoration(chartVisualizationDecided);
                if (fontStyle == "u")
                    di_RadarCustomLabelDecoration(chartVisualizationDecided, flagChecked);
                else
                    di_RadarCustomLabelDecoration(chartVisualizationDecided, "n");
            }
            break;
    }
    di_renderRadar(chartVisualizationDecided);
}
function di_chartRadarTextColor(chartVisualizationDecided, flagChecked, itemValue) {
    if (itemValue == "1") {
        di_RadarTitleColor(chartVisualizationDecided, flagChecked);
    }
    else if (itemValue == "2") {
        di_RadarSubTitleColor(chartVisualizationDecided, flagChecked);
    }
    else if (itemValue == "8") {
        di_RadarSourceLabelColor(chartVisualizationDecided, flagChecked);
    }
    else if (itemValue == "9") {
        di_RadarCustomLabelColor(chartVisualizationDecided, flagChecked);
    }
    di_renderRadar(chartVisualizationDecided);
}

function di_chartScatter3dText(chartVisualizationDecided, flagChecked, itemValue) {
    if (itemValue == "1") {
        di_Scatter3dTitle(chartVisualizationDecided, flagChecked);
    }
    else if (itemValue == "2") {
        di_Scatter3dSubTitle(chartVisualizationDecided, flagChecked);
    }
    else if (itemValue == "8") {
        di_Scatter3dSourceText(chartVisualizationDecided, flagChecked);
    }
    else if (itemValue == "9") {
        di_Scatter3dCustomText(chartVisualizationDecided, flagChecked);
    }
}

function di_Scatter3dTextFontSize(chartVisualizationDecided, flagChecked, itemValue) {
    var fontSize;
    if (flagChecked.indexOf("px") > -1) {
        fontSize = flagChecked.substr(0, flagChecked.indexOf("px"));
    }
    if (itemValue == "1") {
        di_RadarTitleFontsize(chartVisualizationDecided, fontSize);
    }
    else if (itemValue == "2") {
        di_RadarSubTitleFontsize(chartVisualizationDecided, fontSize);
    }
    else if (itemValue == "8") {
        di_RadarSourceLabelFontsize(chartVisualizationDecided, fontSize);
    }
    else if (itemValue == "9") {
        di_RadarCustomLabelFontsize(chartVisualizationDecided, fontSize);
    }
}

function GetScatter3dTextGetFontSize(chartVisualizationDecided, flagChecked, itemValue) {
    var R_fontSize = z("di_vctfontlist").value;
    R_fontSize = R_fontSize.substr(0, R_fontSize.length - 2);
    R_fontSize = parseInt(R_fontSize);
    if (flagChecked)
        R_fontSize++;
    else
        R_fontSize--;
    di_setDropdownlist('di_vctfontlist', R_fontSize + "px");
    switch (itemValue) {
        case "1":
            R_fontSize = di_RadarTitleFontsize(chartVisualizationDecided, String(R_fontSize));
            break;
        case "2":
            R_fontSize = di_RadarSubTitleFontsize(chartVisualizationDecided, String(R_fontSize));
            break;
        case "8":
            R_fontSize = di_RadarSourceLabelFontsize(chartVisualizationDecided, String(R_fontSize));
            break;
        case "9":
            R_fontSize = di_RadarCustomLabelFontsize(chartVisualizationDecided, String(R_fontSize));
            break;
    }
}

function di_Scatter3dTextFontStyle(chartVisualizationDecided, flagChecked, itemValue) {
    switch (itemValue) {
        case "1":
            var fontStyle;
            if (flagChecked == "b") {
                fontStyle = di_GetScatter3dTitleWeight(chartVisualizationDecided);
                if (fontStyle == "b")
                    di_Scatter3dTitleWeight(chartVisualizationDecided, "n");
                else
                    di_Scatter3dTitleWeight(chartVisualizationDecided, "b");
            }
            else if (flagChecked == "i") {
                fontStyle = di_GetScatter3dTitleStyle(chartVisualizationDecided);
                if (fontStyle == "i")
                    di_Scatter3dTitleStyle(chartVisualizationDecided, "n");
                else
                    di_Scatter3dTitleStyle(chartVisualizationDecided, "i");
            }
            else if (flagChecked == "u") {
                fontStyle = di_GetScatter3dTitleDecoration(chartVisualizationDecided);
                if (fontStyle == "u")
                    di_Scatter3dTitleDecoration(chartVisualizationDecided, flagChecked);
                else
                    di_Scatter3dTitleDecoration(chartVisualizationDecided, "n");
            }
            break;
        case "2":
            var fontStyle;
            if (flagChecked == "b") {
                fontStyle = di_GetScatter3dSubTitleWeight(chartVisualizationDecided);
                if (fontStyle == "b")
                    di_Scatter3dSubTitleWeight(chartVisualizationDecided, "n");
                else
                    di_RadarSubTitleWeight(chartVisualizationDecided, "b");
            }
            else if (flagChecked == "i") {
                fontStyle = di_GetScatter3dSubTitleStyle(chartVisualizationDecided);
                if (fontStyle == "i")
                    di_Scatter3dSubTitleStyle(chartVisualizationDecided, "n");
                else
                    di_RadarSubTitleStyle(chartVisualizationDecided, "i");
            }
            else if (flagChecked == "u") {
                fontStyle = di_GetScatter3dSubTitleDecoration(chartVisualizationDecided);
                if (fontStyle == "u")
                    di_Scatter3dSubTitleDecoration(chartVisualizationDecided, flagChecked);
                else
                    di_Scatter3dSubTitleDecoration(chartVisualizationDecided, "n");
            }
            break;
        case "8":
            var fontStyle;
            if (flagChecked == "b") {
                fontStyle = di_GetScatter3dSourceLabelWeight(chartVisualizationDecided);
                if (fontStyle == "b")
                    di_Scatter3dSourceLabelWeight(chartVisualizationDecided, "n");
                else
                    di_Scatter3dSourceLabelWeight(chartVisualizationDecided, "b");
            }
            else if (flagChecked == "i") {
                fontStyle = di_GetScatter3dSourceLabelStyle(chartVisualizationDecided);
                if (fontStyle == "i")
                    di_Scatter3dSourceLabelStyle(chartVisualizationDecided, "n");
                else
                    di_Scatter3dSourceLabelStyle(chartVisualizationDecided, "i");
            }
            else if (flagChecked == "u") {
                fontStyle = di_GetScatter3dSourceLabelDecoration(chartVisualizationDecided);
                if (fontStyle == "u")
                    di_Scatter3dSourceLabelDecoration(chartVisualizationDecided, flagChecked);
                else
                    di_Scatter3dSourceLabelDecoration(chartVisualizationDecided, "n");
            }
            break;
        case "9":
            var fontStyle;
            if (flagChecked == "b") {
                fontStyle = di_GetScatter3dCustomLabelWeight(chartVisualizationDecided);
                if (fontStyle == "b")
                    di_Scatter3dCustomLabelWeight(chartVisualizationDecided, "n");
                else
                    di_Scatter3dCustomLabelWeight(chartVisualizationDecided, "b");
            }
            else if (flagChecked == "i") {
                fontStyle = di_GetScatter3dCustomLabelStyle(chartVisualizationDecided);
                if (fontStyle == "i")
                    di_Scatter3dCustomLabelStyle(chartVisualizationDecided, "n");
                else
                    di_Scatter3dCustomLabelStyle(chartVisualizationDecided, "i");
            }
            else if (flagChecked == "u") {
                fontStyle = di_GetScatter3dCustomLabelDecoration(chartVisualizationDecided);
                if (fontStyle == "u")
                    di_Scatter3dCustomLabelDecoration(chartVisualizationDecided, flagChecked);
                else
                    di_Scatter3dCustomLabelDecoration(chartVisualizationDecided, "n");
            }
            break;
    }
}

function di_chartScatter3dTextColor(chartVisualizationDecided, flagChecked, itemValue) {
    if (itemValue == "1") {
        di_Scatter3dTitleColor(chartVisualizationDecided, flagChecked);
    }
    else if (itemValue == "2") {
        di_Scatter3dSubTitleColor(chartVisualizationDecided, flagChecked);
    }
    else if (itemValue == "8") {
        di_RadarSourceLabelColor(chartVisualizationDecided, flagChecked);
    }
    else if (itemValue == "9") {
        di_RadarCustomLabelColor(chartVisualizationDecided, flagChecked);
    }
}
// function to initialize color pickers
function initColorPic() {

    // for chart background color picker
    di_jq('#dicolor1').ColorPicker({
        onSubmit: function (hsb, hex, rgb, el) {
            //di_jq(el).val(hex);
            di_jq(el).ColorPickerHide();
            di_jq('#inp_dicolor1').css('background-color', '#' + hex);
            if (chartVisualizationDecided == "radar") {
                di_RadarBackgroundColor(chartVisualizationDecided, '0x' + hex);
                di_renderRadar(chartVisualizationDecided);
            }
            else {
                di_changeChartBgColor(chartVisualizationDecided, '#' + hex);
            }
        }
    });

    // for chart plot area color picker
    di_jq('#dicolor2').ColorPicker({
        onSubmit: function (hsb, hex, rgb, el) {
            di_jq(el).ColorPickerHide();
            di_jq('#inp_dicolor2').css('background-color', '#' + hex);
            di_changePlotBgColor(chartVisualizationDecided, '#' + hex);
        }
    });

    // for chart lagend color picker
    di_jq('#dicolor3').ColorPicker({
        onSubmit: function (hsb, hex, rgb, el) {
            di_jq(el).ColorPickerHide();
            di_jq('#inp_dicolor3').css('background-color', '#' + hex);
            di_applyLegendBgColor(chartVisualizationDecided, '#' + hex);
        }
    });

    // for chart series color picker
    di_jq('#dicolor4').ColorPicker({
        onSubmit: function (hsb, hex, rgb, el) {
            di_jq(el).ColorPickerHide();
            di_jq('#inp_dicolor4').css('background-color', '#' + hex);
            di_selectSeriesColor(chartVisualizationDecided, '#' + hex);
        }
    });

    // for chart titles color picker
    di_jq('#dicolor5').ColorPicker({
        onSubmit: function (hsb, hex, rgb, el) {
            di_jq(el).ColorPickerHide();
            di_jq('#inp_dicolor5').css('background-color', '#' + hex);
            if (z('di_vcTitlesList').value == "4" || z('di_vcTitlesList').value == "6" || z('di_vcTitlesList').value == "7") {
                di_applyAxisLabelFontColor(chartVisualizationDecided, '#' + hex, z('di_vcTitlesList').value, chartVisualizationDecided);
            }
            else {
                if (chartVisualizationDecided == "radar")
                    di_chartRadarTextColor(chartVisualizationDecided, '0x' + hex, z('di_vcTitlesList').value);
                else if (chartVisualizationDecided == "scatter3d")
                    di_chartScatter3dTextColor(chartVisualizationDecided, '0x' + hex, z('di_vcTitlesList').value);
                else
                    di_applyTitleFontColor(chartVisualizationDecided, '#' + hex, z('di_vcTitlesList').value);
            }
        }
    });

    // for chart data labels color picker
    /*di_jq('#dicolor6').ColorPicker({
    onSubmit: function(hsb, hex, rgb, el) {
    di_jq(el).ColorPickerHide();
    di_jq('#inp_dicolor6').css('background-color', '#'+hex);
    di_applyAxisLabelFontColor(chartVisualizationDecided, '#'+hex, z('di_vcSelLabel').selectedIndex,chartVisualizationDecided);
    }
    });*/

    // for chart legend font color picker
    di_jq('#dicolor7').ColorPicker({
        onSubmit: function (hsb, hex, rgb, el) {
            di_jq(el).ColorPickerHide();
            di_jq('#inp_dicolor7').css('background-color', '#' + hex);
            if (chartVisualizationDecided == "radar") {
                var selectedSeries = di_jq("#di_vcSelSeries :selected").text();
                var seriesColorObject = { name: selectedSeries, color: '0x' + hex };
                var SeriesObject = [];
                SeriesObject.push(seriesColorObject);
                di_ChangeRadarSeriesColor(chartVisualizationDecided, SeriesObject);
                di_renderRadar(chartVisualizationDecided);
            }
            else {
                di_selectSeriesColor(chartVisualizationDecided, '#' + hex, z('di_vcSelSeries').selectedIndex);
            }
        }
    });

    // for chart series border color picker
    di_jq('#dicolor8').ColorPicker({
        onSubmit: function (hsb, hex, rgb, el) {
            di_jq(el).ColorPickerHide();
            di_jq('#inp_dicolor8').css('background-color', '#' + hex);
            di_onchangeSeriesBorderColor(chartVisualizationDecided, '#' + hex, chartVisualizationDecided);
        }
    });

    // for map titles
    di_jq('#dicolormap1').ColorPicker({
        onSubmit: function (hsb, hex, rgb, el) {
            di_jq(el).ColorPickerHide();
            di_jq('#inp_dicolormap1').css('background-color', '#' + hex);
            di_jq('#h_dicolormap1').val('#' + hex);
            setMapTitleDetails();
        }
    });

}



// function to initialize color pickers
function popUpInitColorPic() {

    // for chart major gridlines
    di_jq('#dicolor10').ColorPicker({
        onSubmit: function (hsb, hex, rgb, el) {
            di_jq(el).ColorPickerHide();
            di_jq('#popchartBoderColor').val('#' + hex);
            di_jq('#popchartBoderColor').css("backgroundColor", "#" + hex);
            di_jq('#popchartBoderColor').css("color", "#" + hex);
            //function to call
        }
    });

    // for chart minor gridlines
    di_jq('#dicolor11').ColorPicker({
        onSubmit: function (hsb, hex, rgb, el) {
            di_jq(el).ColorPickerHide();
            di_jq('#popMinorXLineColorTxt').val('#' + hex);
            di_jq('#popMinorXLineColorTxt').css("backgroundColor", "#" + hex);
            di_jq('#popMinorXLineColorTxt').css("color", "#" + hex);
            callChartProperty("popupMGLColor", '#' + hex);
            //function to call
        }
    });

    // for chart minor gridlines
    di_jq('#dicolor12').ColorPicker({
        onSubmit: function (hsb, hex, rgb, el) {
            di_jq(el).ColorPickerHide();
            di_jq('#popGridLineColorTxt').val('#' + hex);
            di_jq('#popGridLineColorTxt').css("backgroundColor", "#" + hex);
            di_jq('#popGridLineColorTxt').css("color", "#" + hex);
            callChartProperty("popupGLColor", '#' + hex);
            //function to call
        }
    });

    // for chart area color 1
    di_jq('#dicolor13').ColorPicker({
        onSubmit: function (hsb, hex, rgb, el) {
            di_jq(el).ColorPickerHide();
            di_jq('#popChartBgColorTxt').val('#' + hex);
            di_jq('#popChartBgColorTxt').css("backgroundColor", "#" + hex);
            di_jq('#popChartBgColorTxt').css("color", "#" + hex);
            //function to call
        }
    });

    // for chart area color 2
    di_jq('#dicolor14').ColorPicker({
        onSubmit: function (hsb, hex, rgb, el) {
            di_jq(el).ColorPickerHide();
            di_jq('#popChartBgColorTxt2').val('#' + hex);
            di_jq('#popChartBgColorTxt2').css("backgroundColor", "#" + hex);
            di_jq('#popChartBgColorTxt2').css("color", "#" + hex);
            //function to call
        }
    });

    // for chart area color 2
    di_jq('#dicolor15').ColorPicker({
        onSubmit: function (hsb, hex, rgb, el) {
            di_jq(el).ColorPickerHide();
            di_jq('#popChartPBgColorTxt').val('#' + hex);
            di_jq('#popChartPBgColorTxt').css("backgroundColor", "#" + hex);
            di_jq('#popChartPBgColorTxt').css("color", "#" + hex);
            //function to call
        }
    });

    // for chart area color 2
    di_jq('#dicolor16').ColorPicker({
        onSubmit: function (hsb, hex, rgb, el) {
            di_jq(el).ColorPickerHide();
            di_jq('#popChartPBgColorTxt2').val('#' + hex);
            di_jq('#popChartPBgColorTxt2').css("backgroundColor", "#" + hex);
            di_jq('#popChartPBgColorTxt2').css("color", "#" + hex);
            //function to call
        }
    });

    // for chart area color 2
    di_jq('#dicolor17').ColorPicker({
        onSubmit: function (hsb, hex, rgb, el) {
            di_jq(el).ColorPickerHide();
            di_jq('#popLegendBorderColorTxt').val('#' + hex);
            di_jq('#popLegendBorderColorTxt').css("backgroundColor", "#" + hex);
            di_jq('#popLegendBorderColorTxt').css("color", "#" + hex);
            //function to call
        }
    });

    // for chart area color 2
    di_jq('#dicolor18').ColorPicker({
        onSubmit: function (hsb, hex, rgb, el) {
            di_jq(el).ColorPickerHide();
            di_jq('#poplegendBgColorTxt').val('#' + hex);
            di_jq('#poplegendBgColorTxt').css("backgroundColor", "#" + hex);
            di_jq('#poplegendBgColorTxt').css("color", "#" + hex);
            //function to call
        }
    });

    // for chart area color 2
    di_jq('#dicolor19').ColorPicker({
        onSubmit: function (hsb, hex, rgb, el) {
            di_jq(el).ColorPickerHide();
            legendFontcolorCode = '#' + hex;
            //di_jq('#poplegendBgColorTxt').val('#'+hex);
            //function to call
        }
    });

    // for series pop-up
    di_jq('#dicolor20').ColorPicker({
        onSubmit: function (hsb, hex, rgb, el) {
            di_jq(el).ColorPickerHide();
            di_jq('#popSeriesColorTxt').val('#' + hex);
            di_jq('#popSeriesColorTxt').css("backgroundColor", "#" + hex);
            di_jq('#popSeriesColorTxt').css("color", "#" + hex);
            if (z('popSeriesSolidColor').checked) {
                callChartProperty("popupSeriesColor", '#' + hex);
            }
            else if (z('popSeriesGColor').checked) {
                var selectedIndex = z('popSeriesDdl').value;
                if (selectedIndex == "-1")
                    return;
                var color = GradeintColor('#' + hex, z('popSeriesColorTxt2').value, Axis.series[parseInt(selectedIndex) - 1].gcStyle);
                callChartProperty("popupSeriesColor", color);
            }
            //function to call
        }
    });

    // for series pop-up
    di_jq('#dicolor21').ColorPicker({
        onSubmit: function (hsb, hex, rgb, el) {
            di_jq(el).ColorPickerHide();
            di_jq('#popSeriesColorTxt2').val('#' + hex);
            di_jq('#popSeriesColorTxt2').css("backgroundColor", "#" + hex);
            di_jq('#popSeriesColorTxt2').css("color", "#" + hex);
            if (z('popSeriesGColor').checked) {
                var selectedIndex = z('popSeriesDdl').value;
                if (selectedIndex == "-1")
                    return;
                var color = GradeintColor(z('popSeriesColorTxt').value, '#' + hex, Axis.series[parseInt(selectedIndex) - 1].gcStyle);
                callChartProperty("popupSeriesColor", color);
            }
            //function to call
        }
    });

    // for series pop-up
    di_jq('#dicolor22').ColorPicker({
        onSubmit: function (hsb, hex, rgb, el) {
            di_jq(el).ColorPickerHide();
            di_jq('#popSeriesBdrColor').val('#' + hex);
            di_jq('#popSeriesBdrColor').css("backgroundColor", "#" + hex);
            di_jq('#popSeriesBdrColor').css("color", "#" + hex);
            callChartProperty("popupSeriesBdrColor", '#' + hex);
            //function to call
        }
    });
}

// function to initialize facts for visulizer
function initFacts() {

    if (chartVisualizationDecided == 'table') {
        di_jq('#tab-v').closest('li').hide(); // to hide visulization bottom tab
    }
    // init color picker
    initColorPic();

    // create list for vc dropdwons
    di_createFontSizeDdl('di_vctfontlist');
    //di_createFontSizeDdl('di_vclfontlist');	

    showHideDownloads();
}

function createDropdownOptions(textArray, ctrlId) {
    var ddlIdObj = z(ctrlId);
    di_clearDdlItems(ctrlId);
    for (var i = 0; i < textArray.length; i++) {
        if (i == 0) {
            ddlIdObj.options[i] = new Option(textArray[i], "-1");
        }
        else {
            ddlIdObj.options[i] = new Option(textArray[i], i);
        }
    }
}

function createLineStyleDdl(textArray, valueArray, ctrlId) {
    var ddlIdObj = z(ctrlId);
    di_clearDdlItems(ctrlId);
    for (var i = 0; i < textArray.length; i++) {
        if (i == 0) {
            ddlIdObj.options[i] = new Option(textArray[i], "-1");
        }
        else {
            ddlIdObj.options[i] = new Option(textArray[i], valueArray[i]);
        }
    }
}

function setControlForSelect() {
    z('di_vctitleval').value = "";
    di_jq("#di_vctfontlist").val("9px");
    if (di_jq('#vc_Bold').hasClass("active")) {
        di_jq('#vc_Bold').removeClass("active");
    }
    if (di_jq('#vc_Italic').hasClass("active")) {
        di_jq('#vc_Italic').removeClass("active");
    }
    if (di_jq('#vc_Underline').hasClass("active")) {
        di_jq('#vc_Underline').removeClass("active");
    }
    di_jq("#labelXPos").val("");
    di_jq("#labelYPos").val("");
    z('di_vctitleval').disabled = true;
    z('di_vctfontlist').disabled = true;
    di_setColorDialogBox("inp_dicolor5", "#ffffff");
    z('titles_fun_row1').style.display = "";
    z('titles_fun_row2').style.display = "";
    z('labels_fun_row1').style.display = "none";
    z('labels_fun_row2').style.display = "none";
    di_setColorDialogBox("inp_dicolor7", "#ffffff");
}

function applyControlsSettings() {

    if (chartVisualizationDecided.toLowerCase() == "table" || chartVisualizationDecided.toLowerCase() == "map" || chartVisualizationDecided.toLowerCase() == "map2d" || chartVisualizationDecided.toLowerCase() == "map3d")
        return;
    var uid = chartVisualizationDecided;
    try {
        createDropdownOptions([z("lngSelectTxt").innerHTML, z("lngChartTitleTxt").innerHTML, z("lngChartSubtitleTxt").innerHTML, z("lngXAxisTitleTxt").innerHTML, z("lngXAxisLabelTxt").innerHTML, z("lngYAxisTitleTxt").innerHTML, z("lngYAxisLabelTxt").innerHTML, z("lngDataLabelsTxt").innerHTML, z("lngSourceTxt").innerHTML, z("lngNoteTxt").innerHTML], "di_vcTitlesList");
    }
    catch (err) { }
    var borderChkbox = z('di_vcpborder');
    var gridXChkbox = z('di_vcpgridx');
    var gridYChkbox = z('di_vcpgridy');
    var dataLabelChkbox = z('di_vcpdatalbl');
    var legendChkbox = z('di_vcplegend');
    var titleDdl = z('di_vcTitlesList');
    var xPosTxt = z('labelXPos');
    var yPosTxt = z('labelYPos');
    var labelChkbox = z('di_showLabel');
    var dataLabelPos = z('di_PosTxt');
    var seperatorChkbox = z('di_seperatorChkbox');
    var decimalText = z('di_decimalTxt');
    var seriesDdl = z('di_vcSelSeries');
    var generalPart = z('settings_section1');
    var titlePart = z('settings_section2');
    var seriesPart = z('settings_section4');
    var xGridLineText = z("langVcGridLnX");
    var yGridLineText = z("langVcGridLnY");
    var editSereis = z("editSeries");
    var styleSeries = z("styleSeries");
    var lineStyle = z("lineStyle");
    var noBorderIcon = z("noBorderIcon");
    var colorpick_pa = z("colorpick_pa_hiddn");


    xGridLineText.innerHTML = z("langGLTemp").innerHTML;
    //var pieSeriesDdl = z('di_seriesPie');	    
    generalPart.style.display = seriesPart.style.display = "";
    editSereis.style.display = "";
    styleSeries.style.display = "";
    lineStyle.style.display = "";
    noBorderIcon.style.display = "";
    xPosTxt.disabled = true;
    yPosTxt.disabled = true;
    yGridLineText.style.display = "";
    dataLabelPos.disabled = true;
    z("vcLangLblDlSVG").style.display = "";
    colorpick_pa.style.display = "";

    if (chartVisualizationDecided == "pie") {
        borderChkbox.defaultChecked = isChartBorder(uid);
        dataLabelChkbox.defaultChecked = di_dataLabelStatus(uid, chartVisualizationDecided);
        legendChkbox.defaultChecked = di_legendVisibility(uid);
        di_setColorDialogBox("inp_dicolor1", di_getChartBgColor(uid));
        di_setColorDialogBox("inp_dicolor2", di_getChartPlotAreaColor(uid));
        di_jq("#di_vcTitlesList >option[value='3']").remove();
        di_jq("#di_vcTitlesList >option[value='4']").remove();
        di_jq("#di_vcTitlesList >option[value='5']").remove();
        di_jq("#di_vcTitlesList >option[value='6']").remove();
        borderChkbox.disabled = false;
        gridXChkbox.disabled = true;
        gridYChkbox.disabled = true;
        dataLabelChkbox.disabled = false;
        legendChkbox.disabled = false;
        //xPosTxt.disabled = false;
        //yPosTxt.disabled = false;			
        labelChkbox.disabled = false;
        dataLabelPos.disabled = false;
        seperatorChkbox.disabled = false;
        decimalText.disabled = false;
        seriesDdl.disabled = false;
        //pieSeriesDdl.style.display="";						
    }

    else if (chartVisualizationDecided == "treemap" || chartVisualizationDecided == "cloud" || chartVisualizationDecided == "scatter3d") {
        if (chartVisualizationDecided.toLowerCase() == "scatter3d") {
            di_jq("#di_vcTitlesList >option[value='3']").remove();
            di_jq("#di_vcTitlesList >option[value='4']").remove();
            di_jq("#di_vcTitlesList >option[value='5']").remove();
            di_jq("#di_vcTitlesList >option[value='6']").remove();
            di_jq("#di_vcTitlesList >option[value='7']").remove();
            z("vcLangLblDlSVG").style.display = "none";
        }
        else {
            di_jq("#di_vcTitlesList >option[value='2']").remove();
            di_jq("#di_vcTitlesList >option[value='3']").remove();
            di_jq("#di_vcTitlesList >option[value='4']").remove();
            di_jq("#di_vcTitlesList >option[value='5']").remove();
            di_jq("#di_vcTitlesList >option[value='6']").remove();
            di_jq("#di_vcTitlesList >option[value='7']").remove();
            di_jq("#di_vcTitlesList >option[value='9']").remove();
        }
        borderChkbox.disabled = true;
        gridXChkbox.disabled = true;
        gridYChkbox.disabled = true;
        dataLabelChkbox.disabled = true;
        legendChkbox.disabled = true;
        //xPosTxt.disabled = true;
        //yPosTxt.disabled = true;			
        labelChkbox.disabled = true;
        dataLabelPos.disabled = true;
        seperatorChkbox.disabled = true;
        decimalText.disabled = true;
        seriesDdl.disabled = true;
        generalPart.style.display = seriesPart.style.display = "none";

        //pieSeriesDdl.style.display="";
    }
    else if (chartVisualizationDecided == "radar") {
        z("vcLangLblDlSVG").style.display = "none";
        colorpick_pa.style.display = "none";
        di_jq("#di_vcTitlesList >option[value='3']").remove();
        di_jq("#di_vcTitlesList >option[value='4']").remove();
        di_jq("#di_vcTitlesList >option[value='5']").remove();
        di_jq("#di_vcTitlesList >option[value='6']").remove();
        di_jq("#di_vcTitlesList >option[value='7']").remove();
        borderChkbox.disabled = false;
        gridXChkbox.disabled = false;
        xGridLineText.innerHTML = z("lngGridlineTxt").innerHTML;
        gridYChkbox.disabled = true;
        //gridYChkbox.style.display = "none";	
        //yGridLineText.style.display = "none";
        dataLabelChkbox.disabled = true;
        legendChkbox.disabled = false;
        //xPosTxt.disabled = true;
        //yPosTxt.disabled = true;			
        labelChkbox.disabled = true;
        dataLabelPos.disabled = false;
        seperatorChkbox.disabled = true;
        decimalText.disabled = true;
        seriesDdl.disabled = false;
        editSereis.style.display = "none";
        styleSeries.style.display = "none";
        lineStyle.style.display = "none";
        noBorderIcon.style.display = "none";
        borderChkbox.defaultChecked = di_GetRadarBorderVisible(chartVisualizationDecided);
        gridXChkbox.defaultChecked = di_GetRadarGridVisibility(chartVisualizationDecided);
        legendChkbox.defaultChecked = di_GetRadarLegendVisible(chartVisualizationDecided);
        var radarBgcolor = di_GetRadarBackgroundColor(chartVisualizationDecided);
        if (radarBgcolor == "0x0") {
            radarBgcolor = "#000000";
        }
        radarBgcolor = radarBgcolor.replace("0x", "#");
        di_setColorDialogBox("inp_dicolor1", radarBgcolor);
    }
    else {
        try {
            var seriesColArray = di_getSeriesNames(uid);
            seriesColArray[0] = z("mapTxtSelSeries").innerHTML;
            borderChkbox.defaultChecked = isChartBorder(uid);
            dataLabelChkbox.defaultChecked = di_dataLabelStatus(uid, chartVisualizationDecided);
            legendChkbox.defaultChecked = di_legendVisibility(uid);
            di_setColorDialogBox("inp_dicolor1", di_getChartBgColor(uid));
            di_setColorDialogBox("inp_dicolor2", di_getChartPlotAreaColor(uid));
            borderChkbox.disabled = false;
            gridXChkbox.disabled = false;
            gridYChkbox.disabled = false;
            dataLabelChkbox.disabled = false;
            legendChkbox.disabled = false;
            //xPosTxt.disabled = false;
            //yPosTxt.disabled = false;			
            labelChkbox.disabled = false;
            dataLabelPos.disabled = false;
            seperatorChkbox.disabled = false;
            decimalText.disabled = false;
            seriesDdl.disabled = false;
            //pieSeriesDdl.style.display="none";
            gridXChkbox.defaultChecked = z('di_vcpgridx').checked; //isXGridLine(uid);
            gridYChkbox.defaultChecked = z('di_vcpgridy').checked;  //isYGridLine(uid);			
            if (chartVisualizationDecided != "radar")
                createDropdownOptions(seriesColArray, "di_vcSelSeries");
        }
        catch (err) { }
    }
    if (chartVisualizationDecided == "pyramid" && z('visChartDiv').style.display == "none") {
        z('visChartDiv').style.display = "";
        var maxVal = di_getYAxisMaxValue(uid);
        di_setYAxisMaxValue(uid, maxVal);
    }
    callChartProperty("title", "-1");
    try {
        var svgObj = document.getElementsByTagName("svg");
        svgObj[0].style.direction = 'ltr';
    }
    catch (err) { }
}
function isChartBorder(uid) {
    var isBorder = false;
    if (di_getChartBorderWidth(uid) > 0)
        isBorder = true;
    return isBorder;
}

function isXGridLine(uid) {
    var isLine = false;
    if (di_getChartXGridLine(uid) > 0)
        isLine = true;
    return isLine;
}

function isYGridLine(uid) {
    var isLine = false;
    if (di_getChartYGridLine(uid) > 0)
        isLine = true;
    return isLine;
}

// Apply settings in pop

function applyPopSetting(popName) {
    switch (popName.toLowerCase()) {
        case "general":
            if (chartVisualizationDecided != "radar") {
                applyGeneralTabSettings();
            }
            else {
                applyGeneralTabSettingsForRadar();
            }
            break;
        case "series":
            if (chartVisualizationDecided != "radar") {
                applySeriesTabSettings();
            }
            else {
                //applySeriesTabSettingsForRadar();
                closeVisPropertyPopUp();
            }
            break;
    }
}

function applySeriesTabSettingsForRadar() {
    z('popSeriesDdl').innerHTML = z('di_vcSelSeries').innerHTML;
    z('popSeriesColorTxt').value = "#ffffff";
    di_jq('#popSeriesColorTxt').css("backgroundColor", z('popSeriesColorTxt').value);
    di_jq('#popSeriesColorTxt').css("color", z('popSeriesColorTxt').value);
    z('popSeriesColorTxt2').value = z('popSeriesBdrColor').value = "#ffffff";
    di_jq('#popSeriesColorTxt2').css("backgroundColor", "#ffffff");
    di_jq('#popSeriesColorTxt2').css("color", "#ffffff");
    di_jq('#popSeriesBdrColor').css("backgroundColor", "#ffffff");
    di_jq('#popSeriesBdrColor').css("color", "#ffffff");
    z('popSeriesBdrColor').disabled = false;
    var seriesItems = z('popSeriesDdl').options.length;
    if (seriesItems > 1) {
        var index;
        for (index = 1; index < seriesItems; index++) {
            var seriesTempItem = new Object();
            var seriesname = z('popSeriesDdl').options[index].text;
            seriesTempItem.color = di_GetRadarSeriesColor(chartVisualizationDecided, seriesname);
            //seriesTempItem.width = getSeriesBorderWidth(chartVisualizationDecided,chartVisualizationDecided);
            //seriesTempItem.borderColor = getSeriesBorderColor(chartVisualizationDecided,chartVisualizationDecided);
            //seriesTempItem.gcStyle = "1";
            Radar.series.push(seriesTempItem);
        }
    }
}
var Radar = { series: [] };
/*Apply radar general settings while popup opens*/
function applyGeneralTabSettingsForRadar() {
    var currentPopup = z('popupTab').value;
    if (currentPopup == "general") {
        z('popBorderChkbox').defaultChecked = di_GetRadarBorderVisible(chartVisualizationDecided);
        z('popBorderWidthTxt').value = di_GetRadarChartBorderWidth(chartVisualizationDecided);

        var borderColor = di_GetRadarChartBorderColor(chartVisualizationDecided);
        borderColor = borderColor.replace("0x", "#");
        if (borderColor == "#0") {
            borderColor = "#000000";
        }
        z('popchartBoderColor').value = borderColor;
        di_jq('#popchartBoderColor').css("backgroundColor", borderColor);
        di_jq('#popchartBoderColor').css("color", borderColor);
        z('popBorderRadiusTxt').value = di_GetRadarChartBorderRadius(chartVisualizationDecided);

        var chartBgColor = di_GetRadarBackgroundColor(chartVisualizationDecided);
        chartBgColor = chartBgColor.replace("0x", "#");
        if (chartBgColor == "#0") {
            chartBgColor = "#000000";
        }
        z('popChartBgColorTxt').value = chartBgColor;
        di_jq('#popChartBgColorTxt').css("backgroundColor", chartBgColor);
        di_jq('#popChartBgColorTxt').css("color", chartBgColor);
        if (chartBgColor == "#ffffff") {
            z('popChartBgNoColor').defaultChecked = true;
            chartAreabgColor('no');
        }
        else {
            z('popChartBgColor').defaultChecked = true;
            chartAreabgColor('solid');
        }
        z('popLegendChkbox').defaultChecked = z('di_vcplegend').checked;
        z('popLegendBorderChkbox').defaultChecked = di_GetRadarLegendBorderVisible(chartVisualizationDecided);
        z('popLegendBorderTxt').value = di_GetRadarLegendBorderWidth(chartVisualizationDecided);
        var legendBorderColor = di_GetRadarLegendBorderColor(chartVisualizationDecided);
        legendBorderColor = legendBorderColor.replace("0x", "#");
        if (legendBorderColor == "#0") {
            legendBorderColor = "#000000";
        }
        z('popLegendBorderColorTxt').value = legendBorderColor;
        di_jq('#popLegendBorderColorTxt').css("backgroundColor", legendBorderColor);
        di_jq('#popLegendBorderColorTxt').css("color", legendBorderColor);
        z('popLegendBorderRadiusTxt').value = di_GetRadarLegendBorderRadius(chartVisualizationDecided);
        var legendBgColor = di_GetRadarLegendBgColor(chartVisualizationDecided);
        legendBgColor = legendBgColor.replace("0x", "#");
        if (legendBgColor == "#0") {
            legendBgColor = "#000000";
        }
        z('poplegendBgColorTxt').value = legendBgColor;
        di_jq('#poplegendBgColorTxt').css("backgroundColor", legendBgColor);
        di_jq('#poplegendBgColorTxt').css("color", legendBgColor);
        var legendLayout = di_GetRadarLegendLayout(chartVisualizationDecided);
        di_setDropdownlist('popLengedPos', legendLayout);
        var seriesType = di_GetRadarSeriesType(chartVisualizationDecided);
        if (seriesType == "l") {
            z("vcpoplineradio").checked = true;
            z("vcpopcolumnradio").checked = false;
        }
        else {
            z("vcpoplineradio").checked = false;
            z("vcpopcolumnradio").checked = true;
        }
        var selectedRadialAxis = di_GetRadialAxis(chartVisualizationDecided);
        di_setDropdownlist('vcpopRadialAxisddl', selectedRadialAxis);

        var selectedForm = di_GetRadarWaveLayout(chartVisualizationDecided);
        di_setDropdownlist('vcpopFormDdl', selectedForm);
        var chartShape = di_GetRadarType(chartVisualizationDecided);
        if (chartShape == "c") {
            z("vcpopCircularradio").defaultChecked = true;
            z("vcpopPolygonalradio").defaultChecked = false;
        }
        else if (chartShape == "p") {
            z("vcpopCircularradio").defaultChecked = false;
            z("vcpopPolygonalradio").defaultChecked = true;
        }
    }
    else if (currentPopup == "series") {
    }
}

// General Tab Settings while showing popup
function applyGeneralTabSettings() {
    try {
        var lineStyleArrayValue = ["Select Style", "Solid", "ShortDash", "ShortDot", "ShortDashDot", "ShortDashDotDot", "Dot", "Dash", "LongDash", "DashDot", "LongDashDot", "LongDashDotDot"];
        var lineStyleArrayText = [z("lngSelectStyleTxt").innerHTML, z("lngSolidTxt").innerHTML, z("lngShortDashTxt").innerHTML, z("lngShortDotTxt").innerHTML, z("lngShortDashDotTxt").innerHTML, z("lngShortDashDotDotTxt").innerHTML, z("lngDotTxt").innerHTML, z("lngDashTxt").innerHTML, z("lngLongDashTxt").innerHTML, z("lngDashDotTxt").innerHTML, z("lngLongDashDotTxt").innerHTML, z("lngLongDashDotDotTxt").innerHTML];
        //var legendPositionsValue = ["Select", "Left top", "Left middle", "Left bottom", "Top", "Bottom", "Right top", "Right middle", "Right bottom"];
        var legendPositions = [z("lngSelectTxt").innerHTML, z("lngLeftTopTxt").innerHTML, z("lngLeftMiddleTxt").innerHTML, z("lngLeftBottomTxt").innerHTML, z("lngTopTxt").innerHTML, z("lngBottomTxt").innerHTML, z("lngRightTopTxt").innerHTML, z("lngRightMiddleTxt").innerHTML, z("lngRightBottomTxt").innerHTML];
        //var gradientStyleValue = ["Select", "Top to bottom", "Bottom to top", "Left to right", "Right to left"];
        var gradientStyle = [z("lngSelectTxt").innerHTML, z("lngTopToBottomTxt").innerHTML, z("lngBottomToTopTxt").innerHTML, z("lngLeftToRightTxt").innerHTML, z("lngRightToLeftTxt").innerHTML];
        di_createFontSizeDdl('popLegendFontDdl');
        z('popBorderChkbox').defaultChecked = z('di_vcpborder').checked;
        z('popBorderWidthTxt').value = di_getChartBorderWidth(chartVisualizationDecided);
        z('popchartBoderColor').value = di_getChartBorderColor(chartVisualizationDecided);
        di_jq('#popchartBoderColor').css("backgroundColor", z('popchartBoderColor').value);
        di_jq('#popchartBoderColor').css("color", z('popchartBoderColor').value);
        z('popBorderRadiusTxt').value = di_getChartBorderRadius(chartVisualizationDecided);
        if (chartVisualizationDecided != "pie") {
            z('popMajorGridChkbox').defaultChecked = z('di_vcpgridx').checked;
            z('popGridLineWidthTxt').value = di_getXAxisMajorGridWidth(chartVisualizationDecided);
            z('popGridLineColorTxt').value = di_getXAxisMajorGridColor(chartVisualizationDecided);
            di_jq('#popGridLineColorTxt').css("backgroundColor", z('popGridLineColorTxt').value);
            di_jq('#popGridLineColorTxt').css("color", z('popGridLineColorTxt').value);
            var minorXWidth = di_getXAxisMinorGridWidth(chartVisualizationDecided);
            if (minorXWidth > 0) {
                z('popXMinorLineChkbox').defaultChecked = true;
            }
            else {
                z('popXMinorLineChkbox').defaultChecked = false;
            }
            z('popMinorXLineWidthTxt').value = minorXWidth;
            z('popMinorXLineColorTxt').value = di_getXAxisMinorGridColor(chartVisualizationDecided);
            di_jq('#popMinorXLineColorTxt').css("backgroundColor", z('popMinorXLineColorTxt').value);
            di_jq('#popMinorXLineColorTxt').css("color", z('popMinorXLineColorTxt').value);
        }
        else {
            z('popGridLineWidthTxt').disabled = true;
            z('popGridLineColorTxt').disabled = true;
            z('popXMinorLineChkbox').disabled = true;
            z('popMinorXLineWidthTxt').disabled = true;
            z('popMinorXLineColorTxt').disabled = true;
            z('popAxisDdl').disabled = true;
            z('popMajorGridChkbox').disabled = true;
            z('popMajorLineStyle').disabled = true;
            z('popMinorLineStyle').disabled = true;
        }
        var chartBgColor = di_getChartBgColor(chartVisualizationDecided);
        if (chartBgColor == "rgba(255, 255, 255, 0.0)") {
            z('popChartBgNoColor').defaultChecked = true;
            chartAreabgColor('no');
        }
        else if (chartBgColor.indexOf('~') > -1) {
            z('popChartBgGColor').defaultChecked = true;
            chartAreabgColor('grad');
            z('popChartBgColorTxt').value = chartBgColor.split('~')[0];
            di_jq('#popChartBgColorTxt').css("backgroundColor", z('popChartBgColorTxt').value);
            di_jq('#popChartBgColorTxt').css("color", z('popChartBgColorTxt').value);
            z('popChartBgColorTxt2').value = chartBgColor.split('~')[1];
            di_jq('#popChartBgColorTxt2').css("backgroundColor", z('popChartBgColorTxt2').value);
            di_jq('#popChartBgColorTxt2').css("color", z('popChartBgColorTxt2').value);
        }
        else {
            chartAreabgColor('solid');
            z('popChartBgColor').defaultChecked = true;
            z('popChartBgColorTxt').value = chartBgColor;
            di_jq('#popChartBgColorTxt').css("backgroundColor", z('popChartBgColorTxt').value);
            di_jq('#popChartBgColorTxt').css("color", z('popChartBgColorTxt').value);
        }
        var chartPBgColor = di_getChartPlotAreaColor(chartVisualizationDecided);
        if (chartPBgColor == "rgba(255, 255, 255, 0.0)") {
            z('popChartPBgNoColor').defaultChecked = true;
            chartPAreabgColor('no');
        }
        else if (chartPBgColor.indexOf('~') > -1) {
            z('popChartPBgGColor').defaultChecked = true;
            chartPAreabgColor('grad');
            z('popChartPBgColorTxt').value = chartPBgColor.split('~')[0];
            di_jq('#popChartPBgColorTxt').css("backgroundColor", z('popChartPBgColorTxt').value);
            di_jq('#popChartPBgColorTxt').css("color", z('popChartPBgColorTxt').value);
            z('popChartPBgColorTxt2').value = chartPBgColor.split('~')[1];
        }
        else {
            z('popChartPBgColor').defaultChecked = true;
            chartPAreabgColor('solid');
            z('popChartPBgColorTxt').value = chartPBgColor;
            di_jq('#popChartPBgColorTxt').css("backgroundColor", chartPBgColor);
            di_jq('#popChartPBgColorTxt').css("color", chartPBgColor);
        }
        z('popLegendChkbox').defaultChecked = z('di_vcplegend').checked;
        z('popLegendBorderTxt').value = di_getLegendBorderWidth(chartVisualizationDecided);
        if (z('popLegendBorderTxt').value == "0") {
            z('popLegendBorderChkbox').defaultChecked = false;
        }
        else {
            z('popLegendBorderChkbox').defaultChecked = true;
        }
        z('popLegendBorderColorTxt').value = di_getLegendBorderColor(chartVisualizationDecided);
        di_jq('#popLegendBorderColorTxt').css("backgroundColor", z('popLegendBorderColorTxt').value);
        di_jq('#popLegendBorderColorTxt').css("color", z('popLegendBorderColorTxt').value);
        z('popLegendBorderRadiusTxt').value = di_getLegendBorderRadius(chartVisualizationDecided);
        z('poplegendBgColorTxt').value = di_getLegendBgColor(chartVisualizationDecided);
        if (z('poplegendBgColorTxt').value == "rgba(255, 255, 255, 0.0)") {
            z('legendBgColorChkbox').defaultChecked = false;
        }
        else {
            z('legendBgColorChkbox').defaultChecked = true;
        }
        di_jq('#poplegendBgColorTxt').css("backgroundColor", z('poplegendBgColorTxt').value);
        di_jq('#poplegendBgColorTxt').css("color", z('poplegendBgColorTxt').value);
        z('legendFloatingChkbox').defaultChecked = di_getIsLegendFloating(chartVisualizationDecided);
        z('legendFixedChkbox').defaultChecked = !di_getIsLegendFloating(chartVisualizationDecided);
        z('legendXTxt').value = di_getLegendXPos(chartVisualizationDecided);
        z('legendYTxt').value = di_getLegendYPos(chartVisualizationDecided);
        var legendFontSize = di_getLegendFontSize(chartVisualizationDecided);
        di_setDropdownlist('popLegendFontDdl', legendFontSize);
        createLineStyleDdl(lineStyleArrayText, lineStyleArrayValue, 'popMajorLineStyle');
        createLineStyleDdl(lineStyleArrayText, lineStyleArrayValue, 'popMinorLineStyle');
        createDropdownOptions(legendPositions, 'popLengedPos');
        createDropdownOptions(gradientStyle, 'popChartBgStyleDdl');
        createDropdownOptions(gradientStyle, 'popChartPBgColorDdl');
        if (di_getLegendWidth(chartVisualizationDecided) == "" || di_getLegendWidth(chartVisualizationDecided) == null) {
            z('legendAutoLenChkbox').defaultChecked = true;
        }
        else {
            z('poplegendwdcount').value = di_getLegendWidth(chartVisualizationDecided);
            z('legendFixLenChkbox').defaultChecked = true;
        }
        di_setDropdownlist('popLengedPos', di_getLegendPosition(chartVisualizationDecided));
        di_setDropdownlist('popChartBgStyleDdl', di_getBgcGStyle(chartVisualizationDecided));
        di_setDropdownlist('popChartPBgColorDdl', di_getPBgcGStyle(chartVisualizationDecided));
        legendBold = di_isLegendFontStyle(chartVisualizationDecided, 'b');
        legendItalic = di_isLegendFontStyle(chartVisualizationDecided, 'i');
        legendUnderline = di_isLegendFontStyle(chartVisualizationDecided, 'u');
        if (legendBold) {
            di_jq("#legend_bold").addClass("active");
        }
        else {
            di_jq("#legend_bold").removeClass("active");
        }
        if (legendItalic) {
            di_jq("#legend_italic").addClass("active");
        }
        else {
            di_jq("#legend_italic").removeClass("active");
        }
        if (legendUnderline) {
            di_jq("#legend_underline").addClass("active");
        }
        else {
            di_jq("#legend_underline").removeClass("active");
        }
        AxisSetting();
        di_setDropdownlist('popMajorLineStyle', Axis.xAxis.MajorAxis.style);
        di_setDropdownlist('popMinorLineStyle', Axis.xAxis.MinorAxis.style);
    }
    catch (err) { }
}
// Global variable
var legendBold, legendItalic, legendUnderline;
// Series Tab Settings while showing popup
function applySeriesTabSettings() {
    //var lineStyleArray = ["Select Style", "Solid", "ShortDash", "ShortDot", "ShortDashDot", "ShortDashDotDot", "Dot", "Dash", "LongDash", "DashDot", "LongDashDot", "LongDashDotDot"];
    var lineStyleArray = [z("lngSelectStyleTxt").innerHTML, z("lngSolidTxt").innerHTML, z("lngShortDashTxt").innerHTML, z("lngShortDotTxt").innerHTML, z("lngShortDashDotTxt").innerHTML, z("lngShortDashDotDotTxt").innerHTML, z("lngDotTxt").innerHTML, z("lngDashTxt").innerHTML, z("lngLongDashTxt").innerHTML, z("lngDashDotTxt").innerHTML, z("lngLongDashDotTxt").innerHTML, z("lngLongDashDotDotTxt").innerHTML];
    var gradientStyle = [z("lngSelectTxt").innerHTML, z("lngTopToBottomTxt").innerHTML, z("lngBottomToTopTxt").innerHTML, z("lngLeftToRightTxt").innerHTML, z("lngRightToLeftTxt").innerHTML];
    //var gradientStyle = ["Select","Top to bottom","Bottom to top","Left to right","Right to left"];		
    if (chartVisualizationDecided != "pie") {
        var seriesColArray = di_getSeriesNames(chartVisualizationDecided);
        seriesColArray[0] = z("mapTxtSelSeries").innerHTML;
        createDropdownOptions(seriesColArray, "popSeriesDdl");
    }
    else {
        z('popSeriesDdl').innerHTML = z('di_vcSelSeries').innerHTML;
    }
    z('popSeriesWidthTxt').value = "0";
    z('popSeriesColorTxt').value = "#ffffff";
    di_jq('#popSeriesColorTxt').css("backgroundColor", z('popSeriesColorTxt').value);
    di_jq('#popSeriesColorTxt').css("color", z('popSeriesColorTxt').value);
    createDropdownOptions(lineStyleArray, 'popSeriesStyle');
    z('popSeriesColorTxt2').value = z('popSeriesBdrColor').value = "#ffffff";
    di_jq('#popSeriesColorTxt2').css("backgroundColor", "#ffffff");
    di_jq('#popSeriesColorTxt2').css("color", "#ffffff");
    di_jq('#popSeriesBdrColor').css("backgroundColor", "#ffffff");
    di_jq('#popSeriesBdrColor').css("color", "#ffffff");
    if (chartVisualizationDecided == "line" || chartVisualizationDecided == "area") {
        z('popSeriesStyle').disabled = false;
        z('popSeriesBdrColor').disabled = true;
    }
    else {
        z('popSeriesStyle').disabled = true;
        z('popSeriesBdrColor').disabled = false;
    }
    if (chartVisualizationDecided == "column" || chartVisualizationDecided == "bar" || chartVisualizationDecided == "pyramid") {
        var GapVal = di_getGroupPadding(chartVisualizationDecided, chartVisualizationDecided);
        var OverlapVal = di_getBarPadding(chartVisualizationDecided, chartVisualizationDecided);
        z('srGapNum').disabled = z('srOverlapNum').disabled = false;
        z('srGapNum').value = GapVal;
        z('srOverlapNum').value = OverlapVal;
    }
    else {
        z('srGapNum').disabled = z('srOverlapNum').disabled = true;
    }
    di_jq("#sliderId1").slider({ value: GapVal * 100 });
    di_jq("#sliderId2").slider({ value: OverlapVal * 100 });
    legendBold = di_isLegendFontStyle(chartVisualizationDecided, 'b');
    legendItalic = di_isLegendFontStyle(chartVisualizationDecided, 'i');
    legendUnderline = di_isLegendFontStyle(chartVisualizationDecided, 'u');
    var seriesItems = z('popSeriesDdl').options.length;
    if (seriesItems > 1) {
        var index;
        for (index = 1; index < seriesItems; index++) {
            var seriesTempItem = new Object();
            seriesTempItem.color = di_selectSeriesName(chartVisualizationDecided, index - 1);
            seriesTempItem.width = getSeriesBorderWidth(chartVisualizationDecided, chartVisualizationDecided);
            seriesTempItem.borderColor = getSeriesBorderColor(chartVisualizationDecided, chartVisualizationDecided);
            seriesTempItem.gcStyle = "1";
            Axis.series.push(seriesTempItem);
        }
    }
    createDropdownOptions(gradientStyle, 'popSeriesStyleTxt');
}
var Axis = { xAxis: { MajorAxis: { enabled: false, width: 1, color: "", style: "" }, MinorAxis: { enabled: false, width: 1, color: "", style: ""} }, yAxis: { MajorAxis: { enabled: false, width: 1, color: "", style: "" }, MinorAxis: { enabled: false, width: 1, color: "", style: ""} }, series: [] };

function AxisSetting() {
    Axis.xAxis.MajorAxis.enabled = z('di_vcpgridx').checked;
    Axis.xAxis.MajorAxis.width = di_getXAxisMajorGridWidth(chartVisualizationDecided);
    Axis.xAxis.MajorAxis.color = di_getXAxisMajorGridColor(chartVisualizationDecided);
    Axis.xAxis.MajorAxis.style = di_getMajorGridDashLine(chartVisualizationDecided, 0);
    Axis.xAxis.MinorAxis.width = di_getXAxisMinorGridWidth(chartVisualizationDecided);
    Axis.xAxis.MinorAxis.color = di_getXAxisMinorGridColor(chartVisualizationDecided);
    Axis.xAxis.MinorAxis.style = di_getMinorGridDashLine(chartVisualizationDecided, 0);
    if (Axis.xAxis.MinorAxis.width > 0) {
        Axis.xAxis.MinorAxis.enabled = true;
    }
    else {
        Axis.xAxis.MinorAxis.enabled = false;
    }

    Axis.yAxis.MajorAxis.enabled = z('di_vcpgridy').checked;
    Axis.yAxis.MajorAxis.width = di_getYAxisMajorGridWidth(chartVisualizationDecided);
    Axis.yAxis.MajorAxis.color = di_getYAxisMajorGridColor(chartVisualizationDecided);
    Axis.yAxis.MajorAxis.style = di_getMajorGridDashLine(chartVisualizationDecided, 1);
    Axis.yAxis.MinorAxis.width = di_getYAxisMinorGridWidth(chartVisualizationDecided);
    Axis.yAxis.MinorAxis.color = di_getYAxisMinorGridColor(chartVisualizationDecided);
    Axis.yAxis.MinorAxis.style = di_getMinorGridDashLine(chartVisualizationDecided, 1);
    if (Axis.yAxis.MinorAxis.width > 0) {
        Axis.yAxis.MinorAxis.enabled = true;
    }
    else {
        Axis.yAxis.MinorAxis.enabled = false;
    }
    legendFontcolorCode = di_getLegendFontColor(chartVisualizationDecided);
}
function ApplyClosePopup() {
   
    if (z('popPresentationName')) {
        var presentationName = z('popPresentationName').value;
        if (presentationName.length == 0) {
            alert(z("langNameMandatory").innerHTML);
            return;
        }
        di_jq('#divShowLoading').show();
        //    di_jq('#divShowLoadingText').html("Saving...");
        di_jq('#divShowLoadingText').html(z('langSavingGallery').value);
        setTimeout("postGalleryDetails();", 1000);
    }
    else {
        di_jq('#divShowLoading').show();
        di_jq('#divShowLoadingText').html("Applying...");
        if (chartVisualizationDecided == "radar") {
            applyMoreSettingsToRadar();
        }
        else {
            applyMoreSettings();
        }
        di_jq('#divShowLoading').hide();
        closeVisPropertyPopUp();
    }
    //setTimeout(function () { di_jq('#divShowLoading').hide(); }, 5000);
    //closeVisPropertyPopUp();
}

function Apply() {
   // alert("got it",1);
    di_jq('#divShowLoadingText').html("Applying...");
    di_jq('#divShowLoading').show();
    if (chartVisualizationDecided == "radar") {
        applyMoreSettingsToRadar();
    }
    else {
        applyMoreSettings();
    }
    di_jq('#divShowLoading').hide();
}
/*Apply more settings on chart*/
function applyMoreSettings() {
   // alert("got it",2);
    var currentPopup = z('popupTab').value;

    var chartObj = di_getChartObject(chartVisualizationDecided);
    var chartInput = chartObj.options;
    if (currentPopup == "general") {
        if (z('popBorderChkbox').checked) {
            var chartWidth = parseInt(z('popBorderWidthTxt').value);
            chartInput.chart.borderWidth = chartWidth;
            //chartInput.chart.width = chartInput.chart.width - 2*chartWidth;
            //chartInput.chart.height = chartInput.chart.height - 2*chartWidth;
            di_setChartBorderValue(chartVisualizationDecided, chartWidth);
        }
        else {
            chartInput.chart.borderWidth = 0;
        }
        if (z('popchartBoderColor').value != "") {
            chartInput.chart.borderColor = z('popchartBoderColor').value;
        }
        if (z('popBorderRadiusTxt').value != "") {
            chartInput.chart.borderRadius = z('popBorderRadiusTxt').value;
        }
        if (chartVisualizationDecided != "pie") {
            if (chartInput.xAxis.length) {
                chartInput.xAxis[0].gridLineColor = Axis.xAxis.MajorAxis.color;
                chartInput.xAxis[0].minorGridLineColor = Axis.xAxis.MinorAxis.color;
                chartInput.xAxis[0].gridLineDashStyle = Axis.xAxis.MajorAxis.style;
                chartInput.xAxis[0].minorGridLineDashStyle = Axis.xAxis.MinorAxis.style;
                if (Axis.xAxis.MajorAxis.enabled) {
                    chartInput.xAxis[0].gridLineWidth = Axis.xAxis.MajorAxis.width;
                }
                else {
                    chartInput.xAxis[0].gridLineWidth = 0;
                }
                if (Axis.xAxis.MinorAxis.enabled) {
                    chartInput.xAxis[0].minorGridLineWidth = Axis.xAxis.MinorAxis.width;
                }
                else {
                    chartInput.xAxis[0].minorGridLineWidth = 0;
                }
            }
            else {
                chartInput.xAxis.gridLineColor = Axis.xAxis.MajorAxis.color;
                chartInput.xAxis.minorGridLineColor = Axis.xAxis.MinorAxis.color;
                chartInput.xAxis.gridLineDashStyle = Axis.xAxis.MajorAxis.style;
                chartInput.xAxis.minorGridLineDashStyle = Axis.xAxis.MinorAxis.style;
                if (Axis.xAxis.MajorAxis.enabled) {
                    chartInput.xAxis.gridLineWidth = Axis.xAxis.MajorAxis.width;
                }
                else {
                    chartInput.xAxis.gridLineWidth = 0
                }
                if (Axis.xAxis.MinorAxis.enabled) {
                    chartInput.xAxis.minorGridLineWidth = Axis.xAxis.MinorAxis.width;
                }
                else {
                    chartInput.xAxis.minorGridLineWidth = 0;
                }
            }
            chartInput.yAxis.gridLineColor = Axis.yAxis.MajorAxis.color;
            chartInput.yAxis.minorGridLineColor = Axis.yAxis.MinorAxis.color;
            chartInput.yAxis.gridLineDashStyle = Axis.yAxis.MajorAxis.style;
            chartInput.yAxis.minorGridLineDashStyle = Axis.yAxis.MinorAxis.style;
            if (Axis.yAxis.MajorAxis.enabled) {
                chartInput.yAxis.gridLineWidth = Axis.yAxis.MajorAxis.width;
            }
            else {
                chartInput.yAxis.gridLineWidth = 0
            }
            if (Axis.yAxis.MinorAxis.enabled) {
                chartInput.yAxis.minorGridLineWidth = Axis.yAxis.MinorAxis.width;
            }
            else {
                chartInput.yAxis.minorGridLineWidth = 0;
            }
        }
        var chartBgColor;
        if (z('popChartBgNoColor').checked) {
            chartBgColor = 'rgba(255, 255, 255, 0.0)'; //No fill
        }
        else if (z('popChartBgColor').checked) {
            chartBgColor = z('popChartBgColorTxt').value // color
        }
        else if (z('popChartBgGColor').checked) {
            //chartBgColor = z('popChartBgColorTxt') // gradeint color
            chartBgColor = GradeintColor(z('popChartBgColorTxt').value, z('popChartBgColorTxt2').value, z('popChartBgStyleDdl').value);
            di_setBgcGStyle(chartVisualizationDecided, z('popChartBgStyleDdl').value);
        }
        chartInput.chart.backgroundColor = chartBgColor;
        var chartPlotBgColor;
        if (z('popChartPBgNoColor').checked) {
            chartPlotBgColor = 'rgba(255, 255, 255, 0.0)'; //No fill
        }
        else if (z('popChartPBgColor').checked) {
            chartPlotBgColor = z('popChartPBgColorTxt').value // color
        }
        else if (z('popChartPBgGColor').checked) {
            //chartBgColor = z('popChartBgColorTxt') // gradeint color
            chartPlotBgColor = GradeintColor(z('popChartPBgColorTxt').value, z('popChartPBgColorTxt2').value, z('popChartPBgColorDdl').value);
            di_setPBgcGStyle(chartVisualizationDecided, z('popChartPBgColorDdl').value);
        }
        chartInput.chart.plotBackgroundColor = chartPlotBgColor;
        chartInput.legend.enabled = z('di_vcplegend').defaultChecked = z('popLegendChkbox').checked;
        if (z('popLegendBorderChkbox').checked) {
            chartInput.legend.borderWidth = z('popLegendBorderTxt').value;
            di_setLegendBorderValue(chartVisualizationDecided, z('popLegendBorderTxt').value)
        }
        else {
            chartInput.legend.borderWidth = 0;
        }
        chartInput.legend.borderColor = z('popLegendBorderColorTxt').value;
        chartInput.legend.borderRadius = z('popLegendBorderRadiusTxt').value;
        var legendBgColor;
        if (!z('legendBgColorChkbox').checked) {
            legendBgColor = 'rgba(255, 255, 255, 0.0)';
        }
        else {
            legendBgColor = z('poplegendBgColorTxt').value;
        }
        chartInput.legend.backgroundColor = legendBgColor;
        chartInput.legend.floating = z('legendFloatingChkbox').checked;
        var fontStyleArray = chartInput.legend.itemStyle.font.split(' ');
        chartInput.legend.itemStyle.font = fontStyleArray[0] + " " + z('popLegendFontDdl').value + " " + fontStyleArray[2];
        if (z('legendFloatingChkbox').checked) {
            chartInput.legend.align = "left";
            chartInput.legend.verticalAlign = "top";
            chartInput.legend.x = parseInt(z('legendXTxt').value);
            chartInput.legend.y = parseInt(z('legendYTxt').value);
        }
        if (z('legendFixedChkbox').checked) {
            chartInput = di_setLegendPosition(chartInput, z('popLengedPos').value);
        }
        if (z('legendAutoLenChkbox').checked) {
            chartInput.legend.width = "";
        }
        else {
            chartInput.legend.width = parseInt(z('poplegendwdcount').value);
        }
        if (legendBold) {
            chartInput.legend.itemStyle.fontWeight = "bold";
        }
        else {
            chartInput.legend.itemStyle.fontWeight = "normal";
        }
        if (legendUnderline) {
            chartInput.legend.itemStyle.textDecoration = "underline";
        }
        else {
            chartInput.legend.itemStyle.textDecoration = "none";
        }
        var italic = chartInput.legend.itemStyle.font;
        if (legendItalic) {
            italic = italic.replace('normal', 'italic');
        }
        else {
            italic = italic.replace('italic', 'normal');
        }
        chartInput.legend.itemStyle.font = italic;
        chartInput.legend.itemStyle.color = legendFontcolorCode;
    }
    else if (currentPopup == "series") {
        switch (chartVisualizationDecided) {
            case "column":
                chartInput.plotOptions.column.groupPadding = z('srGapNum').value;
                chartInput.plotOptions.column.pointPadding = z('srOverlapNum').value;
                break;
            case "bar":
                chartInput.plotOptions.bar.groupPadding = z('srGapNum').value;
                chartInput.plotOptions.bar.pointPadding = z('srOverlapNum').value;
                break;
            case "pyramid":
                chartInput.plotOptions.bar.groupPadding = z('srGapNum').value;
                chartInput.plotOptions.bar.pointPadding = z('srOverlapNum').value;
                break;
        }
        var seriesItems = z('popSeriesDdl').options.length;
        if (seriesItems > 1) {
            for (var seriesIndex = 1; seriesIndex < seriesItems; seriesIndex++) {
                //var index = seriesIndex - 1;
                chartInput.colors[seriesIndex - 1] = Axis.series[seriesIndex - 1].color;
                switch (chartVisualizationDecided) {
                    case "column":
                        chartInput.plotOptions.column.borderWidth = Axis.series[seriesIndex - 1].width;
                        chartInput.plotOptions.column.borderColor = Axis.series[seriesIndex - 1].borderColor;
                        break;
                    case "line":
                        var borderStyleName = Axis.series[seriesIndex - 1].style;
                        chartInput.plotOptions.line.lineWidth = Axis.series[seriesIndex - 1].width;
                        chartInput.plotOptions.line.dashStyle = borderStyleName;
                        break;
                    case "bar":
                        chartInput.plotOptions.bar.borderWidth = Axis.series[seriesIndex - 1].width;
                        chartInput.plotOptions.bar.borderColor = Axis.series[seriesIndex - 1].borderColor;
                        break;
                    case "area":
                        var borderStyleName = Axis.series[seriesIndex - 1].style;
                        chartInput.plotOptions.area.lineWidth = Axis.series[seriesIndex - 1].width;
                        chartInput.plotOptions.area.dashStyle = borderStyleName;
                        break;
                    case "pie":
                        chartInput.plotOptions.pie.borderWidth = Axis.series[seriesIndex - 1].width;
                        chartInput.plotOptions.pie.borderColor = Axis.series[seriesIndex - 1].borderColor;
                        break;
                    case "donut":
                        chartInput.plotOptions.pie.borderWidth = Axis.series[seriesIndex - 1].width;
                        chartInput.plotOptions.pie.borderColor = Axis.series[seriesIndex - 1].borderColor;
                        break;
                    case "pyramid":
                        chartInput.plotOptions.bar.borderWidth = Axis.series[seriesIndex - 1].width;
                        chartInput.plotOptions.bar.borderColor = Axis.series[seriesIndex - 1].borderColor;
                        break;
                }
            }
        }
    }
   
    chartInput.tooltip.style.padding = 5;
    chartObj = new Highcharts.Chart(chartInput);
    setTimeout("applyControlsSettings()", 1000);
}

/*Apply more settings on Radar*/
function applyMoreSettingsToRadar() {
    var currentPopup = z('popupTab').value;
    if (currentPopup == "general") {
        di_RadarBorderVisible(chartVisualizationDecided, z('popBorderChkbox').checked);
        if (z('popBorderChkbox').checked) {
            var chartWidth = z('popBorderWidthTxt').value;
            di_RadarChartBorderWidth(chartVisualizationDecided, chartWidth);
        }
        var borderColor = z('popchartBoderColor').value;
        borderColor = borderColor.replace("#", "0x");
        di_RadarChartBorderColor(chartVisualizationDecided, borderColor);
        di_RadarChartBorderRadius(chartVisualizationDecided, z('popBorderRadiusTxt').value)

        var chartBgColor = "0xffffff";
        if (z('popChartBgNoColor').checked) {
            chartBgColor = '0xffffff'; //No fill
        }
        else if (z('popChartBgColor').checked) {
            chartBgColor = z('popChartBgColorTxt').value // color
        }
        chartBgColor = chartBgColor.replace("#", "0x");
        di_RadarBackgroundColor(chartVisualizationDecided, chartBgColor);
        z('di_vcplegend').defaultChecked = z('popLegendChkbox').checked;
        di_RadarLegendVisible(chartVisualizationDecided, z('popLegendChkbox').checked);
        di_RadarLegendBorderVisible(chartVisualizationDecided, z('popLegendBorderChkbox').checked);
        if (z('popLegendBorderChkbox').checked) {

            di_RadarLegendBorderWidth(chartVisualizationDecided, z('popLegendBorderTxt').value);
        }
        var legendBorderColor = z('popLegendBorderColorTxt').value;
        legendBorderColor = legendBorderColor.replace("#", "0x");
        di_RadarLegendBorderColor(chartVisualizationDecided, legendBorderColor);
        di_RadarLegendBorderRadius(chartVisualizationDecided, z('popLegendBorderRadiusTxt').value);
        var legendBgColor;
        if (!z('legendBgColorChkbox').checked) {
            legendBgColor = '0xffffff';
        }
        else {
            legendBgColor = z('poplegendBgColorTxt').value;
        }
        legendBgColor = legendBgColor.replace("#", "0x");
        di_RadarLegendBgColor(chartVisualizationDecided, legendBgColor);
        var legendLayout = "r";
        legendLayout = z('popLengedPos').value;
        if (z("vcpoplineradio").checked) {
            di_RadarSeriesType(chartVisualizationDecided, 'l');
        }
        else {
            di_RadarSeriesType(chartVisualizationDecided, 'c');
        }
        var selectedRadialAxis = z("vcpopRadialAxisddl").value;
        if (selectedRadialAxis == "-1") {
            selectedRadialAxis = "n";
        }
        di_RadialAxis(chartVisualizationDecided, selectedRadialAxis);
        var selectedForm = z("vcpopFormDdl").value;
        if (selectedForm == "-1") {
            selectedForm = "s";
        }
        di_RadarWaveLayout(chartVisualizationDecided, selectedForm);
        if (z("vcpopCircularradio").checked) {
            di_RadarType(chartVisualizationDecided, "c");
        }
        else {
            di_RadarType(chartVisualizationDecided, "p");
        }
        di_RadarLegendLayout(chartVisualizationDecided, legendLayout);
    }
    else if (currentPopup == "series") {
        /*var seriesItems = z('popSeriesDdl').options.length;
        if(seriesItems>1)		
        {
        for(var seriesIndex = 1;seriesIndex<seriesItems;seriesIndex++)
        {
        //var index = seriesIndex - 1;
        chartInput.colors[seriesIndex-1] = Axis.series[seriesIndex - 1].color;			    
        switch(chartVisualizationDecided)
        {
        case "column":
        chartInput.plotOptions.column.borderWidth =  Axis.series[seriesIndex - 1].width;
        chartInput.plotOptions.column.borderColor = Axis.series[seriesIndex - 1].borderColor;
        break;
        case "line":
        var borderStyleName = Axis.series[seriesIndex - 1].style;
        chartInput.plotOptions.line.lineWidth = Axis.series[seriesIndex - 1].width;
        chartInput.plotOptions.line.dashStyle = borderStyleName;
        break;
        case "bar":
        chartInput.plotOptions.bar.borderWidth = Axis.series[seriesIndex - 1].width;
        chartInput.plotOptions.bar.borderColor = Axis.series[seriesIndex - 1].borderColor;
        break;
        case "area":
        var borderStyleName = Axis.series[seriesIndex - 1].style;
        chartInput.plotOptions.area.lineWidth = Axis.series[seriesIndex - 1].width;
        chartInput.plotOptions.area.dashStyle = borderStyleName;
        break;
        case "pie":
        chartInput.plotOptions.pie.borderWidth = Axis.series[seriesIndex - 1].width;
        chartInput.plotOptions.pie.borderColor =  Axis.series[seriesIndex - 1].borderColor;
        break;
        case "donut":
        chartInput.plotOptions.pie.borderWidth = Axis.series[seriesIndex - 1].width;
        chartInput.plotOptions.pie.borderColor =  Axis.series[seriesIndex - 1].borderColor;
        break;
        case "pyramid":
        chartInput.plotOptions.bar.borderWidth = Axis.series[seriesIndex - 1].width;
        chartInput.plotOptions.bar.borderColor =  Axis.series[seriesIndex - 1].borderColor;
        break;	
        }
        }
        }*/
    }
    di_renderRadar(chartVisualizationDecided);
}



function GradeintColor(color1, color2, style) {
    var Gcolor;
    switch (style) {
        case "1":
            Gcolor = { 'linearGradient': [0, 0, 0, '100%'], 'stops': [['0', color1], ['1', color2]] };
            break;
        case "2":
            Gcolor = { 'linearGradient': ['100%', '100%', '100%', 0], 'stops': [['0', color1], ['1', color2]] };
            break;
        case "3":
            Gcolor = { 'linearGradient': [0, '100%', '100%', '100%'], 'stops': [['0', color1], ['1', color2]] };
            break;
        case "4":
            Gcolor = { 'linearGradient': ['100%', '100%', 0, '100%'], 'stops': [['0', color1], ['1', color2]] };
            break;
        default:
            Gcolor = "rgba(255, 255, 255, 0.0)";
            break;
    }
    return Gcolor;
}
function PopupChangeSeries(value) {
    var color;
    if (value == "-1") {
        color = "#ffffff";
        SeriesOptColor("no");
        return;
    }
    else {
        var index = parseInt(value) - 1;
        color = di_selectSeriesName(chartVisualizationDecided, index);
    }

    if (color == "rgba(255, 255, 255, 0.0)") {
        z('popSeriesNoColor').defaultChecked = true;
        SeriesOptColor("no");
    }
    else if (color.indexOf('~') > -1) {
        z('popSeriesGColor').checked = true;
        z('popSeriesColorTxt').value = color.split('~')[0];
        di_jq('#popSeriesColorTxt').css("backgroundColor", z('popSeriesColorTxt').value);
        di_jq('#popSeriesColorTxt').css("color", z('popSeriesColorTxt').value);
        z('popSeriesColorTxt2').value = color.split('~')[1];
        di_jq('#popSeriesColorTxt2').css("backgroundColor", z('popSeriesColorTxt2').value);
        di_jq('#popSeriesColorTxt2').css("color", z('popSeriesColorTxt2').value);
        di_setDropdownlist('popSeriesStyleTxt', Axis.series[parseInt(value) - 1].gcStyle);
        SeriesOptColor("grad");
    }
    else {
        z('popSeriesSolidColor').checked = true;
        z('popSeriesColorTxt').value = color;
        di_jq('#popSeriesColorTxt').css("backgroundColor", color);
        di_jq('#popSeriesColorTxt').css("color", color);
        SeriesOptColor("solid");
    }
    z('popSeriesWidthTxt').value = getSeriesBorderWidth(chartVisualizationDecided, chartVisualizationDecided);
    z('popSeriesBdrColor').value = getSeriesBorderColor(chartVisualizationDecided, chartVisualizationDecided);
    di_jq('#popSeriesBdrColor').css("backgroundColor", z('popSeriesBdrColor').value);
    di_jq('#popSeriesBdrColor').css("color", z('popSeriesBdrColor').value);
}

function toggleLegendItemStyle(selectedObj, styleType) {
    switch (styleType) {
        case "b":
            legendBold = !legendBold;
            break;
        case "i":
            legendItalic = !legendItalic;
            break;
        case "u":
            legendUnderline = !legendUnderline;
            break;
    }
    toggleActiveClass(selectedObj);
}
function increaseDdlValue(isIncrease) {
    var currentFontSize = z('popLegendFontDdl').value;
    currentFontSize = currentFontSize.substr(0, currentFontSize.length - 2);
    currentFontSize = parseInt(currentFontSize);
    if (isIncrease) {
        currentFontSize++;
    }
    else {
        currentFontSize--;
    }
    di_setDropdownlist('popLegendFontDdl', currentFontSize + "px");
}

function createHTMLForSharedTreemap(type) {    
    var di_version_url = di_diuilib_url + di_component_version + '/';
    var htmlStr = "<html><head></title></title>";
    htmlStr += "<!--[if IE]><script type='text/javascript' src='" + di_getBaseURL() + "libraries/js/excanvas.js'></script><![endif]-->"
    htmlStr += "<link href='" + di_version_url + "css/divc/di.base.css' rel='stylesheet' type='text/css' />";
    htmlStr += "<link href='../../../themes/default/css/style.css' rel='stylesheet'>";
    htmlStr += "<link href='" + di_version_url + "css/divc/di.treemap.css' rel='stylesheet' type='text/css' />";
    htmlStr += "<script type='text/javascript' src='" + di_getBaseURL() + "libraries/js/di.jquery-1.4.4.js'></script>";
    htmlStr += "<script type='text/javascript' src='" + di_version_url + "js/divc/di.jit.js'></script>";
    htmlStr += "<script type='text/javascript' src='" + di_version_url + "js/divc/di.treemap.js'></script>";
    htmlStr += "<script type='text/javascript' src='" + di_getBaseURL() + "libraries/js/visualizer1.js'></script>";
    if (type == "gallery")
        htmlStr += "</head><body onload=\"gettVisualizerFromGallery('treemap')\"><div><table cellpadding='0' cellspacing='0' width='100%' height='100%'>";
    else
        htmlStr += "</head><body onload='getTreeInputFromServer()'><div id='dvTreeMap'><table cellpadding='0' cellspacing='0' width='100%' height='100%'>";
    htmlStr += "<tr>" + z('treemapHeader').innerHTML + "</tr>";
    htmlStr += "<tr><td colspan='2'><div id='di_vc_treeContainer' style='height:90%'></div></td></tr>";
    htmlStr += "<tr>" + z('treemapFooter').innerHTML + "</tr></table></div>";
    // div for  link powered by devinfo 
    htmlStr += "<div id=\"logo_lt_dvinf\" style=\"padding-left: 40px;\">";
    htmlStr += "<a href=\"http://www.devinfo.org:80\" id=\"mastersiteurl\"><img src=\"../../../themes/default/images/spacer.gif\" alt=\"Powered By DevInfo\" width=\"192\" height=\"44\"></a></div>";
    htmlStr += "</body>";
    htmlStr += "<script>function getQueryStr(qSName) {var url = window.location.search.substring(1);var url = url.split('&');for (var i = 0; i < url.length; i++) {";
    htmlStr += "var strVal = url[i].split('=');if (strVal[0] == qSName) {return strVal[1];}}}";
	htmlStr += "var w = getQueryStr('w');if (w != null && w != '') {di_jq('#dvTreeMap').css('width', w + 'px');}";
	htmlStr += "var h = getQueryStr('h');if (h != null && h != '') {di_jq('#dvTreeMap').css('height', h + 'px');}";
	htmlStr += "</script>";
    htmlStr += "</html>";
    return htmlStr;
}

function createHTMLForSharedCloud(type) {
    var di_version_url = di_diuilib_url + di_component_version + '/';
    var htmlStr = "<html><head></title></title>";
    htmlStr += "<link href='" + di_version_url + "css/divc/di.base.css' rel='stylesheet' type='text/css' />";
    htmlStr += "<link href='" + di_version_url + "css/divc/di.cloud.css' rel='stylesheet' type='text/css' />";
    htmlStr += "<link href='../../../themes/default/css/style.css' rel='stylesheet'>";
    htmlStr += "<script type='text/javascript' src='" + di_getBaseURL() + "libraries/js/di.jquery-1.4.4.js'></script>";
    htmlStr += "<script type='text/javascript' src='" + di_version_url + "js/divc/di.cloud.js'></script>";
    htmlStr += "<script type='text/javascript' src='" + di_getBaseURL() + "libraries/js/visualizer1.js'></script>";
    if (type == "gallery")
        htmlStr += "</head><body onload=\"gettVisualizerFromGallery('cloud')\"><div><table cellpadding='0' cellspacing='0' width='100%' height='100%'>";
    else
        htmlStr += "</head><body onload='getCloudInputFromServer()'><div><table cellpadding='0' cellspacing='0' width='100%' height='100%'>";
    htmlStr += "<tr>" + z('treemapHeader').innerHTML + "</tr>";
    htmlStr += "<tr><td colspan='2'><div id='di_vc_treeContainer' style='height:90%'></div></td></tr>";
    htmlStr += "<tr>" + z('treemapFooter').innerHTML + "</tr></table></div>";
    // div for  link powered by devinfo 
    htmlStr += "<div id=\"logo_lt_dvinf\" style=\"padding-left: 40px;\">";
    htmlStr += "<a href=\"http://www.devinfo.org:80\" id=\"mastersiteurl\"><img src=\"../../../themes/default/images/spacer.gif\" alt=\"Powered By DevInfo\" width=\"192\" height=\"44\"></a></div>";
    htmlStr += "</body></html>";
    return htmlStr;
}

function createHTMLforTreeOrCloud(type) {
    var innerhtml = "<table cellpadding='0' cellspacing='0' width='100%' height='100%'>";
    if (type == "treemap") {
        innerhtml += "<tr id='treemapHeader'><td height='32' align='center' width='97%'><span id='di_vc_title'></span></td><td width='3%'><img id='di_vc_GoRootbtn' alt='" + z('lngGoToParent').value + "' title='" + z('lngGoToParent').value + "' src='" + di_getBaseURL() + "stock/themes/default/images/dataview/goup.png' style='cursor:pointer;'/></td></tr>";
        innerhtml += "<tr><td colspan='2'><div id='di_vc_treeContainer' style='height:418px;'></div></td></tr>";
    }
    else if (type == "cloud") {
        innerhtml += "<tr id='treemapHeader'><td colspan='2' height='32' align='center' width='100%'><span id='di_vc_title'></span></td></tr>";
        innerhtml += "<tr><td colspan='2'><div id='di_vc_treeContainer' style='height:418px;overflow:scroll'></div></td></tr>";
    }
    //innerhtml += "<tr><td height='32' align='center' colspan='2'><span id='di_vc_subtitle'></span></td></tr>";
    //innerhtml += "<tr><td height='16' align='left'><a id='di_vc_GoRootbtn' href='javascript:void(0)'>Go to root</a></td></tr>";

    innerhtml += "<tr id='treemapFooter'><td height='30' colspan='2'><span id='di_vc_source'></span></td></tr></table>";
    return innerhtml;
}

function setTreemapHtml() {
    var titleCtrl, subtitleCtrl, sourceCtrl, titleObj, subtitleObj, sourceObj;
    titleCtrl = z('di_vc_title');
    //subtitleCtrl = z('di_vc_subtitle');
    sourceCtrl = z('di_vc_source');
    titleObj = di_getTreeTitleObj(chartVisualizationDecided, 0);
    //subtitleObj = di_getTreeTitleObj(chartVisualizationDecided,1);
    sourceObj = di_getTreeTitleObj(chartVisualizationDecided, 2);
    titleCtrl.innerHTML = titleObj.text;
    titleCtrl.style.font = titleObj.style.font;
    titleCtrl.style.color = titleObj.style.color;
    titleCtrl.style.fontSize = titleObj.style.fontSize;
    titleCtrl.style.fontWeight = titleObj.style.fontWeight;
    titleCtrl.style.textDecoration = titleObj.style.textDecoration;
    /*subtitleCtrl.innerHTML = subtitleObj.text;
    subtitleCtrl.style.font = subtitleObj.style.font;
    subtitleCtrl.style.color = subtitleObj.style.color;
    subtitleCtrl.style.fontSize = subtitleObj.style.fontSize;
    subtitleCtrl.style.fontWeight = subtitleObj.style.fontWeight;
    subtitleCtrl.style.textDecoration = subtitleObj.style.textDecoration;*/
    sourceCtrl.innerHTML = sourceObj.text;
    sourceCtrl.style.font = sourceObj.style.font;
    sourceCtrl.style.color = sourceObj.style.color;
    sourceCtrl.style.fontSize = sourceObj.style.fontSize;
    sourceCtrl.style.fontWeight = sourceObj.style.fontWeight;
    sourceCtrl.style.textDecoration = sourceObj.style.textDecoration;
}

function storeTreemapHtml() {
    var titleCtrl, subtitleCtrl, sourceCtrl, titleObj, subtitleObj, sourceObj;
    try {
        titleCtrl = z('di_vc_title');
        //subtitleCtrl = z('di_vc_subtitle');
        sourceCtrl = z('di_vc_source');
        titleObj = di_getTreeTitleObj(chartVisualizationDecided, 0);
        //subtitleObj = di_getTreeTitleObj(chartVisualizationDecided,1);
        sourceObj = di_getTreeTitleObj(chartVisualizationDecided, 2);
        titleObj.text = titleCtrl.innerHTML;
        titleObj.style.font = titleCtrl.style.font;
        titleObj.style.color = titleCtrl.style.color;
        titleObj.style.fontSize = titleCtrl.style.fontSize;
        titleObj.style.fontWeight = titleCtrl.style.fontWeight;
        titleObj.style.textDecoration = titleCtrl.style.textDecoration;
        /*subtitleObj.text = subtitleCtrl.innerHTML;
        subtitleObj.style.font = subtitleCtrl.style.font;
        subtitleObj.style.color = subtitleCtrl.style.color;
        subtitleObj.style.fontSize = subtitleCtrl.style.fontSize;
        subtitleObj.style.fontWeight = subtitleCtrl.style.fontWeight;
        subtitleObj.style.textDecoration = subtitleCtrl.style.textDecoration;*/
        sourceObj.text = sourceCtrl.innerHTML;
        sourceObj.style.font = sourceCtrl.style.font;
        sourceObj.style.color = sourceCtrl.style.color;
        sourceObj.style.fontSize = sourceCtrl.style.fontSize;
        sourceObj.style.fontWeight = sourceCtrl.style.fontWeight;
        sourceObj.style.textDecoration = sourceCtrl.style.textDecoration;
        di_setTreeTitleObj(chartVisualizationDecided, titleObj, 0);
        //di_setTreeTitleObj(chartVisualizationDecided,subtitleObj,1);
        di_setTreeTitleObj(chartVisualizationDecided, sourceObj, 2);
    }
    catch (err) {
    }
}
function setAxisSettingOnChange(axisType, property, val) {
    switch (axisType) {
        case "xaxis":
            if (property == "glEnabled")
                Axis.xAxis.MajorAxis.enabled = val;
            else if (property == "glWidth")
                Axis.xAxis.MajorAxis.width = val;
            else if (property == "glColor")
                Axis.xAxis.MajorAxis.color = val;
            else if (property == "glStyle")
                Axis.xAxis.MajorAxis.style = val;
            else if (property == "mglEnabled")
                Axis.xAxis.MinorAxis.enabled = val;
            else if (property == "mglWidth")
                Axis.xAxis.MinorAxis.width = val;
            else if (property == "mglColor")
                Axis.xAxis.MinorAxis.color = val;
            else if (property == "mglStyle")
                Axis.xAxis.MinorAxis.style = val;
            break;
        case "yaxis":
            if (property == "glEnabled")
                Axis.yAxis.MajorAxis.enabled = val;
            else if (property == "glWidth")
                Axis.yAxis.MajorAxis.width = val;
            else if (property == "glColor")
                Axis.yAxis.MajorAxis.color = val;
            else if (property == "glStyle")
                Axis.yAxis.MajorAxis.style = val;
            else if (property == "mglEnabled")
                Axis.yAxis.MinorAxis.enabled = val;
            else if (property == "mglWidth")
                Axis.yAxis.MinorAxis.width = val;
            else if (property == "mglColor")
                Axis.yAxis.MinorAxis.color = val;
            else if (property == "mglStyle")
                Axis.yAxis.MinorAxis.style = val;
            break;
    }
}

function toggleActiveClass(selectedObj) {
    if (di_jq("#" + selectedObj.id).hasClass("active")) {
        di_jq("#" + selectedObj.id).removeClass("active");
    }
    else {
        di_jq("#" + selectedObj.id).addClass("active");
    }
}

function RotationActiveClass(selectedObj) {
    var rotationTypeArray = ["rotation1", "rotation2", "rotation3", "rotation4", "rotation5"];
    for (var i = 0; i < rotationTypeArray.length; i++) {
        if (selectedObj.id == rotationTypeArray[i]) {
            di_jq("#" + rotationTypeArray[i]).addClass("active");
        }
        else {
            if (di_jq("#" + rotationTypeArray[i]).hasClass("active")) {
                di_jq("#" + rotationTypeArray[i]).removeClass("active");
            }
        }
    }
}

function getOATtableContent() {
    var InputParam;
    var objOAT = OAT.getSelfInstance();
    InputParam = objOAT.toXML();
 
    return InputParam;
}

function getSourceList() {
    var liTags = z('ulSources').getElementsByTagName("li");
    var seperator = "{}";
    var sourceCol = "";
    for (var i = 0; i < liTags.length; i++) {
        var aTag = liTags[i].childNodes[0];
        sourceCol = sourceCol + seperator + aTag.innerHTML;
    }
 

    return sourceCol;
}


function ExportOATtoExcel() {
    var tableData;
    var xlsname = "table";
    titleText = z('dataTitle').innerHTML;
    titleText = titleText.trim();
    if (titleText != "") {
        xlsname = removeInvalidCharacters(titleText);
    }
    tableData = getOATtableContent();

    var form = document.createElement("form");
    form.setAttribute("method", "post");
    form.setAttribute("action", "ExportVisualization.aspx");

    var filenameField = document.createElement("input");
    filenameField.setAttribute("type", "hidden");
    filenameField.setAttribute("name", "filename");
    filenameField.setAttribute("value", xlsname);
    form.appendChild(filenameField);

    var filetypeField = document.createElement("input");
    filetypeField.setAttribute("type", "hidden");
    filetypeField.setAttribute("name", "type");
    filetypeField.setAttribute("value", "application/vnd.xls");
    form.appendChild(filetypeField);

    var svgField = document.createElement("input");
    svgField.setAttribute("type", "hidden");
    svgField.setAttribute("name", "svg");
    svgField.setAttribute("value", "");
    form.appendChild(svgField);

    var sourceField = document.createElement("input");
    sourceField.setAttribute("type", "hidden");
    sourceField.setAttribute("name", "source");
    sourceField.setAttribute("value", getSourceList());
    form.appendChild(sourceField);

    var tableField = document.createElement("input");
    tableField.setAttribute("type", "hidden");
    tableField.setAttribute("name", "tabledata");
    tableField.setAttribute("value", tableData);
    form.appendChild(tableField);

    var keywordsField = document.createElement("input");
    keywordsField.setAttribute("type", "hidden");
    keywordsField.setAttribute("name", "keywords");
    keywordsField.setAttribute("value", "Keyword 1{}Keyword 2");
    form.appendChild(keywordsField);

    var mapurlField = document.createElement("input");
    mapurlField.setAttribute("type", "hidden");
    mapurlField.setAttribute("name", "mapurl");
    mapurlField.setAttribute("value", "");
    form.appendChild(mapurlField);
    document.body.appendChild(form);
    form.submit();
}

/************** MAP RELTED functions ****************/
/* function for map (to be moved in dataview1.js) */
function mapDBNSCalls(checked, act) {
    if (checked) var bdrWait = 1; else var bdrWait = 0;
    switch (act) {
        case "dl":
            if (checked == true) di_jq('#mapNud').show(); else di_jq('#mapNud').hide();
            mapObj.setDataLabels(checked);
            break;
        case "bdr":
            mapObj.setBorder(bdrWait);
            break;
        case "ns":
            mapObj.setNorthSymbol(checked);
            break;
        case "sc":
            mapObj.setScale(checked);
            break;
    }
}
/* function to reset mode */
function resetModeType() {
    di_jq('#diMapParentMode span a').remove();
    di_jq('#mapNull').append('<a class="mapModeIcon selMode"></a>');
    //di_jq('#diMapImg').css('cursor', '-moz-zoom-in'); // for zoom in
    //mapObj.mode = 'zin'; // set zoom in default
}
/* function to create ui for series */
function createSeriesUi() {

    var data = mapObj.getSeriesInfo();

    if (data != '') {
        var srObj = di_jq(parseXml(data)).find('SeriesInfo');

        var srIdObj = z('di_mapSeries');
        di_jq('#di_mapSeries').children('option').remove();

        var i = 0;
        srObj.each(function () {
            var srID = di_jq(this).find('SeriesID').text();
            var srName = di_jq(this).find('SeriesName').text();

            srIdObj.options[i] = new Option(srName, srID);

            i++;
        });
    }
}
/* function to change map theme selection */
function changeMapThemeSelection(selval) {
    if (selval == 'addtheme') {
        di_jq('#di_mapTheme').val(themePreviousSelVal);
        openMapPropertyPopUp('theme_add');
    }
    else {
        themePreviousSelVal = selval;
        var thArrayData = createArrColThemeData(false, selval);
        changeThemeDetails(thArrayData, selval);
    }
}
/* function to create array of theme data */
function createArrColThemeData(create, themeId) {
    if (themeId == undefined) themeId = '';
    var data = mapObj.getThemeInfo(themeId);
    var isIe8 = 'no';

    if (di_jq.browser.msie) {
        if (jQuery.browser.version.substring(0, 2) == "8.") {
            isIe8 = 'yes';
        }
    }

    var thArrayData = [];
    var delemeter = '[****]';
    var i = 0;
    if (data != '') {
        var thObj = di_jq(parseXml(data)).find('Theme');

        if (create) { // if true recreate theme drop down menu
            var thIdObj = z('di_mapTheme');
            di_jq('#di_mapTheme').children('option').remove();

            if (mapviewtype != 'single') {
                // added for Add new theme
                var addthtext = di_jq('#lngMapAddTheme').html();
                z('di_mapTheme').options[0] = new Option(addthtext, 'addtheme');
                var addthemeopt = new Option('---------------------------', '');
                z('di_mapTheme').options[1] = addthemeopt;
                addthemeopt.setAttribute("disabled", "disabled");

                var i = 2;  //var i =2; for add new theme
            }
            //
        } //

        thObj.each(function () {
            var arrayData = [];

            var thID = di_jq(this).find('ID').text();
            var thName = di_jq(this).find('Name').text();
            var thType = di_jq(this).find('Type').text();
            var serIesID = di_jq(this).find('SelectedSeriesID').text();
            var selTimePeriod = di_jq(this).find('SelectedTimePeriod').text();
            var selIsMRD = di_jq(this).find('isMRD').text();
            var legends = di_jq(this).find('Legend');
            var breakType = di_jq(this).find('BreakType').text();
            var breakCount = di_jq(this).find('BreakCount').text();
            var thVisible = di_jq(this).find('Visible:first').text();
            var ChartType = di_jq(this).find('ChartType').text();

            var Decimals = di_jq(this).find('Decimals').text();
            var Minimum = di_jq(this).find('Minimum').text();
            var Maximum = di_jq(this).find('Maximum').text();

            // for dot density
            var dotStyle = di_jq(this).find('DotStyle').text();
            var dotSize = di_jq(this).find('DotSize').text();
            var dotColor = di_jq(this).find('DotColor').text();
            var dotValue = di_jq(this).find('DotValue').text();

            // for time periods
            var TimePeriods = di_jq(this).find('TimePeriods > string');
            var timePeriodsAr = [];
            for (var a = 0; a < TimePeriods.length; a++) {
                if (di_jq(TimePeriods[a]).context.childNodes[0] != undefined) {
                    if (isIe8 == 'yes') {
                        var tpy = di_jq(TimePeriods[a]).context.childNodes[0].text;
                    }
                    else {
                        var tpy = di_jq(TimePeriods[a]).context.childNodes[0].wholeText;
                    }
                    if (tpy == '' || tpy == null || tpy == undefined) {
                        tpy = di_jq(TimePeriods[a]).text();
                    }
                    timePeriodsAr.push(tpy);
                }
            }
            // for chart series id
            var ChartSeriesID = di_jq(this).find('SeriesIds > string');
            var chartSrIDAr = [];
            for (var a = 0; a < ChartSeriesID.length; a++) {
                if (di_jq(ChartSeriesID[a]).context.childNodes[0] != undefined) {
                    if (isIe8 == 'yes') {
                        var seriesId = di_jq(ChartSeriesID[a]).context.childNodes[0].text;
                    }
                    else {
                        var seriesId = di_jq(ChartSeriesID[a]).context.childNodes[0].wholeText;
                    }
                    chartSrIDAr.push(seriesId);
                }
            }
            // for chart series id
            var ChartSeriesNM = di_jq(this).find('SeriesNames > string');
            var chartSrNMAr = [];
            for (var a = 0; a < ChartSeriesNM.length; a++) {
                if (di_jq(ChartSeriesNM[a]).context.childNodes[0] != undefined) {
                    if (isIe8 == 'yes') {
                        var seriesName = di_jq(ChartSeriesNM[a]).context.childNodes[0].text;
                    }
                    else {
                        var seriesName = di_jq(ChartSeriesNM[a]).context.childNodes[0].wholeText;
                    }
                    chartSrNMAr.push(seriesName);
                }
            }
            // for chart series color
            var ChartSeriesColor = di_jq(this).find('SeriesFillStyle > string');
            var chartSrColorAr = [];
            var chartLegendAr = [];
            for (var a = 0; a < ChartSeriesColor.length; a++) {
                if (di_jq(ChartSeriesColor[a]).context.childNodes[0] != undefined) {
                    if (isIe8 == 'yes') {
                        var seriesColor = di_jq(ChartSeriesColor[a]).context.childNodes[0].text;
                    }
                    else {
                        var seriesColor = di_jq(ChartSeriesColor[a]).context.childNodes[0].wholeText;
                    }
                    chartSrColorAr.push(seriesColor);

                    var seriesInfo = chartSrIDAr[a] + mapDataDelemeter + chartSrNMAr[a] + mapDataDelemeter + seriesColor;
                    chartLegendAr.push(seriesInfo);
                }
            }
            var ChartWidth = di_jq(this).find('ChartWidth').text();
            var ChartHeight = di_jq(this).find('ChartSize').text();
            var ChartPieWidth = di_jq(this).find('PieAutoSizeFactor').text();
            var ChartDecimals = Decimals;
            var ChartShowData = di_jq(this).find('DisplayChartData').text();
            var ChartMRD = di_jq(this).find('DisplayChartMRD').text();
            var ChartLineTick = di_jq(this).find('ChartLineThickness').text();
            //var ChartTimeSeries = di_jq(this).find('ChartTimePeriods').text();

            var legendAr = [];
            for (var j = 0; j < legends.length; j++) {
                var LCaption = di_jq(legends[j]).find('Caption').text();
                if (LCaption == '' || LCaption == null || LCaption == undefined)
                    LCaption = di_jq(legends[j]).find('Caption').context.childNodes[0].wholeText;
                var LTitle = di_jq(legends[j]).find('Title').text();
                var LColor = di_jq(legends[j]).find('Color').text();
                var RangeFrom = di_jq(legends[j]).find('RangeFrom').text(); // RangeFrom
                var RangeTo = di_jq(legends[j]).find('RangeTo').text(); // RangeTo
                var ShapeCount = di_jq(legends[j]).find('ShapeCount').text(); // ShapeCount

                var LFillStyle = di_jq(legends[j]).find('FillStyle').text(); // case of Hatch patern

                // case of Symbol patern
                var MarkerType = di_jq(legends[j]).find('MarkerType').text();
                var MarkerSize = di_jq(legends[j]).find('MarkerSize').text();
                var MarkerFont = di_jq(legends[j]).find('MarkerFont').text();
                var MarkerChar = di_jq(legends[j]).find('MarkerChar').text();

                /*
                title - 0, color - 1, hatchPatern - 2, cpation - 3, rangeFrom - 4, RangeTo - 5, count - 6, MarkerType - 7, MarkerSize - 8, MarkerFont - 9, MarkerChar - 10
                */
                legendAr[j] = LTitle + delemeter + LColor + delemeter + LFillStyle + delemeter + LCaption + delemeter + RangeFrom + delemeter + RangeTo + delemeter + ShapeCount + delemeter + MarkerType + delemeter + MarkerSize + delemeter + MarkerFont + delemeter + MarkerChar;
            }
            arrayData.push(thType); // 0 type
            arrayData.push(serIesID); // 1 series index
            arrayData.push(legendAr); // 2 legend data
            arrayData.push(breakType); // 3 break type
            arrayData.push(breakCount); // 4 break count
            arrayData.push(thVisible); // 5 visiblity
            arrayData.push(thName); // 6 theme name
            arrayData.push(ChartType); // 7 chart type

            arrayData.push(dotStyle); // 8 dot density style Circle/Square/Triangle/Cross/Custom
            arrayData.push(dotSize); // 9 dot density size
            arrayData.push(dotColor); // 10 dot density color code
            arrayData.push(dotValue); // 11 dot density value

            arrayData.push(Decimals); // 12 legend Decimals
            arrayData.push(Minimum); // 13 legend minimum
            arrayData.push(Maximum); // 14 legend maximum

            // for chart
            arrayData.push(chartLegendAr); // 15 chart legend
            arrayData.push(ChartWidth); // 16 chart width
            arrayData.push(ChartHeight); // 17 chart height
            arrayData.push(ChartDecimals); // 18 chart Decimals
            arrayData.push(ChartShowData); // 19 chart show data value
            arrayData.push(ChartMRD); // 20 is MRD/TimeSeries
            //arrayData.push(ChartTimeSeries); // 21 chart time series list
            arrayData.push(selIsMRD); 		// 21 theme is MRD
            arrayData.push(selTimePeriod); 	// 22 theme selected time period
            arrayData.push(timePeriodsAr); 	// 23 theme time periods array
            arrayData.push(ChartPieWidth); 	// 24 chart pie size
            arrayData.push(ChartLineTick); 	// 25 chart line thickness

            if (thID != '') {
                if (create) { // if true recreate theme drop down menu
                    thIdObj.options[i] = new Option(thName, thID);
                } //

                thArrayData[thID] = arrayData;
                i++;
            }
        });
    }
    return thArrayData;
}
/* function to create ui for theme */
function createThemeUi() {

    var thArrayData = createArrColThemeData(true, ''); // create theme drop down (true) and getting data

    SHideThemeAction(); // show/hide action for single theme

    // set details of first theme
    var selThemeID = di_jq('#di_mapTheme option:last').val();
    di_jq('#di_mapTheme').val(selThemeID);
    themePreviousSelVal = selThemeID;
    changeThemeDetails(thArrayData, selThemeID);
}
/* function to show/hide facts for single theme */
function SHideThemeAction() {
    var cntopt = 1;
    var opCount = z('di_mapTheme').options.length;
    if (mapviewtype != 'single') cntopt = 3;

    if (opCount > cntopt) {
        //di_jq('#shMapTheme').show();
        //di_jq('#mapDelTheme').show();
        //di_jq('#mapShowTheme').show();

        //di_jq('#di_mapTheme').attr('disabled', false);
        di_jq('#mapDelTheme').attr('disabled', false);
        di_jq('#mapDelTheme').removeClass('dv_mapButton_disabled');
        di_jq('#mapDelTheme').addClass('dv_mapButton');
        di_jq('#mapChkShowTh').attr('disabled', false);
    }
    else {
        //di_jq('#shMapTheme').hide();
        //di_jq('#mapDelTheme').hide();
        //di_jq('#mapShowTheme').hide();

        //di_jq('#di_mapTheme').attr('disabled', true);
        di_jq('#mapDelTheme').attr('disabled', true);
        di_jq('#mapDelTheme').addClass('dv_mapButton_disabled');
        di_jq('#mapDelTheme').removeClass('dv_mapButton');
        di_jq('#mapChkShowTh').attr('disabled', true);
    }
}
/* function to set theme type and legend for selected theme */
function changeThemeDetails(thArrayData, index) {
    try {
        di_jq('#di_mapSeries').removeAttr('multiple');
        di_jq("#di_mapSeries").multiselect("destroy");

        var delemeter = '[****]';
        if (index == '') return;
        var type = thArrayData[index][0];
        var series = thArrayData[index][1];
        var legends = thArrayData[index][2];
        var brktype = thArrayData[index][3];
        var brkcount = thArrayData[index][4];
        var thvisible = thArrayData[index][5];
        //var chart = thArrayData[index][7];
        // set theme type
        di_jq('#mapThemeType').attr("class", type);
        di_jq('#mapThemeType').attr("title", type);
        di_jq('#mapThemeType').attr("src", "../../stock/themes/default/images/dataview/map/" + type + ".png");
        if (thvisible == "true") { di_jq('#mapChkShowTh').attr('checked', true); } else { di_jq('#mapChkShowTh').attr('checked', false); }

        // set series index
        if (series != '' && type != 'Chart') di_jq('#di_mapSeries').val(series);

        di_jq('#mapThTypeDotDensity').hide();
        di_jq('#mapThTypeColorHatch').hide();
        di_jq('#divIdColoHatchShow').hide();
        di_jq('#divIdChartShow').hide();

        if (type == 'DotDensity') { // for Dot Density type theme
            di_jq('#mapThTypeDotDensity').show();

            // for map dot density marker color
            di_jq('#mapDotDtyColor').ColorPicker({
                onSubmit: function (hsb, hex, rgb, el) {
                    di_jq(el).ColorPickerHide();
                    di_jq('#mapDotDtyColor').css('background-color', '#' + hex);
                    var opacityDec = di_jq('#mapDotDtyColor').attr('rel');
                    di_jq('#mapDotDtyColorCodeH').val('#' + decimalToHexString(opacityDec) + hex);
                    setMapDotDensityDetails();
                }
            });

            var dotStyle = thArrayData[index][8];
            var dotSize = thArrayData[index][9];
            var getAVal = Math.abs(getA(Number(thArrayData[index][10])));
            var dotOpacityHtml = getOpacityHtml(getAVal);

            if (decimalToHexString(Number(thArrayData[index][10])) != 0)
                var dotColor = '#' + decimalToHexString(Number(thArrayData[index][10]));
            else
                var dotColor = '#000000';
            if (getAVal == 1) getAVal = 255;
            var alphaColor = '#' + decimalToHexString(getAVal) + dotColor.replace("#", "");
            var dotValue = thArrayData[index][11];
            di_jq('#mapDotDtyStyle_0').attr('class', 'mapDotDtyStyle_' + dotStyle);
            di_jq('#mapDotDtyStyle_0').attr('rel', dotStyle);
            di_jq('#mapDotDtySize').val(dotSize);
            di_jq('#mapDotDtyColor').css('background-color', dotColor);
            di_jq('#mapDotDtyColor').css('opacity', dotOpacityHtml);
            di_jq('#mapDotDtyColor').attr('rel', getAVal);
            di_jq('#mapDotDtyColorCodeH').val(alphaColor);
            di_jq('#mapDotDtyValue').val(dotValue);

        }
        else if (type == 'Chart') { // for Chart type theme

            var chartLegend = thArrayData[index][15];
            var chartWidth = thArrayData[index][16];
            var chartHeight = thArrayData[index][17];
            var chartType = thArrayData[index][7];
            var pieWidth = thArrayData[index][24];
            if (chartType == 'Pie' || chartType == 'Line') chartWidth = Math.round(pieWidth * 2);

            di_jq('#mapThTypeColorHatch').show();
            di_jq('#divIdChartShow').show();
            di_jq('#map_hatchpat_list').hide();

            var seriesLength = di_jq('#di_mapSeries option').length;
            if (seriesLength > 1) {
                di_jq('#di_mapSeries').attr('multiple', 'multiple');
                di_jq('#di_mapSeries option').each(function () { di_jq(this).attr("selected", false); });
                var seriesArr = series.split(",");
                for (var i = 0; i < seriesArr.length; i++) {
                    di_jq('#di_mapSeries option[value="' + seriesArr[i] + '"]').attr("selected", "selected");
                }
                di_jq("#di_mapSeries").multiselect({
                    selectedText: "# of # selected",
                    height: 'auto',
                    minWidth: 200,
                    header: false
                });
            }
            else {
                di_jq("#di_mapSeries").multiselect("destroy");
            }

            var legendUi = '';
            di_jq('#mapThemeLegend').html('');
            // enable slider
            di_jq("#mapChartSliderW").slider({
                range: "max",
                min: 1,
                max: 20,
                step: 1,
                value: chartWidth,
                stop: function (event, ui) {
                    //di_jq("#mapChartSliderWVal").val(ui.value);
                    mapObj.setChartSettings(index, 'width', ui.value);
                }
            });
            di_jq("#mapChartSliderH").slider({
                range: "max",
                min: 1,
                max: 20,
                step: 1,
                value: chartHeight,
                stop: function (event, ui) {
                    //di_jq("#mapChartSliderHVal").val(ui.value);
                    mapObj.setChartSettings(index, 'height', ui.value);
                }
            });

            // start for chart legend
            if (chartLegend != '') {
                var themeID = di_jq('#di_mapTheme').val();
                var alphaVal = mapObj.getChartLegendAphaVal(themeID, 'ChartLegend');

                for (var i = 0; i < chartLegend.length; i++) {
                    var legendsAr = chartLegend[i].split(delemeter);
                    var SrId = legendsAr[0];
                    var SrName = legendsAr[1];
                    var SrColor = '#' + decimalToHexString(Number(legendsAr[2]));
                    //var	sybOpecty = 'opacity:' + getOpacityHtml( getA(Number(legendsAr[2])) ) +';';
                    var sybOpecty = 'opacity:' + getOpacityHtml(alphaVal) + ';';

                    var legendUi = '<p style="padding:1px;"><div id="mapchartdicolor_' + i + '" class="mapchartdicolor" style="background-color:' + SrColor + '; ' + sybOpecty + '"></div> &nbsp;<label class="normalText" title="' + SrName + '">';

                    if (SrName.length > 20)
                        legendUi += SrName.substr(0, 20) + '...';
                    else
                        legendUi += SrName;

                    legendUi += '</label></p>';

                    di_jq('#mapThemeLegend').append(legendUi);

                    initMapColorPic('mapchartdicolor_' + i, i, index, SrColor); // here index is themeId

                } // end for

                scroller = new jsScroller(document.getElementById("scrollSubContainer"), 400, 97);
                scrollbar = new jsScrollbar(document.getElementById("Scrollbar-Container"), scroller, true, scrollbarEvent);
            } // end legend

            if (chartType == 'Pie' || chartType == 'Line') {
                di_jq('#mapChartSliderH').parent().hide();
            }
            else {
                di_jq('#mapChartSliderH').parent().show();
            }

        }
        else { // for Color and Hatch types themes

            // set legend
            var legendUi = '';
            di_jq('#mapThemeLegend').html('');

            di_jq('#mapThTypeColorHatch').show();
            di_jq('#divIdColoHatchShow').show();

            if (type == 'Hatch' || type == 'Symbol') di_jq('#mapSeriesFormatTable').hide(); else di_jq('#mapSeriesFormatTable').show();

            // set break type
            if (brktype != '') di_jq('#di_mapBrkType').val(brktype);
            // set break count
            if (brkcount != '') di_jq('#di_mapBrkCount').val(brkcount);

            if (legends != '') {
                for (var i = 0; i < legends.length; i++) {
                    var legendsSp = legends[i].split(delemeter);
                    var colorCode = '#' + decimalToHexString(Number(legendsSp[1]));
                    var hatchPat = '';
                    var sybOpecty = '';
                    if (type == 'Hatch') {
                        //hatchPat = '<div id="map_hatchpat_'+i+'" style="cursor:pointer;float:left;width:15px;height:15px;background-color:red;margin-left:3px;" onclick=\'mapOpenHatchPaternList('+i+')\' class="" title="'+legendsSp[2]+'"></div>';

                        hatchPat = '<div id="map_hatchpat_' + i + '" style="cursor:pointer;float:left;margin:0;margin-left:3px;" class="hatch_patern_' + legendsSp[2] + '"   onclick=\'mapOpenHatchPaternList(' + i + ')\' title="' + legendsSp[2] + '"></div>';
                    }

                    if (type == 'Symbol') {
                        sybOpecty = 'opacity:' + getOpacityHtml(getA(Number(legendsSp[1]))) + ';';
                    }

                    legendUi = '<p style="padding:1px;"><div id="mapdicolor_' + i + '" class="mapchartdicolor" style="background-color:' + colorCode + ';' + sybOpecty + '"></div>' + hatchPat + ' &nbsp;<label class="normalText">' + legendsSp[0] + '</label></p>';

                    di_jq('#mapThemeLegend').append(legendUi);

                    initMapColorPic('mapdicolor_' + i, i, index, colorCode); // here index is themeId
                }
                scroller = new jsScroller(document.getElementById("scrollSubContainer"), 400, 97);
                scrollbar = new jsScrollbar(document.getElementById("Scrollbar-Container"), scroller, true, scrollbarEvent);
            } // end legend if
        } // end else for type

    }
    catch (err) { }
}
/* function to set Hatch patern type from legend */
function setMapHatchPatern(patType) {
    var patLocation = di_jq('#mapThemeType').attr('rel');
    patLocation = patLocation.split("_");
    if (patLocation[0] == 'quick') {
        var indexVal = di_jq('#map_hatchpat_list').attr('rel');
        var themeId = di_jq('#di_mapTheme').val();

        di_jq('#map_hatchpat_' + indexVal).attr('class', 'hatch_patern_' + patType);
        di_jq('#map_hatchpat_' + indexVal).attr('title', patType);

        di_jq('#map_hatchpat_list').removeAttr('rel');

        mapObj.setHatchPatern(themeId, indexVal, patType);
    }
    else if (patLocation[0] == 'edit') {
        di_jq('#mappop_hatchpat_' + patLocation[1]).attr('class', 'hatch_patern_' + patType);
        di_jq('#mappop_hatchpat_' + patLocation[1]).attr('title', patType);
    }
    else if (patLocation[0] == 'layer') {
        var layerId = patLocation[1].replace(/\#/g, '_');
        di_jq('#ly_style_' + layerId).attr('class', 'hatch_patern_' + patType);
        di_jq('#ly_style_' + layerId).attr('rel', patType);
        di_jq('#mappop_layer_stylelist_' + layerId).slideToggle();
    }
}
/* function to set dot density details */
function setMapDotDensityDetails() {
    var delemeter = '[****]';
    var themeId = di_jq('#di_mapTheme').val();
    var color = di_jq('#mapDotDtyColorCodeH').val();
    var style = di_jq('#mapDotDtyStyle_0').attr('rel');
    var size = di_jq('#mapDotDtySize').val();
    var dotVal = di_jq('#mapDotDtyValue').val();

    var inputString = style + delemeter + size + delemeter + color + delemeter + dotVal;

    mapObj.setDotDensityDetails(themeId, inputString);
}
/* function to change dot density style */
function mapChangeDotDtyStyle(typeid) {
    di_jq('#mapDotDtyStyle_0').attr('class', 'mapDotDtyStyle_' + typeid);
    di_jq('#mapDotDtyStyle_0').attr('rel', typeid);

    setMapDotDensityDetails();

    di_jq('#mapDotDtyStyleList').slideToggle();
}
/* function to change dot density style */
function mapPopChangeDotDtyStyle(typeid) {
    di_jq('#mapPopDotDtyStyle_0').attr('class', 'mapDotDtyStyle_' + typeid);
    di_jq('#mapPopDotDtyStyle_0').attr('rel', typeid);
    di_jq('#mapPopDotDtyStyleList').slideToggle();
}
/* function to open map hatch patern list */
function mapOpenHatchPaternList(index) {
    di_jq('#mapThemeType').attr('rel', 'quick_');
    di_jq('#map_hatchpat_list').css('margin-top', eval(index * 17) + 3 + 'px');
    if (di_jq('#map_hatchpat_' + index).attr('rel') == '1') {
        di_jq('#map_hatchpat_list').hide('slow');
        di_jq('#map_hatchpat_' + index).attr('rel', 0);
        di_jq('#map_hatchpat_list').removeAttr('rel');
    }
    else {
        di_jq('#map_hatchpat_list').show('slow');

        di_jq('#scrollContainer div[id*="map_hatchpat_"]').each(function () {
            if (this.id != 'map_hatchpat_list') {
                di_jq(this).attr('rel', 0);
            }
        });
        di_jq('#map_hatchpat_' + index).attr('rel', 1);

        di_jq('#map_hatchpat_list').attr('rel', index);
    }
}
/* function to open map hatch patern list */
function mapPOPOpenHatchPaternList(index) {
    di_jq('#mapThemeType').attr('rel', 'edit_' + index);
    di_jq('#mapPopIdCrHatLdTbl div[id*="mappop_hatchpat_list_"]').each(function () {
        if (this.id != 'mappop_hatchpat_list_' + index)
            di_jq(this).hide('slow');
    });
    var paternUI = di_jq('#map_hatchpat_list').html();
    di_jq('#mappop_hatchpat_list_' + index).html(paternUI);
    di_jq('#mappop_hatchpat_list_' + index).slideToggle();
}
function getASCIICharsHtml(fontType) {
    var html = '<table id="mapMarkerCharsTbl" cellpadding="0" cellspacing="0" width="100%" style="border-collapse:collapse;"><tr>';
    var numCell = 16;

    var countCel = 1;
    for (var i = 32; i < 256; i++) {

        html += '<td style="border:solid 1px #aca899;"><div id="MSF_' + i + '" class="webding_container ' + fontType + '_' + i + '" onclick="clickMartkerFontChar(' + i + ')" style="cursor:pointer;"><img height="20" border="0" width="30" src="../../stock/themes/default/images/dataview/spacer.gif"></div></td>';

        if (countCel == numCell) {
            countCel = 0;
            html += '</tr><tr>';
        }

        countCel++;
    }

    html += '</tr></table>';
    return html;
}
/* function to click on marker cell */
function clickMartkerFontChar(charNo) {
    di_jq('#mapMarkerCharsTbl div').removeClass('selectedSymbol');
    di_jq('#MSF_' + charNo).addClass('selectedSymbol');
    di_jq('#mapMarkerCharStyle').attr('rel', charNo);
}
/* function to change marker fomr style */
function changeMartkerFontStyle(val) {
    var markerChar = di_jq('#mapMarkerCharStyle').attr('rel');
    di_jq('#mapMarkerCharStyle').html(getASCIICharsHtml(val));
    di_jq('#MSF_' + markerChar).addClass('selectedSymbol');
}
/* function to open sysmbol patern pop-up */
function mapPOPOpenSymbolPaternList(index) {
    di_jq('#mapThemeType').attr('rel', 'edit_' + index);

    ApplyMaskingDiv(1300);
    //Set close button at right corner of popup div
    SetCloseButtonInPopupDiv(di_jq('#dv_container_popup_symbol_pat'), 'mapPOPCloseSymbolPaternList');
    di_jq('#dv_container_popup_symbol_pat').show('slow');
    GetWindowCentered(z('dv_container_popup_symbol_pat'), 540, 350);

    // find details from rel tag
    var details = di_jq('#mappop_symbolpat_' + index).attr('rel');
    var detailsAr = details.split('~~');
    var MColor = '#' + detailsAr[0].substr(7, 6);
    var MChar = detailsAr[1];
    var MSize = detailsAr[2];
    var MFont = detailsAr[3];
    var MOpacity = detailsAr[4];

    // put marker symbol style
    di_jq('#mapMarkerCharStyle').html(getASCIICharsHtml(MFont));
    di_jq('#mapMarkerCharStyle').attr('rel', MChar);
    var dotOpacityHtml = getOpacityHtml(MOpacity);

    di_jq('#mapPopMarkerSize').children('option').remove();
    di_jq('#mapPopMarkerSize').append(getDropDownOptions(1, 200));

    // enable slider
    di_jq('#mapPopMarkerOpacityTxt').val(Math.abs(MOpacity));
    di_jq("#mapPopMarkerOpacity").slider({
        range: "max",
        min: 0,
        max: 255,
        step: 1,
        value: Math.abs(MOpacity),
        slide: function (event, ui) {
            di_jq("#mapPopMarkerOpacityTxt").val(ui.value);
            var BgColor = di_jq('#mapPopMarkerColor').css('background-color');
            di_jq('#mapPopMarkerColor').css('opacity', getOpacityHtml(ui.value));
        }
    });

    // ser default marker color
    di_jq('#mapPopMarkerColor').css('background-color', MColor);
    di_jq('#mapPopMarkerColor').css('opacity', dotOpacityHtml);
    di_jq('#mapPopMarkerFont').val(MFont);
    di_jq('#mapPopMarkerSize').val(MSize);
    di_jq('#popupMapSymbolTab').val(index);

    di_jq('#MSF_' + MChar).addClass('selectedSymbol');
}
/* function to close sysmbol patern pop-up */
function mapPOPCloseSymbolPaternList() {
    ApplyMaskingDiv(1100);
    di_jq('#dv_container_popup_symbol_pat').hide('slow');
}
/* function to click on ok button on symbol patern pop-up */
function applyMapSymbolPat() {
    var markerFont = di_jq('#mapPopMarkerFont').val();
    var markerSize = di_jq('#mapPopMarkerSize').val();
    var markerOpacity = di_jq('#mapPopMarkerOpacityTxt').val();
    var markerColor = di_jq('#mapPopMarkerColor').css('background-color');
    var markerColorA = '#' + decimalToHexString(markerOpacity) + hexc(markerColor).replace("#", "");
    var markerChar = di_jq('#mapMarkerCharStyle').attr('rel');

    var legendIndex = di_jq('#popupMapSymbolTab').val();

    var markerDetails = markerColorA + '~~' + markerChar + '~~' + markerSize + '~~' + markerFont + '~~' + markerOpacity;

    // update symbol rel tag
    di_jq('#mappop_symbolpat_' + legendIndex).attr('rel', markerDetails);
    // update selected symbol
    di_jq('#mappop_symbolpat_' + legendIndex).attr('class', 'webding_container ' + markerFont + '_' + markerChar);

    mapPOPCloseSymbolPaternList(); // close the pop-up
}
/* function to get A value from decimal number */
function getA(num) {
    // eg value for num is 84545883
    return (parseInt(num, 10) & 0xFF000000) >> 24;  // does this correctly get the R value from a ARGB value?
}
/* function to get opacity value for css */
function getOpacityHtml(num) {
    var posNum = Math.abs(num);
    if (posNum != 1) return posNum / 256;
    else return 1;
}
/* function to get numeric value from 0-100 for drop down*/
function getOpacityS(num) {
    num = Math.abs(num);
    if (num != 1)
        var posNum = Math.round((num * 100) / 256);
    else
        var posNum = 100;

    if (posNum > 100) posNum = 100;
    return posNum;
}
/* function to number to hexa*/
function decimalToHexString(num) {
    //if (num < 0)
    //{
    //num = 0xFFFFFF + num + 1;
    //}
    //return num.toString(16).toUpperCase();

    if (num == null || num == "undefined") { return "#FFFFFF"; }

    var intNum = (parseInt(num, 10)) & 0x00FFFFFF;
    var strNum = intNum.toString(16);
    while (strNum.length < 6) { strNum = "0" + strNum; }

    return strNum;  //+intNum.toString(16);
}
/* init color picker for map section */
function initMapColorPic(id, index, themeId, selColor) {

    if (selColor == '' || selColor == undefined) selColor = '#FF0000';

    var themeType = di_jq('#mapThemeType').attr('title');

    // for chart background color picker
    di_jq('#' + id).ColorPicker({
        color: selColor,
        onSubmit: function (hsb, hex, rgb, el) {
            //di_jq(el).val(hex);
            di_jq(el).ColorPickerHide();
            di_jq('#' + id).css('background-color', '#' + hex);
            var hexaCCode = '#' + hex;
            if (themeType == 'Symbol' || themeType == 'Chart') {
                var idOpacity = di_jq('#' + id).css('opacity');
                if (idOpacity != 1)
                    idOpacity = Math.round(idOpacity * 256);
                else
                    idOpacity = 255;
                hexaCCode = '#' + decimalToHexString(idOpacity) + hex;
            }
            // callback for change legend color
            if (themeId != '') {
                mapObj.setLegendColor(themeId, index, hexaCCode);
            }
        }
    });
}
/* init color picker for map section */
function initMapChartColorPic(id, index, selColor) {

    // for chart background color picker
    di_jq('#' + id).ColorPicker({
        color: selColor,
        onSubmit: function (hsb, hex, rgb, el) {
            //di_jq(el).val(hex);
            di_jq(el).ColorPickerHide();
            di_jq('#' + id).css('background-color', '#' + hex);
            di_jq('#' + id).attr('val', hex);
            var opacity = di_jq("#mapPopChartOpacityTxt").val();
            var hexaCCode = '#' + decimalToHexString(opacity) + hex;
            di_jq('#' + id).attr('rel', hexaCCode);
        }
    });
}
/* function to change series/break type/break count */
function changeSeriesBreaks() {
    var themeID = di_jq('#di_mapTheme').val();
    var seriesID = di_jq('#di_mapSeries').val();
    var breakType = di_jq('#di_mapBrkType').val();
    var breakCount = di_jq('#di_mapBrkCount').val();
    var themeType = di_jq('#mapThemeType').attr('class');
    if (themeType == 'Chart') {
        var array_of_checked_values = di_jq("#di_mapSeries").multiselect("getChecked").map(function () {
            return this.value;
        }).get();
        if (array_of_checked_values == '' || array_of_checked_values == null) {
            alert('Please select series');
            return false;
        }
        else {
            seriesID = array_of_checked_values.join(",");
        }
    }

    // get refresh map after changing series or break type/break count
    mapObj.setSeriesNBreaks(themeID, seriesID, breakType, breakCount);

    // check if label on then should be init areainfo array in map component (added 15 May 2012)
    if (di_jq('#map_dtlabels').is(':checked')) {
        mapObj.refreshAreaInfoArray();
    }

    // redraw theme details
    redrawThemeDetails(['detail', 'title', 'thname']); // thname for update theme name also

    //mapChangeSeriesColor('mapSrC_1');
    //di_jq('#mapSrC_0').removeAttr('class');
    //di_jq('#mapSrC_0').addClass('map_color_palette_1');

    if (mapviewtype == 'single') {
        di_jq('#mapSeriesDropDwnbox').val(seriesID);
    }
}
/* funtion to send request for filterring */
function requestToMapFilter(inids, anids) {
    if (chartVisualizationDecided == 'map') {
        loadingVisualization(true);
        setTimeout(function () {
            // send a request to server side to reset map image and theme setting
            mapObj.setMapFilterRequest(inids, anids, 'yes'); // yes to render map image
            // redraw theme details
            redrawThemeDetails(['theme', 'series', 'detail', 'title', 'thname', 'sthseries']); // thname for update theme name also

            loadingVisualization(false);
        }, 500);
    }
    else {
        // just sending request to update inids and anid no response.
        mapObj.setMapFilterRequest(inids, anids, 'no'); // no to render map image
    }
}
/* function to resize map */
function resizeMap(act) {
    try {
        var panelWidth = di_jq('#mapImage').width();
        var panelHeight = di_jq('#mapImage').height();
        var leftPanWidth = di_jq('#example5').width();
        if (act) {
            panelWidth = eval(panelWidth + leftPanWidth);
        }
        if (mapObj)
            mapObj.setMapSize(panelWidth, panelHeight);
    }
    catch (err) { }
}
/* common function to send callback for changing map title */
function setMapTitleDetails() {
    var delemeter = '[****]';
    var selTType = di_jq('#di_mapSelTitle').val();
    var selText = di_jq('#di_mapTxtTitle').val();
    var selFontSize = di_jq('#di_mapSelFont').val();
    var selFontColor = di_jq('#h_dicolormap1').val();
    var selBold = di_jq('#h_fontbold').val();
    var selItalic = di_jq('#h_fontitalic').val();
    var selUnderline = di_jq('#h_fontunderline').val();

    var isCheckedDtLbl = di_jq('#map_dtlabels').is(':checked');

    switch (selTType) {
        case "1": // title case
            di_jq('#mapTitle').html(selText);
            if (selFontSize != '') di_jq('#mapTitle').css('font-size', selFontSize + 'px');
            if (selFontColor != '') di_jq('#mapTitle').css('color', selFontColor);
            if (selBold == "true") di_jq('#mapTitle').css('font-weight', 'bold'); else di_jq('#mapTitle').css('font-weight', 'normal');
            if (selItalic == "true") di_jq('#mapTitle').css('font-style', 'italic'); else di_jq('#mapTitle').css('font-style', 'normal');
            if (selUnderline == "true") di_jq('#mapTitle').css('text-decoration', 'underline'); else di_jq('#mapTitle').css('text-decoration', 'none');

            var titleString = selText + delemeter + selFontSize + delemeter + selBold + delemeter + selItalic + delemeter + selUnderline + delemeter + selFontColor;
            // callback
            mapObj.setMapTitleInfo(titleString);

            break;
        case "2": // data labels case
            var dataString = delemeter + selFontSize + delemeter + selBold + delemeter + selItalic + delemeter + selUnderline + delemeter + selFontColor + delemeter;

            // callback
            if (di_jq('#map_dtlabels').is(":checked"))
                mapObj.setMapDataLblInfo(dataString, true, '1');
            else
                mapObj.setMapDataLblInfo(dataString, false, '1');

            break;
        case "6": // data labels case
            var dataString = delemeter + selFontSize + delemeter + selBold + delemeter + selItalic + delemeter + selUnderline + delemeter + selFontColor + delemeter;

            // callback
            if (di_jq('#map_dtlabels').is(":checked"))
                mapObj.setMapDataLblInfo(dataString, true, '2');
            else
                mapObj.setMapDataLblInfo(dataString, false, '2');

            break;
        case "3": // disclaimer case
            var disString = selText + delemeter + selFontSize + delemeter + selBold + delemeter + selItalic + delemeter + selUnderline + delemeter + selFontColor;

            // callback
            mapObj.setMapDisclaimerInfo(disString);

            break;
        case "4": // legend title case
            var disString = selText + delemeter + selFontSize + delemeter + selBold + delemeter + selItalic + delemeter + selUnderline + delemeter + selFontColor;

            // callback
            mapObj.setImageLegendTitle('title', disString);

            break;
        case "5": // legend body case
            var disString = selText + delemeter + selFontSize + delemeter + selBold + delemeter + selItalic + delemeter + selUnderline + delemeter + selFontColor;

            // callback
            mapObj.setImageLegendTitle('body', disString);

            break;
    }
}
/* common function to send callback for changing map title */
function getMapTitleDetails(val) {
    try {
        var delemeter = '[****]';
        // title[****]fontsize[****]bold[****]italic[****]underline[****]colorcode
        if (val == 1) { // title case
            di_jq('#di_mapTxtTitle').attr('disabled', false);
            var titleInfo = mapObj.getMapTitleInfo();
            titleInfo = titleInfo.split(delemeter);

            di_jq('#di_mapTxtTitle').val(titleInfo[0]); // title
            di_jq('#di_mapSelFont').val(titleInfo[1]); // font size
            di_jq('#h_fontbold').val(titleInfo[2]); // bold
            di_jq('#h_fontitalic').val(titleInfo[3]); // italic
            di_jq('#h_fontunderline').val(titleInfo[4]); // underline
            di_jq('#h_dicolormap1').val(titleInfo[5]); // color code

            // setting for map title div
            di_jq('#mapTitle').html(titleInfo[0]); // title
            if (titleInfo[1] != '') di_jq('#mapTitle').css('font-size', titleInfo[1] + 'px');
            if (titleInfo[5] != '') di_jq('#mapTitle').css('color', titleInfo[5]);
            if (titleInfo[2] == "true") di_jq('#mapTitle').css('font-weight', 'bold'); else di_jq('#mapTitle').css('font-weight', 'normal');
            if (titleInfo[3] == "true") di_jq('#mapTitle').css('font-style', 'italic'); else di_jq('#mapTitle').css('font-style', 'normal');
            if (titleInfo[4] == "true") di_jq('#mapTitle').css('text-decoration', 'underline'); else di_jq('#mapTitle').css('text-decoration', 'none');

            // selected class
            if (titleInfo[2] == "true") di_jq('#mapTitle_Bold').addClass('active'); else di_jq('#mapTitle_Bold').removeClass('active');
            if (titleInfo[3] == "true") di_jq('#mapTitle_Italic').addClass('active'); else di_jq('#mapTitle_Italic').removeClass('active');
            if (titleInfo[4] == "true") di_jq('#mapTitle_Underline').addClass('active'); else di_jq('#mapTitle_Underline').removeClass('active');
            if (titleInfo[5] != '') di_jq('#inp_dicolormap1').css('background-color', titleInfo[5]); else di_jq('#inp_dicolormap1').css('background-color', '#000000');

        }
        else if (val == 2 || val == 6) { // data labels row 1&2 case
            var labelInfo = '1';
            if (val == 6) labelInfo = '2';
            di_jq('#di_mapTxtTitle').attr('disabled', true);
            var dataInfo = mapObj.getMapDataLblInfo(labelInfo);
            dataInfo = dataInfo.split(delemeter);

            di_jq('#di_mapTxtTitle').val(dataInfo[0]); // title
            di_jq('#di_mapSelFont').val(dataInfo[1]); // font size
            di_jq('#h_fontbold').val(dataInfo[2]); // bold
            di_jq('#h_fontitalic').val(dataInfo[3]); // italic
            di_jq('#h_fontunderline').val(dataInfo[4]); // underline
            di_jq('#h_dicolormap1').val(dataInfo[5]); // color code

            // selected class
            if (dataInfo[2] == "true") di_jq('#mapTitle_Bold').addClass('active'); else di_jq('#mapTitle_Bold').removeClass('active');
            if (dataInfo[3] == "true") di_jq('#mapTitle_Italic').addClass('active'); else di_jq('#mapTitle_Italic').removeClass('active');
            if (dataInfo[4] == "true") di_jq('#mapTitle_Underline').addClass('active'); else di_jq('#mapTitle_Underline').removeClass('active');
            if (dataInfo[5] != '') di_jq('#inp_dicolormap1').css('background-color', dataInfo[5]); else di_jq('#inp_dicolormap1').css('background-color', '#000000');
        }
        else if (val == 3) { // disclaimer case
            di_jq('#di_mapTxtTitle').attr('disabled', false);
            var disInfo = mapObj.getMapDisclaimerInfo();
            disInfo = disInfo.split(delemeter);

            di_jq('#di_mapTxtTitle').val(disInfo[0]); // title
            di_jq('#di_mapSelFont').val(disInfo[1]); // font size
            di_jq('#h_fontbold').val(disInfo[2]); // bold
            di_jq('#h_fontitalic').val(disInfo[3]); // italic
            di_jq('#h_fontunderline').val(disInfo[4]); // underline
            di_jq('#h_dicolormap1').val(disInfo[5]); // color code

            // selected class
            if (disInfo[2] == "true") di_jq('#mapTitle_Bold').addClass('active'); else di_jq('#mapTitle_Bold').removeClass('active');
            if (disInfo[3] == "true") di_jq('#mapTitle_Italic').addClass('active'); else di_jq('#mapTitle_Italic').removeClass('active');
            if (disInfo[4] == "true") di_jq('#mapTitle_Underline').addClass('active'); else di_jq('#mapTitle_Underline').removeClass('active');
            if (disInfo[5] != '') di_jq('#inp_dicolormap1').css('background-color', disInfo[5]); else di_jq('#inp_dicolormap1').css('background-color', '#000000');
        }
        else if (val == 4) { // legend title case
            di_jq('#di_mapTxtTitle').attr('disabled', true);
            var dataInfo = mapObj.getImageLegendTitle('title');
            dataInfo = dataInfo.split(delemeter);

            di_jq('#di_mapTxtTitle').val(dataInfo[0]); // title
            di_jq('#di_mapSelFont').val(dataInfo[1]); // font size
            di_jq('#h_fontbold').val(dataInfo[2]); // bold
            di_jq('#h_fontitalic').val(dataInfo[3]); // italic
            di_jq('#h_fontunderline').val(dataInfo[4]); // underline
            di_jq('#h_dicolormap1').val(dataInfo[5]); // color code

            // selected class
            if (dataInfo[2] == "true") di_jq('#mapTitle_Bold').addClass('active'); else di_jq('#mapTitle_Bold').removeClass('active');
            if (dataInfo[3] == "true") di_jq('#mapTitle_Italic').addClass('active'); else di_jq('#mapTitle_Italic').removeClass('active');
            if (dataInfo[4] == "true") di_jq('#mapTitle_Underline').addClass('active'); else di_jq('#mapTitle_Underline').removeClass('active');
            if (dataInfo[5] != '') di_jq('#inp_dicolormap1').css('background-color', dataInfo[5]); else di_jq('#inp_dicolormap1').css('background-color', '#000000');
        }
        else if (val == 5) { // legend body case
            di_jq('#di_mapTxtTitle').attr('disabled', true);
            var dataInfo = mapObj.getImageLegendTitle('body');
            dataInfo = dataInfo.split(delemeter);

            di_jq('#di_mapTxtTitle').val(dataInfo[0]); // title
            di_jq('#di_mapSelFont').val(dataInfo[1]); // font size
            di_jq('#h_fontbold').val(dataInfo[2]); // bold
            di_jq('#h_fontitalic').val(dataInfo[3]); // italic
            di_jq('#h_fontunderline').val(dataInfo[4]); // underline
            di_jq('#h_dicolormap1').val(dataInfo[5]); // color code

            // selected class
            if (dataInfo[2] == "true") di_jq('#mapTitle_Bold').addClass('active'); else di_jq('#mapTitle_Bold').removeClass('active');
            if (dataInfo[3] == "true") di_jq('#mapTitle_Italic').addClass('active'); else di_jq('#mapTitle_Italic').removeClass('active');
            if (dataInfo[4] == "true") di_jq('#mapTitle_Underline').addClass('active'); else di_jq('#mapTitle_Underline').removeClass('active');
            if (dataInfo[5] != '') di_jq('#inp_dicolormap1').css('background-color', dataInfo[5]); else di_jq('#inp_dicolormap1').css('background-color', '#000000');
        }

    }
    catch (err) { }

}

/* function to click on font size and type for map*/
function checkMapFontST(action) {
    try {
        switch (action) {
            case "BF": // bold font
                var currentSize = di_jq('#di_mapSelFont').val();
                if (currentSize > 5 && currentSize < 31)
                    di_jq('#di_mapSelFont').val(eval(currentSize) + 1);
                break;
            case "SF": // small font
                var currentSize = di_jq('#di_mapSelFont').val();
                if (currentSize > 6 && currentSize < 32)
                    di_jq('#di_mapSelFont').val(eval(currentSize) - 1);
                break;
            case "B": // bold
                var currentStatus = di_jq('#h_fontbold').val();
                if (currentStatus == "true") {
                    di_jq('#h_fontbold').val("false");
                    di_jq('#mapTitle_Bold').removeClass('active');
                }
                else {
                    di_jq('#h_fontbold').val("true");
                    di_jq('#mapTitle_Bold').addClass('active');
                }
                break;
            case "I": // italic
                var currentStatus = di_jq('#h_fontitalic').val();
                if (currentStatus == "true") {
                    di_jq('#h_fontitalic').val("false");
                    di_jq('#mapTitle_Italic').removeClass('active');
                }
                else {
                    di_jq('#h_fontitalic').val("true");
                    di_jq('#mapTitle_Italic').addClass('active');
                }
                break;
            case "U": // underline
                var currentStatus = di_jq('#h_fontunderline').val();
                if (currentStatus == "true") {
                    di_jq('#h_fontunderline').val("false");
                    di_jq('#mapTitle_Underline').removeClass('active');
                }
                else {
                    di_jq('#h_fontunderline').val("true");
                    di_jq('#mapTitle_Underline').addClass('active');
                }
                break;
        }

        // set details
        setMapTitleDetails();
    }
    catch (err) { }
}

/* function to click on font size and type for map on pop-up*/
function checkMapFontSTPopUp(action, type) {
    if (type == undefined) type = '';
    try {
        switch (action) {
            case "BF": // bold font
                var currentSize = di_jq('#di_mapPopSelFont' + type).val();
                if (currentSize > 5 && currentSize < 31)
                    di_jq('#di_mapPopSelFont' + type).val(eval(currentSize) + 1);
                break;
            case "SF": // small font
                var currentSize = di_jq('#di_mapPopSelFont' + type).val();
                if (currentSize > 6 && currentSize < 32)
                    di_jq('#di_mapPopSelFont' + type).val(eval(currentSize) - 1);
                break;
            case "B": // bold
                var currentStatus = di_jq('#poph_fontbold' + type).val();
                if (currentStatus == "true") {
                    di_jq('#poph_fontbold' + type).val("false");
                    di_jq('#mapPopTitle_Bold' + type).removeClass('active');
                }
                else {
                    di_jq('#poph_fontbold' + type).val("true");
                    di_jq('#mapPopTitle_Bold' + type).addClass('active');
                }
                break;
            case "I": // italic
                var currentStatus = di_jq('#poph_fontitalic' + type).val();
                if (currentStatus == "true") {
                    di_jq('#poph_fontitalic' + type).val("false");
                    di_jq('#mapPopTitle_Italic' + type).removeClass('active');
                }
                else {
                    di_jq('#poph_fontitalic' + type).val("true");
                    di_jq('#mapPopTitle_Italic' + type).addClass('active');
                }
                break;
            case "U": // underline
                var currentStatus = di_jq('#poph_fontunderline' + type).val();
                if (currentStatus == "true") {
                    di_jq('#poph_fontunderline' + type).val("false");
                    di_jq('#mapPopTitle_Underline' + type).removeClass('active');
                }
                else {
                    di_jq('#poph_fontunderline' + type).val("true");
                    di_jq('#mapPopTitle_Underline' + type).addClass('active');
                }
                break;
        }
    }
    catch (err) { }
}

/* function to change seriec color */
function mapChangeSeriesColor(id) {
    var colorArray = ["#b6effd,#1c84a0", "#afc6ff,#1153fc", "#e4cefe,#602d9d", "#00ffff,#03b6b6", "#e0ffe0,#a3cca3", "#cdf281,#6b9417", "#6afd63,#0b8d04", "#66ffc2,#01b56d", "#fed3d1,#c0504d", "#fe9494,#d10202", "#cc8080,#990000", "#b97953,#682801", "#fdf399,#aa9909", "#fbdc8c,#eda900", "#f9c98b,#f28300", "#fec493,#aa5209", "#dad9d9,#626262", "#7991b7,#264e89", "#c36297,#8e014f", "#d66384,#be0137", "#d1b2d1,#944d94", "#ffccff,#ff33ff", "#fc51ee,#8d0482", "#944d94,#5c005c"];

    var newType = di_jq('#' + id).attr('rel');
    var newClass = di_jq('#' + id).attr('class');
    var colorCode = colorArray[newType - 1].split(",");
    var themeId = di_jq('#di_mapTheme').val();
    mapObj.setSeriesColor(themeId, colorCode[0], colorCode[1]);

    di_jq('#mapSeriesFormatDiv').hide();

    di_jq('#mapSrC_0').attr('rel', newType);
    di_jq('#mapSrC_0').attr('class', newClass);

    // redraw theme details
    redrawThemeDetails(['detail']);
}

/* function to get setting and apply */
function applyMapPopUpSetting(action) {
    var delemeter = '[****]';
    switch (action) {
        case "border":

            var borderSetting = mapObj.getBorderStyle();
            if (borderSetting != "false") {
                borderSetting = borderSetting.split(delemeter);
                // show border
                if (borderSetting[0] == "true") di_jq('#popBorderChkbox_m').attr('checked', true); else di_jq('#popBorderChkbox_m').attr('checked', false);
                // width weight
                if (borderSetting[1] != '') di_jq('#popBorderWidthTxt_m').val(borderSetting[1]); else di_jq('#popBorderWidthTxt_m').val(0);


                // border color
                if (borderSetting[2] != '') { di_jq('#popchartBoderColor_m').css('background-color', borderSetting[2]); di_jq('#popchartBoderColor_m').css('color', borderSetting[2]); di_jq('#popchartBoderColor_m').val(borderSetting[2]); }
               

                // border style
                if (borderSetting[3] != '') { mapChangeBdrStyle('mapBdrStl_' + borderSetting[3]); }
            }

            di_jq('#popupMapTab').val('border');

            break;
        case "label":
            //var jsoNString = '{"showLabel":"false","swde":"false","multiRow":"false","area":[1],"label1":["Arial","12","false","false","false","#000000","no"],"label2":["Arial","10","false","false","false","#000000","no"],"showEffect":"false","effectType":"Block","effectDepth":"1","effectColor":"#808080","showLeader":"true","straightLine":"yes","lineStyle":"Solid","lineWidth":"1","lineColor":"#808080"}';
            var labelSetting = mapObj.getLabelStyle();  // return json string
            if (labelSetting != "false") {
                //labelSetting = labelSetting.split(delemeter);
                var dataObj = di_jq.parseJSON(labelSetting);
                // show labels
                if (dataObj.showLabel == "true") di_jq('#popLabelChkbox_m').attr('checked', true); else di_jq('#popLabelChkbox_m').attr('checked', false);
                // area
                var areaLabels = dataObj.area;
                if (jQuery.inArray(0, areaLabels) > -1) di_jq('#popLabelAID_m').attr('checked', true); else di_jq('#popLabelAID_m').attr('checked', false);
                if (jQuery.inArray(1, areaLabels) > -1) di_jq('#popLabelANM_m').attr('checked', true); else di_jq('#popLabelANM_m').attr('checked', false);
                if (jQuery.inArray(2, areaLabels) > -1) di_jq('#popLabelDTV_m').attr('checked', true); else di_jq('#popLabelDTV_m').attr('checked', false);
                if (jQuery.inArray(3, areaLabels) > -1) di_jq('#popLabelCNSY_m').attr('checked', true); else di_jq('#popLabelCNSY_m').attr('checked', false);
                // multi row
                if (dataObj.multiRow == "true") {
                    di_jq('#popLabelMROW_m').attr('checked', true);
                    di_jq('#mapPopLblRowSeptor').show();
                    di_jq('#mapPopLblRow2').show();
                }
                else {
                    di_jq('#popLabelMROW_m').attr('checked', false);
                    di_jq('#mapPopLblRowSeptor').hide();
                    di_jq('#mapPopLblRow2').hide();
                    mapLblRowClick(1);
                }
                // indendent
                if (dataObj.indendent == "true") di_jq('#popLabelINDT_m').attr('checked', true); else di_jq('#popLabelINDT_m').attr('checked', false);
                // show where data exist
                if (dataObj.swde == "true") di_jq('#popLabelSWDE_m').attr('checked', true); else di_jq('#popLabelSWDE_m').attr('checked', false);
                // effects
                if (dataObj.showEffect == "true") di_jq('#popEffectsChkbox_m').attr('checked', true); else di_jq('#popEffectsChkbox_m').attr('checked', false);
                // effect type
                if (dataObj.effectType != "") di_jq('#popEffectType_m').val(dataObj.effectType);
                // depth
                if (dataObj.effectDepth != "") di_jq('#popEffectDepth_m').val(dataObj.effectDepth);
                // color
                if (dataObj.effectColor != "") { di_jq('#popEffectColor_m').css('background-color', dataObj.effectColor); di_jq('#popEffectColor_m').css('color', dataObj.effectColor); di_jq('#popEffectColor_m').val(dataObj.effectColor); }
                // show leader line
                if (dataObj.showLeader == "true") di_jq('#popLeaderChkbox_m').attr('checked', true); else di_jq('#popLeaderChkbox_m').attr('checked', false);
                // straight line
                if (dataObj.straightLine != "") di_jq('#popLeaderLnStr_m').val(dataObj.straightLine);
                // line style
                if (dataObj.lineStyle != '') { mapChangeLeaderStyle('mapLeaderStylel_' + dataObj.lineStyle); }
                // line width
                if (dataObj.lineWidth != '') di_jq('#popLeaderWidth_m').val(dataObj.lineWidth); else di_jq('#popLeaderWidth_m').val(1);
                // line color
                if (dataObj.lineColor != "") { di_jq('#popLeaderColor_m').css('background-color', dataObj.lineColor); di_jq('#popLeaderColor_m').css('color', dataObj.lineColor); di_jq('#popLeaderColor_m').val(dataObj.lineColor); }

                // manage label row 1
                if (dataObj.label1 != undefined && dataObj.label1 != '') {
                    // ["Arial","12","false","false","false","#000000","no"]
                    if (dataObj.label1[0] != '') di_jq('#di_mapLabelFontNm1').val(dataObj.label1[0]);
                    if (dataObj.label1[1] != '') di_jq('#di_mapPopSelFont1').val(dataObj.label1[1]);
                    if (dataObj.label1[2] == "true") di_jq('#mapPopTitle_Bold1').addClass('active'); else di_jq('#mapPopTitle_Bold1').removeClass('active');
                    if (dataObj.label1[3] == "true") di_jq('#mapPopTitle_Italic1').addClass('active'); else di_jq('#mapPopTitle_Italic1').removeClass('active');
                    if (dataObj.label1[4] == "true") di_jq('#mapPopTitle_Underline1').addClass('active'); else di_jq('#mapPopTitle_Underline1').removeClass('active');
                    var selColor1 = dataObj.label1[5];
                    if (selColor1 != '') {
                        di_jq('#inp_popdicolormap1').css('background-color', selColor1);
                        di_jq('#poph_dicolormap1').val(selColor1);
                    }
                    else {
                        selColor1 = '#000000';
                        di_jq('#inp_popdicolormap1').css('background-color', selColor1);
                        di_jq('#poph_dicolormap1').val(selColor1);
                    }
                    // for Caps
                    if (dataObj.label1[6]) {
                        // coming soon
                    }
                }
                // manage label row 2
                if (dataObj.label2 != undefined && dataObj.label2 != '') {
                    // ["Arial","12","false","false","false","#000000","no"]
                    if (dataObj.label2[0] != '') di_jq('#di_mapLabelFontNm2').val(dataObj.label2[0]);
                    if (dataObj.label2[1] != '') di_jq('#di_mapPopSelFont2').val(dataObj.label2[1]);
                    if (dataObj.label2[2] == "true") di_jq('#mapPopTitle_Bold2').addClass('active'); else di_jq('#mapPopTitle_Bold2').removeClass('active');
                    if (dataObj.label2[3] == "true") di_jq('#mapPopTitle_Italic2').addClass('active'); else di_jq('#mapPopTitle_Italic2').removeClass('active');
                    if (dataObj.label2[4] == "true") di_jq('#mapPopTitle_Underline2').addClass('active'); else di_jq('#mapPopTitle_Underline2').removeClass('active');
                    var selColor2 = dataObj.label2[5];
                    if (selColor2 != '') {
                        di_jq('#inp_popdicolormap2').css('background-color', selColor2);
                        di_jq('#poph_dicolormap2').val(selColor2);
                    }
                    else {
                        selColor2 = '#000000';
                        di_jq('#inp_popdicolormap2').css('background-color', selColor2);
                        di_jq('#poph_dicolormap2').val(selColor2);
                    }
                    // for Caps
                    if (dataObj.label2[6]) {
                        // coming soon
                    }
                }

                // for color picker
                di_jq('#mapPopTitle_Color1').ColorPicker({
                    color: selColor1,
                    onSubmit: function (hsb, hex, rgb, el) {
                        di_jq(el).ColorPickerHide();
                        di_jq('#inp_popdicolormap1').css('background-color', '#' + hex);
                        di_jq('#poph_dicolormap1').val('#' + hex);
                        //setMapDotDensityDetails();
                    }
                });
                // for map dot density marker color
                di_jq('#mapPopTitle_Color2').ColorPicker({
                    color: selColor2,
                    onSubmit: function (hsb, hex, rgb, el) {
                        di_jq(el).ColorPickerHide();
                        di_jq('#inp_popdicolormap2').css('background-color', '#' + hex);
                        di_jq('#poph_dicolormap2').val('#' + hex);
                        //setMapDotDensityDetails();
                    }
                });

                // check for visiblity of multi row and indented
                if (areaLabels.length > 1) {
                    di_jq('#popLabelMROW_m').show();
                    if (dataObj.multiRow == "true") {
                        di_jq('#popLabelINDT_m').attr('disabled', false);
                        di_jq('#mapPopLblRowSeptor').show();
                        di_jq('#mapPopLblRow2').show();
                    }
                    else {
                        di_jq('#popLabelINDT_m').attr('disabled', true);
                        di_jq('#mapPopLblRowSeptor').hide();
                        di_jq('#mapPopLblRow2').hide();
                    }
                    showHideMapLabel(0);
                }
                else {
                    di_jq('#popLabelMROW_m').attr('disabled', true);
                    di_jq('#popLabelINDT_m').attr('disabled', true);
                }
            }

            di_jq('#popupMapTab').val('label');
            break;
        case "addtheme": // add theme case
            //di_jq('#popThemeNmTxt_m').val('Theme - '+z('popSeries_m').options[0].text);
            di_jq('#popupMapTab').val('addtheme');
            di_jq('#shChartType').hide();
            di_jq('#popThemeNmTxt_m').focus();
            break;
        case "edittheme": // edit theme case
            di_jq('#popupMapTab').val('edittheme');

            var themeID = di_jq('#di_mapTheme').val();
            var thArrayData = createArrColThemeData(false, themeID);

            if (thArrayData != undefined) {
                // theme settings
                di_jq('#popSeries_m').removeAttr('multiple');
                di_jq("#popSeries_m").multiselect("destroy");

                var themeName = thArrayData[themeID][6];
                var themeType = thArrayData[themeID][0];
                var seriesId = thArrayData[themeID][1];
                var chartType = thArrayData[themeID][7];
                var tpismrd = thArrayData[themeID][21]; // there is mrd
                var tpselected = thArrayData[themeID][22]; // theme selected time period
                var tparray = thArrayData[themeID][23]; // theme time period array


                di_jq('#popThemeNmTxt_m').val(themeName);
                di_jq('#popThemeType_m').val(themeType);
                if (seriesId != '' && themeType != 'Chart') di_jq('#popSeries_m').val(seriesId);
                di_jq('#popChartType_m').val(chartType);

                //if(themeType!='Chart') {
                // for time period drop down manu
                di_jq('#pop_mapThStTimePrMenu').children('option').remove();
                if (tparray.length > 0) {
                    for (var i = 0; i < tparray.length; i++) {
                        z('pop_mapThStTimePrMenu').options[i] = new Option(tparray[i], tparray[i]);
                    }
                }
                if (tpismrd == "true") {
                    di_jq('#pop_mapThStTimePr2').attr("checked", "checked");
                    di_jq('#pop_mapThStTimePrMenu').hide();
                }
                else {
                    di_jq("#pop_mapThStTimePr1").attr("checked", "checked");
                    di_jq('#pop_mapThStTimePrMenu').show();
                }
                di_jq('#pop_mapThStTimePrMenu').val(tpselected);

                if (tparray.length == 0) {
                    //di_jq('#di_popThemeTPCont').hide();
                    di_jq('#pop_mapThStTimePr1').attr('disabled', true);
                    di_jq('#pop_mapThStTimePr2').attr('disabled', true);
                }
                //} // end chart if
                //else {
                //di_jq('#di_popThemeTPCont').hide();
                //}
                if (themeType == 'Chart') {
                    di_jq('#di_popThemeTPCont').hide();
                }

                // legend settings
                if (themeType == 'Color' || themeType == 'Hatch' || themeType == 'Symbol') {
                    if (themeType == 'Symbol') di_jq('#mapPopSmoothCl').hide(); else di_jq('#mapPopSmoothCl').show();
                    var breakCount = thArrayData[themeID][4];
                    var breakType = thArrayData[themeID][3];
                    var decimals = thArrayData[themeID][12];
                    var minimum = thArrayData[themeID][13];
                    var maximum = thArrayData[themeID][14];

                    di_jq('#pop_mapBrkCount').val(breakCount);
                    di_jq('#pop_mapBrkType').val(breakType);
                    di_jq('#pop_mapMin').val(minimum);
                    di_jq('#pop_mapMax').val(maximum);
                    di_jq('#pop_mapDecimals').val(decimals);

                    var legends = thArrayData[themeID][2];
                    if (legends != '') {
                        di_jq('#mapPopSmoothCl').attr('rel', legends.length);
                        di_jq('#mapPopIdColorHatchLegendMask').hide();
                        di_jq('#mapPopIdColorHatchLegend').html('');
                        var legendUI = '<table id="mapPopIdCrHatLdTbl" width="100%" cellpadding="1" cellspacing="1"><tr><td class="normalText"><b>Caption</b></td><td class="normalText"><b>Range</b></td><td class="normalText"><b>From</b></td><td class="normalText"><b>To</b></td><td class="normalText"><b>Count</b></td>';
                        if (themeType != 'Symbol') {
                            legendUI += '<td class="normalText"><b>Color</b></td>';
                        }
                        if (themeType == 'Hatch') {
                            legendUI += '<td class="normalText"><b>Polygon</b></td>';
                        }
                        else if (themeType == 'Symbol') {
                            // Start for settings
                            di_jq('#mapPopMarkerColor').ColorPicker({
                                onSubmit: function (hsb, hex, rgb, el) {
                                    di_jq(el).ColorPickerHide();
                                    di_jq('#mapPopMarkerColor').css('background-color', '#' + hex);
                                }
                            });
                            legendUI += '<td class="normalText"><b>Symbol</b></td>';
                        }
                        legendUI += '</tr></table>';
                        di_jq('#mapPopIdColorHatchLegend').append(legendUI);

                        for (var i = 0; i < legends.length; i++) {
                            var legendsSp = legends[i].split(delemeter);
                            var lCaption = legendsSp[3]; // legend caption
                            var lRangeFrom = legendsSp[4]; // range from
                            var lRagneTo = legendsSp[5]; // range to
                            var lCount = legendsSp[6]; // count
                            var lHatPat = legendsSp[2]; // hatch patern type
                            var ltitle = lRangeFrom + ' - ' + lRagneTo; // title of legend
                            var colorCode = '#' + decimalToHexString(Number(legendsSp[1])); // color code

                            // for symbol
                            if (themeType == 'Symbol') {
                                var MarkerType = legendsSp[7];
                                var MarkerSize = legendsSp[8];
                                var MarkerFont = legendsSp[9].split(",");
                                var MarkerChar = legendsSp[10];
                            }

                            if (i == (legends.length - 1)) {
                                lRangeFrom = '';
                                lRagneTo = '';
                                ltitle = '';
                            }

                            var legendUITr = '<tr><td class="legendCellText"><input type="text" id="legendLbl_' + i + '" value="' + lCaption + '" class="editableTxtBox"></td><td class="legendCellText" id="rangeLFT_' + i + '">' + ltitle + '</td><td class="legendCellText"><input type="text" id="rangeIF_' + i + '" value="' + lRangeFrom + '" class="editableTxtBox" style="width:60px;" onchange=\'getCalculatedLegend("' + themeID + '", "' + i + '", this.value)\'></td><td class="legendCellText"><input type="text" id="rangeIT_' + i + '" value="' + lRagneTo + '" class="editableTxtBox" style="width:60px;" onchange=\'getCalculatedLegend("' + themeID + '", "' + i + '", this.value)\'></td><td class="legendCellText" id="rangeLC_' + i + '">' + lCount + '</td>';
                            if (themeType != 'Symbol') {
                                legendUITr += '<td class="legendCellText"><div id="mappop_dicolor_' + i + '" style="cursor:pointer;float:left;width:15px;height:15px;background-color:' + colorCode + ';"></div></td>';
                            }
                            if (themeType == 'Hatch') {
                                legendUITr += '<td class="legendCellText"><div id="mappop_hatchpat_list_' + i + '" style="display:none;position:absolute;margin-left:-270px;border:1px solid #d3d3d3;background-color:#fff;width:264px; height:120px;padding:3px;">Patern List</div><div id="mappop_hatchpat_' + i + '" style="cursor:pointer;float:left;margin:0;margin-left:3px;" class="hatch_patern_' + lHatPat + '"   onclick=\'mapPOPOpenHatchPaternList(' + i + ')\' title="' + lHatPat + '"></div></td>';
                            }
                            else if (themeType == 'Symbol') {
                                var getAVal = Math.abs(getA(Number(legendsSp[1])));
                                if (getAVal == 1) getAVal = 255;
                                var markerColor = '#' + decimalToHexString(getAVal) + decimalToHexString(Number(legendsSp[1]));
                                var markerDetails = markerColor + '~~' + MarkerChar + '~~' + MarkerSize + '~~' + MarkerFont[0] + '~~' + getAVal;

                                legendUITr += '<td class="legendCellText"><div id="mappop_symbolpat_' + i + '" style="width:31px;height:21px;cursor:pointer;margin-left:3px;overflow:hidden;" onclick=\'mapPOPOpenSymbolPaternList(' + i + ')\' rel="' + markerDetails + '" class="webding_container ' + MarkerFont[0] + '_' + MarkerChar + '"></div></td>';
                            }
                            legendUITr += '</tr>';

                            di_jq('#mapPopIdCrHatLdTbl').append(legendUITr);

                            if (themeType != 'Symbol') {
                                initMapColorPic('mappop_dicolor_' + i, i, '', colorCode);
                            }
                            // binding even for lagend validation
                            /*di_jq('#rangeIF_'+i).bind('change', function () { 
                            // valling function to validate legend range
                            var val = this.value;
                            if(val < minimum || val > maximum) {
                            alert('Please enter valid value.');
                            di_jq(this).focus();
                            }
								
								
                            });
                            di_jq('#rangeIT_'+i).bind('change', function () { 
                            var val = this.value;
                            if(val > maximum || val < minimum || val < lRangeFrom) {
                            alert('Please enter valid value.');
                            di_jq(this).focus();
                            }
                            });*/
                        } // end for

                    } // end legend if

                    makeEdtableLegend(breakType);
                }
                else if (themeType == 'DotDensity') { // DotDensity legend/settings

                    var dotStyle = thArrayData[themeID][8];
                    var dotSize = thArrayData[themeID][9];
                    var dotColor = '#' + decimalToHexString(Number(thArrayData[themeID][10]));

                    // for map dot density marker color
                    di_jq('#mapPopDotDtyColor').ColorPicker({
                        color: dotColor,
                        onSubmit: function (hsb, hex, rgb, el) {
                            di_jq(el).ColorPickerHide();
                            di_jq('#mapPopDotDtyColor').css('background-color', '#' + hex);
                            //di_jq('#mapDotDtyColorCodeH').val('#'+hex);
                            //setMapDotDensityDetails();
                        }
                    });

                    var dotValue = thArrayData[themeID][11];
                    var getAVal = Math.abs(getA(Number(thArrayData[themeID][10])));
                    var dotOpacityHtml = getOpacityHtml(getAVal);
                    //var dotOpacityS2 = getOpacityS(getAVal);

                    if (getAVal == 1) getAVal = 255;
                    di_jq('#mapPopDotDtyOpacityTxt').val(getAVal);
                    di_jq("#mapPopDotDtyOpacity").slider({
                        range: "max",
                        min: 0,
                        max: 255,
                        step: 1,
                        value: getAVal,
                        slide: function (event, ui) {
                            di_jq("#mapPopDotDtyOpacityTxt").val(ui.value);
                        }
                    });

                    di_jq('#mapPopDotDtyStyle_0').attr('class', 'mapDotDtyStyle_' + dotStyle);
                    di_jq('#mapPopDotDtyStyle_0').attr('rel', dotStyle);
                    di_jq('#mapPopDotDtySize').val(dotSize);
                    di_jq('#mapPopDotDtyColor').css('background-color', dotColor);
                    di_jq('#mapPopDotDtyColor').css('opacity', dotOpacityHtml);
                    di_jq('#mapPopDotDtyValue').val(dotValue);
                }
                else if (themeType == 'Chart') { // Chart legend/settings

                    var chartLegend = thArrayData[themeID][15];
                    var chartWidth = thArrayData[themeID][16];
                    var chartHeight = thArrayData[themeID][17];
                    var chartDecimals = thArrayData[themeID][18];
                    var chartShowData = thArrayData[themeID][19];
                    var chartIsMrd = thArrayData[themeID][20];
                    //var chartTimeSeries = thArrayData[index][21];
                    var pieWidth = thArrayData[themeID][24];
                    if (chartType == 'Pie' || chartType == 'Line') chartWidth = Math.round(pieWidth * 2);
                    var chartLineTickness = thArrayData[themeID][25];

                    // multiple selecttion
                    var seriesLength = di_jq('#popSeries_m option').length;
                    if (seriesLength > 1) {
                        di_jq('#popSeries_m').attr('multiple', 'multiple');
                        di_jq('#popSeries_m option').each(function () { di_jq(this).attr("selected", false); });
                        var seriesArr = seriesId.split(",");
                        for (var i = 0; i < seriesArr.length; i++) {
                            di_jq('#popSeries_m option[value="' + seriesArr[i] + '"]').attr("selected", "selected");
                        }
                        di_jq("#popSeries_m").multiselect({
                            selectedText: "# of # selected",
                            height: 'auto',
                            header: false
                        });
                    }
                    else {
                        di_jq("#popSeries_m").multiselect("destroy");
                    } //

                    // start for chart legend
                    // start for chart legend
                    var themeID = di_jq('#di_mapTheme').val();
                    var alphaVal = mapObj.getChartLegendAphaVal(themeID, 'ChartLegend');
                    if (alphaVal == 1) alphaVal = 255;

                    var opacity = '255';
                    if (chartLegend != '') {
                        di_jq('#mapPopChartLegend').html('');
                        di_jq('#mapPopChartLegend').attr('rel', chartLegend.length);
                        for (var i = 0; i < chartLegend.length; i++) {
                            var legendsAr = chartLegend[i].split(delemeter);
                            var SrId = legendsAr[0];
                            var SrName = legendsAr[1];
                            var SrColor = decimalToHexString(Number(legendsAr[2]));

                            if (i == 0) {
                                opacity = Math.abs(getA(Number(legendsAr[2])));
                                if (opacity == 1) opacity = 255;
                                var alphaColor = '#' + decimalToHexString(alphaVal) + SrColor;
                            }

                            //var	sybOpecty = 'opacity:' + getOpacityHtml( getA(Number(legendsAr[2])) ) +';';
                            var sybOpecty = 'opacity:' + getOpacityHtml(alphaVal) + ';';

                            var legendUi = '<p style="padding:1px;"><div id="pop_mapChartDiColor_' + i + '" class="mapchartdicolor" style="background-color:#' + SrColor + '; ' + sybOpecty + '" rel="' + alphaColor + '" val="' + SrColor + '"></div> &nbsp;<label class="normalText" title="' + SrName + '">';

                            if (SrName.length > 100)
                                legendUi += SrName.substr(0, 100) + '...';
                            else
                                legendUi += SrName;

                            legendUi += '</label></p>';

                            di_jq('#mapPopChartLegend').append(legendUi);

                            // initiate color picker for chart legend
                            initMapChartColorPic('pop_mapChartDiColor_' + i, i, SrColor);

                        } // end for
                    } // end legend

                    // enable slider for legend opacity
                    di_jq('#mapPopChartOpacityTxt').val(alphaVal);
                    di_jq("#mapPopChartOpacity").slider({
                        range: "max",
                        min: 2,
                        max: 255,
                        step: 1,
                        value: alphaVal,
                        slide: function (event, ui) {
                            di_jq("#mapPopChartOpacityTxt").val(ui.value);
                            var legendCount = di_jq('#mapPopChartLegend').attr('rel');
                            for (var i = 0; i < legendCount; i++) {
                                //var BgColor = di_jq('#pop_mapChartDiColor_'+i).css('background-color');
                                var BgColor = di_jq('#pop_mapChartDiColor_' + i).attr('val');
                                //var hexaCCode = '#'+ decimalToHexString(ui.value) + hexc(BgColor).replace("#", "");
                                var hexaCCode = '#' + decimalToHexString(ui.value) + BgColor;
                                di_jq('#pop_mapChartDiColor_' + i).attr('rel', hexaCCode);
                                di_jq('#pop_mapChartDiColor_' + i).css('opacity', getOpacityHtml(ui.value));
                            }
                        }
                    });
                    // start for chart settings tab
                    di_jq("#pop_mapChartStDecimals").val(chartDecimals);
                    if (chartShowData == "true") di_jq("#pop_mapChartStViewDtVal").attr("checked", true); else di_jq("#pop_mapChartStViewDtVal").attr("checked", false);
                    if (chartIsMrd == "true") di_jq('#pop_mapChartStTimeMR').attr("checked", "checked"); else di_jq("#pop_mapChartStTimeSr").attr("checked", "checked");
                    di_jq('#pop_mapChartSliderWTxt').val(chartWidth);
                    di_jq('#pop_mapChartSliderHTxt').val(chartHeight);
                    di_jq('#pop_mapChartLineSliderTTxt').val(chartLineTickness);

                    // enable width slider
                    di_jq("#pop_mapChartSliderW").slider({
                        range: "max",
                        min: 1,
                        max: 20,
                        step: 1,
                        value: chartWidth,
                        slide: function (event, ui) {
                            di_jq("#pop_mapChartSliderWTxt").val(ui.value);
                        }
                    });
                    // enable height slider
                    di_jq("#pop_mapChartSliderH").slider({
                        range: "max",
                        min: 1,
                        max: 20,
                        step: 1,
                        value: chartHeight,
                        slide: function (event, ui) {
                            di_jq("#pop_mapChartSliderHTxt").val(ui.value);
                        }
                    });
                    // enable line tickness
                    di_jq("#pop_mapChartLineSliderT").slider({
                        range: "max",
                        min: 1,
                        max: 20,
                        step: 1,
                        value: chartLineTickness,
                        slide: function (event, ui) {
                            di_jq("#pop_mapChartLineSliderTTxt").val(ui.value);
                        }
                    });
                } // end chart
            }

            //di_jq('#popThemeNmTxt_m').focus();

            break;
        case "themelegend":
            di_jq('#popupMapTab').val('themelegend');
            break;
        case "chartSetting":
            di_jq('#popupMapTab').val('chartSetting');
            // added 22 May for getting time period list
            var themeID = di_jq('#di_mapTheme').val();

            var thArrayData = createArrColThemeData(false, themeID);
            var chartIsMrd = thArrayData[themeID][20];
            var chartType = thArrayData[themeID][7];

            var timePs = mapObj.getChartTimePeriods(themeID);
            if (timePs != '' && timePs != null && timePs != undefined && timePs != 'false') {
                var ui = '<div id="diChartTimePeriods">';
                var tmpArr = timePs.split(',');
                for (var i = 0; i < tmpArr.length; i++) {
                    var checked = '';
                    var tmpValAr = tmpArr[i].split('_');
                    var tmpVal = tmpValAr[0]; // time period
                    var isChk = tmpValAr[1]; // is checked t/f
                    if (isChk == 't') checked = 'checked';
                    ui += '<p style="padding:0px;"><input type="checkbox" value="' + tmpVal + '" ' + checked + '> ' + tmpVal + '</p>';
                } //end for
                ui += '</div>';
                di_jq('#mapPopChartStTimeList').html(ui).show();
            } // end if

            if (chartIsMrd == "true") {
                di_jq('#mapPopChartStTimeList').hide();
            }

            if (chartType == 'Pie' || chartType == 'Line') {
                di_jq('#mapPopChartHghtTR').hide();

                if (chartType == 'Line') di_jq('#mapPopChartLineThTR').show(); else di_jq('#mapPopChartLineThTR').hide();
            }
            else {
                di_jq('#mapPopChartHghtTR').show();
                di_jq('#mapPopChartLineThTR').hide();
            }

            break;
        case "layer":
            if (isDisputedBdriesLoaded == 'no') {
                loadingMapDataProcess(true);
                isDisputedBdriesLoaded = 'yes';
                mapObj.getDisputedBoundry();
                loadingMapDataProcess(false);
            }
            di_jq('#popupMapTab').val('baselayer');

            // getting label text
            var pLayerTxt = di_jq('#lngMapPopLayer').html();
            var pColorTxt = di_jq('#lngMapPopColor').html();
            var pStyleTxt = di_jq('#lngMapPopStyle').html();

            // start for base layer
            var baseStrJson = mapObj.getBaseLayerInfo();
            var bjObj = di_jq.parseJSON(baseStrJson).base;
            if (bjObj.length > 0) {
                var htmlUI = '<table width="100%" cellpadding="1" cellspacing="1"><tr><td class="normalText"><b>' + pLayerTxt + '</b></td></tr>';
                for (var i = 0; i < bjObj.length; i++) {
                    var jstr = bjObj[i].split(mapDataDelemeter);
                    var layerid = jstr[0]; // layer id
                    var visiblity = jstr[1]; // layer visibility
                    var type = jstr[2]; // layer type
                    var checked = '';

                    if (visiblity == 'true') checked = "checked";
                    htmlUI += '<tr><td class="legendCellText" onclick="mapSetLayerSelection(this)" style="cursor:pointer;"><span style="float:left;"><input id="' + layerid + '" type="checkbox" ' + checked + '></span> <span class="LayerIcon LayerIcon_' + type + '" title="' + type + '" style="margin-top:-4px;"></span>&nbsp;' + layerid + '</td></tr>';
                }
                htmlUI += '</table>';

                di_jq('#mapPopTab-1_sec_con').html(htmlUI);
            } // end if

            // start for feature layer
            var featureStrJson = mapObj.getFeatureLayerInfo();
            var fjObj = di_jq.parseJSON(featureStrJson);
            var fjObj1 = fjObj.disputed;
            var fjObj2 = fjObj.feature;
            var fjObj3 = fjObj.showdisputed;
            // for show/hide disputed boundries
            if (fjObj3[0] == false) di_jq('#flDisputedBnd').closest('div').hide();

            // for check/uncheck disputed boundries
            if (fjObj1[0] == true) di_jq('#flDisputedBnd').attr('checked', true); else di_jq('#flDisputedBnd').attr('checked', false);

            // for feature layer
            if (fjObj2.length > 0) {
                di_jq('#mapPopTab-2_sec_con').html('');
                var htmlUI = '<table id="mapPopTab-2_sec_con_tbl" width="100%" cellpadding="1" cellspacing="1"><tr><td class="normalText"><b>' + pLayerTxt + '</b></td><td class="normalText" width="45"><b>' + pColorTxt + '</b></td><td class="normalText" width="60"><b>' + pStyleTxt + '</b></td><td width="50">&nbsp;</td></tr></table>';

                di_jq('#mapPopTab-2_sec_con').append(htmlUI);

                for (var i = 0; i < fjObj2.length; i++) {
                    var jstr = fjObj2[i].split(mapDataDelemeter);
                    var layerid = jstr[0]; // layer id
                    var visiblity = jstr[1]; // layer visibility
                    var type = jstr[2]; // layer type
                    var colorCode = jstr[3]; // layer color decimal number
                    var style = jstr[4]; // layer style
                    var size = jstr[5]; // layer opacity/size/thickness
                    var isdisputed = jstr[6]; // is layer disputed true/false
                    var checked = '';
                    var sybOpecty = '';
                    var isdisputedtxt = '';
                    if (isdisputed == "true") isdisputedtxt = 'disputed';                
                    var commonAlpha = size;
                    if (type == 'PolygonFeature') {
                        sybOpecty = 'opacity:' + getOpacityHtml(size) + ';';
                    }
                    else {
                        commonAlpha = 255;
                    }
                    var alphaColor = '#' + decimalToHexString(commonAlpha) + colorCode.replace("#", "");
                    if (visiblity == 'true') checked = "checked";
                    var htmlUI = '<tr><td class="legendCellText"><span style="float:left;"><input id="' + layerid + '" type="checkbox" ' + checked + ' class="' + isdisputedtxt + '"></span> <span class="LayerIcon LayerIcon_' + type + '" title="' + type + '"></span>&nbsp;' + layerid + '</td><td class="legendCellText"><div id="ly_color_' + layerid + '" style="cursor:pointer;float:left;width:40px;height:15px;background-color:' + colorCode + '; ' + sybOpecty + '" rel="' + alphaColor + '"></div></td><td class="legendCellText">';

                    // for style
                    htmlUI += '<div id="mappop_layer_stylelist_' + layerid + '" style="display:none;position:absolute;border:1px solid #d3d3d3;background-color:#fff;padding:3px;z-index:999;">List</div>';

                    if (type == 'PolygonFeature') {
                        htmlUI += '<div id="ly_style_' + layerid + '" class="hatch_patern_' + style + '" style="cursor:pointer;margin:0;margin-left:10px;" onclick=\'mapPOPOpenLayerStyleList("' + layerid + '", "Polygon")\' rel="' + style + '"></div>';
                    }
                    else if (type == 'PolyLineFeature') {
                        htmlUI += '<div id="ly_style_' + layerid + '" class="mapBdrStIcon mapBdrStIcon_' + style + '" style="cursor:pointer;width:60px;" rel="' + style + '" onclick=\'mapPOPOpenLayerStyleList("' + layerid + '", "PolyLine")\'></div>';
                    }
                    else if (type == 'PointFeature') {
                        htmlUI += '<div id="ly_style_' + layerid + '" class="mapDotDtyStyle_' + style + '" style="margin:1px;width:60px;" rel="' + style + '" onclick=\'mapPOPOpenLayerStyleList("' + layerid + '", "Point")\'></div>';
                    }

                    htmlUI += '</td><td class="legendCellText">';

                    // for opacity/size/thickness
                    if (type == 'PolygonFeature') {
                        htmlUI += '<div id="ly_slider_' + layerid + '" style="background-color:#e2e2e2;width:40px;"></div><input type="hidden" id="ly_opszth_' + layerid + '" value="' + size + '">';
                    }
                    else if (type == 'PolyLineFeature' || type == 'PointFeature') {
                        htmlUI += '<input type="text" id="ly_opszth_' + layerid + '" value="' + size + '" class="vcInputTxtBox" style="width:45px;">';
                    }

                    htmlUI += '</td></tr>';

                    di_jq('#mapPopTab-2_sec_con_tbl').append(htmlUI);

                    // init color picker
                    initLayerColorPickers('ly_color_' + layerid, commonAlpha, colorCode);

                    if (type == 'PolygonFeature') {
                        initLayerOpacitySlider(layerid, size);
                    }
                }

            } // end if

            break;
    }
}
/*function to change visibilty of chart seeting time period list */
function changeVisibilityTMP(val) {
    if (val == 'MR') {
        di_jq('#mapPopChartStTimeList').hide();
    }
    else {
        di_jq('#mapPopChartStTimeList').show();
    }
}
/*function to change visibilty of chart seeting time period list */
function changeVisibilityTHTP(val) {
    if (val == 'MR') {
        di_jq('#pop_mapThStTimePrMenu').hide();
    }
    else {
        di_jq('#pop_mapThStTimePrMenu').show();
    }
}
/* function to check/uncheck disputed boundries */
function chkUnchkDisputedBdr(checkval) {
    if (checkval) {
        di_jq('#mapPopTab-2_sec_con_tbl input[class="disputed"]').attr('checked', true);
    }
    else {
        di_jq('#mapPopTab-2_sec_con_tbl input[class="disputed"]').attr('checked', false);
    }
}
/* function to init slider for layer */
function initLayerOpacitySlider(layerId, alphaVal) {
    di_jq("#ly_slider_" + layerId).slider({
        range: "max",
        min: 0,
        max: 255,
        step: 1,
        value: Math.abs(alphaVal),
        stop: function (event, ui) {
            di_jq("#ly_opszth_" + layerId).val(ui.value);
        }
    });
}
/* function to init slider for layer */
function initLayerColorPickers(layerId, alphaVal, selColor) {
    di_jq('#' + layerId).ColorPicker({
        color: selColor,
        onSubmit: function (hsb, hex, rgb, el) {
            di_jq(el).ColorPickerHide();
            di_jq('#' + layerId).css('background-color', '#' + hex);
            if (alphaVal == 1) alphaVal = 255;
            hexaCCode = '#' + decimalToHexString(alphaVal) + hex;

            di_jq('#' + layerId).attr('rel', hexaCCode);
        }
    });
}
/* function to open map hatch patern list */
function mapPOPOpenLayerStyleList(layerId, type) {
    di_jq('#mapThemeType').attr('rel', 'layer_' + layerId.replace(/\_/g, '#'));
    di_jq('#mapPopTab-2_sec_con div[id*="mappop_layer_stylelist_"]').each(function () {
        if (this.id != 'mappop_layer_stylelist_' + layerId)
            di_jq(this).hide();
    });
    if (type == 'Polygon') {
        di_jq('#mappop_layer_stylelist_' + layerId).css('width', '264px');
        di_jq('#mappop_layer_stylelist_' + layerId).css('height', '120px');
        di_jq('#mappop_layer_stylelist_' + layerId).css('margin-left', '-270px');
        var paternUI = di_jq('#map_hatchpat_list').html();
        di_jq('#mappop_layer_stylelist_' + layerId).html(paternUI);
    }
    else if (type == 'PolyLine') {
        var parentUI = '<div class="mapBdrStIcon mapBdrStIcon_Solid" style="height:12px;" onclick=\'mapLayerChangeLineStyle("' + layerId + '","Solid")\'></div><div class="mapBdrStIcon mapBdrStIcon_Dash" style="height:12px;" onclick=\'mapLayerChangeLineStyle("' + layerId + '","Dash")\'></div><div class="mapBdrStIcon mapBdrStIcon_Dot" style="height:12px;" onclick=\'mapLayerChangeLineStyle("' + layerId + '","Dot")\'></div><div class="mapBdrStIcon mapBdrStIcon_DashDot" style="height:12px;" onclick=\'mapLayerChangeLineStyle("' + layerId + '","DashDot")\'></div><div class="mapBdrStIcon mapBdrStIcon_DashDotDot" style="height:12px;" onclick=\'mapLayerChangeLineStyle("' + layerId + '","DashDotDot")\'></div>';
        di_jq('#mappop_layer_stylelist_' + layerId).css('width', '55px');
        di_jq('#mappop_layer_stylelist_' + layerId).css('height', '62px');
        di_jq('#mappop_layer_stylelist_' + layerId).css('margin-left', '-61px');
        di_jq('#mappop_layer_stylelist_' + layerId).html(parentUI);
    }
    else if (type == 'Point') {
        var parentUI = '<div class="mapDotDtyStyle_Circle" onclick=\'mapLayerChangeDotStyle("' + layerId + '","Circle")\'></div><div class="mapDotDtyStyle_Square" onclick=\'mapLayerChangeDotStyle("' + layerId + '","Square")\'></div><div class="mapDotDtyStyle_Triangle" onclick=\'mapLayerChangeDotStyle("' + layerId + '","Triangle")\'></div><div class="mapDotDtyStyle_Cross" onclick=\'mapLayerChangeDotStyle("' + layerId + '","Cross")\'></div>';
        di_jq('#mappop_layer_stylelist_' + layerId).css('width', '73px');
        di_jq('#mappop_layer_stylelist_' + layerId).css('height', '70px');
        di_jq('#mappop_layer_stylelist_' + layerId).css('margin-left', '-79px');
        di_jq('#mappop_layer_stylelist_' + layerId).html(parentUI);
    }
    di_jq('#mappop_layer_stylelist_' + layerId).slideToggle();
}
/* function to change layer dot style */
function mapLayerChangeDotStyle(layerId, type) {
    di_jq('#ly_style_' + layerId).attr('class', 'mapDotDtyStyle_' + type);
    di_jq('#ly_style_' + layerId).attr('rel', type);
    di_jq('#mappop_layer_stylelist_' + layerId).slideToggle();
}
/* function to change layer line style */
function mapLayerChangeLineStyle(layerId, type) {
    di_jq('#ly_style_' + layerId).attr('class', 'mapBdrStIcon mapBdrStIcon_' + type);
    di_jq('#ly_style_' + layerId).attr('rel', type);
    di_jq('#mappop_layer_stylelist_' + layerId).slideToggle();
}
/* function to change layer order */
function mapChangeLayerOrder(act) {
    di_jq('#mapPopTab-1_sec_con td').each(function () {
        if (di_jq(this).is('.layerSelected')) {
            var row = di_jq(this).parents("tr:first");
            // set selected layer id
            var selId = di_jq(this).find('input').attr('id');
            var allIds = di_jq('#mapPopTab-1_sec_con').attr('rel');
            var checkExist = -1;
            if (allIds != '' && allIds != null && allIds != undefined) {
                checkExist = allIds.search(selId);
            } else allIds = '';
            if (checkExist == -1) {
                di_jq('#mapPopTab-1_sec_con').attr('rel', allIds + selId + mapDataDelemeter);
            }
            //

            if (act == 'up') {
                if (row.prevAll().length > 1) {
                    row.insertBefore(row.prev());
                }
            }
            else if (act == 'down') {
                row.insertAfter(row.next());
            }
            else if (act == 'first') {
                if (row.prevAll().length > 1) {
                    var row2 = di_jq('#mapPopTab-1_sec_con tr:first');
                    row.insertBefore(row2.next());
                }
            }
            else if (act == 'last') {
                var row2 = di_jq('#mapPopTab-1_sec_con tr:last');
                row.insertAfter(row2);
            }
        }
    });
}
/* function to set layer selection on map pop-up */
function mapSetLayerSelection(obj) {
    di_jq('#mapPopTab-1_sec_con td').removeClass('layerSelected');
    di_jq(obj).addClass('layerSelected');
}
/* function to make readonly true/false for legend text field */
function makeEdtableLegend(action) {
    di_jq('#mapPopIdCrHatLdTbl input[class="editableTxtBox"]').each(function () {
        var eleId = this.id;
        if (action == 'Continuous') {
            if (eleId.indexOf("rangeIF_") >= 0) {
                di_jq(this).attr('readonly', true);
            }
            if (eleId.indexOf("rangeIT_") >= 0) {
                di_jq(this).attr('readonly', false);
            }
        }
        else if (action == 'Discontinuous') {
            if (eleId.indexOf("rangeIF_") >= 0) {
                di_jq(this).attr('readonly', false);
            }
            if (eleId.indexOf("rangeIT_") >= 0) {
                di_jq(this).attr('readonly', false);
            }
        }
        else {
            if (eleId.indexOf("rangeIF_") >= 0) {
                di_jq(this).attr('readonly', true);
            }
            if (eleId.indexOf("rangeIT_") >= 0) {
                di_jq(this).attr('readonly', true);
            }
        }
    });
}
/* function to get calculated legend ranges */
function getCalculatedLegend(themeId, index, val) {
    try {
        var breakType = di_jq('#pop_mapBrkType').val();
        if (breakType == 'Continuous') { // only for Continuous break type
            var newLegend = mapObj.GetLegendRanges(themeId, index, val);
            var newLegendAr = newLegend.split(mapDataDelemeter);
            if (newLegendAr[0] == "false") return false;
            for (var i = 0; i < newLegendAr.length; i++) {
                var legendStr = newLegendAr[i].split("~~");
                var rangeFrom = legendStr[0];
                var rangeTo = legendStr[1];
                var rangeCount = legendStr[2];
                var captionName = legendStr[3];

                var upIndex = parseInt(i) + parseInt(index);
                di_jq('#rangeLFT_' + upIndex).html(rangeFrom + ' - ' + rangeTo);
                di_jq('#rangeIF_' + upIndex).val(rangeFrom);
                di_jq('#rangeIT_' + upIndex).val(rangeTo);
                di_jq('#rangeLC_' + upIndex).html(rangeCount);
                di_jq('#legendLbl_' + upIndex).val(captionName);
            }
        }
        else if (breakType == 'Discontinuous') { // for Discontinuous
            //alert(index+', '+val);
            var rangeFrom = di_jq('#rangeIF_' + index).val();
            var rangeTo = di_jq('#rangeIT_' + index).val();
            var captionName = rangeFrom + ' - ' + rangeTo;
            di_jq('#legendLbl_' + index).val(captionName);
        }
    }
    catch (err) { }

}
/* function to upload theme settings */
function doUploadThemeSettings() {
    di_jq('#mapPopIdColorHatchLegendMask').show();
    di_jq('#mapPopIdColorHatchLegendMask img').hide();
    var filename = di_jq('#uplThemeSetPop').val();
    var filetype = filename.split('.').pop().toLowerCase(); // get file type
    if (filetype != "xml") { alert('Please upload correct file'); return false; }

    //
    di_jq('#uplThemeSetPopFrm').ajaxForm({
        success: function (responce) {
            if (responce != '') {
                di_jq('#uplThemeSetPop').val('');
                //redraw parent UI
                //redrawThemeDetails(['detail', 'title']);
                di_jq('#popupMapTab').val('uploadlegend');
                di_jq('#uploadedTLFileNM').val(responce);

                alert('File has been uploaded, please click OK/Apply button to apply settings.');
            } // end if
            else {
                di_jq('#popupMapTab').val('themelegend');
                di_jq('#uploadedTLFileNM').val('');
            }
        } // end success
    });

    di_jq('#uplThemeSetPopFrm').submit();
}
/* function to apply after uploading theme legend xml file*/
function applyUploadedThemeLegendSettings() {
}
/* function to download map theme settings */
function downloadMapThemeSet() {
    var themeId = di_jq('#di_mapTheme').val();

    var form = document.createElement("form");
    form.setAttribute("method", "post");
    form.setAttribute("action", "ExportMap.aspx");

    // export type
    var Field1 = document.createElement("input");
    Field1.setAttribute("type", "hidden");
    Field1.setAttribute("name", "ExportType");
    Field1.setAttribute("value", "legendinfo");
    form.appendChild(Field1);
    // theme id
    var Field2 = document.createElement("input");
    Field2.setAttribute("type", "hidden");
    Field2.setAttribute("name", "themeId");
    Field2.setAttribute("value", themeId);
    form.appendChild(Field2);
    // filename
    var Field3 = document.createElement("input");
    Field3.setAttribute("type", "hidden");
    Field3.setAttribute("name", "filename");
    Field3.setAttribute("value", "legendsettings");
    form.appendChild(Field3);

    document.body.appendChild(form);
    form.submit();
}
/* function to get smooth colors */
function getSmoothColors() {
    try {
        var legendCount = di_jq('#mapPopSmoothCl').attr('rel');
        if (legendCount > 1) {
            var themeId = di_jq('#di_mapTheme').val();
            var startColor = di_jq('#mappop_dicolor_0').css('background-color');
            var endColor = di_jq('#mappop_dicolor_' + eval(legendCount - 2)).css('background-color');

            var colors = mapObj.getSmoothColors(themeId, hexc(startColor), hexc(endColor));
            var colorsAr = colors.split(',');
            for (var i = 0; i < colorsAr.length; i++) {
                if (colorsAr[i] != '') {
                    di_jq('#mappop_dicolor_' + i).css('background-color', colorsAr[i]);
                }
            }
        }
    }
    catch (err) { }

}
function hexc(colorval) {
    var parts = colorval.match(/^rgb\((\d+),\s*(\d+),\s*(\d+)\)$/);
    delete (parts[0]);
    for (var i = 1; i <= 3; ++i) {
        parts[i] = parseInt(parts[i]).toString(16);
        if (parts[i].length == 1) parts[i] = '0' + parts[i];
    }
    color = '#' + parts.join('');
    return color;
}

/* function to change legend settings on map POP-up */
function setMapLegendSetting(type, val) {
    // desable legend box
    di_jq('#mapPopIdColorHatchLegendMask').show();
    di_jq('#mapPopIdColorHatchLegendMask img').show();
    //di_jq('#popupMapTab').val('legendSetting');

    // set and load map legend setting
    setTimeout(function () {
        updaetLoadMapLegendSettings();
    }, 500);
}
/* function to change map border style */
function mapChangeBdrStyle(id) {

    var newType = di_jq('#' + id).attr('rel');
    var newClass = di_jq('#' + id).attr('class');

    di_jq('#mapBdrStyleDiv').hide();

    di_jq('#mapBdrStl_Selected').attr('rel', newType);
    di_jq('#mapBdrStl_Selected').attr('class', newClass);
}
/* function to change map leader style */
function mapChangeLeaderStyle(id) {

    var newType = di_jq('#' + id).attr('rel');
    var newClass = di_jq('#' + id).attr('class');

    di_jq('#mapLeaderStyleDiv').hide();

    di_jq('#mapLeaderStylel_Selected').attr('rel', newType);
    di_jq('#mapLeaderStylel_Selected').attr('class', newClass);
}
/* function to click on ok/apply button on map pop-up */
function applyMapPopup(action) {
    var selectedTab = di_jq('#popupMapTab').val();
    if (selectedTab == 'border') {
        setMapBorderStyle(); // set border style
    }
    else if (selectedTab == 'label') {
        setMapLabelStyle();
    }
    else if (selectedTab == 'addtheme') {
        if (di_jq('#popSeries_m').val() == '') {
            alert('Please select series.');
            di_jq('#popSeries_m').focus();
            return false;
        }
        else if (di_jq('#popThemeNmTxt_m').val() == '') {
            alert('Please enter theme name.');
            di_jq('#popThemeNmTxt_m').focus();
            return false;
        } else {
            var res = updateMapTheme('add');
            if (res == false) { action = 'apply'; di_jq('#popThemeNmTxt_m').focus(); return false; }

            if (action == 'apply') openMapPropertyPopUp('theme_add');
        }
    }
    else if (selectedTab == 'edittheme') {
        // update only theme info
        if (di_jq('#popThemeNmTxt_m').val() == '') {
            alert('Please enter theme name.');
            di_jq('#popThemeNmTxt_m').focus();
            return false;
        }
        else {
            var themeType = di_jq('#popThemeType_m').val();
            var res = updateMapTheme('edit');
            if (res == false) {
                action = 'apply';
                //di_jq('#popThemeNmTxt_m').focus();
            }
            else {
                if (action == 'apply') openMapPropertyPopUp('theme_edit');
            }
            //else if(themeType=='Chart'){
            //if(action=='apply') openMapPropertyPopUp('theme_edit');
            //}
            //else {
            //if(action=='apply') di_jq('#popTabChart_m').parent().hide();
            //}
        }
    }
    else if (selectedTab == 'legendSetting') {

        // set and load map legend setting
        updaetLoadMapLegendSettings();

    }
    else if (selectedTab == 'uploadlegend') {
        var filename = di_jq('#uploadedTLFileNM').val();
        var themeId = di_jq('#di_mapTheme').val();

        if (filename != '')
            mapObj.setUploadedLegendSettings(themeId, filename);

        di_jq('#uploadedTLFileNM').val('');
        di_jq('#mapPopIdColorHatchLegendMask').hide();

        applyMapPopUpSetting('edittheme');
        di_jq('#popupMapTab').val('themelegend');

        //redraw parent UI
        redrawThemeDetails(['detail', 'title']);
    }
    else if (selectedTab == 'themelegend' || selectedTab == 'chartSetting') {
        // update map legend, nutral form (update all legend togather and map image will update)
        //legendIndex~~Caption~~Title~~RangeFrom~~RangeTo~~ShapeCount~~Color~~HatchPatern
        var subDelm = '~~';
        var themeId = di_jq('#di_mapTheme').val();
        var themeType = di_jq('#mapThemeType').attr('title');
        var legendCount = di_jq('#mapPopSmoothCl').attr('rel');
        var inputStr = '';

        if (themeType == 'Color' || themeType == 'Hatch' || themeType == 'Symbol') {
            for (var i = 0; i < legendCount; i++) {
                var LCaption = di_jq('#legendLbl_' + i).val();
                var LFrom = di_jq('#rangeIF_' + i).val();
                var LTo = di_jq('#rangeIT_' + i).val();
                var LFromTo = LFrom + ' - ' + LTo;
                var LCount = di_jq('#rangeLC_' + i).html();
                var LColor = di_jq('#mappop_dicolor_' + i).css('background-color'); //hexc()
                var LHatchPat = '';
                if (themeType == 'Hatch')
                    LHatchPat = di_jq('#mappop_hatchpat_' + i).attr('title');

                if (i > 0)
                    inputStr += mapDataDelemeter;

                if (themeType == 'Symbol') {
                    LSymbolPat = di_jq('#mappop_symbolpat_' + i).attr('rel');
                    inputStr += i + subDelm + LCaption + subDelm + LFromTo + subDelm + LFrom + subDelm + LTo + subDelm + LCount + subDelm + LSymbolPat;
                }
                else {
                    inputStr += i + subDelm + LCaption + subDelm + LFromTo + subDelm + LFrom + subDelm + LTo + subDelm + LCount + subDelm + hexc(LColor) + subDelm + LHatchPat;
                }
            }
            mapObj.setThemeLegend(themeId, inputStr);

            // refresh pop-up ui
            if (action == 'apply') {
                applyMapPopUpSetting('edittheme');
                di_jq('#popupMapTab').val('themelegend');
            }
        }
        else if (themeType == 'DotDensity') {
            var dotStyle = di_jq('#mapPopDotDtyStyle_0').attr('rel');
            var dotSize = di_jq('#mapPopDotDtySize').val();
            var dotColor = di_jq('#mapPopDotDtyColor').css('background-color');
            var dotValue = di_jq('#mapPopDotDtyValue').val();
            var opacity = di_jq('#mapPopDotDtyOpacityTxt').val();
            //var opacityA = (opacity*256)/100;
            //if(opacity==100) opacityA = 255;

            var argbColor = '#' + decimalToHexString(opacity) + hexc(dotColor).replace("#", "");
            var inputString = dotStyle + mapDataDelemeter + dotSize + mapDataDelemeter + argbColor + mapDataDelemeter + dotValue;

            mapObj.setDotDensityDetails(themeId, inputString);
        }
        else if (themeType == 'Chart') {
            var tmpList = '';
            var settingStr = '';
            var legendStr = '';
            var width = di_jq('#pop_mapChartSliderWTxt').val();
            var height = di_jq('#pop_mapChartSliderHTxt').val();
            var viewdtval = di_jq('#pop_mapChartStViewDtVal').is(":checked");
            var decimals = di_jq('#pop_mapChartStDecimals').val();
            if (di_jq('#pop_mapChartStTimeMR').is(":checked")) var isMrd = true; else var isMrd = false;

            var chartType = di_jq('#popChartType_m').val();
            if (chartType == 'Line') {
                height = di_jq('#pop_mapChartLineSliderTTxt').val();
            }

            settingStr = width + mapDataDelemeter + height + mapDataDelemeter + viewdtval + mapDataDelemeter + decimals + mapDataDelemeter + isMrd;

            var legendCount = di_jq('#mapPopChartLegend').attr('rel');
            for (var i = 0; i < legendCount; i++) {
                if (i > 0)
                    legendStr += mapDataDelemeter;

                legendStr += i + '~~' + di_jq('#pop_mapChartDiColor_' + i).attr('rel');
            }

            // getting timeperiod list if not MRD
            if (isMrd == false) {
                var i = 0;
                di_jq('#diChartTimePeriods input[type="checkbox"]').each(function () {
                    var val = this.value;
                    if (this.checked == false) {
                        if (i == 0) tmpList += val;
                        else tmpList += ',' + val;
                    }
                    i++;
                }); // end
            }

            mapObj.setChartLegendSettings(themeId, settingStr, legendStr, tmpList);
        }

        //redraw parent UI
        redrawThemeDetails(['detail', 'title']);
    }
    else if (selectedTab == 'baselayer') {
        // update base layer setting
        var inputStr = '';
        di_jq('#mapPopTab-1_sec_con input[type="checkbox"]').each(function () {
            var layerID = this.id;
            var visible = true;
            if (di_jq(this).is(':checked')) visible = true; else visible = false;

            inputStr += layerID + '~~' + visible + mapDataDelemeter;
        });
        var ids = di_jq('#mapPopTab-1_sec_con').attr('rel');
        if (ids == null || ids == undefined) ids = '';
        di_jq('#mapPopTab-1_sec_con').attr('rel', '');

        mapObj.setBaseLayerInfo(inputStr, ids);
    }
    else if (selectedTab == 'featurelayer') {
        // update feature layer setting
        var disputed = false;
        var inputStr = '';
        if (di_jq('#flDisputedBnd').is(':checked')) disputed = true; else disputed = false;
        di_jq('#mapPopTab-2_sec_con_tbl input[type="checkbox"]').each(function () {
            var layerID = this.id;
            var visible = true;
            if (di_jq(this).is(':checked')) visible = true; else visible = false;
            var color = di_jq('#ly_color_' + layerID).attr('rel');
            var style = di_jq('#ly_style_' + layerID).attr('rel');
            var size = di_jq('#ly_opszth_' + layerID).val();

            inputStr += layerID + '~~' + visible + '~~' + color + '~~' + style + '~~' + size + mapDataDelemeter;
        });

        mapObj.setFeatureLayerInfo(disputed, inputStr);
    }

    if (action == 'ok') closeMapPropertyPopUp();
}
/* function to load map legend setting */
function updaetLoadMapLegendSettings() {
    // update legend settings only, condition1 - if user changes only legend settings such as breakCount etc (map image will update)
    var brkCount = di_jq('#pop_mapBrkCount').val();
    var brkType = di_jq('#pop_mapBrkType').val();
    var min = di_jq('#pop_mapMin').val();
    var max = di_jq('#pop_mapMax').val();
    var dec = di_jq('#pop_mapDecimals').val();
    var themeId = di_jq('#di_mapTheme').val();
    var inputStr = brkCount + mapDataDelemeter + brkType + mapDataDelemeter + max + mapDataDelemeter + min + mapDataDelemeter + dec;
    mapObj.GetLegendBySettingChange(themeId, inputStr);

    // refresh pop-up ui
    applyMapPopUpSetting('edittheme');
    di_jq('#popupMapTab').val('themelegend');

    //redraw parent UI
    redrawThemeDetails(['detail', 'title']);
}
/* function to change time period on chaning of series in edit theme panel */
function getThemeTimePeriodOfSeries(seriesId, actType) {
    var themeId = di_jq('#di_mapTheme').val(); //popThemeNmTxt_m

    var res = mapObj.getTimePeriodOfSeries(themeId, seriesId);
    var resObj = di_jq.parseJSON(res);
    var tpsList = resObj.tps;
    var sg_name = resObj.sg;

    // change theme name for both add/edit themw
    di_jq('#popThemeNmTxt_m').val(sg_name);

    // time period list in case of edit theme
    if (actType == 'edit') {
        di_jq('#pop_mapThStTimePrMenu').children('option').remove();
        if (tpsList != undefined && tpsList != '' && tpsList.length > 0) {
            for (var i = 0; i < tpsList.length; i++) {
                z('pop_mapThStTimePrMenu').options[i] = new Option(tpsList[i], tpsList[i]);
            }
        }
    }

    /*if(di_jq.trim(res)!='') {
    di_jq('#pop_mapThStTimePrMenu').children('option').remove();
    var resAr = res.split(",");
    if(resAr.length>0) {
    for(var i=0; i<resAr.length; i++) {
    z('pop_mapThStTimePrMenu').options[i] = new Option(resAr[i], resAr[i]);
    } // end for
    } // end if
    } // end if */
}
/* function to add theme of Map */
function updateMapTheme(action) {
    var delemeter = '[****]';

    var themeId = di_jq('#di_mapTheme').val();
    var themeName = di_jq('#popThemeNmTxt_m').val();
    var themeType = di_jq('#popThemeType_m').val();
    var series = di_jq('#popSeries_m').val();
    var chartType = '';
    if (themeType == 'Chart') chartType = di_jq('#popChartType_m').val();
    if (themeType == 'Symbol') di_jq('#mapPopSmoothCl').hide(); else di_jq('#mapPopSmoothCl').show();

    // added on 2 march
    if (themeType == 'Chart' && di_jq("#popSeries_m option").length > 1) {
        var array_of_checked_values = di_jq("#popSeries_m").multiselect("getChecked").map(function () {
            return this.value;
        }).get();
        if (array_of_checked_values == '' || array_of_checked_values == null) {
            alert('Please select series');
            return false;
        }
        else {
            series = array_of_checked_values;
        }
    }

    var inputStr = themeName + delemeter + themeType + delemeter + series + delemeter + chartType + delemeter;

    if (action == 'add') {
        var res = mapObj.addMapTheme(inputStr);
        if (res != false) {
            // redraw theme details
            redrawThemeDetails(['theme', 'title']);
        }
        else {
            return res;
        }
    }
    else if (action == 'edit') {
        var selTP = '';
        var isMrd = '';
        if (themeType != 'Chart') {
            if (di_jq('#pop_mapThStTimePr2').is(":checked")) isMrd = true; else isMrd = false;
            if (isMrd == false) { // get selected time period
                selTP = di_jq('#pop_mapThStTimePrMenu').val();
            }
        }

        var res = mapObj.ediMapTheme(themeId, inputStr, isMrd, selTP);
        if (res != false) {
            // redraw theme details
            di_jq('#di_mapTheme option[value="' + themeId + '"]').text(themeName);
            redrawThemeDetails(['detail', 'title']);
        }
        else {
            return res;
        }
    }
}

/* function to set border style */
function setMapBorderStyle() {
    var delemeter = '[****]';

    var showBdr = di_jq('#popBorderChkbox_m').is(":checked");
    var width = di_jq('#popBorderWidthTxt_m').val();
    var color = di_jq('#popchartBoderColor_m').val();
    var style = di_jq('#mapBdrStl_Selected').attr('rel');
    if (width == '') width = 1;

    if (showBdr) di_jq('#map_borders').attr('checked', true); else di_jq('#map_borders').attr('checked', false);

    mapObj.setBorderStyle(showBdr, width, color, style);
}

/* function to set label style */
function setMapLabelStyle() {
    var delemeter = '[****]';
    var LblStyleStr = '';
    var areaDetail = [];
    var isShowLabel = 'no';

    var showLabel = di_jq('#popLabelChkbox_m').is(":checked");
    if (di_jq('#popLabelAID_m').is(":checked")) areaDetail.push(di_jq('#popLabelAID_m').val());
    if (di_jq('#popLabelANM_m').is(":checked")) areaDetail.push(di_jq('#popLabelANM_m').val());
    if (di_jq('#popLabelDTV_m').is(":checked")) areaDetail.push(di_jq('#popLabelDTV_m').val());
    if (di_jq('#popLabelCNSY_m').is(":checked")) areaDetail.push(di_jq('#popLabelCNSY_m').val());

    var multiRow = di_jq('#popLabelMROW_m').is(":checked");
    var indendent = di_jq('#popLabelINDT_m').is(":checked");
    var whereDE = di_jq('#popLabelSWDE_m').is(":checked");
    var effects = di_jq('#popEffectsChkbox_m').is(":checked");
    var effectType = di_jq('#popEffectType_m').val();
    var effectDepth = di_jq('#popEffectDepth_m').val();
    var effectColor = di_jq('#popEffectColor_m').val();
    var leaderLine = di_jq('#popLeaderChkbox_m').is(":checked");
    var straightLine = di_jq('#popLeaderLnStr_m').val();
    var lineStyle = di_jq('#mapLeaderStylel_Selected').attr('rel');
    var lineWidth = di_jq('#popLeaderWidth_m').val();
    var lineColor = di_jq('#popLeaderColor_m').val();

    var lblFont1 = di_jq('#di_mapLabelFontNm1').val();
    var lblSize1 = di_jq('#di_mapPopSelFont1').val();
    var lblBold1 = di_jq('#poph_fontbold1').val();
    var lblItalic1 = di_jq('#poph_fontitalic1').val();
    var lblULine1 = di_jq('#poph_fontunderline1').val();
    var lblColor1 = di_jq('#poph_dicolormap1').val();
    var lblCaps1 = 'no';
    var lblFont2 = di_jq('#di_mapLabelFontNm2').val();
    var lblSize2 = di_jq('#di_mapPopSelFont2').val();
    var lblBold2 = di_jq('#poph_fontbold2').val();
    var lblItalic2 = di_jq('#poph_fontitalic2').val();
    var lblULine2 = di_jq('#poph_fontunderline2').val();
    var lblColor2 = di_jq('#poph_dicolormap2').val();
    var lblCaps2 = 'no';

    if (showLabel) {
        di_jq('#map_dtlabels').attr('checked', true);
        mapDBNSCalls(true, 'dl');
    }
    else {
        di_jq('#map_dtlabels').attr('checked', false);
        mapDBNSCalls(false, 'dl');
    }

    LblStyleStr = '{"showLabel":"' + showLabel + '", "swde":"' + whereDE + '", "multiRow":"' + multiRow + '", "area":[' + areaDetail + '], "label1":["' + lblFont1 + '","' + lblSize1 + '","' + lblBold1 + '","' + lblItalic1 + '","' + lblULine1 + '","' + lblColor1 + '","' + lblCaps1 + '"], "label2":["' + lblFont2 + '","' + lblSize2 + '","' + lblBold2 + '","' + lblItalic2 + '","' + lblULine2 + '","' + lblColor2 + '","' + lblCaps2 + '"], "showEffect":"' + effects + '", "effectType":"' + effectType + '", "effectDepth":"' + effectDepth + '", "effectColor":"' + effectColor + '", "showLeader":"' + leaderLine + '", "straightLine":"' + straightLine + '", "lineStyle":"' + lineStyle + '", "lineWidth":"' + lineWidth + '", "lineColor":"' + lineColor + '"}';

    if (showLabel) isShowLabel = 'yes';
    mapObj.setLabelStyle(LblStyleStr, isShowLabel);

    if (di_jq('#di_mapSelTitle').val() == 2)
        getMapTitleDetails(2);
}
/* function to check if multi row is selected */
function showHideMapLabel(action, checked) {
    var count = 0;
    if (action == 0) { // data label action
        if ((di_jq('#popLabelAID_m').is(":checked") || di_jq('#popLabelANM_m').is(":checked")) && (di_jq('#popLabelDTV_m').is(":checked") || di_jq('#popLabelCNSY_m').is(":checked"))) count++;

        //if(di_jq('#popLabelAID_m').is(":checked")) count++;
        //if(di_jq('#popLabelANM_m').is(":checked")) count++;
        //if(di_jq('#popLabelDTV_m').is(":checked")) count++;
        //if(di_jq('#popLabelCNSY_m').is(":checked")) count++;

        if (count > 0) {
            di_jq('#popLabelMROW_m').attr('disabled', false);
            if (di_jq('#popLabelMROW_m').is(':checked') == true) {
                di_jq('#popLabelINDT_m').attr('disabled', false);
                di_jq('#mapPopLblRowSeptor').show();
                di_jq('#mapPopLblRow2').show();
            }
            else {
                di_jq('#popLabelINDT_m').attr('disabled', true);
                di_jq('#mapPopLblRowSeptor').hide();
                di_jq('#mapPopLblRow2').hide();
                mapLblRowClick(1);
            }
        }
        else {
            di_jq('#popLabelMROW_m').attr('disabled', true);
            di_jq('#popLabelMROW_m').attr('checked', false);
            di_jq('#popLabelINDT_m').attr('disabled', true);
            di_jq('#mapPopLblRowSeptor').hide();
            di_jq('#mapPopLblRow2').hide();
            mapLblRowClick(1);
        }
    }
    else if (action == 1) { // multi row action
        if (checked == true) {
            di_jq('#popLabelINDT_m').attr('disabled', false);
            di_jq('#mapPopLblRowSeptor').show();
            di_jq('#mapPopLblRow2').show();
        }
        else {
            di_jq('#popLabelINDT_m').attr('disabled', true);
            di_jq('#mapPopLblRowSeptor').hide();
            di_jq('#mapPopLblRow2').hide();
            mapLblRowClick(1);
        }
    }
}
/* function to export map */
function mapImageOptions(show) {
    if (show == true) {
        //mapDownload('image')
        ApplyMaskingDiv();
        //Set close button at right corner of popup div
        SetCloseButtonInPopupDiv(di_jq('#dv_container_popup_download_mapimage'), 'mapImageOptions');
        di_jq('#dv_container_popup_download_mapimage').show('slow');
        GetWindowCentered(z('dv_container_popup_download_mapimage'), 350, 400);

        di_jq('#mapDwnld_width').val(di_jq('#visChartDiv').width());
        di_jq('#mapDwnld_height').val(di_jq('#visChartDiv').height());
    }
    else {
        RemoveMaskingDiv();
        di_jq('#dv_container_popup_download_mapimage').hide('slow');
    }
}


function mapDownloadWithTp(type) {
    var delemeter = '[****]';
    var themeId = di_jq('#di_mapTheme').val();
    var themeName = SliderthArrayData[themeId][6];
    var themeType = SliderthArrayData[themeId][0];
    var seriesId = SliderthArrayData[themeId][1];
    var chartType = SliderthArrayData[themeId][7];
    var inputStr = themeName + delemeter + themeType + delemeter + seriesId + delemeter + chartType + delemeter;
    var mapfilename = 'map';
    var mapTitle = di_jq('#mapTitle').html();
    if (mapTitle != '' && mapTitle != null && mapTitle != undefined)
        mapfilename = mapTitle;

    var form = document.createElement("form");
    form.setAttribute("method", "post");
    form.setAttribute("action", "ExportMap.aspx");

    // export type
    var filenameField = document.createElement("input");
    filenameField.setAttribute("type", "hidden");
    filenameField.setAttribute("name", "ExportType");
    filenameField.setAttribute("value", type);
    form.appendChild(filenameField);

    // filename
    var filetypeField = document.createElement("input");
    filetypeField.setAttribute("type", "hidden");
    filetypeField.setAttribute("name", "filename");
    filetypeField.setAttribute("value", mapfilename);
    form.appendChild(filetypeField);

    // db nid
    var dbnidField = document.createElement("input");
    dbnidField.setAttribute("type", "hidden");
    dbnidField.setAttribute("name", "dbnid");
    dbnidField.setAttribute("value", z('hdbnid').value);
    form.appendChild(dbnidField);
    // lng code
    var dbnidField = document.createElement("input");
    dbnidField.setAttribute("type", "hidden");
    dbnidField.setAttribute("name", "langCode");
    dbnidField.setAttribute("value", z('hlngcodedb').value);
    form.appendChild(dbnidField);
    var source = getSourceList();
    var sourcetypeField = document.createElement("input");
    sourcetypeField.setAttribute("type", "hidden");
    sourcetypeField.setAttribute("name", "source");
    sourcetypeField.setAttribute("value", source);
    form.appendChild(sourcetypeField);

    var themeIdField = document.createElement("input");
    themeIdField.setAttribute("type", "hidden");
    themeIdField.setAttribute("name", "themeId");
    themeIdField.setAttribute("value", themeId);
    form.appendChild(themeIdField);

    var inputAryField = document.createElement("input");
    inputAryField.setAttribute("type", "hidden");
    inputAryField.setAttribute("name", "inputStr");
    inputAryField.setAttribute("value", inputStr);
    form.appendChild(inputAryField);

    if (type == 'image') {
        if (di_jq('#mapDwnld_width').val() == '' || di_jq('#mapDwnld_height').val() == '') {
            di_jq('#mapDwnld_width').focus();
            return false;
        }
        // create for legend
        var showLegendField = document.createElement("input");
        showLegendField.setAttribute("type", "hidden");
        showLegendField.setAttribute("name", "includelegend");
        if (di_jq('#mapDwnld_legend').is(':checked'))
            showLegendField.setAttribute("value", true);
        else
            showLegendField.setAttribute("value", false);
        form.appendChild(showLegendField);

        // create for width
        var widthField = document.createElement("input");
        widthField.setAttribute("type", "hidden");
        widthField.setAttribute("name", "width");
        widthField.setAttribute("value", di_jq('#mapDwnld_width').val());
        form.appendChild(widthField);

        // create for height
        var heightField = document.createElement("input");
        heightField.setAttribute("type", "hidden");
        heightField.setAttribute("name", "height");
        heightField.setAttribute("value", di_jq('#mapDwnld_height').val());
        form.appendChild(heightField);

        // create for image type
        var typeField = document.createElement("input");
        typeField.setAttribute("type", "hidden");
        typeField.setAttribute("name", "extension");
        typeField.setAttribute("value", di_jq('#mapDwnld_type').val());
        form.appendChild(typeField);

        mapImageOptions(false);
    }

    document.body.appendChild(form);
    form.submit();
}


/* function to export map */
function mapDownload(type) {
    if (((type == 'excel') || (type == 'image')) && (di_jq('#mapTimeLine')[0] != null) && (di_jq('#mapTimeLine')[0].checked)) {
        mapDownloadWithTp(type);
        return;
    }
    var mapfilename = 'map';
    var mapTitle = di_jq('#mapTitle').html();
    if (mapTitle != '' && mapTitle != null && mapTitle != undefined)
        mapfilename = mapTitle;

    var form = document.createElement("form");
    form.setAttribute("method", "post");
    form.setAttribute("action", "ExportMap.aspx");

    // export type
    var filenameField = document.createElement("input");
    filenameField.setAttribute("type", "hidden");
    filenameField.setAttribute("name", "ExportType");
    filenameField.setAttribute("value", type);
    form.appendChild(filenameField);

    // filename
    var filetypeField = document.createElement("input");
    filetypeField.setAttribute("type", "hidden");
    filetypeField.setAttribute("name", "filename");
    filetypeField.setAttribute("value", mapfilename);
    form.appendChild(filetypeField);

    // db nid
    var dbnidField = document.createElement("input");
    dbnidField.setAttribute("type", "hidden");
    dbnidField.setAttribute("name", "dbnid");
    dbnidField.setAttribute("value", z('hdbnid').value);
    form.appendChild(dbnidField);

    // lng code
    var dbnidField = document.createElement("input");
    dbnidField.setAttribute("type", "hidden");
    dbnidField.setAttribute("name", "langCode");
    dbnidField.setAttribute("value", z('hlngcodedb').value);
    form.appendChild(dbnidField);

    // source
    if (type == 'excel') {
        var source = getSourceList();
        var sourcetypeField = document.createElement("input");
        sourcetypeField.setAttribute("type", "hidden");
        sourcetypeField.setAttribute("name", "source");
        sourcetypeField.setAttribute("value", source);
        form.appendChild(sourcetypeField);
    }

    if (type == 'image') {
        if (di_jq('#mapDwnld_width').val() == '' || di_jq('#mapDwnld_height').val() == '') {
            di_jq('#mapDwnld_width').focus();
            return false;
        }
        // create for legend
        var showLegendField = document.createElement("input");
        showLegendField.setAttribute("type", "hidden");
        showLegendField.setAttribute("name", "includelegend");
        if (di_jq('#mapDwnld_legend').is(':checked'))
            showLegendField.setAttribute("value", true);
        else
            showLegendField.setAttribute("value", false);
        form.appendChild(showLegendField);

        // create for width
        var widthField = document.createElement("input");
        widthField.setAttribute("type", "hidden");
        widthField.setAttribute("name", "width");
        widthField.setAttribute("value", di_jq('#mapDwnld_width').val());
        form.appendChild(widthField);

        // create for height
        var heightField = document.createElement("input");
        heightField.setAttribute("type", "hidden");
        heightField.setAttribute("name", "height");
        heightField.setAttribute("value", di_jq('#mapDwnld_height').val());
        form.appendChild(heightField);

        // create for image type
        var typeField = document.createElement("input");
        typeField.setAttribute("type", "hidden");
        typeField.setAttribute("name", "extension");
        typeField.setAttribute("value", di_jq('#mapDwnld_type').val());
        form.appendChild(typeField);

        mapImageOptions(false);
    }

    document.body.appendChild(form);
    form.submit();
}
/* function to share map */
function shareMap() {
    var url = mapObj.getMapImagePath();
    if (url != '') SocialSharing("You should check this out!", "DevInfo", url, url, "Powered By DevInfo", "This is dummy description for testing the facebook sharing feature."); else alert('Map image url is empty!');
}
/* function to delete Map theme */
function deleteMapTheme() {
    if (confirm("Are you sure, you want to delete theme?")) {
        var themeId = di_jq('#di_mapTheme').val();
        mapObj.deleteMapTheme(themeId);

        // redraw theme details
        redrawThemeDetails(['theme', 'title']);
    }
}
/* function to change visibliy of Map theme */
function visibilityMapTheme(checked) {
    var themeId = di_jq('#di_mapTheme').val();
    mapObj.visibleMapTheme(themeId, checked);
}

/* function to redraw series, theme and title */
function redrawThemeDetails(redrawArray) {

    if (jQuery.inArray("series", redrawArray) > -1) {
        // create Series UI
        createSeriesUi();
    }

    if (jQuery.inArray("theme", redrawArray) > -1) {
        // redraw theme details
        createThemeUi();
    }

    if (jQuery.inArray("detail", redrawArray) > -1) {
        // redraw theme details
        var themeID = di_jq('#di_mapTheme').val();
        var thArrayData = createArrColThemeData(false, themeID);
        changeThemeDetails(thArrayData, themeID);
    }

    if (jQuery.inArray("title", redrawArray) > -1) {
        // change title/datalabel/disclamer
        var titleType = di_jq('#di_mapSelTitle').val();
        getMapTitleDetails(titleType);
    }

    // added 23 may for change theme text only of selected theme id (this happens only for when series get changed)
    if (jQuery.inArray("thname", redrawArray) > -1) {
        var themeID = di_jq('#di_mapTheme').val();
        var themeName = mapObj.getSeriesThemeName(themeID);
        di_jq('#di_mapTheme option:selected').text(themeName);
    }

    // added 23 Aug to reset single theme series drop down box on map body (Select indicator)
    if (jQuery.inArray("sthseries", redrawArray) > -1) {
        if (mapviewtype == 'single') {
            di_jq('#mapSeriesDropDwnbox').children('option').remove();
            var srIdObj = z('mapSeriesDropDwnbox');
            var i = 0;
            di_jq('#di_mapSeries').children('option').each(function () {
                var opval = this.value;
                var optxt = di_jq(this).text();

                srIdObj.options[i] = new Option(optxt, opval);
                i++;
            }); //
            if (i > 1) {
                di_jq('#mapSeriesDropDwnCon').show();
            }
            else {
                di_jq('#mapSeriesDropDwnCon').hide();
            }
            var seriesID = di_jq('#di_mapSeries').val();
            di_jq('#mapSeriesDropDwnbox').val(seriesID);
        } // end if
    }
}
/* function to toggle map border width */
function toggleMapBorder(act) {
    var widthVal = Number(di_jq('#popBorderWidthTxt_m').val());

    if (act == 1) {
        if (widthVal >= 1) {
            widthVal = widthVal + .5;
        }
        else if (widthVal >= .1) {
            widthVal = widthVal + .1;
        }
        else {
            widthVal = widthVal + .01;
        }
    }
    else {
        if (widthVal > 0 && widthVal <= .1) {
            widthVal = widthVal - .01;
        }
        if (widthVal > .1 && widthVal <= 1) {
            widthVal = widthVal - .1;
        }
        else if (widthVal > 1) {
            widthVal = widthVal - .5;
        }
    }
    widthVal = widthVal.toFixed(2);
    di_jq('#popBorderWidthTxt_m').val(widthVal);
}


/*Prepair Data for radar chart*/
function di_prepairDataForRadar(vc_data) {
    var temp = { categoryCollection: [],
        seriesCollection: []
    }
    var catArray = vc_data.categoryCollection;
    var serArray = vc_data.seriesCollection;
    for (var i = 0; i < serArray.length; i++) {
        var seriesItem = serArray[i];
        var name = ReplaceAll(seriesItem.name, "{@@}", ",");
        temp.categoryCollection.push(name);
    }
    for (var i = 0; i < catArray.length; i++) {
        var newSeriesItem = new Object();
        var name = ReplaceAll(catArray[i], "{@@}", ",");
        newSeriesItem.name = name;
        newSeriesItem.data = [];
        for (var j = 0; j < serArray.length; j++) {
            var newDataValue = serArray[j].data[i];
            newSeriesItem.data.push(newDataValue);
        }
        temp.seriesCollection.push(newSeriesItem);
    }
    return temp;
}

function postSwfImage(rawImage, type, fileName, tableData, titleText, subtitleText, sourceText, keywordsText) {
    var form = document.createElement("form");
    form.setAttribute("method", "post");
    form.setAttribute("action", "exportSwfImage.aspx");

    // export data
    var imgdataField = document.createElement("input");
    imgdataField.setAttribute("type", "hidden");
    imgdataField.setAttribute("name", "chartImg");
    imgdataField.setAttribute("value", rawImage);
    form.appendChild(imgdataField);

    // Type	
    var typeField = document.createElement("input");
    typeField.setAttribute("type", "hidden");
    typeField.setAttribute("name", "chartType");
    typeField.setAttribute("value", type);
    form.appendChild(typeField);

    // Filename
    var filenameField = document.createElement("input");
    filenameField.setAttribute("type", "hidden");
    filenameField.setAttribute("name", "filename");
    filenameField.setAttribute("value", fileName);
    form.appendChild(filenameField);

    // title
    var titleField = document.createElement("input");
    titleField.setAttribute("type", "hidden");
    titleField.setAttribute("name", "title");
    titleField.setAttribute("value", titleText);
    form.appendChild(titleField);

    // subtitle
    var subtitleField = document.createElement("input");
    subtitleField.setAttribute("type", "hidden");
    subtitleField.setAttribute("name", "subtitle");
    subtitleField.setAttribute("value", subtitleText);
    form.appendChild(subtitleField);

    // tableData
    var tableDataField = document.createElement("input");
    tableDataField.setAttribute("type", "hidden");
    tableDataField.setAttribute("name", "tableData");
    tableDataField.setAttribute("value", tableData);
    form.appendChild(tableDataField);

    // source
    var sourceField = document.createElement("input");
    sourceField.setAttribute("type", "hidden");
    sourceField.setAttribute("name", "source");
    sourceField.setAttribute("value", sourceText);
    form.appendChild(sourceField);

    // keywords
    var keywordsField = document.createElement("input");
    keywordsField.setAttribute("type", "hidden");
    keywordsField.setAttribute("name", "keywords");
    keywordsField.setAttribute("value", keywordsText);
    form.appendChild(keywordsField);

    document.body.appendChild(form);
    form.submit();
}

/* Embed swf component into html page
Parameters: swfFilePath - swf component's relative path.
divId - div control id where swf componet is going to embed.
height - swf component's height.
width - swf component's width.
configPath - configuration file path for swf component. */
var SharedRadarSettings;
function LoadSWF(uid, divId, height, width) {
    var swfVersionStr;
    var xiSwfUrlStr;
    var params = {};
    var attributes = {};
    var flashvars = {};
    try {
        // For version detection, set to min. required Flash Player version, or 0 (or 0.0.0), for no version detection. 
        swfVersionStr = "10.0.0";
        // To use express install, set to playerProductInstall.swf, otherwise the empty string.
        xiSwfUrlStr = "playerProductInstall.swf";
        // Set param variable
        params.quality = "high";
        params.bgcolor = "#ffffff";
        params.allowscriptaccess = "sameDomain";
        params.allowfullscreen = "true";
        params.wmode = "transparent";
        var completeDivId = divId;
        //Set arrtribute variable
        attributes.id = completeDivId;
        attributes.align = "left";
        flashvars.id = completeDivId;
        flashvars.uid = uid;
        // Embed Swf in page
        swfobject.embedSWF(SharedRadarSettings.swfURL, completeDivId, width, height, swfVersionStr, xiSwfUrlStr, flashvars, params, attributes);
    }
    catch (err) { }
}

/*This function is called from swf after loading completed swf into html.We get swf id here*/
function radarLoadingComplete(swfId, uid) {
    //alert(swfId+"\n"+uid);	
    var radarObject = swfobject.getObjectById(swfId);
    applySettingsToRadar(radarObject);
}

function applySettingsToRadar(radarObject) {
    radarObject.dataToRadar(SharedRadarSettings.data);
    radarObject.SetChartBorderVisible(SharedRadarSettings.chart.borderVisiblity);
    radarObject.SetChartBorderWidth(SharedRadarSettings.chart.borderWidth);
    radarObject.SetChartBorderRadius(SharedRadarSettings.chart.borderRadius);
    radarObject.SetChartBorderColor(SharedRadarSettings.chart.borderColor);
    radarObject.SetBackgroundColor(SharedRadarSettings.chart.backgroundColor);
    radarObject.SetGridVisibility(SharedRadarSettings.chart.gridlines);
    radarObject.SetNoteLabel(SharedRadarSettings.note.text);

    radarObject.SetNoteLabelFontSize(SharedRadarSettings.note.style.fontSize);
    radarObject.SetNoteLabelColor(SharedRadarSettings.note.style.color);
    radarObject.SetNoteLabelWeight(SharedRadarSettings.note.style.fontWeight);
    radarObject.SetNoteLabelDecoration(SharedRadarSettings.note.style.textDecoration);
    radarObject.SetNoteLabelStyle(SharedRadarSettings.note.style.italic);
    radarObject.SetNoteLabelX(SharedRadarSettings.note.position.x);
    radarObject.SetNoteLabelY(SharedRadarSettings.note.position.y);

    radarObject.SetSourceLabel(SharedRadarSettings.source.text);
    radarObject.SetSourceLabelFontSize(SharedRadarSettings.source.style.fontSize);
    radarObject.SetSourceLabelColor(SharedRadarSettings.source.style.color);
    radarObject.SetSourceLabelWeight(SharedRadarSettings.source.style.fontWeight);
    radarObject.SetSourceLabelDecoration(SharedRadarSettings.source.style.textDecoration);
    radarObject.SetSourceLabelStyle(SharedRadarSettings.source.style.italic);
    radarObject.SetNoteLabelX(SharedRadarSettings.source.position.x);
    radarObject.SetNoteLabelY(SharedRadarSettings.source.position.y);

    radarObject.SetTitle(SharedRadarSettings.title.text);
    radarObject.SetTitleFontSize(SharedRadarSettings.title.style.fontSize);
    radarObject.SetTitleColor(SharedRadarSettings.title.style.color);
    radarObject.SetTitleWeight(SharedRadarSettings.title.style.fontWeight);
    radarObject.SetTitleDecoration(SharedRadarSettings.title.style.textDecoration);
    radarObject.SetTitleStyle(SharedRadarSettings.title.style.italic);
    radarObject.SetSubTitle(SharedRadarSettings.subtitle.text);
    radarObject.SetSubTitleFontSize(SharedRadarSettings.subtitle.style.fontSize);
    radarObject.SetSubTitleColor(SharedRadarSettings.subtitle.style.color);
    radarObject.SetSubTitleWeight(SharedRadarSettings.subtitle.style.fontWeight);
    radarObject.SetSubTitleDecoration(SharedRadarSettings.subtitle.style.textDecoration);
    radarObject.SetSubTitleStyle(SharedRadarSettings.subtitle.style.italic);
    radarObject.SetLegendVisible(SharedRadarSettings.legend.enabled);
    radarObject.SetLegendBorderWidth(SharedRadarSettings.legend.borderWidth);
    radarObject.SetLegendBorderColor(SharedRadarSettings.legend.borderColor);
    radarObject.SetLegendBorderRadius(SharedRadarSettings.legend.borderRadius);
    radarObject.SetLegendBackgroundcolor(SharedRadarSettings.legend.backgroundColor);
    radarObject.SetLegendBorderVisible(SharedRadarSettings.legend.borderVisibility);
    radarObject.SetLegendDirection(SharedRadarSettings.legend.position);
    radarObject.SetSeriesType(SharedRadarSettings.option.seriesType);
    radarObject.SetRadialAxis(SharedRadarSettings.option.radialAxis);
    radarObject.SetForm(SharedRadarSettings.option.form);
    radarObject.SetChartType(SharedRadarSettings.option.chartType);
    radarObject.SetThemeColor(SharedRadarSettings.colors);
    radarObject.render();
}

function openGalleryInputPop() {
    if (OAT.getSelfInstance() == null)
        return;
    var userLoginId;
    if (z('hLoggedInUserNId').value != "") {
        userLoginId = z('hLoggedInUserNId').value.split("|")[0];
    }

    if (userLoginId == "" || z('hLoggedInUserNId').value == "") {
        di_jq('#divShowLoadingText').html(langLoadingGallery);
        di_jq('#divShowLoading').show();
        z("loginIframe").style.display = "none";
        showHideComponent("login");
        //di_jq("#loginPopup").show();
        //PostDataToLogin('frm_dv', 'POST', 'New');
    }
    else {
        var htmlui = "";
        di_jq('#dv_sec_pop').html('<h1>' + z("lang_ppup_SavePres").value + '</h1><h4>' + z("lang_ppup_SavePresDesc").value + '</h4>');
        /* change left */
        htmlui = '<ul class="left_button"><li><a id="popGallery" href="javascript:void(0)" class="selected">' + z("lang_ppup_Gallery").value + '</a></li></ul>';
        di_jq('#dv_sec_left').html(htmlui);
        /* change right */
        var chartObj = di_getChartObject(chartVisualizationDecided);
        if (chartObj == undefined) {
            var NameAsTitle = z('dataTitle').innerHTML;
        }
        else {
            if (chartObj.options != undefined) {
                if (chartObj.options.title.text == '' && chartObj.options.subtitle.text == '') {
                    var NameAsTitle = '';
                }
                else {
                    var NameAsTitle = chartObj.options.title.text + "," + chartObj.options.subtitle.text;
                }
            }
            else {
                var NameAsTitle = z('dataTitle').innerHTML;
            }
        }
        if (NameAsTitle != "") {
            NameAsTitle = removeSpecialCharacters(NameAsTitle)
            NameAsTitle = NameAsTitle.trim();
        }
        htmlui = '<div style="padding:10px 0 0 20px;"><fieldset class="fieldset"><legend id="popfieldSet">' + z("lang_ppup_Gallery").value + '</legend></fieldset></div><div id="popTabbdr_sec" class="panelContainer" style="display:block;"><table width="90%" cellpadding="0" cellspacing="5"><tr><td colspan="2" height="20" valign="top"></td></tr><tr><td>' + z("lang_ppup_Name").value + '<span style="color:red;">*<span></td><td><input id="popPresentationName" value="' + NameAsTitle + '" type="textbox" onkeyup="validateTextLength(this)" style="width:300px;"/></td></tr><tr><td>' + z("lang_ppup_Keywords").value + '</td><td><input id="popPresentationKeyword" style="width:300px;" type="textbox"/></td></tr><tr><td valign="top">' + z("lang_ppup_Desc").value + '</td><td><textarea id="popPresentationDesc" rows="5" cols="7" style="width:300px;height:180px;resize:none;"/></td></tr></table></div>';
        di_jq('#dv_sec_right').html(htmlui);


        di_jq('#btnApply').hide();
        ApplyMaskingDiv();
        //Set close button at right corner of popup div
        SetCloseButtonInPopupDiv(di_jq('#dv_container_popup'), 'closeVisPropertyPopUp');
        di_jq('#dv_container_popup').show('slow');
        GetWindowCentered(z('dv_container_popup'), 580, 440);
    }
}

function validateTextLength(obj) {
    if (obj.value.length > 255) {
        obj.value = obj.value.substr(0, 255);
        return;
    }
}

function PreventCharacters(obj, event) {
    var e = event.keyCode || event.which;
    if (e.toString() == "188") {
        obj.value = obj.value.replace(",", "");
        return;
    }
}

function postGalleryDetails() {

    var delemeter = "[****]";
    var categoryNid = "1";
    var userLoginId = "";
    var paramValue = "";
    var exists = "0";
    if (z('hLoggedInUserNId').value != "") {
        userLoginId = z('hLoggedInUserNId').value.split("|")[0];
    }
    var selectedAreas = selectedItemsForGallery(z("langAreaText").value, 0);
    var selectedIndicators = selectedItemsForGallery(z("langIndicatorText").value, 1);
    var presentationKeywords = z('popPresentationKeyword').value;
    if (presentationKeywords != "") {
        presentationKeywords = ReplaceAll(presentationKeywords, ",", "{@@}")
    }
    var selectAreaFromQds = z("hselarea").value;
    if (chartVisualizationDecided == "map" || chartVisualizationDecided == "mapm" || chartVisualizationDecided == "map2d" || chartVisualizationDecided == "map3d") {
        paramValue = z('hdbnid').value + delemeter + z('hlngcode').value + delemeter + userLoginId + delemeter + categoryNid + delemeter + selectedIndicators + delemeter + selectedAreas + delemeter + presentationKeywords + delemeter + z('popPresentationName').value + delemeter + z('popPresentationDesc').value + delemeter + "M" + delemeter + "map" + delemeter + selectAreaFromQds;
    }
    else if (chartVisualizationDecided == "table") {
        paramValue = z('hdbnid').value + delemeter + z('hlngcode').value + delemeter + userLoginId + delemeter + categoryNid + delemeter + selectedIndicators + delemeter + selectedAreas + delemeter + presentationKeywords + delemeter + z('popPresentationName').value + delemeter + z('popPresentationDesc').value + delemeter + "T" + delemeter + chartVisualizationDecided + delemeter + selectAreaFromQds + delemeter + "" + delemeter + getTableDetailsForGallery() + delemeter + z("dataTitle").innerHTML + delemeter + z("dataSubTitle").innerHTML + delemeter + getSourceList() + delemeter + " " + delemeter + getOATtableContent();
    }
    else if (chartVisualizationDecided == "treemap") {
        paramValue = z('hdbnid').value + delemeter + z('hlngcode').value + delemeter + userLoginId + delemeter + categoryNid + delemeter + selectedIndicators + delemeter + selectedAreas + delemeter + presentationKeywords + delemeter + z('popPresentationName').value + delemeter + z('popPresentationDesc').value + delemeter + "G" + delemeter + chartVisualizationDecided + delemeter + selectAreaFromQds + delemeter + createHTMLForSharedTreemap('gallery') + delemeter + getTreemapSettings(chartVisualizationDecided) + delemeter + z("di_vc_title").innerHTML + delemeter + "" + delemeter + getSourceList() + delemeter + " " + delemeter + getOATtableContent();
    }
    else if (chartVisualizationDecided == "cloud") {
        paramValue = z('hdbnid').value + delemeter + z('hlngcode').value + delemeter + userLoginId + delemeter + categoryNid + delemeter + selectedIndicators + delemeter + selectedAreas + delemeter + presentationKeywords + delemeter + z('popPresentationName').value + delemeter + z('popPresentationDesc').value + delemeter + "G" + delemeter + chartVisualizationDecided + delemeter + selectAreaFromQds + delemeter + createHTMLForSharedCloud('gallery') + delemeter + getCloudSettings(chartVisualizationDecided) + delemeter + z("di_vc_title").innerHTML + delemeter + "" + delemeter + getSourceList() + delemeter + " " + delemeter + getOATtableContent();
    }
    else if (chartVisualizationDecided == "radar") {
        paramValue = z('hdbnid').value + delemeter + z('hlngcode').value + delemeter + userLoginId + delemeter + categoryNid + delemeter + selectedIndicators + delemeter + selectedAreas + delemeter + presentationKeywords + delemeter + z('popPresentationName').value + delemeter + z('popPresentationDesc').value + delemeter + "G" + delemeter + chartVisualizationDecided + delemeter + selectAreaFromQds + delemeter + di_RadarImageBitString(chartVisualizationDecided) + delemeter + di_getRadarCurrentSettings(chartVisualizationDecided) + delemeter + di_GetRadarTitle(chartVisualizationDecided) + delemeter + di_GetRadarSubTitle(chartVisualizationDecided) + delemeter + getSourceList() + delemeter + " " + delemeter + getOATtableContent();
    }
    else if (chartVisualizationDecided == "scatter3d") {
        paramValue = z('hdbnid').value + delemeter + z('hlngcode').value + delemeter + userLoginId + delemeter + categoryNid + delemeter + selectedIndicators + delemeter + selectedAreas + delemeter + presentationKeywords + delemeter + z('popPresentationName').value + delemeter + z('popPresentationDesc').value + delemeter + "G" + delemeter + chartVisualizationDecided + delemeter + selectAreaFromQds + delemeter + di_scatter3dImageBitString(chartVisualizationDecided) + delemeter + di_getScatter3dCurrentSettings(chartVisualizationDecided) + delemeter + "Title" + delemeter + "Subtitle" + delemeter + getSourceList() + delemeter + " " + delemeter + getOATtableContent();
    }
    else {
       
      
        paramValue = z('hdbnid').value + delemeter + z('hlngcode').value + delemeter + userLoginId + delemeter + categoryNid + delemeter + selectedIndicators + delemeter + selectedAreas + delemeter + presentationKeywords + delemeter + z('popPresentationName').value + delemeter + z('popPresentationDesc').value + delemeter + "G" + delemeter + chartVisualizationDecided + delemeter + selectAreaFromQds + delemeter + getImageData(chartVisualizationDecided) + delemeter + getVisualizerSettings(chartVisualizationDecided, chartVisualizationDecided) + delemeter + getTitleForGallery(chartVisualizationDecided, chartVisualizationDecided) + delemeter + getSubTitleForGallery(chartVisualizationDecided, chartVisualizationDecided) + delemeter + getSourceList() + delemeter + " " + delemeter + getOATtableContent();
       // //alert(getImageData(chartVisualizationDecided));
       //// document.write("<script type='text/javascript' src='" + di_version_url + "js/jquery-latest.js'></script>");
       // var options = options || {};
       // var svgim = getImageData(chartVisualizationDecided);
       // alert(svgim);
       // svgAsDataUri(svgim[0], options, function (uri) {

       //     var image = new Image();
       //     image.src = uri;
       //     alert(image.src);
       //     //return image.src;
       // });
        //saveSvgAsPng(getImageData(chartVisualizationDecided)[0], 'test.png');
    }
    var htmlResp = di_jq.ajax({
        type: "POST",
        url: "Callback.aspx",
        data: { 'callback': 34, 'param1': paramValue },
        async: false,
        success: function (data) {
            try {
                if (data == "-999") {
                    exists = "1";
                    alert("Duplicate Gallery Name");
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
    di_jq('#divShowLoading').hide();
    if (exists == "0") {
        closeVisPropertyPopUp();
    }
}
//SVG TO PNG Converssion

//function handleFileSelect(evt,) {
  
//    var $el = $('#filereader');
//    var files = evt.target.files;
//    for (var i = 0, f; f = files[i]; i++) {
//        var reader = new FileReader();
//        reader.onload = (function (file) {
//            return function (e) {
//                $el.find('.load-target').html(e.target.result);
//                svgAsDataUri($el.find('.load-target svg')[0], null, function (uri) {
//                    //$el.find('input').hide()
//                    $el.find('.preview').html('<img src="' + uri + '" />');
//                });
//                //$el.find('.save').click(function () {
//                    saveSvgAsPng($el.find('.load-target svg')[0], 'test.png');
//                //});
//            }
//        })(f);
//        reader.readAsText(f);
//    }
//}



// Get Selected Indicators
function getSelectedIndicators() {
    var indJsonStr = z('hselindo').value;
    var indJsonObj = di_jq.parseJSON(indJsonStr);
    var indArray = indJsonObj.iu;
    var selectedInd = "";
    for (var i = 0; i < indArray.length; i++) {
        var indName = indArray[i].split("||")[1].split("~")[0];
        if (selectedInd.indexOf(indName) == -1) {
            selectedInd += "{@@}" + indName;
        }
    }
    return selectedInd;
}
// Get Selected Areas
function getSelectedArea() {
    var selectedAreas = "";
    var areaJsonStr = z('hselareao').value;
    var areaJsonObj = di_jq.parseJSON(areaJsonStr);
    for (var areaObj in areaJsonObj.area) {
        var arrCurrentLevelAreas = areaJsonObj.area[areaObj];
        for (var i = 0; i < arrCurrentLevelAreas.length; i++) {
            selectedAreas += "{@@}" + arrCurrentLevelAreas[i].split("||")[1];
        }
    }
    return selectedAreas;
}
function getImageData(chartType) {
    if (chartType == "map2d" || chartType == "map3d")
        return;
    var imgData = "";
    if (chartType != "radar") {
        //var svg = z('visChartDiv').innerHTML;
        var svg = di_jq(".highcharts-container").html();
        svg = svg
			.replace(/zIndex="[^"]+"/g, '')
			.replace(/isShadow="[^"]+"/g, '')
			.replace(/symbolName="[^"]+"/g, '')
			.replace(/jQuery[0-9]+="[^"]+"/g, '')
			.replace(/isTracker="[^"]+"/g, '')
			.replace(/url\([^#]+#/g, 'url(#')
			.replace(/<svg /, '<svg xmlns:xlink="http://www.w3.org/1999/xlink" ')
			.replace(/ href=/g, ' xlink:href=')
        /*.replace(/preserveAspectRatio="none">/g, 'preserveAspectRatio="none"/>')*/
        /* This fails in IE < 8
        .replace(/([0-9]+)\.([0-9]+)/g, function(s1, s2, s3) { // round off to save weight
        return s2 +'.'+ s3[0];
        })*/

        // Replace HTML entities, issue #347
			.replace(/&nbsp;/g, '\u00A0') // no-break space
			.replace(/&shy;/g, '\u00AD') // soft hyphen

        // IE specific
			.replace(/id=([^" >]+)/g, 'id="$1"')
			.replace(/class=([^" ]+)/g, 'class="$1"')
			.replace(/ transform /g, ' ')
			.replace(/:(path|rect)/g, '$1')
			.replace(/<img ([^>]*)>/gi, '<image $1 />')
			.replace(/<\/image>/g, '') // remove closing tags for images as they'll never have any content
			.replace(/<image ([^>]*)([^\/])>/gi, '<image $1$2 />') // closes image tags for firefox
			.replace(/width=(\d+)/g, 'width="$1"')
			.replace(/height=(\d+)/g, 'height="$1"')
			.replace(/hc-svg-href="/g, 'xlink:href="')
			.replace(/style="([^"]+)"/g, function (s) {
			    return s.toLowerCase();
			});

        // IE9 beta bugs with innerHTML. Test again with final IE9.
        svg = svg.replace(/(url\(#highcharts-[0-9]+)&quot;/g, '$1')
			.replace(/&quot;/g, "'");
        if (svg.match(/ xmlns="/g).length === 2) {
            svg = svg.replace(/xmlns="[^"]+"/, '');
        }
        imgData = svg;
        imgData = imgData.replace(/NaN/g, "0");
    }
    else {
        imgData = di_RadarImageBitString(chartVisualizationDecided);
    }

    return imgData;
}

function getVisualizerSettings(chartType, id) {
    var chartSettings;
    if (chartType == "map2d" || chartType == "map3d")
        return;
    if (chartType != "radar") {
        var chartObj = di_getChartObject(id);
        var chartInput = chartObj.options;
        chartSettings = JSON.stringify(chartInput);
    }
    else {
        chartSettings = di_getRadarCurrentSettings(id);
    }
    return chartSettings;
}

function getTitleForGallery(chartType, uid) {
    var titleText;
    if (chartType == "map2d" || chartType == "map3d")
        return;
    if (chartType == "radar") {
        titleText = di_GetRadarTitle(uid);
    }
    else if (chartType == "table") {
        titleText = z('dataTitle').innerHTML;
    }
    else {
        var chartObj = di_getChartObject(uid);
        titleText = chartObj.options.title.text;
    }
    return titleText;
}

function getSubTitleForGallery(chartType, uid) {
    var subtitleText;
    if (chartType == "map2d" || chartType == "map3d")
        return;
    if (chartType == "radar") {
        subtitleText = di_GetRadarSubTitle(uid);
    }
    else if (chartType == "table") {
        subtitleText = z('dataSubTitle').innerHTML;
    }
    else {
        var chartObj = di_getChartObject(uid);
        subtitleText = chartObj.options.subtitle.text;
    }
    return subtitleText;
}
function getTableDetailsForGallery() {
    var objOAT = OAT.getSelfInstance();
    var delemeter = "[******]";
    var InputParam = z('dataTitle').innerHTML + delemeter + z('dataSubTitle').innerHTML + delemeter;
    InputParam += JSON.stringify(objOAT.headerRow) + delemeter + JSON.stringify(objOAT.filteredData) + delemeter +
                   JSON.stringify(objOAT.rowConditions) + delemeter + JSON.stringify(objOAT.colConditions) + delemeter
                   + z('hdbnid').value;
    return InputParam;
}

function getTreemapSettings(uid) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.op.viz.json;
    var chartSettings = JSON.stringify(chartInput);
    return chartSettings;
}

function getCloudSettings(uid) {
    var cloudData = di_getChartObject(uid);
    var cloudDataStr = JSON.stringify(cloudData);
    return cloudDataStr;
}

function selectedItemsForGallery(typeText, typeInt) {
    var liTags = z("menu5").childNodes;
    var selectedItems = "";
    for (var tagIndex = 0; tagIndex < liTags.length; tagIndex++) {
        var allLiTags = liTags[tagIndex].childNodes;
        if (trim(allLiTags[0].innerHTML) == typeText) {
            var ulTags = allLiTags[1].childNodes;
            for (var li = 0; li < ulTags.length; li++) {
                if (li > 0) {
                    if (ulTags[li].childNodes[0].checked) {
                        if (typeInt == 0) {
                            selectedItems += "{@@}" + ulTags[li].childNodes[1].name;
                        }
                        else if (typeInt == 1) {
                            var NowIndicatorNId = getIndicatorNIdFromIU(ulTags[li].childNodes[1].name);
                            selectedItems += "{@@}" + trim(NowIndicatorNId);
                        }
                    }
                }
            }
        }
    }
    return selectedItems;
}
function trim(s) {
    var l = 0; var r = s.length - 1;
    while (l < s.length && s[l] == ' ')
    { l++; }
    while (r > l && s[r] == ' ')
    { r -= 1; }
    return s.substring(l, r + 1);
}
function properCategoryText(categoryData) {
    if (categoryData != null) {
        for (var i = 0; i < categoryData.length; i++) {
            var textValue = categoryData[i];
            textValue = trimSlash(textValue);
            textValue = textValue.replace(/{@@}/g, " ");
            categoryData[i] = textValue;
        }
    }
    return categoryData;
}
function trimSlash(s) {
    var l = 0; var r = s.length - 1;
    while (l < s.length && s[l] == '{@@}')
    { l++; }
    while (r > l && s[r] == '{@@}')
    { r -= 1; }
    return s.substring(l, r + 1);
}

function scatterXTitle(chartData) {
    var xTitle = "";
    if (chartData.seriesCollection.length >= 2)
        xTitle = chartData.seriesCollection[0].name;
    return xTitle;
}

function scatterYTitle(chartData) {
    var yTitle = "";
    if (chartData.seriesCollection.length >= 2)
        yTitle = chartData.seriesCollection[1].name;
    return yTitle;
}

/**/
function makeBubbleData(bubbleData) {
    var newBubbleData = new Object();
    var count = -1;
    var isCategoryAdd = false;
    if (bubbleData != null) {
        if (bubbleData.seriesCollection.length >= 3) {
            var selectedAreas = selectedItemsForGallery(z("langAreaText").value, 0);
            var SelectedTimePeriod = selectedItemsForGallery(z("langTimePeriodText").value, 0);
            var TimePeriodCol = SelectedTimePeriod.split("{@@}");
            var AreaCol = selectedAreas.split("{@@}");
            newBubbleData.categoryCollection = [];
            newBubbleData.seriesCollection = [];

            //make series collection
            for (var i = 0; i < 3; i++) {
                var newSeriesObject = new Object();
                newSeriesObject.data = [];
                newSeriesObject.name = bubbleData.seriesCollection[i].name;
                newBubbleData.seriesCollection.push(newSeriesObject);
            }
            // make TimePeriod Record			

            for (var t = 0; t < TimePeriodCol.length; t++) {
                if (TimePeriodCol[t] != "" && TimePeriodCol[t] != "undefined") {
                    var timeObject1 = new Object();
                    timeObject1.year = TimePeriodCol[t];
                    timeObject1.values = [];
                    var timeObject2 = new Object();
                    timeObject2.year = TimePeriodCol[t];
                    timeObject2.values = [];
                    var timeObject3 = new Object();
                    timeObject3.year = TimePeriodCol[t];
                    timeObject3.values = [];
                    newBubbleData.seriesCollection[0].data.push(timeObject1);
                    newBubbleData.seriesCollection[1].data.push(timeObject2);
                    newBubbleData.seriesCollection[2].data.push(timeObject3);
                }
            }
            var isTimePeriod = isTimePeriodInOAT();
            var isTimePeriodHide = isColumnHide(z("langTimePeriodText").value);
            for (var s = 0; s < 3; s++) {
                for (var a = 0; a < AreaCol.length; a++) {
                    if (AreaCol[a] != "" && AreaCol[a] != "undefined") {
                        if (!isCategoryAdd)
                            newBubbleData.categoryCollection.push(AreaCol[a]);
                        count = -1;
                        for (var t = 0; t < TimePeriodCol.length; t++) {
                            if (TimePeriodCol[t] != "" && TimePeriodCol[t] != "undefined") {
                                count++;
                                var newDataItem = new Object();
                                var searchText;
                                if (isTimePeriod && !isTimePeriodHide)
                                    searchText = AreaCol[a] + " " + TimePeriodCol[t];
                                else
                                    searchText = AreaCol[a];
                                var index = getItemIndex(bubbleData.categoryCollection, searchText);
                                if (index == -1) {
                                    searchText = TimePeriodCol[t] + " " + AreaCol[a];
                                    index = getItemIndex(bubbleData.categoryCollection, searchText);
                                }
                                var value = 0;
                                if (index > -1)
                                    value = bubbleData.seriesCollection[s].data[index];
                                newBubbleData.seriesCollection[s].data[count].values.push(value);
                            }
                        }
                    }
                }
                if (!isCategoryAdd)
                    isCategoryAdd = true;
            }
        }
    }
    return newBubbleData;
}

function isColumnHide(text) {
    var isHide = false;
    var liTags = z("divHideColumns").childNodes;
    var selectedItems = "";
    var ulTags = liTags[1].childNodes;
    for (var li = 0; li < ulTags.length; li++) {
        if (!ulTags[li].childNodes[0].checked && ulTags[li].childNodes[1].innerHTML == text) {
            isHide = true;
        }
    }
    return isHide;
}
function isTimePeriodInOAT() {
    var result = false;
    var objOAT = OAT.getSelfInstance();
    var index = getItemIndex(objOAT.headerRow, z("langTimePeriodText").value);
    if (index > -1)
        result = true;
    return result;
}
function getItemIndex(catCollection, text) {
    var index = -1;
    if (catCollection != null) {
        for (var i = 0; i < catCollection.length; i++) {
            if (catCollection[i] == text)
                index = i;
        }
    }
    return index;
}

/*This function is called from swf after loading completed swf into html.We get swf id here*/
function ScatterPlotLoaded(swfId, uid) {
    var scatter3dObject = swfobject.getObjectById(swfId);
    var scatter3dSettings = di_getScatter3dSettings(uid);
    di_applyScatterDefaultSettings(scatter3dObject, scatter3dSettings)
}

/*Apply scatter's default settings*/
function di_applyScatterDefaultSettings(scatter3dObject, scatter3dSettings) {
    scatter3dObject.dataToBubbleChart(scatter3dSettings.data);
    scatter3dObject.SetNoteLabelFontSize(scatter3dSettings.note.style.fontSize);
    scatter3dObject.SetNoteLabelColor(scatter3dSettings.note.style.color);
    scatter3dObject.SetNoteLabelWeight(scatter3dSettings.note.style.fontWeight);
    scatter3dObject.SetNoteLabelDecoration(scatter3dSettings.note.style.textDecoration);
    scatter3dObject.SetNoteLabelStyle(scatter3dSettings.note.style.italic);
    scatter3dObject.SetNoteLabelX(scatter3dSettings.note.position.x);
    scatter3dObject.SetNoteLabelY(scatter3dSettings.note.position.y);

    scatter3dObject.SetSourceLabel(scatter3dSettings.source.text);
    scatter3dObject.SetSourceLabelFontSize(scatter3dSettings.source.style.fontSize);
    scatter3dObject.SetSourceLabelColor(scatter3dSettings.source.style.color);
    scatter3dObject.SetSourceLabelWeight(scatter3dSettings.source.style.fontWeight);
    scatter3dObject.SetSourceLabelDecoration(scatter3dSettings.source.style.textDecoration);
    scatter3dObject.SetSourceLabelStyle(scatter3dSettings.source.style.italic);
    scatter3dObject.SetNoteLabelX(scatter3dSettings.source.position.x);
    scatter3dObject.SetNoteLabelY(scatter3dSettings.source.position.y);

    scatter3dObject.SetTitle(scatter3dSettings.title.text);
    scatter3dObject.SetTitleFontSize(scatter3dSettings.title.style.fontSize);
    scatter3dObject.SetTitleColor(scatter3dSettings.title.style.color);
    scatter3dObject.SetTitleWeight(scatter3dSettings.title.style.fontWeight);
    scatter3dObject.SetTitleDecoration(scatter3dSettings.title.style.textDecoration);
    scatter3dObject.SetTitleStyle(scatter3dSettings.title.style.italic);

    scatter3dObject.SetSubTitle(scatter3dSettings.subtitle.text);
    scatter3dObject.SetSubTitleFontSize(scatter3dSettings.subtitle.style.fontSize);
    scatter3dObject.SetSubTitleColor(scatter3dSettings.subtitle.style.color);
    scatter3dObject.SetSubTitleWeight(scatter3dSettings.subtitle.style.fontWeight);
    scatter3dObject.SetSubTitleDecoration(scatter3dSettings.subtitle.style.textDecoration);
    scatter3dObject.SetSubTitleStyle(scatter3dSettings.subtitle.style.italic);
}
// This function will be called by child window
parentFunction = function (data) {
    if (data.split(ParamDelimiter)[0] == "true") {
        LoginLogoutUser(data.split(ParamDelimiter)[2], data.split(ParamDelimiter)[3]);
        showHideComponent();
        openGalleryInputPop();
    }
}

/* Remove special char characters from textString
Parameters: textString - text with special characters
*/
function removeSpecialCharacters(textString) {
    var invalidCharString = "^!@#$%_+=~?&{}/:|*\"?<>\\";
    var validText = "";
    for (var i = 0; i < textString.length; i++) {
        if (invalidCharString.indexOf(textString[i]) == -1) {
            if (Browser.Version() < 9) {
                // if client is using IE6 or lower, run this code
                validText += textString.charAt(i);
            }
            else {
                validText += textString[i];
            }
        }
    }
    return validText;
}

// This function will be called by child window
childHasLoad = function () {
    z("loginIframe").style.display = "";
    di_jq('#divShowLoading').hide();
}

function PointToDecimal(dataArr) {
    var decimalPoint = 0;
    var decimalArr = [];
    var seriesArray = dataArr;
    for (var i = 0; i < seriesArray.length; i++) {
        var series = seriesArray[i];
        var yData = series.data;
        for (var j = 0; j < yData.length; j++) {
            if (yData[j] != null && IsDecimal(yData[j])) {
                var temp = yData[j].toString();
                var decimalLength = temp.length - (temp.indexOf('.') + 1);
                if (decimalArr.indexOf(decimalLength) < 0) {
                    decimalArr.push(decimalLength);
                }
            }
        }
    }
    if (decimalArr.length > 0) {
        decimalArr.sort(function (a, b) { return a - b });
        decimalPoint = decimalArr[decimalArr.length - 1];
    }
    return decimalPoint;
}

function IsDecimal(sText) {
    var ValidChars = "0123456789.,";
    var IsNumber = true;
    var Char;
    for (i = 0; i < sText.length && IsNumber == true; i++) {
        Char = sText.charAt(i);
        if (ValidChars.indexOf(Char) == -1) {
            IsNumber = false;
            break;
        }
    }
    return IsNumber;
}



