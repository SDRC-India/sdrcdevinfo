<%@ Page Title="" Language="C#" MasterPageFile="~/libraries/aspx/HomeMaster.master"
    AutoEventWireup="true" CodeFile="News.aspx.cs" Inherits="libraries_aspx_News" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="Server">
</asp:Content>
<asp:content id="Content2" contentplaceholderid="cphMainContent" runat="Server">
    <!-- Main Container ...starts-->
    <div class="content_containers">
        <!-- Left Links Section ...starts-->
        <div id="lft_sec_adm" class="lft_sec_adm_pos">
            <ul>
                <li><a href="news.aspx?T=N&PN=diorg/di_news.html" id="aNews" runat="server">
                    <%--News--%></a></li>
                <li><a href="news.aspx?T=A&PN=diorg/devinfo_in_action.html" id="aDIA" runat="server">
                    <%--DevInfo in Actiom--%></a></li>
                <li><a href="news.aspx?T=F&PN=diorg/di_facts.html" id="aFOW" runat="server">
                    <%--Fact of the Week--%></a></li>
            </ul>
        </div>
        <!-- Left Links Section ...ends-->
        <!-- Right Config Data Section ...starts-->
        <div id="rgt_sec_adm" class="rgt_sec_adm_pos">
            <h1>
                News
                <div class="adm_nav_opt pddn_nne flt_rgt">
                    <ul>
                        <li><a href="javascript:void(0);" id="AddNewNews" runat="server" style="display: none;">
                            Add New</a></li>
                        <li id="PipeAddEditNews" runat="server" style="display: none;" class="fnt_sz_twlv">|</li>
                        <li><a href="javascript:void(0);" id="EditPage" runat="server" style="display: none;">
                            Edit This Page</a></li>
                    </ul>
                </div>
            </h1>
            <div class="clear">
            </div>
            <h4>
                Latest database releases and important announcements. Stories from the field highlighting
                use of DevInfo. Facts relating to the Millennium Development Goals</h4>
            <!-- Static Content Area ...starts-->
            <div class="sttc_cntnt_main" id="div_content" runat="server">
                <table width="720" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="top">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td valign="top">
                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td height="20" colspan="3">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="605" valign="top">
                                                    <table width="599" border="0" align="left" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td height="29">
                                                                <table width="598" border="0" align="right" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td width="10" height="29" valign="top">
                                                                            <img src="images/left_top1.gif" width="10" height="29" border="0" />
                                                                        </td>
                                                                        <td width="578" height="29" background="images/top_bg.gif" class="mainheadings_center">
                                                                            News
                                                                        </td>
                                                                        <td width="10" height="29" valign="top">
                                                                            <img src="images/right_top1.gif" width="10" height="29" border="0" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td height="15" valign="top" background="images/middle_bgcenter.gif">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td height="450" valign="top" background="images/middle_bgcenter.gif">
                                                                <table width="94%" border="0" align="center" cellpadding="0" cellspacing="0" class="bodytext">
                                                                    <tr>
                                                                        <td class="news_space">
                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td width="29%" align="center" valign="top">
                                                                                        <a href="news_content/Mexico_children_rights_data_dashboard.html">
                                                                                            <img src="images/news_images/mexicodash_small.jpg" width="140" height="85" border="0"
                                                                                                class="reflect ropacity30" /></a>
                                                                                    </td>
                                                                                    <td width="71%" valign="top">
                                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                                            <tr>
                                                                                                <td valign="top">
                                                                                                    <p>
                                                                                                        <strong><a href="news_content/Mexico_children_rights_data_dashboard.html">Infoniñez.mx
                                                                                                            Interactive Dashboard Released Online</a></strong><span class="adaptation_italic"><br />
                                                                                                                (27 September 2012</span>)</p>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    Keeping track of the latest data on key children&rsquo;s rights in Mexico is now
                                                                                                    possible through a new interactive dashboard launching today. The tool has several
                                                                                                    panels, all downloadable and user-friendly, and is updated in near real-time so
                                                                                                    that regular visitors can regularly learn something new on the site.
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                        <tr>
                                                                                                            <td width="90%" height="20" align="right">
                                                                                                                <a href="news_content/Mexico_children_rights_data_dashboard.html">more</a>
                                                                                                            </td>
                                                                                                            <td width="10%" height="20" align="left">
                                                                                                                &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td bgcolor="#CCCCCC" height="1">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="news_space">
                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td width="29%" align="center" valign="top">
                                                                                        <a href="news_content/a_promise_renewed_data_dashboard.html">
                                                                                            <img src="images/news_images/renew_dashsmall.jpg" width="140" height="85" border="0"
                                                                                                class="reflect ropacity30" /></a>
                                                                                    </td>
                                                                                    <td width="71%" valign="top">
                                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                                            <tr>
                                                                                                <td valign="top">
                                                                                                    <p>
                                                                                                        <strong><a href="news_content/a_promise_renewed_data_dashboard.html">A Promise Renewed
                                                                                                            Dashboard Launched</a></strong><span class="adaptation_italic"> (17 September 2012</span>)</p>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    The technology and know-how are available to scale up child survival commitments
                                                                                                    in an effort to end all preventable child deaths. With<br />
                                                                                                    <a href="http://www.apromiserenewed.org/" target="_blank" class="nav"><em>A Promise
                                                                                                        Renewed</em></a>,<em>&nbsp;</em>the nations of the world have renewed a promise
                                                                                                    to give all children a fair opportunity to survive and thrive.
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                        <tr>
                                                                                                            <td width="90%" height="20" align="right">
                                                                                                                <a href="news_content/a_promise_renewed_data_dashboard.html">more</a>
                                                                                                            </td>
                                                                                                            <td width="10%" height="20" align="left">
                                                                                                                &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td bgcolor="#CCCCCC" height="1">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="news_space">
                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td width="29%" align="center" valign="top">
                                                                                        <a href="news_content/Libya_country_statistics.html">
                                                                                            <img src="images/news_images/libya_small.jpg" width="140" height="85" border="0"
                                                                                                class="reflect ropacity30" /></a>
                                                                                    </td>
                                                                                    <td width="71%" valign="top">
                                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                                            <tr>
                                                                                                <td valign="top">
                                                                                                    <p>
                                                                                                        <strong><a href="news_content/Libya_country_statistics.html">Libya: Drawing on Dubai
                                                                                                            Experience to Build a New Libya</a></strong><span class="adaptation_italic"> (5 September
                                                                                                                2012</span>)</p>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    The liberation of Libya was celebrated in October 2011, promising a new era of freedom
                                                                                                    and transparency in that country. As its new leaders looked for ways to facilitate
                                                                                                    open access to national information, they decided to learn from the experiences
                                                                                                    of other countries in the region.
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                        <tr>
                                                                                                            <td width="90%" height="20" align="right">
                                                                                                                <a href="news_content/Libya_country_statistics.html">more</a>
                                                                                                            </td>
                                                                                                            <td width="10%" height="20" align="left">
                                                                                                                &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td bgcolor="#CCCCCC" height="1">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="news_space">
                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td width="29%" align="center" valign="top">
                                                                                        <a href="news_content/Burundi_national_database_system_updated.html">
                                                                                            <img src="images/news_images/burundi_small.jpg" width="140" height="85" border="0"
                                                                                                class="reflect ropacity30" /></a>
                                                                                    </td>
                                                                                    <td width="71%" valign="top">
                                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                                            <tr>
                                                                                                <td valign="top">
                                                                                                    <p>
                                                                                                        <strong><a href="news_content/Burundi_national_database_system_updated.html">On the
                                                                                                            Eve of Independence Anniversary, Burundi Releases Latest Version of National Database
                                                                                                            System</a></strong><span class="adaptation_italic"> (22 August 2012</span>)</p>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    While July 2nd was a normal working day for most people around the globe, Burundians
                                                                                                    awoke to the sound of celebrations in honour of their country&rsquo;s fiftieth independence
                                                                                                    anniversary. The occasion, which drew a crowd of thousands in the capital, Bujumbura,
                                                                                                    served both as a way to commemorate the nation&rsquo;s rich history and as an opportunity
                                                                                                    to look to the future.
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                        <tr>
                                                                                                            <td width="90%" height="20" align="right">
                                                                                                                <a href="news_content/Burundi_national_database_system_updated.html">more</a>
                                                                                                            </td>
                                                                                                            <td width="10%" height="20" align="left">
                                                                                                                &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td bgcolor="#CCCCCC" height="1">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="news_space">
                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td width="29%" align="center" valign="top">
                                                                                        <a href="news_content/Jamaica_child_protection_database.html">
                                                                                            <img src="images/news_images/jamica_small.jpg" width="140" height="85" border="0"
                                                                                                class="reflect ropacity30" /></a>
                                                                                    </td>
                                                                                    <td width="71%" valign="top">
                                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                                            <tr>
                                                                                                <td valign="top">
                                                                                                    <p>
                                                                                                        <strong><a href="news_content/Jamaica_child_protection_database.html">Jamaica Launches
                                                                                                            Online Child Protection Database</a></strong><span class="adaptation_italic"> (18 July
                                                                                                                2012</span>)</p>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    The Government of Jamaica, with support from <a href="http://www.unicef.org" target="_blank"
                                                                                                        class="nav">UNICEF</a>, recently launched Jamaica's Child Protection Database
                                                                                                    (CPD) as a monitoring tool for the government and the Jamaican public to measure
                                                                                                    the country&rsquo;s performance on national child-related plans of action as well
                                                                                                    as international agreements such as the <a href="http://www.unicef.org/crc/" target="_blank"
                                                                                                        class="nav">UN Convention on the Rights of the Child</a>.
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                        <tr>
                                                                                                            <td width="90%" height="20" align="right">
                                                                                                                <a href="news_content/Jamaica_child_protection_database.html">more</a>
                                                                                                            </td>
                                                                                                            <td width="10%" height="20" align="left">
                                                                                                                &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td bgcolor="#CCCCCC" height="1">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="news_space">
                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td width="29%" align="center" valign="top">
                                                                                        <a href="news_content/Serbia_development_data_portal.html">
                                                                                            <img src="images/news_images/serb_small.jpg" width="140" height="84" border="0" class="reflect ropacity30" /></a>
                                                                                    </td>
                                                                                    <td width="71%" valign="top">
                                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                                            <tr>
                                                                                                <td valign="top">
                                                                                                    <p>
                                                                                                        <strong><a href="news_content/Serbia_development_data_portal.html">Serbia Releases Updated
                                                                                                            DevInfo Data Portal and Online Databases</a></strong><span class="adaptation_italic">
                                                                                                                (18 July 2012</span>)</p>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    In early 2012, the <a href="http://webrzs.stat.gov.rs/WebSite/" target="_blank" class="nav">
                                                                                                        Statistical Office of the Republic of Serbia</a> released two major new databases
                                                                                                    and updated its web data portal to DevInfo 6.0 format.
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                        <tr>
                                                                                                            <td width="90%" height="20" align="right">
                                                                                                                <a href="news_content/Serbia_development_data_portal.html">more</a>
                                                                                                            </td>
                                                                                                            <td width="10%" height="20" align="left">
                                                                                                                &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td bgcolor="#CCCCCC" height="1">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="news_space">
                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td width="29%" align="center" valign="top">
                                                                                        <a href="news_content/Iraq_rural_urban_infrastructure_data.html">
                                                                                            <img src="images/news_images/iraq_census_s.jpg" width="140" height="85" border="0"
                                                                                                class="reflect ropacity30" /></a>
                                                                                    </td>
                                                                                    <td width="71%" valign="top">
                                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                                            <tr>
                                                                                                <td valign="top">
                                                                                                    <p>
                                                                                                        <strong><a href="news_content/Iraq_rural_urban_infrastructure_data.html">Iraq: Using
                                                                                                            CensusInfo to Disseminate Information on Infrastructure</a></strong><span class="adaptation_italic">
                                                                                                                (19 June 2012</span>)</p>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    On June 4th 2012, the <a href="http://cosit.gov.iq/english/" target="_blank" class="nav">
                                                                                                        Iraqi Central Statistical Organization</a> (CSO), with the help of the <a href="http://www.unfpa.org"
                                                                                                            target="_blank" class="nav">United Nations Population Fund</a> (UNFPA),
                                                                                                    launched a database system named CensusInfo Iraq that displays rural and urban statistics
                                                                                                    on infrastructure.
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                        <tr>
                                                                                                            <td width="90%" height="20" align="right">
                                                                                                                <a href="news_content/Iraq_rural_urban_infrastructure_data.html">more</a>
                                                                                                            </td>
                                                                                                            <td width="10%" height="20" align="left">
                                                                                                                &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td bgcolor="#CCCCCC" height="1">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="news_space">
                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td width="29%" align="center" valign="top">
                                                                                        <a href="news_content/Malawi_Population_Housing_Data_CensusInfo.html">
                                                                                            <img src="images/news_images/malawi_cen_s.jpg" width="140" height="85" border="0"
                                                                                                class="reflect ropacity30" /></a>
                                                                                    </td>
                                                                                    <td width="71%" valign="top">
                                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                                            <tr>
                                                                                                <td valign="top">
                                                                                                    <p>
                                                                                                        <strong><a href="news_content/Malawi_Population_Housing_Data_CensusInfo.html">“Making
                                                                                                            the Information Come Alive”: Malawi Disseminates National Population and Housing
                                                                                                            Data Using CensusInfo </a></strong><span class="adaptation_italic">(19 June 2012</span>)</p>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    The landlocked African nation of Malawi experienced enormous change between 1998
                                                                                                    and 2008 as the population increased by no less than 32% (most noticeably in Northern
                                                                                                    and Central districts) and became considerably more urban.
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                        <tr>
                                                                                                            <td width="90%" height="20" align="right">
                                                                                                                <a href="news_content/Malawi_Population_Housing_Data_CensusInfo.html">more</a>
                                                                                                            </td>
                                                                                                            <td width="10%" height="20" align="left">
                                                                                                                &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td bgcolor="#CCCCCC" height="1">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="news_space">
                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td width="29%" align="center" valign="top">
                                                                                        <a href="news_content/Algeria_National_Development_Database.html">
                                                                                            <img src="images/news_images/socinfo_small.jpg" width="140" height="84" border="0"
                                                                                                class="reflect ropacity30" /></a>
                                                                                    </td>
                                                                                    <td width="71%" valign="top">
                                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                                            <tr>
                                                                                                <td valign="top">
                                                                                                    <p>
                                                                                                        <strong><a href="news_content/Algeria_National_Development_Database.html">Algeria Launches
                                                                                                            National Searchable Database on Human Development </a></strong><span class="adaptation_italic">
                                                                                                                (07 May 2012</span>)</p>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    For the first time in Algerian history, the Ministry of Statistics is making available
                                                                                                    to anyone with an Internet connection a national searchable socio-economic database
                                                                                                    covering various key sectors, including health, education, economy, environment
                                                                                                    and gender.
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                        <tr>
                                                                                                            <td width="90%" height="20" align="right">
                                                                                                                <a href="news_content/Algeria_National_Development_Database.html">more</a>
                                                                                                            </td>
                                                                                                            <td width="10%" height="20" align="left">
                                                                                                                &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td bgcolor="#CCCCCC" height="1">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="news_space">
                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td width="29%" align="center" valign="top">
                                                                                        <a href="news_content/Niger_national_database_nis.html">
                                                                                            <img src="images/news_images/nigerinfo_s.jpg" width="140" height="84" border="0"
                                                                                                class="reflect ropacity30" /></a>
                                                                                    </td>
                                                                                    <td width="71%" valign="top">
                                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                                            <tr>
                                                                                                <td valign="top">
                                                                                                    <p>
                                                                                                        <strong><a href="news_content/Niger_national_database_nis.html">Niger Releases Updated
                                                                                                            National Socio-economic Database
                                                                                                            <br />
                                                                                                        </a></strong><span class="adaptation_italic">(03 April 2012</span>)</p>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    The National Institute of Statistics (INS) of Niger has made available the second
                                                                                                    release of the <a href="http://www.nigerinfo.ne/" target="_blank" class="nav">NigerInfo</a>
                                                                                                    database, which contains national and sub-national data for over 300 socio-economic
                                                                                                    indicators.
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                        <tr>
                                                                                                            <td width="90%" height="20" align="right">
                                                                                                                <a href="news_content/Niger_national_database_nis.html">more</a>
                                                                                                            </td>
                                                                                                            <td width="10%" height="20" align="left">
                                                                                                                &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td bgcolor="#CCCCCC" height="1">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="news_space">
                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td width="29%" align="center" valign="top">
                                                                                        <a href="news_content/Educational_database_EFA_Philippines.html">
                                                                                            <img src="images/news_images/efa_philippines_s.jpg" width="140" height="83" border="0"
                                                                                                class="reflect ropacity30" /></a>
                                                                                    </td>
                                                                                    <td width="71%" valign="top">
                                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                                            <tr>
                                                                                                <td valign="top">
                                                                                                    <p>
                                                                                                        <strong><a href="news_content/Educational_database_EFA_Philippines.html">Philippines
                                                                                                            Launches Online Educational Database </a></strong><span class="adaptation_italic">(02
                                                                                                                April 2012</span>)</p>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    The Department of Education of the Republic of the Philippines has launched <a href="http://devinfo.info/EFA_Philippines/"
                                                                                                        target="_blank" class="nav">EFA Philippines info</a>, an online database containing
                                                                                                    a comprehensive set of education indicators for the Philippines.
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                        <tr>
                                                                                                            <td width="90%" height="20" align="right">
                                                                                                                <a href="news_content/Educational_database_EFA_Philippines.html">more</a>
                                                                                                            </td>
                                                                                                            <td width="10%" height="20" align="left">
                                                                                                                &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td bgcolor="#CCCCCC" height="1">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="news_space">
                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td width="29%" align="center" valign="top">
                                                                                        <a href="news_content/MDG_2011_data_updated.html">
                                                                                            <img src="images/news_images/mdg_2011_s.jpg" width="140" height="84" border="0" class="reflect ropacity30" /></a>
                                                                                    </td>
                                                                                    <td width="71%" valign="top">
                                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                                            <tr>
                                                                                                <td valign="top">
                                                                                                    <p>
                                                                                                        <strong><a href="news_content/MDG_2011_data_updated.html">Updated 2011 MDG Dataset Now
                                                                                                            Available Online </a></strong><span class="adaptation_italic">(30 March 2012</span>)</p>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    The United Nations Statistics Division (UNSD) has released an updated dataset on
                                                                                                    Millennium Development Goals (MDGs) for all member states.
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                        <tr>
                                                                                                            <td width="90%" height="20" align="right">
                                                                                                                <a href="news_content/MDG_2011_data_updated.html">more</a>
                                                                                                            </td>
                                                                                                            <td width="10%" height="20" align="left">
                                                                                                                &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td bgcolor="#CCCCCC" height="1">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="news_space">
                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td width="29%" align="center" valign="top">
                                                                                        <a href="news_content/400_Devinfo_adaptations.html">
                                                                                            <img src="images/news_images/adaptation_small.jpg" width="140" height="85" border="0"
                                                                                                class="reflect ropacity30" /></a>
                                                                                    </td>
                                                                                    <td width="71%" valign="top">
                                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                                            <tr>
                                                                                                <td valign="top">
                                                                                                    <p>
                                                                                                        <strong><a href="news_content/400_Devinfo_adaptations.html">Number of Documented DevInfo
                                                                                                            Adaptations Exceeds 400<br />
                                                                                                        </a></strong><span class="adaptation_italic">(30 March 2012</span>)</p>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    The number of documented DevInfo adaptations across the globe has now <a href="http://www.devinfo.org/di_adaptations.html"
                                                                                                        target="_blank" class="nav">exceeded 400</a>, according to data maintained by
                                                                                                    the DevInfo Support Group. This number includes global, regional, national and sub-national
                                                                                                    adaptations, spanning 124 countries and 14 regions.
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                        <tr>
                                                                                                            <td width="90%" height="20" align="right">
                                                                                                                <a href="news_content/400_Devinfo_adaptations.html">more</a>
                                                                                                            </td>
                                                                                                            <td width="10%" height="20" align="left">
                                                                                                                &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td bgcolor="#CCCCCC" height="1">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="news_space">
                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td width="29%" align="center" valign="top">
                                                                                        <a href="news_content/using_DevInfo_to_monitor_equity.html">
                                                                                            <img src="images/news_images/wsscc_elearning_s.jpg" width="140" height="85" border="0"
                                                                                                class="reflect ropacity30" /></a>
                                                                                    </td>
                                                                                    <td width="71%" valign="top">
                                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                                            <tr>
                                                                                                <td valign="top">
                                                                                                    <p>
                                                                                                        <strong><a href="news_content/using_DevInfo_to_monitor_equity.html">New E-learning Course
                                                                                                            Available on Using DevInfo to Monitor Equity<br />
                                                                                                        </a></strong><span class="adaptation_italic">(28 March 2012</span>)</p>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    The <a href="http://www.wsscc.org" target="_blank" class="nav">Water Supply &amp; Sanitation
                                                                                                        Collaborative Council (WSSCC)</a>, in collaboration with the DevInfo Support
                                                                                                    Group, has launched a new e-learning course titled &ldquo;<a href="http://www.devinfo.org/E_Learning/WSSCC/player.html"
                                                                                                        target="_blank" class="nav">Using DevInfo to Monitor Equity</a>.&rdquo;
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                        <tr>
                                                                                                            <td width="90%" height="20" align="right">
                                                                                                                <a href="news_content/using_DevInfo_to_monitor_equity.html">more</a>
                                                                                                            </td>
                                                                                                            <td width="10%" height="20" align="left">
                                                                                                                &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td bgcolor="#CCCCCC" height="1">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="news_space">
                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td width="29%" align="center" valign="top">
                                                                                        <a href="news_content/MICS_data_released_Viet_nam.html">
                                                                                            <img src="images/news_images/mics_small2.jpg" width="140" height="84" border="0"
                                                                                                class="reflect ropacity30" /></a>
                                                                                    </td>
                                                                                    <td width="71%" valign="top">
                                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                                            <tr>
                                                                                                <td valign="top">
                                                                                                    <p>
                                                                                                        <strong><a href="news_content/MICS_data_released_Viet_nam.html">Viet Nam Releases Latest
                                                                                                            MICS Data in DevInfo Format<br />
                                                                                                        </a></strong><span class="adaptation_italic">(28 March 2012</span>)</p>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    The General Statistics Office of Viet Nam has released the datasets for the 2006
                                                                                                    and 2011 Multiple Indicator Cluster Survey (MICS) rounds conducted in Viet Nam,
                                                                                                    in online <a href="http://www.devinfo.org" target="_blank" class="nav">DevInfo</a>
                                                                                                    database format.
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                        <tr>
                                                                                                            <td width="90%" height="20" align="right">
                                                                                                                <a href="news_content/MICS_data_released_Viet_nam.html">more</a>
                                                                                                            </td>
                                                                                                            <td width="10%" height="20" align="left">
                                                                                                                &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td bgcolor="#CCCCCC" height="1">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="news_space">
                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td width="29%" align="center" valign="top">
                                                                                        <a href="news_content/gender_equality_monitoring_database.html">
                                                                                            <img src="images/news_images/unwomen_small.jpg" width="140" height="85" border="0"
                                                                                                class="reflect ropacity30" /></a>
                                                                                    </td>
                                                                                    <td width="71%" valign="top">
                                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                                            <tr>
                                                                                                <td valign="top">
                                                                                                    <p>
                                                                                                        <strong><a href="news_content/gender_equality_monitoring_database.html">UN Women CARO
                                                                                                            to Implement DevInfo Technology to Support Monitoring Efforts</a></strong><span class="adaptation_italic">
                                                                                                                (15 March 2012</span>)</p>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    The UN Women Central Africa Regional Office (CARO) is actively moving forward in
                                                                                                    2012 to implement DevInfo technology in support of its monitoring and evaluation
                                                                                                    objectives and to help track progress towards meeting agency goals and targets.
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                        <tr>
                                                                                                            <td width="90%" height="20" align="right">
                                                                                                                <a href="news_content/gender_equality_monitoring_database.html">more</a>
                                                                                                            </td>
                                                                                                            <td width="10%" height="20" align="left">
                                                                                                                &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td bgcolor="#CCCCCC" height="1">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="news_space">
                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td width="29%" align="center" valign="top">
                                                                                        <a href="news_content/state_of_the_worlds_children_2012.html">
                                                                                            <img src="images/news_images/sowc_2012_small.jpg" width="140" height="85" border="0"
                                                                                                class="reflect ropacity30" /></a>
                                                                                    </td>
                                                                                    <td width="71%" valign="top">
                                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                                            <tr>
                                                                                                <td valign="top">
                                                                                                    <p>
                                                                                                        <strong><a href="news_content/state_of_the_worlds_children_2012.html">UNICEF’s State
                                                                                                            of the World’s Children 2012 Database Launched</a></strong><span class="adaptation_italic">
                                                                                                                <br />
                                                                                                                (14 March 2012</span>)</p>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    UNICEF has extended release of the <a href="http://www.unicef.org/sowc2012/" target="_blank"
                                                                                                        class="nav"><em>State of the World&rsquo;s Children (SOWC) 2012</em></a> Report
                                                                                                    – the agency&rsquo;s flagship report on the status of women and children across
                                                                                                    the globe – through the release of the full data set in DevInfo database format.
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                        <tr>
                                                                                                            <td width="90%" height="20" align="right">
                                                                                                                <a href="news_content/state_of_the_worlds_children_2012.html">more</a>
                                                                                                            </td>
                                                                                                            <td width="10%" height="20" align="left">
                                                                                                                &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td bgcolor="#CCCCCC" height="1">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="news_space">
                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td width="29%" align="center" valign="top">
                                                                                        <a href="news_content/Rwanda_national_database.html">
                                                                                            <img src="images/news_images/rwanda_devinfo_small.jpg" width="140" height="85" border="0"
                                                                                                class="reflect ropacity30" /></a>
                                                                                    </td>
                                                                                    <td width="71%" valign="top">
                                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                                            <tr>
                                                                                                <td valign="top">
                                                                                                    <p>
                                                                                                        <strong><a href="news_content/Rwanda_national_database.html">Rwanda Releases Updated
                                                                                                            National Socio-economic Database </a></strong><span class="adaptation_italic">
                                                                                                                <br />
                                                                                                                (13 March 2012</span>)</p>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    The National Institute of Statistics Rwanda has recently released an updated version
                                                                                                    of the DevInfoRwanda database, which is an adaptation of the <a href="http://www.devinfo.org"
                                                                                                        target="_blank" class="nav">DevInfo</a> database system endorsed by the United
                                                                                                    Nations to monitor achievement of the Millennium Development Goals and other national
                                                                                                    priorities.
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                        <tr>
                                                                                                            <td width="90%" height="20" align="right">
                                                                                                                <a href="news_content/Rwanda_national_database.html">more</a>
                                                                                                            </td>
                                                                                                            <td width="10%" height="20" align="left">
                                                                                                                &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td bgcolor="#CCCCCC" height="1">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="news_space">
                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td width="29%" align="center" valign="top">
                                                                                        <a href="news_content/Emergency_tool_rapid_data_capture.html">
                                                                                            <img src="images/news_images/EmergencyInfo_2012_small.jpg" width="140" height="85"
                                                                                                border="0" class="reflect ropacity30" /></a>
                                                                                    </td>
                                                                                    <td width="71%" valign="top">
                                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                                            <tr>
                                                                                                <td valign="top">
                                                                                                    <p>
                                                                                                        <strong><a href="news_content/Emergency_tool_rapid_data_capture.html">EmergencyInfo
                                                                                                            Available to Support Rapid Data Capture </a></strong><span class="adaptation_italic">
                                                                                                                (28 February 2012</span>)</p>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <a href="http://www.devinfo.info/emergencyinfo/" target="_blank" class="nav">EmergencyInfo</a>
                                                                                                    is a powerful tool for rapid data capture used to support data collection efforts
                                                                                                    during emergency situations. Built on <a href="http://www.devinfo.org/" target="_blank"
                                                                                                        class="nav">DevInfo</a> database technology, the EmergencyInfo suite of tools
                                                                                                    helps bridge information gaps following emergencies by enabling rapid data collection,
                                                                                                    monitoring, and quick preparation of situation reports.
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                        <tr>
                                                                                                            <td width="90%" height="20" align="right">
                                                                                                                <a href="news_content/Emergency_tool_rapid_data_capture.html">more</a>
                                                                                                            </td>
                                                                                                            <td width="10%" height="20" align="left">
                                                                                                                &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td bgcolor="#CCCCCC" height="1">
                                                                        </td>
                                                                    </tr>
                                                                    <td class="news_space">
                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td width="29%" align="center" valign="top">
                                                                                    <a href="news_content/DevInfo_online_data_dashboards.html">
                                                                                        <img src="images/news_images/dashboards_small.jpg" width="140" height="85" border="0"
                                                                                            class="reflect ropacity30" /></a>
                                                                                </td>
                                                                                <td width="71%" valign="top">
                                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                                        <tr>
                                                                                            <td valign="top">
                                                                                                <p>
                                                                                                    <strong><a href="news_content/DevInfo_online_data_dashboards.html">DevInfo Dashboards
                                                                                                        Now Available in Web Gallery </a></strong><span class="adaptation_italic">(28 February
                                                                                                            2012</span>)</p>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                The Global DevInfo Initiative has made available a collection of <a href="http://www.devinfo.org/di_dashboards.html"
                                                                                                    target="_blank" class="nav">DevInfo dashboards</a> for exploration, research,
                                                                                                analysis and decision-making in a new <a href="http://www.devinfo.org/di_dashboards.html"
                                                                                                    target="_blank" class="nav">web gallery</a>. DevInfo dashboards enable complex
                                                                                                data sets to be presented in a standard, focused format, making large volumes of
                                                                                                data easier to understand, interpret and act on.
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                    <tr>
                                                                                                        <td width="90%" height="20" align="right">
                                                                                                            <a href="news_content/DevInfo_online_data_dashboards.html">more</a>
                                                                                                        </td>
                                                                                                        <td width="10%" height="20" align="left">
                                                                                                            &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                        </tr>
                                                        <tr>
                                                            <td bgcolor="#CCCCCC" height="1">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="news_space">
                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td width="29%" align="center" valign="top">
                                                                            <a href="news_content/call_for_devInfo_7_beta_testers.html">
                                                                                <img src="images/news_images/di7_beta_small.jpg" width="140" height="85" border="0"
                                                                                    class="reflect ropacity30" /></a>
                                                                        </td>
                                                                        <td width="71%" valign="top">
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                                <tr>
                                                                                    <td valign="top">
                                                                                        <p>
                                                                                            <strong><a href="news_content/call_for_devInfo_7_beta_testers.html">DevInfo Initiative
                                                                                                Announces Call for DevInfo 7.0 Beta Testers<br />
                                                                                            </a></strong><span class="adaptation_italic">(17 February 2012</span>)</p>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        The Global DevInfo Initiative remains on track to launch a new version of the DevInfo
                                                                                        database system – DevInfo 7.0 – in mid-2012. DevInfo 7.0 will be a cross-browser
                                                                                        compatible web application harnessing the latest advancements in information technology,
                                                                                        Internet connectivity, and data visualization.
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                            <tr>
                                                                                                <td width="90%" height="20" align="right">
                                                                                                    <a href="news_content/call_for_devInfo_7_beta_testers.html">more</a>
                                                                                                </td>
                                                                                                <td width="10%" height="20" align="left">
                                                                                                    &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td bgcolor="#CCCCCC" height="1">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="news_space">
                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td width="29%" align="center" valign="top">
                                                                            <a href="news_content/devinfo7_announcement.html">
                                                                                <img src="images/news_images/di7_announcement_small.jpg" width="140" height="85"
                                                                                    border="0" class="reflect ropacity30" /></a>
                                                                        </td>
                                                                        <td width="71%" valign="top">
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                                <tr>
                                                                                    <td valign="top">
                                                                                        <p>
                                                                                            <strong><a href="news_content/devinfo7_announcement.html">DevInfo Webinar 2012: Preview
                                                                                                of DevInfo 7.0 </a></strong><span class="adaptation_italic">(02 February 2012</span>)</p>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        On Tuesday, 7 February 2012 the DevInfo initiative is hosting a live webinar to
                                                                                        update viewers on the latest version of DevInfo and the year ahead.&nbsp;<strong>DevInfo
                                                                                            7.0</strong> harnesses the latest advancements in <strong>information technology</strong>,
                                                                                        <strong>data visualization, statistical standards</strong> and <strong>Internet connectivity</strong>.
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                            <tr>
                                                                                                <td width="90%" height="20" align="right">
                                                                                                    <a href="news_content/devinfo7_announcement.html">more</a>
                                                                                                </td>
                                                                                                <td width="10%" height="20" align="left">
                                                                                                    &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td bgcolor="#CCCCCC" height="1">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="news_space">
                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td width="29%" align="center" valign="top">
                                                                            <a href="news_content/Download_national_statistics_databases.html">
                                                                                <img src="images/news_images/download_databases_js.jpg" width="140" height="85" border="0"
                                                                                    class="reflect ropacity30" /></a>
                                                                        </td>
                                                                        <td width="71%" valign="top">
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                                <tr>
                                                                                    <td valign="top">
                                                                                        <p>
                                                                                            <strong><a href="news_content/Download_national_statistics_databases.html">Global, Regional
                                                                                                and National Databases Now Available for Desktop Download </a></strong><span class="adaptation_italic">
                                                                                                    (31 January 2012</span>)</p>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        The UN DevInfo Initiative has recently made available over 50 global, regional and
                                                                                        national datasets for desktop download from its official website, <a href="http://www.devinfo.org"
                                                                                            target="_blank" class="nav">www.devinfo.org</a>.
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                            <tr>
                                                                                                <td width="90%" height="20" align="right">
                                                                                                    <a href="news_content/Download_national_statistics_databases.html">more</a>
                                                                                                </td>
                                                                                                <td width="10%" height="20" align="left">
                                                                                                    &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td bgcolor="#CCCCCC" height="1">
                                                            </td>
                                                        </tr>
                                                        <td class="news_space">
                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td width="29%" align="center" valign="top">
                                                                        <a href="news_content/india_releases_latest_national_Socio_economic_database.html">
                                                                            <img src="images/news_images/devinfo_india_small.jpg" width="140" height="84" border="0"
                                                                                class="reflect ropacity30" /></a>
                                                                    </td>
                                                                    <td width="71%" valign="top">
                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                            <tr>
                                                                                <td valign="top">
                                                                                    <p>
                                                                                        <strong><a href="news_content/india_releases_latest_national_Socio_economic_database.html">
                                                                                            India Releases Latest National Socio-Economic Database<br />
                                                                                        </a></strong><span class="adaptation_italic">(06 January 2012</span>)</p>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    The Government of India&rsquo;s Ministry of Statistics and Programme Implementation
                                                                                    (MOSPI) officially released the latest version of India&rsquo;s national socio-economic
                                                                                    database – <a href="http://devinfo.info/devinfoindia/" target="_blank" class="nav">DevInfo
                                                                                        India 3.0</a> – on 27 December 2011.
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                        <tr>
                                                                                            <td width="90%" height="20" align="right">
                                                                                                <a href="news_content/india_releases_latest_national_Socio_economic_database.html">more</a>
                                                                                            </td>
                                                                                            <td width="10%" height="20" align="left">
                                                                                                &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#CCCCCC" height="1">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="news_space">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="29%" align="center" valign="top">
                                                                <a href="news_content/Dominican_Republic_launches_CONANIInfo.html">
                                                                    <img src="images/news_images/conaniinfo_1js.jpg" width="140" height="85" border="0"
                                                                        class="reflect ropacity30" /></a>
                                                            </td>
                                                            <td width="71%" valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <p>
                                                                                <strong><a href="news_content/Dominican_Republic_launches_CONANIInfo.html">Dominican
                                                                                    Republic Launches New Database to Support Policy Making for Children and Adolescents
                                                                                </a></strong><span class="adaptation_italic">(22 December 2011</span>)</p>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            The National Council for Childhood and Adolescence (CONANI) in the Dominican Republic
                                                                            has recently launched a new database – <a href="http://conani.gov.do/CONANIInfo/CONANI-Info.html"
                                                                                target="_blank" class="nav">CONANIInfo</a> - that provides the necessary statistical
                                                                            information in order to track country progress towards fulfilling the rights of
                                                                            children and adolescents.
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                <tr>
                                                                                    <td width="90%" height="20" align="right">
                                                                                        <a href="news_content/Dominican_Republic_launches_CONANIInfo.html">more</a>
                                                                                    </td>
                                                                                    <td width="10%" height="20" align="left">
                                                                                        &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#CCCCCC" height="1">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="news_space">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="29%" align="center" valign="top">
                                                                <a href="news_content/honduras_tracking_unemployment_workforce.html">
                                                                    <img src="images/news_images/honduras_database_small.jpg" width="140" height="83"
                                                                        border="0" class="reflect ropacity30" /></a>
                                                            </td>
                                                            <td width="71%" valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <p>
                                                                                <strong><a href="news_content/honduras_tracking_unemployment_workforce.html">Honduras
                                                                                    Launches New Database to Track Youth Employment and Workforce Migration </a>
                                                                                </strong><span class="adaptation_italic">(20 December 2011</span>)</p>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            The Ministry of Labor and Social Security of the Government of Honduras has launched
                                                                            a new database system - <a href="http://207.42.179.145/infojem/" target="_blank"
                                                                                class="nav">Info JEM</a> - to disseminate relevant data on youth employment
                                                                            and workforce migration in Honduras.
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                <tr>
                                                                                    <td width="90%" height="20" align="right">
                                                                                        <a href="news_content/honduras_tracking_unemployment_workforce.html">more</a>
                                                                                    </td>
                                                                                    <td width="10%" height="20" align="left">
                                                                                        &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#CCCCCC" height="1">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="news_space">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="29%" align="center" valign="top">
                                                                <a href="news_content/Viet_Nam_monitoring_UN_One_Plan.html">
                                                                    <img src="images/news_images/vietnam_small.jpg" width="140" height="85" border="0"
                                                                        class="reflect ropacity30" /></a>
                                                            </td>
                                                            <td width="71%" valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <p>
                                                                                <strong><a href="news_content/Viet_Nam_monitoring_UN_One_Plan.html">Viet Nam Organizes
                                                                                    DevInfo Workshop to Develop a Customized Database for Monitoring the UN One Plan
                                                                                    2012-2016</a></strong><span class="adaptation_italic"><br />
                                                                                        (20 December 2011</span>)</p>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            The <a href="http://www.un.org.vn/en/the-un-in-viet-nam-mainmenu-37.html" target="_blank"
                                                                                class="nav">UN Resident Coordinator&rsquo;s Office in Viet Nam</a> organized
                                                                            a three-day workshop in Hanoi from 30 November to 2 December 2011 to equip participants
                                                                            in using <a href="http://www.devinfo.org" target="_blank" class="nav">DevInfo database
                                                                                technology</a> for monitoring the new UN One Plan for Viet Nam.
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                <tr>
                                                                                    <td width="90%" height="20" align="right">
                                                                                        <a href="news_content/Viet_Nam_monitoring_UN_One_Plan.html">more</a>
                                                                                    </td>
                                                                                    <td width="10%" height="20" align="left">
                                                                                        &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#CCCCCC" height="1">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="news_space">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="29%" align="center" valign="top">
                                                                <a href="news_content/russian_federation_devinfo_implementation.html">
                                                                    <img src="images/news_images/russia_training_small.jpg" width="140" height="85" border="0"
                                                                        class="reflect ropacity30" /></a>
                                                            </td>
                                                            <td width="71%" valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <p>
                                                                                <strong><a href="news_content/russian_federation_devinfo_implementation.html">Visit
                                                                                    to Russia Generates Increased Momentum for DevInfo Use</a></strong><span class="adaptation_italic"><br />
                                                                                        (16 December 2011</span>)</p>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            A training delegation from the DevInfo Support Group visited the Russian Federation
                                                                            from 7 – 11 November 2011 to discuss ways of using DevInfo technology to better
                                                                            support the statistical data management needs of various national organizations
                                                                            and institutions.
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                <tr>
                                                                                    <td width="90%" height="20" align="right">
                                                                                        <a href="news_content/russian_federation_devinfo_implementation.html">more</a>
                                                                                    </td>
                                                                                    <td width="10%" height="20" align="left">
                                                                                        &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#CCCCCC" height="1">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="news_space">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="29%" align="center" valign="top">
                                                                <a href="news_content/Devinfo_Webinar_Series_2011.html">
                                                                    <img src="images/news_images/News_webinar_1.jpg" width="140" height="84" border="0"
                                                                        class="reflect ropacity30" /></a>
                                                            </td>
                                                            <td width="71%" valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <p>
                                                                                <strong><a href="news_content/Devinfo_Webinar_Series_2011.html">2011 DevInfo Webinar
                                                                                    Series Entering Final Month</a></strong><span class="adaptation_italic"> (05 December
                                                                                        2011</span>)</p>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            The <a href="http://www.devinfo.org/di_webinar_2011.html" target="_blank" class="nav">
                                                                                2011 DevInfo Webinar series</a>, currently in its final month, complements face-to-face
                                                                            training and online e-learning courses on DevInfo and further provides insight into
                                                                            on-going projects and best practices in the field.
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                <tr>
                                                                                    <td width="90%" height="20" align="right">
                                                                                        <a href="news_content/Devinfo_Webinar_Series_2011.html">more</a>
                                                                                    </td>
                                                                                    <td width="10%" height="20" align="left">
                                                                                        &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#CCCCCC" height="1">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="news_space">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="29%" align="center" valign="top">
                                                                <a href="news_content/MDG_2011_data_online.html">
                                                                    <img src="images/news_images/mdginfo_2011_small.jpg" width="140" height="84" border="0"
                                                                        class="reflect ropacity30" /></a>
                                                            </td>
                                                            <td width="71%" valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <p>
                                                                                <strong><a href="news_content/MDG_2011_data_online.html">2011 MDG Data Now Available
                                                                                    Online</a></strong><span class="adaptation_italic"> (02 December 2011</span>)</p>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            The United Nations Statistics Division (UNSD) has released the latest data on Millennium
                                                                            Development Goals (MDGs) for all member states. The database, produced with support
                                                                            from UNICEF, is now available online at <a href="http://www.devinfo.info/mdginfo2011/"
                                                                                target="_blank" class="nav">http://www.devinfo.info/mdginfo2011/</a>.
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                <tr>
                                                                                    <td width="90%" height="20" align="right">
                                                                                        <a href="news_content/MDG_2011_data_online.html">more</a>
                                                                                    </td>
                                                                                    <td width="10%" height="20" align="left">
                                                                                        &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#CCCCCC" height="1">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="news_space">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="29%" align="center" valign="top">
                                                                <a href="news_content/Djibouti_Socio_Economic_Database.html">
                                                                    <img src="images/news_djibouti_2.jpg" width="140" height="85" border="0" class="reflect ropacity30" /></a>
                                                            </td>
                                                            <td width="71%" valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <p>
                                                                                <strong><a href="news_content/Djibouti_Socio_Economic_Database.html">Djibouti Launches
                                                                                    New Socio-Economic Database</a></strong><span class="adaptation_italic"> (30 November
                                                                                        2011</span>)</p>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            On 24 November 2011, the Government of Djibouti officially launched <a href="http://www.devinfo.info/djibouti"
                                                                                target="_blank" class="nav">Djibouti DevInfo</a>, a national socio- economic
                                                                            database. The database is an adaptation of the <a href="http://www.devinfo.org" target="_blank"
                                                                                class="nav">DevInfo</a> database system endorsed by the United Nations to monitor
                                                                            progress towards achieving the Millennium Development Goals and other national priorities.
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                <tr>
                                                                                    <td width="90%" height="20" align="right">
                                                                                        <a href="news_content/Djibouti_Socio_Economic_Database.html">more</a>
                                                                                    </td>
                                                                                    <td width="10%" height="20" align="left">
                                                                                        &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#CCCCCC" height="1">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="news_space">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="29%" align="center" valign="top">
                                                                <a href="news_content/Highlighting_devinfo_presentation_outputs.html">
                                                                    <img src="images/news_images/Call_sub_1_s.jpg" width="140" height="85" border="0"
                                                                        class="reflect ropacity30" /></a>
                                                            </td>
                                                            <td width="71%" valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <p>
                                                                                <strong><a href="news_content/Highlighting_devinfo_presentation_outputs.html">Call for
                                                                                    Submissions: Highlighting Outstanding DevInfo Presentation Outputs in Your Country/Organization</a></strong><span
                                                                                        class="adaptation_italic"> (29 November 2011</span>)</p>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            The <a href="http://www.devinfo.org/di_showcase.html" target="_blank" class="nav">DevInfo
                                                                                Showcase</a> presents exhibits, displays and advocacy materials developed for
                                                                            use at country, regional and global events. These materials are showcased in an
                                                                            interactive graphic format, allowing global visitors to browse, explore, and download
                                                                            them as desired.
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                <tr>
                                                                                    <td width="90%" height="20" align="right">
                                                                                        <a href="news_content/Highlighting_devinfo_presentation_outputs.html">more</a>
                                                                                    </td>
                                                                                    <td width="10%" height="20" align="left">
                                                                                        &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#CCCCCC" height="1">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="news_space">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="29%" align="center" valign="top">
                                                                <a href="news_content/Angola_children_development_database.html">
                                                                    <img src="images/news_images/angola_small.jpg" width="140" height="85" border="0"
                                                                        class="reflect ropacity30" /></a>
                                                            </td>
                                                            <td width="71%" valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <p>
                                                                                <strong><a href="news_content/Angola_children_development_database.html">Angola Set
                                                                                    to Launch Data System to Better Track the Well-Being of Children</a></strong><span
                                                                                        class="adaptation_italic"> (24 November 2011</span>)</p>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            As the world celebrates the twenty-second anniversary of the Convention on the Rights
                                                                            of the Child, Angola is announcing the development of a system to collect reliable
                                                                            and regular data on the country&rsquo;s children.
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                <tr>
                                                                                    <td width="90%" height="20" align="right">
                                                                                        <a href="news_content/Angola_children_development_database.html">more</a>
                                                                                    </td>
                                                                                    <td width="10%" height="20" align="left">
                                                                                        &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#CCCCCC" height="1">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="news_space">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="29%" align="center" valign="top">
                                                                <a href="news_content/Cape_Verde_Data_management_training.html">
                                                                    <img src="images/news_images/cape_verde_small.jpg" width="140" height="85" border="0"
                                                                        class="reflect ropacity30" /></a>
                                                            </td>
                                                            <td width="71%" valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <p>
                                                                                <strong><a href="news_content/Cape_Verde_Data_management_training.html">Cape Verde Organizes
                                                                                    DevInfo Training for Census Data Management and the Monitoring and Evaluation of
                                                                                    Development Frameworks </a></strong>
                                                                                <br />
                                                                                <span class="adaptation_italic">(24 November 2011)</span></p>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            The One UN Programme in Cape Verde organized two important DevInfo training events
                                                                            for the UN Country Team and government counterparts from 7-11 November 2011, aimed
                                                                            at strengthening current data management and monitoring and evaluation practices.
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                <tr>
                                                                                    <td width="90%" height="20" align="right">
                                                                                        <a href="news_content/Cape_Verde_Data_management_training.html">more</a>
                                                                                    </td>
                                                                                    <td width="10%" height="20" align="left">
                                                                                        &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#CCCCCC" height="1">
                                                </td>
                                            </tr>
                                            <!--<tr>
                  <td class="news_space"><table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                      <td width="29%" align="center" valign="top"><a href="news_content/Highlighting_devinfo_presentation_outputs.html"><img src="images/news_images/Call_sub_1_s.jpg" width="140" height="85" border="0" class="reflect ropacity30"/></a></td>
                      <td width="71%" valign="top"><table width="100%" border="0" cellspacing="0" cellpadding="2">
                        <tr>
                          <td valign="top"><p><strong><a href="news_content/Highlighting_devinfo_presentation_outputs.html">Call for Submissions: Highlighting Outstanding DevInfo Presentation Outputs in Your Country/Organization 
                            - </a></strong><span class="mainheadings_home"><strong><a href="news_content/Highlighting_devinfo_presentation_outputs.html"> </a></strong><span class="adaptation_italic"><em >22<sup> </sup></em>November 2011</span></span></p></td>
                          </tr>
                        <tr>
                          <td>The <a href="http://www.devinfo.org/di_showcase.html" class="nav">DevInfo  Showcase</a> presents exhibits, displays and advocacy materials developed for  use at country, regional and global events. These materials are showcased in an  interactive graphic format, allowing global visitors to browse, explore, and download them as desired. </td>
                          </tr>
                        <tr>
                          <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                              <td width="90%" height="20" align="right"><a href="news_content/Highlighting_devinfo_presentation_outputs.html">more</a></td>
                              <td width="10%" height="20" align="left">&nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" /></td>
                              </tr>
                            </table></td>
                          </tr>
                        </table></td>
                      </tr>
                    </table></td>
                </tr>
                  <tr>
                    <td bgcolor="#CCCCCC" height="1"></td>
                  </tr>-->
                                            <tr>
                                                <td class="news_space">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="29%" align="center" valign="top">
                                                                <a href="news_content/Monitoring_migration_trends_migrantinfo.html">
                                                                    <img src="images/news_images/migrantinfo_small.jpg" width="140" height="85" border="0"
                                                                        class="reflect ropacity30" /></a>
                                                            </td>
                                                            <td width="71%" valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <p>
                                                                                <strong><a href="news_content/Monitoring_migration_trends_migrantinfo.html">MigrantInfo
                                                                                    Initiative Harnesses the Power of DevInfo Dissemination Technology</a></strong><span
                                                                                        class="adaptation_italic"> (<em>18</em> November 2011)</span></span></p>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Enhanced analyses on issues of international migration have been developed as an
                                                                            output of the <a href="http://www.migrantinfo.org/" target="_new" class="nav">MigrantInfo</a>
                                                                            initiative. These products effectively harness the power of DevInfo database technology
                                                                            to present data.
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                <tr>
                                                                                    <td width="90%" height="20" align="right">
                                                                                        <a href="news_content/Monitoring_migration_trends_migrantinfo.html">more</a>
                                                                                    </td>
                                                                                    <td width="10%" height="20" align="left">
                                                                                        &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#CCCCCC" height="1">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="news_space">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="29%" align="center" valign="top">
                                                                <a href="news_content/Serbia_development_data_online.html">
                                                                    <img src="images/news_images/serbia_small.jpg" width="140" height="85" border="0"
                                                                        class="reflect ropacity30" /></a>
                                                            </td>
                                                            <td width="71%" valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <p>
                                                                                <strong><a href="news_content/Serbia_development_data_online.html">New Human Development
                                                                                    Data Sets from Serbia Available Online
                                                                                    <br />
                                                                                </a></strong><span class="adaptation_italic"><em>(17</em> November 2011)</span></p>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            The Statistical Office of the Republic of Serbia has released its new national DevInfo
                                                                            database, updated as of October 2011, in both DevInfo 6.0 and 5.0 formats.
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                <tr>
                                                                                    <td width="90%" height="20" align="right">
                                                                                        <a href="news_content/Serbia_development_data_online.html">more</a>
                                                                                    </td>
                                                                                    <td width="10%" height="20" align="left">
                                                                                        &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#CCCCCC" height="1">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="news_space">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="29%" align="center" valign="top">
                                                                <a href="news_content/Monitoring_reproductive_health_unfpa.html">
                                                                    <img src="images/news_images/UNFPA-_DevInfo_Training_small.jpg" width="140" height="85"
                                                                        border="0" class="reflect ropacity30" /></a>
                                                            </td>
                                                            <td width="71%" valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <p>
                                                                                <strong><a href="news_content/Monitoring_reproductive_health_unfpa.html">UNFPA Organizes
                                                                                    DevInfo Training to Support Greater Access to Reproductive Health</a></strong><em> (16
                                                                                        November 2011)</em></p>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="bodytext" style="line-height: 18px">The United Nations Population Fund
                                                                                (UNFPA) organized a DevInfo training workshop in Johannesburg, South Africa from
                                                                                1–4 November 2011.</span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                <tr>
                                                                                    <td width="90%" height="20" align="right">
                                                                                        <a href="news_content/Monitoring_reproductive_health_unfpa.html">more</a>
                                                                                    </td>
                                                                                    <td width="10%" height="20" align="left">
                                                                                        &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#CCCCCC" height="1">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="news_space">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="29%" align="center" valign="top">
                                                                <a href="news_content/2011_India_Census_Provisional_Data_Galleries.html">
                                                                    <img src="images/news_images/Census_Galleries_Online_small.jpg" width="140" height="85"
                                                                        border="0" class="reflect ropacity30" /></a>
                                                            </td>
                                                            <td width="71%" valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <p>
                                                                                <strong><a href="news_content/2011_India_Census_Provisional_Data_Galleries.html">India
                                                                                    State 2011 Census Galleries Available Online </a><a href="news_content/DIA_Submissions.html">
                                                                                    </a></strong><em>(16 November 2011)</em></p>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            The governments of the central Indian state of Madhya Pradesh and the&nbsp;western
                                                                            Indian state of Gujarat have published <a href="http://www.devinfo.org/di_customized_galleries.html"
                                                                                target="_blank" class="nav">interactive online galleries</a> displaying information
                                                                            on provisional state population data collected during India&rsquo;s 2011 national
                                                                            census round.
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                <tr>
                                                                                    <td width="90%" height="20" align="right">
                                                                                        <a href="news_content/2011_India_Census_Provisional_Data_Galleries.html">more</a>
                                                                                    </td>
                                                                                    <td width="10%" height="20" align="left">
                                                                                        &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#CCCCCC" height="1">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="news_space">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="29%" align="center" valign="top">
                                                                <a href="news_content/DIA_Submissions.html">
                                                                    <img src="images/news_images/Submission_2.jpg" width="140" height="85" border="0"
                                                                        class="reflect ropacity30" /></a>
                                                            </td>
                                                            <td width="71%" valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <p>
                                                                                <strong><a href="news_content/DIA_Submissions.html">Call for Submissions: Highlighting
                                                                                    “DevInfo in Action” in Your Country/Organization </a></strong><em>(1 November 2011)</em></p>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            The <a href="http://www.devinfo.org/devinfo_in_action.html" target="_blank" class="nav">
                                                                                DevInfo in Action</a> series chronicles inspiring stories of DevInfo technology
                                                                            being put to good use and making a difference around the world. Published via a
                                                                            weekly DevInfo e-mailer, on the <a href="http://www.devinfo.org/" target="_blank"
                                                                                class="nav">DevInfo website</a>, and in <a href="http://www.devinfo.org/diBook/dia/dia_book.html"
                                                                                    target="_blank" class="nav">eBook format</a>, these stories have been very
                                                                            well-received by our global reader base.
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                <tr>
                                                                                    <td width="90%" height="20" align="right">
                                                                                        <a href="news_content/DIA_Submissions.html">more</a>
                                                                                    </td>
                                                                                    <td width="10%" height="20" align="left">
                                                                                        &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#CCCCCC" height="1">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="news_space">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="29%" align="center" valign="top">
                                                                <a href="news_content/mdg_statistics_conference_2011.html">
                                                                    <img src="images/news_images/mdg_Small.jpg" width="140" height="85" border="0" class="reflect ropacity30" /></a>
                                                            </td>
                                                            <td width="71%" valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <p>
                                                                                <strong><a href="news_content/mdg_statistics_conference_2011.html">DevInfo Highlighted
                                                                                    at International Conference on MDG Statistics<br />
                                                                                </a></strong><em>(31 October 2011)</em></p>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            The UN DevInfo Initiative presented at the recent International Conference on <a
                                                                                href="http://mdgs.un.org/unsd/mdg/Host.aspx?Content=Capacity/manila.htm" target="_blank"
                                                                                class="nav">Millennium Development Goals (MDG) Statistics</a> held from 19 –
                                                                            21 October 2011 in Manila, Philippines. Organized by the United Nations Statistics
                                                                            Division (UNSD) in collaboration with the Philippines National Statistical Coordination
                                                                            Board (NSCB).
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                <tr>
                                                                                    <td width="90%" height="20" align="right">
                                                                                        <a href="news_content/mdg_statistics_conference_2011.html">more</a>
                                                                                    </td>
                                                                                    <td width="10%" height="20" align="left">
                                                                                        &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#CCCCCC" height="1">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="news_space">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="29%" align="center" valign="top">
                                                                <a href="news_content/apha_conference_2011.html">
                                                                    <img src="images/news_images/apha_Small.jpg" width="140" height="85" border="0" class="reflect ropacity30" /></a>
                                                            </td>
                                                            <td width="71%" valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <p>
                                                                                <strong><a href="news_content/apha_conference_2011.html"><strong>DevInfo to Participate
                                                                                    in Public Health Conference in Washington<br />
                                                                                </strong></a><a href="news_content/maldives_national_database.html"></a></strong>
                                                                                <em>(31 October 2011)</em></p>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            The UN DevInfo Initiative will be participating in the 139th Annual Meeting and
                                                                            Exposition of the American Public Health Association (APHA), scheduled from 29 October
                                                                            – 2 November 2011 in Washington, DC, USA.
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                <tr>
                                                                                    <td width="90%" height="20" align="right">
                                                                                        <a href="news_content/apha_conference_2011.html">more</a>
                                                                                    </td>
                                                                                    <td width="10%" height="20" align="left">
                                                                                        &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#CCCCCC" height="1">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="news_space">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="29%" align="center" valign="top">
                                                                <a href="news_content/maldives_national_database.html">
                                                                    <img src="images/news_images/maldivinfo_thumb.jpg" width="140" height="85" border="0"
                                                                        class="reflect ropacity30" /></a>
                                                            </td>
                                                            <td width="71%" valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <p>
                                                                                <strong><a href="news_content/maldives_national_database.html">Maldives Builds DevInfo
                                                                                    Training Capacity among Government Staff </a></strong><em>(31 October 2011)</em></p>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            The national government of the Maldives took a key step towards further institutionalizing
                                                                            its national socio-economic database, MaldivInfo, by organizing a DevInfo Training
                                                                            of Trainers workshop this month.
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                <tr>
                                                                                    <td width="90%" height="20" align="right">
                                                                                        <a href="news_content/maldives_national_database.html">more</a>
                                                                                    </td>
                                                                                    <td width="10%" height="20" align="left">
                                                                                        &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#CCCCCC" height="1">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="news_space">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="29%" align="center" valign="top">
                                                                <a href="news_content/devinfo_frgm_2011.html">
                                                                    <img src="images/news_images/frgm_2011_detail_s.jpg" width="140" height="85" border="0"
                                                                        class="reflect ropacity30" /></a>
                                                            </td>
                                                            <td width="71%" valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <p>
                                                                                <strong><a href="news_content/devinfo_frgm_2011.html">DevInfo Field Reference Group
                                                                                    Meeting Reviewed Progress to Date, Way Forward </a></strong><em>(25 October 2011)</em></p>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            The UN DevInfo Initiative conducted the 2011 DevInfo Field Reference Group Meeting
                                                                            in Jaipur, India from 26 - 30 September 2011. The meeting brought together experienced
                                                                            DevInfo users from across the globe to take stock of the current status of DevInfo
                                                                            implementation, introduce new tools and technologies, preview the upcoming release
                                                                            of DevInfo 7.0, and seek solutions to current constraints in DevInfo implementation.
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                <tr>
                                                                                    <td width="90%" height="20" align="right">
                                                                                        <a href="news_content/devinfo_frgm_2011.html">more</a>
                                                                                    </td>
                                                                                    <td width="10%" height="20" align="left">
                                                                                        &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#CCCCCC" height="1">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="news_space">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="29%" align="center" valign="top">
                                                                <a href="news_content/polio_data_portal_launch_2011.html">
                                                                    <img src="images/news_images/polioinfo_small.jpg" width="140" height="85" border="0"
                                                                        class="reflect ropacity30" /></a>
                                                            </td>
                                                            <td width="71%" valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <p>
                                                                                <strong><a href="news_content/polio_data_portal_launch_2011.html">UNICEF Launches New
                                                                                    Data Portal, Website to Strengthen Communication Efforts for Polio Eradication </a>
                                                                                </strong><em>(25 October 2011)</em></p>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            On the occasion of World Polio Day, 24 October 2011, UNICEF is launching a new website
                                                                            and online data portal – <a href="http://www.polioinfo.org/" target="_blank" class="nav">
                                                                                PolioInfo</a> - designed to strengthen communication efforts in all polio priority
                                                                            countries to eradicate polio.
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                <tr>
                                                                                    <td width="90%" height="20" align="right">
                                                                                        <a href="news_content/polio_data_portal_launch_2011.html">more</a>
                                                                                    </td>
                                                                                    <td width="10%" height="20" align="left">
                                                                                        &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#CCCCCC" height="1">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="news_space">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="29%" align="center" valign="top">
                                                                <a href="news_content/wsscc_global_forum_2011.html">
                                                                    <img src="images/news_images/WSSCC_small.jpg" width="140" height="85" border="0"
                                                                        class="reflect ropacity30" /></a>
                                                            </td>
                                                            <td width="71%" valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <p>
                                                                                <strong><a href="news_content/wsscc_global_forum_2011.html">DevInfo Initiative Presents
                                                                                    at Global Forum on Sanitation and Hygiene</a></strong><em> (24 October 2011)</em></p>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            The UN DevInfo Initiative presented at the recent <a href="http://wsscc-global-forum.org/"
                                                                                target="_blank" class="nav">Global Forum on Sanitation and Hygiene</a> held
                                                                            from 9 – 14 October 2011 in Mumbai, India. The Forum was organized as a global platform
                                                                            to share ideas on skills, knowledge, leadership, behaviour change and actions that
                                                                            can improve the lives of the world&rsquo;s 2.6 billion people lacking access to
                                                                            safe sanitation and hygiene.
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                <tr>
                                                                                    <td width="90%" height="20" align="right">
                                                                                        <a href="news_content/wsscc_global_forum_2011.html">more</a>
                                                                                    </td>
                                                                                    <td width="10%" height="20" align="left">
                                                                                        &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#CCCCCC" height="1">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="news_space">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="29%" align="center" valign="top">
                                                                <a href="news_content/regional_development_diana_madagascar.html">
                                                                    <img src="images/news_images/Madagascar_small.jpg" width="140" height="85" border="0"
                                                                        class="reflect ropacity30" /></a>
                                                            </td>
                                                            <td width="71%" valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <p>
                                                                                <strong><a href="news_content/regional_development_diana_madagascar.html">Madagascar’s
                                                                                    Diana Region Using DevInfo to Monitor Regional Development</a></strong><em> (18 October
                                                                                        2011)</em></p>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            The Regional Resource Center of Diana, which is the most northern region of Madagascar,
                                                                            has developed Diana-Info 1.0, an adaptation of the DevInfo database system endorsed
                                                                            by the United Nations to monitor progress towards achieving the Millennium Development
                                                                            Goals and other national priorities.
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                <tr>
                                                                                    <td width="90%" height="20" align="right">
                                                                                        <a href="news_content/regional_development_diana_madagascar.html">more</a>
                                                                                    </td>
                                                                                    <td width="10%" height="20" align="left">
                                                                                        &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#CCCCCC" height="1">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="news_space">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="29%" align="center" valign="top">
                                                                <a href="news_content/news_devinfo_webinar_series.html">
                                                                    <img src="images/news_images/diwebinar_small.jpg" width="140" height="85" border="0"
                                                                        class="reflect ropacity30" /></a>
                                                            </td>
                                                            <td width="71%" valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <p>
                                                                                <strong><a href="news_content/news_devinfo_webinar_series.html">DevInfo Webinar Series
                                                                                    Entering Second Month</a></strong><em> (18 October 2011)</em></p>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            The 2011 DevInfo Webinar series, currently in its second month, complements face-to-face
                                                                            training and online e-learning courses on DevInfo and further provides insight into
                                                                            on-going projects and best practices in the field.
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                <tr>
                                                                                    <td width="90%" height="20" align="right">
                                                                                        <a href="news_content/news_devinfo_webinar_series.html">more</a>
                                                                                    </td>
                                                                                    <td width="10%" height="20" align="left">
                                                                                        &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#CCCCCC" height="1">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="news_space">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="29%" align="center" valign="top">
                                                                <a href="news_content/news_unitar_di_monitoring_training.html">
                                                                    <img src="images/news_images/unitar_small.jpg" width="140" height="85" border="0"
                                                                        class="reflect ropacity30" /></a>
                                                            </td>
                                                            <td width="71%" valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="2">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <p>
                                                                                <strong><a href="news_content/news_unitar_di_monitoring_training.html">UNITAR, DevInfo
                                                                                    Support Group Train Development Officers in Monitoring Local Development Plans</a></strong><em>
                                                                                        (13 October 2011)</em></p>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            The United Nations Institute for Training and Research (UNITAR) and the DevInfo
                                                                            Support Group partnered to train national actors on the new <a href="http://www.devinfo.org/di_monitoring.html"
                                                                                target="_blank" class="nav">di Monitoring</a>&nbsp;tool for improving the creation
                                                                            and implementation of national and local development plans.
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                <tr>
                                                                                    <td width="90%" height="20" align="right">
                                                                                        <a href="news_content/news_unitar_di_monitoring_training.html">more</a>
                                                                                    </td>
                                                                                    <td width="10%" height="20" align="left">
                                                                                        &nbsp;<img src="images/arrow.gif" width="4" height="9" class="arrow_align" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#CCCCCC" height="1">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="bodytext">
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td colspan="9" align="center">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="20%" height="20" align="right">
                                                                <strong>Page</strong>
                                                            </td>
                                                            <td width="8%" align="center">
                                                                <a href="di_news.html" class="selected">1</a>
                                                            </td>
                                                            <td width="8%" align="center">
                                                                <a href="di_news_2.html">2</a>
                                                            </td>
                                                            <td width="8%" align="center">
                                                                <a href="di_news_3.html">3</a>
                                                            </td>
                                                            <td width="8%" align="center">
                                                                <a href="di_news_4.html">4</a>
                                                            </td>
                                                            <td width="8%" align="center">
                                                                <a href="di_news_5.html">5</a>
                                                            </td>
                                                            <td width="8%" height="20" align="center">
                                                                <a href="di_news_6.html">6</a>
                                                            </td>
                                                            <td width="8%" height="20" align="center">
                                                                <a href="di_news_7.html">7</a>
                                                            </td>
                                                            <td width="8%" height="20" align="center">
                                                                <a href="di_news_8.html">8</a>
                                                            </td>
                                                            <td width="8%" align="center">
                                                                <a href="di_news_9.html">9</a>
                                                            </td>
                                                            <td width="8%" align="center">
                                                                <a href="di_news_10.html">10</a>
                                                            </td>
                                                            <td width="8%" align="center">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td height="20">
                                                            </td>
                                                            <td height="20" colspan="9">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <table width="598" border="0" align="right" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td width="10" height="14" valign="top">
                                                    <img src="images/bottom_left1.gif" width="10" height="14" border="0" />
                                                </td>
                                                <td width="578" height="14" background="images/bottom_bg.gif">
                                                </td>
                                                <td width="10" height="14" valign="top">
                                                    <img src="images/bottom_right1.gif" width="10" height="14" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td height="5" colspan="3">
                        </td>
                    </tr>
                </table>
                </td> </tr> </table></td> </tr> </table>
            </div>
            <!-- Static Content Area ...ends-->
            <!-- Right Config Data Section ...ends-->
            <div class="clear">
            </div>
        </div>
        <!-- Main Container ...ends-->
    </div>
    <!-- DEVELOPER CODE -->
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
        createFormHiddenInputs("frmNews", "POST");
        SetCommonLinksHref("frmNews", "POST");
        setPostedData('<%=hdsby%>', '<%=hdbnid%>', '<%=hselarea%>', '<%=hselind%>', '<%=hlngcode%>', '<%=hlngcodedb%>', '<%=hselindo%>', '<%=hselareao%>', 10, '<%=hLoggedInUserNId%>', '<%=hLoggedInUserName%>');
        if (GetLanguageCounts() > 1) {
            z("LanguagePipeLi").style.display = "";
            ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', '<%=hlngcode%>');
        }
    </script>
    <script type="text/javascript">
        di_jq(document).ready(function () {
            SelectNewsMenuOption();
            var Url = document.URL.split("?")[1];
            document.getElementById('<%=EditPage.ClientID%>').href = "EditCmsContent.aspx?" + Url;
            //document.getElementById('<%=AddNewNews.ClientID%>').href = "AddCMSContent.aspx?" + Url;
            document.getElementById('<%=AddNewNews.ClientID%>').href = "AddCMSContents.aspx";
        });	
    </script>
</asp:content>
