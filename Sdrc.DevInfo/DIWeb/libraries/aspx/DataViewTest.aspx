<%@ Page Language="C#" MasterPageFile="~/libraries/aspx/HomeMaster.master" EnableSessionState="false"
    AutoEventWireup="true" CodeFile="DataViewTest.aspx.cs" Inherits="libraries_aspx_DataView"
    Title="Untitled Page" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphHeadContent" runat="Server">
    <!-- START DEVELOPER CODE -->

    <script type="text/javascript">
	var featureList=["tab","panelbar","dialog"];
    </script>

    <!--<script type="text/javascript" src="../js/jquery/jquery-1.4.2.min.js"></script>
<script type="text/javascript" >
	var jq=jQuery.noConflict();
</script>-->
    <!-- Page Markup starts from here -->

    <script type="text/javascript" src="<%=Global.diuilib_url%>/1.9/js/oatgrid/loader.js"></script>

    <script src="../../stock/themes/default/spryassets/sprycollapsiblepanel.js" type="text/javascript"></script>

    <script type="text/javascript" src="../js/DataViewTest.js"></script>
    <script type="text/javascript" src="../js/visualizer.js"></script>

    <link rel="stylesheet" type="text/css" href="<%=Global.diuilib_url%>/1.9/css/oatgrid/pivot_style.css" />
    <link href="../../stock/themes/default/spryassets/sprycollapsiblepanel.css" rel="stylesheet"
        type="text/css" />

    <script type="text/javascript">

// create pivoting object
var DEMO = {};
window.cal = false;
DEMO.pivot = {panel:1,tab:0,div:"pivot",needs:["pivot"],cb:CallBack}

