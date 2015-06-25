<%@ Page Language="C#" MasterPageFile="~/articles/ArticleMaster.master" AutoEventWireup="true"
    CodeFile="ArticleTemplate.aspx.cs" Inherits="articles_ArticleTemplate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="Server">
    <title>
        <%= ATitle%></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="Server">
    <div class="content_containers">
        <div class="flt_lft wdth_ful">
            <!-- Left Links Section ...starts-->
            <div id="lft_sec_adm" class="lft_sec_adm_pos  hgt_auto pddn_tp_fftn">
                <a id="LinkManageCategory" href="../libraries/aspx/ManageCategories.aspx?MC=<%=SelectedPageName %>"
                    class="flt_lft" style="display: none;">
                    <img src="../stock/themes/default/images/ManageCategories.png" /></a>
                <ul id="LeftcatMenuLink">
                </ul>
                <div id="DVTags" runat="server" style="width: 100%; padding-left: 2px; padding-top: 22px;
                    display: none" class="fnt_sz_frten">
                </div>
                <div id="DVTagsMenu" runat="server" style="width: 100%; padding-top: 5px;" class="fnt_sz_twlv ovrflw_Chk_lst">
                </div>
                <div id="Divsubcription" style="width: 100%; padding-top: 15px;">
                    <tr>
                        <td height="40" align="right">
                            <form action="http://devinfo.us2.list-manage.com/subscribe/post?u=dd048496522c675fe306a2b87&id=a24023691d"
                            target="_blank" method="post" id="mc-embedded-subscribe-form2" name="mc-embedded-subscribe-form"
                            class="validate" style="margin-top: 3px; margin: 0; padding: 0;">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td style="width: 270; font-size: 11px; padding-top: 5px; padding-bottom: 5px;" valign="top"
                                        class="flt_lft bodytext">
                                        To subscribe, please enter your email ID here:
                                    </td>
                                </tr>
                                <tr>
                                    <td width="212" height="25" align="left" valign="top" class="news_subc_container">
                                        <input type="text" value="mail@example.com" style="width: 140px; margin-left: 5px;"
                                            onfocus="if(this.value=='mail@example.com')this.value='';" name="EMAIL" id="newsletter_box"
                                            class="required_email2" />
                                    </td>
                                </tr>
                                <tr>
                                    <td height="25" class="flt_lft" align="center" valign="top" style="padding-top: 5px;">
                                        <input type="image" src="../libraries/aspx/diorg/images/newsletter_subscribe.gif"
                                            value="Go!" name="subscribe" id="newsletter_submit" class="btn" />
                                    </td>
                                </tr>
                            </table>
                            </form>
                        </td>
                    </tr>
                </div>
            </div>
            <input type="hidden" id="HdnTagIds" runat="server" value="" />
            <!-- Left Links Section ...ends-->
            <!-- Right Config Data Section ...starts-->
            <div id="rgt_sec_adm" class="CMS_rgt_adm_pos">
                <div id="DIv_Adm_Options" runat="server" class="adm_nav_opt flt_rgt" style="padding: 3px 2.3% 0px 0px !important;
                    display: none;">
                    <ul>
                        <li><a href="../libraries/aspx/AddCMSContents.aspx" id="AddNewNews" runat="server"
                            style="display: none;"><span class="pddn_rt_fv flt_lft">
                                <img src="../stock/themes/default/images/add_news.png" alt="Download PDF" border="0"></span>
                            <span class="flt_lft pddn_rt_et" style="text-decoration: none; color: #404141;">Add
                                New</span> </a></li>
                        <%--<li id="PipeAddNews" runat="server" style="display: none;" class="fnt_sz_twlv">|</li>--%>
                        <li><a href="javascript:void(0);" id="DeleteNews" runat="server" style="display: none;">
                            <span class="pddn_rt_fv flt_lft">
                                <img src="../stock/themes/default/images/delete_news.png" alt="Download PDF" border="0"></span>
                            <span class="flt_lft pddn_rt_et" class="pddn_rt_tn" style="text-decoration: none;
                                color: #404141;">Delete</span> </a></li>
                        <%-- <li id="PipeDeleteNews" runat="server" style="display: none;" class="fnt_sz_twlv">|</li>--%>
                        <li id="Li1" runat="server" style="display: none;" class="fnt_sz_twlv">|</li>
                        <li><a href="javascript:void(0);" id="EditPage" style="display: none;" runat="server">
                            <span class="pddn_rt_fv flt_lft">
                                <img src="../stock/themes/default/images/edit_news.png" alt="Download PDF" border="0"></span>
                            <span class="flt_lft " class="pddn_rt_tn" style="text-decoration: none; color: #404141;">
                                Edit This Page</span> </a></li>
                    </ul>
                </div>
                <h1 class="pddn_tp_fftn flt_lft wdth_ful">
                    <span id="SpanMenuCategoryHeading" runat="server" onmouseover="ShowCommonToolTip(this);"
                        onmouseout="HideToolTip();" style="display:none;"></span>
                </h1>
                <div class="clear">
                </div>
                <h4 class="pddn_bt_svtn">
                    <div style="float: left;">
                        <span id="SpanHeaderDescription" runat="server" onmouseover="ShowCommonToolTip(this);"
                            onmouseout="HideToolTip();" style="display: none;"></span>
                    </div>
                    <div id="DivTopOptions" class="flt_rgt pos_rel" style="display: none; padding-right: 3%;">
                        <span id="SpnPdfContent" style="display: none;"><a id="ArticlePdf" runat="server"
                            style="text-decoration: none; color: #404141;" href="../libraries/aspx/diorg/pdfs/DevInfo_2009_News article_Aug_09_bangladesh_r1.pdf"
                            target="_blank&quot;"><span class="pddn_rt_fv flt_lft">
                                <img src="../stock/themes/default/images/CMS_DownLoad_pdf.jpg" alt="Download PDF"
                                    border="0"></span> <span class="flt_lft pddn_rt_thrtn" class="pddn_rt_tn">Download Pdf</span></a>
                        </span><span><a href="javascript:printSpecialForArticles('width=1,height=1');" style="text-decoration: none;
                            color: #404141;"><span class="pddn_rt_fv flt_lft">
                                <img src="../stock/themes/default/images/CMS_printer.jpg" alt="Print News" border="0"></span>
                            <span class="flt_lft pddn_rt_thrtn" class="pddn_rt_tn">Print</span></a> </span>
                        <span><a id="aShare" href="javascript:void(0);" style="text-decoration: none; color: #404141;">
                            <span class="pddn_rt_fv flt_lft">
                                <img src="../stock/themes/default/images/CMS_Share.jpg" alt="Print News" hspace="6"
                                    border="0"></span> <span class="flt_lft">Share</span></a> </span>
                    </div>
                </h4>
                <!-- Static Content Area ...starts-->
                <div class="sttc_cntnt_news" id="div_content" runat="server">
                    <table width="720" border="0" align="center" cellpadding="0" cellspacing="0">
                        <div id="divContent" runat="server">
                        </div>
                    </table>
                </div>
                <!-- Static Content Area ...ends-->
                <!-- Right Config Data Section ...ends-->
                <div class="clear">
                </div>
            </div>
        </div>
        <div id="divCallout" class="callout border-callout callout_main_cont">
            <div id="divCalloutText" class="content">
                <!--Callout text-->
            </div>
            <b class="border-notch notch"></b><b class="notch"></b>
        </div>
        <!-- Main Container ...ends-->
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
        onPageLoad('<%=hdsby%>', '<%=hdbnid%>', '<%=hselarea%>', '<%=hselind%>', '<%=hlngcode%>', '<%=hlngcodedb%>', '<%=hselindo%>', '<%=hselareao%>', '<%=dbname%>', '<%=dbsummary%>', '<%=dbdesc%>', '<%=DefAreaCount%>', '<%=hLoggedInUserNId%>', '<%=hLoggedInUserName%>', '<%=isGalleryEnabled%>', '<%=hdvnids%>');
    </script>
    <script type="text/javascript">
        di_jq(document).ready(function () {
            var MenuCategory = '<%=MenuCategory %>';
            if (MenuCategory == null || MenuCategory == undefined || MenuCategory == "") {
                MenuCategory = GetMenuCategoryByUrl();
            }
            if (MenuCategory != '') {
                if ('<%=ArticleTags%>' == '' || '<%=ArticleTags%>' == undefined) {
                    GetTagsListByMenuCategory(MenuCategory, '');
                }
                else {
                    GetTagsListByMenuCategory(MenuCategory, '<%=ArticleTags%>');

                }

                CreateLeftLink(MenuCategory, IsArticlesVisible(), '<%=SelectedPageName%>');
            }
            di_jq("#aShare").click(function () {
                var SharingImageUrl = "<%=SharingImageUrl%>";
                var SharingTitle = "<%=SharingTitle%>";
                var SharingSummary = "<%=SharingSummary%>";
               // SharingSummary=SharingSummary.replace(/<br ?\/?>/g, "\n");
                CMSSocialSharing("You should check this out!", " DevInfo", window.location.href, SharingImageUrl, SharingTitle, SharingSummary);
            });
            ManageDefaults();
            ManageHaderContent();
        });
      
    </script>
    <script type="text/javascript">
        di_jq(document).ready(function () {
            var Url = document.URL;
            if (Url.indexOf("articles/") != -1) {
                urlArray = Url.split("articles/");
                Url = urlArray[0] + "libraries/aspx/";
            }
            document.getElementById('<%=EditPage.ClientID%>').href = Url + "EditCmsContents.aspx?" + "PN=" + urlArray[1];
            document.getElementById('<%=AddNewNews.ClientID%>').href = Url + "AddCMSContents.aspx?" + "PC=" + GetArticleMenuCategory();
        });
        function GetArticleMenuCategory() {
            var MenuCategory = '';
            if ('<%=MenuCategory %>' == null && '<%=MenuCategory %>' == undefined) {
                alert('<%=MenuCategory %>');
                MenuCategory = GetMenuCategoryByUrl();
            }
            else {
                MenuCategory = '<%=MenuCategory %>';
            }
            return MenuCategory;
        }
        if (IsArticlesVisible() == "True") {
            di_jq('#LinkManageCategory').css({ display: "block" });
        }
        else {
            di_jq('#LinkManageCategory').css({ display: "none" });
        }
        function IsArticlesVisible() {
            var IsHiddenArticlesVisible1 = '';
            IsHiddenArticlesVisible1 = '<%=IsHiddenArticlesVisible %>';
            return IsHiddenArticlesVisible1;
        }
    </script>
</asp:Content>
