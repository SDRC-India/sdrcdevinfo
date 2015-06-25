using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Runtime.Serialization.Formatters.Binary;

using DevInfo.Lib.DI_LibDataCapture.Questions;
using DevInfo.Lib.DI_LibDataCapture.Questions.ColumnNames;

namespace DevInfo.Lib.DI_LibDataCapture
{
    [Serializable()]
    public class LanguageString
    {
        /// <summary>
        /// Gets the question key from the interface table
        /// </summary>
        /// <param name="xmlDataSet"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValue(DataSet xmlDataSet, string key)
        {
            // read key from interface table and return value to the calling function
            string RetVal = string.Empty;
            DataRow[] Rows;

            Rows = xmlDataSet.Tables[TableNames.InterfaceTable].Select(InterfaceColumns.InterfaceKey + "='" + key + "'");
            if (Rows.Length > 0)
            {
                RetVal = Rows[0][InterfaceColumns.Translate].ToString();
            }

            return RetVal;
        }
    }
}
