using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Runtime.Serialization.Formatters.Binary;

using DevInfo.Lib.DI_LibDataCapture;
using DevInfo.Lib.DI_LibDataCapture.Questions.ColumnNames;
using DevInfo.Lib.DI_LibDataCapture.Questions;
using System.Collections;

namespace DevInfo.Lib.DI_LibDataCapture.Questions
{
    [Serializable()]
    public class Question
    {
        #region "-- Private --"

        #region "-- Variables --"

        public DataTable TempGridTypeTable;
        private DataSet XmlDataSet;

        #endregion

        #region "-- New / Dispose --"

        private Question()
        {
            // do not implement this.
        }

        #endregion

        #region "-- Methods --"

        private void CreateOptions(DataSet xmlDataSet)
        {
            string OptionText = string.Empty;
            string SubgroupGId = string.Empty;
            DataRow[] Rows;

            this._Options = new List<AnswerTypeOption>();

            if (this._AnswerType == AnswerType.CB | this._AnswerType == AnswerType.CH |
this._AnswerType == AnswerType.RB | this._AnswerType == AnswerType.SCB |
this._AnswerType == AnswerType.SRB)
            {
                // add options 
                if (!string.IsNullOrEmpty(this.StringIDs))
                {
                    foreach (string StringID in this.StringIDs.Split(Constants.StringIDSeparator.ToCharArray()))
                    {
                        Rows = xmlDataSet.Tables[TableNames.StringTable].Select(ColumnNames.StringTableColumns.StringId + "=" + StringID + "");
                        if (Rows.Length > 0)
                        {
                            OptionText = Rows[0][ColumnNames.StringTableColumns.DisplayString].ToString();

                            //SubgroupGId = Rows[0][ColumnNames.StringTableColumns.SubgroupValGId].ToString();

                            SubgroupGId = this.GetMappedSubgroupGID(xmlDataSet, this._Key, StringID)
;
                            this._Options.Add(new AnswerTypeOption(OptionText, StringID, StringID, SubgroupGId));
                        }

                    }
                }
            }
        }

        private string GetMappedSubgroupGID(DataSet xmlDataSet, string questionKey, string stringID)
        {
            string RetVal = string.Empty;

            if (this._GridId != "-1")
            {
                questionKey = "GRID" + this._GridId + "_" + this.No.Replace(Constants.QuestionPrefix, "");
            }

            foreach (DataRow Row in xmlDataSet.Tables[TableNames.OptionSubgroupMappingTable].Select(OptionSubgroupMappingTableColumns.QuestionKey + "='" + questionKey + "' and " + OptionSubgroupMappingTableColumns.StringId + "='" + stringID + "'"))
            {
                RetVal = Convert.ToString(Row[OptionSubgroupMappingTableColumns.SubgroupValGId]);
            }

            return RetVal;
        }

