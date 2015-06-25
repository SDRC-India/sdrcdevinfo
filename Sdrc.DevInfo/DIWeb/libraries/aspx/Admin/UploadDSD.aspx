<%@ Page Title="" Language="C#" MasterPageFile="~/libraries/aspx/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="UploadDSD.aspx.cs" Inherits="libraries_aspx_Admin_UploadDSD" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript" src="../../js/di.jquery-1.4.4.js"></script>
<script type="text/javascript" src="../../js/UploadDSD.js"></script>


<div id="reg_lft_sec1">
   <%-- <h1 id="langUploadDSD"><!--Upload Data Structure Definition(DSD) And Metadata Structure Definition(MSD)--></h1> --%>
    <h1 id="langUploadDSD"><!--Upload Data Structure Definition And Metadata Structure Definition--></h1> 
    <h4 id="langUpload_your_DSD_here"><!--Upload your DSD And MSD here--></h4> 
    <h2 id="lang_Upload_DSD_MSD_coln"><!--Upload Your DSD And MSD :--> </h2>
    <br />
    <div class="adm_nav_opt">
		 <p><input type="button" value="" id="btnAddNew" class="submit_button"  onclick="OpenUploadDSDPopup('A','');" /> </p>
	</div>  
    
    <h2 id="lang_List_Of_DSDs"><!--List Of DSDs :--></h2>   	
    <div class="confg_Adm_box" id="divListofDSDs">
    </div>
    <h2 id="lang_List_Of_MSDs"><!--List Of MSDs :--></h2>   	
    <div class="confg_Adm_box" id="divListofMSDs">
    </div>
    <!-- Popup for Upload DSD ...starts-->
    
    <div id="divUploadDSD" class="popup_block">
    <div class="confg_Adm_box">
    
    <div id="divAddUploadDSD">
        <form id="frmUploadDSDAddNew" name="frmUploadDSDAddNew" action="../UploadFile.aspx" enctype="multipart/form-data" method="post">
         <h2 id="lang_Upload_Your_DSD_MSD"><!--Upload Your DSD and MSD--></h2>
		<p class="confg_frm_txt">
        <input id="UplDSDFileFromAdmin" name="UplInputDSDFileFromAdmin" type="file" class="confg_frm_inp_bx_txt_reg di_gui_button" size="100" style="width:500px"/><br/><br/>
		<input type="submit" name="btnUploadDSDFromAdmin" id="btnUploadDSDFromAdmin" value="" class="di_gui_button" />
        </p>
        </form>
    </div>
    <div id="divEditDeleteUploadDSD">
        <form id="frmUpdateDSD" name="frmUpdateDSD" action="../UploadFile.aspx" enctype="multipart/form-data" method="post">
            <h2 id="lang_Update_Your_DSD_MSD"></h2>
            <div class="confg_Adm_box">
            <p class="confg_frm_txt">
            <select id="selDSD" class="confg_frm_inp_bx_txt_dd">
            <option value="0" id="lang_Select_DSD"><!--Select DSD--></option>
            </select><br/><br/>
            <input id="UpdateUplDSDFileFromAdmin" name="UpdateUplDSDFileFromAdmin" type="file" class="confg_frm_inp_bx_txt_reg di_gui_button" size="100" style="width:500px"/><br/><br/>
            <input type="submit" name="btnUpdateDSDFromAdmin" id="btnUpdateDSDFromAdmin" value="" class="di_gui_button" />
             <input type="button" name="btnDeleteDSDFromAdmin" id="btnDeleteDSDFromAdmin" value="" class="di_gui_button" onclick="DeleteArtefactsForUploadedDSD();" />
            </p>
            </div>
        </form>	
    </div>
    </div>
    </div>
     <!-- Popup for Upload DSD ...ends-->
     <!-- Popup for DSD Details ...starts-->
<div id="divArtefacts" class="popup_block">	
    
    <!-- Popup Inside Content ...starts-->
    <div id="reg_content_containers">
        <h2><label id="lblArtefactType"></label></h2>
        <div id="reg_wide_sec_ppup">
        
                <!-- Input Fields Area ...starts-->    
                <div id="confg_Adm_box_lgin" style="background-color:#ffffdd">
                    <pre id="preArtefacts" style="overflow:scroll;height:380px;width:680px;word-wrap:break-word; white-space:pre-wrap; padding:10px;"></pre>                    
                </div>
                <!-- Input Fields Area ...ends-->  
        </div>
    </div> 
    <!-- Popup Inside Content ...ends-->     
    
</div>      
<!-- Popup for DSD Details ...ends-->	
</div>

<script type="text/javascript" src="../../js/di.jquery.form.js"></script>

<script type="text/javascript">
    di_jq(document).ready(function () {

        di_jq('#frmUploadDSDAddNew').ajaxForm({
            success: function (response) {
                if (response == "No File") {
                    alert("Please browse a DSD file and then click on Upload DSD button.");
                }
                else {
                    CreateArtefactsForUploadedDSD(response);
                }
            }

        });

    });

    di_jq(document).ready(function () {
        di_jq('#frmUpdateDSD').ajaxForm({
            success: function (response) {
                if (response == "No File") {
                    if (ValidateUpdateDSD()) {
                        alert("Please browse a DSD file and then click on Update DSD button.");
                    }

                }
                else {

                    if (ValidateUpdateDSD()) {
                        UpdateArtefactsForUploadedDSD(response);
                    }

                }
            }

        });

    });

    SelectLeftMenuItem("UploadDSD");
    BindUploadedDSDsInTheTable();
    BindUploadedMSDsInTheTable();
</script>
</asp:Content>

