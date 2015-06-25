using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;

namespace DevInfo.Lib.DI_LibBAL.Import.DataCapture
{
    internal class EIQuery
    {
        #region"--Private--".

        #region"--Variable--"

        private DIQueries DBQueries;
        private string DataPrefix = string.Empty;
        private string LanguageCode = string.Empty;

        #endregion

        #region"--Methods --"

        #endregion

        #endregion

        #region"--Internal--"

        #region " -- Enums -- "


        internal enum eQueryType      // Enum Query Type
        {
            Update = 1,
            Insert = 2
        }

        internal enum eIDType         // Enum Id Type
        {
            NIDs = 1,
            GIDs = 2,
            SubgroupTypeNIds = 3,
            NAMEs = 4,
            NID_NOT_IN = 5,
            SEARCH = 6,
            PARENT = 7,
            AREA_NID = 8,
            LAYER_NID = 9,
            FEATURE_NID = 10,
            AREA_ID = 11,
            PARENT_CHILD_NID = 12
        }

        internal enum eLightHeavy     // Enum Type
        {
            Lite = 1,
            Heavy = 2
        }

        #endregion


        #region "-- New/Dispose --"

        /// <summary>
        /// Returns instance of EIQuery.
        /// </summary>
        internal EIQuery()
        {
            //dont implements this method
        }

        /// <summary>
        /// Returns instance of EIQuery.
        /// </summary>
        /// <param name="dataPrefix">Data prefix like "UT_"</param>
        /// <param name="languageCode">Language code like "_en"</param>
        internal EIQuery(DIQueries dbQueries)
        {
            this.DBQueries = dbQueries;
            this.DataPrefix = this.DBQueries.DataPrefix;
            this.LanguageCode = this.DBQueries.LanguageCode;
        }

        #endregion

        #region"--Method--"

        internal string CreateTempDataTable()
        {
            string RetVal = string.Empty;

            RetVal = "Select *, '' as MIndName, '' as MUnitName, '' as MSubName,'' as " + Area.AreaName + ", " +
            " '' as " + Timeperiods.TimePeriod + ", '' as Source, 0 as _Updated , " +
                   " 0 as "+ Indicator.IndicatorNId +", 0 as "+Unit.UnitNId +", 0 as "+SubgroupVals.SubgroupValNId+"  Into " +
            " _Data  From " + this.DBQueries.TablesName.Data + " WHERE 1=2";

            return RetVal;
        }

        internal string AlterTempDataTable()
        {
            string RetVal = string.Empty;

            RetVal = "Alter table  _Data alter column MIndName Memo  ";

            return RetVal;
        }

        internal string CreateTempAreaTable()
        {
            string RetVal = string.Empty;

            RetVal = "Select *, '' as MNId, '' as MName, '' as MGid Into ";

            RetVal += "_Area From " + this.DBQueries.TablesName.Area;

            return RetVal;
        }

        internal string createTempBlankAreaTable()
        {
            string RetVal = string.Empty;
            RetVal = "Select *, 0 as MMap ,'' as MMapto, '' as DB_Src_NId, 0 as Mapped, '' as SrcDBName " +
                 " INTO ";

            RetVal += "_AreaUM From " + this.DBQueries.TablesName.Area + " Where 1=2";

            return RetVal;

        }


        #region "-- Drop --"

        internal string Droptable(string sTable_Name)
        {
            string RetVal = string.Empty;
            RetVal = "Drop Table ";
            switch (sTable_Name)
            {
                case Constants.TableNames.Area:
                    RetVal = RetVal + "_Area ";
                    break;
                case Constants.TableNames.IUS:
                    RetVal = RetVal + "_IUS ";
                    break;
                case Constants.TableNames.DATA:
                    RetVal = RetVal + "_Data ";
                    break;
                case Constants.TableNames.AREAUM:
                    RetVal = RetVal + "_AreaUM ";
                    break;
                case Constants.TableNames.IUSUM:
                    RetVal = RetVal + "_IUSUM ";
                    break;
                case Constants.TableNames.FN:
                    RetVal = RetVal + "_Footnote ";
                    break;
                case Constants.TableNames.Time:
                    RetVal = RetVal + "_TimePeriod ";
                    break;
                case Constants.TableNames.Source:
                    RetVal = RetVal + "_Source ";
                    break;
            }
            return RetVal;

        }

        #endregion

        internal string CheckIUSValuesExistsbyGid(string sIndicator_Gid, string sUnit_Gid,
    string sSubGroup_Gid)
        {
            string RetVal = string.Empty;

            RetVal = "SELECT IUS."+Indicator_Unit_Subgroup.IUSNId+" FROM ((" + this.DBQueries.TablesName.IndicatorUnitSubgroup + " AS IUS INNER JOIN " +
                        this.DBQueries.TablesName.Indicator + " AS I " +
                        " ON IUS."+Indicator_Unit_Subgroup.IndicatorNId+" = I."+Indicator.IndicatorNId+") INNER JOIN " +
                        this.DBQueries.TablesName.Unit + " AS U ON IUS."+Indicator_Unit_Subgroup.UnitNId+" = U."+Unit.UnitNId+")" +
                        " INNER JOIN " + this.DBQueries.TablesName.SubgroupVals +
                        " AS S ON IUS."+Indicator_Unit_Subgroup.SubgroupValNId+" = S."+SubgroupVals.SubgroupValNId +
                        " Where I."+Indicator.IndicatorGId+" = '" + sIndicator_Gid + "' and U."+Unit.UnitGId+" = '" +
                        sUnit_Gid + "' and S."+SubgroupVals.SubgroupValGId+" ='" + sSubGroup_Gid + "'";

            return RetVal;

        }

