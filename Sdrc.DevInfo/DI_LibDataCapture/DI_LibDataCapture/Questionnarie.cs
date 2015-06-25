//' **********************************************************************

//' Program Name: FrmQuestions.vb

//' Developed By: DG6

//' Creation date: 

//' Program Comments: 

//' **********************************************************************

//' **********************Change history*********************************

//' No.   Mod:Date    Mod:By  Change Description 

//' c1    2007-09-11  DG6     :Add new question type- "Date" with the option to define the format of the date and validation by date ranges
//' c2    2008-04-25  DG6     :RTE: if there is any quotes in Option string while importing questions
//' c3    2008-05-01  DG6     :Handling for FontSize, New requirement (PSD>> SS070)   
//' c4    2008-05-05  DG6     :New requirement (PSD>>SS071), Allow jumping on checkbox    
//' c5    2008-05-06  DG6     :New requirement (PSD>>SS072), Dont show "Name of the Interviewer" question 
//' **********************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

using DevInfo.Lib.DI_LibDataCapture.Questions;
using DevInfo.Lib.DI_LibDataCapture.Questions.ColumnNames;

namespace DevInfo.Lib.DI_LibDataCapture
{
    /// <summary>
    /// Raises event when the user reaches at the end of survey 
    /// </summary>
    /// <param name="message"></param>
    [Serializable()]
    public delegate void EndOfSurveyDelegate(string message);

    /// <summary>
    /// Helps in opening and saving quesitonnaire file and also provides method for getting questions/pages.
    /// </summary>
    [Serializable()]
    public class Questionnarie
    {
        #region "-- Static --"

        #region "-- Variables --"

        private static SortedList<AnswerType, string> _AnswerTypes = new SortedList<AnswerType, string>();
        /// <summary>
        /// Gets the answertype in key and value pair. Key : AnswerType and Value: string
        /// </summary>       
        public static SortedList<AnswerType, string> AnswerTypes
        {
            get
            {
                return Questionnarie._AnswerTypes;
            }
        }

        /// <summary>
        /// Returns the answer type
        /// </summary>
        /// <param name="AnsType"></param>
        /// <returns></returns>
        public static AnswerType GetAnswerType(string AnsType)
        {
            AnswerType RetVal;
            int Index = -1;
            try
            {
                Index = Questionnarie.AnswerTypes.IndexOfValue(AnsType);
                RetVal = Questionnarie.AnswerTypes.Keys[Index];
            }
            catch (Exception)
            {
                RetVal = AnswerType.TB;
            }
            return RetVal;
        }

        #endregion

        #region "-- New/Dispose --"

        static Questionnarie()
        {
            //add answer types in AnswerTypes
            Questionnarie._AnswerTypes.Add(AnswerType.CB, "CB");
            Questionnarie._AnswerTypes.Add(AnswerType.CH, "CH");
            Questionnarie._AnswerTypes.Add(AnswerType.GridType, "Grid Type");
            Questionnarie._AnswerTypes.Add(AnswerType.RB, "RB");
            Questionnarie._AnswerTypes.Add(AnswerType.SCB, "SCB");
            Questionnarie._AnswerTypes.Add(AnswerType.SRB, "SRB");
            Questionnarie._AnswerTypes.Add(AnswerType.TB, "TB");
            Questionnarie._AnswerTypes.Add(AnswerType.TBN, "TBN");
            Questionnarie._AnswerTypes.Add(AnswerType.Calculate, "CALCULATE");
            Questionnarie._AnswerTypes.Add(AnswerType.Aggregate, "AGGREGATE");

            #region "-- Change For c1 --"
            Questionnarie._AnswerTypes.Add(AnswerType.DateType, "Date Type");
            #endregion
        }

        #endregion

        #region "-- Method --"

        /// <summary>
        /// Returns the Question based on key
        /// </summary>
        /// <param name="xmlDataSet"></param>
        /// <param name="key">TODO</param>
        /// <returns></returns>
        public static Question GetQuestion(DataSet xmlDataSet, string key)
        {
            Question RetVal = null;
            DataRow[] Rows;

            try
            {
                Rows = xmlDataSet.Tables[TableNames.QuestionTable].Select(QuestionTableColumns.QuestionKey + "='" + key + "'");
                if (Rows.Length > 0)
                {
                    RetVal = new Question(key, Rows[0], xmlDataSet);
                }
            }
            catch (Exception)
            {

                //throw;
                RetVal = null;
            }
            return RetVal;
        }

        /// <summary>
        /// Returns the question no.
        /// </summary>
        /// <param name="QuestionKey">Question key like Q1,Q2,Q3,... etc</param>
        /// <returns>integer</returns>
        public static int GetQuestionNo(string QuestionKey)
        {
            int RetVal = -1;
            try
            {
                if (!string.IsNullOrEmpty(QuestionKey))
                {
                    QuestionKey = QuestionKey.Replace(Constants.QuestionPrefix, "");

                    RetVal = Convert.ToInt32(QuestionKey);
                }
            }
            catch (Exception)
            {
                RetVal = -1;
                //throw;
            }

            return RetVal;
        }

        internal static string[] SplitStringNIncludeEmpyValue(string valueString, string delimiter)
        {
            string[] RetVal;

            //replace delimiter
            valueString = valueString.Replace(delimiter, "\n");
            RetVal = valueString.Split("\n".ToCharArray());

            return RetVal;
        }


        internal static string[] SplitString(string valueString, string delimiter)
        {
            string[] RetVal;
            //string[] Arr = new string[1];       //To get splitted values
            //Arr[0] = delimiter;

            //RetVal = value.Split(Arr,StringSplitOptions.RemoveEmptyEntries);
            int Index = 0;
            string Value;
            List<string> SplittedList = new List<string>();
            while (true)
            {
                Index = valueString.IndexOf(delimiter);
                if (Index == -1)
                {
                    if (!string.IsNullOrEmpty(valueString))
                    {
                        SplittedList.Add(valueString);
                    }
                    break;
                }
                else
                {
                    Value = valueString.Substring(0, Index);
                    valueString = valueString.Substring(Index + delimiter.Length);
                    SplittedList.Add(Value);
                }

            }

            RetVal = SplittedList.ToArray();

            return RetVal;
        }