// initiation function for the pivoting
function init(){	
   
	/* tabs */
	var tab = new OAT.Tab("content");
	for (var p in DEMO) {
		var name = DEMO[p].div;
		tab.add ("tab_" + name, name);
	}

	/* panelbar_content */
	var pb = new OAT.Panelbar("panelbar",10);
	pb.addPanel("pb_1","pb_11");
	
	/* create dimmer element */
	var dimmerElm = OAT.Dom.create("div",{border:"2px solid #000",padding:"1em",position:"absolute",backgroundColor:"#fff"});
	document.body.appendChild(dimmerElm);
	OAT.Dom.hide(dimmerElm);
	
	for (var p in DEMO) { DEMO[p].drawn = false; }
	tab.options.goCallback = function(oldIndex,newIndex) {
		var oldName, newName;
		for (var p in DEMO) {
			var v = DEMO[p];
			if (v.tab == oldIndex) { oldName = p; }
			if (v.tab == newIndex) { newName = p; }
		}
		
		
		var obj = DEMO[newName];
				OAT.Dimmer.show(dimmerElm);
				OAT.Dom.center(dimmerElm,1,1);
				var ref = function() {  
					if (!window.location.href.match(/:source/)) { OAT.Dimmer.hide(); }			

		
				}
				
				OAT.Loader.loadFeatures(obj.needs,ref);
			}
			pb.go(0);
			tab.go(0);
}

    </script>

    <!-- END DEVELOPER CODE -->
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContent" runat="Server">
    <!-- Div tag launching visualizer component -->
    <div id="popup_name" class="popup_block" style="height: 600px; width: 800px;">
        <table width='100%' height='100%' cellpadding="0" cellspacing="0">
            <tr>                
                <td>
                    <span>Layout</span>&nbsp;&nbsp;
                    <select id="layout_ddl" onchange="selectLayout(this)">
                        <option value="default">Default</option>
                        <option value="1">Layout 1</option>
                        <option value="2">Layout 2</option>
                        <option value="3">Layout 3</option>
                        <option value="4">Layout 4</option>
                        <option value="5">Layout 5</option>
                        <option value="6">Layout 6</option>
                        <option value="7">Layout 7</option>
                   </select> 
                </td>                
                <td>
                    <span>Background Color</span>&nbsp;&nbsp;
                    <select id="bgColor_ddl" onchange="selectBgColor(this)">                    
                    </select>
                </td>
                <td>
                    <a href="javascript:void(0)" onclick="showChartStyle('styleDiv')">Chart Style</a>                    
                </td>
                <td>
                    <a href="javascript:void(0)" onclick="showFontDiv('formatDiv')">Format</a>                    
                </td>
                <td>
                    <a href="javascript:void(0)" onclick="sharedPresentation()">Share</a>                    
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <div id="divVis" style="width: 800px; height: 550px;">
                    </div>
                </td>
            </tr>
        </table>        
    </div>
    <div id="GradOutDiv" style="width: 100%; height: 100%; background-color: #777777;
        position: absolute; left: 0px; top: 0px; z-index: 149; opacity: 0.7; filter: alpha(opacity=70);
        display: block;">
        <center>
            <div style="position: absolute; top: 25%; left: 50%; background-color: White;">
                <img src="../../stock/themes/default/images/loading_img.gif" id="imgLoader" width="24" height="24" />
            </div>
        </center>
    </div>
    <div id='IndOuterBox' style="padding:1px;background-color:#ffffff;border:1px solid #A6C9E2; width:723px; height: auto;z-index:1200; position: fixed; display:none;left:25%;">
		<div id='IndcloseImgDiv' style="margin-top:-15px;margin-left:-15px;position:absolute;left:100%;width:auto; height: auto; z-index:1000; text-align:right;"><a onclick="showHideComponent('close');" href="javascript:void(0)"><img src="../../stock/themes/default/images/close.png" width="30" height="30" border="0"></a></div>
		<table cellpadding="0" cellspacing="0" border="0">
		 <tr>
		  <td valign="top" style="padding:10px;">
		   <table cellpadding="0" cellspacing="0" border="0" style="border:1px solid #d3d3d3;">
		    <tr>
			 <td><div id="indicator_div"></div></td>
			</tr>
			<tr>
			 <td><div style="text-align:right;padding-top:5px;padding-right:5px;padding-bottom:5px;"><input type="button" name="indOk" id="indOk" value="OK" onclick="onClickOkPopUpButton('ind')" class="di_gui_button">&nbsp;<input type="button" name="indCancel" id="indCancel" value="Cancel" onclick="showHideComponent('close')" class="di_gui_button"></div></td>
			</tr>
		   </table>
		  </td>
		 </tr>
		</table>
	</div>
    <%--<div id='IndOuterBox' style="padding: 1px; background-color: #ffffff; border: 1px solid #A6C9E2;
        width: 705px; height: auto; z-index: 1200; position: absolute; display: none;
        left: 25%;">
        <div id='closeImgDiv' style="margin-top: -15px; margin-left: -15px; position: absolute;
            left: 100%; width: auto; height: auto; z-index: 1000; text-align: right;">
            <a onclick="showHideComponent('close');" href="javascript:void(0)">
                <img src="../../stock/themes/default/images/close.png" width="30" height="30" border="0"></a></div>
        <div id="indicator_div"></div>
        <br>
        <div style="text-align: right; padding-top: 20px; padding-right: 5px; padding-bottom: 5px;">
            <input type="button" name="indOk" id="indOk" value="OK" onclick="onClickOkPopUpButton('ind')"
                class="di_gui_button">
            &nbsp;
            <input type="button" name="indCancel" id="indCancel" value="Cancel" onclick="showHideComponent('close')"
                class="di_gui_button"></div>
    </div>--%>
    <%--<div id='AreaOuterBox' style="padding: 1px; background-color: #ffffff; border: 1px solid #A6C9E2;
        width: 705px; height: auto; z-index: 1200; position: absolute; display: none;
        left: 25%;">
        <div id='closeImgDivArea' style="margin-top: -15px; margin-left: -15px; position: absolute;
            left: 100%; width: auto; height: auto; z-index: 1000; text-align: right;">
            <a onclick="showHideComponent('close');" href="javascript:void(0)">
                <img src="../../stock/themes/default/images/close.png" width="30" height="30" border="0" /></a></div>
        <div id="area_div"></div>
        <br>
        <div style="text-align: right; padding-top: 20px; padding-right: 5px; padding-bottom: 5px;">
            <input type="button" name="areaOk" id="areaOk" value="OK" onclick="onClickOkPopUpButton('area')"
                class="di_gui_button" />
            &nbsp;
            <input type="button" name="areaCancel" id="areaCancel" value="Cancel" onclick="showHideComponent('close')"
                class="di_gui_button" /></div>
    </div>--%>
    
    <div id='AreaOuterBox' style="padding:1px;background-color:#ffffff;border:1px solid #A6C9E2; width:723px; height: auto;z-index:1200; position: fixed; display:none;left:25%;">
		<div id='closeImgDivArea' style="margin-top:-15px;margin-left:-15px;position:absolute;left:100%;width:auto; height: auto; z-index:1000; text-align:right;"><a onclick="showHideComponent('close');" href="javascript:void(0)"><img src="../../stock/themes/default/images/close.png" width="30" height="30" border="0"></a></div>
		<table cellpadding="0" cellspacing="0" border="0">
		 <tr>
		  <td valign="top" style="padding:10px;">
		   <table cellpadding="0" cellspacing="0" border="0" style="border:1px solid #d3d3d3;">
		    <tr>
			 <td><div id="area_div"></div></td>
			</tr>
			<tr>
			 <td><div style="text-align:right;padding-top:5px;padding-right:5px;padding-bottom:5px;"><input type="button" name="areaOk" id="areaOk" value="OK" onclick="onClickOkPopUpButton('area')" class="di_gui_button">&nbsp;<input type="button" name="areaCancel" id="areaCancel" value="Cancel" onclick="showHideComponent('close')" class="di_gui_button"></div></td>
			</tr>
		   </table>
		  </td>
		 </tr>
		</table>
	</div>
    
    <div id='divDataValueRange' style="padding: 1px; background-color: #ffffff; border: 1px solid #A6C9E2;
        width: 350px; height: auto; z-index: 1200; position: absolute; display: none; top: 40%;
        left: 40%;">
        <div id='closeImgDivDV' style="margin-top: -15px; margin-left: -15px; position: absolute;
            left: 100%; width: auto; height: auto; z-index: 1000; text-align: right;">
            <a onclick="showHideComponent('close');" href="javascript:void(0)">
                <img src="../../stock/themes/default/images/close.png" alt="Close" width="30" height="30"
                    border="0" /></a></div>
        <div id="divDataValueRangeSpace" style="padding-top: 20px; padding-left: 10px; padding-right: 10px;">
            <select name="select" id="allIndicators" style="width: 100%;">
            </select>
            <br />
            <center>
                <table width="100%" border="0" align="right" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="left" style="width: 46px; height: 30px;">
                            From :
                        </td>
                        <td width="153" style="height: 30px">
                            <input name="textfield" type="text" value="" id="txtDvStart" size="15" />
                        </td>
                    </tr>
                    <tr>
                        <td height="30" align="left" style="width: 46px">
                            To :
                        </td>
                        <td>
                            <input name="textfield" type="text" value="" id="txtDvEnd" size="15" />
                        </td>
                    </tr>
                </table>
            </center>
            
        </div>
        <br />
        <div style="text-align: right; padding-top: 20px; width:100%; padding-right: 5px; padding-bottom: 5px;">
            <input type="button" name="areaOk" value="OK" onclick="onClickOkPopUpButton('dv')"
                class="di_gui_button" />
            &nbsp;
            <input type="button" name="areaCancel" value="Cancel" onclick="showHideComponent('close');"
                class="di_gui_button" /></div>
    </div>
    <div class="inner_content_container">
        <table border="0" align="center" cellpadding="0" cellspacing="0" class="panel_table">
            <tr>
                <td valign="top">
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <!-- Container section start -->
                                <table width="100%" cellpadding="0" cellspacing="2" border="0">
                                    <tr>
                                        <td colspan="2" height="50">
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td valign="top">
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td class="heading1" style="height: 30px">
                                                                    Data View <%--<b>Database </b> <a href="" id="dbname">Hello</a>--%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table width="95%" border="0" cellspacing="0" cellpadding="0">
                                                                        <tr>
                                                                            <td width="9" height="64" valign="top">
                                                                                <img src="../../stock/themes/default/images/search_left.png" width="9" height="64" /></td>
                                                                            <td align="left" valign="top">
                                                                                <div id="quick_data_search_div">
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td height="20" align="right" colspan="2">
                                                                                <a href="#" class="adv_search">Advanced Search</a></td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td width="2">
                                                        <a href="home.aspx">
                                                            <img src="../../stock/themes/default/images/divider.png" width="2" height="80" border="0" /></a></td>
                                                    <td width="600">
                                                        <table width="580" border="0" align="right" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td width="7">
                                                                    <img src="../../stock/themes/default/images/visualization_left.png" width="7" height="64" /></td>
                                                                <td width="581" class="visualization_bg">
                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                        <tr>
                                                                            <td width="7%" height="64">
                                                                                <a href="#">
                                                                                    <img src="../../stock/themes/default/images/play_l.png" width="29" height="29" border="0" /></a></td>
                                                                            <td width="8%">
                                                                                <a id="btncolumn" href="#" onclick="showVisualizerPopup('column')">
                                                                                    <img src="../../stock/themes/default/images/column.png" name="Image8" width="44"
                                                                                        height="60" border="0" id="Image8" /></a></td>
                                                                            <td width="80%">
                                                                                <img src="../../stock/themes/default/images/other_icons.png" border="0" width="442" height="60" usemap="#visicons"/>
																				<map name="visicons">
																				   <area shape="rect" 
																							coords="0,3,43,53"
																							href="#" onclick="javascript:showVisualizerPopup('line');" alt="Line Chart" 
																							onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
																							onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>
																				   <area shape="rect" 
																							coords="43,3,84,53"
																							href="#" onclick="javascript:showVisualizerPopup('stack');" alt="Pie Chart" 
																							onMouseOver="javascript:ShowVisIconsRollOverImage('Pie');" 
																							onMouseOut="javascript:ShowVisIconsRollOverImage('Pie');"/>
																				   <area shape="rect" 
																							coords="84,4,125,54"
																							href="#" onclick="javascript:showVisualizerPopup('bar');" alt="Bar Chart" 
																							onMouseOver="javascript:ShowVisIconsRollOverImage('Bar');" 
																							onMouseOut="javascript:ShowVisIconsRollOverImage('Bar');"/>
																				   <area shape="rect" 
																							coords="149,4,199,54"
																							href="javascript:ShowVisualizer('Scatter');" alt="Scatter Plot" 
																							onMouseOver="javascript:ShowVisIconsRollOverImage('Scatter');" 
																							onMouseOut="javascript:ShowVisIconsRollOverImage('Scatter');"/>
																				   <area shape="rect" 
																							coords="203,5,253,55"
																							href="javascript:ShowVisualizer('Tree');" alt="Tree Map" 
																							onMouseOver="javascript:ShowVisIconsRollOverImage('Tree');" 
																							onMouseOut="javascript:ShowVisIconsRollOverImage('Tree');"/>
																				   <area shape="rect" 
																							coords="268,5,318,55"
																							href="javascript:ShowVisualizer('Pyramid');" alt="Age-Pyramid Chart" 
																							onMouseOver="javascript:ShowVisIconsRollOverImage('Pyramid');" 
																							onMouseOut="javascript:ShowVisIconsRollOverImage('Pyramid');"/>
																				   <area shape="rect" 
																							coords="333,5,383,55"
																							href="javascript:ShowVisualizer('Wedge');" alt="Wedges" 
																							onMouseOver="javascript:ShowVisIconsRollOverImage('Wedge');" 
																							onMouseOut="javascript:ShowVisIconsRollOverImage('Wedge');"/>
																				   <area shape="rect" 
																							coords="387,5,437,55"
																							href="javascript:ShowVisualizer('Radar');" alt="Radar Chart" 
																							onMouseOver="javascript:ShowVisIconsRollOverImage('Radar');" 
																							onMouseOut="javascript:ShowVisIconsRollOverImage('Radar');"/>
																				</map>
																				
																				</td>
                                                                            <td width="5%">
                                                                                <a href="#">
                                                                                    <img src="../../stock/themes/default/images/play_r.png" width="29" height="29" border="0" /></a></td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td width="7">
                                                                    <img src="../../stock/themes/default/images/visualization_right.png" width="7" height="64" /></td>
                                                            </tr>
                                                        </table>
                                                        <div style="float: right; vertical-align: bottom;">
                                                            <span style="font-weight: bold;">Total Records: &nbsp;</span><span id="divCountDataRows">0</span>
                                                            
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                            <!--<input id="Button1" type="button" value="Show Chart" visible="false" onclick="javascript:getOATDetails();" 

