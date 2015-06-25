<%@ Page Title="" Language="C#" MasterPageFile="~/libraries/aspx/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="AdminCatalog.aspx.cs" Inherits="libraries_aspx_Admin_AdminCatalog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    
    <script type="text/javascript" src="../../js/Catalog.js?v=<%=Global.js_version%>"></script>
    <script type="text/javascript" src="../../js/jquery.ui.datepicker.js?v=<%=Global.js_version%>"></script>    

    <input type="hidden" id="langNumberOf" value="" />
    <input type="hidden" id="langGeographicalAreas" value="" />
    <input type="hidden" id="langVariables" value="" />
    <input type="hidden" id="langTimePeriod" value="" />
    <input type="hidden" id="langData" value="" />
    <input type="hidden" id="langLastUpdated" value="" />
    <input type="hidden" id="langKeywords" value="" />
    <input type="hidden" id="langPreviewData" value="" /> 
    <input type="hidden" id="langGotoData" value="" /> 

    <div id="divCatalog" style="display:block;">
        <h1 id="lang_Catalog"><!--Catalog--></h1> 
        <div class="flt_rgt" style="padding-right:15px;">
            <span>
                <span id="langCount"><%--Count: --%></span>
                <span id="spanAdaptationsCount">0</span>
                &nbsp;&nbsp;
            </span>
            <input type="button" id="btnAddNewAdpt" value="" onclick="ShowDivAddAdptDetails()" class="di_gui_button" />
        </div>
        <h4>&nbsp;</h4>
                
        <div id="divCatalogHTML">            
            <%=CatalogHtml%>
        </div>        
    </div>
