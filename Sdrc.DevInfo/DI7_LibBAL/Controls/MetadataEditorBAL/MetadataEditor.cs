using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Xml.Schema;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using DevInfo.Lib.DI_LibDAL.Queries;
using System.IO;
using DevInfo.Lib.DI_LibBAL;
using DevInfo.Lib.DI_LibBAL.Utility;
using System.Data;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;


namespace DevInfo.Lib.DI_LibBAL.Controls.MetadataEditorBAL
{
    /// <summary>
    /// This class Will used for metadata import and export.
    /// </summary>
    public class MetadataEditor
    {

        #region " -- Private -- "

        #region  " -- Variable -- "
        //XmlDocument MetadataDOM;

        public static int CategoryIndex;

        // XmlNamespaceManager Manager;
        //XmlDocument MetadataMaskDOM = new XmlDocument();

        DIExcel DiExcel;
        SpreadsheetGear.IWorksheet MetadataSheet;

        #endregion

        #region "-- Methods--"

        private string SetCharacterEntities(string sSrcText)
        {
            //
            sSrcText = sSrcText.Replace("&", "&amp;");
            sSrcText = sSrcText.Replace("<", "&lt;");
            return sSrcText;
        }

        private string RemoveCharacterEntities(string sSrcText)
        {
            //
            sSrcText = sSrcText.Replace("&lt;", "<");
            sSrcText = sSrcText.Replace("&gt;", ">");
            sSrcText = sSrcText.Replace("&amp;", "&");
            return sSrcText;
        }

        /// <summary>
        /// writes metadata values to excel
        /// </summary>
        /// <param name="metadataFieldIndex"></param>
        /// <param name="metadataFieldText"></param>
        private void WriteMetadataField(int metadataFieldIndex, string metadataFieldText,string categoryGID)
        {
            if (this.MetadataSheet != null)
            {

                ((SpreadsheetGear.IRange)this.MetadataSheet.Cells[this._MetadataStartRowIndexInExl + metadataFieldIndex, 0]).Font.Bold = true;
                
                this.MetadataSheet.Cells[this._MetadataStartRowIndexInExl + metadataFieldIndex, 0].Value = categoryGID;
                                this.MetadataSheet.Cells[this._MetadataStartRowIndexInExl + metadataFieldIndex, 0].ColumnWidth = 40;
                this.MetadataSheet.Cells[this._MetadataStartRowIndexInExl + metadataFieldIndex, 0].WrapText = true;



                this.MetadataSheet.Cells[this._MetadataStartRowIndexInExl + metadataFieldIndex, 1].Value = metadataFieldText;
                this.MetadataSheet.Cells[this._MetadataStartRowIndexInExl + metadataFieldIndex, 1].ColumnWidth = 40;
                this.MetadataSheet.Cells[this._MetadataStartRowIndexInExl + metadataFieldIndex, 1].WrapText = true;
            }
        }

        /// <summary>
        /// writes metadata value to excel
        /// </summary>
        /// <param name="metadataFieldIndex"></param>
        /// <param name="metadataText"></param>
        private void WriteMetadataValueToExcel(int metadataFieldIndex, string metadataText)
        {
            char tab = '\u0009';
            if (this.MetadataSheet != null)
            {
                // --Format metadataText
                metadataText = metadataText.Replace("\r", "");
                metadataText = metadataText.Replace(" \r", "");
                metadataText = metadataText.Replace("  \r", "");
                metadataText = metadataText.Replace("{{~}}", "");
                metadataText = metadataText.Replace("  {{~}}", "");
                metadataText = metadataText.Replace(" {{~}}", "");
                metadataText = metadataText.TrimEnd();
                metadataText = metadataText.TrimStart();

                //--Replace and tab symbol from metatadata Category text
                metadataText = metadataText.Replace(tab.ToString(), "     ");

                //--Set value in MetadataSheet
                this.MetadataSheet.Cells[this._MetadataStartRowIndexInExl + metadataFieldIndex, 2].Value = metadataText.Trim();
                this.MetadataSheet.Cells[this._MetadataStartRowIndexInExl + metadataFieldIndex, 2].ColumnWidth=100;
                this.MetadataSheet.UsedRange.WrapText = true;
            }
        }

