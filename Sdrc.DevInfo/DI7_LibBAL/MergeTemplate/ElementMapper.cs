using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibDAL.ExceptionHandler;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;

namespace DevInfo.Lib.DI_LibBAL.MergeTemplate
{
    public class ElementMapper
    {


        #region "-- private --"


        #region "-- Variables --"

        private DIConnection DBConnection;
        private DIQueries DBQueries = null;
        private MergeTemplateQueries TemplateQueries;
        private Dictionary<TemplateMergeControlType, MergeTableInfo> MappedTables;

        #endregion


        #region "-- Methods --"

        private void ProcessMappedIndicators()
        {
            DIConnection SrcDBConnection = null;
            DIQueries SrcDBQueries = null;
            
            IndicatorBuilder TrgIndicatorBilderObj = null;
            IndicatorBuilder SourceIndicatorBilderObj = null;
            IndicatorInfo SrcIndicatorInfoObj = null;
            DataTable Table = null;
            
            string SourceFileWPath = string.Empty;
            int TrgIndicatorNid = 0;

            try
            {

                if (this.MappedTables.ContainsKey(TemplateMergeControlType.Indicator))
                {
                    TrgIndicatorBilderObj = new IndicatorBuilder(this.DBConnection, this.DBQueries);

                    foreach (DataRow Row in this.MappedTables[TemplateMergeControlType.Indicator].MappedTable.MappedTable.Rows)
                    {
                        Table = this.DBConnection.ExecuteDataTable(TemplateQueries.GetImportIndicator(Convert.ToString(Row[MergetTemplateConstants.Columns.UNMATCHED_COL_Prefix + Indicator.IndicatorNId])));

                        TrgIndicatorNid = Convert.ToInt32(Row[MergetTemplateConstants.Columns.AVAILABLE_COL_Prefix + Indicator.IndicatorNId]);

                        if (Table != null && Table.Rows.Count > 0)
                        {

                            try
                            {
                                SourceFileWPath = Convert.ToString(Table.Rows[0][MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME]);

                                SrcDBConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, SourceFileWPath, string.Empty, string.Empty);
                                SrcDBQueries = DataExchange.GetDBQueries(SrcDBConnection);

                                // Get Source Indicator Info
                                SourceIndicatorBilderObj = new IndicatorBuilder(SrcDBConnection, SrcDBQueries);
                                SrcIndicatorInfoObj = SrcIndicatorInfoObj = SourceIndicatorBilderObj.GetIndicatorInfo(FilterFieldType.NId, Convert.ToString(Table.Rows[0][MergetTemplateConstants.Columns.COLUMN_SRCNID]), FieldSelection.Light);

                                // Import Mapped Indicator Values from Source
                                TrgIndicatorBilderObj.ImportIndicatorFrmMappedIndicator(SrcIndicatorInfoObj, SrcIndicatorInfoObj.Nid, TrgIndicatorNid, SrcDBQueries, SrcDBConnection);
                            }
                            finally
                            {
                                if (SrcDBConnection != null)
                                {
                                    SrcDBConnection.Dispose();
                                    SrcDBQueries.Dispose(); 
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                ExceptionFacade.ThrowException(ex);
            }
        }

        private void ProcessMappedUnits()
        {

            DIConnection SrcDBConnection = null;
            DIQueries SrcDBQueries = null;

            UnitBuilder TrgUnitBilder = null;
            UnitInfo SrcUnitInfoObj = null;
            UnitBuilder SrcUnitBuilderObj = null;
            DataTable Table = null;
            string SourceFileWPath = string.Empty;
            int TrgUnitNid = 0;


            if (this.MappedTables.ContainsKey(TemplateMergeControlType.Unit))
            {
                TrgUnitBilder = new UnitBuilder(this.DBConnection, this.DBQueries);

                foreach (DataRow Row in this.MappedTables[TemplateMergeControlType.Unit].MappedTable.MappedTable.Rows)
                {
                    Table = this.DBConnection.ExecuteDataTable(TemplateQueries.GetImportUnits(Convert.ToString(Row[MergetTemplateConstants.Columns.UNMATCHED_COL_Prefix + Unit.UnitNId])));

                    TrgUnitNid = Convert.ToInt32(Row[MergetTemplateConstants.Columns.AVAILABLE_COL_Prefix + Unit.UnitNId]);

                    if (Table != null && Table.Rows.Count > 0)
                    {
                       
                       
                        SourceFileWPath = Convert.ToString(Table.Rows[0][MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME]);

                        SrcDBConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, SourceFileWPath, string.Empty, string.Empty);
                        SrcDBQueries = DataExchange.GetDBQueries(SrcDBConnection);

                        // get unit info
                        SrcUnitBuilderObj = new UnitBuilder(SrcDBConnection, SrcDBQueries);
                        SrcUnitInfoObj = SrcUnitBuilderObj.GetUnitInfo(FilterFieldType.NId, Convert.ToString(Table.Rows[0][MergetTemplateConstants.Columns.COLUMN_SRCNID]));  
                  
                        // Import Mapped Unit Values
                        TrgUnitBilder.ImportMappedUnitInformation(SrcUnitInfoObj, SrcUnitInfoObj.Nid, TrgUnitNid, SrcDBQueries, SrcDBConnection);

                        SrcDBConnection.Dispose();
                    }

                }
            }
        }

        private void ProcessMappedSubgroupType()
        {

            DIConnection SrcDBConnection = null;
            DIQueries SrcDBQueries = null;

            DI6SubgroupTypeBuilder TrgSubgroupTypeBilder = null;
            DI6SubgroupTypeBuilder SourceSGTypeBuilder = null;
            DI6SubgroupTypeInfo SrcSubgroupTypeInfo = null;
            
            DataTable Table = null;

            string SourceFileWPath = string.Empty;
            int TrgSGDNid = 0;


            if (this.MappedTables.ContainsKey(TemplateMergeControlType.SubgroupDimensions))
            {
                TrgSubgroupTypeBilder = new DI6SubgroupTypeBuilder(this.DBConnection, this.DBQueries);

                foreach (DataRow Row in this.MappedTables[TemplateMergeControlType.SubgroupDimensions].MappedTable.MappedTable.Rows)
                {
                    Table = this.DBConnection.ExecuteDataTable(TemplateQueries.GetImportSubgroupDimensions(Convert.ToString(Row[MergetTemplateConstants.Columns.UNMATCHED_COL_Prefix + SubgroupTypes.SubgroupTypeNId])));

                    TrgSGDNid = Convert.ToInt32(Row[MergetTemplateConstants.Columns.AVAILABLE_COL_Prefix + SubgroupTypes.SubgroupTypeNId]);

                    if (Table != null && Table.Rows.Count > 0)
                    {

                        try
                        {
                            SourceFileWPath = Convert.ToString(Table.Rows[0][MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME]);

                            SrcDBConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, SourceFileWPath, string.Empty, string.Empty);
                            SrcDBQueries = DataExchange.GetDBQueries(SrcDBConnection);

                            // get subgroup type info
                            SourceSGTypeBuilder = new DI6SubgroupTypeBuilder(SrcDBConnection, SrcDBQueries);
                            SrcSubgroupTypeInfo = SourceSGTypeBuilder.GetSubgroupTypeInfoByNid(Convert.ToInt32(Table.Rows[0][MergetTemplateConstants.Columns.COLUMN_SRCNID]));

                            // Import Mapped Subgroup Type Values
                            TrgSubgroupTypeBilder.ImportSubgroupTypeFrmMappedSubgroupType(SrcSubgroupTypeInfo, SrcSubgroupTypeInfo.Nid, TrgSGDNid, SrcDBQueries, SrcDBConnection);

                        }
                        finally
                        {
                            if (SrcDBConnection != null)
                            {
                                SrcDBConnection.Dispose();
                            }
                        }
                        
                    }

                }
            }
        }

        private void ProcessMappedSubgroupDimValues()
        {

            DIConnection SrcDBConnection = null;
            DIQueries SrcDBQueries = null;

            DI6SubgroupBuilder TrgSubgroupsBuilder = null;
            DI6SubgroupBuilder SourceSubgroupBuilder = null;
            DI6SubgroupInfo SrcSubgroupInfo = null;

            DataTable Table = null;
            string SourceFileWPath = string.Empty;
            int TrgSGDVNid = 0;


            if (this.MappedTables.ContainsKey(TemplateMergeControlType.SubgroupDimensionsValue))
            {
                TrgSubgroupsBuilder = new DI6SubgroupBuilder(this.DBConnection, this.DBQueries);

                foreach (DataRow Row in this.MappedTables[TemplateMergeControlType.SubgroupDimensionsValue].MappedTable.MappedTable.Rows)
                {
                    Table = this.DBConnection.ExecuteDataTable(TemplateQueries.GetImportSubgroupDimensionValues(Convert.ToString(Row[MergetTemplateConstants.Columns.UNMATCHED_COL_Prefix + Subgroup.SubgroupNId])));

                    TrgSGDVNid = Convert.ToInt32(Row[MergetTemplateConstants.Columns.AVAILABLE_COL_Prefix + Subgroup.SubgroupNId]);

                    if (Table != null && Table.Rows.Count > 0)
                    {

                        try
                        {
                            SourceFileWPath = Convert.ToString(Table.Rows[0][MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME]);

                            SrcDBConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, SourceFileWPath, string.Empty, string.Empty);
                            SrcDBQueries = DataExchange.GetDBQueries(SrcDBConnection);

                            // Get Subgroup Dimension Values Info
                            SourceSubgroupBuilder = new DI6SubgroupBuilder(SrcDBConnection, SrcDBQueries);
                            SrcSubgroupInfo = SourceSubgroupBuilder.GetSubgroupInfo(FilterFieldType.NId, Convert.ToString(Table.Rows[0][MergetTemplateConstants.Columns.COLUMN_SRCNID]));

                            // Import Mapped Subgroup Dimension Values 
                            TrgSubgroupsBuilder.ImportSubgroupFrmMappedSubgroup(SrcSubgroupInfo, SrcSubgroupInfo.Nid, TrgSGDVNid, SrcDBQueries, SrcDBConnection);
                        }
                        finally
                        {
                            if (SrcDBConnection != null)
                            {
                                SrcDBConnection.Dispose();
                                SrcDBQueries.Dispose();
                            }
                        }
                        
                    }

                }
            }
        }

        private void ProcessMappedSubgroupVals()
        {

            DIConnection SrcDBConnection = null;
            DIQueries SrcDBQueries = null;

            DI6SubgroupValBuilder TrgSubgroupValBuilder = null;
            DI6SubgroupValBuilder SrcSubgroupBuilder = null;
            DI6SubgroupValInfo SrcSubgroupValInfo = null;

            DataTable Table = null;
            string SourceFileWPath = string.Empty;
            int TrgSGNid = 0;

            if (this.MappedTables.ContainsKey(TemplateMergeControlType.Subgroups))
            {
                TrgSubgroupValBuilder = new DI6SubgroupValBuilder(this.DBConnection, this.DBQueries);

                foreach (DataRow Row in this.MappedTables[TemplateMergeControlType.Subgroups].MappedTable.MappedTable.Rows)
                {
                    Table = this.DBConnection.ExecuteDataTable(TemplateQueries.GetImportSubgroupVals(Convert.ToString(Row[MergetTemplateConstants.Columns.UNMATCHED_COL_Prefix + SubgroupVals.SubgroupValNId])));

                    TrgSGNid = Convert.ToInt32(Row[MergetTemplateConstants.Columns.AVAILABLE_COL_Prefix + SubgroupVals.SubgroupValNId]);

                    if (Table != null && Table.Rows.Count > 0)
                    {
                        
                        SourceFileWPath = Convert.ToString(Table.Rows[0][MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME]);

                        SrcDBConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, SourceFileWPath, string.Empty, string.Empty);
                        SrcDBQueries = DataExchange.GetDBQueries(SrcDBConnection);

                        // Get SubgroupVal Info
                        SrcSubgroupBuilder = new DI6SubgroupValBuilder(SrcDBConnection, SrcDBQueries);
                        SrcSubgroupValInfo= SrcSubgroupBuilder.GetSubgroupValInfo(FilterFieldType.NId, Convert.ToString(Table.Rows[0][MergetTemplateConstants.Columns.COLUMN_SRCNID]));

                        // Import Mapped SubgroupVals 
                        TrgSubgroupValBuilder.ImportSubgroupValFrmMappedSubgroupVal(SrcSubgroupValInfo.Nid, TrgSGNid, SrcDBQueries, SrcDBConnection);
                        SrcDBConnection.Dispose();
                    }

                }
            }
        }

