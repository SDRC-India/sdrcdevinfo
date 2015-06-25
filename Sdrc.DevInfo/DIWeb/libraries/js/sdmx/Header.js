var HeaderLanguageCode;
var hdnHeaderDSDId = ''
var hdnHeaderDSDName = ''
var hdnHeaderPrepared = '';

function CreateSenderRecieverHeader(IsDevInfoDatabase, DbNid, SelectedDbDSDName) {
    var CallbackPageName = "Callback.aspx";
    var InputParam = DbNid;
    InputParam += ParamDelimiter + IsDevInfoDatabase;
    var HTMLStr = "";
    if (IsDevInfoDatabase) {
        CallbackPageName = "../Callback.aspx";
    }
    di_jq.ajax({
        type: "Get",
        url: CallbackPageName,
        data: { 'callback': '1045', 'param1': InputParam },
        async: false,
        beforeSend: function () {
            // ApplyMaskingDiv();
        },
        success: function (data) {
            try {
                if (data != "") {
                    HTMLStr = PopulateSenderRecHtml(data, SelectedDbDSDName, IsDevInfoDatabase, DbNid);
                    z('DivHeaderContainer').innerHTML = HTMLStr;

                }
                else {
                    z('DivHeaderContainer').innerHTML = "";
                }
            }
            catch (ex) {
                alert("Error : " + ex.message);
            }
        },
        error: function () {
           // 
        },
        complete: function () {
            // RemoveMaskingDiv();
            // LanguageHandlingOfSenderRecieverGrid();
        },
        cache: false
    });
}


