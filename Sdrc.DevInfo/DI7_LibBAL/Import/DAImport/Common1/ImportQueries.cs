using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;

using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.Import.DAImport;
using DevInfo.Lib.DI_LibBAL.Import.DAImport.Common;

using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibBAL.Import.DAImport.Common
{
    /// <summary>
    /// This class contains all required SQL queries for Import process.
    /// <para>NOTE: Client program needs to use different Update queries (having JOINS) for different databases like MS Access , SQL Sqerver, Oracle)</para>
    /// <para>To set right updateQuery, call explicitly "SetUpdateQueryObject()" method.</para>
    /// </summary>
    internal class ImportQueries
    {

        # region " -- Variables -- "
        protected DIQueries DBQueries;

        // IUpdateQuery object will be initialized in the basis of DBConnection Type.
        protected IUpdateQuery UpdateQueries;
        # endregion


        # region " -- New /Dispose -- "

        internal ImportQueries(DIQueries DBQueries)
        {
            this.DBQueries = DBQueries;

            // Set IUpdateQuery object, (By Default: Traditional)
            this.UpdateQueries = new TraditionalUpdateQuery(this.DBQueries);
        }

        # endregion

        #region "-- IUpdateQuery --"
        /// <summary>
        /// Sets the IUpdateQuery object on the basis of DBServer type.
        /// <para>Client program need to call this method explicitly.</para>
        /// </summary>
        /// <param name="serverType">Enum value for DIServer type.</param>
        internal void SetUpdateQueryObject(DIServerType serverType)
        {
            switch (serverType)
            {
                // In case of MSACCESS & MYSQL, use Traditional Update queries
                case DIServerType.Excel:
                case DIServerType.MsAccess:
                case DIServerType.MySql:
                    this.UpdateQueries = new TraditionalUpdateQuery(this.DBQueries);
                    break;
                // In case of SqlServer 2005 & Oracle, use new Update Queries.
                case DIServerType.Oracle:
                case DIServerType.SqlServer:
                    this.UpdateQueries = new SqlServerUpdateQuery(this.DBQueries);
                    break;
            }
        }

        #endregion

        # region " -- Queries -- "

        # region " -- Queries for Initiate Import process -- "

        /// <summary>
        /// SQL Query to create TempDataTable used to hold sourceDatabase's Data inn it.
        /// </summary>
        internal string CreateTempDataTable()
        {
            //Create Temp_Data table in Target database or template
            string RetVal = string.Empty;
            RetVal = "SELECT D.*, T.TimePeriod, A.Area_ID, A.Area_Name, SG.Subgroup_Val, SG.Subgroup_Val_GId, IC.IC_Name AS  Source, " +
               " F.FootNote, A.Area_Name AS  Decimals, I.Indicator_Name, I.Indicator_GId, U.Unit_Name, U.Unit_GId," +
 " 0 as Indicator_NId, 0 as Unit_NId, 0 as Subgroup_Val_NId, I.Indicator_Name as SourceFile, 0 as New_IUSNid, 0 as Old_Data_Nid, " +
 " 0 as OLD_IC_NId, IC.IC_Global,IC.IC_Global as RecommendedSource ,'' AS IC_IUS_Label" +
 " INTO TempDataTable " +
 " FROM " + this.DBQueries.TablesName.FootNote + " AS F INNER JOIN (" + this.DBQueries.TablesName.IndicatorClassifications + " AS IC INNER JOIN " +
 " (" + this.DBQueries.TablesName.Unit + " AS U INNER JOIN (" + this.DBQueries.TablesName.SubgroupVals + " AS SG INNER JOIN ((" +
 this.DBQueries.TablesName.Indicator + " AS I INNER JOIN " + this.DBQueries.TablesName.IndicatorUnitSubgroup + " AS IUS ON I.Indicator_NId = IUS.Indicator_NId) INNER JOIN (" + this.DBQueries.TablesName.TimePeriod + " AS T INNER JOIN (" +
 this.DBQueries.TablesName.Area + " AS A INNER JOIN " +
 this.DBQueries.TablesName.Data + " AS D ON A.Area_NId = D.Area_NId) ON T.TimePeriod_NId = D.TimePeriod_NId) ON IUS.IUSNId = D.IUSNId) ON SG.Subgroup_Val_NId = IUS.Subgroup_Val_NId) ON U.Unit_NId = IUS.Unit_NId) ON IC.IC_NId = D.Source_NId) ON F.FootNote_NId = D.FootNote_NId where 1=2;";

            #region "-- Old query --"
            //    RetVal = "SELECT D.*, '' as  " + Timeperiods.TimePeriod + ", '' as  " + Area.AreaID + ", '' as " + Area.AreaName + ", '' as   " + SubgroupVals.SubgroupVal + ", '' as " + SubgroupVals.SubgroupValGId + ", '' as " + Constants.SourceColumnName + ", FN." + FootNotes.FootNote + ", '' as " + Data.DataDenominator + ", '' as   Decimals,'' as " + Indicator.IndicatorName + ", '' as " + Indicator.IndicatorGId + ", " +
            //     " '' as " + Unit.UnitName + ", '' as " + Unit.UnitGId + ", 0 as " + Indicator.IndicatorNId + ", 0 as " + Unit.UnitNId + ", 0 as " + SubgroupVals.SubgroupValNId + " , 0 as " + Constants.NewIUSColumnName + ", '' as " + Constants.Log.SkippedSourceFileColumnName + ", 0 as " + Constants.Old_Data_Nid +",  0 as "+Constants.Old_Source_Nid  + ", " + IndicatorClassifications.ICGlobal + ", " + IndicatorClassifications.ICGlobal +  " as " + IndicatorClassificationsIUS.RecommendedSource + " INTO " + 
            //     Constants.TempDataTableName + " FROM " + this.DBQueries.TablesName.IndicatorClassifications + " As IC INNER JOIN(" +

            //     this.DBQueries.TablesName.Data + " AS D Inner Join " + this.DBQueries.TablesName.FootNote + " AS FN on D." + Data.FootNoteNId + " = FN." + FootNotes.FootNoteNId +") ON D.Source_NId = IC.IC_NId "+
            //     " where 1=0;";
            #endregion

            return RetVal;
        }

        /// <summary>
        /// SQL Query to Alter IUSNid column type from LongInt to Double
        /// </summary>
        internal string AlterIUSNidColumnTypeToDouble()
        {
            string Retval = string.Empty;
            //Retval = "ALTER TABLE " + Constants.TempDataTableName + " Alter " + Data.IUSNId + "  Double";
            Retval = "ALTER TABLE TEMPDATATABLE ALTER COLUMN [IUSNID] Float";

            return Retval;
        }

        /// <summary>
        /// SQL query to create Table TempSheetTable, schema is same as DevInfo spreadsheet columns , plus 5 columns . 
        /// </summary>
        internal string CreateTempSheetTable()
        {
            string Retval = string.Empty;
            Retval = this.UpdateQueries.CreateTempSheetTable();
            //Retval = "Create Table " + Constants.TempSheetTableName + "(" + Timeperiods.TimePeriod + " varchar, " + Area.AreaID + " varchar, " + Area.AreaName + " varchar, " + Data.DataValue + " memo, " + SubgroupVals.SubgroupVal + " varchar, " + Constants.SourceColumnName + " varchar, " + FootNotes.FootNote + " memo, " + Data.DataDenominator + " number, " + Indicator.IndicatorName + " varchar, " + Unit.UnitName + " varchar, " + Indicator.IndicatorGId + " varchar, " + Unit.UnitGId + " varchar, " + Constants.DecimalColumnName + " varchar," + SubgroupVals.SubgroupValGId + " varchar, " + Constants.Log.SkippedSourceFileColumnName + " varchar)";
            //Retval = "Create Table " + Constants.TempSheetTableName + "(F1 char(50), F2 varchar, F3 varchar, F4 memo, F5 varchar, F6 varchar, F7 memo, F8 number, " + Indicator.IndicatorName + " varchar, " + Unit.UnitName + " varchar, " + Indicator.IndicatorGId + " char(50), " + Unit.UnitGId + " char(50), Decimals varchar)";
            return Retval;
        }

        /// <summary>
        /// SQL query to updates TempDataTAble with specified IndicatorName, UnitName, IndicatoGID, UnitGID, Decinmals in respective columns
        /// </summary>
        internal string UpdateValuesInTempSheetTable(string indicatorName, string unitName, string indicatorGID, string unitGID, string decimals)
        {
            string Retval = string.Empty;
            Retval = "UPDATE " + Constants.TempSheetTableName + " SET " + Indicator.IndicatorName + " = '" + indicatorName + "' , " + Unit.UnitName + "= '" + unitName + "' ," + Indicator.IndicatorGId + "='" + indicatorGID + "' , " + Unit.UnitGId + " = '" + unitGID + "' , Decimals='" + decimals + "' ";
            return Retval;
        }

        /// <summary>
        /// SQL query to update TempSheetTable Rounding Data_Value till specified Decimal place only where Data_value is number.
        /// </summary>
        /// <param name="decimalValue">Decimal places required to round off the DataValue</param>
        internal string UpdateDataValueToRightDecimalInTempSheetTable(int decimalValue)
        {
            string Retval = string.Empty;
            string FormatString = "0";
            //Retval = "Update " + Constants.TempSheetTableName + " Set " + Data.DataValue + " =  str(Round(val(" + Data.DataValue + "), " + decimalValue + "))" +
            if (decimalValue > 0)
            {
                FormatString += ".";
            }
            for (int i = 0; i < decimalValue; i++)
            {
                FormatString += "0";
            }

            Retval = "Update " + Constants.TempSheetTableName + " Set " + Data.DataValue + " =  FORMAT(Round(val(" + Data.DataValue + "), " + decimalValue + "),'" + FormatString + "')" +
         " WHERE Isnumeric(" + Data.DataValue + ") = true and  instr(" + Data.DataValue + ",'.') > 0 ";

            return Retval;

        }


        /// <summary>
        /// SQL query to insert all Data from TempSheetTable  into TempDatatable.
        /// </summary>
        /// <returns></returns>
        internal string InsertTempSheetDataIntoTempdata()
        {
            string RetVal = string.Empty;


            RetVal = "Insert into " + Constants.TempDataTableName + "(" + Indicator.IndicatorName + ", " + Unit.UnitName + ", " + Indicator.IndicatorGId + ", " + Unit.UnitGId + "," + SubgroupVals.SubgroupValGId + ", Decimals, " + Timeperiods.TimePeriod + ", " + Area.AreaID + ", " + Area.AreaName + ", " + Data.DataValue + ", " + SubgroupVals.SubgroupVal + ", " + Constants.SourceColumnName + ", " + FootNotes.FootNote + ", " + Data.DataDenominator + ", " + Constants.Log.SkippedSourceFileColumnName + ") " +
                    " Select " + Indicator.IndicatorName + ", " + Unit.UnitName + ", " + Indicator.IndicatorGId + ", " + Unit.UnitGId + "," + SubgroupVals.SubgroupValGId + ", Decimals, " + Timeperiods.TimePeriod + ", " + Area.AreaID + ", " + Area.AreaName + ", " + Data.DataValue + ", " + SubgroupVals.SubgroupVal + ", " + Constants.SourceColumnName + ", " + FootNotes.FootNote + ", " + Data.DataDenominator + ", " + Constants.Log.SkippedSourceFileColumnName + " from " + Constants.TempSheetTableName;

            //RetVal = "Insert into " + Constants.TempDataTableName + "(" + Indicator.IndicatorName + ", " + Unit.UnitName + ", " + Indicator.IndicatorGId + ", " + Unit.UnitGId + ", Decimals, " + Timeperiods.TimePeriod + ", " + Area.AreaID + ", " + Area.AreaName + ", " + Data.DataValue + ", " + SubgroupVals.SubgroupVal + ", " + Constants.SourceColumnName + ", " + FootNotes.FootNote + ", " + Data.DataDenominator + ") " +
            //        " Select " + Indicator.IndicatorName + ", " + Unit.UnitName + ", " + Indicator.IndicatorGId + ", " + Unit.UnitGId + ", Decimals, F1, F2, F3, F4, F5, F6, F7, F8 from " + Constants.TempSheetTableName;

            return RetVal;
        }

        /// <summary>
        /// SQL Query to delete rows which are completely blank (i.e. no Data in all columns)
        /// </summary>
        internal string DeleteBlankRowsFromTempSheet()
        {
            string Retval = string.Empty;
            Retval = "Delete from " + Constants.TempSheetTableName + " where ("
                + Timeperiods.TimePeriod + " Is Null OR " + Timeperiods.TimePeriod + " ='') AND ("
                + Area.AreaID + " Is Null  OR " + Area.AreaID + " ='') AND ("
                + Area.AreaName + " Is Null   OR " + Area.AreaName + " ='') AND ("
                + Data.DataValue + " Is Null OR " + Data.DataValue + " = '') AND ("
                + SubgroupVals.SubgroupVal + " Is Null  OR " + SubgroupVals.SubgroupVal + " ='') AND ("
                + Constants.SourceColumnName + " Is Null  OR " + Constants.SourceColumnName + " ='') AND ("
                + FootNotes.FootNote + " Is Null  OR " + FootNotes.FootNote + " ='') AND ("
                + Data.DataDenominator + " Is Null OR " + Data.DataDenominator + " = 0)";
            return Retval;
        }

        internal string UpdateDataValuesToNullInTempSheet(string dataValueSymbol)
        {
            string Retval = string.Empty;
            Retval = "Update " + DevInfo.Lib.DI_LibBAL.Import.DAImport.Common.Constants.TempSheetTableName + " SET " + Data.DataValue + " = '000000' WHERE TRIM(" + Data.DataValue + ") = '" + DIQueries.RemoveQuotesForSqlQuery(dataValueSymbol) + "'";

            return Retval;
        }

        internal string DeleteFromTempSheetTable()
        {
            string RetVal = string.Empty;
            RetVal = "Delete from " + Constants.TempSheetTableName;
            return RetVal;
        }
        # endregion

        # region " -- Update NID for Matched records -- "

        internal string UpdateNIDForIndicatorGID()
        {

            string RetVal = string.Empty;
            // 1. Update Indicator Nids where Indicator Gids matches:
            //RetVal = "UPDATE " + Constants.TempDataTableName + " AS TD INNER JOIN " + this.DBQueries.TablesName.Indicator + " AS Ind ON TD." + Indicator.IndicatorGId + " = Ind." + Indicator.IndicatorGId +
            //    " SET TD." + Indicator.IndicatorNId + " = Ind." + Indicator.IndicatorNId;
            RetVal = this.UpdateQueries.UpdateNIDForIndicatorGID();
            return RetVal;
        }

        internal string UpdateNIDForSubgroupGID()
        {
            string RetVal = string.Empty;
            //  Update Unit Nids where Subgroup  Gids matches:
            //RetVal = "UPDATE " + Constants.TempDataTableName + " AS TD INNER JOIN " + this.DBQueries.TablesName.SubgroupVals + " AS SG ON TD." + SubgroupVals.SubgroupValGId + " = SG." + SubgroupVals.SubgroupValGId +
            //" SET TD." + SubgroupVals.SubgroupValNId + " = SG." + SubgroupVals.SubgroupValNId;
            RetVal = this.UpdateQueries.UpdateNIDForSubgroupGID();
            return RetVal;
        }

        internal string UpdateNIDForUnitGID()
        {
            string RetVal = string.Empty;

            //  Update Subgroup Nids where Unit Gids matches:
            //RetVal = "UPDATE " + Constants.TempDataTableName + " AS TD INNER JOIN " + this.DBQueries.TablesName.Unit + " AS U ON TD." + Unit.UnitGId + " = U." + Unit.UnitGId +
            //    " SET TD." + Unit.UnitNId + " = U." + Unit.UnitNId;
            RetVal = this.UpdateQueries.UpdateNIDForUnitGID();
            return RetVal;
        }

        internal string UpdateNIDForIndicatorName()
        {
            string RetVal = string.Empty;
            //  Update Nids where Names matches:

            // 1.Update Nid where Indicator Name matches.

            //RetVal = "UPDATE " + Constants.TempDataTableName + " AS TD INNER JOIN " + this.DBQueries.TablesName.Indicator + " AS Ind ON TD." + Indicator.IndicatorName + " = Ind." + Indicator.IndicatorName +
            //    " SET TD." + Indicator.IndicatorNId + " = Ind." + Indicator.IndicatorNId +
            //" WHERE (TD." + Indicator.IndicatorNId + " Is Null)";
            RetVal = this.UpdateQueries.UpdateNIDForIndicatorName();
            return RetVal;
        }

        internal string UpdateNIDForUnitName()
        {
            string RetVal = string.Empty;

            // 1.Update Nid where unit Name matches.

            //RetVal = "UPDATE " + Constants.TempDataTableName + " AS TD INNER JOIN " + this.DBQueries.TablesName.Unit + " AS U ON TD." + Unit.UnitName + " = U." + Unit.UnitName +
            //    " SET TD." + Unit.UnitNId + " = U." + Unit.UnitNId +
            //" WHERE (TD." + Unit.UnitNId + " Is Null)";
            RetVal = this.UpdateQueries.UpdateNIDForUnitName();
            return RetVal;
        }

        internal string UpdateNIDForSubgroupValName()
        {
            string RetVal = string.Empty;

            // 1.Update Nid where Subgroup Name matches.

            //RetVal = "UPDATE " + Constants.TempDataTableName + " AS TD INNER JOIN " + this.DBQueries.TablesName.SubgroupVals + " AS SG ON TD." + SubgroupVals.SubgroupVal + " = SG." + SubgroupVals.SubgroupVal +
            //    " SET TD." + SubgroupVals.SubgroupValNId + " = SG." + SubgroupVals.SubgroupValNId +
            //" WHERE ((TD." + SubgroupVals.SubgroupValNId + ") Is Null)";
            RetVal = this.UpdateQueries.UpdateNIDForSubgroupValName();
            return RetVal;
        }

        internal string UpdateNIDForAreaNId()
        {
            string RetVal = string.Empty;

            // 1.Update Nid where Area Name matches.

            //RetVal = "UPDATE " + Constants.TempDataTableName + " AS TD INNER JOIN " + this.DBQueries.TablesName.Area + " AS Area ON TD." + Area.AreaID + " = Area." + Area.AreaID +
            //     " SET TD." + Area.AreaNId + " = Area." + Area.AreaNId +
            //" WHERE ((TD." + Area.AreaNId + ") Is Null)";
            RetVal = this.UpdateQueries.UpdateNIDForAreaNId();
            return RetVal;
        }

        internal string UpdateIUSNidofMatchedRecords()
        {
            string RetVal = string.Empty;

            //RetVal = "UPDATE " + Constants.TempDataTableName + " AS TD INNER JOIN " + this.DBQueries.TablesName.IndicatorUnitSubgroup + " AS IUS ON (TD." + Indicator.IndicatorNId + " = IUS." + Indicator.IndicatorNId + ") AND (TD." + SubgroupVals.SubgroupValNId + " = IUS." + SubgroupVals.SubgroupValNId + ") AND (TD." + Unit.UnitNId + " = IUS." + Unit.UnitNId + ") " +
            //        " SET TD." + Constants.NewIUSColumnName + " = [IUS].[" + Indicator_Unit_Subgroup.IUSNId + "]";

            RetVal = this.UpdateQueries.UpdateIUSNidofMatchedRecords();
            return RetVal;
        }

        internal string UpdateIUSNidOfUnmatchedRecords()
        {
            string RetVal = string.Empty;
            // Update Nids where Gids matches:
            RetVal = "UPDATE " + Constants.TempDataTableName + " SET " + Indicator_Unit_Subgroup.IUSNId + " = " + Indicator.IndicatorNId + " & " + Unit.UnitNId + " & " + SubgroupVals.SubgroupValNId +
                    " where (" + Constants.NewIUSColumnName + " Is Null ) AND (" + Indicator.IndicatorNId + " Is Not Null ) AND (" + Unit.UnitNId + " Is Not Null ) AND (" + SubgroupVals.SubgroupValNId + " Is Not Null )";

            return RetVal;
        }

        ///// <summary>
        ///// SQL query to update IUSNid = - DataNid where any of IndicatorNid, unitNid, SubgroupNid is NULL.  
        ///// </summary>
        //internal string UpdateIUSNidForBlankI_U_S()
        //{
        //    string RetVal = string.Empty;
        //    RetVal = "UPDATE " + Constants.TempDataTableName + " SET " + Data.IUSNId + " = CINT( '-' & CStr(Data_Nid)) " +
        //        " where " + Indicator.IndicatorNId + " Is NULL Or " + Unit.UnitNId + " Is Null Or " + SubgroupVals.SubgroupValNId + " Is Null";
        //    return RetVal;
        //}

        /// <summary>
        /// SQL query to insert rows from TempDataTable into TempIUSBlank where any of IndicatorNid, unitNid, SubgroupNid is NULL.  
        /// </summary>
        internal string InsertBlankI_U_SIntoTempIUSBlankTable()
        {
            string RetVal = string.Empty;
            RetVal = "Select  Distinct   " + Indicator.IndicatorName + ", " + Indicator.IndicatorGId + ", " + Unit.UnitName + ", " + Unit.UnitGId + ", " + SubgroupVals.SubgroupVal + ", " + SubgroupVals.SubgroupValGId + "  into " + Constants.TempBlankIUSTable + " from " + Constants.TempDataTableName +
             " where " + Indicator.IndicatorNId + " Is NULL Or " + Indicator.IndicatorNId + " = 0 OR " + Unit.UnitNId + " Is Null Or " + Unit.UnitNId + " = 0 OR " + SubgroupVals.SubgroupValNId + " Is Null or " + SubgroupVals.SubgroupValNId + " = 0 OR " + Constants.NewIUSColumnName + " IS NULL OR " + Constants.NewIUSColumnName + " = 0 ";
            return RetVal;
        }

        /// <summary>
        /// SQL query to insert rows from TempDataTable into tempUnmatchedIUSTable where Indicator or unit or subgroup are blank.
        /// </summary>
        /// <remarks>Columns inserted are: (-)Data_NId AS UnmatchedIUSNid, Indicator, Unit, Subgroup  </remarks>
        internal string InsertBlankI_U_SIntoTempUnmatchedIUSTable(bool compareByGID)
        {
            string RetVal = string.Empty;
            if (compareByGID)
            {

                RetVal = "SELECT Min(TD.Data_NId)* -1 AS Unmatched_IUSNid  , TD." + Indicator.IndicatorName + ", TD." + Unit.UnitName + ", TD." + SubgroupVals.SubgroupVal + ", TD." + Indicator.IndicatorGId + " , TD." + Unit.UnitGId + ", TD." + SubgroupVals.SubgroupValGId + " into " + Constants.TempUnmatchedIUSTable +
                 " FROM " + Constants.TempDataTableName + " AS TD INNER JOIN " + Constants.TempBlankIUSTable + " AS IUS ON (TD.Unit_GID = IUS.Unit_GID) AND (TD.Indicator_GID = IUS.Indicator_GID) AND (TD." + SubgroupVals.SubgroupValGId + " = IUS." + SubgroupVals.SubgroupValGId + ")" +
                  " GROUP BY TD." + Indicator.IndicatorGId + ", TD." + Unit.UnitGId + ", TD." + SubgroupVals.SubgroupValGId + ", TD.Indicator_Name, TD.Unit_Name, TD.Subgroup_Val;";
            }
            else
            {
                RetVal = "SELECT Min(TD.Data_NId)* -1 AS Unmatched_IUSNid  , TD." + Indicator.IndicatorName + ", TD." + Unit.UnitName + ", TD." + SubgroupVals.SubgroupVal + " into " + Constants.TempUnmatchedIUSTable +
                 " FROM " + Constants.TempDataTableName + " AS TD INNER JOIN " + Constants.TempBlankIUSTable + " AS IUS ON (TD.Unit_Name = IUS.Unit_Name) AND (TD.Indicator_Name = IUS.Indicator_Name) AND (TD." + SubgroupVals.SubgroupVal + " = IUS." + SubgroupVals.SubgroupVal + ")" +
                  " GROUP BY TD.Indicator_Name, TD.Unit_Name, TD.Subgroup_Val;";
            }
            return RetVal;
        }

        /// <summary>
        /// SQl query to update IUSNid in TempDataTable where Indicator_name, UnitName, Subgroup matches in TempUnmatchedIUS Table
        /// </summary>
        /// <returns></returns>
        internal string UpdateUnmatchedIUSNidFromTempUnmatchedIUSTable(bool updateByGID)
        {
            string RetVal = string.Empty;

            //RetVal = "UPDATE " + Constants.TempDataTableName + " AS TD INNER JOIN " + Constants.TempUnmatchedIUSTable + " AS IUS ON (TD.Indicator_Name = IUS.Indicator_Name) AND (TD.Unit_Name = IUS.Unit_Name) AND (TD.Subgroup_Val = IUS.Subgroup_Val) SET TD.IUSNid = IUS.Unmatched_IUSNid";

            RetVal = this.UpdateQueries.UpdateUnmatchedIUSNidFromTempUnmatchedIUSTable(updateByGID);
            return RetVal;
        }

        internal string DropTempUnmatchedIUSTable()
        {
            string RetVal = string.Empty;
            RetVal = "Drop Table " + Constants.TempUnmatchedIUSTable;
            return RetVal;
        }

        internal string DropTempIUSBlankTable()
        {
            string RetVal = string.Empty;
            RetVal = "Drop Table " + Constants.TempBlankIUSTable;
            return RetVal;
        }

        # endregion

        # region " -- UNDO NID update -- "

        /// <summary>
        /// SQL Query to Updates specified column value (NID) = NULL in TempsheetTable.
        /// This query is used for the purpose of Undo NIDs updated for Mapped elements.
        /// </summary>
        /// <param name="ColumnName">column to be updated</param>
        /// <param name="Nids"></param>
        /// <returns>SQL Query</returns>
        internal string SetMappedElementNIDToNULL(string ColumnName, string ExpressionColumn, string values, bool updateIUSColumns)
        {
            string RetVal = string.Empty;
            if (updateIUSColumns == true)
            {
                //RetVal = "Update " + Constants.TempDataTableName + " Set " + ColumnName + " = NULL, " + Constants.NewIUSColumnName + " = NULL, " + Indicator_Unit_Subgroup.IUSNId + " = NULL where " + ExpressionColumn + " in (" + values + ")";
                RetVal = "Update " + Constants.TempDataTableName + " Set " + ColumnName + " = NULL, " + Constants.NewIUSColumnName + " = NULL  where " + ExpressionColumn + " in (" + values + ")";
            }
            else
            {
                RetVal = "Update " + Constants.TempDataTableName + " Set " + ColumnName + " = NULL where " + ExpressionColumn + " in (" + values + ")";
            }
            return RetVal;
        }

        # endregion

        # region " -- Get Unmatched Elements where NID are blank-- "

        /// <summary>
        /// SQl query to get records from TempDatatable where IndicatorNID = null
        /// </summary>
        internal string GetUnmatchedIndicator()
        {
            string RetVal = string.Empty;
            RetVal = "Select Distinct " + Indicator.IndicatorName + ", " + Indicator.IndicatorGId + " from " + Constants.TempDataTableName + " where " + Indicator.IndicatorNId + " Is Null OR " + Indicator.IndicatorNId + " = 0";
            return RetVal;
        }

        internal string GetUnmatchedUnit()
        {
            string RetVal = string.Empty;
            RetVal = "Select Distinct " + Unit.UnitName + ", " + Unit.UnitGId + " from " + Constants.TempDataTableName + " where " + Unit.UnitNId + " Is Null OR " + Unit.UnitNId + " = 0";
            return RetVal;
        }

        internal string GetUnmatchedSubgroup()
        {
            string RetVal = string.Empty;
            RetVal = "Select Distinct " + SubgroupVals.SubgroupVal + ", " + SubgroupVals.SubgroupValGId + " from " + Constants.TempDataTableName + " where " + SubgroupVals.SubgroupValNId + " Is Null OR " + SubgroupVals.SubgroupValNId + " = 0";
            return RetVal;
        }

        internal string GetUnmatchedIUS()
        {
            string RetVal = string.Empty;
            ////RetVal = "Select Distinct " + Indicator_Unit_Subgroup.IUSNId + ", " + Indicator.IndicatorName + ", " + Unit.UnitName + ", " + SubgroupVals.SubgroupVal + ", " + Indicator.IndicatorNId + ", " + Unit.UnitNId + ", " + SubgroupVals.SubgroupValNId + " from " + Constants.TempDataTableName + " where (" + Indicator_Unit_Subgroup.IUSNId + " Is Not Null OR " + Indicator_Unit_Subgroup.IUSNId + " <> 0 ) AND (" + Constants.NewIUSColumnName + " Is Null OR " + Constants.NewIUSColumnName + " = 0 )";
            RetVal = "Select Distinct " + Indicator_Unit_Subgroup.IUSNId + ", " + Indicator.IndicatorName + ", " + Unit.UnitName + ", " + SubgroupVals.SubgroupVal + ", " + Indicator.IndicatorNId + ", " + Unit.UnitNId + ", " + SubgroupVals.SubgroupValNId + ", " + Indicator.IndicatorGId + ", " + Unit.UnitGId + ", " + SubgroupVals.SubgroupValGId + " from " + Constants.TempDataTableName + " where (" + Indicator_Unit_Subgroup.IUSNId + " Is Not Null OR " + Indicator_Unit_Subgroup.IUSNId + " <> 0 ) AND (" + Constants.NewIUSColumnName + " Is Null OR " + Constants.NewIUSColumnName + " = 0 )";
            return RetVal;
        }

        internal string GetUnmatchedArea()
        {
            string RetVal = string.Empty;
            RetVal = "Select Distinct " + Area.AreaID + ", " + Area.AreaName + ", '' AS " + Area.AreaLevel + " from " + Constants.TempDataTableName + " where " + Area.AreaNId + " Is Null OR " + Area.AreaNId + " = 0";
            return RetVal;
        }

        # endregion

        # region " -- Queries Post Mapping process -- "

        /// <summary>
        /// Return SQL query to Insert new TimePeriods from Temp_Data table which are not in UT_TimePeriod Table.
        /// </summary>
        internal String InsertNewTimePeriods()
        {
            String RetVal = string.Empty;
            RetVal = "Insert into " + this.DBQueries.TablesName.TimePeriod + " (" + Timeperiods.TimePeriod + ") Select Distinct TD." + Timeperiods.TimePeriod + " from " + Constants.TempDataTableName + " AS TD Left Join " + this.DBQueries.TablesName.TimePeriod + " AS TP ON TD." + Timeperiods.TimePeriod + " = TP." + Timeperiods.TimePeriod +
                " Where TP." + Timeperiods.TimePeriod + " Is Null and TD." + Timeperiods.TimePeriod + " <> ''";
            return RetVal;
        }

        /// <summary>
        /// Return SQL query to Update TimePeriod Nid in Temp_Data.
        /// </summary>
        internal String UpdateTimePeriodNid()
        {
            String RetVal = string.Empty;
            // Update TimePeriod Nid in Temp_Data. 

            //RetVal = "Update " + Constants.TempDataTableName + " AS TD Inner JOIN " + this.DBQueries.TablesName.TimePeriod + " AS TP ON TD." + Timeperiods.TimePeriod + " = TP." + Timeperiods.TimePeriod +
            //            " Set TD." + Timeperiods.TimePeriodNId + " = TP." + Timeperiods.TimePeriodNId;
            RetVal = this.UpdateQueries.UpdateTimePeriodNid();
            return RetVal;
        }

        /// <summary>
        ///  Return SQL query to Insert new FootNotes from Temp_Data table which are not in UT_FootNotes Table.
        /// </summary>
        internal string GetNewFootNotes()
        {
            String RetVal = string.Empty;
            RetVal = "Select  TD." + FootNotes.FootNote + " from " + Constants.TempDataTableName + " AS TD, " + this.DBQueries.TablesName.FootNote + " AS FN " +
                " Where TD." + FootNotes.FootNote + " <> FN." + FootNotes.FootNote + " AND TD." + FootNotes.FootNote + " Is Not Null AND TD." + FootNotes.FootNoteNId + " Is NULL";
            return RetVal;
        }


        /// <summary>
        ///  Returns all footnote from temp data table
        /// </summary>
        internal string GetFootNotesFrmTempDataTable()
        {
            String RetVal = string.Empty;
            RetVal = "Select  TD." + FootNotes.FootNote + " from " + Constants.TempDataTableName + " AS TD " +
                " Where TD." + FootNotes.FootNote + " Is Not Null AND TD." + FootNotes.FootNoteNId + " Is NULL";
            return RetVal;
        }

        /// <summary>
        /// Returns SQL query to Update FootNotes Nid in Temp_Data. 
        /// </summary>
        internal string UpdateFootNoteNid()
        {
            String RetVal = string.Empty;
            // 
            //RetVal = "Update " + Constants.TempDataTableName + " AS TD, " + this.DBQueries.TablesName.FootNote + " AS FN " +
            //       " Set TD." + FootNotes.FootNoteNId + " = FN." + FootNotes.FootNoteNId + " where TD." + FootNotes.FootNote + " = FN." + FootNotes.FootNote;
            RetVal = this.UpdateQueries.UpdateFootNoteNid();


            return RetVal;

        }

        /// <summary>
        /// Returns SQL query to Update FootNotes Nid in Temp_Data. 
        /// </summary>
        internal string UpdateFootNoteNid(int nid, string footnoteText)
        {
            String RetVal = string.Empty;
            // 
            //RetVal = "Update " + Constants.TempDataTableName + " AS TD, " + this.DBQueries.TablesName.FootNote + " AS FN " +
            //       " Set TD." + FootNotes.FootNoteNId + " = FN." + FootNotes.FootNoteNId + " where TD." + FootNotes.FootNote + " = FN." + FootNotes.FootNote;
            //RetVal = this.UpdateQueries.UpdateFootNoteNid();


            RetVal = "Update " + Constants.TempDataTableName + " AS TD Set TD." + FootNotes.FootNoteNId + "=" + nid + " where TD." + FootNotes.FootNote + " = '" + DIQueries.RemoveQuotesForSqlQuery(footnoteText) + "'";
            return RetVal;

        }

        internal string GetSourcesWSpecialQuotesFrmTempDataTbl()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT " + Data.DataNId + "," + Constants.SourceColumnName + " FROM " + Constants.TempDataTableName + "  where " + Constants.SourceColumnName + " like '%’%' OR " + Constants.SourceColumnName + " like '%‘%' ";

            return RetVal;
        }

        internal string UpdateSpecialQuotesInSourceText(string sourceText, string dataNId)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + Constants.TempDataTableName + " SET " + Constants.SourceColumnName + "= '" + DIQueries.RemoveQuotesForSqlQuery(sourceText) + "' WHERE " + Data.DataNId + "=" + dataNId;

            return RetVal;
        }

        internal string GetFootnoteTextWSpecialQuotesFrmTempDataTbl()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT " + Data.DataNId + "," + FootNotes.FootNote + " FROM " + Constants.TempDataTableName + "  where " + FootNotes.FootNote + " like '%’%' OR " + FootNotes.FootNote + " like '%‘%' ";

            return RetVal;
        }

        internal string UpdateSpecialInFootnoteText(string footnoteText, string dataNId)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + Constants.TempDataTableName + " SET " + FootNotes.FootNote + "= '" + DIQueries.RemoveQuotesForSqlQuery(footnoteText) + "' WHERE " + Data.DataNId + "=" + dataNId;

            return RetVal;
        }

        /// <summary>
        /// Returns SQL query to Update FootNote Nid = -1 where FootNote is blank.
        /// </summary>
        internal string UpdateNidForEmptyFootNote()
        {
            String RetVal = string.Empty;
            RetVal = "Update " + Constants.TempDataTableName + " AS TD Set TD." + FootNotes.FootNoteNId + " = -1 where " + FootNotes.FootNote + " = '' OR " + FootNotes.FootNote + " Is Null OR " + FootNotes.FootNoteNId + " = 0";
            return RetVal;
        }

        internal string GetAllSourceFrmTempDataTable()
        {
            String RetVal = string.Empty;
            RetVal = "Select  TD." + Constants.SourceColumnName + " from " + Constants.TempDataTableName + " AS TD ";
            return RetVal;
        }

        /// <summary>
        ///  Returns SQL query to get Distinct Sources from Temp_Data which are not in UT_Indicator_Classification
        /// </summary>
        internal string GetDistinctSourcesFromTempData()
        {
            String RetVal = string.Empty;
            RetVal = "Select Distinct TD." + Constants.SourceColumnName + " from " + Constants.TempDataTableName + " AS TD, " + this.DBQueries.TablesName.IndicatorClassifications + " AS IC " +
                " Where TD." + Constants.SourceColumnName + " <> IC." + IndicatorClassifications.ICName + " AND LEN(TD." + Constants.SourceColumnName + ") <> 0 AND TD." + Data.SourceNId + " Is NULL";
            return RetVal;
        }

        /// <summary>
        /// Returns SQL query to Update Source_Nid in Temp_Data. 
        /// </summary>
        internal string UpdateSourceNidInTempData()
        {
            String RetVal = string.Empty;

            //RetVal = "Update " + Constants.TempDataTableName + " AS TD , " + this.DBQueries.TablesName.IndicatorClassifications + " AS IC " +
            //            " Set TD." + Data.SourceNId + " = IC." + IndicatorClassifications.ICNId + " where TD." + Constants.SourceColumnName + " = IC." + IndicatorClassifications.ICName;
            RetVal = this.UpdateQueries.UpdateSourceNidInTempData();
            return RetVal;
        }

        /// <summary>
        /// Returns Sql Query to update IC_Global for recommended sources.
        /// </summary>
        /// <returns></returns>
        internal string UpdateICGlobal(string ICTableName)
        {
            string RetVal = string.Empty;

            //RetVal = "UPDATE  " + ICTableName + " AS IC INNER JOIN "+ Constants.TempDataTableName +" as  TD ON TD." + Data.SourceNId + "= IC." + IndicatorClassificationsIUS.ICNId +" SET IC."+ IndicatorClassifications.ICGlobal + " = TD."+ IndicatorClassifications.ICGlobal +" where TD."+ IndicatorClassifications.ICGlobal +" <>0";
            RetVal = this.UpdateQueries.UpdateICGlobal(ICTableName);
            return RetVal;
        }

        /// <summary>
        /// Returns SQL query to Inserts new IUSNid & Source_Nid from Temp_Data into IndicatorClassificationsIUS.
        /// </summary>
        internal string InsertRecordInIndicator_Classification_IUS()
        {
            string RetVal = string.Empty;

            RetVal = "Insert into " + this.DBQueries.TablesName.IndicatorClassificationsIUS + "(" + IndicatorClassificationsIUS.ICNId + ", " + IndicatorClassificationsIUS.IUSNId;
            RetVal += "," + IndicatorClassificationsIUS.RecommendedSource;

            RetVal += ") " + " Select Distinct TD." + Data.SourceNId + ", TD." + Constants.NewIUSColumnName;

            RetVal += ", TD." + IndicatorClassificationsIUS.RecommendedSource;

            RetVal += " from  " + Constants.TempDataTableName + " AS TD LEFT JOIN " + this.DBQueries.TablesName.IndicatorClassificationsIUS + " AS IC ON (TD." + Data.SourceNId + " = IC." + IndicatorClassificationsIUS.ICNId + ") AND (TD." + Constants.NewIUSColumnName + " = IC." + IndicatorClassificationsIUS.IUSNId + ")" +
                    " Where IC." + IndicatorClassificationsIUS.ICIUSNId + " Is Null AND TD." + Constants.NewIUSColumnName + " Is Not Null AND TD." + Data.SourceNId + " Is Not Null";
            return RetVal;
        }

        /// <summary>
        /// Returns SQL query to get new IUSNid & Source_Nid 
        /// </summary>
        internal string GetNewSourcesNIUSNID()
        {
            string RetVal = string.Empty;


            RetVal = " Select Distinct TD." + Data.SourceNId + ", TD." + Constants.NewIUSColumnName;

            RetVal += ", TD." + IndicatorClassificationsIUS.RecommendedSource;

            RetVal += " from  " + Constants.TempDataTableName + " AS TD LEFT JOIN " + this.DBQueries.TablesName.IndicatorClassificationsIUS + " AS IC ON (TD." + Data.SourceNId + " = IC." + IndicatorClassificationsIUS.ICNId + ") AND (TD." + Constants.NewIUSColumnName + " = IC." + IndicatorClassificationsIUS.IUSNId + ")" +
                    " Where IC." + IndicatorClassificationsIUS.ICIUSNId + " Is Null AND TD." + Constants.NewIUSColumnName + " Is Not Null AND TD." + Data.SourceNId + " Is Not Null";
            return RetVal;
        }

        /// <summary>
        /// SQL query to update TempDataTable specific NIDColumn to value on where condition.
        /// NOTE: Remove apostophe from value to set, in passed parameter.
        /// </summary>
        /// <param name="columnToUpdate"></param>
        /// <param name="NidValue"></param>
        /// <param name="columnExpression"></param>
        /// <param name="expressionValue">string value</param>
        internal string UpdateMappedElementNids(string columnToUpdate, int NidValue, string columnExpression, string expressionValue)
        {
            string RetVal = string.Empty;
            RetVal = "Update " + Constants.TempDataTableName + " Set " + columnToUpdate + " = " + NidValue + " where " + columnExpression + " = '" + expressionValue + "'";
            return RetVal;
        }

        internal string UpdateMappedElementNids(string columnToUpdate, int NidValue, string columnExpression, double expressionValue)
        {
            string RetVal = string.Empty;
            RetVal = "Update " + Constants.TempDataTableName + " Set " + columnToUpdate + " = " + NidValue + " where " + columnExpression + " = " + expressionValue;
            return RetVal;
        }

        # endregion

        # region " -- Update/Insert Data values -- "

        /// <summary>
        /// SQl Query to update TempDataValue into DataValue in UT_Data where IUS NID, AreaID, SourceNId matched.
        /// </summary>
        /// <returns></returns>
        internal string UpdateDataValue()
        {
            String RetVal = string.Empty;
            // Update DataValue in UT_Data table where IUSNid, AreaNid and TimeperiodNid matches with Temp_Data Table.

            //RetVal = "Update " + this.DBQueries.TablesName.Data + " AS DATA Inner JOIN " + Constants.TempDataTableName + " AS TD ON (TD." + Data.TimePeriodNId + " = DATA." + Data.TimePeriodNId + ") AND (TD." + Data.AreaNId + " = DATA." + Data.AreaNId + ") AND (TD." + Constants.NewIUSColumnName + " = DATA." + Data.IUSNId + ") AND ( TD." + Data.SourceNId + " = DATA." + Data.SourceNId + ")" +
            //            " Set DATA." + Data.DataValue + " = TD." + Data.DataValue + " , DATA." + Data.FootNoteNId + " = IIF(LEN(TD." + Data.FootNoteNId + ") = 0, -1, TD." + Data.FootNoteNId + ") , DATA." + Data.DataDenominator + " = TD." + Data.DataDenominator;
            RetVal = this.UpdateQueries.UpdateDataValue();
            return RetVal;
        }

        #region "-- New update process for updating DataValue -"

        internal string UpdateDataValueforAccess()
        {
            string RetVal = string.Empty;

            RetVal = "Update " + this.DBQueries.TablesName.Data + " AS DATA Inner JOIN " + Constants.TempDataTableName + " AS TD ON (TD." + Constants.TempColumnName + " = DATA." + Constants.TempColumnName + ") " +
                         " Set DATA." + Data.DataValue + " = TD." + Data.DataValue + " , DATA." + Data.FootNoteNId + " = IIF(LEN(TD." + Data.FootNoteNId + ") = 0, -1, TD." + Data.FootNoteNId + ") , DATA." + Data.DataDenominator + " = TD." + Data.DataDenominator;

            return RetVal;
        }

        internal string CreateTempColInTempDataTable()
        {
            string RetVal = string.Empty;

            RetVal = "ALTER TABLE " + Constants.TempDataTableName + " ADD Column " + Constants.TempColumnName + " TEXT(255)";

            return RetVal;
        }

        internal string CreateTempColInDataTable()
        {
            string RetVal = string.Empty;

            RetVal = "ALTER TABLE " + this.DBQueries.TablesName.Data + " ADD Column " + Constants.TempColumnName + " TEXT(255)";

            return RetVal;
        }

        internal string UpdateTempColInTempdataTable()
        {
            string RetVal = string.Empty;

            RetVal = "update " + Constants.TempDataTableName + " set " + Constants.TempColumnName + "= " + Constants.NewIUSColumnName + "   & '_' & " + Area.AreaNId + " & '_' & " + Timeperiods.TimePeriodNId + " & '_' & " + Data.SourceNId;

            return RetVal;
        }

        internal string UpdateTempColInDataTable()
        {
            string RetVal = string.Empty;

            RetVal = "update " + this.DBQueries.TablesName.Data + " set " + Constants.TempColumnName + "= " + Data.IUSNId + "   & '_' &  " + Area.AreaNId + " & '_' &  " + Timeperiods.TimePeriodNId + "  & '_' &  " + Data.SourceNId;

            return RetVal;
        }

        /// <summary>
        /// SQL Query to Inserts new DataValue, IUSNID, AreaNID, TimePeriodNid into UT_Data from TempDataTable.
        /// </summary>
        /// <returns></returns>
        internal string InsertDataValueForMsAccess()
        {
            // Insert DataValue in UT_Data table for NEW IUSNid, AreaNid and TimeperiodNid in Temp_Data Table.
            // inner join on TempCol 

            String RetVal = string.Empty;
            RetVal = "Insert into " + this.DBQueries.TablesName.Data + " (" + Data.IUSNId + ", " + Data.DataValue + ", " + Data.TimePeriodNId + ", " + Data.AreaNId + ", " + Data.DataDenominator + ", " + Data.SourceNId + ", " + Data.FootNoteNId + ") " +
                        " Select TD." + Constants.NewIUSColumnName + ", TD." + Data.DataValue + ", TD." + Data.TimePeriodNId + ", TD." + Data.AreaNId + ", TD." + Data.DataDenominator + ", TD." + Data.SourceNId + ", TD." + Data.FootNoteNId +
                        " from " + Constants.TempDataTableName + " AS TD Left Join  " + this.DBQueries.TablesName.Data + " AS DATA  " +
                        " ON (TD." + Constants.TempColumnName + " = DATA." + Constants.TempColumnName + ")   where DATA." + Data.DataNId + " is null";

            return RetVal;
        }

        #endregion


        /// <summary>
        /// SQL Query to Inserts new DataValue, IUSNID, AreaNID, TimePeriodNid into UT_Data from TempDataTable.
        /// </summary>
        /// <returns></returns>
        internal string InsertDataValue()
        {
            // Insert DataValue in UT_Data table for NEW IUSNid, AreaNid and TimeperiodNid in Temp_Data Table.
            //TODO: TimeStart , TimeEnd
            String RetVal = string.Empty;
            RetVal = "Insert into " + this.DBQueries.TablesName.Data + " (" + Data.IUSNId + ", " + Data.DataValue + ", " + Data.TimePeriodNId + ", " + Data.AreaNId + ", " + Data.DataDenominator + ", " + Data.SourceNId + ", " + Data.FootNoteNId + ") " +
                        " Select TD." + Constants.NewIUSColumnName + ", TD." + Data.DataValue + ", TD." + Data.TimePeriodNId + ", TD." + Data.AreaNId + ", TD." + Data.DataDenominator + ", TD." + Data.SourceNId + ", TD." + Data.FootNoteNId +
                        " from " + Constants.TempDataTableName + " AS TD Left Join  " + this.DBQueries.TablesName.Data + " AS DATA  " +
                        " ON (TD." + Data.TimePeriodNId + " = DATA." + Data.TimePeriodNId + ") AND (TD." + Data.AreaNId + " = DATA." + Data.AreaNId + ") AND (TD." + Constants.NewIUSColumnName + " = DATA." + Data.IUSNId + ") AND ( TD." + Data.SourceNId + " = DATA." + Data.SourceNId + ") " +
                        " where DATA." + Data.DataNId + " is null";
            return RetVal;
        }




        # endregion

        # region " -- Delete queries -- "
        internal string DropTempDataTable()
        {
            String RetVal = string.Empty;
            RetVal = "Drop Table " + Constants.TempDataTableName;
            return RetVal;
        }


        internal string DropTempSheetTable()
        {
            return "Drop Table " + Constants.TempSheetTableName;
        }

        internal string RemoveNullRecordsFromTempDataTableForSDMX()
        {
            string RetVal = string.Empty;

            RetVal = "Delete from " + Constants.TempDataTableName + " where (" + Data.DataValue + " Is Null) OR " +
                Data.DataValue + " = '' OR (" +
                Indicator.IndicatorName + " Is Null  AND " + Indicator.IndicatorGId + " IS NULL ) OR (" +
                Unit.UnitName + " Is Null AND " + Unit.UnitGId + " IS NULL ) OR (" +
                SubgroupVals.SubgroupVal + " Is Null AND " + SubgroupVals.SubgroupValGId + " IS NULL ) OR (" +
                Area.AreaID + " Is Null) OR (" +
                Timeperiods.TimePeriod + " Is Null)";
            return RetVal;
        }


        internal string RemoveNullRecordsFromTempDataTable()
        {
            string RetVal = string.Empty;

            RetVal = "Delete from " + Constants.TempDataTableName + " where (" + Data.DataValue + " Is Null) OR " + Data.DataValue + " = '' OR (" + Indicator.IndicatorName + " Is Null ) OR (" + Unit.UnitName + " Is Null) OR (" + SubgroupVals.SubgroupVal + " Is Null) OR (" + Area.AreaID + " Is Null) OR (" + Timeperiods.TimePeriod + " Is Null)" + " OR (" + Constants.SourceColumnName + " Is Null  )" + " OR (" + Constants.SourceColumnName + " ='' )";
            return RetVal;
        }

        /// <summary>
        /// Return SQL query to delete Unmapped records left in TempData Table where Map_IusNid & AreaID are NULL/ empty
        /// </summary>
        internal string DeleteUnMappedRecordsLeft()
        {
            string RetVal = string.Empty;
            RetVal = " Delete from " + Constants.TempDataTableName + " where " + Constants.NewIUSColumnName + " Is Null OR " + Area.AreaNId + " Is Null OR " + Constants.NewIUSColumnName + " = 0 OR " + Area.AreaNId + " = 0 ";
            return RetVal;
        }

        # endregion


        # region " -- Duplcate Records Handling -- "

        internal string InsertDuplicateRecordNidInTemp()
        {
            string RetVal = string.Empty;
            RetVal = "SELECT min(" + Data.DataNId + ") as Nid  into NIDTable FROM " + Constants.TempDataTableName + " AS TD " +
                " group by TD." + Area.AreaNId + ", TD." + Timeperiods.TimePeriod + ", TD." + Constants.SourceColumnName + ", TD." + Constants.NewIUSColumnName + " having count(*) > 1 AND TD." + Constants.NewIUSColumnName + " IS Not Null";
            return RetVal;
        }

        /// <summary>
        /// SQL query to insert Distinct rows of Duplicate Records from TempDataTable into DuplicateTemp.
        /// </summary>
        internal string InsertDuplicateRecordInTemp()
        {
            string RetVal = string.Empty;
            RetVal = "select * into DuplicateTemp  from " + Constants.TempDataTableName + " where " + Data.DataNId + " in ( SELECT Nid FROM NIDTable)";
            return RetVal;
        }


        /// <summary>
        /// SQL query to Set New_IUSNid  = -1 where Records are Duplicated.
        /// Duplicate records were moved to DuplicateTemp Table.
        /// </summary>
        internal string UpdateIUSNidOfDuplicateRecord()
        {
            string RetVal = string.Empty;

            //RetVal = " Update " + Constants.TempDataTableName + " T1 Inner Join DuplicateTemp T2 " +
            //    " On  T1." + Area.AreaNId + " =  T2." + Area.AreaNId + " And T1." + Timeperiods.TimePeriod + " = T2." + Timeperiods.TimePeriod + " And T1." + Constants.SourceColumnName + " = T2." + Constants.SourceColumnName + " And T1." + Constants.NewIUSColumnName + " = T2." + Constants.NewIUSColumnName + " " +
            //    " Set T1." + Constants.NewIUSColumnName + " = -1 ";
            RetVal = this.UpdateQueries.UpdateIUSNidOfDuplicateRecord();
            return RetVal;
        }

        /// <summary>
        /// SQL query to delete records from TemoDataTable where New_IUSNid = -1
        /// New_IUSNid = -1 is present for Duplicate records.
        /// </summary>
        /// <returns></returns>
        internal string DeleteDuplicateRecordsFromTempDataTable()
        {
            string RetVal = string.Empty;
            RetVal = "Delete from " + Constants.TempDataTableName + " where " + Constants.NewIUSColumnName + " = -1";
            return RetVal;
        }

        /// <summary>
        /// SQl query to insert Distinct Duplicated records from DuplicateTemp table into TempDataTable.
        /// </summary>
        /// <returns></returns>
        internal string InsertFromDuplicateTempIntoTempData()
        {

            string RetVal = string.Empty;
            RetVal = "Insert into " + Constants.TempDataTableName + " Select * from DuplicateTemp";

            return RetVal;
        }

        internal string DropTempTableOfDuplcate()
        {
            string RetVal = string.Empty;
            RetVal = "Drop Table DuplicateTemp";

            return RetVal;
        }

        internal string DropTempNIDTableOfDuplcate()
        {
            string RetVal = string.Empty;
            RetVal = "Drop Table NIDTable";

            return RetVal;
        }

        # endregion

        #region "-- HTML LOG Queries --"
        internal string GetDistinctTimePeriodCount()
        {
            string Retval = string.Empty;
            Retval = "Select Count(*) from " + this.DBQueries.TablesName.TimePeriod;
            return Retval;
        }

        internal string GetDistinctSourcesCount()
        {
            string Retval = string.Empty;
            Retval = "Select Count(*) from " + this.DBQueries.TablesName.IndicatorClassifications + " where " + IndicatorClassifications.ICType + " = 'SR' AND " + IndicatorClassifications.ICParent_NId + " <> -1";
            return Retval;
        }

        internal string GetDataCount()
        {
            string RetVal = string.Empty;
            RetVal = "Select count(*) from " + this.DBQueries.TablesName.Data;
            return RetVal;
        }

        /// <summary>
        /// SQL query to get I, U, S Names for those records which were NOT mapped (or left unmatched)
        /// </summary>
        /// <returns></returns>
        internal string GetSkippedRecords()
        {
            string RetVal = string.Empty;
            RetVal = "Select DISTINCT " + Constants.Log.SkippedIndicatorColumnName + ", " + Constants.Log.SkippedUnitColumnName + ", " + Constants.Log.SkippedSubgroupValColumnName + ", " + Constants.Log.SkippedSourceFileColumnName + ", " + Indicator.IndicatorGId + "," + Unit.UnitGId + ", " + SubgroupVals.SubgroupValGId + " from " + Constants.TempDataTableName +
                " where (" + Constants.NewIUSColumnName + " IS Null ) OR (" + Constants.NewIUSColumnName + " = 0 )";
            return RetVal;
        }

        internal string GetMatchedIUS()
        {
            string RetVal = string.Empty;
            RetVal = "Select Distinct " + Indicator.IndicatorName + ", " + Indicator.IndicatorGId + ", " + Unit.UnitName + ", " + Unit.UnitGId + ", " + SubgroupVals.SubgroupVal + ", " + SubgroupVals.SubgroupValGId + " FROM " + Constants.TempDataTableName +
             " where " + Constants.NewIUSColumnName + " IS NOT NULL AND " + Constants.NewIUSColumnName + " <> 0 ";
            return RetVal;
        }

        #endregion

        internal string GetInvalidTimeperiods()
        {
            return DBQueries.Timeperiod.GetInvalidTimeperiods(Constants.TempDataTableName);
        }

        internal string DeleteInvalidTimeperiods()
        {
            return DBQueries.Delete.Timeperiod.DeleteInvalidTimeperiod(Constants.TempDataTableName);
        }

        /// <summary>
        /// SQL query to update ExcelFileName in TempsheetTable. (Note: Remove Apostophe from value passed)
        /// </summary>
        internal string UpdateSourceFileNameInTempSheetTable(string fileNameWSheetName, string rowNumber)
        {
            string Retval = string.Empty;
            Retval = "UPDATE " + Constants.TempSheetTableName + " SET " + Constants.Log.SkippedSourceFileColumnName + " = '" + DICommon.RemoveQuotes(fileNameWSheetName) + "' & " + rowNumber + " & ']'";
            return Retval;
        }

        internal string GetDuplicateRecordsInTempData()
        {
            string RetVal = string.Empty;

            RetVal = " SELECT " + Constants.Log.SkippedSourceFileColumnName + ", " +
                 Constants.Log.DuplicateIndicatorColumnName + ", " + Constants.Log.DuplicateUnitColumnName + ", " + Constants.Log.DuplicateSubgroupValColumnName + ", " + Constants.Log.DuplicateTimeperiodColumnName + ", " + Constants.Log.DuplicateAreaIDColumnName + ", " + Constants.Log.DuplicateSourceColumnName + ", " + Indicator.IndicatorGId + ", " + Unit.UnitGId + ", " + SubgroupVals.SubgroupValGId + " from " + Constants.TempDataTableName + " AS T1 "
                 + " WHERE EXISTS (" +
" Select " + Constants.Log.DuplicateIndicatorColumnName + ", " + Constants.Log.DuplicateUnitColumnName + ", " + Constants.Log.DuplicateSubgroupValColumnName + ", " + Constants.Log.DuplicateTimeperiodColumnName + ", " + Constants.Log.DuplicateAreaIDColumnName + ", " + Constants.Log.DuplicateSourceColumnName + ", " + Indicator.IndicatorGId + ", " + Unit.UnitGId + ", " + SubgroupVals.SubgroupValGId + " from " + Constants.TempDataTableName + " AS T2 " +
" WHERE T1.Data_NID=T2.Data_NID " +
                " group by " + Constants.Log.DuplicateIndicatorColumnName + ", " + Constants.Log.DuplicateUnitColumnName + ", " + Constants.Log.DuplicateSubgroupValColumnName + ", " + Constants.Log.DuplicateTimeperiodColumnName + ", " + Constants.Log.DuplicateAreaIDColumnName + ", " + Constants.Log.DuplicateSourceColumnName + ", " + Indicator.IndicatorGId + ", " + Unit.UnitGId + ", " + SubgroupVals.SubgroupValGId + " having Count(*) > 1 ";

            RetVal += " Order by " + Constants.Log.DuplicateIndicatorColumnName + ", " + Constants.Log.DuplicateUnitColumnName + ", " + Constants.Log.DuplicateSubgroupValColumnName + ", " + Constants.Log.DuplicateTimeperiodColumnName + ", " + Constants.Log.DuplicateAreaIDColumnName + ", " + Constants.Log.DuplicateSourceColumnName + " )";

            return RetVal;
        }


        /// <summary>
        /// SQL query to updates Db_Available_Database TAble with the DbFileName passed in PArameter.
        /// </summary>
        internal string UpdateDB_Available_DatabaseTable(string availableDBName)
        {
            string RetVal = string.Empty;
            RetVal = "UPDATE DB_Available_Databases AS DB SET AvlDB_Name = '" + availableDBName + "' where AvlDB_Default=true ";
            return RetVal;
        }

        internal string UpdateBlankDataValueSymbolWithBlank(string blankDataValueSymbol)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + this.DBQueries.TablesName.Data + " SET " + Data.DataValue
                + "= '' WHERE " + Data.DataValue + " ='" + blankDataValueSymbol + "'";

            return RetVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PublisherMaxLength"> max length of publish field in IC table special for source</param>
        /// <returns></returns>
        internal string GetInvalidSource(int publisherMaxLength)
        {
            string RetVal = string.Empty;

            //select * from TempDataTable WHERE  InStr([Source],'_') > 100
            RetVal = "select * from " + Common.Constants.TempDataTableName + " WHERE InStr([" + Constants.Log.DuplicateSourceColumnName + "],'_') > " + publisherMaxLength + " ";

            return RetVal;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="PublisherMaxLength"> max length of publish field in IC table special for source</param>
        /// <returns></returns>
        internal string UpdateInvalidSourceWithNull(int publisherMaxLength)
        {
            string RetVal = string.Empty;

            //update TempDataTable set source=null WHERE  InStr([Source],'_') > 100
            RetVal = "UPDATE " + Common.Constants.TempDataTableName + " SET " + Constants.Log.DuplicateSourceColumnName + " = NULL WHERE InStr([" + Constants.Log.DuplicateSourceColumnName + "],'_') > " + publisherMaxLength + " ";

            return RetVal;
        }

        # endregion

    }

}