/>-->
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="220" valign="top">
                                            <div id="tab_pivot333">
                                                <table width="100%" cellpadding="0" cellspacing="2" border="0">
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
                                            <!-- data view left section end -->
                                            <table width="195" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td>
                                                        <a href="#" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image10','','../../stock/themes/default/images/topic_h.png',1)"
                                                            onclick="showHideComponent('ind')">
                                                            <img src="../../stock/themes/default/images/topic.png" name="Image10" width="195"
                                                                height="36" border="0" id="Image10" /></a></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <a href="#" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image11','','../../stock/themes/default/images/area_h.png',1)"
                                                            onclick="showHideComponent('area')">
                                                            <img src="../../stock/themes/default/images/area.png" name="Image11" width="195"
                                                                height="37" border="0" id="Image11" /></a></td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 18px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div id="CollapsiblePanel1" class="CollapsiblePanel">
                                                            <div class="CollapsiblePanelTab" tabindex="0">
                                                                <a href="#" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image12','','../../stock/themes/default/images/time_period_h.png',0)">
                                                                    <img src="../../stock/themes/default/images/time_period.png" name="Image12" width="195"
                                                                        height="39" border="0" id="Image12" /></a></div>
                                                            <div class="CollapsiblePanelContent">
                                                                <table width="195" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td height="10">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="25">
                                                                            <table width="185" border="0" align="right" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td height="30" align="left">
                                                                                        <input id="chkIsMRD" checked="checked" type="checkbox" value="off"  onclick="javascript:GenerateData();" />
                                                                                        <label for="checkbox">
                                                                                            Most recent</label></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td height="30" align="left">
                                                                                        <img src="../../stock/themes/default/images/slider.png" width="164" height="22" /></td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="30" align="right" style="padding-right: 15px;">
                                                                            <a href="#">more</a></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="30" align="left">
                                                                        </td>
                                                                        <td height="30" align="left">
                                                                        </td>
                                                                        <td height="30" align="left">
                                                                        </td>
                                                                        <td height="30" align="left">
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div id="CollapsiblePanel2" class="CollapsiblePanel">
                                                            <div class="CollapsiblePanelTab" tabindex="0">
                                                                <a href="#" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image13','','../../stock/themes/default/images/source_h.png',0)">
                                                                    <img src="../../stock/themes/default/images/source.png" name="Image13" width="195"
                                                                        height="39" border="0" id="Image13" /></a></div>
                                                            <div class="CollapsiblePanelContent">
                                                                <table width="195" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td height="10">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table width="185" border="0" align="right" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td height="30" align="left">
                                                                                        <div id="divSources">
                                                                                        </div>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table width="185" border="0" align="right" cellpadding="0" cellspacing="0">
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="30" align="right" style="padding-right: 15px;">
                                                                            <a href="#">more</a></td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div id="CollapsiblePanel3" class="CollapsiblePanel">
                                                            <div class="CollapsiblePanelTab" tabindex="0">
                                                                <a href="#" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image14','','../../stock/themes/default/images/datavalue_h.png',0)"
                                                                    onclick="showHideComponent('dv')">
                                                                    <img src="../../stock/themes/default/images/datavalue.png" name="Image14" width="195"
                                                                        height="39" border="0" id="Image14" /></a></div>
                                                            <div class="CollapsiblePanelContent">
                                                                <table width="195" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td height="10">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="30" align="right" style="padding-right: 15px;">
                                                                            <a href="#">more</a></td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%--<input id="btnUndo" type="button" value="Undo" onclick="javascript:UndoFilter();" />
                    <input id="btnRedo" type="button" value="Redo" onclick="javascript:RedoFilter();"/>
                    <input id="btnReset" type="button" value="Reset" onclick="javascript:ResetFilters();"/>--%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td id="GridContainer" valign="top">
                                            <!-- Data grid section start -->
                                            <div id="gridData" style="width: 700px; overflow: auto;">
                                                <!-- <table width="100%" border="0" cellspacing="2" cellpadding="0" style="border:1px solid #999; margin-bottom:10px;">
  <tr>
    <td height="30"><div id="dataTitle" class="heading1"></div></td>
  </tr>
   <tr>
    <td height="25"><div id="dataSubTitle" class="content"></div></td>
  </tr>