        internal string GetUnlinkedTargetCombo(string mTableName)
        {
            return GetUnlinkedTargetCombo(mTableName, 0);
        }

        internal string GetUnlinkedTargetCombo(string mTableName, int iValue)
        {
            string RetVal = string.Empty;

            RetVal = "Select ";

            switch (mTableName)
            {
                case "IND":
                    RetVal = RetVal + " Distinct "+Indicator.IndicatorName+", MMapto, "+Indicator.IndicatorGId+" From _IndicatorUM ";
                    break;
                case "UNIT":
                    RetVal = RetVal + " Distinct "+Unit.UnitName+", MMapto,"+Unit.UnitGId+" From _UnitUM ";
                    break;
                case "AREA":
                    RetVal = RetVal + " Distinct "+Area.AreaName+", MMapto,"+Area.AreaGId+","+Area.AreaID+","+Area.AreaLevel+" From _AreaUM ";
                    break;
                case "SUB":
                    RetVal = RetVal + " Distinct "+SubgroupVals.SubgroupVal+", MMapto,"+SubgroupVals.SubgroupValGId+" From _SubgroupUM ";
                    break;
                case "IUS":
                    RetVal = RetVal + Indicator.IndicatorName+" From _IndicatorUM ";
                    break;
            }

            RetVal = RetVal + " Where MMap = " + iValue;

            return RetVal;
        }

        internal string GetUnlinkedTarget(string sTableName)
        {
            string RetVal = string.Empty;

            RetVal = "Select *  From ";

            switch (sTableName)
            {
                case "IND":
                    RetVal = RetVal + " _Indicator Order By "+ Indicator.IndicatorName; //where MName = '' ;
                    break;
                case "UNIT":
                    RetVal = RetVal + " _Unit Order By "+ Unit.UnitName;
                    break;
                case "AREA":
                    RetVal = RetVal + " _Area Order By "+ Area.AreaName;
                    break;
                case "SUB":
                    RetVal = RetVal + " _Subgroup Order By "+SubgroupVals.SubgroupVal;
                    break;

            }
            return RetVal;
        }

        internal string LinkIndicator(string sTableName, string sName, string sTargetId)
        {
            return LinkIndicator(sTableName, sName, sTargetId, false);
        }

        internal string LinkIndicator(string sTableName, string sName, string sTargetId,
      bool bGid)
        {
            string RetVal = string.Empty;

            RetVal = "Update ";

            switch (sTableName)
            {
                case "IND":
                    RetVal = RetVal + "_Indicator set mName = '" + sName + "'"
    + " Where "+Indicator.IndicatorNId+" = " + sTargetId;
                    break;
                case "UNIT":
                    RetVal = RetVal + "_Unit set mName = '" + sName + "'" +
                      " Where "+Unit.UnitNId+" = " + sTargetId;
                    break;
                case "SUB":
                    RetVal = RetVal + "_Subgroup set mName = '" + sName + "'" +
                      " Where "+SubgroupVals.SubgroupValNId+" = " + sTargetId;
                    break;
                case "AREA":
                    RetVal = RetVal + "_area set mName = '" + sName + "'";
                    if (bGid)
                    {
                        RetVal = RetVal + " Where "+Area.AreaGId+" = '" + sTargetId + "'";
                    }
                    else
                    {
                        RetVal = RetVal + " Where "+Area.AreaNId+" = " + sTargetId;
                    }
                    break;
                case "IUS":
                    RetVal = RetVal + "_Ius set mName = '" + sName + "'"
   + " Where "+Indicator_Unit_Subgroup.IUSNId+" = " + sTargetId;
                    break;
            }

            return RetVal;
        }


        internal string UpdateStatus(string sTablename, string sName, string sID)
        {
            return UpdateStatus(sTablename, sName, sID, false);
        }