        private void SaveCellValueInTempGridTypeTable()
        {

            int CellID = 0;
            string[] CellValueArray;
            string CellValue = String.Empty;
            string TableName = string.Empty;
            int CellIndex = 0;


            try
            {
                if (this._GridId != null & this._GridId != "-1" & this._AnswerType != AnswerType.GridType)
                {
                    CellID = Convert.ToInt32(this._No.ToString().Replace(Constants.QuestionPrefix, ""));

                    TableName = TableNames.TempGridTypeTable + this.GridId;


                    //get tempgrid type table from xmldataset
                    if (this.XmlDataSet.Tables.Contains(TableName))
                    {
                        // add datavalue
                        CellIndex = 1;
                        for (int i = 1; i < this.XmlDataSet.Tables[TableName].Rows.Count; i++)
                        {
                            for (int j = 1; j < this.XmlDataSet.Tables[TableName].Columns.Count; j++)
                            {
                                if (CellIndex == CellID)
                                {
                                    // update datavalue in tempGridTypeTable
                                    CellValueArray = Questionnarie.SplitStringNIncludeEmpyValue(this.XmlDataSet.Tables[TableName].Rows[i][j].ToString(), Constants.GridCellSeparator);

                                    // get datavalue from grid table
                                    CellValueArray[10] = this._DataValue;

                                    // reset cell value in tempGridType table
                                    CellValue = string.Join(Constants.GridCellSeparator, CellValueArray);
                                    this.XmlDataSet.Tables[TableName].Rows[i][j] = CellValue;
                                    this.XmlDataSet.Tables[TableName].AcceptChanges();
                                }
                                // increment cell index
                                CellIndex += 1;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex.InnerException);
            }
        }

        private void SaveDataValueInDataSet()
        {
            DataTable QuestionTable;
            DataRow[] Rows;
            try
            {
                QuestionTable = this.XmlDataSet.Tables[TableNames.QuestionTable];

                Rows = QuestionTable.Select(QuestionTableColumns.QuestionKey + "= '" + this._Key + "'");
                if (Rows.Length > 0)
                {
                    Rows[0][QuestionTableColumns.DataValue] = this._DataValue.ToString();


                    this.SaveCellValueInTempGridTypeTable();
                }
                QuestionTable.AcceptChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex.InnerException);
            }
        }

        private void SaveNumericValueInDataSet()
        {
            DataTable QuestionTable;
            DataRow[] Rows;
            try
            {
                QuestionTable = this.XmlDataSet.Tables[TableNames.QuestionTable];

                Rows = QuestionTable.Select(QuestionTableColumns.QuestionKey + "= '" + this._Key + "'");
                if (Rows.Length > 0)
                {
                    Rows[0][QuestionTableColumns.NumericValue] = this._NumericValue;
                }

                QuestionTable.AcceptChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex.InnerException);
            }
        }

        private void SaveSourceInDataSet()
        {
            DataTable QuestionTable;
            DataRow[] Rows;
            try
            {
                QuestionTable = this.XmlDataSet.Tables[TableNames.QuestionTable];

                Rows = QuestionTable.Select(QuestionTableColumns.QuestionKey + "= '" + this._Key + "'");
                if (Rows.Length > 0)
                {
                    Rows[0][QuestionTableColumns.Source] = this._Source;
                }

                QuestionTable.AcceptChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex.InnerException);
            }
        }

        private void SaveTimeperiodInDataSet()
        {
            DataTable QuestionTable;
            DataRow[] Rows;
            try
            {
                QuestionTable = this.XmlDataSet.Tables[TableNames.QuestionTable];

                Rows = QuestionTable.Select(QuestionTableColumns.QuestionKey + "= '" + this._Key + "'");
                if (Rows.Length > 0)
                {
                    Rows[0][QuestionTableColumns.TimePeriod] = this._TimePeriod;
                }

                QuestionTable.AcceptChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex.InnerException);
            }
        }

