using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;

namespace DevInfo.Lib.DI_LibBAL.MergeTemplate
{
    public class MergeTemplateQueries:IDisposable 
    {

        #region "-- Public / Friend --"

        #region "-- Variables and Properties --"
        
        private DITables Tables;
        private string TargetDataBaseFileWPath = string.Empty;
        
        private  const string SOURCEFILENAME = "SourceFileName";
       
        #endregion

        #region "-- New/Dispose --"
       
       // /// <summary>
       ///// Constructor
       ///// </summary>
       ///// <param name="dataPrefix"></param>
       ///// <param name="languageCode"></param>
       ///// <param name="dataBaseFileWPath">Target Database File Name With Path</param>
       // public Queries(string dataPrefix,string languageCode,string dataBaseFileWPath)
       // {
       //     this.Tables = new DITables(dataPrefix, languageCode);
       //     TargetDataBaseFileWPath = dataBaseFileWPath;
       // }

        public MergeTemplateQueries(string dataPrefix, string languageCode)
        {
            this.Tables = new DITables(dataPrefix, languageCode);
            
        }

        #endregion


        #region "-- Methods --"

        #region "-- Matched Elements --"

        public string GetMatchedIndicator()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT  Max(  " + MergetTemplateConstants.Columns.COLUMN_SRCNID + ") As " + Indicator.IndicatorNId + ", T." + Indicator.IndicatorName + ",T." + Indicator.IndicatorGId + ",T." + Indicator.IndicatorGlobal + ",T." + Indicator.IndicatorInfo + "," + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME + " FROM "
                + MergetTemplateConstants.TempTable.Indicator + " AS T ," + Tables.Indicator  + " AS I WHERE T."+ Indicator.IndicatorName + " =I." + Indicator.IndicatorName 
                + " OR T." + Indicator.IndicatorGId + " =I." + Indicator.IndicatorGId + " GROUP BY T."
                + Indicator.IndicatorName + ",T." + Indicator.IndicatorGId + ",T." + Indicator.IndicatorGlobal + ",T." + Indicator.IndicatorInfo + ",T." + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME;

            return RetVal;
        }

        public string GetMatchedUnit()
        {
            string RetVal = string.Empty;
            RetVal = "SELECT MAX(" + MergetTemplateConstants.Columns.COLUMN_SRCNID + ") as " + Unit.UnitNId + ", T." + Unit.UnitName + ",T." + Unit.UnitGId + ",T." + Unit.UnitGlobal + "," + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME + " FROM "
                + MergetTemplateConstants.TempTable.Unit + " AS T ," + Tables.Unit + " AS I WHERE T." + Unit.UnitName + " =I." + Unit.UnitName + " OR  T." + Unit.UnitGId + " =I." + Unit.UnitGId + " GROUP BY T." + Unit.UnitName + ",T." + Unit.UnitGId + ",T." + Unit.UnitGlobal + "," + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME;

            return RetVal;
        }

        public string GetMatchedSubgroupVals()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT Max(" + MergetTemplateConstants.Columns.COLUMN_SRCNID + ") as " + SubgroupVals.SubgroupValNId + ", T." + SubgroupVals.SubgroupVal + ",T." + SubgroupVals.SubgroupValGId + ",T." + SubgroupVals.SubgroupValGlobal + "," + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME + " FROM "
                + MergetTemplateConstants.TempTable.SubgroupVals + " AS T , " + Tables.SubgroupVals
                + " AS I WHERE T." + SubgroupVals.SubgroupVal + " =I." + SubgroupVals.SubgroupVal + "  OR  T." + SubgroupVals.SubgroupValGId + " =I." + SubgroupVals.SubgroupValGId + " Group  BY T." + SubgroupVals.SubgroupVal + ",T." + SubgroupVals.SubgroupValGId + ",T." + SubgroupVals.SubgroupValGlobal + "," + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME;

