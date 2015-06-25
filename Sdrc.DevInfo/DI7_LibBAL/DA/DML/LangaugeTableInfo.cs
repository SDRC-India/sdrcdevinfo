using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    /// To store information about language depenedent table.
    /// </summary>
    internal class LanguageTableInfo
    {
        #region "-- New / Dispose --"

        internal LanguageTableInfo(string tableName, string columnName, int columnSize)
        {
            this._TableName = tableName;
            this._ColumnName = columnName;
            this._TextColumnSize = columnSize;
        }
        #endregion

        #region "-- Variables/ Properties --"

        private string _TableName = string.Empty;
        /// <summary>
        /// Get or set new language Table name.
        /// </summary>
        internal string TableName
        {
            get
            {
                return this._TableName;
            }
            set
            {
                this._TableName = value;
            }

        }

        private string _ColumnName = string.Empty;
        /// <summary>
        /// Get or set update Column name. 
        /// </summary>
        internal string ColumnName
        {
            get
            {
                return this._ColumnName;
            }
            set
            {
                this._ColumnName = value;
            }
        }

        private int _TextColumnSize = 0;
        /// <summary>
        /// Get or set text column size.
        /// </summary>
        internal int TextColumnSize
        {
            get
            {
                return this._TextColumnSize;
            }
            set
            {
                this._TextColumnSize = value;
            }
        }

        #endregion

    }

}
