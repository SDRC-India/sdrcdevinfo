var di_qds_indicator_title = 'What?';
var di_qds_area_title = 'Where?';
var di_qds_indicator_folder_path = "";
var di_qds_area_codelist_folder_path = "";
var di_qds_area_search_btn_text = 'Search';
var di_qds_area_isblock_text = 'Subregion';

var di_qds_indicator_display_type = "2";
var di_qds_indicator_value_type = "2";
var di_qds_area_display_type = "2";
var di_qds_area_value_type = "2";
var di_qds_hot_selection_function_name = "";
var di_qds_selection_mode = "multiple";
var di_qds_ind_defalult_text = "";
var di_qds_area_defalut_text = "";
var di_qds_ind_block_details = "";
var di_qds_area_block_details = "";
var di_qds_advance_search = false;
var di_qds_clear_all_click_function_name = "";

var di_qds_id_delimiter = ",";
var di_qds_concatenation_text_delimiter = " - ";
var di_qds_ConcatenationIdDelimiter = "_";
var di_qds_store_all_selected_indicators_id = [];
var di_qds_store_all_selected_indicators_block_details = [];
var di_qds_store_all_selected_areas_id = [];
var di_qds_store_all_selected_areas_block_details = [];
var di_qds_blockDetailsDelimiter = "~~";
var di_qds_prev_ind_search_text = '';
var di_qds_prev_area_search_text = '';
var di_qds_textbox_size_after_search = 100;
var di_qds_ind_sel_index = -1;
var di_qds_area_sel_index = -1;
var di_qds_blockTextDelimiter = "||";
var di_qds_is_ind_hover = false;
var di_qds_is_area_hover = false;
var di_qds_suggestion_list = ''; // added 21/12
var di_qds_ind_popup_width;


/* Draw quick data search interface */
// added suggectionList var 21/12
function di_draw_qds_ui(draw_div_id, indicator_title, area_title, width, height, indicator_codelist_foler_path, area_codelist_folder_path, indicator_display_type, indicator_value_type, area_display_type, area_value_type, callback_on_search_selection, selection_mode, ind_defalult_text, area_defalut_text, search_img, ind_search_text, area_search_text, ind_block_details, area_block_details, advance_search, suggectionList, search_btn_text, area_isblock_text, callback_on_clear_all_click) {
    var di_html_data = '';
    var diCurrentIndCSS = '';
    var diCurrentAreaCSS = '';
    var diCurrentIndText = '';
    var diCurrentAreaText = '';

    var IndBlockDetArr;
    var IndIdDispArr;
    var id = '';
    var DispName = '';
    var AreaBlockDetArr;
    var AreaIdDispArr;
    var CrossImg = '';

    try {
        CrossImg = search_img.replace("search_button.gif", "cross.gif");

        //Clear all preview values
        di_qds_store_all_selected_indicators_id = [];
        di_qds_store_all_selected_indicators_block_details = [];
        di_qds_store_all_selected_areas_id = [];
        di_qds_store_all_selected_areas_block_details = [];

        di_qds_advance_search = advance_search;
        // added 21/12
        if (suggectionList != '' && suggectionList != null && suggectionList != undefined) di_qds_suggestion_list = suggectionList;

        if (indicator_title != '') {
            di_qds_indicator_title = indicator_title;
        }

        if (area_title != '') {
            di_qds_area_title = area_title;
        }

        // Outer Div	   
        di_html_data = '<div style="width:' + width + '; height:' + height + 'px;"><div style="width:100%;"><table id="tbl_qds" cellpadding="0" cellspacing="0" width="100%" border="0"> <tr> <td style="padding-right:1px;">';

        // Set indicator text and css
        if (ind_search_text != '') {
            diCurrentIndCSS = "heading1";
            diCurrentIndText = ind_search_text;
        }
        else {
            diCurrentIndCSS = "qds_textfield_default";
            diCurrentIndText = ind_defalult_text;
        }

        // Set area text and css
        if (area_search_text != '') {
            diCurrentAreaCSS = "di_gui_leafnode_label";
            diCurrentAreaText = area_search_text;
        }
        else {
            diCurrentAreaCSS = "qds_textfield_default";
            diCurrentAreaText = area_defalut_text;
        }

        if (search_btn_text != '') {
            di_qds_area_search_btn_text = search_btn_text;
        }

        di_html_data += '<table width="100%" cellspacing="0" cellpadding="0" border="0"> <tr><td height="33px" class="di_qds_box_mid_strp">';
        
        // Indicator textbox html	    
        di_html_data += '<table id="tbl_ind" width="100%" border="0" cellpadding="0" cellspacing="0"><tr> <td id="tdIndTitle" class="heading1" style="padding-left:3px;">' + di_qds_indicator_title + '</td> <td id="tdSelINDQDS" style="width:0px; padding-left:2px;"><span id="spnSelIndicators"></span></td> <td id="tdIndTBQDS" style="width:100%;padding-right:3px;"><input type="text" id="indicator_txt" class="' + diCurrentIndCSS + '" style="width:100%; border:none; font-weight:normal;  margin-left:2px; padding-left:2px; display:inline;" value="' + diCurrentIndText + '" onclick="di_qds_txt_ind_click();" onKeyup="JavaScript:return di_qds_seach_indicators(this, event);" onblur="di_qds_txt_ind_blur();" /></td> <td><img id="img_qds_ind_cross" src=' + CrossImg + ' onclick="ClearAllIndBlocks();" style="cursor:pointer; border:0; width:16px; height:16px; visibility:hidden;" /><input type="hidden" id="di_qds_first_sugg_indval"></td> </tr>';
        
        // Indicator popup list row
        di_html_data += '<tr> <td></td><td colspan="2" id="indicator_list_td" valign="top" style="padding-left:4px;"><div id="indicators_list_div" class="di_qds_listview_div"></div></td></tr> </table>';

        di_html_data += '</td></tr> <tr><td height="31px" style="padding-right:3px;">';

        // Area textbox html	    
        di_html_data += '<table width="100%" border="0" cellpadding="0" cellspacing="0"><tr> <td id="tdAreaTitle" class="di_gui_leafnode_label" style="font-weight:bold; cursor:auto; padding-left:3px;">' + di_qds_area_title + '</td> <td id="tdSelAreaQDS" style="width:0px; padding-left:2px;"><span id="spnSelAreas"></span></td> <td id="tdAreaTBQDS" style="width:100%; padding-right:3px;"><input type="text" id="area_txt" style="width:100%; border:none; margin-left:2px; padding-left:2px; display:inline;" class="' + diCurrentAreaCSS + '" value="' + diCurrentAreaText + '" onclick="di_qds_txt_area_click();" onKeyup="JavaScript:return di_qds_seach_areas(this, event);" onblur="di_qds_txt_area_blur();" /></td> <td><img id="img_qds_area_cross" src=' + CrossImg + ' onclick="ClearAllAreaBlocks();" style="cursor:pointer; border:0; width:16px; height:16px; visibility:hidden;" /><input type="hidden" id="di_qds_first_sugg_arval"></td> </tr>';
        
        // Area popup list row
        di_html_data += '<tr><td></td><td colspan="2" id="area_list_td" valign="top" style="padding-left:4px;"><div id="area_list_div" class="di_qds_listview_div"></div></td></tr> </table>';

        di_html_data += '</td></tr> </table>';

        di_html_data += '</td> <td width="42" valign="middle" class="di_qds_box_side_strp"> <a onmouseover="di_qds_search_over_image();" onmouseout="di_qds_search_out_image()">';

        // Search button html
        di_html_data += '<img id="qds_search_img" src=' + search_img + ' onclick="JavaScript:return di_qds_search_data();" style="cursor:pointer; border:0;" /></a>';

        //di_html_data += '</a> </td> </tr> </table></div></div>';
        di_html_data += '<div class="di_qds_search_txt"><a onclick="JavaScript:return di_qds_search_data();">' + di_qds_area_search_btn_text + '</a></div>';
        di_html_data += '</td> </tr> </table></div></div>';

        di_html_data += '<span id="IndRuler" class="ruler heading1"></span> <span id="AreaRuler" class="ruler di_gui_leafnode_label"></span>';

        // Set component HTML into div
        di_jq("#" + draw_div_id).html(di_html_data);


        // Set the selection list popup div width
        var outerwidth = di_jq('#tbl_qds').width() - 65;        
        di_jq('#area_list_div').css("width", outerwidth - 60);
        //di_jq('#indicators_list_div').css("width", outerwidth - 72);        
        di_qds_ind_popup_width = di_qds_getPageWidth()* 0.65;
        if(outerwidth-72 > di_qds_ind_popup_width) {
            di_qds_ind_popup_width = outerwidth-72;
        }
        di_jq('#indicators_list_div').css("width", di_qds_ind_popup_width);
                

        // Assing parameter values in global variables
        if (indicator_codelist_foler_path != '') {
            di_qds_indicator_folder_path = indicator_codelist_foler_path;
        }

        if (area_codelist_folder_path != '') {
            di_qds_area_codelist_folder_path = area_codelist_folder_path;
        }

        if (indicator_display_type != '') {
            di_qds_indicator_display_type = indicator_display_type;
        }

        if (indicator_value_type != '') {
            di_qds_indicator_value_type = indicator_value_type;
        }

        if (area_display_type != '') {
            di_qds_area_display_type = area_display_type;
        }

        if (area_value_type != '') {
            di_qds_area_value_type = area_value_type;
        }

        if (callback_on_search_selection != '') {
            di_qds_hot_selection_function_name = callback_on_search_selection;
        }

        if (selection_mode != '') {
            di_qds_selection_mode = selection_mode;
        }

        if (ind_defalult_text != '') {
            di_qds_ind_defalult_text = ind_defalult_text;
        }

        if (area_defalut_text != '') {
            di_qds_area_defalut_text = area_defalut_text;
        }

        if (ind_block_details != '') {
            di_jq("#indicator_txt").val('');

            di_qds_ind_block_details = ind_block_details;

            // Create the indicators item blocks	         
            IndBlockDetArr = di_qds_ind_block_details.split(di_qds_blockTextDelimiter);
            for (var i = 0; i < IndBlockDetArr.length; i++) {
                IndIdDispArr = IndBlockDetArr[i].split(di_qds_blockDetailsDelimiter);
                id = IndIdDispArr[0];
                DispName = IndIdDispArr[1];
                di_qds_store_ind_id(true, id, DispName);
            }
        }

        if (area_block_details != '') {
            di_jq("#area_txt").val('');

            di_qds_area_block_details = area_block_details;

            // Create the areas item blocks	            	    
            AreaBlockDetArr = di_qds_area_block_details.split(di_qds_blockTextDelimiter);
            for (var i = 0; i < AreaBlockDetArr.length; i++) {
                AreaIdDispArr = AreaBlockDetArr[i].split(di_qds_blockDetailsDelimiter);
                id = AreaIdDispArr[0];
                DispName = AreaIdDispArr[1];
                di_qds_store_area_id(true, id, DispName);
            }
        }

        if (area_isblock_text != '') {
            di_qds_area_isblock_text = area_isblock_text;
        }
        
        if (callback_on_clear_all_click!=undefined && callback_on_clear_all_click!='') {
            di_qds_clear_all_click_function_name = callback_on_clear_all_click;
        }        
    }
    catch (err) { }
}


