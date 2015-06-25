using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.Controls.ListViewBAL
{

    /// <summary>
    /// Provides column's information like name, column header,etc.
    /// </summary>
    public class ColumnInfo
    {

        #region "-- Private --"

        #region "-- New/Dispose --"

        private ColumnInfo()
        {
        // don't implement this
        }

        #endregion

        #endregion

        #region "-- Public --"

        #region "-- Variables/Properties --"

        private string _ColumnName = string.Empty;
        /// <summary>
        /// Gets or Sets column name. 
        /// </summary>
        public string ColumnName
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

        private string _ColumnHeader = string.Empty;
        /// <summary>
        /// Gets or sets column header.
        /// </summary>
        public string ColumnHeader
        {
            get
            {
                return this._ColumnHeader;
            }
            set
            {
                this._ColumnHeader = value;
            }
        }

        private string _AssociatedGlobalColumnName = string.Empty;
        /// <summary>
        /// Gets or sets associated global column name
        /// </summary>
        public string AssociatedGlobalColumnName
        {
            get
            {
                return this._AssociatedGlobalColumnName;
            }
            set
            {
                this._AssociatedGlobalColumnName = value;
            }
        }

        #endregion

        #region "-- New/Dispose --"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="columnHeader"></param>
        /// <param name="associatedGlobalColumnName"></param>
        public ColumnInfo(string columnName, string columnHeader, string associatedGlobalColumnName)
        {
            this._ColumnName = columnName;
            this._ColumnHeader = columnHeader;
            this._AssociatedGlobalColumnName = associatedGlobalColumnName;
        }

        #endregion

        #endregion

    }

}
