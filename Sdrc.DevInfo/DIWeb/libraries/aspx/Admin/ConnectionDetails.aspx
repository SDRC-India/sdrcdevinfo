<%@ Page Language="C#" MasterPageFile="~/libraries/aspx/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="ConnectionDetails.aspx.cs" Inherits="libraries_aspx_Admin_DbEdit1" Title="Untitled Page" EnableSessionState="True" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript">
        msgOldPwd = '<%=msgOldPwd %>'
        msgNewPwd = '<%=msgNewPwd %>'
        msgCNewPwd = '<%=msgCNewPwd %>'
        msgPwdNotMatch = '<%=msgPwdNotMatch %>'
    </script>

    <h1 id="langDatabaseSettings"><!--Database Settings--></h1> 
    <h4 id="langAddMod_DCDH"><!--Add and Modify database connection details here--></h4>     
    
    <h2 id="langConnectionDetails"><!--Connection Details--></h2>     
    
    <!-- Input Fields Area ...starts-->    
    <div class="confg_Adm_box">         
        <p class="confg_frm_txt" id="langConnectionName"><!--Connection Name--> <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx_big"><asp:TextBox ID="txtConnName" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox>&nbsp;&nbsp;<input type="button" id="btnTestConnection" value="" onclick="TestConnection();" class="di_gui_button" style="vertical-align:top;" /></p> 
        <div class="clear"></div>         
        
        <p class="confg_frm_txt" id="langDatabaseType"><!--Database Type--> <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx_big"><asp:DropDownList ID="ddlDatabaseType" runat="server" AutoPostBack="false" class="confg_frm_inp_bx_txt_dd"></asp:DropDownList></p> 
        <div class="clear"></div>         
        
        <p class="confg_frm_txt" id="langServerHostName"><!--Server/Host Name--> <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx_big"><asp:TextBox ID="txtServerName" onblur="CheckValidation(this)" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p> 
        <div class="clear"></div>  
        
        <p class="confg_frm_txt" id="langDatabaseName"><!--Database Name--> <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx_big"><asp:TextBox ID="txtDatabaseName" onblur="CheckValidation(this)" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p> 
        <div class="clear"></div>       
        
        <p class="confg_frm_txt" id="langUserName"><!--User Name--> <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx_big"><asp:TextBox ID="txtUserName" onblur="CheckValidation(this)" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox>
        &nbsp;&nbsp;<a id="aChangePassword" href="javascript:void(0)" onclick="showChangePwdDiv(true);"></a></p>
        <div class="clear"></div>

        <div id="dvPassword">
            <p class="confg_frm_txt" id="langPassword">
                <!--Password-->
            </p>
            <p class="confg_frm_inp_bx">
                <asp:textbox id="txtPassword" onblur="CheckValidation(this)" runat="server" textmode="Password"
                    class="confg_frm_inp_bx_txt">
                </asp:textbox></p>
            <div class="clear">
            </div>
        </div>
        <p class="confg_frm_txt" id="langDescription"><!--Description--></p>
        <p class="confg_frm_inp_bx_big"><asp:TextBox ID="txtDesc" runat="server" TextMode="multiLine" Rows="4" class="confg_frm_inp_bx_txt_big"></asp:TextBox></p>
        <p class="confg_frm_undr_data" id="DefnCatlog"><asp:CheckBox  style="display:none" ID="chkDefault" runat="server" Text=""  Checked="true"/><label id="lang_cd_DefaultDatabase" style="display:none"><!--Default Database--></label>
            <span id="IsCatalogReg" style="display: none">
                <asp:checkbox id="chkRegCatalog" runat="server" checked="true" text="" /><label id="lang_cd_RegCatalog"><!--Register in Catalog--></label></span></p>
        <div class="clear">
        </div>
        <div id="DivOuterEditPassword" class="popup_block">
            <p class="fnt_bld fnt_sz_fftn">
                <%=ChangeDBPassword%></p><br />
            <div id="DivInnerEditPassword">                
                <p class="confg_frm_txt_small" id="lngOPWD">
                    <!--User Name-->
                    <span class="Mandatory">*</span></p>
                <p class="confg_frm_inp_bx">
                    <asp:textbox id="txtOPWD" runat="server" textmode="Password" class="confg_frm_inp_bx_txt txtbox_width">
                    </asp:textbox></p>               
                <div class="clear">
                </div>
                <p class="confg_frm_txt_small" id="lngNPWD">
                    <!--User Name-->
                    <span class="Mandatory">*</span></p>
                <p class="confg_frm_inp_bx">
                    <asp:textbox id="txtNOPWD" runat="server" textmode="Password" class="confg_frm_inp_bx_txt txtbox_width">
                    </asp:textbox></p>               
                <div class="clear">
                </div>
                <p class="confg_frm_txt_small" id="lngNPWDR">
                    <!--User Name-->
                    <span class="Mandatory">*</span></p>
                <p class="confg_frm_inp_bx">
                    <asp:textbox id="txtNOPWDR" runat="server" textmode="Password" class="confg_frm_inp_bx_txt txtbox_width">
                    </asp:textbox></p>
                <div class="clear">
                </div>
                <div style="float:right">
                    <br />
                    <input type="button" id="btnOK" value="" onclick="ChangeDBPassword();" class="di_gui_button flt_lft" />
                    <input type="button" id="btnCancel" value="" onclick="javascript:di_jq('#DivOuterEditPassword').hide('slow');RemoveMaskingDiv();" class="di_gui_button" />                    
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
        <div class="clear">
        </div>
    </div>
    <!-- Input Fields Area ...ends-->    
    
    <!-- Database Connections Buttons ...starts-->
    <div class="adm_upd-rgt_bttn">
    <ul>
    <li class="first"><input type="button" id="btnConDetRegisterDb" value="" disabled="disabled" onclick="RegisterDatabase();" class="di_gui_button_disabled flt_lft" /></li>
    <li class="first"><input type="button" id="btnConDetUpdate" value="" onclick="UpdateDbConnection();" class="di_gui_button" /></li>
    <li><input type="button" id="btnlangClear" value="" onclick="ClearControlsValues();" class="di_gui_button" /></li>
    <li><input type="button" id="btnlangBack" value="" onclick="window.location='DbMaster.aspx';" class="di_gui_button" /></li>    
    <%--<li><input type="button" id="btnConDetNext" value="" disabled="disabled" onclick="MoveOnProviderDetails();" class="di_gui_button_disabled" /></li>--%>    
    <li><input type="button" id="btnConDetNext" value="" disabled="disabled" onclick="MoveOnOptimizeDb();" class="di_gui_button_disabled" /></li>
    </ul>
    <div class="clear"></div>     
      <br /><span style="color:green; font-size:11px; text-align:left;" id="LangUpdateNote" style="display:none"> 
    </div>
    <!-- Database Connections Buttons ...ends-->     
    
    <script type="text/javascript">
        SelectLeftMenuItem("DbSettings");
        ShowHideConDetButton();
        if ('<%=IsAdd%>' == "false") {
            di_jq("#dvPassword").hide();
        }
        else {
            di_jq("#aChangePassword").hide();   
        }
    </script>
</asp:Content>