/* Call callback parent function on search selection */
function di_qds_call_hot_sel_function() {
    var call_parent_fun_str;

    try {
        if (di_qds_hot_selection_function_name != '') {
            call_parent_fun_str = di_qds_hot_selection_function_name + "()";
            eval(call_parent_fun_str);
        }
    }
    catch (err) { }
}


/* Call callback parent function on search selection */
function di_qds_clear_all_click_function() {
    var call_parent_fun_str;

    try {
        if (di_qds_clear_all_click_function_name != '') {
            call_parent_fun_str = di_qds_clear_all_click_function_name + "()";
            eval(call_parent_fun_str);
        }
    }
    catch (err) { }
}

/* Generate li for indicator search text */
function di_qds_seach_indicators(objTxt, event) {
    var SearchString = di_jq.trim(objTxt.value);
    var IndicatorslistDiv = di_jq("#indicators_list_div");

    var FileName = '';
    var SearchPatern = '';
    var IndicatorName = '';
    var UnitName = '';
    var IndNId = '';
    var IUChkId = '';
    var UnitNId = '';
    var Type = '';
    var IsInd = "0";
    var SearchIndex = -1;
    var DisplayName = '';
    var IndicatorNameHTML = '';
    var SBHtml = '';
    var data = '';
    var CloseImageURL = di_images_path + "/close.png";
    var IndListHearderHTML = "<a onmouseover='di_qds_img_over_image(this);' onmouseout='di_qds_img_out_image(this);'> <img alt='' align='right' src='" + CloseImageURL + "' onclick='di_qds_set_txt_ind_text();' style='padding-right:3px; cursor:pointer;' / > </a>";
    var Checked = '';
    var IndIcLabelClassName = '';
    var i;
    var id;
    var TotalLICount = -1;
    var DispIndLength = -1;
    var DisplayTitle = '';

    try {
        // Change textbox CSS        
        di_jq("#indicator_txt").attr("class", "heading1");


        if (di_qds_advance_search) {
            if (event.keyCode == 13) {
                //Enter key handling, call search button click method
                di_qds_search_data();
            }
            else {
                //Show cross image to clear textbox value
                ShowSeachListLoader(di_jq("#img_qds_ind_cross"), false);
            }
        }
        else {
            if (event.keyCode == 27) {
                //For escape button handling
                di_qds_hide_indicator_list_div();
                di_jq("#indicator_txt").val('');
                di_qds_ind_sel_index = -1;
            }
            else if (event.keyCode == 40) {
                //Down arrow key handling

                //Count total li in current selection div
                TotalLICount = di_jq("#divIndLI li").length;

                //Check any previous selection is persist, then remove selection
                if (di_qds_ind_sel_index > -1) {
                    di_jq("#divIndLI li")[di_qds_ind_sel_index].className = '';
                }
                else if (di_qds_ind_sel_index == -1) {
                    di_jq("#divIndLI").scrollTop(0);
                }

                di_qds_ind_sel_index++;

                //Reset current index to 0 if current is last index
                if (di_qds_ind_sel_index == TotalLICount) {
                    di_qds_ind_sel_index = 0;
                    di_jq("#divIndLI").scrollTop(0);
                }

                //Apply hover class on selected li
                di_jq("#divIndLI li")[di_qds_ind_sel_index].className = 'di_gui_label_hover';

                //Scroll vertical bar down
                if (di_qds_ind_sel_index > 8) {
                    di_jq("#divIndLI").scrollTop(di_jq("#divIndLI").scrollTop() + 23);
                }
            }
            else if (event.keyCode == 38) {
                //Up arrow key handling

                //Count total li in current selection div
                TotalLICount = di_jq("#divIndLI li").length;

                if (di_qds_ind_sel_index > -1) {
                    di_jq("#divIndLI li")[di_qds_ind_sel_index].className = '';
                }
                else if (di_qds_ind_sel_index == -1) {
                    di_qds_ind_sel_index = 0;
                }

                di_qds_ind_sel_index--;

                //Reset current index to 0 if current is last index
                if (di_qds_ind_sel_index == -1) {
                    di_qds_ind_sel_index = TotalLICount - 1;
                    di_jq("#divIndLI").scrollTop(di_jq("#divIndLI").height());
                }

                di_jq("#divIndLI li")[di_qds_ind_sel_index].className = 'di_gui_label_hover';

                //Scroll vertical bar up
                if (di_qds_ind_sel_index < TotalLICount - 9) {
                    di_jq("#divIndLI").scrollTop(di_jq("#divIndLI").scrollTop() - 23);
                }
            }
            else if (event.keyCode == 13) {
                //Enter key handling

                if (di_qds_ind_sel_index == -1) {
                    //Call seach button click on click of enter key
                    di_qds_search_data();
                }
                else {
                    if (di_jq("#divIndLI li")[di_qds_ind_sel_index] != undefined) {
                        di_jq("#divIndLI li")[di_qds_ind_sel_index].getElementsByTagName("label")[0].onclick();
                    }
                    else {
                        di_qds_search_data();
                    }
                }
            }
            else if (event.keyCode == 8 && di_qds_prev_ind_search_text == '') {
                //Remove one block on press of backspace key
                if (di_qds_store_all_selected_indicators_id.length > 0) {
                    i = di_qds_store_all_selected_indicators_id.length - 1;
                    id = di_qds_store_all_selected_indicators_id[i];

                    di_qds_select_ind_chk(id, '', true, true);

                    di_qds_resize_ind_blocks();
                }
                else {
                    di_jq("#indicator_txt").val('');
                }
            }
            else {
                if (SearchString.indexOf(di_qds_ind_defalult_text) > -1) {
                    SearchString = SearchString.replace(di_qds_ind_defalult_text, '');
                    di_jq("#indicator_txt").val(SearchString);
                }

                if (SearchString != "") {
                    //Start searching only when search characters lenght is more than 1
                    if (SearchString.length > 1) {   
                        ShowSeachListLoader(di_jq("#img_qds_ind_cross"), true);
                                                         
                        // Create file name with start character of search stirng
                        FileName = di_qds_indicator_folder_path + "/" + encodeURIComponent(SearchString.substr(0, 1) + ".xml");

                        di_jq.ajax({
                            type: "GET",
                            url: FileName,
                            dataType: "xml",
                            async: true,
                            success: function (xml) {
                                try {
                                    if (di_qds_suggestion_list == '') {
                                        SearchPatern = 'iu';
                                    }
                                    else if (di_qds_suggestion_list == 'NONE') {
                                        SearchPatern = 'iu[t="IND"]';
                                    }
                                    else {
                                        di_qds_suggestion_listAr = di_qds_suggestion_list.split(",");
                                        var listStr = '';
                                        for (var i = 0; i < di_qds_suggestion_listAr.length; i++) {
                                            if (i == 0) listStr = '[t="' + di_qds_suggestion_listAr[i] + '"]';
                                            else listStr += ',[t="' + di_qds_suggestion_listAr[i] + '"]';
                                        }
                                        SearchPatern = 'iu' + listStr;
                                    }

                                    // Declare arrary to store indicator nid for distinct check.
                                    var IndNIdArr = [];

                                    var qdsSLCount = 0;
                                    di_jq(xml).find(SearchPatern).each(function () {
                                        IndicatorName = di_jq(this).attr("in");
                                        IndNId = di_jq(this).attr("inid");
                                        
                                        Type = di_jq(this).attr("t");
                                        
                                        if(Type != "IC_SR") {

                                            SearchIndex = (" " + IndicatorName.toLowerCase().replace("(","")).indexOf(" " + SearchString.toLowerCase());

                                            // Set display text
                                            if (SearchIndex >= 0 && di_jq.inArray(IndNId, IndNIdArr) == -1) {
                                                IUChkId = '';

                                                switch (di_qds_indicator_display_type) {
                                                    case '1':
                                                        DisplayName = IndicatorName;
                                                        break;
                                                    case '2':
                                                        UnitName = di_jq(this).attr("un");
                                                        DisplayName = IndicatorName + di_qds_concatenation_text_delimiter + UnitName;
                                                        break;
                                                }

                                                // Set id of checkbox
                                                switch (di_qds_indicator_value_type) {
                                                    case '1':
                                                        IUChkId = IndNId;
                                                        IndNIdArr.push(IndNId);
                                                        break;
                                                    case '2':
                                                        UnitNId = di_jq(this).attr("unid");
                                                        IUChkId = IndNId + di_qds_ConcatenationIdDelimiter + UnitNId;
                                                        break;
                                                }

                                                //DisplayName = Common.ReplaceSearchTextToBold(DisplayName, SearchString);

                                                // Replace some character which were breaking the JS string
                                                DisplayName = di_ReplaceAll(DisplayName, "'", "`");
                                                DisplayName = di_ReplaceAll(DisplayName, "\n", "");

                                                if (Type == "IND") {
                                                    IsInd = "1";
                                                    //IndIcLabelClassName = 'di_gui_tree_label_qds_ind';
                                                    IndIcLabelClassName = 'di_gui_leafnode_label';
                                                }
                                                else {
                                                    IsInd = "0";
                                                    //IndIcLabelClassName = 'di_gui_tree_label';
                                                    IndIcLabelClassName = 'di_gui_leafnode_label';
                                                }

                                                IUChkId = IUChkId + di_qds_ConcatenationIdDelimiter + IsInd;

                                                // Trim display name if its length is greater then container    
                                                //DispIndLength = (di_jq('#indicator_list_td').width() - 67) / 5;
                                                DispIndLength = (di_qds_ind_popup_width - 88) / 5;
                                                if (DisplayName.length > DispIndLength) {  
                                                    DisplayTitle = DisplayName;
                                                    DisplayName = DisplayName.substr(0, DispIndLength - 7) + "...";
                                                }
                                                else {
                                                    DisplayTitle = "";
                                                }

                                                IndicatorNameHTML = "<label onclick='di_qds_select_ind_chk(\"" + IUChkId + "\", \"" + DisplayName + "\",true);' title='" + DisplayTitle + "' class='" + IndIcLabelClassName + "'>" + DisplayName + "</label> ";

                                                if (qdsSLCount == 0) {
                                                    di_jq('#di_qds_first_sugg_indval').val(IUChkId + di_qds_blockDetailsDelimiter + DisplayName);
                                                }
                                                qdsSLCount++;

                                                Checked = '';
                                                for (i = 0; i < di_qds_store_all_selected_indicators_id.length; i++) {
                                                    if (di_qds_store_all_selected_indicators_id[i].indexOf(IUChkId) > -1) {
                                                        Checked = 'Checked';
                                                        break;
                                                    }
                                                }

                                                // Create LI HTML for every indicator
                                                SBHtml += "<li onmouseover='di_qds_onmouseover_li(this);' onmouseout='di_qds_onmouseout_li(this);' style='padding:3px;'>";

                                                if (di_qds_selection_mode == "multiple") {
                                                    SBHtml += "<input type='checkbox' id='" + IUChkId + "' onclick='di_qds_close_div(\"Indicator\"); di_qds_select_ind_chk(\"" + IUChkId + "\", \"" + DisplayName + "\",false);' " + Checked + " />";
                                                }

                                                SBHtml += IndicatorNameHTML + "</li>";
                                            }
                                        }

                                    });


                                    // If HTML LI exist for indicators    
                                    if (SBHtml != '') {
                                        data = "<table cellspacing='0' cellpadding='0' width='100%'><tr id='trIndListHeader'><td class='di_qds_listview_header'>" + IndListHearderHTML + "</td></tr><tr><td><div id='divIndLI' class='di_qds_listview_li_div di_gui_body' style='padding-top:0px;' onmouseover='di_qds_onmouseover_sel_list_div();' onmouseout='di_qds_onmouseout_sel_list_div();'>" + SBHtml + "</div></td></tr></table>";
                                    }


                                    // Show selection list div if html data exists for searched text  
                                    if (data != "") {
                                        IndicatorslistDiv.slideDown("fast");

                                        if (di_qds_selection_mode == "single") {
                                            IndicatorslistDiv.html(data);
                                            di_jq("#trIndListHeader").hide();
                                            di_jq("#divIndLI").height(di_jq("#divIndLI").height() + 18);

                                            //Select 1st index of indicator list
                                            di_qds_ind_sel_index = 0;
                                            di_jq("#divIndLI li")[di_qds_ind_sel_index].className = 'di_gui_label_hover';
                                        }
                                        else {
                                            IndicatorslistDiv.html(data);
                                            di_jq("#trIndListHeader").show();
                                        }
                                    }
                                    else {
                                        IndicatorslistDiv.html("");
                                        IndicatorslistDiv.hide();
                                    }
                                    
                                    ShowSeachListLoader(di_jq("#img_qds_ind_cross"), false);
                                    
                                    //check block length and show cross image
                                    if(di_jq("#spnSelIndicators span").length > 0) {
                                        di_jq("#img_qds_ind_cross").css("visibility", "visible");
                                    }
                                }
                                catch (ex) {
                                    //alert("message:" + ex.message);
                                }
                            },
                            error: function () {
                                IndicatorslistDiv.html("");
                                IndicatorslistDiv.hide();
                            },
                            cache: true
                        });
                    }
                }
                else {
                    IndicatorslistDiv.html("");
                    IndicatorslistDiv.hide();
                }
            }

            di_qds_prev_ind_search_text = SearchString;
        }
    }
    catch (err) { }
}

