using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DevInfo.Lib.DI_LibBAL.MergeTemplate
{
    public class MappedRowInfo
    {
        private DataRow _UnmatchedRow;

        public DataRow UnmatchedRow
        {
            get { return this._UnmatchedRow; }
            set { this._UnmatchedRow = value; }
        }

        private DataRow _AvailableRow;

        public DataRow AvailableRow
        {
            get { return this._AvailableRow; }
            set { this._AvailableRow = value; }
        }

        private string  _KeyValue;

        public string  KeyValue
        {
            get { return this._KeyValue; }
            set { this._KeyValue = value; }
        }
	
	
	
    }
}
