using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DevInfo.Lib.DI_LibBAL.MergeTemplate
{
    /// <summary>
    /// Mapped Table containing MappedRows having (Available and Unmatched Row)
    /// </summary>
    public class MappedTableInfo
    {

        #region "-- private --"
         
        #region "-- Methods --"

        private DataTable CreateMappedTableSchema()
        {
            DataTable RetVal = new DataTable();

            foreach (string ColName in this._TableColumnsInfo)
            {
                RetVal.Columns.Add(ColName);
            }

            return RetVal;

        }

        private DataTable ProcessMappedTable(DataTable mappedTable)
        {
            DataTable RetVal = mappedTable.Clone();
            RetVal.TableName = "MergeTempalte";
            try
            {
                
                foreach (string keyCol in this._MappedRows.Keys)
                {
                    DataRow NewDR = RetVal.NewRow();
                    NewDR.BeginEdit();
                    foreach (string ColName in this._TableColumnsInfo)
                    {
                        if (ColName.StartsWith(MergeTemplate.MergetTemplateConstants.Columns.UNMATCHED_COL_Prefix))
                            NewDR[ColName] = this._MappedRows[keyCol].UnmatchedRow[ColName.Replace(MergeTemplate.MergetTemplateConstants.Columns.UNMATCHED_COL_Prefix, "")];
                        else if (ColName.StartsWith(MergeTemplate.MergetTemplateConstants.Columns.AVAILABLE_COL_Prefix))
                            NewDR[ColName] = this._MappedRows[keyCol].AvailableRow[ColName.Replace(MergeTemplate.MergetTemplateConstants.Columns.AVAILABLE_COL_Prefix, "")];
                       else
                            NewDR[ColName] = this._MappedRows[keyCol].KeyValue;
                    }
                    NewDR.EndEdit();
                    RetVal.Rows.Add(NewDR);
                }
            }
            catch (Exception ex) { throw ex; }

            return RetVal;
        }

        #endregion

        #endregion

        #region "-- Public / Friend --"
      
        #region "-- Variables and Properties --"

        private List<string> _TableColumnsInfo;
        /// <summary>
        /// Get or Set Column List for MappedTable
        /// </summary>
        public List<string> TableColumnsInfo
        {
            get
            {
                return this._TableColumnsInfo;
            }
            set
            {
                this._TableColumnsInfo = value;
            }
        }


        private Dictionary<string, MappedRowInfo> _MappedRows;
        
        /// <summary>
        /// Get or Set MappedRows
        /// </summary>
        public Dictionary<string, MappedRowInfo> MappedRows
        {
            get
            {
                if (this._MappedRows == null)
                    this._MappedRows = new Dictionary<string, MappedRowInfo>();
                return this._MappedRows;
            }
            set
            {
                this._MappedRows = value;
            }
        }

        private DataTable _MappedTable;
        /// <summary>
        ///Get or Set MappedTable
        /// </summary>
        public DataTable MappedTable
        {
            get
            {
                if (this._MappedTable == null)
                    this._MappedTable = this.CreateMappedTableSchema();

                this._MappedTable = this.ProcessMappedTable(this._MappedTable);

                return this._MappedTable;

            }
            set
            {
                this._MappedTable = value;
            }
        }
	
        #endregion

       
        #region "-- Methods --"

        #endregion

        #endregion
       
    }
}
