<%@ Page Language="C#" MasterPageFile="~/libraries/aspx/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="DefaultArea.aspx.cs" Inherits="libraries_aspx_Admin_DbEdit3" Title="Untitled Page" EnableSessionState="True" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">



<input type="hidden" id="langHiddenSelectAll" value="" />
<input type="hidden" id="langHiddenSelectarealevel" value="" />
<input type="hidden" id="langHiddenByTree" value="" />
<input type="hidden" id="langHiddenListAtoZ" value="" />
<input type="hidden" id="langHiddenAlphabetically" value="" />
<input type="hidden" id="langHiddenByMap" value="" />
<input type="hidden" id="langHiddenSearch" value="" />
<input type="hidden" id="langHiddenViewselections" value="" />
<input type="hidden" id="langHiddenMetadata" value="" />
<input type="hidden" id="langHiddenShowwheredataexist" value="" />
<input type="hidden" id="langHiddenClear" value="" />
<input type="hidden" id="langHiddenClose" value="" />
<input type="hidden" id="langHiddenFirst" value="" />
<input type="hidden" id="langHiddenBack" value="" />
<input type="hidden" id="langHiddenNext" value="" />
<input type="hidden" id="langHiddenLast" value="" />
<input type="hidden" id="langHiddenPage" value="" />

<input type="hidden" id="langHiddenTree" value="" />
<input type="hidden" id="langHiddenAtoZ" value="" />
<input type="hidden" id="langHiddenSector" value="" />
<input type="hidden" id="langHiddenGoal" value="" />
<input type="hidden" id="langHiddenSource" value="" />
<input type="hidden" id="langHiddenInstitution" value="" />
<input type="hidden" id="langHiddenTheme" value="" />
<input type="hidden" id="langHiddenConvention" value="" />
<input type="hidden" id="langHiddenConceptualFramework" value="" />
<input type="hidden" id="langHiddenselectsubgroup" value="" />
<input type="hidden" id="langHiddenOK" value="" />
<input type="hidden" id="langHiddenSubgroupDimensions" value="" />
<input type="hidden" id="langHiddenMore" value="" />

   
    <h1 id="lang_da_DatabaseSettings"><!--Database Settings--></h1> 
    <h4 id="lang_da_AddMod_DCDH"><!--Add and Modify database connection details here--></h4>          
    
    <h2 id="langDefaultIndicator" class="flt_lft"><!--Default Indicator--></h2>
    <p class="flt_lft co_hdin_txt">
        <label id="lblIndCounts" runat="server">0</label> <span id="langSelected"><!--Selected--></span> 
        <input type="button" value="..." class="di_gui_button" onclick="DoIndicatorSelection();" />
    </p>
    <div id="divSelectedInd" class="sel_ind_ar_box"></div>



    <h2 id="langSetDefaultArea" class="flt_lft"><!--Set default area--></h2> 
    <p class="flt_lft co_hdin_txt">
        <label id="lblAreaCounts" runat="server">0</label> <span id="langSelected_da"><!--Selected--></span>
        <input type="button" value="..." class="di_gui_button" onclick="DoAreaSelection();" />
    </p>
    <div id="divSelectedArea" class="sel_ind_ar_box"></div>
    
     
    
    <!-- Popup for Select Topic Component ...starts-->
    <div id="IndOuterBox" class="popup_block">	
        <h1 class="popup_heading" id="langSelectTopic"><!--Select Topic--></h1>
    
        <!-- Popup Inside Content ...starts-->
        <div class="popup_brd">    
            <div id="indicator_div_popup"></div>    
        </div>
        <!-- Popup Inside Content ...ends--> 
    
            <!-- Popup Buttons ...starts-->
            <div class="di_gui_button_pos"><input type="button" name="indOk" id="indOk" value="" onclick="SaveDefaultIndicator();" class="di_gui_button" /><input type="button" name="indCancel" id="indCancel" value="" onclick="HideIndAreaPopup()" class="di_gui_button" /></div>        
            <!-- Popup Buttons ...ends-->            

    </div>
    <!-- Popup for Select Topic Component ...ends-->


    <!-- Popup for Select Area Component ...starts-->
    <div id="AreaOuterBox" class="popup_block">
        <h1 class="popup_heading" id="langSelectArea"><!--Select Area--></h1>
    
        <!-- Popup Inside Content ...starts-->
        <div class="popup_brd">    
            <div id="area_div_popup"></div>                   
        </div>
        <!-- Popup Inside Content ...ends-->    

            <!-- Popup Buttons ...starts-->
            <div class="di_gui_button_pos"><input type="button" name="areaOk" id="areaOk" value="" onclick="SaveDefaultArea();" class="di_gui_button" /><input type="button" name="areaCancel" id="areaCancel" value="" onclick="HideIndAreaPopup()" class="di_gui_button" /></div>   
            <!-- Popup Buttons ...starts--> 

    </div>
    <!-- Popup for Select Area Component ...ends-->

    <br /><br />
    
    <!-- Buttons ...starts-->
    <div class="adm_upd_bttn" style="width:500px;">
    <span class="overlap_btn_pos" >
    <img id="imgGenerateSiteMapTickGray" alt='' src="../../../stock/themes/default/images/tickmark_grey.png" style="display:none;" />
    <img id="imgGenerateSiteMapProcessing" alt='' src="../../../stock/themes/default/images/processing.gif" style="display:none;" />
    <img id="imgGenerateSiteMapTick" alt='' src="../../../stock/themes/default/images/tickmark.png" style="display:none;" />
    <img id="imgGenerateSiteMapError" alt='' src="../../../stock/themes/default/images/error.png" style="display:none;" />
    </span>
    <input type="button" id="Btn_GenerateSiteMap" style="padding: 4px 23px;" value="" onclick="GenerateSiteMap();" class="di_gui_button" />
    <input type="button" id="btnlang_da_Back" value="" onclick="MoveOnOptimizeDb();" class="di_gui_button" />
    <input type="button" id="btnlangFinish" value="" onclick="window.location='DbMaster.aspx';" class="di_gui_button" />
    </div>
    <!-- Buttons ...ends-->     
   
    <asp:HiddenField ID="hdnSelIndO" runat="server" Value="" />
    <asp:HiddenField ID="hdnSelAreaO" runat="server" Value="" />
    
    <script type="text/javascript">
        var di_components = "Area,Indicator";	   
        var di_component_version = '<%=Global.diuilib_version%>';
        var di_theme_css = '<%=Global.diuilib_theme_css%>';    
        var di_diuilib_url = '<%=Global.diuilib_url%>';
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibincludes.js'" + "><\/script>");
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibcommon.js'" + "><\/script>");
    </script>
    
    <script type="text/javascript">        
        CookiePostfixStr = '_' + '<%=Global.CookiePostfixStr%>';
        LoadLanguage(readCookie("hlngcode"));
        SelectLeftMenuItem("DbSettings");
        ShowSelectedIndicators(di_jq("#" + AddCPH("hdnSelIndO")).val());
        ShowSelectedAreas(di_jq("#" + AddCPH("hdnSelAreaO")).val());

        ShowSiteMapOptimizeStatus('<%=IsSiteMapGenerated%>');
        if ('<%=ShowSiteMap%>' == "false") {
            di_jq("#Btn_GenerateSiteMap").hide();
        }     
    </script>
</asp:Content>

