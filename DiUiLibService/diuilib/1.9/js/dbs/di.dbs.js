
var di_db_sel_db_nids=[];var di_db_sel_db_names=[];function di_db_getXMLHTTPRequest()
{if(window.XMLHttpRequest)
{return new window.XMLHttpRequest();}
else
{for(i=0;i<xmlVersions.length;i++)
{try
{return new ActiveXObject(xmlVersions[i]);}
catch(ex)
{}}}}
function di_db_LoadXMLs(di_db_databaseFileUrl)
{var di_db_xhttp=di_db_getXMLHTTPRequest();di_db_xhttp.open("GET",di_db_databaseFileUrl,false);di_db_xhttp.send("");return di_db_xhttp.responseXML;}
function di_db_ControlContainer(db_title,db_caption,db_searchboxText,db_ctrl_height,db_ctrl_width,db_isMultipleDb,db_lbl_ok,db_lbl_cancel)
{var di_db_searchImg="search.gif";this.di_db_searchText=db_searchboxText;var di_db_inntHtml="<table cellpadding='0' cellspacing='0' border='0'>";var di_db_sub_container_div_height=0;var di_db_sub_detail_div_height=0;if(db_title.length>0)
{if(db_isMultipleDb)
{di_db_sub_container_div_height=60;di_db_sub_detail_div_height=105;}
else
{di_db_sub_container_div_height=30;di_db_sub_detail_div_height=68;}
di_db_inntHtml+="<tr class='popup_heading1' style='height:30px;'><td style='padding:8px;'>"+db_title+"</td><td align='right' style='padding-right:10px;display:none;'><a href='javascript:void(0)' onclick='di_db_hideComponent()'\><img src='"
di_db_inntHtml+=di_images_path+di_db_slash+"cross.gif' width='16' height='16' border='0' /></a></td></tr>";}
di_db_inntHtml+="<tr><td colspan='2'><div class='di_gui_container' style='height:"+(parseInt(db_ctrl_height)-di_db_sub_container_div_height).toString()
di_db_inntHtml+="px;background-color:#ffffff;width:"+db_ctrl_width+";'><table cellspacing='0' width='100%' cellpadding='0' border='0'>";di_db_inntHtml+="<tr><td><div class='di_gui_searchpanel' style='width:96%;'><table width='100%' cellpadding='0' cellspacing='0'><tr><td class='di_gui_searchleft_box'><img src='"+di_images_path+di_db_slash+di_db_searchImg;di_db_inntHtml+="' width='16' height='16' border='0' /><td class='di_gui_searchmiddle_box'><input name='database_search' id='database_search' type='text' value='";di_db_inntHtml+=db_searchboxText+"' onclick='di_db_clearTextBox(this)' onblur='di_db_setSearchTextBox(this)' onkeyup='di_db_searchDataBase(event,this)' class='di_gui_searchbox di_gui_searchbox_defaulttext'"
di_db_inntHtml+="/><td class='di_gui_searchright_box' al><a href='javascript:void(0)' onclick='di_db_setSerachBoxByImg()'\><img src='"+di_images_path+di_db_slash+"cross.gif' width='16' height='16' border='0' /></a></td></tr></table></div>"
di_db_inntHtml+="</td></tr>";di_db_inntHtml+="<tr><td>";di_db_inntHtml+="<div id='dbDetailDiv' class='di_gui_body' style='width:"+(db_ctrl_width)+"px;background-color:#ffffff;overflow-x:hidden;overflow-y:auto;height:"+(parseInt(db_ctrl_height)-di_db_sub_detail_div_height).toString()+"px;margin-top:5px;'>";di_db_inntHtml+="</div>";di_db_inntHtml+="</td></tr></table></div></td></tr>";if(db_isMultipleDb)
{di_db_inntHtml+="<tr><td align='right' style='padding:3px;'> <input type='button' class='di_gui_button' value='"+db_lbl_ok+"' onclick='di_db_CloseComponent(1);' /> <input type='button' class='di_gui_button' value='"+db_lbl_cancel+"' onclick='di_db_CloseComponent(2);' />&nbsp; </td></tr>";}
di_db_inntHtml+="</table>"
return di_db_inntHtml;}
function di_db_setSelectedDB(db_selectedDb,di_db_btnObj)
{var close_fun_str;var ok_fun_str;try
{if(di_db_btnObj.innerHTML.toLowerCase()!=this.di_db_inuseBtnText.toLowerCase())
{this.di_db_selectedDbId=db_selectedDb;var di_db_Btn_List=document.getElementById('dbDetailDiv').getElementsByTagName('Button');var totalBtns=di_db_Btn_List.length;for(var i=0;i<totalBtns;i++)
{if(di_db_Btn_List[i].innerHTML.toLowerCase()==this.di_db_inuseBtnText.toLowerCase())
{di_db_Btn_List[i].innerHTML=this.di_db_selectBtnText;di_jq('#'+di_db_Btn_List[i].id).removeClass('di_gui_button2');di_jq('#'+di_db_Btn_List[i].id).addClass('di_gui_button');}}
di_db_btnObj.innerHTML=this.di_db_inuseBtnText;di_jq('#'+di_db_btnObj.id).addClass('di_gui_button2');di_jq('#'+di_db_btnObj.id).removeClass('di_gui_button');di_db_sel_db_nids=[];di_db_sel_db_names=[];di_db_sel_db_nids.push(db_selectedDb);di_db_sel_db_names.push(di_db_btnObj.innerHTML);if(this.di_db_component_ok_function_name!='')
{ok_fun_str=this.di_db_component_ok_function_name+"()";eval(ok_fun_str);}}
if(this.di_db_component_close_function_name!='')
{close_fun_str=this.di_db_component_close_function_name+"()";eval(close_fun_str);}}
catch(err){}}
function di_db_hideComponent()
{di_jq('#'+this.di_db_controlID).hide();}
function di_db_clearTextBox(di_db_txtBox)
{if(di_db_txtBox.value.toLowerCase()==this.di_db_searchText.toLowerCase())
{di_db_txtBox.value="";di_jq('#'+di_db_txtBox.id).removeClass('di_gui_searchbox_defaulttext');di_jq('#'+di_db_txtBox.id).addClass('di_gui_searchbox_text');}}
function di_db_setSearchTextBox(di_db_txtBox)
{var val=this.di_db_searchText;if(di_db_txtBox.value.toLowerCase()==""||di_db_txtBox.value.toLowerCase()==val.toLowerCase())
{di_db_txtBox.value=val;di_jq('#'+di_db_txtBox.id).removeClass('di_gui_searchbox_text');di_jq('#'+di_db_txtBox.id).addClass('di_gui_searchbox_defaulttext');}}
function di_db_ToggleDbDetail(db_divCtrlId,db_rowObj)
{var db_arrowPrevImg="rightArrow.gif";var db_arrowNextImg="downArrow.gif";di_jq('#'+db_divCtrlId).slideToggle();var imgs=db_rowObj.getElementsByTagName('Img');if(imgs[0].src.indexOf(db_arrowNextImg)!=-1)
{imgs[0].src=imgs[0].src.replace(db_arrowNextImg,db_arrowPrevImg);}
else if(imgs[0].src.indexOf(db_arrowPrevImg)!=-1)
{imgs[0].src=imgs[0].src.replace(db_arrowPrevImg,db_arrowNextImg);}}
function di_db_searchDataBase(e,searchObj)
{di_loading('show');var di_db_html="";if(searchObj.value.length>0)
{try
{di_db_html=di_db_searchDbDetails(searchObj.value);document.getElementById('dbDetailDiv').innerHTML=di_db_html;}
catch(err)
{}}
else if(searchObj.value.length==0)
{try
{di_db_html=di_db_GetContainerItem();document.getElementById('dbDetailDiv').innerHTML=di_db_html;}
catch(err)
{}}
di_loading('hide');}
function di_db_setSerachBoxByImg()
{document.getElementById('database_search').value="";di_jq('database_search').removeClass('di_gui_searchbox_defaulttext');di_jq('database_search').addClass('di_gui_searchbox_text');document.getElementById('database_search').focus();}
function di_db_searchDbDetails(di_db_searchKey)
{var di_db_arrowNextImg="downArrow.gif";var di_db_inntHtml="";var isExist=false;var di_db_colon=":";var di_db_space=" ";var di_db_indicator,di_db_sg,di_db_source,di_db_records,di_db_adaptationName,di_db_desc,di_db_serverType,di_db_serverName,di_db_dbName,di_db_uid,di_db_pwd,di_db_portNo,di_db_dataValues,di_db_defLng,di_db_dbId;var CheckAdapation="";try
{if(this.di_db_dataCollection.length>0)
{di_db_inntHtml="<table style='width:100%;'>";for(var i=0;i<this.di_db_dataCollection.length;i++)
{var di_db_categoryNode=this.di_db_dataCollection[i];di_db_inntHtml+="<tr><td><table width='100%;'><tr onclick=\"di_db_ToggleDbDetail('db_divCategory"+i;di_db_inntHtml+="',this)\"><td class='di_gui_heading1 di_gui_cursor_pointer'>";di_db_inntHtml+="<img src='"+di_images_path+di_db_slash+di_db_arrowNextImg+"'/>&nbsp;&nbsp;"+di_db_categoryNode[0].toString()+"</td></tr>";di_db_inntHtml+="<tr><td><div id='db_divCategory"+i+"'>";for(var j=1;j<di_db_categoryNode.length;j++)
{var di_db_databaseNode=di_db_categoryNode[j];var db_db_localVarDbName=di_db_databaseNode[1].toLowerCase();var db_db_localVarDesc=di_db_databaseNode[13].toLowerCase();if(db_db_localVarDbName.indexOf(di_db_searchKey.toLowerCase())!=-1||db_db_localVarDesc.indexOf(di_db_searchKey.toLowerCase())!=-1)
{isExist=true;for(var k=0;k<di_db_databaseNode.length;k++)
{switch(k)
{case 0:di_db_dbId=di_db_databaseNode[k];break;case 1:di_db_adaptationName=di_db_databaseNode[k];break;case 2:di_db_area=di_db_databaseNode[k];break;case 3:di_db_indicator=di_db_databaseNode[k];break;case 4:di_db_source=di_db_databaseNode[k];break;case 5:di_db_dataValues=di_db_databaseNode[k];break;case 6:di_db_defLng=di_db_databaseNode[k];break;case 7:di_db_serverType=di_db_databaseNode[k];break;case 8:di_db_serverName=di_db_databaseNode[k];break;case 9:di_db_dbName=di_db_databaseNode[k];break;case 10:di_db_uid=di_db_databaseNode[k];break;case 11:di_db_pwd=di_db_databaseNode[k];break;case 12:di_db_portNo=di_db_databaseNode[k];break;case 13:di_db_desc=di_db_databaseNode[k];break;case 14:di_db_lastmod=di_db_databaseNode[k];break;}}
di_db_inntHtml+="<table width='100%'>";if(di_db_dbId.toLowerCase()==this.di_db_selectedDbId.toLowerCase())
{CheckAdapation=" checked";}
else
{CheckAdapation="";}
di_db_inntHtml+="<tr><td class='di_gui_heading2' colspan='2' style='padding-top:12px;'>";if(this.di_db_isMultipleDb)
{di_db_inntHtml+="<input type='checkbox' id='"+di_db_dbId+"' value='"+di_db_adaptationName+"'"+CheckAdapation+" />"}
di_db_inntHtml+=di_db_adaptationName+"</td></tr>";di_db_inntHtml+="<tr><td class='content' >"+di_db_desc+"</td>";if(!this.di_db_isMultipleDb)
{if(di_db_dbId.toLowerCase()==this.di_db_selectedDbId.toLowerCase())
{di_db_inntHtml+="<td rowspan='2' align='right' width='50px' valign='top'><button id='btn"+di_db_dbId.toLowerCase()+"' rowspan='2' class='di_gui_button2' onclick=\"di_db_setSelectedDB('"+di_db_dbId+"',this)\">"+this.di_db_inuseBtnText+"</button></td>";}
else
{di_db_inntHtml+="<td rowspan='2' align='right' width='50px' valign='top'><button id='btn"+di_db_dbId.toLowerCase()+"' rowspan='2' class='di_gui_button' onclick=\"di_db_setSelectedDB('"+di_db_dbId+"',this)\">"+this.di_db_selectBtnText+"</button></td>";}}
di_db_inntHtml+="</tr><tr><td class='di_gui_summary' style='font-style:italic; padding-top:2px;'><span>"+this.di_db_indicatorsText+di_db_colon+di_db_space+di_db_indicator;di_db_inntHtml+="</span>&nbsp;<span >"+this.di_db_areasText+di_db_colon+di_db_space+di_db_area;di_db_inntHtml+="</span>&nbsp;<span>"+this.di_db_sourcesText+di_db_colon+di_db_space+di_db_source;di_db_inntHtml+="</span>&nbsp;<span>"+this.di_db_datavaluesText+di_db_colon+di_db_space+di_db_dataValues+"</span>";di_db_inntHtml+="</span>&nbsp;<span >"+this.di_db_lastmodText+di_db_colon+di_db_space+di_db_lastmod;di_db_inntHtml+="</td><tr></table>";}}
di_db_inntHtml+="</div></td></tr></table></td></tr>";}
di_db_inntHtml+="</table>";}
if(!isExist)
{di_db_inntHtml="";}}
catch(err)
{di_db_inntHtml="";}
return di_db_inntHtml;}
function di_db_GetContainerItem()
{var di_db_arrowNextImg="downArrow.gif";var di_db_colon=":";var di_db_space=" ";var di_db_inntHtml="";var di_db_indicator,di_db_sg,di_db_source,di_db_records,di_db_adaptationName,di_db_desc,di_db_serverType,di_db_serverName,di_db_dbName,di_db_uid,di_db_pwd,di_db_portNo,di_db_dataValues,di_db_defLng,di_db_dbId;var CheckAdapation="";try
{if(this.di_db_dataCollection.length>0)
{di_db_inntHtml="<table width='100%;'>";for(var i=0;i<this.di_db_dataCollection.length;i++)
{var di_db_categoryNode=this.di_db_dataCollection[i];di_db_inntHtml+="<tr><td><table width='100%;'><tr onclick=\"di_db_ToggleDbDetail('db_divCategory"+i;di_db_inntHtml+="',this)\"><td class='di_gui_heading1 di_gui_cursor_pointer'>";di_db_inntHtml+="<img src='"+di_images_path+di_db_slash+di_db_arrowNextImg+"'/>&nbsp;&nbsp;"+di_db_categoryNode[0].toString()+"</td></tr>";di_db_inntHtml+="<tr><td><div id='db_divCategory"+i+"'>";for(var j=1;j<di_db_categoryNode.length;j++)
{var di_db_databaseNode=di_db_categoryNode[j];for(var k=0;k<di_db_databaseNode.length;k++)
{switch(k)
{case 0:di_db_dbId=di_db_databaseNode[k];break;case 1:di_db_adaptationName=di_db_databaseNode[k];break;case 2:di_db_area=di_db_databaseNode[k];break;case 3:di_db_indicator=di_db_databaseNode[k];break;case 4:di_db_source=di_db_databaseNode[k];break;case 5:di_db_dataValues=di_db_databaseNode[k];break;case 6:di_db_defLng=di_db_databaseNode[k];break;case 7:di_db_serverType=di_db_databaseNode[k];break;case 8:di_db_serverName=di_db_databaseNode[k];break;case 9:di_db_dbName=di_db_databaseNode[k];break;case 10:di_db_uid=di_db_databaseNode[k];break;case 11:di_db_pwd=di_db_databaseNode[k];break;case 12:di_db_portNo=di_db_databaseNode[k];break;case 13:di_db_desc=di_db_databaseNode[k];break;case 14:di_db_lastmod=di_db_databaseNode[k];break;}}
di_db_inntHtml+="<table width='100%'>";if(di_db_dbId.toLowerCase()==this.di_db_selectedDbId.toLowerCase())
{CheckAdapation=" checked";}
else
{CheckAdapation="";}
di_db_inntHtml+="<tr><td class='di_gui_heading2' colspan='2' style='padding-top:12px;'> ";if(this.di_db_isMultipleDb)
{di_db_inntHtml+="<input type='checkbox' id='"+di_db_dbId+"' value='"+di_db_adaptationName+"'"+CheckAdapation+" />"}
di_db_inntHtml+=di_db_adaptationName+"</td></tr>";di_db_inntHtml+="<tr><td class='content' >"+di_db_desc+"</td>";if(!this.di_db_isMultipleDb)
{if(di_db_dbId.toLowerCase()==this.di_db_selectedDbId.toLowerCase())
{di_db_inntHtml+="<td rowspan='2' align='right' width='50px' valign='top'><button id='btn"+di_db_dbId.toLowerCase()+"' rowspan='2' class='di_gui_button2' onclick=\"di_db_setSelectedDB('"+di_db_dbId+"',this)\">"+this.di_db_inuseBtnText+"</button></td>";}
else
{di_db_inntHtml+="<td rowspan='2' align='right' width='50px' valign='top'><button id='btn"+di_db_dbId.toLowerCase()+"' rowspan='2' class='di_gui_button' onclick=\"di_db_setSelectedDB('"+di_db_dbId+"',this)\">"+this.di_db_selectBtnText+"</button></td>";}}
di_db_inntHtml+="</tr><tr><td class='di_gui_summary' style='font-style:italic; padding-top:2px;'><span>"+this.di_db_indicatorsText+di_db_colon+di_db_space+di_db_indicator;di_db_inntHtml+="</span>&nbsp;<span >"+this.di_db_areasText+di_db_colon+di_db_space+di_db_area;di_db_inntHtml+="</span>&nbsp;<span>"+this.di_db_sourcesText+di_db_colon+di_db_space+di_db_source;di_db_inntHtml+="</span>&nbsp;<span>"+this.di_db_datavaluesText+di_db_colon+di_db_space+di_db_dataValues+"</span>";di_db_inntHtml+="</span>&nbsp;<span >"+this.di_db_lastmodText+di_db_colon+di_db_space+di_db_lastmod;di_db_inntHtml+="</td><tr></table>";}
di_db_inntHtml+="</div></td></tr></table></td></tr>";}
di_db_inntHtml+="</table>";}}
catch(err)
{di_db_inntHtml="";}
return di_db_inntHtml;}
var di_db_underScore='_';var doublePipe='||';function di_db_makeDataCollection(di_db_interfaceLng,di_db_dbFileObject)
{var di_db_pathExpression;var di_db_nsResolver;var di_db_indicator,di_db_sg,di_db_source,di_db_records,di_db_adaptationName,di_db_desc,di_db_serverType,di_db_serverName,di_db_dbName,di_db_uid,di_db_pwd,di_db_portNo,di_db_dataValues,di_db_defLng,di_db_dbId;try
{var di_db_databaseInfoNode;di_db_pathExpression="/dbinfo";if(window.ActiveXObject)
{di_db_databaseInfoNode=di_db_dbFileObject.selectSingleNode(di_db_pathExpression);for(var i=0;i<di_db_databaseInfoNode.childNodes.length;i++)
{var di_db_categoryNode=di_db_databaseInfoNode.childNodes[i];if(di_db_categoryNode.nodeName.toLowerCase()=="category")
{var categoryArray=new Array();categoryArray.push(di_db_categoryNode.getAttribute("name"));for(var j=0;j<di_db_categoryNode.childNodes.length;j++)
{var di_db_databaseNode=di_db_categoryNode.childNodes[j];if(di_db_databaseNode.nodeName.toLowerCase()=="db")
{var databaseArray=new Array();di_db_dbId=di_db_databaseNode.getAttribute("id");di_db_adaptationName=di_db_databaseNode.getAttribute("n");di_db_area=di_db_databaseNode.getAttribute("count").split(this.di_db_underScore)[0];di_db_indicator=di_db_databaseNode.getAttribute("count").split(this.di_db_underScore)[1];di_db_source=di_db_databaseNode.getAttribute("count").split(this.di_db_underScore)[2];di_db_dataValues=di_db_databaseNode.getAttribute("count").split(this.di_db_underScore)[3];di_db_defLng=di_db_databaseNode.getAttribute("deflng");di_db_serverType=di_db_databaseNode.getAttribute("dbConn").split(this.doublePipe)[0];di_db_serverName=di_db_databaseNode.getAttribute("dbConn").split(this.doublePipe)[1];di_db_dbName=di_db_databaseNode.getAttribute("dbConn").split(this.doublePipe)[2];di_db_uid=di_db_databaseNode.getAttribute("dbConn").split(this.doublePipe)[3];di_db_pwd=di_db_databaseNode.getAttribute("dbConn").split(this.doublePipe)[4];di_db_portNo=di_db_databaseNode.getAttribute("dbConn").split(this.doublePipe)[5];di_db_lastmod=di_db_databaseNode.getAttribute("lastmod");try
{di_db_desc=di_db_databaseNode.getAttribute("desc_"+di_db_interfaceLng);}
catch(err)
{di_db_desc="";}
databaseArray.push(di_db_dbId);databaseArray.push(di_db_adaptationName);databaseArray.push(di_db_area);databaseArray.push(di_db_indicator);databaseArray.push(di_db_source);databaseArray.push(di_db_dataValues);databaseArray.push(di_db_defLng);databaseArray.push(di_db_serverType);databaseArray.push(di_db_serverName);databaseArray.push(di_db_dbName);databaseArray.push(di_db_uid);databaseArray.push(di_db_pwd);databaseArray.push(di_db_portNo);databaseArray.push(di_db_desc);categoryArray.push(databaseArray);databaseArray.push(di_db_lastmod);}}
di_db_dataCollection.push(categoryArray);}}}
else
{di_db_nsResolver=di_db_dbFileObject.createNSResolver(di_db_dbFileObject.ownerDocument==null?di_db_dbFileObject.documentElement:di_db_dbFileObject.ownerDocument.documentElement);var di_db_databaseInfoNodes=di_db_dbFileObject.evaluate(di_db_pathExpression,di_db_dbFileObject,di_db_nsResolver,XPathResult.ANY_TYPE,null);di_db_databaseInfoNode=di_db_databaseInfoNodes.iterateNext();for(var i=0;i<di_db_databaseInfoNode.childNodes.length;i++)
{var di_db_categoryNode=di_db_databaseInfoNode.childNodes[i];if(di_db_categoryNode.nodeName.toLowerCase()=="category")
{var categoryArray=new Array();categoryArray.push(di_db_categoryNode.attributes["name"].nodeValue);for(var j=0;j<di_db_categoryNode.childNodes.length;j++)
{var di_db_databaseNode=di_db_categoryNode.childNodes[j];if(di_db_databaseNode.nodeName.toLowerCase()=="db")
{var databaseArray=new Array();di_db_dbId=di_db_databaseNode.attributes["id"].nodeValue;di_db_adaptationName=di_db_databaseNode.attributes["n"].nodeValue;di_db_area=di_db_databaseNode.attributes["count"].nodeValue.split(this.di_db_underScore)[0];di_db_indicator=di_db_databaseNode.attributes["count"].nodeValue.split(this.di_db_underScore)[1];di_db_source=di_db_databaseNode.attributes["count"].nodeValue.split(this.di_db_underScore)[2];di_db_dataValues=di_db_databaseNode.attributes["count"].nodeValue.split(this.di_db_underScore)[3];di_db_defLng=di_db_databaseNode.attributes["deflng"].nodeValue;di_db_serverType=di_db_databaseNode.attributes["dbConn"].nodeValue.split(this.doublePipe)[0];di_db_serverName=di_db_databaseNode.attributes["dbConn"].nodeValue.split(this.doublePipe)[1];di_db_dbName=di_db_databaseNode.attributes["dbConn"].nodeValue.split(this.doublePipe)[2];di_db_uid=di_db_databaseNode.attributes["dbConn"].nodeValue.split(this.doublePipe)[3];di_db_pwd=di_db_databaseNode.attributes["dbConn"].nodeValue.split(this.doublePipe)[4];di_db_portNo=di_db_databaseNode.attributes["dbConn"].nodeValue.split(this.doublePipe)[5];di_db_desc=di_db_databaseNode.attributes["desc_"+di_db_interfaceLng.toLowerCase()].nodeValue;di_db_lastmod=di_db_databaseNode.getAttribute("lastmod");databaseArray.push(di_db_dbId);databaseArray.push(di_db_adaptationName);databaseArray.push(di_db_area);databaseArray.push(di_db_indicator);databaseArray.push(di_db_source);databaseArray.push(di_db_dataValues);databaseArray.push(di_db_defLng);databaseArray.push(di_db_serverType);databaseArray.push(di_db_serverName);databaseArray.push(di_db_dbName);databaseArray.push(di_db_uid);databaseArray.push(di_db_pwd);databaseArray.push(di_db_portNo);databaseArray.push(di_db_desc);categoryArray.push(databaseArray);databaseArray.push(di_db_lastmod);}}
this.di_db_dataCollection.push(categoryArray);}}}}
catch(err)
{}}
function di_db_pageHeight()
{return window.innerHeight!=null?window.innerHeight:document.documentElement&&document.documentElement.clientHeight?document.documentElement.clientHeight:document.body!=null?document.body.clientHeight:null;}
function di_db_pageWidth()
{return window.innerWidth!=null?window.innerWidth:document.documentElement&&document.documentElement.clientWidth?document.documentElement.clientWidth:document.body!=null?document.body.clientWidth:null;}
function di_db_CloseComponent(val)
{var ChkId="";var ChkValue="";var close_fun_str;var ok_fun_str;try
{di_db_sel_db_nids=[];di_db_sel_db_names=[];if(val==1)
{di_jq("#dbDetailDiv :checked").each(function()
{ChkId=di_jq(this).context.id;di_db_sel_db_nids.push(ChkId);ChkValue=di_jq(this).context.value;di_db_sel_db_names.push(ChkValue);});if(this.di_db_component_ok_function_name!='')
{ok_fun_str=this.di_db_component_ok_function_name+"()";eval(ok_fun_str);}}
if(this.di_db_component_close_function_name!='')
{close_fun_str=this.di_db_component_close_function_name+"()";eval(close_fun_str);}}
catch(err){}}