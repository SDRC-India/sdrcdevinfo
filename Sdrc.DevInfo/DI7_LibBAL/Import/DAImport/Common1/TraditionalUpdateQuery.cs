using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.Import.DAImport;
using DevInfo.Lib.DI_LibBAL.Import.DAImport.Common;

using DevInfo.Lib.DI_LibBAL.DA.DML;

namespace DevInfo.Lib.DI_LibBAL.Import.DAImport.Common
{
    /// <summary>
    /// This class, implements IUpdateQuery, contains all the "UPDATE" SQL queries (having JOINS) required in Import process.
    /// <para>All Update queries are in traditional format , supported in MSACCESS, MYSQL - RDBMS.</para>
    /// <para>Update query format: UPDATE TABLE1 AS T1 INNER JOIN TABLE2 AS T2 ON T1.C1 = T2.C2  SET column = value </para>
    /// </summary>
   internal class TraditionalUpdateQuery : IUpdateQuery
    {

       #region "-- Private --"
       DIQueries DBQueries = null;
       #endregion

       #region "-- New / Dispose --"
       internal TraditionalUpdateQuery(DIQueries dbQueries)
       {
           this.DBQueries = dbQueries;
       }
       #endregion


        #region IUpdateQuery Members

       public string UpdateNIDForIndicatorGID()
        {

            string RetVal = string.Empty;
            // 1. Update Indicator Nids where Indicator Gids matches:
            RetVal = "UPDATE " + Constants.TempDataTableName + " AS TD INNER JOIN " + this.DBQueries.TablesName.Indicator + " AS Ind ON TD." + Indicator.IndicatorGId + " = Ind." + Indicator.IndicatorGId +
                " SET TD." + Indicator.IndicatorNId + " = Ind." + Indicator.IndicatorNId;
            return RetVal;
        }

       public string UpdateNIDForSubgroupGID()
        {
            string RetVal = string.Empty;
            //  Update Unit Nids where Subgroup  Gids matches:
            RetVal = "UPDATE " + Constants.TempDataTableName + " AS TD INNER JOIN " + this.DBQueries.TablesName.SubgroupVals + " AS SG ON TD." + SubgroupVals.SubgroupValGId + " = SG." + SubgroupVals.SubgroupValGId +
                " SET TD." + SubgroupVals.SubgroupValNId + " = SG." + SubgroupVals.SubgroupValNId;
            return RetVal;
        }

       public string UpdateNIDForUnitGID()
        {
            string RetVal = string.Empty;

            //  Update Subgroup Nids where Unit Gids matches:
            RetVal = "UPDATE " + Constants.TempDataTableName + " AS TD INNER JOIN " + this.DBQueries.TablesName.Unit + " AS U ON TD." + Unit.UnitGId + " = U." + Unit.UnitGId +
                " SET TD." + Unit.UnitNId + " = U." + Unit.UnitNId;
            return RetVal;
        }

       public string UpdateNIDForIndicatorName()
        {
            string RetVal = string.Empty;
            //  Update Nids where Names matches:

            // 1.Update Nid where Indicator Name matches.
            RetVal = "UPDATE " + Constants.TempDataTableName + " AS TD INNER JOIN " + this.DBQueries.TablesName.Indicator + " AS Ind ON TD." + Indicator.IndicatorName + " = Ind." + Indicator.IndicatorName +
                " SET TD." + Indicator.IndicatorNId + " = Ind." + Indicator.IndicatorNId +
            " WHERE (TD." + Indicator.IndicatorNId + " Is Null)";
            return RetVal;
        }

       public string UpdateNIDForUnitName()
        {
            string RetVal = string.Empty;

            // 1.Update Nid where unit Name matches.
            RetVal = "UPDATE " + Constants.TempDataTableName + " AS TD INNER JOIN " + this.DBQueries.TablesName.Unit + " AS U ON TD." + Unit.UnitName + " = U." + Unit.UnitName +
                " SET TD." + Unit.UnitNId + " = U." + Unit.UnitNId +
            " WHERE (TD." + Unit.UnitNId + " Is Null)";
            return RetVal;
        }

