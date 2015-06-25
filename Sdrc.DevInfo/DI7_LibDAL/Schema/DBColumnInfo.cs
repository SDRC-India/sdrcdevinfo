using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Schema
{
    /// <summary>
    /// Provides column size &  name.
    /// </summary>
    public class DbColumnInfo
    {
        #region -- Public --

        #region -- Variables/ Properties --

        private string _ColumnName = string.Empty;
        /// <summary>
        /// Gets or sets column name
        /// </summary>
        public string ColumnName
        {
            get { return this._ColumnName; }
            set { this._ColumnName = value; }
        }

        private long _ColumnSize = 0;
        /// <summary>
        /// Gets or sets column size
        /// </summary>
        public long ColumnSize
        {
            get { return this._ColumnSize; }
            set { this._ColumnSize = value; }
        }
	
        #endregion

        #region -- New/Dipose --

        public DbColumnInfo()
        {
            //'HACK 
        }

        public DbColumnInfo(string columnName, long columnSize)
        {
            this._ColumnName = columnName;
            this._ColumnSize = columnSize;
        }        

        #endregion

        #endregion

    }
}