        internal string UpdateStatus(string sTablename, string sName, string sID,
      bool bGid)
        {
            string RetVal = string.Empty;

            RetVal = "Update  ";
            switch (sTablename)
            {
                case "IND":
                    RetVal = RetVal + "_IndicatorUM set MMap = 1,MMapto = '" + sID + "'";
                    if (bGid)
                    {
                        // According to Gid
                        RetVal = RetVal + " Where "+Indicator.IndicatorGId+" = '" + sName + "'";
                    }
                    else
                    {
                        // According to Name
                        RetVal = RetVal + " Where "+Indicator.IndicatorName+" = '" + sName + "'";
                    }
                    break;
                case "UNIT":
                    RetVal = RetVal + "_UnitUM set MMap = 1 ,MMapto = '" + sID + "'";
                    if (bGid)
                    {
                        // According to Gid
                        RetVal = RetVal + " Where "+Unit.UnitGId+" = '" + sName + "'";
                    }
                    else
                    {
                        // According to Name
                        RetVal = RetVal + " Where "+Unit.UnitName+" = '" + sName + "'";
                    }
                    break;
                case "AREA":
                    RetVal = RetVal + "_AreaUM set MMap = 1 , MMapto = '" + sID + "'"
                     + " Where "+Area.AreaID+" = '" + sName + "'";
                    break;
                case "SUB":
                    RetVal = RetVal + "_SubgroupUM set MMap = 1 ,MMapto = '" + sID + "'";
                    if (bGid)
                    {
                        // According to Gid
                        RetVal = RetVal + " Where "+SubgroupVals.SubgroupValGId+" = '" + sName + "'";
                    }
                    else
                    {
                        // According to Name
                        RetVal = RetVal + " Where "+SubgroupVals.SubgroupVal+" = '" + sName + "'";
                    }

                    break;
                case "IUS":
                    RetVal = RetVal + "_IUSUM set MMap = 1 ";
                    break;
                //'& " Where Subgroup_val = '" + sName + "'";
            }
            return RetVal;
        }

        #region " -- IUS -- "

        internal string UpdateBulkData()
        {
            string RetVal = string.Empty;
            RetVal = "UPDATE " + this.DBQueries.TablesName.Data + " INNER JOIN _Data ON ("
                        + this.DBQueries.TablesName.Data + "." + Timeperiods.TimePeriodNId + " = [_Data]." + Timeperiods.TimePeriodNId + ") AND "
                        + "(" + this.DBQueries.TablesName.Data + "." + Data.SourceNId + " = [_Data]." + Data.SourceNId + ") And "
                        + "(" + this.DBQueries.TablesName.Data + "." + Area.AreaNId + " = [_Data]." + Area.AreaNId + ") AND "
                        + "(" + this.DBQueries.TablesName.Data + "." + Indicator_Unit_Subgroup.IUSNId + " = [_Data]." + Indicator_Unit_Subgroup.IUSNId + ")"
                        + "SET " + this.DBQueries.TablesName.Data + "." + Data.DataValue + " =[_Data]." + Data.DataValue + " ";
            return RetVal;
        }

        internal string updateBulkDataTemp()
        {
            string RetVal = string.Empty;
            RetVal = "UPDATE _Data INNER JOIN " + this.DBQueries.TablesName.Data + " as Data ON ("
                        + "Data." + Timeperiods.TimePeriodNId + " = [_Data]." + Timeperiods.TimePeriodNId + ") AND "
                        + "( Data." + Data.SourceNId + " = [_Data]." + Data.SourceNId + ") And "
                        + "( Data." +
Area.AreaNId + " = [_Data]." +
Area.AreaNId+ ") AND "
                        + "( Data." + Indicator_Unit_Subgroup.IUSNId + " = [_Data]." + Indicator_Unit_Subgroup.IUSNId + ")"
                        + "SET _Data._Updated = -1 ";
            return RetVal;
        }

        internal string InsertBulkDataBase()
        {
            string RetVal = string.Empty;
            RetVal = "INSERT INTO " + this.DBQueries.TablesName.Data
                            + " (" + Indicator_Unit_Subgroup.IUSNId + ", " + Timeperiods.TimePeriodNId + ", " + Area.AreaNId + ", " + Data.DataValue + ", "+Data.StartDate+", "+Data.EndDate+", "
                            + " "+ Data.DataDenominator +", " + FootNotes.FootNoteNId+ ", " + Data.SourceNId + ") "
                            + " Select " + Indicator_Unit_Subgroup.IUSNId + ", " + Timeperiods.TimePeriodNId + ", " + Area.AreaNId + ", " + Data.DataValue + ", " + Data.StartDate + ", " + Data.EndDate + ", "
                            + " " + Data.DataDenominator + ", " + FootNotes.FootNoteNId + ", " + Data.SourceNId + " FROM _Data"
                            + " WHERE ( (" + Indicator_Unit_Subgroup.IUSNId + " <> -1) AND (" + Area.AreaNId + "<> -1) AND ([_Updated] = 0)) ";
            return RetVal;
        }


        internal string CreateTempTableIUS(bool bTable)
        {
            string RetVal = string.Empty;

            RetVal = "Select *, '' as MIndName, '' as MUnitName, '' as MSGName ";

            if (bTable)
            {
                RetVal = RetVal + " Into _IUS From " + this.DBQueries.TablesName.IndicatorUnitSubgroup + " WHERE 1=2";
            }
            else
            {
                RetVal = RetVal + ", '' as MMap,'' as SrcDBName  Into  _IUSUM From " +
                this.DBQueries.TablesName.IndicatorUnitSubgroup + " WHERE 1=2";
            }
            return RetVal;
        }

