using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.Import.DAImport.Database
{
    /// <summary>
    /// Provides collection of methods which can be used to import Assistants from database or template
    /// </summary>
   internal static  class AssistantQueries
   {

       #region "-- Internal --"

       #region "-- Methods --"

       internal static string GetAssistantWTopicInfo(string assitantTableName, string assistantTopicTableName)
        {
            return GetAssistantWTopicInfo(assitantTableName, assistantTopicTableName, string.Empty);
        }

        internal static string GetAssistantWTopicInfo(string assitantTableName, string assistantTopicTableName
            , string whereClause)
        {
            string RetVal = string.Empty;
            RetVal = "Select * from " + assitantTableName;
            RetVal = RetVal + " A INNER JOIN " + assistantTopicTableName + " T ON A.Topic_Nid = T.Topic_NId ";

            if (whereClause.Length > 0)
            {
                RetVal = RetVal + whereClause;
            }

            return RetVal;

        }

        internal static string InsertTopicInfo(string tableName, string topicName)
        {
            return  InsertTopicInfo(tableName, topicName, string.Empty, string.Empty);
        }
        internal static string InsertTopicInfo(string tableName, string topicName, string indicatorGID, string topicIntro)
        {
            string RetVal = string.Empty;
            RetVal = "INSERT INTO " + tableName + " ( Topic_Name,Indicator_GID,Topic_Intro ) values ('" + topicName + "', '" + indicatorGID + "' , '" + topicIntro + "')";

            return RetVal;
        }

        internal static string UpdateTopicIntro(string tableName, string topicIntro, string topicName)
        {
            return  UpdateTopicIntro(tableName, topicIntro, topicName, string.Empty, string.Empty);
        }

        internal static string UpdateTopicIntro(string tableName, string topicIntro, string topicName, string indicatorGid)
        {
            return  UpdateTopicIntro(tableName, topicIntro, topicName, indicatorGid, string.Empty);
        }

        internal static string UpdateTopicIntro(string tableName, string topicIntro, string topicName,
            string indicatorGid, string topicNid)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + tableName + " SET  Topic_Intro ='" + topicIntro + "' ";
            if (indicatorGid.Length > 0)
            {
                RetVal = RetVal + " , Indicator_GID='" + indicatorGid + "' ";
            }

            if (topicNid.Length > 0)
            {
                RetVal = RetVal + " where Topic_Nid=" + topicNid;
            }
            else
            {
                RetVal = RetVal + " where Topic_Name='" + topicName + "'";
            }

            return RetVal;
        }

        internal static string GetALLTopics(string tableName)
        {
            return  GetALLTopics(tableName, string.Empty);
        }
        internal static string GetALLTopics(string tableName, string whereClause)
        {
            string RetVal = "Select * from " + tableName;

            if (whereClause.Length > 0)
            {
                RetVal = RetVal + whereClause;
            }

            return RetVal;
        }

        internal static string UpdateBlankTopicRecord(string assistantTableName, string newValue, string type, string NID)
        {
            string RetVal = "UPDATE " + assistantTableName + " SET  Assistant ='" + newValue + "'";

            if (type.Length > 0)
            {
                RetVal = RetVal + ", Assistant_Type='" + type + "' ";
            }

            RetVal = RetVal + " where Assistant_Nid in( " + NID + ")";

            return RetVal;
        }

        internal static string CreateNewAssistantInfo(string assistantTableName, int topicNid,
            string type, string assistantText, string orderNo)
        {
            string RetVal = "INSERT INTO " + assistantTableName + "(Topic_Nid,Assistant_Type,Assistant,Assistant_Order) VALUES (" +
                topicNid + " ,'" + type + "','" + assistantText + "'," + orderNo + ")";
            return RetVal;

        }

        internal static string DeleteFrmEBook(string assistantEBookTableName)
        {

            string RetVal = "Delete from " + assistantEBookTableName;
            return RetVal;
        }

        #endregion

       #endregion
    }
}
