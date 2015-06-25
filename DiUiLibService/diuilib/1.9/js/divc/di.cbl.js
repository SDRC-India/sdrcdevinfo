function di_createCategoryList(data, ddlId) {
    var ddlIdObj = document.getElementById(ddlId);
    di_clearDdlItems(ddlId);
    var catergoryArray = data.categoryCollection;
    ddlIdObj.options[0] = new Option("Select Series", "-1");
    if (catergoryArray.length > 0) {
        for (var i = 0; i < catergoryArray.length; i++) {
            var itemName = catergoryArray[i];
            itemName = di_replaceAll(itemName, "/", ", ");
            ddlIdObj.options[i + 1] = new Option(itemName, i + 1);
        }
    }
}

function di_createSeriesList(data, ddlId, isInner) {
    var ddlIdObj = document.getElementById(ddlId);
    di_clearDdlItems(ddlId);
    var seriesArray = data.seriesCollection;
    if (isInner) {
        for (var i = 0; i < seriesArray.length; i++) {
            var series = seriesArray[i];
            var itemName = series.name;
            itemName = di_replaceAll(itemName, "/", ", ");
            ddlIdObj.options[i] = new Option(itemName, i);
        }
    } else {
        ddlIdObj.options[0] = new Option("Select Series", "-1");
        for (var i = 0; i < seriesArray.length; i++) {
            var series = seriesArray[i];
            var itemName = series.name;
            itemName = di_replaceAll(itemName, "/", ", ");
            ddlIdObj.options[i + 1] = new Option(itemName, i);
        }
    }
}

function di_covertToProperInput(data) {
    var seriesArray = data.seriesCollection;
    for (var i = 0; i < seriesArray.length; i++) {
        var series = seriesArray[i];
        var yData = series.data;
        for (var j = 0; j < yData.length; j++) {
            yData[j] = parseFloat(yData[j]);
        }
    }
    return data;
}

function di_getStyle(hc_fontColor, hc_fontFamily, hc_fontSize, hc_fontStyle, hc_weight, hc_decoration) {
    var style, strColor, strStyle, strSize, strFamily, strWeight, strDecoration;
    strColor = "#4572A7";
    strFontstyle = "normal";
    strSize = "10px";
    strFamily = "arial";
    strWeight = "normal"
    strDecoration = "none";
    if (hc_fontColor != "") {
        strColor = hc_fontColor;
    }
    if (hc_fontStyle != "") {
        strFontstyle = hc_fontStyle;
    }
    if (hc_fontSize != "") {
        strSize = hc_fontSize;
    }
    if (hc_fontFamily != "") {
        strFamily = hc_fontFamily;
    }
    if (hc_weight != "") {
        strWeight = hc_weight;
    }
    if (hc_decoration != "") {
        strDecoration = hc_decoration;
    }
    style = {
        color: strColor,
        font: strFontstyle + " " + strSize + " " + strFamily,
        fontWeight: strWeight,
        textDecoration: strDecoration
    };
    return style;
}

function di_getLegendObject() {
    var chartLegend = {
        borderWidth: 0,
        align: "",
        verticalAlign: "",
        backgroundColor: "",
        enabled: true,
        borderColor: "#909090",
        borderRadius: 5,
        floating: false,
        itemWidth: null,
        layout: "",
        shadow: false,
        itemStyle: null,
        symbolWidth: 10,
        labelFormatter: null,
        x: 0,
        y: 0
    };
    return chartLegend;
}

function di_getAxisLabel(degree, hc_align, hc_style, hc_x, hc_y) {
    var labels = {
        rotation: degree,
        align: hc_align,
        style: hc_style,
        x: hc_x,
        y: hc_y
    }
    return labels;
}

function di_getxAxisObject() {
    var retVal;
    retVal = {
        categories: [],
        gridLineColor: "#C0C0C0",
        gridLineDashStyle: "Solid",
        gridLineWidth: 0,
        labels: {},
        lineColor: "#C0D0E0",
        lineWidth: 1,
        maxZoom: null,
        min: null,
        minorGridLineColor: "#E0E0E0",
        minorGridLineDashStyle: "Solid",
        minorGridLineWidth: 0,
        title: {}
    }
    return retVal;
}

function di_getyAxisObject() {
    var retVal;
    retVal = {
        gridLineWidth: 0,
        gridLineColor: "#C0C0C0",
        gridLineDashStyle: "Solid",
        labels: {},
        lineColor: "#C0D0E0",
        lineWidth: 1,
        maxZoom: null,
        min: null,
        minorGridLineColor: "#E0E0E0",
        minorGridLineDashStyle: "Solid",
        minorGridLineWidth: 0,
        title: {}
    }
    return retVal;
}

function di_getToolTipObject() {
    var tooltip = {
        backgroundColor: "rgba(255, 255, 255, .85)",
        borderRadius: 5,
        borderWidth: 2,
        formatter: null,
        shared: false,
        style: {
            padding: "5px"
        }
    }
    return tooltip;
}

function di_getRenderedChartObject(chartObj) {
    var chartInput = chartObj.options;
    return chartInput.chart;
}

function di_setRenderedChartObject(chartObj, rederdedChart) {
    var chartInput = chartObj.options;
    chartInput.chart = rederdedChart;
    chartObj = new Highcharts.Chart(chartInput);
}

function di_setChartBgColor(chartObj, color) {
    var chartInput = chartObj.options;
    chartInput.chart.backgroundColor = color;
    chartObj = new Highcharts.Chart(chartInput);
}

function di_setChartPlotAreaColor(chartObj, color) {
    var chartInput = chartObj.options;
    chartInput.chart.plotBackgroundColor = color;
    chartObj = new Highcharts.Chart(chartInput);
}

function di_setChartBorder(chartObj, borderWidth) {
    var chartInput = chartObj.options;
    chartInput.chart.borderWidth = borderWidth;
    chartInput.tooltip.style.padding = 5;
    chartObj = new Highcharts.Chart(chartInput);
}

function di_getChartBorderColor(uid) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    var borderWidth = chartInput.chart.borderColor;
    return borderWidth;
}

function di_setChartBorderColor(chartObj, color) {
    var chartInput = chartObj.options;
    chartInput.chart.borderColor = color;
    chartObj = new Highcharts.Chart(chartInput);
}

function di_getChartBorderRadius(uid) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    var borderRadius = chartInput.chart.borderRadius;
    return borderRadius;
}

function di_setChartBorderRadius(chartObj, Radius) {
    var chartInput = chartObj.options;
    chartInput.chart.borderRadius = Radius;
    chartObj = new Highcharts.Chart(chartInput);
}

function di_getLegendBorderWidth(uid) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    var borderWidth = chartInput.legend.borderWidth;
    return borderWidth;
}

function di_setLegendBorderWidth(uid, borderWidth) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    chartInput.legend.borderWidth = borderWidth;
    chartObj = new Highcharts.Chart(chartInput);
}

function di_getLegendBgColor(uid) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    var color = chartInput.legend.backgroundColor;
    return color;
}

function di_getLegendBorderColor(uid) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    var borderColor = chartInput.legend.borderColor;
    return borderColor;
}

function di_setLegendBorderColor(uid, color) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    chartInput.legend.borderColor = color;
    chartObj = new Highcharts.Chart(chartInput);
}

function di_changeLegendBorderColor(uid, color) {
    var chartObj = di_getChartObject(uid);
    di_setLegendBorderColor(chartObj, color);
}

function di_setLegendBorderRadius(uid, radius) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    chartInput.legend.borderRadius = radius;
    chartObj = new Highcharts.Chart(chartInput);
}

function di_getLegendBorderRadius(uid) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    var radius = chartInput.legend.borderRadius;
    return radius;
}

function di_showGridLine(chartObj) {
    var chartInput = chartObj.options;
    if (chartInput.xAxis.length) {
        chartInput.xAxis[0].gridLineWidth = 1;
    } else {
        chartInput.xAxis.gridLineWidth = 1;
    }
    chartInput.yAxis.gridLineWidth = 1;
    chartObj = new Highcharts.Chart(chartInput);
}

function di_hideGridLine(chartObj) {
    var chartInput = chartObj.options;
    if (chartInput.xAxis.length) {
        chartInput.xAxis[0].gridLineWidth = 0;
    } else {
        chartInput.xAxis.gridLineWidth = 0;
    }
    chartInput.yAxis.gridLineWidth = 0;
    chartObj = new Highcharts.Chart(chartInput);
}

function di_showMinorGridLine(chartObj) {
    var chartInput = chartObj.options;
    if (chartInput.xAxis.length) {
        chartInput.xAxis[0].minorGridLineWidth = 1;
        chartInput.xAxis[0].minorTickLength = 0;
        chartInput.xAxis[0].minorTickInterval = 'auto';
    } else {
        chartInput.xAxis.minorGridLineWidth = 1;
        chartInput.xAxis.minorTickLength = 0;
        chartInput.xAxis.minorTickInterval = 'auto';
    }
    chartInput.yAxis.minorGridLineWidth = 1;
    chartInput.yAxis.minorTickLength = 0;
    chartInput.yAxis.minorTickInterval = 'auto';
    chartObj = new Highcharts.Chart(chartInput);
}

function di_hideMinorGridLine(chartObj) {
    var chartInput = chartObj.options;
    if (chartInput.xAxis.length) {
        chartInput.xAxis[0].minorGridLineWidth = 0;
    } else {
        chartInput.xAxis.minorGridLineWidth = 0;
    }
    chartInput.yAxis.minorGridLineWidth = 0;
    chartObj = new Highcharts.Chart(chartInput);
}

function di_showMajorXGridLine(chartObj) {
    var chartInput = chartObj.options;
    if (chartInput.xAxis.length) {
        chartInput.xAxis[0].gridLineWidth = 1;
    } else {
        chartInput.xAxis.gridLineWidth = 1;
    }
    chartObj = new Highcharts.Chart(chartInput);
}

function di_hideMajorXGridLine(chartObj) {
    var chartInput = chartObj.options;
    if (chartInput.xAxis.length) {
        chartInput.xAxis[0].gridLineWidth = 0;
    } else {
        chartInput.xAxis.gridLineWidth = 0;
    }
    chartObj = new Highcharts.Chart(chartInput);
}

function di_showMajorYGridLine(chartObj) {
    var chartInput = chartObj.options;
    chartInput.yAxis.gridLineWidth = 1;
    chartObj = new Highcharts.Chart(chartInput);
}

function di_hideMajorYGridLine(chartObj) {
    var chartInput = chartObj.options;
    chartInput.yAxis.gridLineWidth = 0;
    chartObj = new Highcharts.Chart(chartInput);
}

function di_setXAxisMajorGridWidth(uid, width) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    if (chartInput.xAxis.length) {
        chartInput.xAxis[0].gridLineWidth = width;
    } else {
        chartInput.xAxis.gridLineWidth = width;
    }
    chartObj = new Highcharts.Chart(chartInput);
}

function di_getXAxisMajorGridWidth(uid) {
    var width;
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    if (chartInput.xAxis.length) {
        width = chartInput.xAxis[0].gridLineWidth;
    } else {
        width = chartInput.xAxis.gridLineWidth;
    }
    return width;
}

function di_setYAxisMajorGridWidth(uid, width) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    chartInput.yAxis.gridLineWidth = width;
    chartObj = new Highcharts.Chart(chartInput);
}

function di_getYAxisMajorGridWidth(uid) {
    var width;
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    width = chartInput.yAxis.gridLineWidth;
    return width;
}

function di_setXAxisMinorGridWidth(uid, width) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    if (chartInput.xAxis.length) {
        chartInput.xAxis[0].minorGridLineWidth = width;
    } else {
        chartInput.xAxis.minorGridLineWidth = width;
    }
    chartObj = new Highcharts.Chart(chartInput);
}

function di_getXAxisMinorGridWidth(uid) {
    var width;
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    if (chartInput.xAxis.length) {
        width = chartInput.xAxis[0].minorGridLineWidth;
    } else {
        width = chartInput.xAxis.minorGridLineWidth;
    }
    return width;
}

function di_setYAxisMinorGridWidth(uid, width) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    chartInput.yAxis.minorGridLineWidth = width;
    chartObj = new Highcharts.Chart(chartInput);
}

function di_getYAxisMinorGridWidth(uid) {
    var width;
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    width = chartInput.yAxis.minorGridLineWidth;
    return width;
}

function di_setXAxisMajorGridColor(uid, color) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    if (chartInput.xAxis.length) {
        chartInput.xAxis[0].gridLineColor = color;
    } else {
        chartInput.xAxis.gridLineColor = color;
    }
    chartObj = new Highcharts.Chart(chartInput);
}

function di_getXAxisMajorGridColor(uid) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    var color;
    if (chartInput.xAxis.length) {
        color = chartInput.xAxis[0].gridLineColor;
    } else {
        color = chartInput.xAxis.gridLineColor;
    }
    return color;
}

function di_setYAxisMajorGridColor(uid, color) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    chartInput.yAxis.gridLineColor = color;
    chartObj = new Highcharts.Chart(chartInput);
}

function di_getYAxisMajorGridColor(uid) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    var color;
    color = chartInput.yAxis.gridLineColor;
    return color;
}

function di_setXAxisMinorGridColor(uid, color) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    if (chartInput.xAxis.length) {
        chartInput.xAxis[0].minorGridLineColor = color;
    } else {
        chartInput.xAxis.minorGridLineColor = color;
    }
    chartObj = new Highcharts.Chart(chartInput);
}

function di_getXAxisMinorGridColor(uid) {
    var color;
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    if (chartInput.xAxis.length) {
        color = chartInput.xAxis[0].minorGridLineColor;
    } else {
        color = chartInput.xAxis.minorGridLineColor;
    }
    return color;
}

function di_setYAxisMinorGridColor(uid, color) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    chartInput.yAxis.minorGridLineColor = color;
    chartObj = new Highcharts.Chart(chartInput);
}

function di_getYAxisMinorGridColor(uid) {
    var color;
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    color = chartInput.yAxis.minorGridLineColor;
    return color;
}

function di_setChartTitleStyle(chartObj, styleObj) {
    var chartInput = chartObj.options;
    chartInput.title.style = styleObj;
    chartObj = new Highcharts.Chart(chartInput);
}

function di_getChartTitleStyle(chartObj) {
    var chartInput = chartObj.options;
    var styleObj = chartInput.title.style;
    return styleObj;
}

function di_setChartSubTitleStyle(chartObj, styleObj) {
    var chartInput = chartObj.options;
    chartInput.subtitle.style = styleObj;
    chartObj = new Highcharts.Chart(chartInput);
}

function di_getChartSubTitleStyle(chartObj) {
    var chartInput = chartObj.options;
    var styleObj = chartInput.subtitle.style;
    return styleObj;
}

function di_getXaxisLabels(chartObj) {
    var chartInput = chartObj.options;
    var xAxisLabelObj;
    if (chartInput.xAxis.length) {
        xAxisLabelObj = chartInput.xAxis[0].labels;
    } else {
        xAxisLabelObj = chartInput.xAxis.labels;
    }
    return xAxisLabelObj;
}

function di_setXaxisLabels(chartObj, xAxisLabelObj) {
    var chartInput = chartObj.options;
    if (chartInput.xAxis.length) {
        chartInput.xAxis[0].labels = xAxisLabelObj;
    } else {
        chartInput.xAxis.labels = xAxisLabelObj;
    }
    chartObj = new Highcharts.Chart(chartInput);
}

function di_getXaxisLabelstyle(chartObj) {
    var chartInput = chartObj.options;
    var styleObj;
    if (chartInput.xAxis.length) {
        styleObj = chartInput.xAxis[0].labels.style;
    } else {
        styleObj = chartInput.xAxis.labels.style;
    }
    return styleObj;
}

function di_setXaxisLabelstyle(chartObj, xAxisLabelStyleObj) {
    var chartInput = chartObj.options;
    if (chartInput.xAxis.length) {
        chartInput.xAxis[0].labels.style = xAxisLabelStyleObj;
    } else {
        chartInput.xAxis.labels.style = xAxisLabelStyleObj;
    }
    chartObj = new Highcharts.Chart(chartInput);
}

function di_getSecondaryXaxisLabelstyle(chartObj) {
    var chartInput = chartObj.options;
    var styleObj;
    if (chartInput.xAxis.length) {
        styleObj = chartInput.xAxis[1].labels.style;
    }
    return styleObj;
}

function di_setSecondaryXaxisLabelstyle(chartObj, xAxisLabelStyleObj) {
    var chartInput = chartObj.options;
    if (chartInput.xAxis.length) {
        chartInput.xAxis[1].labels.style = xAxisLabelStyleObj;
    }
    chartObj = new Highcharts.Chart(chartInput);
}

function di_getXaxisTitle(chartObj) {
    var chartInput = chartObj.options;
    var titleObj;
    if (chartInput.xAxis.length) {
        titleObj = chartInput.xAxis[0].title;
    } else {
        titleObj = chartInput.xAxis.title;
    }
    return titleObj;
}

function di_setXaxisTitle(chartObj, xAxisTitleObj) {
    var chartInput = chartObj.options;
    if (chartInput.xAxis.length) {
        chartInput.xAxis[0].title = xAxisTitleObj;
    } else {
        chartInput.xAxis.title = xAxisTitleObj;
    }
    chartObj = new Highcharts.Chart(chartInput);
}

function di_getXaxisTitleStyle(chartObj) {
    var chartInput = chartObj.options;
    var xAxisTitleStyleObj;
    if (chartInput.xAxis.length) {
        xAxisTitleStyleObj = chartInput.xAxis[0].title.style;
    } else {
        xAxisTitleStyleObj = chartInput.xAxis.title.style;
    }
    return xAxisTitleStyleObj;
}

function di_setXaxisTitleStyle(chartObj, xAxisTitleStyleObj) {
    var chartInput = chartObj.options;
    if (chartInput.xAxis.length) {
        chartInput.xAxis[0].title.style = xAxisTitleStyleObj;
    } else {
        chartInput.xAxis.title.style = xAxisTitleStyleObj;
    }
    chartObj = new Highcharts.Chart(chartInput);
}

function di_getSecondaryXaxisTitleStyle(chartObj) {
    var chartInput = chartObj.options;
    var xAxisTitleStyleObj;
    if (chartInput.xAxis.length) {
        xAxisTitleStyleObj = chartInput.xAxis[1].title.style;
    }
    return xAxisTitleStyleObj;
}

function di_setSecondaryXaxisTitleStyle(chartObj, xAxisTitleStyleObj) {
    var chartInput = chartObj.options;
    if (chartInput.xAxis.length) {
        chartInput.xAxis[1].title.style = xAxisTitleStyleObj;
    }
    chartObj = new Highcharts.Chart(chartInput);
}

function di_getYaxisLabels(chartObj) {
    var chartInput = chartObj.options;
    return chartInput.yAxis.labels;
}

function di_setYaxisLabels(chartObj, yAxisLabelObj) {
    var chartInput = chartObj.options;
    chartInput.yAxis.labels = yAxisLabelObj;
    chartObj = new Highcharts.Chart(chartInput);
}

function di_getYaxisLabelstyle(chartObj) {
    var chartInput = chartObj.options;
    return chartInput.yAxis.labels.style;
}

function di_setYaxisLabelstyle(chartObj, yAxisLabelStyleObj) {
    var chartInput = chartObj.options;
    chartInput.yAxis.labels.style = yAxisLabelStyleObj;
    chartObj = new Highcharts.Chart(chartInput);
}

function di_getYaxisTitle(chartObj) {
    var chartInput = chartObj.options;
    return chartInput.yAxis.title;
}

function di_setYaxisTitle(chartObj, yAxisTitleObj) {
    var chartInput = chartObj.options;
    chartInput.yAxis.title = yAxisTitleObj;
    chartObj = new Highcharts.Chart(chartInput);
}

function di_getYaxisTitleStyle(chartObj) {
    var chartInput = chartObj.options;
    return chartInput.yAxis.title.style;
}

function di_setYaxisTitleStyle(chartObj, yAxisTitleStyleObj) {
    var chartInput = chartObj.options;
    chartInput.yAxis.title.style = yAxisTitleStyleObj;
    chartObj = new Highcharts.Chart(chartInput);
}

function di_getLegendStyle(chartObj) {
    var chartInput = chartObj.options;
    return chartInput.legend.style;
}

function di_setLegendStyle(chartObj, styleObj) {
    var chartInput = chartObj.options;
    chartInput.legend.style = styleObj;
    chartObj = new Highcharts.Chart(chartInput);
}

function di_setYaxisStyle(chartObj, styleObj) {
    var chartInput = chartObj.options;
    chartInput.legend.style = styleObj;
    chartObj = new Highcharts.Chart(chartInput);
}

function di_getChartLegendObject(chartObj) {
    var chartInput = chartObj.options;
    return chartInput.legend;
}

function di_setChartLegendObject(chartObj, legendObj) {
    var chartInput = chartObj.options;
    chartInput.legend = legendObj;
    chartObj = new Highcharts.Chart(chartInput);
}

function di_setSeriesColor(chartObj, seriesIndex, color) {
    var chartInput = chartObj.options;
    chartInput.series[seriesIndex].color = color;
    chartObj = new Highcharts.Chart(chartInput);
}

function di_getSeriesColor(chartObj, seriesIndex) {
    var chartInput = chartObj.options;
    var colorObj = null;
    try {
        colorObj = chartInput.series[seriesIndex].color;
    } catch (err) { }
    return colorObj;
}

