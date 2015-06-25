using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.Data
{
    /// <summary>
    /// Provides sql queries to update records
    /// </summary>    
    public static class Update
    {
        #region "-- Private --"

        #region "-- Variables --"

        private const string TableName = "data";

        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- Methods --"


        /// <summary>
        /// To update denominator value for the given data NId
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="dataNid"></param>
        /// <param name="datavalue"></param>
        /// <returns>sql query</returns>
        public static string UpdateDenominatorValue(string dataPrefix, int dataNid, int denominatorValue)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + dataPrefix + Update.TableName + " SET " + DIColumns.Data.DataDenominator + "="
           + denominatorValue + " WHERE " + DIColumns.Data.DataNId + " = " + dataNid;

            return RetVal;
        }

        /// <summary>
        /// To update footnoteNId for the given data NId
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="footnoteNId"></param>
        /// <param name="dataNIds">comma separated data NIds</param>
        /// <returns>sql query</returns>
        public static string UpdateFootnoteNId(string DataPrefix, string footnoteNId, string dataNIds)
        {
            string RetVal = string.Empty;

            RetVal = "Update " + DataPrefix + Update.TableName + " Set " + DIColumns.Data.FootNoteNId + " = " + footnoteNId;

            RetVal += " Where " + DIColumns.Data.DataNId + " IN ( " + dataNIds + ")";

            return RetVal;
        }

        /// <summary>
        /// Removes the given footnote NIds from data table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="footnoteNIds">Comma separated footnote nids</param>
        /// <returns>sql query</returns>
        public static string RemoveFootnoteNId(string DataPrefix, string footnoteNIds)
        {
            string RetVal = string.Empty;

            RetVal = "Update " + DataPrefix + Update.TableName + " Set " + DIColumns.Data.FootNoteNId + " =-1   ";

            RetVal += " Where " + DIColumns.Data.FootNoteNId + " IN ( " + footnoteNIds + ")";

            return RetVal;
        }


        /// <summary>
        /// To update data value
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="dataNid"></param>
        /// <param name="datavalue"></param>
        /// <returns>sql query</returns>
        public static string UpdateDataValue(string dataPrefix, int dataNid, string datavalue)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + dataPrefix + Update.TableName + " SET " + DIColumns.Data.DataValue + "='"
           + DIQueries.RemoveQuotesForSqlQuery(datavalue) + "' Where " + DIColumns.Data.DataNId + " = " + dataNid;

            return RetVal;
        }

        /// <summary>
        /// To update data value
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="dataNid"></param>
        /// <param name="datavalue"></param>
        /// <param name="isPlannedValue"></param>
        /// <param name="cfIntervalUpper"></param>
        /// <param name="cfIntervalLower"></param>
        /// <returns></returns>
        public static string UpdateDataValue(string dataPrefix, int dataNid, string datavalue, bool isPlannedValue, string cfIntervalUpper, string cfIntervalLower, bool isNumeric)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + dataPrefix + Update.TableName + " SET ";

            if (isNumeric)
            {
                RetVal += DIColumns.Data.DataValue + "=" + datavalue + "," + DIColumns.Data.IsTextualData + "=0";
            }
            else
            {
                RetVal += DIColumns.Data.DataValue + "='" + datavalue + "'," + DIColumns.Data.IsTextualData + "=0";
            }

            RetVal += "," + DIColumns.Data.IsPlannedValue + "=" + Convert.ToInt16(isPlannedValue)
            + "," + DIColumns.Data.ConfidenceIntervalUpper + "=" + cfIntervalUpper
            + "," + DIColumns.Data.ConfidenceIntervalLower + "=" + cfIntervalLower
            + " Where " + DIColumns.Data.DataNId + " = " + dataNid;

            return RetVal;
        }

        /// <summary>
        /// To update data value
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="dataNid"></param>
        /// <param name="datavalue"></param>
        /// <param name="isPlannedValue"></param>
        /// <param name="cfIntervalUpper"></param>
        /// <param name="cfIntervalLower"></param>
        /// <returns></returns>
        public static string UpdateTextualDataValue(string dataPrefix, int dataNid, string datavalue, bool isPlannedValue, string cfIntervalUpper, string cfIntervalLower)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + dataPrefix + Update.TableName + " SET " + DIColumns.Data.TextualDataValue + "='"
           + DIQueries.RemoveQuotesForSqlQuery(datavalue) + "'"
           + "," + DIColumns.Data.IsTextualData + "=1"
           + "," + DIColumns.Data.IsPlannedValue + "=" + Convert.ToInt16(isPlannedValue)
           + "," + DIColumns.Data.ConfidenceIntervalUpper + "=" + cfIntervalUpper
           + "," + DIColumns.Data.ConfidenceIntervalLower + "=" + cfIntervalLower
           + " Where " + DIColumns.Data.DataNId + " = " + dataNid;

            return RetVal;
        }

        /// <summary>
        /// To update data value
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="dataNid"></param>
        /// <param name="datavalue"></param>
        /// <returns></returns>
        public static string UpdateTextualDataValue(string dataPrefix, int dataNid, string datavalue)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + dataPrefix + Update.TableName + " SET " + DIColumns.Data.TextualDataValue + "='"
           + DIQueries.RemoveQuotesForSqlQuery(datavalue) + "'"
           + "," + DIColumns.Data.IsTextualData + "=1"

           + " Where " + DIColumns.Data.DataNId + " = " + dataNid;

            return RetVal;
        }

        /// <summary>
        /// To update data value
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="dataNid"></param>
        /// <param name="datavalue"></param>
        /// <param name="sourceNid"></param>
        /// <param name="timeperiodNid"></param>
        /// <returns>sql query</returns>
        public static string UpdateDataValue(string dataPrefix, int dataNid, string datavalue, int timeperiodNid, int sourceNid)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + dataPrefix + Update.TableName + " SET "
                + DIColumns.Data.DataValue + "='" + DIQueries.RemoveQuotesForSqlQuery(datavalue) + "',"
                + DIColumns.Data.TimePeriodNId + "=" + timeperiodNid + ","
                + DIColumns.Data.SourceNId + "=" + sourceNid
                + " Where " + DIColumns.Data.DataNId + " = " + dataNid;

            return RetVal;
        }

        /// <summary>
        /// Update DataValues and Nids by DataNId
        /// </summary>
        /// <param name="dataNId"></param>
        /// <param name="dataPrefix"></param>
        /// <param name="IUSNid"></param>
        /// <param name="timeperiodNid"></param>
        /// <param name="areaNid"></param>
        /// <param name="datavalue"></param>
        /// <param name="footnoteNid"></param>
        /// <param name="sourceNid"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="denominator"></param>
        /// <returns></returns>
        public static string UpdateDataValue(int dataNId, string dataPrefix, int IUSNid, int timeperiodNid, int areaNid, string datavalue, int footnoteNid, int sourceNid, string startDate, string endDate, string denominator)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + dataPrefix + Update.TableName + " SET "
                + DIColumns.Data.IUSNId + "=" + IUSNid + ","
               + DIColumns.Data.TimePeriodNId + "=" + timeperiodNid + ","
               + DIColumns.Data.AreaNId + "=" + areaNid + ","
               + DIColumns.Data.DataValue + "='" + DIQueries.RemoveQuotesForSqlQuery(datavalue) + "',"
               + DIColumns.Data.FootNoteNId + "=" + footnoteNid + ","
               + DIColumns.Data.SourceNId + "=" + sourceNid;

            if (startDate.Length > 0)
            {
                RetVal += "," + DIColumns.Data.StartDate + "='" + Convert.ToDateTime(startDate).Date + "'";

            }

            if (endDate.Length > 0)
            {
                RetVal += "," + DIColumns.Data.EndDate + " = '" + Convert.ToDateTime(endDate).Date + "'";
            }

            if (denominator.Length > 0)
            {
                RetVal += "," + DIColumns.Data.DataDenominator + "='" + denominator + "'";
            }

            RetVal += " WHERE " + DIColumns.Data.DataNId + "=" + dataNId;

            return RetVal;
        }

        /// <summary>
        /// To update IC_IUS_Order
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="dataNid"></param>
        /// <param name="ICIUSOrder"></param>
        /// <returns>sql query</returns>
        public static string UpdateICIUSOrder(string dataPrefix, int dataNid, string ICIUSOrder)
        {
            string RetVal = string.Empty;

            if (string.IsNullOrEmpty(ICIUSOrder))
            {
                ICIUSOrder = "null";
            }

            RetVal = "UPDATE " + dataPrefix + Update.TableName + " SET " + DIColumns.Data.ICIUSOrder + "="
           + ICIUSOrder + " Where " + DIColumns.Data.DataNId + " = " + dataNid;

            return RetVal;
        }

        /// <summary>
        /// Returns a query to update indicator_nid,unit_nid,subgroup_Val_nid .
        /// </summary>
        /// <param name="tableNames"></param>
        /// <returns></returns>
        public static string UpdateIndicatorUnitSubgroupVAlNids(DITables tableNames)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + tableNames.SubgroupVals + " AS SG, " + tableNames.Unit + " as  U , " + tableNames.Indicator + " AS I, " + tableNames.Data + " AS D , " + tableNames.IndicatorUnitSubgroup + " AS IUS  "
                + "SET D." + DIColumns.Data.IndicatorNId + "=I." + DIColumns.Indicator.IndicatorNId
                + " ,D." + DIColumns.Data.UnitNId + "=U." + DIColumns.Unit.UnitNId
                + " ,D." + DIColumns.Data.SubgroupValNId + "= SG." + DIColumns.SubgroupVals.SubgroupValNId
                + " where "
                + " D." + DIColumns.Data.IUSNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId
                + " And ( "
                + " D." + DIColumns.Data.IndicatorNId + " is null Or D." + DIColumns.Data.UnitNId + " is null Or D." + DIColumns.Data.SubgroupValNId + " is null "
                + " OR "
                + " D." + DIColumns.Data.IndicatorNId + " <=0 Or D." + DIColumns.Data.UnitNId + " <=0 Or D." + DIColumns.Data.SubgroupValNId + " <=0 "
                + " ) "
                + " And ("
                + " I." + DIColumns.Indicator.IndicatorNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + "  and U." + DIColumns.Data.UnitNId + "  = IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " and  SG." + DIColumns.SubgroupVals.SubgroupValNId + " = IUS." + DIColumns.Data.SubgroupValNId
                + ") ";

            return RetVal;
        }


        /// <summary>
        /// Returns a query to update indicator_nid,unit_nid,subgroup_Val_nid .
        /// </summary>
        /// <param name="tableNames"></param>
        /// <returns></returns>
        public static string UpdateIUSNIdsForSQLServer(DITables tableNames)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + tableNames.Data
                + " SET " + DIColumns.Data.IndicatorNId + "=I." + DIColumns.Indicator.IndicatorNId
                + " ," + DIColumns.Data.UnitNId + "=U." + DIColumns.Unit.UnitNId + " ,"
                + DIColumns.Data.SubgroupValNId + "= SG." + DIColumns.SubgroupVals.SubgroupValNId
                + " FROM " + tableNames.Data + " AS D ," + tableNames.IndicatorUnitSubgroup + " AS IUS," + tableNames.Indicator + " AS I,"
                + tableNames.Unit + " AS U," + tableNames.SubgroupVals + " AS SG "
                + " where "
                + " D." + DIColumns.Data.IUSNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId
                + " And ( "
                + " D." + DIColumns.Data.IndicatorNId + " is null Or D." + DIColumns.Data.UnitNId + " is null Or D." + DIColumns.Data.SubgroupValNId + " is null "
                + " OR "
                + " D." + DIColumns.Data.IndicatorNId + " <=0 Or D." + DIColumns.Data.UnitNId + " <=0 Or D." + DIColumns.Data.SubgroupValNId + " <=0 "
                + " ) "
                + " And ("
                + " I." + DIColumns.Indicator.IndicatorNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + "  and U." + DIColumns.Data.UnitNId + "  = IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " and  SG." + DIColumns.SubgroupVals.SubgroupValNId + " = IUS." + DIColumns.Data.SubgroupValNId
                + ") ";

            return RetVal;
        }

        /// <summary>
        /// Returns a query to update indicator_nid,unit_nid,subgroup_Val_nid .
        /// </summary>
        /// <param name="tableNames"></param>
        /// <returns></returns>
        public static string UpdateIndicatorUnitSubgroupVAlNids(DIServerType serverType, DITables tableNames)
        {
            string RetVal = string.Empty;

            switch (serverType)
            {
                case DIServerType.SqlServer:
                case DIServerType.SqlServerExpress:
                    RetVal = Data.Update.UpdateIUSNIdsForSQLServer(tableNames);
                    break;
                case DIServerType.MsAccess:
                    RetVal = Data.Update.UpdateIndicatorUnitSubgroupVAlNids(tableNames);
                    break;
                case DIServerType.Oracle:
                    RetVal = Data.Update.UpdateIUSNIdsForSQLServer(tableNames);
                    break;
                case DIServerType.MySql:
                    RetVal = Data.Update.UpdateIndicatorUnitSubgroupVAlNids(tableNames);
                    break;
                case DIServerType.Excel:
                    break;
                case DIServerType.Sqlite:
                    RetVal = Data.Update.UpdateIUSNIdsForSQLLite(tableNames);
                    break;
                default:
                    break;
            }

            return RetVal;
        }

        /// <summary>
        /// Returns a query to update ICIUSOrder into data table from IC_IUS table
        /// </summary>
        /// <param name="tablesName"></param>
        /// <returns></returns>
        public static string UpdateICIUSOrderIntoDataTableFrmICIUS(DITables tablesName)
        {
            string RetVal = string.Empty;
            RetVal = " UPDATE " + tablesName.Data + " D," + tablesName.IndicatorClassificationsIUS + " ICIUS "
                + " SET D." + DIColumns.Data.ICIUSOrder + "=ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSOrder
                + " WHERE ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId + "=D." + DIColumns.Data.IUSNId
                + " AND ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + "= D." + DIColumns.Data.SourceNId
                + " AND ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSOrder + " >0";

            return RetVal;
        }

        /// <summary>
        /// Update IndicatorNID,unitnid and subgroupvalnid in ut_data table
        /// </summary>
        /// <param name="tableNames"></param>
        /// <returns></returns>
        public static string UpdateIUSNIdsForSQLLite(DITables tableNames)
        {
            string RetVal = string.Empty;
            StringBuilder Sb = new StringBuilder();

            Sb.Append("UPDATE " + tableNames.Data);

            Sb.Append(" SET " + DIColumns.Data.IndicatorNId);

            Sb.Append("= (SELECT " + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " FROM " + tableNames.IndicatorUnitSubgroup + " WHERE " + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = " + tableNames.Data + "." + DIColumns.Data.IUSNId + ")");

            Sb.Append(" ," + DIColumns.Data.UnitNId);

            Sb.Append("= (SELECT " + DIColumns.Indicator_Unit_Subgroup.UnitNId + " FROM " + tableNames.IndicatorUnitSubgroup + " WHERE " + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = " + tableNames.Data + "." + DIColumns.Data.IUSNId + ")");

            Sb.Append(" ," + DIColumns.Data.SubgroupValNId);
            Sb.Append("= (SELECT " + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " FROM " + tableNames.IndicatorUnitSubgroup + " WHERE " + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = " + tableNames.Data + "." + DIColumns.Data.IUSNId + ")");

            RetVal = Sb.ToString();

            return RetVal;
        }

        /// <summary>
        /// Update ISMRD value
        /// </summary>
        /// <param name="tableNames"></param>
        /// <returns></returns>
        public static string UpdateISMRD(DITables tableNames)
        {

            string RetVal = string.Empty;
            StringBuilder Sb = new StringBuilder();

            Sb.Append("UPDATE " + tableNames.Data);
            Sb.Append(" SET " + DIColumns.Data.IsMRD + "=1");
            Sb.Append(" WHERE " + DIColumns.Data.DataNId + " IN ( ");
            Sb.Append(" SELECT " + DIColumns.Data.DataNId + " FROM (");
            Sb.Append(" SELECT  d1." + DIColumns.Data.IUSNId + ",d1." + DIColumns.Data.AreaNId + ",d1." + DIColumns.Data.TimePeriodNId + ",MAX(d1." + DIColumns.Data.DataNId + ") AS Data_NId FROM ");

            Sb.Append(" ( SELECT  d2." + DIColumns.Data.IUSNId + ",d2." + DIColumns.Data.AreaNId + ",t2." + DIColumns.Timeperiods.TimePeriod + ",t2." + DIColumns.Timeperiods.TimePeriodNId + ",d2." + DIColumns.Data.DataNId + " FROM " + tableNames.Data + " d2," + tableNames.TimePeriod + " t2 ");

            Sb.Append(" WHERE d2." + DIColumns.Data.TimePeriodNId + " = t2." + DIColumns.Timeperiods.TimePeriodNId + ") as d1,");
            Sb.Append(" ( SELECT  d." + DIColumns.Data.IUSNId + ", d." + DIColumns.Data.AreaNId + ", MAX(t." + DIColumns.Timeperiods.TimePeriod + ") AS timeperiod ");
            Sb.Append(" FROM " + tableNames.Data + " d," + tableNames.TimePeriod + " t WHERE d." + DIColumns.Data.TimePeriodNId + "= t." + DIColumns.Timeperiods.TimePeriodNId);

            Sb.Append(" GROUP BY d." + DIColumns.Data.IUSNId + ",d." + DIColumns.Data.AreaNId + ") AS MRDTable ");

            Sb.Append(" WHERE d1." + DIColumns.Data.IUSNId + "= MRDTable." + DIColumns.Data.IUSNId);
            Sb.Append(" AND d1." + DIColumns.Data.AreaNId + "= MRDTable." + DIColumns.Data.AreaNId);
            Sb.Append(" AND d1." + DIColumns.Timeperiods.TimePeriod + "= MRDTable." + DIColumns.Timeperiods.TimePeriod);
            Sb.Append(" GROUP BY d1." + DIColumns.Data.IUSNId + ",d1." + DIColumns.Data.AreaNId + ",d1." + DIColumns.Data.TimePeriodNId);
            Sb.Append(") AS d4 )");

            RetVal = Sb.ToString();

            return RetVal;
        }


        /// <summary>
        /// Update ISMultipleSource field
        /// </summary>
        /// <param name="tableNames"></param>
        /// <returns></returns>
        public static string UpdateISMultipleSource(DITables tableNames)
        {
            string RetVal = string.Empty;
            StringBuilder Sb = new StringBuilder();


            Sb.Append("UPDATE " + tableNames.Data);
            Sb.Append(" SET " + DIColumns.Data.MultipleSource + "=1");
            Sb.Append(" WHERE " + DIColumns.Data.DataNId + " IN ( ");

            Sb.Append(" SELECT " + DIColumns.Data.DataNId + " FROM (");

            Sb.Append(" SELECT  d1." + DIColumns.Data.DataNId + " FROM ");

            Sb.Append(" ( SELECT  d2." + DIColumns.Data.IUSNId + ",d2." + DIColumns.Data.AreaNId + ",t2." + DIColumns.Timeperiods.TimePeriod + ",t2." + DIColumns.Timeperiods.TimePeriodNId + ",d2." + DIColumns.Data.DataNId + " FROM " + tableNames.Data + " d2," + tableNames.TimePeriod + " t2 ");

            Sb.Append(" WHERE d2." + DIColumns.Data.TimePeriodNId + " = t2." + DIColumns.Timeperiods.TimePeriodNId + ") as d1,");

            Sb.Append(" ( SELECT  d." + DIColumns.Data.IUSNId + ", d." + DIColumns.Data.AreaNId + ", MAX(t." + DIColumns.Timeperiods.TimePeriod + ") AS timeperiod ");
            Sb.Append(" FROM " + tableNames.Data + " d," + tableNames.TimePeriod + " t WHERE d." + DIColumns.Data.TimePeriodNId + "= t." + DIColumns.Timeperiods.TimePeriodNId);

            Sb.Append(" GROUP BY d." + DIColumns.Data.IUSNId + ",d." + DIColumns.Data.AreaNId + ") AS MRDTable ");
            Sb.Append(" WHERE d1." + DIColumns.Data.IUSNId + "= MRDTable." + DIColumns.Data.IUSNId);
            Sb.Append(" AND d1." + DIColumns.Data.AreaNId + "= MRDTable." + DIColumns.Data.AreaNId);
            Sb.Append(" AND d1." + DIColumns.Timeperiods.TimePeriod + "= MRDTable." + DIColumns.Timeperiods.TimePeriod);

            Sb.Append(") AS d4 ");

            Sb.Append(" WHERE " + DIColumns.Data.DataNId + "=d4." + DIColumns.Data.DataNId + ")");

            RetVal = Sb.ToString();

            return RetVal;
        }


        /// <summary>
        /// Update IU Nid 
        /// </summary>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string UpdateIUNIds(DITables tableNames, DIServerType serverType)
        {
            string RetVal = string.Empty;
            StringBuilder Sb = new StringBuilder();

            Sb.Append("UPDATE " + tableNames.Data);
            Sb.Append(" SET " + DIColumns.Data.IUNId + "=");

            if (serverType == DIServerType.MsAccess)
            {
                Sb.Append(DIColumns.Data.IndicatorNId + " & '_' & " + DIColumns.Data.UnitNId);
            }
            else if (serverType == DIServerType.SqlServer || serverType == DIServerType.SqlServerExpress)
            {
                //SELECT Cast(Indicator_NId as varchar(25)) + '_' + Cast(Unit_NId as varchar(25)) FROM UT_Data
                //Sb.Append("Cast(" + DIColumns.Data.IndicatorNId + " as varchar(25) + '_' + Cast(" + DIColumns.Data.UnitNId + " as varchar(25))");
                Sb.Append("(CAST(" + DIColumns.Data.IndicatorNId + " AS varchar(25))   + '_' + CAST(" + DIColumns.Data.UnitNId + " AS varchar(25)))");
            }
            else if (serverType == DIServerType.MySql)
            {
                Sb.Append(" CONCAT(CAST(" + DIColumns.Data.IndicatorNId + " AS CHAR),'_', CAST(" + DIColumns.Data.UnitNId + " AS CHAR))");
            }

            RetVal = Sb.ToString();

            return RetVal;
        }


        /// <summary>
        /// Returns qurey to insert DataValue column into UT_Area_en table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string AlterDataValueColumnDataTypeToDouble(DITables tablesName, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;

            RetVal = "ALTER TABLE " + tablesName.Data + " Add COLUMN  " + DIColumns.Data.DataValue + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " Decimal(18,5) ";
                }
                else
                {
                    RetVal += " Decimal(18,5) ";
                }
            }
            else
            {
                RetVal += " Double ";
            }

            return RetVal;
        }


        #endregion

        #endregion
    }
}
