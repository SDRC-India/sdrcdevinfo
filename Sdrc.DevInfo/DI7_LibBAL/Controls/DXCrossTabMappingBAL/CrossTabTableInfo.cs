using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DevInfo.Lib.DI_LibBAL.Controls.MappingControlsBAL;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibBAL.SerializeClasses;
using DevInfo.Lib.DI_LibBAL.LogFiles;
using DevInfo.Lib.DI_LibBAL.DES;

namespace DevInfo.Lib.DI_LibBAL.Controls.DXCrossTabMappingBAL
{
    /// <summary>
    /// Provides Cross tab table information
    /// </summary>
    public class CrossTabTableInfo
    {
        #region "-- Private  --"

        #region "-- Variables --"


        #endregion

        #region "-- New/Dispose --"


        #endregion

        #region "-- Methods  --"



        #region "-- LogFile --"

        private void WriteUnmatchedElementsIntoLogFile(CrossTabLogFile logFile, UnMatchedElementInfo unmatchedElement)
        {
            //Write unmatched elements into log
            if (unmatchedElement != null)
            {
                //area
                foreach (string Key in unmatchedElement.Areas.Keys)
                {
                    logFile.AddUnmatchedArea(this._Caption, Key, unmatchedElement.Areas[Key]);
                }

                // indicator
                foreach (string IndicatorKey in unmatchedElement.Indicators.Keys)
                {
                    logFile.AddUnmatchedIndicator(this._Caption, unmatchedElement.Indicators[IndicatorKey], IndicatorKey);
                }

                // unit
                foreach (string UnitKey in unmatchedElement.Units.Keys)
                {
                    logFile.AddUnmatchedUnit(this._Caption, unmatchedElement.Units[UnitKey], UnitKey);
                }

                // Subgroup
                foreach (string SGKey in unmatchedElement.Subgroups.Keys)
                {
                    logFile.AddUnmatchedSubgroup(this._Caption, unmatchedElement.Subgroups[SGKey], SGKey);
                }
            }

        }

        private void WriteUnmappedInfoIntoLogFile(CrossTabLogFile logFile, Mapping mappedValues, string cellValue)
        {
            if (!string.IsNullOrEmpty(cellValue))
            {
                logFile.AddUnMappedCells(this._Caption, cellValue,
                    string.IsNullOrEmpty(mappedValues.IndicatorGID),
    string.IsNullOrEmpty(mappedValues.UnitGID),
    string.IsNullOrEmpty(mappedValues.SubgroupValGID),
    string.IsNullOrEmpty(mappedValues.AreaID),
    string.IsNullOrEmpty(mappedValues.Timeperiod),
    string.IsNullOrEmpty(mappedValues.Source));
            }

        }

        #endregion

        private void InitializeColumnsMapping()
        {

            if (this._ColumnsHeaderTable != null)
            {

                this._ColumnsMapping = new SerializableDictionary<int, CrossTabMapping>();
                for (int ColIndex = 0; ColIndex < this._ColumnsHeaderTable.Columns.Count; ColIndex++)
                {
                    this._ColumnsMapping.Add(ColIndex, new CrossTabMapping(ColIndex));
                }

            }
        }

        private DataTable CreateBlankRowsTable()
        {
            DataTable RetVal = null;

            if (this._RowsHeaderTable != null)
            {
                RetVal = new DataTable("MappedRowHeaderTable");
                RetVal.Columns.Add();
                for (int i = 0; i < this._RowsHeaderTable.Rows.Count; i++)
                {
                    RetVal.Rows.Add(RetVal.NewRow());
                }


            }

            return RetVal;
        }

        private DataTable CreateBlankColumnsTable()
        {
            DataTable RetVal = null;

            if (this._ColumnsHeaderTable != null)
            {
                RetVal = this._ColumnsHeaderTable.Clone();
                RetVal.TableName = "MappedColumnHeaderTable";
                RetVal.Rows.Add(RetVal.NewRow());
            }

            return RetVal;
        }

        private DataTable CreateDenominatorTable()
        {
            DataTable RetVal;
            RetVal = new DataTable("DenominatorTable");
            RetVal.Columns.Add(new DataColumn(DenominatorColumns.DenominatorColumn));
            RetVal.Columns.Add(new DataColumn(DenominatorColumns.AppliedColumn));
            return RetVal;
        }

