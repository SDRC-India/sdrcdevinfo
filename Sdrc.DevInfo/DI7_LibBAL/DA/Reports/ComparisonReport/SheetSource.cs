using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.DA.Reports.ComparisonReport
{
    /// <summary>
    /// Base class for generating sheet's missing and additional records & also provide information related to source
    /// </summary>
    internal abstract class SheetSource
    {
                
        #region "-- Private/Proctected --"
                
        #region "-- Methods --"

        /// <summary>
        /// Returns dataTable with S. No. column
        /// </summary>
        /// <returns>DataTable</returns>
        protected DataTable GetTableWithSno(DataTable table)
        {
            DataTable RetVal = new DataTable();
            RetVal.Columns.Add(DILanguage.GetLanguageString("SERIAL_NUMBER"));
            RetVal.Columns[0].AutoIncrement = true;
            RetVal.Columns[0].AutoIncrementSeed = 1;
            RetVal.Merge(table, true, MissingSchemaAction.AddWithKey);
            return RetVal;
        }

        protected abstract void InitializeSheetVariables();

        /// <summary>
        /// Rename Column Names
        /// </summary>
        /// <param name="table"></param>
        protected abstract void RenameColumns(ref DataTable table);

        #endregion

        #endregion

        #region "-- Public / Internal --"

        #region "-- Varibles --"

        internal DIConnection DBConnection;
        internal DIQueries DBQueries;

        internal string ReferenceDBLanguageCode = string.Empty;
        internal string SheetName = string.Empty;

        internal int NameColumnIndex = 0;
        internal int LastColumnIndex = 0;

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Returns Language Name for WorkSheet
        /// </summary>
        /// <returns></returns>
        internal string GetLanguageName()
        {
            string RetVal = string.Empty;
            DataTable Table = null;
            if (this.DBConnection != null)
            {
                try
                {
                    Table = this.DBConnection.DILanguages(this.DBQueries.DataPrefix);
                    DataRow Rows = Table.Select(Language.LanguageCode + "='" + this.DBQueries.LanguageCode.Replace("_","")+ "'")[0];
                    RetVal = Convert.ToString(Rows[Language.LanguageName]);
                }
                catch
                {}
            }
            return RetVal;
        }

        /// <summary>
        /// Get DataTable for Missing Information
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        internal abstract DataTable GetMissingRecordsTable();

        /// <summary>
        /// Get DataTable with Additional Information
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        internal abstract DataTable GetAdditionalRecordsTable();

        #endregion

        #endregion

    }

}
