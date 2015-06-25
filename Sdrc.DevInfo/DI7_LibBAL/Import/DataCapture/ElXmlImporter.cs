
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibDataCapture;
using DevInfo.Lib.DI_LibDataCapture.Questions;
using DevInfo.Lib.DI_LibDataCapture.Questions.ColumnNames;
using System.Data;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;


namespace DevInfo.Lib.DI_LibBAL.Import.DataCapture
{
    public class ElXmlImporter
    {
        #region"--Private--"

        #region"--Variable--"

        private ElXmlProcessor XmlProcessor;
        private EIQuery EIQueries;
        private string TempFolderPath;
        private string DatabaseFileNameWPath;
        private string DataPrefix = string.Empty;
        private string LanguageCode = string.Empty;
        
        private List<string> AggregateQuestionRecords = new List<string>();
        private List<string> PercentRecords = new List<string>();
        #endregion

        #region "-- Enum --"

        private enum AggregateType
        {
            SUM = 0,
            COUNT = 1,
            AVG = 2,
            PERCENT = 3
        }

        #endregion

        #region"--Method--"

        private void ConnectToDatabase(string fileNameWPath)
        {
            this._DBConnection = new DIConnection(new DIConnectionDetails(DIServerType.MsAccess, string.Empty, string.Empty, fileNameWPath, string.Empty, Constants.DBPassword));
            this.DataPrefix = this._DBConnection.DIDataSetDefault();
            this.LanguageCode = this._DBConnection.DILanguageCodeDefault(this.DataPrefix);
            this._DBQuery = new DIQueries(this.DataPrefix, this.LanguageCode);
            this.EIQueries = new EIQuery(this._DBQuery);
        }

        private void CreateTempTables()
        {
            try
            {
                //--IUS
                this._DBConnection.ExecuteNonQuery(this.EIQueries.CreateTempTableIUS(true));

                this._DBConnection.ExecuteNonQuery(this.EIQueries.CreateTempTableIUS(false));

                //--Area
                this._DBConnection.ExecuteNonQuery(this.EIQueries.CreateTempAreaTable());
                this._DBConnection.ExecuteNonQuery(this.EIQueries.createTempBlankAreaTable());

                //DataTable
                this._DBConnection.ExecuteNonQuery(this.EIQueries.CreateTempDataTable());
                this._DBConnection.ExecuteNonQuery(this.EIQueries.AlterTempDataTable());

            }
            catch (Exception ex)
            {
            }
        }

        private void DropTempTables()
        {
            try
            {
                this._DBConnection.ExecuteNonQuery(this.EIQueries.Droptable(Constants.TableNames.Area));
                this._DBConnection.ExecuteNonQuery(this.EIQueries.Droptable(Constants.TableNames.IUS));
                this._DBConnection.ExecuteNonQuery(this.EIQueries.Droptable(Constants.TableNames.AREAUM));
                this._DBConnection.ExecuteNonQuery(this.EIQueries.Droptable(Constants.TableNames.IUSUM));
                this._DBConnection.ExecuteNonQuery(this.EIQueries.Droptable(Constants.TableNames.DATA));
            }
            catch (Exception)
            {
            }
        }