function PopulateSenderRecHtml(data, SelectedDbDSDName, IsDevInfoDatabase, DbNid) {
    var ObjHeadersDetails;
    var DSDName = '';
    var HTMLStr = "";
    ObjHeadersDetails = di_jq.parseJSON(data);
  
    if (ObjHeadersDetails != null) {
        DSDName = SelectedDbDSDName;
        hdnHeaderDSDId = ObjHeadersDetails.HeaderId;
        hdnHeaderDSDName = ObjHeadersDetails.HeaderDsdName;
        HeaderLanguageCode = ObjHeadersDetails.LanguageCode;
        var SenderId = ObjHeadersDetails.SenderId;
        var SenderName = ObjHeadersDetails.SenderName;
        var SenderContactName = ObjHeadersDetails.SenderContactName;
        var SenderDepartment = ObjHeadersDetails.SenderDepartment;
        var SenderRole = ObjHeadersDetails.SenderRole;
        var SenderFax = ObjHeadersDetails.SenderFax;
        var SenderTelephone = ObjHeadersDetails.SenderTelephone;
        var SenderEmail = ObjHeadersDetails.SenderEmail;

        var RecieverName = ObjHeadersDetails.RecieverName;
        var RecieverId = ObjHeadersDetails.RecieverId;
        var RecieverContactName = ObjHeadersDetails.RecieverContactName;
        var RecieverDepartment = ObjHeadersDetails.RecieverDepartment;
        var RecieverRole = ObjHeadersDetails.RecieverRole;
        var RecieverFax = ObjHeadersDetails.RecieverFax;
        var RecieverTelephone = ObjHeadersDetails.RecieverTelephone;
        var RecieverEmail = ObjHeadersDetails.RecieverEmail;
        hdnHeaderPrepared = ObjHeadersDetails.Prepared;
  
        //Set Sender details
        HTMLStr += '<h2 id="DSDName">"' + DSDName + '"</h2></br>';
        HTMLStr += '<div class="pddn_lt_tn">';
        HTMLStr += '<p class="confg_frm_txt fnt_sz_fftn fnt_bld pddn_tp_fftn" id="Sender_Detail_Head_Text" ></p>';
        HTMLStr += '<div class="clear"></div>';

        HTMLStr += '<p class="confg_frm_txt" id="Sender_Name" > </p>';
        HTMLStr += '<p class="confg_frm_inp_bx"><input id="Sender_Name_Val" type="text"class="confg_frm_inp_bx_txt"  value="' + SenderName + '"></p>';
        HTMLStr += '<div class="clear"></div>';

        HTMLStr += '<p class="confg_frm_txt" id="Sender_Id" > </p>';
        HTMLStr += '<p class="confg_frm_inp_bx"><input id="Sender_Id_Val" type="text"class="confg_frm_inp_bx_txt" value="' + SenderId + '" ></p>';
        HTMLStr += '<div class="clear"></div>';

        HTMLStr += '<p class="confg_frm_txt fnt_bld" id="Sender_Contact_Detail" ></p>';
        HTMLStr += '<div class="clear"></div>';

        HTMLStr += '<p class="confg_frm_txt" id="Sender_Contact_Name"> </p>';
        HTMLStr += '<p class="confg_frm_inp_bx"><input id="Sender_Contact_Name_Val" type="text" class="confg_frm_inp_bx_txt" value="' + SenderContactName +'" ><p>';
        HTMLStr += '<div class="clear"></div>';

        HTMLStr += '<p class="confg_frm_txt" id="Sender_Contact_Dept"> </p>';
        HTMLStr += '<p class="confg_frm_inp_bx"><input id="Sender_Contact_Dept_Val" type="text" class="confg_frm_inp_bx_txt" value="' + SenderDepartment + '" ></p>';
        HTMLStr += '<div class="clear"></div>';

        HTMLStr += '<p class="confg_frm_txt" id="Sender_Contact_Role"> </p>';
        HTMLStr += '<p class="confg_frm_inp_bx"><input id="Sender_Contact_Role_Val" type="text" class="confg_frm_inp_bx_txt" value="' + SenderRole + '"></p>';
        HTMLStr += '<div class="clear"></div>';

        HTMLStr += '<p class="confg_frm_txt" id="Sender_Contact_Fax"> </p>';
        HTMLStr += '<p class="confg_frm_inp_bx"><input id="Sender_Contact_Fax_Val"   type="text" class="confg_frm_inp_bx_txt" value="' + SenderFax + '"></p>';
        HTMLStr += '<div class="clear"></div>';

        HTMLStr += '<p class="confg_frm_txt" id="Sender_Contact_Phone"> </p>';
        HTMLStr += '<p class="confg_frm_inp_bx"><input id="Sender_Contact_Phone_Val"  type="text" class="confg_frm_inp_bx_txt" value="' + SenderTelephone + '"></p>';
        HTMLStr += '<div class="clear"></div>';

        HTMLStr += '<p class="confg_frm_txt" id="Sender_Contact_Email"> </p>';
        HTMLStr += '<p class="confg_frm_inp_bx"><input id="Sender_Contact_Email_Val"  type="text" class="confg_frm_inp_bx_txt" value="' + SenderEmail + '"></p>';
        HTMLStr += '<div class="clear"></div>';


        //Set Reciever details
        HTMLStr += '<p class="confg_frm_txt fnt_sz_fftn fnt_bld pddn_tp_tyfor" id="Reciever_Detail_Head_Text" ></p>';
        HTMLStr += '<div class="clear"></div>';

        HTMLStr += '<p class="confg_frm_txt" id="Reciever_Name"> </p>';
        HTMLStr += '<p class="confg_frm_inp_bx"><input id="Reciever_Name_Val" type="text" class="confg_frm_inp_bx_txt" value="' + RecieverName + '" ></p>';
        HTMLStr += '<div class="clear"></div>';

        HTMLStr += '<p class="confg_frm_txt" id="Reciever_Id"> </p>';
        HTMLStr += '<p class="confg_frm_inp_bx"><input id="Reciever_Id_Val"  type="text" class="confg_frm_inp_bx_txt" value="' + RecieverId + '" ></p>';
        HTMLStr += '<div class="clear"></div>';

        HTMLStr += '<p class="confg_frm_txt fnt_bld" id="Reciever_Contact_Detail" ></p>';
        HTMLStr += '<div class="clear"></div>';

        HTMLStr += '<p class="confg_frm_txt" id="Reciever_Contact_Name"> </p>';
        HTMLStr += '<p class="confg_frm_inp_bx"><input id="Reciever_Contact_Name_Val"  type="text" class="confg_frm_inp_bx_txt"  value="' + RecieverContactName + '"></p>';
        HTMLStr += '<div class="clear"></div>';

        HTMLStr += '<p class="confg_frm_txt" id="Reciever_Contact_Dept"> </p>';
        HTMLStr += '<p class="confg_frm_inp_bx"><input id="Reciever_Contact_Dept_Val"  type="text" class="confg_frm_inp_bx_txt" value="' + RecieverDepartment + '"></p>';
        HTMLStr += '<div class="clear"></div>';

        HTMLStr += '<p class="confg_frm_txt" id="Reciever_Contact_Role"> </p>';
        HTMLStr += '<p class="confg_frm_inp_bx"><input id="Reciever_Contact_Role_Val"  type="text" class="confg_frm_inp_bx_txt" value="' + RecieverRole + '"></p>';
        HTMLStr += '<div class="clear"></div>';

        HTMLStr += '<p class="confg_frm_txt" id="Reciever_Contact_Fax"> </p>';
        HTMLStr += '<p class="confg_frm_inp_bx"><input id="Reciever_Contact_Fax_Val" type="text" class="confg_frm_inp_bx_txt" value="' + RecieverFax + '"></p>';
        HTMLStr += '<div class="clear"></div>';

        HTMLStr += '<p class="confg_frm_txt" id="Reciever_Contact_Phone"> </p>';
        HTMLStr += '<p class="confg_frm_inp_bx"><input id="Reciever_Contact_Phone_Val" type="text" class="confg_frm_inp_bx_txt" value="' + RecieverTelephone + '" ></p>';
        HTMLStr += '<div class="clear"></div>';

        HTMLStr += '<p class="confg_frm_txt" id="Reciever_Contact_Email"> </p>';
        HTMLStr += '<p class="confg_frm_inp_bx"><input id="Reciever_Contact_Email_Val" type="text" class="confg_frm_inp_bx_txt" value="' + RecieverEmail + '" ></p>';
        HTMLStr += '<div class="clear"></div>';
        HTMLStr += '</div>';
        HTMLStr += '</div>';
        HTMLStr += "<div class='hdr_upd_bttn pddn_tp_tyfor'><input type='button' id='langUpdate' value='Update' onclick='SaveHeaderDetails(" + IsDevInfoDatabase + "," + DbNid + ");' class='hdr_submit_bttn'></div>";

    }
    return HTMLStr;
}

