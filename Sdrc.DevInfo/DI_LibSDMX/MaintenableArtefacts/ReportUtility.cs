using System;
using System.Collections.Generic;
using System.Text;
using SDMXObjectModel;
using SpreadsheetGear;
using SDMXObjectModel.Message;
using SDMXObjectModel.Structure;
using System.Xml;
using System.IO;
using DevInfo.Lib.DI_LibSDMX.DIExcelWrapper;

namespace DevInfo.Lib.DI_LibSDMX
{
    internal class ReportUtility : ArtefactUtility
    {
        #region "--Properties--"

        #region "--Private--"

        private string _completeXmlFileNameWPath;

        #endregion "--Private--"

        #region "--Public--"

        internal string CompleteXmlFileNameWPath
        {
            get
            {
                return this._completeXmlFileNameWPath;
            }
            set
            {
                this._completeXmlFileNameWPath = value;
            }
        }

        #endregion "--Public--"

        #endregion "--Properties--"

        #region "--Constructors--"

        internal ReportUtility(string completeXmlFileNameWPath, string agencyId, string language, string outputFolder)
            : base(agencyId, language, null, outputFolder)
        {
            this._completeXmlFileNameWPath = completeXmlFileNameWPath;
        }

        #endregion "--Constructors--"

        #region "--Methods--"

        #region "--Private--"

