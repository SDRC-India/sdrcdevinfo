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
using DevInfo.Lib.DI_LibSDMX;
using SDMXObjectModel;
using SDMXObjectModel.Registry;
using SDMXObjectModel.Common;
using SDMXObjectModel.Message;
using System.Text;
using SDMXObjectModel.Structure;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using SpreadsheetGear;

public partial class Callback : System.Web.UI.Page
{
    #region "--Methods--"

    #region "--Private--"

    private string BindComparisonOfTwoDSDs(string dsd1FileNameWPath, string dsd2FileNameWPath, string hlngcodedb, string ComparisonType)
    {

        StringBuilder RetVal;
        XmlDocument DSD1Xml;
        XmlDocument DSD2Xml;

        List<String> ListOfMissingDimensions;
        List<String> ListOfMissingAttributes;
        List<String> DSD2DimensionList;
        List<String> DSD2AttributeList;
        List<String> DSD1DimensionList;
        List<String> DSD1AttributeList;
        List<String> UnMappedDSD2DimensionList;
        List<String> UnMappedDSD2AttributeList;

        SDMXObjectModel.Structure.DimensionType Dimension;
        SDMXObjectModel.Structure.TimeDimensionType TimeDimension;
        SDMXObjectModel.Structure.AttributeType Attribute;
        SDMXObjectModel.Common.ConceptReferenceType ConceptIdentity;
        SDMXObjectModel.Structure.StructuresType ConceptsObjDSD1;
        SDMXObjectModel.Structure.StructuresType ConceptsObjDSD2;

        int i, j;

        DSD1Xml = new XmlDocument();
        DSD2Xml = new XmlDocument();

        DSD2DimensionList = new List<string>();
        DSD2AttributeList = new List<string>();
        DSD1DimensionList = new List<string>();
        DSD1AttributeList = new List<string>();

        ListOfMissingDimensions = new List<string>();
        ListOfMissingAttributes = new List<string>();
        UnMappedDSD2DimensionList = new List<string>();
        UnMappedDSD2AttributeList = new List<string>();



        string DSD1Dimension = string.Empty;
        string DSD1Attribute = string.Empty;
        string UnmatchedDimensionName = string.Empty;
        string UnmatchedAttributeName = string.Empty;

        try
        {
            DSD1Xml.Load(dsd1FileNameWPath);
            DSD2Xml.Load(dsd2FileNameWPath);

            SDMXObjectModel.Message.StructureType DSD1 = new SDMXObjectModel.Message.StructureType();
            SDMXObjectModel.Message.StructureType DSD2 = new SDMXObjectModel.Message.StructureType();
            SDMXObjectModel.Structure.DataStructureComponentsType DSD1DSComponents = new DataStructureComponentsType();
            SDMXObjectModel.Structure.DataStructureComponentsType DSD2DSComponents = new DataStructureComponentsType();

            DSD1 = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureType), DSD1Xml);
            DSD2 = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureType), DSD2Xml);

            DSD1DSComponents = (SDMXObjectModel.Structure.DataStructureComponentsType)(DSD1.Structures.DataStructures[0].Item);
            DSD2DSComponents = (SDMXObjectModel.Structure.DataStructureComponentsType)(DSD2.Structures.DataStructures[0].Item);

            ConceptsObjDSD1 = DSD1.Structures;
            ConceptsObjDSD2 = DSD2.Structures;

            // Binding DSD2 Dimension in a list - DSD2DimensionList

            for (i = 0; i < DSD2DSComponents.Items[0].Items.Count; i++)
            {

                if (DSD2DSComponents.Items[0].Items[i] is SDMXObjectModel.Structure.TimeDimensionType)
                {
                    TimeDimension = (SDMXObjectModel.Structure.TimeDimensionType)(DSD2DSComponents.Items[0].Items[i]);
                    ConceptIdentity = TimeDimension.ConceptIdentity;

                }
                else
                {
                    Dimension = (SDMXObjectModel.Structure.DimensionType)(DSD2DSComponents.Items[0].Items[i]);
                    ConceptIdentity = Dimension.ConceptIdentity;

                }
                DSD2DimensionList.Add(((SDMXObjectModel.Common.ConceptRefType)(ConceptIdentity.Items[0])).id.ToString());

            }

            // Binding DSD2 Attributes in a list - DSD2AttributeList
            for (i = 0; i < DSD2DSComponents.Items[1].Items.Count; i++)
            {
                Attribute = (SDMXObjectModel.Structure.AttributeType)(DSD2DSComponents.Items[1].Items[i]);
                ConceptIdentity = Attribute.ConceptIdentity;
                DSD2AttributeList.Add(((SDMXObjectModel.Common.ConceptRefType)(ConceptIdentity.Items[0])).id.ToString());


            }


            // Binding Unmatched Dimensions in a list- ListOfMissingDimensions that exist in DSD1 and not in DSD2

            for (i = 0; i < DSD1DSComponents.Items[0].Items.Count; i++)
            {

                if (DSD1DSComponents.Items[0].Items[i] is SDMXObjectModel.Structure.TimeDimensionType)
                {
                    TimeDimension = (SDMXObjectModel.Structure.TimeDimensionType)(DSD1DSComponents.Items[0].Items[i]);
                    ConceptIdentity = TimeDimension.ConceptIdentity;

                }
                else
                {
                    Dimension = (SDMXObjectModel.Structure.DimensionType)(DSD1DSComponents.Items[0].Items[i]);
                    ConceptIdentity = Dimension.ConceptIdentity;

                }
                DSD1Dimension = ((SDMXObjectModel.Common.ConceptRefType)(ConceptIdentity.Items[0])).id.ToString();
                DSD1DimensionList.Add(DSD1Dimension);
                if (!(DSD2DimensionList.Contains(DSD1Dimension)))
                {
                    ListOfMissingDimensions.Add(DSD1Dimension);
                }


            }

            // Binding Unmatched Attributes in a list- ListOfMissingAttributes that exist in DSD1 and not in DSD2

            for (i = 0; i < DSD1DSComponents.Items[1].Items.Count; i++)
            {
                Attribute = (SDMXObjectModel.Structure.AttributeType)(DSD1DSComponents.Items[1].Items[i]);
                ConceptIdentity = Attribute.ConceptIdentity;

                DSD1Attribute = ((SDMXObjectModel.Common.ConceptRefType)(ConceptIdentity.Items[0])).id.ToString();
                DSD1AttributeList.Add(DSD1Attribute);
                if (!(DSD2AttributeList.Contains(DSD1Attribute)))
                {
                    ListOfMissingAttributes.Add(DSD1Attribute);
                }

            }

            // Binding UnMapped DSD2 Dimension in a list - UnMappedDSD2DimensionList

            for (i = 0; i < DSD2DimensionList.Count; i++)
            {
                if (!(DSD1DimensionList.Contains(DSD2DimensionList[i])))
                {
                    UnMappedDSD2DimensionList.Add(DSD2DimensionList[i]);
                }


            }

            // Binding UnMapped DSD2 Attribute in a list - UnMappedDSD2AttributeList

            for (i = 0; i < DSD2AttributeList.Count; i++)
            {
                if (!(DSD1AttributeList.Contains(DSD2AttributeList[i])))
                {
                    UnMappedDSD2AttributeList.Add(DSD2AttributeList[i]);
                }


            }

            //Binding Comparison of the two DSDs - DSD1 and DSD2

            RetVal = new StringBuilder();
            RetVal.Append("<br/>");
            if (ComparisonType == "1")
            {
                RetVal.Append("<h4>Unmatched Dimensions that exist in the Uploaded DSD1 and not in DSD2</h4>");
            }
            else
            {
                RetVal.Append("<h4>Unmatched Dimensions that exist in the Uploaded DSD and not in Master DSD</h4>");
            }
           
            RetVal.Append("<ul>");
            if (ListOfMissingDimensions.Count > 0)
            {
                for (i = 0; i < ListOfMissingDimensions.Count; i++)
                {
                    RetVal.Append("<li>");
                    UnmatchedDimensionName = string.Empty;
                    UnmatchedDimensionName = GetLanguageBasedConceptNameFromConceptScheme(ConceptsObjDSD1, ListOfMissingDimensions[i], hlngcodedb);
                    RetVal.Append("<label id=" + ListOfMissingDimensions[i] + " >" + ListOfMissingDimensions[i] + "</label>");
                    if (UnmatchedDimensionName != string.Empty)
                    {


                        RetVal.Append("<span class=\"reg_li_brac_txt\">(");
                        RetVal.Append(UnmatchedDimensionName);
                        RetVal.Append(")</span> ");

                    }


                    RetVal.Append("&nbsp;&nbsp;<select id='ddlDimensionAlternative_" + ListOfMissingDimensions[i] + "'" + "  class='confg_frm_inp_bx_txt_dd' style='width:350px'>");
                    RetVal.Append("<option value='0'>Select UnMapped Dimensions</option>");
                    for (j = 0; j < UnMappedDSD2DimensionList.Count; j++)
                    {
                        UnmatchedDimensionName = string.Empty;
                        UnmatchedDimensionName = GetLanguageBasedConceptNameFromConceptScheme(ConceptsObjDSD2, UnMappedDSD2DimensionList[j], hlngcodedb);
                        if (UnmatchedDimensionName == string.Empty)
                        {
                            RetVal.Append("<option value='" + UnMappedDSD2DimensionList[j] + "'>" + UnMappedDSD2DimensionList[j] + "</option>");
                        }
                        else
                        {
                            RetVal.Append("<option value='" + UnMappedDSD2DimensionList[j] + "'>");
                            RetVal.Append(UnMappedDSD2DimensionList[j]);
                            RetVal.Append(" (");
                            RetVal.Append(UnmatchedDimensionName);
                            RetVal.Append(")");
                            RetVal.Append("</option>");
                        }
                       

                    }
                    RetVal.Append("</select>");
                    RetVal.Append("</li>");
                }
            }
            else
            {
                RetVal.Append("<span class=\"reg_li_brac_txt\">(");
                RetVal.Append("No Unmatched Dimensions");
                RetVal.Append(")</span> ");
            }

            RetVal.Append("</ul>");
            RetVal.Append("<br/>");
            if (ComparisonType == "1")
            {
                RetVal.Append("<h4>Unmatched Attributes that exist in the Uploaded DSD1 and not in DSD2</h4>");
            }
            else
            {
                RetVal.Append("<h4>Unmatched Attributes that exist in the Uploaded DSD and not in Master DSD</h4>");
            }
            RetVal.Append("<ul>");
            if (ListOfMissingAttributes.Count > 0)
            {
                for (i = 0; i < ListOfMissingAttributes.Count; i++)
                {
                    RetVal.Append("<li>");
                    UnmatchedAttributeName = string.Empty;
                    UnmatchedAttributeName = GetLanguageBasedConceptNameFromConceptScheme(ConceptsObjDSD1, ListOfMissingAttributes[i], hlngcodedb);
                    RetVal.Append("<label id=" + ListOfMissingAttributes[i] + " >" + ListOfMissingAttributes[i] + "</label>");
                    if (UnmatchedAttributeName != string.Empty)
                    {


                        RetVal.Append("<span class=\"reg_li_brac_txt\">(");
                        RetVal.Append(UnmatchedAttributeName);
                        RetVal.Append(")</span> ");

                    }

                    RetVal.Append("&nbsp;&nbsp;<select id='ddlAttributeAlternative_" + ListOfMissingAttributes[i] + "'" + "  class='confg_frm_inp_bx_txt_dd' style='width:350px'>");
                    RetVal.Append("<option value='0'>Select UnMapped Attributes</option>");
                    for (j = 0; j < UnMappedDSD2AttributeList.Count; j++)
                    {
                        UnmatchedAttributeName = string.Empty;
                        UnmatchedAttributeName = GetLanguageBasedConceptNameFromConceptScheme(ConceptsObjDSD2, UnMappedDSD2AttributeList[j], hlngcodedb);
                        if (UnmatchedAttributeName == string.Empty)
                        {
                            RetVal.Append("<option value='" + UnMappedDSD2AttributeList[j] + "'>" + UnMappedDSD2AttributeList[j] + "</option>");
                        }
                        else
                        {
                            RetVal.Append("<option value='" + UnMappedDSD2AttributeList[j] + "'>");
                            RetVal.Append(UnMappedDSD2AttributeList[j]);
                            RetVal.Append(" (");
                            RetVal.Append(UnmatchedAttributeName);
                            RetVal.Append(")");
                            RetVal.Append("</option>");
                        }

                    }
                    RetVal.Append("</select>");
                    RetVal.Append("</li>");
                }
            }
            else
            {
                RetVal.Append("<span class=\"reg_li_brac_txt\">(");
                RetVal.Append("No Unmatched Attributes");
                RetVal.Append(")</span> ");
            }

            RetVal.Append("</ul>");
            RetVal.Append("<br/>");
            RetVal.Append("&nbsp;&nbsp;&nbsp;&nbsp;<input type='button' name='btnCompareCodes' id='btnCompareCodes' value='Compare Codes' class='di_gui_button' style='width:150px;float:left;margin-left:20px;' onclick='CompareCodelists(" + ComparisonType + ");'/>");
            RetVal.Append("&nbsp;&nbsp;&nbsp;&nbsp;<input type='button' name='btnGenerateReport' id='btnGenerateReport' value='Generate Report' class='di_gui_button' style='width:150px;float:left;' onclick='GenerateReport(" + ComparisonType + ");'/>");
            RetVal.Append("&nbsp;&nbsp;<div id='divDownloadReport' class='reg_dwnld_bttn_main'  style='display:none;width:150px;float:left; height:11px;'>");
            RetVal.Append("<p><a id='lnkReport' href='#'>Download Report</a></p>");
            RetVal.Append("</div>");
            RetVal.Append("<br/>");
            RetVal.Append("<br/>");
            RetVal.Append("<br/>");
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;

        }
        finally
        {

        }

        return RetVal.ToString();

    }

    private string GetLanguageBasedConceptNameFromConceptScheme(SDMXObjectModel.Structure.StructuresType ConceptsObj, string UnMatchedConceptId, string hlngcodedb)
    {
        string RetVal;
        int j, k;
        RetVal = string.Empty;

        try
        {

            if (ConceptsObj.Concepts.Count > 0)
            {
                for (j = 0; j < ConceptsObj.Concepts.Count; j++)
                {

                    for (k = 0; k < ConceptsObj.Concepts[j].Items.Count; k++)
                    {
                        if (UnMatchedConceptId == ConceptsObj.Concepts[j].Items[k].id.ToString())
                        {

                            RetVal = GetLangSpecificValue(ConceptsObj.Concepts[j].Items[k].Name, hlngcodedb);

                            break;
                        }
                    }

                }
            }

        }
        catch (Exception ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(ex, null);
            throw ex;


        }
        finally
        {

        }

        return RetVal;

    }

    private string BindCodelistComparison(string dsd1FileNameWPath, string dsd2FileNameWPath, string hlngcodedb, Dictionary<string, string> dictMappedIndicators, Dictionary<string, string> dictMappedAttributes, string ComparisonType)
    {
        StringBuilder RetVal;
        XmlDocument DSD1Xml;
        XmlDocument DSD2Xml;

        List<String> ListOfMissingDimensions;
        List<String> ListOfMissingAttributes;
        List<String> DSD2DimensionList;
        List<String> DSD2AttributeList;
        List<String> DSD1DimensionList;
        List<String> DSD1AttributeList;
        List<String> UnMappedDSD2DimensionList;
        List<String> UnMappedDSD2AttributeList;
        List<CodelistType> DSD1Codelists;
        List<CodelistType> DSD2Codelists;

        SDMXObjectModel.Structure.DimensionType Dimension;
        SDMXObjectModel.Structure.TimeDimensionType TimeDimension;
        SDMXObjectModel.Structure.AttributeType Attribute;
        SDMXObjectModel.Common.ConceptReferenceType ConceptIdentity;
        RepresentationType LocalRepresentation;
        SDMXObjectModel.Structure.StructuresType ConceptsObjDSD1;
        SDMXObjectModel.Structure.StructuresType ConceptsObjDSD2;



        int i, j, k;

        DSD1Xml = new XmlDocument();
        DSD2Xml = new XmlDocument();

        DSD2DimensionList = new List<string>();
        DSD2AttributeList = new List<string>();
        DSD1DimensionList = new List<string>();
        DSD1AttributeList = new List<string>();

        ListOfMissingDimensions = new List<string>();
        ListOfMissingAttributes = new List<string>();
        UnMappedDSD2DimensionList = new List<string>();
        UnMappedDSD2AttributeList = new List<string>();



        string DSD1Dimension = string.Empty;
        string DSD2Dimension = string.Empty;
        string DSD1Attribute = string.Empty;
        string DSD2Attribute = string.Empty;
        string UnmatchedDimensionName = string.Empty;
        string UnmatchedAttributeName = string.Empty;

        string DSD1CodelistId = string.Empty;
        string DSD2CodelistId = string.Empty;

        try
        {
            DSD1Xml.Load(dsd1FileNameWPath);
            DSD2Xml.Load(dsd2FileNameWPath);

            SDMXObjectModel.Message.StructureType DSD1 = new SDMXObjectModel.Message.StructureType();
            SDMXObjectModel.Message.StructureType DSD2 = new SDMXObjectModel.Message.StructureType();
            SDMXObjectModel.Structure.DataStructureComponentsType DSD1DSComponents = new DataStructureComponentsType();
            SDMXObjectModel.Structure.DataStructureComponentsType DSD2DSComponents = new DataStructureComponentsType();

            CodelistType DSD1Codelist = new CodelistType();
            CodelistType DSD2Codelist = new CodelistType();

            DSD1 = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureType), DSD1Xml);
            DSD2 = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureType), DSD2Xml);

            DSD1DSComponents = (SDMXObjectModel.Structure.DataStructureComponentsType)(DSD1.Structures.DataStructures[0].Item);
            DSD2DSComponents = (SDMXObjectModel.Structure.DataStructureComponentsType)(DSD2.Structures.DataStructures[0].Item);

            ConceptsObjDSD1 = DSD1.Structures;
            ConceptsObjDSD2 = DSD2.Structures;

            DSD1Codelists = DSD1.Structures.Codelists;
            DSD2Codelists = DSD2.Structures.Codelists;

            // Binding DSD2 Dimension in a list - DSD2DimensionList

            for (i = 0; i < DSD2DSComponents.Items[0].Items.Count; i++)
            {

                if (DSD2DSComponents.Items[0].Items[i] is SDMXObjectModel.Structure.TimeDimensionType)
                {
                    TimeDimension = (SDMXObjectModel.Structure.TimeDimensionType)(DSD2DSComponents.Items[0].Items[i]);
                    ConceptIdentity = TimeDimension.ConceptIdentity;

                }
                else
                {
                    Dimension = (SDMXObjectModel.Structure.DimensionType)(DSD2DSComponents.Items[0].Items[i]);
                    ConceptIdentity = Dimension.ConceptIdentity;

                }
                DSD2DimensionList.Add(((SDMXObjectModel.Common.ConceptRefType)(ConceptIdentity.Items[0])).id.ToString());

            }

            // Binding DSD2 Attributes in a list - DSD2AttributeList
            for (i = 0; i < DSD2DSComponents.Items[1].Items.Count; i++)
            {
                Attribute = (SDMXObjectModel.Structure.AttributeType)(DSD2DSComponents.Items[1].Items[i]);
                ConceptIdentity = Attribute.ConceptIdentity;
                DSD2AttributeList.Add(((SDMXObjectModel.Common.ConceptRefType)(ConceptIdentity.Items[0])).id.ToString());


            }


            // Binding Matched Dimensions in a dictionary- dictMappedIndicators that exist in DSD1 as well as in DSD2

            for (i = 0; i < DSD1DSComponents.Items[0].Items.Count; i++)
            {

                if (DSD1DSComponents.Items[0].Items[i] is SDMXObjectModel.Structure.TimeDimensionType)
                {
                    TimeDimension = (SDMXObjectModel.Structure.TimeDimensionType)(DSD1DSComponents.Items[0].Items[i]);
                    ConceptIdentity = TimeDimension.ConceptIdentity;

                }
                else
                {
                    Dimension = (SDMXObjectModel.Structure.DimensionType)(DSD1DSComponents.Items[0].Items[i]);
                    ConceptIdentity = Dimension.ConceptIdentity;

                }
                DSD1Dimension = ((SDMXObjectModel.Common.ConceptRefType)(ConceptIdentity.Items[0])).id.ToString();
                DSD1DimensionList.Add(DSD1Dimension);
                if ((DSD2DimensionList.Contains(DSD1Dimension)))
                {
                    dictMappedIndicators.Add(DSD1Dimension, DSD1Dimension);
                }


            }

            // Binding Matched Attributes in a dictionary- dictMappedAttributes that exist in DSD1 as well as in DSD2

            for (i = 0; i < DSD1DSComponents.Items[1].Items.Count; i++)
            {
                Attribute = (SDMXObjectModel.Structure.AttributeType)(DSD1DSComponents.Items[1].Items[i]);
                ConceptIdentity = Attribute.ConceptIdentity;

                DSD1Attribute = ((SDMXObjectModel.Common.ConceptRefType)(ConceptIdentity.Items[0])).id.ToString();
                DSD1AttributeList.Add(DSD1Attribute);
                if ((DSD2AttributeList.Contains(DSD1Attribute)))
                {
                    dictMappedAttributes.Add(DSD1Attribute, DSD1Attribute);
                }

            }


            //Binding Comparison of the two DSDs - DSD1 and DSD2

            RetVal = new StringBuilder();
            RetVal.Append("<br/>");
            foreach (string Indicator in dictMappedIndicators.Keys)
            {
                DSD1CodelistId = string.Empty;
                for (i = 0; i < DSD1DSComponents.Items[0].Items.Count; i++)
                {

                    if (DSD1DSComponents.Items[0].Items[i] is SDMXObjectModel.Structure.DimensionType)
                    {
                        Dimension = (SDMXObjectModel.Structure.DimensionType)(DSD1DSComponents.Items[0].Items[i]);
                        ConceptIdentity = Dimension.ConceptIdentity;
                        LocalRepresentation = Dimension.LocalRepresentation;
                        DSD1Dimension = ((SDMXObjectModel.Common.ConceptRefType)(ConceptIdentity.Items[0])).id.ToString();
                        if (DSD1Dimension == Indicator)
                        {
                            DSD1CodelistId = ((CodelistRefType)((((CodelistReferenceType)(LocalRepresentation.Items[0])).Items[0]))).id;
                            break;
                        }

                    }

                }

                if (string.IsNullOrEmpty(DSD1CodelistId) == false)
                {
                    for (j = 0; j < DSD1Codelists.Count; j++)
                    {
                        if (DSD1CodelistId == DSD1Codelists[j].id)
                        {
                            DSD1Codelist = DSD1Codelists[j];
                            break;
                        }
                    }
                }



                for (i = 0; i < DSD2DSComponents.Items[0].Items.Count; i++)
                {
                    DSD2CodelistId = string.Empty;
                    if (DSD2DSComponents.Items[0].Items[i] is SDMXObjectModel.Structure.DimensionType)
                    {
                        Dimension = (SDMXObjectModel.Structure.DimensionType)(DSD2DSComponents.Items[0].Items[i]);
                        ConceptIdentity = Dimension.ConceptIdentity;
                        LocalRepresentation = Dimension.LocalRepresentation;
                        DSD2Dimension = ((SDMXObjectModel.Common.ConceptRefType)(ConceptIdentity.Items[0])).id.ToString();
                        if (DSD2Dimension == dictMappedIndicators[Indicator])
                        {
                            DSD2CodelistId = ((CodelistRefType)((((CodelistReferenceType)(LocalRepresentation.Items[0])).Items[0]))).id;
                            break;
                        }

                    }

                }
                if (string.IsNullOrEmpty(DSD2CodelistId) == false)
                {
                    for (j = 0; j < DSD2Codelists.Count; j++)
                    {
                        if (DSD2CodelistId == DSD2Codelists[j].id)
                        {
                            DSD2Codelist = DSD2Codelists[j];
                            break;
                        }
                    }
                }
                if ((DSD1CodelistId != string.Empty) && (DSD2CodelistId != string.Empty))
                {

                    if (DSD1Codelist.Items.Count > 0)
                    {
                        RetVal.Append("<a href=\"javascript:void(0);\" id=\"lnk_" + DSD1CodelistId + "\" ");
                    }
                    else
                    {
                        RetVal.Append("<a href=\"javascript:void(0);\" id=\"lnk_" + DSD2CodelistId + "\" ");
                    }

                    RetVal.Append(" onclick=\"ToggleExpandCollapse(event);\"");
                    RetVal.Append(" class=\"collapse\" style=\"padding-left:40px;\">" + DSD1CodelistId + "&nbsp;&nbsp;Vs&nbsp;&nbsp;" + DSD2CodelistId + "</a>");
                    RetVal.Append("<br/>");
                    RetVal.Append(BindCodesComparison(DSD1Codelist, DSD2Codelist, hlngcodedb, ComparisonType));
                    RetVal.Append("<br/>");

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

        return RetVal.ToString();
    }

    private string BindCodesComparison(CodelistType Codelist1, CodelistType Codelist2, string hlngcodedb, string ComparisonType)
    {
        StringBuilder RetVal;
        List<CodeType> ListOfMissingCodes;
        List<String> ListOfDSD2Codes;
        int i;

        RetVal = new StringBuilder();
        ListOfMissingCodes = new List<CodeType>();
        ListOfDSD2Codes = new List<string>();

        CodeType Code = new CodeType();
        string CodeId = string.Empty;
        string UnmatchedCodeName = string.Empty;


        try
        {
            for (i = 0; i < Codelist2.Items.Count; i++)
            {
                Code = ((CodeType)(Codelist2.Items[i]));
                CodeId = Code.id;
                ListOfDSD2Codes.Add(CodeId);

            }

            for (i = 0; i < Codelist1.Items.Count; i++)
            {
                Code = ((CodeType)(Codelist1.Items[i]));
                CodeId = Code.id;
                if (!(ListOfDSD2Codes.Contains(CodeId)))
                {
                    ListOfMissingCodes.Add(Code);
                }

            }

            if (Codelist1.Items.Count > 0)
            {
                RetVal.Append("<div id=\"div_" + Codelist1.id + "\" style=\"display:none; \">");
            }
            else
            {
                RetVal.Append("<div id=\"div_" + Codelist2.id + "\" style=\"display:none; \">");
            }

            RetVal.Append("<br/>");

            if (Codelist1.Items.Count > 0)
            {
                if (ListOfMissingCodes.Count > 0)
                {
                    if (ComparisonType == "1")
                    {
                        RetVal.Append("<h4>Unmatched Codes that exist in Uploaded DSD1 and not in DSD2 </h4>");
                    }
                    else
                    {
                        RetVal.Append("<h4>Unmatched Codes that exist in Uploaded DSD and not in Master DSD </h4>");
                    }
                   
                    RetVal.Append("<br/>");
                    RetVal.Append("<ul>");
                    for (i = 0; i < ListOfMissingCodes.Count; i++)
                    {
                        RetVal.Append("<li>");
                        UnmatchedCodeName = GetLangSpecificValue(ListOfMissingCodes[i].Name, hlngcodedb);
                        RetVal.Append(ListOfMissingCodes[i].id);
                        if (UnmatchedCodeName != string.Empty)
                        {


                            RetVal.Append("<span class=\"reg_li_brac_txt\" style=\"padding-left:40px;\">(");
                            RetVal.Append(UnmatchedCodeName);
                            RetVal.Append(")</span> ");

                        }
                        RetVal.Append("</li>");
                    }
                    RetVal.Append("</ul>");
                }
                else
                {
                    RetVal.Append("<span class=\"reg_li_brac_txt\" style=\"padding-left:40px;\">");
                    RetVal.Append("No Unmatched Codes");
                    RetVal.Append("</span> ");

                }

            }
            else
            {
                RetVal.Append("<span class=\"reg_li_brac_txt\" style=\"padding-left:40px;\">");
                RetVal.Append("Codelist does not exists in the Uploaded DSD.");
                RetVal.Append("</span> ");
            }


            RetVal.Append("</div>");


        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;

        }
        finally
        {

        }

        return RetVal.ToString();
    }

    private string SaveComparisonReport(string dsd1FileNameWPath, string dsd2FileNameWPath, string hlngcodedb, Dictionary<string, string> dictMappedIndicators, Dictionary<string, string> dictMappedAttributes)
    {
        string RetVal;
        XmlDocument DSD1Xml;
        XmlDocument DSD2Xml;

        List<String> ListOfMissingDimensions;
        List<String> ListOfMissingAttributes;
        List<String> DSD2DimensionList;
        List<String> DSD2AttributeList;
        List<String> DSD1DimensionList;
        List<String> DSD1AttributeList;
        List<String> AdditionalDSD1DimensionList;
        List<String> AdditionalDSD1AttributeList;
        List<CodelistType> DSD1Codelists;
        List<CodelistType> DSD2Codelists;
        RepresentationType LocalRepresentation;

        SDMXObjectModel.Structure.DimensionType Dimension;
        SDMXObjectModel.Structure.TimeDimensionType TimeDimension;
        SDMXObjectModel.Structure.AttributeType Attribute;
        SDMXObjectModel.Common.ConceptReferenceType ConceptIdentity;
        SDMXObjectModel.Structure.StructuresType ConceptsObjDSD1;
        SDMXObjectModel.Structure.StructuresType ConceptsObjDSD2;

        int i,j,SheetIndex;
        RetVal = string.Empty;
        DSD1Xml = new XmlDocument();
        DSD2Xml = new XmlDocument();

        DSD2DimensionList = new List<string>();
        DSD2AttributeList = new List<string>();
        DSD1DimensionList = new List<string>();
        DSD1AttributeList = new List<string>();

        ListOfMissingDimensions = new List<string>();
        ListOfMissingAttributes = new List<string>();
        AdditionalDSD1DimensionList = new List<string>();
        AdditionalDSD1AttributeList = new List<string>();


        string DSD1Dimension = string.Empty;
        string DSD2Dimension = string.Empty;
        string DSD1Attribute = string.Empty;
        string DSD2Attribute = string.Empty;
        string UnmatchedDimensionName = string.Empty;
        string UnmatchedAttributeName = string.Empty;

        string DSD1CodelistId = string.Empty;
        string DSD2CodelistId = string.Empty;

        DIExcel ReportExcel = new DIExcel();
        string tempPath = string.Empty;
        string FileName = string.Empty;
        string FilePath = string.Empty;


        try
        {
            DSD1Xml.Load(dsd1FileNameWPath);
            DSD2Xml.Load(dsd2FileNameWPath);

            SDMXObjectModel.Message.StructureType DSD1 = new SDMXObjectModel.Message.StructureType();
            SDMXObjectModel.Message.StructureType DSD2 = new SDMXObjectModel.Message.StructureType();
            SDMXObjectModel.Structure.DataStructureComponentsType DSD1DSComponents = new DataStructureComponentsType();
            SDMXObjectModel.Structure.DataStructureComponentsType DSD2DSComponents = new DataStructureComponentsType();

            CodelistType DSD1Codelist = new CodelistType();
            CodelistType DSD2Codelist = new CodelistType();

            DSD1 = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureType), DSD1Xml);
            DSD2 = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureType), DSD2Xml);

            DSD1DSComponents = (SDMXObjectModel.Structure.DataStructureComponentsType)(DSD1.Structures.DataStructures[0].Item);
            DSD2DSComponents = (SDMXObjectModel.Structure.DataStructureComponentsType)(DSD2.Structures.DataStructures[0].Item);

            ConceptsObjDSD1 = DSD1.Structures;
            ConceptsObjDSD2 = DSD2.Structures;

            DSD1Codelists = DSD1.Structures.Codelists;
            DSD2Codelists = DSD2.Structures.Codelists;

            // Binding DSD2 Dimension in a list - DSD2DimensionList

            for (i = 0; i < DSD2DSComponents.Items[0].Items.Count; i++)
            {

                if (DSD2DSComponents.Items[0].Items[i] is SDMXObjectModel.Structure.TimeDimensionType)
                {
                    TimeDimension = (SDMXObjectModel.Structure.TimeDimensionType)(DSD2DSComponents.Items[0].Items[i]);
                    ConceptIdentity = TimeDimension.ConceptIdentity;

                }
                else
                {
                    Dimension = (SDMXObjectModel.Structure.DimensionType)(DSD2DSComponents.Items[0].Items[i]);
                    ConceptIdentity = Dimension.ConceptIdentity;

                }
                DSD2DimensionList.Add(((SDMXObjectModel.Common.ConceptRefType)(ConceptIdentity.Items[0])).id.ToString());

            }

            // Binding DSD2 Attributes in a list - DSD2AttributeList
            for (i = 0; i < DSD2DSComponents.Items[1].Items.Count; i++)
            {
                Attribute = (SDMXObjectModel.Structure.AttributeType)(DSD2DSComponents.Items[1].Items[i]);
                ConceptIdentity = Attribute.ConceptIdentity;
                DSD2AttributeList.Add(((SDMXObjectModel.Common.ConceptRefType)(ConceptIdentity.Items[0])).id.ToString());


            }


            // Binding Matched Dimensions in a dictionary- dictMappedIndicators that exist in DSD1 as well as in DSD2
            // and unmatched Dimensions in a list-AdditionalDSD1DimensionList that exist in DSD1 but not in DSD2

            for (i = 0; i < DSD1DSComponents.Items[0].Items.Count; i++)
            {

                if (DSD1DSComponents.Items[0].Items[i] is SDMXObjectModel.Structure.TimeDimensionType)
                {
                    TimeDimension = (SDMXObjectModel.Structure.TimeDimensionType)(DSD1DSComponents.Items[0].Items[i]);
                    ConceptIdentity = TimeDimension.ConceptIdentity;

                }
                else
                {
                    Dimension = (SDMXObjectModel.Structure.DimensionType)(DSD1DSComponents.Items[0].Items[i]);
                    ConceptIdentity = Dimension.ConceptIdentity;

                }
                DSD1Dimension = ((SDMXObjectModel.Common.ConceptRefType)(ConceptIdentity.Items[0])).id.ToString();
                DSD1DimensionList.Add(DSD1Dimension);
                if ((DSD2DimensionList.Contains(DSD1Dimension)))
                {
                    dictMappedIndicators.Add(DSD1Dimension, DSD1Dimension);
                }
                else
                {
                    AdditionalDSD1DimensionList.Add(DSD1Dimension);
                }


            }

            // Binding Matched Attributes in a dictionary- dictMappedAttributes that exist in DSD1 as well as in DSD2
            // and unmatched Attributes in a list-AdditionalDSD1AttributeList that exist in DSD1 but not in DSD2

            for (i = 0; i < DSD1DSComponents.Items[1].Items.Count; i++)
            {
                Attribute = (SDMXObjectModel.Structure.AttributeType)(DSD1DSComponents.Items[1].Items[i]);
                ConceptIdentity = Attribute.ConceptIdentity;

                DSD1Attribute = ((SDMXObjectModel.Common.ConceptRefType)(ConceptIdentity.Items[0])).id.ToString();
                DSD1AttributeList.Add(DSD1Attribute);
                if ((DSD2AttributeList.Contains(DSD1Attribute)))
                {
                    dictMappedAttributes.Add(DSD1Attribute, DSD1Attribute);
                }
                else
                {
                    AdditionalDSD1AttributeList.Add(DSD1Attribute);
                }

            }

            //Binding Missing Dimensions in a list-ListOfMissingDimensions that exist in DSD2 but not in DSD1

            for (i = 0; i < DSD2DimensionList.Count; i++)
            {
                if (!(DSD1DimensionList.Contains(DSD2DimensionList[i])))
                {
                    ListOfMissingDimensions.Add(DSD2DimensionList[i]);
                }
            }

            //Binding Missing Attributes in a list-ListOfMissingAttributes that exist in DSD2 but not in DSD1

            for (i = 0; i < DSD2AttributeList.Count; i++)
            {
                if (!(DSD1AttributeList.Contains(DSD2AttributeList[i])))
                {
                    ListOfMissingAttributes.Add(DSD2AttributeList[i]);
                }
            }


            //Binding Comparison of the two DSDs - DSD1 and DSD2

            tempPath= Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath,"stock\\tempSDMXFiles");
            FileName = "ComparisonReport_" + Guid.NewGuid().ToString() + ".xls";
            FilePath = tempPath + "\\" + FileName;
            ReportExcel = this.GenerateDimensionAndAttributesComparison(hlngcodedb,DSD1, DSD2, ReportExcel, dictMappedIndicators, ListOfMissingDimensions, AdditionalDSD1DimensionList, dictMappedAttributes, ListOfMissingAttributes, AdditionalDSD1AttributeList);
            SheetIndex = 2;
            foreach (string Indicator in dictMappedIndicators.Keys)
            {
                DSD1CodelistId = string.Empty;
                for (i = 0; i < DSD1DSComponents.Items[0].Items.Count; i++)
                {

                    if (DSD1DSComponents.Items[0].Items[i] is SDMXObjectModel.Structure.DimensionType)
                    {
                        Dimension = (SDMXObjectModel.Structure.DimensionType)(DSD1DSComponents.Items[0].Items[i]);
                        ConceptIdentity = Dimension.ConceptIdentity;
                        LocalRepresentation = Dimension.LocalRepresentation;
                        DSD1Dimension = ((SDMXObjectModel.Common.ConceptRefType)(ConceptIdentity.Items[0])).id.ToString();
                        if (DSD1Dimension == Indicator)
                        {
                            DSD1CodelistId = ((CodelistRefType)((((CodelistReferenceType)(LocalRepresentation.Items[0])).Items[0]))).id;
                            break;
                        }

                    }

                }

                if (string.IsNullOrEmpty(DSD1CodelistId) == false)
                {
                    for (j = 0; j < DSD1Codelists.Count; j++)
                    {
                        if (DSD1CodelistId == DSD1Codelists[j].id)
                        {
                            DSD1Codelist = DSD1Codelists[j];
                            break;
                        }
                    }
                }


                for (i = 0; i < DSD2DSComponents.Items[0].Items.Count; i++)
                {
                    DSD2CodelistId = string.Empty;
                    if (DSD2DSComponents.Items[0].Items[i] is SDMXObjectModel.Structure.DimensionType)
                    {
                        Dimension = (SDMXObjectModel.Structure.DimensionType)(DSD2DSComponents.Items[0].Items[i]);
                        ConceptIdentity = Dimension.ConceptIdentity;
                        LocalRepresentation = Dimension.LocalRepresentation;
                        DSD2Dimension = ((SDMXObjectModel.Common.ConceptRefType)(ConceptIdentity.Items[0])).id.ToString();
                        if (DSD2Dimension == dictMappedIndicators[Indicator])
                        {
                            DSD2CodelistId = ((CodelistRefType)((((CodelistReferenceType)(LocalRepresentation.Items[0])).Items[0]))).id;
                            break;
                        }

                    }

                }
                if (string.IsNullOrEmpty(DSD2CodelistId) == false)
                {
                    for (j = 0; j < DSD2Codelists.Count; j++)
                    {
                        if (DSD2CodelistId == DSD2Codelists[j].id)
                        {
                            DSD2Codelist = DSD2Codelists[j];
                            break;
                        }
                    }
                }
                if ((DSD1CodelistId != string.Empty) && (DSD2CodelistId != string.Empty))
                {

                    if( (DSD1Codelist.Items.Count > 0) && (DSD2Codelist.Items.Count > 0) )
                    {
                        ReportExcel.InsertWorkSheet(DSD1Codelist.id + " Vs " + DSD2Codelist.id);
                        ReportExcel = this.GenerateCodelistsComparison(hlngcodedb, DSD1Codelist, DSD2Codelist, ReportExcel, SheetIndex);
                        SheetIndex = SheetIndex + 1;
                    }
                  
                }

            }
            ReportExcel.ActiveSheetIndex = 0;
            if (ReportExcel.AvailableWorksheetsCount > 1)
            {
               
                ReportExcel.SaveAs(FilePath);
                RetVal = FileName;

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

    private DIExcel GenerateDimensionAndAttributesComparison(string hlngcodedb,SDMXObjectModel.Message.StructureType DSD1, SDMXObjectModel.Message.StructureType DSD2, DIExcel ReportExcel, Dictionary<string, string> dictMappedIndicators, List<String> ListOfMissingDimensions, List<String> AdditionalDSD1DimensionList, Dictionary<string, string> dictMappedAttributes, List<String> ListOfMissingAttributes, List<String> AdditionalDSD1AttributeList)
    {
       
        int rowindex = 0;
        string DimensionName = string.Empty;
        string AttributeName = string.Empty;
        int SNo;
        SDMXObjectModel.Structure.DataStructureComponentsType DSD1DSComponents;
        SDMXObjectModel.Structure.DataStructureComponentsType DSD2DSComponents;
        SDMXObjectModel.Structure.StructuresType ConceptsObjDSD1;
        SDMXObjectModel.Structure.StructuresType ConceptsObjDSD2;
        IWorksheet WorkbookSheet = null;

        try
        {
            DSD1DSComponents = (SDMXObjectModel.Structure.DataStructureComponentsType)(DSD1.Structures.DataStructures[0].Item);
            DSD2DSComponents = (SDMXObjectModel.Structure.DataStructureComponentsType)(DSD2.Structures.DataStructures[0].Item);

            ConceptsObjDSD1 = DSD1.Structures;
            ConceptsObjDSD2 = DSD2.Structures;

            ReportExcel.RenameWorkSheet(0, "Dimensions");
            // Writing into Dimensions Worksheet

            // Writing Matched Dimensions that exist in both DSD1 and DSD2
            rowindex = rowindex + 1;
            WorkbookSheet = ReportExcel.GetWorksheet(0);
            this.WriteValueInCell(ReportExcel, "Matched Dimensions", rowindex, 1, 14, true, 10, 0, 0);
            WorkbookSheet.Cells[rowindex, 1, rowindex, 2].Merge();
            rowindex = rowindex + 2;
            this.WriteValueInCell(ReportExcel, "S.No.", rowindex, 1, 12, true, 10, 0, 0);
            this.WriteValueInCell(ReportExcel, "ID", rowindex, 2, 12, true, 60, 0, 0);
            this.WriteValueInCell(ReportExcel, "Name", rowindex,3, 12, true, 70, 0, 0);
            rowindex = rowindex + 1;
            SNo = 0;
            foreach (string MatchedDimension in dictMappedIndicators.Keys)
            {
                if(!(AdditionalDSD1DimensionList.Contains(MatchedDimension)))
                {
                    SNo = SNo + 1;
                    this.WriteValueInCell(ReportExcel, SNo.ToString(), rowindex, 1, 10, false, 10, 0, 0);
                    DimensionName = string.Empty;
                    DimensionName = GetLanguageBasedConceptNameFromConceptScheme(ConceptsObjDSD1, MatchedDimension, hlngcodedb);
                    this.WriteValueInCell(ReportExcel, MatchedDimension, rowindex, 2, 10, false, 60, 0, 0);
                    if (DimensionName != string.Empty)
                    {
                        this.WriteValueInCell(ReportExcel,DimensionName, rowindex, 3, 10, false, 70, 0, 0);
                    }
                   

                    rowindex = rowindex + 1;
                }

            }

            // Writing Missing Dimensions that exist in DSD2 but not in DSD1

            rowindex = rowindex + 2;
            this.WriteValueInCell(ReportExcel, "Missing Dimensions", rowindex, 1, 14, true, 10, 0, 0);
            WorkbookSheet.Cells[rowindex, 1, rowindex, 2].Merge();
            rowindex = rowindex + 2;
            this.WriteValueInCell(ReportExcel, "S.No.", rowindex, 1, 12, true, 10, 0, 0);
            this.WriteValueInCell(ReportExcel, "ID", rowindex, 2, 12, true, 60, 0, 0);
            this.WriteValueInCell(ReportExcel, "Name", rowindex, 3, 12, true, 70, 0, 0);
            rowindex = rowindex + 1;

            SNo = 0;
            foreach (string MissingDimension in ListOfMissingDimensions)
            {
                SNo = SNo + 1;
                this.WriteValueInCell(ReportExcel, SNo.ToString(), rowindex, 1, 10, false, 10, 0, 0);
                DimensionName = string.Empty;
                DimensionName = GetLanguageBasedConceptNameFromConceptScheme(ConceptsObjDSD2, MissingDimension, hlngcodedb);
                this.WriteValueInCell(ReportExcel, MissingDimension, rowindex, 2, 10, false, 60, 0, 0);
                if (DimensionName != string.Empty)
                {
                    this.WriteValueInCell(ReportExcel, DimensionName, rowindex, 3, 10, false, 70, 0, 0);
                }
                rowindex = rowindex + 1;

            }

            // Writing Additional Dimensions that exist in DSD1 but not in DSD2

            rowindex = rowindex + 2;
            this.WriteValueInCell(ReportExcel, "Additional Dimensions", rowindex, 1, 14, true, 10, 0, 0);
            WorkbookSheet.Cells[rowindex, 1, rowindex, 2].Merge();
            rowindex = rowindex + 2;
            this.WriteValueInCell(ReportExcel, "S.No.", rowindex, 1, 12, true, 10, 0, 0);
            this.WriteValueInCell(ReportExcel, "ID", rowindex, 2, 12, true, 60, 0, 0);
            this.WriteValueInCell(ReportExcel, "Name", rowindex, 3, 12, true, 70, 0, 0);
            rowindex = rowindex + 1;

            SNo = 0;
            foreach (string AdditionalDimension in AdditionalDSD1DimensionList)
            {
                SNo = SNo + 1;
                this.WriteValueInCell(ReportExcel, SNo.ToString(), rowindex, 1, 10, false, 10, 0, 0);
                DimensionName = string.Empty;
                DimensionName = GetLanguageBasedConceptNameFromConceptScheme(ConceptsObjDSD1, AdditionalDimension, hlngcodedb);
                this.WriteValueInCell(ReportExcel, AdditionalDimension, rowindex, 2, 10, false, 60, 0, 0);
                if (DimensionName != string.Empty)
                {
                    this.WriteValueInCell(ReportExcel, DimensionName, rowindex, 3, 10, false, 70, 0, 0);
                }
               
                rowindex = rowindex + 1;

            }

            // Writing into Attributes Worksheet

            ReportExcel.InsertWorkSheet("Attributes");
            WorkbookSheet = ReportExcel.GetWorksheet(1);
            // Writing Matched Attributes that exist in both DSD1 and DSD2

            rowindex = 1;
            this.WriteValueInCell(ReportExcel, "Matched Attributes", rowindex, 1, 14, true, 10, 0, 1);
            WorkbookSheet.Cells[rowindex, 1, rowindex, 2].Merge();
            rowindex = rowindex + 2;
            this.WriteValueInCell(ReportExcel, "S.No.", rowindex, 1, 12, true, 10, 0,1);
            this.WriteValueInCell(ReportExcel, "ID", rowindex, 2, 12, true, 60, 0, 1);
            this.WriteValueInCell(ReportExcel, "Name", rowindex, 3, 12, true, 70, 0, 1);
            rowindex = rowindex + 1;

            SNo = 0;
            foreach (string MatchedAttribute in dictMappedAttributes.Keys)
            {
                if (!(AdditionalDSD1AttributeList.Contains(MatchedAttribute)))
                {
                    SNo = SNo + 1;
                    this.WriteValueInCell(ReportExcel, SNo.ToString(), rowindex, 1, 10, false,10, 0, 1);
                    AttributeName = string.Empty;
                    AttributeName = GetLanguageBasedConceptNameFromConceptScheme(ConceptsObjDSD1, MatchedAttribute, hlngcodedb);
                    this.WriteValueInCell(ReportExcel, MatchedAttribute, rowindex, 2, 10, false, 60, 0, 1);
                    if (AttributeName != string.Empty)
                    {
                        this.WriteValueInCell(ReportExcel, AttributeName, rowindex, 3, 10, false, 70, 0, 1);
                    }
                    rowindex = rowindex + 1;
                }
            }

            // Writing Missing Attributes that exist in DSD2 but not in DSD1
            rowindex = rowindex + 2;
            this.WriteValueInCell(ReportExcel, "Missing Attributes", rowindex, 1, 14, true, 10, 0, 1);
            WorkbookSheet.Cells[rowindex, 1, rowindex, 2].Merge();
            rowindex = rowindex + 2;
            this.WriteValueInCell(ReportExcel, "S.No.", rowindex, 1, 12, true, 10, 0, 1);
            this.WriteValueInCell(ReportExcel, "ID", rowindex, 2, 12, true, 60, 0,1);
            this.WriteValueInCell(ReportExcel, "Name", rowindex, 3, 12, true, 70, 0, 1);
            rowindex = rowindex + 1;

            SNo = 0;
            foreach (string MissingAttribute in ListOfMissingAttributes)
            {
                SNo = SNo + 1;
                this.WriteValueInCell(ReportExcel, SNo.ToString(), rowindex, 1, 10, false, 10, 0, 1);
                AttributeName = string.Empty;
                AttributeName = GetLanguageBasedConceptNameFromConceptScheme(ConceptsObjDSD2, MissingAttribute, hlngcodedb);
                this.WriteValueInCell(ReportExcel, MissingAttribute, rowindex, 2, 10, false, 60, 0, 1);
                if (AttributeName != string.Empty)
                {
                    this.WriteValueInCell(ReportExcel, AttributeName, rowindex, 3, 10, false, 70, 0, 1);
                }
                rowindex = rowindex + 1;
            }

            // Writing Additional Attributes that exist in DSD1 but not in DSD2

            rowindex = rowindex + 2;
            this.WriteValueInCell(ReportExcel, "Additional Attributes", rowindex, 1, 14, true, 10, 0, 1);
            WorkbookSheet.Cells[rowindex, 1, rowindex, 2].Merge();
            rowindex = rowindex + 2;
            this.WriteValueInCell(ReportExcel, "S.No.", rowindex, 1, 12, true, 10, 0, 1);
            this.WriteValueInCell(ReportExcel, "ID", rowindex, 2, 12, true, 60, 0, 1);
            this.WriteValueInCell(ReportExcel, "Name", rowindex, 3, 12, true, 70, 0, 1);
            rowindex = rowindex + 1;

            SNo = 0;
            foreach (string AdditionalAttribute in AdditionalDSD1AttributeList)
            {
                SNo = SNo + 1;
                this.WriteValueInCell(ReportExcel, SNo.ToString(), rowindex, 1, 10, false, 10, 0, 1);
                AttributeName = string.Empty;
                AttributeName = GetLanguageBasedConceptNameFromConceptScheme(ConceptsObjDSD1, AdditionalAttribute, hlngcodedb);
                this.WriteValueInCell(ReportExcel, AdditionalAttribute, rowindex, 2, 10, false, 60, 0, 1);
                if (AttributeName != string.Empty)
                {
                    this.WriteValueInCell(ReportExcel, AttributeName, rowindex, 3, 10, false, 70, 0, 1);
                }
                rowindex = rowindex + 1;
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

        return ReportExcel;

    }

    private DIExcel GenerateCodelistsComparison(string hlngcodedb, CodelistType Codelist1, CodelistType Codelist2, DIExcel ReportExcel,int SheetIndex)
    {


        List<CodeType> ListOfMatchedCodes;
        List<CodeType> ListOfMissingCodes;
        List<CodeType> ListOfAdditionalCodelist1Codes;
        List<String> ListOfDSD1Codes;
        List<String> ListOfDSD2Codes;
        string CodeName = string.Empty;
        int i, SNo, rowindex;

        ListOfDSD1Codes = new List<string>();
        ListOfDSD2Codes = new List<string>();

        ListOfMatchedCodes = new List<CodeType>();
        ListOfMissingCodes = new List<CodeType>();
        ListOfAdditionalCodelist1Codes = new List<CodeType>();

        i = 0;
        SNo = 0;
        rowindex = 0;
       
        CodeType Code = new CodeType();
        string CodeId = string.Empty;
        string UnmatchedCodeName = string.Empty;
        IWorksheet WorkbookSheet = null;

        try
        {
            for (i = 0; i < Codelist2.Items.Count; i++)
            {
                Code = ((CodeType)(Codelist2.Items[i]));
                CodeId = Code.id;
                ListOfDSD2Codes.Add(CodeId);

            }

            for (i = 0; i < Codelist1.Items.Count; i++)
            {
                Code = ((CodeType)(Codelist1.Items[i]));
                CodeId = Code.id;
                ListOfDSD1Codes.Add(CodeId);
                if (!(ListOfDSD2Codes.Contains(CodeId)))
                {
                    ListOfAdditionalCodelist1Codes.Add(Code);
                }
                else
                {
                    ListOfMatchedCodes.Add(Code);
                }

            }

            for (i = 0; i < Codelist2.Items.Count; i++)
            {
                Code = ((CodeType)(Codelist2.Items[i]));
                CodeId = Code.id;

                if (!(ListOfDSD1Codes.Contains(CodeId)))
                {
                    ListOfMissingCodes.Add(Code);
                }
            }

            // Writing Matched Codes that exist in both Codelist1 and Codelist2

            rowindex = rowindex + 1;
            WorkbookSheet = ReportExcel.GetWorksheet(SheetIndex);
            this.WriteValueInCell(ReportExcel, "Matched Codes", rowindex, 1, 14, true, 10, 0, SheetIndex);
           
            WorkbookSheet.Cells[rowindex, 1, rowindex,2].Merge();
            rowindex = rowindex + 2;
            this.WriteValueInCell(ReportExcel, "S.No.", rowindex, 1, 12, true, 10, 0, SheetIndex);
            this.WriteValueInCell(ReportExcel, "ID", rowindex, 2, 12, true, 60, 0, SheetIndex);
            this.WriteValueInCell(ReportExcel, "Name", rowindex, 3, 12, true, 70, 0, SheetIndex);
            rowindex = rowindex + 1;
            SNo = 0;
            foreach (CodeType MatchedCode in ListOfMatchedCodes)
            {
                   SNo = SNo + 1;
                   this.WriteValueInCell(ReportExcel, SNo.ToString(), rowindex, 1, 10, false, 10, 0, SheetIndex);
                   CodeName = string.Empty;
                   CodeName = GetLangSpecificValue(ListOfMatchedCodes[SNo - 1].Name, hlngcodedb);
                   this.WriteValueInCell(ReportExcel, MatchedCode.id, rowindex, 2, 10, false,60, 0, SheetIndex);
                    if (CodeName != string.Empty)
                    {
                        this.WriteValueInCell(ReportExcel, CodeName, rowindex, 3, 10, false, 70, 0, SheetIndex);
                    }
                    rowindex = rowindex + 1;
            }

            // Writing Missing Codes that exist in Codelist2 but not in Codelist1

            rowindex = rowindex + 2;
            this.WriteValueInCell(ReportExcel, "Missing Codes", rowindex, 1, 14, true,10, 0, SheetIndex);
            WorkbookSheet.Cells[rowindex, 1, rowindex, 2].Merge();
            rowindex = rowindex + 2;
            this.WriteValueInCell(ReportExcel, "S.No.", rowindex, 1, 12, true, 10, 0, SheetIndex);
            this.WriteValueInCell(ReportExcel, "ID", rowindex, 2, 12, true, 60, 0, SheetIndex);
            this.WriteValueInCell(ReportExcel, "Name", rowindex, 3, 12, true, 70, 0, SheetIndex);
            rowindex = rowindex + 1;
            SNo = 0;
            foreach (CodeType MissingCode in ListOfMissingCodes)
            {
                SNo = SNo + 1;
                this.WriteValueInCell(ReportExcel, SNo.ToString(), rowindex, 1, 10, false, 10, 0, SheetIndex);
                CodeName = string.Empty;
                CodeName = GetLangSpecificValue(ListOfMissingCodes[SNo - 1].Name, hlngcodedb);
                this.WriteValueInCell(ReportExcel, MissingCode.id, rowindex, 2, 10, false,60, 0, SheetIndex);
                if (CodeName != string.Empty)
                {
                    this.WriteValueInCell(ReportExcel, CodeName, rowindex, 3, 10, false, 70, 0, SheetIndex);
                }
                rowindex = rowindex + 1;
            }

            // Writing Additional Codes that exist in Codelist1 but not in Codelist2

            rowindex = rowindex + 2;
            this.WriteValueInCell(ReportExcel, "Additional Codes", rowindex, 1, 14, true, 10, 0, SheetIndex);
            WorkbookSheet.Cells[rowindex, 1, rowindex, 2].Merge();
            rowindex = rowindex + 2;
            this.WriteValueInCell(ReportExcel, "S.No.", rowindex, 1, 12, true, 10, 0, SheetIndex);
            this.WriteValueInCell(ReportExcel, "ID", rowindex, 2, 12, true, 60, 0, SheetIndex);
            this.WriteValueInCell(ReportExcel, "Name", rowindex, 3, 12, true, 70, 0, SheetIndex);
            rowindex = rowindex + 1;
            SNo = 0;
            foreach (CodeType AdditionalCode in ListOfAdditionalCodelist1Codes)
            {
                SNo = SNo + 1;
                this.WriteValueInCell(ReportExcel, SNo.ToString(), rowindex, 1, 10, false, 10, 0, SheetIndex);
                CodeName = string.Empty;
                CodeName = GetLangSpecificValue(ListOfAdditionalCodelist1Codes[SNo - 1].Name, hlngcodedb);
                this.WriteValueInCell(ReportExcel, AdditionalCode.id, rowindex, 2, 10, false, 60, 0, SheetIndex);
                if (CodeName != string.Empty)
                {
                    this.WriteValueInCell(ReportExcel, CodeName, rowindex, 3, 10, false, 70, 0, SheetIndex);
                }
                rowindex = rowindex + 1;
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

        return ReportExcel;

    }

    private void WriteValueInCell(DIExcel Workbook, string value, int rowNum, int colNum, int fontSize, bool boldFlag, double columnWidth, double rowHeight, int Sheetindex)
    {
        IFont WorkbookFont = null;
        IWorksheet WorkbookSheet = null;

        WorkbookFont = Workbook.GetCellFont(Sheetindex, rowNum, colNum);
        WorkbookSheet = Workbook.GetWorksheet(Sheetindex);


        WorkbookFont.Size = fontSize;
        WorkbookFont.Bold = boldFlag;
        
       
        WorkbookSheet.Cells[rowNum, colNum].Value = value;
        WorkbookSheet.Cells[rowNum, colNum].WrapText = true;
        WorkbookSheet.Cells[rowNum, colNum].ColumnWidth = columnWidth;

        if (rowHeight != 0)
        {
            WorkbookSheet.Cells[rowNum, colNum].RowHeight = rowHeight;
        }
    }

    #endregion "--Private--"

    #region "--Public--"

    public string CompareUserDSDAgainstDevInfoDSD(string requestParam)
    {
        string RetVal;
        string DbNId = string.Empty;
        string hlngcodedb = string.Empty;
        string dsdFileNameWPath;
        string devinfodsdFileNameWPath;
        string tempPath = string.Empty;
        string[] Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
        RetVal = string.Empty;
        DbNId = Params[0];
        hlngcodedb = Params[1];
        dsdFileNameWPath = Params[2];
        try
        {


            devinfodsdFileNameWPath = Server.MapPath(Path.Combine("~", @"stock\\data\\" + DbNId + "\\sdmx\\Complete.xml"));
            RetVal = BindComparisonOfTwoDSDs(dsdFileNameWPath, devinfodsdFileNameWPath, hlngcodedb, "2");

        }
        catch (Exception ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(ex, null);
            throw ex;


        }
        finally
        {

        }

        return RetVal;

    }

    public string CompareUserDSD1AgainstUserDSD2(string requestParam)
    {
        string RetVal;
        string DbNId = string.Empty;
        string hlngcodedb = string.Empty;
        string dsd1FileNameWPath;
        string dsd2FileNameWPath;
        string tempPath = string.Empty;
        string[] Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
        RetVal = string.Empty;
        DbNId = Params[0];
        hlngcodedb = Params[1];
        dsd1FileNameWPath = Params[2];
        dsd2FileNameWPath = Params[3];
        try
        {

            RetVal = BindComparisonOfTwoDSDs(dsd1FileNameWPath, dsd2FileNameWPath, hlngcodedb, "1");

        }
        catch (Exception ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(ex, null);
            throw ex;


        }
        finally
        {

        }

        return RetVal;

    }

    public string CompareCodelists(string requestParam)
    {
        string RetVal;
        string DbNId = string.Empty;
        string hlngcodedb = string.Empty;
        string strComparisonType = string.Empty;
        string strMissingDimensionsMapping = string.Empty;
        string strMissingAttributesMapping = string.Empty;

        string MissingDimension = string.Empty;
        string MissingAttribute = string.Empty;
        string MappedDimension = string.Empty;
        string MappedAttribute = string.Empty;

        string dsd1FileNameWPath;
        string dsd2FileNameWPath;
        string tempPath = string.Empty;

        Dictionary<string, string> dictMappedIndicators;
        Dictionary<string, string> dictMappedAttributes;

        int i;

        string[] Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
        RetVal = string.Empty;
        DbNId = Params[0];
        hlngcodedb = Params[1];
        strComparisonType = Params[2];
        if (strComparisonType == "1")
        {
            dsd1FileNameWPath = Global.SplitString(Params[3], Constants.Delimiters.PivotRowDelimiter)[0];
            dsd2FileNameWPath = Global.SplitString(Params[3], Constants.Delimiters.PivotRowDelimiter)[1];
        }
        else
        {
            dsd1FileNameWPath = Params[3];
            dsd2FileNameWPath = Server.MapPath(Path.Combine("~", @"stock\\data\\" + DbNId + "\\sdmx\\Complete.xml"));
        }

        if (Params.Length > 4)
        {
            strMissingDimensionsMapping = Params[4];
        }

        if (Params.Length > 5)
        {
            strMissingAttributesMapping = Params[5];
        }



        dictMappedIndicators = new Dictionary<string, string>();
        dictMappedAttributes = new Dictionary<string, string>();

        try
        {

            if (strMissingDimensionsMapping != string.Empty)
            {
                for (i = 0; i < Global.SplitString(strMissingDimensionsMapping, Constants.Delimiters.PivotRowDelimiter).Length; i++)
                {
                    MissingDimension = Global.SplitString(Global.SplitString(strMissingDimensionsMapping, Constants.Delimiters.PivotRowDelimiter)[i], Constants.Delimiters.Comma)[0];
                    MappedDimension = Global.SplitString(Global.SplitString(strMissingDimensionsMapping, Constants.Delimiters.PivotRowDelimiter)[i], Constants.Delimiters.Comma)[1];
                    dictMappedIndicators.Add(MissingDimension, MappedDimension);
                }
            }


            if (strMissingAttributesMapping != string.Empty)
            {
                for (i = 0; i < Global.SplitString(strMissingAttributesMapping, Constants.Delimiters.PivotRowDelimiter).Length; i++)
                {
                    MissingAttribute = Global.SplitString(Global.SplitString(strMissingAttributesMapping, Constants.Delimiters.PivotRowDelimiter)[i], Constants.Delimiters.Comma)[0];
                    MappedAttribute = Global.SplitString(Global.SplitString(strMissingAttributesMapping, Constants.Delimiters.PivotRowDelimiter)[i], Constants.Delimiters.Comma)[1];
                    dictMappedAttributes.Add(MissingAttribute, MappedAttribute);
                }
            }

            RetVal = BindCodelistComparison(dsd1FileNameWPath, dsd2FileNameWPath, hlngcodedb, dictMappedIndicators, dictMappedAttributes, strComparisonType);
        }
        catch (Exception ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(ex, null);
            throw ex;


        }
        finally
        {

        }

        return RetVal;

    }

    public string GenerateComparisonReport(string requestParam)
    {
        string RetVal;
        string DbNId = string.Empty;
        string hlngcodedb = string.Empty;
        string strComparisonType = string.Empty;
        string strMissingDimensionsMapping = string.Empty;
        string strMissingAttributesMapping = string.Empty;

        string MissingDimension = string.Empty;
        string MissingAttribute = string.Empty;
        string MappedDimension = string.Empty;
        string MappedAttribute = string.Empty;

        string dsd1FileNameWPath;
        string dsd2FileNameWPath;
        string tempPath = string.Empty;

        Dictionary<string, string> dictMappedIndicators;
        Dictionary<string, string> dictMappedAttributes;

        int i;

        string[] Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
        RetVal = string.Empty;
        DbNId = Params[0];
        hlngcodedb = Params[1];
        strComparisonType = Params[2];
        if (strComparisonType == "1")
        {
            dsd1FileNameWPath = Global.SplitString(Params[3], Constants.Delimiters.PivotRowDelimiter)[0];
            dsd2FileNameWPath = Global.SplitString(Params[3], Constants.Delimiters.PivotRowDelimiter)[1];
        }
        else
        {
            dsd1FileNameWPath = Params[3];
            dsd2FileNameWPath = Server.MapPath(Path.Combine("~", @"stock\\data\\" + DbNId + "\\sdmx\\Complete.xml"));
        }

        if (Params.Length > 4)
        {
            strMissingDimensionsMapping = Params[4];
        }

        if (Params.Length > 5)
        {
            strMissingAttributesMapping = Params[5];
        }



        dictMappedIndicators = new Dictionary<string, string>();
        dictMappedAttributes = new Dictionary<string, string>();

        try
        {

            if (strMissingDimensionsMapping != string.Empty)
            {
                for (i = 0; i < Global.SplitString(strMissingDimensionsMapping, Constants.Delimiters.PivotRowDelimiter).Length; i++)
                {
                    MissingDimension = Global.SplitString(Global.SplitString(strMissingDimensionsMapping, Constants.Delimiters.PivotRowDelimiter)[i], Constants.Delimiters.Comma)[0];
                    MappedDimension = Global.SplitString(Global.SplitString(strMissingDimensionsMapping, Constants.Delimiters.PivotRowDelimiter)[i], Constants.Delimiters.Comma)[1];
                    dictMappedIndicators.Add(MissingDimension, MappedDimension);
                }
            }


            if (strMissingAttributesMapping != string.Empty)
            {
                for (i = 0; i < Global.SplitString(strMissingAttributesMapping, Constants.Delimiters.PivotRowDelimiter).Length; i++)
                {
                    MissingAttribute = Global.SplitString(Global.SplitString(strMissingAttributesMapping, Constants.Delimiters.PivotRowDelimiter)[i], Constants.Delimiters.Comma)[0];
                    MappedAttribute = Global.SplitString(Global.SplitString(strMissingAttributesMapping, Constants.Delimiters.PivotRowDelimiter)[i], Constants.Delimiters.Comma)[1];
                    dictMappedAttributes.Add(MissingAttribute, MappedAttribute);
                }
            }

            RetVal = SaveComparisonReport(dsd1FileNameWPath, dsd2FileNameWPath, hlngcodedb, dictMappedIndicators, dictMappedAttributes);
        }
        catch (Exception ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(ex, null);
            throw ex;


        }
        finally
        {

        }

        return RetVal;

    }


    #endregion "--Public--"

    #endregion "--Methods--"
}