        #region "-- Get Info --"

        private IndicatorClassificationInfo GetIndicatorClassificationInfo(DataRow row)
        {
            IndicatorClassificationInfo RetVal;

            try
            {
                // Get unit from source table
                RetVal = new IndicatorClassificationInfo();
                RetVal.Name = DICommon.RemoveQuotes(row[IndicatorClassifications.ICName].ToString());
                RetVal.GID = row[IndicatorClassifications.ICGId].ToString();
                RetVal.IsGlobal = Convert.ToBoolean(row[IndicatorClassifications.ICGlobal]);
                RetVal.ClassificationInfo = DICommon.RemoveQuotes(Convert.ToString(row[IndicatorClassifications.ICInfo]));
                RetVal.Parent = new IndicatorClassificationInfo();
                RetVal.Parent.Nid = Convert.ToInt32(row[IndicatorClassifications.ICParent_NId]);
                RetVal.Nid = Convert.ToInt32(row[IndicatorClassifications.ICNId]);
            }
            catch (Exception ex)
            {
                RetVal = null;
                ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }

        #endregion

        #endregion

        #endregion

        #region "-- Public / Friend --"

        #region "-- Variables and Properties --"

        #endregion

        #region "-- New/Dispose --"

        public ElementMapper(DIConnection dbConnection, DIQueries dbQueries, Dictionary<TemplateMergeControlType, MergeTableInfo> mappedTables)
        {
            this.DBConnection = dbConnection;
            this.DBQueries = dbQueries;
            this.TemplateQueries = new MergeTemplateQueries(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode);
            this.MappedTables = mappedTables;
        }


        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Process Mapped Values for Indicator, Unit, SubgrupType, Subgroup Type Values, SubgroupVals.
        /// </summary>
        public void ProcessMappedIUS()
        {
            this.ProcessMappedIndicators();
            this.ProcessMappedUnits();

            this.ProcessMappedSubgroupType();
            this.ProcessMappedSubgroupDimValues();
            this.ProcessMappedSubgroupVals();

        }

        /// <summary>
        /// Process Mapped IC Values 
        /// </summary>
        /// <param name="importOnlyICIUS"></param>
        public void ProcessMappedIC(bool importOnlyICIUS)
        {

            DIConnection SrcDBConnection = null;
            DIQueries SrcDBQueries = null;

            IndicatorClassificationBuilder TrgICBuilder = null;
            IndicatorClassificationInfo SrcICInfo = null;

            DataTable Table = null;
            string SourceFileWPath = string.Empty;
            int TrgICNid = 0;

            if (this.MappedTables.ContainsKey(TemplateMergeControlType.IndicatorClassification))
            {
                TrgICBuilder = new IndicatorClassificationBuilder(this.DBConnection, this.DBQueries);

                foreach (MappedRowInfo RowInfo in this.MappedTables[TemplateMergeControlType.IndicatorClassification].MappedTable.MappedRows.Values)
                {
                    TrgICNid = Convert.ToInt32(RowInfo.AvailableRow[ IndicatorClassifications.ICNId]);

                    SrcICInfo = this.GetIndicatorClassificationInfo(RowInfo.UnmatchedRow);

                    SourceFileWPath = Convert.ToString(RowInfo.UnmatchedRow[MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME]);
                    try
                    {
                        SrcDBConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, SourceFileWPath, string.Empty, string.Empty);
                        SrcDBQueries = DataExchange.GetDBQueries(SrcDBConnection);

                        // update IC in target only if importOnlyICIUS = false
                        if (importOnlyICIUS)
                        {
                            TrgICBuilder.ImportICFrmMappedIC(SrcICInfo, SrcICInfo.Nid, TrgICNid, SrcDBQueries, SrcDBConnection);
                        }

                        // update IC IUS relation in target file
                        TrgICBuilder.ImportICAndIUSRelations(SrcICInfo.Nid, TrgICNid, SrcICInfo.Type, SrcDBQueries, SrcDBConnection);
                    }
                    finally 
                    {
                        if (SrcDBConnection != null)
                        {
                            SrcDBConnection.Dispose();
                            SrcDBQueries.Dispose();
                        }
                    }

                    
                }
            }
        }

