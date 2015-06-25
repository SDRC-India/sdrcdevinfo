using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.Footnote
{
    /// <summary>
    /// Provides sql queries to delete records
    /// </summary>
    public static class Delete
    {
        /// <summary>
        /// Returns query to delete footnote
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="nids">Comma separated nids which may be blank</param>
        /// <returns></returns>
        public static string DeleteFootnote(string TableName, string nids)
        {
            string RetVal = string.Empty;

            RetVal = "DELETE FROM " + TableName + " where " + DIColumns.FootNotes.FootNoteNId + " <>-1 ";

            if(!string.IsNullOrEmpty(nids))
            {
                RetVal +=" AND "+ DIColumns.FootNotes.FootNoteNId + " IN( " + nids + " )";
            }

            return RetVal;
        }
    }
}
