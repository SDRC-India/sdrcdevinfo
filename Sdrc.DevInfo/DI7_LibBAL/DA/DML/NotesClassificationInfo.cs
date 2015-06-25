using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    public class NotesClassificationInfo
    {
        private int _Classification_NId;
        /// <summary>
        /// Get and Set Notes Classification NId
        /// </summary>
        public int Classification_NId
        {
            get { return this._Classification_NId; }
            set { this._Classification_NId = value; }
        }

        private string _Classification_Name;
        /// <summary>
        /// Get and Set Notes Classification Name
        /// </summary>
        public string Classification_Name
        {
            get { return _Classification_Name; }
            set { _Classification_Name = value; }
        }
	
	
    }
}
