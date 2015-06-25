<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminLogin.aspx.cs" Inherits="libraries_aspx_Admin_Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../../stock/themes/default/css/diuilibcommon.css" rel="stylesheet" type="text/css" />    
    <link href="../../../stock/themes/default/css/style.css" rel="stylesheet" type="text/css" />   
<!--[if IE]>
        <link rel="stylesheet" type="text/css" href="../../../stock/themes/default/css/style-ie.css" />
<![endif]--> 

	<link href="../../../stock/themes/default/images/favicon.ico" rel="shortcut icon" type="image/x-icon" />

    <script type="text/javascript" src="../../js/di.jquery-1.4.4.js?v=<%=Global.js_version%>"></script>
    <script type="text/javascript" src="../../js/common.js?v=<%=Global.js_version%>"></script>    
    <script type="text/javascript" src="../../js/admin.js?v=<%=Global.js_version%>"></script>
    <script type="text/javascript" src="../../js/browsr_selector.js?v=<%=Global.js_version%>"></script>
</head>
<body>
    <!-- Header Area ...starts-->
    <div id="header">
        <div class="header">
            <!-- Logo Section ...starts-->
	        <div class="main_logo"><a href="../home.aspx" id="aHeader"><img src="../../../stock/themes/default/images/cust/logo.png" alt="" width="230" height="71"/></a></div>
	        <!-- Logo Section ...ends-->  	        
        </div>
    </div>
    <!-- Header Area ...ends-->
    <div class="clear"></div>


    <div class="content_containers">
    