            return RetVal;
        }

        public string GetMatchedSubgroupDimensions()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT Max(T." +MergetTemplateConstants.Columns.COLUMN_SRCNID + ") as " + SubgroupTypes.SubgroupTypeNId + ", T." + SubgroupTypes.SubgroupTypeName + ",T." + SubgroupTypes.SubgroupTypeGID + ",T." + SubgroupTypes.SubgroupTypeGlobal + ","+ MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME 
                + " FROM " + MergetTemplateConstants.TempTable.SubgroupType + " AS T , " + Tables.SubgroupType
                + " AS I WHERE T." + SubgroupTypes.SubgroupTypeName + " =I." + SubgroupTypes.SubgroupTypeName
                + " OR T." + SubgroupTypes.SubgroupTypeGID + " =I." + SubgroupTypes.SubgroupTypeGID + " GROUP By T." + SubgroupTypes.SubgroupTypeName + ",T." + SubgroupTypes.SubgroupTypeGID + ",T." + SubgroupTypes.SubgroupTypeGlobal+ "," + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME;

            return RetVal;
        }

        public string GetMatchedSubgroupDimValues()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT Max(" + MergetTemplateConstants.Columns.COLUMN_SRCNID + " ) AS " + Subgroup.SubgroupNId + ", T." + Subgroup.SubgroupName
                + ",T." + Subgroup.SubgroupGId + ",T." + Subgroup.SubgroupGlobal + "," + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME + " FROM " + MergetTemplateConstants.TempTable.Subgroup 
                + " AS T , " + Tables.Subgroup + " AS I WHERE T." + Subgroup.SubgroupName + " =I." + Subgroup.SubgroupName 
                + "  OR   T." + Subgroup.SubgroupGId + " =I." + Subgroup.SubgroupGId + " GROUP By T." + Subgroup.SubgroupName
                + ",T." + Subgroup.SubgroupGId + ",T." + Subgroup.SubgroupGlobal + "," + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME;

            return RetVal;
        }

        public string UpdateMatchedIC(string sourceFileName)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("Update " + MergetTemplateConstants.TempTable.IndicatorClassification + " AS IC SET IC." + MergetTemplateConstants.Columns.COLUMN_UNMATCHED + "= 1");

            sbQuery.Append(" WHERE IC." + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME + " = '" + sourceFileName + "'");

            sbQuery.Append(" AND IC." + IndicatorClassifications.ICType + "<>" + DIQueries.ICTypeText[ICType.Source]);

            sbQuery.Append(" AND exists (SELECT * FROM " + Tables.IndicatorClassifications + " AS I WHERE IC." + IndicatorClassifications.ICGId + " =I." + IndicatorClassifications.ICGId + ")");

            RetVal = sbQuery.ToString();
            return RetVal;
        }
                    
        public string GetMatchedIC()
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT IC." + MergetTemplateConstants.Columns.COLUMN_SRCNID + " AS " + IndicatorClassifications.ICNId + " ,IC." + IndicatorClassifications.ICParent_NId + ",IC." + IndicatorClassifications.ICName + ",IC." + IndicatorClassifications.ICGId + ",IC." + IndicatorClassifications.ICGlobal + ",IC." + IndicatorClassifications.ICType + ",IC." + IndicatorClassifications.ICInfo + ",IC." + MergetTemplateConstants.Columns.COLUMN_UNMATCHED + ",IC." + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME );

            sbQuery.Append(" FROM " + MergetTemplateConstants.TempTable.IndicatorClassification + " AS IC, "+ Tables.IndicatorClassifications +" AS I ");

            sbQuery.Append(" WHERE IC." + IndicatorClassifications.ICGId + "=" + "I." + IndicatorClassifications.ICGId);

            sbQuery.Append(" AND  IC." + IndicatorClassifications.ICType + "<>" + DIQueries.ICTypeText[ICType.Source]);

            sbQuery.Append(" ORDER BY IC." + IndicatorClassifications.ICName);
            RetVal = sbQuery.ToString();
            return RetVal;
        }
        
        public string GetMatchedAreas()
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT T." + MergetTemplateConstants.Columns.COLUMN_SRCNID + " AS " + Area.AreaNId + ", T." + Area.AreaParentNId + ",T." + Area.AreaName + ",T." + Area.AreaGId + ",T." + Area.AreaGlobal + ",T." + Area.AreaID + ",T." + Area.AreaBlock + ",T." + Area.AreaMap + ",T." + Area.AreaLevel + ",T." + MergetTemplateConstants.Columns.COLUMN_UNMATCHED + "," + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME);

            sbQuery.Append(" FROM " + MergetTemplateConstants.TempTable.Area + " as T ," + Tables.Area +" AS A " );

            sbQuery.Append(" WHERE T." + Area.AreaID + " = A." + Area.AreaID);
           

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        #endregion


        #region "-- Unmatched Elements --"

        public string GetUnmatchedIndicator()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT  Max(  " + Indicator.IndicatorNId + ") As " + Indicator.IndicatorNId + ", T." + Indicator.IndicatorName + ",T." + Indicator.IndicatorGId + ",T." + Indicator.IndicatorGlobal + " FROM "
                + MergetTemplateConstants.TempTable.Indicator + " AS T WHERE  NOT EXISTS (SELECT * FROM " + Tables.Indicator + " AS I WHERE T." 
                + Indicator.IndicatorName + " =I."+ Indicator.IndicatorName + " ) and  NOT exists (SELECT * FROM " + Tables.Indicator
                + " AS I WHERE T." + Indicator.IndicatorGId + " =I." + Indicator.IndicatorGId + ") GROUP BY T." + Indicator.IndicatorName + ",T." + Indicator.IndicatorGId + ",T." + Indicator.IndicatorGlobal;

            return RetVal;
        }

        public string GetUnmatchedUnit()
        {
            string RetVal = string.Empty;
            RetVal = "SELECT MAX(" + Unit.UnitNId + ") as " + Unit.UnitNId + ", T." + Unit.UnitName + ",T." + Unit.UnitGId + ",T." + Unit.UnitGlobal + " FROM "
                + MergetTemplateConstants.TempTable.Unit + " AS T WHERE  NOT EXISTS (SELECT * FROM " + Tables.Unit + " AS I WHERE T."
                + Unit.UnitName + " =I." + Unit.UnitName + " ) and  NOT exists (SELECT * FROM " + Tables.Unit
                + " AS I WHERE T." + Unit.UnitGId + " =I." + Unit.UnitGId + ") GROUP BY T." + Unit.UnitName + ",T." + Unit.UnitGId + ",T." + Unit.UnitGlobal;

            return RetVal;
        }

        public string GetUnmatchedSubgroupVals()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT Max(" + SubgroupVals.SubgroupValNId + ") as " + SubgroupVals.SubgroupValNId + ", T." + SubgroupVals.SubgroupVal + ",T." + SubgroupVals.SubgroupValGId + ",T." + SubgroupVals.SubgroupValGlobal + " FROM "
                + MergetTemplateConstants.TempTable.SubgroupVals + " AS T WHERE  NOT EXISTS (SELECT * FROM " + Tables.SubgroupVals
                + " AS I WHERE T." + SubgroupVals.SubgroupVal + " =I." + SubgroupVals.SubgroupVal + " ) and  NOT exists (SELECT * FROM "
                + Tables.SubgroupVals + " AS I WHERE T." + SubgroupVals.SubgroupValGId + " =I." + SubgroupVals.SubgroupValGId + ") Group  BY T." + SubgroupVals.SubgroupVal + ",T." + SubgroupVals.SubgroupValGId + ",T." + SubgroupVals.SubgroupValGlobal;

            return RetVal;
        }

        public string GetUnmatchedSubgroupDimensions()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT Max(T." + SubgroupTypes.SubgroupTypeNId + ") as " + SubgroupTypes.SubgroupTypeNId + ", T." + SubgroupTypes.SubgroupTypeName + ",T." + SubgroupTypes.SubgroupTypeGID + ",T." + SubgroupTypes.SubgroupTypeGlobal
                + " FROM " + MergetTemplateConstants.TempTable.SubgroupType + " AS T WHERE  NOT EXISTS (SELECT * FROM " + Tables.SubgroupType
                + " AS I WHERE T." + SubgroupTypes.SubgroupTypeName + " =I." + SubgroupTypes.SubgroupTypeName
                + " ) and  NOT exists (SELECT * FROM " + Tables.SubgroupType + " AS I WHERE T." + SubgroupTypes.SubgroupTypeGID
                + " =I." + SubgroupTypes.SubgroupTypeGID + ") GROUP By T." + SubgroupTypes.SubgroupTypeName + ",T." + SubgroupTypes.SubgroupTypeGID + ",T." + SubgroupTypes.SubgroupTypeGlobal;

            return RetVal;
        }

        public string GetUnmatchedSubgroupDimValues()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT Max(" + Subgroup.SubgroupNId + " ) AS " + Subgroup.SubgroupNId + ", T." + Subgroup.SubgroupName + ",T." + Subgroup.SubgroupGId + ",T." + Subgroup.SubgroupGlobal
                + " FROM " + MergetTemplateConstants.TempTable.Subgroup + " AS T WHERE  NOT EXISTS (SELECT * FROM " + Tables.Subgroup
                + " AS I WHERE T." + Subgroup.SubgroupName + " =I." + Subgroup.SubgroupName
                + " ) and  NOT exists (SELECT * FROM " + Tables.Subgroup + " AS I WHERE T." + Subgroup.SubgroupGId
                + " =I." + Subgroup.SubgroupGId + ")GROUP By T." + Subgroup.SubgroupName + ",T." + Subgroup.SubgroupGId + ",T." + Subgroup.SubgroupGlobal;

            return RetVal;
        }

        public string UpdateUnmatchedAreas(string sourceFileName)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("UPDATE " + MergetTemplateConstants.TempTable.Area + " as T SET " + MergetTemplateConstants.Columns.COLUMN_UNMATCHED + "= 1");

            sbQuery.Append(" WHERE T." + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME + " = '" + sourceFileName + "'  AND NOT EXISTS (SELECT * FROM " + Tables.Area + " AS I WHERE T." + Area.AreaID + " =I." + Area.AreaID + ")");

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        public string UpdateUnmatchedIC(ICType classificationType, string sourceFileName)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("Update " + MergetTemplateConstants.TempTable.IndicatorClassification + " AS IC SET IC." + MergetTemplateConstants.Columns.COLUMN_UNMATCHED + "= 1");

            sbQuery.Append(" WHERE IC." + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME + " = '" + sourceFileName + "'");

            sbQuery.Append(" AND IC." + IndicatorClassifications.ICType + "=" + DIQueries.ICTypeText[classificationType]);

            sbQuery.Append(" AND NOT exists (SELECT * FROM " + Tables.IndicatorClassifications + " AS I WHERE IC." + IndicatorClassifications.ICGId + " =I." + IndicatorClassifications.ICGId + ")");

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        public string GetUnmatchedIC(ICType classificationType, string sourceFileName)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT IC." + MergetTemplateConstants.Columns.COLUMN_SRCNID + " AS " + IndicatorClassifications.ICNId + " ,IC." + IndicatorClassifications.ICParent_NId + ",IC." + IndicatorClassifications.ICName + ",IC." + IndicatorClassifications.ICGId + ",IC." + IndicatorClassifications.ICGlobal + ",IC." + IndicatorClassifications.ICType + ",IC." + IndicatorClassifications.ICInfo + ",IC." + MergetTemplateConstants.Columns.COLUMN_UNMATCHED + ",IC." + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME +
               ", IC." + IndicatorClassifications.ICNId + " AS " + MergetTemplateConstants.Columns.COLUMNS_NEWICNID );

            sbQuery.Append(" FROM " + MergetTemplateConstants.TempTable.IndicatorClassification + " AS IC");

            sbQuery.Append(" WHERE IC." + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME + " = '" + sourceFileName + "'");

            sbQuery.Append(" AND IC." + IndicatorClassifications.ICType + "=" + DIQueries.ICTypeText[classificationType]);

            sbQuery.Append(" ORDER BY IC." + IndicatorClassifications.ICName);
            RetVal = sbQuery.ToString();
            return RetVal;
        }


        public string GetUnmatchedAreas(string sourceFileName)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT T." + MergetTemplateConstants.Columns.COLUMN_SRCNID + " AS " + Area.AreaNId + ", T." + Area.AreaParentNId + ",T." + Area.AreaName + ",T." + Area.AreaGId + ",T." + Area.AreaGlobal + ",T." + Area.AreaID + ",T." + Area.AreaBlock + ",T." + Area.AreaMap + ",T." + Area.AreaLevel + ",T." + MergetTemplateConstants.Columns.COLUMN_UNMATCHED);

            sbQuery.Append(" FROM " + MergetTemplateConstants.TempTable.Area + " as T ");

            sbQuery.Append(" WHERE T." + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME + " = '" + sourceFileName + "'");
            //"'  AND NOT EXISTS (SELECT * FROM " + Tables.Area + " AS I WHERE T." + Area.AreaID + " =I." + Area.AreaID +")" );
            // GROUP BY T." + Area.AreaID + ", T." + Area.AreaParentNId + ",T." + Area.AreaName + ",T." + Area.AreaGId + ",T." + Area.AreaGlobal + ",T." + Area.AreaParentNId +",T." + Area.AreaBlock + ",T." + Area.AreaMap + ",T." + Area.AreaLevel+ ",T." + Constants.Columns.COLUMN_UNMATCHED);

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        #endregion
       
        public string GetAvailableIndicator()
        {
            string RetVal = string.Empty;
            RetVal = "SELECT " + Indicator.IndicatorNId + "," + Indicator.IndicatorName + "," + Indicator.IndicatorGId + "," 
                    + Indicator.IndicatorGlobal + " FROM " + Tables.Indicator ;
            return RetVal;
        }

        public string GetAvailableUnit()
        {
            string RetVal = string.Empty;
            RetVal = "SELECT " + Unit.UnitNId + "," + Unit.UnitName + "," + Unit.UnitGId + "," + Unit.UnitGlobal
                    + " FROM " + Tables.Unit;
            return RetVal;
        }

        public string GetAvailableSubgroupVals()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT " + SubgroupVals.SubgroupValNId + "," + SubgroupVals.SubgroupVal + "," + SubgroupVals.SubgroupValGId + "," + SubgroupVals.SubgroupValGlobal 
                    + " FROM " + Tables.SubgroupVals;

            return RetVal;
        }

        public string GetAvailableSubgroupDimensions()
        {
            string RetVal = string.Empty;
            RetVal = "SELECT " + SubgroupTypes.SubgroupTypeNId + "," + SubgroupTypes.SubgroupTypeName + "," + SubgroupTypes.SubgroupTypeGID + "," + SubgroupTypes.SubgroupTypeGlobal + " FROM " + Tables.SubgroupType;
            return RetVal;
        }

        public string GetAvailableSubgroupDimValues()
        {
            string RetVal = string.Empty;
            RetVal = "SELECT " + Subgroup.SubgroupNId + "," + Subgroup.SubgroupName + "," + Subgroup.SubgroupGId + "," + Subgroup.SubgroupGlobal  + " FROM " + Tables.Subgroup;
            return RetVal;
        }

        public string ClearAllUnmatchedAreas()
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("UPDATE " + MergetTemplateConstants.TempTable.Area + " SET " + MergetTemplateConstants.Columns.COLUMN_UNMATCHED + "= 0");

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        public string GetAvailableAreas()
        {
            string RetVal = string.Empty;
            RetVal = "SELECT " + Area.AreaNId + "," + Area.AreaName + "," + Area.AreaID + "," + Area.AreaGId + "," + Area.AreaGlobal + "," + Area.AreaLevel + "," + Area.AreaMap +  " FROM " + Tables.Area;
            return RetVal;
        }

        public string ClearAllUnmatchedIC()
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("Update " + MergetTemplateConstants.TempTable.IndicatorClassification + " AS IC SET IC." + MergetTemplateConstants.Columns.COLUMN_UNMATCHED + "= 0");

            RetVal = sbQuery.ToString();
            return RetVal;
        }

     

        #region "-- Imports --"

        public string GetImportIndicator(string nids)
        {
            string RetVal = string.Empty;

            RetVal = "SELECT * FROM " + MergetTemplateConstants.TempTable.Indicator + " AS T WHERE  " + Indicator.IndicatorNId + " IN(" + nids + ") ORDER BY " + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME ;
            
            return RetVal;
        }

        public string GetImportUnits(string nids)
        {
            string RetVal = string.Empty;

            RetVal = "SELECT * FROM " + MergetTemplateConstants.TempTable.Unit + " AS T WHERE  " + Unit.UnitNId  + " IN(" + nids + ") ORDER BY " + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME;

            return RetVal;
        }

        public string GetImportSubgroupDimensionValues(string nids)
        {
            string RetVal = string.Empty;

            RetVal = "SELECT * FROM " + MergetTemplateConstants.TempTable.Subgroup  + " AS T WHERE  " + Subgroup.SubgroupNId  + " IN(" + nids + ") ORDER BY " + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME;

            return RetVal;
        }

        public string GetImportSubgroupVals(string nids)
        {
            string RetVal = string.Empty;

            RetVal = "SELECT * FROM " + MergetTemplateConstants.TempTable.SubgroupVals + " AS T WHERE  " + SubgroupVals.SubgroupValNId + " IN(" + nids + ") ORDER BY " + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME;

            return RetVal;
        }

        public string GetImportSubgroupDimensions(string nids)
        {
            string RetVal = string.Empty;

            RetVal = "SELECT * FROM " + MergetTemplateConstants.TempTable.SubgroupType + " AS T WHERE  " + SubgroupTypes.SubgroupTypeNId + " IN(" + nids + ") ORDER BY " + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME;

            return RetVal;
        }

        public string GetImportAreas(string sourceFileWpath,string nids)
        {
            string RetVal = string.Empty;

            RetVal = "SELECT " + MergetTemplateConstants.Columns.COLUMN_SRCNID + " AS " + Area.AreaNId + ", " + Area.AreaParentNId + "," + Area.AreaName + "," + Area.AreaGId + "," + Area.AreaGlobal + "," + Area.AreaID + "," + Area.AreaBlock + "," + Area.AreaMap + "," + Area.AreaLevel + " FROM " + MergetTemplateConstants.TempTable.Area + " AS T WHERE  " + MergetTemplateConstants.Columns.COLUMN_SRCNID + " IN(" + nids + ") And  " + MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME + "='" + sourceFileWpath + "'";

            return RetVal;
        }

        public string GetImportAreas( )
        {
            string RetVal = string.Empty;

            RetVal = "SELECT " + MergetTemplateConstants.Columns.COLUMN_SRCNID + " AS " + Area.AreaNId + ", " + Area.AreaParentNId + "," + Area.AreaName + "," + Area.AreaGId + "," + Area.AreaGlobal + "," + Area.AreaID + "," + Area.AreaBlock + "," + Area.AreaMap + "," + Area.AreaLevel + " FROM " + MergetTemplateConstants.TempTable.Area + " AS T ";

            return RetVal;
        }

        public string GetImportICs( string nids)
        {
            string RetVal = string.Empty;

            RetVal = "SELECT IC." + MergetTemplateConstants.Columns.COLUMN_SRCNID + " AS " + IndicatorClassifications.ICNId + " ,IC." + IndicatorClassifications.ICParent_NId + ",IC." + IndicatorClassifications.ICName +" , IC."+ MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME + ",IC." + IndicatorClassifications.ICGId + ",IC." + IndicatorClassifications.ICGlobal + ",IC." + IndicatorClassifications.ICType + ",IC." + IndicatorClassifications.ICInfo + " FROM " + MergetTemplateConstants.TempTable.IndicatorClassification + " AS IC  WHERE  IC." + IndicatorClassifications.ICNId + " IN(" + nids + ") ORDER BY "+
                MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME;

            return RetVal;
        }

        #endregion

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
           //-- dispose
        }

        #endregion

        #endregion

       
    }
}