/* Generate li for area search text */
function di_qds_seach_areas(objTxt, event) {

    var SearchString = di_jq.trim(objTxt.value);
    var AreaListDiv = di_jq("#area_list_div");
    var AreaTxtWidth;

    var FileName = '';
    var SearchPatern = '';
    var AreaName = '';
    var AreaID = '';
    var AreaNId = '';
    var SearchIndex = -1;
    var DisplayName = '';
    var AreaChkId = '';    
    var SBHtml = '';
    var data = '';
    var CloseImageURL = di_images_path + "/close.png";
    var AreaListHearderHTML = "<img alt='' align='right' src='" + CloseImageURL + "' onclick='di_qds_set_txt_area_text();' style='padding-right:3px; cursor:pointer;' / >";
    var Checked = '';
    var i;
    var id;
    var TotalLICount;    
    var AreaParentNIdsArr = [];

    try {
        // Change textbox CSS        
        di_jq("#area_txt").attr("class", "di_gui_leafnode_label");

        if (di_qds_advance_search) {
            if (event.keyCode == 13) {
                //Enter key handling, call search button click method
                di_qds_search_data();
            }
            else {
                //Show cross image to clear textbox value
                ShowSeachListLoader(di_jq("#img_qds_area_cross"), false);
            }
        }
        else {
            if (event.keyCode == 27) {
                //For escape button handling
                di_qds_hide_area_list_div();
                di_jq("#area_txt").val('');
                di_qds_area_sel_index = -1;
            }
            else if (event.keyCode == 40) {
                //Down arrow key handling

                //Count total li in current selection div
                TotalLICount = di_jq("#divAreaLI li").length;

                //Check any previous selection is persist, then remove selection
                if (di_qds_area_sel_index > -1) {
                    di_jq("#divAreaLI li")[di_qds_area_sel_index].className = '';
                }
                else if (di_qds_area_sel_index == -1) {
                    di_jq("#divAreaLI").scrollTop(0);
                }

                di_qds_area_sel_index++;

                //Reset current index to 0 if current is last index
                if (di_qds_area_sel_index == TotalLICount) {
                    di_qds_area_sel_index = 0;
                    di_jq("#divAreaLI").scrollTop(0);
                }


                //Apply hover class on selected li
                di_jq("#divAreaLI li")[di_qds_area_sel_index].className = 'di_gui_label_hover';

                //Scroll vertical bar down
                if (di_qds_area_sel_index > 8) {
                    di_jq("#divAreaLI").scrollTop(di_jq("#divAreaLI").scrollTop() + 23);
                }
            }
            else if (event.keyCode == 38) {
                //Up arrow key handling

                //Count total li in current selection div
                TotalLICount = di_jq("#divAreaLI li").length;

                if (di_qds_area_sel_index > -1) {
                    di_jq("#divAreaLI li")[di_qds_area_sel_index].className = '';
                }
                else if (di_qds_area_sel_index == -1) {
                    di_qds_area_sel_index = 0;
                }

                di_qds_area_sel_index--;

                //Reset current index to 0 if current is last index
                if (di_qds_area_sel_index == -1) {
                    di_qds_area_sel_index = TotalLICount - 1;
                    di_jq("#divAreaLI").scrollTop(di_jq("#divAreaLI").height());
                }

                di_jq("#divAreaLI li")[di_qds_area_sel_index].className = 'di_gui_label_hover';

                //Scroll vertical bar up
                if (di_qds_area_sel_index < TotalLICount - 9) {
                    di_jq("#divAreaLI").scrollTop(di_jq("#divAreaLI").scrollTop() - 23);
                }
            }
            else if (event.keyCode == 13) {
                //Enter key handling

                if (di_qds_area_sel_index == -1) {
                    //Call seach button click on click of enter key
                    di_qds_search_data();
                }
                else {
                    if (di_jq("#divAreaLI li")[di_qds_area_sel_index] != undefined) {
                        di_jq("#divAreaLI li")[di_qds_area_sel_index].getElementsByTagName("label")[0].onclick();
                    }
                    else {
                        di_qds_search_data();
                    }
                }
            }
            else if (event.keyCode == 8 && di_qds_prev_area_search_text == '') {
                //Remove one block on press of backspace key
                if (di_qds_store_all_selected_areas_id.length > 0) {
                    i = di_qds_store_all_selected_areas_id.length - 1;
                    id = di_qds_store_all_selected_areas_id[i];

                    di_qds_select_area_chk(id, '', true, true);

                    di_qds_resize_area_blocks();
                }
                else {
                    di_jq("#area_txt").val('');
                }
            }
            else {
                if (SearchString.indexOf(di_qds_area_defalut_text) > -1) {
                    SearchString = SearchString.replace(di_qds_area_defalut_text, '');
                    di_jq("#area_txt").val(SearchString);
                    di_jq("#area_txt").css("cursor", "text");
                }

                if (SearchString != "") {
                    //Start searching only when search characters lenght is more than 1
                    if (SearchString.length > 1) {                        
                        ShowSeachListLoader(di_jq("#img_qds_area_cross"), true);
                    
                        // Create file name with start character of search stirng
                        FileName = di_qds_area_codelist_folder_path + "/" + encodeURIComponent(SearchString.substr(0, 1) + ".xml");

                        di_jq.ajax({
                            type: "GET",
                            url: FileName,
                            dataType: "xml",
                            async: true,
                            success: function (xml) {
                                try {
                                    AreaParentNIdsArr = [];
                                    SearchPatern = 'a';

                                    var qdsSLCount = 0;
                                    di_jq(xml).find(SearchPatern).each(function () {
                                        AreaName = di_jq(this).attr("n");

                                        SearchIndex = (" " + AreaName.toLowerCase()).indexOf(" " + SearchString.toLowerCase());

                                        // Set display text
                                        if (SearchIndex >= 0) {
                                            var ParentNId = di_jq(this).attr("pnid");

                                            if (!IsIdExistInArray(AreaParentNIdsArr, ParentNId)) {
                                                //AreaParentNIdsArr.push(ParentNId);                                                

                                                AreaChkId = '';

                                                switch (di_qds_area_display_type) {
                                                    case '1':
                                                        DisplayName = AreaName;
                                                        break;
                                                    case '2':
                                                        AreaID = di_jq(this).attr("id");
                                                        DisplayName = AreaName + di_qds_concatenation_text_delimiter + AreaID;
                                                        break;
                                                }

                                                // Set id of checkbox
                                                switch (di_qds_area_value_type) {
                                                    case '1':
                                                        AreaNId = di_jq(this).attr("nid");
                                                        AreaChkId = AreaNId;
                                                        break;
                                                    case '2':
                                                        AreaNId = di_jq(this).attr("nid");
                                                        AreaID = di_jq(this).attr("id");
                                                        AreaChkId = AreaNId + di_qds_ConcatenationIdDelimiter + AreaID;
                                                        break;
                                                }
                                                
                                                if (qdsSLCount == 0) {
                                                    di_jq('#di_qds_first_sugg_arval').val(AreaChkId + di_qds_blockDetailsDelimiter + DisplayName);
                                                }
                                                qdsSLCount++;
                                                                                                
                                                SBHtml += di_qds_Create_area_li(DisplayName, AreaChkId);                                                
                                                
//                                                if (di_jq(this).attr("isblock").toLowerCase() == "t") {
//                                                    DisplayName = DisplayName + di_qds_concatenation_text_delimiter + di_qds_area_isblock_text;
//                                                    AreaChkId = "BL" + di_qds_ConcatenationIdDelimiter + AreaChkId;
//                                                    
//                                                    SBHtml += di_qds_Create_area_li(DisplayName, AreaChkId);
//                                                }
                                            }
                                        }
                                    });

                                    // If HTML LI exist for areas                              
                                    if (SBHtml != '') {
                                        data = "<table id='tblSelArea' cellspacing='0' cellpadding='0' width='100%'><tr id='trAreaListHeader'><td class='di_qds_listview_header'>" + AreaListHearderHTML + "</td></tr><tr><td><div id='divAreaLI' class='di_qds_listview_li_div di_gui_body' style='padding-top:0px;' onmouseover='di_qds_onmouseover_sel_list_div();' onmouseout='di_qds_onmouseout_sel_list_div();'>" + SBHtml + "</div></td></tr></table>";
                                    }

                                    // Show selection list div if html data exists for searched text        
                                    if (data != "") {
                                        AreaListDiv.slideDown("fast");
                                        AreaListDiv.html(data);

                                        if (di_qds_selection_mode == "single") {
                                            di_jq("#trAreaListHeader").hide();
                                            di_jq("#divAreaLI").height(di_jq("#divAreaLI").height() + 18);

                                            //Select 1st index of area list
                                            di_qds_area_sel_index = 0;
                                            di_jq("#divAreaLI li")[di_qds_area_sel_index].className = 'di_gui_label_hover';
                                        }
                                        else {
                                            di_jq("#trAreaListHeader").show();
                                        }
                                    }
                                    else {
                                        AreaListDiv.html("");
                                        AreaListDiv.hide();
                                    }
                                    
                                    ShowSeachListLoader(di_jq("#img_qds_area_cross"), false);
                                    
                                    //check block length and show cross image
                                    if(di_jq("#spnSelAreas span").length > 0) {
                                        di_jq("#img_qds_area_cross").css("visibility", "visible");
                                    }
                                }
                                catch (ex) {
                                    alert("message:" + ex.message);
                                }
                            },
                            error: function () {
                                AreaListDiv.html("");
                                AreaListDiv.hide();
                            },
                            cache: true
                        });
                    }                    
                }
                else {
                    AreaListDiv.html("");
                    AreaListDiv.hide();
                }
            }

            di_qds_prev_area_search_text = SearchString;
        }
    }
    catch (err) { }
}