        private void InitializeRowsMapping()
        {
            if (this._RowsHeaderTable != null)
            {
                this._RowsMapping = new SerializableDictionary<int, CrossTabMapping>();
                for (int RowIndex = 0; RowIndex < this._RowsHeaderTable.Rows.Count; RowIndex++)
                {
                    this._RowsMapping.Add(RowIndex, new CrossTabMapping(RowIndex));
                }

            }
        }

        private void AddSubgroupNIds(DI6SubgroupBuilder DI6SGBuilder, SerializableDictionary<string, DI6SubgroupInfo> subgroups, List<string> SGNIds)
        {
            int SGNId = -1;

            foreach (DI6SubgroupInfo SGInfo in subgroups.Values)
            {
                SGNId = DI6SGBuilder.GetSubgroupNid(SGInfo.GID, string.Empty);
                if (SGNId > 0 && SGNIds.Contains(SGNId.ToString()) == false)
                {
                    SGNIds.Add(SGNId.ToString());
                }
            }
        }

        #endregion

        #endregion

        #region "-- Public --"

        #region "-- Variables/Properties --"

        private string _ZeroMaskValue = string.Empty;
        /// <summary>
        /// Gets or sets ZeroMaskValue. May be empty or null. Zero as a datavalue will be inserted if the given zeroMaskValue is found while inserting or updating data rows.
        /// </summary>
        public string ZeroMaskValue
        {
            get { return this._ZeroMaskValue; }
            set { this._ZeroMaskValue = value; }
        }

        private string _MissingValueCharacter= ".";
        /// <summary>
        /// Gets or sets MissingValueCharacter. May be empty or null. If datavalue is equal to the given MissingValueCharacter then it will not be imported into data table.Default Value is "."
        /// </summary>
        public string MissingValueCharacter
        {
            get { return this._MissingValueCharacter; }
            set { this._MissingValueCharacter = value; }
        }


        private string _Caption = string.Empty;
        /// <summary>
        /// Gets or sets 
        /// </summary>
        public string Caption
        {
            get { return this._Caption; }
            set { this._Caption = value; }
        }

        private DataTable _ColumnsHeaderTable = new DataTable("MappedColumnHeaderTable");
        /// <summary>
        /// Gets or sets the column headers table
        /// </summary>
        public DataTable ColumnsHeaderTable
        {
            get { return this._ColumnsHeaderTable; }
            set
            {

                this._ColumnsHeaderTable = value;

                //////// set columns mapping object on the basis of columns header
                //////this.InitializeColumnsMapping();

                //////// create blank columns header table
                //////this._BlankColumnsHeaderTable = this.CreateBlankColumnsTable();
            }
        }

        private DataTable _BlankColumnsHeaderTable = null;
        /// <summary>
        /// Gets blank columns header table which can be used to display mapped values.
        /// </summary>
        public DataTable BlankColumnsHeaderTable
        {
            get
            {
                if (this._BlankColumnsHeaderTable == null)
                {
                    // set columns mapping object on the basis of columns header
                    this.InitializeColumnsMapping();

                    // create blank columns header table
                    this._BlankColumnsHeaderTable = this.CreateBlankColumnsTable();
                }

                return this._BlankColumnsHeaderTable;
            }

        }


        private DataTable _BlankRowsHeaderTable = null;
        /// <summary>
        /// Gets blank rows header table which can be used to display mapped values.
        /// </summary>
        public DataTable BlankRowsHeaderTable
        {
            get
            {
                if (this._BlankRowsHeaderTable == null)
                {
                    // set rows mapping object on the basis of rows header
                    this.InitializeRowsMapping();

                    // create blank rows header table
                    this._BlankRowsHeaderTable = this.CreateBlankRowsTable();
                }

                return this._BlankRowsHeaderTable;
            }

        }

        private SerializableDictionary<int, CrossTabMapping> _ColumnsMapping = null;
        /// <summary>
        /// Gets or sets Columns mapping. Key is column index and value is column mapping
        /// </summary>
        public SerializableDictionary<int, CrossTabMapping> ColumnsMapping
        {
            get
            {
                if (this._ColumnsMapping == null)
                {
                    // set columns mapping object on the basis of columns header
                    this.InitializeColumnsMapping();

                    // create blank columns header table
                    this._BlankColumnsHeaderTable = this.CreateBlankColumnsTable();
                }

                return this._ColumnsMapping;
            }
            set { this._ColumnsMapping = value; }
        }


