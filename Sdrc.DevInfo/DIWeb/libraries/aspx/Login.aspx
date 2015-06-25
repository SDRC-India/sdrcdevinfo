<%@ Page Language="C#" MasterPageFile="~/libraries/aspx/HomeMaster.master" EnableSessionState="true"
    CodeFile="Login.aspx.cs" Inherits="libraries_aspx_Login" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="Server">
    <!-- Main Container ...starts-->
    <div class="content_containers">
    
		<!-- Registry Content Area ...starts-->
		<div id="reg_content_containers"><br />
            <h2 id="spanSignOrRegister"><!--Sign in OR Register--></h2>
			<h5 id="spanRequestLogin"><!--Please login below to continue--></h5> 
            <h2 id="spanUpdateDetails" style = "display:none"><!--Update User Details--></h2>
			<h5 id="spanUpdateUserDetails" style = "display:none"><!--Please update your details.--></h5> 
			
            <!--<div class="desc_pg_main_sec">
            <p>Sign-up today and join DevInfo’s global user community by accessing innovations and creating your own visualizations.</p>
            </div>-->

	        <!-- Reg Left Section ...starts-->
	        <div id="reg_lft_sec">
                <!-- Input Fields Area ...starts-->    
                <div id="confg_Adm_box_lgin">
                    <p class="confg_frm_txt_lgin" id="spanEmail"><span id="lang_Email_Address"><!--E-Mail Address--></span> <span class="Mandatory" id="spanEmailMandatory">*</span></p>
                    <p class="confg_frm_inp_bx"><input tabindex="1" id="txtEmail" type="text" class="confg_frm_inp_bx_txt" /></p> 
                    <div class="clear"></div> 
                    
                    <p class="confg_frm_txt_lgin_qry"><input id="radioHavePassword" type="radio" name="Password" onclick="HideShowRegisterForm();" /><span id="spanHavePassword" ><!--I already have a password--></span></p>  
                    <p class="confg_frm_inp_bx"><input tabindex="2" id="txtHavePassword" type="password" class="confg_frm_inp_bx_txt_qry" onkeydown="HandleEnter(event); " onkeyUp="KeyUp(event.keyCode);"/></p>     
                    <div class="clear"></div>     
                    
                    <p class="confg_frm_txt_lgin confg_frm_txt_lgin_qry_spc"><input id="radioRegister" type="radio" name="Password" onclick="HideShowRegisterForm();"/><span id="spanRegister"><!--I need to register--></span></p>  
                    <p class="confg_frm_inp_bx"><input tabindex="3" id="btnLogin" type="button" class="di_gui_button di_gui_button_qry" value="" onclick="Login();"/></p>    
                    <div class="clear"></div>  
					
					<p>&nbsp;</p>
					
                    <div id="tblRegister" style="display:none;">
                        <p class="confg_frm_txt_lgin" id="spanFirstName"><span id="lang_First_Name"><!--First Name--></span> <span class="Mandatory" id="spanFirstNameMandatory">*</span></p>  
                        <p class="confg_frm_inp_bx"><input tabindex="4" id="txtFirstName" type="text" class="confg_frm_inp_bx_txt"/></p>     
                        <div class="clear"></div>    
						
                        <%--<p class="confg_frm_txt_lgin" id="spanLastName"><span id="lang_Last_Name"><!--Last Name--></span><span class="Mandatory">*</span></p>  
                        <p class="confg_frm_inp_bx"><input tabindex="5" id="txtLastName" type="text" class="confg_frm_inp_bx_txt"/></p>     
                        <div class="clear"></div>   --%>
						
                        <p class="confg_frm_txt_lgin" id="spanCountry"><span id="lang_Country"><!--Country--></span><span class="Mandatory">*</span></p>  
                        <p class="confg_frm_inp_bx"><select tabindex="5" id="ddlCountry" class="confg_frm_inp_bx_txt_dd"></select></p>     
                        <div class="clear"></div> 
						
                        <p class="confg_frm_txt_lgin" id="spanPassword"><span id="lang_Password"><!--Password--></span> <span class="Mandatory" id="spanPasswordMandatory">* <br /><span id="lang_P_alphanumeric_password"><!--Provide an alphanumeric password--></span> <br /><span id="lang_A8C_Long"><!--at least 8 characters long--></span></span></p>  
                        <p class="confg_frm_inp_bx" id="txtboxPwd"><input tabindex="6" id="txtPassword" type="password" onkeydown="" onkeyUp="KeyUp(event.keyCode);" class="confg_frm_inp_bx_txt"/></p> 
                        <div class="clear"></div> 						  												

                        <p class="confg_frm_txt_lgin" id="spanConfirmPassword"><span id="lang_Confirm_Password"><!--Confirm Password--></span> <span class="Mandatory" id="spanConfirmPasswordMandatory">*</span></p>  
                        <p class="confg_frm_inp_bx" id="txtboxConfirmPwd"><input tabindex="7" id="txtConfirmPassword" type="password" onkeydown="" onkeyUp="KeyUp(event.keyCode);" class="confg_frm_inp_bx_txt"/></p>  
                        <div class="clear"></div>
						
                        <p class="confg_frm_txt_lgin" id="spanRegisterProvider"><!--Request data provider right--></p>
                        <p class="confg_frm_inp_bx"><input id="chkRegisterProvider" tabindex="8" type="checkbox" style="float:left"/>
                        <img id="imghelpRegisterProvider" src="../../stock/themes/default/images/help.gif" alt="Help" 
                         onclick="ToggleCallout('divCallout', event, 'divHelpRegisterProvider');" style="vertical-align:bottom; cursor:pointer;"
                         onmouseout="HideCallout('divCallout')";/> 
                         </p> 
                        
                        <div class="clear"></div>
                        
                        <p class="confg_frm_txt_lgin" id="spanSendDevInfoUpdates"><!--Send me DevInfo updates--></p>
                        <p class="confg_frm_inp_bx"><input id="chkSendDevInfoUpdates" tabindex="9" type="checkbox"/></p>  
                        <div class="clear"></div>	
						
    <!-- Configuration Update Button ...starts-->
    <div class="adm_upd_bttn_lginpg"><input tabindex="10" id="btnSignup" type="button" class="di_gui_button" value="" onclick="SignUp();" style="display:none;"/><input id="btnUpdate" type="button" tabindex="10" class="di_gui_button" value="" onclick="Update();" style="display:none;"/>
    <a id="changePwdATag" href="javascript:void(0)" onclick="ShowHidePasswordPopup(true)" >Change password</a>
    </div>
    <!-- Configuration Update Button ...ends-->   											

                    </div>                                            
                    
                    <div class="clear"></div>
                    <p class="frgtn_txt_link" style="float:left;"><a id="aForgotPassword" href="javascript:ForgotPassword();"><!--I've forgotten my password--></a></p>&nbsp;&nbsp;&nbsp;&nbsp;
                    <p class="frgtn_txt_link" style="float:left;padding-left:30px;"><a id="regenerateLink" href="javascript:ForgotPassword();">Regenerate the activation link</a></p>
                </div>     
                <!-- Input Fields Area ...ends-->                                          	    
	        </div>
	        <!-- Reg Left Section ...ends-->
	        
		    <!-- Reg Right Section ...starts-->
		    <div id="reg_rgt_sec">
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
    
    <!-- Popup For Help starts -->
    <div id="divCallout" class="callout border-callout" style="z-index:2400">
        <div id="divCalloutText" class="content">
            <!-- Popup For Help Text starts -->
            <div id="divHelpRegisterProvider" style="display:none">
              <i class="content" id="i_Help_Register_Provider"></i>
            </div>
            <!-- Popup For Help Text ends -->
        </div>
        <b class="border-notch notch"></b><b class="notch"></b>
    </div>