function di_qds_Create_area_li(displayName, areaChkId)
{
    var RetVal = '';
    var DispAreaLength = -1;
    var DisplayTitle = '';
    var AreaNameHTML = '';
    
    // Trim display name if its length is greater then container
    DispAreaLength = (di_jq('#area_list_td').width() - 65) / 5;
    if (displayName.length > DispAreaLength) {
        DisplayTitle = displayName;
        displayName = displayName.substr(0, DispAreaLength - 5) + "...";
    }
    else {
        DisplayTitle = "";
    }

    Checked = '';
    for (i = 0; i < di_qds_store_all_selected_areas_id.length; i++) {
        if (di_qds_store_all_selected_areas_id[i].indexOf(areaChkId) > -1) {
            Checked = 'Checked';
            break;
        }
    }

    // Create LI HTML for each area
    RetVal = "<li onmouseover='di_qds_onmouseover_li(this);' onmouseout='di_qds_onmouseout_li(this);' style='padding:3px;'>";

    if (di_qds_selection_mode == "multiple") {
        RetVal += "<input type='checkbox' id='" + areaChkId + "' onclick='di_qds_close_div(\"Area\"); di_qds_select_area_chk(\"" + areaChkId + "\", \"" + displayName + "\",false);' " + Checked + " />";
    }
    
    AreaNameHTML = '<label onclick="di_qds_select_area_chk(\'' + areaChkId + '\', \'' + di_qds_escape_spclchar(displayName) + '\',true);" title="' + DisplayTitle + '" class="di_gui_leafnode_label">' + displayName + '</label> ';

    RetVal += AreaNameHTML + "</li>";
    
    return RetVal;
}

