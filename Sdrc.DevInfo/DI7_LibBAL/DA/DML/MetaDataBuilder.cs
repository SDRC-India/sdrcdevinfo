using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.VisualBasic;

using DevInfo.Lib.DI_LibDAL.Connection;
using DALQueries = DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.IO;
using System.Xml;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using DevInfo.Lib.DI_LibBAL.Metadata;
using SpreadsheetGear;
using DevInfo.Lib.DI_LibBAL.Controls.MetadataEditorBAL;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{/// <summary>
    /// Provides methods for creating and importing metadata into database.
    /// </summary>
    public class MetaDataBuilder : ProcessEventCreator
    {
        #region "-- Private --"

        #region "-- Variables --"

        protected DIConnection DBConnection;
        protected DIQueries DBQueries;

        private const int ElementSheetIndex = 0;
        private const int ElementGIdRowIndex = 2;
        private const int ElementGIdColumnIndex = 0;
        private const int ElementNameRowIndex = 1;
        private const int ElementNameColumnIndex = 0;
        private const string METADATA_CATEGORYTAG = "metadata/Category";
        private const string METADATA_CATNAMETAG = "@name";
        // private const string METADATA_NAMEAttribute = "name";
        #endregion

        #region "-- Methods --"

        /// <summary> 
        /// Update Metadata text inside database 
        /// </summary> 
        /// <param name="metadataType">Metadata Type. For IC types Sector may be set as code remains for all IC types</param> 
        /// <param name="elementGId">IndicatorGId / LayerName / ICName</param> 
        /// <param name="elementNId">IndicatorNId, LayerNId, ICNId</param> 
        /// <param name="metadataInfo">Metadata content as String</param> 
        /// <returns>Bool value to indicate success or failure</returns> 
        /// <remarks>Used in frmMDO_Metadata, frmMDO_Edit, frmDL_InfoEdit</remarks> 
        internal bool UpdateMetadataInfo(MetaDataType metadataType, string elementGId, int elementNId, string metadataInfo)
        {
            bool RetVal = false;
            try
            {
                string SqlQuery = string.Empty;

                switch (metadataType)
                {
                    case MetaDataType.Indicator:
                        if (elementGId.Length > 0)
                        {
                            SqlQuery = "UPDATE " + this.DBQueries.TablesName.Indicator + " SET " + Indicator.IndicatorInfo + " = @MDInfo WHERE " + Indicator.IndicatorGId + " ='" + DICommon.RemoveQuotes(elementGId) + "'";
                        }
                        else if (elementNId != -1)
                        {
                            SqlQuery = "UPDATE " + this.DBQueries.TablesName.Indicator + " SET " + Indicator.IndicatorInfo + " = @MDInfo WHERE " + Indicator.IndicatorNId + " =" + elementNId;
                        }
                        break;

                    case MetaDataType.Map:
                        if (elementGId.Length > 0)
                        {
                            SqlQuery = "UPDATE " + this.DBQueries.TablesName.AreaMapMetadata + " SET " + Area_Map_Metadata.MetadataText + " = @MDInfo WHERE " + Area_Map_Metadata.LayerName + " ='" + DICommon.RemoveQuotes(elementGId) + "'";
                        }
                        else if (elementNId != -1)
                        {
                            SqlQuery = "UPDATE " + this.DBQueries.TablesName.AreaMapMetadata + " SET " + Area_Map_Metadata.MetadataText + " = @MDInfo WHERE " + Area_Map_Metadata.LayerNId + " =" + elementNId;
                        }
                        break;

                    case MetaDataType.Sector:
                    case MetaDataType.Goal:
                    case MetaDataType.CF:
                    case MetaDataType.Theme:
                    case MetaDataType.Source:
                    case MetaDataType.Institution:
                    case MetaDataType.Convention:
                        if (elementGId.Length > 0)
                        {
                            SqlQuery = "UPDATE " + this.DBQueries.TablesName.IndicatorClassifications + " SET " + IndicatorClassifications.ICInfo + " = @MDInfo WHERE " + IndicatorClassifications.ICGId + " ='" + DICommon.RemoveQuotes(elementGId) + "'";
                        }
                        else if (elementNId != -1)
                        {
                            SqlQuery = "UPDATE " + this.DBQueries.TablesName.IndicatorClassifications + " SET " + IndicatorClassifications.ICInfo + " = @MDInfo WHERE " + IndicatorClassifications.ICNId + "=" + elementNId;
                        }
                        break;

                }

                if (SqlQuery.Length > 0)
                {

                    //Create Command object 
                    DbCommand Command = this.DBConnection.GetCurrentDBProvider().CreateCommand();
                    Command.Connection = this.DBConnection.GetConnection();
                    Command.CommandType = CommandType.Text;
                    Command.CommandText = SqlQuery;

                    //Create Parameter 
                    DbParameter Parameter = this.DBConnection.GetCurrentDBProvider().CreateParameter();
                    {
                        Parameter.ParameterName = "@MDInfo";
                        //the name used in the query for the parameter 

                        //set the data type 
                        //http://www.carlprothman.net/Default.aspx?tabid=97 (Data Type Mapping) 
                        //OleDbType.LongVarChar - OLE DB Provider: Microsoft.Jet.OLEDB.3.51, Access 97 (3.5 format) 
                        //OleDbType.VarWChar - OLE DB Provider: Microsoft.Jet.OLEDB.4.0 , Access 2000 (4.0 format) 
                        //Parameter.DbType = DbType.VarNumeric;

                        //No need to remove quotes. Handled automatically 
                        //assign the contents of the Info to the value of the parameter 
                        Parameter.Value = metadataInfo;
                    }
                    //Add Parameter to command object 
                    Command.Parameters.Add(Parameter);


                    //Create Command object 
                    //OleDb.OleDbCommand cmd = new OleDb.OleDbCommand(SqlQuery, (OleDb.OleDbConnection)MD_DB.GetConnection); 
                    //cmd.CommandType = CommandType.Text; 

                    //Create Parameter 
                    //OleDb.OleDbParameter paramInfo = new OleDb.OleDbParameter(); 
                    //{ 
                    //    paramInfo.ParameterName = "@MDInfo"; 
                    //    //the name used in the query for the parameter 

                    //    //set the data type 
                    //    //http://www.carlprothman.net/Default.aspx?tabid=97 (Data Type Mapping) 
                    //    //OleDbType.LongVarChar - OLE DB Provider: Microsoft.Jet.OLEDB.3.51, Access 97 (3.5 format) 
                    //    //OleDbType.VarWChar - OLE DB Provider: Microsoft.Jet.OLEDB.4.0 , Access 2000 (4.0 format) 
                    //    paramInfo.OleDbType = OleDb.OleDbType.VarWChar; 

                    //    //No need to remove quotes. Handled automatically 
                    //    paramInfo.Value = metadataInfo; 
                    //    //assign the contents of the Info to the value of the parameter 
                    //} 

                    //Add Parameter to command object 
                    //cmd.Parameters.Add(paramInfo); 
                    //add the parameter to the command 

                    //Open connection if closed 
                    //if (((OleDb.OleDbConnection)MD_DB.GetConnection).State == ConnectionState.Broken | ((OleDb.OleDbConnection)MD_DB.GetConnection).State == ConnectionState.Closed) { 
                    //    ((OleDb.OleDbConnection)MD_DB.GetConnection).Open(); 
                    //} 

                    //Execute the command - this saves the image to the database 
                    if (Command.ExecuteNonQuery() > 0)
                    {
                        RetVal = true;
                    }

                    //Dispose Command 
                    if ((Command != null))
                    {
                        Command.Dispose();
                        Command = null;
                    }

                }
            }


            catch (Exception ex)
            {
                RetVal = false;
            }

            return RetVal;
        }

        private int GetElementNId(string excelFilePath, MetaDataType elementType, int elementNameRowIndex)
        {
            int RetVal = 0;
            string ElementName = string.Empty;
            string ElementGId = string.Empty;
            SpreadsheetGear.IWorksheet MetadataSheet;
            IndicatorBuilder IndBuilder = null;
            IndicatorInfo IndInfo = new IndicatorInfo();
            DataTable Table = null;
            int GIdRowIndex = elementNameRowIndex + 1;
            //-- Open excel File and get first worksheet
            DIExcel DiExcel = new DIExcel(excelFilePath);
            MetadataSheet = DiExcel.GetWorksheet(0);
            //-- Get Element GId
            ElementGId = DiExcel.GetCellValue(ElementSheetIndex, GIdRowIndex, ElementGIdColumnIndex, GIdRowIndex, ElementGIdColumnIndex);
            //-- Get Element Name
            ElementName = DiExcel.GetCellValue(ElementSheetIndex, elementNameRowIndex, ElementNameColumnIndex, elementNameRowIndex, ElementNameColumnIndex);

            //-- Get GId By Name if GID is blank in Excel File
            if (!string.IsNullOrEmpty(ElementGId) || !string.IsNullOrEmpty(ElementName))
            {
                switch (elementType)
                {
                    case MetaDataType.Indicator:
                        IndBuilder = new IndicatorBuilder(this.DBConnection, this.DBQueries);
                        RetVal = IndBuilder.GetIndicatorNid(ElementGId, ElementName);

                        break;
                    case MetaDataType.Map:
                        Table = this.DBConnection.ExecuteDataTable(this.DBQueries.Area.GetAreaMapMetadataByName(string.IsNullOrEmpty(ElementGId) ? ElementName : ElementGId));
                        if (Table.Rows.Count > 0)
                        {
                            RetVal = Convert.ToInt32(Table.Rows[0][Area_Map_Metadata.LayerNId]);
                        }
                        break;
                    case MetaDataType.Source:
                    case MetaDataType.Sector:
                    case MetaDataType.Goal:
                    case MetaDataType.CF:
                    case MetaDataType.Theme:
                    case MetaDataType.Institution:
                    case MetaDataType.Convention:
                    case MetaDataType.IndicatorClassification:
                        Table = this.DBConnection.ExecuteDataTable(this.DBQueries.IndicatorClassification.GetIC(FilterFieldType.GId, "'" + ElementGId + "'", FieldSelection.Heavy));
                        if (Table.Rows.Count > 0)
                        {
                            RetVal = Convert.ToInt32(Table.Rows[0][IndicatorClassifications.ICNId]);
                        }
                        break;
                    default:
                        break;
                }
            }

            return RetVal;
        }

        private string GetElementGId(string excelFilePath, MetaDataType elementType, int elementNameRowIndex)
        {
            string RetVal = string.Empty;
            string ElementName = string.Empty;
            SpreadsheetGear.IWorksheet MetadataSheet;
            IndicatorBuilder IndBuilder = null;
            IndicatorInfo IndInfo = new IndicatorInfo();
            DataTable Table = null;
            int GIdRowIndex = elementNameRowIndex + 1;

            //-- Open excel File and get first worksheet
            DIExcel DiExcel = new DIExcel(excelFilePath);
            MetadataSheet = DiExcel.GetWorksheet(0);

            RetVal = DiExcel.GetCellValue(ElementSheetIndex, GIdRowIndex, ElementGIdColumnIndex, GIdRowIndex, ElementGIdColumnIndex);

            //-- Get GId if By Name if GID is blank in Excel File
            if (string.IsNullOrEmpty(RetVal))
            {
                //-- Get Element Name
                ElementName = DiExcel.GetCellValue(ElementSheetIndex, elementNameRowIndex, ElementNameColumnIndex, elementNameRowIndex, ElementNameColumnIndex);
                switch (elementType)
                {
                    case MetaDataType.Indicator:
                        IndBuilder = new IndicatorBuilder(this.DBConnection, this.DBQueries);
                        IndInfo = IndBuilder.GetIndicatorInfo(FilterFieldType.Name, ElementName, FieldSelection.Heavy);
                        if (IndInfo != null)
                        {
                            RetVal = IndInfo.GID;
                        }
                        break;
                    case MetaDataType.Map:
                        Table = this.DBConnection.ExecuteDataTable(this.DBQueries.Area.GetAreaMapMetadataByName(ElementName));
                        if (Table.Rows.Count > 0)
                        {
                            RetVal = Convert.ToString(Table.Rows[0][Area_Map_Metadata.LayerName]);
                        }
                        break;
                    case MetaDataType.Source:
                    case MetaDataType.Sector:
                    case MetaDataType.Goal:
                    case MetaDataType.CF:
                    case MetaDataType.Theme:
                    case MetaDataType.Institution:
                    case MetaDataType.Convention:
                    case MetaDataType.IndicatorClassification:
                        Table = this.DBConnection.ExecuteDataTable(this.DBQueries.IndicatorClassification.GetIC(FilterFieldType.Name, ElementName, FieldSelection.Heavy));
                        if (Table.Rows.Count > 0)
                        {
                            RetVal = Convert.ToString(Table.Rows[0][IndicatorClassifications.ICGId]);
                        }
                        break;
                    default:
                        break;
                }
            }

            return RetVal;
        }

        private string GetElementInfo(MetaDataType elementType, string elementGId)
        {
            string RetVal = string.Empty;
            IndicatorBuilder IndBuilder = null;
            IndicatorInfo IndInfo = new IndicatorInfo();
            DataTable Table = null;

            elementGId = DICommon.RemoveQuotes(elementGId);
            try
            {


                switch (elementType)
                {
                    case MetaDataType.Indicator:
                        IndBuilder = new IndicatorBuilder(this.DBConnection, this.DBQueries);
                        IndInfo = IndBuilder.GetIndicatorInfo(FilterFieldType.GId, "'" + elementGId + "'", FieldSelection.Heavy);
                        RetVal = IndInfo.Info;
                        break;
                    case MetaDataType.Map:
                        Table = this.DBConnection.ExecuteDataTable(this.DBQueries.Area.GetAreaMapMetadataByName(elementGId));
                        RetVal = Convert.ToString(Table.Rows[0][Area_Map_Metadata.MetadataText]);
                        break;
                    case MetaDataType.Source:
                    case MetaDataType.Sector:
                    case MetaDataType.Goal:
                    case MetaDataType.CF:
                    case MetaDataType.Theme:
                    case MetaDataType.Institution:
                    case MetaDataType.Convention:
                    case MetaDataType.IndicatorClassification:
                        Table = this.DBConnection.ExecuteDataTable(this.DBQueries.IndicatorClassification.GetIC(FilterFieldType.GId, "'" + elementGId + "'", FieldSelection.Heavy));
                        RetVal = Convert.ToString(Table.Rows[0][IndicatorClassifications.ICInfo]);
                        break;
                    default:
                        break;
                }

            }
            catch (Exception)
            {
            }
            return RetVal;

        }

        private string GetCategoryTypeText(MetaDataType elementType)
        {
            string RetVal = string.Empty;

            switch (elementType)
            {
                case MetaDataType.Indicator:
                    RetVal = "I";
                    break;
                case MetaDataType.Map:
                    RetVal = "A";
                    break;
                case MetaDataType.IndicatorClassification:
                    RetVal = "S";
                    break;
                default:
                    RetVal = "S";
                    break;
            }


            return RetVal;

        }

        private Dictionary<int, string> GetMetadataXmlFromExcelFile(string excelFilePath, MetaDataType elementType, string fldrMetadataTemplatePath)
        {
            Dictionary<int, string> RetVal = new Dictionary<int, string>();
            string ElementGId = string.Empty;
            int ElementNId = 0;
            SpreadsheetGear.IWorksheet MetadataSheet;
            int CategoryRowStartIndex = 0;
            string MetadataText = string.Empty;

            //-- Open excel File and get first worksheet
            DIExcel DiExcel = new DIExcel(excelFilePath);
            MetadataSheet = DiExcel.GetWorksheet(0);

            //-- Get ElementGId
            ElementGId = this.GetElementGId(excelFilePath, elementType, MetaDataBuilder.ElementNameRowIndex);
            ElementNId = this.GetElementNId(excelFilePath, elementType, MetaDataBuilder.ElementNameRowIndex);
            CategoryRowStartIndex = MetaDataBuilder.ElementGIdRowIndex + 1;
            try
            {
                CategoryRowStartIndex = CategoryRowStartIndex > 3 ? CategoryRowStartIndex : 4;
                MetadataText = this.GetElementMetadata(elementType, ElementGId, ElementNId, DiExcel, CategoryRowStartIndex);

                RetVal.Add(ElementNId, MetadataText);


            }
            catch (Exception)
            {

                throw;
            }

            return RetVal;

        }

        private Dictionary<int, string> GetMetadataXmlForAllElementFromExcelFile(string excelFilePath, MetaDataType elementType, string fldrMetadataTemplatePath)
        {
            Dictionary<int, string> RetVal = new Dictionary<int, string>();
            string ElementGId = string.Empty;
            int ElementNId = 0;
            SpreadsheetGear.IWorksheet MetadataSheet;
            int CategoryRowStartIndex = 0;
            string MetadataText = string.Empty;

            //-- Open excel File and get first worksheet
            DIExcel DiExcel = new DIExcel(excelFilePath);
            MetadataSheet = DiExcel.GetWorksheet(0);

            //-- Get ElementGId
            ElementGId = this.GetElementGId(excelFilePath, elementType, MetaDataBuilder.ElementNameRowIndex);
            ElementNId = this.GetElementNId(excelFilePath, elementType, MetaDataBuilder.ElementNameRowIndex);
            CategoryRowStartIndex = MetaDataBuilder.ElementGIdRowIndex + 1;
            try
            {

                while (ElementNId > 0)
                {
                    CategoryRowStartIndex = CategoryRowStartIndex > 3 ? CategoryRowStartIndex : 4;
                    MetadataText = GetElementMetadata(elementType, ElementGId, ElementNId, DiExcel, CategoryRowStartIndex);
                    if (!RetVal.ContainsKey(ElementNId))
                    {
                        RetVal.Add(ElementNId, MetadataText);
                    }
                    //-- Get Next Element Metadata
                    CategoryRowStartIndex = this.GetNextElementNameRowIndex(DiExcel, CategoryRowStartIndex > 4 ? CategoryRowStartIndex : 5);
                    if (CategoryRowStartIndex > 0)
                    {
                        //-- Get ElementNId
                        ElementNId = this.GetElementNId(excelFilePath, elementType, CategoryRowStartIndex);
                        CategoryRowStartIndex = CategoryRowStartIndex + 2;
                    }
                    else
                    {
                        ElementNId = 0;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return RetVal;

        }

        private string GetElementMetadata(MetaDataType elementType, string elementGId, int elementNId, DIExcel diExcel, int startRowIndex)
        {
            string RetVal = string.Empty;
            MetadataCategoryBuilder MDCatBuilder;
            MetadataCategoryInfo MDCatInfo;
            XmlDocument MetadataXmlDoc = new XmlDocument();
            XmlElement CategoryNode;
            XmlElement ParaNode;
            XmlText CategoryText;
            string Category = string.Empty;
            string CategoryValue = string.Empty;
            string ElementMetadataXml = string.Empty;
            int CategoryNId = 0;

            //-- Get Metadata Info for Element
            ElementMetadataXml = this.GetElementInfo(elementType, elementGId);

            if (string.IsNullOrEmpty(ElementMetadataXml))
            {
                MetadataConverter.InsertRootNode(MetadataXmlDoc);
            }
            else
            {
                MetadataXmlDoc.LoadXml(ElementMetadataXml);
            }

            MDCatBuilder = new MetadataCategoryBuilder(this.DBConnection, this.DBQueries);

            //foreach (IRange Row in DiExcel.GetUsedRange(0).Rows )
            for (int Index = startRowIndex; Index < diExcel.GetUsedRange(0).Rows.RowCount; Index++)
            {
                Category = Convert.ToString(diExcel.GetUsedRange(0).Rows[Index, 0].Value);
                CategoryValue = Convert.ToString(diExcel.GetUsedRange(0).Rows[Index, 1].Value);

                if (!string.IsNullOrEmpty(Category))
                {
                    //-- Get Metadata CategoryInfo
                    MDCatInfo = MDCatBuilder.GetMetadataCategoryInfo(FilterFieldType.Name, "'" + DICommon.RemoveQuotes(Category) + "'");
                    //-- If Metadata not category exists
                    if (MDCatInfo != null && string.IsNullOrEmpty(MDCatInfo.CategoryName))
                    {
                        MDCatInfo.CategoryName = Category;
                        MDCatInfo.CategoryType = this.GetCategoryTypeText(elementType);

                        //Create new xmlelement as Category
                        CategoryNode = MetadataXmlDoc.CreateElement(MetaDataConstants.CategoryTagName);

                        //Set attribute values of category element.
                        CategoryNode.SetAttribute(MetaDataConstants.NameAttribute, Category);
                        // create para element                
                        ParaNode = MetadataXmlDoc.CreateElement(MetaDataConstants.ParaTagName);
                        //Append as para node under category node
                        CategoryNode.AppendChild(ParaNode);
                        // create text node
                        CategoryText = MetadataXmlDoc.CreateTextNode(this.ReplaceInvalidString(CategoryValue));
                        //Append category text in paraNode 
                        ParaNode.AppendChild(CategoryText);
                        //Append it under metadata node
                        MetadataXmlDoc.DocumentElement.AppendChild(CategoryNode);
                    }
                    //-- Get CategoryNId
                    CategoryNId = MDCatBuilder.CheckNCreateMetadataCategory(MDCatInfo);
                    this.UpdateMetadataCategory(MetadataXmlDoc, Category, CategoryValue);
                }
                else
                {
                    //-- Exit for Next Metadata
                    break;
                }

            }

            RetVal = MetadataXmlDoc.InnerXml;

            return RetVal;
        }

        private string ReplaceInvalidString(string paraText)
        {
            string RetVal = paraText;
            int i = 0;
            int CurrIndex = 0;
            string ConcatenatedParaNodeText = string.Empty;
            string ParaNodeText = string.Empty;
            try
            {

                ParaNodeText = paraText;
                // add para node into concatenated category value
                ParaNodeText = ParaNodeText.TrimStart();
                ParaNodeText = ParaNodeText.TrimEnd();

                //--Replace  - {{~}} special character from paranode text
                ParaNodeText = ParaNodeText.Replace(" - {{~}}", "");


                ParaNodeText = ParaNodeText.Replace(MetadataEditorCostants.MetadataCategorySpecialSymbol, Microsoft.VisualBasic.ControlChars.NewLine);
                ParaNodeText = ParaNodeText.Replace(" - {{~}}", "");
                ParaNodeText = ParaNodeText.Replace(" {{~}} ", Microsoft.VisualBasic.ControlChars.NewLine);
                ParaNodeText = ParaNodeText.Replace("  {{~}}  ", Microsoft.VisualBasic.ControlChars.NewLine);
                ParaNodeText = ParaNodeText.Replace("{{~}} ", Microsoft.VisualBasic.ControlChars.NewLine);
                ParaNodeText = ParaNodeText.Replace(" {{~}}", Microsoft.VisualBasic.ControlChars.NewLine);

                //Concatenate ParaNodeText
                ConcatenatedParaNodeText = ConcatenatedParaNodeText + Microsoft.VisualBasic.ControlChars.NewLine + ParaNodeText;
                //Format ConcatenatedParaNodeText
                ConcatenatedParaNodeText.Replace("{{~}}", Microsoft.VisualBasic.ControlChars.NewLine);
                ParaNodeText = ParaNodeText.Replace(" {{~}} ", Microsoft.VisualBasic.ControlChars.NewLine);
                ParaNodeText = ParaNodeText.Replace("  {{~}}  ", Microsoft.VisualBasic.ControlChars.NewLine);
                ParaNodeText = ParaNodeText.Replace("{{~}} ", Microsoft.VisualBasic.ControlChars.NewLine);
                ParaNodeText = ParaNodeText.Replace(" {{~}}", Microsoft.VisualBasic.ControlChars.NewLine);


                RetVal = ParaNodeText;
            }
            catch (Exception ex)
            { }

            return RetVal;
        }
        private void CheckNUpdateMetadataCategory(string metadataXml)
        {
            string CategoryName = string.Empty;
            string CategoryValue = string.Empty;
            XmlDocument XmlDoc = new XmlDocument();
            MetadataCategoryBuilder MDCatBuilder;
            MetadataCategoryInfo MDCatInfo;
            XmlDoc.LoadXml(metadataXml);

            try
            {

                MDCatBuilder = new MetadataCategoryBuilder(this.DBConnection, this.DBQueries);
                XmlNodeList XmlList = XmlDoc.SelectNodes(METADATA_CATEGORYTAG);

                for (int i = 0; i < XmlList.Count; i++)
                {
                    CategoryName = XmlList[i].Attributes["name"].Value;

                    MDCatInfo = MDCatBuilder.GetMetadataCategoryInfo(FilterFieldType.Name, "'" + DICommon.RemoveQuotes(CategoryName) + "'");

                    MDCatBuilder.CheckNCreateMetadataCategory(MDCatInfo);


                }

            }
            catch (Exception)
            {
            }

        }

        private bool ISMetadataXmlForCategory(string metadataXml, MetaDataType elementType)
        {
            bool RetVal = false;
            string CategoryName = string.Empty;
            string CategoryValue = string.Empty;
            XmlDocument XmlDoc = new XmlDocument();
            MetadataCategoryBuilder MDCatBuilder;
            MetadataCategoryInfo MDCatInfo;
            XmlDoc.LoadXml(metadataXml);

            try
            {

                MDCatBuilder = new MetadataCategoryBuilder(this.DBConnection, this.DBQueries);
                XmlNodeList XmlList = XmlDoc.SelectNodes(METADATA_CATEGORYTAG);

                for (int i = 0; i < XmlList.Count; i++)
                {
                    CategoryName = XmlList[i].Attributes["name"].Value;

                    MDCatInfo = MDCatBuilder.GetMetadataCategoryInfo(FilterFieldType.Name, "'" + DICommon.RemoveQuotes(CategoryName) + "'");

                    if (MDCatInfo.CategoryType == this.GetCategoryTypeText(elementType))
                    {
                        MDCatBuilder.CheckNCreateMetadataCategory(MDCatInfo);
                        RetVal = true;
                    }

                }

            }
            catch (Exception)
            {
            }

            return RetVal;
        }


        private int GetNextElementNameRowIndex(DIExcel diExcel, int startRowIndex)
        {
            int RetVal = 0;

            for (int Index = startRowIndex; Index < diExcel.GetUsedRange(0).Rows.RowCount; Index++)
            {
                if (string.IsNullOrEmpty(Convert.ToString(diExcel.GetUsedRange(0).Rows[Index, 0].Value)))
                {
                    RetVal = Index + 1;
                    break;
                }
            }

            return RetVal;
        }


        private void UpdateMetadataCategory(XmlDocument xmlDoc, string categoryName, string categoryParaValue)
        {
            XmlElement ParaNode = null;
            XmlElement CategoryNode = null;
            XmlAttribute CategoryNameAtt;
            XmlNode Node;
            try
            {
                Node = xmlDoc.SelectSingleNode(METADATA_CATEGORYTAG + "[" + METADATA_CATNAMETAG + "=\"" + categoryName + "\"]");

                ParaNode = xmlDoc.CreateElement(MetaDataConstants.ParaTagName);
                ParaNode.InnerText = categoryParaValue;

                if (Node == null)
                {
                    //-- Careate "Category" Node
                    CategoryNode = xmlDoc.CreateElement(MetaDataConstants.CategoryTagName);
                    //-- Set CategoryName In "name" 
                    CategoryNode.SetAttribute(MetaDataConstants.NameAttribute, categoryName);
                    CategoryNode.AppendChild(ParaNode);
                    Node = (XmlNode)CategoryNode;
                    xmlDoc.DocumentElement.AppendChild(Node);

                }
                else
                {
                    CategoryNode = (XmlElement)Node;
                    CategoryNode.RemoveAll();
                    CategoryNode.SetAttribute(MetaDataConstants.NameAttribute, categoryName);
                    CategoryNode.AppendChild(ParaNode);
                }

            }
            catch (Exception)
            {
            }
        }

        #endregion

        #endregion

        #region "-- Public --"

        #region "-- New/Dispose --"

        public MetaDataBuilder(DIConnection connection, DIQueries queries)
        {
            this.DBConnection = connection;
            this.DBQueries = queries;
        }

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// To Import metadata information 
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="sourceDBConnection"></param>
        /// <param name="sourceDBQueries"></param>
        /// <param name="selectedNIDs"></param>
        /// <param name="selectionCount"></param>
        /// <param name="metadataType"> </param>
        public void InsertImportValues(string dataPrefix, DIConnection sourceDBConnection, DIQueries sourceDBQueries, string selectedNIDs, int selectionCount, MetaDataType metadataType)
        {

            int Index;
            string MetadataInfo = string.Empty;
            DataTable TempDataTable;
            DataTable TempTargetIdTable = new DataTable();
            ICType ClassificationType;
            Dictionary<string, string> OldIconNId_NewIconNId = new Dictionary<string, string>();
            int CurrentRecordIndex = 0;
            MetadataCategoryBuilder MDCategoryBuilder = null;
            int IndicatorNid = 0;

            this.RaiseStartProcessEvent();

            switch (metadataType)
            {
                case MetaDataType.Indicator:
                    // -- INDICATORS 
                    try
                    {
                        if (selectionCount == -1)
                        {
                            // -- GET ALL 
                            TempDataTable = sourceDBConnection.ExecuteDataTable(sourceDBQueries.Indicators.GetIndicator(FilterFieldType.None, string.Empty, FieldSelection.Heavy));
                        }
                        else
                        {
                            // -- GET SELECTED 
                            TempDataTable = sourceDBConnection.ExecuteDataTable(sourceDBQueries.Indicators.GetIndicator(FilterFieldType.NId, selectedNIDs, FieldSelection.Heavy));
                        }


                        //////// -- Initialize Progress Bar 
                        this.RaiseBeforeProcessEvent(TempDataTable.Rows.Count);

                        for (Index = 0; Index <= TempDataTable.Rows.Count - 1; Index++)
                        {
                            CurrentRecordIndex++;

                            if (!Information.IsDBNull(TempDataTable.Rows[Index][Indicator.IndicatorInfo]))
                            {
                                if (!string.IsNullOrEmpty(TempDataTable.Rows[Index][Indicator.IndicatorInfo].ToString()))
                                {
                                    OldIconNId_NewIconNId = new Dictionary<string, string>();

                                    //-- Get Target IndicatorNId against IndicatorGId 
                                    TempTargetIdTable = this.DBConnection.ExecuteDataTable(this.DBQueries.Indicators.GetIndicator(FilterFieldType.GId, "'" + DICommon.RemoveQuotes(TempDataTable.Rows[Index][Indicator.IndicatorGId].ToString()) + "'", FieldSelection.Heavy));

                                    if (TempTargetIdTable.Rows.Count == 0)
                                    {
                                        TempTargetIdTable = this.DBConnection.ExecuteDataTable(this.DBQueries.Indicators.GetIndicator(FilterFieldType.Name, "'" + DICommon.RemoveQuotes(TempDataTable.Rows[Index][Indicator.IndicatorName].ToString()) + "'", FieldSelection.Heavy));
                                    }

                                    IndicatorNid = -1;

                                    if (TempTargetIdTable.Rows.Count > 0)
                                    {
                                        IndicatorNid = Convert.ToInt32(TempTargetIdTable.Rows[0][Indicator.IndicatorNId]);

                                        //-- Update Icons in Icon Table 
                                        OldIconNId_NewIconNId = DIIcons.ImportElement(
                                           Convert.ToInt32(TempDataTable.Rows[Index][Indicator.IndicatorNId]),
                                           Convert.ToInt32(TempTargetIdTable.Rows[0][Indicator.IndicatorNId]),
                                           IconElementType.Indicator, sourceDBQueries, sourceDBConnection,
                                           this.DBQueries, this.DBConnection);
                                    }


                                    //-- Retrieve Metadata Information 
                                    MetadataInfo = TempDataTable.Rows[Index][Indicator.IndicatorInfo].ToString();

                                    //-- Convert Metdata into latest format 
                                    MetadataInfo = DICommon.CheckNConvertMetadataXml(MetadataInfo);


                                    //-- Update Metadata Category from XML File
                                    MDCategoryBuilder = new MetadataCategoryBuilder(this.DBConnection, this.DBQueries);

                                    MDCategoryBuilder.ReadXmlAndUpdateCategoryTable(MetadataInfo, this.GetCategoryTypeText(metadataType));
                                    //-- Update IconNids in xml if it exists 
                                    foreach (KeyValuePair<string, string> kvp in OldIconNId_NewIconNId)
                                    {
                                        MetadataInfo = MetadataInfo.Replace(kvp.Key, kvp.Value);
                                    }

                                    //-- Update Metadata 
                                    this.UpdateMetadataInfo(metadataType, string.Empty, IndicatorNid, MetadataInfo);

                                }
                            }

                            // -- Increemnt the Progress Bar Value 
                            this.RaiseProcessInfoEvent(CurrentRecordIndex);

                        }

                        // -- Dispose the Data Table object 
                        TempDataTable.Dispose();
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException(ex.ToString());
                    }

                    break;

                case MetaDataType.Map:
                    // -- MAP 
                    try
                    {
                        if (selectionCount == -1)
                        {
                            // -- GET ALL 
                            TempDataTable = sourceDBConnection.ExecuteDataTable(sourceDBQueries.Area.GetAreaMapMetadata(string.Empty));
                        }
                        else
                        {
                            // -- GET SELECTED 
                            TempDataTable = sourceDBConnection.ExecuteDataTable(sourceDBQueries.Area.GetAreaMapMetadata(selectedNIDs));
                        }



                        // -- Initialize Progress Bar 
                        this.RaiseBeforeProcessEvent(TempDataTable.Rows.Count);
                        CurrentRecordIndex = 0;

                        for (Index = 0; Index <= TempDataTable.Rows.Count - 1; Index++)
                        {
                            CurrentRecordIndex++;

                            if (!Information.IsDBNull(TempDataTable.Rows[Index][Area_Map_Metadata.MetadataText]))
                            {
                                if (!(TempDataTable.Rows[Index][Area_Map_Metadata.MetadataText].ToString().Length == 0))
                                {

                                    OldIconNId_NewIconNId = new Dictionary<string, string>();
                                    //-- Update Icons in Icon Table 
                                    //-- Get Target LayerNId against LayerName 
                                    TempTargetIdTable = this.DBConnection.ExecuteDataTable(
                                        this.DBQueries.Area.GetAreaMapMetadataByName(
                                        TempDataTable.Rows[Index][Area_Map_Metadata.LayerName].ToString()));

                                    if (TempTargetIdTable.Rows.Count > 0)
                                    {
                                        OldIconNId_NewIconNId = DIIcons.ImportElement(
                                           Convert.ToInt32(TempDataTable.Rows[Index][Area_Map_Metadata.LayerNId]),
                                           Convert.ToInt32(TempTargetIdTable.Rows[0][Area_Map_Metadata.LayerNId]),
                                           IconElementType.MetadataArea,
                                           sourceDBQueries, sourceDBConnection,
                                           this.DBQueries, this.DBConnection);
                                    }


                                    //-- Retrieve Metadata Information 
                                    MetadataInfo = TempDataTable.Rows[Index][Area_Map_Metadata.MetadataText].ToString();
                                    //-- Convert Metdata into latest format 
                                    MetadataInfo = DICommon.CheckNConvertMetadataXml(MetadataInfo);


                                    //-- Update Metadata Category from XML File
                                    MDCategoryBuilder = new MetadataCategoryBuilder(this.DBConnection, this.DBQueries);
                                    MDCategoryBuilder.ReadXmlAndUpdateCategoryTable(MetadataInfo, this.GetCategoryTypeText(metadataType));
                                    //-- Update IconNids in xml if it exists 
                                    foreach (KeyValuePair<string, string> kvp in OldIconNId_NewIconNId)
                                    {
                                        MetadataInfo = MetadataInfo.Replace(kvp.Key, kvp.Value);
                                    }

                                    //-- Update Metadata 
                                    this.UpdateMetadataInfo(metadataType, TempDataTable.Rows[Index][Area_Map_Metadata.LayerName].ToString(), -1, MetadataInfo);
                                }
                            }

                            // -- Increemnt the Progress Bar Value 
                            this.RaiseProcessInfoEvent(CurrentRecordIndex);


                        }

                        // -- Dispose the Data Table object 
                        TempDataTable.Dispose();
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException(ex.ToString());
                    }

                    break;
                default:
                    //AppGeneral.MetadataType.Source 
                    // -- SOURCE 
                    ClassificationType = ICType.Sector;
                    switch (metadataType)
                    {
                        case MetaDataType.Sector:
                            ClassificationType = ICType.Sector;
                            break;
                        case MetaDataType.Goal:
                            ClassificationType = ICType.Goal;
                            break;
                        case MetaDataType.CF:
                            ClassificationType = ICType.CF;
                            break;
                        case MetaDataType.Theme:
                            ClassificationType = ICType.Theme;
                            break;
                        case MetaDataType.Source:
                            ClassificationType = ICType.Source;
                            break;
                        case MetaDataType.Institution:
                            ClassificationType = ICType.Institution;
                            break;
                        case MetaDataType.Convention:
                            ClassificationType = ICType.Convention;
                            break;
                    }
                    try
                    {
                        if (selectionCount == -1)
                        {
                            // -- GET ALL 
                            TempDataTable = sourceDBConnection.ExecuteDataTable(sourceDBQueries.IndicatorClassification.GetIC(FilterFieldType.None, string.Empty, ClassificationType, FieldSelection.Heavy));
                        }
                        else
                        {
                            // -- GET SELECTED 
                            TempDataTable = sourceDBConnection.ExecuteDataTable(sourceDBQueries.IndicatorClassification.GetIC(FilterFieldType.NId, selectedNIDs, ClassificationType, FieldSelection.Heavy));
                        }



                        ////// -- Initialize Progress Bar 
                        this.RaiseBeforeProcessEvent(TempDataTable.Rows.Count);
                        CurrentRecordIndex = 0;

                        for (Index = 0; Index <= TempDataTable.Rows.Count - 1; Index++)
                        {
                            CurrentRecordIndex++;

                            if (!Information.IsDBNull(TempDataTable.Rows[Index][IndicatorClassifications.ICInfo]))
                            {
                                if (!(TempDataTable.Rows[Index][IndicatorClassifications.ICInfo].ToString().Length == 0))
                                {

                                    //-- Retrieve Metadata Information 
                                    MetadataInfo = TempDataTable.Rows[Index][IndicatorClassifications.ICInfo].ToString();

                                    if (ClassificationType == ICType.Source)
                                    {
                                        //Only for Source which has xml Metadata 
                                        OldIconNId_NewIconNId = new Dictionary<string, string>();
                                        //-- Update Icons in Icon Table 
                                        //-- Get Target LayerNId against LayerName 
                                        TempTargetIdTable = this.DBConnection.ExecuteDataTable(this.DBQueries.IndicatorClassification.GetIC(FilterFieldType.Name, "'" + DICommon.RemoveQuotes(Convert.ToString(TempDataTable.Rows[Index][IndicatorClassifications.ICName])) + "'", ClassificationType, FieldSelection.Heavy));

                                        if (TempTargetIdTable.Rows.Count > 0)
                                        {
                                            OldIconNId_NewIconNId = DIIcons.ImportElement(
                                               Convert.ToInt32(TempDataTable.Rows[Index][IndicatorClassifications.ICNId]),
                                              Convert.ToInt32(TempTargetIdTable.Rows[0][IndicatorClassifications.ICNId]),
                                              IconElementType.MetadataSource,
                                              sourceDBQueries, sourceDBConnection,
                                              this.DBQueries, this.DBConnection);
                                        }

                                        //-- Convert Metdata into latest format 
                                        MetadataInfo = DICommon.CheckNConvertMetadataXml(MetadataInfo);


                                        //-- Update IconNids in xml if it exists 
                                        foreach (KeyValuePair<string, string> kvp in OldIconNId_NewIconNId)
                                        {
                                            MetadataInfo = MetadataInfo.Replace(kvp.Key, kvp.Value);
                                        }
                                        //-- Update Metadata Category from XML File
                                        MDCategoryBuilder = new MetadataCategoryBuilder(this.DBConnection, this.DBQueries);
                                        MDCategoryBuilder.ReadXmlAndUpdateCategoryTable(MetadataInfo, this.GetCategoryTypeText(metadataType));
                                    }
                                    //-- Update Metadata 
                                    this.UpdateMetadataInfo(metadataType, Convert.ToString(TempDataTable.Rows[Index][IndicatorClassifications.ICGId]), -1, MetadataInfo);
                                }
                            }

                            ////// -- Increemnt the Progress Bar Value 
                            this.RaiseProcessInfoEvent(CurrentRecordIndex);

                        }


                        // -- Dispose the Data Table object 
                        TempDataTable.Dispose();
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException(ex.ToString());
                    }

                    break;
            }

        }

        /// <summary>
        /// To Import or create transform information into XSLT and Element_XSLT table
        /// </summary>
        /// <param name="XSLTText"></param>
        /// <param name="XsltFilename"></param>
        /// <param name="elementNId"></param>
        /// <param name="elementType"></param>
        public void ImportTransformInfo(string XSLTText, string XsltFilename, string elementNId, MetadataElementType elementType)
        {
            // -- this function insert the values into RT_XSLT table 
            string SqlString = string.Empty;
            try
            {
                int XsltNid = 0;

                // -- step1 : check xslt info already exists or not 
                SqlString = this.DBQueries.Xslt.GetXSLT(FilterFieldType.Name, XsltFilename);

                if (this.DBConnection.ExecuteDataTable(SqlString).Rows.Count == 0)
                {
                    //-- step 2: insert into xslt table 
                    SqlString = DALQueries.Xslt.Insert.InsertXSLT(this.DBQueries.DataPrefix, DICommon.RemoveQuotes(XSLTText), DICommon.RemoveQuotes(XsltFilename));

                    this.DBConnection.ExecuteNonQuery(SqlString);
                }
                else
                {
                    //-- step 2: Update xslt table 
                    SqlString = DALQueries.Xslt.Update.UpdateXSLT(this.DBQueries.DataPrefix, DICommon.RemoveQuotes(XSLTText), XsltFilename);
                    this.DBConnection.ExecuteNonQuery(SqlString);
                }
                //-- step 3: find the xslt_nid against xslFilename 
                SqlString = this.DBQueries.Xslt.GetXSLT(FilterFieldType.Name, XsltFilename);

                // -- step 4: insert into Element_xslt table 
                XsltNid = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(SqlString));

                SqlString = DALQueries.Xslt.Insert.InsertElementXSLT(this.DBQueries.DataPrefix, Convert.ToInt32(elementNId), elementType, Convert.ToInt32(XsltNid));

                this.DBConnection.ExecuteNonQuery(SqlString);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }


        /// <summary>
        /// To import metadata from Xml file for indicator , Map and source only
        /// </summary>
        /// <param name="xmlText"></param>
        /// <param name="metadataType"></param>
        /// <param name="elementNId"></param>
        /// <param name="xsltFolderPath"></param>
        public void ImportMetadataFromXML(string xmlText, MetaDataType metadataType, int elementNId, string xsltFolderPath)
        {
            int XsltNId = -1;
            string XslFileText = string.Empty;
            MetadataElementType ElementType = MetadataElementType.Area;
            string DefaultXsl = string.Empty;
            DataTable Table;
            try
            {

                xmlText = DICommon.CheckNConvertMetadataXml(xmlText);

                //*** Update Xml content 
                switch (metadataType)
                {
                    case MetaDataType.Indicator:
                        ElementType = MetadataElementType.Indicator;
                        DefaultXsl = "IND";

                        break;
                    case MetaDataType.Map:
                        ElementType = MetadataElementType.Area;
                        DefaultXsl = "MAP";
                        break;
                    case MetaDataType.Source:
                        ElementType = MetadataElementType.Source;
                        DefaultXsl = "SRC";

                        break;

                }
                //-- validate xml for Metadata Category type
                if (this.ISMetadataXmlForCategory(xmlText, metadataType))
                {
                    //-- Create Category if not in database
                    this.CheckNUpdateMetadataCategory(xmlText);

                    this.UpdateMetadataInfo(metadataType, string.Empty, elementNId, xmlText);

                    // XSlt file not associated while importing xml through browse option 
                    //*** Get XsltNId 
                    if (!string.IsNullOrEmpty(DefaultXsl))
                    {
                        Table = this.DBConnection.ExecuteDataTable(this.DBQueries.Xslt.GetXSLT(FilterFieldType.Name, DefaultXsl));
                        if (Table.Rows.Count == 0)
                        {
                            //*** Insert Default Xsl if it does not exists in database                     
                            XmlDocument oXslDoc = new XmlDocument();
                            if (File.Exists(xsltFolderPath + "\\" + DefaultXsl + ".xsl"))
                            {
                                oXslDoc.Load(xsltFolderPath + "\\" + DefaultXsl + ".xsl");
                                XslFileText = "";
                                XslFileText = oXslDoc.OuterXml;
                                XslFileText = XslFileText.Replace("'", "''");

                                this.DBConnection.ExecuteNonQuery(DALQueries.Xslt.Insert.InsertXSLT(this.DBQueries.DataPrefix, XslFileText, DefaultXsl));
                                XsltNId = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery("SELECT @@Identity"));
                            }
                        }
                        else
                        {
                            XsltNId = Convert.ToInt32(Table.Rows[0][XSLT.XSLTNId]);
                        }
                    }

                    //*** Enter / Update Element_XSLT relationship 
                    if (XsltNId != -1)
                    {
                        Table = this.DBConnection.ExecuteDataTable(this.DBQueries.Xslt.GetXSLT(elementNId.ToString(), ElementType));
                        if (Table.Rows.Count == 0)
                        {
                            //*** Insert 
                            this.DBConnection.ExecuteNonQuery(DALQueries.Xslt.Insert.InsertElementXSLT(this.DBQueries.DataPrefix,
                                elementNId, ElementType, XsltNId));
                        }
                        else
                        {
                            //*** Update 
                            this.DBConnection.ExecuteNonQuery(DALQueries.Xslt.Update.UpdateElementXSLT(this.DBQueries.DataPrefix, XsltNId, elementNId, ElementType));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        /// <summary>
        /// Imports metadata from excel file (only for  Indicator, Map and Source)
        /// </summary>
        /// <param name="metadataType">Metadata Type : Indicator,Map ,Source</param>
        /// <param name="elementNId"></param>
        /// <param name="xlsFileNameWPath">Excel File path from which metadata will be imported</param>
        public void ImportMetataFromExcel(MetaDataType metadataType, int elementNId, string xlsFileNameWPath, string xsltFldrPath)
        {
            Dictionary<int, string> MetadataCollection = new Dictionary<int, string>();
            DI_LibBAL.Controls.MetadataEditorBAL.MetadataEditor mdEditor = null;
            string MetadataTemplateFldrPath = string.Empty;
            string MetadataText = string.Empty;

            // This is the parent folder of XSLT Folder
            MetadataTemplateFldrPath = xsltFldrPath.Replace(@"\XSLT", " ").Trim();

            // Create metadata text in xml formate as it is stored in database

            // Get Metadata Text from Excel File using BAL Metadata Editor Class 
            mdEditor = new DI_LibBAL.Controls.MetadataEditorBAL.MetadataEditor();

            // elementNId = this.GetElementNId(xlsFileNameWPath, metadataType, MetaDataBuilder.ElementNameRowIndex);
            try
            {

                // Parameter xls file,metadatatype,MetadataTemplate Folder                        
                MetadataCollection = this.GetMetadataXmlFromExcelFile(xlsFileNameWPath, metadataType, MetadataTemplateFldrPath);
                //MetadataText = mdEditor.GetMetadataFromExcelFile(xlsFileNameWPath, metadataType, MetadataTemplateFldrPath);


                // If metadata found .Update Database
                foreach (int ElementNId in MetadataCollection.Keys)
                {
                    // Import Metadata for selected element
                    if (elementNId == ElementNId)
                    {
                        MetadataText = MetadataCollection[ElementNId];
                        if (!string.IsNullOrEmpty(MetadataText))
                        {
                            // Updata Database Using exising importFromXML 
                            this.ImportMetadataFromXML(MetadataText, metadataType, ElementNId, xsltFldrPath);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// To import metadata from RTF file for all indicator classifications except source
        /// </summary>
        /// /// <param name="metadataInfo"></param>
        /// <param name="metaDataType"></param>
        /// <param name="elementNid"></param>        
        public void ImportMetadataFromRTFFile(string metadataInfo, MetaDataType metadataType, int elementNId)
        {
            this.UpdateMetadataInfo(metadataType, string.Empty, elementNId, metadataInfo);
        }

        /// <summary>
        /// Deletes metadata information 
        /// </summary>
        /// <param name="elementNIds"></param>
        /// <param name="elementType"></param>
        public void DeleteMetadata(string elementNIds, MetadataElementType elementType)
        {
            try
            {
                this.DBConnection.ExecuteNonQuery(this.DBQueries.Delete.Xslt.DeleteElementXSLT(elementNIds, elementType));
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        public static string GetNewMetaDataCategoryGID()
        {
            string RetVal = System.Guid.NewGuid().ToString();

            if (char.IsDigit(RetVal[0]))
            {
                MetaDataBuilder.GetNewMetaDataCategoryGID();
            }

            return RetVal;
        }

        #endregion

        #endregion

    }
}




