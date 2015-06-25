<%@ Page Language="C#" MasterPageFile="~/libraries/aspx/RegistryMaster.master" AutoEventWireup="true"
    CodeFile="RegProviders.aspx.cs" Inherits="libraries_aspx_SDMX_sdmx_ProvisioningMetadata"
    Title="Provisioning Metadata" EnableSessionState="true" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphRegistryMainContent" runat="Server">
   
    <script type="text/javascript" src="../js/di.jquery-1.4.4.js?v=<%=Global.js_version%>"></script>
    <script type="text/javascript" src="../js/sdmx/Providers.js?v=<%=Global.js_version%>"></script>

    <script type="text/javascript">

        // create pivoting object
//        var DEMO = {};
//        window.cal = false;
//        DEMO.pivot = { panel: 1, tab: 0, div: "pivot", needs: ["pivot"], cb: CallBack }

        // initiation function for the pivoting
//        function init() {

//            /* tabs */
//            var tab = new OAT.Tab("content");
//            for (var p in DEMO) {
//                var name = DEMO[p].div;
//                tab.add("tab_" + name, name);
//            }

//            /* panelbar_content */
//            var pb = new OAT.Panelbar("panelbar", 10);
//            pb.addPanel("pb_1", "pb_11");

//            /* create dimmer element */
//            var dimmerElm = OAT.Dom.create("div", { border: "2px solid #000", padding: "1em", position: "absolute", backgroundColor: "#fff" });
//            document.body.appendChild(dimmerElm);
//            OAT.Dom.hide(dimmerElm);

//            for (var p in DEMO) { DEMO[p].drawn = false; }
//            tab.options.goCallback = function (oldIndex, newIndex) {
//                var oldName, newName;
//                for (var p in DEMO) {
//                    var v = DEMO[p];
//                    if (v.tab == oldIndex) { oldName = p; }
//                    if (v.tab == newIndex) { newName = p; }
//                }


//                var obj = DEMO[newName];
//                //OAT.Dimmer.show(dimmerElm);
//                OAT.Dom.center(dimmerElm, 1, 1);
//                var ref = function () {
//                    if (!window.location.href.match(/:source/)) {

//                        BindDataProviders();
//                        SetHiddenFieldValuesForOAT();

//                    }
//                }
//                OAT.Loader.loadFeatures(obj.needs, ref);
//                //alert('after loadfeatures');
//            }
//            pb.go(0);
//            tab.go(0);
//        }

</script>

	    <!-- Reg Left Section ...starts-->
          <div id="reg_wide_sec">
        <!-- User Grid ...starts-->
        <div id="divUsers" style="display:none;">
            <h3 id="lang_List_of_Users" class="flt_lft"><%-->List of Users:--%></h3>	 

            <table style="width:100%; float:left;" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <div id="divInnerUsers"></div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input type="button" id="btnUpdate" value="" class="di_gui_button di_gui_button_qry" onclick="UpdateUsers();"/>
                        <input type="button" id="btnReset" value="" class="di_gui_button di_gui_button_qry" onclick="GetAllUsersHTMLForAdmin();SetHiddenFieldValues();"/>
                    </td>
                </tr>
            </table>
        </div>   
        <!-- User Grid ...ends-->   
         
	    </div>
	    <!-- Reg Left Section ...ends--> 
	    
	    
<!-- Popup for Registration Details ...starts-->
<div id="divArtefacts" class="popup_block">	
    
    <!-- Popup Inside Content ...starts-->
    <div id="reg_content_containers">
        <h2><label id="lblArtefactType"></label></h2>
        <h5></h5>
        
        <div id="reg_wide_sec_ppup">
        
                <!-- Input Fields Area ...starts-->    
                <div id="confg_Adm_box_lgin" style="background-color:#ffffdd">
                    <pre id="preArtefacts" style="overflow:scroll;height:380px;width:664px;word-wrap:break-word; white-space:pre-wrap; padding:10px;"></pre>                    
                </div>
                <!-- Input Fields Area ...ends-->  
				

        </div>

    </div> 
    <!-- Popup Inside Content ...ends-->     
    
</div>      
<!-- Popup for Registration Details ...ends-->		

    
<!-- Popup For Help starts -->
<div id="divCallout" class="callout border-callout">
    <div id="divCalloutText" class="content">
         <!-- Popup For Help Text starts -->
            <div id="divHelpDPScheme" style="display:none">
              <span class="heading2" id="span_Help_Data_Provider_Scheme"></span>
              <br />
              <i class="content" id="i_Help_Data_Provider_Scheme"></i>
            </div>
            <div id="divHelpMAScheme" style="display:none">
              <span class="heading2" id="span_Help_Maintenance_Agency_Scheme"></span>
              <br />
              <i class="content" id="i_Help_Maintenance_Agency_Scheme"></i>
            </div>
            <div id="divHelpProvidersAndPA" style="display:none">
                  <div>
                       <span class="heading2" id="span_Help_Providers"></span>
                       <br />
                       <i class="content" id="i_Help_Providers"></i>
                 </div>
                 <div>
                       <span class="heading2" id="span_Help_PA"></span>
                       <br />
                       <i class="content" id="i_Help_PA"></i>
                 </div>
            </div>
            <!-- Popup For Help Text ends -->
    </div>
    <b class="border-notch notch"></b><b class="notch"></b>
</div>
<!-- Popup For Help ...ends -->	    	    
   <input type="hidden" id="hView" />
   <input type="hidden" id="hDownload" /> 
   <input  type="hidden" id="hEmail" />
   <input  type="hidden" id="hName" />
   <input  type="hidden" id="hCountry" />
   <input  type="hidden" id="hProvider" />
    <input  type="hidden" id="hUpdatedSuccess" />
    <input  type="hidden" id="hArtifactNotFound" />
<!-- For OAT Grid Purpose ...starts-->        
        <div id="tab_pivot333">
             <table width="100%" cellpadding="0" cellspacing="2" border="0" style="float:left;">
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

    <!-- END OF DEVELOPER CODE -->
</asp:Content>