function di_setChartLayout(chartObj, layoutValue) {
    var chartInput = chartObj.options;
    switch (layoutValue) {
        case "1":
            chartInput.text = this._chartTitle;
            chartInput.xAxis.title.text = this._xAxisTitle;
            chartInput.yAxis.title.text = this._yAxisTitle;
            chartInput.xAxis.gridLineWidth = 1;
            chartInput.yAxis.gridLineWidth = 1;
            chartInput.yAxis.minorGridLineWidth = 1;
            chartInput.legend.enabled = true;
            chartInput.legend.align = "left";
            chartInput.legend.layout = "vertical";
            chartInput.legend.verticalAlign = "center";
            chartInput.legend.y = 20;
            break;
        case "2":
            chartInput.title.text = "";
            chartInput.xAxis.title.text = "";
            chartInput.yAxis.title.text = "";
            chartInput.xAxis.gridLineWidth = 1;
            chartInput.yAxis.gridLineWidth = 1;
            chartInput.yAxis.minorGridLineWidth = 1;
            chartInput.legend.enabled = true;
            chartInput.legend.align = "center";
            chartInput.legend.layout = "horizontal";
            chartInput.legend.verticalAlign = "bottom";
            chartInput.legend.y = 0;
            break;
        case "3":
            chartInput.title.text = this._chartTitle;
            chartInput.xAxis.title.text = "";
            chartInput.yAxis.title.text = "";
            chartInput.xAxis.gridLineWidth = 0;
            chartInput.yAxis.gridLineWidth = 0;
            chartInput.yAxis.minorGridLineWidth = 0;
            chartInput.legend.enabled = true;
            chartInput.legend.align = "right";
            chartInput.legend.layout = "vertical";
            chartInput.legend.verticalAlign = "center";
            chartInput.legend.y = 20;
            break;
        case "4":
            chartInput.xAxis.title.text = this._xAxisTitle;
            chartInput.title.text = "";
            chartInput.yAxis.title.text = "";
            chartInput.xAxis.gridLineWidth = 1;
            chartInput.yAxis.gridLineWidth = 1;
            chartInput.yAxis.minorGridLineWidth = 1;
            chartInput.legend.enabled = true;
            chartInput.legend.align = "center";
            chartInput.legend.layout = "horizontal";
            chartInput.legend.verticalAlign = "top";
            chartInput.legend.y = 0;
            break;
        case "5":
            chartInput.xAxis.title.text = "";
            chartInput.title.text = "";
            chartInput.yAxis.title.text = "";
            chartInput.xAxis.gridLineWidth = 1;
            chartInput.yAxis.gridLineWidth = 1;
            chartInput.yAxis.minorGridLineWidth = 1;
            chartInput.legend.enabled = false;
            break;
        case "6":
            chartInput.title.text = this._chartTitle;
            chartInput.xAxis.title.text = this._xAxisTitle;
            chartInput.yAxis.title.text = this._yAxisTitle;
            chartInput.xAxis.gridLineWidth = 0;
            chartInput.yAxis.gridLineWidth = 0;
            chartInput.yAxis.minorGridLineWidth = 0;
            chartInput.legend.enabled = false;
            break;
        case "7":
            chartInput.title.text = this._chartTitle;
            chartInput.xAxis.title.text = this._xAxisTitle;
            chartInput.yAxis.title.text = this._yAxisTitle;
            chartInput.xAxis.gridLineWidth = 1;
            chartInput.yAxis.gridLineWidth = 1;
            chartInput.yAxis.minorGridLineWidth = 1;
            chartInput.legend.enabled = true;
            chartInput.legend.align = "right";
            chartInput.legend.layout = "vertical";
            chartInput.legend.verticalAlign = "center";
            chartInput.legend.y = 20;
            break;
        default:
            chartInput.title.text = this._chartTitle;
            chartInput.xAxis.title.text = this._xAxisTitle;
            chartInput.yAxis.title.text = this._yAxisTitle;
            chartInput.xAxis.gridLineWidth = 0;
            chartInput.yAxis.gridLineWidth = 0;
            chartInput.yAxis.minorGridLineWidth = 0;
            chartInput.legend.enabled = true;
            chartInput.legend.align = "left";
            chartInput.legend.layout = "vertical";
            chartInput.legend.verticalAlign = "center";
            chartInput.legend.y = 20;
            break;
    }
    chartObj = new Highcharts.Chart(chartInput);
}

function downloadChartSettings(chartInput, chartType) {
    if (chartType == null) {
        chartType = chartInput.chart.defaultSeriesType;
    }
    var newChartSettings;
    if (chartType.toLowerCase() == "pie" || chartType.toLowerCase() == "donut") {
        newChartSettings = getPieChartSettingsObject(chartInput, chartType);
    } else {
        newChartSettings = getChartSettingsObject(chartInput, chartType);
    }
    var InputParam = JSON.stringify(newChartSettings);
    var form = document.createElement("form");
    form.setAttribute("method", "post");
    form.setAttribute("action", "SaveChartSettings.aspx");
    var hiddenField1 = document.createElement("input");
    hiddenField1.setAttribute("name", "settings");
    hiddenField1.setAttribute("value", InputParam);
    form.appendChild(hiddenField1);
    var hiddenField2 = document.createElement("input");
    hiddenField2.setAttribute("name", "filename");
    hiddenField2.setAttribute("value", chartType + ".settings.txt");
    form.appendChild(hiddenField2);
    document.body.appendChild(form);
    form.submit();
}

function di_shareVisualizer(uid, sharedfunName) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    var url;
    var InputParam = JSON.stringify(chartInput);
    eval(sharedfunName + "(" + InputParam + ")");
}

function di_changeChartBgColor(uid, color) {
    var chartObj = di_getChartObject(uid);
    di_setChartBgColor(chartObj, color);
}

function di_changePlotBgColor(uid, color) {
    var chartObj = di_getChartObject(uid);
    di_setChartPlotAreaColor(chartObj, color);
}

function di_toggleChartBorder(uid, isShow) {
    var chartObj = di_getChartObject(uid);
    if (isShow) {
        di_setChartBorder(chartObj, di_getChartBorderValue(uid));
    } else {
        di_setChartBorder(chartObj, 0);
    }
}

function di_changeChartBorderWidth(uid, width) {
    var chartObj = di_getChartObject(uid);
    di_setChartBorder(chartObj, width);
}

function di_changeChartBorderColor(uid, color) {
    var chartObj = di_getChartObject(uid);
    di_setChartBorderColor(chartObj, color);
}

function di_changeChartBorderRadius(uid, radius) {
    var chartObj = di_getChartObject(uid);
    di_setChartBorderRadius(chartObj, radius);
}

function di_toggleLegendVisibility(uid, isShow) {
    var chartObj = di_getChartObject(uid);
    var legendObj;
    legendObj = di_getChartLegendObject(chartObj);
    legendObj.enabled = isShow;
    di_setChartLegendObject(chartObj, legendObj);
}

function di_toggleMajorGridLine(uid, isShow) {
    var chartObj = di_getChartObject(uid);
    if (isShow) {
        di_showGridLine(chartObj);
    } else {
        di_hideGridLine(chartObj);
    }
}

function di_toggleMinorGridLine(uid, isShow) {
    var chartObj = di_getChartObject(uid);
    if (isShow) {
        di_showMinorGridLine(chartObj);
    } else {
        di_hideMinorGridLine(chartObj);
    }
}

function di_toggleMajorXGridLine(uid, isShow) {
    var chartObj = di_getChartObject(uid);
    if (isShow) {
        di_showMajorXGridLine(chartObj);
    } else {
        di_hideMajorXGridLine(chartObj);
    }
}

function di_toggleMajorYGridLine(uid, isShow) {
    var chartObj = di_getChartObject(uid);
    if (isShow) {
        di_showMajorYGridLine(chartObj);
    } else {
        di_hideMajorYGridLine(chartObj);
    }
}

function di_toggleMinorXGridLine(uid, isShow) {
    if (isShow) {
        di_setXAxisMinorGridWidth(uid, 1);
    } else {
        di_setXAxisMinorGridWidth(uid, 0);
    }
}

function di_toggleMinorYGridLine(uid, isShow) {
    if (isShow) {
        di_setYAxisMinorGridWidth(uid, 1);
    } else {
        di_getXAxisMinorGridWidth(uid, 0);
    }
}

function di_toggleDatalabels(uid, isShow, type) {
    var chartObj = di_getChartObject(uid);
    if (isShow) {
        switch (type) {
            case "column":
                di_showColumnDataLabels(chartObj);
                break;
            case "line":
                di_showLineDataLabels(chartObj);
                break;
            case "bar":
                di_showBarDataLabels(chartObj);
                break;
            case "area":
                di_showAreaDataLabels(chartObj);
                break;
            case "pie":
                di_showPieDataLabels(chartObj);
                break;
            case "donut":
                di_showDonutDataLabels(chartObj);
                break;
            case "pyramid":
                di_showPyramidDataLabels(chartObj);
                break;
            case "stackcolumn":
                di_showColumnDataLabels(chartObj);
                break;
            case "stackline":
                di_showLineDataLabels(chartObj);
                break;
            case "stackbar":
                di_showBarDataLabels(chartObj);
                break;
            case "stackarea":
                di_showAreaDataLabels(chartObj);
                break;
            case "100stackcolumn":
                di_showColumnDataLabels(chartObj);
                break;
            case "100stackline":
                di_showLineDataLabels(chartObj);
                break;
            case "100stackbar":
                di_showBarDataLabels(chartObj);
                break;
            case "100stackarea":
                di_showAreaDataLabels(chartObj);
                break;
            case "scatter2d":
                di_showScatterDataLabels(chartObj);
                break;
        }
    } else {
        switch (type) {
            case "column":
                di_hideColumnDataLabels(chartObj);
                break;
            case "line":
                di_hideLineDataLabels(chartObj);
                break;
            case "bar":
                di_hideBarDataLabels(chartObj);
                break;
            case "area":
                di_hideAreaDataLabels(chartObj);
                break;
            case "pie":
                di_hidePieDataLabels(chartObj);
                break;
            case "donut":
                di_hideDonutDataLabels(chartObj);
                break;
            case "pyramid":
                di_hidePyramidDataLabels(chartObj);
                break;
            case "stackcolumn":
                di_hideColumnDataLabels(chartObj);
                break;
            case "stackline":
                di_hideLineDataLabels(chartObj);
                break;
            case "stackbar":
                di_hideBarDataLabels(chartObj);
                break;
            case "100stackarea":
                di_hideAreaDataLabels(chartObj);
                break;
            case "100stackcolumn":
                di_hideColumnDataLabels(chartObj);
                break;
            case "100stackline":
                di_hideLineDataLabels(chartObj);
                break;
            case "100stackbar":
                di_hideBarDataLabels(chartObj);
                break;
            case "100stackarea":
                di_hideAreaDataLabels(chartObj);
                break;
            case "scatter2d":
                di_hideScatterDataLabels(chartObj);
                break;
        }
    }
}

function di_changeTitleStyle(uid, styleStr, size) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getChartTitleStyle(chartObj);
    switch (styleStr) {
        case "b":
            if (styleObj.fontWeight == "bold") {
                styleObj.fontWeight = "normal";
            } else {
                styleObj.fontWeight = "bold";
            }
            break;
        case "i":
            if (styleObj.font.indexOf('italic') > -1) {
                styleObj.font = "normal " + size + " Arial";
            } else {
                styleObj.font = "italic " + size + " Arial";
            }
            break;
        case "u":
            if (styleObj.textDecoration == "underline") {
                styleObj.textDecoration = "none";
            } else {
                styleObj.textDecoration = "underline";
            }
            break;
    }
    di_setChartTitleStyle(chartObj, styleObj);
}

function di_changeSubTitleStyle(uid, styleStr, size) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getChartSubTitleStyle(chartObj);
    switch (styleStr) {
        case "b":
            if (styleObj.fontWeight == "bold") {
                styleObj.fontWeight = "normal";
            } else {
                styleObj.fontWeight = "bold";
            }
            break;
        case "i":
            if (styleObj.font.indexOf('italic') > -1) {
                styleObj.font = "normal " + size + " Arial";
            } else {
                styleObj.font = "italic " + size + " Arial";
            }
            break;
        case "u":
            if (styleObj.textDecoration == "underline") {
                styleObj.textDecoration = "none";
            } else {
                styleObj.textDecoration = "underline";
            }
            break;
    }
    di_setChartSubTitleStyle(chartObj, styleObj);
}

function di_changeXaxisTitleStyle(uid, styleStr, size) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getXaxisTitleStyle(chartObj);
    switch (styleStr) {
        case "b":
            if (styleObj.fontWeight == "bold") {
                styleObj.fontWeight = "normal";
            } else {
                styleObj.fontWeight = "bold";
            }
            break;
        case "i":
            if (styleObj.font.indexOf('italic') > -1) {
                styleObj.font = "normal " + size + " Arial";
            } else {
                styleObj.font = "italic " + size + " Arial";
            }
            break;
        case "u":
            if (styleObj.textDecoration == "underline") {
                styleObj.textDecoration = "none";
            } else {
                styleObj.textDecoration = "underline";
            }
            break;
    }
    di_setXaxisTitleStyle(chartObj, styleObj);
}

function di_changeSecondaryXaxisTitleStyle(uid, styleStr, size) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getSecondaryXaxisTitleStyle(chartObj);
    switch (styleStr) {
        case "b":
            if (styleObj.fontWeight == "bold") {
                styleObj.fontWeight = "normal";
            } else {
                styleObj.fontWeight = "bold";
            }
            break;
        case "i":
            if (styleObj.font.indexOf('italic') > -1) {
                styleObj.font = "normal " + size + " Arial";
            } else {
                styleObj.font = "italic " + size + " Arial";
            }
            break;
        case "u":
            if (styleObj.textDecoration == "underline") {
                styleObj.textDecoration = "none";
            } else {
                styleObj.textDecoration = "underline";
            }
            break;
    }
    di_setSecondaryXaxisTitleStyle(chartObj, styleObj);
}

function di_changeYaxisTitleStyle(uid, styleStr, size) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getYaxisTitleStyle(chartObj);
    switch (styleStr) {
        case "b":
            if (styleObj.fontWeight == "bold") {
                styleObj.fontWeight = "normal";
            } else {
                styleObj.fontWeight = "bold";
            }
            break;
        case "i":
            if (styleObj.font.indexOf('italic') > -1) {
                styleObj.font = "normal " + size + " Arial";
            } else {
                styleObj.font = "italic " + size + " Arial";
            }
            break;
        case "u":
            if (styleObj.textDecoration == "underline") {
                styleObj.textDecoration = "none";
            } else {
                styleObj.textDecoration = "underline";
            }
            break;
    }
    di_setYaxisTitleStyle(chartObj, styleObj);
}

function di_changeTitleFontSize(uid, val) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getChartTitleStyle(chartObj);
    var fontStr = styleObj.font.split(" ");
    styleObj.font = fontStr[0] + " " + val + " Arial";
    di_setChartTitleStyle(chartObj, styleObj);
}

function di_changeTitleText(uid, txtVal, itemValue) {
    if (itemValue == '')
        var itemValue = di_jq('#di_vcTitlesList' + uid).attr("selectedIndex");
    var labelText = "";
    if (itemValue == "8") {
        var sourceCtrl = document.getElementById('di_vc_source');
        if (sourceCtrl) {
            var sourceObj = di_getTreeTitleObj(uid, 2);
            sourceObj.text = txtVal;
            di_setTreeTitleObj(uid, sourceObj, 2);
        } else {
            di_setSourceText(uid, txtVal);
        }
    } else if (itemValue == "9") {
        di_setLabelText(uid, txtVal);
    }
    if (itemValue != "-1") {
        var chartObj = di_getChartObject(uid);
        if (chartObj.options) {
            var sourceStyleObj = di_getSourceTextStyle(uid);
            var labelStyleObj = di_getLabelTextStyle(uid);
            var labelText = di_getLabelText(uid);
            var sourceText = di_getSourceText(uid);
            var sourcePos = di_getSourcePosition(uid);
            var customPos = di_getCustomLabelPosition(uid);
            var functionObj = di_getTextDrawFunction(sourceText, sourcePos.x, sourcePos.y, labelText, customPos.x, customPos.y, sourceStyleObj, labelStyleObj);
            var titleObj = di_getTitleObj(chartObj, itemValue);
            if (titleObj != null) {
                titleObj.text = txtVal;
            }
            chartObj.options.chart.events.load = functionObj;
            di_setTitleObj(chartObj, titleObj, itemValue);
        } else {
            if (itemValue == "1") {
                var treeMapTitleObj = document.getElementById('di_vc_title');
                if (treeMapTitleObj != null) {
                    treeMapTitleObj.innerHTML = txtVal;
                }
            } else if (itemValue == "2") {
                var treeMapSubTitleObj = document.getElementById('di_vc_subtitle');
                if (treeMapSubTitleObj != null) {
                    treeMapSubTitleObj.innerHTML = txtVal;
                }
            } else if (itemValue == "8") {
                var treeMapSourceObj = document.getElementById('di_vc_source');
                if (treeMapSourceObj != null) {
                    treeMapSourceObj.innerHTML = txtVal;
                }
            }
        }
    }
}

function di_getTitleObj(chartObj, titleType) {
    var chartInput = chartObj.options;
    var titleObj;
    if (chartInput) {
        if (titleType == 1) {
            titleObj = chartInput.title;
        } else if (titleType == 2) {
            titleObj = chartInput.subtitle;
        } else if (titleType == 3) {
            if (chartInput.xAxis.length) {
                titleObj = chartInput.xAxis[0].title;
            } else {
                titleObj = chartInput.xAxis.title;
            }
        } else if (titleType == 5) {
            titleObj = chartInput.yAxis.title;
        }
    } else {
        var treeMapTitleObj;
        titleObj = new Object();
        if (titleType == 1) {
            treeMapTitleObj = document.getElementById('di_vc_title');
        } else if (titleType == 2) {
            treeMapTitleObj = document.getElementById('di_vc_subtitle');
        } else if (titleType == 8) {
            treeMapTitleObj = document.getElementById('di_vc_source');
        }
        titleObj.text = treeMapTitleObj.innerHTML;
    }
    return titleObj;
}

function di_setTitleObj(chartObj, titleObj, titleType) {
    var chartInput = chartObj.options;
    if (titleType == 1) {
        chartInput.title = titleObj;
    } else if (titleType == 2) {
        chartInput.subtitle = titleObj;
    } else if (titleType == 3) {
        if (chartInput.xAxis.length) {
            chartInput.xAxis[0].title = titleObj;
        } else {
            chartInput.xAxis.title = titleObj;
        }
    } else if (titleType == 4) {
        chartInput.yAxis.title = titleObj;
    } else if (titleType == 7) {
        chartInput.xAxis[1].title = titleObj;
    }
    chartObj = new Highcharts.Chart(chartInput);
}

function di_changeSubTitleFontSize(uid, val) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getChartSubTitleStyle(chartObj);
    var fontStr = styleObj.font.split(" ");
    styleObj.font = fontStr[0] + " " + val + " Arial";
    di_setChartSubTitleStyle(chartObj, styleObj);
}

function di_changeXAxisTitleFontSize(uid, val) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getXaxisTitleStyle(chartObj);
    var fontStr = styleObj.font.split(" ");
    styleObj.font = fontStr[0] + " " + val + " Arial";
    di_setXaxisTitleStyle(chartObj, styleObj);
}

function di_changeSecondaryXAxisTitleFontSize(uid, val) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getSecondaryXaxisTitleStyle(chartObj);
    var fontStr = styleObj.font.split(" ");
    styleObj.font = fontStr[0] + " " + val + " Arial";
    di_setSecondaryXaxisTitleStyle(chartObj, styleObj);
}

function di_changeYAxisTitleFontSize(uid, val) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getYaxisTitleStyle(chartObj);
    var fontStr = styleObj.font.split(" ");
    styleObj.font = fontStr[0] + " " + val + " Arial";
    di_setYaxisTitleStyle(chartObj, styleObj);
}

function di_changeTitleFontColor(uid, color) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getChartTitleStyle(chartObj);
    styleObj.color = color;
    di_setChartTitleStyle(chartObj, styleObj);
}

function di_changeSubTitleFontColor(uid, color) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getChartSubTitleStyle(chartObj);
    styleObj.color = color;
    di_setChartSubTitleStyle(chartObj, styleObj);
}

function di_changeXAxisTitleFontColor(uid, color) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getXaxisTitleStyle(chartObj);
    styleObj.color = color;
    di_setXaxisTitleStyle(chartObj, styleObj);
}

function di_changeSecondaryXAxisTitleFontColor(uid, color) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getSecondaryXaxisTitleStyle(chartObj);
    styleObj.color = color;
    di_setSecondaryXaxisTitleStyle(chartObj, styleObj);
}

function di_changeYAxisTitleFontColor(uid, color) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getYaxisTitleStyle(chartObj);
    styleObj.color = color;
    di_setYaxisTitleStyle(chartObj, styleObj);
}

function di_changeXaxisLabelSize(uid, val) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getXaxisLabelstyle(chartObj);
    var fontStr = styleObj.font.split(" ");
    styleObj.font = fontStr[0] + " " + val + " Arial";
    di_setXaxisLabelstyle(chartObj, styleObj);
}

function di_changeXSecondaryaxisLabelSize(uid, val) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getSecondaryXaxisLabelstyle(chartObj);
    var fontStr = styleObj.font.split(" ");
    styleObj.font = fontStr[0] + " " + val + " Arial";
    di_setSecondaryXaxisLabelstyle(chartObj, styleObj);
}

function di_changeYaxisLabelSize(uid, val) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getYaxisLabelstyle(chartObj);
    var fontStr = styleObj.font.split(" ");
    styleObj.font = fontStr[0] + " " + val + " Arial";
    di_setYaxisLabelstyle(chartObj, styleObj);
}

function di_changeXaxisLabelFontStyle(uid, styleStr, size) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getXaxisLabelstyle(chartObj);
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
    di_setXaxisLabelstyle(chartObj, styleObj);
}

function di_changeSecondaryXaxisLabelFontStyle(uid, styleStr, size) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getSecondaryXaxisLabelstyle(chartObj);
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
    di_setSecondaryXaxisLabelstyle(chartObj, styleObj);
}

function di_changeYaxisLabelFontStyle(uid, styleStr, size) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getYaxisLabelstyle(chartObj);
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
    di_setYaxisLabelstyle(chartObj, styleObj);
}

function di_changeXaxisLabelRotation(uid, angle) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getXaxisLabels(chartObj);
    styleObj.rotation = angle;
    if (angle == "360") {
        if (chartObj.options.chart.defaultSeriesType == "bar") {
            styleObj.align = "right";
        } else {
            styleObj.align = "center";
        }
    } else if (angle == "270" || angle == "315") {
        if (chartObj.options.chart.defaultSeriesType == "bar") {
            styleObj.align = "right";
        } else {
            styleObj.align = "right";
        }
    } else {
        if (chartObj.options.chart.defaultSeriesType == "bar") {
            styleObj.align = "right";
        } else {
            styleObj.align = "left";
        }
    }
    di_setXaxisLabels(chartObj, styleObj);
}

function di_changeSecondaryXaxisLabelRotation(uid, angle) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getSecondaryXaxisLabels(chartObj);
    styleObj.rotation = angle;
    if (angle == "360") {
        if (chartObj.options.chart.defaultSeriesType == "bar") {
            styleObj.align = "right";
        } else {
            styleObj.align = "center";
        }
    } else if (angle == "270" || angle == "315") {
        if (chartObj.options.chart.defaultSeriesType == "bar") {
            styleObj.align = "right";
        } else {
            styleObj.align = "right";
        }
    } else {
        if (chartObj.options.chart.defaultSeriesType == "bar") {
            styleObj.align = "right";
        } else {
            styleObj.align = "left";
        }
    }
    di_setSecondaryXaxisLabels(chartObj, styleObj);
}

function di_changeYaxisLabelRotation(uid, angle) {
    var chartObj = di_getChartObject(uid);
    var labels = di_getYaxisLabels(chartObj);
    labels.rotation = angle;
    di_setYaxisLabels(chartObj, labels);
}

function di_changeXaxisLabelColor(uid, color) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getXaxisLabelstyle(chartObj);
    styleObj.color = color;
    di_setXaxisLabelstyle(chartObj, styleObj);
}

function di_changeSecondaryXaxisLabelColor(uid, color) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getSecondaryXaxisLabelstyle(chartObj);
    styleObj.color = color;
    di_setSecondaryXaxisLabelstyle(chartObj, styleObj);
}

function di_changeYaxisLabelColor(uid, color) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getYaxisLabelstyle(chartObj);
    styleObj.color = color;
    di_setYaxisLabelstyle(chartObj, styleObj);
}

function di_exportImage(uid, imgType, chartType,isDataLabel) {
    var chartObj = di_getChartObject(uid);
    var isHighChart = false;
    if (imgType == "txt") {
        downloadChartSettings(chartObj.options, chartType);
    } else {
        var fileName = chartObj.options.title.text;
        titleText = fileName;
        if (fileName == "") {
            fileName = "chart";
        }
        if (fileName.indexOf("&lt;") > -1) {
            fileName = di_replaceAll(fileName, "&lt;", "");
        }
        if (fileName.indexOf("&gt;") > -1) {
            fileName = di_replaceAll(fileName, "&gt;", "");
        }
        if (imgType == "application/vnd.xls") {
            chartObj.options.exporting.url = "ExportVisualization.aspx";
            isHighChart = false;
        } else {
            chartObj.options.exporting.url = "http://export.highcharts.com";
            isHighChart = true;
        }
        var htmlTable = getOATtableContent();
        var subtitleText = chartObj.options.subtitle.text;
        fileName = di_removeInvalidCharacters(fileName);
        chartObj.exportChart({
            type: imgType,
            filename: fileName
        }, null, {
            title: titleText,
            subtitle: subtitleText,
            source: getSourceList(),
            tabledata: htmlTable,
            keywords: ""
        }, isHighChart, isDataLabel);
    }
}