function IsIdExistInArray(arrayName, id) {
    var RetVal = false;

    if (id != "-1") {
        for (var i = 0; i < arrayName.length; i++) {
            if (arrayName[i] == id) {
                RetVal = true;
                break;
            }
        }
    }

    return RetVal;
}


/* Get values of selected indicators */
function di_qds_get_selected_indicators() {
    var RetVal = "";

    try {
//        //added 22/12
//        var indSrText = di_qds_get_ind_search_data();
//        if (indSrText != '') {
//            di_qds_add_default_indicator();
//        } // end
        RetVal = di_qds_store_all_selected_indicators_id;
    }
    catch (err) { }

    return RetVal;
}

/* Get values of selected areas */
function di_qds_get_selected_areas() {
    var RetVal = "";

    try {
//        //added 22/12
//        var areaSrText = di_qds_get_area_search_data();
//        if (areaSrText != '') {
//            di_qds_add_default_area();
//        } // end
        RetVal = di_qds_store_all_selected_areas_id;
    }
    catch (err) { }

    return RetVal;
}

/* Call the parent callback method */
function di_qds_search_data() {
    try {
        di_qds_set_txt_ind_text();
        di_qds_set_txt_area_text();

        di_qds_call_hot_sel_function();
    }
    catch (err) { }
}

/* Hide area list div */
function di_qds_hide_area_list_div() {
    di_jq("#area_list_div").hide();
}

/* Hide indicator list div */
function di_qds_hide_indicator_list_div() {
    di_jq("#indicators_list_div").hide();
}


/* On click of any area select/unselect it checkbox */
function di_qds_select_area_chk(chkId, dispName, isToggle, isCallFromBlockCrossImage) {
    var chk_check_status = false;

    try {
        if (di_qds_selection_mode == "single") {
            if (di_jq("#ar_" + chkId).length > 0) {
                chk_check_status = false;
            }
            else {
                chk_check_status = true;
            }
        }
        else {
            if (isToggle) {
                di_jq("#" + chkId).attr('checked', !di_jq("#" + chkId).attr('checked'));
            }

            chk_check_status = di_jq("#" + chkId).attr('checked');
        }

        // Push/pop the id in array
        di_qds_store_area_id(chk_check_status, chkId, dispName, isCallFromBlockCrossImage);

        di_qds_area_sel_index = -1;

        //Set the focus in textbox
        di_jq("#area_txt").focus();

        if (di_qds_selection_mode == "single") {
            di_qds_set_txt_area_text();
        }

        di_jq("#area_txt").val('');
    }
    catch (err) { }
}

// Store selected area id and create its item block
function di_qds_store_area_id(isPush, id, dispName, isCallFromBlockCrossImage) {
    try {
        if (isPush) {
            //Insert id in array
            di_qds_store_all_selected_areas_id.push(id);
            di_qds_store_all_selected_areas_block_details.push(id + di_qds_blockDetailsDelimiter + dispName);
            di_jq("#spnSelAreas").html(di_jq("#spnSelAreas").html() + di_qds_create_Sel_item_block("ar_" + id, dispName, "di_gui_leafnode_label"));
        }
        else {
            if(isCallFromBlockCrossImage){
                //Remove id from array
                for (var i = 0; i < di_qds_store_all_selected_areas_id.length; i++) {
                    if (di_qds_store_all_selected_areas_id[i].indexOf(id) > -1) {
                        di_qds_store_all_selected_areas_id.splice(i, 1);
                        di_qds_store_all_selected_areas_block_details.splice(i, 1);
                        di_qds_hide_block("ar_" + id);
                        break;
                    }
                }
            }
        }

        //Call for resizing the blocks
        di_qds_resize_area_blocks();
    }
    catch (err) { }
}

