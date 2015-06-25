using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;

namespace DevInfo.Lib.DI_LibBAL.Import.DAImport
{
    /// <summary>
    /// Helps in importing or exporting Xml file which contains mapping information
    /// </summary>
    public class MappedInfoGenerator
    {
        public static class Constants
        {
            public const string COL_IND = "Indicator";
            public const string COL_UNIT = "Unit";
            public const string COL_SUBGROUP = "Subgroup";
            public const string COL_UNMATCHED_IND_GID = "Unmatched_Indicator_GID";
            public const string COL_UNMATCHED_UNIT_GID = "Unmatched_Unit_GID";
            public const string COL_UNMATCHED_SUBGROUP_GID = "Unmatched_Subgroup_GID";
            public const string COL_UNMATCHED_IND = "Unmatched_Indicator";
            public const string COL_UNMATCHED_UNIT  = "Unmatched_Unit";
            public const string COL_UNMATCHED_SUBGROUP = "Unmatched_Subgroup";
            public const string COL_UNMATCHED_AERA_ID = "Unmatched_Area_ID";
            public const string COL_UNMATCHED_AERA_GID = "Unmatched_Area_GID";
            public const string COL_UNMATCHED_AERA = "Unmatched_Area_name";
            public const string TBL_IND = "INDICATOR";
            public const string TBL_UNIT = "UNIT";
            public const string TBL_SUBGROUP = "SUBGOUP_VAL";
            public const string TBL_AREA = "AREA";
            public const string DATASET_IMP = "IMP_DATASET";
        }
        
        #region "-- Private --"

        #region "-- Methods --"

        #region "-- Tables --"

