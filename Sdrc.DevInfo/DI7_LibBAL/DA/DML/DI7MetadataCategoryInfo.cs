using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    /// Provide Metadata Category information like CategoryNId,CategoryName, type and order.
    /// </summary>
    public class DI7MetadataCategoryInfo
    {
        #region "-- Public --"

        #region "-- Variables/Properties --"
       
        private int _CategoryNId=-1;
        /// <summary>
        /// get or set CategoryNId
        /// </summary>
        public int CategoryNId
        {
            get { return this._CategoryNId; }
            set { this._CategoryNId = value; }
        }

        private string _CategoryName = string.Empty;
        /// <summary>
        /// get or set CategoryName
        /// </summary>
        public string CategoryName
        {
            get { return this._CategoryName; }
            set { this._CategoryName = value; }
        }

        private string _CategoryType = string.Empty;
        /// <summary>
        /// get or set CategoryType  – [I-Indicator; A-Area; S-Source]
        /// </summary>
        public string CategoryType
        {
            get { return _CategoryType; }
            set { _CategoryType = value; }
        }

        private int _CategoryOrder;
        /// <summary>
        /// get or set CategoryOrder
        /// </summary>
        public int CategoryOrder
        {
            get { return this._CategoryOrder; }
            set { this._CategoryOrder = value; }
        }

        private string _CategoryGID=string.Empty;
        /// <summary>
        /// Gets or sets  category gid
        /// </summary>
        public string CategoryGID
        {
            get { return this._CategoryGID; }
            set { this._CategoryGID = value; }
        }

        private int _ParentNid=-1;
        /// <summary>
        /// Gets or sets parent nid 
        /// </summary>
        public int ParentNid
        {
            get { return this._ParentNid; }
            set { this._ParentNid = value; }
        }

        
        private bool _IsMandatory=false;
        /// <summary>
        /// Gets or sets true/false. Default is false
        /// </summary>
        public bool IsMandatory
        {
            get { return this._IsMandatory; }
            set { this._IsMandatory = value; }
        }

        private bool _IsPresentational=false;
        /// <summary>
        /// Gets or sets true/false. Default is false
        /// </summary>
        public bool IsPresentational
        {
            get { return this._IsPresentational; }
            set { this._IsPresentational = value; }
        }

        private string _Description=string.Empty;
        /// <summary>
        /// Gets or sets description
        /// </summary>
        public string Description
        {
            get { return this._Description; }
            set { this._Description = value; }
        }

        private Dictionary<string, DI7MetadataCategoryInfo> _SubCategories = new Dictionary<string,DI7MetadataCategoryInfo>();
        /// <summary>
        /// Gets or sets sub categories. Key = Category GId and Value = DI7MetadataCategoryInfo object
        /// </summary>
        public Dictionary<string,DI7MetadataCategoryInfo> SubCategories
        {
            get { return this._SubCategories; }
            set { this._SubCategories = value; }
        }
	
        #endregion

        #endregion

    }
}