        private DIExcel GenerateDSDWorksheet(DIExcel ReportExcel, int SheetIndex, SDMXObjectModel.Message.StructureType CompleteStructure)
        {

            int i, j, k;
            SDMXObjectModel.Structure.DataStructureComponentsType DSComponents;
            IWorksheet DSDWorkSheet = null;
            SDMXObjectModel.Structure.MeasureListType MeasureList;
            SDMXObjectModel.Structure.DimensionType Dimension;
            SDMXObjectModel.Structure.TimeDimensionType TimeDimension;
            SDMXObjectModel.Structure.AttributeType Attribute;
            SDMXObjectModel.Common.ConceptReferenceType ConceptIdentity;
            SDMXObjectModel.Structure.AttributeRelationshipType AttributeRelationship;
            SDMXObjectModel.Common.LocalPrimaryMeasureReferenceType LocalPrimaryMeasureReference;
            SDMXObjectModel.Structure.PrimaryMeasureType PrimaryMeasure;
            string AttributeImportance = string.Empty;

            SDMXObjectModel.Structure.StructuresType ConceptsObj;
            int rowindex = 0;

            DSComponents = new DataStructureComponentsType();
            try
            {
                DSComponents = (SDMXObjectModel.Structure.DataStructureComponentsType)(CompleteStructure.Structures.DataStructures[0].Item);
                ConceptsObj = CompleteStructure.Structures;

                DSDWorkSheet = ReportExcel.GetWorksheet(0);
                ReportExcel.RenameWorkSheet(0, "DSD");
                rowindex = rowindex + 1;
                this.WriteValueInCell(ReportExcel, "Data Structure Definition", rowindex, 1, 14, true, 30, 0, 0);
                rowindex = rowindex + 2;
                //Binding Dimensions  
                this.WriteValueInCell(ReportExcel, "Dimensions", rowindex, 1, 12, true, 30, 0, 0);
                rowindex = rowindex + 2;
                for (i = 0; i < DSComponents.Items[0].Items.Count; i++)
                {

                    if (DSComponents.Items[0].Items[i] is SDMXObjectModel.Structure.TimeDimensionType)
                    {
                        TimeDimension = (SDMXObjectModel.Structure.TimeDimensionType)(DSComponents.Items[0].Items[i]);
                        ConceptIdentity = TimeDimension.ConceptIdentity;
                    }
                    else
                    {
                        Dimension = (SDMXObjectModel.Structure.DimensionType)(DSComponents.Items[0].Items[i]);
                        ConceptIdentity = Dimension.ConceptIdentity;
                    }

                    for (j = 0; j < ConceptsObj.Concepts.Count; j++)
                        if (((SDMXObjectModel.Common.ConceptRefType)(ConceptIdentity.Items[0])).maintainableParentID.ToString() == ConceptsObj.Concepts[j].id.ToString())
                        {

                            for (k = 0; k < ConceptsObj.Concepts[j].Items.Count; k++)
                            {
                                if (((SDMXObjectModel.Common.ConceptRefType)(ConceptIdentity.Items[0])).id.ToString() == ConceptsObj.Concepts[j].Items[k].id.ToString())
                                {
                                    this.WriteValueInCell(ReportExcel, GetLangSpecificValue(ConceptsObj.Concepts[j].Items[k].Name, this.Language), rowindex, 1, 10, false, 30, 0, 0);
                                    this.WriteValueInCell(ReportExcel, GetLangSpecificValue(ConceptsObj.Concepts[j].Items[k].Description, this.Language), rowindex, 2, 10, false, 250, 0, 0);
                                    rowindex = rowindex + 1;

                                    break;
                                }
                            }

                        }
                }

                rowindex = rowindex + 2;
                //Binding Attributes  
                this.WriteValueInCell(ReportExcel, "Attributes", rowindex, 1, 12, true, 30, 0, 0);

                rowindex = rowindex + 2;
                for (i = 0; i < DSComponents.Items[1].Items.Count; i++)
                {
                    Attribute = (SDMXObjectModel.Structure.AttributeType)(DSComponents.Items[1].Items[i]);
                    ConceptIdentity = Attribute.ConceptIdentity;
                    AttributeRelationship = Attribute.AttributeRelationship;
                    LocalPrimaryMeasureReference = (SDMXObjectModel.Common.LocalPrimaryMeasureReferenceType)(AttributeRelationship.Items[0]);
                    for (j = 0; j < ConceptsObj.Concepts.Count; j++)
                    {
                        if (((SDMXObjectModel.Common.ConceptRefType)(ConceptIdentity.Items[0])).maintainableParentID.ToString() == ConceptsObj.Concepts[j].id.ToString())
                        {

                            for (k = 0; k < ConceptsObj.Concepts[j].Items.Count; k++)
                            {
                                if (((SDMXObjectModel.Common.ConceptRefType)(ConceptIdentity.Items[0])).id.ToString() == ConceptsObj.Concepts[j].Items[k].id.ToString())
                                {
                                    this.WriteValueInCell(ReportExcel, GetLangSpecificValue(ConceptsObj.Concepts[j].Items[k].Name, this.Language), rowindex, 1, 10, false, 30, 0, 0);
                                    this.WriteValueInCell(ReportExcel, GetLangSpecificValue(ConceptsObj.Concepts[j].Items[k].Description, this.Language), rowindex, 2, 10, false, 250, 0, 0);
                                    rowindex = rowindex + 1;


                                    this.WriteValueInCell(ReportExcel, "Attachment Level : " + ((SDMXObjectModel.Common.LocalPrimaryMeasureRefType)(LocalPrimaryMeasureReference.Items[0])).id.ToString(), rowindex, 1, 10, false, 30, 0, 0);

                                    if (((UsageStatusType)(Attribute.assignmentStatus)) == UsageStatusType.Mandatory)
                                    {
                                        AttributeImportance = "Mandatory : " + "Yes";
                                    }
                                    else
                                    {
                                        AttributeImportance = "Mandatory : " + "No";
                                    }
                                    this.WriteValueInCell(ReportExcel, AttributeImportance, rowindex, 2, 10, false, 30, 0, 0);
                                    rowindex = rowindex + 2;

                                    break;
                                }
                            }

                        }
                    }


                }


                rowindex = rowindex + 1;

                //Binding Measure  
                this.WriteValueInCell(ReportExcel, "Measure", rowindex, 1, 12, true, 30, 0, 0);

                rowindex = rowindex + 1;

                MeasureList = ((SDMXObjectModel.Structure.MeasureListType)((SDMXObjectModel.Structure.DataStructureComponentsType)(DSComponents)).MeasureList);

                for (i = 0; i < DSComponents.Items[2].Items.Count; i++)
                {

                    PrimaryMeasure = (SDMXObjectModel.Structure.PrimaryMeasureType)(DSComponents.Items[2].Items[i]);
                    ConceptIdentity = PrimaryMeasure.ConceptIdentity;
                    for (j = 0; j < ConceptsObj.Concepts.Count; j++)
                    {
                        if (((SDMXObjectModel.Common.ConceptRefType)(ConceptIdentity.Items[0])).maintainableParentID.ToString() == ConceptsObj.Concepts[j].id.ToString())
                        {

                            for (k = 0; k < ConceptsObj.Concepts[j].Items.Count; k++)
                            {
                                if (((SDMXObjectModel.Common.ConceptRefType)(ConceptIdentity.Items[0])).id.ToString() == ConceptsObj.Concepts[j].Items[k].id.ToString())
                                {
                                    this.WriteValueInCell(ReportExcel, GetLangSpecificValue(ConceptsObj.Concepts[j].Items[k].Name, this.Language), rowindex, 1, 10, false, 30, 0, 0);
                                    this.WriteValueInCell(ReportExcel, GetLangSpecificValue(ConceptsObj.Concepts[j].Items[k].Description, this.Language), rowindex, 2, 10, false, 250, 0, 0);
                                    rowindex = rowindex + 1;

                                    break;
                                }
                            }

                        }

                    }

                }



            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }

            return ReportExcel;
        }

