using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.Indicator
{
    /// <summary>
    /// Provides sql queries to get records
    /// </summary>
    public class Select
    {
        #region "-- Private --"

        #region "-- Variables --"

        private DITables TablesName;

        #endregion


        #region "-- New / Dispose --"

        private Select()
        {
            // don't implement this method
        }

        #endregion

        #endregion

        #region "-- Public / Internal--"

        #region "-- New / Dispose --"


        internal Select(DITables tablesName)
        {
            this.TablesName = tablesName;
        }

        #endregion


        #region "-- Methods --"

        /// <summary>
        /// Get Missing Indicator metadata.
        /// </summary>
        /// <returns></returns>
        public string GetMissingInfoIndicators(DIServerType serverType)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT " + DIColumns.Indicator.IndicatorName );
            sbQuery.Append(" FROM " + this.TablesName.Indicator );

            if (serverType == DIServerType.MsAccess)
            {
                sbQuery.Append(" WHERE ((" + DIColumns.Indicator.IndicatorInfo + " IS Null) or Ltrim(Rtrim(" + DIColumns.Indicator.IndicatorInfo + ")) = '' )");
            }
            else
            {
                sbQuery.Append(" WHERE (" + DIColumns.Indicator.IndicatorInfo + " IS Null OR " + DIColumns.Indicator.IndicatorInfo + " LIKE  '')");
            }
            RetVal = sbQuery.ToString();

            return RetVal;
        }

        /// <summary>
        /// Get  NId, Name, GID, Global from Indicator table for a given  Filter Criteria
        /// </summary>
        /// <param name="filterFieldType">Applicable for NId, Name, Search, NIdNotIn, NameNotIn</param>
        /// <param name="filterText">For FilterFieldType "Search" FilterText should be Indicator.IndicatorName LIKE '%Poverty%' or Indicator.IndicatorName LIKE '%Under-five%'</param>
        /// <param name="fieldSelection">Use heavy for all fields or use light to exclude IndicatorInfo field</param>
        /// <returns>Sql query string</returns>
        public string GetIndicator(FilterFieldType filterFieldType, string filterText, FieldSelection fieldSelection)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            //   SELECT clause
            sbQuery.Append("SELECT " + DIColumns.Indicator.IndicatorNId);    //FieldSelection.NId
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append("," + DIColumns.Indicator.IndicatorName + "," + DIColumns.Indicator.IndicatorGId + "," + DIColumns.Indicator.IndicatorGlobal + "," + DIColumns.Indicator.HighIsGood);
            }
            if (fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append("," + DIColumns.Indicator.IndicatorInfo);
            }

            //   FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.Indicator);

            //   WHERE Clause
            if (filterFieldType != FilterFieldType.None && filterText.Length>0)
                sbQuery.Append(" WHERE ");
            if (filterText.Length > 0)
            {
                switch (filterFieldType)
                {
                    case FilterFieldType.None:
                        break;
                    case FilterFieldType.NId:
                        sbQuery.Append(DIColumns.Indicator.IndicatorNId + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.ParentNId:
                        break;
                    case FilterFieldType.ID:
                        break;
                    case FilterFieldType.GId:
                        sbQuery.Append(DIColumns.Indicator.IndicatorGId + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.Name:
                        sbQuery.Append(DIColumns.Indicator.IndicatorName + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.Search:
                        sbQuery.Append(filterText);
                        break;
                    case FilterFieldType.Global:
                        break;
                    case FilterFieldType.NIdNotIn:
                        sbQuery.Append(DIColumns.Indicator.IndicatorNId + " NOT IN (" + filterText + ")");
                        break;
                    case FilterFieldType.NameNotIn:
                        sbQuery.Append(DIColumns.Indicator.IndicatorName + " NOT IN (" + filterText + ")");
                        break;
                    default:
                        break;
                }
            }

            //   ORDER BY Clause
            // sbQuery.Append(" ORDER BY " + DIColumns.Indicator.IndicatorName);

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get  NId, Name, GID, Global from Indicator table based on Indicator Classification Type
        /// </summary>
        /// <param name="ICType">Indicator Classification Type with which associated indicator are to be retrieved</param>
        /// <param name="ICNId">ICNId which may be -1 if only ICType is to be considered</param>
        /// <param name="fieldSelection">Use heavy for all fields or use light to exclude IndicatorInfo field</param>
        /// <returns>Sql query string</returns>
        public string GetIndicatorByIC(ICType ICType, string ICNId, FieldSelection fieldSelection)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            //   SELECT clause
            sbQuery.Append("SELECT DISTINCT I." + DIColumns.Indicator.IndicatorNId);    //FieldSelection.NId
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorName + ",I." + DIColumns.Indicator.IndicatorGId + ",I." + DIColumns.Indicator.IndicatorGlobal + ",I." + DIColumns.Indicator.HighIsGood);
            }
            if (fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorInfo);
            }

            //   FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.Indicator + " AS I");
            sbQuery.Append("," + this.TablesName.IndicatorUnitSubgroup + " AS IUS");
            sbQuery.Append("," + this.TablesName.IndicatorClassificationsIUS + " AS ICIUS");
            sbQuery.Append("," + this.TablesName.IndicatorClassifications + " AS IC");

            //   WHERE Clause
            sbQuery.Append(" WHERE I." + DIColumns.Indicator.IndicatorNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId);
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId);
            sbQuery.Append(" AND ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + " = IC." + DIColumns.IndicatorClassifications.ICNId);
            sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICType + " = " + DIQueries.ICTypeText[ICType]);
            if (!string.IsNullOrEmpty(ICNId) && ICNId != "-1")
            {
                sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICNId + " IN (" + ICNId + ")");
            }


            //   ORDER BY Clause
            sbQuery.Append(" ORDER BY " + DIColumns.Indicator.IndicatorName);

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get  NId, Name, GID, Global from Indicator table based on Indicator Classification Type
        /// </summary>
        /// <param name="ICType">Indicator Classification Type with which associated indicator are to be retrieved</param>
        /// <param name="ICNId">ICNId which may be -1 if only ICType is to be considered</param>
        /// <param name="fieldSelection">Use heavy for all fields or use light to exclude IndicatorInfo field</param>
        /// <returns>Sql query string</returns>
        public string GetAllIndicatorByIC(ICType ICType, string ICNId, FieldSelection fieldSelection)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            //   SELECT clause
            sbQuery.Append("SELECT I." + DIColumns.Indicator.IndicatorNId);    //FieldSelection.NId
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorName + ",I." + DIColumns.Indicator.IndicatorGId + ",I." + DIColumns.Indicator.IndicatorGlobal);
            }
            if (fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorInfo);
            }

            //   FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.Indicator + " AS I");
            sbQuery.Append("," + this.TablesName.IndicatorUnitSubgroup + " AS IUS");
            sbQuery.Append("," + this.TablesName.IndicatorClassificationsIUS + " AS ICIUS");
            sbQuery.Append("," + this.TablesName.IndicatorClassifications + " AS IC");

            //   WHERE Clause
            sbQuery.Append(" WHERE I." + DIColumns.Indicator.IndicatorNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId);
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId);
            sbQuery.Append(" AND ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + " = IC." + DIColumns.IndicatorClassifications.ICNId);
            sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICType + " = " + DIQueries.ICTypeText[ICType]);
            if (!string.IsNullOrEmpty(ICNId) && ICNId != "-1")
            {
                sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICNId + " IN (" + ICNId + ")");
            }


            //   ORDER BY Clause
            sbQuery.Append(" ORDER BY " + DIColumns.Indicator.IndicatorName);

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// AutoSelect Indiactor based on TimePeriod and Area selection
        /// </summary>
        /// <param name="timePeriodNIds">commma delimited Timeperiod NIds which may be blank</param>
        /// <param name="areaNIds">commma delimited Area NIds which may be blank</param>
        /// <param name="ICNId">Indicator Classification NId of selected item in IC tree which may be -1 for all </param>
        /// <param name="fieldSelection">NId, Light or Heavy</param>
        /// <returns>Sql query string</returns>
        public string GetAutoSelectByTimePeriodArea(string timePeriodNIds, string areaNIds, int ICNId, FieldSelection fieldSelection)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            // Set SELECT clause based on fieldSelection type
            sbQuery.Append("SELECT DISTINCT I." + DIColumns.Indicator.IndicatorNId);    //FieldSelection.NId
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(", I." + DIColumns.Indicator.IndicatorName + ", I." + DIColumns.Indicator.IndicatorGId + ", I." + DIColumns.Indicator.IndicatorGlobal);
            }
            if (fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(", I." + DIColumns.Indicator.IndicatorInfo);
            }

            // Set FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.Data + " AS D," + this.TablesName.IndicatorUnitSubgroup + " AS IUS," + this.TablesName.Indicator + " AS I");
            //sbQuery.Append(" FROM " + this.TablesName.Indicator + " AS I," + this.TablesName.IndicatorUnitSubgroup + " AS IUS," + this.TablesName.Data + " AS D");
            if (timePeriodNIds.Length > 0)
            {
                sbQuery.Append("," + this.TablesName.TimePeriod + " AS T");
            }
            if (areaNIds.Length > 0)
            {
                sbQuery.Append("," + this.TablesName.Area + " AS A");
            }
            sbQuery.Append("," + this.TablesName.IndicatorClassifications + " AS IC," + this.TablesName.IndicatorClassificationsIUS + " AS ICIUS");

            // Set WHERE clause 
            sbQuery.Append(" WHERE I." + DIColumns.Indicator.IndicatorNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = D." + DIColumns.Indicator_Unit_Subgroup.IUSNId);
            if (timePeriodNIds.Length > 0)
            {
                sbQuery.Append(" AND T." + DIColumns.Timeperiods.TimePeriodNId + " = D." + DIColumns.Data.TimePeriodNId + " AND T." + DIColumns.Timeperiods.TimePeriodNId + " IN (" + timePeriodNIds + ")");
            }
            if (areaNIds.Length > 0)
            {
                sbQuery.Append(" AND A." + DIColumns.Area.AreaNId + " = D." + DIColumns.Data.AreaNId + " AND A." + DIColumns.Area.AreaNId + " IN (" + areaNIds + ")");
            }
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId);
            sbQuery.Append(" AND ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + " = IC." + DIColumns.IndicatorClassifications.ICNId);
            if (ICNId != -1)
            {
                sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICNId + " = " + ICNId);
            }
            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// AutoSelect Indiactor based on TimePeriod, Area  and source selection
        /// </summary>
        /// <param name="timePeriodNIds">commma delimited Timeperiod NIds which may be blank</param>
        /// <param name="areaNIds">commma delimited Area NIds which may be blank</param>
        /// <param name="sourceNids">Source Nids which may be blank </param>
        /// <param name="fieldSelection">NId, Light or Heavy</param>
        /// <returns>Sql query string</returns>
        public string GetAutoSelectByTimePeriodAreaSource(string timePeriodNIds, string areaNIds, string sourceNids, FieldSelection fieldSelection)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            // Set SELECT clause based on fieldSelection type
            sbQuery.Append("SELECT DISTINCT I." + DIColumns.Indicator.IndicatorNId);    //FieldSelection.NId
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(", I." + DIColumns.Indicator.IndicatorName + ", I." + DIColumns.Indicator.IndicatorGId + ", I." + DIColumns.Indicator.IndicatorGlobal);
            }
            if (fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(", I." + DIColumns.Indicator.IndicatorInfo);
            }

            // Set FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.Data + " AS D," + this.TablesName.IndicatorUnitSubgroup + " AS IUS," + this.TablesName.Indicator + " AS I");
            
            
            // Set WHERE clause 
            sbQuery.Append(" WHERE I." + DIColumns.Indicator.IndicatorNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = D." + DIColumns.Indicator_Unit_Subgroup.IUSNId);

            if (timePeriodNIds.Length > 0)
            {
                sbQuery.Append("  AND D." + DIColumns.Timeperiods.TimePeriodNId + " IN (" + timePeriodNIds + ")");
            }
            if (areaNIds.Length > 0)
            {
                sbQuery.Append(" AND D." + DIColumns.Area.AreaNId + " IN (" + areaNIds + ")");
            }
            
                        
            if (!string.IsNullOrEmpty(sourceNids ))
            {
                sbQuery.Append(" AND D." + DIColumns.Data.SourceNId + " IN( " + sourceNids+") ");
            }

            sbQuery.Append(" ORDER BY " + DIColumns.Indicator.IndicatorName);
            RetVal = sbQuery.ToString();
            return RetVal;
        }