//function di_getDataLableVisiblity(chartType) {
//    var isVisible = false;
//    switch (chartType) {
//        case "line":
//            isVisible = chartInput.plotOptions.line.dataLabels.enabled;
//            break;
//        case "column":
//            isVisible = chartInput.plotOptions.column.dataLabels.enabled;
//            break;
//        case "area":
//            isVisible = chartInput.plotOptions.area.dataLabels.enabled;
//            break;
//        case "bar":
//            isVisible = chartInput.plotOptions.bar.dataLabels.enabled;
//            break;
//        case "pie":
//            isVisible = chartInput.plotOptions.pie.dataLabels.enabled;
//            break;
//        case "pyramid":
//            isVisible = chartInput.plotOptions.pyramid.dataLabels.enabled;
//            break;
//        case "scatter2d":
//            isVisible = chartInput.plotOptions.pyramid.dataLabels.enabled;
//            break;
//    }
//    return isVisible;
//}

function di_removeInvalidCharacters(invalidFilename) {
    var invalidCharString = "/:|*\"?<>\\";
    var validFilename = "";
    for (var i = 0; i < invalidFilename.length; i++) {
        if (invalidCharString.indexOf(invalidFilename[i]) == -1) {
            validFilename += invalidFilename[i];
        }
    }
    return validFilename;
}

function di_applyTitleFontStyle(uid, styleStr, itemValue, size) {
    if (itemValue == "")
        itemValue = document.getElementById('di_vcTitlesList' + uid).value;
    if (itemValue == "-1")
        return;
    switch (itemValue) {
        case "1":
            var treeMapTitleObj = document.getElementById('di_vc_title');
            if (treeMapTitleObj != null) {
                di_changeTextStyleForTree(treeMapTitleObj, styleStr, size);
            } else {
                di_changeTitleStyle(uid, styleStr, size);
            }
            break;
        case "2":
            var treeMapSubTitleObj = document.getElementById('di_vc_subtitle');
            if (treeMapSubTitleObj != null) {
                di_changeTextStyleForTree(treeMapSubTitleObj, styleStr, size);
            } else {
                di_changeSubTitleStyle(uid, styleStr, size);
            }
            break;
        case "3":
            di_changeXaxisTitleStyle(uid, styleStr, size);
            break;
        case "5":
            di_changeYaxisTitleStyle(uid, styleStr, size);
            break;
        case "8":
            var treeMapSourceObj = document.getElementById('di_vc_source');
            if (treeMapSourceObj != null) {
                di_changeTextStyleForTree(treeMapSourceObj, styleStr, size);
            } else {
                di_changeSourceStyle(uid, styleStr, 's');
            }
            break;
        case "9":
            di_changeSourceStyle(uid, styleStr, 'l');
            break;
    }
}

function di_changeSourceStyle(uid, styleStr, flag) {
    var styleObj;
    if (flag == 's') {
        styleObj = di_getSourceTextStyle(uid);
    } else if (flag == 'l') {
        styleObj = di_getLabelTextStyle(uid);
    }
    var newFont;
    switch (styleStr.toLowerCase()) {
        case "b":
            var newWeight;
            if (styleObj.fontWeight == "bold") {
                styleObj.fontWeight = "normal";
            } else {
                styleObj.fontWeight = "bold";
            }
            break;
        case "i":
            var currentFont = styleObj.font.split(" ");
            var newFont;
            if (currentFont[0] == "normal") {
                newFont = "italic" + " " + currentFont[1] + " " + currentFont[2];
            } else {
                newFont = "normal" + " " + currentFont[1] + " " + currentFont[2];
            }
            styleObj.font = newFont;
            break;
        case "u":
            if (styleObj.textDecoration == "none") {
                styleObj.textDecoration = "underline";
            } else {
                styleObj.textDecoration = "none";
            }
            break;
    }
    if (flag == 's') {
        styleObj = di_setSourceTextStyle(uid, styleObj);
    } else if (flag == 'l') {
        styleObj = di_setLabelTextStyle(uid, styleObj);
    }
    di_ChangeSourceLabel(uid);
}

function di_applyTitleFontColor(uid, color, itemValue) {
    if (itemValue == "")
        itemValue = document.getElementById('di_vcTitlesList' + uid).value;
    if (itemValue == "-1")
        return;
    switch (itemValue) {
        case "1":
            var treeMapTitleObj = document.getElementById('di_vc_title');
            if (treeMapTitleObj != null) {
                treeMapTitleObj.style.color = color;
            } else {
                di_changeTitleFontColor(uid, color);
            }
            break;
        case "2":
            var treeMapSubTitleObj = document.getElementById('di_vc_subtitle');
            if (treeMapSubTitleObj != null) {
                treeMapSubTitleObj.style.color = color;
            } else {
                di_changeSubTitleFontColor(uid, color);
            }
            break;
        case "3":
            di_changeXAxisTitleFontColor(uid, color);
            break;
        case "5":
            di_changeYAxisTitleFontColor(uid, color);
            break;
        case "8":
            var treeMapSourceObj = document.getElementById('di_vc_source');
            if (treeMapSourceObj != null) {
                treeMapSourceObj.style.color = color;
            } else {
                di_changeSourceLabelColor(uid, color, 's');
            }
            break;
        case "9":
            di_changeSourceLabelColor(uid, color, 'l');
            break;
    }
}

function di_changeSourceLabelColor(uid, color, flag) {
    var styleObj;
    if (flag == 's') {
        styleObj = di_getSourceTextStyle(uid);
    } else if (flag == 'l') {
        styleObj = di_getLabelTextStyle(uid);
    }
    styleObj.color = color;
    if (flag == 's') {
        styleObj = di_setSourceTextStyle(uid, styleObj);
    } else if (flag == 'l') {
        styleObj = di_setLabelTextStyle(uid, styleObj);
    }
    di_ChangeSourceLabel(uid);
}

function di_applyTitltFontSize(uid, val, itemValue) {
    if (itemValue == '')
        itemValue = document.getElementById('di_vcTitlesList' + uid).value;
    if (itemValue == "-1")
        return;
    switch (itemValue) {
        case "1":
            var treeMapTitleObj = document.getElementById('di_vc_title');
            if (treeMapTitleObj != null) {
                treeMapTitleObj.style.fontSize = val;
            } else {
                di_changeTitleFontSize(uid, val);
            }
            break;
        case "2":
            var treeMapSubTitleObj = document.getElementById('di_vc_subtitle');
            if (treeMapSubTitleObj != null) {
                treeMapSubTitleObj.style.fontSize = val;
            } else {
                di_changeSubTitleFontSize(uid, val);
            }
            break;
        case "3":
            di_changeXAxisTitleFontSize(uid, val);
            break;
        case "5":
            di_changeYAxisTitleFontSize(uid, val);
            break;
        case "8":
            var treeMapSourceObj = document.getElementById('di_vc_source');
            if (treeMapSourceObj != null) {
                treeMapSourceObj.style.fontSize = val;
            } else {
                var sourceStyle = di_getSourceTextStyle(uid);
                var currentFont = sourceStyle.font.split(" ");
                var newFont = currentFont[0] + " " + val + " " + currentFont[2];
                sourceStyle.font = newFont;
                di_setSourceTextStyle(uid, sourceStyle);
                di_ChangeSourceLabel(uid);
            }
            break;
        case "9":
            var labelStyle = di_getLabelTextStyle(uid);
            var currentFont = labelStyle.font.split(" ");
            var newFont = currentFont[0] + " " + val + " " + currentFont[2];
            labelStyle.font = newFont;
            di_setLabelTextStyle(uid, labelStyle);
            di_ChangeSourceLabel(uid);
            break;
    }
}

function di_ChangeSourceLabel(uid) {
    var chartObj = di_getChartObject(uid);
    var sourceStyleObj = di_getSourceTextStyle(uid);
    var labelStyleObj = di_getLabelTextStyle(uid);
    var labelText = di_getLabelText(uid);
    var sourceText = di_getSourceText(uid);
    var sourcePos = di_getSourcePosition(uid);
    var customPos = di_getCustomLabelPosition(uid);
    var functionObj = di_getTextDrawFunction(sourceText, sourcePos.x, sourcePos.y, labelText, customPos.x, customPos.y, sourceStyleObj, labelStyleObj);
    var chartInput = chartObj.options;
    chartInput.chart.events.load = functionObj;
    chartObj = new Highcharts.Chart(chartInput);
}

function di_applyAxisDataLabelRotation(uid, angle, itemValue, chartType) {
    if (itemValue == "")
        itemValue = document.getElementById('di_vcSelLabel' + uid).selectedIndex;
    if (itemValue == "-1")
        return;
    switch (itemValue) {
        case "4":
            di_changeXaxisLabelRotation(uid, angle);
            break;
        case "6":
            di_changeYaxisLabelRotation(uid, angle);
            break;
        case "7":
            if (chartType == "column") {
                di_changeColumnDataLabelRotation(uid, angle);
            } else if (chartType == "line") {
                di_changeLineDataLabelRotation(uid, angle);
            } else if (chartType == "area") {
                di_changeAreaDataLabelRotation(uid, angle);
            } else if (chartType == "bar") {
                di_changeBarDataLabelRotation(uid, angle);
            } else if (chartType == "pie") {
                di_changePieDataLabelRotation(uid, angle);
            } else if (chartType == "pyramid") {
                di_changePyramidDataLabelRotation(uid, angle);
            } else if (chartType == "scatter2d") {
                di_changeScatterDataLabelRotation(uid, angle);
            }
            break;
    }
}

function di_applyAxisDataLabelFontSize(uid, val, itemValue, chartType) {
    if (itemValue == "")
        itemValue = document.getElementById('di_vcSelLabel' + uid).value;
    if (itemValue == "-1")
        return;
    switch (itemValue) {
        case "4":
            di_changeXaxisLabelSize(uid, val);
            break;
        case "6":
            di_changeYaxisLabelSize(uid, val);
            break;
        case "7":
            if (chartType == "column") {
                di_setColumnDataLabelFontSize(uid, val);
            } else if (chartType == "line") {
                di_setLineDataLabelFontSize(uid, val);
            } else if (chartType == "area") {
                di_setAreaDataLabelFontSize(uid, val);
            } else if (chartType == "bar") {
                di_setBarDataLabelFontSize(uid, val);
            } else if (chartType == "pie") {
                di_setPieDataLabelFontSize(uid, val)
            } else if (chartType == "pyramid") {
                di_setPyramidDataLabelFontSize(uid, val);
            } else if (chartType == "scatter2d") {
                di_setScatterDataLabelFontSize(uid, val);
            }
            break;
    }
}

function di_applyAxisLabelFontStyle(uid, styleStr, itemValue, chartType, size) {
    if (itemValue == "")
        itemValue = document.getElementById('di_vcSelLabel' + uid).value;
    if (itemValue == "-1")
        return;
    var tempStr;
    if (styleStr.toLowerCase() == "b") {
        tempStr = "bold";
    } else if (styleStr.toLowerCase() == "i") {
        tempStr = "italic";
    } else if (styleStr.toLowerCase() == "u") {
        tempStr = "underline";
    }
    switch (itemValue) {
        case "4":
            di_changeXaxisLabelFontStyle(uid, tempStr, size);
            break;
        case "6":
            di_changeYaxisLabelFontStyle(uid, tempStr, size);
            break;
        case "7":
            if (chartType == "column") {
                di_changeColumnDataLabelFontStyle(uid, tempStr, size);
            } else if (chartType == "line") {
                di_changeLineDataLabelFontStyle(uid, tempStr, size);
            } else if (chartType == "area") {
                di_changeAreaDataLabelFontStyle(uid, tempStr, size);
            } else if (chartType == "bar") {
                di_changeBarDataLabelFontStyle(uid, tempStr, size);
            } else if (chartType == "pie") {
                di_changePieDataLabelFontStyle(uid, tempStr, size);
            } else if (chartType == "pyramid") {
                di_changePyramidDataLabelFontStyle(uid, tempStr, size);
            } else if (chartType == "scatter2d") {
                di_changeScatterDataLabelFontStyle(uid, tempStr, size);
            }
            break;
    }
}

function di_applyAxisLabelFontColor(uid, color, itemValue, chartType) {
    if (itemValue == "-1")
        return;
    switch (itemValue) {
        case "4":
            di_changeXaxisLabelColor(uid, color);
            break;
        case "6":
            di_changeYaxisLabelColor(uid, color);
            break;
        case "7":
            if (chartType == "column") {
                di_changeColumnDataLabelColor(uid, color);
            } else if (chartType == "line") {
                di_changeLineDataLabelColor(uid, color);
            } else if (chartType == "area") {
                di_changeAreaDataLabelColor(uid, color);
            } else if (chartType == "bar") {
                di_changeBarDataLabelColor(uid, color);
            } else if (chartType == "pie") {
                di_changePieDataLabelColor(uid, color);
            } else if (chartType == "pyramid") {
                di_changePyramidDataLabelColor(uid, color);
            } else if (chartType == "scatter2d") {
                di_changeScatterDataLabelColor(uid, color);
            }
            break;
    }
}

function di_toggleLegendBorder(uid, isShow) {
    if (isShow) {
        di_setLegendBorderWidth(uid, 1);
    } else {
        di_setLegendBorderWidth(uid, 0);
    }
}

function di_applyLegendFontSize(uid, val) {
    var chartObj = di_getChartObject(uid);
    var legendObj = di_getChartLegendObject(chartObj);
    legendObj.itemStyle.fontSize = val;
    var fontStr = legendObj.itemStyle.font.split(" ");
    legendObj.itemStyle.font = fontStr[0] + " " + val + " Arial";
    di_setChartLegendObject(chartObj, legendObj);
}

function di_getLegendFontSize(uid) {
    var LegendfontSize;
    var chartObj = di_getChartObject(uid);
    var legendObj = di_getChartLegendObject(chartObj);
    var fontStr = legendObj.itemStyle.font.split(" ");
    LegendfontSize = fontStr[1];
    return LegendfontSize;
}

function di_applyLegendFontColor(uid, color) {
    var chartObj = di_getChartObject(uid);
    var legendObj = di_getChartLegendObject(chartObj);
    legendObj.itemStyle.color = color;
    di_setChartLegendObject(chartObj, legendObj);
}

function di_getLegendFontColor(uid) {
    var chartObj = di_getChartObject(uid);
    var legendObj = di_getChartLegendObject(chartObj);
    var color = legendObj.itemStyle.color;
    return color;
}

function di_applyLegendFontStyle(uid, styleStr, size) {
    var chartObj = di_getChartObject(uid);
    var legendObj = di_getChartLegendObject(chartObj);
    if (styleStr == "b") {
        if (legendObj.itemStyle.fontWeight == "bold") {
            legendObj.itemStyle.fontWeight = "normal";
        } else {
            legendObj.itemStyle.fontWeight = "bold";
        }
    } else if (styleStr == "i") {
        if (legendObj.itemStyle.font.indexOf('italic') > -1) {
            legendObj.itemStyle.font = "normal " + size + " Arial";
        } else {
            legendObj.itemStyle.font = "italic " + size + " Arial";
        }
    } else if (styleStr == "u") {
        if (legendObj.itemStyle.textDecoration == "underline") {
            legendObj.itemStyle.textDecoration = "none";
        } else {
            legendObj.itemStyle.textDecoration = "underline";
        }
    }
    di_setChartLegendObject(chartObj, legendObj);
}

function di_isLegendFontStyle(uid, styleStr) {
    var chartObj = di_getChartObject(uid);
    var legendObj = di_getChartLegendObject(chartObj);
    var retVal = false;
    if (styleStr == "b") {
        if (legendObj.itemStyle.fontWeight == "bold") {
            retVal = true;
        }
    } else if (styleStr == "i") {
        if (legendObj.itemStyle.font.indexOf('italic') > -1) {
            retVal = true;
        }
    } else if (styleStr == "u") {
        if (legendObj.itemStyle.textDecoration == "underline") {
            retVal = true;
        }
    }
    return retVal;
}

function di_applyLegendBgColor(uid, color) {
    var chartObj = di_getChartObject(uid);
    var legendObj = di_getChartLegendObject(chartObj);
    legendObj.backgroundColor = color
    di_setChartLegendObject(chartObj, legendObj);
}

function di_printChart(uid) {
    var chartObj = di_getChartObject(uid);
    chartObj.print();
}

function di_selectSeriesColor(uid, color, seriesIndex) {
    if (seriesIndex == "") {
        var ddlObj = document.getElementById('di_vcSelSeries' + uid);
        seriesIndex = ddlObj.options[ddlObj.selectedIndex].value;
    }
    seriesIndex = seriesIndex - 1;
    if (seriesIndex == -1) {
        alert("Select series name");
        return;
    }
    di_changeSeriesColor(uid, seriesIndex, color);
}

function di_changeSeriesColor(uid, seriesIndex, color) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    chartInput.colors[seriesIndex] = color;
    chartInput.tooltip.style.padding = 5;
    chartObj = new Highcharts.Chart(chartInput);
}

function returnColorArray(colorCode) {
    var colorArray = [];
    var constant = 12;
    for (var j = 0; j < 2; j++) {
        var lightColor;
        for (var i = 0; i < 5; i++) {
            var percent = 20 * i;
            lightColor = increase_brightness(colorCode, percent);
            colorArray.push(lightColor);
        }
        lightColor = increase_brightness(colorCode, 92);
        colorArray.push(lightColor);
    }
    return colorArray;
}

function returnGradientColorArray(colorCode) {
    var GradientColor = [];
    var constant = 12;
    var colorArray = [];
    var lightColor;
    for (var i = 0; i < 5; i++) {
        var percent = 20 * i;
        lightColor = increase_brightness(colorCode, percent);
        colorArray.push(lightColor);
    }
    lightColor = increase_brightness(colorCode, 92);
    colorArray.push(lightColor);
    for (var j = 0; j < 3; j++) {
        for (var i = 0; i < colorArray.length / 2; i++) {
            var gradientColor = {
                linearGradient: [0, '20%', 1, '80%'],
                stops: [
                    [0, colorArray[i + 3]],
                    [1, colorArray[i]]
                ]
            };
            GradientColor.push(gradientColor);
        }
    }
    return GradientColor;
}

function di_setChartStyle(uid, styleId) {
    var chartObj = di_getChartObject(uid);
    var defaultStyle = ['#4572A7', '#AA4643', '#89A54E', '#80699B', '#3D96AE', '#DB843D', '#92A8CD', '#A47D7C', '#B5CA92'];
    var chartInput = chartObj.options;
    chartInput.chart.backgroundColor = "#ffffff";
    chartInput.chart.plotBackgroundColor = "#ffffff";
    chartInput.legend.backgroundColor = "#ffffff";
    if (styleId == "1") {
        chartInput.colors = returnColorArray("#888888");
    } else if (styleId == "2") {
        chartInput.colors = returnColorArray("#39608e");
    } else if (styleId == "3") {
        chartInput.colors = returnColorArray("#748c41");
    } else if (styleId == "4") {
        chartInput.colors = returnColorArray("#903a38");
    } else if (styleId == "5") {
        chartInput.colors = returnColorArray("#6600CC");
    } else if (styleId == "6") {
        chartInput.colors = returnColorArray("#006B8F");
    } else if (styleId == "7") {
        chartInput.colors = returnColorArray("#CC7A00");
    } else if (styleId == "8") {
        chartInput.colors = returnColorArray("#f4dd0f");
    } else if (styleId == "9") {
        chartInput.colors = returnColorArray("#FF1919");
    } else if (styleId == "10") {
        chartInput.colors = returnColorArray("#006600");
    } else if (styleId == "11") {
        chartInput.colors = returnGradientColorArray("#888888");
    } else if (styleId == "12") {
        chartInput.colors = returnGradientColorArray("#39608e");
    } else if (styleId == "13") {
        chartInput.colors = returnGradientColorArray("#748c41");
    } else if (styleId == "14") {
        chartInput.colors = returnGradientColorArray("#903a38");
    } else if (styleId == "15") {
        chartInput.colors = returnGradientColorArray("#6600CC");
    } else if (styleId == "16") {
        chartInput.colors = returnGradientColorArray("#006B8F");
    } else if (styleId == "17") {
        chartInput.colors = returnGradientColorArray("#CC7A00");
    } else if (styleId == "18") {
        chartInput.colors = returnGradientColorArray("#f4dd0f");
    } else if (styleId == "19") {
        chartInput.colors = returnGradientColorArray("#FF1919");
    } else if (styleId == "20") {
        chartInput.colors = returnGradientColorArray("#006600");
    }
    chartObj = new Highcharts.Chart(chartInput);
}

function di_applyTitleFontSize(uid, isIncrease, itemValue) {
    if (itemValue == "")
        itemValue = document.getElementById('di_vcTitlesList' + uid).value;
    if (itemValue == "-1") {
        return;
    }
    switch (itemValue) {
        case "1":
            var treeMapTitleObj = document.getElementById('di_vc_title');
            if (treeMapTitleObj != null) {
                di_increaseTextFontSizeForTree(treeMapTitleObj, isIncrease);
            } else {
                di_increaseTitleFontSize(uid, isIncrease);
            }
            break;
        case "2":
            var treeMapSubTitleObj = document.getElementById('di_vc_subtitle');
            if (treeMapSubTitleObj != null) {
                di_increaseTextFontSizeForTree(treeMapSubTitleObj, isIncrease);
            } else {
                di_increaseSubTitleFontSize(uid, isIncrease);
            }
            break;
        case "3":
            di_increaseXAxisTitleFontSize(uid, isIncrease);
            break;
        case "5":
            di_increaseYAxisTitleFontSize(uid, isIncrease);
            break;
        case "8":
            var treeMapSourceObj = document.getElementById('di_vc_source');
            if (treeMapSourceObj != null) {
                di_increaseTextFontSizeForTree(treeMapSourceObj, isIncrease);
            } else {
                di_increaseSourceLabelFontSize(uid, isIncrease, 's');
            }
            break;
        case "9":
            di_increaseSourceLabelFontSize(uid, isIncrease, 'l');
            break;
    }
}

function di_increaseSourceLabelFontSize(uid, isIncrease, flag) {
    var styleObj;
    if (flag == 's') {
        styleObj = di_getSourceTextStyle(uid);
    } else if (flag == 'l') {
        styleObj = di_getLabelTextStyle(uid);
    }
    var currentFont = styleObj.font.split(" ");
    var currentFontSize = parseInt(currentFont[1].substr(0, currentFont[1].length - 2));
    if (isIncrease) {
        currentFontSize = currentFontSize + 1;
    } else {
        currentFontSize = currentFontSize - 1;
    }
    styleObj.font = currentFont[0] + " " + currentFontSize + "px " + currentFont[2]
    if (flag == 's') {
        styleObj = di_setSourceTextStyle(uid, styleObj);
    } else if (flag == 'l') {
        styleObj = di_setLabelTextStyle(uid, styleObj);
    }
    di_ChangeSourceLabel(uid);
}