        private DataTable GetCategoryDataTable( MetadataElementType elementType)
        {
            DataTable RetVal = null;

            RetVal = this.DBConnection.ExecuteDataTable(this.DBQueries.Metadata_Category.GetMetadataCategoriesForEditor(FilterFieldType.Type, DIQueries.MetadataElementTypeText[elementType]));

            return RetVal;
        }

        private DataTable GetMetadataReportsTableFrmDB(int elementNid, MetadataElementType elementType)
        {
            DataTable RetVal = null;

            // get indicator metadata from metadatareport table
            RetVal = this.DBConnection.ExecuteDataTable(this.DBQueries.MetadataReport.GetMetadataReportsByTargetNid(elementNid.ToString(), elementType));

            return RetVal;
        }

        /// <summary>
        /// Load Data into excel report file
        /// </summary>
        private void LoadData(MetadataElementType elementType,int elementNID)
        {
            
            string ParaNodeText = string.Empty;
            string ConcatenatedParaNodeText = string.Empty;
            DataTable CategoryTable = null;
            DataTable MetadataReportTable = null;
            string CategoryName = string.Empty;
            string CategoryNid = string.Empty;
            string MetadataText = string.Empty;
            string ReplaceableSymbol = "{{~}}";
            string CategoryGID = string.Empty;

                try
                {                  
                    
                    // 1. Get category table from database where ispresentational is false
                    CategoryTable = this.GetCategoryDataTable(elementType);

                    // rearrange categories 
                    CategoryTable = DI7MetadataCategoryBuilder.RearrangeCategoryTableByCategoryOrder(CategoryTable);
                    
                    // 2. get metadata reports 
                    MetadataReportTable = this.GetMetadataReportsTableFrmDB(elementNID,elementType);
                    

                    // 3. Add Title
                    // writes metadata values in excel report.
                    this.WriteMetadataField(CategoryIndex, DILanguage.GetLanguageString("CATEGORY"), DILanguage.GetLanguageString("CATEGORY_GID"));
                    //Write values to excel
                    this.WriteMetadataValueToExcel(CategoryIndex, DILanguage.GetLanguageString("METADATA"));

                    this.MetadataSheet.Cells[this._MetadataStartRowIndexInExl + CategoryIndex, 0, this._MetadataStartRowIndexInExl + CategoryIndex, 2].Font.Bold = true;
                    this.MetadataSheet.Cells[this._MetadataStartRowIndexInExl + CategoryIndex, 0, this._MetadataStartRowIndexInExl + CategoryIndex, 2].Interior.Color = SpreadsheetGear.Drawing.Color.FromArgb(System.Drawing.Color.LightGray.ToArgb());
                    CategoryIndex++;



                    //Iterate through category node
                    foreach (DataRow CategoryRow in CategoryTable.Rows)
                    {
                        // 4.2 get category label form table
                        CategoryName = Convert.ToString(CategoryRow[Metadata_Category.CategoryName]);
                        CategoryNid = Convert.ToString(CategoryRow[Metadata_Category.CategoryNId]);
                        CategoryGID = Convert.ToString(CategoryRow[Metadata_Category.CategoryGId]);

                        // writes metadata values in excel report.
                        this.WriteMetadataField(CategoryIndex, CategoryName,CategoryGID);
                      
                        // Get metadata report from metadata report table for the  current category
                        foreach (DataRow Row in MetadataReportTable.Select(MetadataReport.CategoryNid + "=" + CategoryNid))
                        {
                            MetadataText = Convert.ToString(Row[MetadataReport.Metadata]).Replace(ReplaceableSymbol, Microsoft.VisualBasic.ControlChars.NewLine);
                            
                            //Write values to excel
                            WriteMetadataValueToExcel(CategoryIndex, MetadataText);
                        }
                        

                        CategoryIndex = CategoryIndex + 1;
                    }

                }
                catch (Exception ex)
                { ExceptionHandler.ExceptionFacade.ThrowException(ex); }            
        }



      
        /// <summary>
        /// Set Common value in Metadata Sheet  like element name and GId 
        /// Also set font size and bold property of these cells
        /// </summary>
        /// <param name="elementValue">Indicator  or Area or source</param>
        /// <param name="elementName">eX Indicator name</param>
        /// <param name="elementGId">GId in case of Indicator and source LayerName in case of area</param>
        private void SetMetadataCommonCellValues(string elementValue, string elementName, string elementGId)
        {
            int CurrentRow = 0;
            this.MetadataSheet.Cells[0, 0].Value = elementValue;

            // -- Font Size Of First Row like Indicator Metadata etc size 16 ,Font Bold
            ((SpreadsheetGear.IRange)this.MetadataSheet.Cells[0, 0]).Font.Size = 16;
            ((SpreadsheetGear.IRange)this.MetadataSheet.Cells[0, 0]).Font.Bold = true;
            if (CategoryIndex == 0)
            {
                
                this.MetadataSheet.UsedRange.Cells[1,0].Value = string.Empty;
                this.MetadataSheet.UsedRange.Cells[2, 0].Value = elementName;
                this.MetadataSheet.UsedRange.Cells[3, 0].Value = elementGId;
                ((SpreadsheetGear.IRange)this.MetadataSheet.Cells[2, 0]).Font.Bold = true;
                ((SpreadsheetGear.IRange)this.MetadataSheet.Cells[3, 0]).Font.Bold = false;
            }
            else
            {
                

                //--Insert blank row before each indicator name.
                CurrentRow = CategoryIndex + this._MetadataStartRowIndexInExl +1 ;

                //--Insert Indicator name and indicator guid.
                this.MetadataSheet.Cells[CurrentRow, 0].Value = elementName;
                this.MetadataSheet.Cells[CurrentRow, 0].Font.Bold = true;
                this.MetadataSheet.Cells[CurrentRow, 0].Font.Size = 11;
                CategoryIndex++;
                CurrentRow += 1;
                this.MetadataSheet.Cells[CurrentRow, 0].Value = elementGId;
                this.MetadataSheet.Cells[CurrentRow, 0].Font.Size =11;
                this.MetadataSheet.Cells[CurrentRow, 0].Font.Bold = false;
                CategoryIndex++;
                CategoryIndex++;
             }
        }

