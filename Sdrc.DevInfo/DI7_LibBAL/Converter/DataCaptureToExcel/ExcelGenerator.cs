using System;
using System.Collections.Generic;
using System.Text;

using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibDataCapture;
using DevInfo.Lib.DI_LibDataCapture.Questions;
using DevInfo.Lib.DI_LibDataCapture.Questions.ColumnNames;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
//using DevInfo.UC.DI_UCExcelControl;
using System.IO;

namespace DevInfo.Lib.DI_LibBAL.Converter.DataCaptureToExcel
{
    public class ExcelGenerator
    {

        #region "-- Private --"
        
        #region "-- Variables --"


        #endregion

        #region "-- Methods --"

        private void SetExcelFileNames(List<string> xmlFiles)
        {
            string ExcelFileName=string.Empty;

           this._Files=new  Dictionary<string,string>();
           foreach (string XmlFile in xmlFiles)
            {
                ExcelFileName=Path.GetDirectoryName(XmlFile) +"\\"+ Path.GetFileNameWithoutExtension(XmlFile) + DICommon.FileExtension.Excel;
                this._Files.Add(XmlFile, ExcelFileName);
            } 
        }

        private void GenerateExcelFile(string xmlFileName,string excelFileName)
        {

            Questionnarie XMLFile = new Questionnarie();
            DIExcel ExcelFile;
            int RowIndex=1;
            string HeaderText = string.Empty;
            try
            {
                ExcelFile= new DIExcel();//excelFileName);
                if (XMLFile.OpenQuestionnaire(xmlFileName))
                {
                    foreach (Question XmlQuestion in XMLFile.GetAllQuestions().Values)
                    {
                        //Write Section Name
                        if (HeaderText != XmlQuestion.HeaderTxt)
                        {
                            HeaderText = XmlQuestion.HeaderTxt;
                            this.WriteHeader(ExcelFile, RowIndex, HeaderText);
                            RowIndex += 1;
                        }

                        this.WriteQuestionIntoExcel(ExcelFile, XmlQuestion,ref RowIndex);
                    }

                    //delete file if already exists
                    if (File.Exists(excelFileName))
                    {
                        File.Delete(excelFileName);
                    }

                    ExcelFile.SaveAs(excelFileName);
                    ExcelFile.Close();
                }
            }
            catch (Exception)
            {
                
            }           

        }

        private void WriteHeader(DIExcel excelFile, int rowIndex, string headerText)
        {
            excelFile.SetCellValue(Constants.SheetIndex, rowIndex, Constants.HeaderTextColumnIndex, headerText);
            excelFile.GetCellFont(Constants.SheetIndex, rowIndex, Constants.HeaderTextColumnIndex).Bold = true;
            excelFile.GetCellFont(Constants.SheetIndex, rowIndex, Constants.HeaderTextColumnIndex).Size = 12;
        }

