using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.Assistant
{
    /// <summary>
    /// Provides sql queries to get records
    /// </summary>
    public class Select
    {
        #region "-- Private --"

        #region "-- Variables --"

        private DITables TablesName;

        #endregion


        #region "-- New / Dispose --"

        private Select()
        {
            // don't implement this method
        }

        #endregion

        #endregion

        #region "-- Public / Internal --"

        #region "-- New / Dispose --"


        internal Select(DITables tablesName)
        {
            this.TablesName = tablesName;
        }

        #endregion


        #region "-- Methods --"

        /// <summary>
        /// Get the Assistant table record.
        /// </summary>
        /// <returns></returns>
        public string GetAssistant()
        {
            StringBuilder RetVal = new StringBuilder();
            try
            {
                RetVal.Append("SELECT A." + DIColumns.Assistant.AssistantNId + ", A." + DIColumns.Assistant.AssistantOrder + ", A." + DIColumns.Assistant.AssistantText + ", A." + DIColumns.Assistant.AssistantText);
                RetVal.Append(", A." + DIColumns.Assistant.AssistantType + ", A." + DIColumns.Assistant.TopicNId);

                RetVal.Append(" FROM " + TablesName.Assistant + " A");
            }
            catch (Exception ex)
            {
                RetVal.Length = 0;
            }
            return RetVal.ToString();
        }


        #region "-- Assistant --"

        #endregion

        #region "-- Assistant Ebook --"

        /// <summary>
        /// Get the Ebook.
        /// </summary>
        /// <returns></returns>
        public string GetAssistant_Ebook()
        {
            StringBuilder RetVal = new StringBuilder();
            try
            {
                RetVal.Append("SELECT E." + DIColumns.Assistant_eBook.EbookNId + ", E." + DIColumns.Assistant_eBook.EBook);

                RetVal.Append(" FROM " + TablesName.AssistanteBook + " E");   
            }
            catch (Exception ex)
            {
                RetVal.Length = 0;
            }
            return RetVal.ToString();
        }

        #endregion

        #endregion

        #endregion

    }
}
