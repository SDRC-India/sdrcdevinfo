using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    /// Provides information about source.
    /// </summary>
    public class SourceInfo : CommonInfo, ISourceInfo
    {
        #region "-- Variables / Properties --"

        private int _ParentNid=0;
        /// <summary>
        /// Gets or sets parent nid
        /// </summary>
        public int ParentNid
        {
            get { return this._ParentNid; }
            set { this._ParentNid = value; }
        }

        private bool _IsGlobal=false;
        /// <summary>
        /// Gets or sets true /false . True if source is global.Default is false. 
        /// </summary>
        public bool Global
        {
            get { return this._IsGlobal; }
            set { this._IsGlobal = value; }
        }

        private string _Info= string.Empty;
        /// <summary>
        /// Gets or sets source metadata info.
        /// </summary>
        public string Info
        {
            get { return this._Info; }
            set { this._Info= value; }
        }

        private string _ISBN = string.Empty;
        /// <summary>
        /// Gets or sets ISBN
        /// </summary>
        public string ISBN
        {
            get { return _ISBN; }
            set { _ISBN = value; }
        }

        private string _Nature = string.Empty;
        /// <summary>
        /// gets or set Nature
        /// </summary>
        public string Nature
        {
            get { return _Nature; }
            set { _Nature = value; }
        }
	
	
        #endregion

        #region "-- Methods --"

        #endregion

       
       
    }
}