        private void WriteQuestionIntoExcel(DIExcel excelFile, Question xmlQuestion,ref int RowIndex)
        {
            int DataValueRowIndex = RowIndex + 1;
            int BoldStartIndex = -1;
            int BoldEndIndex = -1;
            int ItalicStartIndex = -1;
            int ItalicEndIndex = -1;
            string BoldStartTag="<b>";
            string ItalicStartTag="<i>";
            string BoldEndTag = "</b>";
            string ItalicEndTag = "</i>";
            string QuestionText=xmlQuestion.Text;

            //set question no and question text
            excelFile.SetCellValue(Constants.SheetIndex, RowIndex, Constants.QuestionNoColumnIndex, xmlQuestion.No);
            excelFile.SetColumnWidth(Constants.SheetIndex, 6, RowIndex, Constants.QuestionNoColumnIndex, RowIndex, Constants.QuestionNoColumnIndex);
            //replace <br> with new line character and remove enter key
            
QuestionText= QuestionText.Replace(Char.ConvertFromUtf32(13), string.Empty);

QuestionText = QuestionText.Replace("\n", string.Empty);
QuestionText = QuestionText.Replace(Microsoft.VisualBasic.ControlChars.CrLf, string.Empty);
QuestionText = QuestionText.Replace(Microsoft.VisualBasic.ControlChars.Lf.ToString(), string.Empty);
QuestionText=QuestionText.Replace("<br>", Microsoft.VisualBasic.ControlChars.NewLine);

            //apply bold tag
            BoldStartIndex = QuestionText.IndexOf(BoldStartTag);
            if (BoldStartIndex >= 0)
            {
                QuestionText = QuestionText.Replace(BoldStartTag, string.Empty);
                BoldEndIndex = QuestionText.IndexOf(BoldEndTag);
                QuestionText = QuestionText.Replace(BoldEndTag, string.Empty);
            }

            //apply italic tag
            ItalicStartIndex = QuestionText.IndexOf(ItalicStartTag);
            if (ItalicStartIndex >= 0)
            {
                QuestionText = QuestionText.Replace(ItalicStartTag, string.Empty);
                ItalicEndIndex = QuestionText.IndexOf(ItalicEndTag);
                QuestionText = QuestionText.Replace(ItalicEndTag, string.Empty);
            }

            //set question text
            QuestionText.Replace(ItalicStartTag, string.Empty);
            excelFile.SetCellValue(Constants.SheetIndex, RowIndex, Constants.QuestionTextColumnIndex, QuestionText);

            //set bold
            if (BoldStartIndex >= 0 & BoldEndIndex >= 0)
            {
                excelFile.GetCellCharacters(Constants.SheetIndex, Constants.QuestionTextColumnIndex, RowIndex, BoldStartIndex, BoldEndIndex-BoldStartIndex).Font.Bold = true;
            }
            
            //set italic
            if (ItalicStartIndex >= 0 & ItalicEndIndex >= 0)
            {
                excelFile.GetCellCharacters(Constants.SheetIndex, Constants.QuestionTextColumnIndex, RowIndex, ItalicStartIndex, ItalicEndIndex-ItalicStartIndex).Font.Italic= true;
            }
            

            //set layout of question text cell and question no cell
            //excelFile.MergeCells(Constants.SheetIndex, RowIndex, Constants.QuestionTextColumnIndex, RowIndex, Constants.QuestionTextColumnIndex + 6);
            //excelFile.SetHorizontalAlignment(Constants.SheetIndex, RowIndex, Constants.QuestionTextColumnIndex, SpreadsheetGear.HAlign.Justify);
            //excelFile.SetVerticalAlignment(Constants.SheetIndex, RowIndex, Constants.QuestionTextColumnIndex, SpreadsheetGear.VAlign.Justify);
            //excelFile.GetCellFont(Constants.SheetIndex, RowIndex, Constants.QuestionTextColumnIndex).Bold = true;

            excelFile.GetCellFont(Constants.SheetIndex, RowIndex, Constants.QuestionNoColumnIndex).Bold = true;
            excelFile.AutoFitColumn(Constants.SheetIndex, Constants.QuestionNoColumnIndex);


            this.WriteDataValue(excelFile, xmlQuestion,ref DataValueRowIndex);

            RowIndex =DataValueRowIndex+ 2;
        }

