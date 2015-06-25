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
using System.Text;
using System.Collections;

public partial class Callback : System.Web.UI.Page
{
    #region "--Variables--"

    #region "--Private--"

    private DateTime GetSearchResults_StartTime;

    #endregion "--Private--"

    #region "--Public--"

    #endregion "--Public--"

    #endregion "--Variables--"

    #region "--Methods--"

    #region "--Private--"

    #region "--Common--"

    private string GetSearchResultsInnerHTML(DataTable DtQDSResults, string SearchLanguage, int DBNId, bool HandleAsDIUAOrDIUFlag, bool IsASFlag)
    {
        string RetVal;
        List<string> ProcessedIUANIds;

        ProcessedIUANIds = new List<string>();

        if (HandleAsDIUAOrDIUFlag)
        {
            RetVal = this.GetDIUAInnerHTML(DtQDSResults, ProcessedIUANIds, SearchLanguage, DBNId, IsASFlag);
        }
        else
        {
            RetVal = this.GetDIUInnerHTML(DtQDSResults, ProcessedIUANIds, SearchLanguage, DBNId, IsASFlag);
        }

        return RetVal;
    }

    private string GetDIUAInnerHTML(DataTable DtQDSResults, List<string> ProcessedIUANIds, string SearchLanguage, int DBNId, bool IsASFlag)
    {
        string RetVal;
        string Indicator, ICName, Unit, Area, DefaultSG, MRDTP, MRD, SGNIds, TPNIds, SourceNIds, DVNIds, DVSeries;
        int IndicatorNId, UnitNId, AreaNId, SGCount, TPCount, SourceCount, DVCount;

        RetVal = string.Empty;

        foreach (DataRow DrQDSResults in DtQDSResults.Rows)
        {
            IndicatorNId = Convert.ToInt32(DrQDSResults["IndicatorNId"].ToString());
            UnitNId = Convert.ToInt32(DrQDSResults["UnitNId"].ToString());
            AreaNId = Convert.ToInt32(DrQDSResults["AreaNId"].ToString());

            if (!ProcessedIUANIds.Contains(IndicatorNId.ToString() + "_" + UnitNId.ToString() + "_" + AreaNId.ToString())) 
            {
                ProcessedIUANIds.Add(IndicatorNId.ToString() + "_" + UnitNId.ToString() + "_" + AreaNId.ToString());
            }
            else
            {
                continue;
            }

            Indicator = DrQDSResults["Indicator"].ToString();
            ICName = DrQDSResults["ICName"].ToString();
            Unit = DrQDSResults["Unit"].ToString();
            Area = DrQDSResults["Area"].ToString();
            DefaultSG = DrQDSResults["DefaultSG"].ToString();
            MRDTP = DrQDSResults["MRDTP"].ToString();
            MRD = DrQDSResults["MRD"].ToString();
            SGCount = Convert.ToInt32(DrQDSResults["SGCount"].ToString());
            SourceCount = Convert.ToInt32(DrQDSResults["SourceCount"].ToString());
            TPCount = Convert.ToInt32(DrQDSResults["TPCount"].ToString());
            DVCount = Convert.ToInt32(DrQDSResults["DVCount"].ToString());
            SGNIds = DrQDSResults["SGNIds"].ToString();
            SourceNIds = DrQDSResults["SourceNIds"].ToString();
            TPNIds = DrQDSResults["TPNIds"].ToString();
            DVNIds = DrQDSResults["NId"].ToString(); // CHANGE : DVNIds
            DVSeries = DrQDSResults["DVSeries"].ToString();

            RetVal += this.GetDIUASingleDivHTML(IsASFlag, SearchLanguage, DBNId, IndicatorNId, UnitNId, AreaNId, Indicator, ICName, Unit, Area, DefaultSG, MRDTP, MRD, SGCount, SourceCount, TPCount, DVCount, SGNIds, SourceNIds, TPNIds, DVNIds, DVSeries);
        }

        return RetVal;
    }

    private string GetDIUInnerHTML(DataTable DtQDSResults, List<string> ProcessedIUANIds, string SearchLanguage, int DBNId, bool IsASFlag)
    {
        string RetVal;
        string Indicator, Unit, ICName, AreaNIds, SGNIds, SourceNIds, TPNIds, DVNIds, DefaultSG, DVSeries, SearchAreas;
        int IndicatorNId, UnitNId, AreaCount, SGCount, SourceCount, TPCount, DVCount;

        RetVal = string.Empty;

        foreach (DataRow DrQDSResults in DtQDSResults.Rows)
        {
            IndicatorNId = Convert.ToInt32(DrQDSResults["IndicatorNId"].ToString());
            UnitNId = Convert.ToInt32(DrQDSResults["UnitNId"].ToString());

            if (!ProcessedIUANIds.Contains(IndicatorNId.ToString() + "_" + UnitNId.ToString() + "_0"))
            {
                ProcessedIUANIds.Add(IndicatorNId.ToString() + "_" + UnitNId.ToString() + "_0");
            }
            else
            {
                continue;
            }

            SearchAreas = DrQDSResults["SearchAreas"].ToString();
            Indicator = DrQDSResults["Indicator"].ToString();
            ICName = DrQDSResults["ICName"].ToString();
            Unit = DrQDSResults["Unit"].ToString();
            AreaCount = Convert.ToInt32(DrQDSResults["AreaCount"].ToString());
            SGCount = Convert.ToInt32(DrQDSResults["SGCount"].ToString());
            SourceCount = Convert.ToInt32(DrQDSResults["SourceCount"].ToString());
            TPCount = Convert.ToInt32(DrQDSResults["TPCount"].ToString());
            DVCount = Convert.ToInt32(DrQDSResults["DVCount"].ToString());
            AreaNIds = DrQDSResults["AreaNIds"].ToString();
            SGNIds = DrQDSResults["SGNIds"].ToString();
            SourceNIds = DrQDSResults["SourceNIds"].ToString();
            TPNIds = DrQDSResults["TPNIds"].ToString();
            DVNIds = DrQDSResults["NId"].ToString(); // CHANGE : DVNIds
            DefaultSG = DrQDSResults["DefaultSG"].ToString();
            DVSeries = DrQDSResults["DVSeries"].ToString();

            RetVal += this.GetDIUSingleDivHTML(IsASFlag,SearchAreas, SearchLanguage, DBNId, IndicatorNId, UnitNId, Indicator, ICName, Unit, AreaCount, SGCount, SourceCount, TPCount, DVCount, AreaNIds, SGNIds, SourceNIds, TPNIds, DVNIds, DefaultSG, DVSeries);
        }

        return RetVal;
    }

    private string GetDIUASingleDivHTML(bool IsASFlag, string SearchLanguage, int DBNId, int IndicatorNId, int UnitNId, int AreaNId, string Indicator, string ICName, string Unit, string Area, string DefaultSG, string MRDTP, string MRD, int SGCount, int SourceCount, int TPCount, int DVCount, string SGNIds, string SourceNIds, string TPNIds, string DVNIds, string DVSeries)
    {
        string RetVal;
        string IdUniquePart;
        Double NumericMRD;
        string GallerySeparatorLengthString;
        int GallerySeparatorLength;

        IdUniquePart = DBNId.ToString() + "_" + IndicatorNId.ToString() + "_" + UnitNId.ToString() + "_" + AreaNId.ToString();

        RetVal = string.Empty;
        RetVal += "<div id=\"DIUA_" + IdUniquePart + "\">";
        RetVal += "<a style=\"line-height:20px;cursor:pointer;text-decoration:underline;\" onclick=\"javascript:PostToDataView('" + DBNId.ToString() + "', '" + DVNIds + "');\">";
        RetVal += "<span class=\"heading2\" id=\"Area_" + IdUniquePart + "\">" + Area + "</span>";
        RetVal += " - <span class=\"heading2\" id=\"Indicator_" + IdUniquePart + "\">" + Indicator + "</span>";
        RetVal += "</a>";
        RetVal += ", <span class=\"content\" id=\"Unit_" + IdUniquePart + "\">" + Unit + "</span>";

        if (!string.IsNullOrEmpty(DefaultSG) && !string.IsNullOrEmpty(MRD) && !string.IsNullOrEmpty(MRDTP))
        {
            RetVal += "&nbsp;&nbsp;";
            RetVal += "<span style=\"color:#c00000\">";

            if (Double.TryParse(MRD, out NumericMRD))
            {
                if (NumericMRD > 1)
                {
                    RetVal += "<span class=\"heading2\">" + NumericMRD.ToString("0,0") + "</span>";
                }
                else
                {
                    RetVal += "<span class=\"heading2\">" + MRD + "</span>";
                }
            }
            else
            {
                RetVal += "<span class=\"heading2\">" + MRD + "</span>";
            }

            RetVal += "<span class=\"content\"> (" + MRDTP + ")</span>";

            RetVal += "</span>";
        }

        if (!string.IsNullOrEmpty(DVSeries) && DVSeries.Contains(","))
        {
            RetVal += "&nbsp;&nbsp;";
            RetVal += "<span class=\"sparkline\" values=\"" + DVSeries.Split(':')[1] + "\"></span>";
        }

        RetVal += this.GetIndicatorDescriptionDiv(IndicatorNId, UnitNId, AreaNId, SearchLanguage, DBNId);
        RetVal += this.GetICNameDiv(ICName);
        RetVal += "<div style=\"line-height:20px;cursor:default;\" class=\"content\">";

        RetVal += "<span onmouseover=\"GetSGs('" + DBNId.ToString() + "','" + SGNIds + "', event, this);\"";
        RetVal += " onmouseout=\"HideCallout('divCallout');\">" + SGCount.ToString() + " subgroups,</span>";

        RetVal += " <span onmouseover=\"GetSources('" + DBNId.ToString() + "','" + SourceNIds + "', event, this);\"";
        RetVal += " onmouseout=\"HideCallout('divCallout');\">" + SourceCount.ToString() + " sources,</span>";

        RetVal += " <span onmouseover=\"GetTPs('" + DBNId.ToString() + "','" + TPNIds + "', event, this);\"";
        RetVal += " onmouseout=\"HideCallout('divCallout');\">" + TPCount.ToString() + " time periods</span>";

        RetVal += " | <a onclick=\"javascript:PostToDataView('" + DBNId.ToString() + "', '" + DVNIds + "');\"  style=\"cursor:pointer;\"><u>" + DVCount.ToString() + " data values</u></a>";

        RetVal += " | <a onclick=\"javascript:ShareResult('" + IdUniquePart + "');\" style=\"cursor:pointer;\"><u>share</u></a>";

        RetVal += "&nbsp;&nbsp;<image align=\"top\" src=\"../../stock/themes/default/images/add_to_cart.png\" style=\"cursor:pointer;\" id=\"ATDV_" + IdUniquePart + "\" value=\"add to data view\" onclick=\"javascript:AddtoDataView('" +
                  IdUniquePart + "', '" + Area + "', '" + Indicator + "', '" + Unit + "', '" + DBNId.ToString() + "', '" + DVNIds + "');\"/>";

        RetVal += "</div>";

        GallerySeparatorLengthString = SGCount.ToString() + " subgroups, " + SourceCount.ToString() + " sources, " +
                                       TPCount.ToString() + " time periods " + DVCount.ToString() + "   data values   Share";
        GallerySeparatorLength = GallerySeparatorLengthString.Length;

        RetVal += "<div style=\"padding:5px;\"></div>";

        RetVal += "<a id=\"btnVisualizer_" + IdUniquePart + "\" class=\"expand\" onclick=\"ToggleExpandCollapse(event);\" style=\"display:none;cursor:pointer;width:100%;padding-left:40px;\">Chart</a>";
        if (!string.IsNullOrEmpty(DVSeries) && DVSeries.Contains(","))
        {
            RetVal += "<div id=\"Visualizer_" + IdUniquePart + "\" type=\"line\" style=\"display:none;padding-left:30px;width:45%;height:150px;\">" +
                      this.Get_Visualizer_Creation_Data(DefaultSG, DVSeries.Split(':')[0], DVSeries.Split(':')[1]) + "</div>";
            RetVal += "<div  id=\"VisualizerBuffer_" + IdUniquePart + "\"style=\"padding:5px;display:none;\"></div>";
        }
        else
        {
            RetVal += "<div id=\"Visualizer_" + IdUniquePart + "\" type=\"column\" style=\"display:none;\">" +
                      string.Empty + "</div>";
        }

        RetVal += "<a id=\"btnGallery_" + IdUniquePart + "\" class=\"expand\" onclick=\"ToggleExpandCollapse(event);\" style=\"display:none;cursor:pointer;width:100%;padding-left:40px;\">Gallery Objects</a>";
        RetVal += "<div id=\"Gallery_" + IdUniquePart + "\" style=\"padding-left:40px;\"></div>";
        RetVal += "<div  id=\"GalleryBuffer_" + IdUniquePart + "\"style=\"padding:5px;display:none;\"></div>";

        RetVal += "<div style=\"padding:5px;\"></div>";

        RetVal += "</div>";

        return RetVal;
    }