function di_increaseTitleFontSize(uid, isIncrease) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getChartTitleStyle(chartObj);
    var fontStr = styleObj.font.split(" ");
    var val;
    if (isIncrease) {
        val = parseInt(fontStr[1].substr(0, fontStr[1].length - 2)) + 1;
    } else {
        val = parseInt(fontStr[1].substr(0, fontStr[1].length - 2)) - 1;
    }
    if (parseInt(val) >= 9 && parseInt(val) <= 31) {
        styleObj.font = fontStr[0] + " " + val + "px Arial";
        di_setChartTitleStyle(chartObj, styleObj);
    }
    return val + "px";
}

function di_increaseSubTitleFontSize(uid, isIncrease) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getChartSubTitleStyle(chartObj);
    var fontStr = styleObj.font.split(" ");
    var val;
    if (isIncrease) {
        val = parseInt(fontStr[1].substr(0, fontStr[1].length - 2)) + 1;
    } else {
        val = parseInt(fontStr[1].substr(0, fontStr[1].length - 2)) - 1;
    }
    if (parseInt(val) >= 9 && parseInt(val) <= 31) {
        styleObj.font = fontStr[0] + " " + val + "px Arial";
        di_setChartSubTitleStyle(chartObj, styleObj);
    }
    return val + "px";
}

function di_increaseXAxisTitleFontSize(uid, isIncrease) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getXaxisTitleStyle(chartObj);
    var fontStr = styleObj.font.split(" ");
    var val;
    if (isIncrease) {
        val = parseInt(fontStr[1].substr(0, fontStr[1].length - 2)) + 1;
    } else {
        val = parseInt(fontStr[1].substr(0, fontStr[1].length - 2)) - 1;
    }
    if (parseInt(val) >= 9 && parseInt(val) <= 31) {
        styleObj.font = fontStr[0] + " " + val + "px Arial";
        di_setXaxisTitleStyle(chartObj, styleObj);
    }
    return val + "px";
}

function di_increaseSecondaryXAxisTitleFontSize(uid, isIncrease) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getSecondaryXaxisTitleStyle(chartObj);
    var fontStr = styleObj.font.split(" ");
    var val;
    if (isIncrease) {
        val = parseInt(fontStr[1].substr(0, fontStr[1].length - 2)) + 1;
    } else {
        val = parseInt(fontStr[1].substr(0, fontStr[1].length - 2)) - 1;
    }
    if (parseInt(val) >= 9 && parseInt(val) <= 31) {
        styleObj.font = fontStr[0] + " " + val + "px Arial";
        di_setSecondaryXaxisTitleStyle(chartObj, styleObj);
    }
    return val + "px";
}

function di_increaseYAxisTitleFontSize(uid, isIncrease) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getYaxisTitleStyle(chartObj);
    var fontStr = styleObj.font.split(" ");
    var val;
    if (isIncrease) {
        val = parseInt(fontStr[1].substr(0, fontStr[1].length - 2)) + 1;
    } else {
        val = parseInt(fontStr[1].substr(0, fontStr[1].length - 2)) - 1;
    }
    if (parseInt(val) >= 9 && parseInt(val) <= 31) {
        styleObj.font = fontStr[0] + " " + val + "px Arial";
        di_setYaxisTitleStyle(chartObj, styleObj);
    }
    return val + "px";
}

function di_applyAxisLabelFontSize(uid, isIncrease, itemValue, chartType) {
    if (itemValue == "")
        itemValue = document.getElementById('di_vcSelLabel' + uid).value;
    if (itemValue == 0) {
        return;
    }
    switch (itemValue) {
        case "4":
            di_increaseXaxisLabelSize(uid, isIncrease);
            break;
        case "6":
            di_increaseYaxisLabelSize(uid, isIncrease);
            break;
        case "7":
            if (chartType == "column") {
                di_toggleColumnDataLabelFontSize(uid, isIncrease);
            } else if (chartType == "line") {
                di_toggleLineDataLabelFontSize(uid, isIncrease);
            } else if (chartType == "area") {
                di_toggleAreaDataLabelFontSize(uid, isIncrease);
            } else if (chartType == "bar") {
                di_toggleBarDataLabelFontSize(uid, isIncrease);
            } else if (chartType == "pie") {
                di_togglePieDataLabelFontSize(uid, isIncrease);
            } else if (chartType == "pyramid") {
                di_togglePyramidDataLabelFontSize(uid, isIncrease);
            } else if (chartType == "scatter2d") {
                di_toggleScatterDataLabelFontSize(uid, isIncrease);
            }
            break;
    }
}

function di_increaseXaxisLabelSize(uid, isIncrease) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getXaxisLabelstyle(chartObj);
    var fontStr = styleObj.font.split(" ");
    var val;
    if (isIncrease) {
        val = parseInt(fontStr[1].substr(0, fontStr[1].length - 2)) + 1;
    } else {
        val = parseInt(fontStr[1].substr(0, fontStr[1].length - 2)) - 1;
    }
    if (parseInt(val) >= 9 && parseInt(val) <= 31) {
        styleObj.font = fontStr[0] + " " + val + "px Arial";
        di_setXaxisLabelstyle(chartObj, styleObj);
    }
}

function di_increaseSecondaryXaxisLabelSize(uid, isIncrease) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getSecondaryXaxisLabelstyle(chartObj);
    var fontStr = styleObj.font.split(" ");
    var val;
    if (isIncrease) {
        val = parseInt(fontStr[1].substr(0, fontStr[1].length - 2)) + 1;
    } else {
        val = parseInt(fontStr[1].substr(0, fontStr[1].length - 2)) - 1;
    }
    if (parseInt(val) >= 9 && parseInt(val) <= 31) {
        styleObj.font = fontStr[0] + " " + val + "px Arial";
        di_setSecondaryXaxisLabelstyle(chartObj, styleObj);
    }
}

function di_increaseYaxisLabelSize(uid, isIncrease) {
    var chartObj = di_getChartObject(uid);
    var styleObj = di_getYaxisLabelstyle(chartObj);
    var fontStr = styleObj.font.split(" ");
    var val;
    if (isIncrease) {
        val = parseInt(fontStr[1].substr(0, fontStr[1].length - 2)) + 1;
    } else {
        val = parseInt(fontStr[1].substr(0, fontStr[1].length - 2)) - 1;
    }
    if (parseInt(val) >= 9 && parseInt(val) <= 31) {
        styleObj.font = fontStr[0] + " " + val + "px Arial";
        di_setYaxisLabelstyle(chartObj, styleObj);
    }
}

function di_applyLegendItemSize(uid, isIncrease) {
    var chartObj = di_getChartObject(uid);
    var legendObj = di_getChartLegendObject(chartObj);
    var fontStr = legendObj.itemStyle.font.split(" ");
    var val;
    if (isIncrease) {
        val = parseInt(fontStr[1].substr(0, fontStr[1].length - 2)) + 1;
    } else {
        val = parseInt(fontStr[1].substr(0, fontStr[1].length - 2)) - 1;
    }
    if (parseInt(val) >= 9 && parseInt(val) <= 31) {
        legendObj.itemStyle.font = fontStr[0] + " " + val + "px Arial";
        di_setChartLegendObject(chartObj, legendObj);
        di_setDropdownlist('di_vcslfontlist' + uid, val + "px");
    }
}

function di_getChartObject(uid) {
    var chartObj;
    if (chartCollection.length > 0) {
        for (var i = 0; i < chartCollection.length; i++) {
            if (chartCollection[i].id == uid) {
                chartObj = chartCollection[i].chartInstance;
                break;
            }
        }
    }
    return chartObj;
}

function di_setChartObject(uid, chartObj) {
    if (chartCollection.length > 0) {
        for (var i = 0; i < chartCollection.length; i++) {
            if (chartCollection[i].id == uid) {
                chartCollection[i].chartInstance.series = chartObj.series;
                chartCollection[i].chartInstance.xAxis = chartObj.xAxis;
                break;
            }
        }
    }
}

function di_getSourceText(uid) {
    var source;
    if (chartCollection.length > 0) {
        for (var i = 0; i < chartCollection.length; i++) {
            if (chartCollection[i].id == uid) {
                source = chartCollection[i].source;
                break;
            }
        }
    }
    return source;
}

function di_setSourceText(uid, sourceText) {
    if (chartCollection.length > 0) {
        for (var i = 0; i < chartCollection.length; i++) {
            if (chartCollection[i].id == uid) {
                chartCollection[i].source = sourceText;
                break;
            }
        }
    }
}

function di_getChartBorderValue(uid) {
    var width;
    if (chartCollection.length > 0) {
        for (var i = 0; i < chartCollection.length; i++) {
            if (chartCollection[i].id == uid) {
                width = chartCollection[i].chartBorderWidth;
                break;
            }
        }
    }
    return width;
}

function di_setChartBorderValue(uid, width) {
    if (chartCollection.length > 0) {
        for (var i = 0; i < chartCollection.length; i++) {
            if (chartCollection[i].id == uid) {
                chartCollection[i].chartBorderWidth = width;
                break;
            }
        }
    }
}

function di_getSourcePosition(uid) {
    var pos = {
        x: 0,
        y: 0
    };
    if (chartCollection.length > 0) {
        for (var i = 0; i < chartCollection.length; i++) {
            if (chartCollection[i].id == uid) {
                pos.x = chartCollection[i].sourceXPos;
                pos.y = chartCollection[i].sourceYPos;
                break;
            }
        }
    }
    return pos;
}

function di_setSourcePosition(uid, pos) {
    if (chartCollection.length > 0) {
        for (var i = 0; i < chartCollection.length; i++) {
            if (chartCollection[i].id == uid) {
                chartCollection[i].sourceXPos = pos.x;
                chartCollection[i].sourceYPos = pos.y;
                break;
            }
        }
    }
}

function di_getLegendBorderValue(uid) {
    var width;
    if (chartCollection.length > 0) {
        for (var i = 0; i < chartCollection.length; i++) {
            if (chartCollection[i].id == uid) {
                width = chartCollection[i].legendBorderWidth;
                break;
            }
        }
    }
    return width;
}

function di_setLegendBorderValue(uid, width) {
    if (chartCollection.length > 0) {
        for (var i = 0; i < chartCollection.length; i++) {
            if (chartCollection[i].id == uid) {
                chartCollection[i].legendBorderWidth = width;
                break;
            }
        }
    }
}

function di_getLabelText(uid) {
    var label;
    if (chartCollection.length > 0) {
        for (var i = 0; i < chartCollection.length; i++) {
            if (chartCollection[i].id == uid) {
                label = chartCollection[i].label;
                break;
            }
        }
    }
    return label;
}

function di_setLabelText(uid, labelText) {
    var label;
    if (chartCollection.length > 0) {
        for (var i = 0; i < chartCollection.length; i++) {
            if (chartCollection[i].id == uid) {
                chartCollection[i].label = labelText;
                break;
            }
        }
    }
}

function di_getSourceTextStyle(uid) {
    var sourceStyle;
    if (chartCollection.length > 0) {
        for (var i = 0; i < chartCollection.length; i++) {
            if (chartCollection[i].id == uid) {
                sourceStyle = chartCollection[i].sourceStyle;
                break;
            }
        }
    }
    return sourceStyle;
}

function di_setSourceTextStyle(uid, sourceStyle) {
    if (chartCollection.length > 0) {
        for (var i = 0; i < chartCollection.length; i++) {
            if (chartCollection[i].id == uid) {
                chartCollection[i].sourceStyle = sourceStyle;
                break;
            }
        }
    }
}

function di_getBgcGStyle(uid) {
    var GStyle;
    if (chartCollection.length > 0) {
        for (var i = 0; i < chartCollection.length; i++) {
            if (chartCollection[i].id == uid) {
                GStyle = chartCollection[i].chartBgcGS;
                break;
            }
        }
    }
    return GStyle;
}

function di_setBgcGStyle(uid, GStyle) {
    if (chartCollection.length > 0) {
        for (var i = 0; i < chartCollection.length; i++) {
            if (chartCollection[i].id == uid) {
                chartCollection[i].chartBgcGS = GStyle;
                break;
            }
        }
    }
}

function di_getPBgcGStyle(uid) {
    var GStyle;
    if (chartCollection.length > 0) {
        for (var i = 0; i < chartCollection.length; i++) {
            if (chartCollection[i].id == uid) {
                GStyle = chartCollection[i].chartPBgcGS;
                break;
            }
        }
    }
    return GStyle;
}

function di_setPBgcGStyle(uid, GStyle) {
    if (chartCollection.length > 0) {
        for (var i = 0; i < chartCollection.length; i++) {
            if (chartCollection[i].id == uid) {
                chartCollection[i].chartPBgcGS = GStyle;
                break;
            }
        }
    }
}

function di_getLabelTextStyle(uid) {
    var labelStyle;
    if (chartCollection.length > 0) {
        for (var i = 0; i < chartCollection.length; i++) {
            if (chartCollection[i].id == uid) {
                labelStyle = chartCollection[i].labelStyle;
                break;
            }
        }
    }
    return labelStyle;
}

function di_setLabelTextStyle(uid, labelStyle) {
    if (chartCollection.length > 0) {
        for (var i = 0; i < chartCollection.length; i++) {
            if (chartCollection[i].id == uid) {
                chartCollection[i].labelStyle = labelStyle;
                break;
            }
        }
    }
}

function di_createFontSizeDdl(ddlId) {
    var ddlObject = document.getElementById(ddlId);
    di_clearDdlItems(ddlId);
    for (var i = 9; i < 32; i++) {
        ddlObject.options[i - 9] = new Option(i, i + "px");
    }
}

function di_clearDdlItems(ddlId) {
    di_jq('#' + ddlId).empty();
}

function di_changeLegendPosition(uid, position) {
    var chartObj = di_getChartObject(uid);
    var legendObj = di_getChartLegendObject(chartObj);
    legendObj.x = 0;
    legendObj.y = 0;
    switch (position.toLowerCase()) {
        case "1":
            legendObj.align = "left";
            legendObj.verticalAlign = "top";
            break;
        case "2":
            legendObj.align = "left";
            legendObj.verticalAlign = "middle";
            break;
        case "3":
            legendObj.align = "left";
            legendObj.verticalAlign = "bottom";
            break;
        case "4":
            legendObj.align = "center";
            legendObj.verticalAlign = "top";
            break;
        case "5":
            legendObj.align = "center";
            legendObj.verticalAlign = "bottom";
            break;
        case "6":
            legendObj.align = "right";
            legendObj.verticalAlign = "top";
            break;
        case "7":
            legendObj.align = "right";
            legendObj.verticalAlign = "middle";
            break;
        case "8":
            legendObj.align = "right";
            legendObj.verticalAlign = "bottom";
            break;
        case "default":
            chartlegend.align = "right";
            legendObj.verticalAlign = "top";
            break;
    }
    di_setChartLegendObject(chartObj, legendObj);
}

function di_selectChartLayout(uid, chartLayoutId, chartTitle, xAxisTitle, yAxisTitle) {
    di_setChartLayout(uid, chartLayoutId, chartTitle, xAxisTitle, yAxisTitle);
}

function di_setChartLayout(uid, chartLayoutId, chartTitle, xAxisTitle, yAxisTitle) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    switch (chartLayoutId) {
        case "1":
            chartInput.title.text = chartTitle
            chartInput.xAxis.title.text = "";
            chartInput.yAxis.title.text = "";
            chartInput.xAxis.gridLineWidth = 0;
            chartInput.yAxis.gridLineWidth = 1;
            chartInput.xAxis.minorGridLineWidth = 0;
            chartInput.yAxis.minorGridLineWidth = 0;
            chartInput.legend.enabled = true;
            chartInput.legend.align = "right";
            chartInputlegend.verticalAlign = "middle";
            chartInput.chart.marginTop = 20;
            chartInput.chart.marginBottom = 80;
            chartInput.chart.marginLeft = 20;
            chartInput.chart.marginRight = 400;
            break;
        case "2":
            chartInput.title.text = chartTitle;
            chartInput.xAxis.title.text = "";
            chartInput.yAxis.title.text = "";
            chartInput.xAxis.gridLineWidth = 0;
            chartInput.yAxis.gridLineWidth = 0;
            chartInput.xAxis.minorGridLineWidth = 0;
            chartInput.yAxis.minorGridLineWidth = 0;
            chartInput.legend.enabled = true;
            chartInput.legend.align = "center";
            chartInput.legend.verticalAlign = "top";
            chartInput.legend.layout = "horizontal";
            chartInput.chart.marginTop = 200;
            chartInput.chart.marginBottom = 80;
            chartInput.chart.marginLeft = 20;
            chartInput.chart.marginRight = 20;
            break;
        case "3":
            chartInput.title.text = chartTitle;
            chartInput.xAxis.title.text = "";
            chartInput.yAxis.title.text = "";
            chartInput.xAxis.gridLineWidth = 0;
            chartInput.yAxis.gridLineWidth = 1;
            chartInput.xAxis.minorGridLineWidth = 0;
            chartInput.yAxis.minorGridLineWidth = 0;
            chartInput.legend.enabled = true;
            chartInput.legend.align = "center";
            chartInput.legend.verticalAlign = "bottom";
            chartInput.chart.marginTop = 20;
            chartInput.chart.marginBottom = 150;
            chartInput.chart.marginLeft = 20;
            chartInput.chart.marginRight = 20;
            break;
        case "4":
            chartInput.title.text = "";
            chartInput.xAxis.title.text = "";
            chartInput.yAxis.title.text = "";
            chartInput.xAxis.gridLineWidth = 0;
            chartInput.yAxis.gridLineWidth = 0;
            chartInput.xAxis.minorGridLineWidth = 0;
            chartInput.yAxis.minorGridLineWidth = 0;
            chartInput.legend.enabled = true;
            chartInput.legend.align = "center";
            chartInput.legend.verticalAlign = "bottom";
            chartInput.chart.marginTop = 20;
            chartInput.chart.marginBottom = 150;
            chartInput.chart.marginLeft = 20;
            chartInput.chart.marginRight = 20;
            break;
        case "5":
            chartInput.title.text = chartTitle;
            chartInput.xAxis.title.text = "";
            chartInput.yAxis.title.text = yAxisTitle;
            chartInput.xAxis.gridLineWidth = 0;
            chartInput.yAxis.gridLineWidth = 1;
            chartInput.xAxis.minorGridLineWidth = 0;
            chartInput.yAxis.minorGridLineWidth = 0;
            chartInput.legend.enabled = false;
            chartInput.chart.marginTop = 20;
            chartInput.chart.marginBottom = 20;
            chartInput.chart.marginLeft = 20;
            chartInput.chart.marginRight = 20;
            break;
    }
    chart = new Highcharts.Chart(chartInput);
}

function di_setColorDialogBox(ctrlId, color) {
    var ctrlObj = document.getElementById(ctrlId);
    ctrlObj.style.color = color;
    ctrlObj.style.backgroundColor = color;
}

function di_setChartAndPlotareaBgUI(uid) {
    var chartObj = di_getChartObject(uid);
    var renderChartObj = di_getRenderedChartObject(chartObj);
    di_setColorDialogBox('inp_dicolor1' + uid, renderChartObj.backgroundColor);
    di_setColorDialogBox('inp_dicolor2' + uid, renderChartObj.plotBackgroundColor);
}

function di_setHideShowUI(uid) {
    var chartObj = di_getChartObject(uid);
    di_setChartBorderChkbox(uid, chartObj);
    di_setGridlineChkbox(uid, chartObj);
    di_setLegendChkbox(uid, chartObj);
    di_setLegendBorderChkbox(uid, chartObj);
}

function di_setLegendChkbox(uid, chartObj) {
    var legendObj = di_getChartLegendObject(chartObj);
    if (legendObj.enabled) {
        document.getElementById('di_vcplegend' + uid).checked = true;
    } else {
        document.getElementById('di_vcplegend' + uid).checked = false;
    }
}

function di_setLegendBorderChkbox(uid, chartObj) {
    var legendObj = di_getChartLegendObject(chartObj);
    if (legendObj.borderWidth == 0) {
        document.getElementById('di_vclborderchk' + uid).checked = false;
    } else {
        document.getElementById('di_vclborderchk' + uid).checked = true;
    }
}

function di_setGridlineChkbox(uid, chartObj) {
    if (chartObj.options.xAxis.gridLineWidth == 1 && chartObj.options.yAxis.gridLineWidth == 1) {
        document.getElementById('di_vcpgridl' + uid).checked = true;
    } else {
        document.getElementById('di_vcpgridl' + uid).checked = false;
    }
}

function di_setLegendUI(uid) {
    var chartObj = di_getChartObject(uid);
    var legendObj = di_getChartLegendObject(chartObj);
    var fontsize = legendObj.itemStyle.font.split(" ")[1];
    di_setDropdownlist('di_vcslfontlist' + uid, fontsize);
    di_setColorDialogBox('inp_dicolor7' + uid, legendObj.itemStyle.color);
}

function di_setDropdownlist(ddlId, value) {
    var ddlObj = document.getElementById(ddlId);
    for (i = 0; i < ddlObj.options.length; i++) {
        if (ddlObj.options[i].value == value) {
            ddlObj.options[i].selected = true;
        } else {
            ddlObj.options[i].selected = false;
        }
    }
    return;
}

function di_resetChartUIHeight(uid, newHeight) {
    try {
        var chartObj = di_getChartObject(uid);
        if (chartObj != undefined) {
            if (chartObj.options) {
                var renderChartObj = di_getRenderedChartObject(chartObj);
                var sourceStyleObj = di_getSourceTextStyle(uid);
                var labelStyleObj = di_getLabelTextStyle(uid);
                var labelText = di_getLabelText(uid);
                var sourceText = di_getSourceText(uid);
                var customPos = di_getCustomLabelPosition(uid);
                var sourcePos = di_getSourcePosition(uid);
                var functionObj = di_getTextDrawFunction(sourceText, sourcePos.x, sourcePos.y - 10, labelText, customPos.x, customPos.y, sourceStyleObj, labelStyleObj);
                renderChartObj.height = newHeight;
                chartObj.options.tooltip.style.padding = "8px";
                chartObj.options.chart.events.load = functionObj;
                di_setRenderedChartObject(chartObj, renderChartObj);
            } else {
                var jsonData = chartObj.op.viz.json;
                document.getElementById("di_visulcontainer_chart" + uid).style.height = (newHeight - 70) + "px";
                document.getElementById("di_visulcontainer_chart" + uid).innerHTML = "";
                di_create_treemap_chart(uid, "di_visulcontainer_chart" + uid, jsonData, "treemap")
            }
        }
    } catch (err) { }
}

function di_vcclose(uid) {
    di_jq('#di_vccontainer' + uid).hide('slow');
}

function di_coloPicker(uid, fieldid, value, onchangefunc, width, height, chartType) {
    var return_ui;
    var onchangeui;
    if (onchangefunc != '') {
        if (chartType == null) {
            onchangeui = "onchange=" + onchangefunc + "('" + uid + "',this.value)";
        } else {
            onchangeui = "onchange=" + onchangefunc + "('" + uid + "',this.value,'" + chartType + "')";
        }
    }
    return_ui = '<input readonly id="' + fieldid + uid + '" value="' + value + '" style="width:' + width + 'px;height:' + height + 'px;border:1px solid #D1D1D1;cursor:pointer;font-size:7px;" ' + onchangeui + '>';
    return return_ui;
}

function di_coloPicker1(uid, fieldid, value, basepath, bgimg_flag) {
    var stylemk = 'cursor:pointer;width:24px;height:20px;';
    if (bgimg_flag == 'y') {
        stylemk += 'background:url(' + basepath + 'images/icon_cpicker.png) no-repeat;';
    }
    return_ui = '<div id="' + fieldid + uid + '" style="' + stylemk + '"><div id="inp_' + fieldid + uid + '" style="border:1px solid #d3d3d3;position:relative;top:13px;left:0px;font-size:1px;width:20px;height:5px;background-color:' + value + ';"></div></div>';
    return return_ui;
}

