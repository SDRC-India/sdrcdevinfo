function di_draw_line_chart(title, uid, width, height, div_id, data, datatype, x_title, y_title, allowTabs, is_save, save_url, lngcodefile, isCloseBtn, hotFuncCloseBtn, shareHotSelection, vcType, settingFileUrl, chartCategory, sourceText, DCURL) {
    var subtitle = '';
    var visu_height = height - 50;
    var visu_height_wsmenu = visu_height - 130;
    var div_obj = document.getElementById(div_id);
    var di_vc_libpath = 'http://dgps/di7poc/diuilib/1.1/';
    if (hotFuncCloseBtn == null || hotFuncCloseBtn == '' || lngcodefile == undefined) {
        hotFuncCloseBtn = 'di_vcclose("' + uid + '")';
    }
    if (lngcodefile == '' || lngcodefile == null || lngcodefile == undefined) {
        lngcodefile = di_vc_libpath + 'library/store/lng.xml';
    }
    var visul_div_id = 'di_visulcontainer_chart' + uid;
    var html_data = di_getVisualizerMarkup(uid, title, subtitle, x_title, y_title, width, height, allowTabs, di_vc_libpath, isCloseBtn, hotFuncCloseBtn, shareHotSelection, 'line', vcType, DCURL);
    div_obj.innerHTML = html_data;
    di_jq(document).ready(function () {
        di_createFontSizeDdl('di_vctfontlist' + uid);
        di_createFontSizeDdl('di_vclfontlist' + uid);
        di_createFontSizeDdl('di_vcslfontlist' + uid);
        di_readDefaultChartSetting(settingFileUrl, title, uid, visul_div_id, data, datatype, x_title, y_title, subtitle, 'line', chartCategory, sourceText);
        di_jq('#di_vcfrm1' + uid).ajaxForm({
            beforeSubmit: function () {
                di_jq('#di_vcprogress_bar' + uid).show();
                di_jq('#di_vcproc1' + uid).removeClass('di_gui_proc_deactive');
                di_jq('#di_vcproc1' + uid).addClass('di_gui_proc_active');
            },
            success: function (filename) {
                drawOATGrid(filename, 'di_visulcontainer_grid' + uid);
                di_jq('#di_vcupbttn' + uid).attr('disabled', false);
                di_jq('#di_vcprogress_bar' + uid).hide();
                di_jq('#di_vcupfile' + uid).val('');
            }
        });
        if (jQuery.inArray('Data', allowTabs) > -1) {
            di_vc_changeTab('gtabm_0' + uid, 'Data', "'" + allowTabs + "'", uid, visu_height);
        }
    });
}

