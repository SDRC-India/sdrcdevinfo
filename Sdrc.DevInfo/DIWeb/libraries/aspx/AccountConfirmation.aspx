<%@ Page Title="" Language="C#" MasterPageFile="~/libraries/aspx/HomeMaster.master" AutoEventWireup="true" CodeFile="AccountConfirmation.aspx.cs" Inherits="libraries_aspx_AccountConfirmation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
<!-- DEVELOPER CODE -->

<div class="content_containers">
    
		<!-- Registry Content Area ...starts-->
		<div id="reg_content_containers">
            <h2 id="resultTag"></h2>
			<h5> <a id="proceedInfoAtag">Click here.</a></h5> 

	        <!-- Reg Left Section ...starts-->
	        <div id="reg_lft_sec">

                <!-- Input Fields Area ...starts-->    
                <div id="confg_Adm_box_lgin">
                    <p class="confg_frm_txt_lgin pddn_tp_fftn"><span id="NewPwdSpan"><!--New Password--></span> <span class="Mandatory">*</span></p>
                    <p class="confg_frm_inp_bx"><input tabindex="1" id="txtNewPwd" type="password" class="confg_frm_inp_bx_txt_qry" onkeydown="HandleEnter(event);" onkeyUp="KeyUp(event.keyCode);"/></p> 
                    <div class="clear"></div> 

                    <p class="confg_frm_txt_lgin pddn_tp_fftn"><span id="ConfirmPwdSpan"><!--Confirm Password--></span> <span class="Mandatory">*</span></p>
                    <p class="confg_frm_inp_bx"><input tabindex="2" id="txtConfirmPwd" type="password" class="confg_frm_inp_bx_txt_qry" onkeydown="HandleEnter(event);" onkeyUp="KeyUp(event.keyCode);"/></p> 
                    <div class="clear"></div> 
 
    <!-- Configuration Update Button ...starts-->
    <div class="adm_upd_bttn_lginpg"><input tabindex="4" style="display:none" id="btnSkip" type="button" class="di_gui_button di_gui_button_qry" value="Skip" onclick="SkipChangePassword()"/><input tabindex="3" id="btnLogin" type="button" class="di_gui_button di_gui_button_qry" value="" onclick="ChangePassword()"/></div>
    <!-- Configuration Update Button ...ends-->   											

                    </div>                                            
		
                    
                    <div class="clear"></div>
                    
                </div>     
                <!-- Input Fields Area ...ends-->                                          	    

            </div>
	        <!-- Reg Left Section ...ends-->
	         <p class="frgtn_txt_link" style="float:left;padding-left:30px;"><a id="regenerateLink" href="javascript:RegenerateForgotPasswordLink();" style="display:none">Regenerate the forgot password link</a></p>							
			   
		</div>
		<!-- Registry Content Area ...ends-->	    
        
    <!-- Main Container ...ends-->

<!-- Start Hidden Variables for language-->
<span id="lngSelectCountryMsg" style="display:none;"><!--Please select a country--></span>
<span id="lngIDPasswordMsg" style="display:none;"><!--Enter email id and password--></span>
<span id="lngAccountNotActivatedMsg" style="display:none;"><!--A/c not activated yet--></span>
<span id="lngSuccessfullyLoginMsg" style="display:none;"><!--Successfully Login!--></span>
<span id="lngRegenerateLinkMsg" style="display:none;"><!--Regenerate activation link!--></span>

<span id="lngInvalidCredentialsMsg" style="display:none;"><!--Invalid Credentials!--></span>
<span id="lngInternalErrorMsg" style="display:none;"><!--Internal Error!--></span>
<span id="lngPlzVerifyMsg" style="display:none;"><!--Verify mail--></span>
<span id="lngDetailsUpdatedMsg" style="display:none;"><!--Details Updated--></span>
<span id="lngEnterIdMsg" style="display:none;"><!--Enter email id--></span>
<span id="lngIdMandatoryMsg" style="display:none;"><!--Email id Mandatory--></span>
<span id="lngIdFormatMsg" style="display:none;"><!--Email id Format--></span>
<span id="lngNameMandatoryMsg" style="display:none;"><!--Name Mandatory--></span>
<span id="lngEnterPwdMsg" style="display:none;"><!--Enter password--></span>
<span id="lngPwdLenMsg" style="display:none;"><!--Password length should be 8 characters long.--></span>
<span id="lngConfirmPwdMsg" style="display:none;"><!--Confirm Password--></span>
<span id="lngReenterPwdMsg" style="display:none;"><!--Re Enter Password and confirm password--></span>
<span id="lngOldPwdWrongMsg" style="display:none;"><!--Old Password is wrong--></span>
<span id="lngPwdChangedMsg" style="display:none;"><!--Password has been changed--></span>
<span id="lngAccountNotExistMsg" style="display:none;"><!--Account doesn't exist!--></span>
<span id="lngAccountActivationMsg" style="display:none;"><!--Congrats A/c has been activated--></span>
<span id="lngTimeExpireMsg" style="display:none;"><!--Time Expired--></span>
<span id="lngTokenExpireMsg" style="display:none;"><!--Token Expired--></span>
<span id="lngIdNotRegisterMsg" style="display:none;"><!--ID not registered--></span>
<!-- End Hidden Variables for language-->

<input type="hidden" id="unihidden" />
    <script type="text/javascript">
        CookiePostfixStr = '_' + '<%=Global.CookiePostfixStr%>';    // use in hosting app

        var di_components = "Language,Database";
        var di_component_version = '<%=Global.diuilib_version%>';
        var di_theme_css = '<%=Global.diuilib_theme_css%>';
        var di_diuilib_url = '<%=Global.diuilib_url%>';
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibincludes.js'" + "><\/script>");
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibcommon.js'" + "><\/script>");
    </script>
    <script language="javascript" type="text/javascript" src="../js/di.jquery-1.4.4.js"></script>
    <script type="text/javascript" src="../js/Login.js?v=<%=Global.js_version%>"></script>

    <script type="text/javascript">        onActivationPageLoad('<%=hdsby%>', '<%=hdbnid%>', '<%=hselarea%>', '<%=hselind%>', '<%=hlngcode%>', '<%=hlngcodedb%>', '<%=hselindo%>', '<%=hselareao%>', '<%=hLoggedInUserNId%>', '<%=hLoggedInUserName%>', '<%=hOriginaldbnid%>', '<%=hdvnids%>');</script>
    <script type="text/javascript">
        window.onload = function () {
            try {
                AccountActivationProcess();

            }
            catch (err) { }
        }
    </script>
    <!-- END OF DEVELOPER CODE -->
</asp:Content>

