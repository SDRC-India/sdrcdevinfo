using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibDAL.Queries;

namespace DevInfo.Lib.DI_LibBAL.DA.Reports.ComparisonReport
{
    internal static class ComparisonReportQuery
    {
        /// <summary>
        /// Get Missing Indicator Present In Database with respect To other DB.
        /// </summary>
        /// <returns></returns>
        internal static string MissingIndicators()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT I." + Indicator.IndicatorName + " FROM " + DataBaseComparisonReportGenerator.DBQueries.TablesName.Indicator
              + " AS I WHERE I." + Indicator.IndicatorGId + " NOT IN ( SELECT T." + Indicator.IndicatorGId + " FROM "
              + Constants.TempTables.TempTablePrefix + DataBaseComparisonReportGenerator.DBQueries.TablesName.Indicator
              + " AS T WHERE T." + Indicator.IndicatorGId + " = I." + Indicator.IndicatorGId + ") ORDER BY I." + Indicator.IndicatorName;


            return RetVal;
        }

        /// <summary>
        /// Get Additional Indicator Present in Database with respect To other DB.
        /// </summary>
        /// <returns></returns>
        internal static string AdditionalIndicators()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT T." + Indicator.IndicatorName + " FROM " + Constants.TempTables.TempTablePrefix
               + DataBaseComparisonReportGenerator.DBQueries.TablesName.Indicator + " AS T WHERE T." + Indicator.IndicatorGId
               + " NOT IN ( SELECT I." + Indicator.IndicatorGId + " FROM "
               + DataBaseComparisonReportGenerator.DBQueries.TablesName.Indicator + " AS I WHERE I."
               + Indicator.IndicatorGId + " = T." + Indicator.IndicatorGId + ") ORDER BY T." + Indicator.IndicatorName;

