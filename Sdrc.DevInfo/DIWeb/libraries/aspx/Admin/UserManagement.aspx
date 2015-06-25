<%@ Page Title="" Language="C#" MasterPageFile="~/libraries/aspx/Admin/AdminMaster.master"
    AutoEventWireup="true" CodeFile="UserManagement.aspx.cs" Inherits="libraries_aspx_Admin_UserManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .Pagination
        {
            padding: 3px;
            margin: 3px;
            text-align: right;
        }
        
        .a
        {
            padding: 2px 5px 2px 5px;
            margin-right: 2px;
            border: 1px solid #ddd;
            margin-bottom: 5px;
            display: inline-block;
            text-decoration: none;
            color: #aaa;
        }
        .a:hover, a:active
        {
            padding: 2px 5px 2px 5px;
            margin-right: 2px;
            border: 1px solid #a0a0a0;
        }
        .paginationCurrent
        {
            padding: 2px 5px 2px 5px;
            margin-right: 2px;
            border: 1px solid #e0e0e0;
            font-weight: bold;
            background-color: #f0f0f0;
            color: #aaa;
        }
        .news_subc_container
        {
            background: url("../../../stock/themes/default/images/User_srch_bg.png") no-repeat scroll 0 0 transparent;
        }
        
        .user_srch_container
        {
            background: none repeat scroll 0 0 #FFFFFF;
            border: 1px solid #CCCCCC;
            -webkit-border-radius: 5px;
            -moz-border-radius: 5px;
            border-radius: 5px;
            box-shadow: 1px 1px 4px;
            height: 21px;
            width: 220px;
            -moz-box-shadow: inset 0 0 5px #ccc;
            -webkit-box-shadow: inset 0 0 5px #ccc;
            box-shadow: inset 0 0 5px #ccc;
        }
        
        #kwd_search
        {
            width: 220px;
            margin: -1px 0px 5px 5px;
            border-color: #FFFFFF;
            border-style: none;
            color: #999999;
            font-family: verdana;
            font-size: 10px;
            line-height: 10px;
            cursor: pointer;
            height: 20px;
        }
        .srch_btn_bg
        {
            background: url("../../../stock/themes/default/images/btn_cross_srch.png") no-repeat scroll 197px -15px transparent;
        }
        .srch_cancle_bg
        {
            background: url("../../../stock/themes/default/images/btn_cross_srch.png") no-repeat scroll 197px 5px transparent;
        }
    </style>
    <h1 id="lang_AdaptationUser_Settings">
        <%--User Settings--%></h1>
    <div>
        <h4 id="lang_Add_AdapMUDH">
        </h4>
    </div>
    <div class="user_srch_container " id="div_search" style="float: right; position: relative;
        top: -30px">
        <input type="text" id="kwd_search" class="srch_btn_bg" name="EMAIL" value="Enter username/email to search">
    </div>
    <h2 id="langAvailableUsers">
        Available Users</h2>
    <!--- This section was previously used for showing drop downlist for adaptations list  starts---!>
<%--<div class="adm_nav_opt">
<p id="lang_ChooseAdap" class="flt_lft"></p>
<select id="ddlAdaptations" onchange="AdaptationChange(this);">
</select>
</div>--%>

<%--<div class="frm_spc">
    <p class="frm_txt flt_lft" id="lang_ChooseAdap"></p>
    <select id="ddlAdaptations" onchange="AdaptationChange(this);" class="confg_frm_inp_bx_txt_sml">
    </select>
</div>--%>
<!--- This section was previously used for showing drop downlist for adaptations list  end---!>

