// ***********************Copy Right Notice*****************************
// 
// **********************************************************************
// Program Name:									       
// Developed By: DG6
// Creation date: 2007-10-31							
// Program Comments: 
// **********************************************************************
// **********************Change history*********************************
// No.	Mod: Date	      Mod: By	       Change Description		        
// c1   2008-04-01        DG6               Delete the calculate question if all depenedent                                               questions are not answered.
//											          
// **********************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDataCapture;
using DevInfo.Lib.DI_LibDataCapture.Questions;
using DevInfo.Lib.DI_LibDataCapture.Questions.ColumnNames;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;

namespace DevInfo.Lib.DI_LibBAL.Import.DataCapture
{
    internal class ElXmlProcessor
    {
        #region"--Private--"

        #region"--Variable--"

        #endregion

        #region"--Method--"        

        private void ProcessCalculateTypeQuestions(string fileNameWPath)
        {
            string CalculatedValue = string.Empty;
            DataSet QuestionnaireDataset = new DataSet();
            DataRow[] Rows;
            string FormulaString = string.Empty;
            string[] QuestionsArray;
            string NumericValue;
            DataRow QuestionRow;

            System.Threading.Thread thisThread = System.Threading.Thread.CurrentThread;
            System.Globalization.CultureInfo originalCulture = thisThread.CurrentCulture;
            string NumDecSep = originalCulture.NumberFormat.NumberDecimalSeparator;
            string NumGroupDecSep = originalCulture.NumberFormat.NumberGroupSeparator;

            // Use an exception block to switch back in case of a run-time error.
            try
            {
                thisThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");


                try
                {
                    QuestionnaireDataset.ReadXml(fileNameWPath, XmlReadMode.ReadSchema);


                    // insert GPS Value
                    this.InsertGPSValue(QuestionnaireDataset);

                    Rows = QuestionnaireDataset.Tables[TableNames.QuestionTable].Select(QuestionTableColumns.AnswerType + " ='" + Questionnarie.AnswerTypes[AnswerType.Calculate] + "'");

                    foreach (DataRow Row in Rows)
                    {
                        FormulaString = Row[QuestionTableColumns.DataValue].ToString();
                        FormulaString = FormulaString.Replace(" ", "");
                        if (FormulaString.Length > 0)
                        {
                            // find questions and string_ID used to create formula
                            QuestionsArray = Utility.SplitStringNIncludeEmpyValue(FormulaString.Substring(FormulaString.IndexOf("@@@")), ",");
                            if (QuestionsArray.Length > 0)
                            {
                                QuestionsArray[0] = QuestionsArray[0].Substring(3);
                            }

                            // replace formula string 
                            FormulaString = FormulaString.Substring(0, FormulaString.IndexOf("@@@"));

                            // get Data_Value of each question
                            foreach (string QuestionKey in QuestionsArray)
                            {
                                if (QuestionKey.StartsWith("Q"))
                                {
                                    QuestionRow = QuestionnaireDataset.Tables[TableNames.QuestionTable].Select(QuestionTableColumns.QuestionKey + "='" + QuestionKey + "'")[0];
                                    NumericValue = QuestionRow[QuestionTableColumns.NumericValue].ToString();

                                    if (NumericValue.Length == 0)
                                    {
                                        ////Temp : only for tbn if numeric value is missing 
                                        if (QuestionRow[QuestionTableColumns.AnswerType].ToString() == Questionnarie.AnswerTypes[AnswerType.TBN])
                                        {
                                            try
                                            {
                                                if (!string.IsNullOrEmpty(QuestionRow[QuestionTableColumns.DataValue].ToString()))
                                                {
                                                    NumericValue = QuestionRow[QuestionTableColumns.DataValue].ToString();
                                                }
                                            }
                                            catch (Exception)
                                            {
                                                NumericValue = string.Empty;
                                            }
                                        }
                                    }


                                    if (NumericValue.Length == 0)
                                    {
                                    
                                        #region "-- start change for no c1 --"

                                        // as per the new requirement, delete the calculate question if all depenedent questions are not answered.
                                        //Old logic:
                                        // if data value is nothing or empty then delete the calculate question
                                        NumericValue = string.Empty;
                                        Row.Delete();
                                        FormulaString = string.Empty;

                                        //delete calculate question
                                        QuestionnaireDataset.Tables[TableNames.QuestionTable].AcceptChanges();


                                        // update total question no in xml file
                                        DataRow TotalQuestionValueRow = QuestionnaireDataset.Tables[TableNames.QuestionTable].Select(QuestionTableColumns.QuestionKey + "='" + MandatoryQuestionsKey.TotalQuestions + "'")[0];
                                        TotalQuestionValueRow[QuestionTableColumns.QuestionText] = Convert.ToInt32(TotalQuestionValueRow[QuestionTableColumns.QuestionText]) - 1;
                                        QuestionnaireDataset.Tables[TableNames.QuestionTable].AcceptChanges();
                                        break;

                                        //sNumericValue = "0"
                                        //dr.Item("Data_Value") = ""
                                        //dr.Item("AnsType") = "TBN"
                                        //dr.Delete()     '-- calculate: Question
                                        //dts.Tables("Question").AcceptChanges()
                                        //sFormula = ""
                                        ////'-- update total question no in xml file
                                        ///Dim dr1 As DataRow = dts.Tables("Question").Select("Key='TotalQuestions'")(0)
                                        ////dr1.Item("IUS") = CInt(dr1.Item("IUS")) - 1
                                        ////dts.Tables("Question").AcceptChanges()
                                        ////dr1 = Nothing
                                        ////Exit For

                                        #region "-- Old logic --" 

                                        //// if numeric value of any target question is nothing or null then replace it with zero in calculate questions
                                        //NumericValue = "0";
                                                                               

                                        ////Temp : only for tbn if numeric value is missing 
                                        //if (QuestionRow[QuestionTableColumns.AnswerType].ToString() == Questionnarie.AnswerTypes[AnswerType.TBN])
                                        //{
                                        //    try
                                        //    {
                                        //        if (!string.IsNullOrEmpty(QuestionRow[QuestionTableColumns.DataValue].ToString()))
                                        //        {
                                        //            NumericValue = QuestionRow[QuestionTableColumns.DataValue].ToString();
                                        //        }
                                        //    }
                                        //    catch (Exception)
                                        //    {
                                        //        NumericValue = "0";
                                        //    }
                                        //}

                                        #endregion


                                        #endregion
                                    }

                                    thisThread.CurrentCulture = originalCulture;

                                    if (Microsoft.VisualBasic.Information.IsNumeric(NumericValue))
                                    {
                                        NumericValue = NumericValue.Replace(NumGroupDecSep, "");
                                        NumericValue = NumericValue.Replace(NumDecSep, ".");
                                    }

                                    thisThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

                                    //replace Question no with DataValue in the Formula
                                    FormulaString = FormulaString.Replace("'" + QuestionKey + "'", NumericValue);
                                }

                                if (QuestionKey.StartsWith("S"))
                                {
                                    FormulaString = FormulaString.Replace("'" + QuestionKey + "'", QuestionKey.Substring(1));
                                }



                            }

                            if (FormulaString.Length > 0)    // sFormula.length=0 when calculate question has been deleted
                            {
                                // calculate value and stores it into 
                                string str = string.Empty;
                                str = "=" + FormulaString.Trim();

                                try
                                {

                                    // Get the result.
                                    CalculatedValue = DIExcel.Evaluate(str);
                                    CalculatedValue = Math.Round(Convert.ToDecimal( CalculatedValue), 2).ToString();
                                }
                                catch (Exception)
                                {
                                    CalculatedValue = "0";
                                }

                                Row[QuestionTableColumns.DataValue] = CalculatedValue; 
                                Row[QuestionTableColumns.AnswerType] = Questionnarie.AnswerTypes[AnswerType.TBN];
                                QuestionnaireDataset.Tables[TableNames.QuestionTable].AcceptChanges();
                            }
                        }
                        FormulaString = "";
                    }

                    QuestionnaireDataset.WriteXml(fileNameWPath, XmlWriteMode.WriteSchema);
                }
                catch (Exception)
                {
                    //Call showErrorMessage(ex)
                }                
            }
            finally
            {
                // Restore the culture information for the thread after the Excel calls have completed.
                thisThread.CurrentCulture = originalCulture;
            }
        }