/* Resize area blocks depend upon number of blocks */
function di_qds_resize_area_blocks() {
    var ParentTDWidth = '';
    var AvlWidth;
    var AvlWidthPerBlock;
    var BlockCount;
    var LblTextOldLength;
    var LblOrgText = '';
    var SelSpanNTxtParentTdWidth = 0;
    var AllBlockWidth = 0;
    var TdAreaTBQDSWidth = 0;
    var TdSelAreaQDSWidth = 0;

    try {
        BlockCount = di_jq("#spnSelAreas span").length;

        if (BlockCount > 0) {
            // Visible cross image for closing all indicator blocks
            di_jq("#img_qds_area_cross").css("visibility", "visible");

            // Set the width of the TD containing the text box
            di_jq("#tdAreaTBQDS").css('width', '100px');

            //Block Span and textbox Control parent td width            
            SelSpanNTxtParentTdWidth = di_jq("#spnSelAreas span").parent().parent().parent().css("width").replace("px", "") - 53;

            //Parent TD
            ParentTDWidth = SelSpanNTxtParentTdWidth - 40;

            AvlWidth = ParentTDWidth - (BlockCount * 17);
            AvlWidthPerBlock = AvlWidth / BlockCount;

            di_jq("#spnSelAreas span").each(function () {
                LblTextOldLength = di_jq("#" + this.id + " table tr td label").html().length;
                LblOrgText = di_jq("#" + this.id + " table tr td label")[0].title;


                if (LblOrgText.visualLength("AreaRuler") > AvlWidthPerBlock) {
                    di_jq("#" + this.id + " table tr td label").html(LblOrgText.trimToPx("AreaRuler", AvlWidthPerBlock));
                    // Calculate all block width
                    //AllBlockWidth += AvlWidthPerBlock + 18;
                    //AllBlockWidth += AvlWidthPerBlock + 20;
                    AllBlockWidth += AvlWidthPerBlock + 25;
                }
                else {
                    di_jq("#" + this.id + " table tr td label").html(LblOrgText);
                    // Calculate all block width
                    //AllBlockWidth += LblOrgText.visualLength("AreaRuler") + 18;
                    //AllBlockWidth += LblOrgText.visualLength("AreaRuler") + 20;
                    AllBlockWidth += LblOrgText.visualLength("AreaRuler") + 25;
                }

            });

            // Calculate available width for textbox td
            TdAreaTBQDSWidth = (SelSpanNTxtParentTdWidth - AllBlockWidth);

            // Set textbox td width
            di_jq("#tdSelAreaQDS").css('width', AllBlockWidth + 'px');
            di_jq("#tdAreaTBQDS").css('width', TdAreaTBQDSWidth + 'px');
            di_jq("#area_txt").css("cursor", "text");
        }
        else {
            // Hidden cross image for closing all indicator blocks
            di_jq("#img_qds_area_cross").css("visibility", "hidden");

            di_jq("#tdSelAreaQDS").css('width', '0px');
            di_jq("#tdAreaTBQDS").css('width', '100%');


            //Set default text if area list div is closed			
            if (di_jq("#area_list_div").css("display") == "none") {
                di_qds_txt_area_blur();
            }
        }
    }
    catch (err) { }
}


/* On click of any indicator select/unselect it checkbox */
function di_qds_select_ind_chk(chkId, dispName, isToggle, isCallFromBlockCrossImage) {
    var chk_check_status = false;

    try {
        if (di_qds_selection_mode == "single") {
            if (di_jq("#in_" + chkId).length > 0) {
                chk_check_status = false;
            }
            else {
                chk_check_status = true;
            }
        }
        else {
            if (isToggle) {
                di_jq("#" + chkId).attr('checked', !di_jq("#" + chkId).attr('checked'));
            }

            chk_check_status = di_jq("#" + chkId).attr('checked');
        }


        // Push/pop the id in array
        di_qds_store_ind_id(chk_check_status, chkId, dispName, isCallFromBlockCrossImage);

        di_qds_ind_sel_index = -1;

        //Set the focus in textbox                
        di_jq("#indicator_txt").focus();

        if (di_qds_selection_mode == "single") {
            di_qds_set_txt_ind_text();
        }

        di_jq("#indicator_txt").val('');
    }
    catch (err) { }
}

// Store selected indicator id and create its item block
function di_qds_store_ind_id(isPush, id, dispName, isCallFromBlockCrossImage) {
    try {
        if (isPush) {
            //Insert id in array
            di_qds_store_all_selected_indicators_id.push(id);
            di_qds_store_all_selected_indicators_block_details.push(id + di_qds_blockDetailsDelimiter + dispName);
            di_jq("#spnSelIndicators").html(di_jq("#spnSelIndicators").html() + di_qds_create_Sel_item_block("in_" + id, dispName, "heading1"));
        }
        else {
            if(isCallFromBlockCrossImage){
                //Remove id from array
                for (var i = 0; i < di_qds_store_all_selected_indicators_id.length; i++) {
                    if (di_qds_store_all_selected_indicators_id[i].indexOf(id) > -1) {
                        di_qds_store_all_selected_indicators_id.splice(i, 1);
                        di_qds_store_all_selected_indicators_block_details.splice(i, 1);
                        di_qds_hide_block("in_" + id);
                        break;
                    }
                }
            }
        }

        //Call for resizing the blocks
        di_qds_resize_ind_blocks();
    }
    catch (err) { }
}

/* Resize indicator blocks depend upon number of blocks */
function di_qds_resize_ind_blocks() {
    var ParentTDWidth = '';
    var AvlWidth;
    var AvlWidthPerBlock;
    var BlockCount;
    var LblTextOldLength;
    var LblOrgText = '';
    var SelSpanNTxtParentTdWidth = 0;
    var AllBlockWidth = 0;
    var TdIndTBQDSWidth = 0;
    var TdSelINDQDSWidth = 0;

    try {
        BlockCount = di_jq("#spnSelIndicators span").length;

        if (BlockCount > 0) {
            // Visible cross image for closing all indicator blocks
            di_jq("#img_qds_ind_cross").css("visibility", "visible");

            // Set the width of the TD containing the text box
            di_jq("#tdIndTBQDS").css('width', '100px');

            //Block Span and textbox Control parent td width
            SelSpanNTxtParentTdWidth = di_jq("#spnSelIndicators span").parent().parent().parent().css("width").replace("px", "") - 55;

            //Parent TD            
            ParentTDWidth = SelSpanNTxtParentTdWidth - 100;

            AvlWidth = ParentTDWidth - (BlockCount * 17);
            AvlWidthPerBlock = AvlWidth / BlockCount;

            di_jq("#spnSelIndicators span").each(function () {
                LblTextOldLength = di_jq("#" + this.id + " table tr td label").html().length;
                LblOrgText = di_jq("#" + this.id + " table tr td label")[0].title;

                if (LblOrgText.visualLength("IndRuler") > AvlWidthPerBlock) {
                    di_jq("#" + this.id + " table tr td label").html(LblOrgText.trimToPx("IndRuler", AvlWidthPerBlock));                    
                    //AllBlockWidth += AvlWidthPerBlock + 10;
                    //AllBlockWidth += AvlWidthPerBlock + 15;
                    //AllBlockWidth += AvlWidthPerBlock + 18;
                    
                    if(BlockCount <3){
                        AllBlockWidth += AvlWidthPerBlock + 18;
                    }
                    else{
                        AllBlockWidth += AvlWidthPerBlock + 22;
                    }
                }
                else {
                    di_jq("#" + this.id + " table tr td label").html(LblOrgText);
                    //AllBlockWidth += LblOrgText.visualLength("IndRuler") + 10;
                    //AllBlockWidth += LblOrgText.visualLength("IndRuler") + 15;
                    //AllBlockWidth += LblOrgText.visualLength("IndRuler") + 18;
                    
                    if(BlockCount <3){
                        AllBlockWidth += LblOrgText.visualLength("IndRuler") + 18;
                    }
                    else{
                        AllBlockWidth += LblOrgText.visualLength("IndRuler") + 22;
                    }
                }
            });

            // Calculate available width for textbox td            
            TdIndTBQDSWidth = (SelSpanNTxtParentTdWidth - AllBlockWidth);

            // Set td width            
            di_jq("#tdSelINDQDS").css('width', AllBlockWidth + 'px');
            di_jq("#tdIndTBQDS").css('width', TdIndTBQDSWidth + 'px');
        }
        else {
            // Hidden cross image for closing all indicator blocks
            di_jq("#img_qds_ind_cross").css("visibility", "hidden");

            di_jq("#tdSelINDQDS").css('width', '0px');
            di_jq("#tdIndTBQDS").css('width', '100%');

            //Set default text if indicator list div is closed
            if (di_jq("#indicators_list_div").css("display") == "none") {
                di_qds_txt_ind_blur();
            }
        }
    }
    catch (err) { }
}

