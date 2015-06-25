using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DevInfo.Lib.DI_LibDAL.UserSelection;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;

namespace DevInfo.Lib.DI_LibBAL.Export
{
    internal class Common
    {

        internal static string GetCommaSeperatedString(System.Data.DataTable sourceTable, string columnName)
        {
            string RetVal = string.Empty;
            string[] DistinctColumn = new string[1];
            DataColumn Column = null;
            DataTable TempTable = null;

            DistinctColumn[0] = columnName;
            if (sourceTable != null && sourceTable.Columns.Contains(columnName))
            {
                //-- Get Distinct records for given columnName
                TempTable = sourceTable.DefaultView.ToTable(true, columnName);

                Column = (DataColumn)(sourceTable.Columns[columnName]);
                //DataTable Table2 = table.DefaultView.ToTable(true, DistinctColumn);
                foreach (DataRow dr in TempTable.Rows)
                {
                    if (Column.DataType.Name.ToLower() == "String".ToLower())
                    {
                        if (RetVal.Length == 0)
                        {
                            RetVal = "'" + dr[columnName].ToString() + "'";
                        }
                        else
                        {
                            RetVal += ", '" + dr[columnName].ToString() + "'";
                        }
                    }
                    else
                    {
                        if (RetVal.Length == 0)
                        {
                            RetVal = dr[columnName].ToString();
                        }
                        else
                        {
                            RetVal += ", " + dr[columnName].ToString();
                        }
                    }
                }
            }
            return RetVal;
        }

        internal static string MergeNIds(string NIds1, string NIds2)
        {
            string RetVal = string.Empty;

            if (string.IsNullOrEmpty(NIds1) == false)
            {
                RetVal = NIds1;
            }

            if (string.IsNullOrEmpty(NIds2) == false)
            {
                if (string.IsNullOrEmpty(RetVal))
                {
                    RetVal = NIds2;
                }
                else
                {
                    RetVal = string.Concat(RetVal, ",", NIds2);
                }
            }
            return RetVal;
        }

        /// <summary>
        /// It returns Dilimited NIds Array in form of {1,4,8} & { 2,6,9}, if FilterNIds are in given as 1_2, 4_6, 8_9 
        /// </summary>
        /// <param name="filterNIds"></param>
        /// <param name="characterUsedToJoinTwoNIds"></param>
        /// <returns></returns>
        internal static string[] GetDelimtedNIdsArray(string filterNIds, char characterUsedToJoinTwoNIds)
        {
            // It returns Dilimited NIds Array in form of {1,4,8} & { 2,6,9}, if FilterNIds are in given as 1_2, 4_6, 8_9 
            string[] RetVal = new string[2];
            string[] SplittedNIds = null;

            if (string.IsNullOrEmpty(filterNIds) == false)
            {
                SplittedNIds = filterNIds.Split(',');

                //- Get DelimietedNId for each Element
                // For e.g: 1_2,4_6,8_9
                foreach (string JoinedNId in SplittedNIds)
                {
                    RetVal[0] += "," + JoinedNId.Split(characterUsedToJoinTwoNIds)[0];
                    RetVal[1] += "," + JoinedNId.Split(characterUsedToJoinTwoNIds)[1];
                }

                RetVal[0] = RetVal[0].Replace("'", "");
                RetVal[0] = RetVal[0].Trim(',');

                RetVal[1] = RetVal[1].Replace("'", "");
                RetVal[1] = RetVal[1].Trim(',');
            }

            return RetVal;
        }

        internal static string GetDataValueRangeFilderString(DataValueFilter dataValueFilter)
        {
            string WhereClause = " WHERE NOT (";
            string CurrentThreadDecimalChar = System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            string InvariantCultureDecimalChar = new System.Globalization.CultureInfo("en-US").NumberFormat.NumberDecimalSeparator;
            switch (dataValueFilter.OpertorType)
            {
                case OpertorType.EqualTo:
                    WhereClause += " VAL([" + Data.DataValue + "]) = " + dataValueFilter.FromDataValue.ToString().Replace(CurrentThreadDecimalChar, InvariantCultureDecimalChar);
                    break;
                case OpertorType.Between:
                    //Inclusive of min & max values as per ANSI SQL
                    WhereClause += "(VAL([" + Data.DataValue + "]) >= " + dataValueFilter.FromDataValue.ToString().Replace(CurrentThreadDecimalChar, InvariantCultureDecimalChar) + " AND VAL([" + Data.DataValue + "]) <= " + dataValueFilter.ToDataValue.ToString().Replace(CurrentThreadDecimalChar, InvariantCultureDecimalChar) + ")";
                    break;
                case OpertorType.GreaterThan:
                    WhereClause += "(VAL([" + Data.DataValue + "]) > " + dataValueFilter.FromDataValue.ToString().Replace(CurrentThreadDecimalChar, InvariantCultureDecimalChar) + ")";
                    break;
                case OpertorType.LessThan:
                    WhereClause += "(VAL([" + Data.DataValue + "]) < " + dataValueFilter.ToDataValue.ToString().Replace(CurrentThreadDecimalChar, InvariantCultureDecimalChar) + ")";
                    break;
            }

            WhereClause += " )";

            return WhereClause;
        }

    }
}
