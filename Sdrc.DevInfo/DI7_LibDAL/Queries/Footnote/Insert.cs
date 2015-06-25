using System;
using System.Collections.Generic;
using System.Text;


namespace DevInfo.Lib.DI_LibDAL.Queries.Footnote
{
    /// <summary>
    /// Provides sql queries to insert records
    /// </summary>
    public static class Insert
    {
        #region "-- Private --"

        #region "-- Variables --"

        private const string TableName = "footnote";

        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- Methods --"

        /// <summary>
        /// Insert Footnote into Footnote Table
        /// </summary>
        /// <param name="dataPrefix">DataPrefix like UT_</param>
        /// <param name="languageCode">language code like _en</param>
        /// <param name="footnote">Footnote Name</param>
        /// <param name="footnoteGID">Footnote Gid</param>        
        /// <returns></returns>
        public static string InsertFootnote(string dataPrefix, string languageCode, string footnote, string footnoteGID)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + dataPrefix + Insert.TableName + languageCode + "(" + DIColumns.FootNotes.FootNote + ","
            + DIColumns.FootNotes.FootNoteGId + " ) "
            + " VALUES('" + DIQueries.RemoveQuotesForSqlQuery( footnote) + "','" + DIQueries.RemoveQuotesForSqlQuery(footnoteGID) + "')";

            return RetVal;
        }

        #endregion

        #endregion
    }
}