        /// <summary>
        /// To generate preview of the questionnaire.
        /// </summary>
        /// <param name="filename">Questionnaire file name with path</param>
        /// <param name="outputFolder">Output folder path</param>
        /// <param name="xmlStyleSheetFileNameWPath">Style sheet file name with path</param>
        /// <returns></returns>
        public static string PreviewQuestionnaire(string filename, string outputFolder, string xmlStyleSheetFileNameWPath)
        {
            string RetVal = string.Empty;
            string PreviewFilename = string.Empty;
            const string QNO = "Qno";
            const string KEY = "key";
            const string PREVIEW = "preview";
            DataColumn DataColName;

            try
            {
                if (!outputFolder.EndsWith("\\"))
                {
                    outputFolder = outputFolder + "\\";
                }

                //copy xml stylesheet file into output folder
                System.IO.File.Copy(xmlStyleSheetFileNameWPath, outputFolder + Path.GetFileName(xmlStyleSheetFileNameWPath), true);

                //create a dataset and read the Questionnaire file
                DataSet DataSetQuestionnaire = new DataSet();
                DataSetQuestionnaire.ReadXml(filename);

                //Create and Add the Column QNO
                DataColName = new DataColumn();
                DataColName.DataType = Type.GetType("System.Int32");
                DataColName.ColumnName = QNO;
                DataSetQuestionnaire.Tables[0].Columns.Add(DataColName);

                //Adding the Column
                foreach (DataRow row in DataSetQuestionnaire.Tables[0].Rows)
                {
                    if (row[KEY].ToString().StartsWith("Q"))
                    {
                        row[QNO] = row[KEY].ToString().Replace("Q", "");

                    }
                    else
                    {
                        row[QNO] = "0";
                    }
                }

                DataSetQuestionnaire.Tables[0].DefaultView.Sort = QNO;
                DataSetQuestionnaire.Tables[0].AcceptChanges();
                DataTable Table = DataSetQuestionnaire.Tables[0];
                Table = DataSetQuestionnaire.Tables[0].DefaultView.ToTable();
                Table.AcceptChanges();
                PreviewFilename = PREVIEW + Path.GetFileName(filename);

                DataSetQuestionnaire.Tables[0].Clear();
                DataSetQuestionnaire.Tables[0].Merge(Table);


                //Write the new xml to the output folder
                DataSetQuestionnaire.WriteXml(outputFolder + PreviewFilename, XmlWriteMode.WriteSchema);


                //Create a FileStream of the previewFile
                FileStream fs = new FileStream(outputFolder + PreviewFilename.Replace(".xml", ".htm"), FileMode.Create, FileAccess.Write);

                //access mode of writing
                StreamWriter s = new StreamWriter(fs);
                //creating a new StreamWriter and passing the filestream object fs as argument
                s.BaseStream.Seek(0, SeekOrigin.End);
                //the seek method is used to move the cursor to next position to avoid text to be
                //overwritten
                s.WriteLine("<html><head><script>");
                s.WriteLine("var XMLDoc,XSLDoc");
                s.WriteLine("function load(){XMLDoc = new ActiveXObject('Microsoft.XMLDOM');XMLDoc.async=true;XMLDoc.onreadystatechange=function(){if(XMLDoc.readyState==4){ loadXSL() }};XMLDoc.load('" + PreviewFilename + "')}");
                s.WriteLine("function loadXSL(){XSLDoc = new ActiveXObject('Microsoft.XMLDOM');XSLDoc.async=true;XSLDoc.onreadystatechange=function(){if(XMLDoc.readyState==4){ SetPrview() }};XSLDoc.load('Question.xsl')}");
                s.WriteLine("function SetPrview(){if(XSLDoc.readyState==4){document.write(XMLDoc.transformNode(XSLDoc))}}");
                s.WriteLine("</script></head><body onload='load()'></body></html>");
                //writing text to the newly created file
                s.Close();
                fs.Close();
                fs.Dispose();
                DataSetQuestionnaire.Dispose();

                RetVal = outputFolder + PreviewFilename.Replace(".xml", ".htm");
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        #endregion

        #endregion

        #region "-- Private --"

        #region "-- Variables --"

        /// <summary>
        /// Returns the Deafault Table count in Quetionnaire Schema file, this is for the back ward compatibility
        /// </summary>
        private const int DefaultTableCount = 3;

        #endregion

        #region "-- Methods --"

        #region "-- Raise Event --"

        private void RaiseEndOfSurveyEvent()
        {
            string Message = string.Empty;
            DataRow[] Rows;

            if (this.EndOfSuveryEvent != null)
            {
                Rows = this._QuestionnaireDataSet.Tables[TableNames.InterfaceTable].Select(InterfaceColumns.InterfaceKey + "='" + KeyForMultiLanguage.EndOfSurvey + "'");
                if (Rows.Length > 0)
                {
                    Message = Rows[0][InterfaceColumns.Translate].ToString();
                }
                this.EndOfSuveryEvent(Message);
            }

        }

        #endregion

        /// <summary>
        /// Its checked the paging and update the TotalQuestion and TotalPages accordingly
        /// </summary>
        private void CheckPagingNUpdateTotal()
        {
            IsPagingAvailable();    //check for paging.
            if (this._Paging) // get available pages
            {
                this._TotalQuestions = this.TotalAvailableQuestions(); ;  //count available questions value
                this._TotalPages = this.TotalAvailablePages();  // count available nos of page 
            }
            else
            {
                this._TotalPages = -1;  //reset available pages value 
                this._TotalQuestions = this.TotalAvailableQuestions();  // count total nos of questions 
            }
            this._CurrentPageNo = 0;
            this._CurrentQuestionNo = 0;
        }

        private DataTable GetNewOptionSubgroupMappingTable()
        {
            DataTable RetVal = new DataTable(TableNames.OptionSubgroupMappingTable);

            RetVal.Columns.Add(OptionSubgroupMappingTableColumns.QuestionKey);
            RetVal.Columns.Add(OptionSubgroupMappingTableColumns.StringId);
            RetVal.Columns.Add(OptionSubgroupMappingTableColumns.SubgroupValGId);

            return RetVal;
        }

        private void ProcessOptionSubgroupMappingTable(DataTable table)
        {
            string StringIDs = string.Empty;
            string SubgroupValGId = string.Empty;
            string QuestionKey = string.Empty;
            DataRow NewRow = null;

            // 1. get questions where string id column is not null or empty
            try
            {
                foreach (DataRow Row in this._QuestionnaireDataSet.Tables[TableNames.QuestionTable].Select(QuestionTableColumns.StringID + " <> '' and " + QuestionTableColumns.StringID + " is not null"))
                {
                    QuestionKey = Convert.ToString(Row[QuestionTableColumns.QuestionKey]);
                    StringIDs = Convert.ToString(Row[QuestionTableColumns.StringID]);

                    if (Convert.ToString(Row[QuestionTableColumns.AnswerType]) != AnswerTypes[AnswerType.DateType])
                    {
                        if (QuestionKey.StartsWith(Constants.QuestionPrefix))
                        {
                            // insert record into optionsubgroupmapping table and for each string id
                            foreach (string StringID in StringIDs.Split(Constants.StringIDSeparator.ToCharArray()))
                            {
                                if (!string.IsNullOrEmpty(StringID))
                                {
                                    // get record from string table by string id
                                    foreach (DataRow MappedStringRow in this._QuestionnaireDataSet.Tables[TableNames.StringTable].Select(StringTableColumns.StringId + "='" + StringID + "'"))
                                    {
                                        SubgroupValGId = Convert.ToString(MappedStringRow[StringTableColumns.SubgroupValGId]);

                                        // insert record into optionSubgroupMapping table 
                                        NewRow = table.NewRow();
                                        NewRow[OptionSubgroupMappingTableColumns.QuestionKey] = QuestionKey;
                                        NewRow[OptionSubgroupMappingTableColumns.StringId] = StringID;
                                        NewRow[OptionSubgroupMappingTableColumns.SubgroupValGId] = SubgroupValGId;
                                        table.Rows.Add(NewRow);
                                        table.AcceptChanges();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        /// <summary>
        /// It updates the Questionnaire Schema
        /// </summary>
        private void UpdateQuestionnaireSchema()
        {
            DataSet QuestionDataSet = new DataSet();
            DataColumn NewColumn;
            try
            {
                // check "OptionSubgroupMapping" table exists or not

                if (!this._QuestionnaireDataSet.Tables.Contains(TableNames.OptionSubgroupMappingTable))
                {
                    // if not then create it
                    this._QuestionnaireDataSet.Tables.Add(this.GetNewOptionSubgroupMappingTable());

                    // insert record into table 
                    this.ProcessOptionSubgroupMappingTable(this._QuestionnaireDataSet.Tables[TableNames.OptionSubgroupMappingTable]);
                }



                QuestionDataSet.ReadXmlSchema("PDA_XML_SCHEMA_FILE");
                foreach (DataTable Tables in QuestionDataSet.Tables)
                {
                    if (Tables.TableName.StartsWith(TableNames.TempGridTypeTable) == false)
                    {
                        if (this._QuestionnaireDataSet.Tables.Contains(Tables.TableName) == false)
                        {
                            this._QuestionnaireDataSet.Tables.Add(Tables.Clone());
                            this._QuestionnaireDataSet.AcceptChanges();
                        }
                    }
                    else
                    {
                        foreach (DataColumn col in Tables.Columns)
                        {
                            if (this._QuestionnaireDataSet.Tables[Tables.TableName].Columns.Contains(col.ColumnName) == false)
                            {
                                NewColumn = new DataColumn();
                                NewColumn.ColumnName = col.ColumnName;
                                NewColumn.DataType = col.DataType;
                                NewColumn.DefaultValue = col.DefaultValue;
                                this._QuestionnaireDataSet.Tables[Tables.TableName].Columns.Add(NewColumn);
                                this._QuestionnaireDataSet.Tables[Tables.TableName].AcceptChanges();
                                NewColumn = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// it loads the Questionnaire schema into _QuestionnaireDataSet
        /// </summary>
        /// <param name="xmlFileName"></param>
        /// <returns></returns>
        private bool LoadXML(string xmlFileName)
        {
            bool RetVal = false;
            try
            {
                //load  xml file 
                DataSet TempDataSet = new DataSet("Questionnaire");
                TempDataSet.ReadXml(xmlFileName);

                //check xml file  is in valid format or not
                if (IsValidQuestionnaire(TempDataSet))
                {
                    this._QuestionnaireDataSet = TempDataSet;
                    this.IsPagingAvailable();
                    this.AddJumpBoxquestions();
                    RetVal = true;

                }
            }
            catch (Exception ex)
            {
                RetVal = false;
            }
            return RetVal;
        }

        /// <summary>
        /// It checks for valid Questionnaire
        /// </summary>
        /// <param name="tempDataSet"></param>
        /// <returns></returns>
        private bool IsValidQuestionnaire(DataSet tempDataSet)
        {
            //  use tempDataset instead ds
            bool RetVal = true;

            try
            {
                if (tempDataSet.Tables.Count < Questionnarie.DefaultTableCount)
                {
                    tempDataSet = null;
                    RetVal = false;

                }
                if (tempDataSet.Tables[0].TableName.ToLower() != TableNames.QuestionTable.ToLower())
                {
                    tempDataSet = null;
                    RetVal = false;
                }
                if (tempDataSet.Tables[1].TableName.ToLower() != TableNames.StringTable.ToLower())
                {
                    tempDataSet = null;
                    RetVal = false;
                }
                if (tempDataSet.Tables[2].TableName.ToLower() != TableNames.HeaderTable.ToLower())
                {
                    tempDataSet = null;
                    RetVal = false;
                }
            }
            catch (Exception ex)
            {
                tempDataSet = null;
                RetVal = false;
            }
            return RetVal;
        }

        /// <summary>
        /// Returns the total available questions
        /// </summary>
        /// <returns></returns>
        private int TotalAvailableQuestions()
        {
            //use this._QuestionnaireDataSet  to count total  Available questions
            int RetVal = 0;
            int TotalGridDependentQuestions = 0;
            DataRow[] Rows = this._QuestionnaireDataSet.Tables[TableNames.QuestionTable].Select(QuestionTableColumns.QuestionKey + "='" + Constants.TotalQuestionsKey + "'");

            if (Rows.Length > 0)
            {
                RetVal = Convert.ToInt32(Rows[0][QuestionTableColumns.QuestionText].ToString());
            }

            //RetVal= total questions - Grid dependent questions
            Rows = this._QuestionnaireDataSet.Tables[TableNames.QuestionTable].Select(QuestionTableColumns.AnswerType + "<> '" + Questionnarie.AnswerTypes[AnswerType.GridType] + "' and " + QuestionTableColumns.GridID + "<>-1");
            TotalGridDependentQuestions = Rows.Length;
            RetVal -= TotalGridDependentQuestions;
            ////if (this._QuestionnaireDataSet.Tables[TableNames.QuestionTable].Rows.Count > 0)
            ////{
            ////    RetVal = this._QuestionnaireDataSet.Tables[TableNames.QuestionTable].Rows.Count;
            ////}
            //else
            //{
            //    RetVal = 0;
            //}
            return RetVal;

        }

        /// <summary>
        /// Returns the total available pages
        /// </summary>
        /// <returns></returns>
        private int TotalAvailablePages()
        {
            // use this._QuestionnaireDataSet  to count total Available pages
            int RetVal = 0;
            if (this._QuestionnaireDataSet.Tables[TableNames.PagesTable].Rows.Count > 0)
            {
                RetVal = this._QuestionnaireDataSet.Tables[TableNames.PagesTable].Rows.Count;
            }
            else
            {
                RetVal = 0;
            }
            return RetVal;

        }

        /// <summary>
        /// To get the data value from default value in case of SCB,SRB
        /// </summary>
        /// <param name="stringId"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private string GetDataValuefromStringId(string stringId, string defaultValue)
        {
            string RetVal = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(defaultValue))
                {
                    // -- if there is any default value.
                    string[] StringId = SplitString(stringId, Constants.StringIDSeparator);
                    for (int i = 0; i < StringId.Length; i++)
                    {
                        if (StringId[i] == defaultValue)
                        {
                            // -- for the default value
                            RetVal += "1,";
                        }
                        else
                        {
                            RetVal += "0,";
                        }
                    }
                }
                else
                {
                    RetVal = string.Empty;
                }
            }
            catch (Exception)
            {
                RetVal = string.Empty;
            }
            return RetVal;
        }

        #region "-- Questions --"

        /// <summary>
        /// Returns the Previous QuestionKey
        /// </summary>
        /// <returns></returns>
        private string GetPreviousQuestionKey()
        {
            string RetVal = string.Empty;
            //AnswerType AnsType;
            //bool IsVisible = false;
            //string GridID = String.Empty;
            int PreviousQuestionNo;

            if (this.CurrentQuestionNo > 1)
            {
                PreviousQuestionNo = this.CurrentQuestionNo - 1;

                // // get value for next question
                //NextQuestion=this.GetQuestion("Q" + RetVal);
                //AnsType = NextQuestion.AnswerType ;
                //IsVisible =NextQuestion.Visible;
                //GridID = NextQuestion.GridId;

                //if (AnsType == AnswerType.Calculate | AnsType == AnswerType.Aggregate | IsVisible == false |
                // (AnsType != AnswerType.GridType & (GridID.Length != 0 & GridID != "-1")))
                //{

                PreviousQuestionNo = this.findPrevNormalQuestion(PreviousQuestionNo);
                this._CurrentQuestionNo = PreviousQuestionNo;
                RetVal = GetQuestionKey(PreviousQuestionNo.ToString());

                //}

            }
            else
            {
                RetVal = GetQuestionKey(this.CurrentQuestionNo.ToString());
            }

            return RetVal;
        }

        /// <summary>
        /// Returns the previous normal question
        /// </summary>
        /// <param name="prevQuestionNo"></param>
        /// <returns></returns>
        private int findPrevNormalQuestion(int prevQuestionNo)
        {
            int RetVal = prevQuestionNo;
            AnswerType AnsType;
            bool IsVisible = false;
            string GridID = String.Empty;

            Question PreviousQuestion;

            while (true)
            {
                if (RetVal < 1)
                {
                    RetVal = this.CurrentQuestionNo;
                    break;
                }

                // get value for previous question
                PreviousQuestion = this.GetQuestion(GetQuestionKey(RetVal.ToString()));
                AnsType = PreviousQuestion.AnswerType;
                IsVisible = PreviousQuestion.Visible;
                GridID = PreviousQuestion.GridId;


                if (AnsType == AnswerType.Calculate | AnsType == AnswerType.Aggregate | IsVisible == false |
                (AnsType != AnswerType.GridType & (GridID.Length != 0 & GridID != "-1")))
                {
                    RetVal -= 1;
                }
                else
                {
                    break;
                    //   return prevQuestionNo;
                }
            }
            return RetVal;
        }

        /// <summary>
        /// Returns the next question key
        /// </summary>
        /// <returns></returns>
        private string GetNextQuestionKey()
        {
            string RetVal = string.Empty;
            //AnswerType AnsType ;
            //bool IsVisible = false;
            //string GridID = String.Empty;
            int NextQuestionNo;

            if (this.CurrentQuestionNo < this.TotalQuestions)
            {
                NextQuestionNo = this.CurrentQuestionNo + 1;

                // // get value for next question
                //NextQuestion=this.GetQuestion("Q" + RetVal);
                //AnsType = NextQuestion.AnswerType ;
                //IsVisible =NextQuestion.Visible;
                //GridID = NextQuestion.GridId;

                //if (AnsType == AnswerType.Calculate | AnsType == AnswerType.Aggregate | IsVisible == false |
                // (AnsType != AnswerType.GridType & (GridID.Length != 0 & GridID != "-1")))
                //{

                NextQuestionNo = this.FindNextNormalQuestionNo(NextQuestionNo);
                this._CurrentQuestionNo = NextQuestionNo;
                RetVal = GetQuestionKey(NextQuestionNo.ToString());
            }
            else
            {
                RetVal = GetQuestionKey(this.CurrentQuestionNo.ToString());
            }

            return RetVal;
        }

        /// <summary>
        /// Search & return the next normal question
        /// </summary>
        /// <param name="nextQuestionNo"></param>
        /// <returns></returns>
        private int FindNextNormalQuestionNo(int nextQuestionNo)
        {
            int RetVal = nextQuestionNo;
            AnswerType AnsType;
            bool IsVisible = false;
            string GridID = String.Empty;
            Question NextQuestion;

            while (true)
            {
                if (RetVal > this.TotalQuestions)
                {
                    RetVal = this.CurrentQuestionNo;
                    break;
                }

                // get value for next question
                NextQuestion = this.GetQuestion(GetQuestionKey(RetVal.ToString()));
                AnsType = NextQuestion.AnswerType;
                IsVisible = NextQuestion.Visible;
                GridID = NextQuestion.GridId;


                if (AnsType == AnswerType.Calculate | AnsType == AnswerType.Aggregate | IsVisible == false |
                (AnsType != AnswerType.GridType & (GridID.Length != 0 & GridID != "-1")))
                {
                    RetVal += 1;
                }
                else
                {
                    break;
                    //   return nextQuestionNo;
                }
            }
            return RetVal;
        }

        #endregion


        #region "-- Paging --"


        /// <summary>
        /// Returns the Questions for next & previos page.
        /// </summary>
        /// <param name="forNextPage"></param>
        /// <returns></returns>
        private Dictionary<string, Question> GetPage(bool forNextPage)
        {
            // Return Collecion of questions
            Dictionary<string, Question> RetVal = new Dictionary<string, Question>();

            int NewPageNo = -1;
            string NewQuestionKey = string.Empty;

            try
            {
                if (forNextPage)    // if paging available
                {
                    //   get next page no
                    NewPageNo = this.GetNextPageNo();
                }
                else
                {
                    // get previous page no
                    NewPageNo = this.GetPreviousPageNo();
                }

                // get Questions
                RetVal = this.GetQuestionsForPage(NewPageNo);

                //Reset currentpage no
                this._CurrentPageNo = NewPageNo;

            }
            catch (Exception)
            {
                RetVal = null;
            }
            return RetVal;
        }

        /// <summary>
        ///  Return the next page number
        /// </summary>
        /// <returns></returns>
        private int GetNextPageNo()
        {
            int RetVal = -1;

            if (this.CurrentPageNo < 1)
            {
                RetVal = 1;
            }
            else
            {
                if (this.CurrentPageNo < this.TotalPages)
                {
                    RetVal = this.CurrentPageNo + 1;
                }
                else
                {
                    RetVal = this.CurrentPageNo;
                }
            }

            return RetVal;
        }

        /// <summary>
        /// Returns the previous page number
        /// </summary>
        /// <returns></returns>
        private int GetPreviousPageNo()
        {
            int RetVal = -1;

            if (this.CurrentPageNo > 1)
            {
                RetVal = this.CurrentPageNo - 1;
            }
            else
            {
                RetVal = this.CurrentPageNo;
            }


            return RetVal;
        }

        /// <summary>
        /// Returns the question dataset
        /// </summary>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        private Dictionary<string, Question> GetQuestionsForPage(int pageNo)
        {
            Dictionary<string, Question> RetVal = new Dictionary<string, Question>();
            string[] QuestionKeys;

            //Get questions key for page
            QuestionKeys = this.GetQuestionsKeyForPage(pageNo).ToString().Split(',');

            foreach (string QuestionKey in QuestionKeys)
            {
                //string AnsType = Questionnarie.GetAnswerType(QuestionKey);
                DataRow[] Rows;
                Rows = QuestionnaireDataSet.Tables[TableNames.QuestionTable].Select(QuestionTableColumns.QuestionKey + "= '" + QuestionKey + "'");
                string Type = Rows[0][QuestionTableColumns.AnswerType].ToString();
                AnswerType AnsType = Questionnarie.GetAnswerType(Type);
                if (AnsType == AnswerType.Aggregate | AnsType == AnswerType.Calculate)
                {
                    //Do nothing
                }
                else
                {
                    RetVal.Add(QuestionKey, this.GetQuestion(QuestionKey));
                }
            }

            return RetVal;
        }

        /// <summary>
        /// Returns the question key
        /// </summary>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        private string GetQuestionsKeyForPage(int pageNo)
        {
            string RetVal = string.Empty;
            DataRow[] Rows;

            try
            {
                Rows = this._QuestionnaireDataSet.Tables[TableNames.PagesTable].Select(PagesTableColumns.PageNumber + "='" + pageNo + "'");
                if (Rows.Length > 0)
                {
                    RetVal = Rows[0][PagesTableColumns.Questions].ToString();
                }
            }
            catch (Exception)
            {
                RetVal = string.Empty;
                //MsgBox(ex.Message)
            }
            return RetVal;
        }

        #endregion


        #region "-- Jump Box --"

        /// <summary>
        /// fill the jumb box on the basis of paging
        /// </summary>
        private void AddJumpBoxquestions()
        {
            try
            {
                this._JumpBoxDataSource = new JumpBoxQuesitonCollection();

                // add mandatory question

                #region -- change no 5 --

                //add interviewer question only if it exists
                if (this.IsInterviewerQuestionExists == true)
                {
                    this._JumpBoxDataSource.Add(new JumpBoxQuestion(MandatoryQuestionsKey.INTERVIEWER, this.GetTranslatedValue(MandatoryQuestionsKey.INTERVIEWER)));
                }
                #endregion

                this._JumpBoxDataSource.Add(new JumpBoxQuestion(MandatoryQuestionsKey.Area, this.GetTranslatedValue(KeyForMultiLanguage.AreaName)));

                if (this.DisplayGPsQuestion)
                {
                    this._JumpBoxDataSource.Add(new JumpBoxQuestion(MandatoryQuestionsKey.GPS, this.GetTranslatedValue(KeyForMultiLanguage.GPS)));
                }

                this._JumpBoxDataSource.Add(new JumpBoxQuestion(MandatoryQuestionsKey.TimePeriod, this.GetTranslatedValue(KeyForMultiLanguage.TimePeriod)));
                this._JumpBoxDataSource.Add(new JumpBoxQuestion(MandatoryQuestionsKey.SorucePublisher, this.GetTranslatedValue(KeyForMultiLanguage.Source)));


                // add other questions or pages
                if (this.Paging)
                {
                    this.AddPagesInJumpBoxDataSource(this._JumpBoxDataSource);
                }
                else
                {
                    this.AddQuestionsINJumpBoxDataSource(this._JumpBoxDataSource);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// fill the jump box, if paging is available
        /// </summary>
        private void AddPagesInJumpBoxDataSource(JumpBoxQuesitonCollection questionList)
        {

            foreach (DataRow Row in this._QuestionnaireDataSet.Tables[TableNames.PagesTable].Rows)
            {
                if (Convert.ToInt32(Row[PagesTableColumns.PageNumber]) > 0)
                {
                    questionList.Add(new JumpBoxQuestion(Row[PagesTableColumns.PageNumber].ToString(), Constants.PageText + Row[PagesTableColumns.PageNumber].ToString()));
                }
            }
        }



        /// <summary>
        /// fill the jump box, if paging is not available
        /// </summary>
        private void AddQuestionsINJumpBoxDataSource(JumpBoxQuesitonCollection questionList)
        {

            DataTable TempDataTable = GetSortedTempQuestionTable();
            string QuestionKey = Constants.QuestionPrefix + "%";
            string DataValue = string.Empty;

            bool Required = false;

            TempDataTable.DefaultView.RowFilter = QuestionTableColumns.QuestionKey + " like '" + QuestionKey + "'";

            foreach (DataRowView Row in TempDataTable.DefaultView)
            {
                //string Type = Questionnarie.GetAnswerType(Row[QuestionTableColumns.AnswerType].ToString()).ToString();
                string Type = Row[QuestionTableColumns.AnswerType].ToString();

                //get data value
                if (Row[QuestionTableColumns.DataValue] != null)
                {
                    DataValue = Row[QuestionTableColumns.DataValue].ToString();
                }
                else
                {
                    DataValue = string.Empty;

                }

                //get value of required field
                Required = Convert.ToBoolean(Row[QuestionTableColumns.Required]);



                AnswerType AnsType = Questionnarie.GetAnswerType(Type);

                if (AnsType == AnswerType.Aggregate | AnsType == AnswerType.Calculate | string.IsNullOrEmpty(Row[QuestionTableColumns.QuestionText].ToString().Trim()))
                {
                    // Do Nothing

                }
                else
                {
                    if (Convert.ToBoolean(Row[QuestionTableColumns.Visible]))
                    {
                        // if question is mandatory and datavalue is empty 
                        if (this._AutoTrackMandatoryQuestions & string.IsNullOrEmpty(DataValue) & Required)
                        {
                            questionList.Add(new JumpBoxQuestion(Row[QuestionTableColumns.QuestionKey].ToString(), Constants.MarkForMandatoryQuestion + Row[QuestionTableColumns.QuestionNo].ToString()));
                        }
                        else
                        {
                            questionList.Add(new JumpBoxQuestion(Row[QuestionTableColumns.QuestionKey].ToString(), Row[QuestionTableColumns.QuestionNo].ToString()));
                        }
                    }
                }
            }
        }

        private DataTable GetSortedTempQuestionTable()
        {
            DataTable TempDataTable = new DataTable();
            string SNOColumn = "SNO";
            DataColumn SNoColumn = new DataColumn(SNOColumn);

            SNoColumn.DataType = System.Type.GetType("System.Int32");
            TempDataTable.Columns.Add(SNoColumn);
            TempDataTable.Merge(this.QuestionnaireDataSet.Tables[TableNames.QuestionTable].Copy());
            try
            {
                foreach (DataRow row in TempDataTable.Select(QuestionTableColumns.QuestionKey + "  like'" + Constants.QuestionPrefix + "%'"))
                {

                    try
                    {
                        row[SNOColumn] = Convert.ToInt32(row[QuestionTableColumns.QuestionKey].ToString().Replace(Constants.QuestionPrefix, ""));
                        row.AcceptChanges();
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            catch (Exception)
            {

            }
            TempDataTable.DefaultView.Sort = "SNO";
            return TempDataTable;
        }

        #endregion


        #region "-- Default Values -"
        private void processDefaultValue()
        {
            DataRow[] Rows;

            // set data value to default value
            try
            {
                Rows = this.QuestionnaireDataSet.Tables[TableNames.QuestionTable].Select("Key like '" + Constants.QuestionPrefix + "%'");
                for (int i = 0; i < Rows.Length; i++)
                {
                    this.SetDataValueToDefaultValue(ref Rows[i]);
                }

                this.SetGridTypeQuestionsDataValueToDefaultValue();
            }
            catch (Exception)
            {
                //MsgBox(ex.Message)
            }
        }

        private void SetDataValueToDefaultValue(ref DataRow row)
        {
            string[] ArrStrId;
            string DefaultValue = string.Empty;
            string DataValue = string.Empty;
            AnswerType AnsType;

            try
            {
                DefaultValue = row[QuestionTableColumns.Default].ToString();
                row[QuestionTableColumns.NumericValue] = DefaultValue;

                AnsType = Questionnarie.GetAnswerType(row[QuestionTableColumns.AnswerType].ToString());

                switch (AnsType)
                {
                    case AnswerType.TB:
                    case AnswerType.TBN:
                        row[QuestionTableColumns.DataValue] = DefaultValue;
                        break;
                    case AnswerType.CB:
                    case AnswerType.RB:
                        //if (DefaultValue.Length > 0 & IsNumeric(defaultValue) AndAlso goDSQuestions.Tables("StringTbl").Select("ID =" & defaultValue).Length > 0 Then
                        if (string.IsNullOrEmpty(DefaultValue) & this.QuestionnaireDataSet.Tables[TableNames.StringTable].Select(StringTableColumns.StringId + "=" + DefaultValue).Length > 0)
                        {
                            row[QuestionTableColumns.DataValue] = this.QuestionnaireDataSet.Tables[TableNames.StringTable].Select(StringTableColumns.StringId + " =" + DefaultValue)[0][StringTableColumns.DisplayString].ToString();
                        }

                        break;
                    case AnswerType.SCB:
                    case AnswerType.SRB:
                    case AnswerType.CH:
                        DataValue = string.Empty;

                        // get Str_ID
                        ArrStrId = row[QuestionTableColumns.StringID].ToString().Split(Constants.StringIDSeparator.ToCharArray()); //Split(dr.Item("Str_ID").ToString, ",")

                        // set data value
                        foreach (string StrID in ArrStrId)
                        {
                            // if strID =  default_value then add 1 otherwise 0
                            if (StrID.Trim() == DefaultValue.Trim())
                            {
                                DataValue += "1,";
                            }
                            else
                            {
                                DataValue += "0,";
                            }
                        }
                        row[QuestionTableColumns.DataValue] = DataValue;

                        break;
                    case AnswerType.GridType:
                        break;
                    case AnswerType.Calculate:
                        break;
                    case AnswerType.Aggregate:
                        break;
                    default:
                        break;
                }

                this.QuestionnaireDataSet.Tables[TableNames.QuestionTable].AcceptChanges();
            }
            catch (Exception)
            {
                //MsgBox(ex.Message)
            }
        }

        private void SetGridTypeQuestionsDataValueToDefaultValue()
        {
            int TotalRows = this.QuestionnaireDataSet.Tables[TableNames.GridTypeTable].Rows.Count;
            string TableName = string.Empty;
            string CellValue = string.Empty;

            try
            {
                for (int i = 1; i <= TotalRows; i++)
                {
                    // update each tempGridTypeTable_
                    TableName = TableNames.TempGridTypeTable + i;

                    if (this.QuestionnaireDataSet.Tables.Contains(TableName))
                    {

                        for (int x = 1; x < this.QuestionnaireDataSet.Tables[TableName].Rows.Count; x++)
                        {
                            for (int y = 1; y < this.QuestionnaireDataSet.Tables[TableName].Rows.Count; y++)
                            {

                                try
                                {
                                    CellValue = this.QuestionnaireDataSet.Tables[TableName].Rows[x][y].ToString();
                                    string[] ValArray = CellValue.Split(Constants.GridCellSeparator.ToCharArray());
                                    ValArray[10] = this.GetDefaultValue(Questionnarie.GetAnswerType(ValArray[6].ToString()), ValArray[13].ToString());
                                    CellValue = string.Join(Constants.GridCellSeparator, ValArray);
                                    this.QuestionnaireDataSet.Tables[TableName].Rows[x][y] = CellValue;
                                    this.QuestionnaireDataSet.Tables[TableName].AcceptChanges();
                                }
                                catch (Exception)
                                {

                                    //throw;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                //   'MsgBox(ex.Message)
            }
        }

        #endregion

        private void UpdateSourceValue()
        {
            try
            {
                if (!string.IsNullOrEmpty(this._SourceQuestion.Publisher.DataValue) & !string.IsNullOrEmpty(this._SourceQuestion.Title.DataValue) & !string.IsNullOrEmpty(this._SourceQuestion.Year.DataValue))
                {
                    //   this._SourceQuestion.Publisher.DataValue = this._SourceQuestion.Publisher.DataValue.ToString() + Constants.SourceSeparator + this._SourceQuestion.Title.DataValue.ToString() + Constants.SourceSeparator + this._SourceQuestion.Year.DataValue.ToString();

                    this._SourceQuestion.SourceValue.DataValue = this._SourceQuestion.Publisher.DataValue.ToString() + Constants.SourceSeparator + this._SourceQuestion.Title.DataValue.ToString() + Constants.SourceSeparator + this._SourceQuestion.Year.DataValue.ToString();
                }
            }
            catch (Exception)
            {
            }
        }

        #region -- JumpNext and JumpPervious--

        #region -- Change no c4

        /// <summary>
        /// Updates the all dependent questions
        /// </summary>
        /// <param name="JumpNextQuestions"></param>
        /// <param name="visible"></param>
        private void UpdateAllDependentQuestions(string jumpNextQuestions, bool visible)
        {
            string[] JumpNextArray;
            if (jumpNextQuestions.Length > 0)
            {
                JumpNextArray = jumpNextQuestions.Split(',');
                foreach (string jumpNext in JumpNextArray)
                {
                    this.BeforeUpdatingJumpQuestions(jumpNext.Substring(jumpNext.IndexOf("_") + 1), visible);
                }
            }
        }

        /// <summary>
        /// Updates the dependent grid type questions
        /// </summary>
        /// <param name="GridDataSet"></param>
        /// <param name="tableId"></param>
        private void updateDependentGridTypeQuestions(ref DataSet gridDataSet, string tableId)
        {
            string TableName = string.Empty;
            //int cellId = 0;
            DataRow[] Rows;

            if (gridDataSet.Tables[0].Select(GridTableColumns.GridId + "= '" + tableId + "'").Length > 0)
            {
                TableName = TableNames.TempGridTypeTable + tableId;
                //-- update values in Question table
                //-- Step 1 : find all questions from question table
                Rows = gridDataSet.Tables[0].Select(GridTableColumns.GridId + "='" + tableId + "'");
                foreach (DataRow Row in Rows)
                {
                    //-- Step 2 : set datavalue to empty                    
                    Row[QuestionTableColumns.DataValue] = string.Empty;

                    //-- Step 3 : set numeric value to null
                    Row[QuestionTableColumns.NumericValue] = string.Empty;
                }
                gridDataSet.Tables[0].AcceptChanges();

                //-- update values in tempGridTypeTable_GridID
                // this.updateDataValuesforDependentGridTypeQuestions(gridDataSet, TableName);
            }
        }

        /// <summary>
        /// Update the data values for dependent grid type questions
        /// </summary>
        /// <param name="gridDataSet"></param>
        /// <param name="tableName"></param>
        private void updateDataValuesforDependentGridTypeQuestions(DataSet gridDataSet, string tableName)
        {
            string[] ValueArray;
            string CellValue;
            for (int i = 1; i <= gridDataSet.Tables[tableName].Rows.Count - 1; i++)
            {
                for (int j = 1; j <= gridDataSet.Tables[tableName].Columns.Count - 1; i++)
                {
                    CellValue = gridDataSet.Tables[tableName].Rows[i][j].ToString();
                    if (CellValue.Length > 0)
                    {
                        //-- split cellvalue and save into array
                        ValueArray = Questionnarie.SplitString(CellValue, Constants.GridSeprator);
                    }
                    //-- update cell's value
                    gridDataSet.Tables[tableName].Rows[i][j] = CellValue;
                }
            }
            gridDataSet.Tables[tableName].AcceptChanges();
        }

        private void UpdateVisibilityForChildQuestions(Question currentQuestion)
        {
            string[] JumpNextArray;

            JumpNextArray = Questionnarie.SplitString(currentQuestion.JumpNext, Constants.Dependentquestion);
            foreach (string JumpNext in JumpNextArray)
            {
                if (JumpNext.StartsWith(currentQuestion.NumericValue + Constants.JumpQuestionSeprator))
                {
                    //Do Nothing                        
                }
                else
                {
                    //-- set visible property to false and clear data value
                    this.BeforeUpdatingJumpQuestions(JumpNext.Substring(JumpNext.IndexOf(Constants.JumpQuestionSeprator) + 1), false);
                }
            }

            foreach (string JumpNext in JumpNextArray)
            {
                if (JumpNext.StartsWith(currentQuestion.NumericValue + Constants.JumpQuestionSeprator))
                {
                    //-- if condition is true then set visible property to true
                    this.BeforeUpdatingJumpQuestions(JumpNext.Substring(JumpNext.IndexOf(Constants.JumpQuestionSeprator) + 1), true);
                    break;
                }
            }
        }

        private void UpdateChildQuestionsForChkBox(Question currentQuestion)
        {
            string[] JumpNextArray = null;
            string[] StringIDArray = null;
            string[] DataValueArray = null;
            string[] VisibleQuestionArray = null;
            string SelectedStringID = string.Empty;
            string VisibleQuestions = string.Empty;
            string AllDependentQuestions = string.Empty;


            Dictionary<string, string> JumpNextList = new Dictionary<string, string>();


            try
            {
                JumpNextArray = Questionnarie.SplitString(currentQuestion.JumpNext, Constants.Dependentquestion);

                //create the list of jumpNextArray
                foreach (string JumpNext in JumpNextArray)
                {
                    JumpNextList.Add(JumpNext.Substring(0, JumpNext.IndexOf(Constants.JumpQuestionSeprator)), JumpNext.Substring(JumpNext.IndexOf(Constants.JumpQuestionSeprator) + 1));

                    if (!string.IsNullOrEmpty(AllDependentQuestions))
                    {
                        AllDependentQuestions += Constants.JumpNextQuestionSeparator;
                    }
                    AllDependentQuestions += JumpNext.Substring(JumpNext.IndexOf(Constants.JumpQuestionSeprator) + 1);
                }

                //create an array for string IDs
                StringIDArray = Questionnarie.SplitString(currentQuestion.StringIDs, Constants.StringIDSeparator);

                DataValueArray = Questionnarie.SplitString(currentQuestion.DataValue, Constants.StringIDSeparator);

                for (int Index = 0; Index < DataValueArray.Length; Index++)
                {
                    if (DataValueArray[Index] == "1")
                    {
                        //get string ID for the selected value
                        SelectedStringID = StringIDArray[Index].ToString();

                        //get dependent question for selected stringID
                        if (JumpNextList.ContainsKey(SelectedStringID))
                        {
                            if (!string.IsNullOrEmpty(VisibleQuestions))
                            {
                                VisibleQuestions += Constants.JumpNextQuestionSeparator;
                            }

                            VisibleQuestions += JumpNextList[SelectedStringID].ToString(); ;
                        }
                    }
                }


                //get visible questions array
                VisibleQuestionArray = Questionnarie.SplitString(VisibleQuestions, Constants.JumpNextQuestionSeparator);

                //-- set visible property to false and clear data value
                foreach (string DependentQuestion in Questionnarie.SplitString(AllDependentQuestions, Constants.JumpNextQuestionSeparator))
                {
                    if (Array.IndexOf(VisibleQuestionArray, DependentQuestion) == -1)
                    {
                        this.BeforeUpdatingJumpQuestions(DependentQuestion, false);
                    }
                }



                //-- set visible property to true
                foreach (string VisibleQuestion in VisibleQuestionArray)
                {
                    this.BeforeUpdatingJumpQuestions(VisibleQuestion, true);
                }

            }
            catch (Exception)
            {

            }

        }



        /// <summary>
        /// Call this method whenever user changes/selects any option 
        /// </summary>
        /// <param name="mappedQuestions"></param>
        /// <param name="Visible"></param>
        private void BeforeUpdatingJumpQuestions(string mappedQuestions, bool visible)
        {
            //int i=0;
            //bool valueChanged=false;
            string DataValue = string.Empty;
            string QuestionsKey = string.Empty;
            DataRow[] Rows;

            try
            {
                //this.Cursor = Cursors.WaitCursor;
                foreach (string QuestionNo in Questionnarie.SplitString(mappedQuestions, Constants.JumpNextSeparator))
                {
                    if (QuestionsKey.Length > 0)
                    {
                        QuestionsKey += ",";
                    }

                    QuestionsKey += "'" + GetQuestionKey(QuestionNo) + "'";
                }

                // UpdateQuestion(goDSQuestions.Tables("Question"))

                if (QuestionsKey.Length > 0)
                {
                    Rows = this._QuestionnaireDataSet.Tables[TableNames.QuestionTable].Select(QuestionTableColumns.QuestionKey + " In (" + QuestionsKey + ")");
                    foreach (DataRow Row in Rows)
                    {
                        if (!visible)
                        {
                            //string Type = Questionnarie.GetAnswerType(Row[QuestionTableColumns.AnswerType].ToString()).ToString();

                            AnswerType AnsType = Questionnarie.GetAnswerType(Row[QuestionTableColumns.AnswerType].ToString()); // Questionnarie.GetAnswerType(Type);

                            switch (AnsType)
                            {
                                case AnswerType.TB:
                                    DataValue = "";
                                    break;
                                case AnswerType.TBN:
                                    DataValue = "";
                                    break;
                                case AnswerType.CB:
                                    DataValue = "";
                                    break;
                                case AnswerType.SCB:
                                    DataValue = DataValue.Replace("1", "0");
                                    break;
                                case AnswerType.RB:
                                    DataValue = "";
                                    break;
                                case AnswerType.SRB:
                                    DataValue = DataValue.Replace("1", "0");
                                    break;
                                case AnswerType.CH:
                                    DataValue = DataValue.Replace("1", "0");
                                    break;
                                case AnswerType.GridType:
                                    updateDependentGridTypeQuestions(ref this._QuestionnaireDataSet, Row[GridTableColumns.GridId].ToString());
                                    break;
                                default:
                                    break;
                            }
                            Row[QuestionTableColumns.DataValue] = DataValue;
                            Row[QuestionTableColumns.NumericValue] = "";
                        }
                        Row[QuestionTableColumns.Visible] = visible;
                        this._QuestionnaireDataSet.Tables[TableNames.QuestionTable].AcceptChanges();
                        if (!visible & Row[QuestionTableColumns.JumpNext].ToString().Trim().Length > 0)
                        {
                            UpdateAllDependentQuestions(Row[QuestionTableColumns.JumpNext].ToString(), false);
                        }
                    }
                }
                ////If TotalQuestionPanel > 1 Then
                ////'-- reset panel's controlz
                ////For i = 0 To TotalQuestionPanel - 1
                ////    questionPanels(i).ResetControls()
                ////Next
                ////'-- reset panels positions
                ////Call ResetQuestionPanelsHeight()
                ////End If

            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #endregion

        #endregion

        #endregion

        #region "-- Public --"

        #region "-- Variables / Properties  --"

        private string _XmlFileName;
        /// <summary>
        /// Gets or Sets Xml file name
        /// </summary>
        public string XmlFileName
        {
            get
            {
                return this._XmlFileName;
            }
            set
            {
                this._XmlFileName = value;
            }
        }

        private int _TotalQuestions = -1;
        /// <summary>
        /// Gets total questions available in question table
        /// </summary>
        public int TotalQuestions
        {
            get
            {
                return this._TotalQuestions;
            }
        }

        private int _TotalPages = -1;
        /// <summary>
        /// Gets total pages available in Questionnaire.
        /// </summary>
        public int TotalPages
        {
            get
            {
                return this._TotalPages;
            }
        }

        private DataSet _QuestionnaireDataSet;
        /// <summary>
        /// Gets Questionnarie dataset
        /// </summary>
        public DataSet QuestionnaireDataSet
        {
            get
            {
                return this._QuestionnaireDataSet;
            }
        }

        private Question _InterviewerQuestion;
        /// <summary>
        /// Gets interviewer question.
        /// </summary>
        public Question InterviewerQuestion
        {
            get
            {
                if (this._InterviewerQuestion == null)
                {
                    this._InterviewerQuestion = this.GetQuestion(MandatoryQuestionsKey.INTERVIEWER);
                }
                return this._InterviewerQuestion;
            }
        }

        private Source _SourceQuestion;
        /// <summary>
        /// Gets source question.
        /// </summary>
        public Source SourceQuestion
        {
            get
            {
                if (this._SourceQuestion == null)
                {
                    this._SourceQuestion = new Source(this._QuestionnaireDataSet, this._SourceXmlFileName);
                }
                return this._SourceQuestion;
            }
        }

        private Area _AreaQuestion;
        /// <summary>
        /// Gets area question.
        /// </summary>
        public Area AreaQuestion
        {
            get
            {
                if (this._AreaQuestion == null)
                {
                    this._AreaQuestion = new Area(this._QuestionnaireDataSet, this._AreaXmlFileName);
                }
                else
                {
                    this._AreaQuestion.GetUpdatedValues(this._QuestionnaireDataSet);
                }
                return this._AreaQuestion;
            }
        }

        private Question _TimeperiodQuestion;
        /// <summary>
        /// Gets timeperiod question.
        /// </summary>
        public Question TimeperiodQuestion
        {
            get
            {
                if (this._TimeperiodQuestion == null)
                {
                    this._TimeperiodQuestion = this.GetQuestion(MandatoryQuestionsKey.TimePeriod);
                }
                return this._TimeperiodQuestion;
            }
        }

        private string _AreaXmlFileName = string.Empty; //System.AppDomain.CurrentDomain.BaseDirectory + @"bin\" + Constants.AreaFileName;

        /// <summary>
        /// Gets or sets the Area XML file name
        /// </summary>
        public string AreaXmlFileName
        {
            get
            {
                return this._AreaXmlFileName;
            }
            set
            {
                this._AreaXmlFileName = value;
            }
        }

        private string _SourceXmlFileName = string.Empty; //System.AppDomain.CurrentDomain.BaseDirectory + @"bin\" + Constants.SourceFileName;

        /// <summary>
        /// Gets or sets the Source XML file name
        /// </summary>
        public string SourceXmlFileName
        {
            get
            {
                return this._SourceXmlFileName;
            }
            set
            {
                this._SourceXmlFileName = value;
            }
        }

        private int _CurrentQuestionNo;
        /// <summary>
        /// Gets current question no.Returns string.empty if paging is not available.
        /// </summary>
        public int CurrentQuestionNo
        {
            get
            {
                return this._CurrentQuestionNo;
            }
        }

        private int _CurrentPageNo;
        /// <summary>
        /// Gets current page no. Returns string.empty if paging is available.
        /// </summary>
        public int CurrentPageNo
        {
            get
            {
                return this._CurrentPageNo;
            }
        }

        private bool _Paging = false;
        /// <summary>
        /// Gets the paging 
        /// </summary>
        public bool Paging
        {
            get
            {
                return this._Paging;
            }
        }

        private bool _AutoTrackMandatoryQuestions = true;
        /// <summary>
        ///  Gets or sets Auto-tracking of mandatory fields True/False (on/off). If true, then questions in jumpbox will come with "*" mark if their data value are missing. Defualt is true.
        /// </summary>
        public bool AutoTrackMandatoryQuestions
        {
            get
            {
                return this._AutoTrackMandatoryQuestions;
            }
            set
            {
                this._AutoTrackMandatoryQuestions = value;
            }
        }

        private JumpBoxQuesitonCollection _JumpBoxDataSource;
        /// <summary>
        /// Gets Jumpbox data source which can be directly bind to datasource property.
        /// </summary>
        public JumpBoxQuesitonCollection JumpBoxDataSource
        {
            get
            {
                //if (this._JumpBoxDataSource == null)
                //{
                this.AddJumpBoxquestions();
                //}
                return this._JumpBoxDataSource;
            }
        }

        private bool _DisplayGPsQuestion = false;
        /// <summary>
        /// Gets  or sets whether GPS option is on or off
        /// </summary>
        public bool DisplayGPsQuestion
        {
            get
            {
                return this._DisplayGPsQuestion;
            }
            set
            {
                this._DisplayGPsQuestion = value;
            }
        }

        private string _PageFooter = string.Empty;
        /// <summary>
        /// Gets the page footer  in (current question no)/(total questions) format like 1/10 .
        /// </summary>
        public string PageFooter
        {
            get
            {
                if (this.Paging)
                {
                    this._PageFooter = this._CurrentPageNo + "/" + this.TotalPages;
                }
                else
                {
                    this._PageFooter = this._CurrentQuestionNo + "/" + this.TotalQuestions;
                }

                return this._PageFooter;
            }
        }

        private string _FontName = string.Empty;
        /// <summary>
        /// Get the Font name
        /// </summary>
        public string QuestionFontName
        {
            get
            {
                return _FontName;
            }
        }

        #region -- change no 3 --

        private string _FontSize = "9";
        /// <summary>
        /// Get the Font size
        /// </summary>
        public string QuestionFontSize
        {
            get
            {
                return this._FontSize;
            }
        }

        #endregion

        private List<string> _QuestionnaireSource;
        /// <summary>
        /// Get the source of questionnaire
        /// </summary>
        public List<string> QuestionnaireSource
        {
            get
            {
                this._QuestionnaireSource = this.GetquestionnaireSource();
                return this._QuestionnaireSource;
            }
        }

        private List<string> _QuestionnaireTimePeriod;
        /// <summary>
        /// Get the time period of questionnaire
        /// </summary>
        public List<string> QuestionnaireTimePeriod
        {
            get
            {
                this._QuestionnaireTimePeriod = this.GetquestionnaireTimePeriod();

                return this._QuestionnaireTimePeriod;
            }
        }

        private Question _StartTime;
        /// <summary>
        /// Gets start time of the survey
        /// </summary>
        public Question StartTime
        {
            get
            {
                if (this._StartTime == null)
                {
                    this._StartTime = this.GetQuestion(MandatoryQuestionsKey.StartTime);
                }
                return this._StartTime;
            }
        }

        private Question _EndTime;
        /// <summary>
        /// Gets end time of the survey
        /// </summary>
        public Question EndTime
        {
            get
            {
                if (this._EndTime == null)
                {
                    this._EndTime = this.GetQuestion(MandatoryQuestionsKey.EndTime);
                }
                return this._EndTime;
            }
        }

        private Nullable<bool> _IsInterviewerQuestionExists = null;
        /// <summary>
        /// Gets true or false. True if Interviewer Question exists otherwise false.
        /// </summary>
        public Nullable<bool> IsInterviewerQuestionExists
        {
            get
            {
                if (this._IsInterviewerQuestionExists == null)
                {
                    if (this.QuestionnaireDataSet.Tables[TableNames.QuestionTable].Select(QuestionTableColumns.QuestionKey + "='" + MandatoryQuestionsKey.INTERVIEWER + "'").Length > 0)
                    {
                        this._IsInterviewerQuestionExists = true;
                    }
                    else
                    {
                        this._IsInterviewerQuestionExists = false;
                    }
                }

                return this._IsInterviewerQuestionExists;
            }
        }



        #endregion

        #region "-- New/Dispose --"

        public Questionnarie()
        {
            //do nothing
        }

        #endregion

        #region "-- Events --"

        /// <summary>
        /// Raises when trying to get the next/last page.
        /// </summary>
        public event EndOfSurveyDelegate EndOfSuveryEvent;

        #endregion

        #region "-- Methods --"

        #region "-- Navigation --"

        /// <summary>
        /// Returns the first valid page or question
        /// </summary>
        /// <returns></returns>
        public void GetFirstPage()
        {
            this._CurrentPageNo = 0;
            this._CurrentQuestionNo = 0;
        }

        /// <summary>
        /// Returns the last valid page or question
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Question> GetLastPage()
        {
            Dictionary<string, Question> RetVal = new Dictionary<string, Question>();
            string NewQuestionKey = string.Empty;

            try
            {
                // reset currentPage no and current question no

                this._CurrentPageNo = this.TotalAvailablePages() + 1;
                this._CurrentQuestionNo = this.TotalAvailableQuestions() + 1;

                RetVal = this.GetPreviousPage();


            }
            catch (Exception)
            {

                RetVal = null;
            }
            return RetVal;
        }

        /// <summary>
        /// Return Collecion of questions
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Question> GetNextPage()
        {
            Dictionary<string, Question> RetVal = new Dictionary<string, Question>();
            string NewQuestionKey = string.Empty;
            int ActivePageNo = 0;
            int ActiveQuestionNo = 0;

            try
            {
                // check paging is on or not
                if (this.TotalPages > 0 & this.Paging)
                {
                    ActivePageNo = this._CurrentPageNo;
                    RetVal = this.GetPage(true);

                    // raise end of survey event 
                    if (ActivePageNo == this._CurrentPageNo)
                    {
                        this.RaiseEndOfSurveyEvent();
                    }

                }
                else if (this.TotalQuestions > 0)  // if not, then get next question
                {
                    ActiveQuestionNo = this._CurrentQuestionNo;
                    NewQuestionKey = this.GetNextQuestionKey();
                    RetVal.Add(NewQuestionKey, this.GetQuestion(NewQuestionKey));

                    //raise end of survey event
                    if (ActiveQuestionNo == this._CurrentQuestionNo)
                    {
                        this.RaiseEndOfSurveyEvent();
                    }

                }
            }
            catch (Exception)
            {

                RetVal = null;
            }
            return RetVal;
        }

        /// <summary>
        /// Return Collecion of questions
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Question> GetPreviousPage()
        {
            Dictionary<string, Question> RetVal = new Dictionary<string, Question>();
            string NewQuestionKey = string.Empty;
            try
            {
                // check paging is on or not
                if (this.TotalPages > 0)
                {
                    RetVal = this.GetPage(false);
                }
                else if (this.TotalQuestions > 0) // if not, then get next question
                {
                    NewQuestionKey = this.GetPreviousQuestionKey();
                    RetVal.Add(NewQuestionKey, this.GetQuestion(NewQuestionKey));
                }
            }
            catch (Exception)
            {

                RetVal = null;
            }
            return RetVal;
        }

        #endregion



        /// <summary>
        /// --Returns the question Key like Q1,Q2,Q3.....
        /// </summary>
        /// <param name="QuestionNo"></param>
        /// <returns></returns>
        static string GetQuestionKey(string questionNo)
        {
            string RetVal = string.Empty;
            RetVal = Constants.QuestionPrefix + questionNo.Trim();
            return RetVal.Trim();
        }

        /// <summary>
        /// --Pagind is not available, if there is no record found in page table
        /// </summary>
        public void IsPagingAvailable()
        {
            if (this._QuestionnaireDataSet.Tables[TableNames.PagesTable].Rows.Count > 0)
            {
                this._Paging = true;
            }
            else
            {
                this._Paging = false;
            }
        }

        /// <summary>
        /// Returns the New question
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Question GetQuestion(string key)
        {
            Question RetVal = null;
            DataRow[] Rows;

            try
            {
                Rows = this._QuestionnaireDataSet.Tables[TableNames.QuestionTable].Select(QuestionTableColumns.QuestionKey + "='" + key + "'");
                if (Rows.Length > 0)
                {
                    RetVal = new Question(key, Rows[0], this.QuestionnaireDataSet);
                }
            }
            catch (Exception)
            {

                //throw;
                RetVal = null;
            }
            return RetVal;
        }

        /// <summary>
        /// Returns  all questions except calculate and aggregate questions
        /// </summary>
        /// <param name="key"> </param>
        /// <returns></returns>
        public Dictionary<string, Question> GetAllQuestions()
        {
            return this.GetAllQuestions(false);
        }

        /// <summary>
        /// Returns all  questions 
        /// </summary>
        /// <param name="IncludeAllQuesitonsExceptGridDependent"></param>
        /// <returns></returns>
        public Dictionary<string, Question> GetAllQuestions(bool IncludeAllQuesitonsExceptGridDependent)
        {
            Dictionary<string, Question> RetVal = new Dictionary<string, Question>();

            const string SNoColumn = "SNo";
            string Key = string.Empty;
            string GridID = string.Empty;
            string SelectString = string.Empty;
            bool IsVisible = false;

            AnswerType AnsType;
            DataColumn TempColumn = new DataColumn(SNoColumn);
            DataRow[] Rows;
            //DataRow Row;
            DataTable TempTable;
            try
            {
                // add SNO column to do sorting
                TempTable = this.QuestionnaireDataSet.Tables[TableNames.QuestionTable].Copy();
                TempColumn.DataType = typeof(System.Int32);
                TempTable.Columns.Add(TempColumn);
                foreach (DataRow Row in TempTable.Select(QuestionTableColumns.QuestionKey + " like '" + Constants.QuestionPrefix + "%'"))
                {
                    Row[SNoColumn] = Convert.ToInt32(Row[QuestionTableColumns.QuestionKey].ToString().Replace(Constants.QuestionPrefix, "").ToString().Trim());
                }

                // sort table 
                TempTable.DefaultView.Sort = SNoColumn;
                TempTable.DefaultView.RowFilter = QuestionTableColumns.QuestionKey + " like '" + Constants.QuestionPrefix + "%'";

                //get records and add in collection
                foreach (DataRowView Row in TempTable.DefaultView)
                {
                    AnsType = Questionnarie.GetAnswerType(Row[QuestionTableColumns.AnswerType].ToString());
                    GridID = Row[QuestionTableColumns.GridID].ToString();
                    IsVisible = Convert.ToBoolean(Row[QuestionTableColumns.Visible]);

                    if (AnsType == AnswerType.Calculate | AnsType == AnswerType.Aggregate | IsVisible == false |
                    (AnsType != AnswerType.GridType & (GridID.Length != 0 & GridID != "-1")))
                    {
                        if (IncludeAllQuesitonsExceptGridDependent)
                        {
                            if (!(AnsType != AnswerType.GridType & (GridID.Length != 0 & GridID != "-1")))
                            {
                                Key = Row[QuestionTableColumns.QuestionKey].ToString();
                                RetVal.Add(Key, this.GetQuestion(Key));
                            }
                        }
                        else
                        {

                            //insert grid dependent questions
                            //if ((AnsType != AnswerType.GridType & (GridID.Length != 0 & GridID != "-1")))
                            //{
                            Key = Row[QuestionTableColumns.QuestionKey].ToString();
                            RetVal.Add(Key, this.GetQuestion(Key));
                            //}
                        }

                    }
                    else
                    {
                        Key = Row[QuestionTableColumns.QuestionKey].ToString();
                        RetVal.Add(Key, this.GetQuestion(Key));
                    }
                }
            }
            catch (Exception)
            {

                //throw;
                RetVal = null;
            }

            return RetVal;
        }



        /// <summary>
        /// Returns all normal questions .It returns all questions except grid type, grid dependent ,aggregate and calculate questions.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Question> GetAllNormalQuestions()
        {
            Dictionary<string, Question> RetVal = new Dictionary<string, Question>();

            const string SNoColumn = "SNo";
            string Key = string.Empty;
            string GridID = string.Empty;
            string SelectString = string.Empty;
            bool IsVisible = false;

            AnswerType AnsType;
            DataColumn TempColumn = new DataColumn(SNoColumn);
            DataRow[] Rows;
            //DataRow Row;
            DataTable TempTable;
            try
            {
                // add SNO column to do sorting
                TempTable = this.QuestionnaireDataSet.Tables[TableNames.QuestionTable].Copy();
                TempColumn.DataType = typeof(System.Int32);
                TempTable.Columns.Add(TempColumn);
                foreach (DataRow Row in TempTable.Select(QuestionTableColumns.QuestionKey + " like '" + Constants.QuestionPrefix + "%'"))
                {
                    Row[SNoColumn] = Convert.ToInt32(Row[QuestionTableColumns.QuestionKey].ToString().Replace(Constants.QuestionPrefix, "").ToString().Trim());
                }

                // sort table 
                TempTable.DefaultView.Sort = SNoColumn;
                TempTable.DefaultView.RowFilter = QuestionTableColumns.QuestionKey + " like '" + Constants.QuestionPrefix + "%'";

                //get records and add in collection
                foreach (DataRowView Row in TempTable.DefaultView)
                {
                    AnsType = Questionnarie.GetAnswerType(Row[QuestionTableColumns.AnswerType].ToString());
                    GridID = Row[QuestionTableColumns.GridID].ToString();
                    IsVisible = Convert.ToBoolean(Row[QuestionTableColumns.Visible]);

                    //if (AnsType == AnswerType.Calculate | AnsType == AnswerType.Aggregate | IsVisible == false |
                    //(AnsType != AnswerType.GridType & (GridID.Length != 0 & GridID != "-1")))
                    if (AnsType == AnswerType.Calculate | AnsType == AnswerType.Aggregate |
                     (AnsType != AnswerType.GridType & (GridID.Length != 0 & GridID != "-1")))
                    {
                        // do nothing
                    }
                    else
                    {
                        Key = Row[QuestionTableColumns.QuestionKey].ToString();
                        RetVal.Add(Key, this.GetQuestion(Key));
                    }
                }
            }
            catch (Exception)
            {

                //throw;
                RetVal = null;
            }

            return RetVal;
        }

        /// <summary>
        /// Open the Questionnaire, check for valid Questionnaire, update its schema and update the paging or questions total
        /// </summary>
        /// <param name="xmlFileName"></param>
        /// <returns></returns>
        public bool OpenQuestionnaire(string xmlFileName)
        {
            DataRow[] Rows;
            bool RetVal = false;
            RetVal = this.LoadXML(xmlFileName);

            if (RetVal)
            {
                this._XmlFileName = xmlFileName;
                this._IsInterviewerQuestionExists = null;

                // clear mandatory questions
                this._InterviewerQuestion = null;
                this._SourceQuestion = null;
                this._AreaQuestion = null;
                this._TimeperiodQuestion = null;


                this.UpdateQuestionnaireSchema();
                this.CheckPagingNUpdateTotal();


                // -- set the font name
                Rows = this.QuestionnaireDataSet.Tables[TableNames.QuestionTable].Select(QuestionTableColumns.QuestionKey + "='" + QuestionTableColumns.Font + "'");
                if (Rows.Length > 0)
                {
                    this._FontName = Rows[0][QuestionTableColumns.QuestionText].ToString();
                }

                #region -- change no 3 --

                //-- set the font size
                Rows = this.QuestionnaireDataSet.Tables[TableNames.QuestionTable].Select(QuestionTableColumns.QuestionKey + "='" + MandatoryQuestionsKey.FONT_SIZE + "'");
                if (Rows.Length > 0)
                {
                    this._FontSize = Rows[0][QuestionTableColumns.QuestionText].ToString();
                }


                #endregion

                // set value of display gps questionnaire by checking visible and required properties of GPS question
                try
                {
                    Rows = this.QuestionnaireDataSet.Tables[TableNames.QuestionTable].Select(QuestionTableColumns.QuestionKey + "='" + MandatoryQuestionsKey.GPS + "'");

                    if (Rows.Length > 0)
                    {
                        if (Convert.ToBoolean(Rows[0][QuestionTableColumns.Visible]) & Convert.ToBoolean(Rows[0][QuestionTableColumns.Required]))
                        {
                            this.DisplayGPsQuestion = true;
                        }
                        else
                        {
                            this.DisplayGPsQuestion = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // do nothing
                }

            }

            return RetVal;
        }



        /// <summary>
        ///  To Save questionnaire file.
        /// </summary>
        /// <param name="filename">Filename with path</param>
        /// <returns>Ture/False.</returns>
        public bool SaveFile(string filename)
        {
            bool RetVal = false;

            try
            {
                this.UpdateSourceValue();

                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
                this.QuestionnaireDataSet.WriteXml(filename, XmlWriteMode.WriteSchema);
                RetVal = true;
            }
            catch (Exception)
            {
                RetVal = false;
                //throw;
            }
            return RetVal;

        }

        /// <summary>
        /// Returns string ID
        /// </summary>
        /// <param name="answer"></param>
        /// <returns></returns>
        public string GetStringID(string value)
        {
            string RetVal = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(value))
                {
                    #region -- change no 2 --

                    RetVal = this.QuestionnaireDataSet.Tables[TableNames.StringTable].Select(StringTableColumns.DisplayString + "='" + Utility.RemoveQuotes(value) + "'")[0][StringTableColumns.StringId].ToString();

                    #endregion
                }
            }
            catch (Exception)
            {
                //		throw;
            }
            return RetVal;
        }

        /// <summary>
        /// Returns string value from string table
        /// </summary>
        /// <param name="string Id"></param>
        /// <returns></returns>
        public string GetStringValueFromStringTable(string stringID)
        {
            string RetVal = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(stringID))
                {
                    RetVal = this.QuestionnaireDataSet.Tables[TableNames.StringTable].Select(StringTableColumns.StringId + "='" + stringID + "'")[0][StringTableColumns.DisplayString].ToString();
                }
            }
            catch (Exception)
            {
                //		throw;
            }
            return RetVal;
        }

        /// <summary>
        /// Returns the default value
        /// </summary>
        /// <param name="ansType"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string GetDefaultValue(AnswerType ansType, string defaultValue)
        {
            string RetVal = string.Empty;
            DataRow[] Rows;
            try
            {
                switch (ansType)
                {
                    case AnswerType.TB:
                    case AnswerType.TBN:
                        RetVal = defaultValue;
                        break;
                    case AnswerType.CB:
                    case AnswerType.SCB:
                    case AnswerType.RB:
                    case AnswerType.SRB:
                    case AnswerType.CH:
                        // get value from string table
                        if (defaultValue.Length > 0)
                        {
                            Rows = this.QuestionnaireDataSet.Tables[TableNames.StringTable].Select(StringTableColumns.StringId + "=" + defaultValue);
                            if (Rows.Length > 0)
                            {
                                RetVal = Rows[0][StringTableColumns.DisplayString].ToString();
                            }
                        }
                        break;
                    case AnswerType.GridType:
                        break;
                    case AnswerType.Calculate:
                        break;
                    case AnswerType.Aggregate:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                RetVal = string.Empty;
                //'MsgBox(ex.Message)

            }

            return RetVal;
        }

        /// <summary>
        /// Returns the translated value from the interface table
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetTranslatedValue(string key)
        {
            // read key from interface table and return value to the calling function
            string RetVal = string.Empty;
            DataRow[] Rows;

            Rows = this.QuestionnaireDataSet.Tables[TableNames.InterfaceTable].Select(InterfaceColumns.InterfaceKey + "='" + key + "'");
            if (Rows.Length > 0)
            {
                RetVal = Rows[0][InterfaceColumns.Translate].ToString();
            }

            return RetVal;
        }

        /// <summary>
        /// Reset question no and page no for mandatory questions.
        /// </summary>
        public void ResetQuestionNPageNoForJumpBox()
        {
            this._CurrentQuestionNo = 0;
            this._CurrentPageNo = 0;
        }

        /// <summary>
        /// Applies the Default Value of each question to its datavalue
        /// </summary>
        public void ApplyDefaultValueToDataValue()
        {
            foreach (DataRow row in this._QuestionnaireDataSet.Tables[TableNames.QuestionTable].Rows)
            {
                AnswerType AnsType = Questionnarie.GetAnswerType(row[QuestionTableColumns.AnswerType].ToString());
                switch (AnsType)
                {
                    case AnswerType.TB:
                        row[QuestionTableColumns.DataValue] = row[QuestionTableColumns.Default];
                        break;
                    case AnswerType.TBN:
                        row[QuestionTableColumns.DataValue] = row[QuestionTableColumns.Default];
                        break;
                    case AnswerType.CB:
                        row[QuestionTableColumns.DataValue] = this.GetStringValueFromStringTable(row[QuestionTableColumns.Default].ToString());
                        break;
                    case AnswerType.SCB:
                        // -- To get the datavalue
                        row[QuestionTableColumns.DataValue] = this.GetDataValuefromStringId(row[QuestionTableColumns.StringID].ToString(), row[QuestionTableColumns.Default].ToString());
                        break;
                    case AnswerType.RB:
                        row[QuestionTableColumns.DataValue] = this.GetStringValueFromStringTable(row[QuestionTableColumns.Default].ToString());
                        break;
                    case AnswerType.SRB:
                        // -- To get the datavalue
                        row[QuestionTableColumns.DataValue] = this.GetDataValuefromStringId(row[QuestionTableColumns.StringID].ToString(), row[QuestionTableColumns.Default].ToString());
                        break;
                    case AnswerType.CH:
                        row[QuestionTableColumns.DataValue] = row[QuestionTableColumns.Default];
                        break;
                    case AnswerType.GridType:
                    //row[QuestionTableColumns.DataValue] = row[QuestionTableColumns.Default];
                    //break;
                    case AnswerType.Calculate:
                    //row[QuestionTableColumns.DataValue] = row[QuestionTableColumns.Default];
                    //break;
                    case AnswerType.Aggregate:
                        //row[QuestionTableColumns.DataValue] = row[QuestionTableColumns.Default];
                        // do nothing for calculate and aggregate questions
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Returns list of all available timeperiods
        /// </summary>
        /// <returns></returns>
        public List<string> GetquestionnaireTimePeriod()
        {
            List<string> RetVal = new List<string>();

            DataRow[] Rows;
            Rows = this.QuestionnaireDataSet.Tables[TableNames.QuestionTable].Select(QuestionTableColumns.QuestionKey + "  like'" + Constants.QuestionPrefix + "%'");
            if (Rows.Length > 0)
            {
                for (int i = 0; i <= Rows.Length - 1; i++)
                {
                    if (!RetVal.Contains(Rows[i][QuestionTableColumns.TimePeriod].ToString()) && !string.IsNullOrEmpty(Rows[i][QuestionTableColumns.TimePeriod].ToString()))
                    {
                        RetVal.Add(Rows[i][QuestionTableColumns.TimePeriod].ToString());
                    }
                }
            }
            if (!string.IsNullOrEmpty(this.TimeperiodQuestion.DataValue))
            {
                RetVal.Add(this.TimeperiodQuestion.DataValue);
            }
            return RetVal;
        }

        /// <summary>
        /// Returns list of all available sources
        /// </summary>
        /// <returns></returns>
        public List<string> GetquestionnaireSource()
        {
            List<string> RetVal = new List<string>();
            DataRow[] Rows;

            Rows = this.QuestionnaireDataSet.Tables[TableNames.QuestionTable].Select(QuestionTableColumns.QuestionKey + "  like'" + Constants.QuestionPrefix + "%'");
            if (Rows.Length > 0)
            {
                for (int i = 0; i <= Rows.Length - 1; i++)
                {
                    if (!string.IsNullOrEmpty(Rows[i][QuestionTableColumns.Source].ToString()))
                    {
                        if (!RetVal.Contains(Rows[i][QuestionTableColumns.Source].ToString()))
                        {
                            RetVal.Add(Rows[i][QuestionTableColumns.Source].ToString());
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(this.SourceQuestion.Publisher.DataValue) && !string.IsNullOrEmpty(this.SourceQuestion.Title.DataValue) && !string.IsNullOrEmpty(this.SourceQuestion.Year.DataValue))
            {
                RetVal.Add(this.SourceQuestion.Publisher.DataValue + "_" + this.SourceQuestion.Title.DataValue + "_" + this.SourceQuestion.Year.DataValue);
            }

            return RetVal;
        }

        /// <summary>
        /// Returns the message string for missing mandatroy questions
        /// </summary>
        /// <returns></returns>
        public string GetMissingMandatoryQuestionsString()
        {
            return this.GetMissingMandatoryQuestionsString(false);
        }

        public string GetMissingMandatoryQuestionsString(bool forPDAApplicaton)
        {
            string RetVal = string.Empty;

            RetVal = this.GetMissingMandatoryQuestionKeys();

            if (RetVal.Trim().Length > 0)
            {
                if (forPDAApplicaton)
                {
                    if (RetVal.Length > 40)
                    {
                        RetVal = RetVal.Substring(0, 40) + "...";
                    }
                }

                RetVal = "Mandatory questions (" + RetVal + ") not responded. Do you still want to save or close ?";
            }

            return RetVal;
        }

        private string GetMissingMandatoryQuestionKeys()
        {
            string RetVal = string.Empty;
            DataTable TempDataTable = GetSortedTempQuestionTable();
            string QuestionKey = Constants.QuestionPrefix + "%";
            string DataValue = string.Empty;

            bool Required = false;



            try
            {
                TempDataTable.DefaultView.RowFilter = QuestionTableColumns.QuestionKey + " like '" + QuestionKey + "'";

                foreach (DataRowView Row in TempDataTable.DefaultView)
                {
                    //string Type = Questionnarie.GetAnswerType(Row[QuestionTableColumns.AnswerType].ToString()).ToString();
                    string Type = Row[QuestionTableColumns.AnswerType].ToString();

                    //get data value
                    if (Row[QuestionTableColumns.DataValue] != null)
                    {
                        DataValue = Row[QuestionTableColumns.DataValue].ToString();
                    }
                    else
                    {
                        DataValue = string.Empty;
                    }

                    //get value of required field
                    Required = Convert.ToBoolean(Row[QuestionTableColumns.Required]);

                    AnswerType AnsType = Questionnarie.GetAnswerType(Type);
                    if (AnsType == AnswerType.Aggregate | AnsType == AnswerType.Calculate | string.IsNullOrEmpty(Row[QuestionTableColumns.QuestionText].ToString().Trim()))
                    {
                        // Do Nothing

                    }
                    else if (AnsType == AnswerType.SCB)
                    {
                            // if question is mandatory and numeric value is empty 
                        if (this._AutoTrackMandatoryQuestions & Required & string.IsNullOrEmpty(Convert.ToString(Row[QuestionTableColumns.NumericValue])))
                            {
                                if (RetVal.Length > 0)
                                {
                                    RetVal += ",";
                                }

                                RetVal += Row[QuestionTableColumns.QuestionNo].ToString();
                            }               
                    }
                    else if (AnsType == AnswerType.CH)
                    {
                        // if question is mandatory and any checkbox is not selected
                        if (this._AutoTrackMandatoryQuestions & Required & Convert.ToString(Row[QuestionTableColumns.NumericValue]).IndexOf("1") == -1)
                        {
                            if (RetVal.Length > 0)
                            {
                                RetVal += ",";
                            }

                            RetVal += Row[QuestionTableColumns.QuestionNo].ToString();
                        }               
                    }
                    else if (AnsType == AnswerType.GridType)
                    {
                        if (this._AutoTrackMandatoryQuestions)
                        {
                            bool IsAnyQuestionAnswered = false;

                            // if any of grid dependent question is answered then it will be considered as answered
                            foreach (Question GridDependentQuestion in this.GetQuestion(Convert.ToString(Row[QuestionTableColumns.QuestionKey])).GridQuestions.Values)
                            {
                                if (GridDependentQuestion.AnswerType == AnswerType.SCB)
                                {
                                    if (!string.IsNullOrEmpty(GridDependentQuestion.NumericValue))
                                    {
                                        IsAnyQuestionAnswered = true;
                                        break;
                                    }
                                }
                                else
                                {
                                    // if question is mandatory and datavalue is empty 
                                    if (!string.IsNullOrEmpty(GridDependentQuestion.DataValue))
                                    {
                                        IsAnyQuestionAnswered = true;
                                        break;
                                    }
                                }

                            }
                            if (IsAnyQuestionAnswered == false)
                            {
                                if (RetVal.Length > 0)
                                {
                                    RetVal += ",";
                                }

                                RetVal += Convert.ToString(Row[QuestionTableColumns.QuestionNo]);
                            }
                        }
                    }
                    else if (Convert.ToInt32(Row[QuestionTableColumns.GridID]) <= 0)
                    {
                        if (Convert.ToBoolean(Row[QuestionTableColumns.Visible]))
                        {
                            // if question is mandatory and datavalue is empty 
                            if (this._AutoTrackMandatoryQuestions & string.IsNullOrEmpty(DataValue) & Required)
                            {
                                if (RetVal.Length > 0)
                                {
                                    RetVal += ",";
                                }

                                RetVal += Row[QuestionTableColumns.QuestionNo].ToString();
                            }
                        }
                    }
                }

            }
            catch (Exception)
            {
            }

            return RetVal;
        }

        #region "-- Jump Box --"

        /// <summary>
        /// Returns the collection of questions.
        /// </summary>
        /// <param name="jumpString">Seleceted value from jump box</param>
        /// <returns>Dictionary<string, Question>, Key and value pair. Key: Question Key, Value:Object of Question type.</returns>
        public Dictionary<string, Question> GetPageForJumpBox(string jumpString)
        {
            Dictionary<string, Question> RetVal = new Dictionary<string, Question>();
            string JumpKey = string.Empty;
            string QuestionKey = string.Empty;
            int PageNo = -1;

            // get Question key or Page no from JumpBoxDataSource
            JumpKey = this.JumpBoxDataSource.GetKey(jumpString);

            switch (JumpKey)
            {
                case MandatoryQuestionsKey.Area:
                case MandatoryQuestionsKey.INTERVIEWER:
                case MandatoryQuestionsKey.SorucePublisher:
                case MandatoryQuestionsKey.SourceTitle:
                case MandatoryQuestionsKey.SourceYear:
                case MandatoryQuestionsKey.TimePeriod:
                    this.ResetQuestionNPageNoForJumpBox();
                    break;
                default:

                    //check paging is available
                    if (this.Paging)     //if paging available, then return questions collection
                    {
                        // get quesitons
                        PageNo = Convert.ToInt32(JumpKey);
                        RetVal = this.GetQuestionsForPage(PageNo);

                        //reset current page no
                        this._CurrentPageNo = PageNo;
                    }
                    else                // if paging not available, then return selected question
                    {
                        QuestionKey = JumpKey;
                        if (Questionnarie.GetQuestionNo(QuestionKey) > 0)
                        {

                            // get question
                            RetVal.Add(QuestionKey, this.GetQuestion(QuestionKey));

                            //reset current question no
                            this._CurrentQuestionNo = Questionnarie.GetQuestionNo(QuestionKey);
                        }
                    }
                    break;
            }

            return RetVal;
        }

        #endregion


        #region "-- JumpNext N JumpPrevious --"

        /// <summary>
        /// assumption: 
        /// 1. before calling this function update currentQuestion object : like get datavalue form control N update question.
        /// </summary>
        /// <param name="currentQuestion"></param>
        public void updateChildQuestions(Question currentQuestion)
        {
            string JumpNextString = string.Empty;


            //getDataValueFromControls
            //-- if  jump condition is true then hide & clear value of all mapped questions

            JumpNextString = currentQuestion.JumpNext;
            if (JumpNextString.Length > 0)
            {
                if (currentQuestion.AnswerType == AnswerType.CH)
                {
                    this.UpdateChildQuestionsForChkBox(currentQuestion);
                }
                else
                {
                    this.UpdateVisibilityForChildQuestions(currentQuestion);
                }
            }
        }

        #endregion

        #endregion

        #endregion
    }

}
