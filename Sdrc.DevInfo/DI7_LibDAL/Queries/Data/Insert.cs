using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.Data
{
    /// <summary>
    /// Provides sql queries to insert records
    /// </summary>
    public static class Insert
    {
        #region "-- Private --"

        #region "-- Variables --"

        private const string TableName = "data";


        #endregion

        #endregion

        #region "-- public --"

        #region "-- Methods --"

        /// <summary>
        /// Returns insert query to insert data into data table
        /// </summary>
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
        public static string InsertDataValue(string dataPrefix, int IUSNid, int timeperiodNid, int areaNid, string datavalue, int footnoteNid, int sourceNid, string startDate, string endDate, string denominator)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + dataPrefix + Insert.TableName + "(" + DIColumns.Data.IUSNId + ","
           + DIColumns.Data.TimePeriodNId + ","
           + DIColumns.Data.AreaNId + ","
           + DIColumns.Data.DataValue + ","
           + DIColumns.Data.FootNoteNId + ","
           + DIColumns.Data.SourceNId;

            if (startDate.Length > 0)
            {
                RetVal += "," + DIColumns.Data.StartDate + "," + DIColumns.Data.EndDate;
            }

            if (denominator.Length > 0)
            {
                RetVal += "," + DIColumns.Data.DataDenominator;
            }

            RetVal += ")" + "VALUES(" + IUSNid + "," + timeperiodNid + ","
                    + areaNid + ",'" + DIQueries.RemoveQuotesForSqlQuery(datavalue) + "',"
                    + footnoteNid + " , " + sourceNid;

            if (startDate.Length > 0)
            {
                RetVal += ",'" + Convert.ToDateTime(startDate).Date + "'";
            }

            if (endDate.Length > 0)
            {
                RetVal += ",'" + Convert.ToDateTime(endDate).Date + "'";
            }

            if (denominator.Length > 0)
                RetVal += ",'" + denominator + "')";
            else
                RetVal += ")";

            return RetVal;
        }

        /// <summary>
        /// Returns insert query to insert numeric data into data table
        /// </summary>
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
        public static string InsertDataValue(string dataPrefix, int IUSNid, int timeperiodNid, int areaNid, string datavalue, int footnoteNid, int sourceNid, string startDate, string endDate, string denominator, bool isPlannedValue, string cfIntervalUpper, string cfIntervalLower, bool isNumeric)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + dataPrefix + Insert.TableName + "(" + DIColumns.Data.IUSNId + ","
           + DIColumns.Data.TimePeriodNId + ","
           + DIColumns.Data.AreaNId + ","
           + DIColumns.Data.DataValue + ","
           + DIColumns.Data.FootNoteNId + ","
           + DIColumns.Data.SourceNId + ","
           + DIColumns.Data.IsTextualData;

            if (startDate.Length > 0)
            {
                RetVal += "," + DIColumns.Data.StartDate + "," + DIColumns.Data.EndDate;
            }

            RetVal += "," + DIColumns.Data.IsPlannedValue + "," + DIColumns.Data.ConfidenceIntervalUpper + "," + DIColumns.Data.ConfidenceIntervalLower;

            if (denominator.Length > 0)
            {
                RetVal += "," + DIColumns.Data.DataDenominator;
            }

            RetVal += ")" + "VALUES(" + IUSNid + "," + timeperiodNid + ","
                    + areaNid + ",";

            if (isNumeric)
            {
                RetVal += datavalue + ",";
            }
            else
            {
                RetVal += "'" + DIQueries.RemoveQuotesForSqlQuery(datavalue) + "',";
            }

            RetVal += footnoteNid + " , " + sourceNid + ",0";

            if (startDate.Length > 0)
            {
                RetVal += ",'" + Convert.ToDateTime(startDate).Date + "'";
            }

            if (endDate.Length > 0)
            {
                RetVal += ",'" + Convert.ToDateTime(endDate).Date + "'";
            }

            RetVal += "," + Convert.ToInt16(isPlannedValue) + "," + cfIntervalUpper + "," + cfIntervalLower;

            if (denominator.Length > 0)
                RetVal += ",'" + denominator + "')";
            else
                RetVal += ")";

            return RetVal;
        }

        /// <summary>
        /// Insert textual data into data table
        /// </summary>
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
        /// <param name="isPlannedValue"></param>
        /// <param name="cfIntervalUpper"></param>
        /// <param name="cfIntervalLower"></param>
        /// <returns></returns>
        public static string InsertTexttualDataValue(string dataPrefix, int IUSNid, int timeperiodNid, int areaNid, string datavalue, int footnoteNid, int sourceNid, string startDate, string endDate, string denominator, bool isPlannedValue, string cfIntervalUpper, string cfIntervalLower)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + dataPrefix + Insert.TableName + "(" + DIColumns.Data.IUSNId + ","
           + DIColumns.Data.TimePeriodNId + ","
           + DIColumns.Data.AreaNId + ","
           + DIColumns.Data.TextualDataValue + ","
           + DIColumns.Data.FootNoteNId + ","
           + DIColumns.Data.SourceNId + ","
           + DIColumns.Data.IsTextualData;

            if (startDate.Length > 0)
            {
                RetVal += "," + DIColumns.Data.StartDate + "," + DIColumns.Data.EndDate;
            }

            RetVal += "," + DIColumns.Data.IsPlannedValue + "," + DIColumns.Data.ConfidenceIntervalUpper + "," + DIColumns.Data.ConfidenceIntervalLower;

            if (denominator.Length > 0)
            {
                RetVal += "," + DIColumns.Data.DataDenominator;
            }

            RetVal += ")" + "VALUES(" + IUSNid + "," + timeperiodNid + ","
                    + areaNid + ",'" + DIQueries.RemoveQuotesForSqlQuery(datavalue) + "',"
                    + footnoteNid + " , " + sourceNid + ",1";

            if (startDate.Length > 0)
            {
                RetVal += ",'" + Convert.ToDateTime(startDate).Date + "'";
            }

            if (endDate.Length > 0)
            {
                RetVal += ",'" + Convert.ToDateTime(endDate).Date + "'";
            }

            RetVal += "," + Convert.ToInt16(isPlannedValue) + "," + cfIntervalUpper + "," + cfIntervalLower;

            if (denominator.Length > 0)
                RetVal += ",'" + denominator + "')";
            else
                RetVal += ")";

            return RetVal;
        }

        /// <summary>
        /// Insert datavalue into data table
        /// </summary>
        /// <param name="dataPrefix">DataPrefix like UT_</param>
        /// <param name="IUSNid">IUS Nid</param>
        /// <param name="timeperiodNid">TimePeriod Nid </param>
        /// <param name="areaNid">Area Nid</param>
        /// <param name="datavalue">Data Value </param>
        /// <param name="footnoteNid">Footnote Nid </param>
        /// <param name="sourceNid">Source Nid</param>
        /// <returns></returns>
        public static string InsertDataValue(string dataPrefix, int IUSNid, int timeperiodNid, int areaNid, string datavalue, int footnoteNid, int sourceNid)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + dataPrefix + Insert.TableName + "(" + DIColumns.Data.IUSNId + ","
           + DIColumns.Data.TimePeriodNId + ","
           + DIColumns.Data.AreaNId + ","
           + DIColumns.Data.DataValue + ","
           + DIColumns.Data.FootNoteNId + ","
           + DIColumns.Data.SourceNId + ")"
           + "VALUES(" + IUSNid + "," + timeperiodNid + ","
           + areaNid + ",'" + DIQueries.RemoveQuotesForSqlQuery(datavalue) + "',"
           + footnoteNid + " , " + sourceNid + " ) ";

            return RetVal;
        }

        /// <summary>
        /// Returns qurey to insert Indicator_Nid column into data table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertIndicatorNidColumn(string dataPrefix, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + new DITables(dataPrefix, String.Empty).Data + " ADD COLUMN  " + DIColumns.Data.IndicatorNId + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " int(4) ";
                }
                else
                {
                    RetVal += " int ";
                }
            }
            else
            {
                RetVal += " number ";
            }

            return RetVal;
        }


        /// <summary>
        /// Returns qurey to insert Unit_Nid column into data table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertUnitNidColumn(string dataPrefix, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + new DITables(dataPrefix, String.Empty).Data + " ADD COLUMN  " + DIColumns.Data.UnitNId + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " int(4) ";
                }
                else
                {
                    RetVal += " int ";
                }
            }
            else
            {
                RetVal += " number ";
            }
            return RetVal;
        }

        /// <summary>
        /// Returns qurey to insert Subgroup_Val_Nid column into data table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertSubgroupVAlNidColumn(string dataPrefix, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + new DITables(dataPrefix, String.Empty).Data + " ADD COLUMN  " + DIColumns.Data.SubgroupValNId + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " int(4) ";
                }
                else
                {
                    RetVal += " int ";
                }
            }
            else
            {
                RetVal += " number ";
            }
            return RetVal;
        }



        /// <summary>
        /// Returns qurey to insert IC_IUS_Order column into data table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertICIUSOrderColumn(string dataPrefix, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + new DITables(dataPrefix, String.Empty).Data + " ADD COLUMN  " + DIColumns.Data.ICIUSOrder + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " int(4) ";
                }
                else
                {
                    RetVal += " int ";
                }
            }
            else
            {
                RetVal += " number ";
            }
            return RetVal;
        }

        /// <summary>
        /// Returns qurey to insert Textual_Data_Value column into data table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertDI7TextualDataValueColumn(string dataPrefix, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + new DITables(dataPrefix, String.Empty).Data + " ADD COLUMN  " + DIColumns.Data.TextualDataValue + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " varchar(4000) ";
                }
                else
                {
                    RetVal += " nvarchar(4000) ";
                }
            }
            else
            {
                RetVal += " Memo ";
            }
            return RetVal;
        }

        /// <summary>
        /// Returns qurey to insert IsTextualData column into data table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertDI7IsTextualDataColumn(string dataPrefix, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + new DITables(dataPrefix, String.Empty).Data + " ADD COLUMN  " + DIColumns.Data.IsTextualData + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " TinyInt(1) ";
                }
                else
                {
                    RetVal += " Bit ";
                }
            }
            else
            {
                RetVal += " Bit ";
            }
            return RetVal;
        }


        /// <summary>
        /// Returns qurey to insert IsPlannedValue column into data table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertDI7IsPlannedValueColumn(string dataPrefix, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + new DITables(dataPrefix, String.Empty).Data + " ADD COLUMN  " + DIColumns.Data.IsPlannedValue + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " TinyInt(1) ";
                }
                else
                {
                    RetVal += " Bit ";
                }
            }
            else
            {
                RetVal += " Bit ";
            }
            return RetVal;
        }


        /// <summary>
        /// Returns qurey to insert IsMRD column into data table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertDI7IsMRDColumn(string dataPrefix, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + new DITables(dataPrefix, String.Empty).Data + " ADD COLUMN  " + DIColumns.Data.IsMRD + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " TinyInt(1) ";
                }
                else
                {
                    RetVal += " Bit ";
                }
            }
            else
            {
                RetVal += " Bit ";
            }
            return RetVal;
        }

        /// <summary>
        /// Returns qurey to insert IsMRD column into data table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertDI7IUNIdColumn(string dataPrefix, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + new DITables(dataPrefix, String.Empty).Data + " ADD COLUMN  " + DIColumns.Data.IUNId + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " varchar(255) ";
                }
                else
                {
                    RetVal += " varchar(255) ";
                }
            }
            else
            {
                RetVal += " Text(255) ";
            }
            return RetVal;
        }

        /// <summary>
        /// Returns qurey to insert ConfidenceIntervalLower column into data table 
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertDI7ConfidenceIntervalLowerColumn(string dataPrefix, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + new DITables(dataPrefix, String.Empty).Data + " ADD COLUMN  " + DIColumns.Data.ConfidenceIntervalLower + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " int(4) ";
                }
                else
                {
                    RetVal += " int ";
                }
            }
            else
            {
                RetVal += " Number ";
            }
            return RetVal;
        }


        /// <summary>
        /// Returns qurey to insert ConfidenceIntervalUpper column into data table 
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertDI7ConfidenceIntervalUpperColumn(string dataPrefix, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + new DITables(dataPrefix, String.Empty).Data + " ADD COLUMN  " + DIColumns.Data.ConfidenceIntervalUpper + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " int(4) ";
                }
                else
                {
                    RetVal += " int ";
                }
            }
            else
            {
                RetVal += " Number ";
            }

            return RetVal;
        }



        /// <summary>
        /// Returns qurey to insert MultipleSource column into data table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertDI7MultipleSourceColumn(string dataPrefix, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + new DITables(dataPrefix, String.Empty).Data + " ADD COLUMN  " + DIColumns.Data.MultipleSource + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " TinyInt(1) ";
                }
                else
                {
                    RetVal += " Bit ";
                }
            }
            else
            {
                RetVal += " Bit ";
            }
            return RetVal;
        }
        #endregion

        #endregion
    }
}