function SaveHeaderDetails(IsDevInfoDatabase, DbNid) {
    var CallbackPageName = "Callback.aspx";
    var HeaderId = hdnHeaderDSDId;
    var HeaderName = hdnHeaderDSDName;
    var HeaderPrepared = hdnHeaderPrepared;
    var DbDefaultLanguage = HeaderLanguageCode;
    var InputParam = DbNid;
    InputParam += ParamDelimiter + DbDefaultLanguage;
    InputParam += ParamDelimiter + IsDevInfoDatabase;
    InputParam += ParamDelimiter + HeaderId;
    InputParam += ParamDelimiter + HeaderName;
    InputParam += ParamDelimiter + z('Sender_Id_Val').value;
    InputParam += ParamDelimiter + z('Reciever_Id_Val').value;
    InputParam += ParamDelimiter + z('Sender_Name_Val').value;
    InputParam += ParamDelimiter + z('Sender_Contact_Name_Val').value;
    InputParam += ParamDelimiter + z('Sender_Contact_Dept_Val').value;
    InputParam += ParamDelimiter + z('Sender_Contact_Role_Val').value;
    InputParam += ParamDelimiter + z('Sender_Contact_Fax_Val').value;
    InputParam += ParamDelimiter + z('Sender_Contact_Phone_Val').value;
    InputParam += ParamDelimiter + z('Sender_Contact_Email_Val').value;

    InputParam += ParamDelimiter + z('Reciever_Name_Val').value
    InputParam += ParamDelimiter + z('Reciever_Contact_Name_Val').value;
    InputParam += ParamDelimiter + z('Reciever_Contact_Dept_Val').value;
    InputParam += ParamDelimiter + z('Reciever_Contact_Role_Val').value;
    InputParam += ParamDelimiter + z('Reciever_Contact_Fax_Val').value;
    InputParam += ParamDelimiter + z('Reciever_Contact_Phone_Val').value;
    InputParam += ParamDelimiter + z('Reciever_Contact_Email_Val').value;
    InputParam += ParamDelimiter + z('hLoggedInUserNId').value;
    InputParam += ParamDelimiter + HeaderPrepared;
    var HTMLStr = "";
    if (IsDevInfoDatabase) {
        CallbackPageName = "../Callback.aspx";
    }

    di_jq.ajax({
        type: "Get",
        url: CallbackPageName,
        data: { 'callback': '1046', 'param1': InputParam },
        async: false,
        beforeSend: function () {
            ApplyMaskingDiv();
        },
        success: function (data) {
            try {

                if (data == 'true') {
                    var SuccessMsg = "Header updated successfully. ";
                    if (z('langHeaderUpdateSuccess').value != null && z('langHeaderUpdateSuccess').value != '') {
                        SuccessMsg = z('langHeaderUpdateSuccess').innerHTML;
                    }
                    alert(SuccessMsg);
                    if (!IsDevInfoDatabase) {
                        HideHeaderUpdatePopup();
                    }
                }
                else {
                    var ErrorMsg = "Error occoured in updating header. ";
                    if (z('langHeaderUpdateFailure').value != null && z('langHeaderUpdateFailure').value != '') {
                        ErrorMsg = z('langHeaderUpdateFailure').value;
                    }
                    alert(ErrorMsg);
                }
            }
            catch (ex) {
                alert("Error : " + ex.message);
            }
        },
        error: function () {
        //    
        },
        complete: function () {
            RemoveMaskingDiv();
        },
        cache: false
    });
}
