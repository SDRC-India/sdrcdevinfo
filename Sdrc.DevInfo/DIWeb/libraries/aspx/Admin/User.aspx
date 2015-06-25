<%@ Page Title="" Language="C#" MasterPageFile="~/libraries/aspx/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="User.aspx.cs" Inherits="libraries_aspx_Admin_User" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <h1 id="lang_User_Settings"><!--User Settings--></h1> 
    <h4 id="lang_Add_MUDH"><!--Add and Modify user details here--></h4>    

    <h2 id="langAvailableConnections"><!--Available Users--></h2>  
    
    <!-- Nav Option ...starts-->
    <div class="adm_nav_opt">
		<ul>
			<li><a href="JavaScript:NewUserDetails();" id="lang_New"><!--New--></a></li>
			<li>|</li>
			<li><a href="JavaScript:EditUserDetails();" id="lang_Edit"><!--Edit--></a></li> 
			<li>|</li>
			<li><a href="JavaScript:DeleteUser();" id="lang_Delete"><!--Delete--></a></li>
            <%--<li>|</li>  --%>    <%--Don't delete this line--%>
			<%--<li><a href="JavaScript:ShowForgotPasswordDialog();" >Forgot Password</a></li>--%> <%--Don't delete this line--%>
		</ul>
	</div>  	
    <!-- Nav Option ...ends-->    

    <!-- Data Grid ...starts-->            
    <div id="divUsersList" runat="server" class="DivUsersGrid">                    
    </div>     
    <!-- Data Grid ...ends-->    

    <div id="divUserOuterBox" class="popup_block">
        
        <h1 id="hdnNewUser" class="popup_heading" style="display:none;"><!--New User--></h1>
        <h1 id="hdnEditUser" class="popup_heading" style="display:none;"><!--Edit User--></h1>

        <div class="popup_brd">  
        
            <span id="lang_EMail_Address"><!--E-Mail Address--></span> * <input id="txtEmail" type="text" onkeypress="ClickInsertOrUpdateUser(event);" /> <br /><br />
            <span id="lang_First_Name"><!--First Name--></span> * <input id="txtFirstName" type="text" onkeypress="ClickInsertOrUpdateUser(event);" /> <br /><br />            
            <span id="lang_Country"><!--Country--></span>             
            <select id="ddlSelectCountryUser"></select>
            <br /><br />
            
            <span id="lang_Password"><!--Password--></span> * <input id="txtPassword" type="password" onkeypress="ClickInsertOrUpdateUser(event);" /> <br /><br />
            <span id="lang_Confirm_Password"><!--Confirm Password--></span> * <input id="txtConfirmPassword" type="password" onkeypress="ClickInsertOrUpdateUser(event);" /> <br /><br />

            <span id="spanSendDevInfoUpdates"><!--Send me DevInfo updates--></span>
            <input id="chkSendDevInfoUpdates" type="checkbox"/>                        
        </div>

        <input id="btnNewUser" type="button" value="" onclick="CreateNewUser();" />
        <input id="btnEditUser" type="button" value="" onclick="UpdateUser();" />
        <input id="btnCancelUser" type="button" value="" onclick="HideUserPopup();" />

    </div>

    <div id="divForgotPwdOuterBox" class="popup_block">
        
        <h1 id="hdnForgotPwd" class="popup_heading" ><!--Forgot Password--></h1>        

        <div class="popup_brd">        
            <span id="lang_EMail_Address_ppup"><!--E-Mail Address--></span> * <input id="txtFPEmail" type="text" onkeypress="ClickForgotPwd(event);" /> <br /><br />     
        </div>

        <input id="btnFPOk" type="button" value="" onclick="SendForgotPassword();" />        
        <input id="btnFPCancel" type="button" value="" onclick="HideForgotPwdPopup();" />

    </div>

    <script type="text/javascript">
        SelectLeftMenuItem("User");
        FillUserDdlCountries();
    </script>

</asp:Content>


