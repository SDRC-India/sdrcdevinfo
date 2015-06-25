using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DevInfo.Lib.DI_LibBAL.Controls.TreeSelectionBAL;
namespace DevInfo.Lib.DI_LibBAL.Controls.DISelectionBAL
{
    public abstract class BaseSelection:BaseSelectionSource
    {
        public abstract DataTable GetAllRecordsTable();

        internal protected abstract DataView processDataView(DataView dv);

        public  DataView GetRecordsByNids(string nids)
        {
            DataView RetVal;

            RetVal=base.GetRecordsByNids(nids);
            RetVal=this.processDataView(RetVal);

            return RetVal;
        }
    }
}
