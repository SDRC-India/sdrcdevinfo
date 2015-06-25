using System;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;

using DevInfo.Lib.DI_LibBAL.UI.UserPreference;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.Controls.DimensionsSelectionControlBAL
{
    public class DimensionsSelection
    {
        #region "-- Public --"

        #region "-- New / Dispose --"

        public DimensionsSelection(DIConnection dbConnection, DIQueries dbQueries, UserPreference userPreference)
        {
            this.DbConnection = dbConnection;
            this.DbQueries = dbQueries;
            this.UserPreference = userPreference;
        }

        #endregion

        #region "-- Properties --"

        private int _SubgroupValCount = -1;
        /// <summary>
        /// Get the SubgroupValCount
        /// </summary>
        public int SubgroupValCount
        {
            get 
            {
                return this._SubgroupValCount; 
            }
        }

        private int _SelectedSubgroupValCount = 0;
        /// <summary>
        /// Gets the SelectedSubgroupValCount
        /// </summary>
        public int SelectedSubgroupValCount
        {
            get 
            {
                return this._SelectedSubgroupValCount; 
            }
        }

        private string _TrimedSubgroupVal = string.Empty;
        /// <summary>
        /// Get the TrimedSubgroupVal
        /// </summary>
        public string TrimedSubgroupVal
        {
            get 
            { 
                return this._TrimedSubgroupVal; 
            }
        }

        private Dictionary<int, string> _SelectedSubgroupVal = new Dictionary<int, string>();
        /// <summary>
        /// Get the SelectedSubgroupVal
        /// </summary>
        public Dictionary<int, string> SelectedSubgroupVal
        {
            get 
            {
                return this._SelectedSubgroupVal; 
            }
        }
	
	

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Get the IUSNIds for the indicator and unit NId
        /// </summary>
        /// <param name="subgroupNIds"></param>
        /// <param name="exactMatch"></param>
        /// <param name="indicatorNId"></param>
        /// <param name="unitNId"></param>
        /// <returns></returns>
        public string GetIUSNIds(OrderedDictionary subgroupNIds, int indicatorNId, int unitNId)
        {
            string RetVal = string.Empty;
            try
            {
                DataView dvIUS;
                DataRow[] Rows = new DataRow[0];
                string SubgroupVal = string.Empty;
                string[] SelSubgroupVals = new string[subgroupNIds.Count];
                bool exactMatch = true;

                dtIUS = this.DbConnection.ExecuteDataTable(this.DbQueries.IUS.GetAutoSelectIUS(FilterFieldType.None, "", FieldSelection.Light));
                dvIUS = this.dtIUS.DefaultView;
                dvIUS.RowFilter = Indicator.IndicatorNId + " = " + indicatorNId + " AND " + Unit.UnitNId + " = " + unitNId;
                this.dtIUS = dvIUS.ToTable();
                this._SubgroupValCount = dtIUS.Rows.Count;

                this.SelectedSubgroups = subgroupNIds;

                this.SubgroupValBuilder = new DI6SubgroupValBuilder(this.DbConnection, this.DbQueries);                

                this.IntializeSubgroupVal(ref SelSubgroupVals, exactMatch);
                this.GetSubgroupValsFromNIds(SelSubgroupVals, 0, this.SelectedSubgroups.Count - 1, 0, exactMatch);

                if (exactMatch && this.SelSubgroupVals.Length > 0)
                {
                    this.SelSubgroupVals.Insert(0, SubgroupVals.SubgroupVal + " IN (");
                    this.SelSubgroupVals.Append(")");
                }

                if (this.SelSubgroupVals.Length > 0)
                {
                    Rows = dtIUS.Select(this.SelSubgroupVals.ToString());                    
                    this._SelectedSubgroupValCount = Rows.Length;

                    foreach (DataRow Row in Rows)
                    {
                        RetVal += "," + Row[Indicator_Unit_Subgroup.IUSNId].ToString();
                        this._TrimedSubgroupVal += "," + Row[SubgroupVals.SubgroupVal].ToString();
                    }
                }

                if (!string.IsNullOrEmpty(RetVal))
                {
                    RetVal = RetVal.Substring(1);
                    this._TrimedSubgroupVal = this._TrimedSubgroupVal.Substring(1);
                    this._TrimedSubgroupVal = this._TrimedSubgroupVal.Substring(0, SubgroupValLength - 3) + "...";
                }
                else 
                {
                    RetVal = "-1";
                }
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        /// <summary>
        /// Get all combinations of IUSNIds for the indicator and unit NId
        /// </summary>
        /// <param name="subgroupNIds"></param>
        /// <param name="indicatorNId"></param>
        /// <param name="unitNId"></param>
        /// <param name="showCombinations"></param>
        /// <returns></returns>
        public string GetIUSNIdCombinations(OrderedDictionary subgroupNIds, int indicatorNId, int unitNId, bool showCombinations, bool dataExists)
        {
            string RetVal = string.Empty;
            try
            {
                this._SelectedSubgroupVal = new Dictionary<int, string>();
                this._TrimedSubgroupVal = string.Empty;
                DataView dvIUS;
                DataTable IUSTable;
                DataTable SubgroupTable;
                IDataReader SubgroupValReader;
                DataRow[] Rows = new DataRow[0];
                string SubgroupVal = string.Empty;
                string[] SelSubgroupNIds = new string[0];
                string SelectedSubgroupNIds = string.Empty;
                string SubgroupValNIds = string.Empty;
                string TempSelectedSubgroupNIds=string.Empty;
                bool Found = false;

                this.SelectedSubgroups = subgroupNIds;

                if (!dataExists || (string.IsNullOrEmpty(this.UserPreference.UserSelection.AreaNIds) && string.IsNullOrEmpty(this.UserPreference.UserSelection.TimePeriodNIds) && string.IsNullOrEmpty(this.UserPreference.UserSelection.SourceNIds)))
                {
                    //-- Get the IUSNIDs
                    dtIUS = this.DbConnection.ExecuteDataTable(this.DbQueries.IUS.GetIUS(FilterFieldType.None, "", FieldSelection.Light));

                    dvIUS = this.dtIUS.DefaultView;
                    dvIUS.RowFilter = Indicator.IndicatorNId + " = " + indicatorNId + " AND " + Unit.UnitNId + " = " + unitNId;
                    this.dtIUS = dvIUS.ToTable();
                    this._SubgroupValCount = dtIUS.Rows.Count;
                }
                else
                {
                    dtIUS = this.DbConnection.ExecuteDataTable(this.DbQueries.IUS.GetIUS(indicatorNId.ToString(), unitNId.ToString(), this.UserPreference.UserSelection.AreaNIds, this.UserPreference.UserSelection.TimePeriodNIds, this.UserPreference.UserSelection.SourceNIds));
                    SubgroupTable = this.DbConnection.ExecuteDataTable(this.DbQueries.IUS.GetSubgroupValByIU(indicatorNId, unitNId));
                    this._SubgroupValCount = SubgroupTable.Rows.Count;
                }


                if (dataExists)
                {
                    dvIUS = this.dtIUS.DefaultView;
                    dvIUS.RowFilter = Indicator_Unit_Subgroup.DataExist + " = true";
                    this.dtIUS = dvIUS.ToTable();                    
                }                

                //-- Get the selected subgroupNIds
                for (int Index = 0; Index < subgroupNIds.Count; Index++)
                {
                    if(!string.IsNullOrEmpty(SelectedSubgroupNIds))
                    {
                        SelectedSubgroupNIds += ",";
                    }
                    SelectedSubgroupNIds += subgroupNIds[Index].ToString();
                }

                if (!string.IsNullOrEmpty(SelectedSubgroupNIds))
                {                    
                    if (showCombinations)
                    {
                        //-- Get the combinations of SubgroupVals of the selected subgroupNIds                    
                        SubgroupValReader = this.DbConnection.ExecuteReader(this.DbQueries.SubgroupValSubgroup.GetSubgroupValsSubgroup(string.Empty, SelectedSubgroupNIds, true));
                        while (SubgroupValReader.Read())
                        {
                            Found = false;
                            SelSubgroupNIds = DICommon.SplitString(SubgroupValReader[Indicator_Unit_Subgroup.SubgroupNids].ToString(), ",");
                            TempSelectedSubgroupNIds="," + SelectedSubgroupNIds + ",";
                            foreach (string SelSubgroupNId in SelSubgroupNIds)
                            {
                                if (TempSelectedSubgroupNIds.Contains("," + SelSubgroupNId + ","))
                                {
                                    Found = true;                                    
                                }
                                else 
                                {
                                    Found = false;
                                    break;
                                }
                            }

                            if (Found)
                            {
                                SubgroupValNIds += "," + SubgroupValReader[SubgroupVals.SubgroupValNId].ToString();
                            }                            
                        }
                        if (!string.IsNullOrEmpty(SubgroupValNIds))
                        {
                            SubgroupValNIds = SubgroupValNIds.Substring(1);
                        }
                        SubgroupValReader.Close();
                    }
                    else
                    {
                        IUSTable = this.DbConnection.ExecuteDataTable(this.DbQueries.IUS.GetIUSNIdByI_U_SubgroupNIds(indicatorNId.ToString(), unitNId.ToString(), SelectedSubgroupNIds));
                        //-- Get the subgroup val of the selected subgroupNIds only
                        SelSubgroupNIds = DICommon.SplitString(SelectedSubgroupNIds, ",");

                        foreach (string SelSubgroupNId in SelSubgroupNIds)
                        {
                            Rows = IUSTable.Select(Subgroup.SubgroupNId + " = " + SelSubgroupNId + " AND " + Indicator_Unit_Subgroup.DataExist + " = true");
                            if (Rows.Length > 0)
                            {
                                foreach (DataRow Row in Rows)
                                {
                                    if (DICommon.SplitString(Row[Indicator_Unit_Subgroup.SubgroupNids].ToString(), ",").Length == 1)
                                    {
                                        SubgroupValNIds += "," + Row[SubgroupVals.SubgroupValNId].ToString();
                                        break;
                                    }                                 
                                }

                            }
                        } 


                        if (!string.IsNullOrEmpty(SubgroupValNIds))
                        {
                            SubgroupValNIds = SubgroupValNIds.Substring(1);
                        }
                    }                    
                }

                //-- Get the subgroupVals
                if (!string.IsNullOrEmpty(SubgroupValNIds))
                {
                    Rows = dtIUS.Select(SubgroupVals.SubgroupValNId + " IN (" + SubgroupValNIds + ")");
                    this._SelectedSubgroupValCount = Rows.Length;

                    IUSTable = new DataTable();
                    IUSTable = this.dtIUS.Clone();
                    foreach (DataRow Row in Rows)
                    {
                        IUSTable.ImportRow(Row);
                    }

                    dvIUS = IUSTable.DefaultView;
                    dvIUS.Sort = SubgroupVals.SubgroupValOrder;
                    IUSTable = dvIUS.ToTable();

                    foreach (DataRow Row in IUSTable.Rows)
                    {
                        RetVal += "," + Row[Indicator_Unit_Subgroup.IUSNId].ToString();
                        this._TrimedSubgroupVal += "," + Row[SubgroupVals.SubgroupVal].ToString();
                        this._SelectedSubgroupVal.Add(Convert.ToInt32(Row[SubgroupVals.SubgroupValNId].ToString()), Row[SubgroupVals.SubgroupVal].ToString());
                    }
                }

                if (!string.IsNullOrEmpty(RetVal))
                {
                    RetVal = RetVal.Substring(1);
                    this._TrimedSubgroupVal = this._TrimedSubgroupVal.Substring(1);
                    if (this._TrimedSubgroupVal.Length > SubgroupValLength)
                    {
                        this._TrimedSubgroupVal = this._TrimedSubgroupVal.Substring(0, SubgroupValLength - 3) + "...";
                    }
                }
                else
                {
                    this._SelectedSubgroupValCount = 0;
                    RetVal = "-1";
                }
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        /// <summary>
        /// Get the IUSNIds from the subgroupValNIds
        /// </summary>
        /// <param name="indicatorNId"></param>
        /// <param name="unitNId"></param>
        /// <param name="subgroupValNIds"></param>
        /// <returns></returns>
        public string GetIUSNIdFromSubgroupValNIds(int indicatorNId, int unitNId, string subgroupValNIds)
        {
            string RetVal = string.Empty;
            DataTable IUSTable;
            try
            {
                this._TrimedSubgroupVal = string.Empty;
                IUSTable = this.DbConnection.ExecuteDataTable(this.DbQueries.IUS.GetIUSByI_U_S(indicatorNId.ToString(), unitNId.ToString(), subgroupValNIds));

                //-- Get the subgroupVals
                this._SelectedSubgroupValCount = IUSTable.Rows.Count;
                foreach (DataRow Row in IUSTable.Rows)
                {
                    RetVal += "," + Row[Indicator_Unit_Subgroup.IUSNId].ToString();
                    this._TrimedSubgroupVal += "," + Row[SubgroupVals.SubgroupVal].ToString();
                }

                if (!string.IsNullOrEmpty(RetVal))
                {
                    RetVal = RetVal.Substring(1);
                    this._TrimedSubgroupVal = this._TrimedSubgroupVal.Substring(1);
                    this._TrimedSubgroupVal = this._TrimedSubgroupVal.Substring(0, SubgroupValLength - 3) + "...";
                    this._SelectedSubgroupValCount = IUSTable.Rows.Count;
                }
                else
                {
                    this._TrimedSubgroupVal = string.Empty;
                    this._SelectedSubgroupValCount = 0;
                    RetVal = "-1";
                }
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        /// <summary>
        /// Get the subgroupNIds on the basis of indicator, unit and IUS NIds
        /// </summary>
        /// <param name="iusNIds"></param>
        /// <param name="indicatorNId"></param>
        /// <param name="unitNId"></param>
        /// <returns></returns>
        public string GetSubgroupNIds(string iusNIds, int indicatorNId, int unitNId)
        {
            string RetVal = string.Empty;
            try
            {
                IDataReader SubgroupReader;

                SubgroupReader = this.DbConnection.ExecuteReader(this.DbQueries.Subgroup.GetSubgroupForIndicatorUnit(indicatorNId, unitNId, iusNIds));
                while (SubgroupReader.Read())
                {
                    RetVal += "," + SubgroupReader[Subgroup.SubgroupNId].ToString();
                }
                SubgroupReader.Close();

                if (!string.IsNullOrEmpty(RetVal))
                {
                    RetVal = RetVal.Substring(1);
                }
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        #endregion

        #endregion

        #region "-- Private --"

         #region "-- Variables --"

        /// <summary>
        /// DAL connection object
        /// </summary>
        private DIConnection DbConnection;

        /// <summary>
        /// DAL query object
        /// </summary>
        private DIQueries DbQueries;

        private UserPreference UserPreference;

        private DataTable dtIUS;

        private OrderedDictionary SelectedSubgroups = new OrderedDictionary();

        private List<string> SelectedNIds = new List<string>();

        private DI6SubgroupValBuilder SubgroupValBuilder;

        private StringBuilder SelSubgroupVals = new StringBuilder();

        #endregion

        #region "-- Constant --"

        private const int SubgroupValLength = 60;

        #endregion

        #region "-- Methods --"

        private void GetSubgroupValsFromNIds(string[] sgRow, int startColIndex, int endRowIndex, int endColIndex, bool exactMatch)
        {
            string[] SubgroupNids = new string[0];
            int RowIndex = 0;

            if (this.SelectedSubgroups.Count > 1)
            {
                for (int SGTypeIndex = 0; SGTypeIndex < SelectedSubgroups.Count; SGTypeIndex++)
                {
                    //-- Increase the end column index so that it can reach to the end of the selected row.
                    SubgroupNids = DICommon.SplitString(this.SelectedSubgroups[endRowIndex].ToString(), ",");
                    if (SGTypeIndex + 1 < SelectedSubgroups.Count && endColIndex < SubgroupNids.Length - 1)
                    {
                        endColIndex += 1;
                        sgRow[endRowIndex] = endColIndex.ToString();
                        this.SelectedNIds[endRowIndex] = SubgroupNids[endColIndex];
                        this.GetCommaSepratedSubgroupNids(exactMatch);
                    }
                    else if (endColIndex == SubgroupNids.Length - 1)
                    {
                        RowIndex = this.GetEndColumnIndex(ref sgRow, exactMatch);
                        if (RowIndex == -1)
                        {
                            break;
                        }
                        else
                        {
                            endColIndex = 0;
                        }
                    }
                    this.GetSubgroupValsFromNIds(sgRow, startColIndex, endRowIndex, endColIndex, exactMatch);
                }
            }
            else
            {
                if (this.SelectedSubgroups.Count > 0)
                {
                    SubgroupNids = DICommon.SplitString(this.SelectedSubgroups[0].ToString(), ",");
                    for (int Index = 1; Index < SubgroupNids.Length; Index++)
                    {
                        this.SelectedNIds.Clear();
                        this.SelectedNIds.Add(SubgroupNids[Index]);
                        this.GetCommaSepratedSubgroupNids(exactMatch);
                    }
                }
            }
        }

        /// <summary>
        /// Get the row against which next subgroup nid is used to generate the subgroupVal.
        /// </summary>
        /// <param name="sgRow"></param>
        /// <param name="endRowIndex"></param>
        /// <param name="endColIndex"></param>
        /// <returns></returns>
        private int GetEndColumnIndex(ref string[] sgRow, bool exactMatch)
        {
            int RetVal = -1;
            try
            {
                string[] SubgroupNids = new string[0];
                for (int Index = this.SelectedSubgroups.Count - 2; Index >= 0; Index--)
                {
                    SubgroupNids = DICommon.SplitString(this.SelectedSubgroups[Index].ToString(), ",");

                    //-- Check if the dimension contains unused subgroupNIds
                    if (Convert.ToInt32(sgRow[Index]) < SubgroupNids.Length-1)
                    {
                        sgRow[Index] = Convert.ToString(Convert.ToInt32(sgRow[Index]) + 1);
                        this.SelectedNIds[Index] = SubgroupNids[Convert.ToInt32(sgRow[Index])];

                        RetVal = this.SelectedSubgroups.Count - 1;
                        for (int NIndex = Index+1; NIndex < this.SelectedSubgroups.Count; NIndex++)
                        {
                            sgRow[NIndex] = "0";

                            SubgroupNids = DICommon.SplitString(this.SelectedSubgroups[NIndex].ToString(), ",");
                            this.SelectedNIds[NIndex] = SubgroupNids[0];

                            this.GetCommaSepratedSubgroupNids(exactMatch);
                        }
                        break;
                    }
                }
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        private void IntializeSubgroupVal(ref string[] sgRow, bool exactMatch)
        {
            string[] SubgroupNids = new string[0];
            for (int Index = 0; Index < this.SelectedSubgroups.Count; Index++)
            {
                sgRow[Index] = "0";

                SubgroupNids = DICommon.SplitString(this.SelectedSubgroups[Index].ToString(), ",");
                this.SelectedNIds.Add(SubgroupNids[0]);
            }

            this.GetCommaSepratedSubgroupNids(exactMatch);
        }

        private void GetCommaSepratedSubgroupNids(bool exactMatch)
        {
            try
            {
                string SubgroupVal = string.Empty;

                foreach (string SgNid in this.SelectedNIds)
                {
                    SubgroupVal += "," + SgNid;
                }

                if (!string.IsNullOrEmpty(SubgroupVal))
                {
                    SubgroupVal = SubgroupVal.Substring(1);
                }

                if (exactMatch)
                {
                    if (!string.IsNullOrEmpty(SubgroupVal))
                    {
                        if (this.SelSubgroupVals.Length > 0)
                        {
                            this.SelSubgroupVals.Append(",");
                        }
                        this.SelSubgroupVals.Append("'" + SubgroupValBuilder.CreateSubgroupValTextBySubgroupNids(SubgroupVal) + "'");
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(SubgroupVal))
                    {
                        if (this.SelSubgroupVals.Length > 0)
                        {
                            this.SelSubgroupVals.Append(" OR ");
                        }
                        this.SelSubgroupVals.Append(SubgroupVals.SubgroupVal + " LIKE ('" + SubgroupValBuilder.CreateSubgroupValTextBySubgroupNids(SubgroupVal) + "%')");
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #endregion
    }
}
