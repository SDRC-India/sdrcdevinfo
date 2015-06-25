<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="libraries_aspx_Admin_Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../../stock/themes/default/css/diuilibcommon.css" rel="stylesheet" type="text/css" />    
    <link href="../../../stock/themes/default/css/style.css" rel="stylesheet" type="text/css" />   

    <script type="text/javascript" src="../../js/common.js?v=<%=Global.js_version%>"></script>
    <script type="text/javascript" src="../../js/di.jquery-1.4.4.js?v=<%=Global.js_version%>"></script>
    <script type="text/javascript" src="../../js/Admin.js?v=<%=Global.js_version%>"></script>
    <script type="text/javascript" src="../../js/browsr_selector.js?v=<%=Global.js_version%>"></script>
</head>
<body>
    <form id="frmAdminLogin" runat="server">

    <!-- Header Area ...starts-->
    <div id="header">
        <div class="header">
            <!-- Logo Section ...starts-->
	        <div class="main_logo"><a href="../home.aspx"><img src="../../../stock/themes/default/images/cust/logo.png" alt="Education Info Cambodia" width="221" height="51"/></a></div>
	        <!-- Logo Section ...ends-->  	        
        </div>
    </div>
    <!-- Header Area ...ends-->
    
    <div class="content_containers">  
        <div style="height:75px;"></div>
        <br /><br />        
        E-Mail Address * <input id="txtEmail" type="text" onkeypress="ClickValidateCredential(event);" /> <br /><br />            
        Password * <input id="txtHavePassword" type="password" onkeypress="ClickValidateCredential(event);" /> <br /><br />
        <input id="btnLogin" type="button" value="Login" onclick="ValidateCredential();" />
    </div>

    </form>
</body>
</html>
