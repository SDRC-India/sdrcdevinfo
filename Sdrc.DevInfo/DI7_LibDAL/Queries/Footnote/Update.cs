using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.Footnote
{
    /// <summary>
    /// Provides sql queries to update records
    /// </summary>
    public static class Update
    {
        /// <summary>
        /// Return Query to Update Footnote
        /// </summary>
        /// <param name="footNoteTable"></param>
        /// <param name="footNoteNid"></param>
        /// <param name="footnote"></param>
        /// <returns></returns>
        public static string UpdateFootnote(string footNoteTable,int footNoteNid,string footnote)
        {
            string RetVal=string.Empty;

            RetVal = "UPDATE " + footNoteTable + " Set " + DIColumns.FootNotes.FootNote + "= '" + DIQueries.RemoveQuotesForSqlQuery(footnote) + "' WHERE " +  DIColumns.FootNotes.FootNoteNId + " =" + footNoteNid;

            return RetVal;
        }
    }
}