        private SerializableDictionary<int, CrossTabMapping> _RowsMapping = null;
        /// <summary>
        /// Gets or ses rows mapping. Key is row index and value is row mapping
        /// </summary>
        public SerializableDictionary<int, CrossTabMapping> RowsMapping
        {
            get
            {
                if (this._RowsMapping == null)
                {
                    // set rows mapping object on the basis of rows header
                    this.InitializeRowsMapping();

                    // create blank rows header table
                    this._BlankRowsHeaderTable = this.CreateBlankRowsTable();
                }

                return this._RowsMapping;
            }
            set
            {
                this._RowsMapping = value;
            }

            //////    // set rows mapping object on the basis of rows header
            //////    this.InitializeRowsMapping();

            //////    // create blank rows header table
            //////    this._BlankRowsHeaderTable = this.CreateBlankRowsTable();
            //////}
        }


        private DataTable _RowsHeaderTable = new DataTable("MappedRowHeaderTable");
        /// <summary>
        /// Gets or sets the row headers  table 
        /// </summary>
        public DataTable RowsHeaderTable
        {
            get { return this._RowsHeaderTable; }
            set { this._RowsHeaderTable = value; }
        }


        private DataTable _CompleteCrossTabTable = new DataTable("CrossTabTable");
        /// <summary>
        /// Gets or sets complete cross tab table
        /// </summary>
        public DataTable CompleteCrossTabTable
        {
            get { return this._CompleteCrossTabTable; }
            set { this._CompleteCrossTabTable = value; }
        }

        private DataTable _DenominatorTable = null;
        /// <summary>
        /// Gets or sets denominator table
        /// </summary>
        public DataTable DenominatorTable
        {
            get
            {
                if (this._DenominatorTable == null)
                {
                    this._DenominatorTable = this.CreateDenominatorTable();
                }

                return this._DenominatorTable;
            }
            set { this._DenominatorTable = value; }
        }


        private DataTable _DataValueTable = new DataTable("DataValueTable");
        /// <summary>
        /// Gets or sets the data value table 
        /// </summary>
        public DataTable DataValueTable
        {
            get { return this._DataValueTable; }
            set { this._DataValueTable = value; }
        }

        private Mapping _DefaultMapping = new Mapping();
        /// <summary>
        /// Gets or sets default mapping value
        /// </summary>
        public Mapping DefaultMapping
        {
            get { return this._DefaultMapping; }
            set { this._DefaultMapping = value; }
        }

        private int _DecimalValue = 0;
        /// <summary>
        /// Gets or sets Decimal value
        /// </summary>
        public int DecimalValue
        {
            get { return this._DecimalValue; }
            set
            {
                this._DecimalValue = value;
                this._DefaultMapping.DefaultDecimalValue = this._DecimalValue;
            }
        }


        #endregion

        #region "-- New/Dispose --"

        public CrossTabTableInfo()
        {
        }

        public CrossTabTableInfo(string caption, DataTable columnsHeaderTable, DataTable rowsHeadersTable, DataTable completeCrossTabTable, DataTable dataValueTable)
        {
            this._Caption = caption;
            this._ColumnsHeaderTable = columnsHeaderTable;
            this._RowsHeaderTable = rowsHeadersTable;
            this._CompleteCrossTabTable = completeCrossTabTable;
            this._DataValueTable = dataValueTable;
        }

        #endregion

        #region "-- Methods  --"

