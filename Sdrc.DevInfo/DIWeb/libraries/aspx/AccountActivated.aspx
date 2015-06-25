<%@ Page Title="" Language="C#" MasterPageFile="~/libraries/aspx/HomeMaster.master" AutoEventWireup="true" CodeFile="AccountActivated.aspx.cs" Inherits="libraries_aspx_AccountActivated" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
<!-- DEVELOPER CODE -->
<div class="content_containers">
    <div id="reg_content_containers"><br />
          <h2 id="langRegistration"><%--User Registration--%></h2>
			<h5 id="hAccountActivated"><%--Please click on the links below to login to the site--%></h5> 
           <!-- <br />
            <br />-->
<div id="reg_lft_sec">
  <p class="pddn_lft_nne fnt_sz_fftn"><b><span id="langAccountActivated" style="display:none"><%--Your account is activated.--%></span></b></p>
  <p class="pddn_lft_nne fnt_sz_fftn"><b><span id="langErrorInActivation" style="display:none"><%--Some Error Occurred! Please try later--%></span></b></p>
   <p class="frgtn_txt_link" style="float:left;padding-left:30px;"><a id="regenerateLink" href="javascript:RegenerateActivationLink();" style="display:none">Regenerate the activation link</a></p>
</div>
 <span id="lngRegenerateLinkMsg" style="display:none;"></span>
<div id="reg_rgt_sec">
 <div id ="divLoginHome" class="reg_lgn_confrm_txt_box" style="display:none">
 <p id="spanLogin" class="fnt_nrml"><a id="aActivationLogin" href ="Login.aspx"><%--Click here to Login--%></a></p>
 <br /><br />
 <p id="spanHome" class="fnt_nrml"><a id="aActivationHome" href ="Home.aspx"><%--Click here to go to Home Page--%></a></p>
 </div>
</div>
 <div class="clear"></div>
 </div>
</div>
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
                AccountActivation();

            }
            catch (err) { }
        }
    </script>
    <!-- END OF DEVELOPER CODE -->
</asp:Content>