</table> -->
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" style="margin-top: 5px;
                                                    border-bottom: 1px solid #999;">
                                                    <tr>
                                                        <td width="8" height="60" style="background-image: url(../../stock/themes/default/images/table_left.png);
                                                            background-repeat: repeat-x">
                                                            &nbsp;</td>
                                                        <td style="background-image: url(../../stock/themes/default/images/table_Center.png);
                                                            background-repeat: repeat-x">
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td height="30">
                                                                        <div id="dataTitle" class="heading2">
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td height="20">
                                                                        <div id="dataSubTitle" style="font-size: 10px;">
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td width="8" style="background-image: url(../../stock/themes/default/images/table_right.png);
                                                            background-repeat: no-repeat">
                                                            &nbsp;</td>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td width="115">
                                                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td height="60" align="center">
                                                                        <a href="#">
                                                                            <img src="../../stock/themes/default/images/toolbar/Print.png" onmouseover="this.src='../../stock/themes/default/images/toolbar/print_h.png'" onmouseout="this.src='../../stock/themes/default/images/toolbar/print.png'" border="0" /></a></td>
                                                                    <td align="center">
                                                                        <a href="#">
                                                                            <img src="../../stock/themes/default/images/toolbar/Xls.png" onmouseover="this.src='../../stock/themes/default/images/toolbar/xls_h.png'" onmouseout="this.src='../../stock/themes/default/images/toolbar/xls.png'" border="0"/></a></td>
                                                                    <td align="center">
                                                                        <a href="#">
                                                                            <img src="../../stock/themes/default/images/toolbar/Share.png" onmouseover="this.src='../../stock/themes/default/images/toolbar/share_h.png'" onmouseout="this.src='../../stock/themes/default/images/toolbar/share.png'" border="0" /></a></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div id="content">
                                                </div>
                                                <div id="pivot" style="width: 100%;">
                                                    <div id="pivot_page" style="display: none;">
                                                    </div>
                                                    <div id="pivot_content">
                                                    </div>
                                                    <div id="pivot_chart">
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- Data grid section end -->
                                        </td>
                                    </tr>
                                </table>
                                <!-- Container section end -->
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    
    <div id="db_popup_name" class="popup_block" style="height:400px;width:492px;"></div>
    <!--Format Markup-->
    <div id="formatDiv" style="padding:10px;width:400px;height:270px;background-color:#ececec; position:absolute;z-index:2000;display:none;left:350px;top:200px;">
        <table width="100%" cellpadding="0" cellspacing="0">                                                     
            <tr>
                <td>Series Name</td>
                <td><select id="series_ddl" onchange="selectSeries(this)"></select></td>
            </tr>                                
            <tr>
                <td>Series Color</td>
                <td><select id="seriesColor_ddl" onchange="selectSeriesColor(this)"></select></td>
            </tr> 
            <tr>
                <td colspan="2">
                    <hr style="width:100%;color:#cecece"/>
                </td>
            </tr>           
            <tr>
                <td>Chart Item</td>
                <td>
                    <select id="chartItemDdl" onchange="setDdlValues(this)">
                        <option value="Select">Select</option>
                        <option value="charttitle">Chart Title</option>
                        <option value="xaxislabel">X Axis Label</option>
                        <option value="xaxistitle">X Axis Title</option>
                        <option value="yaxislabel">Y Axis Label</option>
                        <option value="yaxistitle">Y Axis Title</option>
                    </select>
                </td>
            </tr>            
            <tr>
                <td>Font Size</td>                            
                <td>
                    <select id="fontSizeDdl">
                    </select>
                </td>
            </tr>
            <tr>
                <td>Font Family</td>
                <td>
                    <select id="fontFamilyDdl">
                    </select>
                </td>
            </tr>
            <tr>
                <td>Font Color</td> 
                <td>
                    <select id="colorDdl">
                    </select>
                </td>
            </tr>
            <tr>
                <td colspan="2" height="12">&nbsp;</td> 
            </tr>
            <tr>
                <td align="right" colspan="2">
                    <button onclick="closeFontDiv('formatDiv')">Cancel</button>&nbsp;&nbsp;
                    <button onclick="applyFont()">Apply</button>
                </td>                
            </tr>            
        </table>
    </div>
    <!--End Format Markup-->
    <!--Start Chart Style Markup-->
    <div id="styleDiv" style="padding:10px;width:442px;height:250px;background-color:#ececec; position:absolute;z-index:1800;display:none;left:350px;top:200px;">
        <img src="../../stock/themes/default/images/chartstyle.PNG" border="0" width="442" height="250" usemap="#styleicons"/>
        <map name="styleicons">
			   <area shape="rect" 
						coords="0,3,60,43"
						href="#" onclick="javascript:applyStyle('1');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>
				<area shape="rect" 
						coords="64,3,124,43"
						href="#" onclick="javascript:applyStyle('2');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>			   
				<area shape="rect" 
						coords="128,3,188,43"
						href="#" onclick="javascript:applyStyle('3');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>				
				<area shape="rect" 
						coords="192,3,252,43"
						href="#" onclick="javascript:applyStyle('4');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>
				<area shape="rect" 
						coords="256,3,316,43"
						href="#" onclick="javascript:applyStyle('5');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>
				<area shape="rect" 
						coords="320,3,380,43"
						href="#" onclick="javascript:applyStyle('6');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>
				<area shape="rect" 
						coords="384,3,444,43"
						href="#" onclick="javascript:applyStyle('7');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>
				<area shape="rect" 
						coords="0,47,60,84"
						href="#" onclick="javascript:applyStyle('8');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>
				<area shape="rect" 
						coords="64,47,124,84"
						href="#" onclick="javascript:applyStyle('9');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>			   
				<area shape="rect" 
						coords="128,47,188,84"
						href="#" onclick="javascript:applyStyle('10');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>				
				<area shape="rect" 
						coords="192,47,252,84"
						href="#" onclick="javascript:applyStyle('11');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>
				<area shape="rect" 
						coords="256,47,316,84"
						href="#" onclick="javascript:applyStyle('12');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>
				<area shape="rect" 
						coords="320,47,380,84"
						href="#" onclick="javascript:applyStyle('13');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>
				<area shape="rect" 
						coords="384,47,444,84"
						href="#" onclick="javascript:applyStyle('14');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>
				<area shape="rect" 
						coords="0,85,60,125"
						href="#" onclick="javascript:applyStyle('15');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>
				<area shape="rect" 
						coords="64,85,124,125"
						href="#" onclick="javascript:applyStyle('16');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>			   
				<area shape="rect" 
						coords="128,85,188,125"
						href="#" onclick="javascript:applyStyle('17');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>				
				<area shape="rect" 
						coords="192,85,252,125"
						href="#" onclick="javascript:applyStyle('18');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>
				<area shape="rect" 
						coords="256,85,316,125"
						href="#" onclick="javascript:applyStyle('19');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>
				<area shape="rect" 
						coords="320,85,380,125"
						href="#" onclick="javascript:applyStyle('20');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>
				<area shape="rect" 
						coords="384,85,444,125"
						href="#" onclick="javascript:applyStyle('21');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>
				<area shape="rect" 
						coords="0,127,60,166"
						href="#" onclick="javascript:applyStyle('22');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>
				<area shape="rect" 
						coords="64,127,124,166"
						href="#" onclick="javascript:applyStyle('23');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>			   
				<area shape="rect" 
						coords="128,127,188,166"
						href="#" onclick="javascript:applyStyle('24');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>				
				<area shape="rect" 
						coords="192,127,252,166"
						href="#" onclick="javascript:applyStyle('25');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>
				<area shape="rect" 
						coords="256,127,316,166"
						href="#" onclick="javascript:applyStyle('26');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>
				<area shape="rect" 
						coords="320,127,380,166"
						href="#" onclick="javascript:applyStyle('27');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>
				<area shape="rect" 
						coords="384,127,444,166"
						href="#" onclick="javascript:applyStyle('28');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>
				<area shape="rect" 
						coords="0,168,60,207"
						href="#" onclick="javascript:applyStyle('29');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>
				<area shape="rect" 
						coords="64,168,124,207"
						href="#" onclick="javascript:applyStyle('30');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>			   
				<area shape="rect" 
						coords="128,168,188,207"
						href="#" onclick="javascript:applyStyle('31');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>				
				<area shape="rect" 
						coords="192,168,252,207"
						href="#" onclick="javascript:applyStyle('32');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>
				<area shape="rect" 
						coords="256,168,316,207"
						href="#" onclick="javascript:applyStyle('33');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>
				<area shape="rect" 
						coords="320,168,380,207"
						href="#" onclick="javascript:applyStyle('34');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>
				<area shape="rect" 
						coords="384,168,444,207"
						href="#" onclick="javascript:applyStyle('35');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>
				<area shape="rect" 
						coords="0,209,60,248"
						href="#" onclick="javascript:applyStyle('36');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>
				<area shape="rect" 
						coords="64,209,124,248"
						href="#" onclick="javascript:applyStyle('37');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>			   
				<area shape="rect" 
						coords="128,209,188,248"
						href="#" onclick="javascript:applyStyle('38');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>				
				<area shape="rect" 
						coords="192,209,252,248"
						href="#" onclick="javascript:applyStyle('39');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>
				<area shape="rect" 
						coords="256,209,316,248"
						href="#" onclick="javascript:applyStyle('40');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>
				<area shape="rect" 
						coords="320,209,380,248"
						href="#" onclick="javascript:applyStyle('41');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>
				<area shape="rect" 
						coords="384,209,444,248"
						href="#" onclick="javascript:applyStyle('42');" alt="Style1" 
						onMouseOver="javascript:ShowVisIconsRollOverImage('line');" 
						onMouseOut="javascript:ShowVisIconsRollOverImage('line');"/>
				
	    </map>
        
    </div>
    <!--End Chart Style Markup-->
    <!--Start Share Link-->
    <div id="shareDiv" style="padding:10px;width:442px;height:70px;background-color:#ececec; position:absolute;z-index:1900;display:none;left:350px;top:200px;">
    <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <td align="right">
                <img src="../../stock/themes/default/images/close.png" style="height:20px;width:20px;border:0px;cursor:pointer;" onclick="closeShareDiv()"/>
            </td>
        </tr>
        <tr>
            <td>
                <span id="shareSpan"></span>
            </td>
        </tr>
    </table>
    </div>
    <!--End Share Link-->
    <!-- End of Page Markup -->
    <!-- START DEVELOPER CODE -->

    <script type="text/javascript">
        CookiePostfixStr = '_' + '<%=Global.CookiePostfixStr%>';    // use in hosting app

        var di_components = "Area,Indicator,Language,Qds";
        var di_component_version = '<%=Global.diuilib_version%>';
        var di_theme_css = '<%=Global.diuilib_theme_css%>';
        var di_diuilib_url = '<%=Global.diuilib_url%>';
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibincludes.js'" + 

