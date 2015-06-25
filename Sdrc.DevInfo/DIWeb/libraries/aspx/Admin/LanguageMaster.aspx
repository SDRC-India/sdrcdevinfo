<%@ Page Language="C#" MasterPageFile="~/libraries/aspx/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="LanguageMaster.aspx.cs" Inherits="libraries_aspx_Admin_LanguageMaster" Title="Untitled Page" EnableSessionState="True" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <h1 id="lang_lm_LanguageSettings"><!--Language Settings--></h1>  
    <h4 id="langConfigure_Site_MLang"><!--Configure your site in multiple languages--></h4>       
    
    <h2 id="langAvailableLanguages"><!--Available Languages--></h2>  
    
    <!-- Nav Option ...starts-->
    <div class="adm_nav_opt">
		<ul>
			<%--<li><a href="Language.aspx" id="lang_lm_New"><!--New--></a></li>--%>
            <%--<li><a href="#" id="lang_lm_New"><!--New--></a></li>--%>
            <li><a href="JavaScript:ShowNewLanguageDiv();" id="lang_lm_New" style="display:none"><!--New--></a></li>
			<%--<li>|</li>--%>
			<li><a href="JavaScript:EditLanguage();" id="lang_lm_Edit"><!--Edit--></a></li> 
			<%--<li>|</li>--%>
			<li><a href="JavaScript:DeleteLanguage();" id="lang_lm_Delete" style="display:none"><!--Delete--></a></li>
		</ul>
	</div>  
    <!-- Nav Option ...ends-->   
    
    <!-- Data Grid ...starts-->            
    <div id="divLngList" runat="server" class="DivUsersGrid">                    
    </div>     
    
    
    <!-- Button ...starts-->
    <div class="adm_upd_bttn"><input type="button" id="langGeneratePageXMLs" value="" onclick="GenerateAllPagesXML();" class="di_gui_button adm_genpg_bttn_sz" /></div>
    <!-- Button ...ends-->      
            
    <!-- Data Grid ...ends-->    

    <div id="NewLngDiv" class="popup_block" style="width:400px; height:223px; z-index:150;">
        <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr>
		        <td class="popup_heading1" id="lang_Language"><!--Language--></td>
		    </tr>
		    <tr>
	            <td valign="top">
	                <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:1px solid #d3d3d3;">
                        <tr>
                            <td id="lang_Language_Name"><!--Language Name--></td>
                        </tr>
                        <tr>
                            <td><input type="text" id="txtLanguageName" onKeyPress="AddNewLanguageDefaultButton(event);" /></td>
                        </tr>
                        <tr>
                            <td id="lang_Language_Code"><!--Language Code--></td>
                        </tr>
                        <tr>
                            <td><input type="text" id="txtLanguageCode" maxlength="2" onKeyPress="AddNewLanguageDefaultButton(event);" /></td>
                        </tr>
                        <tr>
                            <td id="lang_Right-to-Left"><!--Right-to-Left--></td>
                        </tr>
                        <tr>
                            <td><asp:DropDownList ID="ddlPageDir" runat="server" AutoPostBack="false"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td align="right">
                                <input type="button" id="btnSave" value="" onclick="AddNewLanguage();" class="di_gui_button" />
                                <input type="button" id="btnCancel" value="" onclick="HideNewLanguageDiv();" class="di_gui_button" />                            
                            </td>
                        </tr>
                    </table>
                </td>
	        </tr>
	    </table>
    </div>

    
    <script type="text/javascript">
        SelectLeftMenuItem("LngSettings");
        GetAllLanguageList();
    </script>    
</asp:Content>

