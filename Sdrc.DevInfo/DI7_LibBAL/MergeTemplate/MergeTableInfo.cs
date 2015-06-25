using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DevInfo.Lib.DI_LibBAL.MergeTemplate
{
    public class MergeTableInfo
    {
        private DataTable _UnMatchedTable;
        /// <summary>
        /// Get Or Set Unmatched Table
        /// </summary>
        public DataTable UnMatchedTable
        {
            get { return this._UnMatchedTable; }
            set { this._UnMatchedTable = value; }
        }

        private DataTable _AvailableTable;
        /// <summary>
        ///Get or set Available Table
        /// </summary>
        public DataTable AvailableTable
        {
            get { return this._AvailableTable; }
            set { this._AvailableTable = value; }
        }

        private MappedTableInfo  _MappedTable;
        /// <summary>
        /// Get or Set MappedTableInfo for Control Type
        /// </summary>
        public MappedTableInfo  MappedTable
        {
            get { return this._MappedTable; }
            set { this._MappedTable = value; }
        }
	
    }
}
