var ErrorMessage = '';

function onPageLoad(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hLoggedInUserNId, hLoggedInUserName, hOriginaldbnid, hdvnids) {    
    var hsgcount = 10;
    FillM49Countries("Callback.aspx");
	createFormHiddenInputs("frmLogin", "POST");
	SetCommonLinksHref("frmLogin", "POST");
	SetOriginaldbnidInForm("frmLogin", hOriginaldbnid);
	if ((hlngcode == null) || (hlngcode == "")) {
	    hlngcode = getDefaultLanguage();
	}
	setPostedData(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hsgcount, hLoggedInUserNId, hLoggedInUserName, hdvnids);
	// Load Language Component
	// ************************************************
	if (GetLanguageCounts() > 1) {
	    z("LanguagePipeLi").style.display = "";
	    ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', hlngcode);
	}

    z('radioHavePassword').checked = true;
    z("regenerateLink").style.display = "none";
    if (hdsby != null && hdsby != '' && hdsby.split('|').length > 1 && hdsby.split('|')[1] == "Update") {
        z("spanUpdateDetails").style.display = z("spanUpdateUserDetails").style.display = "";
        z("spanSignOrRegister").style.display = z("spanRequestLogin").style.display = "none";
        z("reg_rgt_sec").style.display = "none";
        z("spanPassword").style.display = z("txtboxPwd").style.display = z("spanConfirmPassword").style.display = z("txtboxConfirmPwd").style.display = "none";       
        if (hLoggedInUserNId.split('|')[1] == 'True') {
            z('chkRegisterProvider').style.display = z('spanRegisterProvider').style.display = z('imghelpRegisterProvider').style.display = "none";
        }
        GetUserDetails(hLoggedInUserNId);        
    }
    
}


function Login() {
   
    var LoginErrorMsgArr = [z("lngAccountNotActivatedMsg").innerHTML, z("lngSuccessfullyLoginMsg").innerHTML, z("lngRegenerateLinkMsg").innerHTML, z("lngInvalidCredentialsMsg").innerHTML, z("lngInternalErrorMsg").innerHTML];
    var InputParam;
 
    if ((z('txtEmail').value != null) && (z('txtEmail').value != "") && (z('txtHavePassword').value != null) && (z('txtHavePassword').value != ""))
    {
       
        InputParam = z('txtEmail').value;
        InputParam += ParamDelimiter + z('txtHavePassword').value;
       
        try
        {
           
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: CallBackPageName,
                data: { 'callback': '49', 'param1': InputParam },
                async: true,
                success: function (data) {
                    try {
                        if (data.split(ParamDelimiter)[0] == "true") {
                            if (getQueryString("popup") == "true") {
                                var parentfunObjet = getParentFunction("parentFunction");
                                parentfunObjet(data);
                            }
                            else {
                                LoginLogoutUser(data.split(ParamDelimiter)[2], data.split(ParamDelimiter)[3]);
                                if (z('hdsby').value != null && z('hdsby').value != '' && z('hdsby').value.split('|')[0] != 'Login.aspx') {
                                    PostData("frmLogin", z('hdsby').value.split('|')[0], "POST");
                                }
                                else {
                                    if (GetStandalonRegistryStatus().toLowerCase() == "true") {
                                        PostData("frmLogin", "RegStructure.aspx", "POST");
                                    }
                                    else {
                                        PostData("frmLogin", "home.aspx", "POST");
                                    }
                                }
                            }
                        }
                        else {
                            if (data.split(ParamDelimiter)[1] != "1") {
                                alert(LoginErrorMsgArr[data.split(ParamDelimiter)[1]]);
                                if (data.split(ParamDelimiter)[1] == "2") {
                                    z("regenerateLink").style.display = "block";
                                }
                                z('txtEmail').focus();
                                z('txtHavePassword').value = "";
                            }
                        }
                    }
                    catch (ex) {
                        alert("message:" + ex.message);
                    }
                },
                error: function () {
                    
                },
                cache: false
            });
        }
        catch(ex){        
        }
    }
    else
    {
        alert(z("lngIDPasswordMsg").innerHTML);
    }
}