        internal string InsertIUSUM(string sIndName, string sUnitname, string sSubName)
        {
            return this.InsertIUSUM(sIndName, sUnitname, sSubName, -1, -1, -1, string.Empty);
        }

        internal string InsertIUSUM(string sIndName, string sUnitname, string sSubName,
  int iIndNId, int iUnitNId, int iSGNId, string sSource)
        {
            string RetVal = string.Empty;

            RetVal = "Insert Into _IUSUM (MMap, MindName, MUnitName, MSgName,SrcDBName";

            if (iIndNId != -1)
            {
                RetVal = RetVal + ", "+Indicator.IndicatorNId;
            }

            if (iUnitNId != -1)
            {
                RetVal = RetVal + ", "+ Unit.UnitNId;
            }

            if (iSGNId != -1)
            {
                RetVal = RetVal + ", "+SubgroupVals.SubgroupValNId;
            }

            RetVal = RetVal + ") ";

            // -- VALUES
            RetVal = RetVal + " Values (0,'" + sIndName + "','" + sUnitname + "','" + sSubName + "','" + sSource + "'";

            if (iIndNId != -1)
            {
                RetVal = RetVal + ", " + iIndNId;
            }

            if (iUnitNId != -1)
            {
                RetVal = RetVal + ", " + iUnitNId;
            }

            if (iSGNId != -1)
            {
                RetVal = RetVal + ", " + iSGNId;
            }

            RetVal = RetVal + ") ";

            return RetVal;
        }


        internal string GetImpIUSfromExcel()
        {
            return "Select distinct MIndName,MUnitName,MSGName,MMap," + Indicator.IndicatorNId + "," + Unit.UnitNId + ","+SubgroupVals.SubgroupValNId+" From _IUSUM "
                     + " Where MMAP = '0'";
        }

        internal string UpdateIUSMmap(string sIndicator, string sUnit, string sSubgroup,
       string sIUSNid)
        {
            string RetVal = string.Empty;

            RetVal = "Update _IUSUM set MMap = " + sIUSNid
                        + " Where MIndNAme = '" + sIndicator + "'"
                        + " and MUnitName = '" + sUnit + "' and MSGName = '" + sSubgroup + "'";
            return RetVal;
        }

        internal string MappedIUS()
        {
            return MappedIUS(true);
        }

        internal string MappedIUS(bool bValue)
        {
            string RetVal = string.Empty;

            RetVal = "Select distinct MIndName,MUnitName,MSGName,MMap," + Indicator.IndicatorNId + ", "
            + " " + Unit.UnitNId + "," + SubgroupVals.SubgroupValNId + "  from _IUSUM Where";

            if (bValue)
            {
                RetVal = RetVal + "  MMAP <> '0' ";
            }
            else
            {
                RetVal = RetVal + " " + Indicator.IndicatorNId + " <> 0 and " + Unit.UnitNId + "  <> 0"
                          + " and " + SubgroupVals.SubgroupValNId + "  <> 0 ";
            }

            return RetVal;
        }



        #endregion


        #region "-- Aggregate --"


        internal string GetDataValue(int iDataNId)
        {
            string RetVal = string.Empty;

            RetVal = "Select " + Data.DataValue + " from " + this.DBQueries.TablesName.Data + "  where "+Data.DataNId +"=" + iDataNId;

            return RetVal;
        }

        internal string CompareAreaForAggregate(string sArea, bool bArea)
        {
            string RetVal = string.Empty;

            // bArea = True for Area Nid
            // bArea = False for Area Name
            RetVal = "Select *  From " + this.DBQueries.TablesName.Area + " A ";
            if (bArea)
            {
                RetVal = RetVal + " Where A."+Area.AreaID+" = '" + sArea + "'";
            }

            if (!bArea)
            {
                RetVal = RetVal + " Where A." + Area.AreaNId + "= " + Convert.ToInt32(sArea);
            }

            return RetVal;
        }

        internal string GetRecordFrmData(int iIUSNid, int iArea_Nid, int iTimePeriod_NId, int iSourceNid)
        {
            string RetVal = string.Empty;
            RetVal = "Select * From " + this.DBQueries.TablesName.Data + " Where "
                        + " " + Indicator_Unit_Subgroup.IUSNId + " =" + iIUSNid + " AND  " + Area.AreaNId + "= "
                        + iArea_Nid + " AND " + Timeperiods.TimePeriodNId + " = " + iTimePeriod_NId
                        + " AND  " + Data.SourceNId + " = " + iSourceNid;
            return RetVal;
        }


        internal string UpdateRecordInData(int iDataNid, string sDataValue)
        {
            string RetVal = string.Empty;
            RetVal = "Update " + this.DBQueries.TablesName.Data + " SET  " + Data.DataValue + "='" + sDataValue + "' Where " + Data.DataNId + "=" + iDataNid;
            return RetVal;
        }

