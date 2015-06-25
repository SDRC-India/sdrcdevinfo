using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;

namespace DevInfo.Lib.DI_LibBAL.MergeTemplate
{
    public class ImportTableQueries
    {

        #region "-- Private --"

        #region "-- Variables and Properties --"
        
        private DITables Tables;
        private string TargetDataBaseFileWPath = string.Empty;
       
        #endregion

        #endregion

        #region "-- Public / Friend --"

        private string _SourceFileNameWPath;

        public string  ASourceFileNameWPath
        {
            get { return this._SourceFileNameWPath; }
            set { _SourceFileNameWPath = value; }
        }
	

        #region "-- New/Dispose --"
       
        /// <summary>
       /// Constructor
       /// </summary>
       /// <param name="dataPrefix"></param>
       /// <param name="languageCode"></param>
       /// <param name="dataBaseFileWPath">Target Database File Name With Path</param>
        public ImportTableQueries(string dataPrefix, string languageCode, string dataBaseFileWPath,string sourceFileNameWPath)
        {
            this.Tables = new DITables(dataPrefix, languageCode);
            TargetDataBaseFileWPath = dataBaseFileWPath;
            this._SourceFileNameWPath = sourceFileNameWPath;
        }
               

        #endregion
        
        #region "-- Methods --"

        #region "-- Carete Temp Table --"

        public string GetTempIndicatorTable()
        {
            string RetVal=string.Empty;

            RetVal = @"SELECT " + Indicator.IndicatorNId + "," + Indicator.IndicatorName + "," + Indicator.IndicatorGId + "," + Indicator.IndicatorGlobal + "," + Indicator.IndicatorInfo + ",CStr(" + Indicator.IndicatorNId + ") AS " + MergetTemplateConstants.Columns.COLUMN_SRCNID + ",'" + this._SourceFileNameWPath + "' AS " + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME + " INTO [MS Access;Database=" + this.TargetDataBaseFileWPath + ";pwd=" + MergetTemplateConstants.DBPassword + ";].[" + MergetTemplateConstants.TempTable.Indicator + "] FROM " + Tables.Indicator;

            return RetVal;
        }

        public string GetTempUnitTable()
        {
            string RetVal = string.Empty;
            RetVal = "SELECT *,CStr(" + Unit.UnitNId + ") AS "+ MergetTemplateConstants.Columns.COLUMN_SRCNID  + ",'" + this._SourceFileNameWPath + "' AS "+ MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME + " INTO [MS Access;Database=" + this.TargetDataBaseFileWPath + ";pwd=" + MergetTemplateConstants.DBPassword + ";].[" + MergetTemplateConstants.TempTable.Unit + "]  FROM " + Tables.Unit;
            return RetVal;
        }

        public string GetTempSubgroupValsTable()
        {
            string RetVal = string.Empty;
            RetVal = "SELECT *,CStr(" + SubgroupVals.SubgroupValNId + ") AS "+ MergetTemplateConstants.Columns.COLUMN_SRCNID  + ",'" + this._SourceFileNameWPath + "' AS " + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME + " INTO [MS Access;Database=" + this.TargetDataBaseFileWPath + ";pwd=" + MergetTemplateConstants.DBPassword + ";].[" + MergetTemplateConstants.TempTable.SubgroupVals + "]  from " + Tables.SubgroupVals;
            return RetVal;

        }

        public string GetTempSubgrouopDimensionsTable()
        {
            string RetVal = string.Empty;
            RetVal = "SELECT *,CStr(" + SubgroupTypes.SubgroupTypeNId + ") AS "+ MergetTemplateConstants.Columns.COLUMN_SRCNID  + ",'" + this._SourceFileNameWPath + "' AS " + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME + "  into [MS Access;Database=" + this.TargetDataBaseFileWPath + ";pwd=" + MergetTemplateConstants.DBPassword + ";].[" + MergetTemplateConstants.TempTable.SubgroupType + "]  from " + Tables.SubgroupType;
            return RetVal;
        }

