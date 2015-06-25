<%@ Page Language="C#" MasterPageFile="~/libraries/aspx/HomeMaster.master" AutoEventWireup="true" CodeFile="admin.aspx.cs" Inherits="libraries_aspx_admin_admin" Title="Untitled Page" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
<form id="frmAdmin" runat="server" >

<div class="content_containers">

    <!-- Left Links Section ...starts-->
    <div id="lft_sec_adm">
        <ul>
        <li><a id="aConfig" onclick="ShowConfiguration();" class="navActive_admn">Configuration</a></li>
        <li><a id="aDbConnec" onclick="ShowDbConnection();">Database Connections</a></li>
        </ul>
    </div>
    <!-- Left Links Section ...ends-->   
    
    <!-- Right Config Data Section ...starts-->
    <div id="rgt_sec_adm">
        
        <!-- Configuration ...starts-->
        <div id="div_configuration">             
            
            <h1>Configuration</h1>
            <div class="confg_Adm_box">
            
                <p class="confg_frm_txt">Adaptation Name <span class="Mandatory">*</span></p>
                <p class="confg_frm_inp_bx"><asp:TextBox ID="txtAdaptationName" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p>
                <div class="clear"></div>
                
                <p class="confg_frm_txt">Version <span class="Mandatory">*</span></p>
                <p class="confg_frm_inp_bx"><asp:TextBox ID="txtVersion" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p> 
                <div class="clear"></div>  
                
                <p class="confg_frm_txt">Dev Info User Interface Library URL</p>
                <p class="confg_frm_inp_bx"><asp:TextBox ID="txtDiUiLibUrl" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p> 
                <div class="clear"></div>  
                
                <p class="confg_frm_txt">Dev Info User Interface Library Theme CSS</p>
                <p class="confg_frm_inp_bx"><asp:TextBox ID="txtDiUiLibThemCss" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p> 
                <div class="clear"></div>  
                
                <p class="confg_frm_txt">Facebook Application Id</p>
                <p class="confg_frm_inp_bx"><asp:TextBox ID="txtFBAppID" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p> 
                <div class="clear"></div>    
                
                <p class="confg_frm_txt">Facebook Application Secret</p>
                <p class="confg_frm_inp_bx"><asp:TextBox ID="txtFBAppSecret" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p> 
                <div class="clear"></div>  
                                                    
            </div>
            
            <!-- Configuration Update Button ...starts-->
            <div class="adm_upd_bttn"><input type="button" value="Update" onclick="UpdateConfiguration();" class="di_gui_button" /></div>
            <!-- Configuration Update Button ...ends-->    
         
        <!-- Configuration ...ends-->
        </div>
          
        
        <!--Database Connections ...starts-->      
        <div id="div_dbConnection" style="display:none;">
                 
            <h1>Database Connections</h1>
            <div class="confg_Adm_box">
            
                <%-- Category --%>
                <p class="confg_frm_txt_sml">Category</p>
                <p class="confg_frm_inp_bx_big"><asp:DropDownList ID="cmbCategory" runat="server" AutoPostBack="false" onchange="ChangeCategory();" class="confg_frm_inp_bx_txt_big"></asp:DropDownList></p>
                <div class="clear"></div>
                
                <%-- Available Connection --%>
                <p class="confg_frm_txt_sml">Available Connection</p>
                <p class="confg_frm_inp_bx_big"><asp:DropDownList ID="cmbAvilableConn" runat="server" AutoPostBack="false" onchange="ChangeConnection();" class="confg_frm_inp_bx_txt_dd"></asp:DropDownList>&nbsp;<img alt="New Connection" src="../../stock/themes/default/images/new_connection.png" onclick="NewConnection();" class="cur_pointr" style="vertical-align:top;" /><img alt="Delete Connection" src="../../stock/themes/default/images/delete_connection.png" onclick="DeleteConnection();" class="cur_pointr" style="vertical-align:top;" /></p> 
                <div class="clear"></div>  
                
                <%-- Connection Name --%>
                <p class="confg_frm_txt_sml">Connection Name <span class="Mandatory">*</span></p>
                <p class="confg_frm_inp_bx_big"><asp:TextBox ID="txtConnName" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox>&nbsp;&nbsp;<input type="button" id="btnTestConnection" value="Test Connection" onclick="TestConnection();" class="di_gui_button" style="width:110px; vertical-align:top; visibility:hidden" /></p> 
                <div class="clear"></div>  
                
                <%-- Database Type --%>
                <p class="confg_frm_txt_sml">Database Type <span class="Mandatory">*</span></p>
                <p class="confg_frm_inp_bx_big TextBox"><asp:DropDownList ID="cmbDatabaseType" runat="server" AutoPostBack="false" class="confg_frm_inp_bx_txt_dd"></asp:DropDownList></p> 
                <div class="clear"></div>  
                
                <%-- ServerName --%>
                <div id="tr_ServerName" runat="server">
                <p class="confg_frm_txt_sml">Server/Host Name <span class="Mandatory">*</span></p>
                <p class="confg_frm_inp_bx_big"><asp:TextBox ID="txtServerName" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p> 
                </div>  
                <div class="clear"></div>  
                
                <%-- DatabaseName --%>
                <div runat="server" id="tr_DatabaseName">
                <p class="confg_frm_txt_sml">Database Name <span class="Mandatory">*</span></p>
                <p class="confg_frm_inp_bx TextBox"><asp:TextBox ID="txtDatabaseName" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p>
                </div>
                <div class="clear"></div>  
                
                <%-- UserName --%>
                <div runat="server" id="tr_UserName">
                <p class="confg_frm_txt_sml">User Name <span class="Mandatory">*</span></p>
                <p class="confg_frm_inp_bx_big"><asp:TextBox ID="txtUserName" runat="server" CssClass="TextBox confg_frm_inp_bx_txt" OnTextChanged="txtUserName_TextChanged"></asp:TextBox></p> 
                </div>
                <div class="clear"></div>                          
                
                <%-- Password --%>  
                <div runat="server" id="tr_Password">
                <p class="confg_frm_txt_sml">Password</p>
                <p class="confg_frm_inp_bx_big"><asp:textbox ID="txtPassword" runat="server" TextMode="Password" class="confg_frm_inp_bx_txt"></asp:textbox></p> 
                </div>
                <div class="clear"></div>     
                
                <%-- Defalut Area --%>   
                <div runat="server" id="tr1">
                <p class="confg_frm_txt_sml">Default Area</p>
                <p class="confg_frm_inp_bx_big"><asp:TextBox ID="txtDefaultArea" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p> 
                </div>
                <div class="clear"></div>   

                <%-- Description --%>  
                <div runat="server" id="tr2">
                <p class="confg_frm_txt_sml">Description</p>
                <p class="confg_frm_inp_bx_big"><asp:TextBox ID="txtDesc" runat="server" TextMode="multiLine" class="confg_frm_inp_bx_txt_big"></asp:TextBox></p>
                <p class="confg_frm_undr_data"><asp:CheckBox ID="chkDefault" runat="server" Text="Default Database" /></p> 
                </div>
                <div class="clear"></div>                           
                
            </div>    
             
            <!-- Database Connections Update/Register Button ...starts-->
            <div class="adm_upd-rgt_bttn"><input type="button" id="btnDbUpdate" value="Update" onclick="return UpdateDbConnection();" class="di_gui_button" /><input type="button" id="btnRegisterDatabase" value="Register Database" onclick="RegisterDatabase();" class="di_gui_button" /><input type="button" id="btnRegDbCancel" value="Cancel" onclick="CancelRegDbConnection();" class="di_gui_button" /></div>
            <!-- Database Connections Update/Register Button ...ends--> 
                    
        <!--Database Connections ...ends-->           
        </div>            
              
    <!-- Right Config Data Section ...ends-->        
    <div class="clear"></div>
    </div>
    
</div>    
</form>    
    
    <!-- DEVELOPER CODE -->
    <script type="text/javascript">
    var di_components = "Language";
    var di_component_version = '<%=Global.diuilib_version%>';
    var di_theme_css = '<%=Global.diuilib_theme_css%>';
    var di_diuilib_url = '<%=Global.diuilib_url%>';
    document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibincludes.js'" + "><\/script>");
    document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibcommon.js'" + "><\/script>");
    </script>
	
    <script type="text/javascript" src="../js/admin.js?v=<%=Global.js_version%>"></script>
    
    <script type="text/javascript">onPageLoad('<%=hlngcode%>','<%=hdefdbnid%>');</script>
    <!-- END OF DEVELOPER CODE -->
    
</asp:Content>

