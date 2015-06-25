<%@ Page Language="C#" MasterPageFile="~/libraries/aspx/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="ProviderDetails.aspx.cs" Inherits="libraries_aspx_Admin_ProviderDetails" Title="Untitled Page" EnableSessionState="True" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <h1 id="lang_pd_DatabaseSettings"><!--Database Settings--></h1> 
    <h4 id="lang_pd_AddMod_DCDH"><!--Add and Modify database connection details here--></h4>            
    
    <h2 id="langProviderDetails"><!--Provider Details--></h2> 
    
    <!-- Input Fields Area ...starts-->    
    <div class="confg_Adm_box">  
    
        <p class="confg_frm_txt" id="langUserId"><!--User Id--></p>
        <p class="confg_frm_inp_bx"><input type="text" class="confg_frm_inp_bx_txt" /></p>
        <div class="clear"></div>   
        
        <p class="confg_frm_txt" id="langName"><!--Name--></p>
        <p class="confg_frm_inp_bx"><input type="text" class="confg_frm_inp_bx_txt" /></p>
        <div class="clear"></div>      
        
        <p class="confg_frm_txt" id="langDescription"><!--Description--></p>
        <p class="confg_frm_inp_bx"><input type="text" class="confg_frm_inp_bx_txt" /></p>
        <div class="clear"></div>             

        <p class="confg_frm_txt" id="langDepartment"><!--Department--></p>
        <p class="confg_frm_inp_bx"><input type="text" class="confg_frm_inp_bx_txt" /></p>
        <div class="clear"></div>
        
        <p class="confg_frm_txt" id="langEmail"><!--Email--></p>
        <p class="confg_frm_inp_bx"><input type="text" class="confg_frm_inp_bx_txt" /></p>
        <div class="clear"></div>        

        <p class="confg_frm_txt" id="langRole"><!--Role--></p>
        <p class="confg_frm_inp_bx"><input type="text" class="confg_frm_inp_bx_txt" /></p>
        <div class="clear"></div>
        
        <p class="confg_frm_txt" id="langFax"><!--Fax--></p>
        <p class="confg_frm_inp_bx"><input type="text" class="confg_frm_inp_bx_txt" /></p>
        <div class="clear"></div>  
        
        <p class="confg_frm_txt" id="langTelephone"><!--Telephone--></p>
        <p class="confg_frm_inp_bx"><input type="text" class="confg_frm_inp_bx_txt" /></p>
        <div class="clear"></div>              
       
    </div>
    <!-- Input Fields Area ...ends-->    
    
    <!-- Buttons ...starts-->
    <div class="adm_upd_bttn"><input type="button" id="lang_pd_Save" value="" class="di_gui_button" /><input type="button" id="btnlang_pd_Back" value="" onclick="MoveOnConnectionDetails();" class="di_gui_button" /><input type="button" id="btnOptDb_pd_Next" value="" onclick="MoveOnOptimizeDb()" class="di_gui_button" /></div>
    <!-- Buttons ...ends-->      
    
    <script type="text/javascript">
        SelectLeftMenuItem("DbSettings");
    </script> 
    
</asp:Content>


