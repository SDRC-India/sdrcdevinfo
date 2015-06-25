<%@ Page Language="C#" MasterPageFile="~/libraries/aspx/HomeMaster.master" EnableSessionState="true" AutoEventWireup="true"
    CodeFile="Selection.aspx.cs" Inherits="libraries_aspx_Selection" %>

<asp:Content ID="cntMainContent" ContentPlaceHolderID="cphMainContent" runat="Server">

    <!-- Page Markup starts from here -->
    
    <div class="inner_content_container">
      <table border="0" align="center" cellpadding="0" cellspacing="0" class="panel_table">
        <tr>
          <td valign="top"><table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                          <td width="12" height="12"><img src="../../stock/themes/default/images/left_top.png" width="12" height="12" /></td>
                          <td height="12" class="round_top"></td>
                          <td width="12" height="12"><img src="../../stock/themes/default/images/top_right.png" width="12" height="12" /></td>
                        </tr>
                        <tr>
                          <td width="12"class="round_left">&nbsp;</td>
                          <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                              <td class="heading2">Select Topic</td>
                            </tr>
                            <tr>
                              <td><table width="100%">
    <tr>
        <td><div id="breadcrum_div" class="breadcrum_content"></div></td>
    </tr>
    <tr><td>
	<div>
        
        <!-- Inidcator Selection Box -->
        <div id="indicator_div"></div>
        
        <!-- Area Selection Box -->
        <div id="area_div"></div>
            
    </div>
	</td></tr>
	<tr><td align="right"><input type="button" value="View Data" onclick="onGotoDataViewPg();" class="submit_button" /></td></tr>
	</table></td>
                            </tr>
                          </table></td>
                          <td width="12" class="round_right">&nbsp;</td>
                        </tr>
                        <tr>
                          <td width="12" height="12"><img src="../../stock/themes/default/images/bottom_left.png" width="12" height="12" /></td>
                          <td height="12" class="round_botttom"></td>
                          <td width="12" height="12"><img src="../../stock/themes/default/images/bottom_right.png" width="12" height="12" /></td>
                        </tr>
                      </table>
          </td>
        </tr>
      </table>
    </div>       
    <!-- End of Page Markup -->
    



    <!-- DEVELOPER CODE -->
    <script type="text/javascript">
        CookiePostfixStr = '_' + '<%=Global.CookiePostfixStr%>';    // use in hosting app

        var di_components = "Area,Indicator,Language";
        var di_component_version = '<%=Global.diuilib_version%>';
        var di_theme_css = '<%=Global.diuilib_theme_css%>';
        var di_diuilib_url = '<%=Global.diuilib_url%>';
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibincludes.js'" + "><\/script>");
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibcommon.js'" + "><\/script>");
    </script>
    <script type="text/javascript" src="../js/Selection.js?v=<%=Global.js_version%>"></script>
    <script type="text/javascript">onPageLoad('<%=hdsby%>', '<%=hdbnid%>', '<%=hselarea%>', '<%=hselind%>', '<%=hlngcode%>', '<%=hlngcodedb%>', '<%=hselindo%>', '<%=hselareao%>');</script>
    <!-- END OF DEVELOPER CODE -->


</asp:Content>