        /// <summary>
        /// Updates denominator table
        /// </summary>
        /// <param name="denominatorColIndex"></param>
        /// <param name="appliedColIndexes"></param>
        public void UpdateDenominatorTable(int denominatorColIndex, string appliedColIndexes)
        {
            DataRow NewRow;
            DataTable TempTable;
            String TempAppliedColIndex=string.Empty;

            // update denominator column info
            if (this._DenominatorTable != null)
            {
                // delete already mapped row where denominator column is equal to denominator column index
                foreach (DataRow Row in this._DenominatorTable.Select(DenominatorColumns.DenominatorColumn + " = '" + denominatorColIndex.ToString()+"'"))
                {
                    Row.Delete();
                }

                // delete already mapped row where denominator column is equal to applied column indexes
                foreach (string appliedColIndex in DICommon.SplitString(appliedColIndexes, ","))
                {
                    if (TempAppliedColIndex.Length > 0)
                    {
                        TempAppliedColIndex += ",";
                    }
                    TempAppliedColIndex = "'" + appliedColIndex +"'";
                }

                foreach (DataRow Row1 in this._DenominatorTable.Select(DenominatorColumns.DenominatorColumn + " IN ( " + TempAppliedColIndex + ")"))
                {
                    Row1.Delete();
                }

                // delete already mapped row where applied column is equal to denominator column 
                foreach (DataRow Row2 in this._DenominatorTable.Select(DenominatorColumns.AppliedColumn + " = '" + denominatorColIndex+"'"))
                {
                    Row2.Delete();
                }


                // add denominator column index and applied columns indexes 
                foreach (string appliedColIndex in DICommon.SplitString(appliedColIndexes, ","))
                {
                    NewRow = this._DenominatorTable.NewRow();
                    NewRow[DenominatorColumns.DenominatorColumn] = denominatorColIndex.ToString();
                    NewRow[DenominatorColumns.AppliedColumn] = appliedColIndex;
                    this._DenominatorTable.Rows.Add(NewRow);
                }

                this._DenominatorTable.AcceptChanges();
            }

        }

        /// <summary>
        /// Returns complete column name from columns header table for the given column index
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public string GetCompleteColumnName(int columnIndex)
        {
            string RetVal = string.Empty;

            foreach (DataRow Row in this._ColumnsHeaderTable.Rows)
            {
                if (!string.IsNullOrEmpty(RetVal))
                {
                    RetVal += " ";
                }

                RetVal += Convert.ToString(Row[columnIndex]);
            }

            return RetVal;
        }

        /// <summary>
        /// Returns complete row header name from rows header table for the given row index
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public string GetCompleteRowHeaderName(int rowIndex)
        {
            string RetVal = string.Empty;

            foreach (DataColumn Col in this._RowsHeaderTable.Columns)
            {
                if (!string.IsNullOrEmpty(RetVal))
                {
                    RetVal += " ";
                }

                RetVal = Convert.ToString(this._RowsHeaderTable.Rows[rowIndex][Col]);
            }

            return RetVal;
        }