        private void InsertGPSValue(DataSet questionnaireDataSet)
        {

            DataRow[] Rows = questionnaireDataSet.Tables[TableNames.QuestionTable].Select(QuestionTableColumns.DataValue + " Like '" + Constants.Latitude + "'");
            DataRow Row;

            // save latitude
            if (Rows.Length > 0)
            {
                Row = questionnaireDataSet.Tables[TableNames.QuestionTable].Select(QuestionTableColumns.QuestionKey + " ='" + MandatoryQuestionsKey.Latitude + "' ")[0];
                Rows[0][QuestionTableColumns.DataValue] = Row[QuestionTableColumns.DataValue].ToString();
                Rows[0][QuestionTableColumns.AnswerType] = Questionnarie.AnswerTypes[AnswerType.TBN];
                questionnaireDataSet.Tables[TableNames.QuestionTable].AcceptChanges();
            }
            Row = null;


            // save longitude
            Rows = null;
            Rows = questionnaireDataSet.Tables[TableNames.QuestionTable].Select(QuestionTableColumns.DataValue + " like '" + Constants.Longitude + "'");
            if (Rows.Length > 0)
            {
                Row = questionnaireDataSet.Tables[TableNames.QuestionTable].Select(QuestionTableColumns.QuestionKey + " ='" + MandatoryQuestionsKey.Longitude + "' ")[0];
                Rows[0][QuestionTableColumns.DataValue] = Row[QuestionTableColumns.DataValue].ToString();
                Rows[0][QuestionTableColumns.AnswerType] = Questionnarie.AnswerTypes[AnswerType.TBN];
                questionnaireDataSet.Tables[TableNames.QuestionTable].AcceptChanges();
            }
            Rows = null;

        }

