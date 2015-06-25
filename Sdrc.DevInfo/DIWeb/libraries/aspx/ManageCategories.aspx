<%@ Page Title="" Language="C#" MasterPageFile="~/libraries/aspx/HomeMaster.master"
    AutoEventWireup="true" CodeFile="ManageCategories.aspx.cs" Inherits="libraries_aspx_ManageCategories" %>

<asp:content id="Content1" contentplaceholderid="cphHeadContent" runat="Server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="cphMainContent" runat="Server">
    <input type="hidden" id="lang_ToolTipMovUp" value="" />
    <input type="hidden" id="lang_ToolTipMovDown" value="" />
    <input type="hidden" id="Lang_MenuCategory_Alert" value="" />
    <input type="hidden" id="Lang_MenuLinkText_Alert" value="" />
    <input type="hidden" id="Lang_MenuHeader_Alert" value="" />
    <input type="hidden" id="Lang_MenuHeaderDesc_Alert" value="" />
    <div class="content_containers">
        <!-- Left Links Section ...starts-->
        <div id="lft_sec_adm" class="lft_sec_adm_pos  hgt_auto">
            <ul>
                <li><a id="LinkManageCategories" href="ManageCategories.aspx" class="adm_lft_nav_seld">
                </a></li>
            </ul>
            <%--            <ul id="LeftcatMenuLink">
            </ul>--%>
        </div>
        <!-- Left Links Section ...ends-->
        <!-- Right Config Data Section ...starts-->
        <div id="rgt_sec_adm">
            <h1>
                <span id="SpanMenuCategoryHeading" ></span>
            </h1>
            <div class="clear">
            </div>
            <h4>
                <span id="Lang_SpanHeaderDesc" ></span>
            </h4>
            <!-- Static Content Area ...starts-->
            <div>
                <div class="flt_lft">
                    <h2 class="pddn_tp_tn" id="Lang_SpanAvalableCat">
                    </h2>
                </div>
              
                <div class="flt_rgt pddn_tp_fftn">
                    <div class="adm_nav_opt menuCat_flt_rit">
                        <ul>
                            <li><a runat="server" style="display:none;" href="javascript:AddNewCategoryPopUp(true);" id="lang_AddNewCategory">
                            </a></li>
                            <li id="PipeAddCategory" style="display:none;">|</li>
                            <li><a runat="server" href="JavaScript:EditCategoryPopUp(true);" id="lang_EditCategory">
                            </a></li>
                            <li id="PipeEditCategory" style="display:none;">|</li>
                            <li><a runat="server" style="display:none;" href="JavaScript:DeleteCategoryPopUp(true);" id="lang_DeleteCategory">
                            </a></li>
                            <li id="PipeMoveUpNdwn"  style="display:none;">|</li>
                            <li id="LiMoveUpAndDown" style="display:none;">
                                <div class="up_dwn_arrw_pos">
                                    <img onclick="MoveUp();" id="ImgUp" src="../../stock/themes/default/images/mov-up.png"
                                        onmouseover="javascript:CallToShowCmsToolTip(this);" onmouseout="javascript:HideToolTip();"
                                        style="cursor: pointer; height: 12px;width: 12px;">
                                    <img onclick="MoveDown();" id="ImgDown" src="../../stock/themes/default/images/mov-dwn.png"
                                        onmouseover="javascript:CallToShowCmsToolTip(this);" onmouseout="javascript:HideToolTip();"
                                        style="cursor: pointer;height: 12px;width: 12px;">
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>

            </div>
             <div class="clear">
            </div>

            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <div id="divContent" runat="server">
                    <!-- Nav Option ...ends-->
                    <div id="divCategoriesList" runat="server" class="DivUsersGrid">
                    </div>
                </div>
            </table>
            <!-- Static Content Area ...ends-->
            <!-- Right Config Data Section ...ends-->
            <div class="clear"> </div>
        </div>
        <!-- Main Container ...ends-->
    </div>
 

    <!-- Nav Option ...starts-->
    <div id="AddNEditCategoryPopup" class="popup_block" style="height: 280px; width: 550px;">
        <!-- Reg Left Section ...starts-->
        <div id="CPDiv1">
            <div id="CPDiv2">
                <h1 id="Lang_HddAdd" style="display: none">
                </h1>
                <h1 id="Lang_HddEdit" style="display: none">
                </h1>
                <p class="confg_frm_txt_lgin">
                    <span id="Lang_SpnMenuCategory"></span><span class="Mandatory">*</span>
                </p>
                <p class="confg_frm_inp_bx">
                    <input tabindex="50" id="txtMenuCategory" type="input" class="confg_frm_inp_bx_txt_qry" /></p>
                <p class="confg_frm_txt_lgin">
                    <span id="Lang_SpnMenuLinkText"></span><span class="Mandatory">*</span></p>
                <p class="confg_frm_inp_bx">
                    <input tabindex="50" id="txtLeftLinkText" type="input" class="confg_frm_inp_bx_txt_qry" /></p>
                <div class="clear">
                </div>
                <p class="confg_frm_txt_lgin">
                    <span id="Lang_SpnMenuHeader"></span><span class="Mandatory">*</span></p>
                <p class="confg_frm_inp_bx">
                    <input tabindex="51" id="txtHeaderText" type="input" class="confg_frm_inp_bx_txt_qry" />
                </p>
                <p class="confg_frm_txt_lgin">
                    <span id="Lang_SpnMenuHeaderDesc"></span></p>
                <p class="confg_frm_inp_bx">
                    <input tabindex="51" id="txtHeaderDescText" type="input" class="confg_frm_inp_bx_txt_qry" /></p>
                <div class="clear">
                </div>
                <!-- Configuration Update Button ...starts-->
                <div class="adm_upd_bttn_lginpg">
                    <input tabindex="52" id="btnEditCategory" type="button" class="di_gui_button di_gui_button_qry"
                        value="" onclick="EditCategory(GetUrlVars()['MC']);" />
                    <input tabindex="52" id="btnAddCategory" type="button" class="di_gui_button di_gui_button_qry"
                        value="" onclick="AddNewCategory(GetUrlVars()['MC']);" />
                </div>
                <!-- Configuration Update Button ...ends-->
            </div>
        </div>
    </div>

    <div id="divCallout" class="callout border-callout callout_main_cont">
        <div id="divCalloutText" class="content">
            <!--Callout text-->
        </div>
        <b class="border-notch notch"></b><b class="notch"></b>
    </div>
    <script type="text/javascript">
        CookiePostfixStr = '_' + '<%=Global.CookiePostfixStr%>';    // use in hosting app
        var VisibleSlidersCount = '<%=Global.VisibleSlidersCount%>';
        var SliderAnimationSpeed = '<%=Global.SliderAnimationSpeed%>';
        var di_components = "Area,Indicator,Qds,Language";
        var di_vctype = "";
        var di_component_version = '<%=Global.diuilib_version%>';
        var di_theme_css = '<%=Global.diuilib_theme_css%>';
        var di_diuilib_url = '<%=Global.diuilib_url%>';
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibincludes.js'" + "><\/script>");
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibcommon.js'" + "><\/script>");
    </script>
    <script type="text/javascript">
        DbAdmName = '<%=DbAdmName%>';
        DbAdmInstitution = '<%=DbAdmInstitution%>';
        DbAdmEmail = '<%=DbAdmEmail%>';

        createFormHiddenInputs("frmManageCategories", "POST");
        SetCommonLinksHref("frmManageCategories", "POST");
        setPostedData('<%=hdsby%>', '<%=hdbnid%>', '<%=hselarea%>', '<%=hselind%>', '<%=hlngcode%>', '<%=hlngcodedb%>', '<%=hselindo%>', '<%=hselareao%>', 10, '<%=hLoggedInUserNId%>', '<%=hLoggedInUserName%>');
        if (GetLanguageCounts() > 1) {
            z("LanguagePipeLi").style.display = "";
            ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', '<%=hlngcode%>');
        }
        PageName = {
            News: "news",
            Innovations: "Innovations",
            FAQ: "Innovations"
        }
        di_jq(document).ready(function () {
            var SelectedPageName = GetUrlVars()['MC'];
            CreateMenuCategoryGrid('');
            di_jq('#LinkManageCategories').attr("href", "ManageCategories.aspx?MC=" + SelectedPageName);
            if (SelectedPageName.toLowerCase() == PageName.News.toLowerCase()) {
                di_jq('#cphMainContent_lang_AddNewCategory').css({ display: "block" });
                di_jq('#PipeAddCategory').css({ display: "block" });
                di_jq('#PipeEditCategory').css({ display: "block" });
                di_jq('#cphMainContent_lang_DeleteCategory').css({ display: "block" });
            }
            else {
                di_jq('#cphMainContent_lang_AddNewCategory').css({ display: "none" });
                di_jq('#PipeAddCategory').css({ display: "none" });
                di_jq('#PipeEditCategory').css({ display: "none" });
                di_jq('#cphMainContent_lang_DeleteCategory').css({ display: "none" });
            }
        });

       
    </script>
</asp:content>