function SignUp() {
    if (di_jq("#ddlCountry").val() == "-1") {
        alert(z("lngSelectCountryMsg").innerHTML);
        return;
    }
    var InputParam, EmailId;
   
    ApplyMaskingDiv();
    ShowLoadingDiv();
        
    if(ValidateInputs() == "true")
    {
            
        InputParam = z('txtEmail').value;
        InputParam += ParamDelimiter + z('txtFirstName').value;
        InputParam += ParamDelimiter + di_jq('#ddlCountry').val();
        InputParam += ParamDelimiter + z('txtPassword').value;
        
        if (z('chkRegisterProvider').checked == true)
        {
            InputParam += ParamDelimiter + "true";
        }
        else
        {
            InputParam += ParamDelimiter + "false";
        }

        InputParam += ParamDelimiter + "false";

        if (z('chkSendDevInfoUpdates').checked == true) {
            InputParam += ParamDelimiter + "true";
        }
        else {
            InputParam += ParamDelimiter + "false";
        }

        InputParam += ParamDelimiter + document.getElementById('hlngcode').value;
        if (getQueryString("popup") == "true") {
            InputParam += ParamDelimiter + "true";
        }
        else {
            InputParam += ParamDelimiter + "false";
        }

        try
        {
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: CallBackPageName,
                data: { 'callback': '48', 'param1': InputParam },
                async: false,
                success: function (data) {
                    try {
                        if (data.split(ParamDelimiter)[0] == "true") {
                            if (getQueryString("popup") == "true") {
                                data += ParamDelimiter + z("txtFirstName").value;
                                var parentfunObjet = getParentFunction("parentFunction");                                
                                parentfunObjet(data);                                
                            }
                            else {
                                alert(z("lngPlzVerifyMsg").innerHTML);
                                EmailId = z('txtEmail').value;
                                LoginLogoutUser('', '');
                                z('txtEmail').value = EmailId;
                            }
                        }
                        else {
                            alert(data.split(ParamDelimiter)[1]);
                        }
                    }
                    catch (ex) {
                        alert("message:" + ex.message);
                    }
                },
                error: function () {
                    
                },
                cache: false
            });
        }
        catch(ex){        
        }
    }
    else
    {
        alert(ErrorMessage);
    }
    
     HideLoadingDiv();
     RemoveMaskingDiv();
}