        private void WriteDataValue(DIExcel excelFile, Question xmlQuestion,ref int dataValueRowIndex)
        {
            string[] SelectedValues =null;
            int Index=0;
            switch (xmlQuestion.AnswerType)
            {
                case AnswerType.TB:
                case AnswerType.TBN:
                case AnswerType.RB:
                case AnswerType.CB:
                case AnswerType.DateType:
                    excelFile.SetCellValue(Constants.SheetIndex, dataValueRowIndex, Constants.DataValueColumnIndex, xmlQuestion.DataValue);
                    break;
                case AnswerType.SCB:
                case AnswerType.SRB:
                    break;
                case AnswerType.CH:
                    SelectedValues = DICommon.SplitStringNIncludeEmpyValue(xmlQuestion.DataValue, ",");
                    foreach (AnswerTypeOption Option in xmlQuestion.Options)
                    {
                        excelFile.GetCellFont(Constants.SheetIndex, dataValueRowIndex, Constants.DataValueColumnIndex).Name = "Wingdings";

                        if (SelectedValues[Index] == "1")
                        {
                            excelFile.SetCellValue(Constants.SheetIndex, dataValueRowIndex, Constants.DataValueColumnIndex, Char.ConvertFromUtf32(252) );
                        }
                        else
                        {
                            excelFile.SetCellValue(Constants.SheetIndex, dataValueRowIndex, Constants.DataValueColumnIndex, Char.ConvertFromUtf32(251));
                        }
                     
                        
                        excelFile.SetCellValue(Constants.SheetIndex, dataValueRowIndex, Constants.DataValueColumnIndex + 1, Option.Text);
                        dataValueRowIndex += 1;
                        Index += 1;
                    }
                    break;
                case AnswerType.GridType:
                    int StartRowIndex=dataValueRowIndex;
                    int EndRowIndex;
                    int EndColIndex;
                    excelFile.LoadDataTableIntoSheet(dataValueRowIndex,Constants.DataValueColumnIndex, xmlQuestion.GridTable,Constants.SheetIndex, true);
                    dataValueRowIndex += xmlQuestion.GridTable.Rows.Count;
                    EndRowIndex=dataValueRowIndex-1;
                    EndColIndex =Constants.DataValueColumnIndex+  xmlQuestion.GridTable.Columns.Count-1;
                    excelFile.SetRangeBorder(Constants.SheetIndex, StartRowIndex, Constants.DataValueColumnIndex, EndRowIndex, EndColIndex,
                         SpreadsheetGear.LineStyle.Continous, SpreadsheetGear.BorderWeight.Thin, System.Drawing.Color.Black, SpreadsheetGear.BordersIndex.InsideHorizontal);
                    excelFile.SetRangeBorder(Constants.SheetIndex, StartRowIndex, Constants.DataValueColumnIndex, EndRowIndex, EndColIndex,
                         SpreadsheetGear.LineStyle.Continous, SpreadsheetGear.BorderWeight.Thin, System.Drawing.Color.Black, SpreadsheetGear.BordersIndex.InsideVertical);
                    excelFile.SetRangeBorder(Constants.SheetIndex, StartRowIndex, Constants.DataValueColumnIndex, EndRowIndex, EndColIndex,
                         SpreadsheetGear.LineStyle.Continous, SpreadsheetGear.BorderWeight.Thin, System.Drawing.Color.Black, SpreadsheetGear.BordersIndex.EdgeBottom);
                    excelFile.SetRangeBorder(Constants.SheetIndex, StartRowIndex, Constants.DataValueColumnIndex, EndRowIndex, EndColIndex,
                         SpreadsheetGear.LineStyle.Continous, SpreadsheetGear.BorderWeight.Thin, System.Drawing.Color.Black, SpreadsheetGear.BordersIndex.EdgeTop);
                    excelFile.SetRangeBorder(Constants.SheetIndex, StartRowIndex, Constants.DataValueColumnIndex, EndRowIndex, EndColIndex,
                                             SpreadsheetGear.LineStyle.Continous, SpreadsheetGear.BorderWeight.Thin, System.Drawing.Color.Black, SpreadsheetGear.BordersIndex.EdgeLeft);
                    excelFile.SetRangeBorder(Constants.SheetIndex, StartRowIndex, Constants.DataValueColumnIndex, EndRowIndex, EndColIndex,
                         SpreadsheetGear.LineStyle.Continous, SpreadsheetGear.BorderWeight.Thin, System.Drawing.Color.Black, SpreadsheetGear.BordersIndex.EdgeRight);
                    
                    
                    break;
                case AnswerType.Calculate:
                    break;
                case AnswerType.Aggregate:
                    break;
                
                default:
                    break;
            }
        }


        private void ApplyHTMLTags()
        {

        }

        #endregion

        #endregion

        #region "-- Public --"

        #region "-- Variables /Properties --"

        private Dictionary<string,string> _Files;
        /// <summary>
        /// Sets source xml files.
        /// </summary>
        public Dictionary<string,string> Files
        {
            set
            {
                this._Files = value; 
            }
        }
	
#endregion

        #region "--  New/Dispose --"

        public ExcelGenerator(List<string> sourceXMLFiles)
        {
            this.SetExcelFileNames(sourceXMLFiles);
        }

        #endregion

        #region "-- Methods --"

        public void GenerateExcelFiles()
        {
            foreach (string XmlFileName in this._Files.Keys)
            {
                this.GenerateExcelFile(XmlFileName, this._Files[XmlFileName]);
            }
        }

        #endregion

        #endregion

    }
}