        private void CreateMappedIndTbl()
        {
            try
            {
                this._MappedIndicatorTable = new DataTable(Constants.TBL_IND);
                this._MappedIndicatorTable.Columns.Add(Constants.COL_IND);
                this._MappedIndicatorTable.Columns.Add(Indicator.IndicatorNId);
                this._MappedIndicatorTable.Columns.Add(Indicator.IndicatorGId);
                this._MappedIndicatorTable.Columns.Add(Constants. COL_UNMATCHED_IND);
                this._MappedIndicatorTable.Columns.Add(Constants.COL_UNMATCHED_IND_GID);
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
        }

        private void CreateMappedUnitTbl()
        {
            try
            {
                this._MappedUnitTable = new DataTable(Constants.TBL_UNIT);
                this._MappedUnitTable.Columns.Add(Constants.COL_UNIT);
                this._MappedUnitTable.Columns.Add(Unit.UnitNId);
                this._MappedUnitTable.Columns.Add(Unit.UnitGId);
                this._MappedUnitTable.Columns.Add(Constants.COL_UNMATCHED_UNIT );
                this._MappedUnitTable.Columns.Add(Constants.COL_UNMATCHED_UNIT_GID);
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
        }

        private void CreateMappedSubgroupTbl()
        {
            try
            {
                this._MappedSubgroupTable = new DataTable(Constants.TBL_SUBGROUP);
                this._MappedSubgroupTable.Columns.Add(Constants.COL_SUBGROUP);
                this._MappedSubgroupTable.Columns.Add(Subgroup.SubgroupNId);
                this._MappedSubgroupTable.Columns.Add(Subgroup.SubgroupGId);
                this._MappedSubgroupTable.Columns.Add(Constants.COL_UNMATCHED_SUBGROUP);
                this._MappedSubgroupTable.Columns.Add(Constants.COL_UNMATCHED_SUBGROUP_GID);
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
        }

        private void CreateMappedAreaTbl()
        {
            try
            {
                this._MappedAreaTable = new DataTable(Constants.TBL_AREA);
                this._MappedAreaTable.Columns.Add(Area.AreaName);
                this._MappedAreaTable.Columns.Add(Area.AreaID);
                this._MappedAreaTable.Columns.Add(Area.AreaGId);
                this._MappedAreaTable.Columns.Add(Constants.COL_UNMATCHED_AERA);
                this._MappedAreaTable.Columns.Add(Constants.COL_UNMATCHED_AERA_ID);
                this._MappedAreaTable.Columns.Add(Constants.COL_UNMATCHED_AERA_GID);
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
        }

        private void ClearMappedTbls()
        {
            if ((this._MappedIndicatorTable != null))
                this._MappedIndicatorTable.Dispose();

            this._MappedIndicatorTable = null;

            if ((this._MappedUnitTable != null))
                this._MappedUnitTable.Dispose();

            this._MappedUnitTable = null;

            if ((this._MappedSubgroupTable != null))
                this._MappedSubgroupTable.Dispose();

            this._MappedSubgroupTable = null;

            if ((this._MappedAreaTable != null))
                this._MappedAreaTable.Dispose();

            this._MappedAreaTable = null;
        }

        #endregion

        private void CreateIMPMappingXMLFile(string filenameWPath)
        {
            DataSet IMPMappingDataSet = null;
            try
            {
                IMPMappingDataSet = new DataSet(Constants.DATASET_IMP);
                IMPMappingDataSet.Tables.Add(this._MappedAreaTable.Copy());
                IMPMappingDataSet.Tables.Add(this._MappedIndicatorTable.Copy());
                IMPMappingDataSet.Tables.Add(this._MappedUnitTable.Copy());
                IMPMappingDataSet.Tables.Add(this._MappedSubgroupTable.Copy());
                IMPMappingDataSet.WriteXml(filenameWPath, XmlWriteMode.WriteSchema);
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
            finally
            {
                if ((IMPMappingDataSet != null))
                {
                    IMPMappingDataSet.Dispose();
                }

                IMPMappingDataSet = null;
            }
        }

        private void CheckAndCreateImpMappedFile(string tempFilenameWPath)
        {
            try
            {
                
                string DirName = Path.GetDirectoryName(tempFilenameWPath);

                if (!File.Exists(tempFilenameWPath))
                {
                    if (!Directory.Exists(DirName))
                    {
                        Directory.CreateDirectory(DirName);
                    }
                    this.SaveNewIMPMappingInfo(tempFilenameWPath);
                }
                else
                {
                    if ((this._MappedIndicatorTable == null))
                        this.LoadWDITbls(tempFilenameWPath);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
        }

        private void LoadWDITbls(string filenameWpath)
        {
            DataSet ImpMappingDataSet = null;
            try
            {
                this.ClearMappedTbls();
                this._MappedAreaTable = new DataTable(Constants.TBL_AREA);
                this._MappedIndicatorTable = new DataTable(Constants.TBL_IND);
                this._MappedSubgroupTable = new DataTable(Constants.TBL_SUBGROUP);
                this._MappedUnitTable = new DataTable(Constants.TBL_UNIT);

                ImpMappingDataSet = new DataSet(Constants.DATASET_IMP);
                ImpMappingDataSet.ReadXml(filenameWpath, XmlReadMode.ReadSchema);

                this._MappedAreaTable = ImpMappingDataSet.Tables[Constants.TBL_AREA];
                this._MappedIndicatorTable = ImpMappingDataSet.Tables[Constants.TBL_IND];
                this._MappedSubgroupTable = ImpMappingDataSet.Tables[Constants.TBL_SUBGROUP];
                this._MappedUnitTable = ImpMappingDataSet.Tables[Constants.TBL_UNIT];
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
            finally
            {
                if ((ImpMappingDataSet != null))
                {
                    ImpMappingDataSet.Dispose();
                }

                ImpMappingDataSet = null;

            }
        }

        private void SaveNewIMPMappingInfo(string filenameWPath)
        {
            try
            {
                this.ClearMappedTbls();
                this.CreateMappedAreaTbl();
                this.CreateMappedIndTbl();
                this.CreateMappedSubgroupTbl();
                this.CreateMappedUnitTbl();
                this.CreateIMPMappingXMLFile(filenameWPath);
            }

            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
        }

        #endregion

        #endregion

        #region "-- Public --"

        #region "-- Variables/Properties --"

        
        private DataTable _MappedIndicatorTable=null;
        /// <summary>
        /// Gets or Sets mapped Indicator table
        /// </summary>
        public DataTable MappedIndicatorTable
        {
            get 
            {
                return this._MappedIndicatorTable; 
            }
            set 
            {
                this._MappedIndicatorTable= value; 
            }
        }
	

        
        private DataTable _MappedUnitTable=null;
        /// <summary>
        /// Gets or sets mapped unit table
        /// </summary>
        public DataTable MappedUnitTable
        {
            get
            {
                return this._MappedUnitTable; 
            }
            set 
            {
                this._MappedUnitTable = value; 
            }
        }
	
        private DataTable _MappedAreaTable=null;
        /// <summary>
        /// Gets or sets mapped area table
        /// </summary>
        public DataTable MappedAreaTable
        {
            get
            {
                return this._MappedAreaTable; 
            }
            set
            {
                this._MappedAreaTable = value; 
            }
        }
	
        
        private DataTable _MappedSubgroupTable;
        /// <summary>
        /// Gets or sets mapped subgroup table
        /// </summary>
        public DataTable MappedSubgroupTable
        {
            get
            {
                return this._MappedSubgroupTable; 
            }
            set 
            {
                this._MappedSubgroupTable = value; 
            }
        }
	

        
        #endregion

        #region "-- New/Dispose --"

        public MappedInfoGenerator(string filenameWPath)
        {
            this.CheckAndCreateImpMappedFile(filenameWPath);
        }

        #endregion

        #region "-- Methods --"

        
        #region "-- Indicator --"

        public void InsertRecordInINDMapping(string indicatorName, string GID, string unmatchedGID, string unmatchedName)
        {
            try
            {

                DataRow[] MappedIndicatorRows;
                DataRow MappedRow;



                MappedIndicatorRows = this._MappedIndicatorTable.Select(Indicator.IndicatorGId + " ='" + DICommon.RemoveQuotes(GID.Trim()) + "'");
                if (MappedIndicatorRows.Length > 0)
                {
                    MappedIndicatorRows[0][Constants.COL_IND] = indicatorName;
                    MappedIndicatorRows[0][Indicator.IndicatorGId] = GID.Trim();
                    MappedIndicatorRows[0][Indicator.IndicatorNId] = string.Empty;
                    MappedIndicatorRows[0][Constants.COL_UNMATCHED_IND] = unmatchedName;
                    MappedIndicatorRows[0][Constants.COL_UNMATCHED_IND_GID] = unmatchedGID.Trim();
                    MappedIndicatorRows = null;
                }
                else
                {
                    MappedRow = this._MappedIndicatorTable.NewRow();

                    MappedRow[Constants.COL_IND] = indicatorName;
                    MappedRow[Indicator.IndicatorGId] = GID.Trim();
                    MappedRow[Indicator.IndicatorNId] = string.Empty;
                    MappedRow[Constants.COL_UNMATCHED_IND] = unmatchedName;
                    MappedRow[Constants.COL_UNMATCHED_IND_GID] = unmatchedGID.Trim();

                    this._MappedIndicatorTable.Rows.Add(MappedRow);
                    MappedRow = null;
                }

            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }

        }

        #endregion

        #region "-- Unit --"

        public void InsertUNITMapping(string unitName, string unitGID, string unmatchedUnitGID, string unmatchedUnitName)
        {
            try
            {
                DataRow[] MappedUnitRows;
                DataRow MappedRow;

                MappedUnitRows = this._MappedUnitTable.Select(Unit.UnitGId + " ='" + DICommon.RemoveQuotes(unitGID) + "'");
                if (MappedUnitRows.Length > 0)
                {
                    MappedUnitRows[0][Constants.COL_UNIT] = unitName;
                    MappedUnitRows[0][Unit.UnitGId] = unitGID.Trim();
                    MappedUnitRows[0][Unit.UnitNId] = string.Empty;
                    MappedUnitRows[0][Constants.COL_UNMATCHED_UNIT ] = unmatchedUnitName;
                    MappedUnitRows[0][Constants.COL_UNMATCHED_UNIT_GID] = unmatchedUnitGID.Trim();
                    MappedUnitRows = null;
                }
                else
                {
                    MappedRow = this._MappedUnitTable.NewRow();
                    MappedRow[Constants.COL_UNIT] = unitName;
                    MappedRow[Unit.UnitGId] = unitGID.Trim();
                    MappedRow[Unit.UnitNId] = string.Empty;
                    MappedRow[Constants.COL_UNMATCHED_UNIT ] = unmatchedUnitName;
                    MappedRow[Constants.COL_UNMATCHED_UNIT_GID] = unmatchedUnitGID.Trim();
                    this._MappedUnitTable.Rows.Add(MappedRow);
                    MappedRow = null;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
        }

        #endregion

        #region "-- Subgroup --"

        public void InsertSubgroupMapping(string name, string GID, string unmatchedGID, string unmatchedName)
        {
            DataRow[] MappedSubgroupRows;
            DataRow MappedRows;

            try
            {
                MappedSubgroupRows = this._MappedSubgroupTable.Select(Subgroup.SubgroupGId + " ='" + DICommon.RemoveQuotes(GID) + "'");
                if (MappedSubgroupRows.Length > 0)
                {
                    MappedSubgroupRows[0][Constants.COL_SUBGROUP] = name;
                    MappedSubgroupRows[0][Subgroup.SubgroupGId] = GID.Trim();
                    MappedSubgroupRows[0][Subgroup.SubgroupNId] = string.Empty;
                    MappedSubgroupRows[0][Constants.COL_UNMATCHED_SUBGROUP] = unmatchedName;
                    MappedSubgroupRows[0][Constants.COL_UNMATCHED_SUBGROUP_GID] = unmatchedGID.Trim();
                    MappedSubgroupRows = null;
                }
                else
                {
                    MappedRows = this._MappedSubgroupTable.NewRow();
                    MappedRows[Constants.COL_SUBGROUP] = name;
                    MappedRows[Subgroup.SubgroupGId] = GID;
                    MappedRows[Subgroup.SubgroupNId] = string.Empty;
                    MappedRows[Constants.COL_UNMATCHED_SUBGROUP] = unmatchedName;
                    MappedRows[Constants.COL_UNMATCHED_SUBGROUP_GID] = unmatchedGID;
                    this._MappedSubgroupTable.Rows.Add(MappedRows);
                    MappedRows = null;
                }

            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }

        }

        #endregion

        #region "-- Area --"

        public void InsertAreaMapping(string name, string ID, string GID, string unmatchedID, string unmatchedName, string unmatchedGID)
        {
            DataRow[] MappedAreaRows = null;
            DataRow MappedRow = null;

            try
            {

                //-- check on the unmatched Area_ID already mapped or not 
                MappedAreaRows = this._MappedAreaTable.Select(Constants.COL_UNMATCHED_AERA_ID + " ='" + DICommon.RemoveQuotes(unmatchedID) + "'");
                if (MappedAreaRows.Length > 0)
                {
                    MappedAreaRows[0][Area.AreaName] = name;
                    MappedAreaRows[0][Area.AreaGId] = GID.Trim();
                    MappedAreaRows[0][Area.AreaID] = ID;
                    MappedAreaRows[0][Constants.COL_UNMATCHED_AERA] = unmatchedName;
                    MappedAreaRows[0][Constants.COL_UNMATCHED_AERA_GID] = unmatchedGID.Trim();
                    MappedAreaRows[0][Constants.COL_UNMATCHED_AERA_ID] = unmatchedID;
                    MappedAreaRows = null;
                }
                else
                {
                    MappedRow = this._MappedAreaTable.NewRow();
                    MappedRow[Area.AreaName] = name;
                    MappedRow[Area.AreaGId] = GID.Trim();
                    MappedRow[Area.AreaID] = ID;
                    MappedRow[Constants.COL_UNMATCHED_AERA] = unmatchedName;
                    MappedRow[Constants.COL_UNMATCHED_AERA_GID] = unmatchedGID.Trim();
                    MappedRow[Constants.COL_UNMATCHED_AERA_ID] = unmatchedID;
                    this._MappedAreaTable.Rows.Add(MappedRow);
                    MappedRow = null;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }

        }

        #endregion 

        #region "-- Save File --"

        public void SaveFile(string fileNameWPath)
        {
            try
            {
                    if (File.Exists(fileNameWPath))
                    {
                        File.Delete(fileNameWPath);
                    }

                    this.CreateIMPMappingXMLFile(fileNameWPath);
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
        }

        #endregion

        #endregion

        #endregion



    }
}
