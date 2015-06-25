using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    /// Get or Set Sender or Receiver Information
    /// </summary>
    public class SenderInfo
    {
        private string _ID = string.Empty;
        /// <summary>
        /// Get or Set ID
        /// </summary>
        public string ID
        {
            get
            {
                return this._ID;
            }
            set
            {
                this._ID = value;
            }
        }

        private string _Name = string.Empty;
        /// <summary>
        /// Get or Set Name
        /// </summary>
        public string SenderName
        {
            get
            {
                return this._Name;
            }
            set
            {
                this._Name = value;
            }
        }

        private string _ContactName = string.Empty;
        /// <summary>
        /// Get or Set Contact Name
        /// </summary>
        public string ContactName
        {
            get
            {
                return this._ContactName;
            }
            set
            {
                this._ContactName = value;
            }
        }

        private string _Department = string.Empty;
        /// <summary>
        /// Get or Set Department
        /// </summary>
        public string Department
        {
            get
            {
                return this._Department;
            }
            set
            {
                this._Department = value;
            }
        }

        private string _Role = string.Empty;

        public string Role
        {
            get
            {
                return this._Role;
            }
            set
            {
                this._Role = value;
            }
        }

        private string _Email = string.Empty;
        /// <summary>
        /// Get or Set Email
        /// </summary>
        public string Email
        {
            get { return this._Email; }
            set { this._Email = value; }
        }

        private string _Telephone = string.Empty;
        /// <summary>
        /// Get or Set Telephone
        /// </summary>
        public string Telephone
        {
            get
            {return this._Telephone;}
            set
            {this._Telephone = value;}
        }

        private string _Fax = string.Empty;
        /// <summary>
        /// Get or Set Fax
        /// </summary>
        public string Fax
        {
            get
            {return this._Fax;}
            set
            {this._Fax = value;}
        }
    }
}