        #endregion

        #endregion

        #region"--Internal--"

        #region"-- Variable / Properties --"

        /// <summary>
        /// Key is actual file name with path and value is temp file name with path
        /// </summary>
        internal Dictionary<string, string> XmlFiles;

        #endregion

        #region "-- New / Dispose --"

        internal ElXmlProcessor(List<string> xmlFiles, string tempFolder)
        {
            string TempFileNameWPath = string.Empty;

            //create temp file name and save it into XMLFilesCollection
            this.XmlFiles = new Dictionary<string, string>();

            foreach (string XmlFile in xmlFiles)
            {
                // Check whether the file is valid or not
                if (ElXmlImporter.IsValidXML(XmlFile))
                {
                    //add xml file and tempfile into xml file collection
                    TempFileNameWPath = tempFolder + "\\" + Path.GetFileName(XmlFile);
                    this.XmlFiles.Add(XmlFile, TempFileNameWPath);
                }
            }
        }

        #endregion

        #region"-- Methods --"

        internal void ProcessXmlFiles()
        {
            foreach (string SourceXmlFile in this.XmlFiles.Keys)
            {
                //Step 1: Copy to temp location
                Utility.CopyFile(SourceXmlFile, this.XmlFiles[SourceXmlFile]);

                // Step 2: Calculate the values of Calculate Type questions and replace the data value
                this.ProcessCalculateTypeQuestions(this.XmlFiles[SourceXmlFile]);
            }
        }

        #endregion

        #endregion
    }
}