<div class="clear navls_top_pg" style="margin-top:15px"></div>
 

		<!-- Registry Content Area ...starts-->
		<div id="reg_content_containers"><br />
            <h2 id="spanSignOrRegister"><!--Sign in OR Register--></h2>
			<h5 id="spanRequestLogin"><!--Please login below to continue--></h5> 
			
			
	        <!-- Reg Left Section ...starts-->
	        <div id="reg_lft_sec">
                <!-- Input Fields Area ...starts-->    
                <div id="confg_Adm_box_lgin">
                     
                    <p class="confg_frm_txt_lgin" id="spanEmail"><span id="lang_Email_Address"><!--E-Mail Address--></span> <span class="Mandatory" id="spanEmailMandatory">*</span></p>
                    <p class="confg_frm_inp_bx"><input tabindex="1" id="txtEmail" type="text" class="confg_frm_inp_bx_txt" /></p> 
                    <div class="clear"></div> 
                    
                    <p class="confg_frm_txt_lgin_qry"><input id="radioHavePassword" type="radio" name="Password" style="display:none;" onclick="HideShowRegisterForm();" /><span id="login_Password"><!--I already have a password--></span></p>  
                    <p class="confg_frm_inp_bx"><input tabindex="2" id="txtHavePassword" type="password" class="confg_frm_inp_bx_txt_qry" onkeydown="HandleEnter(event);" onkeyUp="KeyUp(event.keyCode);"/></p>     
                    <div class="clear"></div>     
                    
                    <p class="confg_frm_txt_lgin confg_frm_txt_lgin_qry_spc" style="display:none;"><input id="radioRegister" type="radio" name="Password" onclick="HideShowRegisterForm();"/><span id="spanRegister">I need to register</span></p>  
                    <div class="adm_upd_bttn_lginpg"><input tabindex="3" id="btnLogin" type="button" class="di_gui_button di_gui_button_qry" value="" onclick="ValidateCredential();"/></div>    
                    <div class="clear"></div>  
					
					<p>&nbsp;</p>
					
                    <div id="tblRegister" style="display:block;">
                        <p class="confg_frm_txt_lgin" id="spanFirstName"><span id="lang_First_Name"><!--First Name--></span> <span class="Mandatory" id="spanFirstNameMandatory">*</span></p>  
                        <p class="confg_frm_inp_bx"><input tabindex="4" id="txtFirstName" type="text" class="confg_frm_inp_bx_txt"/></p>     
                        <div class="clear"></div>    
						
                        <!--<p class="confg_frm_txt_lgin" id="spanLastName"><span id="lang_Last_Name">Last Name</span></p>  
                        <p class="confg_frm_inp_bx"><input tabindex="5" id="txtLastName" type="text" class="confg_frm_inp_bx_txt"/></p>    
                        <div class="clear" ></div>   -->
						
                        <p class="confg_frm_txt_lgin" id="spanCountry"><span id="lang_Country"><!--Country--></span><span class="Mandatory">*</span></p>  
                        <p class="confg_frm_inp_bx"><select id="ddlCountry" class="confg_frm_inp_bx_txt_dd"></select> <!--<input tabindex="6" id="txtCountry" type="text" class="confg_frm_inp_bx_txt"/>--></p>     
                        <div class="clear"></div> 
						
                        <p class="confg_frm_txt_lgin" id="spanPassword"><span id="lang_Password"><!--Password--></span> <span class="Mandatory" id="spanPasswordMandatory">* <br /><span id="lang_P_alphanumeric_password"><!--Provide an alphanumeric password--></span> <br /><span id="lang_A8C_Long"><!--atleast 8 characters long--></span></span></p>  
                        <p class="confg_frm_inp_bx"><input tabindex="7" id="txtPassword" type="password" onkeydown="HandleEnter(event);" onkeyUp="KeyUp(event.keyCode);" class="confg_frm_inp_bx_txt"/></p> 
                        <div class="clear"></div> 						  												

                        <p class="confg_frm_txt_lgin" id="spanConfirmPassword"><span id="lang_Confirm_Password"><!--Confirm Password--></span> <span class="Mandatory" id="spanConfirmPasswordMandatory">*</span></p>  
                        <p class="confg_frm_inp_bx"><input tabindex="8" id="txtConfirmPassword" type="password" onkeydown="HandleEnter(event);" onkeyUp="KeyUp(event.keyCode);" class="confg_frm_inp_bx_txt"/></p>  
                        <div class="clear"></div>
						
                        <p class="confg_frm_txt_lgin" style="display:none;" id="spanRegisterProvider"><!--Register me as data provider--></p>  
                        <p class="confg_frm_inp_bx" style="display:none;"><input tabindex="9" id="chkRegisterProvider" type="checkbox"/></p>  
                        <div class="clear"></div>	
						
    <!-- Configuration Update Button ...starts-->
    <div class="adm_upd_bttn_lginpg"><input tabindex="10" id="btnSignup" type="button" class="di_gui_button" value="" onclick="SignUp(true);" style="display:block;"/><input id="btnUpdate" type="button" tabindex="10" class="di_gui_button" value="" onclick="Update();" style="display:none;"/></div>
    <!-- Configuration Update Button ...ends-->   											

                    </div>                                            
                    
                    <div class="clear"></div>
                    <p class="frgtn_txt_link" style="float:left;"><a id="aForgotPassword" href="javascript:ForgotPassword();" style="display:none;"><!--I've forgotten my password--></a></p>
                </div>     
                <!-- Input Fields Area ...ends-->                                          	    
	        </div>
	        <!-- Reg Left Section ...ends-->
	        
		    <!-- Reg Right Section ...starts-->
		    <div id="reg_rgt_sec" style="display:none;">
				<!-- Login Confirm Email Text Box ...starts-->
				<div class="reg_lgn_confrm_txt_box">
				<b id="lang_Note"><!--Note:--></b> <span id="lang_Notification_ESI_S_Update"><!--A notification email will be sent to your inbox on signup/update.--></span>
				</div>
				<!-- Login Confirm Email Text Box ...ends-->	
		    </div>
		    <!-- Reg Right Section ...ends-->		        			
			   
		</div>
		<!-- Registry Content Area ...ends-->	    
        
    <!-- Main Container ...ends-->
    </div>
    <input id="hlngcode" type="hidden" value="en" name="hlngcode"/>       
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
<!-- End Hidden Variables for language-->
    <script type="text/javascript">
        di_jq(document).ready(function () {
            SetAdaptationModeFromAdmin();

            CookiePostfixStr = '_' + '<%=Global.CookiePostfixStr%>';
            LoadLanguage(readCookie("hlngcode"));

            onLanguageSelection(true);

            z('hlngcode').value = '<%=hlngcode%>';
        });
    </script>
    <script type="text/javascript">
        AppVersion = '<%=Global.js_version%>';
        onPageLoad('<%=hlngcode%>', '<%=hLoggedInUserNId%>');
    </script>
    <script type="text/javascript">
        try {
           // window.onload = HideShowLoginRegisterForm();
        }
        catch (err) {
        }
        
    </script>
</body>
</html>
