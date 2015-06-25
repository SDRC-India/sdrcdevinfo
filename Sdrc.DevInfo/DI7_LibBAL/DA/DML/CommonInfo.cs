using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    /// Provides common information about devinfo like nid ,gid ,name etc. 
    /// </summary>
    public class CommonInfo : ICommonInfo
    {
      
        #region "-- Public --"

        #region "-- Variables & Properties --"

        private int _NId=0;
        /// <summary>
        /// Gets or sets the NId
        /// </summary>
        public int Nid
        {
            get { return this._NId; }
            set { this._NId = value;}
        }

        private string _GID=string.Empty;
        /// <summary>
        /// Gets or sets the GID.
        /// </summary>
        public string GID
        {
            get { return this._GID; }
            set { this._GID = value; }
        }

        private string _Name=string.Empty;
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get { return this._Name; }
            set { this._Name = value; }
        }

        private bool _Global;
        /// <summary>
        /// Gets or sets true/false.
        /// </summary>
        public bool Global
        {
            get { return _Global; }
            set { _Global = value; }
        }
	
        #endregion

        
        #endregion
        


       
    }
}