<!--<br /><br />-->
    <div id="divUserPaging" class="dsp_inln flt_rgt">
    </div>
    <!--<br /><br />-->
    <!-- Nav Option ...starts-->
    <div class="adm_nav_opt">
        <ul>
            <%--<li><a href="ConnectionDetails.aspx" id="langNew"><!--New--></a></li>--%>
            <li><a href="javascript:AddNewAdminPopUp();" id="lang_CreateAdminAccount"></a></li>
            <li id="pipeSetAsAdmin">|</li>
            <li><a href="JavaScript:SetAsAdminPopUp();" id="lang_SetAsAdmin" style="display: none">
            </a></li>
            <li id="pipeChangePassword">|</li>
            <li><a href="JavaScript:ChangePasswordPopup(true);" id="lang_ChangePassword" style="display: none">
                <%--Change Password--%></a></li>
        </ul>
    </div>
    <!-- Nav Option ...ends-->
    <div id="divAdapUsersList" style="min-height: 390px;" runat="server" class="DivUsersGrid">
    </div>
    <!-- Change Password Pop-Up container for all -->
    <div id="ChangePasswodPopup" class="popup_block" style="height: 250px; width: 550px;">
        <!-- Reg Left Section ...starts-->
        <div id="CPDiv1">
            <div id="CPDiv2">
                <p class="confg_frm_txt_lgin">
                    <span id="NewPwdSpanCP">
                        <%--New Password--%></span> <span class="Mandatory">*</span></p>
                <p class="confg_frm_inp_bx">
                    <input tabindex="50" id="txtNewPwdCP" type="password" class="confg_frm_inp_bx_txt_qry"
                        onkeydown="HandleEnter(event);" onkeyup="KeyUp(event.keyCode);" /></p>
                <div class="clear">
                </div>
                <p class="confg_frm_txt_lgin">
                    <span id="ConfirmPwdSpanCP">
                        <%--Confirm Password--%></span> <span class="Mandatory">*</span></p>
                <p class="confg_frm_inp_bx">
                    <input tabindex="51" id="txtConfirmPwdCP" type="password" class="confg_frm_inp_bx_txt_qry"
                        onkeydown="HandleEnter(event);" onkeyup="KeyUp(event.keyCode);" /></p>
                <div class="clear">
                </div>
                <!-- Configuration Update Button ...starts-->
                <div class="adm_upd_bttn_lginpg">
                    <input tabindex="52" id="btnPwdChange" type="button" class="di_gui_button di_gui_button_qry"
                        value="" onclick="ChangeAdapUserPassword()" /></div>
                <!-- Configuration Update Button ...ends-->
            </div>
        </div>
    </div>
    <!-- Start Hidden Variables for language-->
    <span id="lngEnterPwdMsg" style="display: none;">
        <!--Enter password-->
    </span><span id="lngPwdLenMsg" style="display: none;">
        <!--Password length should be 8 characters long.-->
    </span><span id="lngConfirmPwdMsg" style="display: none;">
        <!--Confirm Password-->
    </span><span id="lngReenterPwdMsg" style="display: none;">
        <!--Re Enter Password and confirm password-->
    </span>
    <!-- End Hidden Variables for language-->
    <script type="text/javascript">
        di_jq(document).ready(function () {
            //  GetAllAdaptations();
            // Write on keyup event of keyword input element
            di_jq("#kwd_search").keyup(function () {
                // When value of the input is not blank
                if (di_jq(this).val() != "" && di_jq(this).val() != "Enter username/email to search") {
                    GetCurrentAdaptationUsers(di_jq(this).val());
                    di_jq(this).attr("class", "srch_cancle_bg");
                    // show cross img
                }
                else {
                    // When there is no input or clean again, show everything back
                    GetCurrentAdaptationUsers();
                    //remove cross
                }

            });

            di_jq('#kwd_search').focus(function () {
                if (di_jq(this).val() == "Enter username/email to search") {
                    di_jq(this).val("");
                }
            });
            di_jq('#kwd_search').blur(function () {
                if (di_jq(this).val() == "") {
                    di_jq(this).val("Enter username/email to search");
                    di_jq(this).attr("class", "srch_btn_bg");
                }
            });

            di_jq("#kwd_search").click(function () {
                var CurrentClass = di_jq(this).attr("class");
                if (CurrentClass.indexOf("srch_cancle_bg") != -1) {
                    GetCurrentAdaptationUsers();
                    di_jq(this).attr("class", "srch_btn_bg");
                    di_jq(this).val("");
                }
            });

            GetCurrentAdaptationUsers();
        });
    </script>
</asp:Content>
<%--<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>--%>
