<%@ Page Title="" Language="C#" MasterPageFile="~/libraries/aspx/HomeMaster.master"
    AutoEventWireup="true" CodeFile="sitemap.aspx.cs" Inherits="libraries_aspx_sitemap" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="Server">
<script type="text/javascript" src="../js/sitemap.js?v=<%=Global.js_version%>"></script>

</asp:Content>
<asp:content id="Content2" contentplaceholderid="cphMainContent" runat="Server">
    <div id="MainContent">
        <div class="content_containers">
            <div id="reg_content_containers">
                <h2>
                    <span id="SiteMapMainHeading"></span>
                </h2>
                <div class="clear" style="padding: 4px">
                </div>
                <h5>
                </h5>
            </div>
            <div>
                <div id="sitemapfooter_container" class="smap_nav">
                    <div class="footer_links" style="width:13.3%">
                        <ul>
                            <li id="slangDevInfo" class="fnt_sz_frten"></li>
                            <li><a id="slangWI_DevInfo" href="AboutDevInfo.aspx?T=ADI&PN=diorg/di_about.html"></a>
                            </li>
                            <li><a id="slangDI7Features" href="DevInfoFeatures.aspx?T=DF&PN=diorg/DevInfoFeatures.html">
                            </a></li>
                            <li><a id="slangDIDownloads" href="DevInfoDownloads.aspx?T=DL7&PN=diorg/di_downloads_7.html">
                            </a></li>
                            <li><a id="slangDITraining" href="DevInfoTraining.aspx?T=TG&PN=diorg/di_training.html">
                            </a></li>
                            <li><a id="slangDIMapLibrary" href="DevInfoMapLibrary.aspx?T=ML&PN=diorg/di_digital_map_library.html">
                            </a></li>
                            <li><a id="slangDIWorldWide" href="DevInfoWorldwide.aspx?T=W1&PN=diorg/worldwide.html">
                            </a></li>
                            <li><a id="slangFAQ" href="../../articles/Faq">
                             <li><a id="slangKB" href="../../articles/KnowledgeBase">
                            <li><a id="slangHowToVideos"  href="../../articles/HowToVideo">
                            </a></li>
                        </ul>
                    </div>
                    <div class="footer_links">
                        <ul>
                            <li id="slangDataSearch" class="fnt_sz_frten">
                                <!--Data Search-->
                            </li>
                            <li><a id="slangByKTGA" href="Home.aspx">
                                <!--By Keywords, Topic, Geographic Area-->
                            </a></li>
                            <li><a id="slangCatalog" href="Catalog.aspx">
                                <!--Catalog-->
                            </a></li>
                            <li><a id="slangdiGallery" href="Gallery.aspx">
                                <!--di Gallery-->
                            </a></li>
                            <li><a href="sRegStructure.aspx" id="slangRegistry">
                                <!--Registry-->
                            </a></li>
                            <li><a href="sRegDataQuery.aspx" id="slangDataQueryService">
                                <!--Data Query Service-->
                            </a></li>
                        </ul>
                    </div>
                    <div class="footer_links" id="sfooterNews" runat="server">
                    </div>
                    <div class="footer_links" id="footerInnovations" runat="server">
                        <ul>
                            <li id="slangInnovation" class="fnt_sz_frten">
                                <!--Innovation-->
                            </li>
                            <li><a id="slangdiMonitoring" href="../../articles/monitoring">
                                <!--di Monitoring-->
                            </a></li>
                            <li><a id="slangdiProfile" href="../../articles/profile">
                                <!--di Profile-->
                            </a></li>
                            <li><a id="slangdiGameworks"target="_blank" href="http://www.digw.org/">
                                <!--di Gameworks-->
                            </a></li>
                            <li><a id="slangdiDashboard" href="../../articles/dashboards">
                                <!--di Dashboard-->
                            </a></li>
                        </ul>
                    </div>
                    <div class="footer_links">
                        <ul>
                            <li id="slangAboutUs" class="fnt_sz_frten">
                                <!--About us-->
                            </li>
                            <li><a id="slangDevInfoTeam" href="ContactUs.aspx?T=C&PN=diorg/di_contacts.html">
                                <!--DevInfo Team-->
                            </a></li>
                            <li><a id="slangSiteMap" href="sitemap.aspx">
                                <!--DevInfo Sitemap-->
                            </a></li>
                        </ul>
                    </div>
                </div>
                <!-- Disclaimer ...ends-->
            </div>
             <div class="clear" style="padding: 8px;margin: 0 0 19px 0;">
            </div>                    
            <h1 style="padding-left: 30px;font-weight:bold">
                <span id="SitemapDataHeader" runat="server"></span> - Data Sitemap
            </h1>
            <div style="width: 889px; border: 0; align: center; padding-left: 30px" id="div_content"
                runat="server">
            </div>
        </div>
    </div>
    <script type="text/javascript">
        CookiePostfixStr = '_' + '<%=Global.CookiePostfixStr%>';    // use in hosting app
        var di_components = "Language";
        var di_component_version = '<%=Global.diuilib_version%>';
        var di_theme_css = '<%=Global.diuilib_theme_css%>';
        var di_diuilib_url = '<%=Global.diuilib_url%>';
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibincludes.js'" + "><\/script>");
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibcommon.js'" + "><\/script>");
    </script>
    <script type="text/javascript">
        createFormHiddenInputs("frmSiteMap", "POST");
        SetCommonLinksHref("frmSiteMap", "POST");
        setPostedData('<%=hdsby%>', '<%=hdbnid%>', '<%=hselarea%>', '<%=hselind%>', '<%=hlngcode%>', '<%=hlngcodedb%>', '<%=hselindo%>', '<%=hselareao%>', 10, '<%=hLoggedInUserNId%>', '<%=hLoggedInUserName%>');
        if (GetLanguageCounts() > 1) {
            z("LanguagePipeLi").style.display = "";
            ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', '<%=hlngcode%>');
        }       
        
    </script>    
</asp:content>
