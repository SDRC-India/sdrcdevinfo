/*
*  $Id: pivot.js,v 1.7 2007/12/04 17:15:05 source Exp $
*
*  This file is part of the OpenLink Software Ajax Toolkit (OAT) project.
*
*  Copyright (C) 2005-2007 OpenLink Software
*
*  See LICENSE file for details.
*/
/*
p = new OAT.Pivot(div, chartDiv, filterDiv, headerRow, dataRows, headerRowIndexes, headerColIndexes, filterIndexes, dataColumnIndex, optObj) 
div, filterDiv - dom element
headerRow - array
dataRows - array of arrays
headerRowIndexes, headerColIndexes, filterIndexes - arrays
dataColumnIndex - number
optObj - options object
	
p.toXML(xslStr,saveCredentials,noCredentials,query)  -- if query == false than data is dumped

var defOpt = {
headingBefore:1,
headingAfter:1,
agg:1,
aggTotals:1,
showChart:0,
showRowChart:0,
showColChart:0,
type:OAT.PivotData.TYPE_BASIC[0],
customType:function(data){return data;},
showEmpty:1,
subtotals:1,
totals:1
}
	
CSS: .pivot_table, .h1, .h2, .odd, .even .subtotal .total .gtotal .pivot_chart .pivot_row_chart .pivot_col_chart
*/
var allDetails = null;
OAT.getSelfInstance = function () {
    return allDetails;
}

OAT.PivotData = {
    TYPE_BASIC: [0, "Basic - 1234.56"],
    TYPE_PERCENT: [1, "Percentual - 1234.56%"],
    TYPE_SCI: [2, "Scientific - 1234E+02"],
    TYPE_SPACE: [3, "With space - 1 234.56"],
    TYPE_CUSTOM: [4, "Custom"], /* function in options.customType */
    TYPE_COMMA: [5, "With comma - 1,234.56"],
    TYPE_CURRENCY: [6, "Currency - $ 1,234.56"] /* currency symbol in options.currencySymbol. $ is default */
}