    int IU_RandomAreaNid = 0;
    private string GetDIUSingleDivHTML(bool IsASFlag, string SearchAreas, string SearchLanguage, int DBNId, int IndicatorNId, int UnitNId, string Indicator, string ICName, string Unit, int AreaCount, int SGCount, int SourceCount, int TPCount, int DVCount, string AreaNIds, string SGNIds, string SourceNIds, string TPNIds, string DVNIds, string DefaultSG, string DVSeries)
    {
        string RetVal;
        string IdUniquePart;

        IdUniquePart = DBNId.ToString() + "_" + IndicatorNId.ToString() + "_" + UnitNId.ToString() + "_" + IU_RandomAreaNid.ToString();
        IU_RandomAreaNid++;

        RetVal = string.Empty;
        RetVal += "<div id=\"DIUA_" + IdUniquePart + "\">"; //
        RetVal += "<a style=\"line-height:20px; cursor:pointer;text-decoration:underline;\" onclick=\"javascript:PostToDataView('" + DBNId.ToString() + "', '" + DVNIds + "');\">";
        RetVal += "<span class=\"heading2\" id=\"Area_" + IdUniquePart + "\">" + SearchAreas + "</span>";
        RetVal += " - <span class=\"heading2\" id=\"Indicator_" + IdUniquePart + "\">" + Indicator + "</span>";
        RetVal += "</a>";
        RetVal += ", <span class=\"content\" id=\"Unit_" + IdUniquePart + "\">" + Unit + " </span>";

        RetVal += this.GetIndicatorDescriptionDiv(IndicatorNId, UnitNId, 0, SearchLanguage, DBNId);
        RetVal += this.GetICNameDiv(ICName);
        RetVal += "<div style=\"line-height:20px;cursor:default;\" class=\"content\">";

        RetVal += " <img src=\"..\\..\\stock\\themes\\default\\images\\Cloud.png\" style=\"cursor:pointer;\" width=\"19px\" height=\"12px\" onclick=\"javascript:ShowCloud('" + IdUniquePart + "');\"/>";

        RetVal += " <span onmouseover=\"GetAreas('" + DBNId.ToString() + "','" + AreaNIds + "', event, this);\"";
        RetVal += " onmouseout=\"HideCallout('divCallout');\">" + AreaCount.ToString() + " areas,</span>";

        RetVal += " <span onmouseover=\"GetSGs('" + DBNId.ToString() + "','" + SGNIds + "', event, this);\"";
        RetVal += " onmouseout=\"HideCallout('divCallout');\">" + SGCount.ToString() + " subgroups,</span>";

        RetVal += " <span onmouseover=\"GetSources('" + DBNId.ToString() + "','" + SourceNIds + "', event, this);\"";
        RetVal += " onmouseout=\"HideCallout('divCallout');\">" + SourceCount.ToString() + " sources,</span>";

        RetVal += " <span onmouseover=\"GetTPs('" + DBNId.ToString() + "','" + TPNIds + "', event, this);\"";
        RetVal += " onmouseout=\"HideCallout('divCallout');\">" + TPCount.ToString() + " time periods</span>";

        RetVal += " | <a onclick=\"javascript:PostToDataView('" + DBNId.ToString() + "', '" + DVNIds + "');\" style=\"cursor:pointer;\"><u>" + DVCount.ToString() + " data values</u></a>";

        RetVal += " | <a onclick=\"javascript:ShareResult('" + IdUniquePart + "');\" style=\"cursor:pointer;\"><u>share</u></a>";

        RetVal += "&nbsp;&nbsp;<img align=\"top\" src=\"../../stock/themes/default/images/add_to_cart.png\" style=\"cursor:pointer;\" id=\"ATDV_" + IdUniquePart + "\" value=\"add to data view\" onclick=\"javascript:AddtoDataView('" +
                  IdUniquePart + "', '" + string.Empty + "', '" + Indicator + "', '" + Unit + "', '" + DBNId.ToString() + "', '" + DVNIds + "');\"/>";

        RetVal += "</div>";

        RetVal += "<div style=\"padding:5px;\"></div>";

        RetVal += "<a id=\"btnVisualizer_" + IdUniquePart + "\" class=\"expand\" onclick=\"ToggleExpandCollapse(event);\" style=\"display:none;cursor:pointer;width:100%;padding-left:40px;\">Chart</a>";
        if (!string.IsNullOrEmpty(DVSeries) && DVSeries.Contains(","))
        {
            RetVal += "<div id=\"Visualizer_" + IdUniquePart + "\" type=\"column\" style=\"display:none;padding-left:30px;width:45%;height:150px;\">" +
                      this.Get_Visualizer_Creation_Data(DefaultSG, DVSeries.Split(':')[0], DVSeries.Split(':')[1]) + "</div>";
            RetVal += "<div  id=\"VisualizerBuffer_" + IdUniquePart + "\"style=\"padding:5px;display:none;\"></div>";
        }
        else
        {
            RetVal += "<div id=\"Visualizer_" + IdUniquePart + "\" type=\"column\" style=\"display:none;\">" +
                      string.Empty + "</div>";
        }

        RetVal += "<a id=\"btnGallery_" + IdUniquePart + "\" class=\"expand\" onclick=\"ToggleExpandCollapse(event);\" style=\"display:none;cursor:pointer;width:100%;padding-left:40px;\">Gallery Objects</a>";
        RetVal += "<div id=\"Gallery_" + IdUniquePart + "\" style=\"padding-left:40px;\"></div>";
        RetVal += "<div  id=\"GalleryBuffer_" + IdUniquePart + "\"style=\"padding:5px;display:none;\"></div>";

        RetVal += "<div style=\"padding:5px;\"></div>";

        RetVal += "</div>";

        return RetVal;
    }

    private string GetIndicatorDescriptionDiv(int IndicatorNId, int UnitNId, int AreaNId, string SearchLanguage, int DBNId)
    {
        string RetVal;
        string Description;

        Description = this.GetIndicatorDescription(IndicatorNId, SearchLanguage, DBNId);

        RetVal = "<div><i id=\"IDesc_" + IndicatorNId.ToString() + "_" + UnitNId.ToString() + "_" + AreaNId.ToString() + "\"";

        if (Description.Length > 100)
        {
            RetVal += "onmouseover=\"ShowCallout('divCallout', '" + Description + "', event);\"";
            RetVal += " onmouseout=\"HideCallout('divCallout');\"";

            Description = Description.Substring(0, 100) + "...";
        }

        RetVal += "style=\"line-height:20px; color:#999999\" class=\"content\">";
        RetVal += Description;
        RetVal += "</i></div>";

        return RetVal;
    }

