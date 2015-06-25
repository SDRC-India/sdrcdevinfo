using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;

namespace DevInfo.Lib.DI_LibDAL.Queries.Notes
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
        /// Get Query to get Notes Data with IUS
        /// </summary>
        /// <returns></returns>
        public string GetNotesDataWithIUS()
        {
            string RetVal = string.Empty;
            
            RetVal = "SELECT DISTINCT D." + DIColumns.Data.IUSNId + ",I." + DIColumns.Indicator.IndicatorNId + ",I."
                + DIColumns.Indicator.IndicatorName + ",U." + DIColumns.Unit.UnitName + ",S." + DIColumns.SubgroupVals.SubgroupVal
                + " FROM " + TablesName.SubgroupVals + " S," + TablesName.Unit + " U," + TablesName.Indicator + " I,"
                + TablesName.IndicatorUnitSubgroup + " IUS," + TablesName.Data + " D," + TablesName.NotesData + " ND"
                + " WHERE  IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + "= D." + DIColumns.Data.IUSNId + " AND D."
                + DIColumns.Data.DataNId + "= ND." + DIColumns.Notes_Data.DataNId + " AND I." + DIColumns.Indicator.IndicatorNId
                + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " and  U." + DIColumns.Unit.UnitNId
                + "= IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " AND S." + DIColumns.SubgroupVals.SubgroupValNId
                + "= IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId;
 
            return RetVal;
        }

        /// <summary>
        /// Get Notes for IUsNid if isIUS is True or by IndicatorNid if isIUS is false and by ProfileNids
        /// </summary>
        /// <param name="profilesNids">Comma seperated ProfilesNids</param>
        /// <param name="iusNid">either IUSNId or IndicatorNID (single nid)</param>
        /// <param name="isIUS">True for IUSNid False for IndicatorNid</param>
        /// <returns>String Query</returns>
        public string GetNotesByProfileNidIUSIndicatorNid(string profilesNids,string iusNid,bool isIUS)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT DISTINCT " + " N." + DIColumns.Notes.NotesNId + ",N." + DIColumns.Notes.Note + ",N."
                + DIColumns.Notes.NotesApproved + ",NP." + DIColumns.Notes_Profile.ProfileName + ",NC."
                + DIColumns.Notes_Classification.ClassificationName);

            sbQuery.Append(" FROM " + TablesName.IndicatorUnitSubgroup + " IUS," + TablesName.Data + " D," + TablesName.NotesProfile + " NP," + TablesName.NotesClassification + " NC," + TablesName.Notes + " N," + TablesName.NotesData + " ND ");

            sbQuery.Append(" WHERE  NC." + DIColumns.Notes_Classification.ClassificationNId + "= N." + DIColumns.Notes.ClassificationNId
                + " AND N." + DIColumns.Notes.NotesNId + "= ND." + DIColumns.Notes_Data.NotesNId + " AND NP."
                + DIColumns.Notes_Profile.ProfileNId + " = N." + DIColumns.Notes.NotesNId + " AND  D." + DIColumns.Data.DataNId + "= ND."
                + DIColumns.Notes_Data.DataNId + " AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + "= D." + DIColumns.Data.IUSNId);
                        

            if (!string.IsNullOrEmpty(profilesNids) && !string.IsNullOrEmpty(iusNid))
            {
                if (isIUS )
                     sbQuery.Append(" AND IUS."+ DIColumns.Indicator_Unit_Subgroup.IUSNId + "=" + iusNid + " AND N." 
                            + DIColumns.Notes.ProfileNId +" in (" + profilesNids + ")");
                else
                    sbQuery.Append(" AND IUS." + Indicator_Unit_Subgroup.IndicatorNId + "=" + iusNid + " AND N." + DIColumns.Notes.ProfileNId                            + " in (" + profilesNids + ")");
                
            }
            else if(!string.IsNullOrEmpty(iusNid))
            {
                if (isIUS )
                    sbQuery.Append(" AND IUS."+ DIColumns.Indicator_Unit_Subgroup.IUSNId + "=" + iusNid);
                else
                     sbQuery.Append( " AND IUS." + Indicator_Unit_Subgroup.IndicatorNId + "=" + iusNid);
            }
            else if(!string.IsNullOrEmpty(profilesNids))
            {
                sbQuery.Append( " AND N." + DIColumns.Notes.ProfileNId + " in (" + profilesNids + ")");
            }

            RetVal = sbQuery.ToString();
          
            return RetVal;
        }

        /// <summary>
        /// Get Notes Profiles Details
        /// </summary>
        /// <param name="noteProfileTableName">Note_Profile Table Name</param>
        /// <param name="filterType"></param>
        /// <param name="filterText"></param>
        /// <returns></returns>
        public string GetNotesProfiles( FilterFieldType filterType, string filterText)
        {

            string RetVal = string.Empty;

            RetVal = "SELECT " + Notes_Profile.ProfileNId + "," + Notes_Profile.ProfileName + "," + Notes_Profile.ProfileEMail + "," + Notes_Profile.ProfileCountry + "," + Notes_Profile.ProfileOrg + "," + Notes_Profile.ProfileOrgType + " FROM " + this.TablesName.NotesProfile;
            if (!string.IsNullOrEmpty(filterText))
            {

                RetVal += " WHERE ";
                switch (filterType)
                {
                    case FilterFieldType.None:
                        break;
                    case FilterFieldType.NId:
                        RetVal += Notes_Profile.ProfileNId + " in (" + filterText + ")";
                        break;
                    case FilterFieldType.ID:
                        RetVal += Notes_Profile.ProfileNId + " in (" + filterText + ")";
                        break;
                    case FilterFieldType.Name:
                        RetVal += Notes_Profile.ProfileName + " in (" + filterText + ")";
                        break;
                    case FilterFieldType.Type:
                        RetVal += Notes_Profile.ProfileOrgType + " in (" + filterText + ")";
                        break;
                    case FilterFieldType.Search:
                        RetVal += filterText;
                        break;
                    case FilterFieldType.NameNotIn:
                        RetVal += Notes_Profile.ProfileName + " NOT IN (" + filterText + ")";
                        break;
                    default:
                        break;
                }

            }
            return RetVal;
        }

        /// <summary>
        /// Get Notes based on various filter conditions
        /// </summary>
        /// <param name="dataNId">Comma delimited DataNIds which may be NullOrEmpty</param>
        /// <param name="notesNId">Comma delimited NotesNIds which may be NullOrEmpty</param>
        /// <param name="profileNId">Comma delimited ProfileNIds which may be NullOrEmpty</param>
        /// <param name="classificationNId">Comma delimited ClassificationNIds which may be NullOrEmpty</param>
        /// <param name="NotesApproved">NotesApproved status. For most of the cases it shall be CheckedStatus.True</param>
        /// <param name="fieldSelection">Light gets column from Notes Table. Heavy gets additional column for ProfileName, ClassificationName</param>
        /// <returns></returns>
        public string GetNotes(string dataNIds, string notesNIds, string profileNIds, string classificationNIds, CheckedStatus NotesApproved, FieldSelection fieldSelection)
        {
            string RetVal = string.Empty;
            string SelectClause = "SELECT ";
            string FromClause = " FROM ";
            string WhereClause = string.Empty;

            if (!string.IsNullOrEmpty(dataNIds))
            {
                SelectClause += " ND." + DIColumns.Notes_Data.DataNId + ",";
                FromClause += this.TablesName.NotesData + " AS ND " + ",";
            }

            SelectClause += "N." + DIColumns.Notes.NotesNId + ",N." + DIColumns.Notes.ProfileNId + ",N." + DIColumns.Notes.ClassificationNId + "," + DIColumns.Notes.Note + "," + DIColumns.Notes.NotesDateTime + "," + DIColumns.Notes.NotesApproved;
            FromClause += this.TablesName.Notes + " AS N";

            switch (fieldSelection)
            {
                case FieldSelection.NId:
                case FieldSelection.Name:
                case FieldSelection.Light:
                    break;
                case FieldSelection.Heavy:
                    SelectClause += ",NP." + DIColumns.Notes_Profile.ProfileName + ",NC." + DIColumns.Notes_Classification.ClassificationName;
                    FromClause += "," + this.TablesName.NotesProfile + " AS NP," + this.TablesName.NotesClassification + " AS NC";
                    break;
            }


            if (!string.IsNullOrEmpty(dataNIds))
            {
                WhereClause = "ND." + DIColumns.Notes_Data.DataNId + " IN (" + dataNIds + ")";
            }


            if (!string.IsNullOrEmpty(notesNIds))
            {
                if (WhereClause.Trim().Length > 0)
                {
                    WhereClause += " AND ";
                }
                WhereClause = "N." + DIColumns.Notes.NotesNId + " IN (" + notesNIds + ")";
            }

            if (!string.IsNullOrEmpty(profileNIds))
            {
                if (WhereClause.Trim().Length > 0)
                {
                    WhereClause += " AND ";
                }
                WhereClause += " NP." + DIColumns.Notes_Profile.ProfileNId + " IN (" + profileNIds + ")";
            }

            if (!string.IsNullOrEmpty(classificationNIds))
            {
                if (WhereClause.Trim().Length > 0)
                {
                    WhereClause += " AND ";
                }
                WhereClause += " NC." + DIColumns.Notes_Classification.ClassificationNId + " IN (" + classificationNIds + ")";
            }

            if (NotesApproved != CheckedStatus.All)
            {
                if (WhereClause.Trim().Length > 0)
                {
                    WhereClause += " AND ";
                }
                switch (NotesApproved)
                {
                    //Use 0 based logic for boolean fields as this syntax works on all databases
                    case CheckedStatus.False:
                        WhereClause += " N." + DIColumns.Notes.NotesApproved + " = 0";
                        break;
                    case CheckedStatus.True:
                        WhereClause += " N." + DIColumns.Notes.NotesApproved + " <> 0";
                        break;
                }
            }

            if (!string.IsNullOrEmpty(dataNIds))
            {
                if (WhereClause.Trim().Length > 0)
                {
                    WhereClause += " AND ";
                }
                WhereClause += " N." + DIColumns.Notes.NotesNId + " = ND." + DIColumns.Notes_Data.NotesNId;
            }

            if (fieldSelection== FieldSelection.Heavy)
            {
                if (WhereClause.Trim().Length > 0)
                {
                    WhereClause += " AND ";
                }
                WhereClause += " N." + DIColumns.Notes.ClassificationNId + " = NC." + DIColumns.Notes_Classification.ClassificationNId;
                WhereClause += " AND N." + DIColumns.Notes.ProfileNId + " = NP." + DIColumns.Notes_Profile.ProfileNId;
            }

            RetVal = SelectClause + FromClause;
            if (WhereClause.Trim().Length > 0)
            {
                RetVal += " WHERE " + WhereClause;
            }

            return RetVal;
        }

        /// <summary>
        /// Gets Notes_Data records for given Notes and/or Data filter
        /// </summary>
        /// <param name="notesNId">Comma delimited NotesNId which may be blank</param>
        /// <param name="dataNId">Comma delimited DataNId which may be blank</param>
        /// <param name="NotesApproved">Whether to consider all notes or approved notes only. In case of DXComments all notes will be considered whereelse in case of UI only approved notes will be considered</param>
        /// <returns></returns>
        public string GetNotes_Data(string notesNId, string dataNId, CheckedStatus NotesApproved)
        {
            return this.GetNotes_Data(notesNId, dataNId, string.Empty, string.Empty, NotesApproved);
        }

        /// <summary>
        /// Gets Notes_Data records for given Notes / Data / Profile / ClassificationType filter
        /// </summary>
        /// <param name="notesNId">Comma delimited NotesNId which may be blank</param>
        /// <param name="dataNId">Comma delimited DataNId which may be blank</param>
        /// <param name="profileNIds">Comma delimited ProfileNId which may be blank</param>
        /// <param name="classificationNIds">Comma delimited Classification NId which may be blank</param>
        /// <param name="NotesApproved">Whether to consider all notes or approved notes only. In case of DXComments all notes will be considered whereelse in case of UI only approved notes will be considered</param>
        /// <returns></returns>
        public string GetNotes_Data(string notesNId, string dataNId, string profileNIds, string classificationNIds, CheckedStatus NotesApproved)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT ND." + DIColumns.Notes_Data.NotesDataNId + ",ND." + DIColumns.Notes_Data.NotesNId + ",ND." + DIColumns.Notes_Data.DataNId);
            sbQuery.Append(" FROM " + this.TablesName.NotesData + " AS ND," + this.TablesName.Notes + " AS N");

            sbQuery.Append(" WHERE ND." + DIColumns.Notes_Data.NotesNId + " = N." + DIColumns.Notes.NotesNId);


            if (notesNId.Trim().Length > 0)
            {
                sbQuery.Append(" AND ND." + DIColumns.Notes_Data.NotesNId + " IN (" + notesNId + ")");
            }

            if (dataNId.Trim().Length > 0)
            {
                sbQuery.Append(" AND ND." + DIColumns.Notes_Data.DataNId + " IN (" + dataNId + ")");
            }

            if (profileNIds.Trim().Length > 0)
            {
                sbQuery.Append(" AND N." + DIColumns.Notes.ProfileNId + " IN (" + profileNIds + ")");
            }

            if (classificationNIds.Trim().Length > 0)
            {
                sbQuery.Append(" AND N." + DIColumns.Notes.ClassificationNId + " IN (" + classificationNIds + ")");
            }

            switch (NotesApproved)
            {
                case CheckedStatus.All:
                    //All records to be considered ignore where clause
                    break;
                case CheckedStatus.False:
                    sbQuery.Append(" AND N." + DIColumns.Notes.NotesApproved + " = 0");
                    break;
                case CheckedStatus.True:
                    sbQuery.Append(" AND N." + DIColumns.Notes.NotesApproved + " <> 0");
                    break;
            }

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get Query to Get NotesData for Notes_Data_Nid
        /// </summary>
        /// <param name="notesData_NIds">Notes_Data_Nid field of Notes_Data Table</param>
        /// <returns></returns>
        public string GetNoteData(string notesData_NIds)
        { 
            string RetVal=string.Empty;

            RetVal = "SELECT DISTINCT("+ DIColumns.Notes_Data.NotesDataNId + "," + DIColumns.Notes_Data.NotesNId + "," + DIColumns.Notes_Data.DataNId + ")  FROM " + this.TablesName.NotesData + " WHERE(" + DIColumns.Notes_Data.NotesDataNId  + " IN (" + notesData_NIds + "))";

                return RetVal;

        }

        /// <summary>
        /// Returns a query to get distinct NotesNid from NotesData table
        /// </summary>
        /// <param name="dataNIds"></param>
        /// <returns></returns>
        public string GetDistinctNotesNidFromNotesData(string dataNIds)
        {
            string RetVal = string.Empty;

            RetVal = "SELECT DISTINCT("+ DIColumns.Notes_Data.NotesNId+")  FROM " + this.TablesName.NotesData +
                " WHERE("+ DIColumns.Notes_Data.DataNId+" IN (" +dataNIds + "))";

            return RetVal;
        }

        /// <summary>
        /// Returns a query to get NotesNid with count from Notes_Data table
        /// </summary>
        /// <param name="notesNIds"></param>
        /// <returns></returns>
        public string GetNotesNidWCountFromNotesData(string notesNIds)
        {
        string RetVal= string.Empty;

        RetVal = "Select " + DIColumns.Notes_Data.NotesNId + " FROM " + this.TablesName.NotesData + " WHERE " + DIColumns.Notes_Data.NotesNId + " In (" + notesNIds + "  )" + " GROUP BY " + DIColumns.Notes_Data.NotesNId + " having count(*)>0;";
        return RetVal;

        }


        /// <summary>
        /// Get Notes Related Details for ProfileNId,ClassificationNid,NotesNid and Approved or NonApproved NId
        /// </summary>
        /// <param name="tablePrefix">DataPrefix</param>
        /// <param name="language">language code like "_en"</param>
        /// <param name="msProfile_NId"></param>
        /// <param name="msClassificationNId"></param>
        /// <param name="notesNIds"></param>
        /// <param name="showApprovedNotes">In case of DI UI only approved Notes shall be displayed
        /// In case of DXN all notes shall be displayed but Only user with admin profile shall be able to make changes to Approved status
        /// </param>
        /// <returns></returns>
        public string GetNotesForProfileClassificationNIds(string msProfile_NId, string msClassificationNId, string notesNIds, bool showApprovedNotes)
        {
            string RetVal = string.Empty;

            RetVal = "SELECT NP." + Notes_Profile.ProfileName + ", N." + DIColumns.Notes.NotesDateTime + ",NC." + Notes_Classification.ClassificationName + ",N." + DIColumns.Notes.Note + ",N." + DIColumns.Notes.NotesNId + ",NC." + Notes_Classification.ClassificationNId + ",N." + DIColumns.Notes.ProfileNId + ",N." + DIColumns.Notes.NotesApproved + ",'ShowRelatedDetails'";

            RetVal += " FROM " + this.TablesName.Notes + " as N" + "," + this.TablesName.NotesProfile + " as NP," + this.TablesName.NotesClassification + " as NC";

            RetVal += " WHERE N." + DIColumns.Notes.ProfileNId + " = NP." + Notes_Profile.ProfileNId + " AND N." + Notes_Classification.ClassificationNId + " = NC." + Notes_Classification.ClassificationNId;

            //*** Filter for Profile
            if (msProfile_NId != "-1")
                RetVal += " AND N." + DIColumns.Notes.ProfileNId + " = " + msProfile_NId;

            //*** Filter for Comment Type
            if (msClassificationNId != "-1")
                RetVal += " AND N." + Notes_Classification.ClassificationNId + " = " + msClassificationNId;

            //*** Filter for Approved
            // In case of DI UI only approved Notes shall be displayed
            // In case of DXN all notes shall be displayed but Only user with admin profile shall be able to make changes to Approved status
            if (showApprovedNotes)
                RetVal += " AND N." + DIColumns.Notes.NotesApproved + "<> 0";

            //*** Filter for Notes_NId
            if (notesNIds.Length > 0)
                RetVal += " AND N."+ DIColumns.Notes.NotesNId + " IN (" + notesNIds + ")";


            return RetVal;
        }

        /// <summary>
        /// Get Notes_Data Nids
        /// </summary>
        /// <param name="notesDataTableName"></param>
        /// <param name="fieldFilterType"></param>
        /// <param name="filterText"></param>
        /// <returns></returns>
        public string GetNotesDataNids(string notesDataTableName)
        {
            string RetVal = string.Empty;

            RetVal = "SELECT " + Notes_Data.DataNId + "," + Notes_Data.NotesNId + "," + Notes_Data.NotesDataNId + " FROM "
                    + this.TablesName.NotesData;
            return RetVal;
        }

        /// <summary>
        /// Get Notes_Classification either All or for Notes_ClassificationNId 
        /// </summary>
        /// <param name="tablePrefix"></param>
        /// <param name="language">Language Code like "_en"</param>
        /// <returns></returns>
        public string GetAllNotesClassification( string classificationNId)
        {
            string RetVal = string.Empty;

            RetVal = "SELECT " + Notes_Classification.ClassificationNId + "," + Notes_Classification.ClassificationName + " FROM " + this.TablesName.NotesClassification;

            if (!string.IsNullOrEmpty(classificationNId))
            {
                RetVal += " WHERE " + Notes_Classification.ClassificationNId + " in (" + classificationNId + ")"; 
            }

            return RetVal;
        }


        /// <summary>
        /// Get Query to get Notes_ClassificationNId for Notes_Classification_Name
        /// </summary>
        /// <param name="tablePrefix"></param>
        /// <param name="language">Language Code like "_en"</param>
        /// <returns></returns>
        public string GetNotesClassificationBYName(string classificationName)
        {
            string RetVal = string.Empty;

            RetVal = "SELECT " + Notes_Classification.ClassificationNId + "," + Notes_Classification.ClassificationName + " FROM " + this.TablesName.NotesClassification;

            if (!string.IsNullOrEmpty(classificationName))
            {
                RetVal += " WHERE " + Notes_Classification.ClassificationName  + "= '" + classificationName + "'";
            }

            return RetVal;
        }

        /// <summary>
        /// Get Notes Data Notes_Nid for DataNids
        /// </summary>
        /// <param name="tablePrefix">DataPrefix e.g. "UT_"</param>
        /// <param name="dataNids"></param>
        /// <returns></returns>
        public string GetNotesDataBYDataNids( string dataNids)
        {
            string RetVal = string.Empty;

            RetVal = "SELECT DISTINCT ND." + DIColumns.Notes_Data.NotesNId + " FROM " + this.TablesName.NotesData + " AS ND WHERE "+ Notes_Data.DataNId  +" in (" + dataNids + ")";

            return RetVal;

        }

        #endregion

        #endregion

    }
}