function di_vc_changeTab(id, tabref, allowTabs, uid, vcHeight) {
    di_jq('#' + id).blur();
    var allowTabsBrk = allowTabs.split(",");
    if (allowTabsBrk.length > 0) {
        for (var i = 0; i < allowTabsBrk.length; i++) {
            var tabref2 = allowTabsBrk[i].replace(" & ", "");
            if (tabref2 != tabref)
                di_jq('#di_vcsubmenu_' + tabref2.toLowerCase() + uid).hide();
        }
    }
    var existCl = di_jq('#' + id).attr('class');
    var vcMenuHeight = 130;
    if (existCl.indexOf("di_gui_activeTab") > -1) {
        di_jq('#di_vcsubmenu_' + tabref.toLowerCase() + uid).hide();
        di_jq('#' + id).removeClass('di_gui_activeTab');
        di_jq('#di_visulcontainer_chart' + uid).css('height', vcHeight);
        di_jq('#di_visulcontainer_map' + uid).css('height', vcHeight - 10);
        di_jq('#di_visulcontainer_grid' + uid).css('height', vcHeight - 10);
        di_resetChartUIHeight(uid, vcHeight - 10);
        if (document.getElementById('divcframe' + uid).contentWindow.document.body.scrollHeight != 0)
            document.getElementById('divcframe' + uid).contentWindow.changeObjectHeight(vcHeight - 10);
    } else {
        di_jq('#di_vcglobalnav' + uid + ' a').removeClass('di_gui_activeTab');
        di_jq('#' + id).addClass('di_gui_activeTab');
        di_jq('#di_vcsubmenu_' + tabref.toLowerCase() + uid).show('slow');
        di_jq('#di_visulcontainer_chart' + uid).css('height', vcHeight - vcMenuHeight);
        di_jq('#di_visulcontainer_map' + uid).css('height', vcHeight - vcMenuHeight);
        di_jq('#di_visulcontainer_grid' + uid).css('height', vcHeight - vcMenuHeight);
        di_resetChartUIHeight(uid, vcHeight - vcMenuHeight);
        if (document.getElementById('divcframe' + uid).contentWindow.document.body.scrollHeight != 0)
            document.getElementById('divcframe' + uid).contentWindow.changeObjectHeight(vcHeight - vcMenuHeight);
    }
}

function di_vcshowmap(vc_libpath, xml_file_name, height, uid, vcType) {
    if (xml_file_name != '' && xml_file_name != null) {
        var scrUrl = vc_libpath + 'PHP/showmap.php?type=' + vcType + '&fname=' + xml_file_name + '&ht=' + height;
        di_jq('#divcframe' + uid).attr('src', scrUrl);
    }
}

function di_vcGetTitles(title, subtitle, xtitle, ytitle, source, customLabel, uid) {
    var ret_ui;
    ret_ui = '<select id="di_vcTitlesList' + uid + '" onchange=\'di_vcSelTitleVal(this.value, "' + uid + '"); di_setChartTitleUI("' + uid + '", this);\' class="di_vcdrop_box"><option value="-1">Select Title</option><option value="' + title + '">Chart Title</option><option value="' + subtitle + '">Sub Title</option><option value="' + xtitle + '">X-Axis Title</option><option value="' + ytitle + '">Y-Axis Title</option><option value="' + source + '">Source</option><option value="' + customLabel + '">Label</option></select>';
    return ret_ui;
}

function di_vcSelTitleVal(val, uid) {
    if (val != -1) {
        di_jq('#di_vctitleval' + uid).attr('disabled', false);
        di_jq('#di_vctitleval' + uid).val(val);
    } else {
        di_jq('#di_vctitleval' + uid).val('');
        di_jq('#di_vctitleval' + uid).attr('disabled', true);
    }
}

function setLangVars(codefile) {
    var divcLngArry = new Array();
    try {
        di_jq.ajax({
            type: "GET",
            url: codefile,
            dataType: "xml",
            async: false,
            success: function (xml) {
                di_jq(xml).find('lng').each(function () {
                    var key = di_jq(this).attr("key");
                    var val = di_jq(this).attr("val");
                    divcLngArry[key] = val;
                });
            }
        });
    } catch (err) { }
    return divcLngArry;
}

function onchangeSeriesBorderWidth(uid, bdrWidth, chartType) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    switch (chartType) {
        case "column":
            chartInput.plotOptions.column.borderWidth = bdrWidth;
            break;
        case "line":
            break;
        case "bar":
            chartInput.plotOptions.bar.borderWidth = bdrWidth;
            break;
        case "area":
            chartInput.plotOptions.area.lineWidth = bdrWidth;
            break;
        case "pie":
            chartInput.plotOptions.pie.borderWidth = bdrWidth;
            break;
        case "donut":
            chartInput.plotOptions.pie.borderWidth = bdrWidth;
            break;
        case "pyramid":
            chartInput.plotOptions.series.borderWidth = bdrWidth;
            break;
        case "stackcolumn":
            chartInput.plotOptions.column.borderWidth = bdrWidth;
            break;
        case "stackline":
            chartInput.plotOptions.line.lineWidth = bdrWidth;
            break;
        case "stackbar":
            chartInput.plotOptions.bar.borderWidth = bdrWidth;
            break;
        case "stackarea":
            chartInput.plotOptions.area.lineWidth = bdrWidth;
            break;
        case "100stackcolumn":
            chartInput.plotOptions.column.borderWidth = bdrWidth;
            break;
        case "100stackline":
            chartInput.plotOptions.line.lineWidth = bdrWidth;
            break;
        case "100stackbar":
            chartInput.plotOptions.bar.borderWidth = bdrWidth;
            break;
        case "100stackarea":
            chartInput.plotOptions.area.lineWidth = bdrWidth;
            break;
    }
    chartInput.tooltip.style.padding = 5;
    chartObj = new Highcharts.Chart(chartInput);
}

function onchangeSeriesStyle(uid, seriesStyle, chartType) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    switch (chartType) {
        case "column":
            chartInput.plotOptions.column.dashStyle = seriesStyle;
            break;
        case "line":
            chartInput.plotOptions.line.dashStyle = seriesStyle;
            break;
        case "bar":
            chartInput.plotOptions.bar.dashStyle = seriesStyle;
            break;
        case "area":
            chartInput.plotOptions.area.dashStyle = seriesStyle;
            break;
        case "pie":
            chartInput.plotOptions.pie.dashStyle = seriesStyle;
            break;
        case "donut":
            chartInput.plotOptions.pie.dashStyle = seriesStyle;
            break;
        case "pyramid":
            chartInput.plotOptions.series.dashStyle = seriesStyle;
            break;
        case "stackcolumn":
            chartInput.plotOptions.column.dashStyle = seriesStyle;
            break;
        case "stackline":
            chartInput.plotOptions.line.dashStyle = seriesStyle;
            break;
        case "stackbar":
            chartInput.plotOptions.bar.dashStyle = seriesStyle;
            break;
        case "stackarea":
            chartInput.plotOptions.area.dashStyle = seriesStyle;
            break;
        case "100stackcolumn":
            chartInput.plotOptions.column.dashStyle = seriesStyle;
            break;
        case "100stackline":
            chartInput.plotOptions.line.dashStyle = seriesStyle;
            break;
        case "100stackbar":
            chartInput.plotOptions.bar.dashStyle = seriesStyle;
            break;
        case "100stackarea":
            chartInput.plotOptions.area.dashStyle = seriesStyle;
            break;
        case "scatter2d":
            chartInput.plotOptions.scatter.dashStyle = seriesStyle;
            break;
    }
    chartObj = new Highcharts.Chart(chartInput);
}

function di_onchangeSeriesBorderColor(uid, color, chartType) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    switch (chartType) {
        case "column":
            chartInput.plotOptions.column.borderColor = color;
            break;
        case "line":
            chartInput.plotOptions.line.borderColor = color;
            break;
        case "bar":
            chartInput.plotOptions.bar.borderColor = color;
            break;
        case "area":
            chartInput.plotOptions.area.borderColor = color;
            break;
        case "pie":
            chartInput.plotOptions.pie.borderColor = color;
            break;
        case "donut":
            chartInput.plotOptions.pie.borderColor = color;
            break;
        case "pyramid":
            chartInput.plotOptions.series.borderColor = color;
            break;
        case "stackcolumn":
            chartInput.plotOptions.column.borderColor = color;
            break;
        case "stackline":
            chartInput.plotOptions.line.borderColor = color;
            break;
        case "stackbar":
            chartInput.plotOptions.bar.borderColor = color;
            break;
        case "stackarea":
            chartInput.plotOptions.area.borderColor = color;
            break;
        case "100stackcolumn":
            chartInput.plotOptions.column.borderColor = color;
            break;
        case "100stackline":
            chartInput.plotOptions.line.borderColor = color;
            break;
        case "100stackbar":
            chartInput.plotOptions.bar.borderColor = color;
            break;
        case "100stackarea":
            chartInput.plotOptions.area.borderColor = color;
            break;
        case "scatter2d":
            chartInput.plotOptions.scatter.borderColor = color;
            break;
    }
    chartObj = new Highcharts.Chart(chartInput);
}

function di_getVisualizerMarkup(uid, title, subtitle, x_title, y_title, width, height, allowTabs, di_vc_libpath, isCloseBtn, hotFuncCloseBtn, storagePath, chartType, vcType, DCURL) {
    var html_data;
    var visu_height = height - 50;
    if (vcType == '' || vcType == null || vcType == undefined)
        vcType = 'map';
    if (width.indexOf('%') > 0) {
        html_data = '<div id="di_vccontainer' + uid + '" style="width:' + width + ';height:' + height + 'px;">';
    } else {
        html_data = '<div id="di_vccontainer' + uid + '" style="width:' + width + 'px;height:' + height + 'px;">';
    }
    html_data += '<div id="di_vcmenu' + uid + '"><table cellpadding="0" cellspacing="0"><tr><td nowrap><ul id="di_vcglobalnav' + uid + '" class="di_gui_vctabcontainer">';
    if (allowTabs != '' && allowTabs.length > 0) {
        for (var i = 0; i < allowTabs.length; i++) {
            var tabref = allowTabs[i].replace(" & ", "");
            html_data += '<li class="di_gui_vctabcontainerli"><a id="gtabm_' + i + uid + '" class="di_gui_vctabcontainerlia" href="javascript:void(0)" onclick=\'di_vc_changeTab(this.id, "' + tabref + '", "' + allowTabs + '", "' + uid + '", "' + (visu_height - 10) + '")\'>' + allowTabs[i] + '</a></li>';
        }
    }
    html_data += '</ul></td></tr></table></div>';
    html_data += '<div id="di_vcinnercontainer' + uid + '" class="di_gui_vccontainer" style="height:' + visu_height + 'px;"><input type="hidden" name="di_vcmapxmlfile" id="di_vcmapxmlfile' + uid + '" value="">';
    if (isCloseBtn == true) {
        html_data += '<div class="di_vcbtn_close"><img src="' + di_vc_libpath + 'images/di_close_bttn.png" title="Close Window" alt="Close"  border="0" onclick=\'' + hotFuncCloseBtn + ';\' /></div>';
    }
    if (jQuery.inArray('Data', allowTabs) > -1) {
        html_data += '<div id="di_vcsubmenu_data' + uid + '" class="di_gui_navsubmenu" style="display:none;"><div id="main_box"><div class="di_vcsec_box2" style="width:772px"><table width="100%" height="100%" cellpadding="0" cellspacing="0"><tr><td valign="top" height="99" style="padding:0px 5px 0px 5px;"><form name="di_vcfrm1' + uid + '" id="di_vcfrm1' + uid + '" method="POST" action="' + DCURL + '?type=' + vcType + '" enctype="multipart/form-data"><table width="90%" cellpadding="0" cellspacing="0"><tr><td valign="top"><p>Upload data XLS file: <input type="file" name="di_vcupfile" id="di_vcupfile' + uid + '" size="28">&nbsp;<input type="hidden" name="di_vctype" id="di_vctype' + uid + '" value="' + vcType + '">&nbsp; <input type="submit" name="di_vcupbttn" id="di_vcupbttn' + uid + '" value="Upload" class="di_vcclickbttn"></p><div id="di_vcprogress_bar' + uid + '" style="display:none;"><div id="di_vcproc1' + uid + '" class="di_gui_proc_deactive">* Uploading data...</div><div id="di_vcproc2' + uid + '" class="di_gui_proc_deactive">* Validating data...</div><div id="di_vcproc3' + uid + '" class="di_gui_proc_deactive">* Preparing data for Map...</div><div id="di_vcproc4' + uid + '" style="height:5px;" class="di_gui_proc_active"></div></div></td><td valign="top" nowrap><ul><li><a class="di_vcclicklink" href="' + di_vc_libpath + 'library/store/SampleDataEntrySheet.xls">Download data entry sheet</a></li><li><a class="di_vcclicklink" href="' + di_vc_libpath + 'library/store/AreaSheet.xls">Download Area Excel file</a></li></ul><div><a href="javascript:void(0)" onclick=\'di_toggleContainer("' + uid + '","c");setVisualizerData("' + chartType + '")\'>Chart</a> | <a href="javascript:void(0)" onclick=\'di_toggleContainer("' + uid + '","g")\'>Grid</a></div></td></tr></table></form></td></tr><tr><td height="20" class="di_vcsec_box_bttmtxt">Upload Data</td></tr></table></div></div></div>';
    }
    html_data += '<div id="di_vcsubmenu_settings' + uid + '" class="di_gui_navsubmenu" style="display:none;"><div id="main_box"><div class="di_vcsec_box"><table width="100%" height="100%" cellpadding="0" cellspacing="0"><tr><td valign="top" height="99" style="padding:0px 5px 0px 5px;"><p><input type="checkbox" id="di_vcpborder' + uid + '" onclick=\'di_toggleChartBorder("' + uid + '", this.checked)\'>Border</p><p><input type="checkbox" id="di_vcpgridl' + uid + '" onclick=\'di_toggleGridLine("' + uid + '", this.checked)\'>Gridlines</p><p><input type="checkbox" id="di_vcpdatalbl' + uid + '" onclick=\'di_toggleDatalabels("' + uid + '", this.checked,"' + chartType + '")\'>Data Labels</p><p><input type="checkbox" onclick="">Data Tabs</p></td><td valign="top" style="padding:0px 5px 0px 5px;"><p><input type="checkbox" onclick=\'di_toggleLegendVisibility("' + uid + '",this.checked)\' id="di_vcplegend' + uid + '" onclick="">&nbsp;&nbsp;Legend</p><div><table cellspacing="0"><tr><td>' + di_coloPicker1(uid, 'dicolor1', '#ffffff', di_vc_libpath, 'y') + '</td><td><p>Chart</p></td></tr></table></div><div><table cellspacing="0"><tr><td>' + di_coloPicker1(uid, 'dicolor2', '#ffffff', di_vc_libpath, 'y') + '</td><td><p>Plot Area</p></td></tr></table></div></td></tr><tr><td colspan="2" height="20" class="di_vcsec_box_bttmtxt">General</td></tr></table></div><div class="di_vcsec_box"><table width="100%" height="100%" cellpadding="0" cellspacing="0"><tr><td valign="top" height="99" style="padding:0px 5px 0px 5px;"><table width=100%" cellpadding="0" cellspacing="0"><tr><td>' + di_vcGetTitles(title, subtitle, x_title, y_title, 'Source', 'Label', uid) + '</td></tr><tr><td height="30"><input type="text" id="di_vctitleval' + uid + '" value="" class="di_vcdrop_box" disabled="true" onchange=\'di_changeTitleText("' + uid + '", this.value)\'/></td></tr><tr><td height="30"><table cellpadding="0" cellspacing="0"><tr><td><select id="di_vctfontlist' + uid + '" onchange=\'di_applyTitltFontSize("' + uid + '",this.value)\' class="di_vcsmalldrop_box"></select>&nbsp</td><td><span class="di_vc_icon di_vc_icon_fontbig" onclick=\'di_applyTitleFontSize("' + uid + '",true)\'></span></td><td><span class="di_vc_icon di_vc_icon_fontsmall" onclick=\'di_applyTitleFontSize("' + uid + '",false)\'></span></td><td><span class="di_vc_icon di_vc_icon_bold" onclick=\'di_applyTitleFontStyle("' + uid + '", "b")\'></span></td><td><span class="di_vc_icon di_vc_icon_italic" onclick=\'di_applyTitleFontStyle("' + uid + '", "i")\'></span></td><td><span class="di_vc_icon di_vc_icon_underline" onclick=\'di_applyTitleFontStyle("' + uid + '", "u")\'></span></td><td><span class="di_vc_icon di_vc_icon_color">' + di_coloPicker1(uid, 'dicolor5', '#FF0500', di_vc_libpath, 'n') + '</span></td></tr></table></td></tr></table></td></tr><tr><td height="20" class="di_vcsec_box_bttmtxt">Titles</td></tr></table></div><div class="di_vcsec_box" style="width:316px"><table width="100%" height="100%" cellpadding="0" cellspacing="0"><tr><td valign="top" height="99" style="padding:0px 5px 0px 5px;"><table width=100%" cellpadding="0" cellspacing="0"><tr><td><select id="di_vcSelLabel' + uid + '" class="di_vcdrop_box" onchange=\'di_setAxisDatalabelsUI("' + uid + '", this,"' + chartType + '")\'><option value="-1">Select Axis</option><option value="x">X-Axis</option><option value="y">Y-Axis</option><option value="dl">Data Labels</option></select></td></tr><tr><td height="30"><table cellpadding="0" cellspacing="0"><tr><td><select id="di_vclfontlist' + uid + '" onchange=\'di_applyAxisDataLabelFontSize("' + uid + '",this.value,"' + chartType + '")\' class="di_vcsmalldrop_box"></select>&nbsp</td><td><span class="di_vc_icon di_vc_icon_fontbig" onclick=\'di_applyAxisLabelFontSize("' + uid + '",true,"' + chartType + '")\'></span></td><td><span class="di_vc_icon di_vc_icon_fontsmall" onclick=\'di_applyAxisLabelFontSize("' + uid + '",false,"' + chartType + '")\'></span></td><td><span class="di_vc_icon di_vc_icon_bold"   onclick=\'di_applyAxisLabelFontStyle("' + uid + '", "b","' + chartType + '")\'></span></td><td><span class="di_vc_icon di_vc_icon_italic" onclick=\'di_applyAxisLabelFontStyle("' + uid + '", "i","' + chartType + '")\'></span></td><td><span class="di_vc_icon di_vc_icon_underline" onclick=\'di_applyAxisLabelFontStyle("' + uid + '", "u","' + chartType + '")\'></span></td><td><span class="di_vc_icon di_vc_icon_color">' + di_coloPicker1(uid, 'dicolor6', '#FF0500', di_vc_libpath, 'n') + '</span></td></tr></table></td></tr><tr><td height="30"><table cellspacing="0" cellpadding="0"><tr><td height="28"><span class="di_vc_lblicon di_vc_lblicon_1" onclick=\'di_applyAxisDataLabelRotation("' + uid + '","315","' + chartType + '")\'></span></td><td><span class="di_vc_lblicon di_vc_lblicon_2" onclick=\'di_applyAxisDataLabelRotation("' + uid + '","-315","' + chartType + '")\'></span></td><td><span class="di_vc_lblicon di_vc_lblicon_3" onclick=\'di_applyAxisDataLabelRotation("' + uid + '","270","' + chartType + '")\'></span></td><td><span class="di_vc_lblicon di_vc_lblicon_4" onclick=\'di_applyAxisDataLabelRotation("' + uid + '","-270","' + chartType + '")\'></span></td><td><span class="di_vc_lblicon di_vc_lblicon_5" onclick=\'di_applyAxisDataLabelRotation("' + uid + '","360","' + chartType + '")\'></span></td></tr></table></td></tr></table></td></tr><tr><td height="20" class="di_vcsec_box_bttmtxt">Data Labels</td></tr></table></div></div></div>';
    html_data += '<div id="di_vcsubmenu_series' + uid + '" class="di_gui_navsubmenu" style="display:none;"><div id="main_box"><div class="di_vcsec_box"><table width="100%" height="100%" cellpadding="0" cellspacing="0"><tr><td valign="top" height="99" style="padding:0px 5px 0px 5px;"><table width=100%" cellpadding="0" cellspacing="0"><tr><td valign="top"><select id="di_vcSelLegend' + uid + '" onchange=\'di_changeLegendPosition("' + uid + '", this.value)\' class="di_vcdrop_box70"><option value="1">Left top</option><option value="2">Left middle</option><option value="3">Left bottom</option><option value="4">Top</option><option value="5">Bottom</option><option value="6">Right top</option><option value="7">Right middle</option><option value="8">Right bottom</option></select></td><td><p><input type="checkbox" id="di_vclborderchk' + uid + '" onclick=\'di_toggleLegendBorder("' + uid + '", this.checked)\'/>&nbsp;&nbsp;Border </p><p><table cellspacing="0"><tr><td>' + di_coloPicker1(uid, 'dicolor3', '#FFFFFF', di_vc_libpath, 'y') + '</td><td><p>Background Color</p></td></tr></table></p></td></tr><tr><td height="30" colspan="2"><table cellpadding="0" cellspacing="0"><tr><td><select id="di_vcslfontlist' + uid + '" onchange=\'di_applyLegendFontSize("' + uid + '",this.value)\' class="di_vcsmalldrop_box"></select>&nbsp</td><td><span class="di_vc_icon di_vc_icon_fontbig" onclick=\'di_applyLegendItemSize("' + uid + '",true)\'></span></td><td><span class="di_vc_icon di_vc_icon_fontsmall" onclick=\'di_applyLegendItemSize("' + uid + '",false)\'></span></td><td><span class="di_vc_icon di_vc_icon_bold"   onclick=\'di_applyLegendFontStyle("' + uid + '", "b")\'></span></td><td><span class="di_vc_icon di_vc_icon_italic" onclick=\'di_applyLegendFontStyle("' + uid + '", "i")\'></span></td><td><span class="di_vc_icon di_vc_icon_underline" onclick=\'di_applyLegendFontStyle("' + uid + '", "u")\'></span></span></td><td><span class="di_vc_icon di_vc_icon_color">' + di_coloPicker1(uid, 'dicolor7', '#000000', di_vc_libpath, 'n') + '</span></td></tr></table></td></tr></table></td></tr><tr><td height="20" class="di_vcsec_box_bttmtxt">Legend</td></tr></table></div><div class="di_vcsec_box2"><table width="100%" height="100%" cellpadding="0" cellspacing="0"><tr><td valign="top" height="99" style="padding:0px 5px 0px 5px;"><table cellpadding="0" cellspacing="0"><tr><td><span class="di_vc_sformaticon di_vc_sformaticon_1" onclick=\'di_setChartStyle("' + uid + '","1")\'></span></td><td><span class="di_vc_sformaticon di_vc_sformaticon_2" onclick=\'di_setChartStyle("' + uid + '","2")\'></span></td><td><span class="di_vc_sformaticon di_vc_sformaticon_3" onclick=\'di_setChartStyle("' + uid + '","3")\'></span></td></tr></table></td><td valign="top" style="padding:0px 5px 0px 5px;"><p><select id="di_vcSelSeries' + uid + '" class="di_vcdrop_box" onchange=\'di_selectSeriesName("' + uid + '", this.selectedIndex)\'></select></p><p><table cellpadding="0" cellspacing="0"><tr><td height="30">' + di_coloPicker1(uid, 'dicolor4', '#FFFFFF', di_vc_libpath, 'y') + '</td><td>&nbsp;</td><td><span class="di_vc_sricon di_vc_sricon_1">' + di_coloPicker1(uid, 'dicolor8', '#FFFFFF', di_vc_libpath, 'n') + '</span></td><td><span class="di_vc_sricon di_vc_sricon_2" onclick=\'onchangeSeriesBorderWidth("' + uid + '",2,"' + chartType + '")\'></span></td><td><span class="di_vc_sricon di_vc_sricon_3" onclick=\'onchangeSeriesStyle("' + uid + '","dot","' + chartType + '")\'></span></td><td><span class="di_vc_sricon di_vc_sricon_4" onclick=\'onchangeSeriesBorderWidth("' + uid + '",0,"' + chartType + '")\'></span></td></tr></table></p></td></tr><tr><td height="20" class="di_vcsec_box_bttmtxt" colspan="2">Format</td></tr></table></div></div></div>';
    html_data += '<div id="di_vcsubmenu_options' + uid + '" class="di_gui_navsubmenu" style="display:none;"></div>';
    html_data += '<form id="divcsvglfrm_' + uid + '" method="POST" style="display:none;"><input type="hidden" name="filename" id="filename_' + uid + '"><input type="hidden" name="type" id="type_' + uid + '"><input type="hidden" name="width" id="width_' + uid + '"><input type="hidden" name="svg" id="svg_' + uid + '"></form>';
    html_data += '<div id="di_vcsubmenu_saveexport' + uid + '" class="di_gui_navsubmenu" style="display:none;"><div id="main_box"><div class="di_vcsec_box3" style="width:180px"><table width="100%" height="100%" cellpadding="0" cellspacing="0"><tr><td valign="top" height="99" style="padding:0px 5px 0px 5px;"><ul class="img_pointr"><li><img src="' + di_vc_libpath + 'images/save_gallery.png" width="16px" height="16px" style="padding-right:10px;" id="idicolor1' + uid + '"/><a href="javascript:void(0)" onclick="saveToGalPopUp()" class="di_vcclicklink">Save to gallery</a><div id="divcsvGalpop" class="di_vctogglediv" style="display:none;"><div>Name<br/><input type="text" id="divc_pname' + uid + '" class="di_vcdrop_box"></div><div>Description<br/><textarea id="divc_pdesc' + uid + '" cols="30" rows="4"></textarea></div><br/><div>Tag<br/><input type="text" id="divc_ptag' + uid + '" class="di_vcdrop_box"></div><br/><br/><div style="text-align:right;"><input type="button" value="OK" onclick=\'saveToGallary("' + uid + '", "' + di_vc_libpath + 'library/PHP/exporting-server/")\' class="di_vcclickbttn">&nbsp;<input type="button" value="Cancel" onclick="saveToGalPopUp()" class="di_vcclickbttn"></div></div></li><li><img src="' + di_vc_libpath + 'images/share_compn.png" width="16px" height="16px" style="padding-right:10px;" id="idicolor1' + uid + '"/><a href="javascript:void(0)" onclick=\'di_shareVisualizer("' + uid + '","' + storagePath + '")\' class="di_vcclicklink">Share</a></li><li><img src="' + di_vc_libpath + 'images/print_compn.png" width="16px" height="16px" style="padding-right:10px;" id="idicolor1' + uid + '"/><a href="javascript:void(0)" onclick=\'di_printChart("' + uid + '")\' class="di_vcclicklink">Print</a></li></ul></td></tr><tr><td height="20" class="di_vcsec_box_bttmtxt">Save</td></tr></table></div><div class="di_vcsec_box3" style="width:589px;"><table width="100%" height="100%" cellpadding="0" cellspacing="0"><tr><td valign="top" height="99" style="padding:0px 5px 0px 5px;"><ul><li style="padding:2px;"><a href="javascript:void(0)" onclick=\'di_exportImage("' + uid + '","image/png","' + chartType + '")\' class="di_vcclicklink">Download PNG Image</a></li><li style="padding:2px;"><a href="javascript:void(0)" onclick=\'di_exportImage("' + uid + '","image/jpeg","' + chartType + '")\' class="di_vcclicklink">Download JPEG Image</a></li><li style="padding:2px;"><a href="javascript:void(0)" onclick=\'di_exportImage("' + uid + '","image/png","' + chartType + '")\' class="di_vcclicklink">Download PDF Document</a></li><li style="padding:2px;"><a href="javascript:void(0)" onclick=\'di_exportImage("' + uid + '","image/svg+xml","' + chartType + '")\' class="di_vcclicklink" class="di_vcclicklink">Download SVG Vector Image</a></li></ul></td></tr><tr><td height="20" class="di_vcsec_box_bttmtxt">Export</td></tr></table></div></div></div>';
    html_data += '<div id="di_visulcontainer_map' + uid + '" style="height:' + visu_height + 'px;display:none;"><iframe width="100%" height="100%" scrolling="no" frameborder="0" name="divcframe' + uid + '" id="divcframe' + uid + '" marginheight="0" marginwidth="0" src=""></iframe></div>';
    html_data += '<div style="height:10px;">&nbsp;</div>';
    html_data += '<div id="di_visulcontainer_chart' + uid + '" style="height:' + (visu_height - 10) + 'px;width:"' + width + 'px;"></div>';
    html_data += '<div id="di_visulcontainer_grid' + uid + '" style="height:' + (visu_height - 10) + 'px;display:none;overflow:auto;"></div>';
    html_data += '<div></div>';
    return html_data;
}

