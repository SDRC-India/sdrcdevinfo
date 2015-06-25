using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevInfo.Lib.DI_LibDAL.Connection;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using iTextSharp.text;
using DevInfo.Lib.DI_LibSDMX;
using SDMXObjectModel;
using SDMXObjectModel.Registry;
using SDMXObjectModel.Common;
using DevInfo.Lib.DI_LibDAL.Queries;
using System.Runtime.Serialization.Formatters.Soap;
using System.Net;
using System.Net.Mail;
using SDMXObjectModel.Message;
using System.Text;
using DevInfo.Lib.DI_LibDATA;
using System.Web.Services.Protocols;
using SDMXObjectModel.Query;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;

public partial class Callback : System.Web.UI.Page
{
    #region "--Methods--"

    #region "--Private--"

    #region "--Codelists Mapping--"

    private string GetIndicatorMappingList(string DbNId, string Language, StructureType SourceCodelistStructure, SDMXApi_2_0.Message.StructureType TargetCodelistStructure, StructureType MappingCodelistStructure)
    {
        string RetVal;
        string DictSourceIndicatorValue, DictTargetIndicatorValue, DictMappingIndicatorValue, SourceIndicatorGId, TargetIndicatorGId;
        Dictionary<string, string> DictSourceIndicator, DictTargetIndicator, DictMappingIndicator;
        StringBuilder Builder;

        RetVal = string.Empty;
        DictSourceIndicatorValue = string.Empty;
        DictTargetIndicatorValue = string.Empty;
        DictMappingIndicatorValue = string.Empty;
        SourceIndicatorGId = string.Empty;
        TargetIndicatorGId = string.Empty;

        DictSourceIndicator = null;
        DictTargetIndicator = null;
        DictMappingIndicator = null;

        Builder = new StringBuilder(RetVal);
        string CodeListId = string.Empty;
        try
        {
            foreach (SDMXApi_2_0.Structure.DimensionType Dimensions in TargetCodelistStructure.KeyFamilies[0].Components.Dimension)
            {
                if (Dimensions.conceptRef == Constants.UNSD.Concept.Indicator.Id)
                {
                    CodeListId = Dimensions.codelist;
                }
            }
            DictSourceIndicator = this.Get_DictSourceCodelist(Language, DevInfo.Lib.DI_LibSDMX.Constants.CodeList.Indicator.Id, SourceCodelistStructure);
            DictTargetIndicator = this.Get_DictTargetCodelist(Language, CodeListId, TargetCodelistStructure);
            DictMappingIndicator = this.Get_DictMappingCodelist(DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.Indicator.id, MappingCodelistStructure);

            Builder.Append("<div style=\"overflow-x:hidden;\">");

            Builder.Append("<table id=\"tblIndicatorHeader\" style=\"width:100%; table-layout:fixed;\">");
            //Builder.Append("<col width=\"267\"/>");
            //Builder.Append("<col width=\"179\"/>");
            //Builder.Append("<col width=\"267\"/>");
            //Builder.Append("<col width=\"179\"/>");
            //Builder.Append("<col width=\"18\"/>");
            Builder.Append("<col width=\"30%\"/>");
            Builder.Append("<col width=\"19%\"/>");
            Builder.Append("<col width=\"30%\"/>");
            Builder.Append("<col width=\"19%\"/>");
            Builder.Append("<col width=\"1%\"/>");

            Builder.Append("<tr style=\"height:30px;\">");

            Builder.Append("<td colspan=\"5\" style=\"width:100%; align:left; overflow:hidden;\">");
            Builder.Append("<a id=\"aShowIndicatorAll\" href=\"javascript:void(0);\" style=\"color:#000000;\" onclick=\"FilterRowsByStatus('tblIndicator', 'aShowIndicatorAll', 'aShowIndicatorMapped', 'aShowIndicatorUnMapped', 'aShowIndicatorUnSaved', 'all');\"></a>");
            Builder.Append("   |   ");
            Builder.Append("<a id=\"aShowIndicatorMapped\" href=\"javascript:void(0);\" style=\"color:#1e90ff;\" onclick=\"FilterRowsByStatus('tblIndicator', 'aShowIndicatorAll', 'aShowIndicatorMapped', 'aShowIndicatorUnMapped', 'aShowIndicatorUnSaved', 'mapped');\"></a>&nbsp;");
            Builder.Append("<span style=\"background-color:#bbffbb;width:10px;\">&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            Builder.Append("   |   ");
            Builder.Append("<a id=\"aShowIndicatorUnMapped\" href=\"javascript:void(0);\" style=\"color:#1e90ff;\" onclick=\"FilterRowsByStatus('tblIndicator', 'aShowIndicatorAll', 'aShowIndicatorMapped', 'aShowIndicatorUnMapped', 'aShowIndicatorUnSaved', 'unmapped');\"></a>&nbsp;");
            Builder.Append("<span style=\"background-color:#ffbbbb;width:10px;\">&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            Builder.Append("   |   ");
            Builder.Append("<a id=\"aShowIndicatorUnSaved\" href=\"javascript:void(0);\" style=\"color:#1e90ff;\" onclick=\"FilterRowsByStatus('tblIndicator', 'aShowIndicatorAll', 'aShowIndicatorMapped', 'aShowIndicatorUnMapped', 'aShowIndicatorUnSaved', 'unsaved');\"></a>&nbsp;");
            Builder.Append("<span style=\"background-color:#bbbbff;width:10px;\">&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            Builder.Append("</td>");

            Builder.Append("</tr>");

            Builder.Append("<tr style=\"height:30px; background-color:#dddddd;\">");

            Builder.Append("<td style=\"width:30%; align:left; overflow:hidden;\">");
            Builder.Append("<b id=\"lang_DevInfo_Indicator\"></b>");
            Builder.Append("<div id=\"divIndicatorSearch\">");
            Builder.Append("<input id=\"txtIndicatorSearch\" type=\"text\"/>");
            Builder.Append("</div>");
            Builder.Append("</td>");

            Builder.Append("<td style=\"width:19%; align:left; overflow:hidden; padding-bottom:30px;\"><b id=\"lang_Indicator_GIds\"></b></td>");

            Builder.Append("<td style=\"width:30%; align:left; overflow:hidden;padding-bottom:30px;\">");

            Builder.Append("<b id=\"lang_UNSD_Indicator\"></b>");

            Builder.Append("<select id=\"ddlUNSDIndicator\" class=\"confg_frm_inp_bx_txt_dd\" style=\"display:none;\">");
            Builder.Append("<option value=\"-1\">Select</option>");

            foreach (KeyValuePair<string, string> TargetIndicator in DictTargetIndicator)
            {
                TargetIndicatorGId = TargetIndicator.Key;
                DictTargetIndicatorValue = TargetIndicator.Value;

                Builder.Append("<option value=\"" + TargetIndicatorGId + "\" title=\"" + DictTargetIndicatorValue + "\">" + this.Get_TrimmedInnerHTML(DictTargetIndicatorValue, 75) + "</option>");
            }

            Builder.Append("</select>");

            Builder.Append("</td>");

            Builder.Append("<td style=\"width:19%; align:left; overflow:hidden;padding-bottom:30px;\"><b id=\"lang_Indicator_Ids\"></b></td>");
            Builder.Append("<td style=\"width:1%; align:left; overflow:hidden;\"></td>");

            Builder.Append("</tr>");

            Builder.Append("</table>");

            Builder.Append("</div>");

            Builder.Append("<div style=\"overflow-y:auto; height:200px;\" class=\"chosenddloverflow\">");

            Builder.Append("<table id=\"tblIndicator\" style=\"width:100%; table-layout:fixed; z-index:-41\">");
            Builder.Append("<col width=\"30%\"/>");
            Builder.Append("<col width=\"19%\"/>");
            Builder.Append("<col width=\"30%\"/>");
            Builder.Append("<col width=\"19%\"/>");

            foreach (KeyValuePair<string, string> Indicator in DictSourceIndicator)
            {
                DictSourceIndicatorValue = string.Empty;
                DictTargetIndicatorValue = string.Empty;
                DictMappingIndicatorValue = string.Empty;

                SourceIndicatorGId = Indicator.Key;
                DictSourceIndicatorValue = Indicator.Value.Replace("'", "&#8217;"); ;

                DictMappingIndicator.TryGetValue(SourceIndicatorGId, out DictMappingIndicatorValue);

                if (!string.IsNullOrEmpty(DictMappingIndicatorValue))
                {
                    Builder.Append("<tr id=\"rowIndicator_" + SourceIndicatorGId + "\" style=\"background-color:rgb(221, 255, 221)\" status=\"mapped\">");
                }
                else
                {
                    Builder.Append("<tr id=\"rowIndicator_" + SourceIndicatorGId + "\" style=\"background-color:rgb(255, 221, 221)\" status=\"unmapped\">");
                }

                Builder.Append("<td style=\"width:30%; align:left; overflow:hidden;\">");
                Builder.Append("<span id=\"spanDevInfoIndicatorName_" + SourceIndicatorGId + "\" value=\"" + DictSourceIndicatorValue + "\" style=\"font-weight:bold;\" onmouseover=\"ShowCallout('divCallout', '" + DictSourceIndicatorValue + "', event);\" onmouseout=\"HideCallout('divCallout');\">" + this.Get_TrimmedInnerHTML(DictSourceIndicatorValue, 75) + "</span>");// onmouseover=\"ShowCallout('divCallout', '" + DictSourceIndicatorValue + "', event);\" onmouseout=\"HideCallout('divCallout');
                Builder.Append("</td>");

                Builder.Append("<td style=\"width:19%; align:left; overflow:hidden;\">");
                Builder.Append("<span id=\"spanDevInfoIndicatorGId_" + SourceIndicatorGId + "\" value=\"" + SourceIndicatorGId + "\" style=\"font-weight:bold;\"  onmouseover=\"ShowCallout('divCallout', '" + SourceIndicatorGId + "', event);\" onmouseout=\"HideCallout('divCallout');\">" + SourceIndicatorGId + "</span>");//onmouseover=\"ShowCallout('divCallout', '" + SourceIndicatorGId + "', event);\" onmouseout=\"HideCallout('divCallout');
                Builder.Append("</td>");

                Builder.Append("<td style=\"width:30%;height:23px;line-height:24px; align:left;\">");//overflow:hidden;
                //confg_frm_inp_bx_txt_dd_abc

                if (!string.IsNullOrEmpty(DictMappingIndicatorValue))
                {

                    //Builder.Append("<span id=\"div_" + SourceIndicatorGId + "\" onclick=\"AddOptionstoSelect(this);>");//chzn-select
                    Builder.Append("<select id=\"ddlUNSDIndicator_" + SourceIndicatorGId + "\" class=\"cus_slct_dd\"  style=\"width:245px; opacity:0.01;z-index:1000 ;position:relative\" onchange=\" SelectCodelistCode('tblIndicator', 'rowIndicator_" + SourceIndicatorGId + "', 'spanUNSDIndicatorGId_" + SourceIndicatorGId + "', this, '" + DictMappingIndicatorValue + "', 'rgb(221, 255, 221)', 'mapped');\">");
                    Builder.Append("<option value=\"-1\" id=\"SelectUNSDIndicator_" + SourceIndicatorGId + "\"></option>");
                    Builder.Append("<option value=\"" + DictMappingIndicatorValue + "\" selected=\"selected\" title=\"" + DictTargetIndicator[DictMappingIndicatorValue] + "\">" + this.Get_TrimmedInnerHTML(DictTargetIndicator[DictMappingIndicatorValue], 75) + "</option>");
                }
                else
                {

                    // Builder.Append("<div id=\"div_" + SourceIndicatorGId + "\" onclick=\"AddOptionstoSelect(this);>");//chzn-select
                    Builder.Append("<select id=\"ddlUNSDIndicator_" + SourceIndicatorGId + "\" class=\"cus_slct_dd\" style=\"width:245px; opacity:0.01;z-index:1000 ;\" onchange=\" SelectCodelistCode('tblIndicator', 'rowIndicator_" + SourceIndicatorGId + "', 'spanUNSDIndicatorGId_" + SourceIndicatorGId + "', this, '-1', 'rgb(255, 221, 221)', 'unmapped');\">");
                    Builder.Append("<option value=\"-1\" selected=\"selected\" id=\"SelectUNSDIndicator_" + SourceIndicatorGId + "\"></option>");
                }

                Builder.Append("</select>");
                // Builder.Append("<div id=\"remove_ddlUNSDIndicator_" + SourceIndicatorGId + "\"  class=\"cus_slct_dd chzn-select\" style=\"width:235px;top:-25px; font-size: 12px;\" >Select</div>");
                if (string.IsNullOrEmpty(DictMappingIndicatorValue))
                {
                    Builder.Append("<div id=\"remove_ddlUNSDIndicator_" + SourceIndicatorGId + "\"  class=\"cus_slct_dd chzn-select\" style=\"width:235px;top:-25px;font-size: 12px;\" >Select</div>");
                }
                else
                {
                    Builder.Append("<div id=\"remove_ddlUNSDIndicator_" + SourceIndicatorGId + "\"  class=\"cus_slct_dd chzn-select\" style=\"width:235px;top:-25px;font-size: 12px;\" >" + this.Get_TrimmedInnerHTML(DictTargetIndicator[DictMappingIndicatorValue], 75) + "</div>");
                }
                Builder.Append("</td>");


                Builder.Append("<td style=\"width:19%; align:left; overflow:hidden;\">");

                if (!string.IsNullOrEmpty(DictMappingIndicatorValue))
                {
                    Builder.Append("<span id=\"spanUNSDIndicatorGId_" + SourceIndicatorGId + "\" value=\"" + DictMappingIndicatorValue + "\"  onmouseover=\"ShowCallout('divCallout', '" + DictMappingIndicatorValue + "', event);\" onmouseout=\"HideCallout('divCallout');\">" + DictMappingIndicatorValue + "</span>");// onmouseover=\"ShowCallout('divCallout', '" + DictMappingIndicatorValue + "', event);\" onmouseout=\"HideCallout('divCallout');
                }
                else
                {
                    Builder.Append("<span id=\"spanUNSDIndicatorGId_" + SourceIndicatorGId + "\"></span>");
                }

                Builder.Append("</td>");

                Builder.Append("</tr>");
            }

            Builder.Append("</table>");

            Builder.Append("</div>");


        }
        catch (Exception ex)
        {
            Builder.Append("false" + Constants.Delimiters.ParamDelimiter + ex.Message);
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        RetVal = Builder.ToString();
        return RetVal;
    }

    private string GetUnitMappingList(string DbNId, string Language, StructureType SourceCodelistStructure, SDMXApi_2_0.Message.StructureType TargetCodelistStructure, StructureType MappingCodelistStructure)
    {
        string RetVal;
        string DictSourceUnitValue, DictTargetUnitValue, DictMappingUnitValue, SourceUnitGId, TargetUnitGId;
        Dictionary<string, string> DictSourceUnit, DictTargetUnit, DictMappingUnit;
        StringBuilder Builder;

        RetVal = string.Empty;
        DictSourceUnitValue = string.Empty;
        DictTargetUnitValue = string.Empty;
        DictMappingUnitValue = string.Empty;
        SourceUnitGId = string.Empty;
        TargetUnitGId = string.Empty;

        DictSourceUnit = null;
        DictTargetUnit = null;
        DictMappingUnit = null;

        Builder = new StringBuilder(RetVal);
        string CodeListId = string.Empty;
        try
        {
            foreach (SDMXApi_2_0.Structure.DimensionType Dimensions in TargetCodelistStructure.KeyFamilies[0].Components.Dimension)
            {
                if (Dimensions.conceptRef == Constants.UNSD.Concept.Unit.Id)
                {
                    CodeListId = Dimensions.codelist;
                }
            }
            DictSourceUnit = this.Get_DictSourceCodelist(Language, DevInfo.Lib.DI_LibSDMX.Constants.CodeList.Unit.Id, SourceCodelistStructure);
            DictTargetUnit = this.Get_DictTargetCodelist(Language, CodeListId, TargetCodelistStructure);
            DictMappingUnit = this.Get_DictMappingCodelist(DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.Unit.id, MappingCodelistStructure);

            Builder.Append("<div style=\"overflow-x:hidden;\">");

            Builder.Append("<table id=\"tblUnitHeader\" style=\"width:100%; table-layout:fixed;\">");
            //Builder.Append("<col width=\"267\"/>");
            //Builder.Append("<col width=\"179\"/>");
            //Builder.Append("<col width=\"267\"/>");
            //Builder.Append("<col width=\"179\"/>");
            //Builder.Append("<col width=\"18\"/>");

            Builder.Append("<col width=\"30%\"/>");
            Builder.Append("<col width=\"19%\"/>");
            Builder.Append("<col width=\"30%\"/>");
            Builder.Append("<col width=\"19%\"/>");
            Builder.Append("<col width=\"1%\"/>");

            Builder.Append("<tr style=\"height:30px;\">");

            Builder.Append("<td colspan=\"5\" style=\"width:100%; align:left; overflow:hidden;\">");
            Builder.Append("<a id=\"aShowUnitAll\" href=\"javascript:void(0);\" style=\"color:#000000;\" onclick=\"FilterRowsByStatus('tblUnit', 'aShowUnitAll', 'aShowUnitMapped', 'aShowUnitUnMapped', 'aShowUnitUnSaved', 'all');\"></a>");
            Builder.Append("   |   ");
            Builder.Append("<a id=\"aShowUnitMapped\" href=\"javascript:void(0);\" style=\"color:#1e90ff;\" onclick=\"FilterRowsByStatus('tblUnit', 'aShowUnitAll', 'aShowUnitMapped', 'aShowUnitUnMapped', 'aShowUnitUnSaved', 'mapped');\"></a>&nbsp;");
            Builder.Append("<span style=\"background-color:#bbffbb;width:10px;\">&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            Builder.Append("   |   ");
            Builder.Append("<a id=\"aShowUnitUnMapped\" href=\"javascript:void(0);\" style=\"color:#1e90ff;\" onclick=\"FilterRowsByStatus('tblUnit', 'aShowUnitAll', 'aShowUnitMapped', 'aShowUnitUnMapped', 'aShowUnitUnSaved', 'unmapped');\"></a>&nbsp;");
            Builder.Append("<span style=\"background-color:#ffbbbb;width:10px;\">&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            Builder.Append("   |   ");
            Builder.Append("<a id=\"aShowUnitUnSaved\" href=\"javascript:void(0);\" style=\"color:#1e90ff;\" onclick=\"FilterRowsByStatus('tblUnit', 'aShowUnitAll', 'aShowUnitMapped', 'aShowUnitUnMapped', 'aShowUnitUnSaved', 'unsaved');\"></a>&nbsp;");
            Builder.Append("<span style=\"background-color:#bbbbff;width:10px;\">&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            Builder.Append("</td>");

            Builder.Append("</tr>");

            Builder.Append("<tr style=\"height:30px; background-color:#dddddd;\">");

            Builder.Append("<td style=\"width:30%; align:left; overflow:hidden;\">");
            Builder.Append("<b id=\"lang_DevInfo_Unit\"></b>");
            Builder.Append("<div id=\"divUnitSearch\">");
            Builder.Append("<input id=\"txtUnitSearch\" type=\"text\"/>");
            Builder.Append("</div>");
            Builder.Append("</td>");

            Builder.Append("<td style=\"width:19%; align:left; overflow:hidden; padding-bottom:30px;\"><b id=\"lang_Unit_GIds\"></b></td>");

            Builder.Append("<td style=\"width:30%; align:left; overflow:hidden;padding-bottom:30px;\">");
            Builder.Append("<b id=\"lang_UNSD_Unit\"></b>");
            //Builder.Append("<div id=\"divUnitFilter\">");
            //Builder.Append("<input id=\"txtUnitFilter\" type=\"text\"/>");
            //Builder.Append("</div>");

            Builder.Append("<select id=\"ddlUNSDUnit\" class=\"confg_frm_inp_bx_txt_dd\" style=\"display:none;\">");
            Builder.Append("<option value=\"-1\">Select</option>");

            foreach (KeyValuePair<string, string> TargetUnit in DictTargetUnit)
            {
                TargetUnitGId = TargetUnit.Key;
                DictTargetUnitValue = TargetUnit.Value;

                Builder.Append("<option value=\"" + TargetUnitGId + "\" title=\"" + DictTargetUnitValue + "\">" + this.Get_TrimmedInnerHTML(DictTargetUnitValue, 75) + "</option>");
            }

            Builder.Append("</select>");

            Builder.Append("</td>");

            Builder.Append("<td style=\"width:19%; align:left; overflow:hidden;padding-bottom:30px;\"><b id=\"lang_Unit_Ids\"></b></td>");
            Builder.Append("<td style=\"width:1%; align:left; overflow:hidden;\"></td>");

            Builder.Append("</tr>");

            Builder.Append("</table>");

            Builder.Append("</div>");

            Builder.Append("<div style=\"overflow-y:auto; height:200px;\">");

            Builder.Append("<table id=\"tblUnit\" style=\"width:100%; table-layout:fixed;\">");
            Builder.Append("<col width=\"30%\"/>");
            Builder.Append("<col width=\"19%\"/>");
            Builder.Append("<col width=\"30%\"/>");
            Builder.Append("<col width=\"19%\"/>");

            foreach (KeyValuePair<string, string> Unit in DictSourceUnit)
            {
                DictSourceUnitValue = string.Empty;
                DictTargetUnitValue = string.Empty;
                DictMappingUnitValue = string.Empty;

                SourceUnitGId = Unit.Key;
                DictSourceUnitValue = Unit.Value;

                DictMappingUnit.TryGetValue(SourceUnitGId, out DictMappingUnitValue);

                if (!string.IsNullOrEmpty(DictMappingUnitValue))
                {
                    Builder.Append("<tr id=\"rowUnit_" + SourceUnitGId + "\" style=\"background-color:rgb(221, 255, 221)\" status=\"mapped\">");
                }
                else
                {
                    Builder.Append("<tr id=\"rowUnit_" + SourceUnitGId + "\" style=\"background-color:rgb(255, 221, 221)\" status=\"unmapped\">");
                }

                Builder.Append("<td style=\"width:30%; align:left; overflow:hidden;\">");
                Builder.Append("<span id=\"spanDevInfoUnitName_" + SourceUnitGId + "\" value=\"" + DictSourceUnitValue + "\" style=\"font-weight:bold;\" onmouseover=\"ShowCallout('divCallout', '" + DictSourceUnitValue + "', event);\" onmouseout=\"HideCallout('divCallout');\">" + this.Get_TrimmedInnerHTML(DictSourceUnitValue, 75) + "</span>");//onmouseover=\"ShowCallout('divCallout', '" + DictSourceUnitValue + "', event);\" onmouseout=\"HideCallout('divCallout');
                Builder.Append("</td>");

                Builder.Append("<td style=\"width:19%; align:left; overflow:hidden;\">");
                Builder.Append("<span id=\"spanDevInfoUnitGId_" + SourceUnitGId + "\" value=\"" + SourceUnitGId + "\" onmouseover=\"ShowCallout('divCallout', '" + SourceUnitGId + "', event);\" onmouseout=\"HideCallout('divCallout');\">" + SourceUnitGId + "</span>");// onmouseover=\"ShowCallout('divCallout', '" + SourceUnitGId + "', event);\" onmouseout=\"HideCallout('divCallout');
                Builder.Append("</td>");

                Builder.Append("<td style=\"width:30%; align:left; \">");//overflow:hidden;

                if (!string.IsNullOrEmpty(DictMappingUnitValue))
                {// claass chzn-select
                    Builder.Append("<select id=\"ddlUNSDUnit_" + SourceUnitGId + "\" class=\"cus_slct_dd\" style=\"width:245px; opacity:0.01;z-index:1000;position:relative\" onchange=\"SelectCodelistCode('tblUnit', 'rowUnit_" + SourceUnitGId + "', 'spanUNSDUnitGId_" + SourceUnitGId + "', this, '" + DictMappingUnitValue + "', 'rgb(221, 255, 221)', 'mapped');\">");
                    Builder.Append("<option value=\"-1\" id=\"SelectUNSDUnit_" + SourceUnitGId + "\"></option>");
                    Builder.Append("<option value=\"" + DictMappingUnitValue + "\" selected=\"selected\" title=\"" + DictTargetUnit[DictMappingUnitValue] + "\">" + this.Get_TrimmedInnerHTML(DictTargetUnit[DictMappingUnitValue], 75) + "</option>");
                }
                else
                {//class chzn-select
                    Builder.Append("<select id=\"ddlUNSDUnit_" + SourceUnitGId + "\" class=\"cus_slct_dd\" style=\"width:245px;opacity:0.01;z-index:1000;\" onchange=\"SelectCodelistCode('tblUnit', 'rowUnit_" + SourceUnitGId + "', 'spanUNSDUnitGId_" + SourceUnitGId + "', this, '-1', 'rgb(255, 221, 221)', 'unmapped');\">");
                    Builder.Append("<option value=\"-1\" selected=\"selected\" id=\"SelectUNSDUnit_" + SourceUnitGId + "\"></option>");
                }

                Builder.Append("</select>");
                //Builder.Append("<div id=\"remove_ddlUNSDUnit_" + SourceUnitGId + "\"  class=\"cus_slct_dd chzn-select\" style=\"width:235px;top:-25px;font-size: 12px;\" >Select</div>");
                if (string.IsNullOrEmpty(DictMappingUnitValue))
                {
                    Builder.Append("<div id=\"remove_ddlUNSDUnit_" + SourceUnitGId + "\"  class=\"cus_slct_dd chzn-select\" style=\"width:235px;top:-25px;font-size: 12px;\" >Select</div>");
                }
                else
                {
                    Builder.Append("<div id=\"remove_ddlUNSDUnit_" + SourceUnitGId + "\"  class=\"cus_slct_dd chzn-select\" style=\"width:235px;top:-25px;font-size: 12px;\" >" + this.Get_TrimmedInnerHTML(DictTargetUnit[DictMappingUnitValue], 75) + "</div>");
                }

                Builder.Append("</td>");

                Builder.Append("<td style=\"width:169px; align:left; overflow:hidden;\">");

                if (!string.IsNullOrEmpty(DictMappingUnitValue))
                {
                    Builder.Append("<span id=\"spanUNSDUnitGId_" + SourceUnitGId + "\" value=\"" + DictMappingUnitValue + "\" onmouseover=\"ShowCallout('divCallout', '" + DictMappingUnitValue + "', event);\" onmouseout=\"HideCallout('divCallout');\">" + DictMappingUnitValue + "</span>");// onmouseover=\"ShowCallout('divCallout', '" + DictMappingUnitValue + "', event);\" onmouseout=\"HideCallout('divCallout');
                }
                else
                {
                    Builder.Append("<span id=\"spanUNSDUnitGId_" + SourceUnitGId + "\"></span>");
                }

                Builder.Append("</td>");

                Builder.Append("</tr>");
            }

            Builder.Append("</table>");

            Builder.Append("</div>");
        }
        catch (Exception ex)
        {
            Builder.Append("false" + Constants.Delimiters.ParamDelimiter + ex.Message);
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        RetVal = Builder.ToString();
        return RetVal;
    }

    private string GetAgeMappingList(string DbNId, string Language, string SelectedAgeCodeList, StructureType SourceCodelistStructure, SDMXApi_2_0.Message.StructureType TargetCodelistStructure, StructureType MappingCodelistStructure)
    {
        string RetVal;
        string DictSourceAgeValue, DictTargetAgeValue, DictMappingAgeValue, SourceAgeGId, TargetAgeGId;
        Dictionary<string, string> DictSourceAge, DictTargetAge, DictMappingAge;
        StringBuilder Builder;

        RetVal = string.Empty;
        DictSourceAgeValue = string.Empty;
        DictTargetAgeValue = string.Empty;
        DictMappingAgeValue = string.Empty;
        SourceAgeGId = string.Empty;
        TargetAgeGId = string.Empty;

        DictSourceAge = null;
        DictTargetAge = null;
        DictMappingAge = null;

        Builder = new StringBuilder(RetVal);
        string CodeListId = string.Empty;
        try
        {
            if (string.IsNullOrEmpty(SelectedAgeCodeList))
            {
                SelectedAgeCodeList = DevInfo.Lib.DI_LibSDMX.Constants.CodelistPrefix + "AGE";
            }

            foreach (SDMXApi_2_0.Structure.DimensionType Dimensions in TargetCodelistStructure.KeyFamilies[0].Components.Dimension)
            {
                if (Dimensions.conceptRef == Constants.UNSD.Concept.Age.Id)
                {
                    CodeListId = Dimensions.codelist;
                }
            }
            DictSourceAge = this.Get_DictSourceCodelist(Language, SelectedAgeCodeList, SourceCodelistStructure);
            DictTargetAge = this.Get_DictTargetCodelist(Language, CodeListId, TargetCodelistStructure);//Constants.UNSD.CodeList.Age.Id
            DictMappingAge = this.Get_DictMappingCodelist(DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.Age.id, MappingCodelistStructure);

            Builder.Append("<div style=\"overflow-x:hidden;\">");

            Builder.Append("<table id=\"tblAgeHeader\" style=\"width:100%; table-layout:fixed;\">");
            Builder.Append("<col width=\"30%\"/>");
            Builder.Append("<col width=\"19%\"/>");
            Builder.Append("<col width=\"30%\"/>");
            Builder.Append("<col width=\"19%\"/>");
            Builder.Append("<col width=\"1%\"/>");

            Builder.Append("<tr style=\"height:30px;\">");

            Builder.Append("<td colspan=\"5\" style=\"width:100%; align:left; overflow:hidden;\">");
            Builder.Append("<a id=\"aShowAgeAll\" href=\"javascript:void(0);\" style=\"color:#000000;\" onclick=\"FilterRowsByStatus('tblAge', 'aShowAgeAll', 'aShowAgeMapped', 'aShowAgeUnMapped', 'aShowAgeUnSaved', 'all');\"></a>");
            Builder.Append("   |   ");
            Builder.Append("<a id=\"aShowAgeMapped\" href=\"javascript:void(0);\" style=\"color:#1e90ff;\" onclick=\"FilterRowsByStatus('tblAge', 'aShowAgeAll', 'aShowAgeMapped', 'aShowAgeUnMapped', 'aShowAgeUnSaved', 'mapped');\"></a>&nbsp;");
            Builder.Append("<span style=\"background-color:#bbffbb;width:10px;\">&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            Builder.Append("   |   ");
            Builder.Append("<a id=\"aShowAgeUnMapped\" href=\"javascript:void(0);\" style=\"color:#1e90ff;\" onclick=\"FilterRowsByStatus('tblAge', 'aShowAgeAll', 'aShowAgeMapped', 'aShowAgeUnMapped', 'aShowAgeUnSaved', 'unmapped');\"></a>&nbsp;");
            Builder.Append("<span style=\"background-color:#ffbbbb;width:10px;\">&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            Builder.Append("   |   ");
            Builder.Append("<a id=\"aShowAgeUnSaved\" href=\"javascript:void(0);\" style=\"color:#1e90ff;\" onclick=\"FilterRowsByStatus('tblAge', 'aShowAgeAll', 'aShowAgeMapped', 'aShowAgeUnMapped', 'aShowAgeUnSaved', 'unsaved');\"></a>&nbsp;");
            Builder.Append("<span style=\"background-color:#bbbbff;width:10px;\">&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            Builder.Append("</td>");

            Builder.Append("</tr>");

            Builder.Append("<tr style=\"height:30px; background-color:#dddddd;\">");

            Builder.Append("<td style=\"width:30%; align:left; overflow:hidden;\">");
            Builder.Append("<b id=\"lang_DevInfo_Age\"></b>");
            Builder.Append("<div id=\"divAgeSearch\">");
            Builder.Append("<input id=\"txtAgeSearch\" type=\"text\"/>");
            Builder.Append("</div>");
            Builder.Append("</td>");

            Builder.Append("<td style=\"width:19%; align:left; overflow:hidden;\"><b id=\"lang_Age_GIds\"></b></td>");

            Builder.Append("<td style=\"width:30%; align:left; overflow:hidden;\">");
            Builder.Append("<b id=\"lang_UNSD_Age\"></b>");
            //Builder.Append("<div id=\"divAgeFilter\">");
            //Builder.Append("<input id=\"txtAgeFilter\" type=\"text\"/>");
            //Builder.Append("</div>");

            Builder.Append("<select id=\"ddlUNSDAge\" class=\"confg_frm_inp_bx_txt_dd\" style=\"display:none;\">");
            Builder.Append("<option value=\"-1\">Select</option>");

            foreach (KeyValuePair<string, string> TargetAge in DictTargetAge)
            {
                TargetAgeGId = TargetAge.Key;
                DictTargetAgeValue = TargetAge.Value;

                Builder.Append("<option value=\"" + TargetAgeGId + "\" title=\"" + DictTargetAgeValue + "\">" + this.Get_TrimmedInnerHTML(DictTargetAgeValue, 75) + "</option>");
            }

            Builder.Append("</select>");
            Builder.Append("</td>");

            Builder.Append("<td style=\"width:19%; align:left; overflow:hidden;\"><b id=\"lang_Age_Ids\"></b></td>");
            Builder.Append("<td style=\"width:1%; align:left; overflow:hidden;\"></td>");

            Builder.Append("</tr>");

            Builder.Append("</table>");

            Builder.Append("</div>");

            Builder.Append("<div style=\"overflow-y:auto; height:200px;\">");

            Builder.Append("<table id=\"tblAge\" style=\"width:100%; table-layout:fixed;\">");
            Builder.Append("<col width=\"30%\"/>");
            Builder.Append("<col width=\"19%\"/>");
            Builder.Append("<col width=\"30%\"/>");
            Builder.Append("<col width=\"19%\"/>");

            foreach (KeyValuePair<string, string> Age in DictSourceAge)
            {
                DictSourceAgeValue = string.Empty;
                DictTargetAgeValue = string.Empty;
                DictMappingAgeValue = string.Empty;

                SourceAgeGId = Age.Key;
                DictSourceAgeValue = Age.Value;

                DictMappingAge.TryGetValue(SourceAgeGId, out DictMappingAgeValue);

                if (!string.IsNullOrEmpty(DictMappingAgeValue))
                {
                    Builder.Append("<tr id=\"rowAge_" + SourceAgeGId + "\" style=\"background-color:rgb(221, 255, 221)\" status=\"mapped\">");
                }
                else
                {
                    Builder.Append("<tr id=\"rowAge_" + SourceAgeGId + "\" style=\"background-color:rgb(255, 221, 221)\" status=\"unmapped\">");
                }

                Builder.Append("<td style=\"width:30%; align:left; overflow:hidden;\">");
                Builder.Append("<span id=\"spanDevInfoAgeName_" + SourceAgeGId + "\" value=\"" + DictSourceAgeValue + "\" style=\"font-weight:bold;\" onmouseover=\"ShowCallout('divCallout', '" + DictSourceAgeValue + "', event);\" onmouseout=\"HideCallout('divCallout');\">" + this.Get_TrimmedInnerHTML(DictSourceAgeValue, 75) + "</span>");// onmouseover=\"ShowCallout('divCallout', '" + DictSourceAgeValue + "', event);\" onmouseout=\"HideCallout('divCallout');
                Builder.Append("</td>");

                Builder.Append("<td style=\"width:19%; align:left; overflow:hidden;\">");
                Builder.Append("<span id=\"spanDevInfoAgeGId_" + SourceAgeGId + "\" value=\"" + SourceAgeGId + "\" onmouseover=\"ShowCallout('divCallout', '" + SourceAgeGId + "', event);\" onmouseout=\"HideCallout('divCallout');\">" + SourceAgeGId + "</span>");// onmouseover=\"ShowCallout('divCallout', '" + SourceAgeGId + "', event);\" onmouseout=\"HideCallout('divCallout');
                Builder.Append("</td>");

                Builder.Append("<td style=\"width:30%; align:left;\">");//overflow:hidden;

                if (!string.IsNullOrEmpty(DictMappingAgeValue))
                {
                    //confg_frm_inp_bx_txt_dd chzn-select
                    Builder.Append("<select id=\"ddlUNSDAge_" + SourceAgeGId + "\" class=\"cus_slct_dd\" style=\"width:245px; opacity:0.01;z-index:1000 ;\" onchange=\"SelectCodelistCode('tblAge', 'rowAge_" + SourceAgeGId + "', 'spanUNSDAgeGId_" + SourceAgeGId + "', this, '" + DictMappingAgeValue + "', 'rgb(221, 255, 221)', 'mapped');\">");
                    Builder.Append("<option value=\"-1\" id=\"SelectUNSDAge_" + SourceAgeGId + "\"></option>");
                    Builder.Append("<option value=\"" + DictMappingAgeValue + "\" selected=\"selected\" title=\"" + DictTargetAge[DictMappingAgeValue] + "\">" + this.Get_TrimmedInnerHTML(DictTargetAge[DictMappingAgeValue], 75) + "</option>");
                }
                else
                {
                    Builder.Append("<select id=\"ddlUNSDAge_" + SourceAgeGId + "\" class=\"cus_slct_dd\" style=\"width:245px;opacity:0.01;z-index:1000 ;\" onchange=\"SelectCodelistCode('tblAge', 'rowAge_" + SourceAgeGId + "', 'spanUNSDAgeGId_" + SourceAgeGId + "', this, '-1', 'rgb(255, 221, 221)', 'unmapped');\">");
                    Builder.Append("<option value=\"-1\" selected=\"selected\" id=\"SelectUNSDAge_" + SourceAgeGId + "\"></option>");
                }

                Builder.Append("</select>");
                //  Builder.Append("<div id=\"remove_ddlUNSDAge_" + SourceAgeGId + "\"  class=\"cus_slct_dd chzn-select\" style=\"width:235px;top:-25px;font-size: 12px;\" >Select</div>");
                if (string.IsNullOrEmpty(DictMappingAgeValue))
                {
                    Builder.Append("<div id=\"remove_ddlUNSDAge_" + SourceAgeGId + "\"  class=\"cus_slct_dd chzn-select\" style=\"width:235px;top:-25px;font-size: 12px;\" >Select</div>");
                }
                else
                {
                    Builder.Append("<div id=\"remove_ddlUNSDAge_" + SourceAgeGId + "\"  class=\"cus_slct_dd chzn-select\" style=\"width:235px;top:-25px;font-size: 12px;\" >" + this.Get_TrimmedInnerHTML(DictTargetAge[DictMappingAgeValue], 75) + "</div>");
                }

                Builder.Append("</td>");


                Builder.Append("<td style=\"width:19%; align:left; overflow:hidden;\">");

                if (!string.IsNullOrEmpty(DictMappingAgeValue))
                {
                    Builder.Append("<span id=\"spanUNSDAgeGId_" + SourceAgeGId + "\" value=\"" + DictMappingAgeValue + "\"  onmouseover=\"ShowCallout('divCallout', '" + DictMappingAgeValue + "', event);\" onmouseout=\"HideCallout('divCallout');\">" + DictMappingAgeValue + "</span>");//onmouseover=\"ShowCallout('divCallout', '" + DictMappingAgeValue + "', event);\" onmouseout=\"HideCallout('divCallout');
                }
                else
                {
                    Builder.Append("<span id=\"spanUNSDAgeGId_" + SourceAgeGId + "\"></span>");
                }

                Builder.Append("</td>");

                Builder.Append("</tr>");
            }

            Builder.Append("</table>");

            Builder.Append("</div>");
        }
        catch (Exception ex)
        {
            Builder.Append("false" + Constants.Delimiters.ParamDelimiter + ex.Message);
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        RetVal = Builder.ToString();
        return RetVal;
    }

    private string GetSexMappingList(string DbNId, string Language, string SelectedSexCodeList, StructureType SourceCodelistStructure, SDMXApi_2_0.Message.StructureType TargetCodelistStructure, StructureType MappingCodelistStructure)
    {
        string RetVal;
        string DictSourceSexValue, DictTargetSexValue, DictMappingSexValue, SourceSexGId, TargetSexGId;
        Dictionary<string, string> DictSourceSex, DictTargetSex, DictMappingSex;
        StringBuilder Builder;

        RetVal = string.Empty;
        DictSourceSexValue = string.Empty;
        DictTargetSexValue = string.Empty;
        DictMappingSexValue = string.Empty;
        SourceSexGId = string.Empty;
        TargetSexGId = string.Empty;

        DictSourceSex = null;
        DictTargetSex = null;
        DictMappingSex = null;

        Builder = new StringBuilder(RetVal);
        string CodeListId = string.Empty;
        try
        {

            if (string.IsNullOrEmpty(SelectedSexCodeList))
            {
                SelectedSexCodeList = DevInfo.Lib.DI_LibSDMX.Constants.CodelistPrefix + "SEX";
            }
            foreach (SDMXApi_2_0.Structure.DimensionType Dimensions in TargetCodelistStructure.KeyFamilies[0].Components.Dimension)
            {
                if (Dimensions.conceptRef == Constants.UNSD.Concept.Sex.Id)
                {
                    CodeListId = Dimensions.codelist;
                }
            }
            DictSourceSex = this.Get_DictSourceCodelist(Language, SelectedSexCodeList, SourceCodelistStructure);
            DictTargetSex = this.Get_DictTargetCodelist(Language, CodeListId, TargetCodelistStructure);//Constants.UNSD.CodeList.Sex.Id
            DictMappingSex = this.Get_DictMappingCodelist(DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.Sex.id, MappingCodelistStructure);

            Builder.Append("<div style=\"overflow-x:hidden;\">");

            Builder.Append("<table id=\"tblSexHeader\" style=\"width:100%; table-layout:fixed;\">");
            Builder.Append("<col width=\"30%\"/>");
            Builder.Append("<col width=\"19%\"/>");
            Builder.Append("<col width=\"30%\"/>");
            Builder.Append("<col width=\"19%\"/>");
            Builder.Append("<col width=\"1%\"/>");

            Builder.Append("<tr style=\"height:30px;\">");

            Builder.Append("<td colspan=\"5\" style=\"width:100%; align:left; overflow:hidden;\">");
            Builder.Append("<a id=\"aShowSexAll\" href=\"javascript:void(0);\" style=\"color:#000000;\" onclick=\"FilterRowsByStatus('tblSex', 'aShowSexAll', 'aShowSexMapped', 'aShowSexUnMapped', 'aShowSexUnSaved', 'all');\"></a>");
            Builder.Append("   |   ");
            Builder.Append("<a id=\"aShowSexMapped\" href=\"javascript:void(0);\" style=\"color:#1e90ff;\" onclick=\"FilterRowsByStatus('tblSex', 'aShowSexAll', 'aShowSexMapped', 'aShowSexUnMapped', 'aShowSexUnSaved', 'mapped');\"></a>&nbsp;");
            Builder.Append("<span style=\"background-color:#bbffbb;width:10px;\">&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            Builder.Append("   |   ");
            Builder.Append("<a id=\"aShowSexUnMapped\" href=\"javascript:void(0);\" style=\"color:#1e90ff;\" onclick=\"FilterRowsByStatus('tblSex', 'aShowSexAll', 'aShowSexMapped', 'aShowSexUnMapped', 'aShowSexUnSaved', 'unmapped');\"></a>&nbsp;");
            Builder.Append("<span style=\"background-color:#ffbbbb;width:10px;\">&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            Builder.Append("   |   ");
            Builder.Append("<a id=\"aShowSexUnSaved\" href=\"javascript:void(0);\" style=\"color:#1e90ff;\" onclick=\"FilterRowsByStatus('tblSex', 'aShowSexAll', 'aShowSexMapped', 'aShowSexUnMapped', 'aShowSexUnSaved', 'unsaved');\"></a>&nbsp;");
            Builder.Append("<span style=\"background-color:#bbbbff;width:10px;\">&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            Builder.Append("</td>");

            Builder.Append("</tr>");

            Builder.Append("<tr style=\"height:30px; background-color:#dddddd;\">");

            Builder.Append("<td style=\"width:30%; align:left; overflow:hidden;\">");
            Builder.Append("<b id=\"lang_DevInfo_Sex\"></b>");
            Builder.Append("<div id=\"divSexSearch\">");
            Builder.Append("<input id=\"txtSexSearch\" type=\"text\"/>");
            Builder.Append("</div>");
            Builder.Append("</td>");

            Builder.Append("<td style=\"width:19%; align:left; overflow:hidden;\"><b id=\"lang_Sex_GIds\"></b></td>");

            Builder.Append("<td style=\"width:30%; align:left; overflow:hidden;\">");
            Builder.Append("<b id=\"lang_UNSD_Sex\"></b>");
            //Builder.Append("<div id=\"divSexFilter\">");
            //Builder.Append("<input id=\"txtSexFilter\" type=\"text\"/>");
            //Builder.Append("</div>");

            Builder.Append("<select id=\"ddlUNSDSex\" class=\"confg_frm_inp_bx_txt_dd\" style=\"display:none;\">");
            Builder.Append("<option value=\"-1\">Select</option>");

            foreach (KeyValuePair<string, string> TargetSex in DictTargetSex)
            {
                TargetSexGId = TargetSex.Key;
                DictTargetSexValue = TargetSex.Value;

                Builder.Append("<option value=\"" + TargetSexGId + "\" title=\"" + DictTargetSexValue + "\">" + this.Get_TrimmedInnerHTML(DictTargetSexValue, 75) + "</option>");
            }

            Builder.Append("</select>");
            Builder.Append("</td>");

            Builder.Append("<td style=\"width:19%; align:left; overflow:hidden;\"><b id=\"lang_Sex_Ids\"></b></td>");
            Builder.Append("<td style=\"width:1%; align:left; overflow:hidden;\"></td>");

            Builder.Append("</tr>");

            Builder.Append("</table>");

            Builder.Append("</div>");

            Builder.Append("<div style=\"overflow-y:auto; height:200px;\">");

            Builder.Append("<table id=\"tblSex\" style=\"width:100%; table-layout:fixed;\">");
            Builder.Append("<col width=\"30%\"/>");
            Builder.Append("<col width=\"19%\"/>");
            Builder.Append("<col width=\"30%\"/>");
            Builder.Append("<col width=\"19%\"/>");

            foreach (KeyValuePair<string, string> Sex in DictSourceSex)
            {
                DictSourceSexValue = string.Empty;
                DictTargetSexValue = string.Empty;
                DictMappingSexValue = string.Empty;

                SourceSexGId = Sex.Key;
                DictSourceSexValue = Sex.Value;

                DictMappingSex.TryGetValue(SourceSexGId, out DictMappingSexValue);

                if (!string.IsNullOrEmpty(DictMappingSexValue))
                {
                    Builder.Append("<tr id=\"rowSex_" + SourceSexGId + "\" style=\"background-color:rgb(221, 255, 221)\" status=\"mapped\">");
                }
                else
                {
                    Builder.Append("<tr id=\"rowSex_" + SourceSexGId + "\" style=\"background-color:rgb(255, 221, 221)\" status=\"unmapped\">");
                }

                Builder.Append("<td style=\"width:30%; align:left; overflow:hidden;\">");
                Builder.Append("<span id=\"spanDevInfoSexName_" + SourceSexGId + "\" value=\"" + DictSourceSexValue + "\" style=\"font-weight:bold;\" onmouseover=\"ShowCallout('divCallout', '" + DictSourceSexValue + "', event);\" onmouseout=\"HideCallout('divCallout');\">" + this.Get_TrimmedInnerHTML(DictSourceSexValue, 75) + "</span>");// onmouseover=\"ShowCallout('divCallout', '" + DictSourceSexValue + "', event);\" onmouseout=\"HideCallout('divCallout');
                Builder.Append("</td>");

                Builder.Append("<td style=\"width:19%; align:left; overflow:hidden;\">");
                Builder.Append("<span id=\"spanDevInfoSexGId_" + SourceSexGId + "\" value=\"" + SourceSexGId + "\" onmouseover=\"ShowCallout('divCallout', '" + SourceSexGId + "', event);\" onmouseout=\"HideCallout('divCallout');\">" + SourceSexGId + "</span>");//onmouseover=\"ShowCallout('divCallout', '" + SourceSexGId + "', event);\" onmouseout=\"HideCallout('divCallout');
                Builder.Append("</td>");

                Builder.Append("<td style=\"width:30%; align:left; \">");//overflow:hidden;

                if (!string.IsNullOrEmpty(DictMappingSexValue))
                {
                    //confg_frm_inp_bx_txt_dd chzn-select 
                    Builder.Append("<select id=\"ddlUNSDSex_" + SourceSexGId + "\" class=\"cus_slct_dd\" style=\"width:245px;opacity:0.01;z-index:1000;\" onchange=\"SelectCodelistCode('tblSex', 'rowSex_" + SourceSexGId + "', 'spanUNSDSexGId_" + SourceSexGId + "', this, '" + DictMappingSexValue + "', 'rgb(221, 255, 221)', 'mapped');\">");
                    Builder.Append("<option value=\"-1\" id=\"SelectUNSDSex_" + SourceSexGId + "\"></option>");
                    Builder.Append("<option value=\"" + DictMappingSexValue + "\" selected=\"selected\" title=\"" + DictTargetSex[DictMappingSexValue] + "\">" + this.Get_TrimmedInnerHTML(DictTargetSex[DictMappingSexValue], 75) + "</option>");
                }
                else
                {
                    Builder.Append("<select id=\"ddlUNSDSex_" + SourceSexGId + "\" class=\"cus_slct_dd\" style=\"width:245px;opacity:0.01;z-index:1000;\" onchange=\"SelectCodelistCode('tblSex', 'rowSex_" + SourceSexGId + "', 'spanUNSDSexGId_" + SourceSexGId + "', this, '-1', 'rgb(255, 221, 221)', 'unmapped');\">");
                    Builder.Append("<option value=\"-1\" selected=\"selected\" id=\"SelectUNSDSex_" + SourceSexGId + "\"></option>");
                }

                Builder.Append("</select>");
                //   Builder.Append("<div id=\"remove_ddlUNSDSex_" + SourceSexGId + "\"  class=\"cus_slct_dd chzn-select\" style=\"width:235px;top:-25px;\" >Select</div>");
                if (string.IsNullOrEmpty(DictMappingSexValue))
                {
                    Builder.Append("<div id=\"remove_ddlUNSDSex_" + SourceSexGId + "\"  class=\"cus_slct_dd chzn-select\" style=\"width:235px;top:-25px;font-size: 12px;\" >Select</div>");
                }
                else
                {
                    Builder.Append("<div id=\"remove_ddlUNSDSex_" + SourceSexGId + "\"  class=\"cus_slct_dd chzn-select\" style=\"width:235px;top:-25px;font-size: 12px;\" >" + this.Get_TrimmedInnerHTML(DictTargetSex[DictMappingSexValue], 75) + "</div>");
                }

                Builder.Append("</td>");

                Builder.Append("<td style=\"width:19%; align:left; overflow:hidden;\">");

                if (!string.IsNullOrEmpty(DictMappingSexValue))
                {
                    Builder.Append("<span id=\"spanUNSDSexGId_" + SourceSexGId + "\" value=\"" + DictMappingSexValue + "\" onmouseover=\"ShowCallout('divCallout', '" + DictMappingSexValue + "', event);\" onmouseout=\"HideCallout('divCallout');\">" + DictMappingSexValue + "</span>");//onmouseover=\"ShowCallout('divCallout', '" + DictMappingSexValue + "', event);\" onmouseout=\"HideCallout('divCallout');
                }
                else
                {
                    Builder.Append("<span id=\"spanUNSDSexGId_" + SourceSexGId + "\"></span>");
                }

                Builder.Append("</td>");

                Builder.Append("</tr>");
            }

            Builder.Append("</table>");

            Builder.Append("</div>");
        }
        catch (Exception ex)
        {
            Builder.Append("false" + Constants.Delimiters.ParamDelimiter + ex.Message);
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        RetVal = Builder.ToString();
        return RetVal;
    }

    private string GetLocationMappingList(string DbNId, string Language, string SelectedLocationCodeList, StructureType SourceCodelistStructure, SDMXApi_2_0.Message.StructureType TargetCodelistStructure, StructureType MappingCodelistStructure)
    {
        string RetVal;
        string DictSourceLocationValue, DictTargetLocationValue, DictMappingLocationValue, SourceLocationGId, TargetLocationGId;
        Dictionary<string, string> DictSourceLocation, DictTargetLocation, DictMappingLocation;
        StringBuilder Builder;

        RetVal = string.Empty;
        DictSourceLocationValue = string.Empty;
        DictTargetLocationValue = string.Empty;
        DictMappingLocationValue = string.Empty;
        SourceLocationGId = string.Empty;
        TargetLocationGId = string.Empty;

        DictSourceLocation = null;
        DictTargetLocation = null;
        DictMappingLocation = null;

        Builder = new StringBuilder(RetVal);
        string CodeListId = string.Empty;
        try
        {
            if (string.IsNullOrEmpty(SelectedLocationCodeList))
            {
                SelectedLocationCodeList = DevInfo.Lib.DI_LibSDMX.Constants.CodelistPrefix + "LOCATION";
            }
            foreach (SDMXApi_2_0.Structure.DimensionType Dimensions in TargetCodelistStructure.KeyFamilies[0].Components.Dimension)
            {
                if (Dimensions.conceptRef == Constants.UNSD.Concept.Location.Id)
                {
                    CodeListId = Dimensions.codelist;
                }
            }
            DictSourceLocation = this.Get_DictSourceCodelist(Language, SelectedLocationCodeList, SourceCodelistStructure);
            DictTargetLocation = this.Get_DictTargetCodelist(Language, CodeListId, TargetCodelistStructure);//Constants.UNSD.CodeList.Location.Id
            DictMappingLocation = this.Get_DictMappingCodelist(DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.Location.id, MappingCodelistStructure);

            Builder.Append("<div style=\"overflow-x:hidden;\">");

            Builder.Append("<table id=\"tblLocationHeader\" style=\"width:100%; table-layout:fixed;\">");
            Builder.Append("<col width=\"30%\"/>");
            Builder.Append("<col width=\"19%\"/>");
            Builder.Append("<col width=\"30%\"/>");
            Builder.Append("<col width=\"19%\"/>");
            Builder.Append("<col width=\"1%\"/>");

            Builder.Append("<tr style=\"height:30px;\">");

            Builder.Append("<td colspan=\"5\" style=\"width:100%; align:left; overflow:hidden;\">");
            Builder.Append("<a id=\"aShowLocationAll\" href=\"javascript:void(0);\" style=\"color:#000000;\" onclick=\"FilterRowsByStatus('tblLocation', 'aShowLocationAll', 'aShowLocationMapped', 'aShowLocationUnMapped', 'aShowLocationUnSaved', 'all');\"></a>");
            Builder.Append("   |   ");
            Builder.Append("<a id=\"aShowLocationMapped\" href=\"javascript:void(0);\" style=\"color:#1e90ff;\" onclick=\"FilterRowsByStatus('tblLocation', 'aShowLocationAll', 'aShowLocationMapped', 'aShowLocationUnMapped', 'aShowLocationUnSaved', 'mapped');\"></a>&nbsp;");
            Builder.Append("<span style=\"background-color:#bbffbb;width:10px;\">&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            Builder.Append("   |   ");
            Builder.Append("<a id=\"aShowLocationUnMapped\" href=\"javascript:void(0);\" style=\"color:#1e90ff;\" onclick=\"FilterRowsByStatus('tblLocation', 'aShowLocationAll', 'aShowLocationMapped', 'aShowLocationUnMapped', 'aShowLocationUnSaved', 'unmapped');\"></a>&nbsp;");
            Builder.Append("<span style=\"background-color:#ffbbbb;width:10px;\">&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            Builder.Append("   |   ");
            Builder.Append("<a id=\"aShowLocationUnSaved\" href=\"javascript:void(0);\" style=\"color:#1e90ff;\" onclick=\"FilterRowsByStatus('tblLocation', 'aShowLocationAll', 'aShowLocationMapped', 'aShowLocationUnMapped', 'aShowLocationUnSaved', 'unsaved');\"></a>&nbsp;");
            Builder.Append("<span style=\"background-color:#bbbbff;width:10px;\">&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            Builder.Append("</td>");

            Builder.Append("</tr>");

            Builder.Append("<tr style=\"height:30px; background-color:#dddddd;\">");

            Builder.Append("<td style=\"width:30%; align:left; overflow:hidden;\">");
            Builder.Append("<b id=\"lang_DevInfo_Location\"></b>");
            Builder.Append("<div id=\"divLocationSearch\">");
            Builder.Append("<input id=\"txtLocationSearch\" type=\"text\"/>");
            Builder.Append("</div>");
            Builder.Append("</td>");

            Builder.Append("<td style=\"width:19%; align:left; overflow:hidden;\"><b id=\"lang_Location_GIds\"></b></td>");

            Builder.Append("<td style=\"width:30%; align:left; overflow:hidden;\">");
            Builder.Append("<b id=\"lang_UNSD_Location\"></b>");
            //Builder.Append("<div id=\"divLocationFilter\">");
            //Builder.Append("<input id=\"txtLocationFilter\" type=\"text\"/>");
            //Builder.Append("</div>");

            Builder.Append("<select id=\"ddlUNSDLocation\" class=\"confg_frm_inp_bx_txt_dd\" style=\"display:none;\">");
            Builder.Append("<option value=\"-1\">Select</option>");

            foreach (KeyValuePair<string, string> TargetLocation in DictTargetLocation)
            {
                TargetLocationGId = TargetLocation.Key;
                DictTargetLocationValue = TargetLocation.Value;

                Builder.Append("<option value=\"" + TargetLocationGId + "\" title=\"" + DictTargetLocationValue + "\">" + this.Get_TrimmedInnerHTML(DictTargetLocationValue, 75) + "</option>");
            }

            Builder.Append("</select>");
            Builder.Append("</td>");

            Builder.Append("<td style=\"width:19%; align:left; overflow:hidden;\"><b id=\"lang_Location_Ids\"></b></td>");
            Builder.Append("<td style=\"width:1%; align:left; overflow:hidden;\"></td>");

            Builder.Append("</tr>");

            Builder.Append("</table>");

            Builder.Append("</div>");

            Builder.Append("<div style=\"overflow-y:auto; height:200px;\">");

            Builder.Append("<table id=\"tblLocation\" style=\"width:100%; table-layout:fixed;\">");
            Builder.Append("<col width=\"30%\"/>");
            Builder.Append("<col width=\"19%\"/>");
            Builder.Append("<col width=\"30%\"/>");
            Builder.Append("<col width=\"19%\"/>");

            foreach (KeyValuePair<string, string> Location in DictSourceLocation)
            {
                DictSourceLocationValue = string.Empty;
                DictTargetLocationValue = string.Empty;
                DictMappingLocationValue = string.Empty;

                SourceLocationGId = Location.Key;
                DictSourceLocationValue = Location.Value;

                DictMappingLocation.TryGetValue(SourceLocationGId, out DictMappingLocationValue);

                if (!string.IsNullOrEmpty(DictMappingLocationValue))
                {
                    Builder.Append("<tr id=\"rowLocation_" + SourceLocationGId + "\" style=\"background-color:rgb(221, 255, 221)\" status=\"mapped\">");
                }
                else
                {
                    Builder.Append("<tr id=\"rowLocation_" + SourceLocationGId + "\" style=\"background-color:rgb(255, 221, 221)\" status=\"unmapped\">");
                }

                Builder.Append("<td style=\"width:30%; align:left; overflow:hidden;\">");
                Builder.Append("<span id=\"spanDevInfoLocationName_" + SourceLocationGId + "\" value=\"" + DictSourceLocationValue + "\" style=\"font-weight:bold;\" onmouseover=\"ShowCallout('divCallout', '" + DictSourceLocationValue + "', event);\" onmouseout=\"HideCallout('divCallout');\">" + this.Get_TrimmedInnerHTML(DictSourceLocationValue, 75) + "</span>");//onmouseover=\"ShowCallout('divCallout', '" + DictSourceLocationValue + "', event);\" onmouseout=\"HideCallout('divCallout');
                Builder.Append("</td>");

                Builder.Append("<td style=\"width:19%; align:left; overflow:hidden;\">");
                Builder.Append("<span id=\"spanDevInfoLocationGId_" + SourceLocationGId + "\" value=\"" + SourceLocationGId + "\" style=\"font-weight:bold;\"  onmouseover=\"ShowCallout('divCallout', '" + SourceLocationGId + "', event);\" onmouseout=\"HideCallout('divCallout');\">" + SourceLocationGId + "</span>");// onmouseover=\"ShowCallout('divCallout', '" + SourceLocationGId + "', event);\" onmouseout=\"HideCallout('divCallout');
                Builder.Append("</td>");

                Builder.Append("<td style=\"width:30%; align:left;\">");// overflow:hidden;

                if (!string.IsNullOrEmpty(DictMappingLocationValue))
                {//confg_frm_inp_bx_txt_dd chzn-select 
                    Builder.Append("<select id=\"ddlUNSDLocation_" + SourceLocationGId + "\" class=\"cus_slct_dd\" style=\"width:245px;opacity:0.01;z-index:1000;position:relative;\" onchange=\"SelectCodelistCode('tblLocation', 'rowLocation_" + SourceLocationGId + "', 'spanUNSDLocationGId_" + SourceLocationGId + "', this, '" + DictMappingLocationValue + "', 'rgb(221, 255, 221)', 'mapped');\">");
                    Builder.Append("<option value=\"-1\" id=\"SelectUNSDLocation_" + SourceLocationGId + "\"></option>");
                    Builder.Append("<option value=\"" + DictMappingLocationValue + "\" selected=\"selected\" title=\"" + DictTargetLocation[DictMappingLocationValue] + "\">" + this.Get_TrimmedInnerHTML(DictTargetLocation[DictMappingLocationValue], 75) + "</option>");
                }
                else
                {
                    Builder.Append("<select id=\"ddlUNSDLocation_" + SourceLocationGId + "\" class=\"cus_slct_dd\" style=\"width:245px;opacity:0.01;z-index:1000;\" onchange=\"SelectCodelistCode('tblLocation', 'rowLocation_" + SourceLocationGId + "', 'spanUNSDLocationGId_" + SourceLocationGId + "', this, '-1', 'rgb(255, 221, 221)', 'unmapped');\">");
                    Builder.Append("<option value=\"-1\" selected=\"selected\" id=\"SelectUNSDLocation_" + SourceLocationGId + "\"></option>");
                }

                Builder.Append("</select>");
                if (string.IsNullOrEmpty(DictMappingLocationValue))
                {
                    Builder.Append("<div id=\"remove_ddlUNSDLocation_" + SourceLocationGId + "\"  class=\"cus_slct_dd chzn-select\" style=\"width:235px;top:-25px;font-size:12px;\" >Select</div>");
                }
                else
                {
                    Builder.Append("<div id=\"remove_ddlUNSDLocation_" + SourceLocationGId + "\"  class=\"cus_slct_dd chzn-select\" style=\"width:235px;top:-25px;font-size:12px;\" >" + this.Get_TrimmedInnerHTML(DictTargetLocation[DictMappingLocationValue], 75) + "</div>");
                }
                Builder.Append("</td>");

                Builder.Append("<td style=\"width:19%; align:left; overflow:hidden;\">");

                if (!string.IsNullOrEmpty(DictMappingLocationValue))
                {
                    Builder.Append("<span id=\"spanUNSDLocationGId_" + SourceLocationGId + "\" value=\"" + DictMappingLocationValue + "\" onmouseover=\"ShowCallout('divCallout', '" + DictMappingLocationValue + "', event);\" onmouseout=\"HideCallout('divCallout');\">" + DictMappingLocationValue + "</span>");//onmouseover=\"ShowCallout('divCallout', '" + DictMappingLocationValue + "', event);\" onmouseout=\"HideCallout('divCallout');
                }
                else
                {
                    Builder.Append("<span id=\"spanUNSDLocationGId_" + SourceLocationGId + "\"></span>");
                }

                Builder.Append("</td>");

                Builder.Append("</tr>");
            }

            Builder.Append("</table>");

            Builder.Append("</div>");
        }
        catch (Exception ex)
        {
            Builder.Append("false" + Constants.Delimiters.ParamDelimiter + ex.Message);
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        RetVal = Builder.ToString();
        return RetVal;
    }

    private string GetAreaMappingList(string DbNId, string Language, StructureType SourceCodelistStructure, SDMXApi_2_0.Message.StructureType TargetCodelistStructure, StructureType MappingCodelistStructure,string SelectedAreaLevel)
    {
        string RetVal;
        string DictSourceAreaValue, DictTargetAreaValue, DictMappingAreaValue, SourceAreaGId, TargetAreaGId;
        Dictionary<string, string> DictSourceArea, DictTargetArea, DictMappingArea;
        StringBuilder Builder;

        RetVal = string.Empty;
        DictSourceAreaValue = string.Empty;
        DictTargetAreaValue = string.Empty;
        DictMappingAreaValue = string.Empty;
        SourceAreaGId = string.Empty;
        TargetAreaGId = string.Empty;

        DictSourceArea = null;
        DictTargetArea = null;
        DictMappingArea = null;

        Builder = new StringBuilder(RetVal);

        string CodeListId = string.Empty;
        try
        {
            foreach (SDMXApi_2_0.Structure.DimensionType Dimensions in TargetCodelistStructure.KeyFamilies[0].Components.Dimension)
            {
                if (Dimensions.conceptRef == Constants.UNSD.Concept.Area.Id)
                {
                    CodeListId = Dimensions.codelist;
                }
            }
            // Constants.UNSD.CodeList.Area.Id
            DictSourceArea = this.Get_DictSourceCodelist(Language, DevInfo.Lib.DI_LibSDMX.Constants.CodeList.Area.Id, SourceCodelistStructure, SelectedAreaLevel);
            DictTargetArea = this.Get_DictTargetCodelist(Language, CodeListId, TargetCodelistStructure);
            DictMappingArea = this.Get_DictMappingCodelist(DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.Area.id, MappingCodelistStructure);

            Builder.Append("<div style=\"overflow-x:hidden;\">");

            Builder.Append("<table id=\"tblAreaHeader\" style=\"width:100%; table-layout:fixed;\">");
            Builder.Append("<col width=\"30%\"/>");
            Builder.Append("<col width=\"19%\"/>");
            Builder.Append("<col width=\"30%\"/>");
            Builder.Append("<col width=\"19%\"/>");
            Builder.Append("<col width=\"1%\"/>");

            Builder.Append("<tr style=\"height:30px;\">");

            Builder.Append("<td colspan=\"5\" style=\"width:100%; align:left; overflow:hidden;\">");
            Builder.Append("<a id=\"aShowAreaAll\" href=\"javascript:void(0);\" style=\"color:#000000;\" onclick=\"FilterRowsByStatus('tblArea', 'aShowAreaAll', 'aShowAreaMapped', 'aShowAreaUnMapped', 'aShowAreaUnSaved', 'all');\"></a>");
            Builder.Append("   |   ");
            Builder.Append("<a id=\"aShowAreaMapped\" href=\"javascript:void(0);\" style=\"color:#1e90ff;\" onclick=\"FilterRowsByStatus('tblArea', 'aShowAreaAll', 'aShowAreaMapped', 'aShowAreaUnMapped', 'aShowAreaUnSaved', 'mapped');\"></a>&nbsp;");
            Builder.Append("<span style=\"background-color:#bbffbb;width:10px;\">&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            Builder.Append("   |   ");
            Builder.Append("<a id=\"aShowAreaUnMapped\" href=\"javascript:void(0);\" style=\"color:#1e90ff;\" onclick=\"FilterRowsByStatus('tblArea', 'aShowAreaAll', 'aShowAreaMapped', 'aShowAreaUnMapped', 'aShowAreaUnSaved', 'unmapped');\"></a>&nbsp;");
            Builder.Append("<span style=\"background-color:#ffbbbb;width:10px;\">&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            Builder.Append("   |   ");
            Builder.Append("<a id=\"aShowAreaUnSaved\" href=\"javascript:void(0);\" style=\"color:#1e90ff;\" onclick=\"FilterRowsByStatus('tblArea', 'aShowAreaAll', 'aShowAreaMapped', 'aShowAreaUnMapped', 'aShowAreaUnSaved', 'unsaved');\"></a>&nbsp;");
            Builder.Append("<span style=\"background-color:#bbbbff;width:10px;\">&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            Builder.Append("</td>");

            Builder.Append("</tr>");

            Builder.Append("<tr style=\"height:30px; background-color:#dddddd;\">");

            Builder.Append("<td style=\"width:30%; align:left; overflow:hidden;\">");
            Builder.Append("<b id=\"lang_DevInfo_Area\"></b>");
            Builder.Append("<div id=\"divAreaSearch\">");
            Builder.Append("<input id=\"txtAreaSearch\" type=\"text\"/>");
            Builder.Append("</div>");
            Builder.Append("</td>");

            Builder.Append("<td style=\"width:19%; align:left; overflow:hidden; padding-bottom:30px;\"><b id=\"lang_Area_GIds\"></b></td>");

            Builder.Append("<td style=\"width:30%; align:left; overflow:hidden;padding-bottom:30px;\">");
            Builder.Append("<b id=\"lang_UNSD_Area\"></b>");

            Builder.Append("<select id=\"ddlUNSDArea\" class=\"confg_frm_inp_bx_txt_dd\" style=\"display:none;\">");
            Builder.Append("<option value=\"-1\">Select</option>");

            foreach (KeyValuePair<string, string> TargetArea in DictTargetArea)
            {
                TargetAreaGId = TargetArea.Key;
                DictTargetAreaValue = TargetArea.Value;

                Builder.Append("<option value=\"" + TargetAreaGId + "\" title=\"" + DictTargetAreaValue + "\">" + this.Get_TrimmedInnerHTML(DictTargetAreaValue, 75) + "</option>");
            }

            Builder.Append("</select>");
            Builder.Append("</td>");

            Builder.Append("<td style=\"width:19%; align:left; overflow:hidden;padding-bottom:30px;\"><b id=\"lang_Area_Ids\"></b></td>");
            Builder.Append("<td style=\"width:1%; align:left; overflow:hidden;\"></td>");

            Builder.Append("</tr>");

            Builder.Append("</table>");

            Builder.Append("</div>");

            Builder.Append("<div style=\"overflow-y:auto; height:200px;\">");

            Builder.Append("<table id=\"tblArea\" style=\"width:100%; table-layout:fixed;\">");
            Builder.Append("<col width=\"30%\"/>");
            Builder.Append("<col width=\"19%\"/>");
            Builder.Append("<col width=\"30%\"/>");
            Builder.Append("<col width=\"19%\"/>");

            foreach (KeyValuePair<string, string> Area in DictSourceArea)
            {
                DictSourceAreaValue = string.Empty;
                DictTargetAreaValue = string.Empty;
                DictMappingAreaValue = string.Empty;

                SourceAreaGId = Area.Key;
                DictSourceAreaValue = Area.Value;

                DictMappingArea.TryGetValue(SourceAreaGId, out DictMappingAreaValue);

                if (!string.IsNullOrEmpty(DictMappingAreaValue))
                {
                    Builder.Append("<tr id=\"rowArea_" + SourceAreaGId + "\" style=\"background-color:rgb(221, 255, 221)\" status=\"mapped\">");
                }
                else
                {
                    Builder.Append("<tr id=\"rowArea_" + SourceAreaGId + "\" style=\"background-color:rgb(255, 221, 221)\" status=\"unmapped\">");
                }

                Builder.Append("<td style=\"width:30%; align:left; overflow:hidden;\">");
                if (DictSourceArea.Count > 1)
                {
                    if (DictMappingArea.Count > 0)
                    {
                        if (DictMappingAreaValue == Global.registryMSDAreaId.ToString())//DictMappingArea[SourceAreaGId].ToString() == Global.registryMSDAreaId.ToString()
                        {
                            Builder.Append("<span><input checked=\"true\" type=\"radio\" name=\"refArea\" id=\"radioRefArea_" + SourceAreaGId + "\"></input></span>");

                        }
                        else
                        {
                            Builder.Append("<span><input type=\"radio\" name=\"refArea\" id=\"radioRefArea_" + SourceAreaGId + "\"></input></span>");
                        }
                    }
                    else
                    {
                        Builder.Append("<span><input type=\"radio\" name=\"refArea\" id=\"radioRefArea_" + SourceAreaGId + "\"></input></span>");
                    }
                }
                Builder.Append("<span id=\"spanDevInfoAreaName_" + SourceAreaGId + "\" value=\"" + DictSourceAreaValue + "\" style=\"font-weight:bold;\" onmouseover=\"ShowCallout('divCallout', '" + DictSourceAreaValue + "', event);\" onmouseout=\"HideCallout('divCallout');\">" + this.Get_TrimmedInnerHTML(DictSourceAreaValue, 75) + "</span>");// onmouseover=\"ShowCallout('divCallout', '" + DictSourceAreaValue + "', event);\" onmouseout=\"HideCallout('divCallout');
                Builder.Append("</td>");

                Builder.Append("<td style=\"width:19%; align:left; overflow:hidden;\">");
                Builder.Append("<span id=\"spanDevInfoAreaGId_" + SourceAreaGId + "\" value=\"" + SourceAreaGId + "\" onmouseover=\"ShowCallout('divCallout', '" + SourceAreaGId + "', event);\" onmouseout=\"HideCallout('divCallout');\">" + SourceAreaGId + "</span>");// onmouseover=\"ShowCallout('divCallout', '" + SourceAreaGId + "', event);\" onmouseout=\"HideCallout('divCallout');
                Builder.Append("</td>");

                Builder.Append("<td style=\"width:30%; align:left; \">");//overflow:hidden;

                if (!string.IsNullOrEmpty(DictMappingAreaValue))
                {
                    // chzn-select
                    Builder.Append("<select id=\"ddlUNSDArea_" + SourceAreaGId + "\" class=\"cus_slct_dd \" style=\"width:245px; opacity:0.01;z-index:1000 ;position:relative;\" onchange=\"SelectCodelistCode('tblArea', 'rowArea_" + SourceAreaGId + "', 'spanUNSDAreaGId_" + SourceAreaGId + "', this, '" + DictMappingAreaValue + "', 'rgb(221, 255, 221)', 'mapped');\">");
                    Builder.Append("<option value=\"-1\" id=\"SelectUNSDArea_" + SourceAreaGId + "\"></option>");
                    Builder.Append("<option value=\"" + DictMappingAreaValue + "\" selected=\"selected\" title=\"" + DictTargetArea[DictMappingAreaValue] + "\">" + this.Get_TrimmedInnerHTML(DictTargetArea[DictMappingAreaValue], 75) + "</option>");
                }
                else
                {// chzn-select
                    Builder.Append("<select id=\"ddlUNSDArea_" + SourceAreaGId + "\" class=\"cus_slct_dd \" style=\"width:245px; opacity:0.01;z-index:1000;\" onchange=\"SelectCodelistCode('tblArea', 'rowArea_" + SourceAreaGId + "', 'spanUNSDAreaGId_" + SourceAreaGId + "', this, '-1', 'rgb(255, 221, 221)', 'unmapped');\">");
                    Builder.Append("<option value=\"-1\" selected=\"selected\" id=\"SelectUNSDArea_" + SourceAreaGId + "\"></option>");
                }

                Builder.Append("</select>");

                if (string.IsNullOrEmpty(DictMappingAreaValue))
                {
                    Builder.Append("<div id=\"remove_ddlUNSDArea_" + SourceAreaGId + "\"  class=\"cus_slct_dd chzn-select\" style=\"width:235px;top:-25px;font-size: 12px;\" >Select</div>");
                }
                else
                {
                    Builder.Append("<div id=\"remove_ddlUNSDArea_" + SourceAreaGId + "\"  class=\"cus_slct_dd chzn-select\" style=\"width:235px;top:-25px;font-size: 12px;\" >" + this.Get_TrimmedInnerHTML(DictTargetArea[DictMappingAreaValue], 75) + "</div>");
                }

                Builder.Append("</td>");

                Builder.Append("<td style=\"width:169px; align:left; overflow:hidden;\">");

                if (!string.IsNullOrEmpty(DictMappingAreaValue))
                {
                    Builder.Append("<span id=\"spanUNSDAreaGId_" + SourceAreaGId + "\" value=\"" + DictMappingAreaValue + "\" onmouseover=\"ShowCallout('divCallout', '" + DictMappingAreaValue + "', event);\" onmouseout=\"HideCallout('divCallout');\">" + DictMappingAreaValue + "</span>");//onmouseover=\"ShowCallout('divCallout', '" + DictMappingAreaValue + "', event);\" onmouseout=\"HideCallout('divCallout');
                }
                else
                {
                    Builder.Append("<span id=\"spanUNSDAreaGId_" + SourceAreaGId + "\"></span>");
                }

                Builder.Append("</td>");

                Builder.Append("</tr>");
            }

            Builder.Append("</table>");

            Builder.Append("</div>");
        }
        catch (Exception ex)
        {
            Builder.Append("false" + Constants.Delimiters.ParamDelimiter + ex.Message);
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        RetVal = Builder.ToString();
        return RetVal;
    }


    private bool GenerateCodelistMappingXmlArtefact(string DbNId, string AgencyId, string Language, string CodelistMappingData, StructureType SourceCodelistStructure, SDMXApi_2_0.Message.StructureType TargetCodelistStructure, string SelectedAgeCodelist, string SelectedSexCodelist, string SelectedLocationCodelist)
    {
        bool RetVal;
        string FileNameWPath, OutputFolder;
        string SourceCodelistId, TargetCodelistId;
        string SourceId, SourceAgencyId, SourceVersion, TargetId, TargetAgencyId, TargetVersion, CodelistName;
        string AreaCodelistId, IndicatorCodelistId, UnitCodelistId, AgeCodelist, SexCodelist, LocationCodelist;
        Dictionary<string, Dictionary<string, string>> DictMappingPerCodelist;
        Dictionary<string, string> DictMapping;

        RetVal = false;
        FileNameWPath = string.Empty;
        OutputFolder = string.Empty;
        SourceCodelistId = string.Empty;
        TargetCodelistId = string.Empty;
        CodelistName = string.Empty;
        DictMappingPerCodelist = null;
        DictMapping = null;
        IndicatorCodelistId = string.Empty;
        UnitCodelistId = string.Empty;
        AreaCodelistId = string.Empty;
        AgeCodelist = string.Empty;
        SexCodelist = string.Empty;
        LocationCodelist = string.Empty;
        try
        {
            FileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId + "\\" + Constants.FolderName.SDMX.Mapping + DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.FileName);
            OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId + "\\" + Constants.FolderName.SDMX.Mapping);

            if (File.Exists(FileNameWPath))
            {
                File.Delete(FileNameWPath);
            }

            foreach (SDMXApi_2_0.Structure.DimensionType Dimensions in TargetCodelistStructure.KeyFamilies[0].Components.Dimension)
            {
                if (Dimensions.conceptRef == Constants.UNSD.Concept.Indicator.Id)
                {
                    IndicatorCodelistId = Dimensions.codelist;
                }
                if (Dimensions.conceptRef == Constants.UNSD.Concept.Unit.Id)
                {
                    UnitCodelistId = Dimensions.codelist;
                }
                if (Dimensions.conceptRef == Constants.UNSD.Concept.Area.Id)
                {
                    AreaCodelistId = Dimensions.codelist;
                }
                if (Dimensions.conceptRef == Constants.UNSD.Concept.Age.Id)
                {
                    AgeCodelist = Dimensions.codelist;
                }
                if (Dimensions.conceptRef == Constants.UNSD.Concept.Sex.Id)
                {
                    SexCodelist = Dimensions.codelist;
                }
                if (Dimensions.conceptRef == Constants.UNSD.Concept.Location.Id)
                {
                    LocationCodelist = Dimensions.codelist;
                }

            }
            if (!string.IsNullOrEmpty(CodelistMappingData))
            {
                DictMappingPerCodelist = new Dictionary<string, Dictionary<string, string>>();

                foreach (string RowMappingData in CodelistMappingData.Split(new string[] { Constants.Delimiters.RowDelimiter }, StringSplitOptions.None))
                {
                    if (DictMappingPerCodelist.ContainsKey(RowMappingData.Split(new string[] { Constants.Delimiters.ColumnDelimiter }, StringSplitOptions.None)[0]))
                    {
                        DictMappingPerCodelist[RowMappingData.Split(new string[] { Constants.Delimiters.ColumnDelimiter }, StringSplitOptions.None)[0]].Add(RowMappingData.Split(new string[] { Constants.Delimiters.ColumnDelimiter }, StringSplitOptions.None)[1], RowMappingData.Split(new string[] { Constants.Delimiters.ColumnDelimiter }, StringSplitOptions.None)[2]);
                    }
                    else
                    {
                        DictMapping = new Dictionary<string, string>();
                        DictMapping.Add(RowMappingData.Split(new string[] { Constants.Delimiters.ColumnDelimiter }, StringSplitOptions.None)[1], RowMappingData.Split(new string[] { Constants.Delimiters.ColumnDelimiter }, StringSplitOptions.None)[2]);

                        DictMappingPerCodelist.Add(RowMappingData.Split(new string[] { Constants.Delimiters.ColumnDelimiter }, StringSplitOptions.None)[0], DictMapping);
                    }
                }

                foreach (string Key in DictMappingPerCodelist.Keys)
                {
                    DictMapping = DictMappingPerCodelist[Key];

                    switch (Key)
                    {
                        case "Indicator":
                            SourceCodelistId = DevInfo.Lib.DI_LibSDMX.Constants.CodeList.Indicator.Id;
                            //TargetCodelistId = Constants.UNSD.CodeList.Indicator.Id;
                            TargetCodelistId = IndicatorCodelistId;
                            CodelistName = DevInfo.Lib.DI_LibSDMX.Constants.CodeList.Indicator.Id;
                            break;
                        case "Unit":
                            SourceCodelistId = DevInfo.Lib.DI_LibSDMX.Constants.CodeList.Unit.Id;
                            // TargetCodelistId = Constants.UNSD.CodeList.Unit.Id;
                            TargetCodelistId = UnitCodelistId;
                            CodelistName = DevInfo.Lib.DI_LibSDMX.Constants.CodeList.Unit.Id;
                            break;
                        case "Age":
                            SourceCodelistId = SelectedAgeCodelist;//DevInfo.Lib.DI_LibSDMX.Constants.CodelistPrefix + AGE
                            // TargetCodelistId = Constants.UNSD.CodeList.Age.Id;
                            TargetCodelistId = AgeCodelist;
                            CodelistName = DevInfo.Lib.DI_LibSDMX.Constants.CodelistPrefix + "AGE";
                            break;
                        case "Sex":
                            SourceCodelistId = SelectedSexCodelist;//SEX
                            // TargetCodelistId = Constants.UNSD.CodeList.Sex.Id;
                            TargetCodelistId = SexCodelist;
                            CodelistName = DevInfo.Lib.DI_LibSDMX.Constants.CodelistPrefix + "SEX";
                            break;
                        case "Location":
                            SourceCodelistId = SelectedLocationCodelist;//LOCATION
                            //TargetCodelistId = Constants.UNSD.CodeList.Location.Id;
                            TargetCodelistId = LocationCodelist;
                            CodelistName = DevInfo.Lib.DI_LibSDMX.Constants.CodelistPrefix + "LOCATION";
                            break;
                        case "Area":
                            SourceCodelistId = DevInfo.Lib.DI_LibSDMX.Constants.CodelistPrefix + "AREA";
                            // TargetCodelistId = Constants.UNSD.CodeList.Area.Id;
                            TargetCodelistId = AreaCodelistId;
                            CodelistName = DevInfo.Lib.DI_LibSDMX.Constants.CodelistPrefix + "AREA";
                            break;
                        default:
                            break;
                    }

                    this.Get_SourceIdAgencyIdVersionCodelist(SourceCodelistId, out SourceId, out SourceAgencyId, out SourceVersion, SourceCodelistStructure);
                    this.Get_TargetIdAgencyIdVersionCodelist(TargetCodelistId, out TargetId, out TargetAgencyId, out TargetVersion, TargetCodelistStructure);

                    if (File.Exists(FileNameWPath))
                    {
                        SDMXUtility.Generate_Mapping(SDMXSchemaType.Two_One, DevInfo.Lib.DI_LibSDMX.MappingType.Codelist, DictMapping, SourceId, SourceAgencyId, SourceVersion, TargetId, TargetAgencyId, TargetVersion, AgencyId, Language, null, FileNameWPath, string.Empty, CodelistName);
                    }
                    else
                    {
                        SDMXUtility.Generate_Mapping(SDMXSchemaType.Two_One, DevInfo.Lib.DI_LibSDMX.MappingType.Codelist, DictMapping, SourceId, SourceAgencyId, SourceVersion, TargetId, TargetAgencyId, TargetVersion, AgencyId, Language, null, string.Empty, OutputFolder, CodelistName);
                    }
                }
            }

            RetVal = true;
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
        }

        return RetVal;
    }

    private Dictionary<string, string> Get_DictSourceCodelist(string Language, string CodelistId, StructureType Structure,string SelectedAreaLevel=null)
    {
        Dictionary<string, string> RetVal;
        string CodeId, CodeName;
        SDMXObjectModel.Structure.CodelistType SourceCodelist;

        RetVal = new Dictionary<string, string>();
        CodeId = string.Empty;
        CodeName = string.Empty;
        SourceCodelist = null;

        try
        {
            
            if (Structure != null && Structure.Structures != null && Structure.Structures.Codelists != null && Structure.Structures.Codelists.Count > 0)
            {
                foreach (SDMXObjectModel.Structure.CodelistType Codelist in Structure.Structures.Codelists)
                {
                    if (Codelist.id == CodelistId)
                    {
                        SourceCodelist = Codelist;
                        break;
                    }
                }



                if (SourceCodelist != null && SourceCodelist.Items != null && SourceCodelist.Items.Count > 0)
                {
                    foreach (SDMXObjectModel.Structure.ItemType Code in SourceCodelist.Items)
                    {
                        CodeId = Code.id;
                        CodeName = this.GetLangSpecificValue_For_Version_2_1(Code.Name, Language);
                        // Add Country Level Areas in the list
                        if (CodelistId == DevInfo.Lib.DI_LibSDMX.Constants.CodeList.Area.Id)
                        {
                            if (SelectedAreaLevel == "null")
                            {
                                SelectedAreaLevel = Global.registryAreaLevel;
                            }
                            
                            if (Code.Annotations != null && Code.Annotations.Count > 0)
                            {
                                foreach (AnnotationType Annotation in Code.Annotations)
                                {
                                    if (Annotation.AnnotationTitle == DevInfo.Lib.DI_LibSDMX.Constants.Annotations.Area_Level && Annotation.AnnotationText[0].Value.ToString() == SelectedAreaLevel)//Global.registryAreaLevel.ToString()
                                    {
                                        RetVal.Add(CodeId, CodeName);
                                    }
                                }
                            }
                        }
                        else
                        {
                            RetVal.Add(CodeId, CodeName);
                        }

                    }
                }

                if (RetVal != null && RetVal.Keys.Count > 0)
                {
                    RetVal = this.Sort_Dictionary(RetVal);
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
        }

        return RetVal;
    }



    private void Get_SourceIdAgencyIdVersionCodelist(string CodelistId, out string SourceId, out string SourceAgencyId, out string SourceVersion, StructureType Structure)
    {
        SDMXObjectModel.Structure.CodelistType SourceCodelist;

        SourceCodelist = null;

        SourceId = string.Empty;
        SourceAgencyId = string.Empty;
        SourceVersion = string.Empty;

        try
        {
            if (Structure != null && Structure.Structures != null && Structure.Structures.Codelists != null &&
                Structure.Structures.Codelists.Count > 0)
            {
                foreach (SDMXObjectModel.Structure.CodelistType Codelist in Structure.Structures.Codelists)
                {

                    if (Codelist.id.ToUpper() == CodelistId.ToUpper())
                    {
                        SourceCodelist = Codelist;
                        break;
                    }

                }

                SourceId = SourceCodelist.id;
                SourceAgencyId = SourceCodelist.agencyID;
                SourceVersion = SourceCodelist.version;
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
        }
    }

    private void Get_TargetIdAgencyIdVersionCodelist(string CodelistId, out string TargetId, out string TargetAgencyId, out string TargetVersion, SDMXApi_2_0.Message.StructureType Structure)
    {
        SDMXApi_2_0.Structure.CodeListType TargetCodelist;

        TargetCodelist = null;

        TargetId = string.Empty;
        TargetAgencyId = string.Empty;
        TargetVersion = string.Empty;

        try
        {
            if (Structure != null && Structure.CodeLists != null && Structure.CodeLists.Count > 0)
            {
                foreach (SDMXApi_2_0.Structure.CodeListType Codelist in Structure.CodeLists)
                {
                    if (Codelist.id == CodelistId)
                    {
                        TargetCodelist = Codelist;
                        break;
                    }

                }

                TargetId = TargetCodelist.id;
                TargetAgencyId = TargetCodelist.agencyID;
                TargetVersion = TargetCodelist.version;
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
        }
    }



    private void Get_Codelist_Source_Target_Structure(string DbNId, out StructureType SourceCodelistStructure, out SDMXApi_2_0.Message.StructureType TargetCodelistStructure)
    {
        string AssociatedDbNId, SourceCodelistFileNameWPath, TargetCodelistFileNameWPath;

        AssociatedDbNId = string.Empty;
        SourceCodelistFileNameWPath = string.Empty;
        TargetCodelistFileNameWPath = string.Empty;

        SourceCodelistStructure = null;
        TargetCodelistStructure = null;

        try
        {
            AssociatedDbNId = Get_AssociatedDB_NId(DbNId).ToString();
            if (Convert.ToInt32(AssociatedDbNId) < 1)
            {
                SourceCodelistFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId + "\\" + Constants.FolderName.SDMX.sdmx + DevInfo.Lib.DI_LibSDMX.Constants.Complete_XML.FileName);
            }
            else
            {
                SourceCodelistFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + AssociatedDbNId + "\\" + Constants.FolderName.SDMX.sdmx + DevInfo.Lib.DI_LibSDMX.Constants.Complete_XML.FileName);
                TargetCodelistFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId + "\\" + Constants.FolderName.SDMX.sdmx + DevInfo.Lib.DI_LibSDMX.Constants.Complete_XML.FileName);
            }
            //  SourceCodelistFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + AssociatedDbNId + "\\" + Constants.FolderName.SDMX.sdmx + DevInfo.Lib.DI_LibSDMX.Constants.Complete_XML.FileName);


            if (File.Exists(SourceCodelistFileNameWPath))
            {
                SourceCodelistStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), SourceCodelistFileNameWPath);
            }

            if (File.Exists(TargetCodelistFileNameWPath))
            {
                TargetCodelistStructure = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromFile(typeof(SDMXApi_2_0.Message.StructureType), TargetCodelistFileNameWPath);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
        }
    }

    private string GetAllMappedCodeLists(string DbNId, string Language, StructureType SourceCodelistStructure, SDMXApi_2_0.Message.StructureType TargetCodelistStructure, StructureType MappingCodelistStructure, string SelectedAgeCodelist, string SelectedSexCodelist, string SelectedLocationCodelist, string SelectedAreaLevel)
    {
        string RetVal;
        RetVal = string.Empty;

        RetVal = this.GetIndicatorMappingList(DbNId, Language, SourceCodelistStructure, TargetCodelistStructure, MappingCodelistStructure);
        RetVal += Constants.Delimiters.ParamDelimiter;
        RetVal += this.GetUnitMappingList(DbNId, Language, SourceCodelistStructure, TargetCodelistStructure, MappingCodelistStructure);
        RetVal += Constants.Delimiters.ParamDelimiter;
        RetVal += this.GetAgeMappingList(DbNId, Language, SelectedAgeCodelist, SourceCodelistStructure, TargetCodelistStructure, MappingCodelistStructure);
        RetVal += Constants.Delimiters.ParamDelimiter;
        RetVal += this.GetSexMappingList(DbNId, Language, SelectedSexCodelist, SourceCodelistStructure, TargetCodelistStructure, MappingCodelistStructure);
        RetVal += Constants.Delimiters.ParamDelimiter;
        RetVal += this.GetLocationMappingList(DbNId, Language, SelectedLocationCodelist, SourceCodelistStructure, TargetCodelistStructure, MappingCodelistStructure);
        RetVal += Constants.Delimiters.ParamDelimiter;
        RetVal += this.GetAreaMappingList(DbNId, Language, SourceCodelistStructure, TargetCodelistStructure, MappingCodelistStructure, SelectedAreaLevel);
        return RetVal;
    }

    #endregion "--Codelists Mapping--"

    #region "--Metadata Mapping--"

    private string GetMetadataMappingList(string DbNId, string Language)
    {
        string RetVal;
        string AssociatedDbNId;
        string DictSourceMetadataValue, DictTargetMetadataValue, DictMappingMetadataValue, SourceMetadataGId, TargetMetadataGId;
        Dictionary<string, string> DictSourceMetadata, DictTargetMetadata, DictMappingMetadata;
        StringBuilder Builder;

        RetVal = string.Empty;
        AssociatedDbNId = string.Empty;
        DictSourceMetadataValue = string.Empty;
        DictTargetMetadataValue = string.Empty;
        DictMappingMetadataValue = string.Empty;
        SourceMetadataGId = string.Empty;
        TargetMetadataGId = string.Empty;

        DictSourceMetadata = null;
        DictTargetMetadata = null;
        DictMappingMetadata = null;

        Builder = new StringBuilder(RetVal);

        try
        {
            AssociatedDbNId = Get_AssociatedDB_NId(DbNId).ToString();

            DictSourceMetadata = this.Get_DictSourceMetadata(AssociatedDbNId, Language);
            DictTargetMetadata = this.Get_DictTargetMetadata(DbNId, Language);
            DictMappingMetadata = this.Get_DictMappingMetadata(DbNId);

            Builder.Append("<div style=\"overflow-x:hidden;\">");

            Builder.Append("<table id=\"tblMetadataHeader\" style=\"width:100%; table-layout:fixed;\">");
            Builder.Append("<col width=\"30%\"/>");
            Builder.Append("<col width=\"19%\"/>");
            Builder.Append("<col width=\"30%\"/>");
            Builder.Append("<col width=\"19%\"/>");
            Builder.Append("<col width=\"1%\"/>");

            Builder.Append("<tr style=\"height:30px;\">");

            Builder.Append("<td colspan=\"5\" style=\"width:100%; align:left; overflow:hidden;\">");
            Builder.Append("<a id=\"aShowMetadataAll\" href=\"javascript:void(0);\" style=\"color:#000000;\" onclick=\"FilterRowsByStatus('tblMetadata', 'aShowMetadataAll', 'aShowMetadataMapped', 'aShowMetadataUnMapped', 'aShowMetadataUnSaved', 'all');\"></a>");
            Builder.Append("   |   ");
            Builder.Append("<a id=\"aShowMetadataMapped\" href=\"javascript:void(0);\" style=\"color:#1e90ff;\"  onclick=\"FilterRowsByStatus('tblMetadata', 'aShowMetadataAll', 'aShowMetadataMapped', 'aShowMetadataUnMapped', 'aShowMetadataUnSaved', 'mapped');\"></a>&nbsp;");
            Builder.Append("<span style=\"background-color:#bbffbb;width:10px;\">&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            Builder.Append("   |   ");
            Builder.Append("<a id=\"aShowMetadataUnMapped\" href=\"javascript:void(0);\" style=\"color:#1e90ff;\"  onclick=\"FilterRowsByStatus('tblMetadata', 'aShowMetadataAll', 'aShowMetadataMapped', 'aShowMetadataUnMapped', 'aShowMetadataUnSaved', 'unmapped');\"></a>&nbsp;");
            Builder.Append("<span style=\"background-color:#ffbbbb;width:10px;\">&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            Builder.Append("   |   ");
            Builder.Append("<a id=\"aShowMetadataUnSaved\" href=\"javascript:void(0);\" style=\"color:#1e90ff;\"  onclick=\"FilterRowsByStatus('tblMetadata', 'aShowMetadataAll', 'aShowMetadataMapped', 'aShowMetadataUnMapped', 'aShowMetadataUnSaved', 'unsaved');\"></a>&nbsp;");
            Builder.Append("<span style=\"background-color:#bbbbff;width:10px;\">&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            Builder.Append("</td>");

            Builder.Append("</tr>");

            Builder.Append("<tr style=\"height:30px; background-color:#dddddd;\">");

            Builder.Append("<td style=\"width:30%; align:left; overflow:hidden;\">");
            Builder.Append("<b id=\"lang_DevInfo_Metadata\"></b>");
            Builder.Append("<div id=\"divMetadataSearch\">");
            Builder.Append("<input id=\"txtMetadataSearch\" type=\"text\"/>");
            Builder.Append("</div>");
            Builder.Append("</td>");

            Builder.Append("<td style=\"width:19%; align:left; overflow:hidden;\"><b id=\"lang_Category_GIds\"></b></td>");

            Builder.Append("<td style=\"width:30%; align:left; overflow:hidden;\">");
            Builder.Append("<b id=\"lang_UNSD_Metadata\"></b>");
            //Builder.Append("<div id=\"divMetadataFilter\">");
            //Builder.Append("<input id=\"txtMetadataFilter\" type=\"text\"/>");
            //Builder.Append("</div>");

            Builder.Append("<select id=\"ddlUNSDMetadata\" class=\"confg_frm_inp_bx_txt_dd\" style=\"display:none;\">");
            Builder.Append("<option value=\"-1\">Select</option>");

            foreach (KeyValuePair<string, string> TargetCategory in DictTargetMetadata)
            {
                TargetMetadataGId = TargetCategory.Key;
                DictTargetMetadataValue = TargetCategory.Value;

                Builder.Append("<option value=\"" + TargetMetadataGId + "\" title=\"" + DictTargetMetadataValue + "\">" + this.Get_TrimmedInnerHTML(DictTargetMetadataValue, 75) + "</option>");
            }

            Builder.Append("</select>");
            Builder.Append("</td>");

            Builder.Append("<td style=\"width:19%; align:left; overflow:hidden;\"><b id=\"lang_Concept_Ids\"></b></td>");
            Builder.Append("<td style=\"width:1%; align:left; overflow:hidden;\"></td>");

            Builder.Append("</tr>");

            Builder.Append("</table>");

            Builder.Append("</div>");

            Builder.Append("<div style=\"overflow:auto; height:200px;\">");

            Builder.Append("<table id=\"tblMetadata\" style=\"width:100%; table-layout:fixed;\">");
            Builder.Append("<col width=\"30%\"/>");
            Builder.Append("<col width=\"19%\"/>");
            Builder.Append("<col width=\"30%\"/>");
            Builder.Append("<col width=\"19%\"/>");

            foreach (KeyValuePair<string, string> Category in DictSourceMetadata)
            {
                DictSourceMetadataValue = string.Empty;
                DictTargetMetadataValue = string.Empty;
                DictMappingMetadataValue = string.Empty;

                SourceMetadataGId = Category.Key;
                if (SourceMetadataGId.Contains("'"))
                {
                    SourceMetadataGId = SourceMetadataGId.Replace("'", "&prime;");
                }
                DictSourceMetadataValue = Category.Value;
                DictMappingMetadata.TryGetValue(SourceMetadataGId, out DictMappingMetadataValue);

                if (!string.IsNullOrEmpty(DictMappingMetadataValue))
                {
                    Builder.Append("<tr id=\"rowMetadata_" + SourceMetadataGId + "\" style=\"background-color:rgb(221, 255, 221)\" status=\"mapped\">");
                }
                else
                {
                    Builder.Append("<tr id=\"rowMetadata_" + SourceMetadataGId + "\" style=\"background-color:rgb(255, 221, 221)\" status=\"unmapped\">");
                }

                Builder.Append("<td style=\"width:30%; align:left; overflow:hidden;\">");
                Builder.Append("<span id=\"spanDevInfoMetadataName_" + SourceMetadataGId + "\" value=\"" + DictSourceMetadataValue + "\" style=\"font-weight:bold;\" onmouseover=\"ShowCallout('divCallout', '" + DictSourceMetadataValue + "', event);\" onmouseout=\"HideCallout('divCallout');\">" + this.Get_TrimmedInnerHTML(DictSourceMetadataValue, 75) + "</span>");//onmouseover=\"ShowCallout('divCallout', '" + DictSourceMetadataValue + "', event);\" onmouseout=\"HideCallout('divCallout');
                Builder.Append("</td>");

                Builder.Append("<td style=\"width:19%; align:left; overflow:hidden;\">");
                Builder.Append("<span id=\"spanDevInfoMetadataGId_" + SourceMetadataGId + "\" value=\"" + SourceMetadataGId + "\" onmouseover=\"ShowCallout('divCallout', '" + SourceMetadataGId + "', event);\" onmouseout=\"HideCallout('divCallout');\">" + SourceMetadataGId + "</span>");//onmouseover=\"ShowCallout('divCallout', '" + SourceMetadataGId + "', event);\" onmouseout=\"HideCallout('divCallout');
                Builder.Append("</td>");

                Builder.Append("<td style=\"width:30%; align:left;\">");// overflow:hidden;

                if (!string.IsNullOrEmpty(DictMappingMetadataValue))
                {

                    Builder.Append("<select id=\"ddlUNSDMetadata_" + SourceMetadataGId + "\" class=\"confg_frm_inp_bx_txt_dd chzn-select\" style=\"width:200px;\" onchange=\"SelectMetadataCategory('rowMetadata_" + SourceMetadataGId + "', 'spanUNSDMetadataGId_" + SourceMetadataGId + "', this, '" + DictMappingMetadataValue + "', 'rgb(221, 255, 221)', 'mapped');\">");
                    Builder.Append("<option value=\"-1\" id=\"SelectUNSDMetadata_" + SourceMetadataGId + "\"></option>");
                    Builder.Append("<option value=\"" + DictMappingMetadataValue + "\" selected=\"selected\" title=\"" + DictTargetMetadata[DictMappingMetadataValue] + "\">" + this.Get_TrimmedInnerHTML(DictTargetMetadata[DictMappingMetadataValue], 75) + "</option>");
                }
                else
                {

                    Builder.Append("<select id=\"ddlUNSDMetadata_" + SourceMetadataGId + "\" class=\"confg_frm_inp_bx_txt_dd chzn-select\" style=\"width:200px;\" onchange=\"SelectMetadataCategory('rowMetadata_" + SourceMetadataGId + "', 'spanUNSDMetadataGId_" + SourceMetadataGId + "', this, '-1', 'rgb(255, 221, 221)', 'unmapped');\">");
                    Builder.Append("<option value=\"-1\" selected=\"selected\" id=\"SelectUNSDMetadata_" + SourceMetadataGId + "\"></option>");
                }
                //if (!string.IsNullOrEmpty(DictMappingMetadataValue))
                //{

                //    Builder.Append("<select id=\"ddlUNSDMetadata_" + SourceMetadataGId + "\" class=\"confg_frm_inp_bx_txt_dd chzn-select\" style=\"width:200px;\" onchange=\"SelectMetadataCategory(\"rowMetadata_" + SourceMetadataGId + "\", \"spanUNSDMetadataGId_" + SourceMetadataGId + "\", this, '" + DictMappingMetadataValue + "', 'rgb(221, 255, 221)', 'mapped');\">");
                //    Builder.Append("<option value=\"-1\" id=\"SelectUNSDMetadata_" + SourceMetadataGId + "\"></option>");
                //    Builder.Append("<option value=\"" + DictMappingMetadataValue + "\" selected=\"selected\" title=\"" + DictTargetMetadata[DictMappingMetadataValue] + "\">" + this.Get_TrimmedInnerHTML(DictTargetMetadata[DictMappingMetadataValue], 75) + "</option>");
                //}
                //else
                //{

                //    Builder.Append("<select id=\"ddlUNSDMetadata_" + SourceMetadataGId + "\" class=\"confg_frm_inp_bx_txt_dd chzn-select\" style=\"width:200px;\" onchange=\"SelectMetadataCategory(\"rowMetadata_" + SourceMetadataGId + "\", \"spanUNSDMetadataGId_" + SourceMetadataGId + "\", this, '-1', 'rgb(255, 221, 221)', 'unmapped');\">");
                //    Builder.Append("<option value=\"-1\" selected=\"selected\" id=\"SelectUNSDMetadata_" + SourceMetadataGId + "\"></option>");
                //}

                Builder.Append("</select>");
                Builder.Append("</td>");


                Builder.Append("<td style=\"width:19%; align:left; overflow:hidden;\">");

                if (!string.IsNullOrEmpty(DictMappingMetadataValue))
                {
                    Builder.Append("<span id=\"spanUNSDMetadataGId_" + SourceMetadataGId + "\" value=\"" + DictMappingMetadataValue + "\" onmouseover=\"ShowCallout('divCallout', '" + DictMappingMetadataValue + "', event);\" onmouseout=\"HideCallout('divCallout');\">" + DictMappingMetadataValue + "</span>");//onmouseover=\"ShowCallout('divCallout', '" + DictMappingMetadataValue + "', event);\" onmouseout=\"HideCallout('divCallout');
                }
                else
                {
                    Builder.Append("<span id=\"spanUNSDMetadataGId_" + SourceMetadataGId + "\"></span>");
                }

                Builder.Append("</td>");

                Builder.Append("</tr>");
            }

            Builder.Append("</table>");

            Builder.Append("</div>");
        }
        catch (Exception ex)
        {
            Builder.Append("false" + Constants.Delimiters.ParamDelimiter + ex.Message);
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        RetVal = Builder.ToString();

        return HttpUtility.UrlEncode(RetVal);
    }

    private bool GenerateMetadataMappingXmlArtefact(string DbNId, string AssociatedDbNId, string AgencyId, string Language, string MetadataMappingData)
    {
        bool RetVal;
        string OutputFolder, SourceId, SourceAgencyId, SourceVersion, TargetId, TargetAgencyId, TargetVersion;
        Dictionary<string, string> DictMapping;

        RetVal = false;
        OutputFolder = string.Empty;
        DictMapping = null;

        try
        {
            OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId + "\\" + Constants.FolderName.SDMX.Mapping);

            if (!string.IsNullOrEmpty(MetadataMappingData))
            {
                DictMapping = new Dictionary<string, string>();

                foreach (string RowMappingData in MetadataMappingData.Split(new string[] { Constants.Delimiters.RowDelimiter }, StringSplitOptions.None))
                {
                    DictMapping.Add(RowMappingData.Split(new string[] { Constants.Delimiters.ColumnDelimiter }, StringSplitOptions.None)[0], RowMappingData.Split(new string[] { Constants.Delimiters.ColumnDelimiter }, StringSplitOptions.None)[1]);
                }
            }

            this.Get_SourceIdAgencyIdVersionMetadata(out SourceId, out SourceAgencyId, out SourceVersion, AssociatedDbNId);
            this.Get_TargetIdAgencyIdVersionMetadata(out TargetId, out TargetAgencyId, out TargetVersion, DbNId);

            SDMXUtility.Generate_Mapping(SDMXSchemaType.Two_One, DevInfo.Lib.DI_LibSDMX.MappingType.Metadata, DictMapping, SourceId, SourceAgencyId, SourceVersion, TargetId, TargetAgencyId, TargetVersion, AgencyId, Language, null, string.Empty, OutputFolder, SourceId);

            RetVal = true;
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
        }

        return RetVal;
    }

    public Dictionary<string, string> Get_DictSourceMetadata(string AssociatedDbNId, string Language)
    {
        Dictionary<string, string> RetVal;
        string ConceptSchemeFileNameWPath, ConceptId, ConceptName;
        SDMXObjectModel.Message.StructureType ConceptScheme;

        RetVal = new Dictionary<string, string>();
        ConceptSchemeFileNameWPath = string.Empty;
        ConceptId = string.Empty;
        ConceptName = string.Empty;
        ConceptScheme = null;

        try
        {
            ConceptSchemeFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + AssociatedDbNId + "\\" + Constants.FolderName.SDMX.Concepts + DevInfo.Lib.DI_LibSDMX.Constants.ConceptScheme.MSD_Indicator.FileName);
            ConceptScheme = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), ConceptSchemeFileNameWPath);

            if (ConceptScheme != null && ConceptScheme.Structures != null && ConceptScheme.Structures.Concepts != null &&
                ConceptScheme.Structures.Concepts.Count > 0 && ConceptScheme.Structures.Concepts[0].Items != null &&
                ConceptScheme.Structures.Concepts[0].Items.Count > 0)
            {
                foreach (SDMXObjectModel.Structure.ItemType Concept in ConceptScheme.Structures.Concepts[0].Items)
                {
                    ConceptId = Concept.id;
                    ConceptName = this.GetLangSpecificValue_For_Version_2_1(Concept.Name, Language);
                    if (RetVal.ContainsKey(ConceptId) | RetVal.ContainsKey(ConceptName))
                    {
                        continue;
                    }
                    else
                    {
                        RetVal.Add(ConceptId, ConceptName);
                    }
                }

                if (RetVal != null && RetVal.Keys.Count > 0)
                {

                    RetVal = this.Sort_Dictionary(RetVal);
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
        }

        return RetVal;
    }

    public Dictionary<string, string> Get_DictTargetMetadata(string DbNId, string Language)
    {
        Dictionary<string, string> RetVal;
        string ConceptSchemeFileNameWPath, ConceptId, ConceptName;
        SDMXApi_2_0.Message.StructureType ConceptScheme;

        RetVal = new Dictionary<string, string>();
        ConceptSchemeFileNameWPath = string.Empty;
        ConceptId = string.Empty;
        ConceptName = string.Empty;
        ConceptScheme = null;

        try
        {
            ConceptSchemeFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId + "\\" + Constants.FolderName.SDMX.Concepts + Constants.UNSD.Concept.MSDConcepts.FileName);
            ConceptScheme = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromFile(typeof(SDMXApi_2_0.Message.StructureType), ConceptSchemeFileNameWPath);

            if (ConceptScheme != null && ConceptScheme.Concepts != null && ConceptScheme.Concepts.ConceptScheme != null &&
                ConceptScheme.Concepts.ConceptScheme.Count > 0 && ConceptScheme.Concepts.ConceptScheme[0].Concept != null &&
                ConceptScheme.Concepts.ConceptScheme[0].Concept.Count > 0)
            {
                foreach (SDMXApi_2_0.Structure.ConceptType Concept in ConceptScheme.Concepts.ConceptScheme[0].Concept)
                {
                    ConceptId = Concept.id;
                    ConceptName = this.GetLangSpecificValueFor_Version_2_0(Concept.Name, Language);

                    RetVal.Add(ConceptId, ConceptName);
                }

                if (RetVal != null && RetVal.Keys.Count > 0)
                {
                    RetVal = this.Sort_Dictionary(RetVal);
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
        }

        return RetVal;
    }

    public Dictionary<string, string> Get_DictMappingMetadata(string DbNId)
    {
        Dictionary<string, string> RetVal;
        string MetadataMappingFileNameWPath;
        SDMXObjectModel.Message.StructureType Structure;

        RetVal = new Dictionary<string, string>();
        MetadataMappingFileNameWPath = string.Empty;

        try
        {
            MetadataMappingFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId + "\\" + Constants.FolderName.SDMX.Mapping + DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.ConceptSchemeMap.MetadataMap.FileName);

            if (File.Exists(MetadataMappingFileNameWPath))
            {
                Structure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), MetadataMappingFileNameWPath);

                if (Structure != null && Structure.Structures != null && Structure.Structures.StructureSets != null &&
                    Structure.Structures.StructureSets.Count > 0 && Structure.Structures.StructureSets[0].Items != null &&
                    Structure.Structures.StructureSets[0].Items.Count > 0 &&
                    Structure.Structures.StructureSets[0].Items[0] is SDMXObjectModel.Structure.ConceptSchemeMapType &&
                    ((SDMXObjectModel.Structure.ConceptSchemeMapType)Structure.Structures.StructureSets[0].Items[0]).Items != null &&
                    ((SDMXObjectModel.Structure.ConceptSchemeMapType)Structure.Structures.StructureSets[0].Items[0]).Items.Count > 0)
                {
                    foreach (SDMXObjectModel.Structure.ItemAssociationType ConceptMap in ((SDMXObjectModel.Structure.ConceptSchemeMapType)Structure.Structures.StructureSets[0].Items[0]).Items)
                    {
                        RetVal.Add(((SDMXObjectModel.Common.LocalConceptRefType)ConceptMap.Source.Items[0]).id, ((SDMXObjectModel.Common.LocalConceptRefType)ConceptMap.Target.Items[0]).id);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
        }

        return RetVal;
    }

    private void Get_SourceIdAgencyIdVersionMetadata(out string SourceId, out string SourceAgencyId, out string SourceVersion, string AssociatedDbNId)
    {
        string ConceptSchemeFileNameWPath;
        SDMXObjectModel.Message.StructureType ConceptScheme;

        ConceptSchemeFileNameWPath = string.Empty;
        ConceptScheme = null;

        SourceId = string.Empty;
        SourceAgencyId = string.Empty;
        SourceVersion = string.Empty;

        try
        {
            ConceptSchemeFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + AssociatedDbNId + "\\" + Constants.FolderName.SDMX.Concepts + DevInfo.Lib.DI_LibSDMX.Constants.ConceptScheme.MSD_Indicator.FileName);
            ConceptScheme = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), ConceptSchemeFileNameWPath);

            if (ConceptScheme != null && ConceptScheme.Structures != null && ConceptScheme.Structures.Concepts != null &&
                ConceptScheme.Structures.Concepts.Count > 0)
            {
                SourceId = ConceptScheme.Structures.Concepts[0].id;
                SourceAgencyId = ConceptScheme.Structures.Concepts[0].agencyID;
                SourceVersion = ConceptScheme.Structures.Concepts[0].version;
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
        }
    }

    private void Get_TargetIdAgencyIdVersionMetadata(out string TargetId, out string TargetAgencyId, out string TargetVersion, string DbNId)
    {
        string ConceptSchemeFileNameWPath;
        SDMXApi_2_0.Message.StructureType ConceptScheme;

        ConceptSchemeFileNameWPath = string.Empty;
        ConceptScheme = null;

        TargetId = string.Empty;
        TargetAgencyId = string.Empty;
        TargetVersion = string.Empty;

        try
        {
            ConceptSchemeFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId + "\\" + Constants.FolderName.SDMX.Concepts + Constants.UNSD.Concept.MSDConcepts.FileName);
            ConceptScheme = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromFile(typeof(SDMXApi_2_0.Message.StructureType), ConceptSchemeFileNameWPath);

            if (ConceptScheme != null && ConceptScheme.Concepts != null && ConceptScheme.Concepts.ConceptScheme != null &&
                ConceptScheme.Concepts.ConceptScheme.Count > 0)
            {
                TargetId = ConceptScheme.Concepts.ConceptScheme[0].id;
                TargetAgencyId = ConceptScheme.Concepts.ConceptScheme[0].agencyID;
                TargetVersion = ConceptScheme.Concepts.ConceptScheme[0].version;
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
        }
    }

    #endregion "--Metadata Mapping--"

    #region "--IUS Mapping--"

    private string GetIUSMappingList(string DbNId, string Language, StructureType SourceCodelistStructure, SDMXApi_2_0.Message.StructureType TargetCodelistStructure, StructureType MappingCodelistStructure, string SelectedAgeCodelist, string SelectedSexCodelist, string SelectedLocationCodelist)
    {
        string RetVal;
        string IndicatorGId, UnitGId, SubgroupValGId, AgeGId, SexGId, LocationGId, Indicator, Unit, SubgroupVal;
        string OriginalRowColor, OriginalRowStatus, OriginalRowState;
        string IndicatorCodelistId, UnitCodelistId, AreaCodelistId, AgeCodelistId, SexCodelistId, LocationCodelistId, NatureCodelistId, FreqCodelistId, SourceTypeCodelistId, UnitMultCodelistId;
        Dictionary<string, string> DictSourceIUS, DictTargetIndicator, DictTargetUnit, DictTargetAge, DictTargetSex, DictTargetLocation, DictTargetFrequency, DictTargetSourceType, DictTargetNature, DictTargetUnitMult, DictMappingIUS, DictMappingIndicator, DictMappingUnit, DictMappingAge, DictMappingSex, DictMappingLocation;
        StringBuilder Builder;

        RetVal = string.Empty;
        IndicatorGId = string.Empty;
        UnitGId = string.Empty;
        SubgroupValGId = string.Empty;
        AgeGId = string.Empty;
        SexGId = string.Empty;
        LocationGId = string.Empty;
        Indicator = string.Empty;
        Unit = string.Empty;
        SubgroupVal = string.Empty;

        OriginalRowColor = string.Empty;
        OriginalRowStatus = string.Empty;
        OriginalRowState = string.Empty;

        DictSourceIUS = null;
        DictTargetIndicator = null;
        DictTargetUnit = null;
        DictTargetAge = null;
        DictTargetSex = null;
        DictTargetLocation = null;
        DictTargetFrequency = null;
        DictTargetSourceType = null;
        DictTargetNature = null;
        DictTargetUnitMult = null;

        DictMappingIUS = null;
        DictMappingIndicator = null;
        DictMappingUnit = null;
        DictMappingAge = null;
        DictMappingSex = null;
        DictMappingLocation = null;
        IndicatorCodelistId = string.Empty;
        UnitCodelistId = string.Empty;
        AreaCodelistId = string.Empty;
        AgeCodelistId = string.Empty;
        SexCodelistId = string.Empty;
        LocationCodelistId = string.Empty;
        NatureCodelistId = string.Empty;
        FreqCodelistId = string.Empty;
        SourceTypeCodelistId = string.Empty;
        UnitMultCodelistId = string.Empty;
        Builder = new StringBuilder(RetVal);
        string NoRecordsFound = string.Empty;
        try
        {
            Global.GetAppSetting();

            foreach (SDMXApi_2_0.Structure.DimensionType Dimensions in TargetCodelistStructure.KeyFamilies[0].Components.Dimension)
            {
                if (Dimensions.conceptRef == Constants.UNSD.Concept.Indicator.Id)
                {
                    IndicatorCodelistId = Dimensions.codelist;
                }
                if (Dimensions.conceptRef == Constants.UNSD.Concept.Unit.Id)
                {
                    UnitCodelistId = Dimensions.codelist;
                }
                if (Dimensions.conceptRef == Constants.UNSD.Concept.Area.Id)
                {
                    AreaCodelistId = Dimensions.codelist;
                }
                if (Dimensions.conceptRef == Constants.UNSD.Concept.Age.Id)
                {
                    AgeCodelistId = Dimensions.codelist;
                }
                if (Dimensions.conceptRef == Constants.UNSD.Concept.Sex.Id)
                {
                    SexCodelistId = Dimensions.codelist;
                }
                if (Dimensions.conceptRef == Constants.UNSD.Concept.Location.Id)
                {
                    LocationCodelistId = Dimensions.codelist;
                }
                if (Dimensions.conceptRef == Constants.UNSD.Concept.Frequency.Id)
                {
                    FreqCodelistId = Dimensions.codelist;
                }

                if (Dimensions.conceptRef == Constants.UNSD.Concept.SourceType.Id)
                {
                    SourceTypeCodelistId = Dimensions.codelist;
                }

            }
            foreach (SDMXApi_2_0.Structure.AttributeType Attributes in TargetCodelistStructure.KeyFamilies[0].Components.Attribute)
            {
                if (Attributes.conceptRef == Constants.UNSD.Concept.Nature.Id)
                {
                    NatureCodelistId = Attributes.codelist;
                }
                if (Attributes.conceptRef == Constants.UNSD.Concept.UnitMult.Id)
                {
                    UnitMultCodelistId = Attributes.codelist;
                }
            }
            DictSourceIUS = this.Get_DictSourceIUS(Language, SourceCodelistStructure, MappingCodelistStructure);
            //DictTargetUnit = this.Get_DictTargetCodelist(Language, Constants.UNSD.CodeList.Unit.Id, TargetCodelistStructure);
            //DictTargetAge = this.Get_DictTargetCodelist(Language, Constants.UNSD.CodeList.Age.Id, TargetCodelistStructure);
            //DictTargetSex = this.Get_DictTargetCodelist(Language, Constants.UNSD.CodeList.Sex.Id, TargetCodelistStructure);
            //DictTargetLocation = this.Get_DictTargetCodelist(Language, Constants.UNSD.CodeList.Location.Id, TargetCodelistStructure);
            //DictTargetFrequency = this.Get_DictTargetCodelist(Language, Constants.UNSD.CodeList.Frequency.Id, TargetCodelistStructure);
            //DictTargetSourceType = this.Get_DictTargetCodelist(Language, Constants.UNSD.CodeList.SourceType.Id, TargetCodelistStructure);
            //DictTargetNature = this.Get_DictTargetCodelist(Language, Constants.UNSD.CodeList.Nature.Id, TargetCodelistStructure);
            //DictTargetUnitMult = this.Get_DictTargetCodelist(Language, Constants.UNSD.CodeList.UnitMult.Id, TargetCodelistStructure);

            DictTargetIndicator = this.Get_DictTargetCodelist(Language, IndicatorCodelistId, TargetCodelistStructure);
            DictTargetUnit = this.Get_DictTargetCodelist(Language, UnitCodelistId, TargetCodelistStructure);
            DictTargetAge = this.Get_DictTargetCodelist(Language, AgeCodelistId, TargetCodelistStructure);
            DictTargetSex = this.Get_DictTargetCodelist(Language, SexCodelistId, TargetCodelistStructure);
            DictTargetLocation = this.Get_DictTargetCodelist(Language, LocationCodelistId, TargetCodelistStructure);
            DictTargetFrequency = this.Get_DictTargetCodelist(Language, FreqCodelistId, TargetCodelistStructure);
            DictTargetSourceType = this.Get_DictTargetCodelist(Language, SourceTypeCodelistId, TargetCodelistStructure);
            DictTargetNature = this.Get_DictTargetCodelist(Language, NatureCodelistId, TargetCodelistStructure);
            DictTargetUnitMult = this.Get_DictTargetCodelist(Language, UnitMultCodelistId, TargetCodelistStructure);

            DictMappingIUS = this.Get_DictMappingIUS(DbNId);
            DictMappingIndicator = this.Get_DictMappingCodelist(DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.Indicator.id, MappingCodelistStructure);
            DictMappingUnit = this.Get_DictMappingCodelist(DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.Unit.id, MappingCodelistStructure);
            DictMappingAge = this.Get_DictMappingCodelist(DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.Age.id, MappingCodelistStructure);
            DictMappingSex = this.Get_DictMappingCodelist(DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.Sex.id, MappingCodelistStructure);
            DictMappingLocation = this.Get_DictMappingCodelist(DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.Location.id, MappingCodelistStructure);
            Builder.Append("<div style=\"overflow-x:hidden;\">");
            if (DictMappingIndicator.Count == 0)
            {
                NoRecordsFound = "NRF";
                RetVal = NoRecordsFound;
                return RetVal;
            }

            Builder.Append("<table>");
            Builder.Append("<tr>");

            Builder.Append("<td>");
            Builder.Append("<table id=\"tblIUSHeader\" style=\"width:100%; \">");//table-layout:fixed;
            Builder.Append("<col width=\"110\"/>");
            Builder.Append("<col width=\"60\"/>");
            Builder.Append("<col width=\"60\"/>");
            Builder.Append("<col width=\"23\"/>");
            Builder.Append("<col width=\"78\"/>");
            Builder.Append("<col width=\"78\"/>");
            Builder.Append("<col width=\"78\"/>");
            Builder.Append("<col width=\"78\"/>");
            Builder.Append("<col width=\"78\"/>");
            Builder.Append("<col width=\"78\"/>");
            Builder.Append("<col width=\"78\"/>");
            Builder.Append("<col width=\"78\"/>");
            Builder.Append("<col width=\"33\"/>");

            Builder.Append("<tr style=\"height:30px;\">");

            Builder.Append("<td colspan=\"7\" style=\"width:542px; align:left; overflow:hidden;\">");
            Builder.Append("<a id=\"aShowIUSAll\" href=\"javascript:void(0);\" style=\"color:#000000;\" onclick=\"FilterRowsByStatus('tblIUS', 'aShowIUSAll', 'aShowIUSMapped', 'aShowIUSUnMapped', 'aShowIUSUnSaved', 'all');\"></a>");
            Builder.Append("   |   ");
            Builder.Append("<a id=\"aShowIUSMapped\" href=\"javascript:void(0);\" style=\"color:#1e90ff;\" onclick=\"FilterRowsByStatus('tblIUS', 'aShowIUSAll', 'aShowIUSMapped', 'aShowIUSUnMapped', 'aShowIUSUnSaved', 'mapped');\"></a>&nbsp;");
            Builder.Append("<span style=\"background-color:#bbffbb;width:10px;\">&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            Builder.Append("   |   ");
            Builder.Append("<a id=\"aShowIUSUnMapped\" href=\"javascript:void(0);\" style=\"color:#1e90ff;\" onclick=\"FilterRowsByStatus('tblIUS', 'aShowIUSAll', 'aShowIUSMapped', 'aShowIUSUnMapped', 'aShowIUSUnSaved', 'unmapped');\"></a>&nbsp;");
            Builder.Append("<span style=\"background-color:#ffbbbb;width:10px;\">&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            Builder.Append("   |   ");
            Builder.Append("<a id=\"aShowIUSUnSaved\" href=\"javascript:void(0);\" style=\"color:#1e90ff;\" onclick=\"FilterRowsByStatus('tblIUS', 'aShowIUSAll', 'aShowIUSMapped', 'aShowIUSUnMapped', 'aShowIUSUnSaved', 'unsaved');\"></a>&nbsp;");
            Builder.Append("<span style=\"background-color:#bbbbff;width:10px;\">&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            Builder.Append("</td>");

            Builder.Append("<td colspan=\"6\" style=\"width:358px; align:left; overflow:hidden;\">");
            Builder.Append("<div id=\"divIUSSearch\">");
            Builder.Append("<input id=\"txtIUSSearch\" type=\"text\"/>");
            Builder.Append("</div>");
            Builder.Append("</td>");

            Builder.Append("</tr>");

            //Grid Header
            Builder.Append("<tr style=\"height:30px; width:100%; background: #dddddd;\">");
            Builder.Append("<td style=\"text-align: left; width: 11.4% ! important;\"><b id=\"lang_Indicator\"></b></td>");
            Builder.Append("<td style=\"width: 6%; text-align: left;\"><b id=\"lang_Unit\"></b></td>");
            Builder.Append("<td style=\"text-align: left; width: 6.2%;\"><b id=\"lang_Subgroup\"></b></td>");
            Builder.Append("<td style=\"text-align: left; width: 2.5%;\"><b id=\"lang_Mapped\"></b></td>");
            Builder.Append("<td style=\"text-align: left; width: 8%;\"><b id=\"lang_Indicator\">Indicator</b></td>");
            Builder.Append("<td style=\"text-align: left; width: 8%;\">");
            Builder.Append("<b id=\"lang_Unit_UNSD\"></b>");
            Builder.Append("</td>");
            Builder.Append("<td style=\"text-align: left; width: 8%;\">");
            Builder.Append("<b id=\"lang_Age\"></b>");
            Builder.Append("</td>");
            Builder.Append("<td style=\"text-align: left; width: 8%;\">");
            Builder.Append("<b id=\"lang_Sex\"></b>");
            Builder.Append("</td>");
            Builder.Append("<td style=\"text-align: left; width: 8%;\">");
            Builder.Append("<b id=\"lang_Location\"></b>");
            Builder.Append("</td>");
            Builder.Append("<td style=\"text-align: left; width: 8%;\">");
            Builder.Append("<b id=\"lang_Frequency\"></b>");
            Builder.Append("</td>");
            Builder.Append("<td style=\"text-align: left; width: 8%;\">");
            Builder.Append("<b id=\"lang_SourceType\"></b>");
            Builder.Append("</td>");
            Builder.Append("<td style=\"text-align: left; width: 8%;\">");
            Builder.Append("<b id=\"lang_Nature\"></b>");
            Builder.Append("</td>");
            Builder.Append("<td style=\"text-align: left; width: 10%;\">");
            Builder.Append("<b id=\"lang_UnitMultiplier\"></b>");
            Builder.Append("</td>");

            Builder.Append("</tr>");



            Builder.Append("</table>");
            Builder.Append("</td>");

            Builder.Append("</tr>");
            Builder.Append("<tr>");

            Builder.Append("<td>");
            //Grid Data
            Builder.Append("<div style=\"height:200px; overflow-y:scroll!important;\">");

            Builder.Append("<table id=\"tblIUS\" style=\"width:100%; table-layout:fixed;\">");

            //moved
            #region DIV
            Builder.Append("<tr>");//style=\"height:30px; background-color:#dddddd;\"
            Builder.Append("<td style=\"width:110px; align:left; overflow:hidden;\"></td>");
            Builder.Append("<td style=\"width:60px; align:left; overflow:hidden;\"></td>");
            Builder.Append("<td style=\"width:60px; align:left; overflow:hidden;\"></td>");
            Builder.Append("<td style=\"width:23px; align:left; overflow:hidden;\"><b id=\"lang_Mapped\" ></b></td>");

            Builder.Append("<td style=\"width:78px; align:left; overflow:hidden;\">");

            Builder.Append("<select id=\"ddlUNSDIUSIndicator\" class=\"confg_frm_inp_bx_txt_dd\" style=\"display:none;\">");
            Builder.Append("<option value=\"-1\">Select</option>");

            foreach (KeyValuePair<string, string> TargetIndicator in DictTargetIndicator)
            {
                Builder.Append("<option value=\"" + TargetIndicator.Key + "\" title=\"" + TargetIndicator.Value + "\">" + this.Get_TrimmedInnerHTML(TargetIndicator.Value, 75) + "</option>");
            }

            Builder.Append("</select>");
            Builder.Append("</td>");

            Builder.Append("<td style=\"width:78px; align:left; overflow:hidden;\">");
            //Builder.Append("<b id=\"lang_Unit_UNSD\"></b>");

            Builder.Append("<select id=\"ddlUNSDIUSUnit\" class=\"confg_frm_inp_bx_txt_dd\" style=\"display:none;\">");
            Builder.Append("<option value=\"-1\">Select</option>");

            foreach (KeyValuePair<string, string> TargetUnit in DictTargetUnit)
            {
                Builder.Append("<option value=\"" + TargetUnit.Key + "\" title=\"" + TargetUnit.Value + "\">" + this.Get_TrimmedInnerHTML(TargetUnit.Value, 75) + "</option>");
            }

            Builder.Append("</select>");
            Builder.Append("</td>");

            Builder.Append("<td style=\"width:78px; align:left; overflow:hidden;\">");
            //Builder.Append("<b id=\"lang_Age\"></b>");

            Builder.Append("<select id=\"ddlUNSDIUSAge\" class=\"confg_frm_inp_bx_txt_dd\" style=\"display:none;\">");
            Builder.Append("<option value=\"-1\">Select</option>");

            foreach (KeyValuePair<string, string> TargetAge in DictTargetAge)
            {
                Builder.Append("<option value=\"" + TargetAge.Key + "\" title=\"" + TargetAge.Value + "\">" + this.Get_TrimmedInnerHTML(TargetAge.Value, 75) + "</option>");
            }

            Builder.Append("</select>");
            Builder.Append("</td>");

            Builder.Append("<td style=\"width:78px; align:left; overflow:hidden;\">");
            //Builder.Append("<b id=\"lang_Sex\"></b>");

            Builder.Append("<select id=\"ddlUNSDIUSSex\" class=\"confg_frm_inp_bx_txt_dd\" style=\"display:none;\">");
            Builder.Append("<option value=\"-1\">Select</option>");

            foreach (KeyValuePair<string, string> TargetSex in DictTargetSex)
            {
                Builder.Append("<option value=\"" + TargetSex.Key + "\" title=\"" + TargetSex.Value + "\">" + this.Get_TrimmedInnerHTML(TargetSex.Value, 75) + "</option>");
            }

            Builder.Append("</select>");
            Builder.Append("</td>");

            Builder.Append("<td style=\"width:78px; align:left; overflow:hidden;\">");
            //Builder.Append("<b id=\"lang_Location\"></b>");

            Builder.Append("<select id=\"ddlUNSDIUSLocation\" class=\"confg_frm_inp_bx_txt_dd\" style=\"display:none;\">");
            Builder.Append("<option value=\"-1\">Select</option>");

            foreach (KeyValuePair<string, string> TargetLocation in DictTargetLocation)
            {
                Builder.Append("<option value=\"" + TargetLocation.Key + "\" title=\"" + TargetLocation.Value + "\">" + this.Get_TrimmedInnerHTML(TargetLocation.Value, 75) + "</option>");
            }

            Builder.Append("</select>");
            Builder.Append("</td>");

            Builder.Append("<td style=\"width:78px; align:left; overflow:hidden;\">");
            //Builder.Append("<b id=\"lang_Frequency\"></b>");

            Builder.Append("<select id=\"ddlUNSDIUSFrequency\" class=\"confg_frm_inp_bx_txt_dd\" style=\"display:none;\">");
            Builder.Append("<option value=\"-1\">Select</option>");

            foreach (KeyValuePair<string, string> TargetFrequency in DictTargetFrequency)
            {
                Builder.Append("<option value=\"" + TargetFrequency.Key + "\" title=\"" + TargetFrequency.Value + "\">" + this.Get_TrimmedInnerHTML(TargetFrequency.Value, 75) + "</option>");
            }

            Builder.Append("</select>");
            Builder.Append("</td>");

            Builder.Append("<td style=\"width:78px; align:left; overflow:hidden;\">");
            //Builder.Append("<b id=\"lang_SourceType\"></b>");

            Builder.Append("<select id=\"ddlUNSDIUSSourceType\" class=\"confg_frm_inp_bx_txt_dd\" style=\"display:none;\">");
            Builder.Append("<option value=\"-1\">Select</option>");

            foreach (KeyValuePair<string, string> TargetSourceType in DictTargetSourceType)
            {
                Builder.Append("<option value=\"" + TargetSourceType.Key + "\" title=\"" + TargetSourceType.Value + "\">" + this.Get_TrimmedInnerHTML(TargetSourceType.Value, 75) + "</option>");
            }

            Builder.Append("</select>");
            Builder.Append("</td>");

            Builder.Append("<td style=\"width:78px; align:left; overflow:hidden;\">");
            //Builder.Append("<b id=\"lang_Nature\"></b>");

            Builder.Append("<select id=\"ddlUNSDIUSNature\" class=\"confg_frm_inp_bx_txt_dd\" style=\"display:none;\">");
            Builder.Append("<option value=\"-1\">Select</option>");

            foreach (KeyValuePair<string, string> TargetNature in DictTargetNature)
            {
                Builder.Append("<option value=\"" + TargetNature.Key + "\" title=\"" + TargetNature.Value + "\">" + this.Get_TrimmedInnerHTML(TargetNature.Value, 75) + "</option>");
            }

            Builder.Append("</select>");
            Builder.Append("</td>");

            Builder.Append("<td style=\"width:78px; align:left; overflow:hidden;\">");
            //Builder.Append("<b id=\"lang_UnitMultiplier\"></b>");

            Builder.Append("<select id=\"ddlUNSDIUSUnitMult\" class=\"confg_frm_inp_bx_txt_dd\" style=\"display:none;\">");
            Builder.Append("<option value=\"-1\">Select</option>");

            foreach (KeyValuePair<string, string> TargetUnitMult in DictTargetUnitMult)
            {
                Builder.Append("<option value=\"" + TargetUnitMult.Key + "\" title=\"" + TargetUnitMult.Value + "\">" + this.Get_TrimmedInnerHTML(TargetUnitMult.Value, 75) + "</option>");
            }

            Builder.Append("</select>");
            Builder.Append("</td>");

            // Builder.Append("<td style=\"width:33px; align:left; overflow:hidden;\"></td>");

            Builder.Append("</tr>");

            #endregion DIV
            //till here

            Builder.Append("<col width=\"110\"/>");
            Builder.Append("<col width=\"60\"/>");
            Builder.Append("<col width=\"60\"/>");
            Builder.Append("<col width=\"23\"/>");
            Builder.Append("<col width=\"78\"/>");
            Builder.Append("<col width=\"78\"/>");
            Builder.Append("<col width=\"78\"/>");
            Builder.Append("<col width=\"78\"/>");
            Builder.Append("<col width=\"78\"/>");
            Builder.Append("<col width=\"78\"/>");
            Builder.Append("<col width=\"78\"/>");
            Builder.Append("<col width=\"78\"/>");

            foreach (string IUSGId in DictSourceIUS.Keys)
            {
                //check here
                IndicatorGId = IUSGId.Split(new string[] { "@__@" }, StringSplitOptions.None)[0];
                UnitGId = IUSGId.Split(new string[] { "@__@" }, StringSplitOptions.None)[1];
                SubgroupValGId = IUSGId.Split(new string[] { "@__@" }, StringSplitOptions.None)[2];
                this.Get_SubgroupBreakup(SubgroupValGId, ref AgeGId, ref SexGId, ref LocationGId, SourceCodelistStructure, SelectedAgeCodelist, SelectedSexCodelist, SelectedLocationCodelist);
                Indicator = DictSourceIUS[IUSGId].ToString().Split(new string[] { "@__@" }, StringSplitOptions.None)[0];
                Unit = DictSourceIUS[IUSGId].ToString().Split(new string[] { "@__@" }, StringSplitOptions.None)[1];
                SubgroupVal = DictSourceIUS[IUSGId].ToString().Split(new string[] { "@__@" }, StringSplitOptions.None)[2];

                OriginalRowState = string.Empty;
                if (DictMappingIUS.ContainsKey(IUSGId))
                {
                    OriginalRowColor = "rgb(221, 255, 221);";
                    OriginalRowStatus = "mapped";
                }
                else
                {
                    OriginalRowColor = "rgb(255, 221, 221);";
                    OriginalRowStatus = "unmapped";
                }

                Builder.Append("<tr id=\"rowIUS_" + IUSGId + "\" style=\"background-color:" + OriginalRowColor + ");\" status=\"" + OriginalRowStatus + "\">");

                Builder.Append("<td style=\"width:110px; align:left; overflow:hidden;\">");
                Builder.Append("<span id=\"spanDevInfoIUSIndicatorName_" + IndicatorGId + "\" value=\"" + Indicator + "\" style=\"font-weight:bold;\" onmouseover=\"ShowCallout('divCallout', '" + Indicator + "', event);\" onmouseout=\"HideCallout('divCallout');\">" + this.Get_TrimmedInnerHTML(Indicator, 25) + "</span>");// onmouseover=\"ShowCallout('divCallout', '" + Indicator + "', event);\" onmouseout=\"HideCallout('divCallout');
                Builder.Append("</td>");

                Builder.Append("<td style=\"width:60px; align:left; overflow:hidden;\">");
                Builder.Append("<span id=\"spanDevInfoIUSUnitName_" + UnitGId + "\" value=\"" + Unit + "\"  onmouseover=\"ShowCallout('divCallout', '" + Unit + "', event);\" onmouseout=\"HideCallout('divCallout');\">" + this.Get_TrimmedInnerHTML(Unit, 25) + "</span>");// onmouseover=\"ShowCallout('divCallout', '" + Unit + "', event);\" onmouseout=\"HideCallout('divCallout');
                Builder.Append("</td>");

                Builder.Append("<td style=\"width:60px; align:left; overflow:hidden;\">");
                Builder.Append("<span id=\"spanDevInfoIUSSubgroupName_" + SubgroupValGId + "\" value=\"" + SubgroupVal + "\" onmouseover=\"ShowCallout('divCallout', '" + SubgroupVal + "', event);\" onmouseout=\"HideCallout('divCallout');\">" + this.Get_TrimmedInnerHTML(SubgroupVal, 25) + "</span>");//onmouseover=\"ShowCallout('divCallout', '" + SubgroupVal + "', event);\" onmouseout=\"HideCallout('divCallout');
                Builder.Append("</td>");

                Builder.Append("<td style=\"width:23px; align:left; overflow:hidden;\">");

                if (DictMappingIUS.ContainsKey(IUSGId))
                {
                    Builder.Append("<input type=\"checkbox\" id=\"chkIsMapped_" + IUSGId + "\" checked=\"checked\" value=\"" + IUSGId + "\" onchange=\"HandleStateChange('rowIUS_" + IUSGId + "', '[**[ORIGINAL_ROW_STATE]**]', '" + OriginalRowColor + "', '" + OriginalRowStatus + "');\"/>");
                    OriginalRowState += "true" + Constants.Delimiters.ParamDelimiter;
                }
                else
                {
                    Builder.Append("<input type=\"checkbox\" id=\"chkIsMapped_" + IUSGId + "\" value=\"" + IUSGId + "\" onchange=\"HandleStateChange('rowIUS_" + IUSGId + "', '[**[ORIGINAL_ROW_STATE]**]', '" + OriginalRowColor + "', '" + OriginalRowStatus + "');\"/>");
                    OriginalRowState += "false" + Constants.Delimiters.ParamDelimiter;
                }

                Builder.Append("</td>");

                //Builder.Append("<td style=\"display:none;\">");
                //Builder.Append("<span style=\"display:none;\">" + DictMappingIndicator[IndicatorGId].ToString() + "</span>");
                //Builder.Append("</td>");
                //new 
                Builder.Append("<td style=\"width:78px; align:left;overflow:hidden;\">");// 
                Builder.Append("<select id=\"ddlUNSDIUSIndicator_" + IUSGId + "\" style=\"width:75px\" class=\"confg_frm_inp_bx_txt_dd\" onchange=\"HandleStateChange('rowIUS_" + IUSGId + "', '[**[ORIGINAL_ROW_STATE]**]', '" + OriginalRowColor + "', '" + OriginalRowStatus + "');\">");
                Builder.Append("<option value=\"-1\" id=\"SelectUNSDIUSIndicator_" + IUSGId + "\"></option>");

                if (DictMappingIUS.ContainsKey(IUSGId))
                {
                    OriginalRowState += DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[0] + Constants.Delimiters.ParamDelimiter;
                    Builder.Append("<option value=\"" + DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[0] + "\" selected=\"selected\" title=\"" + DictTargetIndicator[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[0]] + "\">" + this.Get_TrimmedInnerHTML(DictTargetIndicator[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[0]], 75) + "</option>");
                }
                else if (DictMappingIndicator.ContainsKey(IndicatorGId))
                {
                    OriginalRowState += DictMappingIndicator[IndicatorGId].ToString() + Constants.Delimiters.ParamDelimiter;
                    Builder.Append("<option value=\"" + DictMappingIndicator[IndicatorGId] + "\" selected=\"selected\" title=\"" + DictTargetIndicator[DictMappingIndicator[IndicatorGId]] + "\">" + this.Get_TrimmedInnerHTML(DictTargetIndicator[DictMappingIndicator[IndicatorGId]], 75) + "</option>");
                }
                else
                {
                    OriginalRowState += "-1" + Constants.Delimiters.ParamDelimiter;
                }

                Builder.Append("</select>");
                Builder.Append("</td>");

                //new 
                Builder.Append("<td style=\"width:78px; align:left;overflow:hidden;\">");// 
                Builder.Append("<select id=\"ddlUNSDIUSUnit_" + IUSGId + "\" style=\"width:75px\" class=\"confg_frm_inp_bx_txt_dd\" onchange=\"HandleStateChange('rowIUS_" + IUSGId + "', '[**[ORIGINAL_ROW_STATE]**]', '" + OriginalRowColor + "', '" + OriginalRowStatus + "');\">");
                Builder.Append("<option value=\"-1\" id=\"SelectUNSDIUSUnit_" + IUSGId + "\"></option>");

                if (DictMappingIUS.ContainsKey(IUSGId))
                {
                    OriginalRowState += DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[1] + Constants.Delimiters.ParamDelimiter;
                    Builder.Append("<option value=\"" + DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[1] + "\" selected=\"selected\" title=\"" + DictTargetUnit[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[1]] + "\">" + this.Get_TrimmedInnerHTML(DictTargetUnit[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[1]], 75) + "</option>");
                }
                else if (DictMappingUnit.ContainsKey(UnitGId))
                {
                    OriginalRowState += DictMappingUnit[UnitGId].ToString() + Constants.Delimiters.ParamDelimiter;
                    Builder.Append("<option value=\"" + DictMappingUnit[UnitGId] + "\" selected=\"selected\" title=\"" + DictTargetUnit[DictMappingUnit[UnitGId]] + "\">" + this.Get_TrimmedInnerHTML(DictTargetUnit[DictMappingUnit[UnitGId]], 75) + "</option>");
                }
                else
                {
                    OriginalRowState += "-1" + Constants.Delimiters.ParamDelimiter;
                }

                Builder.Append("</select>");
                Builder.Append("</td>");


                Builder.Append("<td style=\"width:78px; align:left;overflow:hidden; \">");//
                Builder.Append("<select id=\"ddlUNSDIUSAge_" + IUSGId + "\" style=\"width:75px\" class=\"confg_frm_inp_bx_txt_dd\" onchange=\"HandleStateChange('rowIUS_" + IUSGId + "', '[**[ORIGINAL_ROW_STATE]**]', '" + OriginalRowColor + "', '" + OriginalRowStatus + "');\">");
                Builder.Append("<option value=\"-1\" id=\"SelectUNSDIUSAge_" + IUSGId + "\"></option>");

                if (DictMappingIUS.ContainsKey(IUSGId))
                {
                    OriginalRowState += DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[2] + Constants.Delimiters.ParamDelimiter;
                    Builder.Append("<option value=\"" + DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[2] + "\" selected=\"selected\" title=\"" + DictTargetAge[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[2]] + "\">" + this.Get_TrimmedInnerHTML(DictTargetAge[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[2]], 75) + "</option>");
                }
                else if (DictMappingAge.ContainsKey(AgeGId))
                {
                    OriginalRowState += DictMappingAge[AgeGId].ToString() + Constants.Delimiters.ParamDelimiter;
                    Builder.Append("<option value=\"" + DictMappingAge[AgeGId] + "\" selected=\"selected\" title=\"" + DictTargetAge[DictMappingAge[AgeGId]] + "\">" + this.Get_TrimmedInnerHTML(DictTargetAge[DictMappingAge[AgeGId]], 75) + "</option>");
                }
                else if (DictTargetAge.ContainsKey(Global.registryMappingAgeDefaultValue))
                {
                    OriginalRowState += Global.registryMappingAgeDefaultValue + Constants.Delimiters.ParamDelimiter;
                    Builder.Append("<option value=\"" + Global.registryMappingAgeDefaultValue + "\" selected=\"selected\" title=\"" + DictTargetAge[Global.registryMappingAgeDefaultValue] + "\">" + this.Get_TrimmedInnerHTML(DictTargetAge[Global.registryMappingAgeDefaultValue], 75) + "</option>");
                }
                else
                {
                    OriginalRowState += "-1" + Constants.Delimiters.ParamDelimiter;
                }

                Builder.Append("</select>");
                Builder.Append("</td>");

                Builder.Append("<td style=\"width:78px; align:left;overflow:hidden; \">");//
                Builder.Append("<select id=\"ddlUNSDIUSSex_" + IUSGId + "\" style=\"width:75px\" class=\"confg_frm_inp_bx_txt_dd\" onchange=\"HandleStateChange('rowIUS_" + IUSGId + "', '[**[ORIGINAL_ROW_STATE]**]', '" + OriginalRowColor + "', '" + OriginalRowStatus + "');\">");
                Builder.Append("<option value=\"-1\" id=\"SelectUNSDIUSSex_" + IUSGId + "\"></option>");

                if (DictMappingIUS.ContainsKey(IUSGId))
                {
                    OriginalRowState += DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[3] + Constants.Delimiters.ParamDelimiter;
                    Builder.Append("<option value=\"" + DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[3] + "\" selected=\"selected\" title=\"" + DictTargetSex[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[3]] + "\">" + this.Get_TrimmedInnerHTML(DictTargetSex[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[3]], 75) + "</option>");
                }
                else if (DictMappingSex.ContainsKey(SexGId))
                {
                    OriginalRowState += DictMappingSex[SexGId].ToString() + Constants.Delimiters.ParamDelimiter;
                    Builder.Append("<option value=\"" + DictMappingSex[SexGId] + "\" selected=\"selected\" title=\"" + DictTargetSex[DictMappingSex[SexGId]] + "\">" + this.Get_TrimmedInnerHTML(DictTargetSex[DictMappingSex[SexGId]], 75) + "</option>");
                }
                else if (DictTargetSex.ContainsKey(Global.registryMappingSexDefaultValue))
                {
                    OriginalRowState += Global.registryMappingSexDefaultValue + Constants.Delimiters.ParamDelimiter;
                    Builder.Append("<option value=\"" + Global.registryMappingSexDefaultValue + "\" selected=\"selected\" title=\"" + DictTargetSex[Global.registryMappingSexDefaultValue] + "\">" + this.Get_TrimmedInnerHTML(DictTargetSex[Global.registryMappingSexDefaultValue], 75) + "</option>");
                }
                else
                {
                    OriginalRowState += "-1" + Constants.Delimiters.ParamDelimiter;
                }

                Builder.Append("</select>");
                Builder.Append("</td>");

                Builder.Append("<td style=\"width:78px; align:left; overflow:hidden;\">");//
                Builder.Append("<select id=\"ddlUNSDIUSLocation_" + IUSGId + "\" style=\"width:75px\" class=\"confg_frm_inp_bx_txt_dd\" onchange=\"HandleStateChange('rowIUS_" + IUSGId + "', '[**[ORIGINAL_ROW_STATE]**]', '" + OriginalRowColor + "', '" + OriginalRowStatus + "');\">");
                Builder.Append("<option value=\"-1\" id=\"SelectUNSDIUSLocation_" + IUSGId + "\"></option>");

                if (DictMappingIUS.ContainsKey(IUSGId))
                {
                    OriginalRowState += DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[4] + Constants.Delimiters.ParamDelimiter;
                    Builder.Append("<option value=\"" + DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[4] + "\" selected=\"selected\" title=\"" + DictTargetLocation[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[4]] + "\">" + this.Get_TrimmedInnerHTML(DictTargetLocation[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[4]], 75) + "</option>");
                }
                else if (DictMappingLocation.ContainsKey(LocationGId))
                {
                    OriginalRowState += DictMappingLocation[LocationGId].ToString() + Constants.Delimiters.ParamDelimiter;
                    Builder.Append("<option value=\"" + DictMappingLocation[LocationGId] + "\" selected=\"selected\" title=\"" + DictTargetLocation[DictMappingLocation[LocationGId]] + "\">" + this.Get_TrimmedInnerHTML(DictTargetLocation[DictMappingLocation[LocationGId]], 75) + "</option>");
                }
                else if (DictTargetLocation.ContainsKey(Global.registryMappingLocationDefaultValue))
                {
                    OriginalRowState += Global.registryMappingLocationDefaultValue + Constants.Delimiters.ParamDelimiter;
                    Builder.Append("<option value=\"" + Global.registryMappingLocationDefaultValue + "\" selected=\"selected\" title=\"" + DictTargetLocation[Global.registryMappingLocationDefaultValue] + "\">" + this.Get_TrimmedInnerHTML(DictTargetLocation[Global.registryMappingLocationDefaultValue], 75) + "</option>");
                }
                else
                {
                    OriginalRowState += "-1" + Constants.Delimiters.ParamDelimiter;
                }

                Builder.Append("</select>");
                Builder.Append("</td>");

                Builder.Append("<td style=\"width:78px; align:left; overflow:hidden;\">");//
                Builder.Append("<select id=\"ddlUNSDIUSFrequency_" + IUSGId + "\" style=\"width:75px\" class=\"confg_frm_inp_bx_txt_dd\" onchange=\"HandleStateChange('rowIUS_" + IUSGId + "', '[**[ORIGINAL_ROW_STATE]**]', '" + OriginalRowColor + "', '" + OriginalRowStatus + "');\">");
                Builder.Append("<option value=\"-1\" id=\"SelectUNSDIUSFrequency_" + IUSGId + "\"></option>");

                if (DictMappingIUS.ContainsKey(IUSGId))
                {
                    OriginalRowState += DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[5] + Constants.Delimiters.ParamDelimiter;
                    Builder.Append("<option value=\"" + DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[5] + "\" selected=\"selected\" title=\"" + DictTargetFrequency[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[5]] + "\">" + this.Get_TrimmedInnerHTML(DictTargetFrequency[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[5]], 75) + "</option>");
                }
                else if (DictTargetFrequency.ContainsKey(Global.registryMappingFrequencyDefaultValue))
                {
                    OriginalRowState += Global.registryMappingFrequencyDefaultValue + Constants.Delimiters.ParamDelimiter;
                    Builder.Append("<option value=\"" + Global.registryMappingFrequencyDefaultValue + "\" selected=\"selected\" title=\"" + DictTargetFrequency[Global.registryMappingFrequencyDefaultValue] + "\">" + this.Get_TrimmedInnerHTML(DictTargetFrequency[Global.registryMappingFrequencyDefaultValue], 75) + "</option>");
                }
                else
                {
                    OriginalRowState += "-1" + Constants.Delimiters.ParamDelimiter;
                }

                Builder.Append("</select>");
                Builder.Append("</td>");

                Builder.Append("<td style=\"width:78px; align:left; overflow:hidden;\">");//
                Builder.Append("<select id=\"ddlUNSDIUSSourceType_" + IUSGId + "\" style=\"width:75px\" class=\"confg_frm_inp_bx_txt_dd\" onchange=\"HandleStateChange('rowIUS_" + IUSGId + "', '[**[ORIGINAL_ROW_STATE]**]', '" + OriginalRowColor + "', '" + OriginalRowStatus + "');\">");
                Builder.Append("<option value=\"-1\" id=\"SelectUNSDIUSSourceType_" + IUSGId + "\"></option>");

                if (DictMappingIUS.ContainsKey(IUSGId))
                {
                    OriginalRowState += DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[6] + Constants.Delimiters.ParamDelimiter;
                    Builder.Append("<option value=\"" + DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[6] + "\" selected=\"selected\" title=\"" + DictTargetSourceType[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[6]] + "\">" + this.Get_TrimmedInnerHTML(DictTargetSourceType[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[6]], 75) + "</option>");
                }
                else if (DictTargetSourceType.ContainsKey(Global.registryMappingSourceTypeDefaultValue))
                {
                    OriginalRowState += Global.registryMappingSourceTypeDefaultValue + Constants.Delimiters.ParamDelimiter;
                    Builder.Append("<option value=\"" + Global.registryMappingSourceTypeDefaultValue + "\" selected=\"selected\" title=\"" + DictTargetSourceType[Global.registryMappingSourceTypeDefaultValue] + "\">" + this.Get_TrimmedInnerHTML(DictTargetSourceType[Global.registryMappingSourceTypeDefaultValue], 75) + "</option>");
                }
                else
                {
                    OriginalRowState += "-1" + Constants.Delimiters.ParamDelimiter;
                }

                Builder.Append("</select>");
                Builder.Append("</td>");

                Builder.Append("<td style=\"width:78px; align:left; overflow:hidden;\">");//
                Builder.Append("<select id=\"ddlUNSDIUSNature_" + IUSGId + "\" style=\"width:75px\" class=\"confg_frm_inp_bx_txt_dd\" onchange=\"HandleStateChange('rowIUS_" + IUSGId + "', '[**[ORIGINAL_ROW_STATE]**]', '" + OriginalRowColor + "', '" + OriginalRowStatus + "');\">");
                Builder.Append("<option value=\"-1\" id=\"SelectUNSDIUSNature_" + IUSGId + "\"></option>");

                if (DictMappingIUS.ContainsKey(IUSGId))
                {
                    OriginalRowState += DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[7] + Constants.Delimiters.ParamDelimiter;
                    Builder.Append("<option value=\"" + DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[7] + "\" selected=\"selected\" title=\"" + DictTargetNature[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[7]] + "\">" + this.Get_TrimmedInnerHTML(DictTargetNature[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[7]], 75) + "</option>");
                }
                else if (DictTargetNature.ContainsKey(Global.registryMappingNatureDefaultValue))
                {
                    OriginalRowState += Global.registryMappingNatureDefaultValue + Constants.Delimiters.ParamDelimiter;
                    Builder.Append("<option value=\"" + Global.registryMappingNatureDefaultValue + "\" selected=\"selected\" title=\"" + DictTargetNature[Global.registryMappingNatureDefaultValue] + "\">" + this.Get_TrimmedInnerHTML(DictTargetNature[Global.registryMappingNatureDefaultValue], 75) + "</option>");
                }
                else
                {
                    OriginalRowState += "-1" + Constants.Delimiters.ParamDelimiter;
                }

                Builder.Append("</select>");
                Builder.Append("</td>");

                Builder.Append("<td style=\"width:78px; align:left;overflow:hidden; \">");//
                Builder.Append("<select id=\"ddlUNSDIUSUnitMult_" + IUSGId + "\" style=\"width:75px\" class=\"confg_frm_inp_bx_txt_dd\" onchange=\"HandleStateChange('rowIUS_" + IUSGId + "', '[**[ORIGINAL_ROW_STATE]**]', '" + OriginalRowColor + "', '" + OriginalRowStatus + "');\">");
                Builder.Append("<option value=\"-1\" id=\"SelectUNSDIUSUnitMult_" + IUSGId + "\"></option>");

                if (DictMappingIUS.ContainsKey(IUSGId))
                {
                    OriginalRowState += DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[8] + Constants.Delimiters.ParamDelimiter;
                    Builder.Append("<option value=\"" + DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[8] + "\" selected=\"selected\" title=\"" + DictTargetUnitMult[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[8]] + "\">" + this.Get_TrimmedInnerHTML(DictTargetUnitMult[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[8]], 75) + "</option>");
                }
                else if (DictTargetUnitMult.ContainsKey(Global.registryMappingUnitMultDefaultValue))
                {
                    OriginalRowState += Global.registryMappingUnitMultDefaultValue + Constants.Delimiters.ParamDelimiter;
                    Builder.Append("<option value=\"" + Global.registryMappingUnitMultDefaultValue + "\" selected=\"selected\" title=\"" + DictTargetUnitMult[Global.registryMappingUnitMultDefaultValue] + "\">" + this.Get_TrimmedInnerHTML(DictTargetUnitMult[Global.registryMappingUnitMultDefaultValue], 75) + "</option>");
                }
                else
                {
                    OriginalRowState += "-1" + Constants.Delimiters.ParamDelimiter;
                }

                Builder.Append("</select>");
                Builder.Append("</td>");

                if (!string.IsNullOrEmpty(OriginalRowState))
                {
                    OriginalRowState = OriginalRowState.Substring(0, OriginalRowState.Length - Constants.Delimiters.ParamDelimiter.Length);
                    Builder = Builder.Replace("[**[ORIGINAL_ROW_STATE]**]", OriginalRowState);
                }

                Builder.Append("</tr>");
            }

            Builder.Append("</table>");
            Builder.Append("</div>");
            Builder.Append("</td>");

            Builder.Append("</tr>");
            Builder.Append("</table>");


            #region Start1 7 nov 2013
            //Builder.Append("<table id=\"tblIUSHeader\" style=\"width:100%; table-layout:fixed;\">");
            //Builder.Append("<col width=\"110\"/>");
            //Builder.Append("<col width=\"60\"/>");
            //Builder.Append("<col width=\"60\"/>");
            //Builder.Append("<col width=\"23\"/>");
            //Builder.Append("<col width=\"78\"/>");
            //Builder.Append("<col width=\"78\"/>");
            //Builder.Append("<col width=\"78\"/>");
            //Builder.Append("<col width=\"78\"/>");
            //Builder.Append("<col width=\"78\"/>");
            //Builder.Append("<col width=\"78\"/>");
            //Builder.Append("<col width=\"78\"/>");
            //Builder.Append("<col width=\"78\"/>");
            //Builder.Append("<col width=\"33\"/>");

            //Builder.Append("<tr style=\"height:30px;\">");

            //Builder.Append("<td colspan=\"7\" style=\"width:542px; align:left; overflow:hidden;\">");
            //Builder.Append("<a id=\"aShowIUSAll\" href=\"javascript:void(0);\" style=\"color:#000000;\" onclick=\"FilterRowsByStatus('tblIUS', 'aShowIUSAll', 'aShowIUSMapped', 'aShowIUSUnMapped', 'aShowIUSUnSaved', 'all');\"></a>");
            //Builder.Append("   |   ");
            //Builder.Append("<a id=\"aShowIUSMapped\" href=\"javascript:void(0);\" style=\"color:#1e90ff;\" onclick=\"FilterRowsByStatus('tblIUS', 'aShowIUSAll', 'aShowIUSMapped', 'aShowIUSUnMapped', 'aShowIUSUnSaved', 'mapped');\"></a>&nbsp;");
            //Builder.Append("<span style=\"background-color:#bbffbb;width:10px;\">&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            //Builder.Append("   |   ");
            //Builder.Append("<a id=\"aShowIUSUnMapped\" href=\"javascript:void(0);\" style=\"color:#1e90ff;\" onclick=\"FilterRowsByStatus('tblIUS', 'aShowIUSAll', 'aShowIUSMapped', 'aShowIUSUnMapped', 'aShowIUSUnSaved', 'unmapped');\"></a>&nbsp;");
            //Builder.Append("<span style=\"background-color:#ffbbbb;width:10px;\">&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            //Builder.Append("   |   ");
            //Builder.Append("<a id=\"aShowIUSUnSaved\" href=\"javascript:void(0);\" style=\"color:#1e90ff;\" onclick=\"FilterRowsByStatus('tblIUS', 'aShowIUSAll', 'aShowIUSMapped', 'aShowIUSUnMapped', 'aShowIUSUnSaved', 'unsaved');\"></a>&nbsp;");
            //Builder.Append("<span style=\"background-color:#bbbbff;width:10px;\">&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            //Builder.Append("</td>");

            //Builder.Append("<td colspan=\"6\" style=\"width:358px; align:left; overflow:hidden;\">");
            //Builder.Append("<div id=\"divIUSSearch\">");
            //Builder.Append("<input id=\"txtIUSSearch\" type=\"text\"/>");
            //Builder.Append("</div>");
            //Builder.Append("</td>");

            //Builder.Append("</tr>");

            ////Grid Header
            //Builder.Append("<tr style=\"height:30px; background-color:#dddddd;\">");
            //Builder.Append("<td style=\"width:110px; align:left; overflow:hidden;\"><b id=\"lang_Indicator\"></b></td>");
            //Builder.Append("<td style=\"width:60px; align:left; overflow:hidden;\"><b id=\"lang_Unit\"></b></td>");
            //Builder.Append("<td style=\"width:60px; align:left; overflow:hidden;\"><b id=\"lang_Subgroup\"></b></td>");
            //Builder.Append("<td style=\"width:23px; align:left; overflow:hidden;\"><b id=\"lang_Mapped\"></b></td>");
            //Builder.Append("<td style=\"width:78px;\"><b id=\"lang_Indicator\">Indicator</b></td>");
            //Builder.Append("<td>");
            //Builder.Append("<b id=\"lang_Unit_UNSD\"></b>");
            //Builder.Append("</td>");
            //Builder.Append("<td>");
            //Builder.Append("<b id=\"lang_Age\"></b>");
            //Builder.Append("</td>");
            //Builder.Append("<td>");
            //Builder.Append("<b id=\"lang_Sex\"></b>");
            //Builder.Append("</td>");
            //Builder.Append("<td>");
            //Builder.Append("<b id=\"lang_Location\"></b>");
            //Builder.Append("</td>");
            //Builder.Append("<td>");
            //Builder.Append("<b id=\"lang_Frequency\"></b>");
            //Builder.Append("</td>");
            //Builder.Append("<td>");
            //Builder.Append("<b id=\"lang_SourceType\"></b>");
            //Builder.Append("</td>");
            //Builder.Append("<td>");
            //Builder.Append("<b id=\"lang_Nature\"></b>");
            //Builder.Append("</td>");
            //Builder.Append("<td>");
            //Builder.Append("<b id=\"lang_UnitMultiplier\"></b>");
            //Builder.Append("</td>");

            //Builder.Append("</tr>");



            //Builder.Append("</table>");


            #endregion Start1 Nov 7

            Builder.Append("</div>");

            //Builder.Append("<div style=\"\">");

            #region Start2 Nov 7

            // //Grid Data
            // Builder.Append("<table id=\"tblIUS\" style=\"width:100%; table-layout:fixed;\">");

            // //moved
            //#region DIV
            // Builder.Append("<tr>");//style=\"height:30px; background-color:#dddddd;\"
            // Builder.Append("<td style=\"width:110px; align:left; overflow:hidden;\"></td>");
            // Builder.Append("<td style=\"width:60px; align:left; overflow:hidden;\"></td>");
            // Builder.Append("<td style=\"width:60px; align:left; overflow:hidden;\"></td>");
            // Builder.Append("<td style=\"width:23px; align:left; overflow:hidden;\"><b id=\"lang_Mapped\" ></b></td>");

            // Builder.Append("<td style=\"width:78px; align:left; overflow:hidden;\">");

            // Builder.Append("<select id=\"ddlUNSDIUSIndicator\" class=\"confg_frm_inp_bx_txt_dd\" style=\"display:none;\">");
            // Builder.Append("<option value=\"-1\">Select</option>");

            // foreach (KeyValuePair<string, string> TargetIndicator in DictTargetIndicator)
            // {
            //     Builder.Append("<option value=\"" + TargetIndicator.Key + "\" title=\"" + TargetIndicator.Value + "\">" + this.Get_TrimmedInnerHTML(TargetIndicator.Value, 75) + "</option>");
            // }

            // Builder.Append("</select>");
            // Builder.Append("</td>");

            // Builder.Append("<td style=\"width:78px; align:left; overflow:hidden;\">");
            // //Builder.Append("<b id=\"lang_Unit_UNSD\"></b>");

            // Builder.Append("<select id=\"ddlUNSDIUSUnit\" class=\"confg_frm_inp_bx_txt_dd\" style=\"display:none;\">");
            // Builder.Append("<option value=\"-1\">Select</option>");

            // foreach (KeyValuePair<string, string> TargetUnit in DictTargetUnit)
            // {
            //     Builder.Append("<option value=\"" + TargetUnit.Key + "\" title=\"" + TargetUnit.Value + "\">" + this.Get_TrimmedInnerHTML(TargetUnit.Value, 75) + "</option>");
            // }

            // Builder.Append("</select>");
            // Builder.Append("</td>");

            // Builder.Append("<td style=\"width:78px; align:left; overflow:hidden;\">");
            // //Builder.Append("<b id=\"lang_Age\"></b>");

            // Builder.Append("<select id=\"ddlUNSDIUSAge\" class=\"confg_frm_inp_bx_txt_dd\" style=\"display:none;\">");
            // Builder.Append("<option value=\"-1\">Select</option>");

            // foreach (KeyValuePair<string, string> TargetAge in DictTargetAge)
            // {
            //     Builder.Append("<option value=\"" + TargetAge.Key + "\" title=\"" + TargetAge.Value + "\">" + this.Get_TrimmedInnerHTML(TargetAge.Value, 75) + "</option>");
            // }

            // Builder.Append("</select>");
            // Builder.Append("</td>");

            // Builder.Append("<td style=\"width:78px; align:left; overflow:hidden;\">");
            // //Builder.Append("<b id=\"lang_Sex\"></b>");

            // Builder.Append("<select id=\"ddlUNSDIUSSex\" class=\"confg_frm_inp_bx_txt_dd\" style=\"display:none;\">");
            // Builder.Append("<option value=\"-1\">Select</option>");

            // foreach (KeyValuePair<string, string> TargetSex in DictTargetSex)
            // {
            //     Builder.Append("<option value=\"" + TargetSex.Key + "\" title=\"" + TargetSex.Value + "\">" + this.Get_TrimmedInnerHTML(TargetSex.Value, 75) + "</option>");
            // }

            // Builder.Append("</select>");
            // Builder.Append("</td>");

            // Builder.Append("<td style=\"width:78px; align:left; overflow:hidden;\">");
            // //Builder.Append("<b id=\"lang_Location\"></b>");

            // Builder.Append("<select id=\"ddlUNSDIUSLocation\" class=\"confg_frm_inp_bx_txt_dd\" style=\"display:none;\">");
            // Builder.Append("<option value=\"-1\">Select</option>");

            // foreach (KeyValuePair<string, string> TargetLocation in DictTargetLocation)
            // {
            //     Builder.Append("<option value=\"" + TargetLocation.Key + "\" title=\"" + TargetLocation.Value + "\">" + this.Get_TrimmedInnerHTML(TargetLocation.Value, 75) + "</option>");
            // }

            // Builder.Append("</select>");
            // Builder.Append("</td>");

            // Builder.Append("<td style=\"width:78px; align:left; overflow:hidden;\">");
            // //Builder.Append("<b id=\"lang_Frequency\"></b>");

            // Builder.Append("<select id=\"ddlUNSDIUSFrequency\" class=\"confg_frm_inp_bx_txt_dd\" style=\"display:none;\">");
            // Builder.Append("<option value=\"-1\">Select</option>");

            // foreach (KeyValuePair<string, string> TargetFrequency in DictTargetFrequency)
            // {
            //     Builder.Append("<option value=\"" + TargetFrequency.Key + "\" title=\"" + TargetFrequency.Value + "\">" + this.Get_TrimmedInnerHTML(TargetFrequency.Value, 75) + "</option>");
            // }

            // Builder.Append("</select>");
            // Builder.Append("</td>");

            // Builder.Append("<td style=\"width:78px; align:left; overflow:hidden;\">");
            // //Builder.Append("<b id=\"lang_SourceType\"></b>");

            // Builder.Append("<select id=\"ddlUNSDIUSSourceType\" class=\"confg_frm_inp_bx_txt_dd\" style=\"display:none;\">");
            // Builder.Append("<option value=\"-1\">Select</option>");

            // foreach (KeyValuePair<string, string> TargetSourceType in DictTargetSourceType)
            // {
            //     Builder.Append("<option value=\"" + TargetSourceType.Key + "\" title=\"" + TargetSourceType.Value + "\">" + this.Get_TrimmedInnerHTML(TargetSourceType.Value, 75) + "</option>");
            // }

            // Builder.Append("</select>");
            // Builder.Append("</td>");

            // Builder.Append("<td style=\"width:78px; align:left; overflow:hidden;\">");
            // //Builder.Append("<b id=\"lang_Nature\"></b>");

            // Builder.Append("<select id=\"ddlUNSDIUSNature\" class=\"confg_frm_inp_bx_txt_dd\" style=\"display:none;\">");
            // Builder.Append("<option value=\"-1\">Select</option>");

            // foreach (KeyValuePair<string, string> TargetNature in DictTargetNature)
            // {
            //     Builder.Append("<option value=\"" + TargetNature.Key + "\" title=\"" + TargetNature.Value + "\">" + this.Get_TrimmedInnerHTML(TargetNature.Value, 75) + "</option>");
            // }

            // Builder.Append("</select>");
            // Builder.Append("</td>");

            // Builder.Append("<td style=\"width:78px; align:left; overflow:hidden;\">");
            // //Builder.Append("<b id=\"lang_UnitMultiplier\"></b>");

            // Builder.Append("<select id=\"ddlUNSDIUSUnitMult\" class=\"confg_frm_inp_bx_txt_dd\" style=\"display:none;\">");
            // Builder.Append("<option value=\"-1\">Select</option>");

            // foreach (KeyValuePair<string, string> TargetUnitMult in DictTargetUnitMult)
            // {
            //     Builder.Append("<option value=\"" + TargetUnitMult.Key + "\" title=\"" + TargetUnitMult.Value + "\">" + this.Get_TrimmedInnerHTML(TargetUnitMult.Value, 75) + "</option>");
            // }

            // Builder.Append("</select>");
            // Builder.Append("</td>");

            //// Builder.Append("<td style=\"width:33px; align:left; overflow:hidden;\"></td>");

            // Builder.Append("</tr>");

            //#endregion DIV
            // //till here

            // Builder.Append("<col width=\"110\"/>");
            // Builder.Append("<col width=\"60\"/>");
            // Builder.Append("<col width=\"60\"/>");
            // Builder.Append("<col width=\"23\"/>");
            // Builder.Append("<col width=\"78\"/>");
            // Builder.Append("<col width=\"78\"/>");
            // Builder.Append("<col width=\"78\"/>");
            // Builder.Append("<col width=\"78\"/>");
            // Builder.Append("<col width=\"78\"/>");
            // Builder.Append("<col width=\"78\"/>");
            // Builder.Append("<col width=\"78\"/>");
            // Builder.Append("<col width=\"78\"/>");

            // foreach (string IUSGId in DictSourceIUS.Keys)
            // {
            //     //check here
            //     IndicatorGId = IUSGId.Split(new string[] { "@__@" }, StringSplitOptions.None)[0];
            //     UnitGId = IUSGId.Split(new string[] { "@__@" }, StringSplitOptions.None)[1];
            //     SubgroupValGId = IUSGId.Split(new string[] { "@__@" }, StringSplitOptions.None)[2];
            //     this.Get_SubgroupBreakup(SubgroupValGId, ref AgeGId, ref SexGId, ref LocationGId, SourceCodelistStructure, SelectedAgeCodelist, SelectedSexCodelist, SelectedLocationCodelist);
            //     Indicator = DictSourceIUS[IUSGId].ToString().Split(new string[] { "@__@" }, StringSplitOptions.None)[0];
            //     Unit = DictSourceIUS[IUSGId].ToString().Split(new string[] { "@__@" }, StringSplitOptions.None)[1];
            //     SubgroupVal = DictSourceIUS[IUSGId].ToString().Split(new string[] { "@__@" }, StringSplitOptions.None)[2];

            //     OriginalRowState = string.Empty;
            //     if (DictMappingIUS.ContainsKey(IUSGId))
            //     {
            //         OriginalRowColor = "rgb(221, 255, 221);";
            //         OriginalRowStatus = "mapped";
            //     }
            //     else
            //     {
            //         OriginalRowColor = "rgb(255, 221, 221);";
            //         OriginalRowStatus = "unmapped";
            //     }

            //     Builder.Append("<tr id=\"rowIUS_" + IUSGId + "\" style=\"background-color:" + OriginalRowColor + ");\" status=\"" + OriginalRowStatus + "\">");

            //     Builder.Append("<td style=\"width:110px; align:left; overflow:hidden;\">");
            //     Builder.Append("<span id=\"spanDevInfoIUSIndicatorName_" + IndicatorGId + "\" value=\"" + Indicator + "\" style=\"font-weight:bold;\" onmouseover=\"ShowCallout('divCallout', '" + Indicator + "', event);\" onmouseout=\"HideCallout('divCallout');\">" + this.Get_TrimmedInnerHTML(Indicator, 25) + "</span>");// onmouseover=\"ShowCallout('divCallout', '" + Indicator + "', event);\" onmouseout=\"HideCallout('divCallout');
            //     Builder.Append("</td>");

            //     Builder.Append("<td style=\"width:60px; align:left; overflow:hidden;\">");
            //     Builder.Append("<span id=\"spanDevInfoIUSUnitName_" + UnitGId + "\" value=\"" + Unit + "\"  onmouseover=\"ShowCallout('divCallout', '" + Unit + "', event);\" onmouseout=\"HideCallout('divCallout');\">" + this.Get_TrimmedInnerHTML(Unit, 25) + "</span>");// onmouseover=\"ShowCallout('divCallout', '" + Unit + "', event);\" onmouseout=\"HideCallout('divCallout');
            //     Builder.Append("</td>");

            //     Builder.Append("<td style=\"width:60px; align:left; overflow:hidden;\">");
            //     Builder.Append("<span id=\"spanDevInfoIUSSubgroupName_" + SubgroupValGId + "\" value=\"" + SubgroupVal + "\" onmouseover=\"ShowCallout('divCallout', '" + SubgroupVal + "', event);\" onmouseout=\"HideCallout('divCallout');\">" + this.Get_TrimmedInnerHTML(SubgroupVal, 25) + "</span>");//onmouseover=\"ShowCallout('divCallout', '" + SubgroupVal + "', event);\" onmouseout=\"HideCallout('divCallout');
            //     Builder.Append("</td>");

            //     Builder.Append("<td style=\"width:23px; align:left; overflow:hidden;\">");

            //     if (DictMappingIUS.ContainsKey(IUSGId))
            //     {
            //         Builder.Append("<input type=\"checkbox\" id=\"chkIsMapped_" + IUSGId + "\" checked=\"checked\" value=\"" + IUSGId + "\" onchange=\"HandleStateChange('rowIUS_" + IUSGId + "', '[**[ORIGINAL_ROW_STATE]**]', '" + OriginalRowColor + "', '" + OriginalRowStatus + "');\"/>");
            //         OriginalRowState += "true" + Constants.Delimiters.ParamDelimiter;
            //     }
            //     else
            //     {
            //         Builder.Append("<input type=\"checkbox\" id=\"chkIsMapped_" + IUSGId + "\" value=\"" + IUSGId + "\" onchange=\"HandleStateChange('rowIUS_" + IUSGId + "', '[**[ORIGINAL_ROW_STATE]**]', '" + OriginalRowColor + "', '" + OriginalRowStatus + "');\"/>");
            //         OriginalRowState += "false" + Constants.Delimiters.ParamDelimiter;
            //     }

            //     Builder.Append("</td>");

            //     //Builder.Append("<td style=\"display:none;\">");
            //     //Builder.Append("<span style=\"display:none;\">" + DictMappingIndicator[IndicatorGId].ToString() + "</span>");
            //     //Builder.Append("</td>");
            //     //new 
            //     Builder.Append("<td style=\"width:78px; align:left;overflow:hidden;\">");// 
            //     Builder.Append("<select id=\"ddlUNSDIUSIndicator_" + IUSGId + "\" style=\"width:75px\" class=\"confg_frm_inp_bx_txt_dd\" onchange=\"HandleStateChange('rowIUS_" + IUSGId + "', '[**[ORIGINAL_ROW_STATE]**]', '" + OriginalRowColor + "', '" + OriginalRowStatus + "');\">");
            //     Builder.Append("<option value=\"-1\" id=\"SelectUNSDIUSIndicator_" + IUSGId + "\"></option>");

            //     if (DictMappingIUS.ContainsKey(IUSGId))
            //     {
            //         OriginalRowState += DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[0] + Constants.Delimiters.ParamDelimiter;
            //         Builder.Append("<option value=\"" + DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[0] + "\" selected=\"selected\" title=\"" + DictTargetIndicator[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[0]] + "\">" + this.Get_TrimmedInnerHTML(DictTargetIndicator[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[0]], 75) + "</option>");
            //     }
            //     else if (DictMappingIndicator.ContainsKey(IndicatorGId))
            //     {
            //         OriginalRowState += DictMappingIndicator[IndicatorGId].ToString() + Constants.Delimiters.ParamDelimiter;
            //         Builder.Append("<option value=\"" + DictMappingIndicator[IndicatorGId]+ "\" selected=\"selected\" title=\"" + DictTargetIndicator[DictMappingIndicator[IndicatorGId]] + "\">" + this.Get_TrimmedInnerHTML(DictTargetIndicator[DictMappingIndicator[IndicatorGId]], 75) + "</option>");
            //     }
            //     else
            //     {
            //         OriginalRowState += "-1" + Constants.Delimiters.ParamDelimiter;
            //     }

            //     Builder.Append("</select>");
            //     Builder.Append("</td>");

            //     //new 
            //     Builder.Append("<td style=\"width:78px; align:left;overflow:hidden;\">");// 
            //     Builder.Append("<select id=\"ddlUNSDIUSUnit_" + IUSGId + "\" style=\"width:75px\" class=\"confg_frm_inp_bx_txt_dd\" onchange=\"HandleStateChange('rowIUS_" + IUSGId + "', '[**[ORIGINAL_ROW_STATE]**]', '" + OriginalRowColor + "', '" + OriginalRowStatus + "');\">");
            //     Builder.Append("<option value=\"-1\" id=\"SelectUNSDIUSUnit_" + IUSGId + "\"></option>");

            //     if (DictMappingIUS.ContainsKey(IUSGId))
            //     {
            //         OriginalRowState += DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[1] + Constants.Delimiters.ParamDelimiter;
            //         Builder.Append("<option value=\"" + DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[1] + "\" selected=\"selected\" title=\"" + DictTargetUnit[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[1]] + "\">" + this.Get_TrimmedInnerHTML(DictTargetUnit[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[1]], 75) + "</option>");
            //     }
            //     else if (DictMappingUnit.ContainsKey(UnitGId))
            //     {
            //         OriginalRowState += DictMappingUnit[UnitGId].ToString() + Constants.Delimiters.ParamDelimiter;
            //         Builder.Append("<option value=\"" + DictMappingUnit[UnitGId] + "\" selected=\"selected\" title=\"" + DictTargetUnit[DictMappingUnit[UnitGId]] + "\">" + this.Get_TrimmedInnerHTML(DictTargetUnit[DictMappingUnit[UnitGId]], 75) + "</option>");
            //     }
            //     else
            //     {
            //         OriginalRowState += "-1" + Constants.Delimiters.ParamDelimiter;
            //     }

            //     Builder.Append("</select>");
            //     Builder.Append("</td>");


            //     Builder.Append("<td style=\"width:78px; align:left;overflow:hidden; \">");//
            //     Builder.Append("<select id=\"ddlUNSDIUSAge_" + IUSGId + "\" style=\"width:75px\" class=\"confg_frm_inp_bx_txt_dd\" onchange=\"HandleStateChange('rowIUS_" + IUSGId + "', '[**[ORIGINAL_ROW_STATE]**]', '" + OriginalRowColor + "', '" + OriginalRowStatus + "');\">");
            //     Builder.Append("<option value=\"-1\" id=\"SelectUNSDIUSAge_" + IUSGId + "\"></option>");

            //     if (DictMappingIUS.ContainsKey(IUSGId))
            //     {
            //         OriginalRowState += DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[2] + Constants.Delimiters.ParamDelimiter;
            //         Builder.Append("<option value=\"" + DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[2] + "\" selected=\"selected\" title=\"" + DictTargetAge[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[2]] + "\">" + this.Get_TrimmedInnerHTML(DictTargetAge[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[2]], 75) + "</option>");
            //     }
            //     else if (DictMappingAge.ContainsKey(AgeGId))
            //     {
            //         OriginalRowState += DictMappingAge[AgeGId].ToString() + Constants.Delimiters.ParamDelimiter;
            //         Builder.Append("<option value=\"" + DictMappingAge[AgeGId] + "\" selected=\"selected\" title=\"" + DictTargetAge[DictMappingAge[AgeGId]] + "\">" + this.Get_TrimmedInnerHTML(DictTargetAge[DictMappingAge[AgeGId]], 75) + "</option>");
            //     }
            //     else if (DictTargetAge.ContainsKey(Global.registryMappingAgeDefaultValue))
            //     {
            //         OriginalRowState += Global.registryMappingAgeDefaultValue + Constants.Delimiters.ParamDelimiter;
            //         Builder.Append("<option value=\"" + Global.registryMappingAgeDefaultValue + "\" selected=\"selected\" title=\"" + DictTargetAge[Global.registryMappingAgeDefaultValue] + "\">" + this.Get_TrimmedInnerHTML(DictTargetAge[Global.registryMappingAgeDefaultValue], 75) + "</option>");
            //     }
            //     else
            //     {
            //         OriginalRowState += "-1" + Constants.Delimiters.ParamDelimiter;
            //     }

            //     Builder.Append("</select>");
            //     Builder.Append("</td>");

            //     Builder.Append("<td style=\"width:78px; align:left;overflow:hidden; \">");//
            //     Builder.Append("<select id=\"ddlUNSDIUSSex_" + IUSGId + "\" style=\"width:75px\" class=\"confg_frm_inp_bx_txt_dd\" onchange=\"HandleStateChange('rowIUS_" + IUSGId + "', '[**[ORIGINAL_ROW_STATE]**]', '" + OriginalRowColor + "', '" + OriginalRowStatus + "');\">");
            //     Builder.Append("<option value=\"-1\" id=\"SelectUNSDIUSSex_" + IUSGId + "\"></option>");

            //     if (DictMappingIUS.ContainsKey(IUSGId))
            //     {
            //         OriginalRowState += DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[3] + Constants.Delimiters.ParamDelimiter;
            //         Builder.Append("<option value=\"" + DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[3] + "\" selected=\"selected\" title=\"" + DictTargetSex[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[3]] + "\">" + this.Get_TrimmedInnerHTML(DictTargetSex[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[3]], 75) + "</option>");
            //     }
            //     else if (DictMappingSex.ContainsKey(SexGId))
            //     {
            //         OriginalRowState += DictMappingSex[SexGId].ToString() + Constants.Delimiters.ParamDelimiter;
            //         Builder.Append("<option value=\"" + DictMappingSex[SexGId] + "\" selected=\"selected\" title=\"" + DictTargetSex[DictMappingSex[SexGId]] + "\">" + this.Get_TrimmedInnerHTML(DictTargetSex[DictMappingSex[SexGId]], 75) + "</option>");
            //     }
            //     else if (DictTargetSex.ContainsKey(Global.registryMappingSexDefaultValue))
            //     {
            //         OriginalRowState += Global.registryMappingSexDefaultValue + Constants.Delimiters.ParamDelimiter;
            //         Builder.Append("<option value=\"" + Global.registryMappingSexDefaultValue + "\" selected=\"selected\" title=\"" + DictTargetSex[Global.registryMappingSexDefaultValue] + "\">" + this.Get_TrimmedInnerHTML(DictTargetSex[Global.registryMappingSexDefaultValue], 75) + "</option>");
            //     }
            //     else
            //     {
            //         OriginalRowState += "-1" + Constants.Delimiters.ParamDelimiter;
            //     }

            //     Builder.Append("</select>");
            //     Builder.Append("</td>");

            //     Builder.Append("<td style=\"width:78px; align:left; overflow:hidden;\">");//
            //     Builder.Append("<select id=\"ddlUNSDIUSLocation_" + IUSGId + "\" style=\"width:75px\" class=\"confg_frm_inp_bx_txt_dd\" onchange=\"HandleStateChange('rowIUS_" + IUSGId + "', '[**[ORIGINAL_ROW_STATE]**]', '" + OriginalRowColor + "', '" + OriginalRowStatus + "');\">");
            //     Builder.Append("<option value=\"-1\" id=\"SelectUNSDIUSLocation_" + IUSGId + "\"></option>");

            //     if (DictMappingIUS.ContainsKey(IUSGId))
            //     {
            //         OriginalRowState += DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[4] + Constants.Delimiters.ParamDelimiter;
            //         Builder.Append("<option value=\"" + DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[4] + "\" selected=\"selected\" title=\"" + DictTargetLocation[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[4]] + "\">" + this.Get_TrimmedInnerHTML(DictTargetLocation[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[4]], 75) + "</option>");
            //     }
            //     else if (DictMappingLocation.ContainsKey(LocationGId))
            //     {
            //         OriginalRowState += DictMappingLocation[LocationGId].ToString() + Constants.Delimiters.ParamDelimiter;
            //         Builder.Append("<option value=\"" + DictMappingLocation[LocationGId] + "\" selected=\"selected\" title=\"" + DictTargetLocation[DictMappingLocation[LocationGId]] + "\">" + this.Get_TrimmedInnerHTML(DictTargetLocation[DictMappingLocation[LocationGId]], 75) + "</option>");
            //     }
            //     else if (DictTargetLocation.ContainsKey(Global.registryMappingLocationDefaultValue))
            //     {
            //         OriginalRowState += Global.registryMappingLocationDefaultValue + Constants.Delimiters.ParamDelimiter;
            //         Builder.Append("<option value=\"" + Global.registryMappingLocationDefaultValue + "\" selected=\"selected\" title=\"" + DictTargetLocation[Global.registryMappingLocationDefaultValue] + "\">" + this.Get_TrimmedInnerHTML(DictTargetLocation[Global.registryMappingLocationDefaultValue], 75) + "</option>");
            //     }
            //     else
            //     {
            //         OriginalRowState += "-1" + Constants.Delimiters.ParamDelimiter;
            //     }

            //     Builder.Append("</select>");
            //     Builder.Append("</td>");

            //     Builder.Append("<td style=\"width:78px; align:left; overflow:hidden;\">");//
            //     Builder.Append("<select id=\"ddlUNSDIUSFrequency_" + IUSGId + "\" style=\"width:75px\" class=\"confg_frm_inp_bx_txt_dd\" onchange=\"HandleStateChange('rowIUS_" + IUSGId + "', '[**[ORIGINAL_ROW_STATE]**]', '" + OriginalRowColor + "', '" + OriginalRowStatus + "');\">");
            //     Builder.Append("<option value=\"-1\" id=\"SelectUNSDIUSFrequency_" + IUSGId + "\"></option>");

            //     if (DictMappingIUS.ContainsKey(IUSGId))
            //     {
            //         OriginalRowState += DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[5] + Constants.Delimiters.ParamDelimiter;
            //         Builder.Append("<option value=\"" + DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[5] + "\" selected=\"selected\" title=\"" + DictTargetFrequency[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[5]] + "\">" + this.Get_TrimmedInnerHTML(DictTargetFrequency[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[5]], 75) + "</option>");
            //     }
            //     else if (DictTargetFrequency.ContainsKey(Global.registryMappingFrequencyDefaultValue))
            //     {
            //         OriginalRowState += Global.registryMappingFrequencyDefaultValue + Constants.Delimiters.ParamDelimiter;
            //         Builder.Append("<option value=\"" + Global.registryMappingFrequencyDefaultValue + "\" selected=\"selected\" title=\"" + DictTargetFrequency[Global.registryMappingFrequencyDefaultValue] + "\">" + this.Get_TrimmedInnerHTML(DictTargetFrequency[Global.registryMappingFrequencyDefaultValue], 75) + "</option>");
            //     }
            //     else
            //     {
            //         OriginalRowState += "-1" + Constants.Delimiters.ParamDelimiter;
            //     }

            //     Builder.Append("</select>");
            //     Builder.Append("</td>");

            //     Builder.Append("<td style=\"width:78px; align:left; overflow:hidden;\">");//
            //     Builder.Append("<select id=\"ddlUNSDIUSSourceType_" + IUSGId + "\" style=\"width:75px\" class=\"confg_frm_inp_bx_txt_dd\" onchange=\"HandleStateChange('rowIUS_" + IUSGId + "', '[**[ORIGINAL_ROW_STATE]**]', '" + OriginalRowColor + "', '" + OriginalRowStatus + "');\">");
            //     Builder.Append("<option value=\"-1\" id=\"SelectUNSDIUSSourceType_" + IUSGId + "\"></option>");

            //     if (DictMappingIUS.ContainsKey(IUSGId))
            //     {
            //         OriginalRowState += DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[6] + Constants.Delimiters.ParamDelimiter;
            //         Builder.Append("<option value=\"" + DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[6] + "\" selected=\"selected\" title=\"" + DictTargetSourceType[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[6]] + "\">" + this.Get_TrimmedInnerHTML(DictTargetSourceType[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[6]], 75) + "</option>");
            //     }
            //     else if (DictTargetSourceType.ContainsKey(Global.registryMappingSourceTypeDefaultValue))
            //     {
            //         OriginalRowState += Global.registryMappingSourceTypeDefaultValue + Constants.Delimiters.ParamDelimiter;
            //         Builder.Append("<option value=\"" + Global.registryMappingSourceTypeDefaultValue + "\" selected=\"selected\" title=\"" + DictTargetSourceType[Global.registryMappingSourceTypeDefaultValue] + "\">" + this.Get_TrimmedInnerHTML(DictTargetSourceType[Global.registryMappingSourceTypeDefaultValue], 75) + "</option>");
            //     }
            //     else
            //     {
            //         OriginalRowState += "-1" + Constants.Delimiters.ParamDelimiter;
            //     }

            //     Builder.Append("</select>");
            //     Builder.Append("</td>");

            //     Builder.Append("<td style=\"width:78px; align:left; overflow:hidden;\">");//
            //     Builder.Append("<select id=\"ddlUNSDIUSNature_" + IUSGId + "\" style=\"width:75px\" class=\"confg_frm_inp_bx_txt_dd\" onchange=\"HandleStateChange('rowIUS_" + IUSGId + "', '[**[ORIGINAL_ROW_STATE]**]', '" + OriginalRowColor + "', '" + OriginalRowStatus + "');\">");
            //     Builder.Append("<option value=\"-1\" id=\"SelectUNSDIUSNature_" + IUSGId + "\"></option>");

            //     if (DictMappingIUS.ContainsKey(IUSGId))
            //     {
            //         OriginalRowState += DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[7] + Constants.Delimiters.ParamDelimiter;
            //         Builder.Append("<option value=\"" + DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[7] + "\" selected=\"selected\" title=\"" + DictTargetNature[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[7]] + "\">" + this.Get_TrimmedInnerHTML(DictTargetNature[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[7]], 75) + "</option>");
            //     }
            //     else if (DictTargetNature.ContainsKey(Global.registryMappingNatureDefaultValue))
            //     {
            //         OriginalRowState += Global.registryMappingNatureDefaultValue + Constants.Delimiters.ParamDelimiter;
            //         Builder.Append("<option value=\"" + Global.registryMappingNatureDefaultValue + "\" selected=\"selected\" title=\"" + DictTargetNature[Global.registryMappingNatureDefaultValue] + "\">" + this.Get_TrimmedInnerHTML(DictTargetNature[Global.registryMappingNatureDefaultValue], 75) + "</option>");
            //     }
            //     else
            //     {
            //         OriginalRowState += "-1" + Constants.Delimiters.ParamDelimiter;
            //     }

            //     Builder.Append("</select>");
            //     Builder.Append("</td>");

            //     Builder.Append("<td style=\"width:78px; align:left;overflow:hidden; \">");//
            //     Builder.Append("<select id=\"ddlUNSDIUSUnitMult_" + IUSGId + "\" style=\"width:75px\" class=\"confg_frm_inp_bx_txt_dd\" onchange=\"HandleStateChange('rowIUS_" + IUSGId + "', '[**[ORIGINAL_ROW_STATE]**]', '" + OriginalRowColor + "', '" + OriginalRowStatus + "');\">");
            //     Builder.Append("<option value=\"-1\" id=\"SelectUNSDIUSUnitMult_" + IUSGId + "\"></option>");

            //     if (DictMappingIUS.ContainsKey(IUSGId))
            //     {
            //         OriginalRowState += DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[8] + Constants.Delimiters.ParamDelimiter;
            //         Builder.Append("<option value=\"" + DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[8] + "\" selected=\"selected\" title=\"" + DictTargetUnitMult[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[8]] + "\">" + this.Get_TrimmedInnerHTML(DictTargetUnitMult[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[8]], 75) + "</option>");
            //     }
            //     else if (DictTargetUnitMult.ContainsKey(Global.registryMappingUnitMultDefaultValue))
            //     {
            //         OriginalRowState += Global.registryMappingUnitMultDefaultValue + Constants.Delimiters.ParamDelimiter;
            //         Builder.Append("<option value=\"" + Global.registryMappingUnitMultDefaultValue + "\" selected=\"selected\" title=\"" + DictTargetUnitMult[Global.registryMappingUnitMultDefaultValue] + "\">" + this.Get_TrimmedInnerHTML(DictTargetUnitMult[Global.registryMappingUnitMultDefaultValue], 75) + "</option>");
            //     }
            //     else
            //     {
            //         OriginalRowState += "-1" + Constants.Delimiters.ParamDelimiter;
            //     }

            //     Builder.Append("</select>");
            //     Builder.Append("</td>");

            //     if (!string.IsNullOrEmpty(OriginalRowState))
            //     {
            //         OriginalRowState = OriginalRowState.Substring(0, OriginalRowState.Length - Constants.Delimiters.ParamDelimiter.Length);
            //         Builder = Builder.Replace("[**[ORIGINAL_ROW_STATE]**]", OriginalRowState);
            //     }

            //     Builder.Append("</tr>");
            // }

            // Builder.Append("</table>");

            #endregion Start2 Nov 7
            //Builder.Append("</div>");
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
        }

        RetVal = Builder.ToString();
        return RetVal;
    }

    private bool GenerateIUSMappingXmlArtefact(string DbNId, string IUSMappingData)
    {
        bool RetVal;
        string IUSMappingFileNameWPath;
        XmlDocument MappingXml;
        XmlDeclaration Declaration;
        XmlElement Root, Mapping;
        XmlAttribute Attribute;

        RetVal = false;
        IUSMappingFileNameWPath = string.Empty;
        MappingXml = new XmlDocument();

        try
        {
            IUSMappingFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId + "\\sdmx" + "\\Mapping\\IUSMapping.xml");

            Declaration = MappingXml.CreateXmlDeclaration("1.0", "UTF-8", null);
            Root = MappingXml.CreateElement("Root");

            foreach (string RowMappingData in IUSMappingData.Split(new string[] { Constants.Delimiters.RowDelimiter }, StringSplitOptions.None))
            {
                Mapping = MappingXml.CreateElement("Mapping");

                Attribute = MappingXml.CreateAttribute("IUS");
                Attribute.Value = RowMappingData.Split(new string[] { Constants.Delimiters.ColumnDelimiter }, StringSplitOptions.None)[0];
                Mapping.Attributes.Append(Attribute);

                Attribute = MappingXml.CreateAttribute("Series");
                Attribute.Value = RowMappingData.Split(new string[] { Constants.Delimiters.ColumnDelimiter }, StringSplitOptions.None)[1];
                Mapping.Attributes.Append(Attribute);

                Attribute = MappingXml.CreateAttribute("Unit");
                Attribute.Value = RowMappingData.Split(new string[] { Constants.Delimiters.ColumnDelimiter }, StringSplitOptions.None)[2];
                Mapping.Attributes.Append(Attribute);

                Attribute = MappingXml.CreateAttribute("Age");
                Attribute.Value = RowMappingData.Split(new string[] { Constants.Delimiters.ColumnDelimiter }, StringSplitOptions.None)[3];
                Mapping.Attributes.Append(Attribute);

                Attribute = MappingXml.CreateAttribute("Sex");
                Attribute.Value = RowMappingData.Split(new string[] { Constants.Delimiters.ColumnDelimiter }, StringSplitOptions.None)[4];
                Mapping.Attributes.Append(Attribute);

                Attribute = MappingXml.CreateAttribute("Location");
                Attribute.Value = RowMappingData.Split(new string[] { Constants.Delimiters.ColumnDelimiter }, StringSplitOptions.None)[5];
                Mapping.Attributes.Append(Attribute);

                Attribute = MappingXml.CreateAttribute("Frequency");
                Attribute.Value = RowMappingData.Split(new string[] { Constants.Delimiters.ColumnDelimiter }, StringSplitOptions.None)[6];
                Mapping.Attributes.Append(Attribute);

                Attribute = MappingXml.CreateAttribute("SourceType");
                Attribute.Value = RowMappingData.Split(new string[] { Constants.Delimiters.ColumnDelimiter }, StringSplitOptions.None)[7];
                Mapping.Attributes.Append(Attribute);

                Attribute = MappingXml.CreateAttribute("Nature");
                Attribute.Value = RowMappingData.Split(new string[] { Constants.Delimiters.ColumnDelimiter }, StringSplitOptions.None)[8];
                Mapping.Attributes.Append(Attribute);

                Attribute = MappingXml.CreateAttribute("UnitMult");
                Attribute.Value = RowMappingData.Split(new string[] { Constants.Delimiters.ColumnDelimiter }, StringSplitOptions.None)[9];
                Mapping.Attributes.Append(Attribute);

                Root.AppendChild(Mapping);
            }

            MappingXml.AppendChild(Declaration);
            MappingXml.AppendChild(Root);
            MappingXml.Save(IUSMappingFileNameWPath);

            RetVal = true;
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
        }

        return RetVal;
    }



    private bool IsIndicatorMapped(string CodeId, StructureType CodelistMapping)
    {
        bool RetVal;

        RetVal = false;

        try
        {
            if (CodelistMapping != null && CodelistMapping.Structures != null && CodelistMapping.Structures.StructureSets != null &&
                CodelistMapping.Structures.StructureSets.Count > 0 && CodelistMapping.Structures.StructureSets[0].Items != null &&
                CodelistMapping.Structures.StructureSets[0].Items.Count > 0 &&
                CodelistMapping.Structures.StructureSets[0].Items[0] is SDMXObjectModel.Structure.CodelistMapType &&
                ((SDMXObjectModel.Structure.CodelistMapType)CodelistMapping.Structures.StructureSets[0].Items[0]).Items != null &&
                ((SDMXObjectModel.Structure.CodelistMapType)CodelistMapping.Structures.StructureSets[0].Items[0]).Items.Count > 0)
            {
                foreach (SDMXObjectModel.Structure.ItemAssociationType CodeMap in ((SDMXObjectModel.Structure.CodelistMapType)CodelistMapping.Structures.StructureSets[0].Items[0]).Items)
                {
                    if (CodeId == ((LocalCodeRefType)((LocalCodeReferenceType)CodeMap.Source).Items[0]).id)
                    {
                        RetVal = true;
                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
        }

        return RetVal;
    }



    private string Get_Language_Based_Name_From_Codelist(SDMXObjectModel.Structure.CodelistType Codelist, string GId, string Language)
    {
        string RetVal;

        RetVal = string.Empty;

        try
        {
            foreach (SDMXObjectModel.Structure.ItemType Code in Codelist.Items)
            {
                if (Code.id == GId)
                {
                    RetVal = GetLangSpecificValue_For_Version_2_1(Code.Name, Language);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
        }

        return RetVal;
    }

    #endregion "--IUS Mapping--"

    #region "--Common--"

    private void Create_Category_Schemes_With_Mapping_Information(string DbNId, string AssociatedDbNId, string AgencyId, string Language)
    {
        string OutputFolder;
        DIConnection DIConnection;
        DIQueries DIQueries;
        Dictionary<string, string> DictIndicator, DictIndicatorMapping;

        DIConnection = Global.GetDbConnection(Convert.ToInt32(AssociatedDbNId));
        DIQueries = new DIQueries(DIConnection.DIDataSetDefault(), DIConnection.DILanguageCodeDefault(DIConnection.DIDataSetDefault()));
        OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId.ToString() + "\\sdmx");

        try
        {
            DictIndicator = RegTwoZeroFunctionality.Get_Indicator_GIds(Path.Combine(OutputFolder, "Complete.xml"));
            DictIndicatorMapping = RegTwoZeroFunctionality.Get_Indicator_Mapping_Dict(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId + "\\" + Constants.FolderName.SDMX.Mapping + DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.FileName), Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId.ToString() + "\\" + Constants.FolderName.SDMX.Mapping + "IUSMapping.xml"));

            SDMXUtility.Generate_CategoryScheme(SDMXSchemaType.Two_One, CategorySchemeTypes.ALL, AgencyId, Language, null, Path.Combine(OutputFolder, "Categories"), DictIndicator, DictIndicatorMapping, DIConnection, DIQueries);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {

        }
    }

    private Dictionary<string, string> Sort_Dictionary(Dictionary<string, string> OriginalDict)
    {
        Dictionary<string, string> RetVal;
        List<string> ListIndicatorSortHelper;
        Dictionary<string, string> DictIndicatorSortHelper;

        RetVal = new Dictionary<string, string>();
        ListIndicatorSortHelper = new List<string>();
        DictIndicatorSortHelper = new Dictionary<string, string>();


        try
        {
            if (OriginalDict != null && OriginalDict.Keys.Count > 0)
            {
                foreach (string Id in OriginalDict.Keys)
                {
                    ListIndicatorSortHelper.Add(OriginalDict[Id]);

                    if (DictIndicatorSortHelper.ContainsKey(OriginalDict[Id]))
                    {
                        continue;
                    }
                    else
                    {
                        DictIndicatorSortHelper.Add(OriginalDict[Id], Id);
                    }
                }

                if (ListIndicatorSortHelper != null && ListIndicatorSortHelper.Count > 0)
                {
                    ListIndicatorSortHelper.Sort();

                    foreach (string Name in ListIndicatorSortHelper)
                    {
                        if (RetVal.ContainsKey(DictIndicatorSortHelper[Name]))
                        {
                            continue;
                        }
                        else
                        {
                            RetVal.Add(DictIndicatorSortHelper[Name], Name);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
        }

        return RetVal;
    }

    private string Get_TrimmedInnerHTML(string InnerHtml, int TrimLength)
    {
        string RetVal;

        RetVal = string.Empty;

        if (InnerHtml.Length > TrimLength)
        {
            RetVal = InnerHtml.Substring(0, TrimLength) + "..";
        }
        else
        {
            RetVal = InnerHtml;
        }

        return RetVal;
    }

    #endregion "--Common--"

    #endregion "--Private--"

    #region "--Public--"

    public void Get_SubgroupBreakup(string SubgroupValGId, ref string AgeGId, ref string SexGId, ref string LocationGId, StructureType SourceCodelistStructure, string SelectedAgeCodelist, string SelectedSexCodeList, string SelectedLocationCodelist)
    {
        AgeGId = string.Empty;
        SexGId = string.Empty;
        LocationGId = string.Empty;

        if (SourceCodelistStructure != null && SourceCodelistStructure.Structures != null && SourceCodelistStructure.Structures.Codelists != null &&
            SourceCodelistStructure.Structures.Codelists.Count > 0)
        {
            foreach (SDMXObjectModel.Structure.CodelistType Codelist in SourceCodelistStructure.Structures.Codelists)
            {
                if (Codelist.id == DevInfo.Lib.DI_LibSDMX.Constants.CodeList.SubgroupVal.Id)
                {
                    foreach (SDMXObjectModel.Structure.ItemType Code in Codelist.Items)
                    {
                        if (Code.id == SubgroupValGId)
                        {
                            if (Code.Annotations != null && Code.Annotations.Count > 0)
                            {
                                foreach (AnnotationType Annotation in Code.Annotations)
                                {
                                    if (Annotation.AnnotationTitle == DevInfo.Lib.DI_LibSDMX.Constants.Annotations.Breakup)
                                    {
                                        foreach (string SubgroupSplits in Annotation.AnnotationText[0].Value.Split(','))
                                        {
                                            if (SubgroupSplits.Trim().Contains(SelectedAgeCodelist) && string.IsNullOrEmpty(SelectedAgeCodelist) == false)//SubgroupSplits.Trim().StartsWith(DevInfo.Lib.DI_LibSDMX.Constants.CodelistPrefix + Constants.UNSD.CodeList.Age.Gid)
                                            {
                                                AgeGId = SubgroupSplits.Trim().Replace(SelectedAgeCodelist + DevInfo.Lib.DI_LibSDMX.Constants.EqualsTo, string.Empty);
                                            }
                                            else if (SubgroupSplits.Trim().Contains(SelectedSexCodeList) && string.IsNullOrEmpty(SelectedSexCodeList) == false)//SubgroupSplits.Trim().StartsWith(DevInfo.Lib.DI_LibSDMX.Constants.CodelistPrefix + Constants.UNSD.CodeList.Sex.Gid)
                                            {
                                                SexGId = SubgroupSplits.Trim().Replace(SelectedSexCodeList + DevInfo.Lib.DI_LibSDMX.Constants.EqualsTo, string.Empty);
                                            }
                                            else if (SubgroupSplits.Trim().Contains(SelectedLocationCodelist) && string.IsNullOrEmpty(SelectedLocationCodelist) == false)//DevInfo.Lib.DI_LibSDMX.Constants.CodelistPrefix + Constants.UNSD.CodeList.Location.Gid
                                            {
                                                LocationGId = SubgroupSplits.Trim().Replace(SelectedLocationCodelist + DevInfo.Lib.DI_LibSDMX.Constants.EqualsTo, string.Empty);//DevInfo.Lib.DI_LibSDMX.Constants.CodelistPrefix + Constants.UNSD.CodeList.Location.Gid

                                            }
                                        }
                                    }
                                }
                            }

                            break;
                        }
                    }

                    break;
                }
            }
        }
    }

    public Dictionary<string, string> Get_DictTargetCodelist(string Language, string CodelistId, SDMXApi_2_0.Message.StructureType Structure)
    {
        Dictionary<string, string> RetVal;
        string CodeId, CodeDescription;
        SDMXApi_2_0.Structure.CodeListType TargetCodelist;

        RetVal = new Dictionary<string, string>();
        CodeId = string.Empty;
        CodeDescription = string.Empty;
        TargetCodelist = null;

        try
        {
            if (Structure != null && Structure.CodeLists != null && Structure.CodeLists.Count > 0)
            {
                foreach (SDMXApi_2_0.Structure.CodeListType Codelist in Structure.CodeLists)
                {
                    if (Codelist.id == CodelistId)
                    {
                        TargetCodelist = Codelist;
                        break;
                    }
                }

                if (TargetCodelist != null && TargetCodelist.Code != null && TargetCodelist.Code.Count > 0)
                {
                    foreach (SDMXApi_2_0.Structure.CodeType Code in TargetCodelist.Code)
                    {
                        CodeId = Code.value;
                        CodeDescription = this.GetLangSpecificValueFor_Version_2_0(Code.Description, Language);

                        RetVal.Add(CodeId, CodeDescription);
                    }
                }

                if (RetVal != null && RetVal.Keys.Count > 0)
                {
                    RetVal = this.Sort_Dictionary(RetVal);
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
        }

        return RetVal;
    }

    public Dictionary<string, string> Get_DictMappingCodelist(string CodelistMapId, StructureType Structure)
    {
        Dictionary<string, string> RetVal;
        SDMXObjectModel.Structure.CodelistMapType MapCodelist;

        RetVal = new Dictionary<string, string>();
        MapCodelist = null;

        try
        {
            if (Structure != null && Structure.Structures != null && Structure.Structures.StructureSets != null &&
                Structure.Structures.StructureSets.Count > 0 && Structure.Structures.StructureSets[0].Items != null &&
                Structure.Structures.StructureSets[0].Items.Count > 0)
            {
                foreach (SDMXObjectModel.Structure.NameableType CodelistMap in Structure.Structures.StructureSets[0].Items)
                {
                    if (CodelistMap.id == CodelistMapId && CodelistMap is SDMXObjectModel.Structure.CodelistMapType)
                    {
                        MapCodelist = CodelistMap as SDMXObjectModel.Structure.CodelistMapType;
                        break;
                    }
                }

                if (MapCodelist != null && MapCodelist.Items != null && MapCodelist.Items.Count > 0)
                {
                    foreach (SDMXObjectModel.Structure.ItemAssociationType CodeMap in MapCodelist.Items)
                    {
                        RetVal.Add(((SDMXObjectModel.Common.LocalCodeRefType)CodeMap.Source.Items[0]).id, ((SDMXObjectModel.Common.LocalCodeRefType)CodeMap.Target.Items[0]).id);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
        }

        return RetVal;
    }

    public Dictionary<string, string> Get_DictSourceIUS(string Language, StructureType SourceCodelistStructure, StructureType MappingCodelistStructure)
    {
        Dictionary<string, string> RetVal;
        string IndicatorGId, UnitGId, SubgroupValGId, Indicator, Unit, SubgroupVal;
        SDMXObjectModel.Structure.CodelistType IUSCodelist, IndicatorCodelist, UnitCodelist, SubgroupCodelist;

        RetVal = new Dictionary<string, string>();
        IndicatorGId = string.Empty;
        UnitGId = string.Empty;
        SubgroupValGId = string.Empty;
        Indicator = string.Empty;
        Unit = string.Empty;
        SubgroupVal = string.Empty;

        IUSCodelist = null;
        IndicatorCodelist = null;
        UnitCodelist = null;
        SubgroupCodelist = null;

        try
        {
            if (SourceCodelistStructure != null && SourceCodelistStructure.Structures != null && SourceCodelistStructure.Structures.Codelists != null &&
                SourceCodelistStructure.Structures.Codelists.Count > 0)
            {
                foreach (SDMXObjectModel.Structure.CodelistType Codelist in SourceCodelistStructure.Structures.Codelists)
                {
                    switch (Codelist.id)
                    {
                        case DevInfo.Lib.DI_LibSDMX.Constants.CodeList.IUS.Id:
                            IUSCodelist = Codelist;
                            break;
                        case DevInfo.Lib.DI_LibSDMX.Constants.CodeList.Indicator.Id:
                            IndicatorCodelist = Codelist;
                            break;
                        case DevInfo.Lib.DI_LibSDMX.Constants.CodeList.Unit.Id:
                            UnitCodelist = Codelist;
                            break;
                        case DevInfo.Lib.DI_LibSDMX.Constants.CodeList.SubgroupVal.Id:
                            SubgroupCodelist = Codelist;
                            break;
                        default:
                            break;
                    }
                }
            }

            if (IUSCodelist != null && IUSCodelist.Items != null && IUSCodelist.Items.Count > 0)
            {
                foreach (SDMXObjectModel.Structure.ItemType Code in IUSCodelist.Items)
                {
                    IndicatorGId = Code.id.Split('@')[0];
                    UnitGId = Code.id.Split('@')[1];
                    SubgroupValGId = Code.id.Split('@')[2];

                    if (this.IsIndicatorMapped(IndicatorGId, MappingCodelistStructure))
                    {
                        Indicator = this.Get_Language_Based_Name_From_Codelist(IndicatorCodelist, IndicatorGId, Language);
                        Unit = this.Get_Language_Based_Name_From_Codelist(UnitCodelist, UnitGId, Language);
                        SubgroupVal = this.Get_Language_Based_Name_From_Codelist(SubgroupCodelist, SubgroupValGId, Language);

                        RetVal.Add(IndicatorGId + "@__@" + UnitGId + "@__@" + SubgroupValGId, Indicator + "@__@" + Unit + "@__@" + SubgroupVal);
                    }
                }

                if (RetVal != null && RetVal.Keys.Count > 0)
                {
                    RetVal = this.Sort_Dictionary(RetVal);
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
        }

        return RetVal;
    }

    public Dictionary<string, string> Get_DictMappingIUS(string DbNId)
    {
        Dictionary<string, string> RetVal;
        XmlDocument IUSMapping;
        string IUSMappingFileNameWPath;

        RetVal = new Dictionary<string, string>();
        IUSMapping = new XmlDocument();
        IUSMappingFileNameWPath = string.Empty;

        try
        {
            IUSMappingFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId + "\\sdmx" + "\\Mapping\\IUSMapping.xml");

            if (File.Exists(IUSMappingFileNameWPath))
            {
                IUSMapping.Load(IUSMappingFileNameWPath);

                foreach (XmlNode Mapping in IUSMapping.GetElementsByTagName("Mapping"))
                {
                    RetVal.Add(Mapping.Attributes["IUS"].Value, Mapping.Attributes["Series"].Value + "@__@" + Mapping.Attributes["Unit"].Value + "@__@" + Mapping.Attributes["Age"].Value + "@__@" + Mapping.Attributes["Sex"].Value + "@__@" + Mapping.Attributes["Location"].Value + "@__@" + Mapping.Attributes["Frequency"].Value + "@__@" + Mapping.Attributes["SourceType"].Value + "@__@" + Mapping.Attributes["Nature"].Value + "@__@" + Mapping.Attributes["UnitMult"].Value);
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
        }

        return RetVal;
    }

    /// <summary>
    /// Returning the deserialized objects on the basis of dsds and codelistmappingxml
    /// </summary>
    /// <param name="DbNId">The Database Id which has been selected</param>
    /// <param name="SourceCodelistStructure"></param>
    /// <param name="TargetCodelistStructure"></param>
    /// <param name="MappingCodelistStructure"></param>
    public void Get_Codelist_Source_Target_Mapping_Structure(string DbNId, out StructureType SourceCodelistStructure, out SDMXApi_2_0.Message.StructureType TargetCodelistStructure, out StructureType MappingCodelistStructure)
    {
        string CodelistMappingFileNameWPath;

        CodelistMappingFileNameWPath = string.Empty;

        MappingCodelistStructure = null;

        try
        {
            CodelistMappingFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId + "\\" + Constants.FolderName.SDMX.Mapping + DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.FileName);

            if (File.Exists(CodelistMappingFileNameWPath))
            {
                MappingCodelistStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), CodelistMappingFileNameWPath);
            }

            this.Get_Codelist_Source_Target_Structure(DbNId, out SourceCodelistStructure, out TargetCodelistStructure);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
        }
    }

    /// <summary>
    /// Get the Souce Code List Dictionary for Indicator
    /// </summary>
    /// <param name="languageCode">Language Code</param>
    /// <param name="dbNId">DBNid</param>
    /// <returns></returns>
    public Dictionary<string, string> GetSourceIndicatorCodeListMapping(string languageCode, string dbNId)
    {
        Dictionary<string, string> RetVal = null;

        string AssociatedDbNId = Get_AssociatedDB_NId(dbNId).ToString();
        if (Convert.ToInt32(AssociatedDbNId) < 1)
        {
            AssociatedDbNId = dbNId;
        }
        string SourceCodelistFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + AssociatedDbNId + "\\" + Constants.FolderName.SDMX.sdmx + DevInfo.Lib.DI_LibSDMX.Constants.Complete_XML.FileName);
        StructureType SourceCodelistStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), SourceCodelistFileNameWPath);

        RetVal = this.Get_DictSourceCodelist(languageCode, DevInfo.Lib.DI_LibSDMX.Constants.CodeList.Indicator.Id, SourceCodelistStructure);

        return RetVal;
    }


    /// <summary>
    /// Get the Souce Code List Dictionary
    /// </summary>
    /// <param name="languageCode">Language Code</param>
    /// <param name="dbNId">DBNid</param>
    /// <returns></returns>
    public Dictionary<string, string> GetTargetIndicatorCodeListMapping(string languageCode, string dbNId)
    {
        Dictionary<string, string> RetVal = null;
        string IndicatorCodelistId = string.Empty;
        string TargetCodelistFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + dbNId + "\\" + Constants.FolderName.SDMX.sdmx + DevInfo.Lib.DI_LibSDMX.Constants.Complete_XML.FileName);
        SDMXApi_2_0.Message.StructureType TargetCodelistStructure = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromFile(typeof(SDMXApi_2_0.Message.StructureType), TargetCodelistFileNameWPath);

        foreach (SDMXApi_2_0.Structure.DimensionType Dimensions in TargetCodelistStructure.KeyFamilies[0].Components.Dimension)
        {
            if (Dimensions.conceptRef == Constants.UNSD.Concept.Indicator.Id)
            {
                IndicatorCodelistId = Dimensions.codelist;
            }
        }
        RetVal = this.Get_DictTargetCodelist(languageCode, IndicatorCodelistId, TargetCodelistStructure);


        return RetVal;
    }


    /// <summary>
    /// Get the Souce Code List Dictionary
    /// </summary>
    /// <param name="languageCode">Language Code</param>
    /// <param name="dbNId">DBNid</param>
    /// <returns></returns>
    public Dictionary<string, string> GetIndicatorMappedCodeList(string dbNId)
    {
        Dictionary<string, string> RetVal = null;

        string CodelistMappingFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + dbNId + "\\" + Constants.FolderName.SDMX.Mapping + DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.FileName);
        StructureType MappingCodelistStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), CodelistMappingFileNameWPath);

        RetVal = this.Get_DictMappingCodelist(DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.Indicator.id, MappingCodelistStructure);

        return RetVal;
    }

    /// <summary>
    /// Get the Souce Code List Dictionary for UNIT
    /// </summary>
    /// <param name="languageCode">Language Code</param>
    /// <param name="dbNId">DBNid</param>
    /// <returns></returns>
    public Dictionary<string, string> GetSourceUnitCodeListMapping(string languageCode, string dbNId)
    {
        Dictionary<string, string> RetVal = null;

        string AssociatedDbNId = Get_AssociatedDB_NId(dbNId).ToString();
        string SourceCodelistFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + AssociatedDbNId + "\\" + Constants.FolderName.SDMX.sdmx + DevInfo.Lib.DI_LibSDMX.Constants.Complete_XML.FileName);
        StructureType SourceCodelistStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), SourceCodelistFileNameWPath);

        RetVal = this.Get_DictSourceCodelist(languageCode, DevInfo.Lib.DI_LibSDMX.Constants.CodeList.Unit.Id, SourceCodelistStructure);

        return RetVal;
    }

    /// <summary>
    /// Get the Souce Code List Dictionary
    /// </summary>
    /// <param name="languageCode">Language Code</param>
    /// <param name="dbNId">DBNid</param>
    /// <returns></returns>
    public Dictionary<string, string> GetTargetUnitCodeListMapping(string languageCode, string dbNId)
    {
        Dictionary<string, string> RetVal = null;
        string UnitCodelistId = string.Empty;
        string TargetCodelistFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + dbNId + "\\" + Constants.FolderName.SDMX.sdmx + DevInfo.Lib.DI_LibSDMX.Constants.Complete_XML.FileName);
        SDMXApi_2_0.Message.StructureType TargetCodelistStructure = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromFile(typeof(SDMXApi_2_0.Message.StructureType), TargetCodelistFileNameWPath);
        foreach (SDMXApi_2_0.Structure.DimensionType Dimensions in TargetCodelistStructure.KeyFamilies[0].Components.Dimension)
        {
            if (Dimensions.conceptRef == Constants.UNSD.Concept.Unit.Id)
            {
                UnitCodelistId = Dimensions.codelist;
            }
        }
        RetVal = this.Get_DictTargetCodelist(languageCode, UnitCodelistId, TargetCodelistStructure);


        return RetVal;
    }

    /// <summary>
    /// Get the Souce Code List Dictionary
    /// </summary>
    /// <param name="languageCode">Language Code</param>
    /// <param name="dbNId">DBNid</param>
    /// <returns></returns>
    public Dictionary<string, string> GetUnitMappedCodeList(string dbNId)
    {
        Dictionary<string, string> RetVal = null;

        string CodelistMappingFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + dbNId + "\\" + Constants.FolderName.SDMX.Mapping + DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.FileName);
        StructureType MappingCodelistStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), CodelistMappingFileNameWPath);

        RetVal = this.Get_DictMappingCodelist(DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.Unit.id, MappingCodelistStructure);

        return RetVal;
    }

    /// <summary>
    /// Get the Souce Code List Dictionary for UNIT
    /// </summary>
    /// <param name="languageCode">Language Code</param>
    /// <param name="dbNId">DBNid</param>
    /// <returns></returns>
    public Dictionary<string, string> GetSourceAgeCodeListMapping(string languageCode, string dbNId, string selectedAgeCodeList)
    {
        Dictionary<string, string> RetVal = null;

        string AssociatedDbNId = Get_AssociatedDB_NId(dbNId).ToString();
        string SourceCodelistFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + AssociatedDbNId + "\\" + Constants.FolderName.SDMX.sdmx + DevInfo.Lib.DI_LibSDMX.Constants.Complete_XML.FileName);
        StructureType SourceCodelistStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), SourceCodelistFileNameWPath);

        if (string.IsNullOrEmpty(selectedAgeCodeList.Trim()))
        {
            string CodelistMappingFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + dbNId + "\\" + Constants.FolderName.SDMX.Mapping + DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.FileName);
            StructureType MappingCodelistStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), CodelistMappingFileNameWPath);

            selectedAgeCodeList = string.Empty;
            foreach (SDMXObjectModel.Structure.CodelistMapType CodelistMap in MappingCodelistStructure.Structures.StructureSets[0].Items)
            {
                if (DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.Age.id == CodelistMap.id)
                {
                    selectedAgeCodeList = ((SDMXObjectModel.Common.CodelistRefType)((SDMXObjectModel.Common.CodelistReferenceType)CodelistMap.Source).Items[0]).id;
                }
            }
            if (string.IsNullOrEmpty(selectedAgeCodeList))
            {
                selectedAgeCodeList = DevInfo.Lib.DI_LibSDMX.Constants.CodelistPrefix + "AGE";
            }
        }

        RetVal = this.Get_DictSourceCodelist(languageCode, selectedAgeCodeList, SourceCodelistStructure);

        return RetVal;
    }

    /// <summary>
    /// Get the Souce Code List Dictionary
    /// </summary>
    /// <param name="languageCode">Language Code</param>
    /// <param name="dbNId">DBNid</param>
    /// <returns></returns>
    public Dictionary<string, string> GetTargetAgeCodeListMapping(string languageCode, string dbNId)
    {
        Dictionary<string, string> RetVal = null;
        string AgeCodelistId = string.Empty;
        string TargetCodelistFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + dbNId + "\\" + Constants.FolderName.SDMX.sdmx + DevInfo.Lib.DI_LibSDMX.Constants.Complete_XML.FileName);
        SDMXApi_2_0.Message.StructureType TargetCodelistStructure = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromFile(typeof(SDMXApi_2_0.Message.StructureType), TargetCodelistFileNameWPath);
        foreach (SDMXApi_2_0.Structure.DimensionType Dimensions in TargetCodelistStructure.KeyFamilies[0].Components.Dimension)
        {
            if (Dimensions.conceptRef == Constants.UNSD.Concept.Age.Id)
            {
                AgeCodelistId = Dimensions.codelist;
            }
        }
        RetVal = this.Get_DictTargetCodelist(languageCode, AgeCodelistId, TargetCodelistStructure);



        return RetVal;
    }

    /// <summary>
    /// Get the Souce Code List Dictionary
    /// </summary>
    /// <param name="languageCode">Language Code</param>
    /// <param name="dbNId">DBNid</param>
    /// <returns></returns>
    public Dictionary<string, string> GetAgeMappedCodeList(string dbNId)
    {
        Dictionary<string, string> RetVal = null;

        string CodelistMappingFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + dbNId + "\\" + Constants.FolderName.SDMX.Mapping + DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.FileName);
        StructureType MappingCodelistStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), CodelistMappingFileNameWPath);

        RetVal = this.Get_DictMappingCodelist(DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.Age.id, MappingCodelistStructure);
        return RetVal;
    }

    /// <summary>
    /// Get the Souce Code List Dictionary for UNIT
    /// </summary>
    /// <param name="languageCode">Language Code</param>
    /// <param name="dbNId">DBNid</param>
    /// <returns></returns>
    public Dictionary<string, string> GetSourceSexCodeListMapping(string languageCode, string dbNId, string selectedSexCodeList)
    {
        Dictionary<string, string> RetVal = null;

        string AssociatedDbNId = Get_AssociatedDB_NId(dbNId).ToString();
        string SourceCodelistFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + AssociatedDbNId + "\\" + Constants.FolderName.SDMX.sdmx + DevInfo.Lib.DI_LibSDMX.Constants.Complete_XML.FileName);
        StructureType SourceCodelistStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), SourceCodelistFileNameWPath);

        if (string.IsNullOrEmpty(selectedSexCodeList.Trim()))
        {
            string CodelistMappingFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + dbNId + "\\" + Constants.FolderName.SDMX.Mapping + DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.FileName);
            StructureType MappingCodelistStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), CodelistMappingFileNameWPath);

            foreach (SDMXObjectModel.Structure.CodelistMapType CodelistMap in MappingCodelistStructure.Structures.StructureSets[0].Items)
            {
                if (DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.Sex.id == CodelistMap.id)
                {
                    selectedSexCodeList = ((SDMXObjectModel.Common.CodelistRefType)((SDMXObjectModel.Common.CodelistReferenceType)CodelistMap.Source).Items[0]).id;
                }
            }
            if (string.IsNullOrEmpty(selectedSexCodeList))
            {
                selectedSexCodeList = DevInfo.Lib.DI_LibSDMX.Constants.CodelistPrefix + "SEX";
            }
        }
        RetVal = this.Get_DictSourceCodelist(languageCode, selectedSexCodeList, SourceCodelistStructure);

        return RetVal;
    }

    /// <summary>
    /// Get the Souce Code List Dictionary
    /// </summary>
    /// <param name="languageCode">Language Code</param>
    /// <param name="dbNId">DBNid</param>
    /// <returns></returns>
    public Dictionary<string, string> GetTargetSexCodeListMapping(string languageCode, string dbNId)
    {
        Dictionary<string, string> RetVal = null;
        string SexCodelistId = string.Empty;
        string TargetCodelistFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + dbNId + "\\" + Constants.FolderName.SDMX.sdmx + DevInfo.Lib.DI_LibSDMX.Constants.Complete_XML.FileName);
        SDMXApi_2_0.Message.StructureType TargetCodelistStructure = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromFile(typeof(SDMXApi_2_0.Message.StructureType), TargetCodelistFileNameWPath);
        foreach (SDMXApi_2_0.Structure.DimensionType Dimensions in TargetCodelistStructure.KeyFamilies[0].Components.Dimension)
        {
            if (Dimensions.conceptRef == Constants.UNSD.Concept.Sex.Id)
            {
                SexCodelistId = Dimensions.codelist;
            }
        }
        RetVal = this.Get_DictTargetCodelist(languageCode, SexCodelistId, TargetCodelistStructure);



        return RetVal;
    }

    /// <summary>
    /// Get the Souce Code List Dictionary
    /// </summary>
    /// <param name="languageCode">Language Code</param>
    /// <param name="dbNId">DBNid</param>
    /// <returns></returns>
    public Dictionary<string, string> GetSexMappedCodeList(string dbNId)
    {
        Dictionary<string, string> RetVal = null;

        string CodelistMappingFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + dbNId + "\\" + Constants.FolderName.SDMX.Mapping + DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.FileName);
        StructureType MappingCodelistStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), CodelistMappingFileNameWPath);
        RetVal = this.Get_DictMappingCodelist(DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.Sex.id, MappingCodelistStructure);
        return RetVal;
    }

    /// <summary>
    /// Get the Souce Code List Dictionary for UNIT
    /// </summary>
    /// <param name="languageCode">Language Code</param>
    /// <param name="dbNId">DBNid</param>
    /// <returns></returns>
    public Dictionary<string, string> GetSourceLocationCodeListMapping(string languageCode, string dbNId, string selectedLocationCodeList)
    {
        Dictionary<string, string> RetVal = null;

        string AssociatedDbNId = Get_AssociatedDB_NId(dbNId).ToString();
        string SourceCodelistFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + AssociatedDbNId + "\\" + Constants.FolderName.SDMX.sdmx + DevInfo.Lib.DI_LibSDMX.Constants.Complete_XML.FileName);
        StructureType SourceCodelistStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), SourceCodelistFileNameWPath);
        if (string.IsNullOrEmpty(selectedLocationCodeList.Trim()))
        {
            string CodelistMappingFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + dbNId + "\\" + Constants.FolderName.SDMX.Mapping + DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.FileName);
            StructureType MappingCodelistStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), CodelistMappingFileNameWPath);

            foreach (SDMXObjectModel.Structure.CodelistMapType CodelistMap in MappingCodelistStructure.Structures.StructureSets[0].Items)
            {
                if (DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.Location.id == CodelistMap.id)
                {
                    selectedLocationCodeList = ((SDMXObjectModel.Common.CodelistRefType)((SDMXObjectModel.Common.CodelistReferenceType)CodelistMap.Source).Items[0]).id;
                }
            }
            if (string.IsNullOrEmpty(selectedLocationCodeList))
            {
                selectedLocationCodeList = DevInfo.Lib.DI_LibSDMX.Constants.CodelistPrefix + "LOCATION";
            }
        }

        RetVal = this.Get_DictSourceCodelist(languageCode, selectedLocationCodeList, SourceCodelistStructure);

        return RetVal;
    }

    /// <summary>
    /// Get the Souce Code List Dictionary
    /// </summary>
    /// <param name="languageCode">Language Code</param>
    /// <param name="dbNId">DBNid</param>
    /// <returns></returns>
    public Dictionary<string, string> GetTargetLocationCodeListMapping(string languageCode, string dbNId)
    {
        Dictionary<string, string> RetVal = null;
        string LocationCodelistId = string.Empty;

        string TargetCodelistFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + dbNId + "\\" + Constants.FolderName.SDMX.sdmx + DevInfo.Lib.DI_LibSDMX.Constants.Complete_XML.FileName);
        SDMXApi_2_0.Message.StructureType TargetCodelistStructure = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromFile(typeof(SDMXApi_2_0.Message.StructureType), TargetCodelistFileNameWPath);
        foreach (SDMXApi_2_0.Structure.DimensionType Dimensions in TargetCodelistStructure.KeyFamilies[0].Components.Dimension)
        {
            if (Dimensions.conceptRef == Constants.UNSD.Concept.Location.Id)
            {
                LocationCodelistId = Dimensions.codelist;
            }
        }
        RetVal = this.Get_DictTargetCodelist(languageCode, LocationCodelistId, TargetCodelistStructure);



        return RetVal;
    }

    /// <summary>
    /// Get the Souce Code List Dictionary
    /// </summary>
    /// <param name="languageCode">Language Code</param>
    /// <param name="dbNId">DBNid</param>
    /// <returns></returns>
    public Dictionary<string, string> GetLocationMappedCodeList(string dbNId)
    {
        Dictionary<string, string> RetVal = null;

        string CodelistMappingFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + dbNId + "\\" + Constants.FolderName.SDMX.Mapping + DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.FileName);
        StructureType MappingCodelistStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), CodelistMappingFileNameWPath);
        RetVal = this.Get_DictMappingCodelist(DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.Location.id, MappingCodelistStructure);
        return RetVal;
    }

    /// <summary>
    /// Get the Souce Code List Dictionary for UNIT
    /// </summary>
    /// <param name="languageCode">Language Code</param>
    /// <param name="dbNId">DBNid</param>
    /// <returns></returns>
    public Dictionary<string, string> GetSourceAreaCodeListMapping(string languageCode, string dbNId)
    {
        Dictionary<string, string> RetVal = null;

        string AssociatedDbNId = Get_AssociatedDB_NId(dbNId).ToString();
        string SourceCodelistFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + AssociatedDbNId + "\\" + Constants.FolderName.SDMX.sdmx + DevInfo.Lib.DI_LibSDMX.Constants.Complete_XML.FileName);
        StructureType SourceCodelistStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), SourceCodelistFileNameWPath);

        string CodelistMappingFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + dbNId + "\\" + Constants.FolderName.SDMX.Mapping + DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.FileName);
        try
        {
            StructureType MappingCodelistStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), CodelistMappingFileNameWPath);
        }
        catch (Exception)
        {
        }
        RetVal = this.Get_DictSourceCodelist(languageCode, DevInfo.Lib.DI_LibSDMX.Constants.CodeList.Area.Id, SourceCodelistStructure);
        return RetVal;
    }

    /// <summary>
    /// Get the Souce Code List Dictionary
    /// </summary>
    /// <param name="languageCode">Language Code</param>
    /// <param name="dbNId">DBNid</param>
    /// <returns></returns>
    public Dictionary<string, string> GetTargetAreaCodeListMapping(string languageCode, string dbNId)
    {
        Dictionary<string, string> RetVal = null;
        string AreaCodelistId = string.Empty;
        string TargetCodelistFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + dbNId + "\\" + Constants.FolderName.SDMX.sdmx + DevInfo.Lib.DI_LibSDMX.Constants.Complete_XML.FileName);
        SDMXApi_2_0.Message.StructureType TargetCodelistStructure = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromFile(typeof(SDMXApi_2_0.Message.StructureType), TargetCodelistFileNameWPath);
        foreach (SDMXApi_2_0.Structure.DimensionType Dimensions in TargetCodelistStructure.KeyFamilies[0].Components.Dimension)
        {
            if (Dimensions.conceptRef == Constants.UNSD.Concept.Area.Id)
            {
                AreaCodelistId = Dimensions.codelist;
            }
        }
        RetVal = this.Get_DictTargetCodelist(languageCode, AreaCodelistId, TargetCodelistStructure);




        return RetVal;
    }

    /// <summary>
    /// Get the Souce Code List Dictionary
    /// </summary>
    /// <param name="languageCode">Language Code</param>
    /// <param name="dbNId">DBNid</param>
    /// <returns></returns>
    public Dictionary<string, string> GetAreaMappedCodeList(string dbNId)
    {
        Dictionary<string, string> RetVal = null;

        string CodelistMappingFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + dbNId + "\\" + Constants.FolderName.SDMX.Mapping + DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.FileName);
        StructureType MappingCodelistStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), CodelistMappingFileNameWPath);
        RetVal = this.Get_DictMappingCodelist(DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.Area.id, MappingCodelistStructure);
        return RetVal;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="requestParam">DBNId|Language|AgeCodelist or SexCodelist or LocationCodelist </param>
    /// <returns>Server Side html for selected codelists</returns>
    public string BindCodelistMappingLists(string requestParam)
    {
        string RetVal;
        string DbNId, Language, SelectedAgeCodelist, SelectedSexCodelist, SelectedLocationCodelist,SelectedAreaLevel;
        string[] Params;
        SDMXObjectModel.Message.StructureType SourceCodelistStructure;
        SDMXApi_2_0.Message.StructureType TargetCodelistStructure;
        SDMXObjectModel.Message.StructureType MappingCodelistStructure;
        RetVal = string.Empty;
        DbNId = string.Empty;
        Language = string.Empty;
        SelectedAgeCodelist = string.Empty;
        SelectedSexCodelist = string.Empty;
        SelectedLocationCodelist = string.Empty;
        SelectedAreaLevel = string.Empty;
        Params = null;
        SourceCodelistStructure = null;
        TargetCodelistStructure = null;
        MappingCodelistStructure = null;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DbNId = Params[0].ToString().Trim();
            Language = Params[1].ToString().Trim();
            SelectedAreaLevel = Params[2].ToString().Trim();
            if (Params.Length > 3)
            {
                if (Params[4].ToString() == "aAgeCodelistSelect")
                {
                    SelectedAgeCodelist = Params[3].ToString().Trim();
                }
                else if (Params[4].ToString() == "aSexCodelistSelect")
                {
                    SelectedSexCodelist = Params[3].ToString().Trim();
                }
                else if (Params[4].ToString() == "aLocationCodelistSelect")
                {
                    SelectedLocationCodelist = Params[3].ToString().Trim();
                }


            }
            this.Get_Codelist_Source_Target_Mapping_Structure(DbNId, out SourceCodelistStructure, out TargetCodelistStructure, out MappingCodelistStructure);
            if (MappingCodelistStructure != null && MappingCodelistStructure.Structures != null && MappingCodelistStructure.Structures.StructureSets.Count > 0 
                && MappingCodelistStructure.Structures.StructureSets != null && MappingCodelistStructure.Structures.StructureSets[0].Items != null
                && MappingCodelistStructure.Structures.StructureSets[0].Items.Count > 0 && Params.Length <= 3)//&& Params.Length <= 2
            {

                foreach (SDMXObjectModel.Structure.CodelistMapType CodelistMap in MappingCodelistStructure.Structures.StructureSets[0].Items)
                {
                    if (DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.Age.id == CodelistMap.id)
                    {
                        SelectedAgeCodelist = ((SDMXObjectModel.Common.CodelistRefType)((SDMXObjectModel.Common.CodelistReferenceType)CodelistMap.Source).Items[0]).id;
                    }
                    if (DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.Sex.id == CodelistMap.id)
                    {
                        SelectedSexCodelist = ((SDMXObjectModel.Common.CodelistRefType)((SDMXObjectModel.Common.CodelistReferenceType)CodelistMap.Source).Items[0]).id;
                    }
                    if (DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.Location.id == CodelistMap.id)
                    {
                        SelectedLocationCodelist = ((SDMXObjectModel.Common.CodelistRefType)((SDMXObjectModel.Common.CodelistReferenceType)CodelistMap.Source).Items[0]).id;
                    }
                }

                RetVal = GetAllMappedCodeLists(DbNId, Language, SourceCodelistStructure, TargetCodelistStructure, MappingCodelistStructure, SelectedAgeCodelist, SelectedSexCodelist, SelectedLocationCodelist,SelectedAreaLevel);
                RetVal += Constants.Delimiters.ParamDelimiter;
                RetVal += SelectedAgeCodelist;
                RetVal += Constants.Delimiters.ParamDelimiter;
                RetVal += SelectedSexCodelist;
                RetVal += Constants.Delimiters.ParamDelimiter;
                RetVal += SelectedLocationCodelist;

            }
            else
            {
                if (string.IsNullOrEmpty(SelectedAgeCodelist) == false || string.IsNullOrEmpty(SelectedSexCodelist) == false || string.IsNullOrEmpty(SelectedLocationCodelist) == false)
                {
                    if (string.IsNullOrEmpty(SelectedAgeCodelist) == false)
                    {
                        // RetVal += Constants.Delimiters.ParamDelimiter;
                        RetVal += this.GetAgeMappingList(DbNId, Language, SelectedAgeCodelist, SourceCodelistStructure, TargetCodelistStructure, MappingCodelistStructure);
                    }
                    if (string.IsNullOrEmpty(SelectedSexCodelist) == false)
                    {
                        // RetVal += Constants.Delimiters.ParamDelimiter;
                        RetVal += this.GetSexMappingList(DbNId, Language, SelectedSexCodelist, SourceCodelistStructure, TargetCodelistStructure, MappingCodelistStructure);
                    }
                    if (string.IsNullOrEmpty(SelectedLocationCodelist) == false)
                    {
                        // RetVal += Constants.Delimiters.ParamDelimiter;
                        RetVal += this.GetLocationMappingList(DbNId, Language, SelectedLocationCodelist, SourceCodelistStructure, TargetCodelistStructure, MappingCodelistStructure);
                    }
                }
                else
                {
                    RetVal = this.GetIndicatorMappingList(DbNId, Language, SourceCodelistStructure, TargetCodelistStructure, MappingCodelistStructure);
                    RetVal += Constants.Delimiters.ParamDelimiter;
                    RetVal += this.GetUnitMappingList(DbNId, Language, SourceCodelistStructure, TargetCodelistStructure, MappingCodelistStructure);
                    RetVal += Constants.Delimiters.ParamDelimiter;
                    RetVal += this.GetAreaMappingList(DbNId, Language, SourceCodelistStructure, TargetCodelistStructure, MappingCodelistStructure, SelectedAreaLevel);

                }
            }
        }
        catch (Exception ex)
        {
            RetVal += "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        // return RetVal;




        return HttpUtility.UrlEncode(RetVal);
    }

    public string BindIndicatorCodelist(string requestParam)
    {
        string RetVal;
        string DbNId, Language;
        string[] Params;
        SDMXObjectModel.Message.StructureType SourceCodelistStructure;
        SDMXApi_2_0.Message.StructureType TargetCodelistStructure;
        SDMXObjectModel.Message.StructureType MappingCodelistStructure;
        RetVal = string.Empty;
        DbNId = string.Empty;
        Language = string.Empty;
        Params = null;
        SourceCodelistStructure = null;
        TargetCodelistStructure = null;
        MappingCodelistStructure = null;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DbNId = Params[0].ToString().Trim();
            Language = Params[1].ToString().Trim();

            this.Get_Codelist_Source_Target_Mapping_Structure(DbNId, out SourceCodelistStructure, out TargetCodelistStructure, out MappingCodelistStructure);

            RetVal = this.GetIndicatorMappingList(DbNId, Language, SourceCodelistStructure, TargetCodelistStructure, MappingCodelistStructure);
            RetVal += Constants.Delimiters.ParamDelimiter;
        }
        catch (Exception ex)
        {
            RetVal += "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }

        return HttpUtility.UrlEncode(RetVal);

    }

    public string BindUnitCodelist(string requestParam)
    {
        string RetVal;
        string DbNId, Language;
        string[] Params;
        SDMXObjectModel.Message.StructureType SourceCodelistStructure;
        SDMXApi_2_0.Message.StructureType TargetCodelistStructure;
        SDMXObjectModel.Message.StructureType MappingCodelistStructure;
        RetVal = string.Empty;
        DbNId = string.Empty;
        Language = string.Empty;
        Params = null;
        SourceCodelistStructure = null;
        TargetCodelistStructure = null;
        MappingCodelistStructure = null;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DbNId = Params[0].ToString().Trim();
            Language = Params[1].ToString().Trim();

            this.Get_Codelist_Source_Target_Mapping_Structure(DbNId, out SourceCodelistStructure, out TargetCodelistStructure, out MappingCodelistStructure);

            RetVal = this.GetUnitMappingList(DbNId, Language, SourceCodelistStructure, TargetCodelistStructure, MappingCodelistStructure);
            RetVal += Constants.Delimiters.ParamDelimiter;
        }
        catch (Exception ex)
        {

            RetVal += "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }

        return HttpUtility.UrlEncode(RetVal);

    }


    public string BindAreaCodelist(string requestParam)
    {
        string RetVal;
        string DbNId, Language, SelectedAreaLevel;
        string[] Params;
        SDMXObjectModel.Message.StructureType SourceCodelistStructure;
        SDMXApi_2_0.Message.StructureType TargetCodelistStructure;
        SDMXObjectModel.Message.StructureType MappingCodelistStructure;
        RetVal = string.Empty;
        DbNId = string.Empty;
        Language = string.Empty;
        SelectedAreaLevel = string.Empty;
        Params = null;
        SourceCodelistStructure = null;
        TargetCodelistStructure = null;
        MappingCodelistStructure = null;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DbNId = Params[0].ToString().Trim();
            Language = Params[1].ToString().Trim();
            SelectedAreaLevel = Params[2].ToString().Trim();
            this.Get_Codelist_Source_Target_Mapping_Structure(DbNId, out SourceCodelistStructure, out TargetCodelistStructure, out MappingCodelistStructure);

            RetVal = this.GetAreaMappingList(DbNId, Language, SourceCodelistStructure, TargetCodelistStructure, MappingCodelistStructure, SelectedAreaLevel);
            RetVal += Constants.Delimiters.ParamDelimiter;
        }
        catch (Exception ex)
        {

            RetVal += "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }

        return HttpUtility.UrlEncode(RetVal);

    }


    public string GenerateCodelistMappingXml(string requestParam)
    {
        string RetVal;
        string DbNId, Language, UserNId, CodelistMappingData, SelectedAgeCodelist, SelectedSexCodelist, SelectedLocationCodelist, MappedArea, AppSettingFile;
        string[] Params;
        SDMXObjectModel.Message.StructureType SourceCodelistStructure;
        SDMXApi_2_0.Message.StructureType TargetCodelistStructure;

        RetVal = string.Empty;
        DbNId = string.Empty;
        Language = string.Empty;
        UserNId = string.Empty;
        SelectedAgeCodelist = string.Empty;
        SelectedSexCodelist = string.Empty;
        SelectedLocationCodelist = string.Empty;
        CodelistMappingData = string.Empty;
        Params = null;
        SourceCodelistStructure = null;
        TargetCodelistStructure = null;
        AppSettingFile = string.Empty;
        XmlDocument XmlDoc;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DbNId = Params[0].ToString().Trim();
            Language = Params[1].ToString().Trim();
            UserNId = Params[2].ToString().Trim();
            SelectedAgeCodelist = Params[3].ToString().Trim();
            SelectedSexCodelist = Params[4].ToString().Trim();
            SelectedLocationCodelist = Params[5].ToString().Trim();
            CodelistMappingData = Params[6].ToString().Trim();
            if (Params.Length > 7)
            {
                MappedArea = Params[7].ToString().Trim();
            }
            else
            {
                MappedArea = Global.registryMSDAreaId.ToString();
            }
            AppSettingFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.AppSettingFile]);
            if (Params.Length > 7)
            {
                XmlDoc = new XmlDocument();
                XmlDoc.Load(AppSettingFile);
                SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.registryMSDAreaId, MappedArea);
                XmlDoc.Save(AppSettingFile);
            }
            this.Get_Codelist_Source_Target_Structure(DbNId, out SourceCodelistStructure, out TargetCodelistStructure);

            if (this.GenerateCodelistMappingXmlArtefact(DbNId, DevInfo.Lib.DI_LibSDMX.Constants.MaintenanceAgencyScheme.Prefix + UserNId, Language,
                CodelistMappingData, SourceCodelistStructure, TargetCodelistStructure, SelectedAgeCodelist, SelectedSexCodelist, SelectedLocationCodelist))
            {
                RetVal = "true";
            }
            else
            {
                RetVal = "false";
            }
        }
        catch (Exception ex)
        {
            RetVal += "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string BindMetadataMappingLists(string requestParam)
    {
        string RetVal;
        string DbNId, Language;
        string[] Params;

        RetVal = string.Empty;
        DbNId = string.Empty;
        Language = string.Empty;
        Params = null;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DbNId = Params[0].ToString().Trim();
            Language = Params[1].ToString().Trim();

            RetVal = this.GetMetadataMappingList(DbNId, Language);
        }
        catch (Exception ex)
        {
            RetVal += "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string GenerateMetadataMappingXml(string requestParam)
    {
        string RetVal;
        string DbNId, Language, UserNId, MetadataMappingData;
        string[] Params;

        RetVal = string.Empty;
        DbNId = string.Empty;
        Language = string.Empty;
        UserNId = string.Empty;
        MetadataMappingData = string.Empty;
        Params = null;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DbNId = Params[0].ToString().Trim();
            Language = Params[1].ToString().Trim();
            UserNId = Params[2].ToString().Trim();
            MetadataMappingData = Params[3].ToString().Trim();

            if (this.GenerateMetadataMappingXmlArtefact(DbNId, this.Get_AssociatedDB_NId(DbNId).ToString(),
                DevInfo.Lib.DI_LibSDMX.Constants.MaintenanceAgencyScheme.Prefix + UserNId, Language, MetadataMappingData))
            {
                RetVal = "true";
            }
            else
            {
                RetVal = "false";
            }
        }
        catch (Exception ex)
        {
            RetVal += "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string BindIUSMappingList(string requestParam)
    {
        string RetVal;
        string DbNId, Language, SelectedAgeCodelist, SelectedSexCodelist, SelectedLocationCodelist;
        string[] Params;
        SDMXObjectModel.Message.StructureType SourceCodelistStructure;
        SDMXApi_2_0.Message.StructureType TargetCodelistStructure;
        SDMXObjectModel.Message.StructureType MappingCodelistStructure;

        RetVal = string.Empty;
        DbNId = string.Empty;
        Language = string.Empty;
        SelectedAgeCodelist = string.Empty;
        SelectedSexCodelist = string.Empty;
        SelectedLocationCodelist = string.Empty;
        Params = null;
        SourceCodelistStructure = null;
        TargetCodelistStructure = null;
        MappingCodelistStructure = null;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DbNId = Params[0].ToString().Trim();
            SelectedAgeCodelist = Params[1].ToString().Trim();
            SelectedSexCodelist = Params[2].ToString().Trim();
            SelectedLocationCodelist = Params[3].ToString().Trim();
            Language = Params[4].ToString().Trim();

            this.Get_Codelist_Source_Target_Mapping_Structure(DbNId, out SourceCodelistStructure, out TargetCodelistStructure, out MappingCodelistStructure);

            RetVal = this.GetIUSMappingList(DbNId, Language, SourceCodelistStructure, TargetCodelistStructure, MappingCodelistStructure, SelectedAgeCodelist, SelectedSexCodelist, SelectedLocationCodelist);
        }
        catch (Exception ex)
        {
            RetVal += "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string GenerateIUSMappingXml(string requestParam)
    {
        string RetVal;
        string DbNId, Language, UserNId, IUSMappingData;
        string[] Params;

        RetVal = string.Empty;
        DbNId = string.Empty;
        Language = string.Empty;
        UserNId = string.Empty;
        IUSMappingData = string.Empty;
        Params = null;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DbNId = Params[0].ToString().Trim();
            Language = Params[1].ToString().Trim();
            UserNId = Params[2].ToString().Trim();
            IUSMappingData = Params[3].ToString().Trim();

            if (this.GenerateIUSMappingXmlArtefact(DbNId, IUSMappingData))
            {
                this.Create_Category_Schemes_With_Mapping_Information(DbNId, this.Get_AssociatedDB_NId(DbNId).ToString(),
                    DevInfo.Lib.DI_LibSDMX.Constants.MaintenanceAgencyScheme.Prefix + UserNId, Language);

                RetVal = "true";
            }
            else
            {
                RetVal = "false";
            }
        }
        catch (Exception ex)
        {
            RetVal += "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string GetCodelistsPopupInnerHTML(string requestParam)
    {
        string RetVal;
        string Language;
        string SummaryXMLFilePath;
        string DbNId, CodelistId, CodelistAgencyId, CodelistVersion, CodelistName;
        string[] Params;
        SDMXObjectModel.Message.StructureType Structure;

        RetVal = string.Empty;
        Language = string.Empty;
        SummaryXMLFilePath = string.Empty;
        DbNId = string.Empty;
        CodelistId = string.Empty;
        CodelistAgencyId = string.Empty;
        CodelistVersion = string.Empty;
        CodelistName = string.Empty;
        Params = null;
        Structure = null;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DbNId = Params[0].ToString().Trim();
            Language = Params[1].ToString().Trim();
            SummaryXMLFilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + this.Get_AssociatedDB_NId(DbNId.ToString()) + "\\sdmx\\" + "Summary.xml");

            if (File.Exists(SummaryXMLFilePath))
            {
                Structure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), SummaryXMLFilePath);

                if (Structure != null && Structure.Structures != null && Structure.Structures.Codelists != null && Structure.Structures.Codelists.Count > 0)
                {
                    RetVal = "<table id=\"tblCodelist\" style=\"width:100%\">";
                    RetVal += "<tr class=\"HeaderRowStyle\">";
                    RetVal += "<td class=\"HeaderColumnStyle\" style=\"width:2%\"></td>";
                    RetVal += "<td class=\"HeaderColumnStyle\"><span  id=\"lang_CodelistId\"></span></td>";
                    RetVal += "<td class=\"HeaderColumnStyle\"><span  id=\"lang_CodelistAgencyId\"></span></td>";
                    RetVal += "<td class=\"HeaderColumnStyle\"><span  id=\"lang_CodelistVersion\"></span></td>";
                    RetVal += "<td class=\"HeaderColumnStyle\"><span  id=\"lang_CodelistName\"></span></td>";
                    RetVal += "</tr>";

                    foreach (SDMXObjectModel.Structure.CodelistType Codelist in Structure.Structures.Codelists)
                    {
                        if ((Codelist.id != DevInfo.Lib.DI_LibSDMX.Constants.CodeList.Area.Id) &&
                            (Codelist.id != DevInfo.Lib.DI_LibSDMX.Constants.CodeList.Indicator.Id) &&
                            (Codelist.id != DevInfo.Lib.DI_LibSDMX.Constants.CodeList.Unit.Id) &&
                            (Codelist.id != DevInfo.Lib.DI_LibSDMX.Constants.CodeList.SubgroupType.Id) &&
                            (Codelist.id != DevInfo.Lib.DI_LibSDMX.Constants.CodeList.SubgroupVal.Id) &&
                            (Codelist.id != DevInfo.Lib.DI_LibSDMX.Constants.CodeList.IUS.Id))
                        {
                            CodelistId = Codelist.id.ToString();
                            CodelistAgencyId = Codelist.agencyID.ToString();
                            CodelistVersion = Codelist.version.ToString();
                            CodelistName = this.GetLangSpecificValue_For_Version_2_1(Codelist.Name, Language);
                            RetVal += "<tr class=\"DataRowStyle\">";
                            RetVal += "<td class=\"DataColumnStyle\">";
                            RetVal += "<input id=\"radio_" + CodelistId + "\" type=\"radio\" name=\"db\" value=\"" + CodelistName + "\" />";
                            RetVal += "</td>";
                            RetVal += "<td class=\"DataColumnStyle\"><span>" + CodelistId + "</span></td>";
                            RetVal += "<td class=\"DataColumnStyle\"><span>" + CodelistAgencyId + "</span></td>";
                            RetVal += "<td class=\"DataColumnStyle\"><span>" + CodelistVersion + "</span></td>";
                            RetVal += "<td class=\"DataColumnStyle\"><span>" + CodelistName + "</span></td>";
                            RetVal += "</tr>";
                        }
                    }
                }
            }
            RetVal += "</table>";
        }
        catch (Exception ex)
        {
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }

        return RetVal;
    }

    public string ExportMappingExcel(string requestParam)
    {
        string RetVal = string.Empty;
        string[] Params;
        string DbNId, LanguageCode, SourceName, TargetName, SelectedAgeCodelist, SelectedSexCodelist, SelectedLocationCodelist;
        int MappingType = 0;
        try
        {
            ExportMapping ObjExpMap = new ExportMapping();
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DbNId = Params[0].ToString().Trim();
            LanguageCode = Params[1].ToString().Trim();
            SourceName = Params[2].ToString().Trim();
            TargetName = Params[3].ToString().Trim();
            SelectedAgeCodelist = Params[4].ToString().Trim();
            SelectedSexCodelist = Params[5].ToString().Trim();
            SelectedLocationCodelist = Params[6].ToString().Trim();
            MappingType = Convert.ToInt32(Params[7].ToString().Trim());
            switch (MappingType)
            {
                case 0:
                    RetVal = ObjExpMap.ExportFiles(EnumHelper.MappingType.CodeList, LanguageCode, DbNId, SourceName, TargetName, SelectedAgeCodelist, SelectedSexCodelist, SelectedLocationCodelist);
                    break;
                case 1:
                    RetVal = ObjExpMap.ExportFiles(EnumHelper.MappingType.IUS, LanguageCode, DbNId, SourceName, TargetName, SelectedAgeCodelist, SelectedSexCodelist, SelectedLocationCodelist);
                    break;
                case 2:
                    RetVal = ObjExpMap.ExportFiles(EnumHelper.MappingType.Metadata, LanguageCode, DbNId, SourceName, TargetName, SelectedAgeCodelist, SelectedSexCodelist, SelectedLocationCodelist);
                    break;
                default:
                    RetVal = string.Empty;
                    break;
            }
        }
        catch (Exception ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(ex, null);
        }
        return RetVal;
    }


    public string ImportMappingExcel(string requestParam)
    {
        string RetVal = string.Empty;
        string[] Params;
        string DbNid, LanguageCode, UserNId, AgeCodeListGID, SexCodeListGId, LocationCodeListGID, FilePath;
        int MappingType = 0;
        ImportMapping ObjImpMap = null;
        string ParamDel = string.Empty;
        //Param string should be in the format of DBNID[****]LangauageCode[****]UserNId[****]AgeCodeListGID[****]SexCodeListGId[****]LocationCodeListGID based on mapping type
        string ImpMappingParam = string.Empty;
        try
        {
            ParamDel = Constants.Delimiters.ParamDelimiter;

            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);

            DbNid = Params[0].ToString().Trim();
            LanguageCode = Params[1].ToString().Trim();
            UserNId = Params[2].ToString().Trim();
            AgeCodeListGID = Params[3].ToString().Trim();
            SexCodeListGId = Params[4].ToString().Trim();
            LocationCodeListGID = Params[5].ToString().Trim();
            FilePath = Params[6].ToString().Trim();
            MappingType = Convert.ToInt32(Params[7].ToString().Trim());
            ObjImpMap = new ImportMapping();
            switch (MappingType)
            {
                case 0:
                    //ImpMappingParam = DbNid + ParamDel + LanguageCode + ParamDel + UserNId;
                    ImpMappingParam = DbNid + ParamDel + LanguageCode + ParamDel + UserNId + ParamDel + AgeCodeListGID + ParamDel + SexCodeListGId + ParamDel + LocationCodeListGID + ParamDel;
                    RetVal = ObjImpMap.ImportMapppingFile(EnumHelper.MappingType.CodeList, ImpMappingParam, LanguageCode, DbNid, FilePath);
                    break;
                case 1:
                    //ImpMappingParam = DbNid + ParamDel + LanguageCode + ParamDel + UserNId + ParamDel + AgeCodeListGID + ParamDel + SexCodeListGId + ParamDel + LocationCodeListGID;
                    ImpMappingParam = DbNid + ParamDel + LanguageCode + ParamDel + UserNId + ParamDel;
                    RetVal = ObjImpMap.ImportMapppingFile(EnumHelper.MappingType.IUS, ImpMappingParam, LanguageCode, DbNid, FilePath);
                    break;
                case 2:
                    //ImpMappingParam = DbNid + ParamDel + LanguageCode + ParamDel + UserNId;
                    ImpMappingParam = DbNid + ParamDel + LanguageCode + ParamDel + UserNId + ParamDel;
                    RetVal = ObjImpMap.ImportMapppingFile(EnumHelper.MappingType.Metadata, ImpMappingParam, LanguageCode, DbNid, FilePath);
                    break;
                default:
                    RetVal = string.Empty;
                    break;
            }

        }
        catch (Exception ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(ex, null);
        }
        return RetVal;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="requestParam">DBNID will always be 1 in this case bcz area levels are to be retrieved from DevInfo DB</param>
    /// <returns>Level Id - Level Name - Is Default</returns>
    public string BindAreaLevels(string requestParam)
    {
        string RetVal = string.Empty;
        string DBNId, Language, UserNId, AppSettingFile;
        string[] Params;
        string AreaLevelIdInSettings = string.Empty;
        RetVal = string.Empty;
        DBNId = string.Empty;
        Language = string.Empty;
        UserNId = string.Empty;
        AppSettingFile = string.Empty;
        Params = null;
        DIConnection DIConnection = null;
        DIQueries DIQueries = null;
        XmlDocument XmlDoc;
        DataTable dtAreaLevels = null;
        DataView dvAreaLevels = null;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DBNId = Params[0].ToString().Trim();
            Language = Params[1].ToString().Trim();
            UserNId = Params[2].ToString().Trim();
            DIConnection = Global.GetDbConnection(Convert.ToInt32(DBNId));
            DIQueries = new DIQueries(DIConnection.DIDataSetDefault(), DIConnection.DILanguageCodeDefault(DIConnection.DIDataSetDefault()));
            AppSettingFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.AppSettingFile]);
            AreaLevelIdInSettings = Global.registryAreaLevel;
            //XmlDoc = new XmlDocument();
            //XmlDoc.Load(AppSettingFile);
            ////SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.registryMSDAreaId, MappedArea);
            //XmlDoc.Save(AppSettingFile);
            dtAreaLevels = new DataTable();
            dtAreaLevels = DIConnection.ExecuteDataTable(DIQueries.Area.GetAreaLevel());
            dvAreaLevels = dtAreaLevels.DefaultView;
            dvAreaLevels.Sort = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area_Level.AreaLevel + " ASC";
            dtAreaLevels = dvAreaLevels.ToTable();
            foreach (DataRow dr in dtAreaLevels.Rows)
            {
                if (AreaLevelIdInSettings == dr[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area_Level.AreaLevel].ToString())
                {
                    RetVal += dr[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area_Level.AreaLevelName].ToString() +
                        Constants.Delimiters.ColumnDelimiter + dr[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area_Level.AreaLevel].ToString() + Constants.Delimiters.ColumnDelimiter + "true" + Constants.Delimiters.RowDelimiter;
                }
                else
                {
                    RetVal += dr[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area_Level.AreaLevelName].ToString() +
                       Constants.Delimiters.ColumnDelimiter + dr[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area_Level.AreaLevel].ToString() + Constants.Delimiters.ColumnDelimiter + "false" + Constants.Delimiters.RowDelimiter;
                }

            }


        }
        catch (Exception ex)
        {
        }
        return RetVal;
    }

    #endregion "--Public--"

    #endregion "--Methods--"

    #region Code Not In Use
    //public string BindAgeCodelist(string requestParam)
    //{
    //    string RetVal;
    //    string DbNId, Language, SelectedAgeCodelist, SelectedSexCodelist, SelectedLocationCodelist;
    //    string[] Params;
    //    SDMXObjectModel.Message.StructureType SourceCodelistStructure;
    //    SDMXApi_2_0.Message.StructureType TargetCodelistStructure;
    //    SDMXObjectModel.Message.StructureType MappingCodelistStructure;
    //    RetVal = string.Empty;
    //    DbNId = string.Empty;
    //    Language = string.Empty;
    //    SelectedAgeCodelist = string.Empty;
    //    SelectedSexCodelist = string.Empty;
    //    SelectedLocationCodelist = string.Empty;
    //    Params = null;
    //    SourceCodelistStructure = null;
    //    TargetCodelistStructure = null;
    //    MappingCodelistStructure = null;

    //    try
    //    {
    //        Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
    //        DbNId = Params[0].ToString().Trim();
    //        Language = Params[1].ToString().Trim();
    //        if (Params.Length > 2)
    //        {
    //            if (Params[3].ToString() == "aAgeCodelistSelect")
    //            {
    //                SelectedAgeCodelist = Params[2].ToString().Trim();
    //            }
    //            else if (Params[3].ToString() == "aSexCodelistSelect")
    //            {
    //                SelectedSexCodelist = Params[2].ToString().Trim();
    //            }
    //            else if (Params[3].ToString() == "aLocationCodelistSelect")
    //            {
    //                SelectedLocationCodelist = Params[2].ToString().Trim();
    //            }
    //        }
    //        this.Get_Codelist_Source_Target_Mapping_Structure(DbNId, out SourceCodelistStructure, out TargetCodelistStructure, out MappingCodelistStructure);

    //        if (string.IsNullOrEmpty(SelectedAgeCodelist) == false)
    //        {
    //            // RetVal += Constants.Delimiters.ParamDelimiter;
    //            RetVal += this.GetAgeMappingList(DbNId, Language, SelectedAgeCodelist, SourceCodelistStructure, TargetCodelistStructure, MappingCodelistStructure);
    //        }
    //        RetVal += Constants.Delimiters.ParamDelimiter;
    //    }
    //    catch (Exception ex)
    //    {

    //        RetVal += "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
    //        Global.CreateExceptionString(ex, null);

    //    }

    //    return HttpUtility.UrlEncode(RetVal);

    //}

    //public string BindSexCodelist(string requestParam)
    //{
    //    string RetVal;
    //    string DbNId, Language, SelectedAgeCodelist, SelectedSexCodelist, SelectedLocationCodelist;
    //    string[] Params;
    //    SDMXObjectModel.Message.StructureType SourceCodelistStructure;
    //    SDMXApi_2_0.Message.StructureType TargetCodelistStructure;
    //    SDMXObjectModel.Message.StructureType MappingCodelistStructure;
    //    RetVal = string.Empty;
    //    DbNId = string.Empty;
    //    Language = string.Empty;
    //    SelectedAgeCodelist = string.Empty;
    //    SelectedSexCodelist = string.Empty;
    //    SelectedLocationCodelist = string.Empty;
    //    Params = null;
    //    SourceCodelistStructure = null;
    //    TargetCodelistStructure = null;
    //    MappingCodelistStructure = null;
    //    try
    //    {
    //        Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
    //        DbNId = Params[0].ToString().Trim();
    //        Language = Params[1].ToString().Trim();
    //        if (Params.Length > 2)
    //        {
    //            if (Params[3].ToString() == "aAgeCodelistSelect")
    //            {
    //                SelectedAgeCodelist = Params[2].ToString().Trim();
    //            }
    //            else if (Params[3].ToString() == "aSexCodelistSelect")
    //            {
    //                SelectedSexCodelist = Params[2].ToString().Trim();
    //            }
    //            else if (Params[3].ToString() == "aLocationCodelistSelect")
    //            {
    //                SelectedLocationCodelist = Params[2].ToString().Trim();
    //            }
    //        }
    //        this.Get_Codelist_Source_Target_Mapping_Structure(DbNId, out SourceCodelistStructure, out TargetCodelistStructure, out MappingCodelistStructure);

    //        if (string.IsNullOrEmpty(SelectedSexCodelist) == false)
    //        {
    //            // RetVal += Constants.Delimiters.ParamDelimiter;
    //            RetVal += this.GetSexMappingList(DbNId, Language, SelectedSexCodelist, SourceCodelistStructure, TargetCodelistStructure, MappingCodelistStructure);
    //        }
    //        RetVal += Constants.Delimiters.ParamDelimiter;
    //    }
    //    catch (Exception ex)
    //    {

    //        RetVal += "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
    //        Global.CreateExceptionString(ex, null);

    //    }

    //    return HttpUtility.UrlEncode(RetVal);

    //}


    //public string BindLocationCodelist(string requestParam)
    //{
    //    string RetVal;
    //    string DbNId, Language, SelectedAgeCodelist, SelectedSexCodelist, SelectedLocationCodelist;
    //    string[] Params;
    //    SDMXObjectModel.Message.StructureType SourceCodelistStructure;
    //    SDMXApi_2_0.Message.StructureType TargetCodelistStructure;
    //    SDMXObjectModel.Message.StructureType MappingCodelistStructure;
    //    RetVal = string.Empty;
    //    DbNId = string.Empty;
    //    Language = string.Empty;
    //    SelectedAgeCodelist = string.Empty;
    //    SelectedSexCodelist = string.Empty;
    //    SelectedLocationCodelist = string.Empty;
    //    Params = null;
    //    SourceCodelistStructure = null;
    //    TargetCodelistStructure = null;
    //    MappingCodelistStructure = null;
    //    try
    //    {
    //        Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
    //        DbNId = Params[0].ToString().Trim();
    //        Language = Params[1].ToString().Trim();
    //        if (Params.Length > 2)
    //        {
    //            if (Params[3].ToString() == "aAgeCodelistSelect")
    //            {
    //                SelectedAgeCodelist = Params[2].ToString().Trim();
    //            }
    //            else if (Params[3].ToString() == "aSexCodelistSelect")
    //            {
    //                SelectedSexCodelist = Params[2].ToString().Trim();
    //            }
    //            else if (Params[3].ToString() == "aLocationCodelistSelect")
    //            {
    //                SelectedLocationCodelist = Params[2].ToString().Trim();
    //            }
    //        }
    //        this.Get_Codelist_Source_Target_Mapping_Structure(DbNId, out SourceCodelistStructure, out TargetCodelistStructure, out MappingCodelistStructure);

    //        if (string.IsNullOrEmpty(SelectedLocationCodelist) == false)
    //        {
    //            // RetVal += Constants.Delimiters.ParamDelimiter;
    //            RetVal += this.GetLocationMappingList(DbNId, Language, SelectedLocationCodelist, SourceCodelistStructure, TargetCodelistStructure, MappingCodelistStructure);
    //        }
    //        RetVal += Constants.Delimiters.ParamDelimiter;
    //    }
    //    catch (Exception ex)
    //    {

    //        RetVal += "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
    //        Global.CreateExceptionString(ex, null);

    //    }

    //    return HttpUtility.UrlEncode(RetVal);

    //}
    #endregion  Code Not In Use


}