       public string UpdateNIDForSubgroupValName()
        {
            string RetVal = string.Empty;

            // 1.Update Nid where SubgroupVal Name matches.
            RetVal = "UPDATE " + Constants.TempDataTableName + " AS TD INNER JOIN " + this.DBQueries.TablesName.SubgroupVals + " AS SG ON TD." + SubgroupVals.SubgroupVal + " = SG." + SubgroupVals.SubgroupVal +
                " SET TD." + SubgroupVals.SubgroupValNId + " = SG." + SubgroupVals.SubgroupValNId +
            " WHERE ((TD." + SubgroupVals.SubgroupValNId + ") Is Null)";
            return RetVal;
        }

       public string UpdateNIDForAreaNId()
        {
            string RetVal = string.Empty;

            // 1.Update Nid where Area Name matches.
            RetVal = "UPDATE " + Constants.TempDataTableName + " AS TD INNER JOIN " + this.DBQueries.TablesName.Area + " AS Area ON TD." + Area.AreaID + " = Area." + Area.AreaID +
                 " SET TD." + Area.AreaNId + " = Area." + Area.AreaNId +
            " WHERE ((TD." + Area.AreaNId + ") Is Null)";

            return RetVal;
        }

       public string UpdateIUSNidofMatchedRecords()
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + Constants.TempDataTableName + " AS TD INNER JOIN " + this.DBQueries.TablesName.IndicatorUnitSubgroup + " AS IUS ON (TD." + Indicator.IndicatorNId + " = IUS." + Indicator.IndicatorNId + ") AND (TD." + SubgroupVals.SubgroupValNId + " = IUS." + SubgroupVals.SubgroupValNId + ") AND (TD." + Unit.UnitNId + " = IUS." + Unit.UnitNId + ") " +
                    " SET TD." + Constants.NewIUSColumnName + " = [IUS].[" + Indicator_Unit_Subgroup.IUSNId + "]";


            return RetVal;
        }

       public string UpdateUnmatchedIUSNidFromTempUnmatchedIUSTable(bool updateByGID)
        {
            string RetVal = string.Empty;
            if (updateByGID)
            {
                RetVal = "UPDATE " + Constants.TempDataTableName + " AS TD INNER JOIN " + Constants.TempUnmatchedIUSTable + " AS IUS ON (TD.Indicator_GID = IUS.Indicator_GID) AND (TD.Unit_GID = IUS.Unit_GID) AND (TD.Subgroup_Val_GID = IUS.Subgroup_Val_GID) SET TD.IUSNid = IUS.Unmatched_IUSNid";
            }
            else
            {
                RetVal = "UPDATE " + Constants.TempDataTableName + " AS TD INNER JOIN " + Constants.TempUnmatchedIUSTable + " AS IUS ON (TD.Indicator_Name = IUS.Indicator_Name) AND (TD.Unit_Name = IUS.Unit_Name) AND (TD.Subgroup_Val = IUS.Subgroup_Val) SET TD.IUSNid = IUS.Unmatched_IUSNid";
            }
            return RetVal;
        }

       public string UpdateTimePeriodNid()
        {
            String RetVal = string.Empty;
            // Update TimePeriod Nid in Temp_Data. 
            RetVal = "Update " + Constants.TempDataTableName + " AS TD Inner JOIN " + this.DBQueries.TablesName.TimePeriod + " AS TP ON TD." + Timeperiods.TimePeriod + " = TP." + Timeperiods.TimePeriod +
                        " Set TD." + Timeperiods.TimePeriodNId + " = TP." + Timeperiods.TimePeriodNId;
            return RetVal;
        }