<div class="clear"></div>
    <div id="divAddAdptDetails" style="display:none;">
        <h1 id="lang_Add_Adaptation_Details" style="display:none"><!--Add Adaptation Details--></h1>
        <h1 id="lang_Edit_Adaptation_Details" style="display:none"><!--Edit Adaptation Details--></h1>
        <h4></h4>    

    <!-- Input Fields Area ...starts-->    
    <div class="confg_Adm_box"> 

        <p class="confg_frm_txt"><span id="langAdaptationName"><!--Adaptation Name--></span> <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx"><input id="txtAdaptationName" type="text" class="confg_frm_inp_bx_txt" /></p>
        <div class="clear"></div>

        <p class="confg_frm_txt" id="lang_Description"><!--Description--></p>
        <p class="confg_frm_inp_bx"><textarea id="txtDescription" rows="5" cols="200" class="confg_frm_inp_bx_txt"></textarea></p>
        <div class="clear"></div>

        <p class="confg_frm_txt"><span id="lang_DevInfo_Version"><!--DevInfo Version--></span> <span class="Mandatory">*</span></p>        
        <p class="confg_frm_inp_bx">
            <select id="ddlVersion">                
            </select>
        </p>
        <div class="clear"></div>

        <p class="confg_frm_txt"><span id="lang_Unicef_Region"><%--Unicef Region--%></span> <span class="Mandatory">*</span></p>        
        <p class="confg_frm_inp_bx">
            <select id="ddlUnicefRegion">
                <option>CEE/CIS</option>
                <option>East Asia and Pacific</option>
                <option>Eastern and Southern Africa</option>
                <option>Global</option>
                <option>Industrialized Countries/Territories</option>
                <option>Latin America and Caribbean</option>
                <option>Middle East and North Africa</option>
                <option>South Asia</option>
                <option>Western and Central Africa</option>
                <option>World</option>
            </select>
        </p>
        <div class="clear"></div>

        <p class="confg_frm_txt"><span id="lang_Adpt_Year"><%--Adaptation Year--%></span> <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx"><input id="txtAdaptationYear" type="text" maxlength="4" class="confg_frm_inp_sub_bx_txt_smlst" onKeyPress="JavaScript:return AllowOnlyDigits(event,this);" /></p>
        <div class="clear"></div>

        <p class="confg_frm_txt"><span id="lang_Db_Lnaguages"><%--Database Languages--%></span> <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx"><input id="txtDatabaseLanguages" type="text" class="confg_frm_inp_bx_txt" /></p>
        <div class="clear"></div>
        
        <h4 class="confg_frm_dttd_btm_sp"></h4>

        <p class="confg_frm_txt"><span id="lang_Available_on_platform"><!--Available on platform--></span> <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx_big">
            <span class="confg_frm_inp_rgt_sp dsp_blck flt_lft">
                <input id="chkDesktop" type="checkbox" /><span id="lang_Desktop"><!--Desktop--></span> 
                <input id="chkWeb" type="checkbox" onclick="EnableWebURL(this.checked);" /><span id="lang_Web"><!--Web--></span> 
            </span>
            <span class="">            
                <span class="confg_frm_inp_sub_bx_txt confg_frm_inp_txt_algn" id="lang_Web_URL"><!--Web URL--></span> 
                <span class="confg_frm_inp_sub_cont"><input id="txtWebURL" type="text" disabled="disabled" class="confg_frm_inp_bx_txt_sml" /></span>
            </span>        
        </p>
        <div class="clear">&nbsp;</div>

        <p class="confg_frm_txt"><span id="lang_Counts"><!--Counts--></span> <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx_big">
            
            <span class="confg_frm_inp_sub_cont">
                <span class="confg_frm_inp_sub_bx_txt" id="lang_Area"><!--Area--></span> <br />
                <span class="dsp_blck"><input id="txtAreaCount" type="text" onKeyPress="JavaScript:return AllowOnlyDigits(event,this);" class="confg_frm_inp_sub_bx_txt_smlst" /></span>
            </span>

            <span class="confg_frm_inp_sub_cont">
                <span class="confg_frm_inp_sub_bx_txt" id="lang_IUS"><!--IUS--></span> <br />
                <span class="dsp_blck"><input id="txtIUSCount" type="text" onKeyPress="JavaScript:return AllowOnlyDigits(event,this);" class="confg_frm_inp_sub_bx_txt_smlst" /></span>
            </span>

            <span class="confg_frm_inp_sub_cont">
                <span class="confg_frm_inp_sub_bx_txt" id="lang_Time_Periods"><!--Time Periods--></span> <br />
                <span class="dsp_blck"><input id="txtTimePeriodsCount" type="text" onKeyPress="JavaScript:return AllowOnlyDigits(event,this);" class="confg_frm_inp_sub_bx_txt_smlst" /></span>
            </span>
            
            <span class="confg_frm_inp_sub_cont">
                <span class="confg_frm_inp_sub_bx_txt" id="lang_Data_Values"><!--Data Values--></span> <br />
                <span class="dsp_blck"><input id="txtDataValuesCount" type="text" onKeyPress="JavaScript:return AllowOnlyDigits(event,this);" class="confg_frm_inp_sub_bx_txt_smlst" /></span>
            </span>

        </p>
        <div class="clear">&nbsp;</div>

        <p class="confg_frm_txt"><span id="lang_Time_range"><!--Time range--></span> <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx_big">
                
            <span class="confg_frm_inp_sub_cont">
                <span class="confg_frm_inp_sub_bx_txt" id="lang_Start_Year"><!--Start Year--></span> <br />
                <span class="dsp_blck"><input id="txtStartYear" maxlength="4" type="text" onKeyPress="JavaScript:return AllowOnlyDigits(event,this);" class="confg_frm_inp_sub_bx_txt_smlst" /></span>
            </span>

            <span class="confg_frm_inp_sub_cont">
                <span class="confg_frm_inp_sub_bx_txt" id="lang_End_Year"><!--End Year--></span> <br />
                <span class="dsp_blck"><input id="txtEndYear" type="text" maxlength="4" onKeyPress="JavaScript:return AllowOnlyDigits(event,this);" class="confg_frm_inp_sub_bx_txt_smlst" /></span>  
            </span>

        </p>
        <div class="clear">&nbsp;</div>

        <p class="confg_frm_txt"><span id="lang_Last_modified_on"><!--Last modified on--></span> <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx"><input id="txtLastModifiedOn" type="text" class="confg_frm_inp_bx_txt" /></p>
        <div class="clear"></div>


        <h4 class="confg_frm_dttd_btm_sp"></h4>

        <p class="confg_frm_txt"><span id="lang_Db_Adm_Name"><%--Database administrator Name--%></span></p>
        <p class="confg_frm_inp_bx"><input id="txtDbAdmName" type="text" class="confg_frm_inp_bx_txt" /></p>
        <div class="clear"></div>

        <p class="confg_frm_txt"><span id="lang_Db_Adm_Institution"><%--Database administrator Institution--%></span></p>
        <p class="confg_frm_inp_bx"><input id="txtDbAdmInstitution" type="text" class="confg_frm_inp_bx_txt" /></p>
        <div class="clear"></div>

        <p class="confg_frm_txt"><span id="lang_Db_Adm_Email"><%--Database administrator Email--%></span></p>
        <p class="confg_frm_inp_bx"><input id="txtDbAdmEmail" type="text" class="confg_frm_inp_bx_txt" /></p>
        <div class="clear"></div>


        <h4 class="confg_frm_dttd_btm_sp"></h4>

        <p class="confg_frm_txt"><span id="lang_Adapted_for"><!--Adapted for--></span> <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx"><input id="rbGlobal" type="radio" name="Adapted" onclick="SelectAdaptedFor('Global');" checked /><span id="lang_Global"><!--Global--></span> <input id="rbNational" type="radio" name="Adapted" onclick="SelectAdaptedFor('National');" /><span id="lang_National"><!--National--></span> <input id="rbSubNational" type="radio" name="Adapted" onclick="SelectAdaptedFor('Sub-national');" /><span id="lang_Sub-national"><!--Sub-national--></span> </p>
        <div class="clear"></div>

        <p class="confg_frm_txt"><span id="lang_Select_country"><!--Select country--></span> <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx"><select id="ddlSelectCountry" disabled="disabled" class="confg_frm_inp_bx_txt_dd"></select></p>
        <div class="clear"></div>

        <p class="confg_frm_txt"><span id="lang_Specify_sub-nation_IA"><!--Specify sub-nation if any--></span> <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx"><input id="txtSubNation" type="text" class="confg_frm_inp_bx_txt" disabled="disabled" /></p>
        <div class="clear"></div>

        <p class="confg_frm_txt"><span id="lang_Catalog_image"><!--Catalog image--></span> <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx_big">
            <form id="frmUploadLogoImageFile" name="frmUploadLogoImageFile" action="../UploadFile.aspx" enctype="multipart/form-data" method="post" >            
                <input id="flCatalogImage" name="flCatalogImageName" type="file" class="confg_frm_inp_bx_txt_reg di_gui_button"  size="30%"/>
                <input type="submit" name="lang_Upload" id="lang_Upload" value="Upload Image" class="di_gui_button" />            
		    </form>	
        </p>
        <div class="clear"></div>

        <p class="confg_frm_txt"></p>
        <p class="confg_frm_inp_bx_big"><img id="imgPreview" alt="" src="../../../stock/themes/default/images/spacer.gif" class="confg_frm_inp_rgt_sp" /><label class="fnt_itl" id="lang_dimentions_135_105"><!--dimentions 135x105--></label></p>        
     
    </div> 
    <!-- Input Fields Area ...ends-->

    <div>&nbsp;</div>

    <!-- Button ...starts-->  
    
    <div class="adm_upd_bttn"><input type="button" value="" id="lang_Save_Adaptation" onclick="SaveAdaptation();" class="di_gui_button" style="display:none" /> <input type="button" value="" id="lang_Update_Adaptation" onclick="UpdateAdaptation();" class="di_gui_button" style="display:none" /> <input type="button" value="" id="btnCancel" onclick="ShowCatalogLists(false);" class="di_gui_button" /> </div>
    <!-- Button ...ends-->  
    </div>           

    <script type="text/javascript" src="../../js/di.jquery.form.js?v=<%=Global.js_version%>"></script>

    <script type="text/javascript">
        var CurrentAdaptationLogoUrl = "";

        di_jq(document).ready(function () {

            SelectLeftMenuItem("Catalog");
            
            //di_jq("#txtLastModifiedOn").datepicker({ dateFormat: 'dd-mm-yy' });                       

            FillAdaptationVersionDdl();

            di_jq(document).click(function (e) {
                // call function to hide opened divs
                HideAdapCallOutDiv(e);
            });

            di_jq('#frmUploadLogoImageFile').ajaxForm({
                success: function (response) {
                    if (response == "No File") {
                        alert("Please browse an image file.");
                    }
                    else {
                        di_jq("#imgPreview").attr("src", response);
                        CurrentAdaptationLogoUrl = response;
                    }
                }

            });

            startRenderingAdaptations(true);
        });
    </script>

</asp:Content>