<!-- Popup For Help ...ends -->	
<!-- Change Password Pop-Up container for all -->
<div id="PasswodPopup" class="popup_block" style="height:250px;width:550px;">

<!-- Reg Left Section ...starts-->
	        <div id="Div1">

                <!-- Input Fields Area ...starts-->    
                <div id="Div2">
                    <p class="confg_frm_txt_lgin pddn_tp_fftn"><span id="OldPwdSpan">Old Password</span> <span class="Mandatory">*</span></p>
                    <p class="confg_frm_inp_bx"><input tabindex="0" id="txtOldPwd" type="password" class="confg_frm_inp_bx_txt_qry" onkeydown="HandleEnter(event);" onkeyUp="KeyUp(event.keyCode);"/></p> 
                    <div class="clear"></div> 

                    <p class="confg_frm_txt_lgin pddn_tp_fftn"><span id="NewPwdSpan">New Password</span> <span class="Mandatory">*</span></p>
                    <p class="confg_frm_inp_bx"><input tabindex="0" id="txtNewPwd" type="password" class="confg_frm_inp_bx_txt_qry" onkeydown="HandleEnter(event);" onkeyUp="KeyUp(event.keyCode);"/></p> 
                    <div class="clear"></div> 

                    <p class="confg_frm_txt_lgin pddn_tp_fftn"><span id="ConfirmPwdSpan">Confirm Password</span> <span class="Mandatory">*</span></p>
                    <p class="confg_frm_inp_bx"><input tabindex="0" id="txtConfirmPwd" type="password" class="confg_frm_inp_bx_txt_qry" onkeydown="HandleEnter(event);" onkeyUp="KeyUp(event.keyCode);"/></p> 
                    <div class="clear"></div> 
 
						
    <!-- Configuration Update Button ...starts-->
    <div class="adm_upd_bttn_lginpg"><input tabindex="2" id="btnPwdUpdate" type="button" class="di_gui_button di_gui_button_qry" value="" onclick="ChangePassword()"/></div>



    <!-- Configuration Update Button ...ends-->   											

                    </div>                                            
                    <!-- Input Fields Area ...ends-->
                    <div class="clear"></div>
                  
                </div>     
                <!-- Reg Left Section ...ends-->	        						   	
	
