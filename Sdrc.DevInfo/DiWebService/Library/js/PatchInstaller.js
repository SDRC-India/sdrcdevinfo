/*-----------------------------------------------------------------------------*/
/*                                   Start  Masking                            */
/*-----------------------------------------------------------------------------*/

function ApplyMaskingDiv(zIndex) {
    var MaskedDiv;

    try {
        MaskedDiv = document.getElementById('MaskedDiv');

        if (MaskedDiv == null) {
            MaskedDiv = document.createElement('MaskedDiv');
            MaskedDiv.id = 'MaskedDiv';
            document.body.appendChild(MaskedDiv);

        }

        // include a temporary iframe to make the ddl invisible/un-accessible
        var iFrameHtml = "<iframe style='position: absolute; display: block; background-color: #ffffff; " +
                            "z-index: -1; width: 100%; height: 100%; top: 0; left: 0; filter: mask();' >" + "</iframe>";

        // Masking the whole screen    
        if (IsIEBrowser()) {
            MaskedDiv.innerHTML += iFrameHtml;
        }
        MaskedDiv.style.display = '';
        MaskedDiv.style.visibility = 'visible';
        MaskedDiv.style.top = '0px';
        MaskedDiv.style.left = '0px';
        MaskedDiv.style.width = document.documentElement.clientWidth + 'px';
        //MaskedDiv.style.height = document.documentElement.scrollHeight + 'px';

        var PageHeight = document.documentElement.clientHeight;
        if (document.documentElement.scrollHeight > document.documentElement.clientHeight) {
            PageHeight = document.documentElement.scrollHeight;
        }
        MaskedDiv.style.height = PageHeight + 'px';

        if (zIndex) {
            MaskedDiv.style.zIndex = zIndex;
        }
    }
    catch (err) { }
}

function RemoveMaskingDiv(zIndex) {
    try {
        var MaskedDiv = document.getElementById('MaskedDiv');

        // Remove the masking from the screen    
        MaskedDiv.style.display = 'none';
        MaskedDiv.innerHTML = "";

        if (zIndex) {
            MaskedDiv.style.zIndex = zIndex;
        }
    }
    catch (err) { }
}


function IsIEBrowser() {
    var RetVal = true;

    try {
        if (/MSIE (\d+\.\d+);/.test(navigator.userAgent)) {
            //test for MSIE x.x;
            var ieversion = new Number(RegExp.$1) // capture x.x portion and store as a number
            if (ieversion >= 8) {
                // IE8 or above
                RetVal = true;
            }
            else if (ieversion >= 7) {
                // IE7.x
                RetVal = true;
            }
            else if (ieversion >= 6) {
                // IE6.x
                RetVal = true;
            }
            else if (ieversion >= 5) {
                //IE5.x
                RetVal = true;
            }
        }
        else {
            RetVal = false;
        }
    }
    catch (err) { }

    return RetVal;
}


function InstallPatch() {
    di_jq("#imgInstPatchError").css("display", "none");
    di_jq("#imgInstPatchTickGray").css("display", "none");
    di_jq("#imgInstPatchTick").css("display", "none");
    di_jq("#imgInstPatchProcessing").css("display", "block");
    ApplyMaskingDiv();
    di_jq.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "PatchInstaller.aspx/InstallPatch",
        data: "{}",
        async: true,
        success: function (data) {
            if (data.d.toLowerCase() == "true") {
                di_jq("#imgInstPatchProcessing").css("display", "none");
                di_jq("#imgInstPatchTick").css("display", "block");
                alert(document.getElementById('LangPatchSetSuccess').value);
                RemoveMaskingDiv();
            }
            else {
                di_jq("#imgInstPatchProcessing").css("display", "none");
                di_jq("#imgInstPatchError").css("display", "block");
                di_jq("#imgInstPatchTick").css("display", "none");
                RemoveMaskingDiv();
                alert(document.getElementById('LangPatchSetError').value);
            }
        },
        error: function (result) {
            di_jq("#imgInstPatchProcessing").css("display", "none");
            di_jq("#imgInstPatchError").css("display", "block");
            di_jq("#imgInstPatchTick").css("display", "none");
            RemoveMaskingDiv();
            alert(document.getElementById('LangPatchSetError').value);
        }
    });
}