        /// <summary>
        /// Process Mapped Areas from MappedRows.
        /// </summary>
        public void ProcessMappedAreas()
        {

            DIConnection SrcDBConnection = null;
            DIQueries SrcDBQueries = null;

            AreaBuilder TrgAreaBuilder = null;
            AreaBuilder SourceAreaBuilder = null;
            AreaInfo SrcAreaInfo = null;

            DataTable Table = null;
            string SourceFileWPath = string.Empty;
            int TrgAreaNid = 0;

            if (this.MappedTables.ContainsKey(TemplateMergeControlType.Areas))
            {
                TrgAreaBuilder = new AreaBuilder(this.DBConnection, this.DBQueries);

                foreach (DataRow Row in this.MappedTables[TemplateMergeControlType.Areas].MappedTable.MappedTable.Rows)
                {
                    //todo:
                    Table = this.DBConnection.ExecuteDataTable(TemplateQueries.GetImportAreas());

                    TrgAreaNid = Convert.ToInt32(Row[MergetTemplateConstants.Columns.AVAILABLE_COL_Prefix + Area.AreaNId]);

                    if (Table != null && Table.Rows.Count > 0)
                    {
                        try
                        {
                            SourceFileWPath = Convert.ToString(Table.Rows[0][MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME]);

                            SrcDBConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, SourceFileWPath, string.Empty, string.Empty);
                            SrcDBQueries = DataExchange.GetDBQueries(SrcDBConnection);

                            // Get Source Area Info
                            SourceAreaBuilder = new AreaBuilder(SrcDBConnection, SrcDBQueries);
                            SrcAreaInfo= SourceAreaBuilder.GetAreaInfo(FilterFieldType.NId, Convert.ToString(Table.Rows[0][MergetTemplateConstants.Columns.COLUMN_SRCNID]));

                            // Import Mapped Area 
                            TrgAreaBuilder.ImportAreaFrmMappedArea(SrcAreaInfo, SrcAreaInfo.Nid, TrgAreaNid, SrcDBQueries, SrcDBConnection);
                        }
                        finally
                        {
                            if (SrcDBConnection != null)
                            {
                                SrcDBConnection.Dispose();
                                SrcDBQueries.Dispose();
                            }
                        }
                    }
                }
            }
        }

       
        #endregion


        #endregion


    }
}
