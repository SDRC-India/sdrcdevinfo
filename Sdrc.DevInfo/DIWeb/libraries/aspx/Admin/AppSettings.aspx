<%@ Page Language="C#" MasterPageFile="~/libraries/aspx/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="AppSettings.aspx.cs" Inherits="libraries_aspx_Admin_AppSettings" Title="Untitled Page" EnableSessionState="True" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <h1 id="langApplicationSetting"><!--Application Setting--></h1> 
    <h4 id="langSet_GASH"><!--Set general application settings here--></h4>    
    
    <h2 id="langApplication"><!--Application--></h2> 
    
    <!-- Input Fields Area ...starts-->    
    <div class="confg_Adm_box"> 
    
        <p class="confg_frm_txt" id="langAdaptationName"><!--Adaptation Name--> <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx"><asp:TextBox ID="txtAdaptationName" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p>
        <div class="clear"></div>

        <p class="confg_frm_txt" id="lang_Adapted_for"><%--Adapted for--%><span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx"><input id="rbGlobal" type="radio" name="Adapted" onclick="SelectAdaptedFor('Global');" /><span id="lang_Global"><%--Global--%></span> <input id="rbNational" type="radio" name="Adapted" onclick="SelectAdaptedFor('National');" /><span id="lang_National"><!--National--></span> <input id="rbSubNational" type="radio" name="Adapted" onclick="SelectAdaptedFor('Sub-national');" /><span id="lang_Sub-national"><!--Sub-national--></span> </p>
        <div class="clear"></div>

        <p class="confg_frm_txt" ><span id="lang_Select_country"><!--Select country--></span> <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx"><select id="ddlSelectCountry" disabled="disabled"></select></p>
        <div class="clear"></div>

        <p class="confg_frm_txt" ><span id="lang_Specify_sub-nation_IA"><!--Specify sub-nation if any--></span> <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx"><input id="txtSubNation" type="text" class="confg_frm_inp_bx_txt" /></p>
        <div class="clear"></div>
        
        <p class="confg_frm_txt" id="langDefaultLanguage"><!--Default Language--> <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx"><asp:DropDownList ID="ddlLanguage" runat="server" class="confg_frm_inp_bx_txt_dd"></asp:DropDownList></p> 
        <div class="clear"></div>
        
        <p class="confg_frm_txt" id="langTheme" style="display:none;"><!--Theme--> <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx" style="display:none;"><asp:DropDownList ID="ddlTheme" runat="server" class="confg_frm_inp_bx_txt_dd"></asp:DropDownList></p> 
        <div class="clear"style="display:none;" ></div>
                
        <p class="confg_frm_txt" id="langShowSliders"><!--Show sliders--></p>
        <p class="confg_frm_inp_bx_radbtn"><asp:CheckBox ID="chkShowSliders" runat="server" /></p> 
        <div class="clear"></div>    

        <p class="confg_frm_txt" id="langSliderCount"><!--Slide count--> <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx_radbtn"><asp:TextBox ID="txtSliderCount" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p>
        <div class="clear"></div>

        <p class="confg_frm_txt" id="lngMrdThreshold" title="<%=MRDTooltip%>"><!--Mrd threshold--> <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx"><asp:TextBox ID="txtMrdThreshold" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p>
        <div class="clear"></div>

        <p class="confg_frm_txt" id="lngHideSourceColumn" title="<%=HideSrcColumnTooltip%>"><!--Hide Source Column--></p>
        <p class="confg_frm_inp_bx_radbtn"><asp:CheckBox ID="chkHideSrc" runat="server" /></p> 
        <div class="clear"></div>

        <p class="confg_frm_txt" id="lngQDSCache"><!--QDS Cache For Densed Areas--></p>
        <p class="confg_frm_inp_bx_radbtn"><asp:CheckBox ID="chkQDSCache" runat="server" /></p> 
        <div class="clear"></div>

        <p class="confg_frm_txt" id="lngDIB" title="<%=DIBTooltip%>"><!--Disputed International Boundries--></p>
        <p class="confg_frm_inp_bx_radbtn"><asp:CheckBox ID="chkDIB" runat="server" /></p> 
        <div class="clear"></div>

        <p class="confg_frm_txt" id="lngAdpMode"><!--Adaptation Mode--></p>
        <p class="confg_frm_inp_bx_radbtn"><asp:RadioButton id="rbAdpModeDI7" runat="server" Text="" GroupName="AdpMode" /><label id="langAdpModeDI7"><!--DI7--></label><asp:RadioButton id="rbAdpModeRegistry" runat="server" Text="" GroupName="AdpMode" /><label id="langAdpModeRegistry"><!--Standalone Registry--></label></p> 
        <div class="clear"></div>

        <p class="confg_frm_txt" id="lngJSVer"><!--JS Version--></p>
        <p class="confg_frm_inp_bx"><asp:TextBox ID="txtJSVersion" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p> 
        <div class="clear"></div>

         <p class="confg_frm_txt" id="lngAppVer" style="display:none"><!--Application Version--></p>
        <p class="confg_frm_inp_bx"><asp:TextBox ID="txtApplicationVersion" runat="server" class="confg_frm_inp_bx_txt" style="display:none"></asp:TextBox></p> 
        <div class="clear" style="display:none"></div>

        <p class="confg_frm_txt"><span id="lngAdptYear"><%--Adaptation year--%></span> <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx"><asp:TextBox ID="txtAdaptationYear" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p> 
        <div class="clear"></div>

        <p class="confg_frm_txt" style="display:none;"><span id="lngUnicefRegion"><%--Unicef region--%></span> <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx" style="display:none;"><asp:DropDownList ID="ddlUnicefRegion" runat="server" class="confg_frm_inp_bx_txt_dd"></asp:DropDownList></p> 
        <div class="clear" style="display:none;"></div>        

        <p class="confg_frm_txt" style="display:none;"><span id="lngDesktopVerAvl"><%--Desktop version available--%></span></p>
        <p class="confg_frm_inp_bx_radbtn" style="display:none;"><asp:CheckBox ID="chkDesktopVerAvailable" runat="server" /></p> 
        <div class="clear" style="display:none;"></div>

        <p class="confg_frm_txt"><span id="lngQDSGallery"><%--Enable Gallery in QDS--%></span></p>
        <p class="confg_frm_inp_bx_radbtn"><asp:CheckBox ID="chkQDSGallery" runat="server" /></p> 
        <div class="clear"></div>

        <p class="confg_frm_txt"><span id="lngNewsMenu"><%--Enable News in Menu--%></span></p>
        <p class="confg_frm_inp_bx_radbtn"><asp:CheckBox ID="chkNewsMenu" runat="server" /></p> 
        <div class="clear"></div>

        <p class="confg_frm_txt"><span id="lngInnovationsMenu"><%--Enable Innovations in Menu--%></span></p>
        <p class="confg_frm_inp_bx_radbtn"><asp:CheckBox ID="chkInnovationsMenu" runat="server" /></p> 
        <div class="clear"></div>

        <p class="confg_frm_txt"><span id="lngQDSCloud"><%--Enable Cloud in QDS Search--%></span></p>
        <p class="confg_frm_inp_bx_radbtn"><asp:CheckBox ID="chkCloudQDS" runat="server" /></p> 
        <div class="clear"></div>

        <p class="confg_frm_txt"><span id="lngContactUsMenu"><%--Enable Contact Us in Menu--%></span></p>
        <p class="confg_frm_inp_bx_radbtn"><asp:CheckBox ID="chkContactUs" runat="server" /></p> 
        <div class="clear"></div>

        <p class="confg_frm_txt"><span id="lngSupportMenu"><%--Enable Support in Menu--%></span></p>
        <p class="confg_frm_inp_bx_radbtn"><asp:CheckBox ID="chkSupport" runat="server" /></p> 
        <div class="clear"></div>

         <p class="confg_frm_txt"><span id="lngDownloadsMenu"><%--Enable Downloads in Menu--%></span></p>
        <p class="confg_frm_inp_bx_radbtn"><asp:CheckBox ID="chkDownloads" runat="server" /></p> 
        <div class="clear"></div>

         <p class="confg_frm_txt"><span id="lngTrainingMenu"><%--Enable Training in Menu--%></span></p>
        <p class="confg_frm_inp_bx_radbtn"><asp:CheckBox ID="chkTraining" runat="server" /></p> 
        <div class="clear"></div>

         <p class="confg_frm_txt"><span id="lngMapLibraryMenu"><%--Enable Map Library in Menu--%></span></p>
        <p class="confg_frm_inp_bx_radbtn"><asp:CheckBox ID="chkMapLibrary" runat="server" /></p> 
        <div class="clear"></div>

          <p class="confg_frm_txt"><span id="lngRSSFeedsMenu"><%--Enable Map Library in Menu--%></span></p>
        <p class="confg_frm_inp_bx_radbtn"><asp:CheckBox ID="chkRSSFeeds" runat="server" /></p> 
        <div class="clear"></div>

        <p class="confg_frm_txt"><span id="lngFAQ"><%--Enable FAQ in Footer--%></span></p>
        <p class="confg_frm_inp_bx_radbtn"><asp:CheckBox ID="chkFAQ" runat="server" /></p> 
        <div class="clear"></div>

        <p class="confg_frm_txt"><span id="lngKB"><%--Enable FAQ in Footer--%></span></p>
        <p class="confg_frm_inp_bx_radbtn"><asp:CheckBox ID="chkKB" runat="server" /></p> 
        <div class="clear"></div>

        <p class="confg_frm_txt"><span id="lngDIWorldWideMenu"><%--Enable Di worldwide in Menu--%></span></p>
        <p class="confg_frm_inp_bx_radbtn"><asp:CheckBox ID="chkDIWorldwide" runat="server" /></p> 
        <div class="clear"></div>        

        <p class="confg_frm_txt"><span id="lngHowToVideo"><%--Enable HowtoVideo in Menu--%></span></p>
        <p class="confg_frm_inp_bx_radbtn"><asp:CheckBox ID="chkHowToVideo" runat="server" /></p> 
        <div class="clear"></div> 
        
        <p class="confg_frm_txt"><span id="lngSitemapLink"><%--Enable HowtoVideo in Menu--%></span></p>
        <p class="confg_frm_inp_bx_radbtn"><asp:CheckBox ID="chkSitemap" runat="server" /></p> 
        <div class="clear"></div>        

        <p class="confg_frm_txt" id="langGoogleAnalyticsId"><!--Google Analytics ID--></p>
        <p class="confg_frm_inp_bx"><asp:TextBox ID="txtGoogleAnalyticsId" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p> 
        <div class="clear"></div>   

        <p class="confg_frm_txt" id="lblAnimationTime"><!--Animation time interval--></p>
        <p class="confg_frm_inp_bx"><asp:TextBox ID="txtAnimationTime" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p>
        <div class="clear">
        </div>
    </div>
    <h2 id="lngHowToVideos">        
        How to Video</h2>
    <div class="confg_Adm_box" id="dvHowToVideo" runat="server">   
               
    </div>
    <h2 id="H1">
        Contact of database administrator</h2>

        <h2 id="LangMetaTags">Meta Tags</h2>
        <div id="MetaTags" class="confg_Adm_box">
        <fieldset style="width:512px;margin-left:35px;margin-bottom:20px" class="confg_frm_inp_bx_txt pos_rel" id="Fs_SiteMap">
        
        <span class="fldset_lbl" id="Lang_Meta_Sitemap">Site Map</span>
        <p><span style="width:220px; display:inline-block" id="Lang_MetaTag_Desc">Description</span>
        <input type="text" value="test" class="confg_frm_inp_bx_txt" id="MetaTag_Desc_Val" runat="server"></p><div class="clear">
        </div><p><span style="width:220px; display:inline-block" id="Lang_MetaTag_Kw">Key Word</span>
        <input type="text" value="" class="confg_frm_inp_bx_txt" id="MetaTag_Kw_Val" runat="server"></p><div class="clear"></div></fieldset>
        </div>

    <div class="confg_Adm_box">
        <p class="confg_frm_txt"><span id="lngName"><%--Name--%></span> <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx"><asp:TextBox ID="txtAdmName" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p>
        <div class="clear"></div>  
        
        <p class="confg_frm_txt"><span id="lngInstitution"><%--Institution--%></span> <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx"><asp:TextBox ID="txtAdmInstitution" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p> 
        <div class="clear"></div>

        <p class="confg_frm_txt"><span id="lngEmail"><%--Email--%></span> <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx"><asp:TextBox ID="txtAdmEmail" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p> 
        <div class="clear"></div>    
    </div>    
    
    <h2 id="langWebComponents"><!--Web Components--></h2> 
    <div class="confg_Adm_box"> 
    
        <p class="confg_frm_txt" id="langComponentVersion"><!--Component Version--> <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx"><asp:TextBox ID="txtComponentVersion" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p> 
        <div class="clear"></div>  
        
        <p class="confg_frm_txt" id="langComponentLibraryURL"><!--Component Library URL--> <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx"><asp:TextBox ID="txtDiUiLibUrl" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p> 
        <div class="clear"></div> 
        
    </div>    
    
    <h2 id="langShare"><!--Share--></h2> 
    <div class="confg_Adm_box">    
    
        <p class="confg_frm_txt" id="langFacebookApplicationId"><!--Facebook Application Id--> <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx"><asp:TextBox ID="txtFBAppID" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p> 
        <div class="clear"></div>      
        
        <p class="confg_frm_txt" id="langFacebookApplicationSK"><!--Facebook Application Secret Key--> <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx"><asp:TextBox ID="txtFBAppSecret" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p> 
        <div class="clear"></div> 
        
        <p class="confg_frm_txt" id="langTwitterApplicationId"><!--Twitter Application Id--> <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx"><asp:TextBox ID="txtTwAppID" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p> 
        <div class="clear"></div>   
        
        <p class="confg_frm_txt" id="langTwitterApplicationSK"><!--Twitter Application Secret Key--></p>
        <p class="confg_frm_inp_bx"><asp:TextBox ID="txtTwAppSecret" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p> 
        <div class="clear"></div>   
        
    </div> 

    <h2 id="langRegistry">Registry</h2> 
    <div class="confg_Adm_box">   
        <p class="confg_frm_txt" id="lngDSDSel"><!--Enable DSD Selection--></p>
        <p class="confg_frm_inp_bx_radbtn"><asp:CheckBox ID="chkDSDSel" runat="server" /></p> 
        <div class="clear"></div>
        
        <p class="confg_frm_txt" id="langRegistryAreaLevel"><%--Area Level--%></p>
        <p class="confg_frm_inp_bx"><asp:TextBox ID="txtAreaLevel" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p> 
        <div class="clear"></div>  
        
         <p class="confg_frm_txt" id="langRegistryMSDAreaId"><%--MSD Area Id--%></p>
        <p class="confg_frm_inp_bx"><asp:TextBox ID="txtMSDAreaId" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p> 
        <div class="clear"></div>
        
        <p class="confg_frm_txt" id="langNotifyViaEmail"><%--Notify Via Email--%></p>
        <p class="confg_frm_inp_bx_radbtn"><asp:CheckBox ID="chkNotifyViaEmail" runat="server" /></p> 
        <div class="clear"></div> 
        
        <p class="confg_frm_txt" id="langMappingAgeDefaultValue"><%--Default Value Of Age For Mapping--%></p>
        <p class="confg_frm_inp_bx"><asp:TextBox ID="txtAgeDefaultvalue" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p> 
        <div class="clear"></div> 
        
        <p class="confg_frm_txt" id="langMappingSexDefaultValue"><%--Default Value Of Sex For Mapping--%></p>
        <p class="confg_frm_inp_bx"><asp:TextBox ID="txtSexDefaultvalue" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p> 
        <div class="clear"></div> 
        
        <p class="confg_frm_txt" id="langMappingLocationDefaultValue"><%--Default Value Of Location For Mapping--%></p>
        <p class="confg_frm_inp_bx"><asp:TextBox ID="txtLocationDefaultvalue" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p> 
        <div class="clear"></div> 
        
        <p class="confg_frm_txt" id="langMappingFrequencyDefaultValue"><%--Default Value Of Frequency For Mapping--%></p>
        <p class="confg_frm_inp_bx"><asp:TextBox ID="txtFrequencyDefaultvalue" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p> 
        <div class="clear"></div> 
        
        <p class="confg_frm_txt" id="langMappingSourceDefaultValue"><%--Default Value Of Source For Mapping--%></p>
        <p class="confg_frm_inp_bx"><asp:TextBox ID="txtSourceDefaultvalue" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p> 
        <div class="clear"></div> 
        
        <p class="confg_frm_txt" id="langMappingNatureDefaultValue"><%--Default Value Of Nature For Mapping--%></p>
        <p class="confg_frm_inp_bx"><asp:TextBox ID="txtNatureDefaultvalue" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p> 
        <div class="clear"></div> 
        
        <p class="confg_frm_txt" id="langMappingUnitMultDefaultValue"><%--Default Value Of Unit Multiplier For Mapping--%></p>
        <p class="confg_frm_inp_bx"><asp:TextBox ID="txtUnitMultDefaultvalue" runat="server" class="confg_frm_inp_bx_txt"></asp:TextBox></p> 
        <div class="clear"></div>  

        <p class="confg_frm_txt" ><span id="spanCategoryScheme">Select CategoryScheme</span> <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx"><select id="selCategoryScheme"></select></p>
        <div class="clear"></div>
         
    </div>
    <!-- Input Fields Area ...ends-->                
    
    <!-- Configuration Update Button ...starts-->
    <div class="adm_upd_bttn"><input type="button" id="langUpdate" value="" onclick="UpdateConfiguration();" class="di_gui_button" /></div>
    <!-- Configuration Update Button ...ends-->   

    <script type="text/javascript">
        SetAppSettingsAdaptedArea('<%=AdaptedFor%>', '<%=AreaNId%>', '<%=SubNation%>');
        SelectLeftMenuItem("AppSettings");
        FillCategoryScheme("selCategoryScheme");
        var VAribaleDeclaredInpage = "<%=VAribaleDeclaredInpage%>";
    </script>    
</asp:Content>

