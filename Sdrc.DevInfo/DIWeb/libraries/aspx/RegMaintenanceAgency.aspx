<%@ Page Title="" Language="C#" MasterPageFile="~/libraries/aspx/RegistryMaster.master"
    AutoEventWireup="true" CodeFile="RegMaintenanceAgency.aspx.cs" Inherits="libraries_aspx_RegMaintenanceAgency" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphRegistryMainContent" runat="Server">
    <script type="text/javascript" src="../js/di.jquery-1.4.4.js?v=<%=Global.js_version%>"></script>
    <script type="text/javascript" src="../js/sdmx/MaintenanceAgency.js?v=<%=Global.js_version%>"></script>
    <!-- Reg Left Section ...starts-->
    <div id="reg_lft_sec" style="width: 100%">
        <h3 id="lang_List_Of_Databases">
            <%--List Of Databases :--%></h3>
        <div class="confg_Adm_box" id="divListofDatabases">
        </div>
        <div>
             <input type="button" value="" id="btnGenerateSDMXArtefacts" class="submit_button" onclick="GenerateSDMXArtefacts();" />
        </div>
        <br />
        <br />
        <h3 id="lang_Upload_DSD_MSD_coln">
            <%--Upload Your DSD And MSD :--%></h3>
        <br />
        <div>
            <input type="button" value="" id="btnAddNew" class="submit_button" onclick="OpenUploadDSDPopup('A','','');" />
        </div>
        <br />
        <h4 id="lang_List_Of_DSDs" style="color:#404141;font-weight:bold">
            <%--List Of Uploaded DSDs :--%></h4>
        <div class="confg_Adm_box" id="divListofUploadedDSDs">
        </div>
        <h4 id="lang_List_Of_MSDs" style="color:#404141;font-weight:bold">
           <%-- List Of Uploaded MSDs :--%></h4>
        <div class="confg_Adm_box" id="divListofUploadedMSDs">
        </div>
        <!-- Popup for Upload DSD ...starts-->
        <div id="divUploadDSD" class="popup_block" style="height:280px">
            <div class="confg_Adm_box">
                <div id="divAddUploadDSD">
                    <form id="frmUploadDSDAddNew" name="frmUploadDSDAddNew" action="UploadFile.aspx"
                    enctype="multipart/form-data" method="post">
                    <h3 id="lang_Upload_Your_DSD_MSD">
                        <%--Upload Your DSD and MSD--%></h3>
                    <br />
                    <p class="confg_frm_txt" id="lang_Assosciated_Database">
                       <%-- Assosciated Database :--%></p>
                    <p class="confg_frm_inp_bx">
                        <select id="selAssosciatedDatabase" class="confg_frm_inp_bx_txt_dd">
                            <option value="0" id="lang_Select_Assosciated_Database"><%--Select Assosciated Database--%></option>
                        </select><br />
                        <br />
                    </p>
                    <p class="confg_frm_txt">
                        <input id="UplDSDFileFromAdmin" name="UplInputDSDFileFromAdmin" type="file" class="confg_frm_inp_bx_txt_reg di_gui_button"
                            size="75" style="width: 500px" /><br />
                        <br />
                        <span id="lang_Default_DSD_Upload"><%--Default DSD:--%></span><input id="chkDefaultDSDUpload" type="checkbox"/>
                        <br />
                        <br />
                        <input type="submit" name="btnUploadDSDFromAdmin" id="btnUploadDSDFromAdmin" value=""
                            class="di_gui_button" />
                    </p>
                    </form>
                </div>
                <div id="divEditDeleteUploadDSD">
                    <form id="frmUpdateDSD" name="frmUpdateDSD" action="UploadFile.aspx" enctype="multipart/form-data"
                    method="post">
                    <h3 id="lang_Update_Your_DSD_MSD">
                    </h3>
                    <div class="confg_Adm_box">
                        <p class="confg_frm_txt" id="lang_Assosciated_Database_Edit_Delete">
                            <%--Assosciated Database :--%></p>
                        <p class="confg_frm_inp_bx">
                            <select id="selAssosciatedDatabaseEditDelete" class="confg_frm_inp_bx_txt_dd">
                                <option value="0" id="lang_Select_Assosciated_Database_Edit_Delete"><%--Select Assosciated Database--%></option>
                            </select>
                            <br />
                        </p>
                        <p class="confg_frm_txt" id="lang_Selected_DSD">
                           <%-- Selected DSD :--%></p>
                        <p class="confg_frm_inp_bx">
                            <select id="selDSD" class="confg_frm_inp_bx_txt_dd">
                                <option value="0" id="lang_Select_DSD"><%--Select DSD--%></option>
                            </select>
                            <br />
                        </p>
                        <p class="confg_frm_txt">
                            <input id="UpdateUplDSDFileFromAdmin" name="UpdateUplDSDFileFromAdmin" type="file"
                                class="confg_frm_inp_bx_txt_reg di_gui_button" size="75" style="width: 500px" />
                            <br />
                            <br />
                            <span id="lang_Default_DSD_Update"><%--Default DSD:--%></span><input id="chkDefaultDSDUpdate" type="checkbox"/>
                            <br />
                            <br />
                            <input type="submit" name="btnUpdateDSDFromAdmin" id="btnUpdateDSDFromAdmin" value=""
                                class="di_gui_button" />
                            <input type="button" name="btnDeleteDSDFromAdmin" id="btnDeleteDSDFromAdmin" value=""
                                class="di_gui_button" onclick="DeleteArtefactsForUploadedDSD();" />
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
                <h2>
                    <label id="lblArtefactType">
                    </label>
                </h2>
                <div id="reg_wide_sec_ppup">
                    <!-- Input Fields Area ...starts-->
                    <div id="confg_Adm_box_lgin" style="background-color: #ffffdd">
                        <pre id="preArtefacts" style="overflow: scroll; height: 380px; width: 680px; word-wrap: break-word;
                            white-space: pre-wrap; padding: 10px;"></pre>
                    </div>
                    <!-- Input Fields Area ...ends-->
                </div>
            </div>
            <!-- Popup Inside Content ...ends-->
        </div>
        <!-- Popup for DSD Details ...ends-->
    </div>
    <!-- Reg Left Section ...ends-->
    <input type="hidden" id="hConnectionName" />
    <input type="hidden" id="hDatabaseType" />
    <input type="hidden" id="hCreatedOn" />
    <input type="hidden" id="hPublisher" />
    <input type="hidden" id="hSNo" />
    <input type="hidden" id="hId" />
    <input type="hidden" id="hAgencyId" />
    <input type="hidden" id="hVersion" />
    <input type="hidden" id="hAssosciatedDatabase" />
    <input type="hidden" id="hDetails" />
    <input type="hidden" id="hAction" />
    <input type="hidden" id="hAssosciatedDSD" />
    <input type="hidden" id="hView" />
    <input type="hidden" id="hDownload" />
    <input type="hidden" id="hEdit" />
    <input type="hidden" id="hDelete" />
    <!-- START DEVELOPER CODE -->
    <script type="text/javascript">
        CookiePostfixStr = '_' + '<%=Global.CookiePostfixStr%>';    // use in hosting app

        var di_components = "Language";
        var di_component_version = '<%=Global.diuilib_version%>';
        var di_theme_css = '<%=Global.diuilib_theme_css%>';
        var di_diuilib_url = '<%=Global.diuilib_url%>';
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibincludes.js'" + "><\/script>");
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibcommon.js'" + "><\/script>");
        var hIsUploadedDSD = '<%= hIsUploadedDSD %>'; 
    </script>

    <script type="text/javascript">
        onPageLoad('<%=hdsby%>', '<%=hdbnid%>', '<%=hselarea%>', '<%=hselind%>', '<%=hlngcode%>', '<%=hlngcodedb%>', '<%=hselindo%>', '<%=hselareao%>', '<%=hLoggedInUserNId%>', '<%=hLoggedInUserName%>', '<%=hOriginaldbnid %>');
    </script>

    <script type="text/javascript" src="../js/di.jquery.form.js?v=<%=Global.js_version%>"></script>
   

    <script type="text/javascript">
        di_jq(document).ready(function () 
        {
            di_jq('#frmUploadDSDAddNew').ajaxForm({
                success: function (response) {
                    if (response == "No File") {
                        alert("Please browse a DSD file and then click on Upload DSD button.");
                    }
                    else {
                        if (ValidateUploadDSD('A', response)) {
                            CreateArtefactsForUploadedDSD(response);
                        }
                    }
                }

            });

        });

        di_jq(document).ready(function () {
            di_jq('#frmUpdateDSD').ajaxForm({
                success: function (response) {
                    if (response == "No File") {
                        alert("Please browse a DSD file and then click on Update DSD button.");
                    }
                    else {

                        if (ValidateUploadDSD('U', response)) {
                            UpdateArtefactsForUploadedDSD(response);
                        }

                    }
                }

            });

        });

        di_jq(document).ready(function () {
            BindDatabasesInTheTable();
            BindUploadedDSDsInTheTable();
            BindUploadedMSDsInTheTable();
        });
    </script>
    <!-- END OF DEVELOPER CODE -->
</asp:Content>