        private void ImportFile(string filename)
        {
            DataSet XmlSourceDataset = new DataSet();    // DataSet
            DataTable QuestionDataTable;       // DataTable
            DataTable StringTable;      // Datatable 
            string IndicatorGid = string.Empty;            //   To hold Indicator Gid's
            string UnitGid = string.Empty;            //   To hold Unit Gid's
            string SubgroupValGid = string.Empty;           //   To hold Subgroup Gid's
            string TimePeriodNId = string.Empty;  // to hold  TimePeriod Nid
            int SourceNId = 0;          // Integers to hold  Source Nid
            int IUSNId = 0;              // Integers to hold  IUS Nid
            int AreaNId = 0;            // Integers to hold  Area Nid
            string Timeperiod = string.Empty;            //   to hold the Time
            string AreaName = string.Empty;        //   to hold the Area name
            string AreaGid = string.Empty;         //   to hold AreaGid
            string AreaID = string.Empty;          //   to hold AreaID
            string Source = string.Empty;          //   to hold Source
            string DataValue = string.Empty;       // DataValue
            string IUSValue = string.Empty;        // IUSValue i.e the Question 
            string QuestionKey = string.Empty;             // The Key value form XML
            string[] StringIDs = null;        //   Array  to hold the Values selected for Subgroups
            string[] SplittedDataValues = null;       //   Array to Split the Datvalue
            string[] SplittedAreaValues;         // Array   To Hold Area  3 fields are send in Area 
            bool IsMultipleSubgroup = false;  // Boolean Variable Default is False
            string DefaultTimePeriodNId = string.Empty; //to hold  TimePeriod Nid
            int DefaultSourceNId = 0;     //-- Integers to hold  Source Nid
            string RowAnswerType;
            string CheckedValue = "1";
            string UncheckedValue = "0";
            DataTable Table;

            try
            {

                // Checking for Existance of the DataSet 
                if ((XmlSourceDataset != null))
                {
                    XmlSourceDataset.Dispose();
                    XmlSourceDataset = null;
                }

                //  New Dataset
                XmlSourceDataset = new DataSet("DevIno5");

                //  Read xml file
                //  load dataset and datatable
                XmlSourceDataset.ReadXml(filename);

                //  Get the DataTable
                QuestionDataTable = XmlSourceDataset.Tables[0];

                //  Time
                //  Get DataValue of The Time Period
                DefaultTimePeriodNId = "";
                DefaultTimePeriodNId = this.GetTimeperiodNID(QuestionDataTable);

                //  Source
                //  Get DataValue of The Source
                DefaultSourceNId = 0;
                DefaultSourceNId = this.GetSourceNid(QuestionDataTable);

                //  Area
                //  Get DataValue of The Area
                SplittedAreaValues = Utility.SplitStringNIncludeEmpyValue(QuestionDataTable.Select("key = 'AREA'")[0]["DATA_VALUE"].ToString(), "{([~])}");

                if (SplittedAreaValues.Length > 2) //   if(  Array of Area has Length of < 3 ) then skip file
                {


                    //  Split the Name,Id and Gid
                    AreaName = Utility.RemoveQuotes(SplittedAreaValues[0]);
                    AreaID = Utility.RemoveQuotes(SplittedAreaValues[1]);
                    AreaGid = Utility.RemoveQuotes(SplittedAreaValues[2]);

                    //  Check _Area for the existence of this Area
                    DataTable AreaTable = this._DBConnection.ExecuteDataTable(this.EIQueries.CompareArea(AreaID, true));
                    if (AreaTable.Rows.Count > 0)     //  Area Exists
                    {
                        AreaNId = Convert.ToInt32(AreaTable.Rows[0]["Area_NId"]);
                        this._DBConnection.ExecuteNonQuery(this.EIQueries.UpdateArea(AreaNId, AreaName, AreaID));
                    }
                    else    //  Area Does not Exists ,Insert into _AreaUM
                    {
                        this._DBConnection.ExecuteNonQuery(this.EIQueries.InsertBlankTable(AreaName, AreaID, string.Empty));
                    }
                    AreaTable.Dispose();



                    //  get checked and unchecked value from xml file
                    if (XmlSourceDataset.Tables.Contains("MultipleOptionValsTable"))
                    {

                        CheckedValue = Convert.ToString(XmlSourceDataset.Tables["MultipleOptionValsTable"].Select("Item='Checked'")[0]["Value"]);
                        UncheckedValue = Convert.ToString(XmlSourceDataset.Tables["MultipleOptionValsTable"].Select("Item='Unchecked'")[0]["Value"]);
                    }
                    else
                    {
                        CheckedValue = "1";
                        UncheckedValue = "0";
                    }


                    //  Data
                    //  Insert Indicator,Unit,Sub Names and Gid's and Values
                    foreach (DataRow Row in QuestionDataTable.Rows)
                    {


                        IsMultipleSubgroup = false;

                        //  Initalise the Variable
                        //   if(  Indicator GId is NULL or Value is Null ) Leave the Record
                        if (Row[QuestionTableColumns.IndicatorGID] == null | String.IsNullOrEmpty(Row[QuestionTableColumns.IndicatorGID].ToString()))
                        {
                            continue;
                        }

                        if (Row[QuestionTableColumns.IndicatorGID].ToString().ToLower() == "null" |
                       Row[QuestionTableColumns.DataValue].ToString().Length == 0)
                        {
                            continue;
                        }

                        IUSValue = Row[QuestionTableColumns.QuestionText].ToString();


                        if (Row[QuestionTableColumns.AnswerType].ToString().ToUpper() == Questionnarie.AnswerTypes[AnswerType.Aggregate].ToUpper())
                        {
                            continue;
                        }


                        //  Handle Longitute ,Latitute and Interviewer Name
                        QuestionKey = Row[QuestionTableColumns.QuestionKey].ToString();

                        if ((QuestionKey.ToLower() == "timeperiod") || (QuestionKey.ToLower() == "SRC_PUBLISHER".ToLower()) ||
                        (QuestionKey.ToLower() == "AREA".ToLower()) || (QuestionKey.ToLower() == "LATITUDE".ToLower()) |
                        (QuestionKey.ToLower() == "LONGITUDE".ToLower()) | (QuestionKey.ToLower() == "NAME_INTERVIEWER".ToLower()))
                        {
                            continue;
                        }


                        //  get timeperiod and source associated with the question
                        TimePeriodNId = null;
                        //TODO: temp. changes 
                        //sTimePeriod_NId = this.GetTimeperiodNID(QuestionDataTable, sKey);
                        if (string.IsNullOrEmpty(TimePeriodNId))
                        {
                            TimePeriodNId = DefaultTimePeriodNId;
                        }

                        SourceNId = 0;
                        //TODO: temp. changes
                        //SourceNId = this.GetSourceNid(QuestionDataTable, sKey);

                        if (SourceNId <= 0)
                        {
                            SourceNId = DefaultSourceNId;
                        }



                        //  Get The Gids
                        if (Row[QuestionTableColumns.SubGroupGid] == null)
                        {
                            SubgroupValGid = "";
                        }
                        else
                        {
                            SubgroupValGid = Row[QuestionTableColumns.SubGroupGid].ToString();
                        }

                        //indicator
                        if (Row[QuestionTableColumns.IndicatorGID] == null)
                        {
                            IndicatorGid = "";
                        }
                        else
                        {
                            IndicatorGid = Row[QuestionTableColumns.IndicatorGID].ToString();
                        }

                        //unit
                        if (Row[QuestionTableColumns.UnitGid] == null)
                        {
                            UnitGid = "";
                        }
                        else
                        {
                            UnitGid = Row[QuestionTableColumns.UnitGid].ToString();
                        }


                        //  DataValue
                        DataValue = Utility.RemoveQuotes(Row[QuestionTableColumns.DataValue].ToString());

                        //  Speciel HaAndling for CH,SRB,SCB
                        RowAnswerType = Row[QuestionTableColumns.AnswerType].ToString();
                        if (RowAnswerType == Questionnarie.AnswerTypes[AnswerType.CH].ToString() |
                           RowAnswerType == Questionnarie.AnswerTypes[AnswerType.SRB].ToString() |
                           RowAnswerType == Questionnarie.AnswerTypes[AnswerType.SCB].ToString())
                        {
                            StringIDs = Utility.SplitStringNIncludeEmpyValue(Row[QuestionTableColumns.StringID].ToString(), ",");
                            SplittedDataValues = Utility.SplitStringNIncludeEmpyValue(DataValue, ",");
                            IsMultipleSubgroup = true;
                        }

                        //  IUS
                        if (IsMultipleSubgroup)
                        {
                            StringTable = XmlSourceDataset.Tables[1];


                            DataTable OptionSubgroupMappingTable = XmlSourceDataset.Tables["OptionSubgroupMappingTable"];

                            //  Get the Subgroup GID From Second table in case of Speciel Cases 
                            for (int i = 0; i < StringIDs.Length; i++)
                            {
                                IUSNId = -1;
                                string SGGid = string.Empty;
                                string GRIDCellIndex = "GRID";
                                DataRow[] aDR = null;

                                if (Convert.ToString(Row[QuestionTableColumns.GridID]) != "-1")
                                {
                                    GRIDCellIndex += Convert.ToString(Row[QuestionTableColumns.GridID]) + "_" + Convert.ToString(Row[QuestionTableColumns.QuestionNo]).Replace("Q", "");

                                    aDR = OptionSubgroupMappingTable.Select("ID ='" + StringIDs[i] + "' AND Key='" + GRIDCellIndex + "'");
                                }
                                else
                                {
                                    aDR = OptionSubgroupMappingTable.Select("ID ='" + StringIDs[i] + "' AND Key='" + QuestionKey + "'");
                                }
                                
                                if (aDR.Length > 0)
                                {
                                    SGGid = Convert.ToString(aDR[0]["Subgroup_VAL_GID"]);
                                }


                                //  Checking for IUS 
                                 Table = this._DBConnection.ExecuteDataTable(this.EIQueries.CheckIUSValuesExistsbyGid(IndicatorGid, UnitGid,
                               SGGid));

                                if (Table.Rows.Count > 0)             //  Found
                                {
                                    IUSNId = Convert.ToInt32(Table.Rows[0]["IUSNId"]);
                                }
                                else        //  IUS not found so insert Into _IUSUM
                                {
                                    this.InsertIUSIUSUM(IUSValue, "", SGGid, -1, -1, -1,
                                    Path.GetFileName(filename));
                                }
                                Table.Dispose();



                                if (!string.IsNullOrEmpty(SplittedDataValues[i]))
                                {
                                    if (SplittedDataValues[i] == "1")
                                    {
                                        SplittedDataValues[i] = CheckedValue.ToString();
                                    }
                                    else
                                    {
                                        SplittedDataValues[i] = UncheckedValue.ToString();
                                    }
                                }


                                //  Insert in Data
                                this.SaveData(IUSValue, "", SGGid, AreaName,
                                AreaNId.ToString(), AreaID
                                    , Timeperiod, TimePeriodNId.ToString(), IUSNId.ToString(), Source,
                                    SourceNId.ToString(), Utility.RemoveRegionalThousandSeperator(SplittedDataValues[i]),
                                                 -1, "", -1, -1, -1);
                            }
                        }
                        else // bAnswerCH  is false
                        {
                            IUSNId = -1;
                            
                            Table = this._DBConnection.ExecuteDataTable(this.EIQueries.CheckIUSValuesExistsbyGid(IndicatorGid, UnitGid, SubgroupValGid));
                            if (Table.Rows.Count > 0)     //  Found
                            {
                                IUSNId = Convert.ToInt32(Table.Rows[0]["IUSNId"]);
                            }
                            else
                            {
                                this.InsertIUSIUSUM(IUSValue, "", "", -1, -1, -1, Path.GetFileName(filename));
                            }
                            Table.Dispose();


                            //  Insert in Data
                            this.SaveData(IUSValue, "", "", AreaName, AreaNId.ToString(), AreaID
                                        , Timeperiod, TimePeriodNId.ToString(), IUSNId.ToString(), Source,
                                        SourceNId.ToString(), Utility.RemoveRegionalThousandSeperator(DataValue),
                                         -1, "", -1, -1, -1);
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }


        #region "-- Aggregate Questions --"

        private void CalculateValueForAggregateType()
        {
            DataSet XmlSourceDataset = null;
            DataTable QuestionTable = null;
            int AreaNId = 0;
            string DataValue = string.Empty;
            string DefaultTimePeriodNId = string.Empty;
            int DefaultSourceNId = 0;
            int AreaParentNid = 0;
            int DataNid = 0;
            int Count = 0;

            try
            {
                //Me.Cursor = Cursors.WaitCursor

                // Loop for Each File Selected 
                foreach (string Filename in this.XmlProcessor.XmlFiles.Values)
                {
                    AreaParentNid = 0;

                    // fill XML Dataset
                    this.FillXMLDataset(ref XmlSourceDataset, ref QuestionTable, Filename);

                    if (XmlSourceDataset == null)
                    {
                        continue; //do processing for next file
                    }

                    // Get DataValue of The Time Period & source
                    // Time
                    DefaultTimePeriodNId = null;
                    DefaultTimePeriodNId = this.GetTimeperiodNID(QuestionTable);

                    // Source
                    DefaultSourceNId = 0;
                    DefaultSourceNId = this.GetSourceNid(QuestionTable);

                    // Get Area Nid
                    AreaNId = this.GetAreaNID(ref QuestionTable, ref AreaParentNid);

                    if (AreaNId == -1)
                    {
                        continue; // goto next file
                    }

                    // get the data value of the target question
                    this.CalculateSum(ref QuestionTable, ref DefaultTimePeriodNId, ref DefaultSourceNId, ref AreaParentNid);
                    this.CalculateCount(ref QuestionTable, ref DefaultTimePeriodNId, ref DefaultSourceNId, ref AreaParentNid);

                    if (Path.GetExtension(this.DatabaseFileNameWPath).ToLower() != ".tpl") // calculate avg. only for template
                    {
                        this.CalculateAvg(ref QuestionTable, ref DefaultTimePeriodNId, ref DefaultSourceNId, ref AreaParentNid);
                        this.CalculatePercent(ref QuestionTable, ref DefaultTimePeriodNId, ref DefaultSourceNId, ref AreaParentNid);
                    }

                }

                //if ( Not IsNothing(msarrRecords) And gbMdbToImport = False Then
                if (this.AggregateQuestionRecords != null & Path.GetExtension(this.DatabaseFileNameWPath).ToLower() != ".tpl")
                {
                    // calculating average
                    for (int cc = 0; cc < this.AggregateQuestionRecords.Count - 1; cc++)
                    {
                        if (!string.IsNullOrEmpty(AggregateQuestionRecords[cc]))
                        {
                             DataNid = Convert.ToInt32(Utility.SplitStringNIncludeEmpyValue(this.AggregateQuestionRecords[cc], ",")[0]);
                            Count = Convert.ToInt32(Utility.SplitStringNIncludeEmpyValue(this.AggregateQuestionRecords[cc], ",")[1]);

                            string sDataValue = this._DBConnection.ExecuteDataTable(this.EIQueries.GetDataValue(DataNid)).Rows[0][0].ToString();
                            sDataValue = (Convert.ToInt32(sDataValue) / Count).ToString();

                            // update datavalue
                            this._DBConnection.ExecuteNonQuery(this.EIQueries.UpdateRecordInData(DataNid, sDataValue));
                        }
                    }
                }


                //if (Not IsNothing(msarrPERC) And gbMdbToImport = False Then
                if (this.PercentRecords != null & Path.GetExtension(this.DatabaseFileNameWPath).ToLower() != ".tpl")
                {
                    // calculating percentage
                    for (int cc = 0; cc < this.PercentRecords.Count - 1; cc++)
                    {
                        if (!string.IsNullOrEmpty(this.PercentRecords[cc]))
                        {
                             DataNid = Convert.ToInt32(Utility.SplitStringNIncludeEmpyValue(this.PercentRecords[cc], ",")[0]);
                            Count = Convert.ToInt32(Utility.SplitStringNIncludeEmpyValue(this.PercentRecords[cc], ",")[1]);

                           DataValue = this._DBConnection.ExecuteDataTable(this.EIQueries.GetDataValue(DataNid)).Rows[0][0].ToString();
                            DataValue = (Convert.ToInt32(DataValue) * 100 / Count).ToString();

                            // update datavalue
                            this._DBConnection.ExecuteNonQuery(this.EIQueries.UpdateRecordInData(DataNid, DataValue));
                        }
                    }
                }

                this.CalculateCompositeIndex();

                System.Threading.Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);                
            }
            
        }

        private void CalculateSum(ref DataTable questionTable, ref string defaultTimePeriodNId, ref int defaultSourceNId, ref int areaParentNid)
        {
            string TimePeriodNId = string.Empty;
            string QuestionKey = string.Empty;
            int SourceNId = 0;
            int RowIndex = 0;
            int IUSNId = 0;
            int ParentNID = 0;
            string DataValue = string.Empty;
            DataTable Table;
            DataRow AggregateRow;

            foreach (DataRow Row in questionTable.Select("AnsType='Aggregate' and Data_Value like 'SUM,%'"))
            {
                AggregateRow = Row; //to pass refernece of row

                double dValue = this.GetTargetQuestionValue(AggregateType.SUM, ref AggregateRow, ref questionTable, ref TimePeriodNId, ref QuestionKey, ref defaultTimePeriodNId, ref SourceNId, ref defaultSourceNId, ref RowIndex, ref IUSNId);

                if (dValue <= 0)
                {
                    continue;
                }


                // get the parent AreaID till Parent_ID is -1 and update/insert value 
                ParentNID = areaParentNid;
                while (true)
                {
                    if (ParentNID == -1)
                    {
                        break;
                    }


                    // update/insert data value
                    Table = this._DBConnection.ExecuteDataTable(this.EIQueries.GetRecordFrmData(IUSNId, ParentNID, Convert.ToInt32(TimePeriodNId), SourceNId));
                    if (Table.Rows.Count > 0)
                    {
                        DataValue = (Convert.ToInt32(Table.Rows[0]["Data_Value"]) + dValue).ToString();

                        this._DBConnection.ExecuteNonQuery(this.EIQueries.UpdateRecordInData(Convert.ToInt32(Table.Rows[0]["Data_Nid"]), DataValue));
                    }
                    else
                    {
                        // create new record
                        DataValue = dValue.ToString();
                        this._DBConnection.ExecuteNonQuery(this.EIQueries.CreateRecordInData(IUSNId, ParentNID, Convert.ToInt32(TimePeriodNId), SourceNId, DataValue));
                    }



                    // find next area_parent_nid
                    Table = this._DBConnection.ExecuteDataTable(this.EIQueries.CompareAreaForAggregate(ParentNID.ToString(), false));
                    if (Table.Rows.Count > 0)
                    {
                        ParentNID = Convert.ToInt32(Table.Rows[0]["Area_Parent_NId"]);
                    }
                    else
                    {
                        // if parent not found in database/template
                        break;
                    }
                    Table.Dispose();

                }
            }

        }

        private void CalculateCount(ref  DataTable questionTable, ref string defaultTimePeriodNId, ref int defaultSourceNId,
            ref int areaParentNid)
        {
            string TimePeriodNId = string.Empty;
            string QuestionKey = string.Empty;
            int SourceNId = 0;
            int TotalRow = 0;
            int IUSNId = 0;
            int ParentNID = 0;
            string DataValue = string.Empty;
            double Value;
            DataRow AggregateRow;
            DataTable Table;

            foreach (DataRow Row in questionTable.Select("AnsType='Aggregate' and Data_Value like 'COUNT,%'"))
            {

                AggregateRow = Row;
                Value = this.GetTargetQuestionValue(AggregateType.COUNT, ref AggregateRow, ref questionTable, ref TimePeriodNId, ref QuestionKey, ref defaultTimePeriodNId, ref SourceNId, ref defaultSourceNId, ref TotalRow, ref IUSNId);

                if (Value <= 0)
                {
                    continue;
                }


                // get the parent AreaID till Parent_ID is -1 and update/insert value 
                ParentNID = areaParentNid;
                while (true)
                {
                    if (ParentNID == -1)
                    {
                        break;
                    }


                    // update/insert data value
                    Table = this._DBConnection.ExecuteDataTable(this.EIQueries.GetRecordFrmData(IUSNId, ParentNID, Convert.ToInt32(TimePeriodNId), SourceNId));
                    if (Table.Rows.Count > 0)
                    {
                        DataValue = (Convert.ToInt32(Table.Rows[0]["Data_Value"]) + Value).ToString();

                        this._DBConnection.ExecuteNonQuery(this.EIQueries.UpdateRecordInData(Convert.ToInt32(Table.Rows[0]["Data_Nid"]), DataValue));
                    }
                    else
                    {
                        // create new record
                        DataValue = Value.ToString();
                        this._DBConnection.ExecuteNonQuery(this.EIQueries.CreateRecordInData(IUSNId, ParentNID, Convert.ToInt32(TimePeriodNId), SourceNId, DataValue));
                    }

                    //find next area_parent_nid
                    Table = this._DBConnection.ExecuteDataTable(this.EIQueries.CompareAreaForAggregate(ParentNID.ToString(), false));

                    if (Table.Rows.Count > 0)
                    {
                        ParentNID = Convert.ToInt32(Table.Rows[0]["Area_Parent_NId"]);
                    }
                    else
                    {
                        // if parent not found in database/template
                        break;
                    }
                    Table.Dispose();
                }
            }
        }


        private void CalculateAvg(ref DataTable questionTable, ref string defaultTimePeriodNId, ref int defaultSourceNId,
            ref int areaParentNid)
        {
            string TimePeriodNId = string.Empty;
            string QuestionKey = string.Empty;
            int SourceNId = 0;
            int TotalRows = 0;
            int IUSNId = 0;
            double Value;
            DataRow AggregateRow;
            DataTable Table;
            string[] Nids;
            string DataValue = string.Empty;

            foreach (DataRow Row in questionTable.Select("AnsType='Aggregate' and Data_Value like 'AVG,%'"))
            {
                AggregateRow = Row;
                Value = this.GetTargetQuestionValue(AggregateType.AVG, ref AggregateRow, ref questionTable, ref TimePeriodNId, ref QuestionKey, ref defaultTimePeriodNId, ref SourceNId, ref defaultSourceNId, ref TotalRows, ref IUSNId);


                if (Value <= 0)
                {
                    continue;
                }

                // get the parent AreaID till Parent_ID is -1 and update/insert value 
                int iParent_NID = areaParentNid;
                while (true)
                {
                    if (iParent_NID == -1)
                    {
                        break;
                    }


                    // update/insert data value
                    Table = this._DBConnection.ExecuteDataTable(this.EIQueries.GetRecordFrmData(IUSNId, iParent_NID, Convert.ToInt32(TimePeriodNId), SourceNId));

                    if (Table.Rows.Count > 0)
                    {
                        DataValue = (Convert.ToInt32(Table.Rows[0]["Data_Value"]) + Value).ToString();

                        this._DBConnection.ExecuteNonQuery(this.EIQueries.UpdateRecordInData(Convert.ToInt32(Table.Rows[0]["Data_Nid"]), DataValue));

                        // Update array 
                        for (int cc = 0; cc < this.AggregateQuestionRecords.Count; cc++)
                        {

                           Nids = Utility.SplitStringNIncludeEmpyValue(this.AggregateQuestionRecords[cc], ",");
                            if (Nids.Length > 0)
                            {
                                if (Nids[0] == Table.Rows[0]["Data_Nid"].ToString())
                                {
                                    this.AggregateQuestionRecords[cc] = Nids[0] + "," + Convert.ToInt32(Nids[1]) + 1;
                                }
                            }
                        }
                    }
                    else
                    {
                        // create new record
                        DataValue = Value.ToString();
                        this._DBConnection.ExecuteNonQuery(this.EIQueries.CreateRecordInData(IUSNId, iParent_NID, Convert.ToInt32(TimePeriodNId), SourceNId, DataValue));

                        if (this.AggregateQuestionRecords == null)
                        {
                            this.AggregateQuestionRecords = new List<string>();
                            //ReDim Preserve msarrRecords(1)
                        }
                        else
                        {

                            //   ReDim Preserve msarrRecords(msarrRecords.Length)
                        }


                        // get data_nid &  'save : Data_NID,1
                        this.AggregateQuestionRecords[this.AggregateQuestionRecords.Count - 1] = this._DBConnection.ExecuteDataTable(this.EIQueries.GetRecordFrmData(IUSNId, iParent_NID, Convert.ToInt32(TimePeriodNId), SourceNId)).Rows[0]["Data_Nid"].ToString() + ",1";


                    }


                    // find next area_parent_nid
                    Table = this._DBConnection.ExecuteDataTable(this.EIQueries.CompareAreaForAggregate(iParent_NID.ToString(), false));
                    if (Table.Rows.Count > 0)
                    {
                        iParent_NID = Convert.ToInt32(Table.Rows[0]["Area_Parent_NId"]);
                    }
                    else
                    {
                        // if parent not found in database/template

                        break;
                    }
                    Table.Dispose();


                }
            }
        }

        private void CalculatePercent(ref DataTable questionTable, ref string defaultTimePeriodNId, ref int defaultSourceNId,
            ref int areaParentNid)
        {
            string TimePeriodNId = string.Empty;
            string QuestionKey = string.Empty;
            int SourceNId = 0;
            int TotalRows = 0;
            int IUSNId = 0;
            int ParentNID = 0;
            DataRow AggregateRow;
            DataTable Table;
            string DataValue = string.Empty;
            double NumeratorQuestionValue;
            double DenominatorQuestionValue; 

            foreach (DataRow Row in questionTable.Select("AnsType='Aggregate' and Data_Value like 'PERC,%'"))
            {
                AggregateRow = Row;

                // to Find data value for numerator: send true
                NumeratorQuestionValue = this.GetTargetQuestionValue(AggregateType.PERCENT, ref AggregateRow, ref questionTable, ref TimePeriodNId, ref QuestionKey, ref defaultTimePeriodNId, ref SourceNId, ref defaultSourceNId, ref TotalRows, ref IUSNId, true);

                if (NumeratorQuestionValue <= 0)
                {
                    NumeratorQuestionValue = 0;
                }

                // to find data value for denominator : send false 
                DenominatorQuestionValue = this.GetTargetQuestionValue(AggregateType.PERCENT, ref AggregateRow, ref questionTable, ref TimePeriodNId, ref QuestionKey, ref defaultTimePeriodNId, ref SourceNId, ref defaultSourceNId, ref TotalRows, ref IUSNId, false);

                if (DenominatorQuestionValue <= 0)
                {
                    continue;
                }


                // get the parent AreaID till Parent_ID is -1 and update/insert value 
                ParentNID = areaParentNid;
                while (true)
                {
                    if (ParentNID == -1)
                    {
                        break;
                    }



                    // update/insert data value
                    Table = this._DBConnection.ExecuteDataTable(this.EIQueries.GetRecordFrmData(IUSNId, ParentNID, Convert.ToInt32(TimePeriodNId), SourceNId));


                    if (Table.Rows.Count > 0)
                    {
                        // save numerator's Data value
                        DataValue = (Convert.ToInt32(Table.Rows[0]["Data_Value"]) + NumeratorQuestionValue).ToString();

                        this._DBConnection.ExecuteNonQuery(this.EIQueries.UpdateRecordInData(Convert.ToInt32(Table.Rows[0]["Data_Nid"]), DataValue));

                        // Update array with denominator's data value
                        for (int cc = 0; cc < this.PercentRecords.Count - 1; cc++)
                        {
                            string[] sarrNids = Utility.SplitStringNIncludeEmpyValue(this.PercentRecords[cc], ",");
                            if (sarrNids.Length > 0)
                            {
                                if (sarrNids[0] == Table.Rows[0]["Data_Nid"].ToString())
                                {
                                    // add QD_value
                                    PercentRecords[cc] = sarrNids[0] + "," + (Convert.ToInt32(sarrNids[1]) + DenominatorQuestionValue).ToString();
                                }
                            }
                        }
                    }
                    else
                    {
                        //create new record
                        DataValue = NumeratorQuestionValue.ToString();    // save numerator's Data value
                        this._DBConnection.ExecuteNonQuery(this.EIQueries.CreateRecordInData(IUSNId, ParentNID, Convert.ToInt32(TimePeriodNId), SourceNId, DataValue));

                        if (this.PercentRecords == null)
                        {
                            this.PercentRecords = new List<string>();
                            this.PercentRecords.Add(string.Empty);
                            //ReDim Preserve msarrPERC(1)
                        }
                        else
                        {
                            //ReDim Preserve msarrPERC(msarrPERC.Length);
                        }


                        // get data_nid &  'save : Data_NID,denominator's data value
                        this.PercentRecords[PercentRecords.Count - 1] = this._DBConnection.ExecuteDataTable(this.EIQueries.GetRecordFrmData(IUSNId, ParentNID, Convert.ToInt32(TimePeriodNId), SourceNId)).Rows[0]["Data_Nid"].ToString() + "," + DenominatorQuestionValue;

                    }



                    // find next area_parent_nid
                    Table = this._DBConnection.ExecuteDataTable(this.EIQueries.CompareAreaForAggregate(ParentNID.ToString(), false));
                    if (Table.Rows.Count > 0)
                    {
                        ParentNID = Convert.ToInt32(Table.Rows[0]["Area_Parent_NId"]);
                    }
                    else
                    {
                        // if parent not found in database/template

                        break;
                    }

                    Table.Dispose();


                }

            }

        }

        #region "-- Composite Index --"

        private void CalculateCompositeIndex()
        {
            #region "--variables for first file --"
            DataSet XmlSourceDataset = null;
            DataTable QuestionTable = null;
            int AreaNId = 0;                   // Integers to hold  Area Nid
            int AreaParentNid = 0;
            string DefaultTimePeriodNId = string.Empty;
            int DefaultSourceNId = 0;
            string FirstFilename = string.Empty;
            string TimePeriodNId = string.Empty;
            string QuestionKey = string.Empty;
            int SourceNId = 0;
            int IUSNId = 0;
            int MinScale = 0;
            int MaxScale = 0;
            int Decimal = 0;
            int TotalTargetQuestions = 0;
            string[] COMDataValues = null;
            string COMDataValue = string.Empty;
            DataTable CompositeQuestionTable = null;

            #endregion

            #region "-- variables for other files --"
            DataSet XMLDataSet = null;
            DataTable XMLTable = null;
            DataRow[] Rows = null;
            int XMLAreaNid = 0;
            int XMLAreaParentNid = 0;
            string XMLDefaultTimePeriod = string.Empty;
            int XMLDefaultSourceNId = 0;
            string XMLTimePeriod = string.Empty;
            int XMLSourceNId = 0;
            int XMLIUSNId = 0;
            string XMLCOMDataValue = string.Empty;
            string[] XMLCOMDataValues = null;

            #endregion

            #region "-- Other Variables --"
            double Score = 0;
            double MinDataValue = 0;
            double MaxDataValue = 0;
            double DataValue = 0;
            double Index = 0;
            double Weight = 0;
            bool bHighIsGood = false;
            double MaxScore = 0;
            double MinScore = 0;
            double CompositeIndex = 0;
            DataTable TempTable;
            #endregion

            try
            {
                //   Me.Cursor = Cursors.WaitCursor


                //Step 1: read COM Questions from  first xml file
                foreach (string XmlFilename in this.XmlProcessor.XmlFiles.Values)
                {
                    FirstFilename = XmlFilename;
                    break;
                }

                // fill XML Dataset
                this.FillXMLDataset(ref XmlSourceDataset, ref QuestionTable, FirstFilename);

                if (XmlSourceDataset != null)
                {

                    // Step 2: get Composite index questions  from first xml file
                    // DATA_VALUE : COM,[Decimal],[MIN_SCALE],[MAX_SCALE],[Q1,WEIGHT,HIGH_IS_GOOD(True/False)],[Q1,WEIGHT,HIGH_IS_GOOD(T/F)]
                    // COM,[3],[1],[100],[Q1,20,True],[Q3,40,True],[Q7,40,False]

                    foreach (DataRow dr in QuestionTable.Select("AnsType='Aggregate' and Data_Value like 'COM,%'"))
                    {
                        TimePeriodNId = string.Empty;
                        QuestionKey = string.Empty;
                        SourceNId = 0;
                        IUSNId = 0;
                        MinScale = 0;
                        MaxScale = 0;
                        Decimal = 0;
                        TotalTargetQuestions = 0;
                        COMDataValues = null;
                        COMDataValue = string.Empty;
                        CompositeQuestionTable = null;



                        // step 3: find Value from xml file
                        // Get DataValue of The Time Period & source
                        // Time
                        DefaultTimePeriodNId = null;
                        DefaultTimePeriodNId = this.GetTimeperiodNID(QuestionTable);

                        // Source
                        DefaultSourceNId = 0;
                        DefaultSourceNId = this.GetSourceNid(QuestionTable);

                        //Get Area Nid
                        AreaNId = this.GetAreaNID(ref QuestionTable, ref AreaParentNid);
                        if (AreaNId == -1)
                        {
                            continue;
                        }

                        // Step 4: get mandatory values to insert data value
                        QuestionKey = dr["Key"].ToString();

                        this.GetCOMBasicValue(ref QuestionKey, ref QuestionTable, ref TimePeriodNId, ref DefaultTimePeriodNId, ref SourceNId,
                            ref DefaultSourceNId, ref IUSNId);

                        if (IUSNId <= 0)
                        {
                            continue;
                        }


                        COMDataValue = dr["Data_Value"].ToString();

                        if (string.IsNullOrEmpty(COMDataValue))
                        {
                            continue;
                        }

                        // Step 5: get min & max scale and also findout total numbers of target questions to create table
                        COMDataValue = (COMDataValue.Replace("COM,", "")).Trim();
                        COMDataValues = Utility.SplitStringNIncludeEmpyValue(COMDataValue, ",[");
                        Decimal = Convert.ToInt32(COMDataValues[0].Substring(1, COMDataValues[0].Length - 2));
                        MinScale = Convert.ToInt32(COMDataValues[1].Substring(0, COMDataValues[1].Length - 1));
                        MaxScale = Convert.ToInt32(COMDataValues[2].Substring(0, COMDataValues[2].Length - 1));
                        TotalTargetQuestions = COMDataValues.Length - 3;

                        // Step 6: create data table for calculation 
                        CompositeQuestionTable = this.CreateTableCompositeIndex(TotalTargetQuestions);

                        // find datavalue for each targetQuestion and save it into table
                        this.SaveValueInComTbl(ref CompositeQuestionTable, QuestionTable, TotalTargetQuestions, COMDataValues, AreaNId, IUSNId,
                            TimePeriodNId, SourceNId, MinScale, MaxScale);

                        // Step 7: get values from other xml files
                        foreach (string FileName in this.XmlProcessor.XmlFiles.Values)
                        {
                            if (FileName != FirstFilename)
                            {

                                XMLDataSet = null;
                                XMLTable = null;
                                Rows = null;
                                XMLAreaNid = 0;
                                XMLAreaParentNid = 0;
                                XMLDefaultTimePeriod = string.Empty;
                                XMLDefaultSourceNId = 0;
                                XMLTimePeriod = string.Empty;
                                XMLSourceNId = 0;
                                XMLIUSNId = 0;
                                XMLCOMDataValue = string.Empty;
                                XMLCOMDataValues = null;

                                // fill XML Dataset
                                this.FillXMLDataset(ref XMLDataSet, ref XMLTable, FileName);

                                if (XMLDataSet != null)
                                {
                                    Rows = XMLTable.Select("Key='" + QuestionKey + "'");
                                    if (Rows.Length > 0)
                                    {

                                        //Get DataValue of The Time Period & source
                                        // Time
                                        XMLDefaultTimePeriod = null;
                                        XMLDefaultTimePeriod = this.GetTimeperiodNID(XMLTable);
                                        // Source
                                        XMLDefaultSourceNId = 0;
                                        XMLDefaultSourceNId = this.GetSourceNid(XMLTable);
                                        //Get Area Nid
                                        XMLAreaNid = this.GetAreaNID(ref XMLTable, ref XMLAreaParentNid);
                                        if (XMLAreaNid == -1)
                                        {
                                            continue;
                                        }

                                        // check records already entered for the selected area_nid or not
                                        if (CompositeQuestionTable.Select("Area_Nid =" + XMLAreaNid).Length > 0)
                                        {
                                            continue;
                                        }


                                        //get mandatory values to insert data value
                                        this.GetCOMBasicValue(ref QuestionKey, ref XMLTable, ref XMLTimePeriod, ref XMLDefaultTimePeriod, ref XMLSourceNId,
                                            ref XMLDefaultSourceNId, ref XMLIUSNId);

                                        if (XMLIUSNId <= 0)
                                        {
                                            continue;
                                        }

                                        XMLCOMDataValue = Rows[0]["Data_Value"].ToString();

                                        if (string.IsNullOrEmpty(XMLCOMDataValue))
                                        {
                                            continue;
                                        }

                                        // get total numbers of target questions 
                                        XMLCOMDataValue = (XMLCOMDataValue.Replace("COM,", "")).Trim();
                                        XMLCOMDataValues = Utility.SplitStringNIncludeEmpyValue(XMLCOMDataValue, ",[");

                                        if (TotalTargetQuestions != COMDataValues.Length - 3)
                                        {
                                            continue;
                                        }

                                        // find datavalue for each targetQuestion and save it into COM table
                                        this.SaveValueInComTbl(ref CompositeQuestionTable, XMLTable, TotalTargetQuestions, XMLCOMDataValues,
                                            XMLAreaNid, XMLIUSNId, XMLTimePeriod, XMLSourceNId, MinScale, MaxScale);

                                    }
                                }

                            }
                        }
                        // Step 8: calculate index for each row
                        if (CompositeQuestionTable == null & CompositeQuestionTable.Rows.Count > 0)
                        {
                            foreach (DataRow drCom in CompositeQuestionTable.Rows)
                            {

                                Score = 0;

                                for (int x = 1; x <= TotalTargetQuestions; x++)
                                {

                                    MinDataValue = 0;
                                    MaxDataValue = 0;
                                    DataValue = 0;
                                    Index = 0;
                                    Weight = 0;
                                    bHighIsGood = false;

                                    MaxDataValue = Microsoft.VisualBasic.Conversion.Val(CompositeQuestionTable.Select("[Ind_" + x + "]=MAX([Ind_" + x + "])")[0]["Ind_" + x]);
                                    MinDataValue = Microsoft.VisualBasic.Conversion.Val(CompositeQuestionTable.Select("[Ind_" + x + "]=MIN([Ind_" + x + "])")[0]["Ind_" + x]);

                                    // get values from row
                                    DataValue = Convert.ToDouble(drCom["Ind_" + x]);
                                    bHighIsGood = Convert.ToBoolean(drCom["HighIsGood_" + x]);
                                    Weight = Convert.ToDouble(drCom["Weight_" + x]);

                                    // calculate
                                    if (MaxDataValue == MinDataValue)
                                    {
                                        Index = 1;
                                    }
                                    else
                                    {
                                        if (bHighIsGood)
                                        {
                                            Index = ((DataValue - MinDataValue) / (MaxDataValue - MinDataValue));
                                        }
                                        else
                                        {
                                            Index = ((DataValue - MaxDataValue) / (MinDataValue - MaxDataValue));
                                        }

                                    }

                                    // save index
                                    drCom["Index_" + x] = Index;
                                    CompositeQuestionTable.AcceptChanges();

                                    // Step 9: calculate score
                                    Score += Index * Weight;
                                }

                                // save score
                                drCom["Score"] = (Score / 100);
                                CompositeQuestionTable.AcceptChanges();
                            }


                            // Step 10: calculate composite index
                            foreach (DataRow rw in CompositeQuestionTable.Rows)
                            {
                                MaxScore = 0;
                                MinScore = 0;
                                CompositeIndex = 0;

                                MinScore = Convert.ToDouble(CompositeQuestionTable.Select("[Score]=Min([Score])")[0]["Score"]);
                                MaxScore = Convert.ToDouble(CompositeQuestionTable.Select("[Score]=MAX([Score])")[0]["Score"]);
                                if ((MaxScore - MinScore) == 0)
                                {
                                    CompositeIndex = MaxScale;
                                }
                                else
                                {
                                    CompositeIndex = MinScale + (((Convert.ToDouble(rw["Score"]) - MinScore) / (MaxScore - MinScore)) * (MaxScale - MinScale));
                                }

                                CompositeIndex = Math.Round(CompositeIndex, Decimal);
                                rw["CompositeIndex"] = CompositeIndex;
                                CompositeQuestionTable.AcceptChanges();

                                // update/insert data value
                                TempTable = this._DBConnection.ExecuteDataTable(this.EIQueries.GetRecordFrmData(Convert.ToInt32(rw["IUSNID"].ToString()),
                                    Convert.ToInt32(rw["Area_NID"].ToString()), Convert.ToInt32(rw["Timeperiod_NID"].ToString()),
                                    Convert.ToInt32(rw["Source_NID"].ToString())));

                                if (TempTable.Rows.Count > 0)
                                {
                                    this._DBConnection.ExecuteNonQuery(this.EIQueries.UpdateRecordInData(Convert.ToInt32(TempTable.Rows[0]["Data_Nid"]), CompositeIndex.ToString()));
                                }
                                else
                                {
                                    // create new record
                                    this._DBConnection.ExecuteNonQuery(this.EIQueries.CreateRecordInData(Convert.ToInt32(rw["IUSNID"].ToString()), Convert.ToInt32(rw["Area_NID"].ToString()),
                                        Convert.ToInt32(rw["Timeperiod_NID"].ToString()), Convert.ToInt32(rw["Source_NID"].ToString()),
                                        CompositeIndex.ToString()));
                                }


                            }
                            CompositeQuestionTable.Dispose();
                            CompositeQuestionTable = null;

                        }       //close if

                    } //close for
                }// close if
            }//close try
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
                //Call showErrorMessage(e1)
            }

        }


        private void SaveValueInComTbl(ref DataTable compositeQuestionTable, DataTable sourceTable, int totalTargetQuestions,
        string[] compositeDataValues, int areaNid, int IUSNID, string timePeriodNId, int sourceNId,
        int minScale, int maxScale)
        {
            string TargetQuestion = string.Empty;
            bool IsHighIsGood = false;
            int Weight = 0;
            double DataValue = 0;
            DataRow CompositeRow;

            for (int i = 1; i <= totalTargetQuestions; i++)
            {
                TargetQuestion = compositeDataValues[i + 2];
                DataValue = this.GetTargetQuestionDataValue(sourceTable, TargetQuestion, ref Weight, ref IsHighIsGood);

                if (i == 1)
                {
                    //insert value in com data table
                    CompositeRow = compositeQuestionTable.NewRow();
                }
                else
                {
                    CompositeRow = compositeQuestionTable.Select("Area_Nid=" + areaNid)[0];
                }
                CompositeRow["Area_NID"] = areaNid;
                CompositeRow["IUSNID"] = IUSNID;
                CompositeRow["Timeperiod_NID"] = timePeriodNId;
                CompositeRow["Source_NID"] = sourceNId;
                CompositeRow["Min_Scale"] = minScale;
                CompositeRow["Max_Scale"] = maxScale;

                CompositeRow["Ind_" + i] = DataValue;
                CompositeRow["Weight_" + i] = Weight;
                CompositeRow["HighIsGood_" + i] = IsHighIsGood;
                CompositeRow["Index_" + i] = 0;

                if (i == 1)
                {
                    compositeQuestionTable.Rows.Add(CompositeRow);
                }

                compositeQuestionTable.AcceptChanges();
                CompositeRow = null;
            }
        }

        private double GetTargetQuestionDataValue(DataTable sourceTable, string targetQuestionKey, ref int weight, ref bool isHighIsGood)
        {
            // strgQuest should be Q10,20,True/False])
            double RetVal = 0;
            DataRow[] Rows;
            string[] TargetQuestions;

            if (targetQuestionKey.StartsWith("Q"))
            {
                targetQuestionKey = targetQuestionKey.Replace("[", "");
                targetQuestionKey = targetQuestionKey.Replace("]", "");
                 TargetQuestions = Utility.SplitStringNIncludeEmpyValue(targetQuestionKey, ",");

                if (TargetQuestions.Length > 0)
                {

                    weight = Convert.ToInt32(TargetQuestions[1]);
                    isHighIsGood = Convert.ToBoolean(TargetQuestions[2]);

                    // get the datavalue of the target question
                    Rows = sourceTable.Select("Key='" + TargetQuestions[0] + "'");
                    if (Rows.Length > 0)
                    {
                        if (Rows[0]["Numeric_value"].ToString().Length > 0)
                        {
                            RetVal = Convert.ToDouble(Rows[0]["Numeric_value"].ToString());
                        }
                    }
                }
            }

            return RetVal;
        }

        private void GetCOMBasicValue(ref string questionKey, ref DataTable questionTable, ref string timePeriodNId,
        ref string defaultTimePeriodNId, ref int sourceNId, ref int defaultSourceNId, ref int IUSNId)
        {
            string SubgroupValGid = string.Empty;
            string IndicatorGid = string.Empty;
            string UnitGid = string.Empty;
            DataRow Row = questionTable.Select("Key='" + questionKey + "'")[0];

            // get timeperiod and source associated with the Target question
            timePeriodNId = null;
            timePeriodNId = this.GetTimeperiodNID(questionTable, questionKey);
            if (string.IsNullOrEmpty(timePeriodNId))
            {
                timePeriodNId = defaultTimePeriodNId;
            }

            sourceNId = 0;
            sourceNId = this.GetSourceNid(questionTable, questionKey);

            if (sourceNId < 0)
            {
                sourceNId = defaultSourceNId;
            }


            // Get The Gids of the Aggregate question
            //subgroup
            if (Microsoft.VisualBasic.Information.IsDBNull(Row["S_Gid"]))
            {
                SubgroupValGid = string.Empty;
            }
            else
            {
                SubgroupValGid = Row["S_Gid"].ToString();
            }

            //indicator
            if (Microsoft.VisualBasic.Information.IsDBNull(Row["I_Gid"]))
            {
                IndicatorGid = string.Empty;
            }
            else
            {
                IndicatorGid = Row["I_Gid"].ToString();
            }

            //unit
            if (Microsoft.VisualBasic.Information.IsDBNull(Row["U_Gid"]))
            {
                UnitGid = string.Empty;
            }
            else
            {
                UnitGid = Row["U_Gid"].ToString();
            }

            // get IUSNID
            IUSNId = this.GetIUSNid(IndicatorGid, UnitGid, SubgroupValGid);

        }

        private DataTable CreateTableCompositeIndex(int totalTragetColumns)
        {
            DataTable RetVal = new DataTable("COM");
            RetVal.Columns.Add(new DataColumn("Area_NID", System.Type.GetType("System.Double")));
            RetVal.Columns.Add(new DataColumn("IUSNID", System.Type.GetType("System.Double")));
            RetVal.Columns.Add(new DataColumn("Timeperiod_NID", System.Type.GetType("System.String")));
            RetVal.Columns.Add(new DataColumn("Source_NID", System.Type.GetType("System.Double")));
            RetVal.Columns.Add(new DataColumn("Min_Scale", System.Type.GetType("System.Double")));
            RetVal.Columns.Add(new DataColumn("Max_Scale", System.Type.GetType("System.Double")));

            for (int i = 1; i <= totalTragetColumns; i++)
            {
                RetVal.Columns.Add(new DataColumn("Ind_" + i, System.Type.GetType("System.Double")));
                RetVal.Columns.Add(new DataColumn("Weight_" + i, System.Type.GetType("System.Double")));
                RetVal.Columns.Add(new DataColumn("HighIsGood_" + i, System.Type.GetType("System.Double")));
                RetVal.Columns.Add(new DataColumn("Index_" + i, System.Type.GetType("System.Double")));
            }

            RetVal.Columns.Add(new DataColumn("Score", System.Type.GetType("System.Double")));
            RetVal.Columns.Add(new DataColumn("CompositeIndex", System.Type.GetType("System.Double")));

            return RetVal;
        }


        #endregion

        #region "-- Common Methods for Aggregate --"

        private double GetTargetQuestionValue(AggregateType aggregateType, ref DataRow row, ref DataTable questionTable, ref string timePeriodNId,
            ref string questionKey, ref string defaultTimePeriodNId, ref int sourceNId, ref int defaultSourceNId, ref int totalRows,
            ref int IUSNId)
        {
            return this.GetTargetQuestionValue(aggregateType, ref row, ref questionTable, ref timePeriodNId,
            ref questionKey, ref defaultTimePeriodNId, ref sourceNId, ref defaultSourceNId, ref totalRows,
            ref  IUSNId, false);
        }

        private double GetTargetQuestionValue(AggregateType aggregateType, ref DataRow row, ref DataTable questionTable, ref string timePeriodNId,
            ref string questionKey, ref string defaultTimePeriodNId, ref int sourceNId, ref int defaultSourceNId, ref int totalRows,
            ref int IUSNId, bool forPercentageQuestionNumerator)
        {


            // get target questions
            double RetVal = 0;
            string IndicatorGid = string.Empty;
            string UnitGid = string.Empty;
            string SubgroupValGid = string.Empty;
            double DataValue = 0;
            string sStringNid = string.Empty;
            string TargetQuestion;
            string AnsType = string.Empty;
            bool ContinueProcess = true;

            try
            {
                TargetQuestion = this.GetTargetQuestion(aggregateType, row["Data_Value"].ToString());

                if (TargetQuestion.Length == 0)
                {
                    ContinueProcess = false;
                }

                if (aggregateType == AggregateType.PERCENT & ContinueProcess)
                {
                    // check given question's aggType is COUNT/SUM 
                    //'( PERC,[COUNT,Q6,4],[COUNT,Q7,4])
                    //'( PERC,[COUNT,Q6,4],[SUM,Q7])
                    //'( PERC,[SUM,Q6],[COUNT,Q7,4])
                    //'( PERC,[SUM,Q6],[SUM,Q7])

                    if (forPercentageQuestionNumerator)             //find QN
                    {
                        TargetQuestion = TargetQuestion.Substring(TargetQuestion.IndexOf("[") + 1).ToString();
                        TargetQuestion = TargetQuestion.Substring(0, TargetQuestion.IndexOf("]"));
                    }
                    else                            //find QD
                    {
                        TargetQuestion = TargetQuestion.Substring(TargetQuestion.IndexOf("],[") + 3).ToString();
                        TargetQuestion = TargetQuestion.Substring(0, TargetQuestion.IndexOf("]"));
                    }

                    // Change Aggregate Type to Count/Sum
                    if (TargetQuestion.StartsWith("COUNT"))
                    {
                        aggregateType = AggregateType.COUNT;
                    }
                    else if (TargetQuestion.StartsWith("SUM"))
                    {
                        aggregateType = AggregateType.SUM;
                    }
                    else
                    {
                        ContinueProcess = false;
                    }

                    // findout target Question
                    if (ContinueProcess)
                    {
                        TargetQuestion = this.GetTargetQuestion(aggregateType, TargetQuestion);

                        if (TargetQuestion.Length == 0)
                        {
                            ContinueProcess = false;
                        }
                    }
                }

                if (ContinueProcess)
                {

                    if (questionTable.Select("Key='" + TargetQuestion + "'").Length > 0)
                    {

                        DataRow drTargetQuestion = questionTable.Select("Key='" + TargetQuestion + "'")[0];
                        AnsType = drTargetQuestion[QuestionTableColumns.AnswerType].ToString();


                        if (aggregateType == AggregateType.COUNT & (AnsType != "TBN" & AnsType != "TB")) // find out the String NID (COUNT,Q6,4)
                        {
                            sStringNid = TargetQuestion.Substring(TargetQuestion.IndexOf(",") + 1).ToString();
                            TargetQuestion = TargetQuestion.Substring(0, TargetQuestion.IndexOf(","));
                        }
                        else
                        {
                            sStringNid = "";
                        }


                        switch (aggregateType)
                        {
                            case AggregateType.SUM:
                                // check Target question is TBN type or not 
                                if (AnsType != "TBN")
                                    ContinueProcess = false;
                                break;

                            case AggregateType.COUNT:
                                // if String ID does not exist then ans type should be "TB" or "TBN"
                                if (sStringNid.Length == 0 & (AnsType != "TBN" & AnsType != "TB"))
                                    ContinueProcess = false;
                                break;

                            case AggregateType.AVG:
                                //check ans type is "TBN" or not
                                if (AnsType != "TBN")
                                    ContinueProcess = false;
                                break;
                        }
                        if (ContinueProcess)
                        {

                            // get the key of Target Question
                            questionKey = drTargetQuestion["Key"].ToString();

                            // get timeperiod and source associated with the Target question
                            timePeriodNId = null;
                            timePeriodNId = this.GetTimeperiodNID(questionTable, questionKey);

                            if (timePeriodNId.ToString().Trim().Length == 0 | timePeriodNId == "0")
                            {
                                timePeriodNId = defaultTimePeriodNId;
                            }

                            sourceNId = 0;
                            sourceNId = this.GetSourceNid(questionTable, questionKey);

                            if (sourceNId <= 0)
                            {
                                sourceNId = defaultSourceNId;
                            }


                            // Get The Gids of the Aggregate question
                            //subgroup
                            if (Microsoft.VisualBasic.Information.IsDBNull(row["S_GID"]))
                            {
                                SubgroupValGid = string.Empty;
                            }
                            else
                            {
                                SubgroupValGid = row["S_Gid"].ToString();
                            }

                            //indicator
                            if (Microsoft.VisualBasic.Information.IsDBNull(row["I_Gid"]))
                            {
                                IndicatorGid = string.Empty;
                            }
                            else
                            {
                                IndicatorGid = row["I_Gid"].ToString();
                            }

                            //unit
                            if (Microsoft.VisualBasic.Information.IsDBNull(row["U_Gid"]))
                            {
                                UnitGid = string.Empty;
                            }
                            else
                            {
                                UnitGid = row["U_Gid"].ToString();
                            }

                            // get IUSNID
                            IUSNId = this.GetIUSNid(IndicatorGid, UnitGid, SubgroupValGid);
                            if (IUSNId <= 0)
                            {
                                ContinueProcess = false;
                            }

                            if (ContinueProcess)
                            {

                                // get the numeric value of Target Question
                                if (!Microsoft.VisualBasic.Information.IsDBNull(drTargetQuestion["Numeric_Value"]) & drTargetQuestion["Numeric_Value"].ToString().Length > 0)
                                {
                                    try
                                    {
                                        DataValue = Convert.ToDouble(drTargetQuestion["Numeric_Value"]);
                                    }
                                    catch (Exception)
                                    {
                                        //skip if numeric value is in string
                                        DataValue = 0;
                                    }
                                }

                                switch (aggregateType)
                                {
                                    case AggregateType.COUNT:
                                        // send data_Value as 1 if following conditions are true


                                        if ((AnsType == "TB" | AnsType == "TBN") & (!Microsoft.VisualBasic.Information.IsDBNull(drTargetQuestion["Numeric_Value"]) & drTargetQuestion["Numeric_Value"].ToString().Length > 0))
                                        {
                                            DataValue = 1;
                                        }
                                        else if (sStringNid == DataValue.ToString())
                                        {
                                            DataValue = 1;
                                        }
                                        else if (AnsType == "CH")
                                        {
                                            string[] sarrVal = Utility.SplitStringNIncludeEmpyValue(drTargetQuestion["Data_Value"].ToString(), ",");
                                            string[] sarrStrIds = Utility.SplitStringNIncludeEmpyValue(drTargetQuestion["Str_ID"].ToString(), ",");
                                            DataValue = 0;

                                            if (sarrVal.Length > 0)
                                            {
                                                // check string ID exits in Str_ID column or not
                                                if (Array.IndexOf(sarrStrIds, sStringNid) >= 0)
                                                {
                                                    DataValue = Convert.ToDouble(sarrVal[Array.IndexOf(sarrStrIds, sStringNid)]);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            DataValue = 0;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                RetVal = DataValue;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

            return RetVal;
        }

        private string GetTargetQuestion(AggregateType aggregateType, string dataValue)
        {
            string RetVal = string.Empty;
            RetVal = dataValue.ToUpper();

            switch (aggregateType)
            {
                case AggregateType.AVG:
                    RetVal = (dataValue.Replace("AVG,", "")).Trim();
                    break;
                case AggregateType.SUM:
                    RetVal = (dataValue.Replace("SUM,", "")).Trim();
                    break;
                case AggregateType.COUNT:
                    RetVal = (dataValue.Replace("COUNT,", "")).Trim();
                    break;
                case AggregateType.PERCENT:
                    RetVal = (dataValue.Replace("PERC,", "")).Trim();
                    break;
            }
            RetVal = RetVal.Replace("'", "");
            RetVal = RetVal.Replace(" ", "");

            return RetVal;
        }

        private void FillXMLDataset(ref DataSet xmlSourceDataset, ref DataTable table, string filename)
        {
            // Checking for Existance of the DataSet 
            if (xmlSourceDataset != null)
            {
                xmlSourceDataset.Dispose();
                xmlSourceDataset = null;
            }

            // New Dataset
            xmlSourceDataset = new DataSet("DevIno5");

            // Read xml file
            // load dataset and datatable
            try
            {
                xmlSourceDataset.ReadXml(filename);

                // Get the DataTable
                table = xmlSourceDataset.Tables[0];
            }
            catch (Exception)
            {
                // If Invalid XMLFile Then Leave it 
                xmlSourceDataset = null;
            }

        }

        #endregion

        #endregion

        #region "-- Database --"

        private string GetTimeperiodNID(DataTable table)
        {
            return this.GetTimeperiodNID(table, string.Empty);
        }

        private string GetTimeperiodNID(DataTable table, string key)
        {
            string RetVal = string.Empty;

            string sTimePeriod_NId = string.Empty;
            string sTime = string.Empty;

            // Time
            // Get DataValue of The Time Period
            try
            {
                if (key.Length == 0)
                {
                    sTime = Convert.ToString(table.Select(QuestionTableColumns.QuestionKey +" = 'TIMEPERIOD'")[0][QuestionTableColumns.DataValue]);
                }
                else
                {
                    sTime = Convert.ToString(table.Select(QuestionTableColumns.QuestionKey +" = '" + key + "'")[0][QuestionTableColumns.TimePeriod]);
                }

                if (!string.IsNullOrEmpty(sTime.Replace(".","").Replace("-","").Trim()))
                {

                    TimeperiodBuilder Timeperiods = new TimeperiodBuilder(this._DBConnection, this._DBQuery);
                    RetVal = Timeperiods.CheckNCreateTimeperiod(sTime).ToString();
                    
                }
            }
            catch (Exception ex)
            {
               // throw new ApplicationException(ex.ToString());
            }

            return RetVal;
        }

        private int GetSourceNid(DataTable table)
        {
            return this.GetSourceNid(table, string.Empty);
        }

        private int GetSourceNid(DataTable table, string key)
        {
            string Source;
            int RetVal = 0;

            //'-- Source
            //'-- Get DataValue of The Source
            try
            {
                if (key.Length == 0)
                {
                    Source = table.Select( QuestionTableColumns.QuestionKey +" = 'SRC_PUBLISHER'")[0][QuestionTableColumns.DataValue].ToString();
                }
                else
                {
                    Source = table.Select(QuestionTableColumns.QuestionKey + " = '" + key + "'")[0][QuestionTableColumns.Source].ToString();
                }

                if (!(string.IsNullOrEmpty(Source)))
                {
                    Source = Utility.RemoveQuotes(Source);

                    SourceBuilder Sources = new SourceBuilder(this._DBConnection, this._DBQuery);
                    RetVal = Sources.CheckNCreateSource(Source);                    
                }
            }
            catch (Exception)
            {
                
            }

            return RetVal;
        }

        private void InsertIUSIUSUM(string indicatorName, string unitName, string subgroupVal)
        {
            this.InsertIUSIUSUM(indicatorName, unitName, subgroupVal, -1, -1, -1, string.Empty);
        }

        private void InsertIUSIUSUM(string indicatorName, string unitName, string subgroupVal,
    int indicatorNId, int unitNId, int subgroupValNId, string sourceName)
        {
            try
            {
                // Insert Into _IUSUM
                this._DBConnection.ExecuteNonQuery(this.EIQueries.InsertIUSUM(indicatorName, unitName, subgroupVal, indicatorNId, unitNId,
                subgroupValNId, sourceName));
            }
            catch (Exception)
            {
                //exception
            }

        }

        private void SaveData(string indicatorName, string unitName, string subgroupName,
 string areaName, string areaNId, string areaId,
  string timePeriod, string timeId, string IUSId,
  string source, string sourceId, string dataValue,
  int footnote, string denominator, int indicatorNid,
  int unitNid, int subgroupNid)
        {
            try
            {

                DataTable Table;
                indicatorName = Utility.RemoveQuotes(indicatorName);

                string sNumDecSep = System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                string sNumGroupDecSep = System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator;

                if (Microsoft.VisualBasic.Information.IsNumeric(dataValue))
                {
                    dataValue = dataValue.Replace(sNumGroupDecSep, "");
                    dataValue = dataValue.Replace(sNumDecSep, ".");
                }



                // checking for Duplicacy
                Table = this._DBConnection.ExecuteDataTable(this.EIQueries.IMP_CheckUpdate_Duplicate_Data(indicatorName, unitName,
                    subgroupName, areaId, timeId, sourceId, string.Empty, string.Empty, string.Empty, true, IUSId));

                if (Table.Rows.Count > 0)     // Data already Exists
                {
                    this._DBConnection.ExecuteNonQuery(this.EIQueries.IMP_CheckUpdate_Duplicate_Data(indicatorName, unitName,
                    subgroupName, areaId, timeId, sourceId, dataValue, footnote.ToString(), denominator, false, IUSId));

                }
                else
                {
                    // Insert the new values
                    this._DBConnection.ExecuteNonQuery(this.EIQueries.Import_Save_Data(EIQuery.eQueryType.Insert,
                    indicatorName, unitName, subgroupName, indicatorNid, unitNid, subgroupNid, timeId, timePeriod,
                    areaNId, areaId, sourceId, source, dataValue, denominator, footnote.ToString(), IUSId));
                }
                Table.Dispose();

            }
            catch (Exception)
            {
                //   showErrorMessage(ex)
            }
        }

        private void SaveDataFromTemp()
        {
            try
            {
                 string IUSIds= string.Empty;                               // String to store the IUS id's

           
                this._DBConnection.ExecuteNonQuery(this.EIQueries.UpdateBulkData());

                // Bulk Update in _Data (Set _Update in Data where matching in Data with _Data)
                this._DBConnection.ExecuteNonQuery(this.EIQueries.updateBulkDataTemp());

                // Bulk Insert
                this._DBConnection.ExecuteNonQuery(this.EIQueries.InsertBulkDataBase());

                foreach (DataRow Row in this._DBConnection.ExecuteDataTable(this.EIQueries.Getvaluesfromdatatable()).Rows)
                {

                    // Check if the Source and IUS combination exists in Ind_class_IUS table

                    IUSIds = this._DBConnection.ExecuteScalarSqlQuery(this.EIQueries.CheckIUSExistanceIUS(Convert.ToInt32(Row["IUSNId"]),
                Convert.ToInt32(Row["Source_NId"]))).ToString();

                    if (IUSIds == null || IUSIds == "0")
                    {
                        // Insert if the combination does not exists
                        this._DBConnection.ExecuteNonQuery(this.EIQueries.InsertSourceIUS_IUS(Convert.ToInt32(Row["IUSNId"]),
                        Convert.ToInt32(Row["Source_NId"])));
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        private int GetIUSNid(string indicatorGid, string unitGid, string subgroupGid)
        {
            int RetVal = -1;
            DataTable IUSTable;
            try
            {
                IUSTable = this._DBConnection.ExecuteDataTable(this.EIQueries.CheckIUSValuesExistsbyGid(indicatorGid, unitGid, subgroupGid));
                if (IUSTable.Rows.Count > 0)
                {
                    RetVal = Convert.ToInt32(IUSTable.Rows[0]["IUSNId"]);
                }
                IUSTable.Dispose();
            }
            catch (Exception)
            {
                RetVal = -1;
            }

            return RetVal;
        }

        private int GetAreaNID(ref DataTable table, ref  int areaParentNid)
        {
            int RetVal = -1;
            string AreaName = string.Empty;
            string AreaGid = string.Empty;
            string AreaID = string.Empty;
            string[] AreaValues = null;   //Array String To Hold Area as 3 fields are send in Area                
            DataTable Table;

            AreaValues = Utility.SplitStringNIncludeEmpyValue(table.Select(QuestionTableColumns.QuestionKey+ " = 'AREA'")[0][QuestionTableColumns.DataValue].ToString(), "{([~])}");

            if (AreaValues.Length < 3)
            {
                RetVal = -1; // If Array of Area has Length of < 3 then return -1
            }
            else
            {
                // Split the Name,Id and Gid
                AreaName = Utility.RemoveQuotes(AreaValues[0]);
                AreaID = Utility.RemoveQuotes(AreaValues[1]);
                AreaGid = Utility.RemoveQuotes(AreaValues[2]);

                //Not applicable because using this library user cant map the unmatched areas
                //// replace area id if area has been mapped 
                //if (gMappedAreas.ContainsKey(sAreaID) Then
                //    sAreaID = gMappedAreas(sAreaID).ToString
                //End If


                // get the Area_Nid  on the basis of AreaID
                Table = this._DBConnection.ExecuteDataTable(this.EIQueries.CompareAreaForAggregate(AreaID, true));
                if (Table.Rows.Count > 0)
                {
                    RetVal = Convert.ToInt32(Table.Rows[0]["Area_NId"]);
                    areaParentNid = Convert.ToInt32(Table.Rows[0]["Area_Parent_NId"]);
                }
                Table.Dispose();
            }

            return RetVal;
        }

        #endregion

        #endregion

        #endregion

        #region"--Public--"

        #region"--Variable/Properties--"

        private DIConnection _DBConnection;
        /// <summary>
        /// Sets object of DIconnection.
        /// </summary>
        public DIConnection DBConnection
        {
            set
            {
                this._DBConnection = value;
            }
        }

        private DIQueries _DBQuery;
        /// <summary>
        /// Sets instance of DIQueries.
        /// </summary>
        public DIQueries DBQuery
        {
            set
            {
                this._DBQuery = value;
            }
        }

        #endregion

        #region "-- New / Dispose --"

        public ElXmlImporter(List<string> xmlFiles, string tempFolderPath)
        {
            this.TempFolderPath = tempFolderPath;
            this.XmlProcessor = new ElXmlProcessor(xmlFiles, this.TempFolderPath);

        }

        #endregion

        #region"--Method--"

        /// <summary>
        /// This method allows you to import data from xml files into DevInfo database. After invoking this method and mapping unmatched elemenets invoke ImportDataValues() method
        /// </summary>
        /// <param name="databaseFileNameWPath"></param>
        public void StartImportProcess(string databaseFileNameWPath)
        {
            string TempDatabaseFileNameWPath = string.Empty;

            //Step 1: Copy the Database to a Temp Location
            this.DatabaseFileNameWPath = databaseFileNameWPath;
            //TempDatabaseFileNameWPath = this.DatabaseFileNameWPath;//this.TempFolderPath + "\\" + Path.GetFileName(databaseFileNameWPath);
            //Utility.CopyFile(databaseFileNameWPath, TempDatabaseFileNameWPath);

            // Step 2: Create connection and set dataset & data prefix
            this.ConnectToDatabase(this.DatabaseFileNameWPath);//TempDatabaseFileNameWPath);

            //Step 3: Process xml files
            this.XmlProcessor.ProcessXmlFiles();

            // Step 4: Create Temp Tables into target database
            this.CreateTempTables();

            // Step 4: Do the following for each xml files(temp files)
            foreach (string TempXmlFilename in this.XmlProcessor.XmlFiles.Values)
            {
                this.ImportFile(TempXmlFilename);
            }

        }

        /// <summary>
        /// This method import all data values from xml files into DevInfo Database. But before calling this method, you have to call StartImportProcess(databasePath) method.
        /// </summary>
        public void ImportDataValues()
        {
            // import data from temp data table
            this.SaveDataFromTemp();

            // Drop tables
            this.DropTempTables();

            // calculate aggregate questions
            this.CalculateValueForAggregateType();

            // Dipsose database objects
            this._DBConnection.Dispose();
            this._DBQuery = null;

            ////Copy the file to desire location
           //Utility.CopyFile(TempDatabaseFileNameWPath, this.DatabaseFileNameWPath);
        }

        /// <summary>
        /// Imports xml files into database. Use this method for silent import process.
        /// </summary>
        /// <param name="databaseFileNameWPath"></param>
        public void ImportXmlFilesIntoDatabase(string databaseFileNameWPath)
        {
            this.StartImportProcess(databaseFileNameWPath);

            this.ImportDataValues();
        }



        #endregion

        #endregion

        #region"--Internal--"

        #region"--Method--"

        #endregion

        #endregion
        
        #region "-- Static --"

        #region "-- Methods --"

        #region "-- Validate XML File--"

        public static bool IsValidXML(string fileNameWPath)
        {
            //Check Existance of mandatory table and questions.
            DataSet QuestionnaireDataset = new DataSet();
            bool RetVal = false;

            try
            {
                QuestionnaireDataset.ReadXml(fileNameWPath, XmlReadMode.ReadSchema);

                //-- xml file always have 3 tables: Question,StringTbl,HeaderTbl
                if (QuestionnaireDataset.Tables.Count > 3)
                {
                    //-- check tbl name
                    if ((QuestionnaireDataset.Tables[0].TableName.ToUpper() == TableNames.QuestionTable.ToUpper()) |
                        (QuestionnaireDataset.Tables[1].TableName.ToUpper() == TableNames.StringTable.ToUpper()) |
                        (QuestionnaireDataset.Tables[2].TableName.ToUpper() == TableNames.HeaderTable.ToUpper()))
                    {
                        //-- check Questions tbl column
                        if ((QuestionnaireDataset.Tables[TableNames.QuestionTable].Columns.Count == 16) |
                        (QuestionnaireDataset.Tables[TableNames.StringTable].Columns.Count == 3) |
                        (QuestionnaireDataset.Tables[TableNames.HeaderTable].Columns.Count == 2))
                        {
                            //-- check default value exists or not: source,timeperiod, area
                            //-- Area
                            if (ElXmlImporter.IsAvailableInQuestionTable(QuestionnaireDataset, MandatoryQuestionsKey.Area))
                            {
                                //-- source
                                if (ElXmlImporter.IsAvailableInQuestionTable(QuestionnaireDataset, MandatoryQuestionsKey.SorucePublisher))
                                {
                                    //-- TIMEPERIOD
                                    RetVal = ElXmlImporter.IsAvailableInQuestionTable(QuestionnaireDataset, MandatoryQuestionsKey.TimePeriod);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                //showErrorMessage(e1);
                return false;
            }
            finally
            {
                QuestionnaireDataset.Dispose();
            }

            return RetVal;
        }

        private static bool IsAvailableInQuestionTable(DataSet questionnarieDataset, string key)
        {
            bool RetVal = false;
            DataRow[] Rows;
            try
            {
                Rows = questionnarieDataset.Tables[TableNames.QuestionTable].Select(QuestionTableColumns.QuestionKey + "='" + key + "'");
                if (Rows.Length > 0)
                {
                    if (Rows[0][QuestionTableColumns.DataValue].ToString().Trim().Length > 0)
                    {
                        RetVal = true;
                    }
                }
            }
            catch (Exception)
            {
                RetVal = false;
            }
            return RetVal;
        }

        #endregion


        #endregion

        #endregion
    }
}