        internal string CreateRecordInData(int iIUSNid, int iArea_Nid, int iTimePeriod_NId, int iSourceNid, string sDataValue)
        {
            string RetVal = string.Empty;
            RetVal = " Insert into " + this.DBQueries.TablesName.Data + " (" + Indicator_Unit_Subgroup.IUSNId + "," + Timeperiods.TimePeriodNId + "," + Area.AreaNId + ", " + Data.DataValue + ", " + FootNotes.FootNoteNId + "," + Data.SourceNId + ") values("
                        + iIUSNid + "," + iTimePeriod_NId + "," + iArea_Nid + ",'" + sDataValue + "',-1," + iSourceNid + ")";
            return RetVal;
        }

        #endregion


        #region " -- Area -- "

        internal string CompareArea(string sArea, bool bArea)
        {
            string RetVal = string.Empty;
            // bArea = True for Area Nid
            // bArea = False for Area Name
            RetVal = "Select *  From _Area A ";
            if (bArea)
            {
                RetVal = RetVal + " Where A." + Area.AreaID + " = '" + sArea + "'";
            }
            if (!bArea)
            {
                RetVal = RetVal + " Where A." + Area.AreaNId + " = " + Convert.ToInt32(sArea);
            }

            return RetVal;
        }


        internal string UpdateArea(int iAreaTargetNID)
        {
            return this.UpdateArea(iAreaTargetNID, string.Empty, string.Empty);
        }

        internal string UpdateArea(int iAreaTargetNID, string sAreaName, string sAreaID)
        {
            string RetVal = string.Empty;
            RetVal = RetVal + "Update _Area Set ";

            if (sAreaName.Length > 0)
            {
                RetVal = RetVal + " MName = '" + sAreaName + "',";
            }

            if (sAreaID.Length > 0)
            {
                RetVal = RetVal + " MNID = '" + sAreaID + "'";
            }

            RetVal = RetVal + " Where " + Area.AreaNId + " = " + iAreaTargetNID;

            return RetVal;
        }

        internal string InsertBlankTable()
        {
            return this.InsertBlankTable(string.Empty, string.Empty, string.Empty);
        }

        internal string InsertBlankTable(string sAreaName, string sAreaID, string sAreaNid)
        {
            string RetVal = string.Empty;

            RetVal = "Insert Into _AreaUM (MMap," + Area.AreaName + "";

            if (sAreaID.Length > 0)
            {
                RetVal = RetVal + "," + Area.AreaID + "";
            }

            RetVal = RetVal + " )Values (0,'" + sAreaName + "'";

            if (sAreaID.Length > 0)
            {
                RetVal = RetVal + ",'" + sAreaID + "'";
            }

            RetVal = RetVal + ")";


            return RetVal;
        }



        #endregion

        #region " -- Insert -- "

        internal string Import_Save_Data(eQueryType QueryType, string sIndicatorName,
        string sUnitName, string sSGName, int iIndNid, int iUnitNid,
        int iSubNid)
        {
            return this.Import_Save_Data(QueryType, sIndicatorName, sUnitName, sSGName, iIndNid, iUnitNid, iSubNid,
                string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty
            , string.Empty, string.Empty);
        }

        internal string Import_Save_Data(eQueryType QueryType, string sIndicatorName,
         string sUnitName, string sSGName, int iIndNid, int iUnitNid,
         int iSubNid, string sTimeid, string sTime, string sAreaid, string sArea,
         string sSourceid, string sSource, string sData, string sDenominator,
         string sFootnotes, string sIUSnid)
        {
            string RetVal = string.Empty;

            if (QueryType == eQueryType.Insert) //'-- In Case of Insert
            {
                RetVal = "Insert Into _Data(MIndName,MUnitName,MSubName,"+ Area.AreaName+","+ Timeperiods.TimePeriod +", "
                + "" + Timeperiods.TimePeriodNId + ",Source," + Data.SourceNId + "," + Indicator_Unit_Subgroup.IUSNId + "," + Data.DataValue + ", _Updated," + Indicator.IndicatorNId + "," + Unit.UnitNId + "," + SubgroupVals.SubgroupValNId + " ";


                if (sAreaid.Length != 0)
                {
                    RetVal = RetVal + "," + Area.AreaNId + "";
                }

                if (sDenominator.Length != 0)
                {
                    RetVal = RetVal + "," + Data.DataDenominator + "";
                }

                if (sFootnotes.Length != 0)
                {
                    RetVal = RetVal + "," + FootNotes.FootNoteNId + ")";
                }
                else
                {
                    RetVal = RetVal + ")";
                }

                // IUS NId will always be -1 at this stage
                RetVal = RetVal + " Values('" + sIndicatorName + "','" + sUnitName + "','" + sSGName + "','" + sArea + "','"
                   + sTime + "','" + sTimeid + "','" + sSource + "','" + sSourceid + "',";

                if (sIUSnid.Length == 0)
                {
                    RetVal = RetVal + " -1";
                }
                else
                {
                    RetVal = RetVal + sIUSnid;
                }

                RetVal = RetVal + ",'" + sData + "',0," + iIndNid + "," + iUnitNid + "," + iSubNid;

                if (sAreaid.Length != 0)
                {
                    RetVal = RetVal + ",'" + sAreaid + "'";
                }

                if (sDenominator.Length != 0)
                {
                    RetVal = RetVal + ",'" + sDenominator + "'";
                }

                if (sFootnotes.Length != 0)
                {
                    RetVal = RetVal + ",'" + sFootnotes + "')";
                }
                else
                {
                    RetVal = RetVal + ")";
                }

            }
            else if (QueryType == eQueryType.Update)       //-- In case of Update
            {
                RetVal = "Update _Data Set " + Indicator_Unit_Subgroup.IUSNId + " = " + sIUSnid;

                RetVal = RetVal + " Where MindName = '" + sIndicatorName + "'"
                       + " and MunitName = '" + sUnitName + "'"
                       + " and MSubName = '" + sSGName + "'";

            }

            return RetVal;
        }