       public string UpdateFootNoteNid()
        {
            String RetVal = string.Empty;
            // 
            RetVal = "Update " + Constants.TempDataTableName + " AS TD, " + this.DBQueries.TablesName.FootNote + " AS FN " +
                   " Set TD." + FootNotes.FootNoteNId + " = FN." + FootNotes.FootNoteNId + " where TD." + FootNotes.FootNote + " = FN." + FootNotes.FootNote;
            return RetVal;
        }

       public string UpdateSourceNidInTempData()
        {
            String RetVal = string.Empty;
            RetVal = "Update " + Constants.TempDataTableName + " AS TD , " + this.DBQueries.TablesName.IndicatorClassifications + " AS IC " +
                        " Set TD." + Data.SourceNId + " = IC." + IndicatorClassifications.ICNId + " where TD." + Constants.SourceColumnName + " = IC." + IndicatorClassifications.ICName;
            return RetVal;
        }

       public string UpdateICGlobal(string ICTableName)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE  " + ICTableName + " AS IC INNER JOIN " + Constants.TempDataTableName + " as  TD ON TD." + Data.SourceNId + "= IC." + IndicatorClassificationsIUS.ICNId + " SET IC." + IndicatorClassifications.ICGlobal + " = TD." + IndicatorClassifications.ICGlobal + " where TD." + IndicatorClassifications.ICGlobal + " <>0";
            return RetVal;
        }

       public string UpdateDataValue()
        {
            String RetVal = string.Empty;
            // Update DataValue in UT_Data table where IUSNid, AreaNid and TimeperiodNid matches with Temp_Data Table.
            RetVal = "Update " + this.DBQueries.TablesName.Data + " AS DATA Inner JOIN " + Constants.TempDataTableName + " AS TD ON (TD." + Data.TimePeriodNId + " = DATA." + Data.TimePeriodNId + ") AND (TD." + Data.AreaNId + " = DATA." + Data.AreaNId + ") AND (TD." + Constants.NewIUSColumnName + " = DATA." + Data.IUSNId + ") AND ( TD." + Data.SourceNId + " = DATA." + Data.SourceNId + ")" +
                        " Set DATA." + Data.DataValue + " = TD." + Data.DataValue + " , DATA." + Data.FootNoteNId + " = IIF(LEN(TD." + Data.FootNoteNId + ") = 0, -1, TD." + Data.FootNoteNId + ") , DATA." + Data.DataDenominator + " = TD." + Data.DataDenominator;
            return RetVal;
        }

       public string UpdateIUSNidOfDuplicateRecord()
        {
            string RetVal = string.Empty;

            RetVal = " Update " + Constants.TempDataTableName + " T1 , DuplicateTemp T2 " +
               " Set T1." + Constants.NewIUSColumnName + " = -1 " +
                 " where  T1." + Area.AreaNId + " =  T2." + Area.AreaNId + " And T1." + Timeperiods.TimePeriod + " = T2." + Timeperiods.TimePeriod + " And T1." + Constants.SourceColumnName + " = T2." + Constants.SourceColumnName + " And T1." + Constants.NewIUSColumnName + " = T2." + Constants.NewIUSColumnName;

            return RetVal;
        }

       public string CreateTempSheetTable()
       {
           string RetVal = string.Empty;

           RetVal = " Create Table " + Constants.TempSheetTableName + "(" + Timeperiods.TimePeriod + " text(30), " + Area.AreaID + " text(255), " + Area.AreaName + " text(60), " + Data.DataValue + " memo, " + SubgroupVals.SubgroupVal + " text(255), " + Constants.SourceColumnName + " memo, " + FootNotes.FootNote + " memo, " + Data.DataDenominator + " long, " + Indicator.IndicatorName + " text(255), " + Unit.UnitName + " text(128), " + Indicator.IndicatorGId + " text(60), " + Unit.UnitGId + " text(60), " + Constants.DecimalColumnName + " text(60)," + SubgroupVals.SubgroupValGId + " text(60), " + Constants.Log.SkippedSourceFileColumnName + " text(255), NID  counter  )";

           return RetVal;
       }

        #endregion
    }
}
