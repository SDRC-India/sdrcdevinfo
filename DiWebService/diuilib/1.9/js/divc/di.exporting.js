var IsdataLabelRequired= false;
(function () {
    var HC = Highcharts,
        Chart = HC.Chart,
        addEvent = HC.addEvent,
        createElement = HC.createElement,
        discardElement = HC.discardElement,
        css = HC.css,
        merge = HC.merge,
        each = HC.each,
        extend = HC.extend,
        math = Math,
        mathMax = math.max,
        doc = document,
        win = window,
        hasTouch = doc.documentElement.ontouchstart !== undefined,
        M = 'M',
        L = 'L',
        DIV = 'div',
        HIDDEN = 'hidden',
        NONE = 'none',
        PREFIX = 'highcharts-',
        ABSOLUTE = 'absolute',
        PX = 'px',
        UNDEFINED, defaultOptions = HC.getOptions();
    extend(defaultOptions.lang, {
        downloadPNG: 'Download PNG image',
        downloadJPEG: 'Download JPEG image',
        downloadPDF: 'Download PDF document',
        downloadSVG: 'Download SVG vector image',
        exportButtonTitle: 'Export to raster or vector image',
        printButtonTitle: 'Print the chart'        
    });
   
    defaultOptions.navigation = {
        menuStyle: {
            border: '1px solid #A0A0A0',
            background: '#FFFFFF'
        },
        menuItemStyle: {
            padding: '0 5px',
            background: NONE,
            color: '#303030',
            fontSize: hasTouch ? '14px' : '11px'
        },
        menuItemHoverStyle: {
            background: '#4572A5',
            color: '#FFFFFF'
        },
        buttonOptions: {
            align: 'right',
            backgroundColor: {
                linearGradient: [0, 0, 0, 20],
                stops: [
                    [0.4, '#F7F7F7'],
                    [0.6, '#E3E3E3']
                ]
            },
            borderColor: '#B0B0B0',
            borderRadius: 3,
            borderWidth: 1,
            height: 20,
            hoverBorderColor: '#909090',
            hoverSymbolFill: '#81A7CF',
            hoverSymbolStroke: '#4572A5',
            symbolFill: '#E0E0E0',
            symbolStroke: '#A0A0A0',
            symbolX: 11.5,
            symbolY: 10.5,
            verticalAlign: 'top',
            width: 24,
            y: 10
        }
    };
    defaultOptions.exporting = {
        type: 'image/png',
        url: 'http://export.highcharts.com/',
        width: 800,
        enableImages: false,
        buttons: {
            exportButton: {
                symbol: 'exportIcon',
                x: -10,
                symbolFill: '#A8BF77',
                hoverSymbolFill: '#768F3E',
                _id: 'exportButton',
                _titleKey: 'exportButtonTitle',
                menuItems: [{
                    textKey: 'downloadPNG',
                    onclick: function () {
                        this.exportChart();
                    }
                }, {
                    textKey: 'downloadJPEG',
                    onclick: function () {
                        this.exportChart({
                            type: 'image/jpeg'
                        });
                    }
                }, {
                    textKey: 'downloadPDF',
                    onclick: function () {
                        this.exportChart({
                            type: 'application/pdf'
                        });
                    }
                }, {
                    textKey: 'downloadSVG',
                    onclick: function () {
                        this.exportChart({
                            type: 'image/svg+xml'
                        });
                    }
                }
                ]
            },
            printButton: {
                symbol: 'printIcon',
                x: -36,
                symbolFill: '#B5C9DF',
                hoverSymbolFill: '#779ABF',
                _id: 'printButton',
                _titleKey: 'printButtonTitle',
                onclick: function () {
                    this.print();
                }
            }
        }
    };
    extend(Chart.prototype, {
        getSVG: function (additionalOptions) {        
            var chart = this,
                chartCopy, sandbox, svg, seriesOptions, config, pointOptions, pointMarker, options = merge(chart.options, additionalOptions);
            if (!doc.createElementNS) {
                doc.createElementNS = function (ns, tagName) {
                    var elem = doc.createElement(tagName);
                    elem.getBBox = function () {
                        return HC.Renderer.prototype.Element.prototype.getBBox.apply({
                            element: elem
                        });
                    };
                    return elem;
                };
            }
            sandbox = createElement(DIV, null, {
                position: ABSOLUTE,
                top: '-9999em',
                width: chart.chartWidth + PX,
                height: chart.chartHeight + PX
            }, doc.body);
            extend(options.chart, {
                renderTo: sandbox,
                forExport: true
            });
            options.exporting.enabled = false;
            if (!options.exporting.enableImages) {
                options.chart.plotBackgroundImage = null;
            }
            options.series = [];

            each(chart.series, function (serie) {                

                seriesOptions = serie.options;
                seriesOptions.animation = false;
                seriesOptions.showCheckbox = false;
                seriesOptions.visible = serie.visible;
                seriesOptions.dataLabels.enabled = IsdataLabelRequired;
                //                seriesOptions.visible.dataLabels = {
                //                    enabled: dataLabelSerie();
                //                };
                if (!options.exporting.enableImages) {
                    if (seriesOptions && seriesOptions.marker && /^url\(/.test(seriesOptions.marker.symbol)) {
                        seriesOptions.marker.symbol = 'circle';
                    }
                }
                seriesOptions.data = [];
                each(serie.data, function (point) {
                    config = point.config;
                    pointOptions = {
                        x: point.x,
                        y: point.y,
                        name: point.name
                    };
                    if (typeof config === 'object' && point.config && config.constructor !== Array) {
                        extend(pointOptions, config);
                    }
                    pointOptions.visible = point.visible;
                    seriesOptions.data.push(pointOptions);
                    if (!options.exporting.enableImages) {
                        pointMarker = point.config && point.config.marker;
                        if (pointMarker && /^url\(/.test(pointMarker.symbol)) {
                            delete pointMarker.symbol;
                        }
                    }
                });
                options.series.push(seriesOptions);
            });
            chartCopy = new Highcharts.Chart(options);
            each(['xAxis', 'yAxis'], function (axisType) {
                each(chart[axisType], function (axis, i) {
                    var axisCopy = chartCopy[axisType][i],
                        extremes = axis.getExtremes(),
                        userMin = extremes.userMin,
                        userMax = extremes.userMax;
                    if (userMin !== UNDEFINED || userMax !== UNDEFINED) {
                        axisCopy.setExtremes(userMin, userMax, true, false);
                    }
                });
            });
            svg = chartCopy.container.innerHTML;
            options = null;
            chartCopy.destroy();
            discardElement(sandbox);
            svg = svg.replace(/zIndex="[^"]+"/g, '').replace(/isShadow="[^"]+"/g, '').replace(/symbolName="[^"]+"/g, '').replace(/jQuery[0-9]+="[^"]+"/g, '').replace(/isTracker="[^"]+"/g, '').replace(/url\([^#]+#/g, 'url(#').replace(/<svg /, '<svg xmlns:xlink="http://www.w3.org/1999/xlink" ').replace(/ href=/g, ' xlink:href=').replace(/&nbsp;/g, '\u00A0').replace(/&shy;/g, '\u00AD').replace(/id=([^" >]+)/g, 'id="$1"').replace(/class=([^" ]+)/g, 'class="$1"').replace(/ transform /g, ' ').replace(/:(path|rect)/g, '$1').replace(/<img ([^>]*)>/gi, '<image $1 />').replace(/<\/image>/g, '').replace(/<image ([^>]*)([^\/])>/gi, '<image $1$2 />').replace(/width=(\d+)/g, 'width="$1"').replace(/height=(\d+)/g, 'height="$1"').replace(/hc-svg-href="/g, 'xlink:href="').replace(/style="([^"]+)"/g, function (s) {
                return s.toLowerCase();
            });
            svg = svg.replace(/(url\(#highcharts-[0-9]+)&quot;/g, '$1').replace(/&quot;/g, "'");
            if (svg.match(/ xmlns="/g).length === 2) {
                svg = svg.replace(/xmlns="[^"]+"/, '');
            }
            svg = svg.replace(/NaN/g, "0");
            return svg;
        },
        exportChart: function (options, chartOptions, myNewOption, isHighChart,isDataLabel) {
            IsdataLabelRequired = isDataLabel;
            var form, chart = this,
                svg = chart.getSVG(chartOptions);
            options = merge(chart.options.exporting, options);
            form = createElement('form', {
                method: 'post',
                action: options.url
            }, {
                display: NONE
            }, doc.body);
            each(['filename', 'type', 'width', 'svg'], function (name) {
                createElement('input', {
                    type: HIDDEN,
                    name: name,
                    value: {
                        filename: options.filename || 'chart',
                        type: options.type,
                        width: options.width,
                        svg: svg
                    }[name]
                }, null, form);
            });
            if (!isHighChart) {
                createElement('input', {
                    type: HIDDEN,
                    name: "title",
                    value: myNewOption.title
                }, null, form);
                createElement('input', {
                    type: HIDDEN,
                    name: "subtitle",
                    value: myNewOption.subtitle
                }, null, form);
                createElement('input', {
                    type: HIDDEN,
                    name: "source",
                    value: myNewOption.source
                }, null, form);
                createElement('input', {
                    type: HIDDEN,
                    name: "tabledata",
                    value: myNewOption.tabledata
                }, null, form);
                createElement('input', {
                    type: HIDDEN,
                    name: "keywords",
                    value: myNewOption.keywords
                }, null, form);
                createElement('input', {
                    type: HIDDEN,
                    name: "mapurl",
                    value: myNewOption.mapurl
                }, null, form);
            }
            form.submit();
            discardElement(form);
        },
        print: function () {
            var chart = this,
                container = chart.container,
                origDisplay = [],
                origParent = container.parentNode,
                body = doc.body,
                childNodes = body.childNodes;
            if (chart.isPrinting) {
                return;
            }
            chart.isPrinting = true;
            each(childNodes, function (node, i) {
                if (node.nodeType === 1) {
                    origDisplay[i] = node.style.display;
                    node.style.display = NONE;
                }
            });
            body.appendChild(container);
            win.print();
            setTimeout(function () {
                origParent.appendChild(container);
                each(childNodes, function (node, i) {
                    if (node.nodeType === 1) {
                        node.style.display = origDisplay[i];
                    }
                });
                chart.isPrinting = false;
            }, 1000);
        },
        contextMenu: function (name, items, x, y, width, height) {
            var chart = this,
                navOptions = chart.options.navigation,
                menuItemStyle = navOptions.menuItemStyle,
                chartWidth = chart.chartWidth,
                chartHeight = chart.chartHeight,
                cacheName = 'cache-' + name,
                menu = chart[cacheName],
                menuPadding = mathMax(width, height),
                boxShadow = '3px 3px 10px #888',
                innerMenu, hide, menuStyle;
            if (!menu) {
                chart[cacheName] = menu = createElement(DIV, {
                    className: PREFIX + name
                }, {
                    position: ABSOLUTE,
                    zIndex: 1000,
                    padding: menuPadding + PX
                }, chart.container);
                innerMenu = createElement(DIV, null, extend({
                    MozBoxShadow: boxShadow,
                    WebkitBoxShadow: boxShadow,
                    boxShadow: boxShadow
                }, navOptions.menuStyle), menu);
                hide = function () {
                    css(menu, {
                        display: NONE
                    });
                };
                addEvent(menu, 'mouseleave', hide);
                each(items, function (item) {
                    if (item) {
                        var div = createElement(DIV, {
                            onmouseover: function () {
                                css(this, navOptions.menuItemHoverStyle);
                            },
                            onmouseout: function () {
                                css(this, menuItemStyle);
                            },
                            innerHTML: item.text || chart.options.lang[item.textKey]
                        }, extend({
                            cursor: 'pointer'
                        }, menuItemStyle), innerMenu);
                        div[hasTouch ? 'ontouchstart' : 'onclick'] = function () {
                            hide();
                            item.onclick.apply(chart, arguments);
                        };
                    }
                });
                chart.exportMenuWidth = menu.offsetWidth;
                chart.exportMenuHeight = menu.offsetHeight;
            }
            menuStyle = {
                display: 'block'
            };
            if (x + chart.exportMenuWidth > chartWidth) {
                menuStyle.right = (chartWidth - x - width - menuPadding) + PX;
            } else {
                menuStyle.left = (x - menuPadding) + PX;
            }
            if (y + height + chart.exportMenuHeight > chartHeight) {
                menuStyle.bottom = (chartHeight - y - menuPadding) + PX;
            } else {
                menuStyle.top = (y + height - menuPadding) + PX;
            }
            css(menu, menuStyle);
        },
        addButton: function (options) {
            var chart = this,
                renderer = chart.renderer,
                btnOptions = merge(chart.options.navigation.buttonOptions, options),
                onclick = btnOptions.onclick,
                menuItems = btnOptions.menuItems,
                buttonWidth = btnOptions.width,
                buttonHeight = btnOptions.height,
                box, symbol, button, borderWidth = btnOptions.borderWidth,
                boxAttr = {
                    stroke: btnOptions.borderColor
                }, symbolAttr = {
                    stroke: btnOptions.symbolStroke,
                    fill: btnOptions.symbolFill
                };
            if (btnOptions.enabled === false) {
                return;
            }

            function revert() {
                symbol.attr(symbolAttr);
                box.attr(boxAttr);
            }
            box = renderer.rect(0, 0, buttonWidth, buttonHeight, btnOptions.borderRadius, borderWidth).align(btnOptions, true).attr(extend({
                fill: btnOptions.backgroundColor,
                'stroke-width': borderWidth,
                zIndex: 19
            }, boxAttr)).add();
            button = renderer.rect(0, 0, buttonWidth, buttonHeight, 0).align(btnOptions).attr({
                id: btnOptions._id,
                fill: 'rgba(255, 255, 255, 0.001)',
                title: chart.options.lang[btnOptions._titleKey],
                zIndex: 21
            }).css({
                cursor: 'pointer'
            }).on('mouseover', function () {
                symbol.attr({
                    stroke: btnOptions.hoverSymbolStroke,
                    fill: btnOptions.hoverSymbolFill
                });
                box.attr({
                    stroke: btnOptions.hoverBorderColor
                });
            }).on('mouseout', revert).on('click', revert).add();
            if (menuItems) {
                onclick = function () {
                    revert();
                    var bBox = button.getBBox();
                    chart.contextMenu('export-menu', menuItems, bBox.x, bBox.y, buttonWidth, buttonHeight);
                };
            }
            button.on('click', function () {
                onclick.apply(chart, arguments);
            });
            symbol = renderer.symbol(btnOptions.symbol, btnOptions.symbolX, btnOptions.symbolY, (btnOptions.symbolSize || 12) / 2).align(btnOptions, true).attr(extend(symbolAttr, {
                'stroke-width': btnOptions.symbolStrokeWidth || 1,
                zIndex: 20
            })).add();
        }
    });
    HC.Renderer.prototype.symbols.exportIcon = function (x, y, radius) {
        return [M, x - radius, y + radius, L, x + radius, y + radius, x + radius, y + radius * 0.5, x - radius, y + radius * 0.5, 'Z', M, x, y + radius * 0.5, L, x - radius * 0.5, y - radius / 3, x - radius / 6, y - radius / 3, x - radius / 6, y - radius, x + radius / 6, y - radius, x + radius / 6, y - radius / 3, x + radius * 0.5, y - radius / 3, 'Z'];
    };
    HC.Renderer.prototype.symbols.printIcon = function (x, y, radius) {
        return [M, x - radius, y + radius * 0.5, L, x + radius, y + radius * 0.5, x + radius, y - radius / 3, x - radius, y - radius / 3, 'Z', M, x - radius * 0.5, y - radius / 3, L, x - radius * 0.5, y - radius, x + radius * 0.5, y - radius, x + radius * 0.5, y - radius / 3, 'Z', M, x - radius * 0.5, y + radius * 0.5, L, x - radius * 0.75, y + radius, x + radius * 0.75, y + radius, x + radius * 0.5, y + radius * 0.5, 'Z'];
    };
    Chart.prototype.callbacks.push(function (chart) {
        var n, exportingOptions = chart.options.exporting,
            buttons = exportingOptions.buttons;
        if (exportingOptions.enabled !== false) {
            for (n in buttons) {
                chart.addButton(buttons[n]);
            }
        }
    });
} ());