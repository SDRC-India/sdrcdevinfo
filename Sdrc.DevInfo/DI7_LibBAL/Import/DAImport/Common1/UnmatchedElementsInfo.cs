using System;
using System.Data;
using System.Collections.Generic;
using System.Text;


namespace DevInfo.Lib.DI_LibBAL.Import.DAImport.Common
{
    /// <summary>
    /// This class is used to store Unmatched DataTable , Available DataTable, and mapping of unmatched DataRow with Avaialble DataRow.
    /// </summary>
    public class UnmatchedElementsInfo
    {
        private DataTable _UnmatchedElementsTable;
        /// <summary>
        /// Gets or sets unmatachedElements table
        /// </summary>
        public DataTable UnmatchedElementsTable
        {
            get 
            {
                return this._UnmatchedElementsTable; 
            }
            set 
            {
                this._UnmatchedElementsTable = value; 
            }
        }

        private DataTable _AvailableElementsTable;
        /// <summary>
        /// Gets or sets Available elements table
        /// </summary>
        public DataTable AvailableElementsTable
        {
            get
            {
                return this._AvailableElementsTable; 
            }
            set
            {
                this._AvailableElementsTable = value; 
            }
        }

        private Dictionary<string, MappedElementInfo> _MappedElements = new Dictionary<string, MappedElementInfo>();
        /// <summary>
        /// Gets or sets Mapped elements 
        /// </summary>
        public Dictionary<string,MappedElementInfo> MappedElements
        {
            get 
            {
                return this._MappedElements; 
            }
            set 
            {
                this._MappedElements = value;
            }
        }
	

	
    }
}
