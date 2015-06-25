<%@ Page Language="C#" MasterPageFile="~/libraries/aspx/RegistryMaster.master" AutoEventWireup="true" ValidateRequest="false" 
         CodeFile="RegComparison.aspx.cs" Inherits="libraries_aspx_RegComparison" Title="COMPARE" EnableSessionState="true" %>
         
<asp:Content ID="Content2" ContentPlaceHolderID="cphRegistryMainContent" Runat="Server">

    <script type="text/javascript" src="../js/di.jquery-1.4.4.js?v=<%=Global.js_version%>"></script>
    
    <script type="text/javascript" src="../js/sdmx/Comparison.js?v=<%=Global.js_version%>"></script> 
    
	<!-- Reg Left Section ...starts-->
	<div id="reg_lft_sec">
		<h3 id="lang_Compare_ATDSD"><!--Compare any two DSDs--></h3>
		
		<form id="frmComparisonDSD1AndDSD2" name="frmComparisonDSD1" action="UploadFile.aspx" enctype="multipart/form-data" method="post" >
		    <h4 id="lang_DSD1"><!--DSD 1--></h4><input id="UplDSD1" name="UplDSD1" type="file" class="confg_frm_inp_bx_txt_reg" size="100" />
		    <br />
		    <br />
		    <h4 id="lang_DSD2"><!--DSD 2--></h4><input id="UplDSD2" name="UplDSD2" type="file" class="confg_frm_inp_bx_txt_reg" size="100"/>
		    <br />
		    <br />
		    <input type="submit" name="btnCompare" id="btnCompare" value="" class="di_gui_button" style="width:150px"/>
		</form>
		<br />
		<br />
		<h3><span id="lang_Compare_YDSDA"><!--Compare your DSD against--></span> <label id="lblAdaptationName" runat="server"></label> <span id="lang_DSD"><!--DSD--></span></h3>
		<form id="frmComparisonDSDAgainstDevInfoDSD" name="frmComparisonDSDAgainstDevInfoDSD" action="UploadFile.aspx" enctype="multipart/form-data" method="post" >
		    <h4 id="lang_DSD_Compare"><!--DSD to compare--></h4><input id="UplDSDAgainstDevInfoDSD" name="UplDSDAgainstDevInfoDSD" type="file" class="confg_frm_inp_bx_txt_reg" size="100"/>
		    <br />
		    <br />
		    <input type="submit" name="btnCompareDSDAgainstDevInfoDSD" id="btnCompareDSDAgainstDevInfoDSD" value=""  class="di_gui_button" style="width:150px"/>
		</form>	
		 <br />
		 <br />	
		 <form id="frmStructuralComparison" action="RegComparison.aspx">
		    <div id="divStructuralComparison" class="roundedcorners">
            </div>
		 </form>	
        <br />
        <div id="divCodelistComparison" class="roundedcorners">
        </div>
        <input type="hidden" name="hdnDSDAgainstDevInfoDSD" id="hdnDSDAgainstDevInfoDSD" />
		<input type="hidden" name="hdnDSD1" id="hdnDSD1" />	
		<input type="hidden" name="hdnDSD2" id="hdnDSD2" />
        <input type="hidden" id="hErrorOccurred" />
        <input type="hidden" id="hMsgPleaseBrowseDSD" />
        <input type="hidden" id="hMsgPleaseBrowseDSD1AndDSD2" />	
        <input type="hidden" id="hMsgGeneratingReport" />	
        <input type="hidden" id="hMsgReportSuccessfullyGenerated" />
	</div>
	<!-- Reg Left Section ...ends-->
	
	<!-- START DEVELOPER CODE -->

    <script type="text/javascript">
        CookiePostfixStr = '_' + '<%=Global.CookiePostfixStr%>';    // use in hosting app

        var di_components = "Language";
        var di_component_version = '<%=Global.diuilib_version%>';
        var di_theme_css = '<%=Global.diuilib_theme_css%>';
        var di_diuilib_url = '<%=Global.diuilib_url%>';
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibincludes.js'" + "><\/script>");
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibcommon.js'" + "><\/script>");
        var hIsUploadedDSD = '<%= hIsUploadedDSD %>'; 
    </script>

    <script type="text/javascript">        onPageLoad('<%=hdsby%>', '<%=hdbnid%>', '<%=hselarea%>', '<%=hselind%>', '<%=hlngcode%>', '<%=hlngcodedb%>', '<%=hselindo%>', '<%=hselareao%>', '<%=hLoggedInUserNId%>', '<%=hLoggedInUserName%>', '<%=hOriginaldbnid %>');
    </script>
     <script type="text/javascript" src="../js/di.jquery.form.js"></script>

    <script type="text/javascript">
        di_jq(document).ready(function () {
            di_jq('#frmComparisonDSD1AndDSD2').ajaxForm({
                success: function (response) {
                    if (response == "No File") {
                        //             alert("Please browse both DSD1 and DSD2 files and then click on Compare button.");
                        alert(document.getElementById('hMsgPleaseBrowseDSD1AndDSD2').value);
                    }
                    else {
                        document.getElementById('divStructuralComparison').innerHTML = "";
                        document.getElementById('divStructuralComparison').style.display = "none";
                        document.getElementById('divCodelistComparison').innerHTML = "";
                        document.getElementById('divCodelistComparison').style.display = "none";
                        document.getElementById('hdnDSD1').value = response.split(",")[0];
                        document.getElementById('hdnDSD2').value = response.split(",")[1];
                        document.getElementById('hdnDSDAgainstDevInfoDSD').value = "";
                        document.getElementById('UplDSDAgainstDevInfoDSD').value = "";
                        CompareUserDSD1AgainstDSD2();
                    }
                }
            });


            di_jq('#frmComparisonDSDAgainstDevInfoDSD').ajaxForm({
                success: function (response) {
                    if (response == "No File") {
                        //             alert("Please browse DSD file to be compared against Master DSD and then click on Compare button.");
                        alert(document.getElementById('hMsgPleaseBrowseDSD').value);

                    }
                    else {
                        document.getElementById('divStructuralComparison').innerHTML = "";
                        document.getElementById('divStructuralComparison').style.display = "none";
                        document.getElementById('divCodelistComparison').innerHTML = "";
                        document.getElementById('divCodelistComparison').style.display = "none";
                        document.getElementById('hdnDSDAgainstDevInfoDSD').value = response;
                        document.getElementById('hdnDSD1').value = "";
                        document.getElementById('UplDSD1').value = "";
                        document.getElementById('hdnDSD2').value = "";
                        document.getElementById('UplDSD2').value = "";
                        CompareUserDSDAgainstDevInfoDSD();
                    }
                }
            });

        });
    </script>

    <!-- END OF DEVELOPER CODE -->
    
</asp:Content>

