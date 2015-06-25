<%@ Page Language="C#" MasterPageFile="~/libraries/aspx/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="OptimizeDb.aspx.cs" Inherits="libraries_aspx_Admin_DbEdit2" Title="Untitled Page" EnableSessionState="True" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <h1 id="lang_db_DatabaseSettings"><!--Database Settings--></h1> 
    <h4 id="lang_db_AddMod_DCDH"><!--Add and Modify database connection details here--></h4>          
    
    <h2 id="langOptimizingDatabase"><!--Optimizing Database--></h2> 

    <input type="hidden" id="hcustomParams" value="<%= hCustomParams %>" />
    <input type="hidden" id="langAdminCustomParams" value="" />
    <input type="hidden" id="enableSDMXPublishData" value="<%=EnableSDMXPublishCheck %>" />
    <!-- Input Fields Area ...starts-->    
    <div class="confg_Adm_box">      
        <div class="adm_db_flt_lft" style="display:none">
            <img id="imgDBTickGray" alt='' src="../../../stock/themes/default/images/tickmark_grey.png" style="display:none;" />
            <img id="imgDBProcessing" alt='' src="../../../stock/themes/default/images/processing.gif" style="display:none;" />
            <img id="imgDBTick" alt='' src="../../../stock/themes/default/images/tickmark.png" style="display:block;" />
            <img id="imgDBError" alt='' src="../../../stock/themes/default/images/error.png" style="display:none;" />
        </div>
        <div class="adm_db_opt_chk"><input id="chkDBGeneration" type="checkbox" style="display:none" checked="checked"/></div>
        <div style="display:none" class="adm_o_db_txt" id="langUpdateDBSchema"><%--Database Objects Generation--%></div>    
               
        <div class="adm_db_flt_lft" >            
            <img id="imgXMLTickGray" alt='' src="../../../stock/themes/default/images/tickmark_grey.png" style="display:none;" />
            <img id="imgXMLProcessing" alt='' src="../../../stock/themes/default/images/processing.gif" style="display:none;" />
            <img id="imgXMLTick" alt='' src="../../../stock/themes/default/images/tickmark.png" style="display:none;" />
            <img id="imgXMLError" alt='' src="../../../stock/themes/default/images/error.png" style="display:none;" />
        </div>        
        <div class="adm_db_opt_chk"><input id="chkXMLGeneration" type="checkbox" checked="checked" /></div>
        <div class="adm_o_db_txt" id="langXMLGeneration"><!--XML Generation--></div>
        
        <div class="adm_o_db_subtxt" style="padding-left:90px">
            <p><input id="chkXmlSelectAll" type="checkbox" onclick="SelectAllXmlGeneration(this);" /><span id="langSelectAll"><!--Select All--></span></p>            
            <p><input id="chkXmlArea" type="checkbox" value="area" onclick="SelectAllChkStatus();" /><span id="langArea"><!--Area--></span></p>
            <div class="adm_o_db_sub_subtxt">
                <span id="lngOrderBy"><!--Order By--></span>
                <input id="rbSortAreaName" type="radio" name="AreaSort" value="AreaName" checked="checked" /><span id="langAreaName"><!--Area Name--></span>
                <input id="rbSortAreaId" type="radio" name="AreaSort" value="AreaId" /><span id="langAreaId"><!--Area Id--></span>          
            </div>

            <p><input id="chkXmlFootnotes" type="checkbox" value="footnotes" onclick="SelectAllChkStatus();" /><span id="langFootnotes"><!--Footnotes--></span></p>
            <p><input id="chkXmlIC" type="checkbox" value="ic" onclick="SelectAllChkStatus();" /><span id="langIC"><!--IC--></span></p>
            <p><input id="chkXmlICIUS" type="checkbox" value="ic_ius" onclick="SelectAllChkStatus();" /><span id="langICIUS"><!--IC IUS--></span></p>
            <p><input id="chkXmlIUS" type="checkbox" value="ius" onclick="SelectAllChkStatus();" /><span id="langIUS"><!--IUS--></span></p>
            <p><input id="chkXmlMetadata" type="checkbox" value="metadata" onclick="SelectAllChkStatus();" /><span id="langMetadata"><!--Metadata--></span></p>   
            
            <p><input id="chkXmlAreaQuickSearch" type="checkbox" value="quicksearch" onclick="SelectAllChkStatus();" /><span id="langQuickSearch"><!--Quick Search--></span></p>
                <div class="adm_o_db_sub_subtxt">
                    <input id="rbQSImmediate" type="radio" name="QuickSearch" value="Immediate" checked="checked" /><span id="langImmediate"> <!--Immediate--></span>
                    <input id="rbQSAll" type="radio" name="QuickSearch" value="All" /><span id="langAll"> <!--All--></span>
                    <input id="rbQSNone" type="radio" name="QuickSearch" value="None" /><span id="langNone"> <!--None--></span>          
                </div>
            <p><input id="chkXmlTimePeriods" type="checkbox" value="tp" onclick="SelectAllChkStatus();" /><span id="langTimePeriods"><!--Time Periods--></span></p>
        </div> 
                
        <div class="adm_db_flt_lft">
            <img id="imgMapTickGray" alt='' src="../../../stock/themes/default/images/tickmark_grey.png" style="display:none;" />
            <img id="imgMapProcessing" alt='' src="../../../stock/themes/default/images/processing.gif" style="display:none;" />
            <img id="imgMapTick" alt='' src="../../../stock/themes/default/images/tickmark.png" style="display:none;" />
            <img id="imgMapError" alt='' src="../../../stock/themes/default/images/error.png" style="display:none;" />
        </div>
        <div class="adm_db_opt_chk"><input id="chkMapGeneration" type="checkbox" checked="checked" /></div>
        <div class="adm_o_db_txt" id="langMapFilesGeneration"><%--Map Files Generation--%></div>

        <div class="adm_db_flt_lft" style="display:none;">
            <img id="imgSDMXTickGray" alt='' src="../../../stock/themes/default/images/tickmark_grey.png" style="display:none;" />
            <img id="imgSDMXProcessing" alt='' src="../../../stock/themes/default/images/processing.gif" style="display:none;" />
            <img id="imgSDMXTick" alt='' src="../../../stock/themes/default/images/tickmark.png" style="display:none;" />
            <img id="imgSDMXError" alt='' src="../../../stock/themes/default/images/error.png" style="display:none;" />
        </div>
        <div class="adm_db_opt_chk"><input id="chkSDMXGeneration" type="checkbox"  style="display:none;" /></div>
        <div class="adm_o_db_txt" id="langSDMXArtifactsGeneration" style="display:none;"><!--SDMX Artifacts Generation--></div>

        <div class="adm_db_flt_lft" style="display:none;">
            <img id="imgSDMXMLTickGray" alt='' src="../../../stock/themes/default/images/tickmark_grey.png" style="display:none;" />
            <img id="imgSDMXMLProcessing" alt='' src="../../../stock/themes/default/images/processing.gif" style="display:none;" />
            <img id="imgSDMXMLTick" alt='' src="../../../stock/themes/default/images/tickmark.png" style="display:none;" />
            <img id="imgSDMXMLError" alt='' src="../../../stock/themes/default/images/error.png" style="display:none;" />
        </div>
        <div class="adm_db_opt_chk"><input id="chkSDMXMLGeneration" type="checkbox" style="display:none;"/></div>
        <div class="adm_o_db_txt" id="langSDMXMLGeneration" style="display:none;"><!--SDMX ML Files Generation--></div>

        <div class="adm_db_flt_lft" style="display:none;">
            <img id="imgMetadataTickGray" alt='' src="../../../stock/themes/default/images/tickmark_grey.png" style="display:none;" />
            <img id="imgMetadataProcessing" alt='' src="../../../stock/themes/default/images/processing.gif" style="display:none;" />
            <img id="imgMetadataTick" alt='' src="../../../stock/themes/default/images/tickmark.png" style="display:none;" />
            <img id="imgMetadataError" alt='' src="../../../stock/themes/default/images/error.png" style="display:none;" />
        </div>
        <div class="adm_db_opt_chk"><input id="chkMetadataGeneration" type="checkbox" style="display:none;"/></div>
        <div class="adm_o_db_txt" id="langMetadataGeneration" style="display:none;"><!--Metadata Report Generation--></div>
        
        <div class="adm_db_flt_lft" >
            <img id="imgCacheTickGray" alt='' src="../../../stock/themes/default/images/tickmark_grey.png" style="display:none;" />
            <img id="imgCacheProcessing" alt='' src="../../../stock/themes/default/images/processing.gif" style="display:none;" />
            <img id="imgCacheTick" alt='' src="../../../stock/themes/default/images/tickmark.png" style="display:none;" />
            <img id="imgCacheError" alt='' src="../../../stock/themes/default/images/error.png" style="display:none;" />
        </div>
        <div class="adm_db_opt_chk"><input id="chkDBCacheResultsGeneration" type="checkbox" onclick="CheckCustomParams();"/></div>
        <div class="adm_o_db_txt" id="langGenerateCacheResults"><!--Generate QDS search results--></div>

        <div id="divCustomParams" style="display:none;">
       <div class="adm_o_db_txt"><label id="lblCustomParameters">Custom Parameters (--Optional--)</label></div>
       <div class="adm_o_db_txt"><input id="txtCustomParams" type="text" /><!--Optional parameters--></div>
        </div>

        <div class="adm_db_flt_lft" style="display:none;">
            <img id="imgRegisterSDMXMLTickGray" alt='' src="../../../stock/themes/default/images/tickmark_grey.png" style="display:none;" />
            <img id="imgRegisterSDMXMLProcessing" alt='' src="../../../stock/themes/default/images/processing.gif" style="display:none;" />
            <img id="imgRegisterSDMXMLTick" alt='' src="../../../stock/themes/default/images/tickmark.png" style="display:none;" />
            <img id="imgRegisterSDMXMLError" alt='' src="../../../stock/themes/default/images/error.png" style="display:none;" />
        </div>
        <div class="adm_db_opt_chk"><input id="chkSDMXMLRegistration" type="checkbox" style="display:none;"/></div>
        <div class="adm_o_db_txt" id="langSDMXMLRegistration" style="display:none;"><!--SDMX ML Files Registration And Constraint Creation--></div>

        <div class="adm_db_flt_lft" style="display:none;">
            <img id="imgRegisterMRTickGray" alt='' src="../../../stock/themes/default/images/tickmark_grey.png" style="display:;" />
            <img id="imgRegisterMRProcessing" alt='' src="../../../stock/themes/default/images/processing.gif" style="display:none;" />
            <img id="imgRegisterMRTick" alt='' src="../../../stock/themes/default/images/tickmark.png" style="display:none;" />
            <img id="imgRegisterMRError" alt='' src="../../../stock/themes/default/images/error.png" style="display:none;" />
        </div>
        <div class="adm_db_opt_chk"><input id="chkMRRegistration" type="checkbox" style="display:none;"/></div>
        <div class="adm_o_db_txt" id="langMRRegistration" style="display:none;"><!--Metadata Report Registration--></div>

        <div class="adm_db_flt_lft">
            <img id="imgGenerateCSVTickGray" alt='' src="../../../stock/themes/default/images/tickmark_grey.png" style="display:none;" />
            <img id="imgGenerateCSVProcessing" alt='' src="../../../stock/themes/default/images/processing.gif" style="display:none;" />
            <img id="imgGenerateCSVTick" alt='' src="../../../stock/themes/default/images/tickmark.png" style="display:none;" />
            <img id="imgGenerateCSVError" alt='' src="../../../stock/themes/default/images/error.png" style="display:none;" />
        </div>
        <div class="adm_db_opt_chk"><input id="chkGenerateCSVFile" type="checkbox" /></div>
        <div class="adm_o_db_txt" id="LangGenerateCSVFile"><!--CSV File Registration--></div>  
               

        <div id="divPublishSDMXData" class="adm_db_flt_lft">
            <img id="imgPublishSDMXDataTickGray" alt='' src="../../../stock/themes/default/images/tickmark_grey.png" style="display:none;" />
            <img id="imgPublishSDMXDataProcessing" alt='' src="../../../stock/themes/default/images/processing.gif" style="display:none;" />
            <img id="imgPublishSDMXDataTick" alt='' src="../../../stock/themes/default/images/tickmark.png" style="display:none;" />
            <img id="imgPublishSDMXDataError" alt='' src="../../../stock/themes/default/images/error.png" style="display:none;" />
        </div>
        <div id="dvSMchk" class="adm_db_opt_chk"><input id="chkPublishSDMXData" type="checkbox" /></div>
        <div class="adm_o_db_txt" id="LangPublishSDMXDataDI"><!--Devinfo Publish--></div>
       
             
        <div id="divPublishDataCountryData" class="adm_db_flt_lft">
            <img id="imgPublishSDMXDataTickGrayCD" alt='' src="../../../stock/themes/default/images/tickmark_grey.png" style="display:none;" />
            <img id="imgPublishSDMXDataProcessingCD" alt='' src="../../../stock/themes/default/images/processing.gif" style="display:none;" />
            <img id="imgPublishSDMXDataTickCD" alt='' src="../../../stock/themes/default/images/tickmark.png" style="display:none;" />
            <img id="imgPublishSDMXDataErrorCD" alt='' src="../../../stock/themes/default/images/error.png" style="display:none;" />
        </div>
              <div id="divPublishDataCountryDatachk" class="adm_db_opt_chk">
       <input id="chkPublishSDMXDataCountryData" runat="server" type="checkbox" disabled="disabled" />
  </div>
        <div class="adm_o_db_txt" id="LangPublishSDMXDataCD"><!--Country Data Publish--></div>
       
        
        
    </div> 
    <!-- Input Fields Area ...ends-->                
    
    <!-- Configuration Update Button ...starts-->
    <div class="adm_upd_bttn">
        <input type="button" id="langOptimizeDatabase" value="" onclick="OptimizeDatabase();" class="di_gui_button lng_bttn_sz" />
        <%--<input type="button" id="btnlang_db_Back" value="" onclick="MoveOnProviderDetails();" class="di_gui_button" />--%>
        <input type="button" id="btnlang_db_Back" value="" onclick="MoveOnConnectionDetails();" class="di_gui_button" />
        <input type="button" id="btnOptDbNext" value="" onclick="MoveOnDefaultArea()" class="di_gui_button" />
    </div>
    <!-- Configuration Update Button ...ends-->          
    
    
    <!-- DEVELOPER CODE -->
    <script type="text/javascript">
        SelectLeftMenuItem("DbSettings");
        IsCsvFileGenerated = 'false'
        if (getCsvFilesName() != '') {
            IsCsvFileGenerated = 'true';
        }
        var IsSDMXDataPublished = '<%=IsSDMXDataPublished %>';
        ShowOptimizeStatus('<%=IsXmlGenerated%>', '<%=IsXmlAreaGenerated%>', '<%=IsXmlFootnotesGenerated%>', '<%=IsXmlICGenerated%>', '<%=IsXmlICIUSGenerated%>', '<%=IsXmlIUSGenerated%>', '<%=IsXmlMetadataGenerated%>', '<%=IsXmlQuickSearchGenerated%>', '<%=IsXmlTimePeriodsGenerated%>', '<%=IsMapGenerated%>', '<%=IsSDMXGenerated%>', '<%=IsSDMXMLGenerated%>', '<%=IsMetadataGenerated%>', '<%=IsCacheResultGenerated%>', '<%=IsSDMXMLRegistered%>', IsCsvFileGenerated,'<%=Global.IsSDMXDataPublished %>');
    </script>
    <!-- END OF DEVELOPER CODE -->

</asp:Content>

