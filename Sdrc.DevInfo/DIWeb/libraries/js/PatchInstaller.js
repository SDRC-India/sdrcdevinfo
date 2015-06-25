
CallbackPageName = "Callback.aspx";
function InstallPatch(LogFilePath) {
    try {       
        ApplyMaskingDiv();
        di_jq("#imgInstPatchError").css("display", "none");
        di_jq("#imgInstPatchTickGray").css("display", "none");
        di_jq("#imgInstPatchTick").css("display", "none");
        di_jq("#imgInstPatchProcessing").css("display", "block");

        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '1035' },
            async: true,
            success: function (data) {
                try {                   
                    di_jq("#aLogFile").attr('href', LogFilePath);
                    di_jq("#imgInstPatchProcessing").css("display", "none");
                    if (data.toLowerCase() == "true") {
                        di_jq("#imgInstPatchTick").css("display", "block");
                        UpdateLanguageFiles();
                    }
                    else {
                        di_jq("#imgInstPatchError").css("display", "block");
                        //alert(z("LangPatchSetError").value);
                        RemoveMaskingDiv();
                    }
                }
                catch (ex) {
                    di_jq("#imgInstPatchProcessing").css("display", "none");
                    di_jq("#imgInstPatchError").css("display", "block");
                    //alert("Error : " + ex.message);

                    RemoveMaskingDiv();
                }
            },
            error: function () {
                di_jq("#imgInstPatchProcessing").css("display", "none");
                di_jq("#imgInstPatchError").css("display", "block");
                // 
                RemoveMaskingDiv();
            },
            cache: false

        });
    }
    catch (err) { }
}


function UpdateLanguageFiles() {
    try {
        di_jq("#imgUpdLngError").css("display", "none");
        di_jq("#imgUpdLngTickGray").css("display", "none");
        di_jq("#imgUpdLngTick").css("display", "none");
        di_jq("#imgUpdLngProcessing").css("display", "block");

        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '1036' },
            async: true,
            success: function (data) {
                try {
                    di_jq("#imgUpdLngProcessing").css("display", "none");

                    if (data.toLowerCase() == "true") {
                        di_jq("#imgUpdLngTick").css("display", "block");
                        UpdateAppSettingFile();
                    }
                    else {
                        di_jq("#imgUpdLngError").css("display", "block");
                        //alert(z("LangPatchSetError").value);
                        RemoveMaskingDiv();
                    }
                }
                catch (ex) {
                    di_jq("#imgUpdLngProcessing").css("display", "none");
                    di_jq("#imgUpdLngError").css("display", "block");
                    //alert("Error : " + ex.message);
                    RemoveMaskingDiv();
                }
            },
            error: function () {
                di_jq("#imgUpdLngProcessing").css("display", "none");
                di_jq("#imgUpdLngError").css("display", "block");
                //
                RemoveMaskingDiv();
            },
            cache: false
        });
    }
    catch (err) { }
}



function UpdateAppSettingFile() {
    try {
        di_jq("#imgUpdSetError").css("display", "none");
        di_jq("#imgUpdSetTickGray").css("display", "none");
        di_jq("#imgUpdSetTick").css("display", "none");
        di_jq("#imgUpdSetProcessing").css("display", "block");

        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '1037' },
            async: true,
            success: function (data) {
                try {
                    di_jq("#imgUpdSetProcessing").css("display", "none");

                    if (data.toLowerCase() == "true") {
                        di_jq("#imgUpdSetTick").css("display", "block");
                        //alert(z("LangPatchSetSuccess").value);
                        RemoveMaskingDiv();
                    }
                    else {
                        di_jq("#imgUpdSetError").css("display", "block");
                        //alert(z("LangPatchSetError").value);
                        RemoveMaskingDiv();
                    }
                }
                catch (ex) {
                    di_jq("#imgUpdSetProcessing").css("display", "none");
                    di_jq("#imgUpdSetError").css("display", "block");
                    //alert("Error : " + ex.message);
                    RemoveMaskingDiv();
                }
            },
            error: function () {
                di_jq("#imgUpdSetProcessing").css("display", "none");
                di_jq("#imgUpdSetError").css("display", "block");
                //
                RemoveMaskingDiv();
            },
            cache: false
        });
    }
    catch (err) { }
}