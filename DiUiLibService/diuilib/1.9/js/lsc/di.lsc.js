
function di_lc_getXMLHTTPRequest()
{if(window.XMLHttpRequest)
{return new window.XMLHttpRequest();}
else
{for(i=0;i<xmlVersions.length;i++)
{try
{return new ActiveXObject(xmlVersions[i]);}
catch(ex)
{}}}}
function di_lc_LoadXMLs(di_lc_FileUrl)
{var di_lc_xhttp=di_lc_getXMLHTTPRequest();di_lc_xhttp.open("GET",di_lc_FileUrl,false);di_lc_xhttp.send("");return di_lc_xhttp.responseXML;}
function di_lc_parseLngFileAndGetMarkup(di_lc_lngFileObj,di_lc_divCtrlId)
{var di_lc_pathExpression;var di_lc_nsResolver;var di_lc_lnginfoNode,count=-1;try
{di_lc_pathExpression="/lnginfo";if(window.ActiveXObject)
{di_lc_lnginfoNode=di_lc_lngFileObj.selectSingleNode(di_lc_pathExpression);for(var i=0;i<di_lc_lnginfoNode.childNodes.length;i++)
{var di_db_lngNode=di_lc_lnginfoNode.childNodes[i];if(di_db_lngNode.nodeName.toLowerCase()=="lng")
{count++;var db_lc_itemValue=di_db_lngNode.getAttribute("code");var db_lc_itemText=di_db_lngNode.getAttribute("n");var db_lc_textDirection=di_db_lngNode.getAttribute("rtl");var di_lc_rtl=new Option();di_lc_rtl.code=db_lc_itemValue;di_lc_rtl.rtl=db_lc_textDirection;di_lc_collection.push(di_lc_rtl);var db_lc_item=new Option(db_lc_itemText,db_lc_itemValue);document.getElementById("di_lc_"+di_lc_divCtrlId).options[count]=db_lc_item}}}
else
{di_lc_nsResolver=di_lc_lngFileObj.createNSResolver(di_lc_lngFileObj.ownerDocument==null?di_lc_lngFileObj.documentElement:di_lc_lngFileObj.ownerDocument.documentElement);var di_lc_lnginfoNodes=di_lc_lngFileObj.evaluate(di_lc_pathExpression,di_lc_lngFileObj,di_lc_nsResolver,XPathResult.ANY_TYPE,null);di_lc_lnginfoNode=di_lc_lnginfoNodes.iterateNext();for(var i=0;i<di_lc_lnginfoNode.childNodes.length;i++)
{var di_db_lngNode=di_lc_lnginfoNode.childNodes[i];if(di_db_lngNode.nodeName.toLowerCase()=="lng")
{count++;var db_lc_itemValue=di_db_lngNode.attributes["code"].nodeValue;var db_lc_itemText=di_db_lngNode.attributes["n"].nodeValue;var db_lc_textDirection=di_db_lngNode.attributes["rtl"].nodeValue;var di_lc_rtl=new Option();di_lc_rtl.code=db_lc_itemValue;di_lc_rtl.rtl=db_lc_textDirection;di_lc_collection.push(di_lc_rtl);var db_lc_item=new Option(db_lc_itemText,db_lc_itemValue);document.getElementById("di_lc_"+di_lc_divCtrlId).options[count]=db_lc_item;}}}}
catch(err)
{}}
function di_lc_onChangeLanguage(di_lc_val)
{this.di_lc_selectedLanguage=di_lc_val;eval(this.di_lc_hotSelectionFunction);}