        public string GetTempSubgroupDimValuesTable()
        {
            string RetVal = string.Empty;
            RetVal = "SELECT *,CStr(" + Subgroup.SubgroupNId + ") AS "+ MergetTemplateConstants.Columns.COLUMN_SRCNID  + ",'" + this._SourceFileNameWPath + "' AS " + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME + "  into [MS Access;Database=" + this.TargetDataBaseFileWPath + ";pwd=" + MergetTemplateConstants.DBPassword + ";].[" + MergetTemplateConstants.TempTable.Subgroup + "]  from " + Tables.Subgroup;
            return RetVal;
        }

        public string GetTempAreaTable()
        {
            string RetVal = string.Empty;
            RetVal = "SELECT *,CInt(" + Area.AreaNId + ") AS " + MergetTemplateConstants.Columns.COLUMN_SRCNID + ",'" + this._SourceFileNameWPath + "' AS " + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME + ", '0' AS " + MergetTemplateConstants.Columns.COLUMN_UNMATCHED  + "  INTO [MS Access;Database=" + this.TargetDataBaseFileWPath + ";pwd=" + MergetTemplateConstants.DBPassword + ";].[" + MergetTemplateConstants.TempTable.Area + "]  from " + Tables.Area;
            return RetVal;
        }

        public string GetTempICTable()
        {
            string RetVal = string.Empty;
            RetVal = "SELECT *,CInt(" + IndicatorClassifications.ICNId + ") AS " + MergetTemplateConstants.Columns.COLUMN_SRCNID + ",'" + this._SourceFileNameWPath + "' AS " + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME + ", '0' AS " +  MergetTemplateConstants.Columns.COLUMN_UNMATCHED  + " INTO [MS Access;Database=" + this.TargetDataBaseFileWPath + ";pwd=" + MergetTemplateConstants.DBPassword + ";].[" + MergetTemplateConstants.TempTable.IndicatorClassification + "] from " + Tables.IndicatorClassifications;
            return RetVal;
        }


        #endregion


         #region "-- Insert Into Temp Table --"

        public string InsertIntoIndicatorTable()
        {
            string RetVal=string.Empty;

            RetVal = @"INSERT INTO [MS Access;Database=" + this.TargetDataBaseFileWPath + ";pwd=" + MergetTemplateConstants.DBPassword + ";].[" + MergetTemplateConstants.TempTable.Indicator + "]SELECT " + Indicator.IndicatorName + "," + Indicator.IndicatorGId + "," + Indicator.IndicatorGlobal + "," + Indicator.IndicatorInfo + ",CStr(" + Indicator.IndicatorNId + ") AS "+ MergetTemplateConstants.Columns.COLUMN_SRCNID  + ",'" + this._SourceFileNameWPath + "' AS " + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME + " FROM " + Tables.Indicator;

            return RetVal;
        }

        public string InsertIntoUnitTable()
        {
            string RetVal = string.Empty;
            RetVal = "INSERT INTO [MS Access;Database=" + this.TargetDataBaseFileWPath + ";pwd=" + MergetTemplateConstants.DBPassword + ";].[" + MergetTemplateConstants.TempTable.Unit + "] SELECT " + Unit.UnitName + "," + Unit.UnitGId + "," + Unit.UnitGlobal + ", CStr(" + Unit.UnitNId + ") AS "+ MergetTemplateConstants.Columns.COLUMN_SRCNID  + ",'" + this._SourceFileNameWPath + "' AS " + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME + " FROM " + Tables.Unit;
            return RetVal;
        }

        public string InsertIntoSubgroupValsTable()
        {
            string RetVal = string.Empty;
            RetVal = "INSERT INTO [MS Access;Database=" + this.TargetDataBaseFileWPath + ";pwd=" + MergetTemplateConstants.DBPassword + ";].[" + MergetTemplateConstants.TempTable.SubgroupVals + "] SELECT " + SubgroupVals.SubgroupVal + "," + SubgroupVals.SubgroupValGId + "," + SubgroupVals.SubgroupValGlobal + ", CStr(" + SubgroupVals.SubgroupValNId + ") AS "+ MergetTemplateConstants.Columns.COLUMN_SRCNID  + ",'" + this._SourceFileNameWPath + "' AS " + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME + " FROM " + Tables.SubgroupVals;
            return RetVal;

        }

