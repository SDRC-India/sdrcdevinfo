<%@ Page Language="C#" MasterPageFile="~/libraries/aspx/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="DbMaster.aspx.cs" Inherits="libraries_aspx_Admin_DbList" Title="Untitled Page" EnableSessionState="True"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">   
    <h1 id="lang_db_DatabaseSettings"><!--Database Settings--></h1>
    <h4 id="lang_db_AddMod_DCDH"><!--Add and Modify database connection details here--></h4>
    
    <h2 id="langAvailableConnections"><!--Available Connections--></h2>

    <input type="hidden" id="langNewConnectionCheck" value="" />
    <!-- Nav Option ...starts-->
    <div class="adm_nav_opt">
		<ul>
			<li><a href="ConnectionDetails.aspx" id="langNew"><!--New--></a></li>
            <%--<li><a href="JavaScript:CheckForExistingConnections()" id="langNew"><!--New--></a></li>--%>
			<li id="liNewPipeline">|</li>
			<li><a href="JavaScript:EditConnectionDetails();" id="langEdit"><!--Edit--></a></li>
			<li>|</li>
			<li><a href="JavaScript:DeleteConnection();" id="langDelete"><!--Delete--></a></li>
		</ul>
	</div>
    <!-- Nav Option ...ends-->    
    
    <!-- Data Grid ...starts-->            
    <div id="divDbList" runat="server" class="DivUsersGrid">
    </div>
    <!-- Data Grid ...ends-->       
    
    <script type="text/javascript">
        CookiePostfixStr = '_' + '<%=Global.CookiePostfixStr%>';
        SelectLeftMenuItem("DbSettings");

        ////MOD LOG
        ////Issue Id:<9>    Issue Date: <7.05.2014>
        ////Issue Fixed by:<bijan@sdrc.co.in>
        ////Description: following line commented to enable the "new" button for adding multipule database.
       //CheckForExistingConnections();   
        
    </script>    
</asp:Content>