"><\/script>");
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibcommon.js'" + "><\/script>");
    </script>

    <script type="text/javascript">onPageLoad('<%=hdsby%>', '<%=hdbnid%>', '<%=hselarea%>', '<%=hselind%>', '<%=hlngcode%>', 

'<%=hlngcodedb%>', '<%=hselindo%>', '<%=hselareao%>', '<%=hdvnids%>');
	di_jq(document).ready(function() { 
		/*assign width for grid container*/
		var containerWidth = di_jq('#GridContainer').width();
		di_jq('#gridData').css('width' , containerWidth);
	});
    </script>

    <script type="text/javascript" src="../js/highcharts.js"></script>   
     <script type="text/javascript" src="../js/exporting.js"></script>   
    
    <script>
		di_jq(document).ready(function() { 
	 //Close Popups and Fade Layer
        di_jq('a.close, #fade').live('click', function() { //When clicking on the close or fade layer...
            di_jq('#fade , .popup_block').fadeOut(function() {
                di_jq('#fade, a.close').remove();  //fade them both out
            });
            closeFontDiv('formatDiv');
            return false;
        });
		});
	</script>
    <!-- END OF DEVELOPER CODE -->
    <!-- START DESIGNER CODE -->

    <script type="text/javascript">
var CollapsiblePanel1 = new Spry.Widget.CollapsiblePanel("CollapsiblePanel1");
var CollapsiblePanel2 = new Spry.Widget.CollapsiblePanel("CollapsiblePanel2");
var CollapsiblePanel3 = new Spry.Widget.CollapsiblePanel("CollapsiblePanel3");
    </script>

    <!-- END OF DESIGNER CODE -->
</asp:Content>
