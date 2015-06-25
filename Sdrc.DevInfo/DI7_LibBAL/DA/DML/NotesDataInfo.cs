using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
   public  class NotesDataInfo
    {

        private int _Notes_Data_NId;
       /// <summary>
       /// Get and Set Notes_Data Nid
       /// </summary>
        public int Notes_Data_NId
        {
            get { return this._Notes_Data_NId; }
            set { this._Notes_Data_NId = value; }
        }

        private int _Notes_NId;
       /// <summary>
       /// Get and Set Comments NId
       /// </summary>
        public int Notes_NId
        {
            get { return this._Notes_NId; }
            set { this._Notes_NId = value; }
        }

        private int _Data_NId;
       /// <summary>
       /// Get and Set DataValue Nid
       /// </summary>
        public int Data_NId
        {
            get { return this._Data_NId; }
            set { this._Data_NId = value; }
        }
	
    }
}