        private void SaveGridValuesInDataSet()
        {
            DataTable QuestionTable;

            try
            {
                QuestionTable = this.XmlDataSet.Tables[TableNames.QuestionTable];

                //-- update tempGridTypeTable in dataset for grid type questions
                if (this.AnswerType == AnswerType.GridType)
                {
                    this.UpdateTempGridTypeTableInDataset(this.XmlDataSet);
                }
                QuestionTable.AcceptChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex.InnerException);
            }
        }

        #region "-- Save other values --"

        #region "-- JumpNext N JumpPrevious --"

        private void SaveJumpNextInDataSet(string oldJumpString, string newJumpString)
        {
            DataTable QuestionTable;
            DataRow[] Rows;
            Hashtable PreMappedValues = new Hashtable();
            Hashtable NewMappedValues = new Hashtable();

            try
            {
                QuestionTable = this.XmlDataSet.Tables[TableNames.QuestionTable];

                Rows = QuestionTable.Select(QuestionTableColumns.QuestionKey + "= '" + this._Key + "'");
                if (Rows.Length > 0)
                {
                    Rows[0][QuestionTableColumns.JumpNext] = newJumpString;

                    //step 1: get previous mapped values
                    PreMappedValues = this.GetJumpMappedValues(oldJumpString);

                    //step 2: clear  previous mapped values from JumpPrevious columns
                    this.RemovePrevMappedValues(PreMappedValues);

                    //step 3: get new mapped values
                    NewMappedValues = this.GetJumpMappedValues(newJumpString);

                    //step 4: update jumpPrevious information for new mapped questions
                    this.UpdateNewMappedValues(NewMappedValues);

                }
                QuestionTable.AcceptChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex.InnerException);
            }
        }

        private Hashtable GetJumpMappedValues(string jumpNextValue)
        {
            Hashtable RetVal = new Hashtable();
            if (!string.IsNullOrEmpty(jumpNextValue))
            {
                foreach (string value in Questionnarie.SplitString(jumpNextValue, Constants.JumpNextStringSeparator))
                {
                    RetVal.Add(value.Substring(0, value.IndexOf(Constants.JumpQuestionSeprator)), value.Substring(value.IndexOf(Constants.JumpQuestionSeprator) + 1));
                }
            }
            return RetVal;
        }

        private void RemovePrevMappedValues(Hashtable preMappedValues)
        {
            //-- invoke this method only when user clicks on Ok button
            DataRow dr;
            IDictionaryEnumerator oEnum;
            string sJumpPrev;
            //-- stores jumpPrev 
            string[] sarrJumpPrev;

            //-- stores splitted value of jumprev 
            //-- clear JumpNext value of the selected row 
            dr = this.XmlDataSet.Tables[TableNames.QuestionTable].Select(QuestionTableColumns.QuestionKey + "='" + this.Key + "'")[0];
            dr[QuestionTableColumns.JumpNext] = string.Empty;
            this.XmlDataSet.Tables[TableNames.QuestionTable].AcceptChanges();

            //-- clear JumpPrevious value of associated Questions
            //-- previous hashtable has STRID as key and Key(Question) as Value
            oEnum = preMappedValues.GetEnumerator();
            while (oEnum.MoveNext())
            {
                string[] valueArray = Questionnarie.SplitString(oEnum.Value.ToString(), Constants.JumpNextQuestionSeparator);
                foreach (string value in valueArray)
                {
                    dr = this.XmlDataSet.Tables[TableNames.QuestionTable].Select(QuestionTableColumns.QuestionKey + "='" + Constants.QuestionPrefix + value + "'")[0];
                    sJumpPrev = dr[QuestionTableColumns.JumpPrevious].ToString();

                    if (sJumpPrev.Length > 0)
                    {
                        sarrJumpPrev = Questionnarie.SplitString(sJumpPrev, Constants.JumpNextStringSeparator);
                        if (Array.IndexOf(sarrJumpPrev, this.Key.ToString().Substring(1)) >= 0)
                        {
                            sJumpPrev = "";
                            foreach (string str in sarrJumpPrev)
                            {
                                if (str != this.Key.ToString().Substring(1))
                                {
                                    if (sJumpPrev.Length > 0)
                                    {
                                        sJumpPrev += Constants.JumpNextStringSeparator + str;

                                    }
                                    else
                                    {
                                        sJumpPrev = str;
                                    }
                                }
                            }
                            dr[QuestionTableColumns.JumpPrevious] = sJumpPrev;
                        }
                    }
                    this.XmlDataSet.Tables[TableNames.QuestionTable].AcceptChanges();
                }
            }
        }

        private void UpdateNewMappedValues(Hashtable newMappedValues)
        {
            string sQno = "";
            DataRow dr;

            IDictionaryEnumerator oEnum = newMappedValues.GetEnumerator();

            while (oEnum.MoveNext())
            {
                dr = this.XmlDataSet.Tables[TableNames.QuestionTable].Select(QuestionTableColumns.QuestionKey + "='" + this._Key + "'")[0];

                //-- save jumpNext value 
                if (dr[QuestionTableColumns.JumpNext].ToString().Length == 0)
                {
                    dr[QuestionTableColumns.JumpNext] = oEnum.Key + "_" + oEnum.Value.ToString();
                }
                else
                {
                    dr[QuestionTableColumns.JumpNext] = dr[QuestionTableColumns.JumpNext].ToString() + Constants.JumpNextStringSeparator + oEnum.Key + Constants.JumpQuestionSeprator + oEnum.Value.ToString();
                }


                //-- update jumpPrevious information
                string[] valueArray = Questionnarie.SplitString(oEnum.Value.ToString(), Constants.JumpNextQuestionSeparator);
                foreach (string value in valueArray)
                {
                    DataRow drChild = this.XmlDataSet.Tables[TableNames.QuestionTable].Select(QuestionTableColumns.QuestionKey + "='" + Constants.QuestionPrefix + value + "'")[0];
                    if (drChild[QuestionTableColumns.JumpPrevious].ToString().Length == 0)
                    {
                        drChild[QuestionTableColumns.JumpPrevious] = this._Key.Substring(1);
                    }
                    else
                    {
                        if (Array.IndexOf(Questionnarie.SplitString(drChild[QuestionTableColumns.JumpPrevious].ToString(), Constants.JumpNextStringSeparator), this._Key.Substring(1)) < 0)
                        {
                            drChild[QuestionTableColumns.JumpPrevious] = drChild[QuestionTableColumns.JumpPrevious].ToString() + Constants.JumpNextStringSeparator + this._Key.Substring(1);
                        }
                    }
                    this.XmlDataSet.Tables[TableNames.QuestionTable].AcceptChanges();
                }
            }
            //-- ***End of Change no: c1
        }
        #endregion

        #region "-- Indicator, unit and subgroup values --"

        private void SaveIndicatorGIdInDataSet()
        {
            DataTable QuestionTable;
            DataRow[] Rows;
            try
            {
                QuestionTable = this.XmlDataSet.Tables[TableNames.QuestionTable];

                Rows = QuestionTable.Select(QuestionTableColumns.QuestionKey + "= '" + this._Key + "'");
                if (Rows.Length > 0)
                {
                    Rows[0][QuestionTableColumns.IndicatorGID] = this._IndicatorGId.ToString();

                }
                QuestionTable.AcceptChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex.InnerException);
            }
        }

        private void SaveUnitGIdInDataSet()
        {
            DataTable QuestionTable;
            DataRow[] Rows;
            try
            {
                QuestionTable = this.XmlDataSet.Tables[TableNames.QuestionTable];

                Rows = QuestionTable.Select(QuestionTableColumns.QuestionKey + "= '" + this._Key + "'");
                if (Rows.Length > 0)
                {
                    Rows[0][QuestionTableColumns.UnitGid] = this._UnitGId.ToString();

                }
                QuestionTable.AcceptChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex.InnerException);
            }
        }

        private void SaveSubgroupInDataSet()
        {
            DataTable QuestionTable;
            DataRow[] Rows;
            try
            {
                QuestionTable = this.XmlDataSet.Tables[TableNames.QuestionTable];

                Rows = QuestionTable.Select(QuestionTableColumns.QuestionKey + "= '" + this._Key + "'");
                if (Rows.Length > 0)
                {
                    Rows[0][QuestionTableColumns.SubGroupGid] = this._SubgroupGId.ToString();

                }
                QuestionTable.AcceptChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex.InnerException);
            }
        }

        #endregion

        #endregion


        /// <summary>
        /// Get header text of questions
        /// </summary>
        private void GetHeaderText()
        {
            DataRow[] Rows;
            if (!string.IsNullOrEmpty(this._HeaderID))
            {
                Rows = this.XmlDataSet.Tables[TableNames.HeaderTable].Select(HeaderTableColumns.HeaderId + "='" + this._HeaderID + "'");
                if (Rows.Length > 0)
                {
                    this._HeaderTxt = Rows[0][HeaderTableColumns.Header].ToString();
                }
                else
                {
                    this._HeaderTxt = string.Empty;
                }
            }
            else
            {
                this._HeaderTxt = string.Empty;
            }
        }

        private void UpdateTempGridTypeTableInDataset(DataSet xmlDataSet)
        {
            int i = 0;
            int j = 0;
            string[] CellValueArray;
            string CellValue = String.Empty;

            try
            {
                for (i = 0; i < this._GridTable.Rows.Count; i++)
                {
                    for (j = 0; j < this._GridTable.Columns.Count; j++)
                    {
                        try
                        {
                            // update datavalue in tempGridTypeTable
                            CellValueArray = Questionnarie.SplitStringNIncludeEmpyValue(this.TempGridTypeTable.Rows[i][j].ToString(), Constants.GridCellSeparator);
                            //CellValueArray = this.TempGridTypeTable.Rows[i][j].ToString().Split(Constants.GridCellSeparator.ToCharArray());

                            // get datavalue from grid table
                            CellValueArray[10] = this._GridTable.Rows[i][j].ToString();

                            // reset cell value in tempGridType table
                            CellValue = string.Join(Constants.GridCellSeparator, CellValueArray);
                            this.TempGridTypeTable.Rows[i][j] = CellValue;

                        }
                        catch (Exception)
                        {

                            //throw;
                        }
                    }
                }


                //-- replace datatable in dataSet
                xmlDataSet.Tables.Remove(this.TempGridTypeTable.TableName);
                xmlDataSet.Tables.Add(this.TempGridTypeTable);
                xmlDataSet.AcceptChanges();
            }
            catch (Exception)
            {
                //       MsgBox(ex.Message)
            }
        }

        #region "-- Grid Type Question --"

        private void ReprocessGridTypeTable(DataSet xmlDataSet)
        {
            int RowIndex = 1;
            int ColIndex = 1;
            int QuestionIndex = 1;
            DataRow[] Rows;
            try
            {
                foreach (Question GridQuestion in this._GridQuestions.Values)
                {
                    if (GridQuestion.AnswerType == AnswerType.SCB)
                    {
                        if (!string.IsNullOrEmpty(GridQuestion.NumericValue))
                        {
                            Rows = xmlDataSet.Tables[TableNames.StringTable].Select(ColumnNames.StringTableColumns.StringId + "=" + GridQuestion.NumericValue + "");
                            if (Rows.Length > 0)
                            {
                                this._GridTable.Rows[RowIndex][ColIndex] = Rows[0][ColumnNames.StringTableColumns.DisplayString].ToString();
                            }
                        }

                    }

                    ColIndex++;
                    if (this._GridTable.Columns.Count == ColIndex)
                    {
                        ColIndex = 1;
                        RowIndex++;
                    }
                }
                this.GridTable.AcceptChanges();
            }
            catch (Exception)
            {
                //           MsgBox(ex.Message)
            }
        }

        private void ProcessGridTypeTable(DataSet xmlDataSet)
        {
            int i = 0;
            int j = 0;
            DataRow RowHeader;
            DataColumn GridColumn;
            string[] CellValue;

            try
            {
                // get tempGridTypeTable from dataset
                this.TempGridTypeTable = this.GetTempGridTypeTable(this.TempGridTypeTable, xmlDataSet);

                // fill  TempGridTypeTable 
                this._GridTable = new DataTable();

                // add columns 
                for (j = 0; j < this.TempGridTypeTable.Columns.Count; j++)
                {
                    GridColumn = new DataColumn();
                    GridColumn.DefaultValue = "";
                    this._GridTable.Columns.Add(GridColumn);
                    this._GridTable.AcceptChanges();
                    GridColumn = null;
                }

                // add rows header
                for (i = 0; i < this.TempGridTypeTable.Rows.Count; i++)
                {
                    RowHeader = this._GridTable.NewRow();
                    if (i > 0)
                    {
                        RowHeader[0] = this.TempGridTypeTable.Rows[i][0];
                    }
                    else
                    {
                        RowHeader[0] = "";
                    }
                    this._GridTable.Rows.Add(RowHeader);
                    this._GridTable.AcceptChanges();
                    RowHeader = null;
                }

                // add columns header
                for (j = 1; j < this.TempGridTypeTable.Columns.Count; j++)
                {

                    this.GridTable.Rows[0][j] = this.TempGridTypeTable.Rows[0][j];
                }

                // add datavalue
                for (i = 1; i < this.TempGridTypeTable.Rows.Count; i++)
                {
                    for (j = 1; j < this.TempGridTypeTable.Columns.Count; j++)
                    {
                        CellValue = Questionnarie.SplitString(this.TempGridTypeTable.Rows[i][j].ToString(), Constants.GridCellSeparator);
                        //CellValue = this.TempGridTypeTable.Rows[i][j].ToString().Split(Constants.GridCellSeparator.ToCharArray());
                        this.GridTable.Rows[i][j] = CellValue[10].ToString();  //datavalue

                    }
                }

                this.GridTable.AcceptChanges();
            }
            catch (Exception)
            {
                //           MsgBox(ex.Message)
            }
        }

        //private string[] SplitString(string value, string delimiter)
        //{
        //    string[] RetVal;
        //    string[] Arr = new string[1];       //To get splitted values
        //    Arr[0] = delimiter;

        //    RetVal = value.Split(Arr, StringSplitOptions.None);
        //    return RetVal;
        //}

        private DataTable GetTempGridTypeTable(DataTable tempGridTypeTable, DataSet xmlDataSet)
        {
            string TableName = String.Empty;
            DataTable RetVal = null;

            try
            {
                tempGridTypeTable = new DataTable();

                // check grid id exists or not
                if (this._GridId != "-1")
                {
                    TableName = TableNames.TempGridTypeTable + this._GridId;
                    tempGridTypeTable.TableName = TableName;

                    // check temp_GridTypeTable_1
                    if (xmlDataSet.Tables.Contains(TableName))
                    {
                        // get temp_GridTypeTable_1 from dataset
                        tempGridTypeTable = xmlDataSet.Tables[TableName];
                    }
                }
                RetVal = tempGridTypeTable;
            }
            catch (Exception)
            {
                RetVal = tempGridTypeTable;
                //   MsgBox(ex.Message)
            }

            return RetVal;
        }


        private Dictionary<string, Question> GetGridQuestions(string gridID, DataSet xmlDataSet)
        {
            Dictionary<string, Question> RetVal = new Dictionary<string, Question>();
            int ArraySize = 0;
            int i = 0;
            string QuestionKey = string.Empty;
            string SelectString = string.Empty;


            DataRow[] Rows;

            try
            {
                //GridTable = CType(Me.DataSource, DataTable)

                // set questionPanelArray's size
                ArraySize = ((this._GridTable.Rows.Count - 1) * (this._GridTable.Columns.Count - 1)) - 1;

                //ReDim questionPanelArray(ArraySize)

                // add all questions into questionPanel
                for (i = 0; i <= ArraySize; i++)
                {
                    // get each question's question key 
                    SelectString = string.Empty;
                    SelectString = QuestionTableColumns.QuestionNo + "='" + Constants.QuestionPrefix + (i + 1) + "'  ";
                    SelectString = SelectString + " and " + QuestionTableColumns.GridID + "='" + gridID + "' ";
                    SelectString = SelectString + " and " + QuestionTableColumns.AnswerType + "<>'Grid Type'";

                    Rows = xmlDataSet.Tables[TableNames.QuestionTable].Select(SelectString);
                    if (Rows.Length > 0)
                    {
                        QuestionKey = Rows[0][QuestionTableColumns.QuestionKey].ToString();
                        RetVal.Add(QuestionKey, Questionnarie.GetQuestion(xmlDataSet, QuestionKey));
                    }
                }

            }
            catch (Exception)
            {
                //            MessageBox.Show(ex.Message)
                RetVal = null;
            }
            return RetVal;
        }

        #endregion

        #endregion

        #endregion

        #region "-- Public --"

        #region "-- Variable / Properties --"

        private string _Key = string.Empty;
        /// <summary>
        /// Gets the key.
        /// </summary>
        public string Key
        {
            get
            {
                return this._Key;
            }
        }

        private string _No = string.Empty;
        /// <summary>
        /// Gets the No
        /// </summary>
        public string No
        {
            get
            {
                return this._No;
            }
        }

        private string _HeaderID = string.Empty;
        /// <summary>
        /// Gets the Header
        /// </summary>
        public string HeaderID
        {
            get
            {
                return this._HeaderID;
            }
        }

        private string _Text = string.Empty;
        /// <summary>
        /// Gets the Text
        /// </summary>
        public string Text
        {
            get
            {
                return this._Text;
            }
        }

        private string _Minimum = string.Empty;
        /// <summary>
        /// Gets the Minimum
        /// </summary>
        public string Minimum
        {
            get
            {
                return this._Minimum;
            }
        }

        private string _Maximum = string.Empty;
        /// <summary>
        /// Get the Maximum
        /// </summary>
        public string Maximum
        {
            get
            {
                return this._Maximum;
            }
        }

        private string _StringIDs = string.Empty;
        /// <summary>
        /// Get the StrIDs
        /// </summary>
        public string StringIDs
        {
            get
            {
                return this._StringIDs;
            }
        }

        private string _DefaultValue = string.Empty;
        /// <summary>
        /// Get the DefaultValue
        /// </summary>
        public string DefaultValue
        {
            get
            {
                return this._DefaultValue;
            }
        }

        private AnswerType _AnswerType = AnswerType.TB;
        /// <summary>
        /// Get the answerType
        /// </summary>
        public AnswerType AnswerType
        {
            get
            {
                return this._AnswerType;
            }
        }

        private string _JumpNext = string.Empty;
        /// <summary>
        /// Gets or sets the JumpNext string
        /// </summary>
        public string JumpNext
        {
            get
            {
                return this._JumpNext;
            }

            set
            {
                string OldJumpNext = this._JumpNext;
                string NewJumpNext = value;

                this.SaveJumpNextInDataSet(OldJumpNext, NewJumpNext);
                this._JumpNext = value;
            }
        }

        private string _JumpPervious = string.Empty;
        /// <summary>
        /// Get the JumpPrevious
        /// </summary>
        public string JumpPervious
        {
            get
            {
                return this._JumpPervious;
            }
        }

        private Boolean _Required = false;
        /// <summary>
        /// Get the Required
        /// </summary>
        public Boolean Required
        {
            get
            {
                return this._Required;
            }
        }

        private string _GridId = "-1";
        /// <summary>
        /// Get the GridId
        /// </summary>
        public string GridId
        {
            get
            {
                return this._GridId;
            }
        }


        private DataTable _GridTable;
        /// <summary>
        /// Gets or Sets Grid table
        /// </summary>
        public DataTable GridTable
        {
            get
            {
                return _GridTable;
            }
            set
            {
                _GridTable = value;
                this.SaveGridValuesInDataSet();
            }
        }


        private string _DataValue = string.Empty;
        /// <summary>
        /// Gets or sets the dataValue
        /// </summary>
        public string DataValue
        {
            get
            {
                return this._DataValue;
            }
            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }
                this._DataValue = value;
                this.SaveDataValueInDataSet();

            }
        }


        private string _NumericValue = string.Empty;
        /// <summary>
        /// gets or sets the Numeric value
        /// </summary>
        public string NumericValue
        {
            get
            {
                return this._NumericValue;
            }
            set
            {
                this._NumericValue = value;
                this.SaveNumericValueInDataSet();
            }
        }

        public string _Source = string.Empty;
        /// <summary>
        /// Gets or sets the source
        /// </summary>
        public string Source
        {
            get
            {
                return this._Source;
            }
            set
            {
                this._Source = value;
                this.SaveSourceInDataSet();
            }
        }

        private string _TimePeriod = string.Empty;
        /// <summary>
        /// Gets or sets the TimePeriod
        /// </summary>
        public string TimePeriod
        {
            get
            {
                return this._TimePeriod;
            }
            set
            {
                this._TimePeriod = value;
                this.SaveTimeperiodInDataSet();
            }
        }

        private Boolean _Visible = true;
        /// <summary>
        /// Get or sets the Visible
        /// </summary>
        public Boolean Visible
        {
            get
            {
                return _Visible;
            }
            set
            {
                _Visible = value;
            }
        }

        private string _HeaderTxt = string.Empty;
        /// <summary>
        /// Returns the Header Text
        /// </summary>
        public string HeaderTxt
        {
            get
            {
                return _HeaderTxt;
            }
            set
            {
                this._HeaderTxt = value;
            }
        }

        private int _QuestionSerialNo;
        /// <summary>
        /// Gets or Sets Question serial no
        /// </summary>
        public int QuestionSerialNo
        {
            get
            {
                return this._QuestionSerialNo;
            }
            set
            {
                this._QuestionSerialNo = value;
            }
        }


        private List<AnswerTypeOption> _Options;
        /// <summary>
        /// Gets options value.
        /// </summary>
        public List<AnswerTypeOption> Options
        {
            get
            {
                return this._Options;
                //return this._Options[1].Text;
                //this.Options[0].Text               
            }
        }

        //public Dictionary<string,Question> GridQuestions
        private Dictionary<string, Question> _GridQuestions = new Dictionary<string, Question>();
        /// <summary>
        /// Gets grid questions collection.
        /// Key: QuestionKey.
        /// Value: Ouestion. 
        /// </summary>
        public Dictionary<string, Question> GridQuestions
        {
            get
            {
                return this._GridQuestions;
            }
        }

        private string _IndicatorGId;
        /// <summary>
        /// Gets or Sets the IndicatorGId
        /// </summary>
        public string IndicatorGId
        {
            get
            {
                return this._IndicatorGId;
            }
            set
            {
                this._IndicatorGId = value;

                // save indicator GID
                this.SaveIndicatorGIdInDataSet();
            }
        }

        private string _UnitGId;
        /// <summary>
        /// Gets or Sets the Unit GId
        /// </summary>
        public string UnitGId
        {
            get
            {
                return this._UnitGId;
            }
            set
            {
                this._UnitGId = value;

                // save unit GID
                this.SaveUnitGIdInDataSet();
            }
        }

        private string _SubgroupGId;
        /// <summary>
        /// Gets or Sets the SubgroupGId
        /// </summary>
        public string SubgroupGId
        {
            get
            {
                return this._SubgroupGId;
            }
            set
            {
                this._SubgroupGId = value;

                //save Subgroup GID
                this.SaveSubgroupInDataSet();
            }
        }


        #endregion

        #region "-- New/Dispose --"

        internal Question(string QuestionKey, DataRow row, DataSet xmlDataSet)
        {
            this.XmlDataSet = xmlDataSet;

            this._Key = QuestionKey;

            // get answer type
            this._AnswerType = Questionnarie.GetAnswerType(row[QuestionTableColumns.AnswerType].ToString());

            this._No = row[QuestionTableColumns.QuestionNo].ToString();
            this._HeaderID = row[QuestionTableColumns.QuestionHeader].ToString();
            this._Text = row[QuestionTableColumns.QuestionText].ToString();
            this._Minimum = row[QuestionTableColumns.Minimum].ToString();
            this._Maximum = row[QuestionTableColumns.Maximum].ToString();
            this._StringIDs = row[QuestionTableColumns.StringID].ToString();
            this._DefaultValue = row[QuestionTableColumns.Default].ToString();
            this._JumpNext = row[QuestionTableColumns.JumpNext].ToString();
            this._JumpPervious = row[QuestionTableColumns.JumpPrevious].ToString();

            this._IndicatorGId = row[QuestionTableColumns.IndicatorGID].ToString();
            this._UnitGId = row[QuestionTableColumns.UnitGid].ToString();
            this._SubgroupGId = row[QuestionTableColumns.SubGroupGid].ToString();

            // set question serial no only if question key starts with "Q" ( means not for mandatory questions)
            if (this._Key.StartsWith(Constants.QuestionPrefix))
            {
                this._QuestionSerialNo = Convert.ToInt32(row[QuestionTableColumns.QuestionKey].ToString().Replace(Constants.QuestionPrefix, "").ToString().Trim());
            }
            else
            {
                this._QuestionSerialNo = -1;
            }

            if (row[QuestionTableColumns.Required] is DBNull)
            {
                this._Required = false;
            }
            else
            {
                this._Required = (bool)(row[QuestionTableColumns.Required]);
            }

            if (row[QuestionTableColumns.Visible] is DBNull)
            {
                this.Visible = false;
            }
            else
            {
                this.Visible = (bool)row[QuestionTableColumns.Visible];
            }


            this._DataValue = row[QuestionTableColumns.DataValue].ToString();
            this._NumericValue = row[QuestionTableColumns.NumericValue].ToString();
            this._Source = row[QuestionTableColumns.Source].ToString();
            this._TimePeriod = row[QuestionTableColumns.TimePeriod].ToString();

            if (row[QuestionTableColumns.GridID] is DBNull | row[QuestionTableColumns.GridID].ToString() == "-1")
            {
                this._GridId = "-1";
            }
            else
            {

                this._GridId = row[QuestionTableColumns.GridID].ToString();
                if (this.AnswerType == AnswerType.GridType)
                {
                    this.ProcessGridTypeTable(xmlDataSet);
                    this._GridQuestions = GetGridQuestions(this._GridId, xmlDataSet);

                    //update grid table to show text value for SCB question type
                    this.ReprocessGridTypeTable(xmlDataSet);
                }
            }


            this.GetHeaderText();
            this.CreateOptions(xmlDataSet);
        }


        #endregion

        #region "-- Methods --"

        /// <summary>
        /// To reset the Question object
        /// </summary>
        public void ResetQuestionFromDataset()
        {
            DataRow[] Rows;
            try
            {
                // get data value ,numeric value and visible from dataset
                Rows = this.XmlDataSet.Tables[TableNames.QuestionTable].Select(QuestionTableColumns.QuestionKey + "='" + this._Key + "'");
                if (Rows.Length > 0)
                {
                    // update question's value
                    this._DataValue = Rows[0][QuestionTableColumns.DataValue].ToString();
                    this._NumericValue = Rows[0][QuestionTableColumns.NumericValue].ToString();

                    //if value of visible column is null then set it to true.
                    if (Convert.ToBoolean(Rows[0][QuestionTableColumns.Visible].ToString() == null))
                    {
                        this.Visible = true;
                    }
                    else
                    {
                        this.Visible = Convert.ToBoolean(Rows[0][QuestionTableColumns.Visible].ToString());
                    }
                }
            }
            catch (Exception)
            {
                //MsgBox(ex.Message)
            }
        }

        #endregion

        #endregion

    }
}
