<%@ Page Title="" Language="C#" MasterPageFile="~/libraries/aspx/RegistryMaster.master"
    AutoEventWireup="true" CodeFile="RegTools.aspx.cs" Inherits="libraries_aspx_Tools" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphRegistryMainContent" runat="Server">
    <script type="text/javascript" src="../js/di.jquery-1.4.4.js?v=<%=Global.js_version%>"></script>
    <script type="text/javascript" src="../js/sdmx/Tools.js?v=<%=Global.js_version%>"></script> 
    <script type="text/javascript">
        window.onload = function setMinWidth() {
            var els = document.getElementsByTagName("input");

            for (var x = 0; x < els.length; x++) {
                if (els[x].type &&
                        els[x].type.toLowerCase() == "submit") {
                    if (parseInt(els[x].offsetWidth, 10) < 90)
                        els[x].style.width = "90px";
                }
            }
        }
    </script>
    <script type="text/javascript">

        // create pivoting object
        var DEMO = {};
        window.cal = false;
        DEMO.pivot = { panel: 1, tab: 0, div: "pivot", needs: ["pivot"], cb: CallBack }

        // initiation function for the pivoting
        function init() {

            /* tabs */
            var tab = new OAT.Tab("content");
            for (var p in DEMO) {
                var name = DEMO[p].div;
                tab.add("tab_" + name, name);
            }

            /* panelbar_content */
            var pb = new OAT.Panelbar("panelbar", 10);
            pb.addPanel("pb_1", "pb_11");

            /* create dimmer element */
            var dimmerElm = OAT.Dom.create("div", { border: "2px solid #000", padding: "1em", position: "absolute", backgroundColor: "#fff" });
            document.body.appendChild(dimmerElm);
            OAT.Dom.hide(dimmerElm);

            for (var p in DEMO) { DEMO[p].drawn = false; }
            tab.options.goCallback = function (oldIndex, newIndex) {
                var oldName, newName;
                for (var p in DEMO) {
                    var v = DEMO[p];
                    if (v.tab == oldIndex) { oldName = p; }
                    if (v.tab == newIndex) { newName = p; }
                }


                var obj = DEMO[newName];
                //OAT.Dimmer.show(dimmerElm);
                OAT.Dom.center(dimmerElm, 1, 1);
                var ref = function () {
                    if (!window.location.href.match(/:source/)) {

                    }
                }
                OAT.Loader.loadFeatures(obj.needs, ref);
                //alert('after loadfeatures');
            }
            pb.go(0);
            tab.go(0);
        }

    </script>
    <div id="DivMainContainer" style="width: 100%; padding-left: 11px; float: left;">
        <div id="DivUpperCont" style="width: 100%; padding-left: 11px; float: left;">
            <div id="DivUpperLeft">
                <div id="reg_tool_sec">
                    <h3 id="lang_Validate_SDF">
                        <!--Validate your SDMX-ML Data Files-->
                    </h3>
                    <h4 id="lang_Select_SDF">
                        <!--Select SDMX-ML Data Files-->
                    </h4>
                    <form id="frmValidationSdmxMlFile" name="frmValidationSdmxMlFile" action="UploadFile.aspx"
                    enctype="multipart/form-data" method="post">
                    <input id="UplSdmxMlFile" name="UplInputSdmxMlFile" type="file" class="confg_frm_inp_bx_txt_reg di_gui_button" /><br />
                    <br />
                    <input type="submit" name="btnValidateSDMXML" id="btnValidateSDMXML" value="" class="di_gui_button" />
                    </form>
                    <br />
                    <div id="divValidateDSD">
                        <h3>
                            <span id="lang_Validate_YDSDA">
                                <!--Validate your DSD against-->
                            </span>
                            <label id="lblAdaptationName" runat="server">
                            </label>
                            <span id="lang_DSD">
                                <!--DSD-->
                            </span>
                        </h3>
                        <h4 id="lang_SelectDSD">
                            <!--Select DSD-->
                        </h4>
                        <form id="frmValidationDSD" name="frmValidationDSD" action="UploadFile.aspx" enctype="multipart/form-data"
                        method="post">
                        <input id="UplDSDFile" name="UplInputDSDFile" type="file" class="confg_frm_inp_bx_txt_reg di_gui_button"
                            /><br />
                        <br />
                        <input type="submit" name="btnValidateDSD" id="btnValidateDSD" value="" class="di_gui_button" />
                        </form>
                        <br />
                    </div>
                    <h3 id="lang_Validate_MR">
                        <%--Validate your Metadata Report Files--%></h3>
                    <h4 id="lang_Select_MR">
                        <%--Select Metadata Report Files--%></h4>
                    <form id="frmValidationMetadataReport" name="frmValidationMetadataReport" action="UploadFile.aspx"
                    enctype="multipart/form-data" method="post">
                    <input id="UplMetadataReportFile" name="UplInputMetadataReportFile" type="file" class="confg_frm_inp_bx_txt_reg di_gui_button"
                       /><br />
                    <br />
                    <input type="submit" name="btnValidateMetadataReportFile" id="btnValidateMetadataReportFile"
                        value="" class="di_gui_button" />
                    </form>
                    <br />
                    <!-- OAT Grid ...starts-->
                    <div id="divGridContainer" class="roundedcorners" style="width: 73%">
                        <div id="gridData">
                            <div id="dataTitle" class="heading2">
                            </div>
                            <div id="dataSubTitle">
                            </div>
                            <div id="content">
                            </div>
                            <!--<br />-->
                            <div id="pivot" class="pivot_sec">
                                <div id="pivot_page">
                                </div>
                                <div id="pivot_content">
                                </div>
                                <div id="pivot_chart">
                                </div>
                            </div>
                        </div>
                        <div id="divCountDataRows">
                        </div>
                        <div id="divSingleSource">
                        </div>
                    </div>
                    <!-- OAT Grid ...ends-->
                </div>
            </div>
            <div id="DivUpperRight">
                <div id="reg_tool_sec">
                    <h3 id="lang_Compare_ATDSD">
                        <!--Compare any two DSDs-->
                    </h3>
                    <form id="frmComparisonDSD1AndDSD2" name="frmComparisonDSD1" action="UploadFile.aspx"
                    enctype="multipart/form-data" method="post">
                    <h4 id="lang_DSD1">
                        <!--DSD 1-->
                    </h4>
                    <input id="UplDSD1" name="UplDSD1" type="file" class="confg_frm_inp_bx_txt_reg" size="100" />
                    <br />
                    <br />
                    <h4 id="lang_DSD2">
                        <!--DSD 2-->
                    </h4>
                    <input id="UplDSD2" name="UplDSD2" type="file" class="confg_frm_inp_bx_txt_reg" size="100" />
                    <br />
                    <br />
                    <input type="submit" name="btnCompare" id="btnCompare" value="" class="di_gui_button"
                        style="width: 150px" />
                    </form>
                    <br />
                    <br />
                    <h3>
                        <span id="lang_Compare_YDSDA">
                            <!--Compare your DSD against-->
                        </span>
                        <label id="lblAdaptationNameComp" runat="server">
                        </label>
                        <span id="lang_DSD_Comp">
                            <!--DSD-->
                        </span>
                    </h3>
                    <form id="frmComparisonDSDAgainstDevInfoDSD" name="frmComparisonDSDAgainstDevInfoDSD"
                    action="UploadFile.aspx" enctype="multipart/form-data" method="post">
                    <h4 id="lang_DSD_Compare">
                        <!--DSD to compare-->
                    </h4>
                    <input id="UplDSDAgainstDevInfoDSD" name="UplDSDAgainstDevInfoDSD" type="file" class="confg_frm_inp_bx_txt_reg"
                        size="100" />
                    <br />
                    <br />
                    <input type="submit" name="btnCompareDSDAgainstDevInfoDSD" id="btnCompareDSDAgainstDevInfoDSD"
                        value="" class="di_gui_button" style="width: 150px" />
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
                    <input type="hidden" id="Hidden1" />
                    <input type="hidden" id="hMsgPleaseBrowseDSD" />
                    <input type="hidden" id="hMsgPleaseBrowseDSD1AndDSD2" />
                    <input type="hidden" id="hMsgGeneratingReport" />
                    <input type="hidden" id="hMsgReportSuccessfullyGenerated" />
                </div>
            </div>
        </div>
        <div id="DivLowerCont">
        </div>
    </div>
    <!-- Reg Left Section ...starts-->
    <div id="divErrorDetails" class="popup_block">
        <!-- Popup Inside Content ...starts-->
        <div id="reg_content_containers">
            <h2 id="lang_Validation_Details">
            </h2>
            <h5>
            </h5>
            <div id="reg_wide_sec_ppup">
                <!-- Input Fields Area ...starts-->
                <div id="confg_Adm_box_lgin">
                    <textarea id="txtError" rows="27" cols="76"></textarea>
                </div>
                <!-- Input Fields Area ...ends-->
            </div>
        </div>
        <!-- Popup Inside Content ...ends-->
    </div>
    <!-- Reg Left Section ...ends-->
    <!-- For OAT Grid Purpose ...starts-->
    <div id="tab_pivot333">
        <table width="100%" cellpadding="0" cellspacing="2" border="0" style="float: left;">
            <tr>
                <td align="right">
                    <div id="panelbar">
                        <div id="pb_1">
                        </div>
                        <div id="pb_11">
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <!-- For OAT Grid Purpose ...ends-->
    <input type="hidden" name="hdnSdmxMlFile" id="hdnSdmxMlFile" />
    <input type="hidden" name="hdnDSD" id="hdnDSD" />
    <input type="hidden" name="hdnMetadataReport" id="hdnMetadataReport" />
    <input type="hidden" id="hErrorOccurred" />
    <input type="hidden" id="hMsgValidateSDMXML" />
    <input type="hidden" id="hMsgValidateDSD" />
    <input type="hidden" id="hMsgValidateMetadata" />
    <input type="hidden" id="hMsgSDMXMLValidationScenarios" />
    <input type="hidden" id="hMsgSDMXMLValidationStatus" />
    <input type="hidden" id="hMsgDSDValidationScenarios" />
    <input type="hidden" id="hMsgDSDValidationStatus" />
    <input type="hidden" id="hMsgMetadataReportValidationScenarios" />
    <input type="hidden" id="hMsgMetadataReportValidationStatus" />
    <input type="hidden" id="hMsgErrorDetailsNotFound" />
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
    <script type="text/javascript" src="../js/di.jquery.form.js?v=<%=Global.js_version%>"></script>
    <script type="text/javascript">
        di_jq(document).ready(function () {
            di_jq('#frmValidationSdmxMlFile').ajaxForm({
                success: function (response) {
                    if (response == "No File") {
                        alert(document.getElementById('hMsgValidateSDMXML').value);
                    }
                    else {
                        document.getElementById('hdnSdmxMlFile').value = response;
                        document.getElementById('hdnDSD').value = "";
                        document.getElementById('UplDSDFile').value = "";
                        document.getElementById('UplMetadataReportFile').value = "";
                        ValidateSDMXML();
                    }

                }

            });

            di_jq('#frmValidationDSD').ajaxForm({
                success: function (response) {
                    if (response == "No File") {
                        alert(document.getElementById('hMsgValidateDSD').value);
                    }
                    else {
                        document.getElementById('hdnDSD').value = response;
                        document.getElementById('hdnSdmxMlFile').value = "";
                        document.getElementById('UplSdmxMlFile').value = "";
                        document.getElementById('UplMetadataReportFile').value = "";
                        ValidateDSD();
                    }
                }


            });

            di_jq('#frmValidationMetadataReport').ajaxForm({
                success: function (response) {
                    if (response == "No File") {
                        alert(document.getElementById('hMsgValidateMetadata').value);
                    }
                    else {
                        document.getElementById('hdnMetadataReport').value = response;
                        document.getElementById('hdnSdmxMlFile').value = "";
                        document.getElementById('hdnDSD').value = "";
                        document.getElementById('UplSdmxMlFile').value = "";
                        document.getElementById('UplDSDFile').value = "";
                        ValidateMetadataReport();
                    }

                }

            });

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