function di_toggleContainer(uid, type) {
    if (type == 'c') {
        di_jq('#di_visulcontainer_grid' + uid).hide();
        di_jq('#di_visulcontainer_chart' + uid).show();
    } else if (type == 'g') {
        di_jq('#di_visulcontainer_grid' + uid).show();
        di_jq('#di_visulcontainer_chart' + uid).hide();
    }
}

function di_initChart(fileUrl, title, uid, visul_div_id, data, datatype, x_title, y_title, subtitle, chartType, chartCategory, sourceText) {
    if (fileUrl == null || fileUrl == "") {
        switch (chartType) {
            case "treemap":
                di_create_treemap_chart(title, subtitle, sourceText, uid, visul_div_id, data, chartType);
                break;
            case "radar":
                di_create_radar_chart(title, uid, visul_div_id, data, datatype, x_title, y_title, subtitle, null, chartType, chartCategory, sourceText);
                break;
            case "cloud":
                di_create_cloud_chart(title, subtitle, sourceText, uid, visul_div_id, data, chartType);
                break;
            case "scatter3d":
                di_create_scatter3d_chart(title, uid, visul_div_id, data, datatype, x_title, y_title, subtitle, null, chartType, chartCategory, sourceText);
                break;
        }
    } else {
        di_jq.get(fileUrl, function (defaultSettings) {
            switch (chartType) {
                case "column":
                    di_create_column_chart(title, uid, visul_div_id, data, datatype, x_title, y_title, subtitle, defaultSettings, chartType, chartCategory, sourceText);
                    break;
                case "line":
                    di_create_line_chart(title, uid, visul_div_id, data, datatype, x_title, y_title, subtitle, defaultSettings, chartType, chartCategory, sourceText);
                    break;
                case "bar":
                    di_create_bar_chart(title, uid, visul_div_id, data, datatype, x_title, y_title, subtitle, defaultSettings, chartType, chartCategory, sourceText);
                    break;
                case "area":
                    di_create_area_chart(title, uid, visul_div_id, data, datatype, x_title, y_title, subtitle, defaultSettings, chartType, chartCategory, sourceText);
                    break;
                case "pie":
                    di_create_pie_chart(title, uid, visul_div_id, data, datatype, x_title, y_title, subtitle, defaultSettings, chartType, sourceText);
                    break;
                case "donut":
                    di_create_donut_chart(title, uid, visul_div_id, data, datatype, x_title, y_title, subtitle, defaultSettings, chartType, sourceText);
                    break;
                case "treemap":
                    di_create_treemap_chart(title, subtitle, sourceText, uid, visul_div_id, data, chartType);
                    break;
                case "pyramid":
                    di_create_pyramid_chart(title, uid, visul_div_id, data, datatype, x_title, y_title, subtitle, defaultSettings, chartType, chartCategory, sourceText);
                    break;
                case "radar":
                    di_create_radar_chart(title, uid, visul_div_id, data, datatype, x_title, y_title, subtitle, defaultSettings, chartType, chartCategory, sourceText);
                    break;
                case "stackcolumn":
                    di_create_column_chart(title, uid, visul_div_id, data, datatype, x_title, y_title, subtitle, defaultSettings, chartType, chartCategory, sourceText);
                    break;
                case "stackline":
                    di_create_line_chart(title, uid, visul_div_id, data, datatype, x_title, y_title, subtitle, defaultSettings, chartType, chartCategory, sourceText);
                    break;
                case "stackbar":
                    di_create_bar_chart(title, uid, visul_div_id, data, datatype, x_title, y_title, subtitle, defaultSettings, chartType, chartCategory, sourceText);
                    break;
                case "stackarea":
                    di_create_area_chart(title, uid, visul_div_id, data, datatype, x_title, y_title, subtitle, defaultSettings, chartType, chartCategory, sourceText);
                    break;
                case "100stackcolumn":
                    di_create_column_chart(title, uid, visul_div_id, data, datatype, x_title, y_title, subtitle, defaultSettings, chartType, chartCategory, sourceText);
                    break;
                case "100stackline":
                    di_create_line_chart(title, uid, visul_div_id, data, datatype, x_title, y_title, subtitle, defaultSettings, chartType, chartCategory, sourceText);
                    break;
                case "100stackbar":
                    di_create_bar_chart(title, uid, visul_div_id, data, datatype, x_title, y_title, subtitle, defaultSettings, chartType, chartCategory, sourceText);
                    break;
                case "100stackarea":
                    di_create_area_chart(title, uid, visul_div_id, data, datatype, x_title, y_title, subtitle, defaultSettings, chartType, chartCategory, sourceText);
                    break;
                case "scatter2d":
                    di_create_scatter_chart(title, uid, visul_div_id, data, datatype, x_title, y_title, subtitle, defaultSettings, chartType, chartCategory, sourceText);
                    break;
                case "scatter3d":
                    di_create_scatter3d_chart(title, uid, visul_div_id, data, datatype, x_title, y_title, subtitle, defaultSettings, chartType, chartCategory, sourceText);
                    break;
            }
        });
    }
}

function di_vcChangeIconStale() {
    var valOpacity = '0.6';
    di_jq(".di_vc_icon").css('opacity', valOpacity);
    di_jq(".di_vc_icon").hover(function () {
        di_jq(this).css('opacity', '1');
    }, function () {
        if (di_jq(this).attr('class').indexOf('di_vc_selicon') == -1) {
            di_jq(this).css('opacity', valOpacity);
        }
    });
    di_jq(".di_vc_icon").click(function () {
        di_jq(".di_vc_icon").removeClass('di_vc_selicon');
        di_jq(".di_vc_icon").css('opacity', valOpacity);
        di_jq(this).css('opacity', '1');
        di_jq(this).addClass('di_vc_selicon');
    });
    di_jq(".di_vc_lblicon").css('opacity', valOpacity);
    di_jq(".di_vc_lblicon").hover(function () {
        di_jq(this).css('opacity', '1');
    }, function () {
        if (di_jq(this).attr('class').indexOf('di_vc_selicon') == -1) {
            di_jq(this).css('opacity', valOpacity);
        }
    });
    di_jq(".di_vc_lblicon").click(function () {
        di_jq(".di_vc_lblicon").removeClass('di_vc_selicon');
        di_jq(".di_vc_lblicon").css('opacity', valOpacity);
        di_jq(this).css('opacity', '1');
        di_jq(this).addClass('di_vc_selicon');
    });
    di_jq(".di_vc_sformaticon").css('opacity', valOpacity);
    di_jq(".di_vc_sformaticon").hover(function () {
        di_jq(this).css('opacity', '1');
    }, function () {
        if (di_jq(this).attr('class').indexOf('di_vc_selicon') == -1) {
            di_jq(this).css('opacity', valOpacity);
        }
    });
    di_jq(".di_vc_sformaticon").click(function () {
        di_jq(".di_vc_sformaticon").removeClass('di_vc_selicon');
        di_jq(".di_vc_sformaticon").css('opacity', valOpacity);
        di_jq(this).css('opacity', '1');
        di_jq(this).addClass('di_vc_selicon');
    });
}

function di_replaceAll(Source, stringToFind, stringToReplace) {
    var temp = Source;
    var index = -1;
    if (temp) {
        index = temp.indexOf(stringToFind);
        while (index != -1) {
            temp = temp.replace(stringToFind, stringToReplace);
            index = temp.indexOf(stringToFind);
        }
    }
    return temp;
}

function getChartSettingsObject(chartInput, chartType) {
    var $category$, $plotOptionId$, $seriesData$;
    var newSettings = {
        colors: chartInput.colors,
        chart: {
            renderTo: "$renderDivId$",
            zoomType: "x",
            defaultSeriesType: chartInput.chart.defaultSeriesType,
            borderWidth: chartInput.chart.borderWidth,
            borderColor: chartInput.chart.borderColor,
            backgroundColor: chartInput.chart.backgroundColor,
            plotBackgroundColor: chartInput.chart.plotBackgroundColor,
            plotBorderWidth: chartInput.chart.plotBorderWidth,
            plotBorderColor: chartInput.chart.plotBorderColor,
            spacingBottom: chartInput.chart.spacingBottom,
            events: {
                load: chartInput.chart.events.load
            }
        },
        xAxis: {
            categories: "$category$",
            gridLineColor: chartInput.xAxis.gridLineColor,
            gridLineDashStyle: chartInput.xAxis.gridLineDashStyle,
            gridLineWidth: chartInput.xAxis.gridLineWidth,
            lineColor: chartInput.xAxis.lineColor,
            lineWidth: chartInput.xAxis.lineWidth,
            minorGridLineColor: chartInput.xAxis.minorGridLineColor,
            minorGridLineDashStyle: chartInput.xAxis.minorGridLineDashStyle,
            minorGridLineWidth: chartInput.xAxis.minorGridLineWidth,
            title: {
                text: "$xAxisTitle$",
                style: {
                    font: chartInput.xAxis.title.style.font,
                    color: chartInput.xAxis.title.style.color,
                    fontWeight: chartInput.xAxis.title.style.fontWeight,
                    textDecoration: chartInput.xAxis.title.style.textDecoration
                }
            },
            maxZoom: chartInput.xAxis.maxZoom,
            min: chartInput.xAxis.min,
            labels: {
                rotation: chartInput.xAxis.labels.rotation,
                align: chartInput.xAxis.labels.align,
                style: {
                    font: chartInput.xAxis.labels.style.font,
                    color: chartInput.xAxis.labels.style.color,
                    fontWeight: chartInput.xAxis.labels.style.fontWeight,
                    textDecoration: chartInput.xAxis.labels.style.textDecoration
                },
                x: chartInput.xAxis.labels.x,
                y: chartInput.xAxis.labels.y
            }
        },
        yAxis: {
            gridLineColor: chartInput.yAxis.gridLineColor,
            gridLineDashStyle: chartInput.yAxis.gridLineDashStyle,
            gridLineWidth: chartInput.yAxis.gridLineWidth,
            lineColor: chartInput.yAxis.lineColor,
            lineWidth: chartInput.yAxis.lineWidth,
            minorGridLineColor: chartInput.yAxis.minorGridLineColor,
            minorGridLineDashStyle: chartInput.yAxis.minorGridLineDashStyle,
            minorGridLineWidth: chartInput.yAxis.minorGridLineWidth,
            title: {
                text: "$yAxisTitle$",
                style: {
                    font: chartInput.yAxis.title.style.font,
                    color: chartInput.yAxis.title.style.color,
                    fontWeight: chartInput.yAxis.title.style.fontWeight,
                    textDecoration: chartInput.yAxis.title.style.textDecoration
                }
            },
            min: chartInput.yAxis.min,
            maxZoom: chartInput.yAxis.maxZoom,
            labels: {
                rotation: chartInput.yAxis.labels.rotation,
                align: chartInput.yAxis.labels.align,
                style: {
                    font: chartInput.yAxis.labels.style.font,
                    color: chartInput.yAxis.labels.style.color,
                    fontWeight: chartInput.yAxis.labels.style.fontWeight,
                    textDecoration: chartInput.yAxis.labels.style.textDecoration
                },
                x: chartInput.yAxis.labels.x,
                y: chartInput.yAxis.labels.y
            }
        },
        title: {
            text: "$chartTitle$",
            style: {
                font: chartInput.title.style.font,
                color: chartInput.title.style.color,
                fontWeight: chartInput.title.style.fontWeight,
                textDecoration: chartInput.title.style.textDecoration
            }
        },
        subtitle: {
            text: "$chartSubTitle$",
            style: {
                font: chartInput.subtitle.style.font,
                color: chartInput.subtitle.style.color,
                fontWeight: chartInput.subtitle.style.fontWeight,
                textDecoration: chartInput.subtitle.style.textDecoration
            }
        },
        legend: {
            borderWidth: chartInput.legend.borderWidth,
            align: chartInput.legend.align,
            verticalAlign: chartInput.legend.verticalAlign,
            backgroundColor: chartInput.legend.backgroundColor,
            layout: chartInput.legend.layout,
            enabled: chartInput.legend.enabled,
            floating: chartInput.legend.floating,
            itemStyle: {
                font: chartInput.legend.itemStyle.font,
                color: chartInput.legend.itemStyle.color,
                fontWeight: chartInput.legend.itemStyle.fontWeight,
                textDecoration: chartInput.legend.itemStyle.textDecoration
            },
            symbolWidth: chartInput.legend.symbolWidth,
            itemWidth: chartInput.legend.itemWidth,
            labelFormatter: null,
            x: chartInput.legend.x,
            y: chartInput.legend.y
        },
        tooltip: {
            backgroundColor: chartInput.tooltip.backgroundColor,
            borderRadius: chartInput.tooltip.borderRadius,
            borderWidth: chartInput.tooltip.borderWidth,
            formatter: null,
            shared: chartInput.tooltip.shared,
            style: {
                font: chartInput.tooltip.style.font,
                color: chartInput.tooltip.style.color,
                fontWeight: chartInput.tooltip.style.fontWeight,
                textDecoration: chartInput.tooltip.style.textDecoration
            }
        },
        plotOptions: "$plotOptionId$",
        series: "$seriesData$",
        exporting: {
            enabled: false
        }
    }
    return newSettings;
}

function saveToGalPopUp() {
    di_jq('#divcsvGalpop').slideToggle();
}

function saveToGallary(uid, service_path) {
    if (di_jq('#divc_pname' + uid).val() == '') {
        alert("Plaese enter presentation name.");
        di_jq('#divc_pname' + uid).focus();
        return false;
    }
    if (di_jq('#divc_pdesc' + uid).val() == '') {
        alert("Plaese enter presentation description.");
        di_jq('#divc_pdesc' + uid).focus();
        return false;
    }
    if (di_jq('#divc_ptag' + uid).val() == '') {
        alert("Plaese enter presentation tag.");
        di_jq('#divc_ptag' + uid).focus();
        return false;
    }
    var imgType = "image/jpeg";
    var chartObj = di_getChartObject(uid);
    var svg = chartObj.getSVG({
        type: imgType,
        url: service_path
    });
    var width = '100';
    var filename = 'chart';
    di_jq('#divcsvglfrm_' + uid).attr("action", service_path)
    di_jq('#filename_' + uid).val(filename);
    di_jq('#type_' + uid).val(imgType);
    di_jq('#width_' + uid).val(width);
    di_jq('#svg_' + uid).val(svg);
    di_jq('#divcsvglfrm_' + uid).submit();
    saveToGalPopUp();
}

function di_getTextDrawFunction(sourceText, sourceXPos, sourceYPos, labelText, labelXPos, labelYPos, sourceStyle, labelStyle) {
    var sourceColor, sourceFont, sourceWeight, sourceDecoration, labelColor, labelFont, labelWeight, labelDecoration;
    sourceColor = labelColor = "#4572A7";
    sourceFont = labelFont = "normal 11px arial";
    sourceWeight = labelWeight = "normal";
    sourceDecoration = labelDecoration = "none";
    if (sourceStyle != null) {
        sourceColor = sourceStyle.color;
        sourceFont = sourceStyle.font;
        sourceWeight = sourceStyle.fontWeight;
        sourceDecoration = sourceStyle.textDecoration;
    }
    if (labelStyle != null) {
        labelColor = labelStyle.color;
        labelFont = labelStyle.font;
        labelWeight = labelStyle.fontWeight;
        labelDecoration = labelStyle.textDecoration;
    }
    var funObj = function (chart) {
        var chart = this,
            text1 = chart.renderer.text(sourceText, sourceXPos, sourceYPos).attr({
                zIndex: 5
            }).css({
                color: sourceColor,
                font: sourceFont,
                fontWeight: sourceWeight,
                textDecoration: sourceDecoration
            }).add();
        var sourceBox = text1.getBBox();
        chart.renderer.rect(sourceBox.x - 5, sourceBox.y - 5, sourceBox.width + 10, sourceBox.height + 10, 5).attr({
            border: 0
        }).add();
        text2 = chart.renderer.text(labelText, labelXPos, labelYPos).attr({
            zIndex: 5
        }).css({
            color: labelColor,
            font: labelFont,
            fontWeight: labelWeight,
            textDecoration: labelDecoration
        }).add();
        var labelBox = text2.getBBox();
        chart.renderer.rect(labelBox.x - 5, labelBox.y - 5, labelBox.width + 10, labelBox.height + 10, 5).attr({
            border: 0
        })
    }
    return funObj;
}

function di_setVisualizerData(uid, vcData, chartType, chartTitle) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    var sourceStyleObj = di_getSourceTextStyle(uid);
    var labelStyleObj = di_getLabelTextStyle(uid);
    var labelText = di_getLabelText(uid);
    var sourceText = di_getSourceText(uid);
    var sourcePos = di_getSourcePosition(uid);
    var customPos = di_getCustomLabelPosition(uid);
    var functionObj = di_getTextDrawFunction(sourceText, sourcePos.x, sourcePos.y, labelText, customPos.x, customPos.y, sourceStyleObj, labelStyleObj);
    chartInput.chart.events.load = functionObj;
    if (chartType.toLowerCase() == "pie") {
        var pieData = di_getInputDataForPieChart(vcData);
        chartInput.series = [pieData[0].seriesData];
        chartInput.title.text = chartTitle;
        chartObj = new Highcharts.Chart(chartInput);
    } else {
        var newChartObj = new Object();
        newChartObj.colors = chartInput.colors;
        newChartObj.chart = chartInput.chart;
        newChartObj.xAxis = chartInput.xAxis;
        newChartObj.xAxis.categories = vcData.categoryCollection;
        newChartObj.yAxis = chartInput.yAxis;
        newChartObj.plotOptions = chartInput.plotOptions;
        chartInput.title.text = chartTitle;
        newChartObj.title = chartInput.title;
        newChartObj.subtitle = chartInput.subtitle;
        newChartObj.series = vcData.seriesCollection;
        newChartObj.tooltip = chartInput.tooltip;
        newChartObj.legend = new Object();
        newChartObj.legend.verticalAlign = chartInput.legend.verticalAlign;
        newChartObj.legend.layout = chartInput.legend.layout;
        newChartObj.legend.align = chartInput.legend.align;
        newChartObj.exporting = chartInput.exporting;
        newChartObj = di_labelRotationSettings(vcData, newChartObj, chartType);
        newChartObj = new Highcharts.Chart(newChartObj);
        di_setChartObject(uid, newChartObj);
    }
}