            return RetVal;
        }

        /// <summary>
        /// Get Missing Indicator Present In Database with respect To other DB.
        /// </summary>
        /// <param name="tableName">Database Table Name</param>
        /// <param name="tempTable">Temp Table Name with TempPrefix </param>
        /// <param name="columnName">Column Name To Filter</param>
        /// <param name="columnGid">Column name for GID comparison </param>
        /// <returns></returns>
        internal static string GetMissingRecords(string tableName, string tempTable, string columnName, string columnGid)
        {
            string RetVal = string.Empty;

            RetVal = "SELECT I." + columnName + " FROM " + tableName
                   + " AS I WHERE  NOT EXISTS( SELECT * FROM "
                   + tempTable + " AS T WHERE T." + columnGid + " = I." + columnGid + ") ORDER BY I." + columnName;


            return RetVal;
        }

        /// <summary>
        /// Get Additional Indicator Present in Database with respect To other DB.
        /// </summary>
        /// <param name="tableName">Database Table Name</param>
        /// <param name="tempTable">Temp Table Name with TempPrefix </param>
        /// <param name="columnName">Column Name To Filter</param>
        /// <param name="columnGid">Column name for GID comparison </param>
        /// <returns></returns>
        internal static string GetAdditionalRecords(string tableName, string tempTable, string columnName, string columnGid)
        {
            string RetVal = string.Empty;

            RetVal = "SELECT T." + columnName + " FROM " + tempTable + " AS T WHERE "
                    + " NOT EXISTS ( SELECT * FROM "
                    + tableName + " AS I WHERE I."
                    + columnGid + " = T." + columnGid + ")ORDER BY T." + columnName;

            return RetVal;
        }

        /// <summary>
        /// Get Missing Indicator Classification Present In Database with respect To other DB.
        /// </summary>
        /// <param name="tableName">Database Table Name</param>
        /// <param name="tempTable">Temp Table Name with TempPrefix </param>
        /// <param name="columnName">Column Name To Filter</param>
        /// <param name="columnGid">Column name for GID comparison </param>
        /// <returns></returns>
        internal static string GetMissingRecords(string tableName, string tempTable, string columnName, string columnGid, ICType icType)
        {
            string RetVal = string.Empty;

            RetVal = "SELECT I." + columnName + " FROM " + tableName
                   + " AS I WHERE I." + columnGid + " NOT IN ( SELECT T." + columnGid + " FROM "
                   + tempTable + " AS T WHERE T." + columnGid + " = I." + columnGid + ")"
                   + " AND I." + IndicatorClassifications.ICType + " = " + DIQueries.ICTypeText[icType]
                   + " ORDER BY I." + columnName;


            return RetVal;
        }

        /// <summary>
        /// Get Additional Indicator Classification Present in Database with respect To other DB.
        /// </summary>
        /// <param name="tableName">Database Table Name</param>
        /// <param name="tempTable">Temp Table Name with TempPrefix </param>
        /// <param name="columnName">Column Name To Filter</param>
        /// <param name="columnGid">Column name for GID comparison </param>
        /// <returns></returns>
        internal static string GetAdditionalRecords(string tableName, string tempTable, string columnName, string columnGid, ICType icType)
        {
            string RetVal = string.Empty;

            RetVal = "SELECT T." + columnName + " FROM " + tempTable + " AS T WHERE T." + columnGid
                    + " NOT IN ( SELECT I." + columnGid + " FROM "
                    + tableName + " AS I WHERE I."
                    + columnGid + " = T." + columnGid + ")"
                    + " AND T." + IndicatorClassifications.ICType + " = " + DIQueries.ICTypeText[icType]
                    + " ORDER BY T." + columnName;

            return RetVal;
        }


        /// <summary>
        ///  Get Additional Timeperiod with respect to other db.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="tempTable"></param>
        /// <param name="columnName"></param>
        /// <param name="columnGid"></param>
        /// <returns></returns>
        internal static string GetAdditionalTimeperiod()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT T." + Timeperiods.TimePeriod + " FROM " + Constants.TempTables.TempTablePrefix + DataBaseComparisonReportGenerator.DBQueries.TablesName.TimePeriod + " AS T WHERE T." + Timeperiods.TimePeriod + " NOT IN ( SELECT I." + Timeperiods.TimePeriod + " FROM " + DataBaseComparisonReportGenerator.DBQueries.TablesName.TimePeriod + " AS I WHERE I." + Timeperiods.TimePeriod + " = T." + Timeperiods.TimePeriod + ") ORDER BY T." + Timeperiods.TimePeriod;

            return RetVal;
        }

        /// <summary>
        /// Get Missing Timeperiod with respect to other db.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="tempTable"></param>
        /// <param name="columnName"></param>
        /// <param name="columnGid"></param>
        /// <returns></returns>
        internal static string GetMissingTimeperiod()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT I." + Timeperiods.TimePeriod + " FROM " + DataBaseComparisonReportGenerator.DBQueries.TablesName.TimePeriod + " AS I WHERE I." + Timeperiods.TimePeriod + " NOT IN ( SELECT T." + Timeperiods.TimePeriod + " FROM " + Constants.TempTables.TempTablePrefix + DataBaseComparisonReportGenerator.DBQueries.TablesName.TimePeriod + " AS T WHERE T." + Timeperiods.TimePeriod + " = I." + Timeperiods.TimePeriod + ")ORDER BY I." + Timeperiods.TimePeriod;

            return RetVal;
        }

        ///////// <summary>
        ///////// Get missing IUS
        ///////// </summary>
        ///////// <returns></returns>
        //////internal static string GetMissingIUS()
        //////{
        //////    string RetVal = string.Empty;
        //////    RetVal = "SELECT TIUS.IUSNId, TS.Subgroup_Val, TI.Indicator_Name, TU.Unit_Name  FROM UT_Subgroup_Vals_en AS TS,UT_Indicator_Unit_Subgroup AS TIUS ,UT_Indicator_en AS TI , UT_Unit_en AS TU  where TS.Subgroup_Val_NId = TIUS.Subgroup_Val_NId AND  TIUS.Unit_NId = TU.Unit_NId 	AND TIUS.Indicator_NId = TI.Indicator_NId and   Not  exists (SELECT  TS.Subgroup_Val_GId, TI.Indicator_GId, TU.Unit_GId FROM  UT_Subgroup_Vals_en AS TS, UT_Indicator_Unit_Subgroup AS TIUS , UT_Indicator_en AS TI ,UT_Unit_en AS TU, (SELECT  TS.Subgroup_Val_GId, TI.Indicator_GId, TU.Unit_GId FROM (Temp_UT_Subgroup_Vals_en  AS TS INNER JOIN (Temp_UT_Indicator_Unit_Subgroup AS TIUS INNER JOIN Temp_UT_Indicator_en AS TI ON TIUS.Indicator_NId =  TI.Indicator_NId) ON TS.Subgroup_Val_NId = TIUS.Subgroup_Val_NId) INNER JOIN Temp_UT_Unit_en AS TU ON TIUS.Unit_NId = TU.Unit_NId) AS A  WHERE  TS.Subgroup_Val_NId = TIUS.Subgroup_Val_NId  AND  TIUS.Unit_NId = TU.Unit_NId AND TIUS.Indicator_NId = TI.Indicator_NId AND   TS.Subgroup_Val_GId = A.Subgroup_Val_GId AND  TI.Indicator_GId = A.Indicator_GId AND  TU.Unit_GId = A.Unit_GId) ";
        //////    return RetVal;
        //////}

        ///////// <summary>
        ///////// Get Additional IUS
        ///////// </summary>
        ///////// <returns></returns>
        //////internal static string GetAdditionalIUS()
        //////{
        //////    string RetVal = string.Empty;

        //////    RetVal = "SELECT TIUS.IUSNId, TS.Subgroup_Val, TI.Indicator_Name, TU.Unit_Name "
        //////       + " FROM Temp_UT_Subgroup_Vals_en AS TS,Temp_UT_Indicator_Unit_Subgroup AS TIUS ,Temp_UT_Indicator_en AS TI ,Temp_UT_Unit_en AS TU "
        //////       + " where TS.Subgroup_Val_NId = TIUS.Subgroup_Val_NId AND  TIUS.Unit_NId = TU.Unit_NId 	AND TIUS.Indicator_NId = TI.Indicator_NId and   Not  exists "
        //////       + "(SELECT  TS.Subgroup_Val_GId, TI.Indicator_GId, TU.Unit_GId FROM  UT_Subgroup_Vals_en AS TS, UT_Indicator_Unit_Subgroup AS TIUS , UT_Indicator_en AS TI ,UT_Unit_en AS TU, "
        //////       + "(SELECT  TS.Subgroup_Val_GId, TI.Indicator_GId, TU.Unit_GId FROM (Temp_UT_Subgroup_Vals_en  AS TS INNER JOIN (Temp_UT_Indicator_Unit_Subgroup AS TIUS INNER JOIN Temp_UT_Indicator_en AS TI ON TIUS.Indicator_NId =  TI.Indicator_NId) ON TS.Subgroup_Val_NId = TIUS.Subgroup_Val_NId) INNER JOIN Temp_UT_Unit_en AS TU ON TIUS.Unit_NId = TU.Unit_NId) AS A "
        //////       + " WHERE  TS.Subgroup_Val_NId = TIUS.Subgroup_Val_NId  AND  TIUS.Unit_NId = TU.Unit_NId AND TIUS.Indicator_NId = TI.Indicator_NId AND   TS.Subgroup_Val_GId = A.Subgroup_Val_GId AND  TI.Indicator_GId = A.Indicator_GId AND  TU.Unit_GId = A.Unit_GId) ";

        //////    return RetVal;
        //////}

        #region "-- IUS Comparison --"

        /// <summary>
        /// Create a Blank Temporary Table from Reference Databse
        /// </summary>
        /// <returns></returns>
        internal static string CreateTempIUSTable()
        {
            string RetVal = string.Empty;

            RetVal = "Select 0 as S_IUS_NId, '' as S_I_GId, '' as S_U_GId, '' as S_SG_GId,  '' as S_I_N, '' as S_U_N, '' as S_SG_N, false as Mapped, 0 as M_IUS_NId, '' as M_I_N, '' as M_U_N, '' as M_SG_N,'' as SrcDBName  INTO "
                + Constants.TempTables.TempIUSTable + " FROM "
                + DataBaseComparisonReportGenerator.DBQueries.TablesName.IndicatorUnitSubgroup + " WHERE 1=2";

            return RetVal;

        }

        /// <summary>
        /// Insert IUS Into Temporary table
        /// </summary>
        /// <returns></returns>
        internal static string InsertIntoIUSTable()
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + Constants.TempTables.TempIUSTable
                + " (S_I_GId, S_U_GId, S_SG_GId, S_I_N, S_U_N, S_SG_N, Mapped,SrcDBName) SELECT l_I."
                + Indicator.IndicatorGId + ", l_U." + Unit.UnitGId + ", l_SG." + SubgroupVals.SubgroupValGId + ", l_I."
                + Indicator.IndicatorName + ", l_U." + Unit.UnitName + ", l_SG." + SubgroupVals.SubgroupVal
                + ", False as Mapped,'' FROM " + Constants.TempTables.TempTablePrefix
                + DataBaseComparisonReportGenerator.TargetDBQueries.TablesName.Indicator + " AS l_I, "
                + Constants.TempTables.TempTablePrefix + DataBaseComparisonReportGenerator.TargetDBQueries.TablesName.IndicatorUnitSubgroup
                + " AS l_IUS," + Constants.TempTables.TempTablePrefix
                + DataBaseComparisonReportGenerator.TargetDBQueries.TablesName.SubgroupVals + " AS l_SG,"
                + Constants.TempTables.TempTablePrefix + DataBaseComparisonReportGenerator.TargetDBQueries.TablesName.Unit + " AS l_U "
                + " WHERE l_I." + Indicator.IndicatorNId + "= l_IUS." + Indicator_Unit_Subgroup.IndicatorNId
                + " AND l_IUS." + Indicator_Unit_Subgroup.SubgroupValNId + "= l_SG." + SubgroupVals.SubgroupValNId
                + " AND l_IUS." + Indicator_Unit_Subgroup.UnitNId + "= l_U." + Unit.UnitNId;

            return RetVal;

        }

        internal static string UpdateIUSTable()
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE _IUS INNER JOIN (" + DataBaseComparisonReportGenerator.DBQueries.TablesName.SubgroupVals + " AS Avl_SG  INNER JOIN (" + DataBaseComparisonReportGenerator.DBQueries.TablesName.Unit + " AS Avl_U  INNER JOIN (" + DataBaseComparisonReportGenerator.DBQueries.TablesName.Indicator + " AS Avl_I  INNER JOIN " + DataBaseComparisonReportGenerator.DBQueries.TablesName.IndicatorUnitSubgroup  + " AS Avl_IUS ON Avl_I.Indicator_NId = Avl_IUS.Indicator_NId)  ON Avl_U.Unit_NId = Avl_IUS.Unit_NId) ON Avl_SG.Subgroup_Val_NId = Avl_IUS.Subgroup_Val_NId) ON  ([_IUS].S_I_GId = Avl_I.Indicator_GId) AND ([_IUS].S_U_GId = Avl_U.Unit_GId)  AND ([_IUS].S_SG_GId = Avl_SG.Subgroup_Val_GId) SET [_IUS].M_IUS_NId = Avl_IUS.IUSNId, [_IUS].Mapped = True";

            return RetVal;

        }

        internal static string SelectAdditionalIUS()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT S_I_N As  " + Indicator.IndicatorName + ", S_U_N As " + Unit.UnitName + ", S_SG_N As " + SubgroupVals.SubgroupVal + " FROM " + Constants.TempTables.TempIUSTable + " WHERE Mapped = 0";

            return RetVal;

        }

        internal static string SelectMissingIUS()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT I."+ Indicator.IndicatorName +",U."+ Unit.UnitName +",S."+ SubgroupVals.SubgroupVal 
               + " FROM "+ DataBaseComparisonReportGenerator.DBQueries.TablesName.IndicatorUnitSubgroup + " AS IUS, "
               + DataBaseComparisonReportGenerator.DBQueries.TablesName.Indicator +" AS I, " 
               + DataBaseComparisonReportGenerator.DBQueries.TablesName.SubgroupVals +" AS S, "
               + DataBaseComparisonReportGenerator.DBQueries.TablesName.Unit +" AS U " + " WHERE " 
               + " IUS." + Indicator_Unit_Subgroup.IndicatorNId + " = I." + Indicator.IndicatorNId 
               + " AND IUS." + Indicator_Unit_Subgroup.SubgroupValNId + " = S." + SubgroupVals.SubgroupValNId 
               + " AND IUS." + Indicator_Unit_Subgroup.UnitNId + "= U." + Unit.UnitNId 
               + " AND IUS."+ Indicator_Unit_Subgroup.IUSNId + " Not In " 
               + " (SELECT DISTINCT  M_IUS_NId  FROM " + Constants.TempTables.TempIUSTable + " WHERE M_IUS_NID IS NOT NULL)"
               +" ORDER BY I." + Indicator.IndicatorName +" ,U." + Unit.UnitName +",S."+ SubgroupVals.SubgroupVal;

            return RetVal;

        }

        #endregion


        #region "-- DATA --"

        /// <summary>
        ///  Step 1: create table TargetDataWithGIDS with two new columns : NewICNid and Mapped 
        /// </summary>
        /// <returns></returns>
        internal static string CreateTargetDataWithGIDSTable()
        {
            string RetVal = string.Empty;

            RetVal = " SELECT A." + Area.AreaID + ", A."+ Area.AreaName +", I."+ Indicator.IndicatorName + ", I."+ Indicator.IndicatorGId + ", S."+ SubgroupVals.SubgroupVal + ", S." + SubgroupVals.SubgroupValGId + ", T." + Timeperiods.TimePeriod + ", U." + Unit.UnitGId + ", U."+ Unit.UnitName + ", IC." + IndicatorClassifications.ICName + ",IC."+ IndicatorClassifications.ICGId + ", D."+ Data.DataValue + ", False AS Mapped INTO " + Constants.TempTables.TargetDataWithGIDs + " FROM "
                + Constants.TempTables.TempTablePrefix + DataBaseComparisonReportGenerator.TargetDBQueries.TablesName.Data + " D,"
                + Constants.TempTables.TempTablePrefix + DataBaseComparisonReportGenerator.TargetDBQueries.TablesName.IndicatorClassifications + " IC," + Constants.TempTables.TempTablePrefix + DataBaseComparisonReportGenerator.TargetDBQueries.TablesName.Indicator + " I, "
            + Constants.TempTables.TempTablePrefix + DataBaseComparisonReportGenerator.TargetDBQueries.TablesName.Area + " A, "
            + Constants.TempTables.TempTablePrefix + DataBaseComparisonReportGenerator.TargetDBQueries.TablesName.IndicatorUnitSubgroup + " IUS,"
            + Constants.TempTables.TempTablePrefix + DataBaseComparisonReportGenerator.TargetDBQueries.TablesName.SubgroupVals + " S,"
            + Constants.TempTables.TempTablePrefix + DataBaseComparisonReportGenerator.TargetDBQueries.TablesName.TimePeriod + " T,"
            + Constants.TempTables.TempTablePrefix + DataBaseComparisonReportGenerator.TargetDBQueries.TablesName.Unit + " U "
            + " WHERE A." + Area.AreaNId + "= D." + Data.AreaNId
            + " AND D." + Data.IUSNId +"= IUS." + Indicator_Unit_Subgroup.IUSNId + " AND I." + Indicator.IndicatorNId 
            + "= IUS." +Indicator_Unit_Subgroup.IndicatorNId
            + " AND IUS." + Indicator_Unit_Subgroup.SubgroupValNId + " = S." + SubgroupVals.SubgroupValNId 
            + " AND D." + Data.TimePeriodNId + " = T." + Timeperiods.TimePeriodNId 
            + " AND IUS."+ Indicator_Unit_Subgroup.UnitNId + " = U." + Unit.UnitNId 
            + " AND IC." + IndicatorClassifications.ICNId + "= D."+ Data.SourceNId ;

            return RetVal;
        }

        /// <summary>
        /// Step 2: create table ReferenceData_withGIDs with one new column named as Mapped.
        /// </summary>
        /// <returns></returns>
        internal static string CreateReferenceDataWithGIDTable()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT A."+ Area.AreaID + ", A." + Area.AreaName + ", I." + Indicator.IndicatorName + ", I." + Indicator.IndicatorGId 
                +", S."+ SubgroupVals.SubgroupVal + ", S."+ SubgroupVals.SubgroupValGId + ", U."+ Unit.UnitName + ", U." + Unit.UnitGId 
                + ", T."+ Timeperiods.TimePeriod + ", IC." + IndicatorClassifications.ICName + ",IC." + IndicatorClassifications.ICGId 
                + ", D." + Data.DataValue + ", False AS Mapped INTO " + Constants.TempTables.ReferenceData_withGIDs + " FROM "
                    + DataBaseComparisonReportGenerator.DBQueries.TablesName.Unit + " AS U INNER JOIN ( "
                    + DataBaseComparisonReportGenerator.DBQueries.TablesName.SubgroupVals + "  AS S INNER JOIN (( "
                    + DataBaseComparisonReportGenerator.DBQueries.TablesName.Indicator + "  AS I INNER JOIN  "
                    + DataBaseComparisonReportGenerator.DBQueries.TablesName.IndicatorUnitSubgroup + " AS IUS ON I." 
                    + Indicator.IndicatorNId + "= IUS."+ Indicator_Unit_Subgroup.IndicatorNId + ") INNER JOIN ( " 
                    + DataBaseComparisonReportGenerator.DBQueries.TablesName.TimePeriod + " AS T INNER JOIN ( "
                    + DataBaseComparisonReportGenerator.DBQueries.TablesName.IndicatorClassifications + "  AS IC INNER JOIN ( "
                    + DataBaseComparisonReportGenerator.DBQueries.TablesName.Area + "  AS A INNER JOIN  "
                    + DataBaseComparisonReportGenerator.DBQueries.TablesName.Data + " AS D ON A." + Area.AreaNId 
                    + "= D." + Data.AreaNId + ") ON IC." + IndicatorClassifications.ICNId + "= D."+ Data.SourceNId + ") ON T."
                    + Timeperiods.TimePeriodNId + " = D." + Data.TimePeriodNId + ") ON IUS." +Indicator_Unit_Subgroup.IUSNId 
                    +"= D."+ Data.IUSNId+ ") ON S."+ SubgroupVals.SubgroupValNId + "= IUS." + Indicator_Unit_Subgroup.SubgroupValNId 
                    + ") ON U." + Unit.UnitNId + "= IUS." + Indicator_Unit_Subgroup.UnitNId ;

            return RetVal;
        }

        /// <summary>
        /// Step 4: Update Reference and target tables set mapped to true where record exists in both table
        /// </summary>
        /// <returns></returns>
        internal static string UpdateRefTargetTable()
        {
            string RetVal = string.Empty;

            // bug fixed : IC mapping should be based on ICName
            RetVal = " UPDATE " + Constants.TempTables.ReferenceData_withGIDs + " AS R , " + Constants.TempTables.TargetDataWithGIDs 
                + " AS T  SET  T.Mapped = true, R.Mapped = True WHERE (R."+ Area.AreaID + "=T."+ Area.AreaID + ") AND (R.Indicator_GId=T.Indicator_GId) AND (R.Subgroup_Val_GId=T.Subgroup_Val_GId) AND (R.Unit_GId=T.Unit_GId) AND (R.TimePeriod=T.TimePeriod) AND (R.IC_Name=T.IC_Name) ";

            return RetVal;
        }



        ///////// <summary>
        /////////  Step 1: create table TargetDataWithGIDS with two new columns : NewICNid and Mapped 
        ///////// </summary>
        ///////// <returns></returns>
        //////internal static string CreateTargetDataWithGIDSTable()
        //////{
        //////    string RetVal = string.Empty;

        //////    RetVal = " SELECT A.Area_ID, A.Area_Name, I.Indicator_Name, I.Indicator_GId, S.Subgroup_Val, S.Subgroup_Val_GId, T.TimePeriod, U.Unit_GId, U.Unit_Name, IC.IC_Name, D.Data_Value, False AS Mapped, IC.IC_Name, -1 AS NewIC_NId INTO " + Constants.TempTables.TargetDataWithGIDs + " FROM "
        //////    + Constants.TempTables.TempTablePrefix + DataBaseComparisonReportGenerator.DBQueries.TablesName.IndicatorClassifications + " IC,"
        //////    + Constants.TempTables.TempTablePrefix + DataBaseComparisonReportGenerator.DBQueries.TablesName.Indicator + " I, "
        //////    + Constants.TempTables.TempTablePrefix + DataBaseComparisonReportGenerator.DBQueries.TablesName.Area + " A, "
        //////    + Constants.TempTables.TempTablePrefix + DataBaseComparisonReportGenerator.DBQueries.TablesName.Data + " D,"
        //////    + Constants.TempTables.TempTablePrefix + DataBaseComparisonReportGenerator.DBQueries.TablesName.IndicatorUnitSubgroup + " IUS,"
        //////    + Constants.TempTables.TempTablePrefix + DataBaseComparisonReportGenerator.DBQueries.TablesName.SubgroupVals + " S,"
        //////    + Constants.TempTables.TempTablePrefix + DataBaseComparisonReportGenerator.DBQueries.TablesName.TimePeriod + " T,"
        //////    + Constants.TempTables.TempTablePrefix + DataBaseComparisonReportGenerator.DBQueries.TablesName.Unit + " U "
        //////    + " WHERE A.Area_NId = D.Area_NId "
        //////    + " AND D.IUSNId = IUS.IUSNId AND I.Indicator_NId = IUS.Indicator_NId "
        //////    + " AND IUS.Subgroup_Val_NId = S.Subgroup_Val_NId "
        //////    + " AND D.TimePeriod_NId = T.TimePeriod_NId "
        //////    + " AND IUS.Unit_NId = U.Unit_NId AND IC.IC_NId = D.Source_NId ";

        //////    return RetVal;
        //////}

        /////////// <summary>
        /////////// Step 2: create table ReferenceData_withGIDs with one new column named as Mapped.
        /////////// </summary>
        /////////// <returns></returns>
        ////////internal static string CreateReferenceDataWithGIDTable()
        ////////{
        ////////    string RetVal = string.Empty;

        ////////    RetVal = "SELECT A.Area_ID, A.Area_Name, I.Indicator_Name, I.Indicator_GId, S.Subgroup_Val, S.Subgroup_Val_GId, U.Unit_Name, U.Unit_GId, T.TimePeriod, IC.IC_Name, D.Data_Value, False AS Mapped, IC.IC_NId INTO " + Constants.TempTables.ReferenceData_withGIDs + " FROM "
        ////////            + DataBaseComparisonReportGenerator.DBQueries.TablesName.Unit + " AS U INNER JOIN ( "
        ////////            + DataBaseComparisonReportGenerator.DBQueries.TablesName.SubgroupVals + "  AS S INNER JOIN (( "
        ////////            + DataBaseComparisonReportGenerator.DBQueries.TablesName.Indicator + "  AS I INNER JOIN  "
        ////////            + DataBaseComparisonReportGenerator.DBQueries.TablesName.IndicatorUnitSubgroup + " AS IUS ON I.Indicator_NId = IUS.Indicator_NId) INNER JOIN ( " + DataBaseComparisonReportGenerator.DBQueries.TablesName.TimePeriod + " AS T INNER JOIN ( "
        ////////            + DataBaseComparisonReportGenerator.DBQueries.TablesName.IndicatorClassifications + "  AS IC INNER JOIN ( "
        ////////            + DataBaseComparisonReportGenerator.DBQueries.TablesName.Area + "  AS A INNER JOIN  "
        ////////            + DataBaseComparisonReportGenerator.DBQueries.TablesName.Data + " AS D ON A.Area_NId = D.Area_NId) ON IC.IC_NId = D.Source_NId) ON T.TimePeriod_NId = D.TimePeriod_NId) ON IUS.IUSNId = D.IUSNId) ON S.Subgroup_Val_NId = IUS.Subgroup_Val_NId) ON U.Unit_NId = IUS.Unit_NId";

        ////////    return RetVal;
        ////////}

        ///////// <summary>
        ///////// Step 3: update NewICNid in TargetDataWithGIDs table 
        ///////// </summary>
        ///////// <returns></returns>
        //////internal static string UpdateTargetDataWithGIDsTable()
        //////{
        //////    string RetVal = string.Empty;

        //////    RetVal = "UPDATE " + DataBaseComparisonReportGenerator.DBQueries.TablesName.IndicatorClassifications + " AS IC, " + Constants.TempTables.TargetDataWithGIDs + " AS T SET T.NewIC_Nid = IC.IC_Nid  WHERE (((T.IC_Name)=[IC].[IC_Name]))";

        //////    return RetVal;
        //////}
        /////////// <summary>
        /////////// Step 4: Update Reference and target tables set mapped to true where record exists in both table
        /////////// </summary>
        /////////// <returns></returns>
        ////////internal static string UpdateRefTargetTable()
        ////////{
        ////////    string RetVal = string.Empty;

        ////////    RetVal = " UPDATE " + Constants.TempTables.ReferenceData_withGIDs + " AS R INNER JOIN " + Constants.TempTables.TargetDataWithGIDs + " AS T ON (R.Area_ID=T.Area_ID) AND (R.Indicator_GId=T.Indicator_GId) AND (R.Subgroup_Val_GId=T.Subgroup_Val_GId) AND (R.Unit_GId=T.Unit_GId) AND (R.TimePeriod=T.TimePeriod) AND (R.IC_NId=T.NewIC_NId) SET T.Mapped = true, R.Mapped = True";

        ////////    return RetVal;
        ////////}

        /// <summary>
        /// Step 5: Get records Avail in Target Not in Reference:  Additional records
        /// </summary>
        /// <returns></returns>
        internal static string GetAdditionalData()
        {
            string RetVal = string.Empty;
            //-- 
            RetVal = "SELECT " + Indicator.IndicatorName + "," + Unit.UnitName + "," + Timeperiods.TimePeriod + ","
                    + Area.AreaID + "," + Area.AreaName  + "," + Data.DataValue + "," + SubgroupVals.SubgroupVal + "," + IndicatorClassifications.ICName
                    + " FROM " + Constants.TempTables.TargetDataWithGIDs + " WHERE Mapped = false ORDER BY " + Indicator.IndicatorName;

            return RetVal;
        }

        /// <summary>
        /// Step 6: Get records Avail in Reference Not in Target: Missing
        /// </summary>
        /// <returns></returns>
        internal static string GetMissingData()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT " + Indicator.IndicatorName + "," + Unit.UnitName + "," + Timeperiods.TimePeriod + ","
                    + Area.AreaID + "," + Area.AreaName  + "," + Data.DataValue + "," + SubgroupVals.SubgroupVal + "," + IndicatorClassifications.ICName  
                    + " FROM " + Constants.TempTables.ReferenceData_withGIDs + " WHERE Mapped = false ORDER BY " + Indicator.IndicatorName;

            return RetVal;

        }

        #endregion

    }
}
