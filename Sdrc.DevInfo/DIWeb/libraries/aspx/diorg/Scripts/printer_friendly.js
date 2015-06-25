
var gAutoPrint = true; // Flag for whether or not to automatically call the print function



function printSpecialForArticles() {
    var disp_setting = "statusbar=no,resizable=no,menubar=no,scrollbars=no,width=600, height=800,left=100, top=50";
    if (document.getElementById != null) {
        var html = '<HTML>\n<HEAD>\n';
        html += '\n</HE' + 'AD>\n<BODY>\n';
        html += '<div style="font-weight: bold; Font-Size: 18px;">' + document.getElementById("cphMainContent_SpanMenuCategoryHeading").innerText.replace('...', '') + '</Div>';
        html += '\n</HE' + 'AD>\n<BODY>\n';
        html += '\n<div style="font-style:italic;font-size: 12px;font-weight: normal;padding-bottom:20px">' + document.getElementById("cphMainContent_SpanHeaderDescription").innerHTML +'</Div>';

        var printReadyElem = document.getElementById("cphMainContent_div_content");

        if (printReadyElem != null) {
            html +='<div>'+printReadyElem.innerHTML+'</div>';
        }
        else {
            alert("Could not find the printReady section in the HTML");
            return;
        }

        html += '\n</BO' + 'DY>\n</HT' + 'ML>';

        var printWin = window.open("", "printSpecial", disp_setting);
        printWin.document.open();
        printWin.document.write(html);
        printWin.document.close();

        if (gAutoPrint)
            printWin.print();
    }
    else {
        alert("Sorry, the print ready feature is only available in modern browsers.");
    }
}

function printSpecial() {
    var disp_setting = "statusbar=no,resizable=no,menubar=no,scrollbars=no,width=600, height=800,left=100, top=50";
    if (document.getElementById != null) {
        var html = '<HTML>\n<HEAD>\n';

        if (document.getElementsByTagName != null) {
            var headTags = document.getElementsByTagName("head");
            if (headTags.length > 0)
                html += headTags[0].innerHTML;
        }

        html += '\n</HE' + 'AD>\n<BODY>\n';

        var printReadyElem = document.getElementById("divPrintReadyArea");

        if (printReadyElem != null) {
            html += printReadyElem.innerHTML;
        }
        else {
            alert("Could not find the printReady section in the HTML");
            return;
        }

        html += '\n</BO' + 'DY>\n</HT' + 'ML>';

        var printWin = window.open("", "printSpecial", disp_setting);
        printWin.document.open();
        printWin.document.write(html);
        printWin.document.close();

        if (gAutoPrint)
            printWin.print();
    }
    else {
        alert("Sorry, the print ready feature is only available in modern browsers.");
    }
}