        #endregion

        #endregion

        #region " -- Public -- "

        #region " --Properties -- "
       
        private String _LanguageCode = "en";
        /// <summary>
        /// Gets or Sets Language code
        /// </summary>
        public String LanguageCode
        {
            get
            {
                return this._LanguageCode;
            }
            set
            {
                this._LanguageCode = value;
            }
        }

        /// <summary>
        /// Metadata will start from 5th row in excel sheet
        /// </summary>
        private int _MetadataStartRowIndexInExl = 4;
        /// <summary>
        /// Gets or sets metadata start row index in excel file.
        /// </summary>        
        public int MetadataStartRowIndexInExl
        {
            get
            {
                return this._MetadataStartRowIndexInExl;
            }
            set
            {
                this._MetadataStartRowIndexInExl = value;
            }
        }

        private DIConnection DBConnection = null;
        private DIQueries DBQueries = null;

        #endregion

        #region -- New/Dispose --

        public MetadataEditor()
        {
            // do nothing
        }


        public MetadataEditor(DIConnection dbConnection, DIQueries dbQueries)
        {
            this.DBConnection = dbConnection;
            this.DBQueries = dbQueries;
        }

        #endregion

        /// <summary>
        /// Sets Reports Font
        /// </summary>
        /// <param name="Index"></param>
        private void SetReportFont(int Index)
        {
            ((SpreadsheetGear.IRange)this.MetadataSheet.Cells[Index, 0]).Font.Size = 16;
            ((SpreadsheetGear.IRange)this.MetadataSheet.Cells[Index, 0]).Font.Bold = true;
            ((SpreadsheetGear.IRange)this.MetadataSheet.Cells[Index + 1, 0]).Font.Bold = true;
            ((SpreadsheetGear.IRange)this.MetadataSheet.Cells[Index + 2, 0]).Font.Bold = false;
            Index = Index - 3;
        }