        private DIExcel GenerateCodelistWorksheets(DIExcel ReportExcel, int SheetIndex, SDMXObjectModel.Message.StructureType CompleteStructure)
        {

            int i, j;
            IWorksheet CodelistWorkSheet = null;
            SDMXObjectModel.Structure.CodelistType Codelist;
            SDMXObjectModel.Structure.CodeType Code;
            string AttributeImportance = string.Empty;
            string CodelistName = string.Empty;
            string CodeId = string.Empty;
            string CodeName = string.Empty;

            SDMXObjectModel.Structure.StructuresType ConceptsObj;
            int rowindex = 0;

            try
            {

                ConceptsObj = CompleteStructure.Structures;



                for (i = 1; i <= CompleteStructure.Structures.Codelists.Count; i++)
                {

                    Codelist = new CodelistType();
                    Codelist = CompleteStructure.Structures.Codelists[i - 1];
                    CodelistName = GetLangSpecificValue(Codelist.Name, this.Language);
                    ReportExcel.InsertWorkSheet(CodelistName);
                    CodelistWorkSheet = ReportExcel.GetWorksheet(i);
                    rowindex = 1;
                    this.WriteValueInCell(ReportExcel, GetLangSpecificValue(Codelist.Description, this.Language), rowindex, 1, 12, true, 30, 0, i);
                    rowindex = rowindex + 2;

                    this.WriteValueInCell(ReportExcel, "Code", rowindex, 1, 10, true, 60, 0, i);
                    this.WriteValueInCell(ReportExcel, "Name", rowindex, 2, 10, true, 250, 0, i);

                    rowindex = rowindex + 2;
                    //Binding Codelist  

                    for (j = 0; j < Codelist.Items.Count; j++)
                    {
                        Code = new CodeType();
                        Code = ((SDMXObjectModel.Structure.CodeType)(Codelist.Items[j]));
                        CodeId = Code.id;
                        CodeName = GetLangSpecificValue(Code.Name, this.Language);
                        if ((CodeId.Length + 1) <= 30)
                        {
                            this.WriteValueInCell(ReportExcel, CodeId, rowindex, 1, 10, false, 30, 0, i);
                        }
                        else
                        {
                            this.WriteValueInCell(ReportExcel, CodeId, rowindex, 1, 10, false, CodeId.Length + 1, 0, i);
                        }
                        
                        this.WriteValueInCell(ReportExcel, CodeName, rowindex, 2, 10, false, 250, 0, i);
                        rowindex = rowindex + 1;

                    }
                }

            }
            catch (Exception ex)
            {
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

        private string GetLangSpecificValue(List<SDMXObjectModel.Common.TextType> ListOfValues, string LangCode)
        {
            string Retval = string.Empty;
            foreach (SDMXObjectModel.Common.TextType ObjectValue in ListOfValues)
            {
                if (ObjectValue.lang.ToString() == LangCode)
                {
                    Retval = ObjectValue.Value.ToString();
                    break;
                }
            }
            if (Retval == string.Empty)
            {
                //ListOfValues index error was thrown
                //if (ListOfValues.Count > 0)
                //{
                    Retval = ListOfValues[0].Value.ToString();
                //}
            }
            return Retval;

        }

        #endregion "--Private--"

        #region "--Public--"

        public override List<ArtefactInfo> Generate_Artefact()
        {
            List<ArtefactInfo> RetVal;
            SDMXObjectModel.Message.StructureType CompleteStructure;
            ArtefactInfo Artefact;
            RetVal = null;
            CompleteStructure = null;
            Artefact = null;

            try
            {
                DIExcel ReportExcel = new DIExcel();
                CompleteStructure = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), this._completeXmlFileNameWPath);
                ReportExcel=this.GenerateDSDWorksheet(ReportExcel, 0, CompleteStructure);
                ReportExcel=this.GenerateCodelistWorksheets(ReportExcel, 0, CompleteStructure);

                ReportExcel.ActiveSheetIndex = 0;
                ReportExcel.SaveAs(Path.Combine(this.OutputFolder, Constants.Report.FileName));
                Artefact = new ArtefactInfo(Constants.Report.Id, this.AgencyId, Constants.Report.Version, string.Empty, ArtefactTypes.Report, Constants.Report.FileName, null);
                
                this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);
            }
            catch (Exception ex)
            {
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
}
