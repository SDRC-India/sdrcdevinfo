using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    public class NotesInfo 
    {
     
        private int _NId;
        /// <summary>
        /// Get and Set Notes_Nid
        /// </summary>
        public int Notes_NId
        {
            get { return this._NId; }
            set { this._NId = value; }
        }

        private int _Profile_NId;
        /// <summary>
        /// Get and set Profile_Nid
        /// </summary>
        public int Profile_NId
        {
            get { return this._Profile_NId; }
            set { this._Profile_NId = value; }
        }

        private int _Classification_NId;
        /// <summary>
        /// Get and Set Notes Classification_NId
        /// </summary>
        public int Classification_NId
        {
            get { return this._Classification_NId; }
            set { this._Classification_NId = value; }
        }

            
        private string _Notes;
        /// <summary>
        /// Get and Set Comments
        /// </summary>
	    public string Notes
	    {
		    get { return this._Notes;}
		    set { this._Notes = value;}
	    }
        
        private string _Notes_DateTime;
        /// <summary>
        /// Get and Set Comeents Date And Time
        /// </summary>
        public string Notes_DateTime
        {
            get { return _Notes_DateTime; }
            set { _Notes_DateTime = value; }
        }


        private int _Notes_Approved;
        /// <summary>
        /// Get and Set Notes Approval
        /// </summary>
        public int Notes_Approved
        {
            get { return this._Notes_Approved;; }
            set { this._Notes_Approved = value; }
        }
	
	
    }
}
