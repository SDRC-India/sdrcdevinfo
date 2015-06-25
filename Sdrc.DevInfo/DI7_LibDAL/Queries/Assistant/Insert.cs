using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;

namespace DevInfo.Lib.DI_LibDAL.Queries.Assistant
{
    /// <summary>
    /// Provides sql queries to insert records
    /// </summary>
    public static class Insert
    {
        #region "-- Public --"

        #region "-- Methods --"

        /// <summary>
        /// Creates Assistant_Ebook table
        /// </summary>
        /// <param name="eBookTableName"></param>
        /// <param name="forOnline"></param>
        /// <returns></returns>
        public static string CreateAssistantEBookTbl(string eBookTableName, bool forOnline)
        {
            string RetVal = string.Empty;

            if (forOnline)
            {
                RetVal = "CREATE TABLE " + eBookTableName + " (" + Assistant_eBook.EbookNId + " int Identity(1,1) primary key," + Assistant_eBook.EBook + " Image)";
            }
            else
            {

                RetVal = "CREATE TABLE " + eBookTableName + " (" + Assistant_eBook.EbookNId + "  counter primary key," + Assistant_eBook.EBook + " OLEObject)";
            }

            return RetVal;
        }

        /// <summary>
        /// Creates Assistant table
        /// </summary>
        /// <param name="assistantTblName"></param>
        /// <param name="forOnlineDB"></param>
        /// <returns></returns>
        public static string CreateAssistantTbl(string assistantTblName, bool forOnlineDB)
        {
            string RetVal = string.Empty;

            if (forOnlineDB)
            {
                RetVal = "CREATE TABLE " + assistantTblName + " (" + DIColumns.Assistant.AssistantNId + " int Identity(1,1) primary key," + DIColumns.Assistant.TopicNId + " int, " + DIColumns.Assistant.AssistantText + "  ntext, " + DIColumns.Assistant.AssistantType + " varchar(3)," + DIColumns.Assistant.AssistantOrder + " int)";
            }
            else
            {
                RetVal = "CREATE TABLE " + assistantTblName + " (" + DIColumns.Assistant.AssistantNId + " counter primary key," + DIColumns.Assistant.TopicNId + " number, " + DIColumns.Assistant.AssistantText + " memo, " + DIColumns.Assistant.AssistantType + " Text(3)," + DIColumns.Assistant.AssistantOrder + " number )";
            }

            return RetVal;
        }

        /// <summary>
        /// Creates Assistant_topic table
        /// </summary>
        /// <param name="assistantTopicTblName"></param>
        /// <param name="forOnlineDB"></param>
        /// <returns></returns>
        public static string CreateAssistantTopicTbl(string assistantTopicTblName, bool forOnlineDB)
        {
            string RetVal = string.Empty;
            if (forOnlineDB)
            {
                RetVal = "CREATE TABLE " + assistantTopicTblName + " (" + DIColumns.Assistant_Topic.TopicNId + " int Identity(1,1) primary key," + DIColumns.Assistant_Topic.TopicName + " varchar(255), " + DIColumns.Assistant_Topic.IndicatorGId + " varchar(255)," + DIColumns.Assistant_Topic.TopicIntro + " ntext)";
            }
            else
            {
                RetVal = "CREATE TABLE " + assistantTopicTblName + " (" + DIColumns.Assistant_Topic.TopicNId + " counter primary key," + DIColumns.Assistant_Topic.TopicName + " text(255), " + DIColumns.Assistant_Topic.IndicatorGId + " Text(255)," + DIColumns.Assistant_Topic.TopicIntro + "  memo)";
            }

            return RetVal;
        }


        #endregion

        #endregion
    }
}