function initColorPickers(uid, chartType) {
    di_jq('#dicolor1' + uid).ColorPicker({
        onSubmit: function (hsb, hex, rgb, el) {
            di_jq(el).ColorPickerHide();
            di_jq('#inp_dicolor1' + uid).css('background-color', '#' + hex);
            di_changeChartBgColor(uid, '#' + hex);
        }
    });
    di_jq('#dicolor2' + uid).ColorPicker({
        onSubmit: function (hsb, hex, rgb, el) {
            di_jq(el).ColorPickerHide();
            di_jq('#inp_dicolor2' + uid).css('background-color', '#' + hex);
            di_changePlotBgColor(uid, '#' + hex);
        }
    });
    di_jq('#dicolor3' + uid).ColorPicker({
        onSubmit: function (hsb, hex, rgb, el) {
            di_jq(el).ColorPickerHide();
            di_jq('#inp_dicolor3' + uid).css('background-color', '#' + hex);
            di_applyLegendBgColor(uid, '#' + hex);
        }
    });
    di_jq('#dicolor4' + uid).ColorPicker({
        onSubmit: function (hsb, hex, rgb, el) {
            di_jq(el).ColorPickerHide();
            di_jq('#inp_dicolor4' + uid).css('background-color', '#' + hex);
            di_selectSeriesColor(uid, '#' + hex);
        }
    });
    di_jq('#dicolor5' + uid).ColorPicker({
        onSubmit: function (hsb, hex, rgb, el) {
            di_jq(el).ColorPickerHide();
            di_jq('#inp_dicolor5' + uid).css('background-color', '#' + hex);
            var index = document.getElementById('di_vcTitlesList').selectedIndex;
            di_applyTitleFontColor(uid, '#' + hex, index);
        }
    });
    di_jq('#dicolor6' + uid).ColorPicker({
        onSubmit: function (hsb, hex, rgb, el) {
            di_jq(el).ColorPickerHide();
            di_jq('#inp_dicolor6' + uid).css('background-color', '#' + hex);
            di_applyAxisLabelFontColor(uid, '#' + hex, chartType);
        }
    });
    di_jq('#dicolor7' + uid).ColorPicker({
        onSubmit: function (hsb, hex, rgb, el) {
            di_jq(el).ColorPickerHide();
            di_jq('#inp_dicolor7' + uid).css('background-color', '#' + hex);
            di_applyLegendFontColor(uid, '#' + hex);
        }
    });
    di_jq('#dicolor8' + uid).ColorPicker({
        onSubmit: function (hsb, hex, rgb, el) {
            di_jq(el).ColorPickerHide();
            di_jq('#inp_dicolor8' + uid).css('background-color', '#' + hex);
            di_onchangeSeriesBorderColor(uid, '#' + hex, chartType);
        }
    });
}

function di_get_random_color(di_tree_themeValue) {
    var color;
    switch (di_tree_themeValue.toLowerCase()) {
        case "randam":
            var letters = '0123456789ABCDEF'.split('');
            color = '#';
            for (var i = 0; i < 6; i++) {
                color += letters[Math.round(Math.random() * 15)];
            }
            break;
        case "2":
            var blueColors = "#A4D3EE,#82CFFD,#67C8FF,#B0E2FF,#87CEFA,#6CA6CD,#4A708B,#9BC4E2,#7EC0EE,#87CEFF,#517693,#5D7B93,#42647F,#4682B4,#4F94CD,#5CACEE,#63B8FF,#525C65,#36648B,#62B1F6,#74BBFB,#F0F8FF,#4E78A0,#708090,#708090,#778899,#778899,#6183A6,#9FB6CD,#7D9EC0,#104E8B,#1874CD,#1C86EE,#60AFFE,#007FFF,#1E90FF,#6C7B8B,#B7C3D0,#739AC5,#75A1D0,#B9D3EE,#499DF5,#C6E2FF,#3B6AA0,#7AA9DD,#0276FD,#003F87,#6E7B8B,#506987,#A2B5CD,#4372AA,#26466D,#1D7CF2,#687C97,#344152,#50729F,#4973AB,#B0C4DE,#3063A5,#BCD2EE,#7EB6FF,#CAE1FF,#4D71A3,#2B4F81,#4981CE,#88ACE0,#5993E5,#3A66A7,#3579DC,#5190ED,#42526C,#4D6FAC,#2C5197,#6495ED,#6D9BF1,#5B90F6,#1464F4,#3A5894,#7093DB,#1B3F8B,#0147FA,#3D59AB,#27408B,#3A5FCD,#4169E1,#436EEE,#003EFF,#4876FF,#A9ACB6,#22316C,#3B4990,#283A90,#6F7285,#838EDE,#E6E8FA,#2F2F4F,#42426F,#8F8FBC,#5959AB,#7171C6,#D9D9F3,#23238E,#3232CC,#3232CD,#191970,#E6E6FA,#000033,#000080,#00008,#00009C,#0000CD,#0000EE,#0000FF,#3333FF,#4D4DFF,#6666FF,#AAAAFF,#CCCCFF,#F8F8FF,#5B59BA,#302B54,#483D8B,#473C8B,#3B3178,#6A5ACD,#6959CD,#7A67EE,#8470FF,#836FFF,#7B68EE,#3300FF,#5D478B,#9F79EE,#8968CD,#9370DB,#AB82FF,#6600FF,#380474".split(',');
            color = blueColors[Math.round(Math.random() * 90)];
            break;
        case "4":
            var redColors = "#660000,#E60000,#FF6666,#800000,#F70000,#FF6F6F,#8B0000,#FE0000,#FF7272,#CD0000,#FF2B2B,#FF8888,#EE0000,#FF4141,#FF9393,#FF0000,#FF4D4D,#FF9999,#FF3030,#FF6D6D,#FFA9A9,#FF3333,#FF6F6F,#FFAAAA,#AF4035,#D17268,#E5AFAA,#ECC3BF,#F0D1CE,#F5DFDD,#CC1100,#FF3C2A,#FF9288,#EED5D2,#F1DDDB,#F5E5E4,#FA8072,#FBA399,#FDC6C0,#FFE4E1,#FFE5E2,#FFE7E4,#8B7D7B,#AEA4A2,#D0CBCA,#FF2400,#FF664D,#FFA799,#8A3324,#CF5B47,#E4A499,#CD4F39,#DC8474,#EBB8AF,#EE5C42,#F38C79,#F8BBB0,#FF5333,#FF856F,#FFB7AA,#FF6347,#FF907C,#FFBDB1,#8B3E2F,#C66957,#DFACA2,#8B3626,#CE5F4A,#E4A69A,#CD5B45,#DC8C7C,#EBBDB4,#EE6A50,#F39583,#F8C0B5,#FF7256,#FF9A86,#FFC2B6,#D43D1A,#EA7256,#F3AF9F,#FF3300,#FF704D,#FFAD99,#FF3D0D,#FF7755,#FFB19D,#8B4C39,#C07A65,#DCB5A9,#CD7054,#DC9A86,#EAC4B9,#C73F17,#EA6F4B,#F3AE9A,#EE8262,#F3A68F,#F8C9BB,#FF8C69,#FFAC92,#FFCCBC,#A78D84,#C1AEA8,#DBD0CC,#E9967A,#EFB39F,#F5D0C4,#FF5721,#FF8962,#FFBAA4,#5E2612,#C65026,#E79C81,#8B2500,#FE4400,#FF9872,#CD3700,#FF642B,#FFA888".split(',');
            color = redColors[Math.round(Math.random() * 70)];
            break;
    }
    return color;
}

function di_getChTitle(uid, selectVal) {
    var returnVal = "";
    if (selectVal != '-1') {
        if (selectVal == "8") {
            returnVal = di_getSourceText(uid);
        } else if (selectVal == "9") {
            returnVal = di_getLabelText(uid);
        } else {
            var chartObj = di_getChartObject(uid);
            var titleObj = di_getTitleObj(chartObj, selectVal);
            returnVal = titleObj.text;
        }
    }
    if (returnVal.style) {
        returnVal = returnVal.text;
    }
    return returnVal;
}

function di_getSeriesNames(uid) {
    var seriesNameArray = [""];
    var chartObj = di_getChartObject(uid);
    if (chartObj) {
        var chartInput = chartObj.options;
        for (var i = 0; i < chartInput.series.length; i++) {
            seriesNameArray.push(chartInput.series[i].name);
        }
    }
    return seriesNameArray;
}

function di_toggleLabel(uid, index, isEnabled) {
    if (index == "-1" || index == "7")
        return;
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    if (index == "4") {
        if (chartInput.xAxis.length) {
            chartInput.xAxis[0].labels.enabled = isEnabled;
        } else {
            chartInput.xAxis.labels.enabled = isEnabled;
        }
    } else if (index == "6") {
        chartInput.yAxis.labels.enabled = isEnabled;
    }
    chartObj = new Highcharts.Chart(chartInput);
}

function di_dataLabelsPosition(uid, val, chartType) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    switch (chartType) {
        case "column":
            chartInput.plotOptions.column.dataLabels.y = val;
            break;
        case "line":
            chartInput.plotOptions.line.dataLabels.y = val;
            break;
        case "area":
            chartInput.plotOptions.area.dataLabels.y = val;
            break;
        case "bar":
            chartInput.plotOptions.bar.dataLabels.x = val;
            break;
        case "pie":
            chartInput.plotOptions.pie.dataLabels.y = val;
            break;
        case "pyramid":
            chartInput.plotOptions.series.dataLabels.x = val;
            break;
        case "stackcolumn":
            chartInput.plotOptions.column.dataLabels.y = val;
            break;
        case "stackline":
            chartInput.plotOptions.line.dataLabels.y = val;
            break;
        case "stackarea":
            chartInput.plotOptions.area.dataLabels.y = val;
            break;
        case "stackbar":
            chartInput.plotOptions.bar.dataLabels.x = val;
            break;
        case "100stackcolumn":
            chartInput.plotOptions.column.dataLabels.y = val;
            break;
        case "100stackline":
            chartInput.plotOptions.line.dataLabels.y = val;
            break;
        case "100stackarea":
            chartInput.plotOptions.area.dataLabels.y = val;
            break;
        case "100stackbar":
            chartInput.plotOptions.bar.dataLabels.x = val;
            break;
        case "scatter":
            chartInput.plotOptions.scatter.dataLabels.x = val;
            break;
    }
    chartObj = new Highcharts.Chart(chartInput);
}

function di_customLabelPosition(uid) {
    var chartObj = di_getChartObject(uid);
    var sourceStyleObj = di_getSourceTextStyle(uid);
    var labelStyleObj = di_getLabelTextStyle(uid);
    var labelText = di_getLabelText(uid);
    var sourceText = di_getSourceText(uid);
    var sourcePos = di_getSourcePosition(uid);
    var labelPos = di_getCustomLabelPosition(uid);
    var functionObj = di_getTextDrawFunction(sourceText, sourcePos.x, sourcePos.y, labelText, labelPos.x, labelPos.y, sourceStyleObj, labelStyleObj);
    chartObj.options.chart.events.load = functionObj;
    chartObj = new Highcharts.Chart(chartObj.options);
}

function di_changeSourcePosition(uid) {
    var chartObj = di_getChartObject(uid);
    var sourceStyleObj = di_getSourceTextStyle(uid);
    var labelStyleObj = di_getLabelTextStyle(uid);
    var labelText = di_getLabelText(uid);
    var sourceText = di_getSourceText(uid);
    var sourcePos = di_getSourcePosition(uid);
    var labelPos = di_getCustomLabelPosition(uid);
    var functionObj = di_getTextDrawFunction(sourceText, sourcePos.x, sourcePos.y, labelText, labelPos.x, labelPos.y, sourceStyleObj, labelStyleObj);
    chartObj.options.chart.events.load = functionObj;
    chartObj = new Highcharts.Chart(chartObj.options);
}

function di_getChartBorderWidth(uid) {
    var borderWidth;
    try {
        var chartObj = di_getChartObject(uid);
        var chartInput = chartObj.options;
        borderWidth = chartInput.chart.borderWidth;
    } catch (err) { }
    return borderWidth;
}

function di_getChartXGridLine(uid) {
    var chartObj = di_getChartObject(uid);
    var border = chartObj.options.xAxis.gridLineWidth;
    return border;
}

function di_getChartYGridLine(uid) {
    var chartObj = di_getChartObject(uid);
    var border = chartObj.options.yAxis.gridLineWidth;
    return border;
}

function di_dataLabelStatus(uid, chartType) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    var isDataLabelOn;
    switch (chartType) {
        case "column":
            isDataLabelOn = chartInput.plotOptions.column.dataLabels.enabled;
            break;
        case "line":
            isDataLabelOn = chartInput.plotOptions.line.dataLabels.enabled;
            break;
        case "area":
            isDataLabelOn = chartInput.plotOptions.area.dataLabels.enabled;
            break;
        case "bar":
            isDataLabelOn = chartInput.plotOptions.bar.dataLabels.enabled;
            break;
        case "pie":
            isDataLabelOn = chartInput.plotOptions.pie.dataLabels.enabled;
            break;
        case "pyramid":
            isDataLabelOn = chartInput.series[0].dataLabels.enabled;
            break;
        case "stackcolumn":
            isDataLabelOn = chartInput.plotOptions.column.dataLabels.enabled;
            break;
        case "stackline":
            isDataLabelOn = chartInput.plotOptions.line.dataLabels.enabled;
            break;
        case "stackarea":
            isDataLabelOn = chartInput.plotOptions.area.dataLabels.enabled;
            break;
        case "stackbar":
            isDataLabelOn = chartInput.plotOptions.bar.dataLabels.enabled;
            break;
        case "100stackcolumn":
            isDataLabelOn = chartInput.plotOptions.column.dataLabels.enabled;
            break;
        case "100stackline":
            isDataLabelOn = chartInput.plotOptions.line.dataLabels.enabled;
            break;
        case "100stackarea":
            isDataLabelOn = chartInput.plotOptions.area.dataLabels.enabled;
            break;
        case "100stackbar":
            isDataLabelOn = chartInput.plotOptions.bar.dataLabels.enabled;
            break;
        case "scatter2d":
            isDataLabelOn = chartInput.plotOptions.scatter.dataLabels.enabled;
            break;
    }
    return isDataLabelOn;
}

function di_legendVisibility(uid) {
    var isShow;
    var chartObj = di_getChartObject(uid);
    var legendObj = di_getChartLegendObject(chartObj);
    isShow = legendObj.enabled;
    return isShow;
}

function di_getChartBgColor(uid) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    var color = chartInput.chart.backgroundColor;
    if (color.linearGradient) {
        var colors = color.stops[0];
        var color1 = colors[1];
        colors = color.stops[1];
        var color2 = colors[1];
        color = color1 + "~" + color2;
    }
    return color;
}

function di_getChartPlotAreaColor(uid) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    var color = chartInput.chart.plotBackgroundColor;
    try {
        if (color.linearGradient) {
            var colors = color.stops[0];
            var color1 = colors[1];
            colors = color.stops[1];
            var color2 = colors[1];
            color = color1 + "~" + color2;
        }
    } catch (err) { }
    return color;
}

function di_getAllTitleSettings(uid, itemValue) {
    var chartObj = di_getChartObject(uid);
    var styleObj, color, fontsize, fontWeight, fontStyle, underline;
    var allSettings = [];
    var treeMapTitleObj;
    switch (itemValue) {
        case "1":
            if (chartObj.options) {
                styleObj = di_getChartTitleStyle(chartObj);
                fontsize = styleObj.font.split(" ")[1];
                color = styleObj.color;
                fontWeight = styleObj.fontWeight;
                fontStyle = styleObj.font.split(" ")[0];
                underline = styleObj.textDecoration;
            } else {
                treeMapTitleObj = document.getElementById('di_vc_title');
                fontsize = treeMapTitleObj.style.fontSize;
                color = treeMapTitleObj.style.color;
                fontWeight = treeMapTitleObj.style.fontWeight;
                fontStyle = treeMapTitleObj.style.font.split(" ")[0];
                underline = treeMapTitleObj.style.textDecoration;
            }
            break;
        case "2":
            if (chartObj.options) {
                styleObj = di_getChartSubTitleStyle(chartObj);
                fontsize = styleObj.font.split(" ")[1];
                color = styleObj.color;
                fontWeight = styleObj.fontWeight;
                fontStyle = styleObj.font.split(" ")[0];
                underline = styleObj.textDecoration;
            } else {
                treeMapTitleObj = document.getElementById('di_vc_subtitle');
                fontsize = treeMapTitleObj.style.fontSize;
                color = treeMapTitleObj.style.color;
                fontWeight = treeMapTitleObj.style.fontWeight;
                fontStyle = treeMapTitleObj.style.font.split(" ")[0];
                underline = treeMapTitleObj.style.textDecoration;
            }
            break;
        case "3":
            styleObj = di_getXaxisTitleStyle(chartObj);
            fontsize = styleObj.font.split(" ")[1];
            color = styleObj.color;
            fontWeight = styleObj.fontWeight;
            fontStyle = styleObj.font.split(" ")[0];
            underline = styleObj.textDecoration;
            break;
        case "5":
            styleObj = di_getYaxisTitleStyle(chartObj);
            fontsize = styleObj.font.split(" ")[1];
            color = styleObj.color;
            fontWeight = styleObj.fontWeight;
            fontStyle = styleObj.font.split(" ")[0];
            underline = styleObj.textDecoration;
            break;
        case "8":
            if (chartObj.options) {
                styleObj = di_getSourceTextStyle(uid);
                fontsize = styleObj.font.split(" ")[1];
                color = styleObj.color;
                fontWeight = styleObj.fontWeight;
                fontStyle = styleObj.font.split(" ")[0];
                underline = styleObj.textDecoration;
            } else {
                treeMapTitleObj = document.getElementById('di_vc_source');
                fontsize = treeMapTitleObj.style.fontSize;
                color = treeMapTitleObj.style.color;
                fontWeight = treeMapTitleObj.style.fontWeight;
                fontStyle = treeMapTitleObj.style.font.split(" ")[0];
                underline = treeMapTitleObj.style.textDecoration;
            }
            break;
        case "9":
            styleObj = di_getLabelTextStyle(uid);
            fontsize = styleObj.font.split(" ")[1];
            color = styleObj.color;
            fontWeight = styleObj.fontWeight;
            fontStyle = styleObj.font.split(" ")[0];
            underline = styleObj.textDecoration;
            break;
    }
    allSettings.push(fontsize);
    allSettings.push(color);
    allSettings.push(fontWeight);
    allSettings.push(fontStyle);
    allSettings.push(underline);
    return allSettings;
}

function di_getAllLabelSettings(uid, itemValue, chartType) {
    var chartObj = di_getChartObject(uid);
    var styleObj;
    var allSettings = [];
    switch (itemValue) {
        case "4":
            styleObj = di_getXaxisLabelstyle(chartObj);
            break;
        case "6":
            styleObj = di_getYaxisLabelstyle(chartObj);
            break;
        case "7":
            if (chartType == "column") {
                styleObj = di_getColumnDataLabelStyle(chartObj);
            } else if (chartType == "line") {
                styleObj = di_getLineDataLabelStyle(chartObj);
            } else if (chartType == "area") {
                styleObj = di_getAreaDataLabelStyle(chartObj);
            } else if (chartType == "bar") {
                styleObj = di_getBarDataLabelStyle(chartObj);
            } else if (chartType == "pie") {
                styleObj = di_getPieDataLabelStyle(chartObj);
            } else if (chartType == "pyramid") {
                styleObj = di_getPyramidDataLabelStyle(chartObj);
            }
            if (chartType == "scatter2d") {
                styleObj = di_getScatterDataLabelStyle(chartObj);
            }
            break;
    }
    var fontsize = styleObj.font.split(" ")[1];
    var fontWeight = styleObj.fontWeight;
    var fontStyle = styleObj.font.split(" ")[0];
    var underline = styleObj.textDecoration;
    allSettings.push(fontsize);
    allSettings.push(styleObj.color);
    allSettings.push(fontWeight);
    allSettings.push(fontStyle);
    allSettings.push(underline);
    return allSettings;
}

function di_selectSeriesName(uid, index) {
    var chartObj = di_getChartObject(uid);
    var color = di_getSeriesColor(chartObj, index);
    if (color == null || color.toLowerCase() == "undefined") {
        color = chartObj.options.colors[index];
        if (color.linearGradient) {
            var colors = color.stops[0];
            var color1 = colors[1];
            colors = color.stops[1];
            var color2 = colors[1];
            color = color1 + "~" + color2;
        }
    }
    return color;
}

function di_getDataLabelsPosition(uid, chartType) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    var val;
    switch (chartType) {
        case "column":
            val = chartInput.plotOptions.column.dataLabels.y
            break;
        case "line":
            val = chartInput.plotOptions.line.dataLabels.y
            break;
        case "area":
            val = chartInput.plotOptions.area.dataLabels.y
            break;
        case "bar":
            val = chartInput.plotOptions.bar.dataLabels.x;
            break;
        case "pie":
            val = chartInput.plotOptions.pie.dataLabels.x;
            break;
        case "pyramid":
            val = chartInput.series[0].dataLabels.x;
            break;
        case "stackcolumn":
            val = chartInput.plotOptions.column.dataLabels.y
            break;
        case "stackline":
            val = chartInput.plotOptions.line.dataLabels.y
            break;
        case "stackarea":
            val = chartInput.plotOptions.area.dataLabels.y
            break;
        case "stackbar":
            val = chartInput.plotOptions.bar.dataLabels.x;
            break;
        case "100stackcolumn":
            val = chartInput.plotOptions.column.dataLabels.y
            break;
        case "100stackline":
            val = chartInput.plotOptions.line.dataLabels.y
            break;
        case "100stackarea":
            val = chartInput.plotOptions.area.dataLabels.y
            break;
        case "100stackbar":
            val = chartInput.plotOptions.bar.dataLabels.x;
            break;
        case "scatter2d":
            val = chartInput.plotOptions.scatter.dataLabels.x;
            break;
    }
    return val;
}

function di_getLabelStatus(uid, index) {
    if (index == "-1" || index == "7")
        return;
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    var isEnabled;
    if (index == "4") {
        if (chartInput.xAxis.length) {
            isEnabled = chartInput.xAxis[0].labels.enabled;
        } else {
            isEnabled = chartInput.xAxis.labels.enabled;
        }
    } else if (index == "6") {
        isEnabled = chartInput.yAxis.labels.enabled;
    }
    return isEnabled;
}