/// <summary>
        /// AutoSelect Indiactor based on TimePeriod, Area  and source selection
        /// </summary>
        /// <param name="timePeriodNIds">commma delimited Timeperiod NIds which may be blank</param>
        /// <param name="areaNIds">commma delimited Area NIds which may be blank</param>
        /// <param name="sourceNids">Source Nids which may be blank </param>
        /// <param name="fieldSelection">NId, Light or Heavy</param>
        /// <returns>Sql query string</returns>
        public string GetAutoSelectIndicator(string timePeriodNIds, string areaNIds, string sourceNids, FieldSelection fieldSelection)
        {
            string RetVal=string.Empty;

            RetVal = "SELECT I." + DIColumns.Indicator.IndicatorNId + ", I." + DIColumns.Indicator.IndicatorName + ", I." + DIColumns.Indicator.IndicatorGId + ", I." + DIColumns.Indicator.IndicatorGlobal + " FROM " + this.TablesName.Indicator
                + " AS I WHERE EXISTS (  SELECT *  FROM " + this.TablesName.Data + " as D WHERE  I." + DIColumns.Indicator.IndicatorNId + "= D." + DIColumns.Data.IndicatorNId +" ";


            ////////RetVal = "SELECT I." + DIColumns.Indicator.IndicatorNId + ", I." + DIColumns.Indicator.IndicatorName + ", I." + DIColumns.Indicator.IndicatorGId + ", I." + DIColumns.Indicator.IndicatorGlobal + " FROM " + this.TablesName.Indicator
            ////////    + " AS I WHERE EXISTS ( SELECT *  FROM  " + this.TablesName.IndicatorUnitSubgroup
            ////////    + " AS IUS WHERE I." + DIColumns.Indicator.IndicatorNId + "= IUS." + DIColumns.Indicator.IndicatorNId
            ////////    + " AND EXISTS (  SELECT *  FROM " + this.TablesName.Data + " as D WHERE  IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + "= D." + DIColumns.Data.IUSNId;

            if (!string.IsNullOrEmpty(timePeriodNIds))
            {
                RetVal += " AND D." + DIColumns.Data.TimePeriodNId + " IN( " + timePeriodNIds + " ) ";
            }
            if (!string.IsNullOrEmpty(areaNIds))
            {
                RetVal += " AND D." + DIColumns.Data.AreaNId + " IN( " + areaNIds + " ) ";
            }

            if (!string.IsNullOrEmpty(sourceNids))
            {
                RetVal += " AND D." + DIColumns.Data.SourceNId + " IN( " + sourceNids + " ) ";
            }
                
                
                RetVal += " ) ORDER BY I." + DIColumns.Indicator.IndicatorName + "";
                //RetVal += " )) ORDER BY I." + DIColumns.Indicator.IndicatorName + "";

            return RetVal;
        }


        /// <summary>
        /// AutoSelect Indiactor based on TimePeriod, Area  and source selection
        /// </summary>
        /// <param name="timePeriodNIds">commma delimited Timeperiod NIds which may be blank</param>
        /// <param name="areaNIds">commma delimited Area NIds which may be blank</param>
        /// <param name="sourceNids">Source Nids which may be blank </param>
        /// <param name="ICParentNId">Set IC_Parent_NID to get records against the given NID otherwise set 0 to get all auto select records</param>
        /// <param name="ICType"></param>
        /// <returns>Sql query string</returns>
        public string GetAutoSelectIndicator(string timePeriodNIds, string areaNIds, string sourceNids,int ICParentNId,ICType ICType)
        {
            string RetVal = string.Empty;

            RetVal = "SELECT I." + DIColumns.Indicator.IndicatorNId + ", I." + DIColumns.Indicator.IndicatorName + ", I." + DIColumns.Indicator.IndicatorGId + ", I." + DIColumns.Indicator.IndicatorGlobal + " FROM " + this.TablesName.Indicator
                + " AS I WHERE EXISTS ( SELECT *  FROM  " + this.TablesName.IndicatorUnitSubgroup
                + " AS IUS WHERE I." + DIColumns.Indicator.IndicatorNId + "= IUS." + DIColumns.Indicator.IndicatorNId
                + " AND EXISTS (  SELECT *  FROM " + this.TablesName.Data + " as D WHERE  IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + "= D." + DIColumns.Data.IUSNId;

            if (!string.IsNullOrEmpty(timePeriodNIds))
            {
                RetVal += " AND D." + DIColumns.Data.TimePeriodNId + " IN( " + timePeriodNIds + " ) ";
            }
            if (!string.IsNullOrEmpty(areaNIds))
            {
                RetVal += " AND D." + DIColumns.Data.AreaNId + " IN( " + areaNIds + " ) ";
            }

            if (!string.IsNullOrEmpty(sourceNids))
            {
                RetVal += " AND D." + DIColumns.Data.SourceNId + " IN( " + sourceNids + " ) ";
            }
            RetVal += " )";

             RetVal += " AND EXISTS ( SELECT * FROM " + this.TablesName.IndicatorClassificationsIUS + " ICIUS WHERE IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = ICIUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " AND  EXISTS ( SELECT * FROM " + this.TablesName.IndicatorClassifications + " IC  WHERE ICIUS." + DIColumns.IndicatorClassifications.ICNId + " =IC." + DIColumns.IndicatorClassifications.ICNId ;

            if (ICParentNId > 0)
            {
               RetVal += " AND IC." + DIColumns.IndicatorClassifications.ICParent_NId + " =" + ICParentNId + " ";
            }

            RetVal +=" AND IC."+ DIColumns.IndicatorClassifications.ICType +"="+ DIQueries.ICTypeText[ICType] + " )) ";
            

            RetVal +=") ORDER BY I." + DIColumns.Indicator.IndicatorName + "";

            return RetVal;
        }



        /// <summary>
        /// Get IUS NIds on the basis of timeperiod and area NIds
        /// </summary>
        /// <param name="timePeriodNIds"></param>
        /// <param name="areaNIds"></param>
        /// <returns></returns>
        public string GetAutoSelectByTimePeriodArea(string timePeriodNIds, string areaNIds, string iusNIDs)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();
            String sWhere = string.Empty;

            sbQuery.Append("SELECT DISTINCT D." + DIColumns.Data.IUSNId);
            sbQuery.Append(" FROM " + TablesName.Data + " D");

            if (!string.IsNullOrEmpty(timePeriodNIds) || !string.IsNullOrEmpty(areaNIds) || !string.IsNullOrEmpty(iusNIDs))
            {
                sbQuery.Append(" WHERE ");
            }

            if (areaNIds.Length > 0)
            {
                if (sWhere.Length > 0)
                {
                    sWhere += " AND ";
                }
                sWhere += " D." + DIColumns.Data.AreaNId + " IN (" + areaNIds + ") ";
            }
            if (timePeriodNIds.Length > 0)
            {
                if (sWhere.Length > 0)
                {
                    sWhere += " AND ";
                }
                sWhere += " D." + DIColumns.Data.TimePeriodNId + " IN (" + timePeriodNIds + ")";
            }
            if (iusNIDs.Length > 0)
            {
                if (sWhere.Length > 0)
                {
                    sWhere += " AND ";
                }
                sWhere += " D." + DIColumns.Data.IUSNId + " IN (" + iusNIDs + ")";
            }
            sbQuery.Append(sWhere);

            RetVal = sbQuery.ToString();
            return RetVal;
        }


        /// <summary>
        /// Get Indicator NIds on the basis of timeperiod, area NIds and sourceNIds
        /// </summary>
        /// <param name="timePeriodNIds"></param>
        /// <param name="areaNIds"></param>
        /// <param name="sourceNIds"></param>
        /// <returns></returns>
        public string GetAutoSelectIndicatorByTimePeriodAreaSource(string timePeriodNIds, string areaNIds, string sourceNIds)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();
            String sWhere = string.Empty;

            sbQuery.Append("SELECT DISTINCT D." + DIColumns.Data.IndicatorNId);
            sbQuery.Append(" FROM " + TablesName.Data + " D");

            if (!string.IsNullOrEmpty(timePeriodNIds) || !string.IsNullOrEmpty(areaNIds) )
            {
                sbQuery.Append(" WHERE ");
            }

            if (areaNIds.Length > 0)
            {
                if (sWhere.Length > 0)
                {
                    sWhere += " AND ";
                }
                sWhere += " D." + DIColumns.Data.AreaNId + " IN (" + areaNIds + ") ";
            }
            if (timePeriodNIds.Length > 0)
            {
                if (sWhere.Length > 0)
                {
                    sWhere += " AND ";
                }
                sWhere += " D." + DIColumns.Data.TimePeriodNId + " IN (" + timePeriodNIds + ")";
            }
           
            if (sourceNIds.Length > 0)
            {
                if (sWhere.Length > 0)
                {
                    sWhere += " AND ";
                }
                sWhere += " D." + DIColumns.Data.SourceNId + " IN (" + sourceNIds + ")";
            }
            sbQuery.Append(sWhere);

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get IUS NIds on the basis of timeperiod, area NIds and sourceNIds
        /// </summary>
        /// <param name="timePeriodNIds"></param>
        /// <param name="areaNIds"></param>
        /// <param name="iusNIDs"></param>
        /// <param name="sourceNIds"></param>
        /// <returns></returns>
        public string GetAutoSelectByTimePeriodAreaSource(string timePeriodNIds, string areaNIds, string iusNIDs,string sourceNIds)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();
            String sWhere = string.Empty;

            sbQuery.Append("SELECT DISTINCT D." + DIColumns.Data.IUSNId);
            sbQuery.Append(" FROM " + TablesName.Data + " D");

            if (!string.IsNullOrEmpty(timePeriodNIds) || !string.IsNullOrEmpty(areaNIds) || !string.IsNullOrEmpty(iusNIDs) || !string.IsNullOrEmpty(sourceNIds))
            {
                sbQuery.Append(" WHERE ");
            }

            if (areaNIds.Length > 0)
            {
                if (sWhere.Length > 0)
                {
                    sWhere += " AND ";
                }
                sWhere += " D." + DIColumns.Data.AreaNId + " IN (" + areaNIds + ") ";
            }
            if (timePeriodNIds.Length > 0)
            {
                if (sWhere.Length > 0)
                {
                    sWhere += " AND ";
                }
                sWhere += " D." + DIColumns.Data.TimePeriodNId + " IN (" + timePeriodNIds + ")";
            }
            if (iusNIDs.Length > 0)
            {
                if (sWhere.Length > 0)
                {
                    sWhere += " AND ";
                }
                sWhere += " D." + DIColumns.Data.IUSNId + " IN (" + iusNIDs + ")";
            }
            if (sourceNIds.Length > 0)
            {
                if (sWhere.Length > 0)
                {
                    sWhere += " AND ";
                }
                sWhere += " D." + DIColumns.Data.SourceNId + " IN (" + sourceNIds + ")";
            }
            sbQuery.Append(sWhere);

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get IUSNId on the basis of IC Types and ICNIds
        /// </summary>
        /// <param name="ICType"></param>
        /// <param name="ICNId"></param>
        /// <returns></returns>
        public string GetIUSNIdFromICType(ICType ICType, string ICNId)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId);
            sbQuery.Append(" FROM " + TablesName.IndicatorClassificationsIUS + " ICIUS," + TablesName.IndicatorClassifications + " IC");
            sbQuery.Append(" WHERE IC." + DIColumns.IndicatorClassificationsIUS.ICNId + " = ICIUS." + DIColumns.IndicatorClassifications.ICNId);
            if (string.IsNullOrEmpty(ICNId))
            {
                // When no IC element is selected - Get data on the basis of the IC Type only
                sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICType + " = " + DIQueries.ICTypeText[ICType]);
            }
            else
            {
                // When some IC Element is selected - No Need for IC Table - Get data from ICIUS table only
                sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICNId + " IN (" + ICNId + ")");
            }
            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get the IUS on the basis of showIUS
        /// </summary>
        /// <param name="iusNId"></param>
        /// <param name="showIUS"></param>
        /// <returns></returns>
        public string GetIndicators(string iusNId, bool showIUS)
        {

            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DISTINCT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ",I." + DIColumns.Indicator.IndicatorNId + ",I." + DIColumns.Indicator.IndicatorName + ",I." + DIColumns.Indicator.IndicatorGlobal + ",I." + DIColumns.Indicator.IndicatorGId);

            if (showIUS)
            {
                sbQuery.Append(",U." + DIColumns.Unit.UnitNId + ",U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitGlobal);
                sbQuery.Append(",SGV." + DIColumns.SubgroupVals.SubgroupValNId + ",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",SGV." + DIColumns.SubgroupVals.SubgroupValGlobal);
            }

            sbQuery.Append(" FROM " + TablesName.IndicatorUnitSubgroup + " IUS," + TablesName.Indicator + " I");

            if (showIUS)
            {
                sbQuery.Append("," + TablesName.Unit + " U," + TablesName.SubgroupVals + " SGV");
            }

            sbQuery.Append(" WHERE I." + DIColumns.Indicator.IndicatorNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId);

            if (showIUS)
            {
                sbQuery.Append(" AND U." + DIColumns.Unit.UnitNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId);
                sbQuery.Append(" AND SGV." + DIColumns.SubgroupVals.SubgroupValNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId);
            }

            if (string.IsNullOrEmpty(iusNId))
            {
                iusNId = "-1";
            }
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " IN (" + iusNId + ")");

            sbQuery.Append(" ORDER BY I." + DIColumns.Indicator.IndicatorName);
            RetVal = sbQuery.ToString();
            return RetVal;
        }



        /// <summary>
        /// AutoSelect Indiactor based on TimePeriod and Area selection
        /// </summary>
        /// <param name="timePeriodNIds">commma delimited Timeperiod NIds which may be blank</param>
        /// <param name="areaNIds">commma delimited Area NIds which may be blank</param>
        /// <param name="ICType">Indiactor Classification Type</param>
        /// <param name="fieldSelection">NId, Light or Heavy</param>
        /// <returns>Sql query string</returns>
        public string GetAutoSelectByTimePeriodArea(string timePeriodNIds, string areaNIds, ICType ICType, FieldSelection fieldSelection)
        {
            string RetVal = String.Empty;
            // Get basic Sql clause from overloaded function
            RetVal = GetAutoSelectByTimePeriodArea(timePeriodNIds, areaNIds, -1, fieldSelection);

            // Append additional clause for ICType
            RetVal += " AND IC." + DIColumns.IndicatorClassifications.ICType + " = " + DIQueries.ICTypeText[ICType];

            return RetVal;
        }


        /// <summary>
        /// Get IndicatorNId / IUSNId based on advance free text search for Indicator, Unit and SubgroupVal
        /// </summary>
        /// <param name="indicators">String array of indicator names which may be null</param>
        /// <param name="units">String array of unit names which may be null</param>
        /// <param name="subgroupVals">String array of subgroupvals which may be null</param>
        /// <param name="getIUSNId">Identifies whether IUSNId or IndicatorNId is to be returned</param>
        /// <returns>Sql query string</returns>
        /// <remarks>All string array items must be already handled for quotes and wildcard character</remarks>
        public string GetIndicatorNId_IUSNIdForFreeTextSearch(string[] indicators, string[] units, string[] subgroupVals, bool getIUSNId)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            // SELECT Clause
            if (getIUSNId == true)
            {
                sbQuery.Append("SELECT DISTINCT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId);
            }
            else
            {
                sbQuery.Append("SELECT DISTINCT IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId);
            }

            // FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS");
            if (indicators != null)
            {
                sbQuery.Append("," + this.TablesName.Indicator + " AS I");
            }

            if (units != null)
            {
                sbQuery.Append("," + this.TablesName.Unit + " AS U");
            }

            if (subgroupVals != null)
            {
                sbQuery.Append("," + this.TablesName.SubgroupVals + " AS SGV");
            }


            // WHERE Clause
            sbQuery.Append(" WHERE 1=1 ");

            if (indicators == null && units == null && subgroupVals == null)
            {

            }
            else
            {

                // Set like clause for Indicator
                if (indicators != null)
                {
                    sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId);
                    for (int i = 0; i <= indicators.Length - 1; i++)
                    {
                        if (i == 0)
                        {
                            sbQuery.Append(" AND (");
                        }
                        else
                        {
                            sbQuery.Append(" OR ");
                        }
                        //start of change on 29-01-09: -- Revert the changes of 18-03-08 for fixing bug id : 3965, In Advance search box , type "infant mortality" in double quotes.Hit enter key, it will show "No Records Found"
                        // indicators[i] is eligible for like search Ex. LIKE %infant mortality%
                        // change on 18-03-08. If indicators[i] is in quote use  equal to (=) in query instead of like
                        if (indicators[i].ToString().StartsWith("'") && indicators[i].ToString().EndsWith("'"))
                        {
                            //sbQuery.Append(" I." + DIColumns.Indicator.IndicatorName + " LIKE " + indicators[i]);
                            indicators[i] = indicators[i].Substring(1,indicators[i].Length-2);
                        }
                        //else
                        //{
                        //    sbQuery.Append(" I." + DIColumns.Indicator.IndicatorName + " LIKE '%" + indicators[i] + "%'");
                        //}
                        sbQuery.Append(" I." + DIColumns.Indicator.IndicatorName + " LIKE '%" + indicators[i] + "%'");
                        // end of change on 18-03-08.
                        //End of change on 29-01-09
                    }
                    sbQuery.Append(")");
                }

                // Set like clause for Unit
                if (units != null)
                {
                    sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId);
                    for (int i = 0; i <= units.Length - 1; i++)
                    {
                        if (i == 0)
                        {
                            sbQuery.Append(" AND (");
                        }
                        else
                        {
                            sbQuery.Append(" OR ");
                        }
                        //start of change on 29-01-09: -- Revert the changes of 18-03-08 for fixing bug id : 3965, In Advance search box , type "infant mortality" in double quotes.Hit enter key, it will show "No Records Found"
                        // change on 18-03-08.If unit[i] is in quote use  equal to (=) in query instead of like
                        if (units[i].ToString().StartsWith("'") && units[i].ToString().EndsWith("'"))
                        {
                            units[i] = units[i].Substring(1,units[i].Length-2);
                        }
                        //else
                        //{
                        //    sbQuery.Append(" U." + DIColumns.Unit.UnitName + " LIKE '%" + units[i] + "%'");
                        //}
                        sbQuery.Append(" U." + DIColumns.Unit.UnitName + " LIKE '%" + units[i] + "%'");
                        // change on 18-03-08.
                        //End of change on 29-01-09

                    }
                    sbQuery.Append(")");

                }

                // Set like clause for SubgroupVal
                if (subgroupVals != null)
                {
                    sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = SGV." + DIColumns.SubgroupVals.SubgroupValNId);
                    for (int i = 0; i <= subgroupVals.Length - 1; i++)
                    {
                        if (i == 0)
                        {
                            sbQuery.Append(" AND (");
                        }
                        else
                        {
                            sbQuery.Append(" OR ");
                        }
                        //start of change on 29-01-09: -- Revert the changes of 18-03-08 for fixing bug id : 3965, In Advance search box , type "infant mortality" in double quotes.Hit enter key, it will show "No Records Found"
                        // change on 18-03-08.If subgroupVals[i] is in quote use  equal to (=) in query instead of like
                        if (subgroupVals[i].ToString().StartsWith("'") && subgroupVals[i].ToString().EndsWith("'"))
                        {
                           // sbQuery.Append(" SGV." + DIColumns.SubgroupVals.SubgroupVal + " LIKE " + subgroupVals[i]);
                            subgroupVals[i] = subgroupVals[i].Substring(1,subgroupVals[i].Length-2) ;
                        }
                        //else
                        //{
                        //    sbQuery.Append(" SGV." + DIColumns.SubgroupVals.SubgroupVal + " LIKE '%" + subgroupVals[i] + "%'");
                        //}
                        sbQuery.Append( " SGV." + DIColumns.SubgroupVals.SubgroupVal + " LIKE '%" + subgroupVals[i] + "%'");
                        // end of change on 18-03-08.If subgroupVals[i] is in quote use  equal to (=) in query instead of like
                        //end of change on 29-01-09
                    }
                    sbQuery.Append(")");
                }
            }

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Returns Indicator, unit and subgroup informations for a particular ICType.
        /// </summary>
        /// <param name="ICNId">IC NId</param>
        /// <param name="classificationType">Indicator classification type</param>
        /// <param name="showIUS">True/False. True to get complete IUS informations and false to get indicator information onlye</param>
        /// <returns></returns>
        public string Indicator_GetAvailable(int ICNId, ICType classificationType, bool showIUS)
        {
            string RetVal = string.Empty;

            if (showIUS)
            {
                RetVal = "SELECT DISTINCT I." + DIColumns.Indicator.IndicatorNId + ", I." + DIColumns.Indicator.IndicatorName + ", I." + DIColumns.Indicator.IndicatorGlobal + ", IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ", U." + DIColumns.Unit.UnitNId + ", U." + DIColumns.Unit.UnitName + ", U." + DIColumns.Unit.UnitGlobal + ", SGV." + DIColumns.SubgroupVals.SubgroupValNId + ", SGV." + DIColumns.SubgroupVals.SubgroupVal + ",SGV." + DIColumns.SubgroupVals.SubgroupValGlobal + " FROM " + this.TablesName.SubgroupVals + " SGV," +
                    this.TablesName.Unit + " U," +
                    this.TablesName.Indicator + " I," +
                    this.TablesName.IndicatorUnitSubgroup + " IUS, " +
                    this.TablesName.IndicatorClassificationsIUS + " CIUS," +
                    this.TablesName.IndicatorClassifications + " IC " + "WHERE I." + DIColumns.Indicator.IndicatorNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId +
                    " And IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + "= CIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId +
                    " AND " + "CIUS." + DIColumns.IndicatorClassifications.ICNId + " = IC." + DIColumns.IndicatorClassifications.ICNId +
                    " AND " + "IUS." + DIColumns.Unit.UnitNId + "= U." + DIColumns.Unit.UnitNId +
                      " AND IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + "=SGV." + DIColumns.SubgroupVals.SubgroupValNId;
            }
            else
            {
                RetVal = "SELECT DISTINCT I." + DIColumns.Indicator.IndicatorNId + ", I." + DIColumns.Indicator.IndicatorName + ", I." + DIColumns.Indicator.IndicatorGlobal +
                    " FROM " + this.TablesName.Indicator + " I," +
                    this.TablesName.IndicatorUnitSubgroup + " IUS, " +
                    this.TablesName.IndicatorClassificationsIUS + " CIUS," +
                    this.TablesName.IndicatorClassifications + " IC " + "WHERE I." + DIColumns.Indicator.IndicatorNId + "= IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId +
                    " And IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + "= CIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId +
                    " AND " + "CIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + "= IC." + DIColumns.IndicatorClassifications.ICNId;
            }


            RetVal += " AND IC." +  DIColumns.IndicatorClassifications.ICType + " = " +  DIQueries.ICTypeText[classificationType];

            if (ICNId > 0)
            {
                RetVal += " AND IC." + DIColumns.IndicatorClassifications.ICNId + " = " + ICNId;
            }

            if (showIUS)
            {
                RetVal += " ORDER BY I." + DIColumns.Indicator.IndicatorName + ",U." + DIColumns.Unit.UnitName + ",SGV." + DIColumns.SubgroupVals.SubgroupVal;
            }
            else
            {
                RetVal += " ORDER BY I." + DIColumns.Indicator.IndicatorName;
            }

            return RetVal;
        }

        /// <summary>
        /// Returns Indicator, unit and subgroup informations for a particular ICType.
        /// </summary>
        /// <param name="ICNId">IC NId</param>
        /// <param name="classificationType">Indicator classification type</param>
        /// <param name="showIUS">True/False. True to get complete IUS informations and false to get indicator information onlye</param>
        /// <returns></returns>
        public string IndicatorUnit_GetAvailable(int ICNId, ICType classificationType, bool showIUS)
        {
            string RetVal = string.Empty;

            if (showIUS)
            {
                RetVal = "SELECT DISTINCT I." + DIColumns.Indicator.IndicatorNId + ", I." + DIColumns.Indicator.IndicatorName + ", I." + DIColumns.Indicator.IndicatorGlobal + ", IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ", U." + DIColumns.Unit.UnitNId + ", U." + DIColumns.Unit.UnitName + ", U." + DIColumns.Unit.UnitGlobal + ", SGV." + DIColumns.SubgroupVals.SubgroupValNId + ", SGV." + DIColumns.SubgroupVals.SubgroupVal + ",SGV." + DIColumns.SubgroupVals.SubgroupValGlobal + " FROM " + this.TablesName.SubgroupVals + " SGV," +
                    this.TablesName.Unit + " U," +
                    this.TablesName.Indicator + " I," +
                    this.TablesName.IndicatorUnitSubgroup + " IUS, " +
                    this.TablesName.IndicatorClassificationsIUS + " CIUS," +
                    this.TablesName.IndicatorClassifications + " IC " + "WHERE I." + DIColumns.Indicator.IndicatorNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId +
                    " And IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + "= CIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId +
                    " AND " + "CIUS." + DIColumns.IndicatorClassifications.ICNId + " = IC." + DIColumns.IndicatorClassifications.ICNId +
                    " AND " + "IUS." + DIColumns.Unit.UnitNId + "= U." + DIColumns.Unit.UnitNId +
                      " AND IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + "=SGV." + DIColumns.SubgroupVals.SubgroupValNId;
            }
            else
            {
                RetVal = "SELECT DISTINCT I." + DIColumns.Indicator.IndicatorNId + ", I." + DIColumns.Indicator.IndicatorName + ", I." + DIColumns.Indicator.IndicatorGlobal + ", U." + DIColumns.Unit.UnitNId + ", U." + DIColumns.Unit.UnitName + ", U." + DIColumns.Unit.UnitGlobal +
                " FROM " + this.TablesName.Indicator + " I," +
                this.TablesName.IndicatorUnitSubgroup + " IUS, " +
                this.TablesName.IndicatorClassificationsIUS + " CIUS," +
                this.TablesName.Unit + " U," +
                this.TablesName.IndicatorClassifications + " IC " + "WHERE I." + DIColumns.Indicator.IndicatorNId + "= IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId +
                " And IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + "= CIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId +
                " AND CIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + "= IC." + DIColumns.IndicatorClassifications.ICNId +
                " AND IUS." + DIColumns.Unit.UnitNId + "= U." + DIColumns.Unit.UnitNId;
            }


            RetVal += " AND IC." + DIColumns.IndicatorClassifications.ICType + " = " + DIQueries.ICTypeText[classificationType];

            if (ICNId > 0)
            {
                RetVal += " AND IC." + DIColumns.IndicatorClassifications.ICNId + " = " + ICNId;
            }

            if (showIUS)
            {
                RetVal += " ORDER BY I." + DIColumns.Indicator.IndicatorName + ",U." + DIColumns.Unit.UnitName + ",SGV." + DIColumns.SubgroupVals.SubgroupVal;
            }
            else
            {
                RetVal += " ORDER BY I." + DIColumns.Indicator.IndicatorName;
            }

            return RetVal;
        }

        /// <summary>
        /// Get  NId, Name, GID, Global from Indicator against which data is present.
        /// </summary>
        /// <param name="fieldSelection">Use heavy for all fields or use light to exclude IndicatorInfo field</param>
        /// <returns>Sql query string</returns>
        public string GetIndicatorWithData(string IUSNIDs, FieldSelection fieldSelection)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();


            //  SELECT Clause
            sbQuery.Append(" SELECT DISTINCT IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId );

            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorName + ",I." + DIColumns.Indicator.IndicatorGId + ",I." + DIColumns.Indicator.IndicatorGlobal);                
            }
            if (fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorInfo);
            }

            //  FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS");

            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append("," + this.TablesName.Indicator + " AS I");
            }

            //  WHERE Clause
            sbQuery.Append(" WHERE 1=1 ");
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + "= I." + DIColumns.Indicator.IndicatorNId + "   and ");
            }

            if (string.IsNullOrEmpty(IUSNIDs))
            {
                IUSNIDs = "0";
            }

            sbQuery.Append(" " + DIColumns.Indicator_Unit_Subgroup.IUSNId + " in( " + IUSNIDs + " )");

            //  ORDER BY Clause
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(" ORDER BY I." + DIColumns.Indicator.IndicatorName);
            }

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Returns query to get distinct indicators by IUSNIds
        /// </summary>
        /// <param name="IUSNIds">Comma separeated IUSNIDs</param>
        /// <returns></returns>
        public string GetDistinctIndicatorsByIUSNIds(string IUSNIds)
        {
            string RetVal = string.Empty;

            RetVal = "SELECT DISTINCT  "+ DIColumns.Indicator_Unit_Subgroup.IndicatorNId +" FROM " + this.TablesName.IndicatorUnitSubgroup + " WHERE " + DIColumns.Indicator_Unit_Subgroup.IUSNId + " IN(" + IUSNIds  +" )";

            return RetVal;
        }

        /// <summary>
        /// Get Duplicate Indicator By Name and GId
        /// </summary>
        /// <returns></returns>
        public string GetDuplicateIndicators()
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT " + DIColumns.Indicator.IndicatorName + "," + DIColumns.Indicator.IndicatorGId + ", count(*) as [Count] FROM " + this.TablesName.Indicator);

            sbQuery.Append(" GROUP BY " + DIColumns.Indicator.IndicatorName + "," + DIColumns.Indicator.IndicatorGId);

            sbQuery.Append(" HAVING COUNT(*) >1 ");

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get Duplicate Indicator By GIds
        /// </summary>
        /// <returns></returns>
        public string GetDuplicateIndicatorByGids()
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT " + DIColumns.Indicator.IndicatorNId + "," + DIColumns.Indicator.IndicatorName + "," + DIColumns.Indicator.IndicatorGId + " FROM " + this.TablesName.Indicator + " WHERE " + DIColumns.Indicator.IndicatorGId + " IN(");
            
            sbQuery.Append("SELECT " + DIColumns.Indicator.IndicatorGId + " FROM " );
            
            sbQuery.Append(this.TablesName.Indicator + " GROUP BY " + DIColumns.Indicator.IndicatorGId);
            
            sbQuery.Append(" HAVING COUNT(*) >1 )");

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get Duplicate Indicator By Name
        /// </summary>
        /// <returns></returns>
        public string GetDuplicateIndicatorByNames()
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT " + DIColumns.Indicator.IndicatorNId + "," + DIColumns.Indicator.IndicatorName + "," + DIColumns.Indicator.IndicatorGId + " FROM " + this.TablesName.Indicator + " WHERE " + DIColumns.Indicator.IndicatorName + " IN( ");

            sbQuery.Append("SELECT " + DIColumns.Indicator.IndicatorName + " FROM ");

            sbQuery.Append(this.TablesName.Indicator + " GROUP BY " + DIColumns.Indicator.IndicatorName);

            sbQuery.Append(" HAVING COUNT(*) >1 )");

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get Nid and Name where two language table have different Gid with same Nid
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="langCode"></param>
        /// <returns></returns>
        public string GetUnmatchedGidForLanguage(string dataPrefix,string langCode)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();
            
            DITables table=new DITables(dataPrefix,langCode);

            sbQuery.Append("SELECT I." + DIColumns.Indicator.IndicatorNId + " AS " + DIColumns.Indicator.IndicatorNId + ",I." + DIColumns.Indicator.IndicatorName + " AS " + DIColumns.Indicator.IndicatorName + ", I." + DIColumns.Indicator.IndicatorGId + " AS " + DIColumns.Indicator.IndicatorGId );
            sbQuery.Append(", I1." + DIColumns.Indicator.IndicatorNId + ",I1." + DIColumns.Indicator.IndicatorName + ",I1." + DIColumns.Indicator.IndicatorGId + " FROM " + this.TablesName.Indicator + " I," + table.Indicator + " I1 ");

            sbQuery.Append(" WHERE ");
            sbQuery.Append(" I." + DIColumns.Indicator.IndicatorNId + "= I1." + DIColumns.Indicator.IndicatorNId);
            sbQuery.Append(" AND I." + DIColumns.Indicator.IndicatorGId + "<> I1." + DIColumns.Indicator.IndicatorGId);
           
            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get  NId, Name, GID, Global from Indicator table based on Indicator Classification Type
        /// </summary>
        /// <param name="ICType">Indicator Classification Type with which associated indicator are to be retrieved</param>
        /// <param name="ICNId">ICNId which may be -1 if only ICType is to be considered</param>
        /// <param name="fieldSelection">Use heavy for all fields or use light to exclude IndicatorInfo field</param>
        /// <returns>Sql query string</returns>
        public string GetIndicatorUnitByIC(ICType ICType, string ICNId, FieldSelection fieldSelection)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            //   SELECT clause
            sbQuery.Append("SELECT DISTINCT I." + DIColumns.Indicator.IndicatorNId + ",U." + DIColumns.Unit.UnitNId);    //FieldSelection.NId
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorName + ",I." + DIColumns.Indicator.IndicatorGId + ",I." + DIColumns.Indicator.IndicatorGlobal);
                sbQuery.Append(",U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitGId + ",U." + DIColumns.Unit.UnitGlobal);
            }
            if (fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorInfo);
            }

            //   FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.Indicator + " AS I");
            sbQuery.Append("," + this.TablesName.Unit + " AS U");
            sbQuery.Append("," + this.TablesName.IndicatorUnitSubgroup + " AS IUS");
            sbQuery.Append("," + this.TablesName.IndicatorClassificationsIUS + " AS ICIUS");
            sbQuery.Append("," + this.TablesName.IndicatorClassifications + " AS IC");

            //   WHERE Clause
            sbQuery.Append(" WHERE I." + DIColumns.Indicator.IndicatorNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId);
            sbQuery.Append(" AND U." + DIColumns.Unit.UnitNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId);
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId);
            sbQuery.Append(" AND ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + " = IC." + DIColumns.IndicatorClassifications.ICNId);
            sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICType + " = " + DIQueries.ICTypeText[ICType]);
            if (!string.IsNullOrEmpty(ICNId) && ICNId != "-1")
            {
                sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICNId + " IN (" + ICNId + ")");
            }


            //   ORDER BY Clause
            sbQuery.Append(" ORDER BY " + DIColumns.Indicator.IndicatorName);

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// AutoSelect Indiactor, Unit based on TimePeriod, Area  and source selection
        /// </summary>
        /// <param name="timePeriodNIds">commma delimited Timeperiod NIds which may be blank</param>
        /// <param name="areaNIds">commma delimited Area NIds which may be blank</param>
        /// <param name="sourceNids">Source Nids which may be blank </param>
        /// <param name="fieldSelection">NId, Light or Heavy</param>
        /// <returns>Sql query string</returns>
        public string GetAutoSelectIndicatorUnitByTimePeriodAreaSource(string timePeriodNIds, string areaNIds, string sourceNids, FieldSelection fieldSelection)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            // Set SELECT clause based on fieldSelection type
            sbQuery.Append("SELECT DISTINCT I." + DIColumns.Indicator.IndicatorNId);    //FieldSelection.NId
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(", I." + DIColumns.Indicator.IndicatorName + ", I." + DIColumns.Indicator.IndicatorGId + ", I." + DIColumns.Indicator.IndicatorGlobal);
                sbQuery.Append(",U." + DIColumns.Unit.UnitNId + ",U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitGId + ",U." + DIColumns.Unit.UnitGlobal);
            }
            if (fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(", I." + DIColumns.Indicator.IndicatorInfo);
            }

            // Set FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.Data + " AS D," + this.TablesName.IndicatorUnitSubgroup + " AS IUS," + this.TablesName.Indicator + " AS I," + this.TablesName.Unit + " AS U");


            // Set WHERE clause 
            sbQuery.Append(" WHERE I." + DIColumns.Indicator.IndicatorNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = D." + DIColumns.Indicator_Unit_Subgroup.IUSNId);
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId);

            if (timePeriodNIds.Length > 0)
            {
                sbQuery.Append("  AND D." + DIColumns.Timeperiods.TimePeriodNId + " IN (" + timePeriodNIds + ")");
            }
            if (areaNIds.Length > 0)
            {
                sbQuery.Append(" AND D." + DIColumns.Area.AreaNId + " IN (" + areaNIds + ")");
            }


            if (!string.IsNullOrEmpty(sourceNids))
            {
                sbQuery.Append(" AND D." + DIColumns.Data.SourceNId + " IN( " + sourceNids + ") ");
            }

            sbQuery.Append(" ORDER BY " + DIColumns.Indicator.IndicatorName);
            RetVal = sbQuery.ToString();
            return RetVal;
        }

        public string GetIndicatorByICNIUSOrder()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT  IUS." + DIColumns.Indicator.IndicatorNId + " FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS INNER JOIN (" + this.TablesName.IndicatorClassifications + " AS IC INNER JOIN " + this.TablesName.IndicatorClassificationsIUS + " AS ICIUS ON IC." + DIColumns.IndicatorClassifications.ICNId + " = ICIUS." + DIColumns.IndicatorClassifications.ICNId + ") ON IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + "= ICIUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " order by IC."+DIColumns.IndicatorClassifications.ICOrder+", ICIUS."+DIColumns.IndicatorClassificationsIUS.ICIUSOrder +" ";

            return RetVal;
        }

        /// <summary>
        /// Get the IUS on the basis of showIUS
        /// </summary>
        /// <param name="iusNId"></param>
        /// <param name="showIUS"></param>
        /// <returns></returns>
        public string GetDistinctIndicatorUnit(string iusNId, bool showIUS)
        {

            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DISTINCT I." + DIColumns.Indicator.IndicatorNId + ",I." + DIColumns.Indicator.IndicatorName + ",I." + DIColumns.Indicator.IndicatorGlobal + ",I." + DIColumns.Indicator.IndicatorGId);
            sbQuery.Append(",U." + DIColumns.Unit.UnitNId + ",U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitGlobal);


            if (showIUS)
            {
                sbQuery.Append(",SGV." + DIColumns.SubgroupVals.SubgroupValNId + ",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",SGV." + DIColumns.SubgroupVals.SubgroupValGlobal);
            }

            sbQuery.Append(" FROM " + TablesName.IndicatorUnitSubgroup + " IUS," + TablesName.Indicator + " I," + TablesName.Unit + " U");

            if (showIUS)
            {
                sbQuery.Append("," + TablesName.SubgroupVals + " SGV");
            }

            sbQuery.Append(" WHERE I." + DIColumns.Indicator.IndicatorNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId);
            sbQuery.Append(" AND U." + DIColumns.Unit.UnitNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId);

            if (showIUS)
            {
                sbQuery.Append(" AND SGV." + DIColumns.SubgroupVals.SubgroupValNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId);
            }

            if (string.IsNullOrEmpty(iusNId))
            {
                iusNId = "-1";
            }
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " IN (" + iusNId + ")");

            sbQuery.Append(" ORDER BY I." + DIColumns.Indicator.IndicatorName);
            RetVal = sbQuery.ToString();
            return RetVal;
        }
        /// <summary>
        /// Returns a Query for finding all distinct Indicator Nid found in MetadataReport table.
        /// </summary>
        /// <returns></returns>
        public string GetDistinctIndicatorfromMetadataReport()
        {
            string RetVal = String.Empty;
            StringBuilder SqlQuery = new StringBuilder();
            SqlQuery.Append("SELECT " + DIColumns.Indicator.IndicatorNId);
            SqlQuery.Append(" FROM " + TablesName.Indicator);
            SqlQuery.Append(" WHERE " + DIColumns.Indicator.IndicatorNId + " IN (");
            SqlQuery.Append("SELECT DISTINCT (");
            SqlQuery.Append(DIColumns.MetadataReport.TargetNid + ")");
            SqlQuery.Append(" FROM " + TablesName.MetadataReport +")" );
            RetVal = SqlQuery.ToString();
            return RetVal;
        
        }
        public string GetInValidSdmxCompliantIndicatorGid()
        {
            StringBuilder SqlQuery = new StringBuilder();
           
            SqlQuery.Append("SELECT " + DIColumns.Indicator.IndicatorGId + "," + DIColumns.Indicator.IndicatorName + "," + DIColumns.Indicator.IndicatorNId);
            SqlQuery.Append(" FROM " + TablesName.Indicator);
           

            return SqlQuery.ToString();
        
        }
        /// <summary>
        /// Getting Duplicate Records of Indicators By Nid
        /// </summary>
        /// <returns></returns>
        public string GetDuplicateIndicatorByNid()
        {
            StringBuilder SqlQuery = new StringBuilder();
            SqlQuery.Append("SELECT " + DIColumns.Indicator.IndicatorNId + "," + DIColumns.Indicator.IndicatorName + "," + DIColumns.Indicator.IndicatorGId + " FROM " + this.TablesName.Indicator + " WHERE " + DIColumns.Indicator.IndicatorNId + " IN( ");

            SqlQuery.Append("SELECT " + DIColumns.Indicator.IndicatorNId + " FROM ");

            SqlQuery.Append(this.TablesName.Indicator + " GROUP BY " + DIColumns.Indicator.IndicatorNId);

            SqlQuery.Append(" HAVING COUNT(*) >1 )");


            return SqlQuery.ToString();

        }
        #endregion

        #endregion
    }



}