function di_create_line_chart(title, uid, div_id, data, datatype, x_title, y_title, subtitle, defaultSettings, chartType, chartCategory, sourceText) {
    var vc_data = new Object();
    if (data != "") {
        if (datatype == "j.str") {
            vc_data = jQuery.parseJSON(data);
            vc_data = di_covertToProperInput(vc_data);
        } else {
            vc_data = di_covertToProperInput(data);
        }
    } else {
        vc_data.categoryCollection = [];
        vc_data.seriesCollection = [];
    }
    defaultSettings = defaultSettings.replace("$renderDivId$", div_id);
    defaultSettings = defaultSettings.replace("$chartTitle$", title);
    defaultSettings = defaultSettings.replace("$chartSubTitle$", subtitle);
    defaultSettings = defaultSettings.replace("$category$", JSON.stringify(vc_data.categoryCollection));
    defaultSettings = defaultSettings.replace("$xAxisTitle$", x_title);
    defaultSettings = defaultSettings.replace("$seriesData$", JSON.stringify(vc_data.seriesCollection));
    defaultSettings = defaultSettings.replace("$yAxisTitle$", y_title);
    if (chartCategory == "normal") {
        defaultSettings = defaultSettings.replace("$plotOptionId$", JSON.stringify({
            line: {
                pointPadding: 0.2,
                borderWidth: 0,
                shadow: false,
                dataLabels: {
                    enabled: false,
                    color: "#000000",
                    style: {
                        font: 'normal 10px arial',
                        color: '#000000',
                        fontWeight: 'normal',
                        textDecoration: 'none'
                    }
                },
                marker: {
                    lineWidth: 1,
                    radius: 3
                }
            }
        }));
    } else if (chartCategory == "stacking") {
        defaultSettings = defaultSettings.replace("$plotOptionId$", JSON.stringify({
            line: {
                stacking: 'normal',
                dataLabels: {
                    enabled: false,
                    color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white'
                },
                shadow: false,
                style: {
                    font: 'normal 10px arial',
                    color: '#000000',
                    fontWeight: 'normal',
                    textDecoration: 'none'
                }
            }
        }));
    } else if (chartCategory == "percent") {
        defaultSettings = defaultSettings.replace("$plotOptionId$", JSON.stringify({
            line: {
                stacking: 'percent',
                dataLabels: {
                    enabled: false,
                    color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white'
                },
                shadow: false,
                style: {
                    font: 'normal 10px arial',
                    color: '#000000',
                    fontWeight: 'normal',
                    textDecoration: 'none'
                }
            }
        }));
    }
    var chartDefSetting = jQuery.parseJSON(defaultSettings);
    chartDefSetting.tooltip.formatter = function () {
        return '<b>' + this.x + ': ' + this.y + "</b><br>" + di_replaceAll(this.series.name, "{@@}", " ");
    };
    chartDefSetting.legend.labelFormatter = function () {
        return di_replaceAll(this.name, "{@@}", " ");
    };
    var totalDataLabels = 0;
    totalDataLabels = vc_data.categoryCollection.length;
    var totalLegendItems = vc_data.seriesCollection.length;
    chartDefSetting = di_labelRotationSettings(totalDataLabels, totalLegendItems, chartDefSetting, chartType);
    var sourceStyleObj = di_getStyle("", "", "", "", "", "");
    var labelStyleObj = di_getStyle("", "", "", "", "", "");
    var yPosStr = document.getElementById(div_id).style.height;
    yPosStr = parseInt(yPosStr.substr(0, yPosStr.length - 2));
    var sourceFunObj = di_getTextDrawFunction(sourceText, 20, yPosStr, '', 50, 50, sourceStyleObj, labelStyleObj);
    chartDefSetting.chart.events.load = sourceFunObj;
    if (chartDefSetting.yAxis.isMaxValue) {
        var allVals = getAllValues(vc_data.seriesCollection);
        var maxVal = allVals.max();
        chartDefSetting.yAxis.max = maxVal;
        chartDefSetting.plotOptions.line.marker.fillColor = "#ffffff";
        chartDefSetting.plotOptions.line.marker.lineColor = null;
        chartDefSetting.plotOptions.line.marker.radius = 2;
    }
    var chart = new Highcharts.Chart(chartDefSetting);
    chart.options.legend.layout = "vertical";
    chart.options.legend.width = "";
    chart.options.tooltip.style.padding = 5;
    chart = new Highcharts.Chart(chart.options);
    var isUidExist = false;
    if (chartCollection.length > 0) {
        for (var i = 0; i < chartCollection.length; i++) {
            if (chartCollection[i].id == uid) {
                isUidExist = true;
                chartCollection[i].chartInstance = chart;
                break;
            }
        }
    }
    if (!isUidExist) {
        var item = new Object();
        item.id = uid;
        item.chartInstance = chart;
        item.source = sourceText;
        item.label = "";
        item.sourceStyle = sourceStyleObj;
        item.labelStyle = labelStyleObj;
        item.xPos = 50;
        item.yPos = 430;
        item.isYAxisSep = false;
        item.isDataLabelSep = false;
        item.YAxisSepDecimal = 0;
        item.isDataLabelSep = 0;
        item.chartBgcGS = "1";
        item.chartPBgcGS = "1";
        item.chartBorderWidth = 1;
        item.legendBorderWidth = 1;
        item.sourceXPos = 20;
        item.sourceYPos = yPosStr;
        chartCollection.push(item);
    }
}

