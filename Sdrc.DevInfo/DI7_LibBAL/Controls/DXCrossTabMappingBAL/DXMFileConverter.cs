using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL.Controls.MappingControlsBAL;

namespace DevInfo.Lib.DI_LibBAL.Controls.DXCrossTabMappingBAL
{
    public static class DXSFileConveter
    {

        public static CrossTabInputFileInfo GetInputFileInfo(string filenameWPath)
        {
            CrossTabInputFileInfo RetVal;
            DataSet DXMDataSet;

            try
            {
                DXMDataSet = new DataSet();
                DXMDataSet.ReadXml(filenameWPath, XmlReadMode.ReadSchema);
                RetVal = new CrossTabInputFileInfo();
                if (DXMDataSet.Tables.Count > 0 && DXMDataSet.Tables.Count % 9 == 0)
                {

                    for (int TableIndex = 0; TableIndex < ((DXMDataSet.Tables.Count) / 9); TableIndex++)
                    {
                        RetVal.Tables.Add(DXSFileConveter.GetCrossTabTableInfo(TableIndex, DXMDataSet));

                        DXSFileConveter.AddDefaultMappingInfo(DXMDataSet, RetVal.Tables[TableIndex], TableIndex);
                        DXSFileConveter.AddColumnsMappingInfo(DXMDataSet, RetVal.Tables[TableIndex], TableIndex);

                        DXSFileConveter.AddRowsMappingInfo(DXMDataSet, RetVal.Tables[TableIndex], TableIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

            return RetVal;
        }

        private static CrossTabTableInfo GetCrossTabTableInfo(int tableIndex, DataSet dts)
        {
            CrossTabTableInfo RetVal = null;

            DataTable ColumnsHeaderTable = null;
            DataTable RowsHeaderTable = null;
            DataTable DataValueTable = null;
            DataTable CrossTabTable = null;
            string Caption = String.Empty;

            try
            {
                if (dts.Tables.Contains(TablesName.ORGCOL_TBL_NAME + tableIndex) &&
    dts.Tables.Contains(TablesName.ORGROW_TBLNAME + tableIndex) &&
       dts.Tables.Contains(TablesName.TABLE_CAPTION + tableIndex))
                {


                    ColumnsHeaderTable = dts.Tables[TablesName.ORGCOL_TBL_NAME + tableIndex];
                    RowsHeaderTable = dts.Tables[TablesName.ORGROW_TBLNAME + tableIndex];

                    // table caption
                    Caption = Convert.ToString(dts.Tables[TablesName.TABLE_CAPTION + tableIndex].Rows[0][0]);

                    // create instance of CrossTabTable
                    RetVal = new CrossTabTableInfo(Caption, ColumnsHeaderTable, RowsHeaderTable, CrossTabTable, DataValueTable);

                    // add denominator table
                    if (dts.Tables.Contains(TablesName.DENOMINATOR_TBLNAME + tableIndex))
                    {
                        RetVal.DenominatorTable = dts.Tables[TablesName.DENOMINATOR_TBLNAME + tableIndex];
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

            return RetVal;
        }

        private static void AddDefaultMappingInfo(DataSet dts, CrossTabTableInfo srcTableTableInfo, int tableIndex)
        {
            DataTable DefaultValueTable;

            try
            {
                if (srcTableTableInfo != null)
                {
                    if (dts.Tables.Contains(TablesName.DEFAULT_TBLNAME + tableIndex))
                    {
                        DefaultValueTable = dts.Tables[TablesName.DEFAULT_TBLNAME + tableIndex];

                        // indicator
                        srcTableTableInfo.DefaultMapping.IndicatorName = Convert.ToString(DefaultValueTable.Rows[0][1]);
                        srcTableTableInfo.DefaultMapping.IndicatorGID = Convert.ToString(DefaultValueTable.Rows[0][2]);

                        // unit
                        srcTableTableInfo.DefaultMapping.UnitName = Convert.ToString(DefaultValueTable.Rows[1][1]);
                        srcTableTableInfo.DefaultMapping.UnitGID = Convert.ToString(DefaultValueTable.Rows[1][2]);
                        // subgroup
                        srcTableTableInfo.DefaultMapping.SubgroupVal = Convert.ToString(DefaultValueTable.Rows[2][1]);
                        srcTableTableInfo.DefaultMapping.SubgroupValGID = Convert.ToString(DefaultValueTable.Rows[2][2]);
                        // source
                        srcTableTableInfo.DefaultMapping.Source = Convert.ToString(DefaultValueTable.Rows[3][1]);
                        // area
                        srcTableTableInfo.DefaultMapping.Area = Convert.ToString(DefaultValueTable.Rows[4][1]);
                        srcTableTableInfo.DefaultMapping.AreaID = Convert.ToString(DefaultValueTable.Rows[4][2]);
                        // timeperiod
                        srcTableTableInfo.DefaultMapping.Timeperiod = Convert.ToString(DefaultValueTable.Rows[5][1]);

                    }
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

        }

        private static void AddColumnsMappingInfo(DataSet dts, CrossTabTableInfo srcTableTableInfo, int tableIndex)
        {
            DataTable MappedTable;
            DataTable MapTypeTable;

            string MappedValueString = string.Empty;
            string MappedTypeString = string.Empty;
            string Value = string.Empty;
            string GID = string.Empty;

            string[] MappedValues;
            string[] MappedTypes;
            string[] ElementValue;
            Mapping  ColMapping;

            try
            {
                if (srcTableTableInfo != null)
                {
                    // only if mappedCol and maptype table exists
                    if (dts.Tables.Contains(TablesName.MAPPEDCOL_TBLNAME + tableIndex) && dts.Tables.Contains(TablesName.MAPTYPECOL_TBLNAME + tableIndex))
                    {
                        MapTypeTable = dts.Tables[TablesName.MAPTYPECOL_TBLNAME + tableIndex];
                        MappedTable = dts.Tables[TablesName.MAPPEDCOL_TBLNAME + tableIndex];



                        if (MapTypeTable.Columns.Count == MappedTable.Columns.Count)
                        {
                            for (int Index = 0; Index < MapTypeTable.Columns.Count; Index++)
                            {
                                MappedTypeString = Convert.ToString(MapTypeTable.Rows[0][Index]);
                                MappedValueString = Convert.ToString(MappedTable.Rows[0][Index]);
                                ColMapping = srcTableTableInfo.ColumnsMapping[Index].Mappings.CellMap;

                                //import mapping
                                DXSFileConveter.ImportMapping(MappedValueString, MappedTypeString, ColMapping);
                            }
                        }

                        //////        if (!string.IsNullOrEmpty(MappedValueString) && !string.IsNullOrEmpty(MappedTypeString))
                        //////        {
                        //////            MappedTypes = DICommon.SplitString(MappedTypeString, "\r\n");
                        //////            MappedValues = DICommon.SplitString(MappedValueString, "\r\n");

                        //////            for (int i = 0; i < MappedTypes.Length; i++)
                        //////            {
                        //////                // only if value exists
                        //////                if (!string.IsNullOrEmpty(MappedTypes[i]) && !string.IsNullOrEmpty(MappedValues[Index]))
                        //////                {
                        //////                    ColMapping = srcTableTableInfo.ColumnsMapping[Index].Mappings.CellMap;
                        //////                    ElementValue = DICommon.SplitString(MappedValues[i], "||");

                        //////                    // only if element value have value and GID
                        //////                    if (ElementValue.Length == 2)
                        //////                    {
                        //////                        Value = Convert.ToString(ElementValue[0]);
                        //////                        GID= Convert.ToString(ElementValue[1]);

                        //////                        switch (MappedTypes[i].ToUpper())
                        //////                        {
                        //////                            case "INDICATOR":
                        //////                                ColMapping.IndicatorName = Value;
                        //////                                ColMapping.IndicatorGID = GID;
                        //////                                break;

                        //////                            case "UNIT":
                        //////                                ColMapping.UnitName = Value;
                        //////                                ColMapping.UnitGID = GID;
                        //////                                break;

                        //////                            case "SUBGROUP":
                        //////                                ColMapping.SubgroupVal = Value;
                        //////                                ColMapping.SubgroupValGID = GID;
                        //////                                break;
                        //////                            case "AREA":
                        //////                                ColMapping.Area = Value;
                        //////                                ColMapping.AreaID = GID;
                        //////                                break;

                        //////                            case "SOURCE":
                        //////                                ColMapping.Source = Value;
                        //////                                break;

                        //////                            case "TIMEPERIOD":
                        //////                                ColMapping.Timeperiod = Value;
                        //////                                break;

                        //////                            case "AGE":
                        //////                            case "SEX":
                        //////                            case "LOCATION":
                        //////                            case "OTHERS":
                        //////                                if(ColMapping.Subgroups.ContainsKey(GID)==false)
                        //////                                {
                        //////                                    DevInfo.Lib.DI_LibBAL.DA.DML.DI6SubgroupInfo SG =new DevInfo.Lib.DI_LibBAL.DA.DML.DI6SubgroupInfo();
                        //////                                    SG.Name=Value;
                        //////                                    SG.GID=GID;
                        //////                                    ColMapping.Subgroups.Add(GID, SG);
                        //////                                }

                        //////                                break;
                        //////                            default:
                        //////                                break;
                        //////                        }
                        //////                    }
                        //////                }
                        //////            }
                                    
                        //////        }
                        //////    }

                        //////}
                    }
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException(ex.ToString());
            }
        }



        private static void AddRowsMappingInfo(DataSet dts, CrossTabTableInfo srcTableTableInfo, int tableIndex)
        {
            DataTable MappedTable;
            DataTable MapTypeTable;

            string MappedValueString = string.Empty;
            string MappedTypeString = string.Empty;
            string Value = string.Empty;
            string GID = string.Empty;

            string[] MappedValues;
            string[] MappedTypes;
            string[] ElementValue;
            Mapping RowMapping;

            try
            {
                if (srcTableTableInfo != null)
                {
                    // only if mappedRow and maptype table exists
                    if (dts.Tables.Contains(TablesName.MAPPEDROW_TBLNAME + tableIndex) && dts.Tables.Contains(TablesName.MAPTYPEROW_TBLNAME + tableIndex))
                    {
                        MapTypeTable = dts.Tables[TablesName.MAPTYPEROW_TBLNAME + tableIndex];
                        MappedTable = dts.Tables[TablesName.MAPPEDROW_TBLNAME + tableIndex];

                        if (MapTypeTable.Rows.Count == MappedTable.Rows.Count)
                        {
                            for (int Index = 0; Index < MapTypeTable.Rows.Count; Index++)
                            {
                                MappedTypeString = Convert.ToString(MapTypeTable.Rows[Index][0]);
                                MappedValueString = Convert.ToString(MappedTable.Rows[Index][0]);

                                RowMapping = srcTableTableInfo.RowsMapping[Index].Mappings.CellMap;

                                //import mapping
                              DXSFileConveter.ImportMapping(MappedValueString, MappedTypeString, RowMapping);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException(ex.ToString());
            }
        }

        private static void ImportMapping(string MappedValueString, string MappedTypeString, Mapping elementMapping)
        {
            

            string Value = string.Empty;
            string GID = string.Empty;

            string[] MappedValues;
            string[] MappedTypes;
            string[] ElementValue;
           

            if (!string.IsNullOrEmpty(MappedValueString) && !string.IsNullOrEmpty(MappedTypeString))
            {
                MappedTypes = DICommon.SplitString(MappedTypeString, "\r\n");
                MappedValues = DICommon.SplitString(MappedValueString, "\r\n");

                for (int i = 0; i < MappedTypes.Length; i++)
                {
                    // only if value exists
                    if (!string.IsNullOrEmpty(MappedTypes[i]) && !string.IsNullOrEmpty(MappedValues[i]))
                    {                        
                        ElementValue = DICommon.SplitString(MappedValues[i], "||");

                        // only if element value have value and GID
                        if (ElementValue.Length == 2)
                        {
                            Value = Convert.ToString(ElementValue[0]);
                            GID = Convert.ToString(ElementValue[1]);

                            switch (MappedTypes[i].ToUpper())
                            {
                                case "INDICATOR":
                                    elementMapping.IndicatorName = Value;
                                    elementMapping.IndicatorGID = GID;
                                    break;

                                case "UNIT":
                                    elementMapping.UnitName = Value;
                                    elementMapping.UnitGID = GID;
                                    break;

                                case "SUBGROUP":
                                    elementMapping.SubgroupVal = Value;
                                    elementMapping.SubgroupValGID = GID;
                                    break;
                                case "AREA":
                                    elementMapping.Area = Value;
                                    elementMapping.AreaID = GID;
                                    break;

                                case "SOURCE":
                                    elementMapping.Source = Value;
                                    break;

                                case "TIMEPERIOD":
                                    elementMapping.Timeperiod = Value;
                                    break;

                                case "AGE":
                                case "SEX":
                                case "LOCATION":
                                case "OTHERS":
                                    if (elementMapping.Subgroups.ContainsKey(GID) == false)
                                    {
                                        DevInfo.Lib.DI_LibBAL.DA.DML.DI6SubgroupInfo SG = new DevInfo.Lib.DI_LibBAL.DA.DML.DI6SubgroupInfo();
                                        SG.Name = Value;
                                        SG.GID = GID;
                                        elementMapping.Subgroups.Add(GID, SG);
                                    }

                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }

            }
        }
    }

}