        internal string IMP_CheckUpdate_Duplicate_Data(string sIndicatorName, string sUnitName,
        string sSubgroupName, string sAreaName, string sTimeId,
        string sSOurceid)
        {
            return this.IMP_CheckUpdate_Duplicate_Data(sIndicatorName, sUnitName,
        sSubgroupName, sAreaName, sTimeId, sSOurceid, string.Empty, string.Empty, string.Empty,
            true, "0");
        }

        internal string IMP_CheckUpdate_Duplicate_Data(string sIndicatorName, string sUnitName,
        string sSubgroupName, string sAreaName, string sTimeId,
        string sSOurceid, string sData, string sFootnoteID,
        string sDenominator, bool bCheck, string IUSNID)
        {
            string RetVal = string.Empty;

            if (bCheck)
            {
                // Check The existance of the data
                RetVal = "Select " + Data.DataNId + " From _Data Where ";

                if ((sIndicatorName.Length == 0 && sUnitName.Length == 0) | IUSNID != "0")
                {
                    RetVal = RetVal + "  " + Indicator_Unit_Subgroup.IUSNId + " =" + IUSNID;
                }
                else
                {
                    RetVal = RetVal + " MIndName ='" + sIndicatorName + "' AND MUnitName = '"
                                        + sUnitName + "' AND MSubName = '" + sSubgroupName + "'";
                }
                RetVal = RetVal + " AND " + Timeperiods.TimePeriodNId + " = " + sTimeId + " AND  " + Data.SourceNId + " = " + sSOurceid
                        + " AND " + Area.AreaName + " = '" + sAreaName + "'";
                //'& " AND AreaName = '" + sAreaName + "'";
            }
            else
            {
                // If Data Exists then update it
                RetVal = "Update _Data Set " + Data.DataValue + " = '" + sData + "'";

                if (sDenominator.Length != 0)
                {
                    RetVal = RetVal + "," + Data.DataDenominator + " = " + Convert.ToInt32(sDenominator);
                }

                if (sFootnoteID.Length != 0)
                {
                    RetVal = RetVal + "," + FootNotes.FootNoteNId + " = " + Convert.ToInt32(sFootnoteID);
                }

                if ((sIndicatorName.Length == 0 & sUnitName.Length == 0) | IUSNID != "0")
                {
                    RetVal = RetVal + " Where " + Indicator_Unit_Subgroup.IUSNId + " =" + IUSNID;
                }
                else
                {
                    RetVal = RetVal + " Where MIndName ='" + sIndicatorName + "' AND MUnitName = '"
                        + sUnitName + "' AND MSubName = '" + sSubgroupName + "'";
                }

                RetVal = RetVal + " AND " + Timeperiods.TimePeriodNId + " = " + sTimeId + " AND  " + Data.SourceNId + " = " + sSOurceid
                + " AND " + Area.AreaName + " = '" + sAreaName + "'";
            }

            return RetVal;
        }

        internal string UpdateDataArea(string sAreaName, string sAreaNid)
        {
            string RetVal = string.Empty;
            RetVal = "Update _data set " + Area.AreaNId + " = " + sAreaNid + " Where " + Area.AreaName + "= '" + sAreaName + "'";
            return RetVal;
        }

        internal string IMP_CheckDuplicate_Data(string sSourceid, string sTimePeriod_NId, string sArea_NId,
        string sIUSNId)
        {
            return this.IMP_CheckDuplicate_Data(sSourceid, sTimePeriod_NId, sArea_NId, sIUSNId
                , string.Empty, string.Empty, string.Empty, true);
        }