function di_showLineDataLabels(chartObj) {
    var chartInput = chartObj.options;
    chartInput.plotOptions.line.dataLabels.enabled = true;
    chartObj = new Highcharts.Chart(chartInput);
}

function di_hideLineDataLabels(chartObj) {
    var chartInput = chartObj.options;
    chartInput.plotOptions.line.dataLabels.enabled = false;
    chartObj = new Highcharts.Chart(chartInput);
}

function di_setLineDataLabelChkbox(uid) {
    var chartObj = di_getChartObject(uid);
    document.getElementById('di_vcpdatalbl' + uid).checked = chartObj.options.plotOptions.line.dataLabels.enabled;
}

function di_changeLineDataLabelColor(uid, color) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    var styleObj = di_getLineDataLabelStyle(chartObj);
    styleObj.color = color;
    di_setLineDataLabelStyle(chartObj, styleObj)
}

function di_getLineDataLabelStyle(chartObj) {
    var styleObj;
    var chartInput = chartObj.options;
    styleObj = chartInput.plotOptions.line.dataLabels.style;
    styleObj.color = chartInput.plotOptions.line.dataLabels.color;
    return styleObj;
}

function di_setLineDataLabelStyle(chartObj, styleObj) {
    var chartInput = chartObj.options;
    chartInput.plotOptions.line.dataLabels.style = styleObj;
    chartInput.plotOptions.line.dataLabels.color = styleObj.color;
    
    chartObj = new Highcharts.Chart(chartInput);
}

function di_changeLineDataLabelFontStyle(uid, styleStr, size) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getLineDataLabelStyle(chartObj);
    if (styleStr == "underline" || styleStr == "none") {
        if (styleObj.textDecoration == "underline") {
            styleObj.textDecoration = "none";
        } else {
            styleObj.textDecoration = "underline";
        }
    } else {
        if (styleStr == "bold") {
            if (styleObj.fontWeight == "bold") {
                styleObj.fontWeight = "normal";
            } else {
                styleObj.fontWeight = styleStr;
            }
        } else {
            if (styleObj.font.indexOf('italic') > -1) {
                styleStr = "normal";
            }
            styleObj.font = styleStr + " " + size + " Arial";
        }
    }
    di_setLineDataLabelStyle(chartObj, styleObj);
}

function di_changeLineDataLabelRotation(uid, angle) {
    var chartObj = di_getChartObject(uid);
    chartObj.options.plotOptions.line.dataLabels.rotation = angle;
    chartObj = new Highcharts.Chart(chartObj.options);
}

function di_setLineDataLabelFontSize(uid, val) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    var fontStr = chartInput.plotOptions.line.dataLabels.style.font;
    var fontArray = fontStr.split(' ');
    var newFontStr = "";
    newFontStr = fontArray[0] + " " + val + " " + fontArray[2];
    chartInput.plotOptions.line.dataLabels.style.font = newFontStr;
    chartObj = new Highcharts.Chart(chartInput);
}

function di_toggleLineDataLabelFontSize(uid, isIncrease) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    var fontStr = chartInput.plotOptions.line.dataLabels.style.font;
    var fontArray = fontStr.split(' ');
    var newFontStr = "";
    var newFont = fontArray[1].substr(0, fontArray[1].length - 2);
    if (isIncrease) {
        newFont = parseInt(newFont) + 1;
    } else {
        newFont = parseInt(newFont) - 1;
    }
    newFontStr = fontArray[0] + " " + newFont + "px " + fontArray[2];
    chartInput.plotOptions.line.dataLabels.style.font = newFontStr;
    chartObj = new Highcharts.Chart(chartInput);
}