/* Close list div */
function di_qds_close_div(listType) {
    try {
        if (di_qds_selection_mode == "single") {
            if (listType == "Indicator") {
                di_qds_set_txt_ind_text();
            }
            else {
                di_qds_set_txt_area_text();
            }
        }
    }
    catch (err) { }
}

/* Set default value on click of indicator search textbox */
function di_qds_txt_ind_click() {
    try {
        if (di_jq("#indicator_txt").val() == di_qds_ind_defalult_text) {
            di_jq("#indicator_txt").val('');
        }

        di_qds_hide_area_list_div();
    }
    catch (err) { }
}

/* Set selected indicator text in indicator search textbox */
function di_qds_set_txt_ind_text() {
    try {
        di_qds_hide_indicator_list_div();

        if (di_jq("#indicator_txt").val() == '' && di_qds_store_all_selected_indicators_id == '') {
            di_jq("#indicator_txt").val(di_qds_ind_defalult_text);
            di_jq("#indicator_txt").attr("class", "qds_textfield_default");
            di_jq("#indicator_txt").css("width", "100%");
        }

    }
    catch (err) { }
}


/* Set default value on click of area search textbox */
function di_qds_txt_area_click() {
    try {
        if (di_jq("#area_txt").val() == di_qds_area_defalut_text) {
            di_jq("#area_txt").val('');
        }

        di_qds_hide_indicator_list_div();
    }
    catch (err) { }
}



/* Set selected area text in area search textbox */
function di_qds_set_txt_area_text() {
    try {
        di_qds_hide_area_list_div();

        if (di_jq("#area_txt").val() == '' && di_qds_store_all_selected_areas_id == '') {
            di_jq("#area_txt").val(di_qds_area_defalut_text);
            di_jq("#area_txt").attr("class", "qds_textfield_default");
            di_jq("#area_txt").css("width", "100%");
        }

    }
    catch (err) { }
}


/* Call on selection list div mouseover event */
function di_qds_onmouseover_sel_list_div() {
    try {
        //Remove selected default li and set hover status true on list div
        if (di_jq("#area_list_div").css("display") == "block") {
            di_jq("#divAreaLI li")[di_qds_area_sel_index].className = '';
            di_qds_is_area_hover = true;
        }

        if (di_jq("#indicators_list_div").css("display") == "block") {
            di_jq("#divIndLI li")[di_qds_ind_sel_index].className = '';
            di_qds_is_ind_hover = true;
        }
    }
    catch (err) { }
}

/* Call on selection list div mouseout event */
function di_qds_onmouseout_sel_list_div() {
    try {
        //Remove hover status on list div
        if (di_jq("#area_list_div").css("display") == "block") {
            di_qds_is_area_hover = false;
        }
        else if (di_jq("#indicators_list_div").css("display") == "block") {
            di_qds_is_ind_hover = false;
        }
    }
    catch (err) { }
}


/* Call on li mouseover event */
function di_qds_onmouseover_li(obj_li) {
    try {
        di_jq(obj_li).addClass('di_gui_label_hover');
    }
    catch (err) { }
}

/* Call on li mouseout event */
function di_qds_onmouseout_li(obj_li) {
    di_jq(obj_li).removeClass('di_gui_label_hover');
}


/* Call on blur of indicator search textbox */
function di_qds_txt_ind_blur() {
    try {
        if (di_jq("#indicator_txt").val() == '' && di_qds_store_all_selected_indicators_id.length == 0) {
            di_qds_set_txt_ind_text();
        }
        else if (!di_qds_is_ind_hover) {
            //Hide indicator div, if its not hover on list
            di_qds_hide_indicator_list_div();
        }
    }
    catch (err) { }
}


/* Call on blur of area search textbox */
function di_qds_txt_area_blur() {
    try {
        if (di_jq("#area_txt").val() == '' && di_qds_store_all_selected_areas_id.length == 0) {
            di_qds_set_txt_area_text();
        }
        else if (!di_qds_is_area_hover) {
            //Hide indicator div, if its not hover on list
            di_qds_hide_area_list_div();
        }
    }
    catch (err) { }
}

/* Get the entered/selected text of indicator textbox */
function di_qds_get_ind_search_text() {
    var RetVal = "";
    var IndText = "";

    try {
        if (di_jq("#indicator_txt").val() != di_qds_ind_defalult_text) {
            IndText = di_jq("#indicator_txt").val();

            if (di_qds_indicator_display_type == '2') {
                IndText = di_qds_get_filtered_text(IndText, di_qds_id_delimiter, di_qds_concatenation_text_delimiter);
            }
        }

        RetVal = IndText;
    }
    catch (err) { }

    return RetVal;
}


/* Get the entered/selected text of area textbox */
function di_qds_get_area_search_text() {
    var RetVal = "";
    var AreaText = "";

    try {
        if (di_jq("#area_txt").val() != di_qds_area_defalut_text) {
            AreaText = di_jq("#area_txt").val();

            if (di_qds_area_display_type == '2') {
                AreaText = di_qds_get_filtered_text(AreaText, di_qds_id_delimiter, di_qds_concatenation_text_delimiter);
            }
        }

        RetVal = AreaText;
    }
    catch (err) { }

    return RetVal;
}

/* Get filtered search text only (indicator name, area name) discard unit name and area id, if exists in selected text */
function di_qds_get_filtered_text(str, idDelimiter, concatTextDelimiter) {
    var RetVal = "";
    var StrArr;

    try {
        StrArr = str.split(idDelimiter);

        for (var i = 0; i < StrArr.length; i++) {
            RetVal += idDelimiter + StrArr[i].split(concatTextDelimiter)[0];
        }

        RetVal = RetVal.substr(idDelimiter.length);
    }
    catch (err) { }

    return RetVal;
}

/* Change image on mouse over effect of search image */
function di_qds_search_over_image() {
    var new_src = '';

    try {
        new_src = di_jq("#qds_search_img")[0].src.replace(".gif", "_h.gif");
        di_jq("#qds_search_img")[0].src = new_src
    }
    catch (err) { }
}

/* Change image on mouse out effect of search image */
function di_qds_search_out_image() {
    var new_src = '';

    try {
        new_src = di_jq("#qds_search_img")[0].src.replace("_h.gif", ".gif");
        di_jq("#qds_search_img")[0].src = new_src
    }
    catch (err) { }
}



// Create an item block
function di_qds_create_Sel_item_block(id, dispText, dispTextClassName) {
    var Retval = '';
    var CloseImageURL = di_images_path + "/close.png";
    var CloseImgHtml = '';
    var CloseClick = '';
    var VisibleText = '';
    var Tooltip = '';
    var StyleSetting = '';

    try {
        if (id.indexOf("ar_") > -1) {
            CloseClick = "di_qds_select_area_chk(\"" + id.substr(3) + "\",\"\",true,true);";
        }
        else {
            CloseClick = "di_qds_select_ind_chk(\"" + id.substr(3) + "\",\"\",true,true);";
            StyleSetting = "style='font-weight:normal;' ";
        }

        // Close button html for item block        
        //CloseImgHtml = "<a onmouseover='di_qds_img_over_image(this);' onmouseout='di_qds_img_out_image(this);'> <img id='img_" + id.substr(3) + "' alt='' align='right' src='" + CloseImageURL + "' onclick='" + CloseClick + "' style='padding-right:3px; margin-top:3px; cursor:pointer; visibility :hidden;' / ></a>";
        CloseImgHtml = "<a onmouseover='di_qds_img_over_image(this);' onmouseout='di_qds_img_out_image(this);'> <img id='img_" + id.substr(3) + "' alt='' align='right' src='" + CloseImageURL + "' onclick='" + CloseClick + "' style='padding:0 1px 3px 0; margin-top:3px; cursor:pointer;' / ></a>";


        // Create outer block
        //RetVal = '<span id=' + id + ' class="di_qds_Sel_item_block"  onmouseover="di_qds_block_over(\'' + id.substr(3) + '\');" onmouseout="di_qds_block_out(\'' + id.substr(3) + '\');" >';
        RetVal = '<span id=' + id + ' class="di_qds_Sel_item_block" >';

        // Create block contents in table
        RetVal += '<table cellspacing="0" cellpadding="0" border="0">';

        if (dispText.length > 20) {
            VisibleText = dispText.substr(0, 16) + '...';
        }
        else {
            VisibleText = dispText;
        }

        Tooltip = dispText;

        RetVal += '<tr><td nowrap><label class="' + dispTextClassName + '" ' + StyleSetting + ' title=\"' + Tooltip + '\" >' + VisibleText + '</label></td><td>' + CloseImgHtml + '</td></tr>';

        RetVal += '</table>';

        RetVal += '</span>';
    }
    catch (err) { }

    return RetVal;
}