        /// <summary>
        /// Export metadata to excel file
        /// </summary>
        /// <param name="ExcelFilePath">Excel File Path to be written as export data</param>
        public void ExportMetaDataToExcel(string ExcelFilePath, MetadataElementType elementType, string elementName, string elementGId,int targetElementNID)
        {
            try
            {
                // --Open excel and get first worksheet
                this.DiExcel = new DIExcel(ExcelFilePath);
                MetadataSheet = DiExcel.GetWorksheet(0);

                // --Set Matadata Type in Cell 0,0
                switch (elementType)
                {
                    case MetadataElementType.Indicator:
                        SetMetadataCommonCellValues(DILanguage.GetLanguageString("INDICATOR") + "-" + DILanguage.GetLanguageString("METADATA"), elementName, elementGId);
                        break;
                    case MetadataElementType.Area:
                        SetMetadataCommonCellValues(DILanguage.GetLanguageString("AREA") + "-" + DILanguage.GetLanguageString("METADATA"), elementName, elementGId);
                        break;
                    case MetadataElementType.Source:
                        SetMetadataCommonCellValues(DILanguage.GetLanguageString("SOURCE") + "-" + DILanguage.GetLanguageString("METADATA"), elementName, elementGId);
                        break;
                    default:
                        break;
                }

                // --Load data from xml to excel file
                this.LoadData(elementType,targetElementNID);

                //-- Save excel 
                this.DiExcel.Save();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Get Metadata from Excel File
        /// </summary>
        /// <remarks>
        /// This method will Extract Metadata from excel file and make a string Containing Matadata in Xml Format .
        /// So this can  be stored in database
        /// </remarks>
        public String GetMetadataFromExcelFile(string excelFilePath, MetaDataType elementType, string fldrMetadataTemplatePath)
        {
            // Step 1 : Open Excel File for reading
            // Step2 : Get Mask and blank Metadata xml file from metadata template Folder
            // Step3 : Update Blank Metadata file from  metadata found in excel file. Metadata will be inserted in xml using
            // Position and path definded in Mask file.

            string RetVal = String.Empty;

            //Step1.  Open excel File
            // Open excel and get first worksheet
            this.DiExcel = new DIExcel(excelFilePath);
            MetadataSheet = DiExcel.GetWorksheet(0);

            // Matadata starts from 5th Row
            //this._MetadataStartRowIndexInExl = 4;

            // step2: Get Mask and blank xml file from Metadata Template Folder
            /////GetMetadataTemplateFilesForImport(elementType, fldrMetadataTemplatePath);

            // Step3: Update metadata blank document using metadata found in metadata excel.
            // Import will be done using Mask File. So check for Mask file
            //if (this._MetadataMaskFilePath.Length > 0 && File.Exists(this._MetadataMaskFilePath))
            //{

            //    if (this._MetadataFilePath.Length > 0 && File.Exists(this._MetadataFilePath))
            //    {
            //        int i = 0;
            //        XmlNode xn = null;
            //        XmlNodeList xnList = null;
            //        try
            //        {
            //            int temp = 0;

            //            // Load Blank xml Structure  as Metadata file                        
            //            MetadataDOM = new XmlDocument();
            //            MetadataDOM.Load(this._MetadataFilePath);

            //            // Load Mask File
            //            MetadataMaskDOM.Load(this._MetadataMaskFilePath);

            //            // Iterate all child elements of mask file.
            //            for (i = 0; i < MetadataMaskDOM.DocumentElement.ChildNodes.Count; i++)
            //            {
            //                try
            //                {
            //                    // Get Position and path information for Metadata
            //                    string[] nodes = MetadataMaskDOM.DocumentElement.ChildNodes[i].SelectNodes("Path")[0].InnerXml.Split('/');
            //                    string[] position = MetadataMaskDOM.DocumentElement.ChildNodes[i].SelectNodes("Position")[0].InnerXml.Split('/');

            //                    if (position.Length < 1)
            //                    {
            //                    }
            //                    else if (position.Length == 1)
            //                    {
            //                        if (MetadataMaskDOM.DocumentElement.ChildNodes[i].SelectNodes("Type")[0].InnerXml == "Element")
            //                        {
            //                            //dom.DocumentElement.InnerXml = this.rtbDataValue[temp].Text;
            //                            MetadataDOM.DocumentElement.InnerXml = this.DiExcel.GetCellValue(0, this._MetadataStartRowIndexInExl + temp, 1, this._MetadataStartRowIndexInExl + temp, 1);
            //                            temp++;
            //                        }
            //                        else if (MetadataMaskDOM.DocumentElement.ChildNodes[i].SelectNodes("Type")[0].InnerXml == "Attribute")
            //                        {
            //                            //dom.DocumentElement.Attributes.GetNamedItem(nodes[nodes.Length - 1]).Value = this.rtbDataValue[temp].Text;
            //                            MetadataDOM.DocumentElement.InnerXml = this.DiExcel.GetCellValue(0, this._MetadataStartRowIndexInExl + temp, 1, this._MetadataStartRowIndexInExl + temp, 1);
            //                            temp++;
            //                        }
            //                    }

            //                    else if (position.Length >= 2)
            //                    {
            //                        // Get the postion of n is postion array 
            //                        int npos = position.Length;
            //                        //--check which position has the n
            //                        for (int l = 1; l < position.Length; l++)
            //                        {
            //                            if (position[l] == "n")
            //                            {
            //                                npos = l;
            //                                break;
            //                            }
            //                        }



            //                        // Select Root Node Like "Indicator_Info" 
            //                        // Xnlist contain list of all node under rootNode like indicator_Info
            //                        xnList = MetadataDOM.DocumentElement.SelectNodes(nodes[1]);

            //                        xnList = MetadataDOM.DocumentElement.SelectNodes(nodes[1]);




            //                        // Handling for second Postion  
            //                        // If n is not at second Postion then then  start from  node  first child under Indicator_Info(Document Element)                           
            //                        if (position[1] != "n")
            //                        {
            //                            xn = xnList[Convert.ToInt32(position[1]) - 1];
            //                        }
            //                        else
            //                        {
            //                            xn = MetadataDOM.DocumentElement;
            //                        }


            //                        // Iterate inside this node. till we reach at n postion
            //                        //--get the value of xn till the nth position     
            //                        if (MetadataMaskDOM.DocumentElement.ChildNodes[i].SelectNodes("Type")[0].InnerXml == "Element")
            //                        {
            //                            for (int j = 2; j < npos; j++)
            //                            {
            //                                XmlNodeList xnTempList;
            //                                // Getting List of all child nodes .
            //                                // If our path nodes array contain Indicator_Info,Row1,FLD_VAL,ROWData,temp1 
            //                                //In First Iteration we selcted all Fld_Val node under Row1
            //                                // In SEcond Iteration  we select AllRowDAta node under FLD_Val
            //                                // Continue until j=  postion of n .So we have all nodes inside nodelist for which n is applied
            //                                xnTempList = xn.SelectNodes(nodes[j]);
            //                                xn = xnTempList[Convert.ToInt32(position[j]) - 1];
            //                            }
            //                            // Insert  metadata value
            //                            if (npos == position.Length)
            //                            {
            //                                //xn.InnerXml = this.rtbDataValue[temp].Text;
            //                                xn.InnerXml = this.DiExcel.GetCellValue(0, this._MetadataStartRowIndexInExl + temp, 1, this._MetadataStartRowIndexInExl + temp, 1);
            //                                temp++;
            //                            }
            //                        }
            //                        else if (MetadataMaskDOM.DocumentElement.ChildNodes[i].SelectNodes("Type")[0].InnerXml == "Attribute")
            //                        {
            //                            for (int j = 2; j < npos - 1; j++)
            //                            {
            //                                XmlNodeList xnTempList;
            //                                xnTempList = xn.SelectNodes(nodes[j]);
            //                                xn = xnTempList[Convert.ToInt32(position[j]) - 1];
            //                            }
            //                            if (npos == position.Length)
            //                            {
            //                                xn.Attributes.GetNamedItem(nodes[nodes.Length - 1]).Value = this.DiExcel.GetCellValue(0, this._MetadataStartRowIndexInExl + temp, 1, this._MetadataStartRowIndexInExl + temp, 1);
            //                                temp++;
            //                            }
            //                        }

            //                        //--get the value of the nodes from the nth position
            //                        if (npos < position.Length)
            //                        {
            //                            // Get all row data for which we have n in  position
            //                            xnList = xn.SelectNodes(nodes[npos]);
            //                            //xnlist is value for total no of metadata paragraph required
            //                            for (int o = 0; o < xnList.Count; o++)
            //                            {
            //                                try
            //                                {
            //                                    xn = xnList[o];
            //                                    if (MetadataMaskDOM.DocumentElement.ChildNodes[i].SelectNodes("Type")[0].InnerXml == "Element")
            //                                    {
            //                                        // Handling for after n node
            //                                        for (int j = npos + 1; j < nodes.Length; j++)
            //                                        {
            //                                            XmlNodeList xnTempList;
            //                                            xnTempList = xn.SelectNodes(nodes[j]);
            //                                            xn = xnTempList[Convert.ToInt32(position[j]) - 1];
            //                                        }

            //                                        // Get Value of each metadata
            //                                        // xn.InnerXml = SetCharacterEntities(this.rtbDataValue[temp].Text);
            //                                        xn.InnerXml = SetCharacterEntities(this.DiExcel.GetCellValue(0, this._MetadataStartRowIndexInExl + temp, 1, this._MetadataStartRowIndexInExl + temp, 1).ToString());

            //                                        temp++;
            //                                    }
            //                                    else if (MetadataMaskDOM.DocumentElement.ChildNodes[i].SelectNodes("Type")[0].InnerXml == "Attribute")
            //                                    {
            //                                        for (int j = npos + 1; j < nodes.Length - 1; j++)
            //                                        {
            //                                            XmlNodeList xnTempList;
            //                                            xnTempList = xn.SelectNodes(nodes[j]);
            //                                            xn = xnTempList[Convert.ToInt32(position[j]) - 1];
            //                                        }

            //                                        //xn.Attributes.GetNamedItem(nodes[nodes.Length - 1]).Value = SetCharacterEntities(this.rtbDataValue[temp].Text);
            //                                        xn.Attributes.GetNamedItem(nodes[nodes.Length - 1]).Value = SetCharacterEntities(this.DiExcel.GetCellValue(0, this._MetadataStartRowIndexInExl + temp, 1, this._MetadataStartRowIndexInExl + temp, 1).ToString());
            //                                        temp++;
            //                                    }
            //                                }
            //                                catch (Exception ex)
            //                                {
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //                catch (Exception ex)
            //                {
            //                }
            //            }
            //            // Get Metadata Text in RetVal
            //            RetVal = MetadataDOM.InnerXml; //MetadataDOM.Save(xmlFile);
            //            DiExcel.Close();

            //        }
            //        catch (Exception ex)
            //        {
            //            RetVal = String.Empty;
            //            DiExcel.Close();
            //        }
            //    }
            //}
            return RetVal;
        }

        #endregion
    }
    class MetadataEditorCostants
    {
        public const string MetadataCategory = "//Category";
        public const string MetadataCategorySpecialSymbol = "{{~}}";
    }

}