    private string GetIndicatorDescription(int IndicatorNId, string SearchLanguage, int DBNId)
    {
        string RetVal;
        XmlDocument MetadataDocument;

        RetVal = string.Empty;
        MetadataDocument = null;

        try
        {
            MetadataDocument = new XmlDocument();
            MetadataDocument.Load(Server.MapPath(@"~\stock\data\" + DBNId.ToString() + @"\ds\" + SearchLanguage + @"\metadata\indicator\" + IndicatorNId.ToString() + ".xml"));
            foreach (XmlNode Category in MetadataDocument.GetElementsByTagName("ReportedAttribute"))
            {
                if (Category.Attributes["id"].Value.ToUpper() == "DEFINITION")
                {
                    RetVal = Category.InnerText;
                    RetVal = RetVal.Replace("{{~}}", "").Replace("\n", "").Replace("\t","").Replace("\"","\\\"");
                    break;
                }
            }
            RetVal = RetVal.Trim();
        }
        catch (Exception ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    private string Get_Visualizer_Creation_Data(string DefaultSG, string CategorySeries, string DVSeries)
    {
        string RetVal;

        RetVal = "{";

        RetVal += "\"categoryCollection\":[";

        foreach (string Category in CategorySeries.Split(','))
        {
            RetVal += "\"" + Category + "\",";
        }
        if (CategorySeries.Split(',').Length > 0)
        {
            RetVal = RetVal.Substring(0, RetVal.Length - 1);
        }
        RetVal += "],";

        RetVal += "\"seriesCollection\":[{\"name\":\"" + DefaultSG + "\",";

        RetVal += "\"data\":[";
        foreach (string DataValue in DVSeries.Split(','))
        {
            RetVal += DataValue + ",";
        }
        if (DVSeries.Split(',').Length > 0)
        {
            RetVal = RetVal.Substring(0, RetVal.Length - 1);
        }
        RetVal += "]";

        RetVal += "}]";

        RetVal += "}";

        return RetVal;
    }

    private string GetICNameDiv(string ICName)
    {
        string RetVal;

        RetVal = string.Empty;

        if (!string.IsNullOrEmpty(ICName))
        {
            RetVal = "<div>";
            RetVal += "<span ";

            ICName = "Searched by: " + ICName.Trim();

            if (ICName.Length > 100)
            {
                RetVal += "onmouseover=\"ShowCallout('divCallout', '" + ICName + "', event);\"";
                RetVal += " onmouseout=\"HideCallout('divCallout');\"";

                ICName = ICName.Substring(0, 100) + "...";
            }

            RetVal += "style=\"line-height:20px; color:#999999\" class=\"content\">";
            RetVal += ICName;
            RetVal += "</span>";
            RetVal += "</div>";
        }

        return RetVal;
    }

    private string GetViewDataChildDiv(string IdUniquePart, string Area, string Indicator, string Unit, string DBNId, string DVNIds)
    {
        string RetVal, Content;

        RetVal = string.Empty;
        Content = string.Empty;

        if (!string.IsNullOrEmpty(Area))
        {
            Content += Area + " - ";
        }
        if (!string.IsNullOrEmpty(Indicator))
        {
            Content += Indicator + ", ";
        }
        if (!string.IsNullOrEmpty(Unit))
        {
            Content += Unit;
        }

        RetVal = "<div id=\"divViewData_" + IdUniquePart + "\" DBNId = \"" + DBNId + "\" DVNIds=\"" + DVNIds + "\"><table width=\"100%\"><tr>";

        RetVal += "<td width=\"90%\" style=\"font-size:11px;\" title=\"" + Content + "\">";

        if (Content.Length > 55)
        {
            RetVal += Content.Substring(0, 55) + ".. ";
        }
        else
        {
            RetVal += Content + " ";
        }

        RetVal += "</td>";

        RetVal += "<td width=\"10%\" title=\"Remove\" valign=\"top\" onmouseover=\"ShowHideHoverImage(this, 'true');\" onmouseout=\"ShowHideHoverImage(this, 'false');\" onclick=\"RemoveFromDataView('" + IdUniquePart + "');\"  style=\"cursor:pointer;\">";
        RetVal += "<img src=\"../../stock/themes/default/images/remove.png\" />";
        RetVal += "</td>";

        RetVal += "</tr></table></div>";

        return RetVal;
    }

    private string SortString(string SearchString, string Splitter)
    {
        string RetVal;
        List<string> SortedString;

        RetVal = string.Empty;
        if (!string.IsNullOrEmpty(SearchString))
        {
            SortedString = new List<string>(SearchString.Split(new string[] { Splitter }, StringSplitOptions.None));
            SortedString.Sort();
            RetVal = string.Join(Splitter, SortedString.ToArray());
        }

        return RetVal;
    }

    private string CustomiseStringForIndexingAndQuery(string SearchString)
    {
        string RetVal;
        List<string> Components, SortedComponents;
        string Component, SortedComponent;

        RetVal = string.Empty;
        SortedComponent = string.Empty;
        if (!string.IsNullOrEmpty(SearchString))
        {
            SortedComponents = new List<string>();
            Components = new List<string>(SearchString.Split(new string[] { "||" }, StringSplitOptions.None));
            for (int i = 0; i < Components.Count; i++)
            {
                Component = Components[i];
                if (!string.IsNullOrEmpty(Component))
                {
                    Component = Component.Trim();
                    if (Component.Contains("\""))
                    {
                        if (Component.StartsWith("\"") && Component.EndsWith("\""))
                        {
                            Component = Component.Replace("\"", "");
                            Component = Component.Replace(" ", "[__]");
                        }
                        else
                        {
                            Component = Component.Replace("\"", "");
                        }
                    }
                    SortedComponent = this.GetSortedComponent(Component);
                }

                SortedComponents.Add(SortedComponent);
            }
            SortedComponents.Sort();
            RetVal = string.Join("||", SortedComponents.ToArray());
        }

        return RetVal;
    }

    private string GetSortedComponent(string Component)
    {
        string RetVal;
        List<string> Words, SortedWords;

        SortedWords = new List<string>();
        Words = new List<string>(Component.Split(new string[] { " " }, StringSplitOptions.None));
        foreach (string Word in Words)
        {
            if (Word.Length >= 3)
            {
                SortedWords.Add(Word);
            }
        }
        SortedWords.Sort();
        RetVal = string.Join(" ", SortedWords.ToArray());

        return RetVal;
    }
    
    private DataTable AddIndicatorDescCol(DataTable DtQDSResults,string SearchLanguage, int DBNId)
    {
        DataTable ReturnVal = DtQDSResults.Copy();

        ReturnVal.Columns.Add("IndicatorDescription", typeof(System.String));

        foreach (DataRow dr in ReturnVal.Rows)
        {
            dr["IndicatorDescription"] = this.GetIndicatorDescription((int)dr["IndicatorNId"], (string)dr["SearchLanguage"], DBNId);
            if ((bool)dr["IsAreaNumeric"] == false && string.IsNullOrEmpty(dr["Area"].ToString()))
            {
                dr["Area"] = GetQsAreaNameText(dr["AreaNId"].ToString(), SearchLanguage, DBNId);

            }
        }

        return ReturnVal;

    }

    private string GetQsAreaNameText(string QsAreaID, string SearchLanguage, int DBNId)
    {
        string RetVal;
        XmlDocument MetadataDocument;

        RetVal = string.Empty;
        MetadataDocument = null;

        try
        {
            MetadataDocument = new XmlDocument();
            MetadataDocument.Load(Server.MapPath(@"~\stock\data\" + DBNId.ToString() + @"\ds\" + SearchLanguage + @"\area\qscodelist.xml"));
            foreach (XmlNode Category in MetadataDocument.GetElementsByTagName("a"))
            {
                if (Category.Attributes["id"].Value == QsAreaID)
                {
                    RetVal = Category.Attributes["n"].Value;
                    break;
                }

            }

            RetVal = RetVal.Trim();
        }
        catch (Exception ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    private DataTable AddIndicatorClassificationCol(string SearchICs, DataTable DtQDSResults, string SearchLanguage, int DBNId)
    {
        DataTable ReturnVal = new DataTable();

        List<string> icFiles = new List<string>();        
        XmlDocument MetadataDocument;

        string[] ICs = SearchICs.Split(',');

        Hashtable IC_Indicators = new Hashtable();

        string FolderPathIC = @"~\stock\data\" + DBNId.ToString() + @"\ds\" + SearchLanguage + @"\ic";
        string FolderPathIC_IUS = @"~\stock\data\" + DBNId.ToString() + @"\ds\" + SearchLanguage + @"\ic_ius";

        MetadataDocument = null;

        try
        {
            foreach (string IC in ICs)
            {
                if (File.Exists(Server.MapPath(FolderPathIC_IUS + @"\iu_" + IC + ".xml"))) icFiles.Add(IC);
                else
                {
                    MetadataDocument = new XmlDocument();
                    MetadataDocument.Load(Server.MapPath(FolderPathIC + @"\sc.xml"));
                    foreach (XmlNode Category in MetadataDocument.GetElementsByTagName("c"))
                    {
                        if (Category.Attributes["nid"].Value == IC && Category.HasChildNodes)
                        {
                            foreach(XmlNode IcNId in Category.ChildNodes) 
                            {
                                icFiles.Add(IcNId.Attributes["nid"].Value);
                            }
                            break;
                        }
                    }
                }
                
            }

            // Get Indicators of each and every IC which are figured out above
            
            foreach (string IC in icFiles)
            {
                MetadataDocument = new XmlDocument();
                MetadataDocument.Load(Server.MapPath(FolderPathIC_IUS + @"\iu_" + IC + ".xml"));

                List<string> IndicatorsForIC = new List<string>();

                string IcName = MetadataDocument.GetElementsByTagName("ic")[0].Attributes["n"].Value;

                foreach (XmlNode Category in MetadataDocument.GetElementsByTagName("iu"))
                {
                    IndicatorsForIC.Add(Category.Attributes["inid"].Value);
                }
                IC_Indicators.Add(IcName, IndicatorsForIC);
            }

            // Search IC Name for each IndicatorNId and update in datatable

            ReturnVal = DtQDSResults.Copy();

            ReturnVal.Columns.Add("IC_Name", typeof(System.String));

            foreach (DataRow dr in ReturnVal.Rows)
            {
                string tmpIndicatorNId = dr["IndicatorNId"].ToString();

                foreach (string tmpIC_Name in IC_Indicators.Keys)
                {
                    List<string> tmpIndicators = (List<string>) IC_Indicators[tmpIC_Name];

                    if (tmpIndicators.Contains(tmpIndicatorNId))
                    {
                        dr["IC_Name"] = tmpIC_Name;
                        break;
                    }

                }
            }

        }
        catch (Exception ex)
        {
            ReturnVal = DtQDSResults;
            Global.CreateExceptionString(ex, null);
        }



        return ReturnVal;

    }

    #endregion "--Common--"

    #region "--GetQDSResults--"

    private DataTable GetQDSResultsTable(int DBNId, string SearchIndicators, string SearchICs, string SearchAreas, string SearchLanguage, bool isBlockResults)
    {
        DataTable RetVal;
        DIConnection DIConnection;
        System.Data.Common.DbParameter SearchIndicatorsParam, SearchICsParam, SearchAreasParam, SearchBlocksParam;
        List<System.Data.Common.DbParameter> DbParams;

        DIConnection = null;
      
        try
        {
            DIConnection = Global.GetDbConnection(DBNId);
            DbParams = new List<System.Data.Common.DbParameter>();

            SearchIndicatorsParam = DIConnection.CreateDBParameter();
            SearchIndicatorsParam.ParameterName = "SearchIndicators";
            SearchIndicatorsParam.DbType = DbType.String;
            SearchIndicatorsParam.Value = SearchIndicators;
            DbParams.Add(SearchIndicatorsParam);

            SearchICsParam = DIConnection.CreateDBParameter();
            SearchICsParam.ParameterName = "SearchICs";
            SearchICsParam.DbType = DbType.String;
            SearchICsParam.Value = SearchICs;
            DbParams.Add(SearchICsParam);

            SearchAreasParam = DIConnection.CreateDBParameter();
            SearchAreasParam.ParameterName = "SearchAreas";
            SearchAreasParam.DbType = DbType.String;
            SearchAreasParam.Value = SearchAreas;
            DbParams.Add(SearchAreasParam);

            SearchBlocksParam = DIConnection.CreateDBParameter();
            SearchBlocksParam.ParameterName = "SearchBlocks";
            SearchBlocksParam.DbType = DbType.Boolean;
            SearchBlocksParam.Value = isBlockResults;
            DbParams.Add(SearchBlocksParam);

            RetVal = DIConnection.ExecuteDataTable("SP_GET_SEARCH_RESULTS_" + SearchLanguage, CommandType.StoredProcedure, DbParams);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;
    }

    #endregion "--GetQDSResults--"

    #region "--GetGalleryThumbnails--"

    private DataTable GetGalleryThumbnailsTable(string Indicator, string Area, string Language, int DBNId, string Type)
    {
        DataTable RetVal;
        DIConnection DIConnection;
        string GetKeywordsQuery, GetPresentationsQuery;
        DataTable DtKeywords;

        DIConnection = null;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                          string.Empty, string.Empty);

            GetPresentationsQuery = "SELECT * FROM Presentations WHERE dbnid = '" + DBNId.ToString() + "' AND lng_code = '" + Language + "'";
            

            if (!string.IsNullOrEmpty(Indicator))
            {
                GetKeywordsQuery = "SELECT * FROM Keywords WHERE keyword = '" + RemoveHtmlEscape(DevInfo.Lib.DI_LibBAL.Utility.DICommon.RemoveQuotes(Indicator)) + "' AND keyword_type = 'I'";
                if (!string.IsNullOrEmpty(Area))
                {
                    GetKeywordsQuery += " UNION SELECT * FROM Keywords WHERE keyword = '" + RemoveHtmlEscape(DevInfo.Lib.DI_LibBAL.Utility.DICommon.RemoveQuotes(Area)) + "' AND keyword_type = 'A'";

                    DtKeywords = DIConnection.ExecuteDataTable(GetKeywordsQuery);
                    if (DtKeywords != null && DtKeywords.Rows.Count == 2)
                    {
                        GetPresentationsQuery += " AND keyword_nids LIKE '%," + DtKeywords.Rows[0]["keyword_nid"].ToString() + ",%'";
                        GetPresentationsQuery += " AND keyword_nids LIKE '%," + DtKeywords.Rows[1]["keyword_nid"].ToString() + ",%'";
                    }
                    //else if (DtKeywords != null && DtKeywords.Rows.Count == 1)
                    //{
                     //   GetPresentationsQuery += " AND keyword_nids LIKE '%," + DtKeywords.Rows[0]["keyword_nid"].ToString() + ",%'";
                        //GetPresentationsQuery += " AND keyword_nids LIKE '%," + DtKeywords.Rows[1]["keyword_nid"].ToString() + ",%'";
                    //}
                    else
                    {
                        GetPresentationsQuery += " AND keyword_nids LIKE '%,-1,%'";
                    }
                }
                else
                {
                    DtKeywords = DIConnection.ExecuteDataTable(GetKeywordsQuery);

                    if (DtKeywords != null && DtKeywords.Rows.Count == 1)
                    {
                        GetPresentationsQuery += " AND keyword_nids LIKE '%," + DtKeywords.Rows[0]["keyword_nid"].ToString() + ",%'";
                    }
                    else
                    {
                        GetPresentationsQuery += " AND keyword_nids LIKE '%,-1,%'";
                    }
                }                
            }
            else
            {
                if (!string.IsNullOrEmpty(Area))
                {
                    GetKeywordsQuery = "SELECT * FROM Keywords WHERE keyword = '" + RemoveHtmlEscape(DevInfo.Lib.DI_LibBAL.Utility.DICommon.RemoveQuotes(Area)) + "' AND keyword_type = 'A'";

                    DtKeywords = DIConnection.ExecuteDataTable(GetKeywordsQuery);

                    if (DtKeywords != null && DtKeywords.Rows.Count == 1)
                    {
                        GetPresentationsQuery += " AND keyword_nids LIKE '%," + DtKeywords.Rows[0]["keyword_nid"].ToString() + ",%'";
                        //GetPresentationsQuery += " AND keyword_nids LIKE '%," + DtKeywords.Rows[1]["keyword_nid"].ToString() + ",%'";
                    }
                    else
                    {
                        GetPresentationsQuery += " AND keyword_nids LIKE '%,-1,%'";
                    }                    
                }                             
            }
            if (Type != "A")
            {
                GetPresentationsQuery += " AND type = '" + Type + "';";
            }
            RetVal = DIConnection.ExecuteDataTable(GetPresentationsQuery);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;
    }

    private string GetGalleryThumbnailsInnerHtml(DataTable DtPresentations)
    {
        string RetVal;
        string PresentationNId, PresentationName, PresentationDesc, PrsentationText;

        RetVal = string.Empty;

        RetVal += "<table><tr>";

        foreach (DataRow DrPresentations in DtPresentations.Rows)
        {
            PresentationNId = DrPresentations["pres_nid"].ToString();
            PresentationName = DrPresentations["pres_name"].ToString();
            PresentationDesc = DrPresentations["description"].ToString();

            PrsentationText = "<div>Title: " + PresentationName + "<br/><br/>Description: " + PresentationDesc + "</div>";

            RetVal += "<td onmouseover=\"ShowCallout('divCallout', '" + PrsentationText + "', event);\" onmouseout=\"HideCallout('divCallout');\">";
            RetVal += "<img src='..\\..\\stock\\Gallery\\" + PresentationNId + "\\" + PresentationNId + "_t.jpg' width='150px' height='150px'/>";
            RetVal += "</td>";
            RetVal += "<td>&nbsp;&nbsp;</td>";
        }

        RetVal += "</tr></table>";

        return RetVal;
    }

    #endregion "--GetGalleryThumbnails--"

    #region "--GetAreas--"
    
    private DataTable GetAreasTable(string AreaNIds, string Language, int DBNId)
    {
        DataTable RetVal;
        DIConnection DIConnection;
        System.Data.Common.DbParameter AreaNIdsParam, LanguageParam;
        List<System.Data.Common.DbParameter> DbParams;

        DIConnection = null;

        try
        {
            DIConnection = Global.GetDbConnection(DBNId);
            DbParams = new List<System.Data.Common.DbParameter>();

            AreaNIdsParam = DIConnection.CreateDBParameter();
            AreaNIdsParam.ParameterName = "AreaNIds";
            AreaNIdsParam.DbType = DbType.String;
            AreaNIdsParam.Value = AreaNIds;
            DbParams.Add(AreaNIdsParam);

            RetVal = DIConnection.ExecuteDataTable("SP_GET_AREAS_FROM_NIDS_" + Language, CommandType.StoredProcedure, DbParams);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;
    }

    private string GetAreasInnerHTML(DataTable DtAreas)
    {
        string RetVal;

        RetVal = string.Empty;

        foreach (DataRow DrAreas in DtAreas.Rows)
        {
            RetVal += DrAreas["Area_Name"].ToString() + ", ";
        }

        if (RetVal.Length > 0)
        {
            RetVal = RetVal.Substring(0, RetVal.Length - 2);
        }

        return RetVal;
    }

    #endregion "--GetAreas--"

    #region "--GetSGs--"

    private DataTable GetSGsTable(string SGNIds, string Language, int DBNId)
    {
        DataTable RetVal;
        DIConnection DIConnection;
        System.Data.Common.DbParameter SGNIdsParam, LanguageParam;
        List<System.Data.Common.DbParameter> DbParams;

        DIConnection = null;

        try
        {
            DIConnection = Global.GetDbConnection(DBNId);
            DbParams = new List<System.Data.Common.DbParameter>();

            SGNIdsParam = DIConnection.CreateDBParameter();
            SGNIdsParam.ParameterName = "SGNIds";
            SGNIdsParam.DbType = DbType.String;
            SGNIdsParam.Value = SGNIds;
            DbParams.Add(SGNIdsParam);

            RetVal = DIConnection.ExecuteDataTable("SP_GET_SGS_FROM_NIDS_" + Language, CommandType.StoredProcedure, DbParams);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;
    }

    private string GetSGsInnerHTML(DataTable DtSGs)
    {
        string RetVal;

        RetVal = string.Empty;

        foreach (DataRow DrSGs in DtSGs.Rows)
        {
            RetVal += DrSGs["Subgroup_Val"].ToString() + ", ";
        }

        if (RetVal.Length > 0)
        {
            RetVal = RetVal.Substring(0, RetVal.Length - 2);
        }

        return RetVal;
    }

    #endregion "--GetSGs--"

    #region "--GetSources--"

    private DataTable GetSourcesTable(string SourceNIds, string Language, int DBNId)
    {
        DataTable RetVal;
        DIConnection DIConnection;
        System.Data.Common.DbParameter SourceNIdsParam, LanguageParam;
        List<System.Data.Common.DbParameter> DbParams;

        DIConnection = null;

        try
        {
            DIConnection = Global.GetDbConnection(DBNId);
            DbParams = new List<System.Data.Common.DbParameter>();

            SourceNIdsParam = DIConnection.CreateDBParameter();
            SourceNIdsParam.ParameterName = "SourceNIds";
            SourceNIdsParam.DbType = DbType.String;
            SourceNIdsParam.Value = SourceNIds;
            DbParams.Add(SourceNIdsParam);

            RetVal = DIConnection.ExecuteDataTable("SP_GET_SOURCES_FROM_NIDS_" + Language, CommandType.StoredProcedure, DbParams); 
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;
    }

    private string GetSourcesInnerHTML(DataTable DtSources)
    {
        string RetVal;

        RetVal = string.Empty;

        foreach (DataRow DrSources in DtSources.Rows)
        {
            RetVal += DrSources["IC_Name"].ToString() + ", ";
        }

        if (RetVal.Length > 0)
        {
            RetVal = RetVal.Substring(0, RetVal.Length - 2);
        }

        return RetVal;
    }

    #endregion "--GetSources--"

    #region "--GetTPs--"

    private DataTable GetTPsTable(string TPNIds, int DBNId)
    {
        DataTable RetVal;
        DIConnection DIConnection;
        System.Data.Common.DbParameter TPNIdsParam;
        List<System.Data.Common.DbParameter> DbParams;

        DIConnection = null;

        try
        {
            DIConnection = Global.GetDbConnection(DBNId);
            DbParams = new List<System.Data.Common.DbParameter>();

            TPNIdsParam = DIConnection.CreateDBParameter();
            TPNIdsParam.ParameterName = "TPNIds";
            TPNIdsParam.DbType = DbType.String;
            TPNIdsParam.Value = TPNIds;
            DbParams.Add(TPNIdsParam);

            RetVal = DIConnection.ExecuteDataTable("SP_GET_TPS_FROM_NIDS", CommandType.StoredProcedure, DbParams);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;
    }

    private string GetTPsInnerHTML(DataTable DtTPs)
    {
        string RetVal;

        RetVal = string.Empty;

        foreach (DataRow DrTPs in DtTPs.Rows)
        {
            RetVal += DrTPs["TimePeriod"].ToString() + ", ";
        }

        if (RetVal.Length > 0)
        {
            RetVal = RetVal.Substring(0, RetVal.Length - 2);
        }

        return RetVal;
    }

    #endregion "--GetTPs--"

    #region "--GetASResults--"

    private DataTable GetASResultsTableFromIndexingDatabase(int DBNId, string SearchIndicatorICs, string SearchAreas, string SearchLanguage, bool HandleAsDIUAOrDIUFlag)
    {
        DataTable RetVal;
        string GetASResultsQuery;
        DIConnection DIConnection;
        System.Data.Common.DbParameter DbParam;
        List<System.Data.Common.DbParameter> DbParams;

        RetVal = null;
        GetASResultsQuery = string.Empty;
        DIConnection = null;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                           string.Empty, string.Empty);
            DbParams = new List<System.Data.Common.DbParameter>();

            DbParam = DIConnection.CreateDBParameter();
            DbParam.ParameterName = "AS_DBNId";
            DbParam.DbType = DbType.Int32;
            DbParam.Value = DBNId.ToString();
            DbParams.Add(DbParam);

            DbParam = DIConnection.CreateDBParameter();
            DbParam.ParameterName = "AS_SearchIndicatorICs";
            DbParam.DbType = DbType.String;
            DbParam.Value = SearchIndicatorICs;
            DbParams.Add(DbParam);

            DbParam = DIConnection.CreateDBParameter();
            DbParam.ParameterName = "AS_SearchAreas";
            DbParam.DbType = DbType.String;

            if (HandleAsDIUAOrDIUFlag)
            {
                DbParam.Value = SearchAreas;
            }
            else
            {
                DbParam.Value = string.Empty;
            }

            DbParams.Add(DbParam);

            DbParam = DIConnection.CreateDBParameter();
            DbParam.ParameterName = "AS_SearchLanguage";
            DbParam.DbType = DbType.String;
            DbParam.Value = SearchLanguage;
            DbParams.Add(DbParam);


            GetASResultsQuery = "SELECT * FROM Indexing_Table " +
                                "WHERE AS_DBNId = @AS_DBNId AND AS_SearchIndicatorICs = @AS_SearchIndicatorICs " +
                                "AND AS_SearchAreas = @AS_SearchAreas AND AS_SearchLanguage = @AS_SearchLanguage;";

            RetVal = DIConnection.ExecuteDataTable(GetASResultsQuery, CommandType.Text, DbParams);

            for (int i = 1; i < RetVal.Columns.Count; i++)
            {
                RetVal.Columns[i].ColumnName = RetVal.Columns[i].ColumnName.Substring(3);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;
    }

    private DataTable GetASResultsTableFromDIDatabase(int DBNId, string SearchIndicatorICs, string SearchAreas, string SearchLanguage, bool HandleAsDIUAOrDIUFlag)
    {
        DataTable RetVal;
        DIConnection DIConnection;
        System.Data.Common.DbParameter SearchIndicatorICsParam, SearchAreasParam, SearchLanguageParam;
        List<System.Data.Common.DbParameter> DbParams;

        DIConnection = null;

        try
        {
            DIConnection = Global.GetDbConnection(DBNId);
            DbParams = new List<System.Data.Common.DbParameter>();

            SearchIndicatorICsParam = DIConnection.CreateDBParameter();
            SearchIndicatorICsParam.ParameterName = "SearchIndicatorICs";
            SearchIndicatorICsParam.DbType = DbType.String;
            SearchIndicatorICsParam.Value = SearchIndicatorICs;
            DbParams.Add(SearchIndicatorICsParam);

            SearchAreasParam = DIConnection.CreateDBParameter();
            SearchAreasParam.ParameterName = "SearchAreas";
            SearchAreasParam.DbType = DbType.String;
            SearchAreasParam.Value = SearchAreas;
            DbParams.Add(SearchAreasParam);

            if (HandleAsDIUAOrDIUFlag)
            {
                RetVal = DIConnection.ExecuteDataTable("SP_GET_AS_RESULTS_IUA_" + SearchLanguage, CommandType.StoredProcedure, DbParams);
            }
            else
            {
                RetVal = DIConnection.ExecuteDataTable("SP_GET_AS_RESULTS_IU_" + SearchLanguage, CommandType.StoredProcedure, DbParams);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;
    }

    private void InsertIntoIndexingDatabase(int DBNId, string SearchIndicatorICs, string SearchAreas, string SearchLanguage, DataTable DtASResults, bool HandleAsDIUAOrDIUFlag)
    {
        string InsertASResultsQuery;
        DIConnection DIConnection;
        System.Data.Common.DbParameter DbParam;
        List<System.Data.Common.DbParameter> DbParams;

        InsertASResultsQuery = string.Empty;
        DIConnection = null;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                           string.Empty, string.Empty);
            DbParams = new List<System.Data.Common.DbParameter>();

            if (DtASResults != null && DtASResults.Rows.Count > 0)
            {
                foreach (DataRow DrASResults in DtASResults.Rows)
                {
                    InsertASResultsQuery = "INSERT INTO Indexing_Table (AS_DBNId, AS_SearchIndicatorICs, AS_SearchAreas, AS_SearchLanguage, " +
                                            "AS_IndicatorNId, AS_UnitNId, AS_AreaNId, AS_Indicator, AS_ICName, AS_Unit, AS_Area, AS_DefaultSG, AS_MRDTP, AS_MRD, " +
                                            "AS_AreaCount, AS_SGCount, AS_SourceCount, AS_TPCount, AS_DVCount, AS_AreaNIds, AS_SGNIds, AS_SourceNIds, AS_TPNIds, AS_DVNIds, AS_DVSeries) " +
                                            "VALUES(@DBNId, @SearchIndicatorICs, @SearchAreas, @SearchLanguage, @IndicatorNId, @UnitNId, @AreaNId, " +
                                            "@AS_Indicator, @AS_ICName, @AS_Unit, @AS_Area, @AS_DefaultSG, @AS_MRDTP, @AS_MRD, @AS_AreaCount, @AS_SGCount, @AS_SourceCount, " +
                                            "@AS_TPCount, @AS_DVCount, @AS_AreaNIds, @AS_SGNIds, @AS_SourceNIds, @AS_TPNIds, @AS_DVNIds, @AS_DVSeries);";

                    DbParams = new List<System.Data.Common.DbParameter>();

                    DbParam = DIConnection.CreateDBParameter();
                    DbParam.ParameterName = "AS_DBNId";
                    DbParam.DbType = DbType.Int32;
                    DbParam.Value = DBNId.ToString();
                    DbParams.Add(DbParam);

                    DbParam = DIConnection.CreateDBParameter();
                    DbParam.ParameterName = "AS_SearchIndicatorICs";
                    DbParam.DbType = DbType.String;
                    DbParam.Value = SearchIndicatorICs;
                    DbParams.Add(DbParam);

                    DbParam = DIConnection.CreateDBParameter();
                    DbParam.ParameterName = "AS_SearchAreas";
                    DbParam.DbType = DbType.String;

                    if (HandleAsDIUAOrDIUFlag)
                    {
                        DbParam.Value = SearchAreas;
                    }
                    else
                    {
                        DbParam.Value = string.Empty;
                    }

                    DbParams.Add(DbParam);

                    DbParam = DIConnection.CreateDBParameter();
                    DbParam.ParameterName = "AS_SearchLanguage";
                    DbParam.DbType = DbType.String;
                    DbParam.Value = SearchLanguage;
                    DbParams.Add(DbParam);

                    DbParam = DIConnection.CreateDBParameter();
                    DbParam.ParameterName = "AS_IndicatorNId";
                    DbParam.DbType = DbType.Int32;
                    DbParam.Value = Convert.ToInt32(DrASResults["IndicatorNId"].ToString());
                    DbParams.Add(DbParam);

                    DbParam = DIConnection.CreateDBParameter();
                    DbParam.ParameterName = "AS_UnitNId";
                    DbParam.DbType = DbType.Int32;
                    DbParam.Value = Convert.ToInt32(DrASResults["UnitNId"].ToString());
                    DbParams.Add(DbParam);

                    DbParam = DIConnection.CreateDBParameter();
                    DbParam.ParameterName = "AS_AreaNId";
                    DbParam.DbType = DbType.Int32;
                    DbParam.Value = Convert.ToInt32(DrASResults["AreaNId"].ToString());
                    DbParams.Add(DbParam);

                    DbParam = DIConnection.CreateDBParameter();
                    DbParam.ParameterName = "AS_Indicator";
                    DbParam.DbType = DbType.String;
                    DbParam.Value = DrASResults["Indicator"].ToString();
                    DbParams.Add(DbParam);

                    DbParam = DIConnection.CreateDBParameter();
                    DbParam.ParameterName = "AS_ICName";
                    DbParam.DbType = DbType.String;
                    DbParam.Value = DrASResults["ICName"].ToString();
                    DbParams.Add(DbParam);

                    DbParam = DIConnection.CreateDBParameter();
                    DbParam.ParameterName = "AS_Unit";
                    DbParam.DbType = DbType.String;
                    DbParam.Value = DrASResults["Unit"].ToString();
                    DbParams.Add(DbParam);

                    DbParam = DIConnection.CreateDBParameter();
                    DbParam.ParameterName = "AS_Area";
                    DbParam.DbType = DbType.String;
                    DbParam.Value = DrASResults["Area"].ToString();
                    DbParams.Add(DbParam);

                    DbParam = DIConnection.CreateDBParameter();
                    DbParam.ParameterName = "AS_DefaultSG";
                    DbParam.DbType = DbType.String;
                    DbParam.Value = DrASResults["DefaultSG"].ToString();
                    DbParams.Add(DbParam);

                    DbParam = DIConnection.CreateDBParameter();
                    DbParam.ParameterName = "AS_MRDTP";
                    DbParam.DbType = DbType.String;
                    DbParam.Value = DrASResults["MRDTP"].ToString();
                    DbParams.Add(DbParam);

                    DbParam = DIConnection.CreateDBParameter();
                    DbParam.ParameterName = "AS_MRD";
                    DbParam.DbType = DbType.String;
                    DbParam.Value = DrASResults["MRD"].ToString();
                    DbParams.Add(DbParam);

                    DbParam = DIConnection.CreateDBParameter();
                    DbParam.ParameterName = "AS_AreaCount";
                    DbParam.DbType = DbType.Int32;
                    DbParam.Value = Convert.ToInt32(DrASResults["AreaCount"].ToString());
                    DbParams.Add(DbParam);

                    DbParam = DIConnection.CreateDBParameter();
                    DbParam.ParameterName = "AS_SGCount";
                    DbParam.DbType = DbType.Int32;
                    DbParam.Value = Convert.ToInt32(DrASResults["SGCount"].ToString());
                    DbParams.Add(DbParam);

                    DbParam = DIConnection.CreateDBParameter();
                    DbParam.ParameterName = "AS_SourceCount";
                    DbParam.DbType = DbType.Int32;
                    DbParam.Value = Convert.ToInt32(DrASResults["SourceCount"].ToString());
                    DbParams.Add(DbParam);

                    DbParam = DIConnection.CreateDBParameter();
                    DbParam.ParameterName = "AS_TPCount";
                    DbParam.DbType = DbType.Int32;
                    DbParam.Value = Convert.ToInt32(DrASResults["TPCount"].ToString());
                    DbParams.Add(DbParam);

                    DbParam = DIConnection.CreateDBParameter();
                    DbParam.ParameterName = "AS_DVCount";
                    DbParam.DbType = DbType.Int32;
                    DbParam.Value = Convert.ToInt32(DrASResults["DVCount"].ToString());
                    DbParams.Add(DbParam);

                    DbParam = DIConnection.CreateDBParameter();
                    DbParam.ParameterName = "AS_AreaNIds";
                    DbParam.DbType = DbType.String;
                    DbParam.Value = DrASResults["AreaNIds"].ToString();
                    DbParams.Add(DbParam);

                    DbParam = DIConnection.CreateDBParameter();
                    DbParam.ParameterName = "AS_SGNIds";
                    DbParam.DbType = DbType.String;
                    DbParam.Value = DrASResults["SGNIds"].ToString();
                    DbParams.Add(DbParam);

                    DbParam = DIConnection.CreateDBParameter();
                    DbParam.ParameterName = "AS_SourceNIds";
                    DbParam.DbType = DbType.String;
                    DbParam.Value = DrASResults["SourceNIds"].ToString();
                    DbParams.Add(DbParam);

                    DbParam = DIConnection.CreateDBParameter();
                    DbParam.ParameterName = "AS_TPNIds";
                    DbParam.DbType = DbType.String;
                    DbParam.Value = DrASResults["TPNIds"].ToString();
                    DbParams.Add(DbParam);

                    DbParam = DIConnection.CreateDBParameter();
                    DbParam.ParameterName = "AS_DVNIds";
                    DbParam.DbType = DbType.String;
                    DbParam.Value = DrASResults["DVNIds"].ToString();
                    DbParams.Add(DbParam);

                    DbParam = DIConnection.CreateDBParameter();
                    DbParam.ParameterName = "AS_DVSeries";
                    DbParam.DbType = DbType.String;
                    DbParam.Value = DrASResults["DVSeries"].ToString();
                    DbParams.Add(DbParam);

                    DIConnection.ExecuteDataTable(InsertASResultsQuery, CommandType.Text, DbParams);
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
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }
    }

    #endregion "--GetASResults--"

    #region "--GetCloudContent--"

    private DataTable GetCloudTable(int IndicatorNId, int UnitNId, string Language, int DBNId)
    {
        DataTable RetVal;
        DIConnection DIConnection;
        System.Data.Common.DbParameter IndicatorNIdParam, UnitNIdParam, LanguageParam, AreaParam;
        List<System.Data.Common.DbParameter> DbParams;

        DIConnection = null;

        try
        {
            DIConnection = Global.GetDbConnection(DBNId);
            DbParams = new List<System.Data.Common.DbParameter>();

            IndicatorNIdParam = DIConnection.CreateDBParameter();
            IndicatorNIdParam.ParameterName = "IndicatorNId";
            IndicatorNIdParam.DbType = DbType.Int32;
            IndicatorNIdParam.Value = IndicatorNId;
            DbParams.Add(IndicatorNIdParam);

            UnitNIdParam = DIConnection.CreateDBParameter();
            UnitNIdParam.ParameterName = "UnitNId";
            UnitNIdParam.DbType = DbType.Int32;
            UnitNIdParam.Value = UnitNId;
            DbParams.Add(UnitNIdParam);

            AreaParam = DIConnection.CreateDBParameter();
            AreaParam.ParameterName = "Area";
            AreaParam.DbType = DbType.String;
            AreaParam.Value = Global.GetDefaultArea(DBNId.ToString());
            DbParams.Add(AreaParam);

            RetVal = DIConnection.ExecuteDataTable("SP_GET_AREA_DVCLOUD_DATA_" + Language, CommandType.StoredProcedure, DbParams);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;
    }

    private string GetCloudContent(DataTable DtDVCloud, int DBNId)
    {
        string RetVal;
        string AreaName, DataValue, DVNId;
        Double DataValueCheck;

        RetVal = "[";

        foreach (DataRow DrDVCloud in DtDVCloud.Rows)
        {
            AreaName = DrDVCloud["Area_Name"].ToString();
            DataValue = DrDVCloud["Data_Value"].ToString();
            DVNId = DrDVCloud["Data_NId"].ToString();

            if (Double.TryParse(DataValue, out DataValueCheck))
            {
                RetVal += "{text: \"" + AreaName + "\", weight: " + DataValue + ", title: \"" + DataValue + "\"},";
            }
        }

        if (RetVal.Length > 1)
        {
            RetVal = RetVal.Substring(0, RetVal.Length - 1);
        }

        RetVal += "]";

        return RetVal;
    }

    #endregion "--GetCloudContent--"

    #region -- Get Default Area, Indicator their JSON and counts --

    private string[] GetDefaultAreas(DIConnection diConnection, string Language)
    {
        string[] results = new string[3];
        StringBuilder tmpAreaJson = new StringBuilder();
        Dictionary<string, List<string>> tmpAreaForJson = new Dictionary<string, List<string>>();
        string CurrentAreaLevel = string.Empty;
        int StartIndex = 0;
        try
        {
            DataTable dtAllDefaultAreas = diConnection.ExecuteDataTable("SP_GET_DEFAULT_AREAS_" + Language, CommandType.StoredProcedure, null);

            string CurrentAreaID = string.Empty;
            List<string> lstAreas = new List<string>();
            string tmpAreaInfo = string.Empty;

            tmpAreaJson.Append("{'area' : {");


            tmpAreaJson.Append("'1' : [");

            foreach (DataRow dr in dtAllDefaultAreas.Rows)
            {
                tmpAreaInfo += ",'" + dr["Area_NId"].ToString() + "|| " + dr["Area_Name"].ToString() + " (" + dr["Area_ID"].ToString() + ") - Level 1'";      
            }

            if (!string.IsNullOrEmpty(tmpAreaInfo))
            {
                tmpAreaInfo = tmpAreaInfo.Substring(1);
            }
            tmpAreaJson.Append(tmpAreaInfo);
            tmpAreaJson.Append("],");

            tmpAreaJson.Append("'2' : [");

            tmpAreaInfo = "";

            foreach (DataRow dr in dtAllDefaultAreas.Rows)
            {
                tmpAreaInfo += ",'QS_" + dr["Area_ID"].ToString() + "_L2|| " + dr["Area_Name"].ToString() + " (" + dr["Area_ID"].ToString() + ") - Level 2'";
            }

            if (!string.IsNullOrEmpty(tmpAreaInfo))
            {
                tmpAreaInfo = tmpAreaInfo.Substring(1);
            }
            tmpAreaJson.Append(tmpAreaInfo);
            tmpAreaJson.Append("]");


            tmpAreaJson.Append("}}");

            tmpAreaJson.Replace("'", "\"");


            string[] AreaCount = GetAreasCount(dtAllDefaultAreas);

            results[0] = AreaCount[0];
            results[1] = tmpAreaJson.ToString();
            results[2] = AreaCount[1];

        }

        catch (Exception ex) {
            Global.CreateExceptionString(ex, null);
        }

        return results;
    }

    private string GetLevelIdAndNameJSon(DataTable dataTable, string level)
    {
        string RetVal = string.Empty;
        string OneLevelIdAndName = string.Empty;
        string ID = string.Empty;
        string Name = string.Empty;

        foreach (DataRow dr in dataTable.Select("Area_Level=" + level))
        {
            ID = dr["Area_ID"].ToString();
            Name = dr["Area_Name"].ToString();

            OneLevelIdAndName += ",'" + ID + "|| " + Name + "'";
        }

        if (!string.IsNullOrEmpty(OneLevelIdAndName))
        {
            RetVal = OneLevelIdAndName.Substring(1);
        }

        return RetVal;
    }

    private string[] GetAreasCount(DataTable dtAllDefaultAreas)
    {
        string[] results = new string[2];
        results[0] = string.Empty;
        results[1] = string.Empty;

        int tmpAreasCount = 0;
        foreach (DataRow dr in dtAllDefaultAreas.Rows)
        {
            string ID = "," + dr["Area_NId"].ToString() + ",QS_" + dr["Area_ID"].ToString() + "_L2";

            if (!results[0].Contains(ID)) results[0] += ID;
            
            if (!(dr["Children"] is DBNull))
            {
                string Children = dr["Children"].ToString();
                tmpAreasCount += int.Parse(Children);                
            }

        }

        if (results[0].Length > 0) results[0] = results[0].Substring(1);
        results[1] = Convert.ToString(tmpAreasCount);
        return results;
    }

    private string[] GetDefaultIndicators(DIConnection diConnection, string Language)
    {
        string[] result = new string[2];

        result[0] = string.Empty;
        result[1] = string.Empty;

        try
        {

            DataTable dtDefaultIndicators = diConnection.ExecuteDataTable("SP_GET_DEFAULT_INDICATORS_" + Language, CommandType.StoredProcedure, null);

            List<string> DistinctIndicators = new List<string>();

            foreach (DataRow dr in dtDefaultIndicators.Rows)
            {
                string tmpIndNId = "," + dr["IUSNId"].ToString();
                if (!result[0].Contains(tmpIndNId)) result[0] += tmpIndNId;

                string I_U = dr["Indicator_NId"].ToString() + "~" + dr["Unit_NId"].ToString();
                string IndName_UnitName = dr["Indicator_Name"].ToString() + "~" + dr["Unit_Name"].ToString();
                string jsonIU = ",'" + I_U + "||" + IndName_UnitName.Replace("'", @"\'") + "'";

                if (!DistinctIndicators.Contains(jsonIU))
                {
                    result[1] += jsonIU;
                    DistinctIndicators.Add(jsonIU);
                }
            }

            if (result[0].Length > 0) result[0] = result[0].Substring(1);
            if (result[1].Length > 0) result[1] = result[1].Substring(1);

            result[1] = "{'iu':[" + result[1] + "],'sg_dim':{},'sg_dim_val':{},'iusnid':{}}";

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return result;
    }

    #endregion

    #endregion "--Private--"

    #region "--Public--"


   
    public string GetQDSResults(string requestParam)
    {
        string RetVal;
        string[] Params;
        string SearchIndicators, SearchICs, SearchAreas, SearchLanguage, CatalogAdaptationUrl = string.Empty,UserNId="";
        int DBNId;
        bool isBlockResults = false;
        bool wasIndicatorBlank = false;
        DataTable DtQDSResults = new DataTable();

        RetVal = string.Empty;
        GetSearchResults_StartTime = DateTime.Now;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            SearchIndicators = Params[0].Trim(); // this.SortString(Params[0].Trim(), ",");
            SearchICs = Params[1].Trim(); // this.SortString(Params[1].Trim(), ",");
            SearchAreas = Params[2].Trim(); // this.SortString(Params[2].Trim(), ",");
            SearchLanguage = Params[3].Trim();
            DBNId = Convert.ToInt32(Params[4].Trim());

            if (Params.Length >= 6) isBlockResults = bool.Parse(Params[5].Trim());
            if (Params.Length >= 7) CatalogAdaptationUrl = Params[6].Trim();
            if (Params.Length >= 8) UserNId = Params[7].Trim();//Gallery Existence
                


            // When catalog page requests to get QDS results from another 'adaptation'
            if (!string.IsNullOrEmpty(CatalogAdaptationUrl))
            {
                // Call webservice methods of that adaptation
                RetVal = getQdsResultsFromAdaptationWebService(CatalogAdaptationUrl, SearchIndicators, SearchAreas, SearchLanguage, isBlockResults);
                return RetVal;
            }

            if (string.IsNullOrEmpty(SearchAreas))
            {
                SearchAreas = Global.GetDefaultArea(DBNId.ToString());
            }

            if (string.IsNullOrEmpty(SearchIndicators) && string.IsNullOrEmpty(SearchICs))
            {
                SearchIndicators = Global.GetDefaultIndicator(DBNId.ToString());
                wasIndicatorBlank = true;
            }

            string DbLanguage = Global.GetDefaultLanguageCodeDB(DBNId.ToString(), SearchLanguage);

            DtQDSResults = this.GetQDSResultsTable(DBNId, SearchIndicators, SearchICs, SearchAreas, DbLanguage, !isBlockResults);
            
            if(wasIndicatorBlank)                
            {
                DtQDSResults.DefaultView.Sort = "Area ASC";
                DtQDSResults = DtQDSResults.DefaultView.ToTable();
            }

            RetVal = ((TimeSpan)DateTime.Now.Subtract(GetSearchResults_StartTime)).TotalSeconds.ToString("0.00");

            DtQDSResults = AddIndicatorDescCol(DtQDSResults, SearchLanguage, DBNId);

            DtQDSResults = AddIndicatorClassificationCol(SearchICs, DtQDSResults, DbLanguage, DBNId);
            //gallery Existence
            bool Result = false;
            int QDSCount = DtQDSResults.Rows.Count;
            string AreaIndTemp = "";
            string AreaInd = "";
            char separatorMain = (char)09;
            char separatorMid = (char)10;
            //Added for Simul Gallery Home Page 
            var GalleryParams = "";
            var NID_GalleryXML = "";
            //Till here
            if (isBlockResults == false)
            {
                
                for (int i = 0; i < QDSCount; i++)
                {
                   AreaIndTemp += separatorMain + DtQDSResults.Rows[i]["Indicator"].ToString() + separatorMid + DtQDSResults.Rows[i]["Area"].ToString() + separatorMid + DtQDSResults.Rows[i]["NId"].ToString();//Added for Simul Gallery Home Page 
                }
                AreaInd = AreaIndTemp.Remove(0, 1);
                string[] arrAreaInd = AreaInd.Split(separatorMain);
                string[] arrAreaIndSing = null;
                for (int j = 0; j < arrAreaInd.Length; j++)
                {
                    arrAreaIndSing = arrAreaInd[j].Split(separatorMid);
                    //Added for Simul Gallery Home Page 
                    string GalleryXML = "";
                    GalleryParams = arrAreaIndSing[0] + Constants.Delimiters.ParamDelimiter + arrAreaIndSing[1] + Constants.Delimiters.ParamDelimiter + SearchLanguage + Constants.Delimiters.ParamDelimiter + DBNId + Constants.Delimiters.ParamDelimiter + UserNId;
                    if(UserNId!="" && UserNId!=null)
                    GalleryXML = GetGalleryThumbnails(GalleryParams);
                    NID_GalleryXML += Constants.Delimiters.GalleryParamDelimeterMain + arrAreaIndSing[2] + Constants.Delimiters.GalleryParamDelimeterSub + GalleryXML;
                    //Till Here
                }
            }
            RetVal += Constants.Delimiters.ParamDelimiter + Global.GetJSONString(DtQDSResults);
            RetVal += Constants.Delimiters.ParamDelimiter + Result;
            RetVal += Constants.Delimiters.ParamDelimiter + NID_GalleryXML;//Added for Simul Gallery Home Page 
            
        }
        catch (Exception ex)
        {
            RetVal = ((TimeSpan)DateTime.Now.Subtract(GetSearchResults_StartTime)).TotalSeconds.ToString("0.00");
            RetVal += Constants.Delimiters.ParamDelimiter + "{\"\" : []}";
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }
    public string RemoveHtmlEscape(string strName)
    {
        string strModname = "";
        if (strName.IndexOf("&amp;") != -1)
        {
            strModname = strName.Replace("&amp;", "&");
            return strModname;
        }

        else
            return strName;
    }
    public string GetCatalogQdsResults(string SearchAreas, string SearchIndicators, string SearchLanguage, bool isBlockResults)
    {
        string RetVal = string.Empty;

        string DefaultIUS = string.Empty, DefaultAreas = string.Empty;

        SearchAreas = SearchAreas.Replace(" ", "");
        SearchIndicators = SearchIndicators.Replace(" ", "");

        try
        {
            int DBNId = int.Parse(Global.GetDefaultDbNId());

            //if (string.IsNullOrEmpty(SearchAreas)) DefaultAreas = Global.GetDefaultArea(DBNId.ToString());

            //if (string.IsNullOrEmpty(SearchIndicators)) DefaultIUS = Global.GetDefaultIusNIds(DBNId.ToString());

            string tmpRequestString = string.Empty;

            tmpRequestString += SearchIndicators + Constants.Delimiters.ParamDelimiter; // Indicators are merged
            tmpRequestString += "" + Constants.Delimiters.ParamDelimiter; // ICs are merged as blank
            tmpRequestString += SearchAreas + Constants.Delimiters.ParamDelimiter; // Areas are merged
            tmpRequestString += SearchLanguage + Constants.Delimiters.ParamDelimiter; // Language is merged
            tmpRequestString += Global.GetDefaultDbNId() + Constants.Delimiters.ParamDelimiter; // DBNId is merged
            tmpRequestString += isBlockResults.ToString() + Constants.Delimiters.ParamDelimiter; // isBlock is merged
            tmpRequestString += "" + Constants.Delimiters.ParamDelimiter; // CatalogAdaptationUrl is merged as blank

            RetVal = GetQDSResults(tmpRequestString);

            // If no results found for Area-Indicator combination
            // Then try to fetch results for Area & Indicator separately
            if (RetVal.Length <= 20)
            {
                string AreaResults = string.Empty;
                string IndicatorResults = string.Empty;

                // Area combinations are fetched
                if (!string.IsNullOrEmpty(SearchAreas))
                {
                    tmpRequestString = string.Empty;

                    tmpRequestString += Constants.Delimiters.ParamDelimiter; // Indicators are NOT merged
                    tmpRequestString += "" + Constants.Delimiters.ParamDelimiter; // ICs are merged as blank
                    tmpRequestString += SearchAreas + Constants.Delimiters.ParamDelimiter; // Areas are merged
                    tmpRequestString += SearchLanguage + Constants.Delimiters.ParamDelimiter; // Language is merged
                    tmpRequestString += Global.GetDefaultDbNId() + Constants.Delimiters.ParamDelimiter; // DBNId is merged
                    tmpRequestString += isBlockResults.ToString() + Constants.Delimiters.ParamDelimiter; // isBlock is merged
                    tmpRequestString += "" + Constants.Delimiters.ParamDelimiter; // CatalogAdaptationUrl is merged as blank

                    AreaResults = GetQDSResults(tmpRequestString);
                }
                if (!string.IsNullOrEmpty(SearchIndicators))
                {
                    tmpRequestString = string.Empty;

                    tmpRequestString += SearchIndicators + Constants.Delimiters.ParamDelimiter; // Indicators are merged
                    tmpRequestString += "" + Constants.Delimiters.ParamDelimiter; // ICs are merged as blank
                    tmpRequestString += Constants.Delimiters.ParamDelimiter; // Areas are NOT merged
                    tmpRequestString += SearchLanguage + Constants.Delimiters.ParamDelimiter; // Language is merged
                    tmpRequestString += Global.GetDefaultDbNId() + Constants.Delimiters.ParamDelimiter; // DBNId is merged
                    tmpRequestString += isBlockResults.ToString() + Constants.Delimiters.ParamDelimiter; // isBlock is merged
                    tmpRequestString += "" + Constants.Delimiters.ParamDelimiter; // CatalogAdaptationUrl is merged as blank

                    IndicatorResults = GetQDSResults(tmpRequestString);
                }

                // Choose which results set is to be returned
                if (AreaResults.Length > IndicatorResults.Length) RetVal = AreaResults;
                else RetVal = IndicatorResults;
            }
            
        }
        catch (Exception ex) {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string GetQdsResultsForAdaptation(string requestParam)
    {

        string AdaptationURL;
        string RetVal;
        string[] Params;
        string SearchIndicators, SearchICs, SearchAreas, SearchLanguage;
        
        RetVal = string.Empty;
        GetSearchResults_StartTime = DateTime.Now;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            SearchIndicators = Params[0].Trim(); // this.SortString(Params[0].Trim(), ",");
            SearchICs = Params[1].Trim(); // this.SortString(Params[1].Trim(), ",");
            SearchAreas = Params[2].Trim(); // this.SortString(Params[2].Trim(), ",");
            SearchLanguage = Params[3].Trim();
            AdaptationURL = Params[4].Trim();

            RetVal = getQdsResultsFromAdaptationWebService(AdaptationURL, SearchIndicators, SearchAreas, SearchLanguage, false);

        }
        catch (Exception ex)
        {
            RetVal = ex.Message;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    private string getQdsResultsFromAdaptationWebService(string AdaptationURL, string SearchIndicators, string SearchAreas, string SearchLanguage, bool isBlock)
    {
        string RetVal = string.Empty;
        try
        {
            CatalogQdsResults.QdsResults objResults = new CatalogQdsResults.QdsResults();
            objResults.Url = AdaptationURL + @"/libraries/ws/QdsResults.asmx";
            RetVal = objResults.getQdsResults(SearchIndicators, SearchAreas, SearchLanguage, isBlock); 
        }
        catch (Exception ex) {
            Global.CreateExceptionString(ex, null);
        } 

        return RetVal;
    }


    public string GetGalleryThumbnails(string requestParam)
    {
        string RetVal;
        string[] Params;
        string Indicator, Area, Language,UserNid ,GalleryItemPath, AdminNid;
        int DBNId;
        DataTable DtPresentations, filterPresentations, PublicPresentation,PrivatePresentaion;
        bool isAdmin =false;

        RetVal = string.Empty;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            Indicator = Params[0].Trim();
            Area = Params[1].Trim();
            Language = Params[2].Trim();
            DBNId = Convert.ToInt32(Params[3].Trim());
            UserNid = Params[4].Trim();
            if (Global.enableQDSGallery == "true")
            {                
                DataRow[] drRows=null;
                filterPresentations = new DataTable("PresentationTable");
                DtPresentations = this.GetGalleryThumbnailsTable(Indicator, Area, Language, DBNId, "A");                
                if (DtPresentations != null && DtPresentations.Rows.Count > 0)
                {                    
                    isAdmin = isUserAdmin(UserNid);
                    AdminNid = this.Get_AdminNId();                    
                    //GalleryItemPath
                    DtPresentations.Columns.Add(new DataColumn("path"));                    
                    DtPresentations.AcceptChanges();
                    PublicPresentation = DtPresentations.Clone();
                    PrivatePresentaion = DtPresentations.Clone();
                    filterPresentations = DtPresentations.Clone();
                  
                    drRows = DtPresentations.Select("user_nid='" + AdminNid + "'");                       
                    if (drRows.Length > 0)
                    {
                        GalleryItemPath = Global.GetAdaptationUrl();
                        GalleryItemPath += "/stock/gallery/public/admin/";
                        foreach (DataRow dr in drRows)
                        {
                            DataRow NewRow = PublicPresentation.NewRow();
                            NewRow.ItemArray = dr.ItemArray;
                            PublicPresentation.Rows.Add(NewRow);
                        }
                        foreach (DataRow dr in PublicPresentation.Rows)
                        {
                            dr["path"] = GalleryItemPath;
                        }
                    }                                            
                    if (!isAdmin && AdminNid != UserNid)
                    {                        
                        drRows = DtPresentations.Select("user_nid='" + UserNid + "'");
                        if (drRows.Length > 0)
                        {
                            GalleryItemPath = Global.GetAdaptationUrl();
                            GalleryItemPath += "/" + "stock/gallery/private/" + UserNid + "/";                            

                            foreach (DataRow dr in drRows)
                            {
                                DataRow NewRow = PrivatePresentaion.NewRow();
                                NewRow.ItemArray = dr.ItemArray;
                                PrivatePresentaion.Rows.Add(NewRow);
                            }
                            foreach (DataRow dr in PrivatePresentaion.Rows)
                            {
                                dr["path"] = GalleryItemPath;
                            }
                        }                        
                    }
                    filterPresentations.Merge(PublicPresentation);
                    filterPresentations.Merge(PrivatePresentaion);
                    if (filterPresentations.Rows.Count > 0)
                    {
                        filterPresentations.TableName = "PresentationTable";
                        filterPresentations.AcceptChanges();
                        //RetVal = Global.GetJSONString(filterPresentations);
                        RetVal = GetXMLString(filterPresentations, Server.MapPath("~//stock//Database.mdb"));//Added to implement Gallery pop up in home page--2

                    }                    
                }
            }
        }
        catch (Exception ex)
        {
            RetVal = "";
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }
    //Added to implement Gallery pop up in home page--2
    public string GetXMLString(DataTable Dt, string DatabaseURL)
    {
        string gPresNId, gPresType, gPresTitle, gPresDesc, gPresModifiedDate, gKeywordNIds, gSearchedKeyWords, gUserNid, gChartType, gFolderPath;
        List<string> UsedPresNIds;
        List<string> SearchWords;
        string RetVal;
        RetVal = string.Empty;
        UsedPresNIds = new List<string>();
        SearchWords = new List<string>();

        RetVal += "<results><s>";
        RetVal += "<base_path path=''/>";
        foreach (DataRow DrPresentations in Dt.Rows)
        {
            if (UsedPresNIds.Contains(DrPresentations["pres_nid"].ToString()))
            {
                continue;
            }
            else
            {
                UsedPresNIds.Add(DrPresentations["pres_nid"].ToString());
            }

            gPresNId = DrPresentations["pres_nid"].ToString();
            gPresType = DrPresentations["type"].ToString();
            gPresTitle = DrPresentations["pres_name"].ToString();
            gPresDesc = DrPresentations["description"].ToString();
            gPresModifiedDate = DrPresentations["modified_time"].ToString();
            gKeywordNIds = DrPresentations["keyword_nids"].ToString();
            gUserNid = DrPresentations["user_nid"].ToString();
            gChartType = DrPresentations["chart_type"].ToString();
            gFolderPath = DrPresentations["path"].ToString();

            if (gKeywordNIds.Length > 0)
            {
                gKeywordNIds = gKeywordNIds.Substring(1, gKeywordNIds.Length - 2);
            }

            //gSearchedKeyWords = Global.GetSearchedKeywords(gKeywordNIds, SearchWords, DatabaseURL);
            gSearchedKeyWords = "";
            string folderPath = gFolderPath.Substring(gFolderPath.IndexOf("stock"));
            if (Directory.Exists(Server.MapPath("~/" + folderPath + gPresNId)))
            {
            //NumResults++;
            RetVal += "<g p=\"" + gPresType + "\" s=\"" + gPresNId + "/" + gPresNId + "_t.png\" sbig=\"" + gPresNId + "/" + gPresNId + ".png\"";
            RetVal += " shtml=\"" + gPresNId + "/" + gPresNId + ".html?id=" + gPresNId + "&unid=" + gUserNid + "\" t=\"" + gPresTitle + "\" i=\"" + gPresNId + "\" desc=\"" + gPresDesc + "\""; RetVal += " md=\"" + gPresModifiedDate + "\" k=\"" + gSearchedKeyWords + "\" kid=\"" + gKeywordNIds + "\" vtype= \"" + gChartType + "\" fpath=\"" + gFolderPath + "\" pnid=\"" + gPresNId + "\" />";
            }
        }
        RetVal += "</s></results>";
        return RetVal;

    }
    //End of code added to implement Gallery pop up in home page
    public string GetAreas(string requestParam)
    {
        string RetVal;
        string[] Params;
        string AreaNIds, Language;
        int DBNId;
        DataTable DtAreas;

        RetVal = string.Empty;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            AreaNIds = Params[0].Trim();
            Language = Params[1].Trim();
            DBNId = Convert.ToInt32(Params[2].Trim());

            DtAreas = this.GetAreasTable(AreaNIds, Language, DBNId);
            RetVal = this.GetAreasInnerHTML(DtAreas);
        }
        catch (Exception ex)
        {
            RetVal = ex.Message;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string GetSGs(string requestParam)
    {
        string RetVal;
        string[] Params;
        string SGNIds, Language;
        int DBNId;
        DataTable DtSGs;

        RetVal = string.Empty;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            SGNIds = Params[0].Trim();
            Language = Params[1].Trim();
            DBNId = Convert.ToInt32(Params[2].Trim());

            DtSGs = this.GetSGsTable(SGNIds, Language, DBNId);
            RetVal = this.GetSGsInnerHTML(DtSGs);
        }
        catch (Exception ex)
        {
            RetVal = ex.Message;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string GetSources(string requestParam)
    {
        string RetVal;
        string[] Params;
        string SourceNIds, Language;
        int DBNId;
        DataTable DtSources;

        RetVal = string.Empty;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            SourceNIds = Params[0].Trim();
            Language = Params[1].Trim();
            DBNId = Convert.ToInt32(Params[2].Trim());

            DtSources = this.GetSourcesTable(SourceNIds, Language, DBNId);
            RetVal = this.GetSourcesInnerHTML(DtSources);
        }
        catch (Exception ex)
        {
            RetVal = ex.Message;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string GetTPs(string requestParam)
    {
        string RetVal;
        string[] Params;
        string TPNIds;
        int DBNId;
        DataTable DtTPs;

        RetVal = string.Empty;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            TPNIds = Params[0].Trim();
            DBNId = Convert.ToInt32(Params[1].Trim());

            DtTPs = this.GetTPsTable(TPNIds, DBNId);
            RetVal = this.GetTPsInnerHTML(DtTPs);
        }
        catch (Exception ex)
        {
            RetVal = ex.Message;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string GetASDatabases(string requestParam)
    {
        string RetVal;
        string ASDatabaseIds;
        XmlDocument DbFile;

        RetVal = string.Empty;
        ASDatabaseIds = requestParam;
        DbFile = new XmlDocument();

        try
        {
            DbFile.Load(Path.Combine(Server.MapPath("~"), @"stock\db.xml"));
            RetVal = "<table id=\"tblASDatabases\" width=\"100%\">";
            foreach (XmlNode Category in DbFile.GetElementsByTagName("category"))
            {
                if (Category.HasChildNodes)
                {
                    RetVal += "<tr><td>";
                    RetVal += "<table width=\"100%\">";

                    RetVal += "<tr height=\"22\" style=\"background-color:#cccccc\"><td class=\"content\"><b>" + Category.Attributes["name"].Value + "</b></td></tr>";
                    foreach (XmlNode Db in Category.ChildNodes)
                    {
                        RetVal += "<tr><td>";

                        if (ASDatabaseIds.Contains("," + Db.Attributes["id"].Value + ","))
                        {
                            RetVal += "<input type=\"checkbox\" value=\"" + Db.Attributes["id"].Value + "\" checked=\"checked\"/>";
                        }
                        else
                        {
                            RetVal += "<input type=\"checkbox\" value=\"" + Db.Attributes["id"].Value + "\"/>";
                        }

                        RetVal += "<span class=\"content\">" + Db.Attributes["n"].Value + "</span>";
                        RetVal += "</td></tr>";
                    }

                    RetVal += "</table>";
                    RetVal += "</td></tr>";
                }
            }

            RetVal += "</table>";
        }
        catch (Exception ex)
        {
            RetVal = ex.Message;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string GetASResults(string requestParam)
    {
        string RetVal;
        string[] Params;
        string SearchIndicatorICs, SearchAreas, SearchLanguage;
        int DBNId, NumResults;
        double ElapsedTime;
        bool HandleAsDIUAOrDIUFlag;
        DataTable DtASResults;

        RetVal = string.Empty;
        HandleAsDIUAOrDIUFlag = true;
        GetSearchResults_StartTime = DateTime.Now;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            SearchIndicatorICs = this.CustomiseStringForIndexingAndQuery(Params[0].Trim());
            SearchAreas = this.CustomiseStringForIndexingAndQuery(Params[1].Trim());
            SearchLanguage = Params[2].Trim();
            DBNId = Convert.ToInt32(Params[3].Trim());
            NumResults = Convert.ToInt32(Params[4].Trim());
            ElapsedTime = Convert.ToDouble(Params[5].Trim());

            if (string.IsNullOrEmpty(SearchAreas))
            {
                SearchAreas = Global.GetDefaultArea(DBNId.ToString());
                HandleAsDIUAOrDIUFlag = false;
            }

            DtASResults = this.GetASResultsTableFromIndexingDatabase(DBNId, SearchIndicatorICs, SearchAreas, SearchLanguage, HandleAsDIUAOrDIUFlag);

            if (DtASResults.Rows.Count == 0)
            {
                DtASResults = this.GetASResultsTableFromDIDatabase(DBNId, SearchIndicatorICs, SearchAreas, SearchLanguage, HandleAsDIUAOrDIUFlag);
                this.InsertIntoIndexingDatabase(DBNId, SearchIndicatorICs, SearchAreas, SearchLanguage, DtASResults, HandleAsDIUAOrDIUFlag);
            }

            RetVal = this.GetSearchResultsInnerHTML(DtASResults, SearchLanguage, DBNId, HandleAsDIUAOrDIUFlag, true);

            if (!string.IsNullOrEmpty(RetVal))
            {
                RetVal = "<fieldset><legend style=\"padding:10px;\" class=\"heading2\">Database: " +
                         Global.GetDbDetails(DBNId.ToString(), SearchLanguage)[0] + "</legend>" + RetVal + "</fieldset>";
            }

            RetVal += Constants.Delimiters.ParamDelimiter + (DtASResults.Rows.Count + NumResults).ToString() + " results in " +
                      (((TimeSpan)DateTime.Now.Subtract(GetSearchResults_StartTime)).TotalSeconds + ElapsedTime).ToString("0.00") + " seconds";
        }
        catch (Exception ex)
        {
            RetVal = ex.Message;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string ShareSearchResult(string requestParam)
    {
        string RetVal;
        string[] Params;
        string DivHtml, TemplateHtml;
        string hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao;
        string Area, Indicator, GalleryParams;
        string AdaptationURL = "";
        string UserNId = "-1";
        string GalleryXML = "";

      


        RetVal = string.Empty;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DivHtml = Params[0].Trim();
            hdsby = Params[1].Trim();
            if (AdaptationURL == "")
            {
                hdbnid = Global.GetDefaultDbNId();//Params[2].Trim();
            }
            else
                hdbnid = Params[2].Trim();
            hselarea = Params[3].Trim();
            hselind = Params[4].Trim();
            hlngcode = Params[5].Trim();
            hlngcodedb = Params[6].Trim();
            //hselindo = ""; // Params[7].Trim();
            //hselareao = ""; // Params[8].Trim();
            //Commented above and added below two lines to resolve bug in sharing
            hselindo = Params[7].Trim();
            hselareao = Params[8].Trim();
            Area = Params[9].Trim();
            Indicator = Params[10].Trim();
            if (Params.Length >= 12)
            AdaptationURL = Params[11].Trim();
            if (AdaptationURL != "")
            {
                try
                {
                    CatalogQdsResults.QdsResults objResults = new CatalogQdsResults.QdsResults();
                    objResults.Url = AdaptationURL + @"/libraries/ws/QdsResults.asmx";
                    string CatalogParams = "";
                    CatalogParams = DivHtml + Constants.Delimiters.ParamDelimiter + hdsby + Constants.Delimiters.ParamDelimiter + hdbnid + Constants.Delimiters.ParamDelimiter + hselarea + Constants.Delimiters.ParamDelimiter + hselind + Constants.Delimiters.ParamDelimiter + hlngcode + Constants.Delimiters.ParamDelimiter + hlngcodedb + Constants.Delimiters.ParamDelimiter + hselindo + Constants.Delimiters.ParamDelimiter + hselareao + Constants.Delimiters.ParamDelimiter + Area+ Constants.Delimiters.ParamDelimiter + Indicator + Constants.Delimiters.ParamDelimiter + "";
                    Params = Global.SplitString(CatalogParams, Constants.Delimiters.ParamDelimiter);
                    RetVal = objResults.getShareCatalogResults(CatalogParams);
                }
                catch (Exception ex) {
                    Global.CreateExceptionString(ex, null);
                }

                return RetVal;

            }
            
            GalleryParams = Indicator + Constants.Delimiters.ParamDelimiter + Area + Constants.Delimiters.ParamDelimiter + hlngcode + Constants.Delimiters.ParamDelimiter + hdbnid + Constants.Delimiters.ParamDelimiter + UserNId;
            GalleryXML = GetGalleryThumbnails(GalleryParams);
            RetVal = Guid.NewGuid().ToString();

            Global.GetAppSetting();
            //For Language Handling
            XmlDocument LangSharedPageGal, LangSharedPageHome;
            LangSharedPageGal = null;
            LangSharedPageHome = null;
            //string langResultsCloudView = "Cloud View";
            //string langResultsVisualize = "Visualize";
            //string langResultsDimensions = "Dimensions";
            //string langResultsDataValues = "Data values";
            string langResultsCloudView = "";
            string langResultsVisualize = "";
            string langResultsDimensions = "";
            string langResultsDataValues = "";
            string lang_ppup_Next = "Next";
            string lang_ppup_Prev = "Previous";
            string langAreas = "Areas";
            string langIndicators = "Indicators";
            string langUDKs = "Keywords";
            string langkeywords = "Keywords";
            string langGalleryModifiedOn = "Modified On  ";
            string langGalleryDescription = "Description  ";
            string langGalleryMore = "more";
            string langGalleryView = "";
            string langGalleryDownload = "";
            string langGalleryShare = "";
            string langGalleryDelete = "";
            string langMRDFor = "";
            string setShowCloud = Global.show_cloud;
            try
            {
                LangSharedPageGal = new XmlDocument();
                LangSharedPageGal.Load(Server.MapPath(@"~\stock\language\" + hlngcode.ToString() + @"\Gallery.xml"));
                foreach (XmlNode Keyword in LangSharedPageGal.GetElementsByTagName("lng"))
                {
                    if (Keyword.Attributes["id"].Value.ToUpper() == "LANGMOREAREAS")
                    {
                        langAreas = Keyword.Attributes["val"].Value;
                    }
                    if (Keyword.Attributes["id"].Value.ToUpper() == "LANGMOREINDICATORS")
                    {
                        langIndicators = Keyword.Attributes["val"].Value;
                    }
                    if (Keyword.Attributes["id"].Value.ToUpper() == "LANGMOREUDKS")
                    {
                        langUDKs = Keyword.Attributes["val"].Value;
                    }
                    if (Keyword.Attributes["id"].Value.ToLower() == "lang_ppup_next")
                    {
                        lang_ppup_Next = Keyword.Attributes["val"].Value;
                    }
                    if (Keyword.Attributes["id"].Value.ToLower() == "lang_ppup_prev")
                    {
                        lang_ppup_Prev = Keyword.Attributes["val"].Value;
                    }
                    if (Keyword.Attributes["id"].Value.ToLower() == "langkeywords ")
                    {
                        langkeywords = Keyword.Attributes["val"].Value;
                    }
                    if (Keyword.Attributes["id"].Value.ToLower() == "langgallerymodifiedon")
                    {
                        langGalleryModifiedOn = Keyword.Attributes["val"].Value;
                    }
                    if (Keyword.Attributes["id"].Value.ToLower() == "langgallerydescription")
                    {
                        langGalleryDescription = Keyword.Attributes["val"].Value;
                    }
                    if (Keyword.Attributes["id"].Value.ToLower() == "langgallerymore")
                    {
                        langGalleryMore = Keyword.Attributes["val"].Value;
                    }
                }
                LangSharedPageHome = new XmlDocument();
                LangSharedPageHome.Load(Server.MapPath(@"~\stock\language\" + hlngcode.ToString() + @"\Home.xml"));
                foreach (XmlNode Keyword in LangSharedPageHome.GetElementsByTagName("lng"))
                {
                    if (Keyword.Attributes["id"].Value.ToLower() == "langresultscloudview")
                    {
                        langResultsCloudView = Keyword.Attributes["val"].Value;
                    }
                    if (Keyword.Attributes["id"].Value.ToLower() == "langresultsvisualize")
                    {
                        langResultsVisualize = Keyword.Attributes["val"].Value;
                    }
                    if (Keyword.Attributes["id"].Value.ToLower() == "langresultsdimensions")
                    {
                        langResultsDimensions = Keyword.Attributes["val"].Value;
                    }
                    if (Keyword.Attributes["id"].Value.ToLower() == "langresultsdatavalues")
                    {
                        langResultsDataValues = Keyword.Attributes["val"].Value;
                    }
                    if (Keyword.Attributes["id"].Value.ToLower() == "home_view")
                    {
                        langGalleryView = Keyword.Attributes["val"].Value;
                    }
                    if (Keyword.Attributes["id"].Value.ToLower() == "home_download")
                    {
                        langGalleryDownload = Keyword.Attributes["val"].Value;
                    }
                    if (Keyword.Attributes["id"].Value.ToLower() == "home_share")
                    {
                        langGalleryShare = Keyword.Attributes["val"].Value;
                    }
                    if (Keyword.Attributes["id"].Value.ToLower() == "home_delete")
                    {
                        langGalleryDelete = Keyword.Attributes["val"].Value;
                    }
                    if (Keyword.Attributes["id"].Value.ToLower() == "langmrdfor")
                    {
                        langMRDFor = Keyword.Attributes["val"].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                Global.CreateExceptionString(ex, null);
            }
            //Till Here
            // Json structure fixes for single quote issues
            DivHtml = DivHtml.Replace("'", @"\'");

            TemplateHtml = File.ReadAllText(Server.MapPath("../../stock/shared/qds/QDS_Template.htm"));
            TemplateHtml = TemplateHtml.Replace("QDS_DIV", DivHtml);
            TemplateHtml = TemplateHtml.Replace("QDS_TITLE", Global.adaptation_name + " - Search");
            TemplateHtml = TemplateHtml.Replace("DI_DIUILIB_URL", Global.diuilib_url);
            TemplateHtml = TemplateHtml.Replace("DI_COMPONENT_VERSION", Global.diuilib_version);
            TemplateHtml = TemplateHtml.Replace("DI_THEME_CSS", Global.diuilib_theme_css);

            TemplateHtml = TemplateHtml.Replace("src=\"..\\..\\", "src=\"..\\..\\..\\");
            TemplateHtml = TemplateHtml.Replace("..%5C..%5C", "..%5C..%5C..%5C");

            TemplateHtml = TemplateHtml.Replace("<u>share</u>", "");
            TemplateHtml = TemplateHtml.Replace(" | ", "&nbsp;&nbsp;");
            TemplateHtml = TemplateHtml.Replace("<img style=\"cursor: pointer;\" id=\"ATDV_", "<img style=\"cursor: pointer; display:none;\" id=\"ATDV_");
            TemplateHtml = TemplateHtml.Replace("QDS_hdsby", "'" + hdsby + "'");
            TemplateHtml = TemplateHtml.Replace("QDS_hdbnid", "'" + hdbnid + "'");
            TemplateHtml = TemplateHtml.Replace("QDS_hselarea", "'" + hselarea + "'");
            TemplateHtml = TemplateHtml.Replace("QDS_hselind", "'" + hselind + "'");
            TemplateHtml = TemplateHtml.Replace("QDS_hlngcode", "'" + hlngcode + "'");
            TemplateHtml = TemplateHtml.Replace("QDS_db_hlngcode", "'" + hlngcodedb + "'");
            TemplateHtml = TemplateHtml.Replace("QDS_o_hselind", "'" + Global.formatQuoteString(hselindo) + "'");
            TemplateHtml = TemplateHtml.Replace("QDS_o_hselarea", "'" + Global.formatQuoteString(hselareao) + "'");
            TemplateHtml = TemplateHtml.Replace("GALLERY_XML_DATA",GalleryXML.Replace('"', '\''));
            TemplateHtml = TemplateHtml.Replace("langHomeDimensions", langResultsDimensions);
            TemplateHtml = TemplateHtml.Replace("langHomedatavalues", langResultsDataValues);
            TemplateHtml = TemplateHtml.Replace("langHomeCloudview",langResultsCloudView);
            TemplateHtml = TemplateHtml.Replace("langHomeVisualize",langResultsVisualize);
            TemplateHtml = TemplateHtml.Replace("langGalleryIndicators",langIndicators);
            TemplateHtml = TemplateHtml.Replace("langGalleryKeywords",langUDKs);
            TemplateHtml = TemplateHtml.Replace("morelangGallery",langGalleryMore);
            TemplateHtml = TemplateHtml.Replace("ModOnlangGallery",langGalleryModifiedOn);
            TemplateHtml = TemplateHtml.Replace("DeslangGallery",langGalleryDescription);
            TemplateHtml = TemplateHtml.Replace("langGalleryAreas",langAreas);
            TemplateHtml = TemplateHtml.Replace("langGalleryPrevious",lang_ppup_Prev);
            TemplateHtml = TemplateHtml.Replace("langGalleryNext",lang_ppup_Next);
            TemplateHtml = TemplateHtml.Replace("langPopUpGalleryKeyword",langkeywords);
            TemplateHtml = TemplateHtml.Replace("langGalleryView", langGalleryView);
            TemplateHtml = TemplateHtml.Replace("langGalleryDownload", langGalleryDownload);
            TemplateHtml = TemplateHtml.Replace("langGalleryShare", langGalleryShare);
            TemplateHtml = TemplateHtml.Replace("langGalleryDelete", langGalleryDelete);
            TemplateHtml = TemplateHtml.Replace("langHomeMRDFor", langMRDFor);
            TemplateHtml = TemplateHtml.Replace("setShowCloud", setShowCloud);

            //TemplateHtml = TemplateHtml.Replace("onPageLoadOfQDS(QDS_hdsby, QDS_hdbnid, QDS_hselarea, QDS_hselind, QDS_hlngcode, QDS_hlngcodedb, QDS_hselindo, QDS_hselareao, );",
                                                //"onPageLoadOfQDS('" + hdsby + "','" + hdbnid + "','" + hselarea + "','" + hselind + "','" + hlngcode +
                                               // "','" + hlngcodedb + "','" + hselindo + "','" + hselareao + "');");

            File.WriteAllText(Server.MapPath("../../stock/shared/qds/") + RetVal + ".htm", TemplateHtml);
        }
        catch (Exception ex)
        {
            RetVal = ex.Message;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }
  
    public string GetCloudContent(string requestParam)
    {
        string RetVal;
        string[] Params;
        string Language, DVCloudContent;
        int IndicatorNId, UnitNId, AreaNId, DBNId;
        DataTable DtDVCloud;

        RetVal = string.Empty;
        DVCloudContent = string.Empty;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            IndicatorNId = Convert.ToInt32(Params[0].Trim());
            UnitNId = Convert.ToInt32(Params[1].Trim());
            AreaNId = Convert.ToInt32(Params[2].Trim());
            Language = Params[3].Trim();
            DBNId = Convert.ToInt32(Params[4].Trim());

            DtDVCloud = this.GetCloudTable(IndicatorNId, UnitNId, Language, DBNId);

            if (DtDVCloud != null && DtDVCloud.Rows.Count > 0)
            {
                RetVal = this.GetCloudContent(DtDVCloud, DBNId);
            }
        }
        catch (Exception ex)
        {
            RetVal = ex.Message;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string GetViewDataChildDivHtml(string requestParam)
    {
        string RetVal;
        string[] Params;
        string IdUniquePart, Area, Indicator, Unit, DBNId, DVNIds;

        RetVal = string.Empty;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            IdUniquePart = Params[0].Trim();
            Area = Params[1].Trim();
            Indicator = Params[2].Trim();
            Unit = Params[3].Trim();
            DBNId = Params[4].Trim();
            DVNIds = Params[5].Trim();

            RetVal = this.GetViewDataChildDiv(IdUniquePart, Area, Indicator, Unit, DBNId, DVNIds);
        }
        catch (Exception ex)
        {
            RetVal = ex.Message;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string GetSelectedDatabasesDetails(string requestParam)
    {
        string RetVal;
        string[] Params;
        string DbNIds, Language, SelectedDBNames;
        string[] DbDetails;

        RetVal = string.Empty;
        SelectedDBNames = string.Empty;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DbNIds = Params[0].Trim();
            Language = Params[1].Trim();

            if (DbNIds.Contains(","))
            {
                RetVal = "Multiple Databases Selected" + Constants.Delimiters.ParamDelimiter;

                foreach (string DbNId in DbNIds.Split(','))
                {
                    DbDetails = Global.GetDbDetails(DbNId, Language);
                    SelectedDBNames += DbDetails[0] + ", ";
                }

                SelectedDBNames = SelectedDBNames.Substring(0, SelectedDBNames.Length - 2);

                if (SelectedDBNames.Length > 150)
                {
                    RetVal += SelectedDBNames.Substring(0, 150) + "... more";
                }
                else
                {
                    RetVal += SelectedDBNames;
                }
            }
            else
            {
                DbDetails = Global.GetDbDetails(DbNIds, Language);
                RetVal = DbDetails[0];
                RetVal += Constants.Delimiters.ParamDelimiter + DbDetails[1].Split('_')[DbDetails[1].Split('_').Length - 1] + " data values";
                RetVal += " updated on " + DbDetails[3];
            }
        }
        catch (Exception ex)
        {
            RetVal = ex.Message;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    private static void CreateCacheResults(string Language, int DBNId, Page currentPage, string AdditionalParameter) // Object objLangDBNId
    {
        //Page currentPage = (Page)objLangDBNId;
        DataTable RetVal;
        DIConnection DIConnection;
        System.Data.Common.DbParameter AreaParam, IndicatorNIdParam, AdditionalParam, BlockAreaNIdParam;
        List<System.Data.Common.DbParameter> DbParams;

        DIConnection = null;

        //string AdditionalParameter = string.Empty;
        AdditionalParameter = AdditionalParameter.ToUpper();


        try
        {
            DIConnection = Global.GetDbConnection(DBNId);

            DataTable AllAreas = GetQsAreas(Language, DBNId, currentPage);
            
            DataTable AllIndicators = DIConnection.ExecuteDataTable("sp_get_all_indicators_" + Language, CommandType.StoredProcedure, null);

            DataTable AllBlockAreas = DIConnection.ExecuteDataTable("sp_get_all_blockAreas_" + Language, CommandType.StoredProcedure, null);
                        
            #region Creating cache results for QS areas
            // Loop for creating cache results for QDS areas only
            for (int currentAreaRow = 0; currentAreaRow < AllAreas.Rows.Count; currentAreaRow++)
            {
                string Area = string.Empty;
                try
                {

                    Area = AllAreas.Rows[currentAreaRow]["Area_NId"].ToString();
                    int Area_Children = int.Parse(AllAreas.Rows[currentAreaRow]["Children_Count"].ToString());
                    #region Create Cache results for all indicators of current QS area

                    if (Area_Children < 300)
                    {
                        DbParams = new List<System.Data.Common.DbParameter>();

                        AreaParam = DIConnection.CreateDBParameter();
                        AreaParam.ParameterName = "SearchArea";
                        AreaParam.DbType = DbType.String;
                        AreaParam.Value = Area;

                        AdditionalParam = DIConnection.CreateDBParameter();
                        AdditionalParam.ParameterName = "AdditionalParams";
                        AdditionalParam.DbType = DbType.String;
                        AdditionalParam.Value = AdditionalParameter;

                        DbParams.Add(AreaParam);
                        DbParams.Add(AdditionalParam);

                        RetVal = DIConnection.ExecuteDataTable("SP_CREATE_CACHE_RESULT_IU_" + Language, CommandType.StoredProcedure, DbParams);
                    }

                    #endregion
                    #region Create Cache results for 'current indicator' of 'current QS area'

                    else
                    {
                        if (Global.isQdsGeneratedForDensedQS_Areas == "true")
                        {
                            for (int currentIndicatorRow = 0; currentIndicatorRow < AllIndicators.Rows.Count; currentIndicatorRow++)
                            {
                                string IndicatorNId = string.Empty;
                                IndicatorNId = AllIndicators.Rows[currentIndicatorRow]["Indicator_NId"].ToString();

                                DbParams = new List<System.Data.Common.DbParameter>();

                                AreaParam = DIConnection.CreateDBParameter();
                                AreaParam.ParameterName = "SearchArea";
                                AreaParam.DbType = DbType.String;
                                AreaParam.Value = Area;

                                IndicatorNIdParam = DIConnection.CreateDBParameter();
                                IndicatorNIdParam.ParameterName = "IndicatorNId";
                                IndicatorNIdParam.DbType = DbType.Int16;
                                IndicatorNIdParam.Value = int.Parse(IndicatorNId);

                                AdditionalParam = DIConnection.CreateDBParameter();
                                AdditionalParam.ParameterName = "AdditionalParams";
                                AdditionalParam.DbType = DbType.String;
                                AdditionalParam.Value = AdditionalParameter;

                                DbParams.Add(AreaParam);
                                DbParams.Add(IndicatorNIdParam);
                                DbParams.Add(AdditionalParam);

                                RetVal = DIConnection.ExecuteDataTable("SP_CREATE_CACHE_RESULT_IU_QSA_" + Language, CommandType.StoredProcedure, DbParams);
                            }
                        }
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    string tmpError = ex.Message;
                    Global.CreateExceptionString(ex, null);
                    //Global.WriteErrorsInLog(currentAreaRow + " = NOW AREA : " + Area + " , ERROR :" + tmpError);
                }

            }
            #endregion

            #region Creating cache results for all AreaNIds(numeric) of provided indicator

            for (int currentIndicatorRow = 0; currentIndicatorRow < AllIndicators.Rows.Count; currentIndicatorRow++)
            {
                string IndicatorNId = string.Empty;

                try
                {

                    IndicatorNId = AllIndicators.Rows[currentIndicatorRow]["Indicator_NId"].ToString();

                    DbParams = new List<System.Data.Common.DbParameter>();
                    
                    IndicatorNIdParam = DIConnection.CreateDBParameter();
                    IndicatorNIdParam.ParameterName = "SearchIndicator";
                    IndicatorNIdParam.DbType = DbType.Int16;
                    IndicatorNIdParam.Value = int.Parse(IndicatorNId);

                    AdditionalParam = DIConnection.CreateDBParameter();
                    AdditionalParam.ParameterName = "AdditionalParams";
                    AdditionalParam.DbType = DbType.String;
                    AdditionalParam.Value = AdditionalParameter;


                    DbParams.Add(IndicatorNIdParam);
                    DbParams.Add(AdditionalParam);

                    RetVal = DIConnection.ExecuteDataTable("SP_CREATE_CACHE_RESULT_IUA_" + Language, CommandType.StoredProcedure, DbParams);
                }
                catch (Exception ex)
                {
                    string tmpErr2 = ex.Message;
                    Global.CreateExceptionString(ex, null);
                    //Global.WriteErrorsInLog(currentIndicatorRow + " = NOW Indicator, ERROR :" + tmpErr2);
                }

            }

#endregion

            #region Create cache results for Block Areas


            for (int currentBlockAreaRow = 0; currentBlockAreaRow < AllBlockAreas.Rows.Count; currentBlockAreaRow++)
            {
                string BlockArea = string.Empty;

                try
                {
                    BlockArea = AllBlockAreas.Rows[currentBlockAreaRow]["Area_NId"].ToString();
                    DbParams = new List<System.Data.Common.DbParameter>();
                    BlockAreaNIdParam = DIConnection.CreateDBParameter();
                    BlockAreaNIdParam.ParameterName = "SearchArea";
                    BlockAreaNIdParam.DbType = DbType.String;
                    BlockAreaNIdParam.Value = BlockArea;
                    AdditionalParam = DIConnection.CreateDBParameter();
                    AdditionalParam.ParameterName = "AdditionalParams";
                    AdditionalParam.DbType = DbType.String;
                    AdditionalParam.Value = AdditionalParameter;
                    DbParams.Add(BlockAreaNIdParam);
                    DbParams.Add(AdditionalParam);
                    RetVal = DIConnection.ExecuteDataTable("SP_CREATE_CACHE_RESULT_BL_" + Language, CommandType.StoredProcedure, DbParams);
                }
                catch (Exception ex)
                {
                    string tmpErr2 = ex.Message;
                    Global.CreateExceptionString(ex, null);
                    //Global.WriteErrorsInLog(currentBlockAreaRow + " = NOW BlockArea, ERROR :" + tmpErr2);
                }
            }

            #endregion
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

    }
    //Added to check gallery existence
    public int GalleryExistence(string requestParam)
    {
        int RetVal;
        DataTable dtPresentation;
        DIConnection DIConnection;
        string GetPresentationsQuery;
        string[] Params;
        int UserNId ; ;
        int AdminNId ;
        DIConnection = null;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            UserNId = Convert.ToInt32(Params[0].Trim());
            AdminNId = Convert.ToInt32(this.Get_AdminNId());
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                          string.Empty, string.Empty);

            GetPresentationsQuery = "SELECT * FROM Presentations WHERE user_nid = " + UserNId + " OR user_nid = " + AdminNId + "";
            dtPresentation = DIConnection.ExecuteDataTable(GetPresentationsQuery);
            RetVal = dtPresentation.Rows.Count;
            return RetVal;
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

    }

    #endregion "--Public--"

    #endregion "--Methods--"   
}