// Hide the item block
function di_qds_hide_block(id) {
    try {
        di_jq("#" + id).html('');
        di_jq("#" + id).css("display", "none");

        if (id.indexOf("ar_") > -1) {
            document.getElementById("spnSelAreas").removeChild(document.getElementById(id));
        }
        else {
            document.getElementById("spnSelIndicators").removeChild(document.getElementById(id));
        }


    }
    catch (err) { }
}


/* Get values of selected indicators block details */
function di_get_selected_ind_block_det() {
    var RetVal = "";

    try {
//        //added 22/12
//        var indSrText = di_qds_get_ind_search_data();
//        if (indSrText != '') {
//            di_qds_add_default_indicator();
//        } // end

        RetVal = di_qds_store_all_selected_indicators_block_details;
    }
    catch (err) { }

    return RetVal;
}
/* function to added default indicator */
function di_qds_add_default_indicator() {
    var defaultVal = di_jq('#di_qds_first_sugg_indval').val();
    if (defaultVal != '' && defaultVal != null && defaultVal != undefined) {
        defaultVal = defaultVal.split(di_qds_blockDetailsDelimiter);
        di_qds_store_ind_id(true, defaultVal[0], defaultVal[1]);

        di_jq("#indicator_txt").val('');
        di_jq('#di_qds_first_sugg_indval').val('');
    }
}

/* Get values of selected area block details */
function di_get_selected_area_block_det() {
    var RetVal = "";

    try {
//        //added 22/12
//        var areaSrText = di_qds_get_area_search_data();
//        if (areaSrText != '') {
//            di_qds_add_default_area();
//        } // end

        RetVal = di_qds_store_all_selected_areas_block_details;

    }
    catch (err) { }

    return RetVal;
}
/* function to added default indicator */
function di_qds_add_default_area() {
    var defaultVal = di_jq('#di_qds_first_sugg_arval').val();
    if (defaultVal != '' && defaultVal != null && defaultVal != undefined) {
        defaultVal = defaultVal.split(di_qds_blockDetailsDelimiter);
        di_qds_select_area_chk(defaultVal[0], defaultVal[1], true);

        //di_jq("#area_txt").val('');
        di_jq('#di_qds_first_sugg_arval').val('');
    }
}


/* Change image on mouse over effect of block cross image */
function di_qds_img_over_image(objThis) {
    var ImgCtrl;
    var NewSrc = '';

    try {
        ImgCtrl = objThis.getElementsByTagName("img")[0];

        NewSrc = ImgCtrl.src.replace(".png", "_h.png");
        ImgCtrl.src = NewSrc;
    }
    catch (err) { }
}

/* Change image on mouse out effect of block cross image */
function di_qds_img_out_image(objThis) {
    var ImgCtrl;
    var NewSrc = '';

    try {
        ImgCtrl = objThis.getElementsByTagName("img")[0];

        NewSrc = ImgCtrl.src.replace("_h.png", ".png");
        ImgCtrl.src = NewSrc
    }
    catch (err) { }
}

/* Get the text of all indicator blocks */
function di_qds_get_all_ind_block_text() {
    var RetVal = '';
    var i = 0;

    try {
        for (i = 0; i < di_qds_store_all_selected_indicators_block_details.length; i++) {
            RetVal += di_qds_blockTextDelimiter + di_qds_store_all_selected_indicators_block_details[i].split(di_qds_blockDetailsDelimiter)[1];
        }
    }
    catch (err) { }

    if (RetVal != '') {
        RetVal = RetVal.substr(2);
    }

    return RetVal;
}


/* Get the text of all area blocks */
function di_qds_get_all_area_block_text() {
    var RetVal = '';
    var i = 0;

    try {
        for (i = 0; i < di_qds_store_all_selected_areas_block_details.length; i++) {
            RetVal += di_qds_blockTextDelimiter + di_qds_store_all_selected_areas_block_details[i].split(di_qds_blockDetailsDelimiter)[1];
        }
    }
    catch (err) { }

    if (RetVal != '') {
        RetVal = RetVal.substr(2);
    }

    return RetVal;
}


//function di_qds_block_over(imgId) {
//    try {
//        document.getElementById("img_" + imgId).style.visibility = 'visible';
//    }
//    catch (err) { }
//}


//function di_qds_block_out(imgId) {
//    try {
//        document.getElementById("img_" + imgId).style.visibility = 'hidden';
//    }
//    catch (err) { }
//}


String.prototype.visualLength = function (rulerId) {
    var RetVal = 0;
    var Ruler;

    try {
        Ruler = document.getElementById(rulerId);
        Ruler.innerHTML = this;
        RetVal = Ruler.offsetWidth;
        Ruler.innerHTML = "";
    }
    catch (err) { }

    return RetVal
}

String.prototype.trimToPx = function (rulerId, length) {
    var tmp = this;
    var trimmed = this;

    try {
        if (tmp.visualLength(rulerId) > length) {
            trimmed += "...";
            while (trimmed.visualLength(rulerId) > length) {
                tmp = tmp.substring(0, tmp.length - 1);
                trimmed = tmp + "...";
            }
        }
    }
    catch (err) { }

    return trimmed;
}


function ClearAllIndBlocks() {
    try {
        //Clear all block html
        di_jq("#spnSelIndicators").html('');

        //Clear all preview values
        di_qds_store_all_selected_indicators_id = [];
        di_qds_store_all_selected_indicators_block_details = [];

        di_qds_resize_ind_blocks();

        //Set the focus in textbox    
        di_jq("#indicator_txt").val('');
        di_jq("#indicator_txt").focus();
        
        di_qds_clear_all_click_function();
    }
    catch (err) { }
}

function ClearAllAreaBlocks() {
    try {
        //Clear all block html
        di_jq("#spnSelAreas").html('');

        //Clear all preview values
        di_qds_store_all_selected_areas_id = [];
        di_qds_store_all_selected_areas_block_details = [];

        di_qds_resize_area_blocks();

        //Set the focus in textbox    
        di_jq("#area_txt").val('');
        di_jq("#area_txt").focus();
        
        di_qds_clear_all_click_function();
    }
    catch (err) { }
}

function di_qds_escape_spclchar(str) {
    return str.replace("'", "\\'");
}

function ShowSeachListLoader(imgObj, isShow) {    

    //var ShowValue = "hidden";
    var ShowValue = "visible";
    var ImageSrc;

    try {
        if(isShow){
            //ShowValue = "visible";
            ImageSrc = imgObj.attr("src").replace("cross.gif", "loading_img.gif");
        }
        else {
            ImageSrc = imgObj.attr("src").replace("loading_img.gif", "cross.gif");
        }
        
        imgObj.attr("src", ImageSrc);
        imgObj.css("visibility", ShowValue);
    }
    catch (err) { }
}

function di_qds_getPageWidth()
{
    return window.innerWidth != null? window.innerWidth: document.documentElement && document.documentElement.clientWidth ? document.documentElement.clientWidth:document.body != null? document.body.clientWidth:null;
}