OAT.Pivot = function (div, chartDiv, filterDiv, headerRow, dataRows, headerRowIndexes, headerColIndexes, filterIndexes, dataColumnIndex, optObj) {
    var self = this;
    this.options = {
        headingBefore: 1,
        headingAfter: 0,
        agg: 1, /* index of default statistic function, SUM */
        aggTotals: 1, /* dtto for subtotals & totals */
        showChart: 0,
        showRowChart: 0,
        showColChart: 0,
        type: OAT.PivotData.TYPE_BASIC[0],
        customType: function (data) { return data; },
        currencySymbol: "$",
        showEmpty: 1,
        subtotals: 0,
        totals: 0,
        externalSort: false
    }
    if (optObj) for (p in optObj) { this.options[p] = optObj[p]; }

    this.gd = new OAT.GhostDrag();
    this.div = $(div);
    this.filterDiv = $(filterDiv);
    this.chartDiv = $(chartDiv);
    this.defCArray = ["rgb(153,153,255)", "rgb(153,51,205)", "rgb(255,255,204)", "rgb(204,255,255)", "rgb(102,0,102)",
						"rgb(255,128,128)", "rgb(0,102,204)", "rgb(204,204,255)", "rgb(0,0,128)", "rgb(255,0,255)",
						"rgb(0,255,255)", "rgb(255,255,0)"];

    if (this.chartDiv) {
        OAT.Dom.clear(self.chartDiv);
        var c1 = OAT.Dom.create("div", {}, "pivot_chart");
        var c2 = OAT.Dom.create("div", {}, "pivot_row_chart");
        var c3 = OAT.Dom.create("div", {}, "pivot_col_chart");
        var l1 = OAT.Dom.button("");
        var l2 = OAT.Dom.button("");
        var l3 = OAT.Dom.button("");

        OAT.Dom.append([self.chartDiv, l1, c1, l3, c3, l2, c2]);
        this.charts = {
            main: new OAT.BarChart(c1, {}),
            row: new OAT.BarChart(c2, {}),
            col: new OAT.BarChart(c3, {}),
            mainLink: l1,
            rowLink: l2,
            colLink: l3,
            mainDiv: c1,
            rowDiv: c2,
            colDiv: c3
        }
        OAT.Dom.attach(l1, "click", function () { self.options.showChart = (self.options.showChart + 1) % 2; self.go(); });
        OAT.Dom.attach(l2, "click", function () { self.options.showRowChart = (self.options.showRowChart + 1) % 2; self.go(); });
        OAT.Dom.attach(l3, "click", function () { self.options.showColChart = (self.options.showColChart + 1) % 2; self.go(); });
    }

    this.headerRow = headerRow; /* store data */
    this.allData = dataRows; /* store data */
    this.filteredData = [];
    this.tabularData = []; /* result */

    this.dataColumnIndex = dataColumnIndex; /* store data */
    this.rowConditions = headerRowIndexes; /* indexes of row conditions */
    this.colConditions = headerColIndexes; /* indexes of column conditions */
    this.filterIndexes = filterIndexes; /* indexes of column conditions */

    this.conditions = [];
    this.filterDiv.selects = [];
    this.rowStructure = {};
    this.colStructure = {};
    this.colPointers = [];
    this.rowPointers = [];
    this.rowTotals = [];
    this.colTotals = [];
    this.gTotal = [];

    /* supplemental routines */
    this.toXML = function (xslStr, saveCredentials, noCredentials, query) {
        var xml = '<?xml version="1.0" ?>\n';
        if (xslStr) { xml += xslStr + '\n'; }
        xml += '\t<pivot>\n';
        xml += '\t\t<headerRow>\n';
        for (var i = 0; i < self.headerRow.length; i++) {
            xml += '\t\t\t<value>' + self.headerRow[i] + '</value>\n';
        }
        xml += '\t\t</headerRow>\n';

        xml += '\t\t<headerRowIndexes>\n';
        for (var i = 0; i < self.rowConditions.length; i++) {
            xml += '\t\t\t<value>' + self.rowConditions[i] + '</value>\n';
        }
        xml += '\t\t</headerRowIndexes>\n';

        xml += '\t\t<headerColIndexes>\n';
        for (var i = 0; i < self.colConditions.length; i++) {
            xml += '\t\t\t<value>' + self.colConditions[i] + '</value>\n';
        }
        xml += '\t\t</headerColIndexes>\n';

        xml += '\t\t<filterIndexes>\n';
        for (var i = 0; i < self.filterIndexes.length; i++) {
            xml += '\t\t\t<value>' + self.filterIndexes[i] + '</value>\n';
        }
        xml += '\t\t</filterIndexes>\n';

        xml += '\t\t<dataColumnIndex>' + self.dataColumnIndex + '</dataColumnIndex>\n';

        if (query) {
            xml += '\t\t<query>' + query + '</query>\n';
            xml += OAT.Xmla.connection.toXML(saveCredentials, noCredentials);
        } else {
            xml += '\t\t<dataRows>\n';
            for (var i = 0; i < self.filteredData.length; i++) {
                xml += '\t\t\t<dataRow>\n';
                for (var j = 0; j < self.filteredData[i].length; j++) {
                    xml += '\t\t\t\t<value>' + self.filteredData[i][j] + '</value>\n';
                }
                xml += '\t\t\t</dataRow>\n';
            }
            xml += '\t\t</dataRows>\n';
        }

        xml += '\t</pivot>\n';
        return xml;
    }

    this.lightOn = function () {
        for (var i = 0; i < self.gd.targets.length; i++) {
            var elm = self.gd.targets[i][0];
            elm.style.color = "#f00";
        }
    }

    this.lightOff = function () {
        for (var i = 0; i < self.gd.targets.length; i++) {
            var elm = self.gd.targets[i][0];
            elm.style.color = "";
        }
    }
    self.gd.onFail = self.lightOff;

    this.process = function (elm) {
        self.lightOn();
        elm.style.backgroundColor = "#888";
        elm.style.padding = "2px";
        elm.style.cursor = "pointer";
        OAT.Dom.attach(elm, "mouseup", function (e) { self.lightOff(); });
    }

    this.filterOK = function (row) { /* does row pass filters? */
        for (var i = 0; i < self.filterIndexes.length; i++) { /* for all filters */
            var fi = self.filterIndexes[i]; /* this column is important */
            var s = self.filterDiv.selects[i]; /* select node */
            if (s.selectedIndex && $v(s) != row[fi]) { return false; }
        }
        for (var i = 0; i < self.rowConditions.length; i++) { /* row blacklist */
            var value = row[self.rowConditions[i]];
            var cond = self.conditions[self.rowConditions[i]];
            if (cond.blackList.find(value) != -1) { return false; }
        }
        for (var i = 0; i < self.colConditions.length; i++) { /* column blacklist */
            var value = row[self.colConditions[i]];
            var cond = self.conditions[self.colConditions[i]];
            if (cond.blackList.find(value) != -1) { return false; }
        }
        return true;
    }

    this.sort = function (cond) { /* sort distinct values of a condition */
        var months = ["january", "february", "march", "april", "may", "june", "july", "august", "september", "october", "november", "december"];
        var sortFunc;
        var coef = cond.sort;
        var numSort = function (a, b) {
            if (a == b) { return 0; }
            return coef * (parseInt(a) > parseInt(b) ? 1 : -1);
        }
        var dictSort = function (a, b) {
            if (a == b) { return 0; }
            return coef * (a > b ? 1 : -1);
        }
        var dateSort = function (a, b) {
            var ia = months.find(a.toLowerCase());
            var ib = months.find(b.toLowerCase());
            return numSort(ia, ib);
        }
        /* get data type, trial & error... */
        var testValue = cond.distinctValues[0];
        if (testValue == parseInt(testValue)) { sortFunc = numSort; } else { sortFunc = dictSort; }
        if (months.find(testValue.toString().toLowerCase()) != -1) { sortFunc = dateSort; }
        cond.distinctValues.sort(sortFunc);
    } /* sort */

    /* init routines */
    this.initCondition = function (index) {
        if (index == self.dataColumnIndex) { /* dummy condition */
            self.conditions.push(false);
            return;
        }
        var cond = { distinctValues: [], blackList: [], sort: 1, subtotals: self.options.subtotals };
        self.conditions.push(cond);
        for (var i = 0; i < self.allData.length; i++) {
            var value = self.allData[i][index];
            if (cond.distinctValues.find(value) == -1) { /* not yet present */
                cond.distinctValues.push(value);
            } /* if new value */
        } /* for all rows */
        if (!self.options.externalSort) self.sort(cond); /* Temporarily turning off the SORT for testing dg26 */
    }

    this.init = function () {
        self.propPage = OAT.Dom.create("div", { position: "absolute", border: "2px solid #000", padding: "2px", backgroundColor: "#fff" });
        document.body.appendChild(self.propPage);
        OAT.Instant.assign(self.propPage);

        for (var i = 0; i < self.headerRow.length; i++) {
            self.initCondition(i);
        }
    } /* init */

    /* callback routines */
    this.getOrderReference = function (conditionIndex) {
        return function (target, x, y) {
            /* somehow reorder conditions */
            self.lightOff();

            /* filters? */
            if (target == self.filterDiv) {
                self.filterIndexes.push(conditionIndex);
                self.conditions[conditionIndex].blackList = [];
                for (var i = 0; i < self.rowConditions.length; i++) {
                    if (self.rowConditions[i] == conditionIndex) { self.rowConditions.splice(i, 1); }
                }
                for (var i = 0; i < self.colConditions.length; i++) {
                    if (self.colConditions[i] == conditionIndex) { self.colConditions.splice(i, 1); }
                }
                self.go();
                return;
            }

            var sourceCI = conditionIndex; /* global index */
            var targetCI = target.conditionIndex; /* global index */
            if (sourceCI == targetCI) { return; } /* dragged onto the same */
            var sourceType = false; var sourceI = -1; /* local */
            var targetType = false; var targetI = -1; /* local */
            for (var i = 0; i < self.rowConditions.length; i++) {
                if (self.rowConditions[i] == sourceCI) { sourceType = self.rowConditions; sourceI = i; }
                if (self.rowConditions[i] == targetCI) { targetType = self.rowConditions; targetI = i; }
            }
            for (var i = 0; i < self.colConditions.length; i++) {
                if (self.colConditions[i] == sourceCI) { sourceType = self.colConditions; sourceI = i; }
                if (self.colConditions[i] == targetCI) { targetType = self.colConditions; targetI = i; }
            }
            if (targetCI == -1) {
                /* no cols - lets create one */
                self.colConditions.push(sourceCI);
                self.rowConditions.splice(sourceI, 1);
                self.go();
                return;
            }
            if (targetCI == -2) {
                /* no rows - lets create one */
                self.rowConditions.push(sourceCI);
                self.colConditions.splice(sourceI, 1);
                self.go();
                return;
            }
            if (sourceType == targetType) {
                /* same condition type */
                if (sourceI + 1 == targetI) {
                    /* dragged on condition immediately after */
                    targetType.splice(targetI + 1, 0, sourceCI);
                    targetType.splice(sourceI, 1);
                } else {
                    targetType.splice(sourceI, 1);
                    targetType.splice(targetI, 0, sourceCI);
                }
            } else {
                /* different condition type */
                sourceType.splice(sourceI, 1);
                targetType.splice(targetI, 0, sourceCI);
            }
            self.go();
        }
    }

    this.getClickReference = function (cond) {
        var refresh = function () {
            self.propPage._Instant_hide();
            self.go();
        }
        return function (event) {
            var coords = OAT.Dom.eventPos(event);
            self.propPage.style.left = coords[0] + "px";
            self.propPage.style.top = coords[1] + "px";
            OAT.Dom.clear(self.propPage);
            /* contents */
            var close = OAT.Dom.create("div", { position: "absolute", top: "3px", right: "3px", cursor: "pointer" });
            close.innerHTML = "X";
            OAT.Dom.attach(close, "click", refresh);

            var asc = OAT.Dom.radio("order");
            asc.id = "pivot_order_asc";
            OAT.Dom.attach(asc, "change", function () { cond.sort = 1; self.sort(cond); self.go(); });
            OAT.Dom.attach(asc, "click", function () { cond.sort = 1; self.sort(cond); self.go(); });
            var alabel = OAT.Dom.create("label");
            alabel.htmlFor = "pivot_order_asc";
            alabel.innerHTML = "Ascending";

            var desc = OAT.Dom.radio("order");
            desc.id = "pivot_order_desc";
            OAT.Dom.attach(desc, "change", function () { cond.sort = -1; self.sort(cond); self.go(); });
            OAT.Dom.attach(desc, "click", function () { cond.sort = -1; self.sort(cond); self.go(); });
            var dlabel = OAT.Dom.create("label");
            dlabel.htmlFor = "pivot_order_desc";
            dlabel.innerHTML = "Descending";

            var hr2 = OAT.Dom.create("hr", { width: "100px" });

            var subtotals = OAT.Dom.create("div");

            var distinct = OAT.Dom.create("div");
            OAT.Dom.append([self.propPage, close, asc, alabel, OAT.Dom.create("br"), desc, dlabel, subtotals, hr2, distinct]);
            self.distinctDivs(cond, distinct);

            self.propPage._Instant_show();

            /* this needs to be here because of IE :/ */
            asc.checked = cond.sort == 1;
            asc.__checked = asc.checked;
            desc.checked = cond.sort == -1;
            desc.__checked = desc.checked;
        }
    }

    this.getDelFilterReference = function (index) {
        return function () {
            var idx = self.filterIndexes.find(index);
            self.filterIndexes.splice(idx, 1);
            self.rowConditions.push(index); /* add to rows */
            self.go();
        }
    }

    this.distinctDivs = function (cond, div) { /* set of distinct values checboxes */
        var getPair = function (text, id) {
            var div = OAT.Dom.create("div");
            var ch = OAT.Dom.create("input");
            ch.type = "checkbox";
            ch.id = id;
            var t = OAT.Dom.create("label");
            t.innerHTML = text;
            t.htmlFor = id;
            div.appendChild(ch);
            div.appendChild(t);
            return [div, ch];
        }

        var getRef = function (ch, value) {
            return function () {
                if (ch.checked) {
                    var index = cond.blackList.find(value);
                    cond.blackList.splice(index, 1);
                } else {
                    cond.blackList.push(value);
                }
                self.go();
            }
        }

        var allRef = function () {
            cond.blackList = [];
            self.go();
            self.distinctDivs(cond, div);
        }

        var noneRef = function () {
            cond.blackList = [];
            for (var i = 0; i < cond.distinctValues.length; i++) { cond.blackList.push(cond.distinctValues[i]); }
            self.go();
            self.distinctDivs(cond, div);
        }

        var reverseRef = function () {
            var newBL = [];
            for (var i = 0; i < cond.distinctValues.length; i++) {
                var val = cond.distinctValues[i];
                if (cond.blackList.find(val) == -1) { newBL.push(val); }
            }
            cond.blackList = newBL;
            self.go();
            self.distinctDivs(cond, div);
        }

        OAT.Dom.clear(div);
        var d = OAT.Dom.create("div");

        var all = OAT.Dom.button("All");
        OAT.Dom.attach(all, "click", allRef);

        var none = OAT.Dom.button("None");
        OAT.Dom.attach(none, "click", noneRef);

        var reverse = OAT.Dom.button("Reverse");
        OAT.Dom.attach(reverse, "click", reverseRef);

        OAT.Dom.append([d, all, none, reverse], [div, d]);
        for (var i = 0; i < cond.distinctValues.length; i++) {
            var value = cond.distinctValues[i];
            var pair = getPair(value, "pivot_distinct_" + i);
            div.appendChild(pair[0]);
            pair[1].checked = (cond.blackList.find(value) == -1);
            pair[1].__checked = (pair[1].checked ? "1" : "0");
            OAT.Dom.attach(pair[1], "change", getRef(pair[1], value));
            OAT.Dom.attach(pair[1], "click", getRef(pair[1], value));
        }
    }

    this.drawFilters = function () {
        var savedValues = [];
        var div = self.filterDiv;
        for (var i = 0; i < div.selects.length; i++) {
            savedValues.push([div.selects[i].filterIndex, div.selects[i].selectedIndex]);
        }
        OAT.Dom.clear(div);
        self.gd.addTarget(div);
        div.selects = [];
        if (!self.filterIndexes.length) {
            div.innerHTML = "[drag paging columns here]";
        }
        for (var i = 0; i < self.filterIndexes.length; i++) {
            var index = self.filterIndexes[i];
            var s = OAT.Dom.create("select");
            OAT.Dom.option("[all]", "", s);
            for (var j = 0; j < self.conditions[index].distinctValues.length; j++) {
                var v = self.conditions[index].distinctValues[j];
                OAT.Dom.option(v, v, s);
            }
            s.filterIndex = index;
            for (var j = 0; j < savedValues.length; j++) {
                if (savedValues[j][0] == index) { s.selectedIndex = savedValues[j][1]; }
            }
            OAT.Dom.attach(s, "change", self.go);
            div.selects.push(s);
            var d = OAT.Dom.create("div");
            d.innerHTML = self.headerRow[index] + ": ";

            var close = OAT.Dom.create("span");
            close.style.color = "#f00";
            close.style.cursor = "pointer";
            close.innerHTML = " X";
            var ref = self.getDelFilterReference(index);
            OAT.Dom.attach(close, "click", ref)

            OAT.Dom.append([self.filterDiv, d], [d, s, close]);
        }
    }


    this.countPointers = function () { /* create arrays of pointers to levels of agg structures */
        function count(struct, arr, propName) {
            self[propName] = [];
            var stack = [struct];
            for (var i = 0; i < arr.length; i++) {
                var newstack = [];
                for (var j = 0; j < stack.length; j++) {
                    var item = stack[j];
                    for (var k = 0; k < item.items.length; k++) {
                        newstack.push(item.items[k]);
                    }
                }
                stack = newstack;
                self[propName].push(stack.copy());
            }
        }

        count(self.rowStructure, self.rowConditions, "rowPointers");
        count(self.colStructure, self.colConditions, "colPointers");
    }

    this.countOffsets = function () { /* starting offsets for aggregate structures */
        function count(ptrArray) {
            for (var i = 0; i < ptrArray.length; i++) {
                var stack = ptrArray[i];
                var counter = 0;
                for (var j = 0; j < stack.length; j++) {
                    var item = stack[j];
                    item.offset = counter;
                    counter += item.spanData;
                }
            }
        }

        count(self.rowPointers);
        count(self.colPointers);
    }

    this.count = function () { /* create tabularData from filteredData */
        /* compute spans = table dimensions */
        function spans(ptr, arr) { /* return span for a given aggregate pointer */
            var s = 0;
            var sD = 0;
            if (!ptr.items) {
                ptr.span = 1;
                ptr.spanData = 1;
                return [ptr.span, ptr.spanData];
            }
            for (var i = 0; i < ptr.items.length; i++) {
                var tmp = spans(ptr.items[i], arr);
                s += tmp[0];
                sD += tmp[1];
            }
            ptr.span = s;
            ptr.spanData = sD;
            if (ptr.items.length && ptr.items[0].items) {
                var cond = self.conditions[arr[ptr.items[0].depth]];
                if (cond.subtotals) { ptr.span += ptr.items.length; }
            }
            return [ptr.span, ptr.spanData];
        }
        spans(self.rowStructure, self.rowConditions);
        spans(self.colStructure, self.colConditions);

        self.countPointers();
        self.countOffsets();

        /* create blank table */
        self.tabularData = [];
        self.w = 1;
        self.h = 1;
        if (self.colConditions.length) { self.w = self.colPointers[self.colPointers.length - 1].length; }
        if (self.rowConditions.length) { self.h = self.rowPointers[self.rowPointers.length - 1].length; }

        for (var i = 0; i < self.w; i++) {
            var col = new Array(self.h);
            for (var j = 0; j < self.h; j++) { col[j] = []; }
            self.tabularData.push(col);
        }

        function coords(struct, arr, row) {
            var pos = 0;
            var ptr = struct;
            for (var i = 0; i < arr.length; i++) {
                var rindex = arr[i];
                var value = row[rindex];
                var o = false;
                for (var j = 0; j < ptr.items.length; j++) {
                    if (ptr.items[j].value != value) {
                        pos += ptr.items[j].spanData;
                    } else {
                        o = ptr.items[j];
                        break;
                    }
                }
                if (!o) { alert("Value not found in distinct?!?!? PANIC!!!"); }
                ptr = o;
            } /* for all conditions */
            return pos;
        }

        for (var i = 0; i < self.filteredData.length; i++) { /* reposition value array to grid */
            var row = self.filteredData[i];
            var x = coords(self.colStructure, self.colConditions, row);
            var y = coords(self.rowStructure, self.rowConditions, row);
            var val = row[self.dataColumnIndex];
            val = val.toString();
            val = val.replace(/%/g, '');
            self.tabularData[x][y].push(val);
        }
        var func = OAT.Statistics[OAT.Statistics.list[self.options.agg].func]; /* statistics */
        for (var i = 0; i < self.w; i++) {
            for (var j = 0; j < self.h; j++) {
                self.tabularData[i][j] = self.tabularData[i][j];
            }
        }
    } /* Pivot::count() */

    this.numericalType = function (event) {
        var coords = OAT.Dom.eventPos(event);
        self.propPage.style.left = coords[0] + "px";
        self.propPage.style.top = coords[1] + "px";
        OAT.Dom.clear(self.propPage);
        var refresh = function () {
            self.propPage._Instant_hide();
            self.go();
        }
        /* contents */


        var showNulls = OAT.Dom.create("div");
        var ch = OAT.Dom.create("input");
        ch.id = "pivot_checkbox_empty";
        ch.setAttribute("type", "checkbox");
        ch.checked = (self.options.showEmpty ? true : false);
        ch.__checked = (ch.checked ? "1" : "0");
        showNulls.appendChild(ch);
        var l = OAT.Dom.create("label");
        l.htmlFor = "pivot_checkbox_empty";
        l.innerHTML = "Show empty items";
        showNulls.appendChild(l);
        OAT.Dom.attach(ch, "change", function () { self.options.showEmpty = ch.checked; refresh(); });
        OAT.Dom.attach(ch, "click", function () { self.options.showEmpty = ch.checked; refresh(); });
        OAT.Dom.append([self.propPage, showNulls]);
        self.propPage._Instant_show();
    }


    this._drawRowTotals = function (tr) {

    }

    this._drawCorner = function (th, target) {
        th.innerHTML = self.headerRow[self.dataColumnIndex]; //dg26 commented
        //th.style.cursor = "pointer"; //dg26 commented
        th.id = "dataValHCell"; //dg26 added
        th.className = "h1";
        if (target) { self.gd.addTarget(th); }
        //OAT.Dom.attach(th,"click",self.numericalType); //dg26 commented
    }

    this._drawRowConditionsHeadings = function (tbody) {
        /* rowConditions headings */
        var tr = OAT.Dom.create("tr");
        tr.id = "HrowTobeHide";
        for (var j = 0; j < self.rowConditions.length; j++) {
            var cond = self.conditions[self.rowConditions[j]];
            var th = OAT.Dom.create("th", { cursor: "pointer" }, "h1");
            var div = OAT.Dom.create("div");
            div.innerHTML = self.headerRow[self.rowConditions[j]];

            //dg26 added 
            div.style.padding = "3px";
            div.style.cursor = "move";
            //

            var ref = self.getClickReference(cond);
            OAT.Dom.attach(th,"click",ref); //dg26 commented for removing filter

            var callback = self.getOrderReference(self.rowConditions[j]);
            self.gd.addSource(div, self.process, callback);
            self.gd.addTarget(th);
            th.conditionIndex = self.rowConditions[j];
            OAT.Dom.append([th, div], [tr, th]);
        }
        var th = OAT.Dom.create("th"); /* blank space above */
        if (!self.colConditions.length) {
            self._drawCorner(th, true);
            th.conditionIndex = -1;
        } else { th.style.border = "none"; }
        th.colSpan = self.colStructure.span + (self.options.headingBefore ? 1 : 0) + (self.options.totals ? 1 : 0);
        tr.appendChild(th);
        if (self.colConditions.length) { /* blank space after */
            var th = OAT.Dom.create("th", { border: "none" });
            tr.appendChild(th);
        }
        tbody.appendChild(tr);

    }

    this._drawColConditionsHeadings = function (tr, i) {
        var cond = self.conditions[self.colConditions[i]];
        var th = OAT.Dom.create("th", { cursor: "pointer" }, "h1");
        th.id = "VcolTobeHideH" + i; //dg26 added
        var div = OAT.Dom.create("div");
        div.innerHTML = self.headerRow[self.colConditions[i]];
        //dg26 added 
        div.style.padding = "3px";
        div.style.cursor = "move";
        //

        var ref = self.getClickReference(cond);
        OAT.Dom.attach(th,"click",ref); //dg26 commented for removing filter

        var callback = self.getOrderReference(self.colConditions[i]);
        self.gd.addSource(div, self.process, callback);
        self.gd.addTarget(th);
        th.conditionIndex = self.colConditions[i];
        th.appendChild(div);
        tr.appendChild(th);


    }

    this.getClassName = function (i, j) { /* decide odd/even class */
        var xCounter = 1;
        var yCounter = 1;
        if (self.colConditions.length > 1) {
            var colItem = self.colPointers[self.colConditions.length - 1][j].parent;
            var index = colItem.parent.items.find(colItem);
            xCounter = (index % 2 ? 1 : -1);
        }
        if (self.rowConditions.length > 1) {
            var rowItem = self.rowPointers[self.rowConditions.length - 1][i].parent;
            var index = rowItem.parent.items.find(rowItem);
            yCounter = (index % 2 ? 1 : -1);
        }
        if (xCounter * yCounter == 1) { return "odd"; } else { return "even"; }
    }

    this.formatValue = function (value) {
        var result = "";
        switch (self.options.type) { /* numeric type */
            case OAT.PivotData.TYPE_BASIC[0]: result = value.toFixed(2); break;
            case OAT.PivotData.TYPE_PERCENT[0]: result = value.toFixed(2) + "%"; break;
            case OAT.PivotData.TYPE_SCI[0]: result = value.toExponential(2); break;
            case OAT.PivotData.TYPE_SPACE[0]:
                result = value.toFixed(2);
                result = result.toString();
                var parts = result.split('.');
                var decPart = (parts.length > 1) ? ('.' + parts[1]) : '';
                var len = parts[0].length;
                var mod = len % 3;
                var wholePart = '';
                var delimiter = '&nbsp;';
                if (mod > 0)
                    wholePart = parts[0].substring(0, mod);
                for (i = mod; i < len; i += 3)
                    wholePart += (i == 0 ? '' : delimiter) + parts[0].substr(i, 3);
                result = wholePart + decPart;
                break;
            case OAT.PivotData.TYPE_COMMA[0]:
                result = value.toFixed(2);
                result = result.toString();
                var parts = result.split('.');
                var decPart = (parts.length > 1) ? ('.' + parts[1]) : '';
                var len = parts[0].length;
                var mod = len % 3;
                var wholePart = '';
                var delimiter = ',';
                if (mod > 0)
                    wholePart = parts[0].substring(0, mod);
                for (i = mod; i < len; i += 3)
                    wholePart += (i == 0 ? '' : delimiter) + parts[0].substr(i, 3);
                result = wholePart + decPart;
                break;
            case OAT.PivotData.TYPE_CURRENCY[0]:
                result = value.toFixed(2);
                result = result.toString();
                var parts = result.split('.');
                var decPart = (parts.length > 1) ? ('.' + parts[1]) : '';
                var len = parts[0].length;
                var mod = len % 3;
                var wholePart = '';
                var delimiter = ',';
                if (mod > 0)
                    wholePart = parts[0].substring(0, mod);
                for (i = mod; i < len; i += 3)
                    wholePart += (i == 0 ? '' : delimiter) + parts[0].substr(i, 3);
                result = self.options.currencySymbol + "&nbsp;" + wholePart + decPart;
                break;
            case OAT.PivotData.TYPE_CUSTOM[0]: result = self.options.customType(value); break;
        } /* switch */
        return result;
    }

    this.drawTable = function () { /* this is the crucial part */
        allDetails = self;
        OAT.Dom.clear(self.div);
        var table = OAT.Dom.create("table", {}, "pivot_table");
        table.id = 'pivot_tableid'; //dg26 added
        var tbody = OAT.Dom.create("tbody");

        /* upper part */
        for (var i = 0; i < self.colConditions.length; i++) {
            var tr = OAT.Dom.create("tr");
            if (i == 0 && self.rowConditions.length) { /* left top corner */
                var th = OAT.Dom.create("th");
                self._drawCorner(th);
                th.colSpan = self.rowConditions.length;
                th.rowSpan = self.colConditions.length;
                tr.appendChild(th);
            }
            if (self.options.headingBefore) { /* column headings before */
                self._drawColConditionsHeadings(tr, i);
            }
            var stack = self.colPointers[i];
            for (var j = 0; j < stack.length; j++) { /* column values */
                var item = stack[j];
                var th = OAT.Dom.create("th", {}, "h2");
                th.id = "h2Colh" + i + j; //DG26 added
                th.innerHTML = item.value;
                th.colSpan = item.span;
                tr.appendChild(th);

                /*//alert(self.colConditions[j]);
                var cond = self.conditions[self.colConditions[j]];
                var th = OAT.Dom.create("th",{cursor:"pointer"},"h2");
                var div = OAT.Dom.create("div");
                div.innerHTML = self.headerRow[self.tabularData[j]];
                var ref = self.getClickReference(cond); //alert(ref);
                OAT.Dom.attach(div.innerHTML,"click",ref);
                var callback = self.getOrderReference(self.tabularData[j]);
                self.gd.addSource(div,self.process,callback);
                self.gd.addTarget(th);
                th.conditionIndex = self.tabularData[j];
                OAT.Dom.append([th,div],[tr,th]);
                th.innerHTML = item.value;
                th.colSpan = item.span;
                tr.appendChild(th);*/


                /*var cond = self.conditions[self.rowConditions[j]];
                var th = OAT.Dom.create("th",{cursor:"pointer"},"h1");
                var div = OAT.Dom.create("div");
                div.innerHTML = self.headerRow[self.rowConditions[j]];
                var ref = self.getClickReference(cond);
                OAT.Dom.attach(th,"click",ref);
                var callback = self.getOrderReference(self.rowConditions[j]);
                self.gd.addSource(div,self.process,callback);
                self.gd.addTarget(th);
                th.conditionIndex = self.rowConditions[j];
                OAT.Dom.append([th,div],[tr,th]);*/

            }

            if (self.options.headingAfter) { /* column headings after */
                self._drawColConditionsHeadings(tr, i);
            }
            tbody.appendChild(tr);
        }

        /* first connector */
        if (self.rowConditions.length && self.options.headingBefore) {
            self._drawRowConditionsHeadings(tbody);
        }

        /* main part */
        for (var i = 0; i < self.h; i++) {
            var tr = OAT.Dom.create("tr");
            if (self.rowConditions.length) {
                var item = self.rowPointers[self.rowConditions.length - 1][i]; /* stack has number of values equal to height of table */
                var ptrArray = [];
                var ptr = item;
                while (ptr.parent) {
                    ptrArray.unshift(ptr);
                    ptr = ptr.parent;
                }
            }

            for (var j = 0; j < self.rowConditions.length; j++) { /* row header values */
                var item = ptrArray[j];
                if (item.offset == i) {
                    var th = OAT.Dom.create("th", {}, "h2");
                    th.rowSpan = ptrArray[j].span;
                    th.style.verticalAlign = "top"; //CHANGED row header center alignment to TOP alignment 
                    th.id = "h2Rowh" + j + i; //DG26 added
                    th.innerHTML = item.value;
                    tr.appendChild(th);
                }
            }

            if (self.colConditions.length && i == 0 && self.options.headingBefore) { /* blank space before */
                var th = OAT.Dom.create("th");
                if (!self.rowConditions.length) {
                    self._drawCorner(th, true);
                    th.conditionIndex = -2;
                } else { th.style.border = "none"; }
                th.rowSpan = self.rowStructure.span;
                th.id = "VcolTobeHide"; //dg26 added
                tr.appendChild(th);
            }

            for (var j = 0; j < self.w; j++) { /* data */
                var td = OAT.Dom.create("td", {}, self.getClassName(i, j));
                //var result = self.tabularData[j][i];
                td.innerHTML = self.tabularData[j][i];
                tr.appendChild(td);
                /* column subtotals */

            } /* for all rows */

            if (self.colConditions.length && i == 0 && self.options.headingAfter) { /* blank space after */
                var th = OAT.Dom.create("th");
                if (!self.rowConditions.length) {
                    self._drawCorner(th, true);
                    th.conditionIndex = -2;
                } else { th.style.border = "none"; }
                th.rowSpan = self.rowStructure.span + (self.options.totals && self.rowConditions.length ? 1 : 0);
                tr.appendChild(th);
            }
            tbody.appendChild(tr);


        } /* for each row */

        /* totals row */


        /* second connector */
        if (self.rowConditions.length && self.options.headingAfter) {
            self._drawRowConditionsHeadings(tbody);
        }

        OAT.Dom.append([table, tbody], [self.div, table]);
    } /* drawTable */

    this.applyFilters = function () { /* create filteredData from allData */
        self.filteredData = [];
        for (var i = 0; i < self.allData.length; i++) {
            if (self.filterOK(self.allData[i])) { self.filteredData.push(self.allData[i]); }
        }



    }

    this.createAggStructure = function () { /* create a multidimensional aggregation structure */
        function createPart(struct, arr) {
            struct.items = false;
            struct.depth = -1;
            var stack = [struct];
            for (var i = 0; i < arr.length; i++) { /* for all conditions */
                var cond = self.conditions[arr[i]];
                var newstack = [];
                for (var j = 0; j < stack.length; j++) { /* for all items to be filled */
                    var items = [];
                    for (var k = 0; k < cond.distinctValues.length; k++) { /* for all currently distinct values */
                        var value = cond.distinctValues[k];
                        if (cond.blackList.find(value) == -1) {
                            var o = { value: cond.distinctValues[k], parent: stack[j], used: false, items: false, depth: i };
                            items.push(o);
                            newstack.push(o);
                        } /* if not blacklisted */
                    } /* distinct values */
                    stack[j].items = items;
                } /* items in stack */
                stack = newstack;
            } /* conditions */
        }

        createPart(self.rowStructure, self.rowConditions);
        createPart(self.colStructure, self.colConditions);
    }

    this.fillAggStructure = function () { /* mark used branches of aggregation structure */
        function fillPart(struct, arr, row) {
            var ptr = struct;
            for (var i = 0; i < arr.length; i++) {
                var rindex = arr[i];
                var value = row[rindex];
                var o = false;
                for (var j = 0; j < ptr.items.length; j++) {
                    if (ptr.items[j].value == value) {
                        o = ptr.items[j];
                        break;
                    }
                }
                if (!o) { alert("Value not found in distinct?!?!? PANIC!!!"); }
                ptr = o;
            } /* for all conditions */
            ptr.used = true;
        }

        function fillAllPart(struct) {
            var ptr = struct;
            if (!ptr.items) {
                ptr.used = true;
                return;
            }
            for (var i = 0; i < ptr.items.length; i++) { fillAllPart(ptr.items[i]); }
        }

        if (self.options.showEmpty) {
            fillAllPart(self.rowStructure);
            fillAllPart(self.colStructure);
        } else {
            for (var i = 0; i < self.filteredData.length; i++) {
                var row = self.filteredData[i];
                fillPart(self.rowStructure, self.rowConditions, row);
                fillPart(self.colStructure, self.colConditions, row);
            }
        }
    }

    this.checkAggStructure = function () { /* check structure for empty parts and delete them */
        function check(ptr) { /* recursive function */
            if (!ptr.items) { return ptr.used; } /* for leaves, return their usage state */
            for (var i = ptr.items.length - 1; i >= 0; i--) { /* if node, decide based on children count */
                if (!check(ptr.items[i])) { ptr.items.splice(i, 1); }
            }
            return (ptr.items.length > 0); /* return children state */
        }

        check(self.rowStructure);
        check(self.colStructure);
    }

    this.getLabels = function (arr, direction, glue) {
        if (!arr) { return []; }
        var result = [];
        for (var i = 0; i < arr.length; i++) {
            var item = arr[i];
            var ptr = item;
            var value = [];
            while (ptr && ptr.parent) {
                value.unshift(ptr.value);
                ptr = ptr.parent;
            }
            result.push(value.join(glue));
        }
        return result;
    }



    this.go = function () {
        self.gd.clearSources();
        self.gd.clearTargets();
        self.drawFilters();
        self.applyFilters();

        self.createAggStructure();
        self.fillAggStructure();
        self.checkAggStructure();

        self.count(); /* fill tabularData with values */
        self.drawTable();
        try {
//            if (isFiltersRedrawNeeded) //dg26 commented for Pivot alert
//            {
//                ReDrawFiltersPanel();
//            }
//            else {


//                updateCount(self); // Tries to update wrapper's record count            

//            }
            //            isFiltersRedrawNeeded = true;

            ReDrawFiltersPanel(self);
        }
        catch (err) { }

    }

    self.init();
    self.go();
}
OAT.Loader.featureLoaded("pivot");




