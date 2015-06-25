using System;
using System.Collections.Generic;
using System.Text;

using DevInfo.Lib.DI_LibDAL.ExceptionHandler;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;
using Queries = DevInfo.Lib.DI_LibDAL.Queries;
using System.Data;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    /// Helps to create new language and language dependent tables into database.
    /// </summary>
    public class LanguageBuilder
    {
        /// <summary>
        /// Provides the column width for language dependent columns. The size of the column should be  one less than the actual size of the column.
        /// </summary>
        internal static class ColumnWidth
        {
            internal const int AreaName = 59;
            internal const int IndicatorName = 254;
            internal const int UnitName = 127;
            internal const int Subgroup = 127;
            internal const int SubgroupTypeName = 127;
            internal const int SubgroupVal = 254;
            internal const int AssistantTopic = 254;
            internal const int Assistant = 2;
            internal const int AgePeriod = 59;
            internal const int AreaLevel = 254;
            internal const int ICName = -1;
            internal const int NotesClassificationName = 149;
            internal const int NotesName = -1;
        }

        #region "-- Private --"

        #region "-- Variables --"

        private DIConnection DBConnection;
        private DIQueries DBQueries;

        #endregion

        #region "-- Methods --"


        internal void UpdateMissingTextValues(string sourceTableName, string targetTableName)
        {
            string SqlQurey = string.Empty;

            if (sourceTableName == DBQueries.TablesName.Indicator)
            {
                SqlQurey = Queries.Indicator.Update.UpdateMissingTextValues(sourceTableName, targetTableName);
            }
            else if (sourceTableName == DBQueries.TablesName.Unit)
            {
                SqlQurey = Queries.Unit.Update.UpdateMissingTextValues(sourceTableName, targetTableName);
            }
            else if (sourceTableName == DBQueries.TablesName.SubgroupVals)
            {
                SqlQurey = Queries.Subgroup.Update.UpdateMissingTextValuesForSubgroupVal(sourceTableName, targetTableName);
            }
            else if (sourceTableName == DBQueries.TablesName.Subgroup)
            {
                SqlQurey = Queries.Subgroup.Update.UpdateMissingTextValuesForSubgroup(sourceTableName, targetTableName);
            }
            else if (sourceTableName == DBQueries.TablesName.Area)
            {
                SqlQurey = this.DBQueries.Update.Area.UpdateMissingAreaLanguageValues(sourceTableName, targetTableName);
            }
            else if (sourceTableName == DBQueries.TablesName.AreaLevel)
            {
                SqlQurey = this.DBQueries.Update.Area.UpdateMissingAreaLevelLanguageValues(sourceTableName, targetTableName);
            }
            else if (sourceTableName == DBQueries.TablesName.AreaMapMetadata)
            {
                SqlQurey = this.DBQueries.Update.Area.UpdateMissingAreaMapMetadataLanguageValues(sourceTableName, targetTableName);
            }
            else if (sourceTableName == DBQueries.TablesName.AreaFeatureType)
            {
                SqlQurey = this.DBQueries.Update.Area.UpdateMissingAreaFeatureLanguageValues(sourceTableName, targetTableName);
            }
            else if (sourceTableName == DBQueries.TablesName.IndicatorClassifications)
            {
                SqlQurey = Queries.IndicatorClassification.Update.UpdateMissingLanguageValues(sourceTableName, targetTableName);

            }
            else if (sourceTableName == DBQueries.TablesName.RecommendedSources)
            {
                SqlQurey = Queries.RecommendedSources.Update.UpdateMissingTextValues(sourceTableName, targetTableName);
            }


            //insert missing records into database/template
            try
            {
                this.DBConnection.ExecuteNonQuery(SqlQurey);
            }
            catch (Exception ex)
            {

                throw new ApplicationException(ex.ToString());
            }

        }


        private void UpdateTextField(LanguageTableInfo languageTable)
        {
            string SqlQuery = string.Empty;
            try
            {
                if (languageTable.TextColumnSize == -1)
                {
                    SqlQuery = " UPDATE " + languageTable.TableName
                                  + " SET " + languageTable.ColumnName + " = '#' & " + languageTable.ColumnName;
                }
                else
                {
                    SqlQuery = " UPDATE " + languageTable.TableName
                              + " SET " + languageTable.ColumnName + " = '#' &" +
                    " iif( len(" + languageTable.ColumnName + ")>" + languageTable.TextColumnSize + " ,left(" + languageTable.ColumnName + ", " + languageTable.TextColumnSize + " ) ," + languageTable.ColumnName + ") ";
                }

                this.DBConnection.ExecuteNonQuery(SqlQuery);
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        /// <summary>
        /// Create new language table like UT_Indicator_ar,UT_Unit_ar,..etc..
        /// </summary>
        /// <param name="tableName">Table Name</param>
        private void CreateTable(string newTableName, string existingTableName)
        {
            string SqlQuery = string.Empty;

            try
            {
                //query for new language table.
                SqlQuery = " SELECT  * INTO " + newTableName + " FROM " + existingTableName;

                this.DBConnection.ExecuteNonQuery(SqlQuery);
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        #endregion

        #endregion

        #region "-- Public --"

        #region "-- New/Dipose --"

        public LanguageBuilder(DIConnection connection, DIQueries queries)
        {
            this.DBConnection = connection;
            this.DBQueries = queries;
        }

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Create new language & language dependent tables like UT_Indicator_ar,UT_Unit_ar,..etc.
        /// </summary>
        /// <param name="languageCode">Language Code like en</param>
        /// <param name="languageName">Language name like DI_English [en]</param>
        /// <param name="dataPrefix">Dataset prefix like UT</param>
        public void CreateNewLanguageTables(string languageCode, string languageName, string dataPrefix)
        {

            Dictionary<string, LanguageTableInfo> LanguageTables = new Dictionary<string, LanguageTableInfo>();

            try
            {
                if (!this.DBConnection.IsValidDILanguage(dataPrefix, languageCode))
                {
                    DITables ExistingTableNames = new DITables(dataPrefix, this.DBConnection.DILanguageCodeDefault(dataPrefix));
                    DITables NewTableNames = new DITables(dataPrefix, "_" + languageCode);

                    //create collection for new language tables 
                    //add table name, text field column and column width
                    LanguageTables.Add(ExistingTableNames.Area, new LanguageTableInfo(NewTableNames.Area, Area.AreaName, ColumnWidth.AreaName));
                    LanguageTables.Add(ExistingTableNames.Indicator, new LanguageTableInfo(NewTableNames.Indicator, Indicator.IndicatorName, ColumnWidth.IndicatorName));
                    LanguageTables.Add(ExistingTableNames.Unit, new LanguageTableInfo(NewTableNames.Unit, Unit.UnitName, ColumnWidth.UnitName));
                    LanguageTables.Add(ExistingTableNames.Subgroup, new LanguageTableInfo(NewTableNames.Subgroup, Subgroup.SubgroupName, ColumnWidth.Subgroup));
                    LanguageTables.Add(ExistingTableNames.SubgroupVals, new LanguageTableInfo(NewTableNames.SubgroupVals, SubgroupVals.SubgroupVal, ColumnWidth.SubgroupVal));
                    LanguageTables.Add(ExistingTableNames.AgePeriod, new LanguageTableInfo(NewTableNames.AgePeriod, AgePeriods.AgePeriod, ColumnWidth.AgePeriod));
                    LanguageTables.Add(ExistingTableNames.AreaLevel, new LanguageTableInfo(NewTableNames.AreaLevel, Area_Level.AreaLevelName, ColumnWidth.AreaLevel));
                    LanguageTables.Add(ExistingTableNames.Assistant, new LanguageTableInfo(NewTableNames.Assistant, Assistant.AssistantType, ColumnWidth.Assistant));
                    LanguageTables.Add(ExistingTableNames.AssistantTopic, new LanguageTableInfo(NewTableNames.AssistantTopic, Assistant_Topic.TopicName, ColumnWidth.AssistantTopic));
                    LanguageTables.Add(ExistingTableNames.FootNote, new LanguageTableInfo(NewTableNames.FootNote, FootNotes.FootNote, -1));
                    LanguageTables.Add(ExistingTableNames.IndicatorClassifications, new LanguageTableInfo(NewTableNames.IndicatorClassifications, IndicatorClassifications.ICName, ColumnWidth.ICName));
                    LanguageTables.Add(ExistingTableNames.NotesClassification, new LanguageTableInfo(NewTableNames.NotesClassification, Notes_Classification.ClassificationName, ColumnWidth.NotesClassificationName));
                    LanguageTables.Add(ExistingTableNames.Notes, new LanguageTableInfo(NewTableNames.Notes, Notes.Note, ColumnWidth.NotesName));

                    LanguageTables.Add(ExistingTableNames.AssistanteBook, new LanguageTableInfo(NewTableNames.AssistanteBook, string.Empty, -1));
                    LanguageTables.Add(ExistingTableNames.AreaFeatureType, new LanguageTableInfo(NewTableNames.AreaFeatureType, string.Empty, -1));
                    LanguageTables.Add(ExistingTableNames.AreaMapMetadata, new LanguageTableInfo(NewTableNames.AreaMapMetadata, string.Empty, -1));

                    // subgroup_type
                    LanguageTables.Add(ExistingTableNames.SubgroupType, new LanguageTableInfo(NewTableNames.SubgroupType, SubgroupTypes.SubgroupTypeName, ColumnWidth.SubgroupTypeName));

                    // add DBMetaData table
                    LanguageTables.Add(ExistingTableNames.DBMetadata, new LanguageTableInfo(NewTableNames.DBMetadata, DBMetaData.Description, -1));

                    // add recommended sources table
                    LanguageTables.Add(ExistingTableNames.RecommendedSources, new LanguageTableInfo(NewTableNames.RecommendedSources, string.Empty, -1));

                    // add metadata category table
                    LanguageTables.Add(ExistingTableNames.MetadataCategory, new LanguageTableInfo(NewTableNames.MetadataCategory, string.Empty, -1));

                    // add metadata report table
                    LanguageTables.Add(ExistingTableNames.MetadataReport, new LanguageTableInfo(NewTableNames.MetadataReport, string.Empty, -1));

                    //Insert new Language record into Language table.
                    this.DBConnection.ExecuteNonQuery(DIQueries.InsertLanguage(dataPrefix, languageName, languageCode, false, false));

                    //create new langauge table and update text field column  according to column width.
                    foreach (string ExistingTableName in LanguageTables.Keys)
                    {

                        this.CreateTable(LanguageTables[ExistingTableName].TableName, ExistingTableName);

                        if (!string.IsNullOrEmpty(LanguageTables[ExistingTableName].ColumnName))
                        {
                            this.UpdateTextField(LanguageTables[ExistingTableName]);
                        }
                    }


                }
            }
            catch (Exception ex)
            {

                ExceptionFacade.ThrowException(ex);
            }

        }

        /// <summary>
        /// Inserts missing language values into all language dependent tables.
        /// </summary>
        public void InsertMissingLanguageValues()
        {
            string LanguageCode = string.Empty;
            DITables TableNames;

            foreach (DataRow Row in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
            {
                try
                {
                    LanguageCode = Row[Language.LanguageCode].ToString();

                    // insert all missing language values
                    if (LanguageCode != this.DBConnection.DILanguageCodeDefault(this.DBQueries.DataPrefix).Replace("_", ""))
                    {
                        TableNames = new DITables(this.DBQueries.DataPrefix, "_" + LanguageCode);

                        // INDICATOR
                        this.UpdateMissingTextValues(this.DBQueries.TablesName.Indicator, TableNames.Indicator);
                        // UNIT
                        this.UpdateMissingTextValues(this.DBQueries.TablesName.Unit, TableNames.Unit);

                        // Subgroup Vals
                        this.UpdateMissingTextValues(this.DBQueries.TablesName.SubgroupVals, TableNames.SubgroupVals);
                        // Subgroup
                        this.UpdateMissingTextValues(this.DBQueries.TablesName.Subgroup, TableNames.Subgroup);

                        // AREA
                        this.UpdateMissingTextValues(this.DBQueries.TablesName.Area, TableNames.Area);

                        // AREA_LEVEL
                        this.UpdateMissingTextValues(this.DBQueries.TablesName.AreaLevel, TableNames.AreaLevel);
                        // AREA_MAP_METADATA
                        this.UpdateMissingTextValues(this.DBQueries.TablesName.AreaMapMetadata, TableNames.AreaMapMetadata);
                        // AREA_FEATURE_TYPE
                        this.UpdateMissingTextValues(this.DBQueries.TablesName.AreaFeatureType, TableNames.AreaFeatureType);
                        // UT_Indicator_Classifications_ar
                        this.UpdateMissingTextValues(this.DBQueries.TablesName.IndicatorClassifications, TableNames.IndicatorClassifications);

                        // UT_RecommendedSources_en
                        this.UpdateMissingTextValues(this.DBQueries.TablesName.RecommendedSources, TableNames.RecommendedSources);
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ex.ToString());
                }
            }


        }

        /// <summary>
        /// Drops the language dependent tables for the given dataprefix and languagecode
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="languageCode"></param>
        public  void DropLanguageDependentTables(string dataPrefix, string languageCode)
        {
            DITables Tables = new DITables(dataPrefix, languageCode);
            string SqlQuery =string.Empty;
            try
            {

                foreach (string tableName in Tables.LanguageDependentTablesName)
                {
                    try
                    {

                        this.DBConnection.DropTable(tableName);
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException(ex.ToString());
                    }
                }               

                // delete language code from language table
                this.DBConnection.ExecuteNonQuery(DIQueries.DeleteLanguageCode(this.DBQueries.TablesName.Language, languageCode.Replace("_","")));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

        }

        #endregion

        #endregion
    }
}