function di_reDrawChart(uid, chartObj, chartData, chartType, chartTitle, xTitle, yTitle, chartWidth) {
    var chartInput = chartObj.options;
    document.getElementById(chartInput.chart.renderTo).innerHTML = "";
    chartInput.chart.width = chartWidth;
    chartInput.title.text = chartTitle;
    var oldChartObj = di_getChartObject(uid);
    if (chartType == "pie") {
        var pieData = di_getInputDataForPieChart(chartData);
        di_setPieDataObject(chartType, pieData);
        chartInput.series = [pieData[0].seriesData];
        if (pieData.length > 0 && pieData[0].seriesData.data.length == 1) {
            chartInput.legend.enabled = false;
        }
    } else if (chartType == "pyramid") {
        var pyaramidData = di_makeDataForPyramid(chartData);
        chartInput.xAxis[0].categories = chartInput.xAxis[1].categories = pyaramidData.categoryCollection;
        var totalDataLabels = 0;
        totalDataLabels = pyaramidData.categoryCollection.length;
        var totalLegendItems = pyaramidData.seriesCollection.length;
        chartInput.series = pyaramidData.seriesCollection;
    } else if (chartType == "scatter2d") {
        var scatterData = di_makeScatterPlotData(chartData, 0, 1);
        var totalDataLabels = 0;
        totalDataLabels = chartData.categoryCollection.length;
        var totalLegendItems = chartData.seriesCollection.length;
        chartInput.series = scatterData;
        chartInput.xAxis.title.text = xTitle;
        chartInput.yAxis.title.text = yTitle;
    } else {
        var totalDataLabels = 0;
        totalDataLabels = chartData.categoryCollection.length;
        var totalLegendItems = chartData.seriesCollection.length;
        chartInput.xAxis.categories = chartData.categoryCollection;
        chartInput.series = chartData.seriesCollection;
    }
    chartInput.tooltip.style.padding = 5;
    chartInput.legend.width = null;
    var newchartObj = new Highcharts.Chart(chartInput);
    di_setChartObject(uid, newchartObj);
}

function di_labelRotationSettings(totalDataLabels, totalLegendItems, chartDefSetting, chartType) {
    chartType = chartType.toLowerCase();
    if (chartType == "column" || chartType == "line" || chartType == "area" || chartType == "scatter2d") {
        if (totalDataLabels >= 0 && totalDataLabels <= 10) {
            chartDefSetting.xAxis.labels.rotation = 360;
            chartDefSetting.xAxis.labels.align = "center";
            chartDefSetting.xAxis.labels.enabled = true;
        } else if (totalDataLabels >= 11 && totalDataLabels <= 30) {
            chartDefSetting.xAxis.labels.rotation = 315;
            chartDefSetting.xAxis.labels.align = "right";
            chartDefSetting.xAxis.labels.enabled = true;
        } else if (totalDataLabels >= 31 && totalDataLabels <= 250) {
            chartDefSetting.xAxis.labels.rotation = 270;
            chartDefSetting.xAxis.labels.align = "right";
            chartDefSetting.xAxis.labels.enabled = true;
        } else if (totalDataLabels > 250) {
            chartDefSetting.xAxis.labels.rotation = 270;
            chartDefSetting.xAxis.labels.align = "right";
            chartDefSetting.xAxis.labels.enabled = false;
        }
        if (totalDataLabels >= 0 && totalDataLabels <= 50) {
            chartDefSetting.xAxis.labels.style.font = "normal 11px arial";
        } else if (totalDataLabels > 50 && totalDataLabels <= 150) {
            chartDefSetting.xAxis.labels.style.font = "normal 10px arial";
        } else if (totalDataLabels > 150) {
            chartDefSetting.xAxis.labels.style.font = "normal 9px arial";
        }
    } else if (chartType == "bar") {
        chartDefSetting.xAxis.labels.rotation = 0;
        chartDefSetting.xAxis.labels.align = "right";
        chartDefSetting.xAxis.labels.x = -10;
        chartDefSetting.xAxis.labels.y = 0;
        chartDefSetting.yAxis.labels.rotation = 0;
        chartDefSetting.yAxis.labels.align = "left";
        chartDefSetting.yAxis.labels.x = -5;
        chartDefSetting.yAxis.labels.y = 20;
    } else if (chartType == "pyramid") {
        chartDefSetting.xAxis[0].labels.rotation = 0;
        chartDefSetting.xAxis[0].labels.align = "right";
        chartDefSetting.xAxis[0].labels.x = -10;
        chartDefSetting.xAxis[0].labels.y = 0;
        chartDefSetting.yAxis.labels.rotation = 0;
        chartDefSetting.yAxis.labels.align = "left";
        chartDefSetting.yAxis.labels.x = -5;
        chartDefSetting.yAxis.labels.y = 20;
    }
    if (totalLegendItems == 1) {
        chartDefSetting.legend.enabled = false;
    } else {
        chartDefSetting.legend.enabled = true;
    }
    return chartDefSetting;
}

function di_updateChartData(uid, newChartData) {
    var chartObj = di_getChartObject(uid);
    var i;
    chartObj.xAxis[0].setCategories(newChartData.categoryCollection, false);
    for (i = 0; i < newChartData.seriesCollection.length; i++) {
        chartObj.series[i].setData(newChartData.seriesCollection[i].data);
        chartObj.series[i].name = newChartData.seriesCollection[i].name;
        chartObj.series[i].show();
    }
    var totalNewSeries = newChartData.seriesCollection.length;
    var diffNoOfSeries = chartObj.series.length - totalNewSeries;
    if (diffNoOfSeries > 0) {
        for (var j = i; j < chartObj.series.length; j++) {
            chartObj.series[j].hide();
        }
    }
    chartObj.redraw();
}

function getAllValues(seriesCollectionArray) {
    var allDataValues = [];
    for (var i = 0; i < seriesCollectionArray.length; i++) {
        for (var j = 0; j < seriesCollectionArray[i].data.length; j++) {
            allDataValues.push(parseInt(seriesCollectionArray[i].data[j]));
        }
    }
    return allDataValues;
}

function getSepratorFunction(decimalPoint) {
    var SepFun = function () {
        return Highcharts.numberFormat(this.value, decimalPoint);
    }
    return SepFun;
}

function getDataLabelSepratorFun(isSeperator, decimalPoint) {
    var SepFun;
    if (isSeperator) {
        SepFun = function () {
            return Highcharts.numberFormat(Math.abs(this.y), decimalPoint);
        }
    } else {
        SepFun = function () {
            return Math.abs(this.y);
        }
    }
    return SepFun;
}

function di_toggleThousandSep(uid, labelItemValue, isThousandSep, decimalPoint, chartType) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    if (labelItemValue == "6") {
        di_setYAxisSeperator(uid, isThousandSep);
        di_setYAxisSeperator(uid, decimalPoint);
        if (isThousandSep) {
            chartInput.yAxis.labels.formatter = getSepratorFunction(decimalPoint);
        } else {
            chartInput.yAxis.labels.formatter = null;
        }
    } else {
        chartInput = di_toggleDataLabelThousandSep(chartInput, chartType, isThousandSep, decimalPoint);
        di_setDataLabelSeperator(uid, isThousandSep);
        di_setDataLabelDecimal(uid, decimalPoint);
    }
    chartObj = new Highcharts.Chart(chartInput);
}

function di_toggleDataLabelThousandSep(chartInput, chartType, isThousandSep, decimalPoint) {
    var sepFun = getDataLabelSepratorFun(isThousandSep, decimalPoint);
    switch (chartType) {
        case "column":
            chartInput.plotOptions.column.dataLabels.formatter = sepFun;
            break;
        case "line":
            chartInput.plotOptions.line.dataLabels.formatter = sepFun;
            break;
        case "bar":
            chartInput.plotOptions.bar.dataLabels.formatter = sepFun;
            break;
        case "area":
            chartInput.plotOptions.area.dataLabels.formatter = sepFun;
            break;
        case "pie":
            chartInput.plotOptions.pie.dataLabels.formatter = sepFun;
            break;
        case "pyramid":
            chartInput.series[0].dataLabels.formatter = chartInput.series[1].dataLabels.formatter = sepFun;
            break;
        case "stackcolumn":
            chartInput.plotOptions.column.dataLabels.formatter = sepFun;
            break;
        case "stackline":
            chartInput.plotOptions.line.dataLabels.formatter = sepFun;
            break;
        case "stackbar":
            chartInput.plotOptions.bar.dataLabels.formatter = sepFun;
            break;
        case "stackarea":
            chartInput.plotOptions.area.dataLabels.formatter = sepFun;
            break;
        case "100stackcolumn":
            chartInput.plotOptions.column.dataLabels.formatter = sepFun;
            break;
        case "100stackline":
            chartInput.plotOptions.line.dataLabels.formatter = sepFun;
            break;
        case "100stackbar":
            chartInput.plotOptions.bar.dataLabels.formatter = sepFun;
            break;
        case "100stackarea":
            chartInput.plotOptions.area.dataLabels.formatter = sepFun;
            break;
        case "scatter2d":
            chartInput.plotOptions.scatter.dataLabels.formatter = sepFun;
            break;
    }
    return chartInput;
}

function di_isSeperatorStatus(uid) {
    var isSeperator = false;
    var chartObj = di_getChartObject(uid);
    var formatterFun = chartObj.chartInput.yAxis.labels.formatter;
    if (formatterFun != null) {
        isSeperator = true;
    }
    return isSeperator;
}

function di_getCustomLabelPosition(uid) {
    var pos = {
        x: 0,
        y: 0
    };
    if (chartCollection.length > 0) {
        for (var i = 0; i < chartCollection.length; i++) {
            if (chartCollection[i].id == uid) {
                pos.x = chartCollection[i].xPos;
                pos.y = chartCollection[i].yPos;
                break;
            }
        }
    }
    return pos;
}

function di_setCustomLabelPosition(uid, pos) {
    if (chartCollection.length > 0) {
        for (var i = 0; i < chartCollection.length; i++) {
            if (chartCollection[i].id == uid) {
                chartCollection[i].xPos = pos.x;
                chartCollection[i].yPos = pos.y;
                break;
            }
        }
    }
}

function di_getYAxisSeperator(uid) {
    var isSep;
    if (chartCollection.length > 0) {
        for (var i = 0; i < chartCollection.length; i++) {
            if (chartCollection[i].id == uid) {
                isSep = chartCollection[i].isYAxisSep;
                break;
            }
        }
    }
    return isSep;
}

function di_setYAxisSeperator(uid, isSep) {
    if (chartCollection.length > 0) {
        for (var i = 0; i < chartCollection.length; i++) {
            if (chartCollection[i].id == uid) {
                chartCollection[i].isYAxisSep = isSep;
                break;
            }
        }
    }
}

function di_getDataLabelSeperator(uid) {
    var isSep;
    if (chartCollection.length > 0) {
        for (var i = 0; i < chartCollection.length; i++) {
            if (chartCollection[i].id == uid) {
                isSep = chartCollection[i].isDataLabelSep;
                break;
            }
        }
    }
    return isSep;
}

function di_setDataLabelSeperator(uid, isSep) {
    if (chartCollection.length > 0) {
        for (var i = 0; i < chartCollection.length; i++) {
            if (chartCollection[i].id == uid) {
                chartCollection[i].isDataLabelSep = isSep;
                break;
            }
        }
    }
}

function di_getYAxisDecimal(uid) {
    var digit;
    if (chartCollection.length > 0) {
        for (var i = 0; i < chartCollection.length; i++) {
            if (chartCollection[i].id == uid) {
                digit = chartCollection[i].YAxisSepDecimal;
                break;
            }
        }
    }
    return digit;
}

function di_setYAxisSeperator(uid, digit) {
    if (chartCollection.length > 0) {
        for (var i = 0; i < chartCollection.length; i++) {
            if (chartCollection[i].id == uid) {
                chartCollection[i].YAxisSepDecimal = digit;
                break;
            }
        }
    }
}

function di_getDataLabelDecimal(uid) {
    var digit;
    if (chartCollection.length > 0) {
        for (var i = 0; i < chartCollection.length; i++) {
            if (chartCollection[i].id == uid) {
                digit = chartCollection[i].isDataLabelSep;
                break;
            }
        }
    }
    return digit;
}

function di_setDataLabelDecimal(uid, digit) {
    if (chartCollection.length > 0) {
        for (var i = 0; i < chartCollection.length; i++) {
            if (chartCollection[i].id == uid) {
                chartCollection[i].isDataLabelSep = digit;
                break;
            }
        }
    }
}

function di_getIsLegendFloating(uid) {
    var isFloating;
    var chartObj = di_getChartObject(uid);
    isFloating = chartObj.options.legend.floating;
    return isFloating;
}

function di_getLegendXPos(uid) {
    var xPos;
    var chartObj = di_getChartObject(uid);
    xPos = chartObj.options.legend.x;
    return xPos;
}

function di_getLegendYPos(uid) {
    var yPos;
    var chartObj = di_getChartObject(uid);
    yPos = chartObj.options.legend.y;
    return yPos;
}

function di_getLegendWidth(uid) {
    var width;
    var chartObj = di_getChartObject(uid);
    width = chartObj.options.legend.width;
    return width;
}

function di_setGroupPadding(uid, groupPaddingVal, chartType) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    if (chartType == "column") {
        chartInput.plotOptions.column.groupPadding = groupPaddingVal;
    } else if (chartType == "bar") {
        chartInput.plotOptions.bar.groupPadding = groupPaddingVal;
    } else if (chartType == "pyramid") {
        chartInput.plotOptions.series.groupPadding = groupPaddingVal;
    }
    chartObj = new Highcharts.Chart(chartInput);
}

function di_getGroupPadding(uid, chartType) {
    var groupPaddingVal;
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    if (chartType == "column") {
        groupPaddingVal = chartInput.plotOptions.column.groupPadding;
    } else if (chartType == "bar") {
        groupPaddingVal = chartInput.plotOptions.bar.groupPadding;
    } else if (chartType == "pyramid") {
        groupPaddingVal = chartInput.plotOptions.bar.groupPadding;
    }
    return groupPaddingVal;
}

function di_setBarPadding(uid, pointPaddingVal, chartType) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    if (chartType == "column") {
        chartInput.plotOptions.column.pointPadding = pointPaddingVal;
    } else if (chartType == "bar") {
        chartInput.plotOptions.bar.pointPadding = pointPaddingVal;
    } else if (chartType == "pyramid") {
        chartInput.plotOptions.bar.pointPadding = pointPaddingVal;
    }
    chartObj = new Highcharts.Chart(chartInput);
}

function di_getBarPadding(uid, chartType) {
    var pointPaddingVal;
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    if (chartType == "column") {
        pointPaddingVal = chartInput.plotOptions.column.pointPadding;
    } else if (chartType == "bar") {
        pointPaddingVal = chartInput.plotOptions.bar.pointPadding;
    } else if (chartType == "pyramid") {
        pointPaddingVal = chartInput.plotOptions.bar.pointPadding;
    }
    return pointPaddingVal;
}

function getSeriesBorderWidth(uid, chartType) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    var bdrWidth;
    switch (chartType) {
        case "column":
            bdrWidth = chartInput.plotOptions.column.borderWidth;
            break;
        case "line":
            bdrWidth = chartInput.plotOptions.line.lineWidth;
            break;
        case "bar":
            bdrWidth = chartInput.plotOptions.bar.borderWidth;
            break;
        case "area":
            bdrWidth = chartInput.plotOptions.area.lineWidth;
            break;
        case "pie":
            bdrWidth = chartInput.plotOptions.pie.borderWidth;
            break;
        case "pyramid":
            bdrWidth = chartInput.plotOptions.bar.lineWidth;
            break;
        case "stackcolumn":
            bdrWidth = chartInput.plotOptions.column.borderWidth;
            break;
        case "stackline":
            bdrWidth = chartInput.plotOptions.line.lineWidth;
            break;
        case "stackbar":
            bdrWidth = chartInput.plotOptions.bar.borderWidth;
            break;
        case "stackarea":
            bdrWidth = chartInput.plotOptions.area.lineWidth;
            break;
        case "100stackcolumn":
            bdrWidth = chartInput.plotOptions.column.borderWidth;
            break;
        case "100stackline":
            bdrWidth = chartInput.plotOptions.line.lineWidth;
            break;
        case "100stackbar":
            bdrWidth = chartInput.plotOptions.bar.borderWidth;
            break;
        case "100stackarea":
            bdrWidth = chartInput.plotOptions.area.lineWidth;
            break;
    }
    return bdrWidth;
}

function getSeriesBorderColor(uid, chartType) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    var bdrColor;
    switch (chartType) {
        case "column":
            bdrColor = chartInput.plotOptions.column.borderColor;
            break;
        case "bar":
            bdrColor = chartInput.plotOptions.bar.borderColor;
            break;
        case "stackcolumn":
            bdrColor = chartInput.plotOptions.column.borderColor;
            break;
        case "stackbar":
            bdrColor = chartInput.plotOptions.bar.borderColor;
            break;
        case "100stackcolumn":
            bdrColor = chartInput.plotOptions.column.borderColor;
            break;
        case "100stackbar":
            bdrColor = chartInput.plotOptions.bar.borderColor;
            break;
        case "pie":
            bdrColor = chartInput.plotOptions.pie.borderColor;
            break;
        case "pyramid":
            bdrColor = chartInput.plotOptions.bar.borderColor;
            break;
    }
    return bdrColor;
}

function di_setLegendPosition(chartObj, position) {
    chartObj.legend.x = chartObj.legend.y = 0;
    switch (position.toLowerCase()) {
        case "1":
            chartObj.legend.align = "left";
            chartObj.legend.verticalAlign = "top";
            chartObj.legend.layout = "vertical";
            break;
        case "2":
            chartObj.legend.align = "left";
            chartObj.legend.verticalAlign = "middle";
            chartObj.legend.layout = "vertical";
            break;
        case "3":
            chartObj.legend.align = "left";
            chartObj.legend.verticalAlign = "bottom";
            chartObj.legend.layout = "vertical";
            break;
        case "4":
            chartObj.legend.align = "center";
            chartObj.legend.verticalAlign = "top";
            chartObj.legend.layout = "horizontal";
            break;
        case "5":
            chartObj.legend.align = "center";
            chartObj.legend.verticalAlign = "bottom";
            chartObj.legend.layout = "horizontal";
            break;
        case "6":
            chartObj.legend.align = "right";
            chartObj.legend.verticalAlign = "top";
            chartObj.legend.layout = "vertical";
            break;
        case "7":
            chartObj.legend.align = "right";
            chartObj.legend.verticalAlign = "middle";
            chartObj.legend.layout = "vertical";
            break;
        case "8":
            chartObj.legend.align = "right";
            chartObj.legend.verticalAlign = "bottom";
            chartObj.legend.layout = "vertical";
            break;
        case "default":
            chartObj.legend.align = "right";
            chartObj.legend.verticalAlign = "top";
            chartObj.legend.layout = "vertical";
            break;
    }
    return chartObj;
}

function di_getLegendPosition(uid) {
    var chartObj = di_getChartObject(uid);
    var alignment = chartObj.options.legend.align;
    var verAlign = chartObj.options.legend.verticalAlign;
    var postionValue;
    if (alignment == "left" && verAlign == "top")
        postionValue = "1";
    else if (alignment == "left" && verAlign == "middle")
        postionValue = "2";
    else if (alignment == "left" && verAlign == "bottom")
        postionValue = "3";
    else if (alignment == "center" && verAlign == "top")
        postionValue = "4";
    else if (alignment == "center" && verAlign == "bottom")
        postionValue = "5";
    else if (alignment == "right" && verAlign == "top")
        postionValue = "6";
    else if (alignment == "right" && verAlign == "middle")
        postionValue = "7";
    else if (alignment == "right" && verAlign == "bottom")
        postionValue = "8";
    else
        postionValue = "-1";
    return postionValue;
}

function di_getYAxisMaxValue(uid) {
    var chartObj = di_getChartObject(uid);
    var Vals = chartObj.yAxis[0].getExtremes();
    var maxVal = Math.abs(Vals.max);
    if (Math.abs(Vals.min) > Math.abs(Vals.max)) {
        maxVal = Math.abs(Vals.min);
    }
    return maxVal;
}

function di_setYAxisMaxValue(uid, maxVal) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    chartInput.yAxis.max = maxVal;
    chartInput.yAxis.min = 0 - maxVal;
    chartObj = new Highcharts.Chart(chartInput);
}

function di_getMajorGridDashLine(uid, type) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    var dashStyle = "solid";
    try {
        if (type == 0) {
            if (chartInput.xAxis.length) {
                dashStyle = chartInput.xAxis[0].gridLineDashStyle;
            } else {
                dashStyle = chartInput.xAxis.gridLineDashStyle;
            }
        } else {
            dashStyle = chartInput.yAxis.gridLineDashStyle;
        }
    } catch (err) {
        dashStyle = "-1";
    }
    return dashStyle;
}

function di_getMinorGridDashLine(uid, type) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    var dashStyle = "solid";
    try {
        if (type == 0) {
            dashStyle = chartInput.xAxis.minorGridLineDashStyle;
        } else {
            dashStyle = chartInput.yAxis.minorGridLineDashStyle;
        }
    } catch (err) {
        dashStyle = "-1";
    }
    return dashStyle;
}

function di_resetChartWidth(uid, newWidth, treeContainer) {
    var chartObj = di_getChartObject(uid);
    if (chartObj == '' || chartObj == null || chartObj == undefined) return;
    if (chartObj.options) {
        var chartInput = chartObj.options;
        if (newWidth == "") {
            chartInput.chart.width = "";
        } else {
            chartInput.chart.width = newWidth;
        }
        chartObj = new Highcharts.Chart(chartInput);
    } else {
        var jsonData = chartObj.op.viz.json;
        document.getElementById(treeContainer).innerHTML = "";
        di_create_treemap_chart("", "", "", uid, treeContainer, jsonData, "treemap")
    }
}
Array.prototype.max = function () {
    var max = this[0];
    var len = this.length;
    for (var i = 1; i < len; i++) if (this[i] > max) max = this[i];
    return max;
}
Array.prototype.min = function () {
    var min = this[0];
    var len = this.length;
    for (var i = 1; i < len; i++) if (this[i] < min) min = this[i];
    return min;
}

function increase_brightness(hex, percent) {
    var r = parseInt(hex.substr(1, 2), 16),
        g = parseInt(hex.substr(3, 2), 16),
        b = parseInt(hex.substr(5, 2), 16);
    return '#' +
        ((0 | (1 << 8) + r + (256 - r) * percent / 100).toString(16)).substr(1) +
        ((0 | (1 << 8) + g + (256 - g) * percent / 100).toString(16)).substr(1) +
        ((0 | (1 << 8) + b + (256 - b) * percent / 100).toString(16)).substr(1);
}

function di_LoadSWF(swfFilePath, uid, divId, height, width) {
    var swfVersionStr;
    var xiSwfUrlStr;
    var params = {};
    var attributes = {};
    var flashvars = {};
    try {
        swfVersionStr = "10.0.0";
        xiSwfUrlStr = "playerProductInstall.swf";
        params.quality = "high";
        params.bgcolor = "#ffffff";
        params.allowscriptaccess = "sameDomain";
        params.allowfullscreen = "true";
        params.wmode = "transparent";
        var completeDivId = uid + divId;
        attributes.id = completeDivId;
        attributes.align = "left";
        flashvars.id = completeDivId;
        flashvars.uid = uid;
        swfobject.embedSWF(swfFilePath, completeDivId, width, height, swfVersionStr, xiSwfUrlStr, flashvars, params, attributes);
    } catch (err) { }
}

function di_changeSeriesStyle(uid, seriesIndex, style) {
    var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    chartInput.series[seriesIndex].dashStyle = style;
    chartInput.tooltip.style.padding = 5;
    chartObj = new Highcharts.Chart(chartInput);
}

function di_selectSeriesStyle(uid, style, seriesIndex) {
    seriesIndex = seriesIndex - 1;
    if (seriesIndex == -1) {
        alert("Select series name");
        return;
    }
    di_changeSeriesStyle(uid, seriesIndex, style);
}