function Update() {
    if (di_jq("#ddlCountry").val() == "-1") {
        alert(z("lngSelectCountryMsg").innerHTML);
        return;
    }

    var InputParam;
   
    ApplyMaskingDiv();
    ShowLoadingDiv();

    if (ValidateInputsForUpdate() == "true")
    {
        InputParam = z('hLoggedInUserNId').value;
        InputParam += ParamDelimiter + z('txtEmail').value;
        InputParam += ParamDelimiter + z('txtFirstName').value;
        InputParam += ParamDelimiter + di_jq('#ddlCountry').val();
        InputParam += ParamDelimiter + "null";

        if (z('hLoggedInUserNId').value.split('|')[1] != 'True') {
            if (z('chkRegisterProvider').checked == true) {
                InputParam += ParamDelimiter + "true";
            }
            else {
                InputParam += ParamDelimiter + "false";
            }
        }
        else {
            InputParam += ParamDelimiter + "false";
        }

        if (z('chkSendDevInfoUpdates').checked == true) {
            InputParam += ParamDelimiter + "true";
        }
        else {
            InputParam += ParamDelimiter + "false";
        }

        InputParam += ParamDelimiter + document.getElementById('hlngcode').value;
        
        try
        {
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: CallBackPageName,
                data: {'callback': '68', 'param1': InputParam},
                async: false,
                success: function(data){                              
                    try
                    {
    				    if (data.split(ParamDelimiter)[0] == "true")
    				    {
    				        alert(z("lngDetailsUpdatedMsg").innerHTML);
    				        //EmailId = z('txtEmail').value;
    				        LoginLogoutUser(z('hLoggedInUserNId').value, z('txtFirstName').value);
    				        //z('txtEmail').value = EmailId;
    				    }
    				    else
    				    {
    				        alert(data.split(ParamDelimiter)[1]);
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
        alert(ErrorMessage);
    }
    
    HideLoadingDiv();
    RemoveMaskingDiv();
}

function ForgotPassword(){
    var InputParam;
    var langcode ="";
    ApplyMaskingDiv();
    ShowLoadingDiv();
    
    if((z('txtEmail').value != null) && (z('txtEmail').value != ""))
    {
        InputParam = z('txtEmail').value;
        if (getQueryString("popup") == "true") {
            InputParam += ParamDelimiter + "true";
        }
        else {
            InputParam += ParamDelimiter + "false";
        }
        if (document.getElementById('hlngcode').value == "" || document.getElementById('hlngcode').value == null) {
            langcode = readCookie("hlngcode");
        }
        else {
            langcode = document.getElementById('hlngcode').value;
        }
        InputParam += ParamDelimiter + langcode;
//        InputParam += ParamDelimiter + document.getElementById('hlngcode').value;
        try
        {
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: CallBackPageName,
                data: {'callback': '70', 'param1': InputParam},
                async: false,
                success: function(data){                              
                    try
                    {
    				    alert(data.split(ParamDelimiter)[1]);
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
        alert(z("lngEnterIdMsg").innerHTML);
    }
    
    HideLoadingDiv();
    RemoveMaskingDiv();
}

function HandleEnter(event) {
    if (event.keyCode == 13) 
    {
        z('btnLogin').click();
    }
}

function GetUserDetails(hLoggedInUserNId){
    InputParam = hLoggedInUserNId.split('|')[0];
            
    try
    {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: { 'callback': '67', 'param1': InputParam },
            async: true,
            success: function (data) {
                try {
                    z('radioRegister').checked = true;
                    HideShowRegisterForm();
                    z('txtEmail').value = data.split(ParamDelimiter)[0];
//                    z('txtPassword').value = z("txtConfirmPassword").value = data.split(ParamDelimiter)[1];
                    z('txtPassword').value = z("txtConfirmPassword").value = '';
                    z('txtFirstName').value = data.split(ParamDelimiter)[2];
                    di_jq('#ddlCountry').val(data.split(ParamDelimiter)[4]);

                    if (data.split(ParamDelimiter)[5].toLowerCase() == "true") {
                        z('chkRegisterProvider').defaultChecked = true;
                    }
                    else {
                        z('chkRegisterProvider').defaultChecked = false;
                    }

                    if (data.split(ParamDelimiter)[7].toLowerCase() == "true") {
                        z('chkSendDevInfoUpdates').defaultChecked = true;
                    }
                    else {
                        z('chkSendDevInfoUpdates').defaultChecked = false;
                    }

                    z('btnSignup').style.display = "none";
                    z('btnUpdate').style.display = "inline";

                    z('radioRegister').style.display = "none";
                    z('radioHavePassword').style.display = "none";
                    z('spanRegister').style.display = "none";
                    z('spanHavePassword').style.display = "none";
                    z("changePwdATag").style.display = "block";
                }
                catch (ex) {
                    alert("message:" + ex.message);
                }
            },
            error: function () {
                
            },
            cache: false
        });
    }
    catch(ex){        
    }
}

function ValidateInputs(){
    var RetVal;
    
    RetVal = "true"
    
    if ((z('txtEmail').value == null) || (z('txtEmail').value == ""))
    {
        RetVal = "false";
        ErrorMessage = z("lngIdMandatoryMsg").innerHTML;
    }
    else if (validateEmail(z('txtEmail').value) == false)
    {
        RetVal = "false";
        ErrorMessage = z("lngIdFormatMsg").innerHTML;
    }
    else if ((z('txtFirstName').value == null) || (z('txtFirstName').value == ""))
    {
        RetVal = "false";
        ErrorMessage = z("lngNameMandatoryMsg").innerHTML;
    }
    else if ((z('txtPassword').value == null) || (z('txtPassword').value == ""))
    {
        RetVal = "false";
        ErrorMessage = z("lngEnterPwdMsg").innerHTML;
    }
    else if (z('txtPassword').value.length < 8)
    {
        RetVal = "false";
        ErrorMessage = z("lngPwdLenMsg").innerHTML;
    }
    else if ((z('txtConfirmPassword').value == null) || (z('txtConfirmPassword').value == ""))
    {
        RetVal = "false";
        ErrorMessage = z("lngConfirmPwdMsg").innerHTML;
    }
    else if(z('txtPassword').value != z('txtConfirmPassword').value)
    {
        RetVal = "false";
        ErrorMessage = z("lngReenterPwdMsg").innerHTML;
        z('txtPassword').value = "";
        z('txtConfirmPassword').value = "";
    }
    
    return RetVal;
}


function ValidateInputsForUpdate() {
    var RetVal;

    RetVal = "true"

    if ((z('txtEmail').value == null) || (z('txtEmail').value == "")) {
        RetVal = "false";
        ErrorMessage = z("lngIdMandatoryMsg").innerHTML;
    }
    else if (validateEmail(z('txtEmail').value) == false) {
        RetVal = "false";
        ErrorMessage = z("lngIdFormatMsg").innerHTML;
    }
    else if ((z('txtFirstName').value == null) || (z('txtFirstName').value == "")) {
        RetVal = "false";
        ErrorMessage = z("lngNameMandatoryMsg").innerHTML;
    }
    return RetVal;
}

function HideShowRegisterForm(){
    Reset();
    z("changePwdATag").style.display = "none";
    if(z('radioRegister').checked == true)
    {
        z('tblRegister').style.display = "block";
        z('txtHavePassword').style.display = "none";
        z('aForgotPassword').style.display = "none";
        z('btnLogin').style.display = "none";
        z('spanEmailMandatory').style.display = "";
        z("regenerateLink").style.display = "none";
        //z("spanPassword").style.display = z("txtboxPwd").style.display = z("spanConfirmPassword").style.display = z("txtboxConfirmPwd").style.display = "";
    }
    else if (z('radioHavePassword').checked == true)
    {
        z('tblRegister').style.display = "none";
        z('txtHavePassword').style.display = "block";
        z('aForgotPassword').style.display = "block";
        z('btnLogin').style.display = "block";
        z('spanEmailMandatory').style.display = "none";
        z("regenerateLink").style.display = "none";
        //z("spanPassword").style.display = z("txtboxPwd").style.display = z("spanConfirmPassword").style.display = z("txtboxConfirmPwd").style.display = "none";
    }
}

function Reset() {
    try {
        z('txtHavePassword').value = "";
        z('txtFirstName').value = "";
        di_jq('#ddlCountry').val("-1");
        z('txtPassword').value = "";
        z('txtConfirmPassword').value = "";
        z('chkRegisterProvider').defaultChecked = false;
        z('chkSendDevInfoUpdates').defaultChecked = false;

        z('btnSignup').style.display = "inline";
        z('btnUpdate').style.display = "none";

        z('radioRegister').style.display = "inline";
        z('radioHavePassword').style.display = "inline";
        z('spanRegister').style.display = "inline";
        z('spanHavePassword').style.display = "inline";
    }
    catch (err) {
    }    
}

function FillM49Countries(callbackurl) {
    var htmlResp = di_jq.ajax({
        type: "POST",
        url: callbackurl,
        data: { 'callback': 270 },
        async: false,
        success: function (data) {
            try {                
                if (data != "") {
                    var SelectObj = z("ddlCountry");
                    var MenuObj = document.createElement("option");                    
                    MenuObj.text = "Select";
                    MenuObj.value = "-1";
                    SelectObj.add(MenuObj);
                    var AreaData = jQuery.parseJSON(data);
                    for (var i = 0; i < AreaData.Areas.length; i++) {
                        var MenuObj = document.createElement("option");
                        var AreaNid = AreaData.Areas[i].AREANID;
                        var AreaName = AreaData.Areas[i].AREANAME;
                        MenuObj.text = AreaName;
                        MenuObj.value = AreaNid;
                        SelectObj.add(MenuObj);
                    }
                }
            }
            catch (ex) {

            }
        },
        error: function () {
            
        },
        cache: false
    });
}

function ToggleCallout(divID, evt, helpDivId) {
    var helpDiv = z(helpDivId);
    var evnt = evt || window.event;
    if ((z(divID).style.display == "none") || (z(divID).style.display == "")) {
        for (var i = 0; i < z(divID + 'Text').children.length; i++) {
            z(divID + 'Text').children[i].style.display = "none";
        }
        helpDiv.style.display = "";

        SetCalloutPosition(evt, z(divID), 20, 0);

        z(divID).style.display = "block";
    }
    else {
        HideCallout(divID);
    }
}

/* function to get location query string */
function getQueryString(qSName) {
    var val=null;
    try
    {
        var url = window.location.search.substring(1);
        var url = url.split("&");
        for (var i = 0; i < url.length; i++) {
            var strVal = url[i].split("=");
            if (strVal[0] == qSName) {
                val= strVal[1];
            }
        }
    }
    catch(err)
    {}
    return val;
}

function getParentFunction(funcName) {
    var func = null;
    // Child opened in new window e.g. target="blank"
    if (top.window.opener && !top.window.opener.closed) {
        try { func = eval("top.window.opener." + funcName); } catch (error) { }
    }
    if (!(func)) {
        // Child opened in IFRAME
        try { func = eval("top." + funcName); } catch (error) { }
    }
    if (!(func)) {
        throw new Error("function \"" + funcName + "\" is not in parent window.");
    }
    return func;
}

function AccountActivationProcess() {
    var InputParam;
    var EmailId = getQueryString("emailid");
    var TokenKey = getQueryString("key");
    var IsRegistration = getQueryString("flow");
    var msgArray = [z("lngAccountActivationMsg").innerHTML, z("lngTimeExpireMsg").innerHTML, z("lngTokenExpireMsg").innerHTML, z("lngIdNotRegisterMsg").innerHTML];
    ApplyMaskingDiv();
    ShowLoadingDiv();
    z("btnSkip").style.display = "none";
    try {
        InputParam = EmailId + ParamDelimiter + TokenKey + ParamDelimiter + IsRegistration;
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: { 'callback': '288', 'param1': InputParam },
            async: false,
            success: function (result) {
                try {
                    var resultArr = result.split(ParamDelimiter);
                    if (z("proceedInfoAtag")) {
                        z('proceedInfoAtag').href = "javascript:PostData('frmActivation','home.aspx','POST');";
                    }
                    if (resultArr[0] == "true") {
                        if (IsRegistration.toLowerCase() == "true") {
                            z("resultTag").innerHTML = msgArray[resultArr[1]];
                            z("proceedInfoAtag").style.display = "none";
                            z("reg_lft_sec").style.display = "none";
                            //z("btnSkip").style.display = "";
                            //LoginLogoutUser(resultArr[2], resultArr[3]);
                        }
                        else {
                            z("resultTag").innerHTML = msgArray[resultArr[1]];
                            z("proceedInfoAtag").style.display = "none";
                            z("reg_lft_sec").style.display = "none";
                            //z("unihidden").value = resultArr[2];
                            //z("btnSkip").style.display = "none";
                        }
                    }
                    else {
                        if (resultArr[1] == "0") {
                            z("unihidden").value = resultArr[2];
                            z("proceedInfoAtag").style.display = "none";
                            z("reg_lft_sec").style.display = "";
                        }
                        else {
                            //                            z("resultTag").innerHTML = msgArray[resultArr[1]];
                            z("proceedInfoAtag").style.display = "none";
                            z("reg_lft_sec").style.display = "none";
                            alert(z("lngRegenerateLinkMsg").innerHTML);
                            z("regenerateLink").style.display = "block";
                        }
                    }
                }
                catch (ex) {
                    alert("message:" + ex.message);
                }
            },
            error: function () {
                
            },
            cache: false
        });
        }
    catch (ex) {
        }    
    HideLoadingDiv();
    RemoveMaskingDiv();
}


function ValidatePasswordInputs() {
    var RetVal;

    RetVal = "true"
    if ((z('txtNewPwd').value == null) || (z('txtNewPwd').value == "")) {
        RetVal = "false";
        ErrorMessage = z("lngEnterPwdMsg").innerHTML;
    }
    else if (z('txtNewPwd').value.length < 8) {
        RetVal = "false";
        ErrorMessage = z("lngPwdLenMsg").innerHTML;
    }
    else if ((z('txtConfirmPwd').value == null) || (z('txtConfirmPwd').value == "")) {
        RetVal = "false";
        ErrorMessage = z("lngConfirmPwdMsg").innerHTML;
    }
    else if (z('txtNewPwd').value != z('txtConfirmPwd').value) {
        RetVal = "false";
        ErrorMessage = z("lngReenterPwdMsg").innerHTML;
        z('txtNewPwd').value = "";
        z('txtConfirmPwd').value = "";
    }

    return RetVal;
}

function ChangePassword() {
    var InputParam;    
    ApplyMaskingDiv();
    ShowLoadingDiv();
    var ErrorMsgArr = [z("lngOldPwdWrongMsg").innerHTML, z("lngPwdChangedMsg").innerHTML, z("lngAccountNotExistMsg").innerHTML, z("lngInternalErrorMsg").innerHTML, z("lngAccountActivationMsg").innerHTML];
    if (ValidatePasswordInputs() == "true") {
        var UserNid, OldPwd;
        var Password = z("txtNewPwd").value;
        var IsRegistration= "true"
        if (getQueryString("flow")) {
            IsRegistration = getQueryString("flow");
        }
        if (z("hLoggedInUserNId").value!="") {
            try {
                UserNid = z("hLoggedInUserNId").value.split("|")[0];
                OldPwd = z("txtOldPwd").value;
            }
            catch (err) { }
        }
        else {
            UserNid = z("unihidden").value;
            OldPwd = "null";
        }        
        try {
            InputParam = UserNid + ParamDelimiter + Password + ParamDelimiter + IsRegistration + ParamDelimiter + OldPwd + ParamDelimiter + document.getElementById('hlngcode').value;
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: CallBackPageName,
                data: { 'callback': '289', 'param1': InputParam },
                async: false,
                success: function (result) {
                    try {
                        var resultArr = result.split(ParamDelimiter);
                        if (resultArr[0] == "true") {
                            //                            alert(ErrorMsgArr[1]);
                            if (OldPwd == "null") {
                                LoginLogoutUser(resultArr[2], resultArr[3]);
                                z("resultTag").innerHTML = ErrorMsgArr[1];
                                z("proceedInfoAtag").style.display = "";
                                z("reg_lft_sec").style.display = "none";
                                //z("proceedInfoAtag").click();
                            }
                            else {
                                ShowHidePasswordPopup(false);
                            }
                        }
                        else {
                            if (resultArr[1] == "0") {
                                z("txtOldPwd").value = z("txtNewPwd").value = z("txtConfirmPwd").value = "";
                            }
                            alert(ErrorMsgArr[resultArr[1]]);
                        }
                    }
                    catch (ex) {
                        alert("message:" + ex.message);
                    }
                },
                error: function () {
                    
                },
                cache: false
            });
        }
        catch (ex) {
        }
    }
    else {
        alert(ErrorMessage);
    }
    HideLoadingDiv();
    RemoveMaskingDiv();
}

function onActivationPageLoad(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hLoggedInUserNId, hLoggedInUserName, hOriginaldbnid, hdvnids) {
    var hsgcount = 10;    
    createFormHiddenInputs("frmActivation", "POST");
    SetCommonLinksHref("frmActivation", "POST");
    SetOriginaldbnidInForm("frmActivation", hOriginaldbnid);

    setPostedData(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hsgcount, hLoggedInUserNId, hLoggedInUserName, hdvnids);
    // Load Language Component
    // ************************************************
    if (GetLanguageCounts() > 1) {
        z("LanguagePipeLi").style.display = "";
        ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', hlngcode);
    }
}

function SkipChangePassword() {
    z("proceedInfoAtag").click();
}

function ShowHidePasswordPopup(isOpen) {
    if (isOpen) {        
        z("txtOldPwd").value = z("txtNewPwd").value = z("txtConfirmPwd").value = "";
        ApplyMaskingDiv();
        SetCloseButtonInPopupDiv(di_jq('#PasswodPopup'), 'ShowHidePasswordPopup');
        di_jq('#PasswodPopup').show('slow');
        GetWindowCentered(z("PasswodPopup"), 710, 450);
    }
    else {
        di_jq('#PasswodPopup').hide('slow');
        RemoveMaskingDiv();
    }
}
function AccountActivation() {
    var InputParam;
    var EmailId = getQueryString("emailid");
    var TokenKey = getQueryString("key");
    var IsRegistration = getQueryString("flow");
    ApplyMaskingDiv();
    ShowLoadingDiv();
    try {
        InputParam = EmailId + ParamDelimiter + TokenKey + ParamDelimiter + IsRegistration;
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: { 'callback': '288', 'param1': InputParam },
            async: false,
            success: function (result) {
                try {
                    var resultArr = result.split(ParamDelimiter);
                    if (resultArr[0] == "false" && resultArr[1] == "1") {
                        alert(z("lngRegenerateLinkMsg").innerHTML);
                        z("regenerateLink").style.display = "block";
                    }
                    else {
                        z('langAccountActivated').style.display = "";
                        z('divLoginHome').style.display = "";
                        if (GetStandalonRegistryStatus().toLowerCase() == "true") {
                            document.getElementById('aActivationHome').href = "RegStructure.aspx";
                        }
                        else {
                            document.getElementById('aActivationHome').href = "Home.aspx";
                        }
                        var defLang = getDefaultLanguage();
                        if ((document.getElementById('hlngcode').value == "") || (document.getElementById('hlngcode').value == null)) {
                            document.getElementById('hlngcode').value = defLang;
                        }
                    }

                }
                catch (ex) {
                    z('langErrorInActivation').style.display = "";
                    //                    z('langAccountActivated').style.display = "";
                }
            },
            error: function () {
                
            },
            cache: false
        });
    }
    catch (ex) {
    }
    HideLoadingDiv();
    RemoveMaskingDiv();
}
function RegenerateActivationLink() {
    var InputParam;
    var langcode = "";
    ApplyMaskingDiv();
    ShowLoadingDiv();


    InputParam = getQueryString("emailid");
    if (getQueryString("popup") == "true") {
        InputParam += ParamDelimiter + "true";
    }
    else {
        InputParam += ParamDelimiter + "false";
    }
    if (document.getElementById('hlngcode').value == "" || document.getElementById('hlngcode').value == null) {
        langcode = readCookie("hlngcode");
    }
    else {
        langcode = document.getElementById('hlngcode').value;
    }
    InputParam += ParamDelimiter + langcode;
    //        InputParam += ParamDelimiter + document.getElementById('hlngcode').value;
    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: { 'callback': '1007', 'param1': InputParam },
            async: false,
            success: function (data) {
                try {
                    alert(data.split(ParamDelimiter)[1]);
                    z("regenerateLink").style.display = "none";
                }
                catch (ex) {
                    alert("message:" + ex.message);
                }
            },
            error: function () {
                
            },
            cache: false
        });
    }
    catch (ex) {
    }


    HideLoadingDiv();
    RemoveMaskingDiv();
}

function RegenerateForgotPasswordLink() {
    var InputParam;
    var langcode = "";
    ApplyMaskingDiv();
    ShowLoadingDiv();


    InputParam = getQueryString("emailid");
    if (getQueryString("popup") == "True") {
        InputParam += ParamDelimiter + "true";
    }
    else {
        InputParam += ParamDelimiter + "false";
    }
    if (document.getElementById('hlngcode').value == "" || document.getElementById('hlngcode').value == null) {
        langcode = readCookie("hlngcode");
    }
    else {
        langcode = document.getElementById('hlngcode').value;
    }
    InputParam += ParamDelimiter + langcode;
    //        InputParam += ParamDelimiter + document.getElementById('hlngcode').value;
    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: { 'callback': '1008', 'param1': InputParam },
            async: false,
            success: function (data) {
                try {
                    alert(data.split(ParamDelimiter)[1]);
                    z("regenerateLink").style.display = "none";
                }
                catch (ex) {
                    alert("message:" + ex.message);
                }
            },
            error: function () {
                
            },
            cache: false
        });
    }
    catch (ex) {
    }


    HideLoadingDiv();
    RemoveMaskingDiv();
}