        public string InsertIntoSubgrouopDimensionsTable()
        {
            string RetVal = string.Empty;
            RetVal = "INSERT  INTO [MS Access;Database=" + this.TargetDataBaseFileWPath + ";pwd=" + MergetTemplateConstants.DBPassword + ";].[" + MergetTemplateConstants.TempTable.SubgroupType + "] SELECT " + SubgroupTypes.SubgroupTypeName + "," + SubgroupTypes.SubgroupTypeGID + "," + SubgroupTypes.SubgroupTypeGlobal + "," + SubgroupTypes.SubgroupTypeOrder + ", CStr(" + SubgroupTypes.SubgroupTypeNId + ") AS "+ MergetTemplateConstants.Columns.COLUMN_SRCNID  + ",'" + this._SourceFileNameWPath + "' AS " + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME + "  from " + Tables.SubgroupType;
            return RetVal;
        }

        public string InsertIntoSubgroupDimValuesTable()
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO [MS Access;Database=" + this.TargetDataBaseFileWPath + ";pwd=" + MergetTemplateConstants.DBPassword + ";].[" + MergetTemplateConstants.TempTable.Subgroup + "] SELECT " + Subgroup.SubgroupName + "," + Subgroup.SubgroupGId + "," + Subgroup.SubgroupGlobal + "," + Subgroup.SubgroupType + ",CStr(" + Subgroup.SubgroupNId + ") AS " + MergetTemplateConstants.Columns.COLUMN_SRCNID + ",'" + this._SourceFileNameWPath + "' AS " + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME + "  from " + Tables.Subgroup;

            return RetVal;
        }

        public string InsertIntoAreaTable()
        {
            string RetVal = string.Empty;
            RetVal = "INSERT INTO [MS Access;Database=" + this.TargetDataBaseFileWPath + ";pwd=" + MergetTemplateConstants.DBPassword + ";].[" + MergetTemplateConstants.TempTable.Area + "] SELECT " + Area.AreaID + "," + Area.AreaName + "," + Area.AreaGId + "," + Area.AreaGlobal + "," + Area.AreaLevel + "," + Area.AreaParentNId + "," + Area.AreaMap + ",CInt(" + Area.AreaNId + ") AS " + MergetTemplateConstants.Columns.COLUMN_SRCNID + ",'" + this._SourceFileNameWPath + "' AS " + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME + ", '0' AS " + MergetTemplateConstants.Columns.COLUMN_UNMATCHED  + "   FROM " + Tables.Area;
            return RetVal;
        }

        public string InsertIntoTempICTable()
        {
            string RetVal = string.Empty;
            RetVal = "INSERT  INTO [MS Access;Database=" + this.TargetDataBaseFileWPath + ";pwd=" + MergetTemplateConstants.DBPassword + ";].[" + MergetTemplateConstants.TempTable.IndicatorClassification + "] SELECT  " + IndicatorClassifications.ICName + "," + IndicatorClassifications.ICGId + "," + IndicatorClassifications.ICGlobal + "," + IndicatorClassifications.ICType + "," + IndicatorClassifications.ICParent_NId + "," + IndicatorClassifications.ICInfo + ", CInt(" + IndicatorClassifications.ICNId + ") AS " + MergetTemplateConstants.Columns.COLUMN_SRCNID + ",'" + this._SourceFileNameWPath + "' AS " + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME + ", '0' AS " + MergetTemplateConstants.Columns.COLUMN_UNMATCHED  + " FROM " + Tables.IndicatorClassifications;
            return RetVal;
        }
        
        #endregion
        
        #endregion

        #endregion
    }
}