        internal string IMP_CheckDuplicate_Data(string sSourceid, string sTimePeriod_NId, string sArea_NId,
         string sIUSNId, string sData, string sFootnoteID, string sDenominator, bool bCheck)
        {

            string RetVal = string.Empty;

            // Check The existance of the data
            if (bCheck)
            {
                RetVal = "Select " + Data.DataNId + " From _Data Where "
                        + " " + Data.SourceNId + " = " + sSourceid
                        + " AND " + Timeperiods.TimePeriodNId + " = " + sTimePeriod_NId + " AND  " + Area.AreaNId + " = " + sArea_NId
                        + " AND " + Indicator_Unit_Subgroup.IUSNId + " = " + sIUSNId;
            }
            else
            {
                // If Data Exists then update it
                RetVal = "Update _Data Set " + Data.DataValue + " = '" + sData + "'";

                if (sDenominator.Length != 0)
                {
                    RetVal = RetVal + "," + Data.DataDenominator + " = " + sDenominator;
                }


                if (sFootnoteID.Length != 0)
                {
                    RetVal = RetVal + "," + FootNotes.FootNoteNId + " = " + sFootnoteID;
                }

                RetVal = RetVal + " Where  " + Data.SourceNId + " = " + sSourceid
                        + " AND " + Timeperiods.TimePeriodNId + " = " + sTimePeriod_NId + " AND  " + Area.AreaNId + " = " + sArea_NId
                        + " AND " + Indicator_Unit_Subgroup.IUSNId + " = " + sIUSNId;
            }
            return RetVal;
        }

        #region " -- Data -- "

        internal string Getvaluesfromdatatable()
        {
            string RetVal = string.Empty;
            RetVal = " Select Distinct " + Indicator_Unit_Subgroup.IUSNId + "," + Data.SourceNId + " From _Data Where " + Indicator_Unit_Subgroup.IUSNId + "<> -1 and " + Area.AreaNId + " <> -1 ";
            return RetVal;
        }

        #endregion

        #endregion

        #region " -- Others-- "

        internal string GetTimePeriod()
        {
            return this.GetTimePeriod(string.Empty, eIDType.NIDs);
        }

        internal string GetTimePeriod(string sIDs, eIDType IDType)
        {
            string RetVal = string.Empty;

            RetVal = "Select Distinct " + Timeperiods.TimePeriodNId + "," + Timeperiods.TimePeriod + " from " + this.DBQueries.TablesName.TimePeriod;

            if (sIDs.Length != 0)
            {
                RetVal = RetVal + " WHERE 1=1";

                if (IDType == eIDType.NIDs)
                {
                    RetVal = RetVal + " AND " + Timeperiods.TimePeriodNId + " in(" + sIDs + ")";
                }
                else if (IDType == eIDType.NAMEs)
                {
                    RetVal = RetVal + " AND " + Timeperiods.TimePeriod+ " in('" + sIDs + "')";
                }

            }

            RetVal = RetVal + " Order by " + Timeperiods.TimePeriod+ " DESC ";

            return RetVal;
        }

        internal string DB_InsertUpdate_Time(eQueryType QueryType, string sTime)
        {
            return this.DB_InsertUpdate_Time(QueryType, sTime, string.Empty);
        }

        internal string DB_InsertUpdate_Time(eQueryType QueryType, string sTime, string sTimeNid)
        {
            string RetVal = string.Empty;

            //insert new time into temp table 
            if (QueryType == eQueryType.Insert)
            {
                RetVal = "INSERT INTO " + this.DBQueries.TablesName.TimePeriod + "(" + Timeperiods.TimePeriod + ")" + " Values('" + sTime + "')";
            }
            // The update query usues idata as the Key value for the TimePeriod table
            else if (QueryType == eQueryType.Update)
            {
                RetVal = "Update " + this.DBQueries.TablesName.TimePeriod + " Set " + Timeperiods.TimePeriod + " ='"
                + sTime + "' Where " + Timeperiods.TimePeriodNId + " = " + sTimeNid;
            }

            return RetVal;
        }

        internal string DB_SelectSource(string sIsoName)
        {
            return this.DB_SelectSource(sIsoName, true);
        }

        internal string DB_SelectSource(string sIsoName, bool bParentchild)
        {
            string RetVal = string.Empty;

            if (bParentchild)
            {
                RetVal = "Select "+IndicatorClassifications.ICNId+" from " + this.DBQueries.TablesName.IndicatorClassifications
                + " Where " + IndicatorClassifications.ICType + " = 'SR' and  " + IndicatorClassifications.ICParent_NId + " = -1 and " + IndicatorClassifications.ICName + " = '"
        + sIsoName + "'";
            }
            else
            {
                RetVal = "Select " + IndicatorClassifications.ICNId + " from " + this.DBQueries.TablesName.IndicatorClassifications
                + " Where " + IndicatorClassifications.ICType + " = 'SR' and  " + IndicatorClassifications.ICName + " = '" + sIsoName + "'";
            }

            return RetVal;
        }

        internal string DB_AddSource(eQueryType QueryType, string sISOData, string sAbbrData)
        {
            return this.DB_AddSource(QueryType, sISOData, sAbbrData, string.Empty, string.Empty, 0, -1);
        }

