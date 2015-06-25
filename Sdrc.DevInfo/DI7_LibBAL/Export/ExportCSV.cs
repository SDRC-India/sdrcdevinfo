using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
namespace DevInfo.Lib.DI_LibBAL.Export
{
    class ExportCSV
    {

        internal static bool ExportDataView(DataView sourceDataView, string outputFileNameWpath)
        {
            bool RetVal = false;
            string cols = string.Empty;
            string rows = string.Empty;
            string delim = ",";
            try
            {
               DataTable _Dt = sourceDataView.ToTable();
               FileStream stream=new FileStream(outputFileNameWpath,FileMode.Create);
               StreamWriter output = new StreamWriter(stream);
                foreach (DataColumn col in _Dt.Columns)
                {
                     cols=cols+col.Caption+delim;
                }
                cols = cols.Trim(',');
                output.Write(cols+Environment.NewLine);
                // write out each data row
               
                foreach (DataRow row in _Dt.Rows)
                {
                    foreach (object value in row.ItemArray)
                    {
                        rows =rows+value.ToString() + delim;
                    }

                    rows = rows.Trim(',');
                   
                    output.Write(rows+Environment.NewLine);
                    rows = "";
                }

                output.Close();
                output.Dispose();
                stream.Close();
                stream.Dispose();

                RetVal = true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }
    }
}