/*************************** custom functions for extension of pivoting */
/* function to toggle pivoting*/
function togglePivot(isPivotOn) {
    var totSubCols = OAT.getSelfInstance().colConditions.length;
    var totSubRows = OAT.getSelfInstance().rowConditions;
    if (totSubCols > 0 && totSubRows.length > 0) {
        if (!isPivotOn) {
            //pivotAct = 0;
            //GenerateDataTest();
            document.getElementById('HrowTobeHide').style.display = 'none';
            document.getElementById('VcolTobeHide').style.display = 'none';
            for (var i = 0; i < totSubCols; i++) {
                document.getElementById('VcolTobeHideH' + i).style.display = 'none';
            }

            // Start for move row
            document.getElementById('dataValHCell').colSpan = "1";
            var tblHeadObj = document.getElementById('pivot_tableid');
            var totHeaderRows = OAT.getSelfInstance().headerRow;
            for (var i = 0; i < totSubRows.length; i++) {
                if (i != totSubRows.length - 1) {
                    //var newCell = tblHeadObj.rows[0].insertCell(i);
                    var newCell = document.createElement('th');
                    tblHeadObj.rows[0].insertBefore(newCell, tblHeadObj.rows[0].childNodes[i]);
                    //tblHeadObj.rows[0].prependChild(newCell);
                    newCell.id = "newColPoff" + i;
                    newCell.rowSpan = totSubCols;
                    newCell.className = "h1";

                    newCell.innerHTML = totHeaderRows[OAT.getSelfInstance().rowConditions[i]];
                }
                else {
                    document.getElementById('dataValHCell').innerHTML = totHeaderRows[OAT.getSelfInstance().rowConditions[i]];
                }
            }

            // change font weight;
            changeFontWeight("bold");
            removeGradRowPointers();
        }
        else {
            //pivotAct = 1;
            //GenerateDataTest();
            document.getElementById('HrowTobeHide').style.display = '';
            document.getElementById('VcolTobeHide').style.display = '';
            for (var i = 0; i < totSubCols; i++) {
                document.getElementById('VcolTobeHideH' + i).style.display = '';
            }

            document.getElementById('dataValHCell').innerHTML = '';

            // Start for removing additional row
            for (var i = 0; i < totSubRows.length - 1; i++) {
                var myTD = document.getElementById('newColPoff' + i);
                if (myTD != null && myTD != '' && myTD != undefined)
                    myTD.parentNode.removeChild(myTD);
            }
            document.getElementById('dataValHCell').colSpan = totSubRows.length;

            // change font weight;
            changeFontWeight("normal");
        }
    }
    else {
        //alert('no pivoting');
    }
}

/* function to make fornt bold for col headers */
function changeFontWeight(type) {
    // make bold text for col sub headers 
    var totColPoiRows = OAT.getSelfInstance().colPointers;
    for (var i = 0; i < totColPoiRows.length; i++) {
        var innerLng = totColPoiRows[i].length;
        for (var j = 0; j < innerLng; j++) {
            var myTD = document.getElementById('h2Colh' + i + j);
            if (myTD != null && myTD != '' && myTD != undefined)
                myTD.style.fontWeight = type;
        }
    }
}

/* function to remove graident for row headers */
function removeGradRowPointers() {
    // make bold text for col sub headers 
    var totRowPoiRows = OAT.getSelfInstance().rowPointers;
    for (var i = 0; i < totRowPoiRows.length; i++) {
        var innerLng = totRowPoiRows[i].length;
        for (var j = 0; j < innerLng; j++) {
            var myTD = document.getElementById('h2Rowh' + i + j);
            if (myTD != null && myTD != '' && myTD != undefined)
                myTD.className = "h2_ext";
        }
    }
}
