using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;

using DevInfo.Lib.DI_LibBAL.DA.DML;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;

namespace DevInfo.Lib.DI_LibBAL.MergeTemplate
{
    /// <summary>
    /// Process Matched Elements For Indicator, Unit, SG, SGD, SGDV, IndiactorClassification, Areas.
    /// </summary>
    public class ProcessMatchedElement
    {

        #region "-- Private --"

        #region "-- Variables --"

        private DIConnection DBConnection;
        private DIQueries DBQueries = null;
        private MergeTemplateQueries TemplateQueries;
        #endregion

        #endregion

        #region "-- Public --"

        #region "-- New/Dippose --"

        public ProcessMatchedElement(DIConnection dbConnection, DIQueries dbQueries)
        {
            this.DBConnection = dbConnection;
            this.DBQueries = dbQueries;
        }

        #endregion

        /// <summary>
        /// Process Matched Elements for Indicator, Unit, SGDimensions, Subgroup Dimension Values,SubgroupVals, IC and Areas.
        /// </summary>
        public void ProcessMatchedElements()
        {
            try
            {
                this.TemplateQueries = new MergeTemplateQueries(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode);
                this.ProcessMatchedIndicators();
                this.ProcessMatchedUnits();
                this.ProcessSubgroupDimensions();
                this.ProcessSubgroupDimensionValues();
                this.ProcessSubgroupVals();
                this.ProcessMatchedIC();
                this.ProcessMatchedAreas();
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        /// <summary>
        /// Process Matched Indicators 
        /// </summary>
        public void ProcessMatchedIndicators()
        {
            DataTable Table = null;
            string selectedNids = string.Empty;

            IndicatorBuilder IndicatorBuilderObj = null;
            IndicatorBuilder SrcIndicatorBuilderObj = null;
            IndicatorInfo SrcIndicatorInfoObj = null;
            Dictionary<string, DataRow> FileWithNids = new Dictionary<string, DataRow>();

            DIConnection SourceDBConnection = null;
            DIQueries SourceDBQueries = null;

            //-- Step 1: Get TempTable with Sorted SourceFileName
            Table = this.DBConnection.ExecuteDataTable(this.TemplateQueries.GetMatchedIndicator());

            //-- Step 2:Initialise Indicator Builder with Target DBConnection
            IndicatorBuilderObj = new IndicatorBuilder(this.DBConnection, this.DBQueries);

            //-- Step 3: Import Nids for each SourceFile
            foreach (DataRow Row in Table.Copy().Rows)
            {
                try
                {
                    string SourceFileWPath = Convert.ToString(Row[MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME]);

                    SourceDBConnection = new DIConnection(DIServerType.MsAccess, String.Empty, String.Empty, SourceFileWPath, String.Empty, MergetTemplateConstants.DBPassword);
                    SourceDBQueries = DataExchange.GetDBQueries(SourceDBConnection);

                    //get indicator info
                    SrcIndicatorBuilderObj = new IndicatorBuilder(SourceDBConnection, SourceDBQueries);
                    SrcIndicatorInfoObj = SrcIndicatorBuilderObj.GetIndicatorInfo(FilterFieldType.NId, Convert.ToString(Row[Indicator.IndicatorNId]), FieldSelection.Light);

                    //import indicator
                    IndicatorBuilderObj.ImportIndicator(SrcIndicatorInfoObj, Convert.ToInt32(Row[Indicator.IndicatorNId]), SourceDBQueries, SourceDBConnection);

                }
                catch (Exception ex) { ExceptionFacade.ThrowException(ex); }
                finally
                {
                    if (SourceDBConnection != null)
                        SourceDBConnection.Dispose();
                    if (SourceDBQueries != null)
                        SourceDBQueries.Dispose();
                }
            }

        }

        /// <summary>
        /// Process Target Units which matched with Source Units
        /// </summary>
        public void ProcessMatchedUnits()
        {
            DataTable Table = null;

            UnitBuilder UnitBuilderObj = null;
            UnitBuilder SrcUnitBuilderObj = null;
            UnitInfo SrcUnitInfoObj = null;
            Dictionary<string, DataRow> FileWithNids = new Dictionary<string, DataRow>();

            DIConnection SourceDBConnection = null;
            DIQueries SourceDBQueries = null;

            //-- Step 1: Get TempTable with Sorted SourceFileName
            Table = this.DBConnection.ExecuteDataTable(this.TemplateQueries.GetMatchedUnit());

            //-- Step 2:Initialise Indicator Builder with Target DBConnection
            UnitBuilderObj = new UnitBuilder(this.DBConnection, this.DBQueries);

            //-- Step 3: Import Nids for each SourceFile
            foreach (DataRow Row in Table.Copy().Rows)
            {
                try
                {
                    string SourceFileWPath = Convert.ToString(Row[MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME]);

                    SourceDBConnection = new DIConnection(DIServerType.MsAccess, String.Empty, String.Empty, SourceFileWPath, String.Empty, MergetTemplateConstants.DBPassword);
                    SourceDBQueries = DataExchange.GetDBQueries(SourceDBConnection);

                    // get unit info
                    SrcUnitBuilderObj = new UnitBuilder(SourceDBConnection, SourceDBQueries);
                    SrcUnitInfoObj = SrcUnitBuilderObj.GetUnitInfo(FilterFieldType.NId, Convert.ToString(Row[Unit.UnitNId]));                    
                    // import unit 
                    UnitBuilderObj.ImportUnit(SrcUnitInfoObj, Convert.ToInt32(Row[Unit.UnitNId]), SourceDBQueries, SourceDBConnection);

                }
                catch (Exception ex) { ExceptionFacade.ThrowException(ex); }
                finally
                {
                    if (SourceDBConnection != null)
                        SourceDBConnection.Dispose();
                    if (SourceDBQueries != null)
                        SourceDBQueries.Dispose();
                }
            }

        }

        /// <summary>
        /// Process Matched Target Subgroupvals
        /// </summary>
        public void ProcessSubgroupVals()
        {
            DataTable Table = null;

            DI6SubgroupValBuilder SGBuilderObj = null;
            DI6SubgroupValInfo SourceSGInfoObj = null;
            Dictionary<string, DataRow> FileWithNids = new Dictionary<string, DataRow>();

            DIConnection SourceDBConnection = null;
            DIQueries SourceDBQueries = null;

            DI6SubgroupValBuilder SourceSGValBuilder = null;

            //-- Step 1: Get TempTable with Sorted SourceFileName
            Table = this.DBConnection.ExecuteDataTable(this.TemplateQueries.GetMatchedSubgroupVals());

            //-- Step 2:Initialise Indicator Builder with Target DBConnection
            SGBuilderObj = new DI6SubgroupValBuilder(this.DBConnection, this.DBQueries);

            //-- Step 3: Import Nids for each SourceFile
            foreach (DataRow Row in Table.Copy().Rows)
            {
                try
                {
                    string SourceFileWPath = Convert.ToString(Row[MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME]);

                    SourceDBConnection = new DIConnection(DIServerType.MsAccess, String.Empty, String.Empty, SourceFileWPath, String.Empty, MergetTemplateConstants.DBPassword);
                    SourceDBQueries = DataExchange.GetDBQueries(SourceDBConnection);

                    // get subgroupval info
                    SourceSGValBuilder = new DI6SubgroupValBuilder(SourceDBConnection, SourceDBQueries);
                    SourceSGInfoObj = SourceSGValBuilder.GetSubgroupValInfo(FilterFieldType.NId, Convert.ToString(Row[SubgroupVals.SubgroupValNId]));
                    

                    // import subgroup val
                    SGBuilderObj.ImportSubgroupVal(Convert.ToInt32(Row[SubgroupVals.SubgroupValNId]), SourceDBQueries, SourceDBConnection);

                }
                catch (Exception ex) { ExceptionFacade.ThrowException(ex); }
                finally
                {
                    if (SourceDBConnection != null)
                        SourceDBConnection.Dispose();
                    if (SourceDBQueries != null)
                        SourceDBQueries.Dispose();
                }
            }

        }

        /// <summary>
        /// Process Matched Target Subgroup Dimension Values
        /// </summary>
        public void ProcessSubgroupDimensionValues()
        {
            DataTable Table = null;

            DI6SubgroupBuilder SGBuilderObj = null;
            DI6SubgroupInfo SourceSGInfoObj = null;
            Dictionary<string, DataRow> FileWithNids = new Dictionary<string, DataRow>();

            DIConnection SourceDBConnection = null;
            DIQueries SourceDBQueries = null;
            DI6SubgroupBuilder SourceSGBuilder = null;

            //-- Step 1: Get TempTable with Sorted SourceFileName
            Table = this.DBConnection.ExecuteDataTable(this.TemplateQueries.GetMatchedSubgroupDimValues());

            //-- Step 2:Initialise Indicator Builder with Target DBConnection
            SGBuilderObj = new DI6SubgroupBuilder(this.DBConnection, this.DBQueries);

            //-- Step 3: Import Nids for each SourceFile
            foreach (DataRow Row in Table.Copy().Rows)
            {
                try
                {
                    string SourceFileWPath = Convert.ToString(Row[MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME]);

                    SourceDBConnection = new DIConnection(DIServerType.MsAccess, String.Empty, String.Empty, SourceFileWPath, String.Empty, MergetTemplateConstants.DBPassword);
                    SourceDBQueries = DataExchange.GetDBQueries(SourceDBConnection);

                    // get subgroup info from source
                    SourceSGBuilder = new DI6SubgroupBuilder(SourceDBConnection, SourceDBQueries);
                    SourceSGInfoObj = SourceSGBuilder.GetSubgroupInfo(FilterFieldType.NId, Convert.ToString(Row[Subgroup.SubgroupNId]));
                                       

                    //import subgroup 
                    SGBuilderObj.ImportSubgroup(SourceSGInfoObj, Convert.ToInt32(Row[Subgroup.SubgroupNId]), SourceDBQueries, SourceDBConnection);

                }
                catch (Exception ex) { ExceptionFacade.ThrowException(ex); }
                finally
                {
                    if (SourceDBConnection != null)
                        SourceDBConnection.Dispose();
                    if (SourceDBQueries != null)
                        SourceDBQueries.Dispose();
                }
            }

        }

        /// <summary>
        /// Process Matched Subgroup Type
        /// </summary>
        public void ProcessSubgroupDimensions()
        {
            DataTable Table = null;

            DI6SubgroupTypeBuilder SGBuilderObj = null;
            DI6SubgroupTypeInfo SourceSGInfoObj = null;
            DI6SubgroupTypeBuilder SourceSGTypeBuilder = null;
            Dictionary<string, DataRow> FileWithNids = new Dictionary<string, DataRow>();

            DIConnection SourceDBConnection = null;
            DIQueries SourceDBQueries = null;

            //-- Step 1: Get TempTable with Sorted SourceFileName
            Table = this.DBConnection.ExecuteDataTable(this.TemplateQueries.GetMatchedSubgroupDimensions());

            //-- Step 2:Initialise Indicator Builder with Target DBConnection
            SGBuilderObj = new DI6SubgroupTypeBuilder(this.DBConnection, this.DBQueries);

            //-- Step 3: Import Nids for each SourceFile
            foreach (DataRow Row in Table.Copy().Rows)
            {
                try
                {
                    string SourceFileWPath = Convert.ToString(Row[MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME]);

                    SourceDBConnection = new DIConnection(DIServerType.MsAccess, String.Empty, String.Empty, SourceFileWPath, String.Empty, MergetTemplateConstants.DBPassword);
                    SourceDBQueries = DataExchange.GetDBQueries(SourceDBConnection);

                    // get subgroup type info
                    SourceSGTypeBuilder = new DI6SubgroupTypeBuilder(SourceDBConnection, SourceDBQueries);
                    SourceSGInfoObj = SourceSGTypeBuilder.GetSubgroupTypeInfoByNid(Convert.ToInt32(Row[SubgroupTypes.SubgroupTypeNId]));

                    //import subgroup type
                    SGBuilderObj.ImportSubgroupType(SourceSGInfoObj, Convert.ToInt32(Row[SubgroupTypes.SubgroupTypeNId]), SourceDBQueries, SourceDBConnection);

                }
                catch (Exception ex) { ExceptionFacade.ThrowException(ex); }
                finally
                {
                    if (SourceDBConnection != null)
                        SourceDBConnection.Dispose();
                    if (SourceDBQueries != null)
                        SourceDBQueries.Dispose();
                }
            }

        }

        /// <summary>
        /// Process Matched Indicator Classification
        /// </summary>
        public void ProcessMatchedIC()
        {

            IndicatorClassificationInfo SrcClassification;
            DataTable Table = null;

            DIConnection SourceDBConnection = null;
            DIQueries SourceDBQueries = null;

            //-- Step 1: Get TempTable with Sorted SourceFileName
            Table = this.DBConnection.ExecuteDataTable(this.TemplateQueries.GetMatchedIC());


            //-- Step 2: Import Nids for each SourceFile
            foreach (DataRow Row in Table.Copy().Rows)
            {
                try
                {
                    SourceDBConnection = new DIConnection(DIServerType.MsAccess, String.Empty, String.Empty, Convert.ToString(Row[MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME]), String.Empty, MergetTemplateConstants.DBPassword);
                    SourceDBQueries = DataExchange.GetDBQueries(SourceDBConnection);

                    //get ic from source table
                    //Row = this.SourceTable.Select(IndicatorClassifications.ICNId + "=" + Nid)[0];
                    SrcClassification = new IndicatorClassificationInfo();
                    SrcClassification.Name = DICommon.RemoveQuotes(Row[IndicatorClassifications.ICName].ToString());
                    SrcClassification.GID = Row[IndicatorClassifications.ICGId].ToString();
                    SrcClassification.IsGlobal = Convert.ToBoolean(Row[IndicatorClassifications.ICGlobal]);
                    SrcClassification.Nid = Convert.ToInt32(Row[IndicatorClassifications.ICNId]);
                    if (!string.IsNullOrEmpty(Convert.ToString((Row[IndicatorClassifications.ICInfo]))))
                    {
                        SrcClassification.ClassificationInfo = DICommon.RemoveQuotes(Row[IndicatorClassifications.ICInfo].ToString());
                    }

                    SrcClassification.Parent = new IndicatorClassificationInfo();
                    SrcClassification.Parent.Nid = Convert.ToInt32(Row[IndicatorClassifications.ICParent_NId]);
                    SrcClassification.Type = (ICType)(Convert.ToInt32(DIQueries.ICTypeText.IndexOfValue("'" + Convert.ToString(Row[IndicatorClassifications.ICType]) + "'")));

                    //import into target database
                    this.CreateClassificationChainFromExtDB(
                        SrcClassification.Nid,
                        SrcClassification.Parent.Nid,
                        SrcClassification.GID,
                        SrcClassification.Name,
                        SrcClassification.Type,
                        SrcClassification.ClassificationInfo,
                        SrcClassification.IsGlobal,
                       SourceDBQueries, SourceDBConnection, this.DBQueries, this.DBConnection);

                }
                catch (Exception ex)
                {
                    ExceptionFacade.ThrowException(ex);
                }

            }
        }

        /// <summary>
        /// Process Target Areas that matched with Source Areas
        /// </summary>
        public void ProcessMatchedAreas()
        {
            DataTable Table = null;

            AreaBuilder AreaBuilderObj = null;
            AreaInfo AreaInfoObj = null;
            Dictionary<string, DataRow> FileWithNids = new Dictionary<string, DataRow>();

            DIConnection SourceDBConnection = null;
            DIQueries SourceDBQueries = null;

            //-- Step 1: Get TempTable with Sorted SourceFileName
            Table = this.DBConnection.ExecuteDataTable(this.TemplateQueries.GetMatchedAreas());

            //-- Step 2:Initialise Indicator Builder with Target DBConnection
            AreaBuilderObj = new AreaBuilder(this.DBConnection, this.DBQueries);

            //-- Step 3: Import Nids for each SourceFile
            foreach (DataRow Row in Table.Copy().Rows)
            {
                try
                {

                    SourceDBConnection = new DIConnection(DIServerType.MsAccess, String.Empty, String.Empty, Convert.ToString(Row[MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME]), String.Empty, MergetTemplateConstants.DBPassword);
                    SourceDBQueries = DataExchange.GetDBQueries(SourceDBConnection);

                 

                    AreaBuilderObj.ImportArea(Convert.ToString(Row[Area.AreaNId]), 1, SourceDBConnection, SourceDBQueries);

                }
                catch (Exception ex) { ExceptionFacade.ThrowException(ex); }
                finally
                {
                    if (SourceDBConnection != null)
                        SourceDBConnection.Dispose();
                    if (SourceDBQueries != null)
                        SourceDBQueries.Dispose();
                }
            }

        }

        internal int CreateClassificationChainFromExtDB(int srcICNId, int srcParentNId, string srcICGid, string srcICName, ICType srcICType, string srcICInfo, bool isGlobal, DIQueries srcQueries, DIConnection srcDBConnection, DIQueries targetDBQueries, DIConnection targetDBConnection)
        {

            int RetVal;
            //int TrgParentNId; 
            //string TrgParentName; 
            int NewParentNId;
            DataTable TempTable;
            IndicatorClassificationInfo ICInfo;
            IndicatorClassificationBuilder ClassificationBuilder = new IndicatorClassificationBuilder(targetDBConnection, targetDBQueries);


            // -- STEP 1: If the Parent NID is -1 then create the Classification at the root 
            if (srcParentNId == -1)
            {
                // -- Create the Classification 

                // -------------------------------------------------------------- 
                // While importing the Classifications, if the NId of the Source Classification is _ 
                // the same as that of the one created, then the Duplicate check fails and a duplicate 
                // classification getscreated. PASS -99 as the first parameter to the calling function 
                // -------------------------------------------------------------- 
                ICInfo = new IndicatorClassificationInfo();
                ICInfo.Parent = new IndicatorClassificationInfo();
                ICInfo.Parent.Nid = srcParentNId;
                ICInfo.Nid = srcICNId;
                ICInfo.Name = srcICName;
                ICInfo.ClassificationInfo = srcICInfo;
                ICInfo.GID = srcICGid;
                ICInfo.IsGlobal = isGlobal;
                ICInfo.Type = srcICType;

                RetVal = ClassificationBuilder.ImportIndicatorClassification(ICInfo, srcICNId, srcQueries, srcDBConnection);

            }



            else
            {
                // -- STEP 2: If the Parent is not -1 then check for the existence of the Parent and then create the Classification 
                // Classification can only be created if the parent exists 
                // -- STEP 2.1: If the Parent Exists then create the Classification under that parent 
                // -- STEP 2.2: If the Parent does not Exist then create the Parent first and then the Classification under that parent 

                // -- STEP 2: Check the existence of the Parent in the Target Database 
                // -- get the parent from the source database 

                TempTable = srcDBConnection.ExecuteDataTable(srcQueries.IndicatorClassification.GetIC(FilterFieldType.NId, srcParentNId.ToString(), srcICType, FieldSelection.Heavy));
                {

                    // -------------------------------------------------------------- 
                    // While importing the Classifications, if the NId of the Source Classification is _ 
                    // the same as that of the one created, then the Duplicate check fails and a duplicate 
                    // classification getscreated. PASS -99 as the first parameter to the calling function 
                    // -------------------------------------------------------------- 
                    DataRow Row;
                    string ClassificationInfo = string.Empty;
                    Row = TempTable.Rows[0];
                    ClassificationInfo = Convert.ToString(Row[IndicatorClassifications.ICInfo]);

                    NewParentNId = CreateClassificationChainFromExtDB(
                       Convert.ToInt32(Row[IndicatorClassifications.ICNId]),
                       Convert.ToInt32(Row[IndicatorClassifications.ICParent_NId]),
                        Row[IndicatorClassifications.ICGId].ToString(),
                        Row[IndicatorClassifications.ICName].ToString(),
                        srcICType,
                        ClassificationInfo, Convert.ToBoolean(Row[IndicatorClassifications.ICGlobal]), srcQueries, srcDBConnection, targetDBQueries, targetDBConnection); ;
                }




                // -- Create the Child Now 
                ICInfo = new IndicatorClassificationInfo();
                ICInfo.Parent = new IndicatorClassificationInfo();
                ICInfo.Parent.Nid = NewParentNId;       // set new parent nid
                ICInfo.Nid = srcICNId;
                ICInfo.Name = srcICName;
                ICInfo.ClassificationInfo = srcICInfo;
                ICInfo.GID = srcICGid;
                ICInfo.IsGlobal = isGlobal;
                ICInfo.Type = srcICType;

                RetVal = ClassificationBuilder.ImportIndicatorClassification(ICInfo, srcICNId, srcQueries, srcDBConnection);

            }

            //import ic and ius relationship into indicator_classification_IUS table
            ClassificationBuilder.ImportICAndIUSRelations(srcICNId, RetVal, ICInfo.Type, srcQueries, srcDBConnection);

            return RetVal;
        }


        
        #endregion




    }
}