        internal string DB_AddSource(eQueryType QueryType, string sISOData, string sAbbrData,
            string sInformation, string sGuId, int iParentId, int iChildId)
        {
            string RetVal = string.Empty;

            // To insert of UPdate the Source
            if (QueryType == eQueryType.Insert)
            {
                if (iParentId == 0)
                {
                    RetVal = "INSERT INTO " + this.DBQueries.TablesName.IndicatorClassifications
                                 + "( " + IndicatorClassifications.ICParent_NId + "," + IndicatorClassifications.ICName + "," + IndicatorClassifications.ICInfo + "," + IndicatorClassifications.ICType + "," + IndicatorClassifications.ICGId + "," + IndicatorClassifications.ICGlobal + ")"
                                 + " Values(-1,'" + sISOData + "','"
                                 + sInformation + "','SR','" + sGuId + "',-1)";
                }
                else
                {
                    RetVal = "INSERT INTO " + this.DBQueries.TablesName.IndicatorClassifications
                                + "( " + IndicatorClassifications.ICParent_NId + ", " + IndicatorClassifications.ICName + "," + IndicatorClassifications.ICInfo + "," + IndicatorClassifications.ICType + "," + IndicatorClassifications.ICGId + "," + IndicatorClassifications.ICGlobal + ")" + " Values("
                                 + iParentId + ",'" + sAbbrData + "','"
                                 + sInformation + "','SR','" + sGuId + "',-1)";
                }
            }
            else
            {
                RetVal = "Update " + this.DBQueries.TablesName.IndicatorClassifications
                            + " Set " + IndicatorClassifications.ICName + " = '" + sAbbrData + "'Where " + IndicatorClassifications.ICNId + " = " + iParentId;
            }
            return RetVal;
        }

        internal string GetIUSInfoIds()
        {
            return this.GetIUSInfoIds(string.Empty, eLightHeavy.Heavy, string.Empty, string.Empty);
        }

        internal string GetIUSInfoIds(string sIDs, eLightHeavy LightHeavy,
                        string sSearch, string sItmSortOrder)
        {

            string RetVal = string.Empty;

            RetVal = "SELECT DISTINCT "
                + "IUS." + Indicator_Unit_Subgroup.IUSNId + ", I." + Indicator.IndicatorName + ", I." + Indicator.IndicatorNId + ", I." + Indicator.IndicatorGlobal + ", U." + Unit.UnitName + ", U." + Unit.UnitGlobal+ ", "
                + "SG." + SubgroupVals.SubgroupVal + " , SG." + SubgroupVals.SubgroupValGlobal + " ,IUS." + Indicator_Unit_Subgroup.MinValue + ",IUS." + Indicator_Unit_Subgroup.MaxValue + " ";

            if (LightHeavy == eLightHeavy.Heavy)
            {
                RetVal = RetVal + ", I." + Indicator.IndicatorGId + ", I." + Indicator.IndicatorInfo + ", "
                   + "U." + Unit.UnitGId + ", "
                   + "SG." + SubgroupVals.SubgroupValGId +"  ";

                //+ " , SG." + SubgroupVals.SubgroupValAge + " , "
                 // + "SG." + SubgroupVals.SubgroupValSex + " , SG." + SubgroupVals.SubgroupValLocation + " , SG." + SubgroupVals.SubgroupValOthers + 
            }

            RetVal = RetVal + " , U." + Unit.UnitNId + ", SG." + SubgroupVals.SubgroupValNId + " ";
            RetVal = RetVal + " FROM "
            + "[" + this.DBQueries.TablesName.Indicator + "] I, "
            + this.DBQueries.TablesName.Unit + " U, "
            + this.DBQueries.TablesName.SubgroupVals + " SG, "
            + this.DBQueries.TablesName.IndicatorUnitSubgroup + " IUS "
            + "WHERE "
            + "IUS." + Indicator.IndicatorNId + "=I." + Indicator.IndicatorNId + " And "
            + "IUS." + Unit.UnitNId + "=U." + Unit.UnitNId + " AND "
            + "IUS." + SubgroupVals.SubgroupValNId + " =SG." + SubgroupVals.SubgroupValNId + "  ";

            if (sIDs.Length > 0)
            {
                RetVal = RetVal + " AND IUS." + Indicator_Unit_Subgroup.IUSNId + " in (" + sIDs + ")";
            }

            if (sSearch.Length > 0)
            {
                RetVal = RetVal + " And I." + Indicator.IndicatorName + " like('%" + sSearch + "%') ";
            }

            if (sItmSortOrder.Length > 0)
            {
                RetVal = RetVal + " Order By " + sItmSortOrder;
            }

            return RetVal;
        }


        internal string CheckIUSExistanceIUS(int iIUSNid, int iSourceid)
        {
            string RetVal = string.Empty;

            RetVal = "Select count(*) From " + this.DBQueries.TablesName.IndicatorClassificationsIUS
                        + " Where " + IndicatorClassifications.ICNId + " = " + iSourceid + " and " + Indicator_Unit_Subgroup.IUSNId + " = " + iIUSNid;

            return RetVal;
        }

        internal string InsertSourceIUS_IUS(int iIUSNid, int iSourceid)
        {
            string RetVal = string.Empty;

            RetVal = "Insert Into " + this.DBQueries.TablesName.IndicatorClassificationsIUS + " (" + IndicatorClassifications.ICNId + "," + Indicator_Unit_Subgroup.IUSNId + " )"
                        + " Values (" + iSourceid + "," + iIUSNid + ")";
            return RetVal;
        }

        #endregion


        #endregion

        #endregion

    }
}