        /// <summary>
        /// Returns cell mapping values
        /// </summary>
        /// <param name="selectedTableInfo"></param>
        /// <param name="rowIndex"></param>
        /// <param name="colIndex"></param>
        /// <returns></returns>
        public Mapping GetCellMapping(int rowIndex, int colIndex, Database dbDatabase)
        {
            Mapping RetVal = null;

            Mapping ColumnMapping;
            Mapping RowMapping;
            int SGValNID = -1;
            List<string> SGNIds = new List<string>();
            DI6SubgroupBuilder DI6SGBuilder;

            try
            {
                // Get mapping information

                if (rowIndex >= 0 && colIndex >= 0)
                {
                    ColumnMapping = this.ColumnsMapping[colIndex].Mappings.CellMap;
                    RowMapping = this.RowsMapping[rowIndex].Mappings.CellMap;

                    // Step1: copy column mapping
                    RetVal = ColumnMapping.Copy();

                    // Step2: set the blank mapping value with the values available in row's mapping
                    RetVal.ReplaceEmptyValues(RowMapping);

                    // Step3: IF SubgoupVal is empty then get subgroupval on the basis of column's subgroups NID + row's subgroups NID
                    if (string.IsNullOrEmpty(RetVal.SubgroupVal))
                    {
                        DI6SGBuilder = new DI6SubgroupBuilder(dbDatabase.DBConnection, dbDatabase.DBQueries);

                        // Step 3a: get subgroup nids(dimensionvalues nid)
                        this.AddSubgroupNIds(DI6SGBuilder, RetVal.Subgroups, SGNIds);
                        this.AddSubgroupNIds(DI6SGBuilder, RowMapping.Subgroups, SGNIds);

                        if (SGNIds.Count > 0)
                        {
                            // Step 3b: get subgroupval for the selected dimensions
                            DI6SubgroupValBuilder SGValBuilder = new DI6SubgroupValBuilder(dbDatabase.DBConnection, dbDatabase.DBQueries);
                            DI6SubgroupValInfo SGValInfo;

                            SGValNID = SGValBuilder.GetSubgroupValNIdBySugbroups(SGNIds);

                            if (SGValNID > 0)
                            {
                                SGValInfo = SGValBuilder.GetSubgroupValInfo(FilterFieldType.NId, SGValNID.ToString());
                                RetVal.SubgroupVal = SGValInfo.Name;
                                RetVal.SubgroupValGID = SGValInfo.GID;
                            }
                        }

                    }

                    // Step 4: set the blank mapping value with the values available in Default mapping
                    RetVal.ReplaceEmptyValues(this._DefaultMapping);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

            return RetVal;
        }

        /// <summary>
        /// Imports mapped data values into database
        /// </summary>
        public void ImportTableValuesIntoDB(Database dbDatabase, CrossTabLogFile logFile)
        {
            Mapping MappedValues = null;
            string DataValue = String.Empty;
            string DenominatorValue = string.Empty;
            UnMatchedElementInfo UnmatchedElement;

            try
            {

                // import datavalues into target database
                for (int RowIndex = 0; RowIndex < this._DataValueTable.Rows.Count; RowIndex++)
                {
                    for (int ColIndex = 0; ColIndex < this._DataValueTable.Columns.Count; ColIndex++)
                    {
                        // dont insert denominator column values
                        if (this.DenominatorTable.Select(DenominatorColumns.DenominatorColumn + "='" + ColIndex +"'").Length == 0)
                        {
                            MappedValues = this.GetCellMapping(RowIndex, ColIndex, dbDatabase);
                            DataValue = Convert.ToString(this._DataValueTable.Rows[RowIndex][ColIndex]);

                            if (!string.IsNullOrEmpty(DataValue) && MappedValues != null && MappedValues.IsVaildMappedValues())
                            {

                                // dont import missingvaluecharacter into data table
                                if (!string.IsNullOrEmpty(this._MissingValueCharacter) && DataValue == this._MissingValueCharacter)
                                {
                                    // dont import missingvaluecharacter into data table
                                }
                                else
                                {
                                    // replace datavalue to zero if datavalue is equal to zero mask value
                                    if (!string.IsNullOrEmpty(this._ZeroMaskValue))
                                    {
                                        if (DataValue == this._ZeroMaskValue)
                                        {
                                            DataValue = "0";
                                        }
                                    }


                                    // round numeric datavalue
                                    if (DataValue != "-" && DataValue != "." && Microsoft.VisualBasic.Information.IsNumeric(DataValue))
                                    {
                                        DataValue = Convert.ToString(Math.Round(Convert.ToDecimal(DataValue), this._DecimalValue));
                                    }


                                    // get denominator value
                                    DenominatorValue = this.GetDenominatorValue(RowIndex, ColIndex);
                                    UnmatchedElement = MappedValues.ImportMappedValuesIntoDB(dbDatabase.DBConnection, dbDatabase.DBQueries, DataValue, DenominatorValue);

                                    this.WriteUnmatchedElementsIntoLogFile(logFile, UnmatchedElement);
                                }


                            }
                            else
                            {
                                // WRITE unmapped information into log file
                                this.WriteUnmappedInfoIntoLogFile(logFile, MappedValues, Convert.ToString(DataValue));
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
        /// Returns denominator value for the given row and column 
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public string GetDenominatorValue(int rowIndex, int columnIndex)
        {
            string RetVal = string.Empty;
            int DenominatorColIndex = -1;

            // get denominator column for the given column index
            foreach (DataRow Row in this._DenominatorTable.Select(DenominatorColumns.AppliedColumn + "='" + columnIndex+"'"))
            {                
                DenominatorColIndex = Convert.ToInt32(Row[DenominatorColumns.DenominatorColumn]);
                
                // get denominator value
                RetVal = Convert.ToString(this._DataValueTable.Rows[rowIndex][DenominatorColIndex]);

                // if value is not numeric 
                if (!Microsoft.VisualBasic.Information.IsNumeric(RetVal))
                {
                    RetVal = string.Empty;
                }
            }

            return RetVal;
        }

        /// <summary>
        /// Sets data tables name . Required for serialization
        /// </summary>
        public void SetAllDataTablesName()
        {
            if (this.DataValueTable != null)
            {
                this.DataValueTable.TableName = TablesName.DATAVALUE_TBLNAME;
            }

            if (this.DenominatorTable != null)
            {
                this.DenominatorTable.TableName = TablesName.DENOMINATOR_TBLNAME;
            }

            if (this.BlankColumnsHeaderTable != null)
            {
                this.BlankColumnsHeaderTable.TableName = TablesName.MAPPEDCOL_TBLNAME;
            }

            if (this.BlankRowsHeaderTable != null)
            {
                this.BlankRowsHeaderTable.TableName = TablesName.MAPPEDROW_TBLNAME;
            }

            if (this.ColumnsHeaderTable != null)
            {
                this.ColumnsHeaderTable.TableName = TablesName.ORGCOL_TBL_NAME;
            }

            if (this.RowsHeaderTable != null)
            {
                this.RowsHeaderTable.TableName = TablesName.ORGROW_TBLNAME;
            }

            if (this.CompleteCrossTabTable != null)
            {
                this.CompleteCrossTabTable.TableName = TablesName.SPO_TBL_NAME;
            }

        }


        /// <summary>
        /// Imports columns mapping 
        /// </summary>
        /// <param name="srcTableInfo"></param>
        public void ImportColumnsMapping(CrossTabTableInfo srcTableInfo)
        {
            string SrcColumnName = string.Empty;
            string TrgColumnName = string.Empty;

            // only if column header table and columns mapping is available in both srctable & current table
            if (this._ColumnsHeaderTable != null && srcTableInfo._ColumnsHeaderTable != null
                && this.ColumnsMapping != null && srcTableInfo.ColumnsMapping != null)
            {

                try
                {
                    for (int SrcColIndex = 0; SrcColIndex < srcTableInfo._ColumnsHeaderTable.Columns.Count; SrcColIndex++)
                    {
                        SrcColumnName = srcTableInfo.GetCompleteColumnName(SrcColIndex);

                        // check this column name is available in current table or not
                        for (int ColIndex = 0; ColIndex < this._ColumnsHeaderTable.Columns.Count; ColIndex++)
                        {
                            TrgColumnName = this.GetCompleteColumnName(ColIndex);

                            // check  src column name and trg column name is same or not
                            if (SrcColumnName.ToUpper().Trim() == TrgColumnName.ToUpper().Trim())
                            {
                                // if same then import mapping values and replace blank values in current mapping
                                if (srcTableInfo.ColumnsMapping.ContainsKey(SrcColIndex))
                                {
                                    // update mapped values
                                    this.ColumnsMapping[ColIndex].Mappings.CellMap.ReplaceEmptyValues(srcTableInfo.ColumnsMapping[SrcColIndex].Mappings.CellMap);

                                    // update mapped values in blankColumnsHeaderTable for display purpose
                                    this.BlankColumnsHeaderTable.Rows[0][ColIndex] = this.ColumnsMapping[ColIndex].MappedValues;

                                }

                                break;
                            }

                        }

                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ex.ToString());
                }
            }

        }

        /// <summary>
        /// Imports rows mapping
        /// </summary>
        /// <param name="srcTableInfo"></param>
        public void ImportRowsMapping(CrossTabTableInfo srcTableInfo)
        {
            string SrcRowName = string.Empty;
            string TrgRowName = string.Empty;

            // only if Row header table and rows mapping is available in both srctable & current table
            if (this._RowsHeaderTable != null && srcTableInfo._RowsHeaderTable != null
                && this.RowsMapping != null && srcTableInfo.RowsMapping != null)
            {

                try
                {
                    for (int SrcRowIndex = 0; SrcRowIndex < srcTableInfo._RowsHeaderTable.Rows.Count; SrcRowIndex++)
                    {
                        SrcRowName = srcTableInfo.GetCompleteRowHeaderName(SrcRowIndex);

                        // check this Row header  is available in current table or not
                        for (int RowIndex = 0; RowIndex < this._RowsHeaderTable.Rows.Count; RowIndex++)
                        {
                            TrgRowName = this.GetCompleteRowHeaderName(RowIndex);

                            // check  src row header and trg header is same or not
                            if (SrcRowName.ToUpper().Trim() == TrgRowName.ToUpper().Trim())
                            {
                                // if same then import mapping values and replace blank values in current mapping
                                if (srcTableInfo.RowsMapping.ContainsKey(SrcRowIndex))
                                {
                                    // update mapped values in mapping object
                                    this.RowsMapping[RowIndex].Mappings.CellMap.ReplaceEmptyValues(srcTableInfo.RowsMapping[SrcRowIndex].Mappings.CellMap);

                                    // update mapped values in BlankRowsHeaderTable for display purpose
                                    this.BlankRowsHeaderTable.Rows[RowIndex][0] = this.RowsMapping[RowIndex].MappedValues;

                                }
                                break;
                            }

                        }

                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ex.ToString());
                }
            }
        }

        /// <summary>
        /// Imports default mapping
        /// </summary>
        /// <param name="srcTableInfo"></param>
        public void ImportDefaultMapping(CrossTabTableInfo srcTableInfo)
        {
            if (this.DefaultMapping != null && srcTableInfo.DefaultMapping != null)
            {
                this.DefaultMapping.ReplaceEmptyValues(srcTableInfo.DefaultMapping);
            }
        }


        #region "-- DES --"


        /// <summary>
        /// Imports mapped data values into data entry spreadsheet
        /// </summary>
        public DataEntrySpreadsheet GetTableValuesIntoDES(Database dbDatabase)
        {
            DataEntrySpreadsheet RetVal;

            Mapping MappedValues = null;
            string DataValue = String.Empty;
            string DenominatorValue = string.Empty;
            UnMatchedElementInfo UnmatchedElement;
            DESValue DESRow;

            try
            {
                // create instance of DES
                RetVal = new DataEntrySpreadsheet();
                               
                // import datavalues into target database
                for (int RowIndex = 0; RowIndex < this._DataValueTable.Rows.Count; RowIndex++)
                {
                    for (int ColIndex = 0; ColIndex < this._DataValueTable.Columns.Count; ColIndex++)
                    {
                        // dont insert denominator column values
                        if (this.DenominatorTable.Select(DenominatorColumns.DenominatorColumn + "='" + ColIndex+"'").Length == 0)
                        {
                            MappedValues = this.GetCellMapping(RowIndex, ColIndex, dbDatabase);
                            DataValue = Convert.ToString(this._DataValueTable.Rows[RowIndex][ColIndex]);

                            if (!string.IsNullOrEmpty(DataValue))
                            {
                                if (DataValue != "-" && DataValue != ".")
                                {
                                    // round datavalue
                                    if (Microsoft.VisualBasic.Information.IsNumeric(DataValue))
                                    {
                                        DataValue = Convert.ToString(Math.Round(Convert.ToDecimal(DataValue), this._DecimalValue));
                                    }
                                }
                            }

                            // get denominator value
                            DenominatorValue = this.GetDenominatorValue(RowIndex, ColIndex);

                            // add values into DES

                            if (MappedValues != null)
                            {
                                DESRow = new DESValue();
                                DESRow.IndicatorGID = MappedValues.IndicatorGID;
                                DESRow.IndicatorName = MappedValues.IndicatorName;
                                DESRow.UnitGID = MappedValues.UnitGID;
                                DESRow.UnitName = MappedValues.UnitName;
                                DESRow.Source = MappedValues.Source;
                                DESRow.SubgroupVal = MappedValues.SubgroupVal;
                                DESRow.SubgroupValGID = MappedValues.SubgroupValGID;
                                DESRow.Timeperiod = MappedValues.Timeperiod;
                                DESRow.AreaID = MappedValues.AreaID;
                                DESRow.AreaName = MappedValues.Area;
                                DESRow.Footnote = MappedValues.Footnote;
                                DESRow.Denominator = DenominatorValue;
                                DESRow.DataValue = DataValue;

                                // add into DES Values
                                RetVal.DESValues.Add(DESRow);
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



        #endregion


        #endregion

        #endregion


    }



}