</div>
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
    <!-- DEVELOPER CODE -->

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

    <script type="text/javascript">        onPageLoad('<%=hdsby%>', '<%=hdbnid%>', '<%=hselarea%>', '<%=hselind%>', '<%=hlngcode%>', '<%=hlngcodedb%>', '<%=hselindo%>', '<%=hselareao%>', '<%=hLoggedInUserNId%>', '<%=hLoggedInUserName%>', '<%=hOriginaldbnid%>', '<%=hdvnids%>');</script>
    <script type="text/javascript">
        window.onload = function () {
            try {
                if (getQueryString("popup") == "true") {
                    //document.body.style.display = "none";                    
                    document.body.className = "body_bg_none";
                    z("reg_rgt_sec").style.display = "none";
                    z("reg_lft_sec").className = "reg_lft_sec_ppup_wdth";
                    z("headerDIVMenu").style.display = "none";

                    z("header").style.display = "none";
                    setTimeout(function () {
                        z("footer").style.display = "none"; z("lo_icon_main").style.display = "none";
                        var loadfunObjet = getParentFunction("childHasLoad");
                        loadfunObjet();
                        document.getElementById('hlngcode').value = getQueryString("lng");
                    }, 10);
                }
                if ((document.getElementById('hlngcode').value == null) || (document.getElementById('hlngcode').value == "")) {
                    document.getElementById('hlngcode').value = getDefaultLanguage();
                }
                if ('<%=hAppProtected%>' == "true") {
                    z("radioRegister").style.display = "none";
                    z("spanRegister").style.display = "none";
                    z("radioHavePassword").style.display = "none";
                    z("spanHavePassword").innerText = '<%=hPasswordText%>';
                    z("spanHavePassword").innerHTML = '<%=hPasswordText%>';
                    z("spanSignOrRegister").innerHTML = '<%=hSignIn%>';
                }
            }
            catch (err) { }
        }
    </script>
    <!-- END OF DEVELOPER CODE -->
</asp:Content>
