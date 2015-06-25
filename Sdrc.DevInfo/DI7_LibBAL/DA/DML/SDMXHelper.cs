using System;
using System.Collections.Generic;
using System.Text;
using SDMXObjectModel.Common;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Connection;
using SDMXConstants = DevInfo.Lib.DI_LibSDMX.Constants;
using DevInfo.Lib.DI_LibSDMX;
using System.Data;
using DevInfo.Lib.DI_LibBAL.Export;
using SDMXObjectModel.Data.StructureSpecific;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.Xml;
using DevInfo.Lib.DI_LibBAL.Utility;
using System.IO;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{

    /// <summary>
    /// Delegate for intitializing Process of Background Worker.
    /// </summary>
    /// <param name="currentProcess"></param>
    /// <param name="totalProgressCount"></param>
    /// <param name="maximumValue"></param>
    public delegate void Initialize_Process(string mainProcessName, int totalProgressCount, int maximumValue);

    /// <summary>
    /// Delegate for setting Process Info to Background Worker.
    /// </summary>
    /// <param name="indicatorName"></param>
    /// <param name="unitName"></param>
    /// <param name="currentProgressCount"></param>
    public delegate void Set_Process_Name(string indicatorName, string unitName);

    /// <summary>
    /// Delegate for creating event for notifying progress of a process.
    /// </summary>
    /// <param name="recordNo"></param>
    public delegate void Notify_Progress(int recordNo);

    /// <summary>
    /// Delegate for creating event for notifying file name on file creation.
    /// </summary>
    /// <param name="fileNameWPath">The file name with path of created file.</param>
    public delegate void Notify_File_Name(string fileNameWPath);

    /// <summary>
    /// Delegate for creating event for IUS skipped during import into database
    /// </summary>
    /// <param name="indicatorName"></param>
    /// <param name="unitName"></param>
    /// <param name="subgroup"></param>
    public delegate void IUSSkipped(string indicatorName, string unitName, string subgroup);


    public static class SDMXHelper
    {

        #region "Events"

        /// <summary>
        /// Event for intitializing Process of Background Worker.
        /// </summary>
        public static event Initialize_Process Initialize_Process_Event;

        /// <summary>
        /// Event for setting Process Info to Background Worker.
        /// </summary>
        public static event Set_Process_Name Set_Process_Name_Event;

        /// <summary>
        /// Event for notifying progress of a process.
        /// </summary>
        public static event Notify_Progress Notify_Progress_Event;

        /// <summary>
        /// Event for notifying file name on file creation.
        /// </summary>
        public static event Notify_File_Name Notify_File_Name_Event;

        /// <summary>
        /// Event for notifying IUS skipped during import.
        /// </summary>
        public static event IUSSkipped IUSSkipped_Event;

        #endregion "Events"

        #region "Raise Event methods"

        /// <summary>
        /// Raise_Initilize_Process_Event method raises the event for intitializing Process of Background Worker.
        /// </summary>
        /// <param name="currentProcess"></param>
        /// <param name="totalSheetCount"></param>
        /// <param name="maximumValue"></param>
        internal static void Raise_Initilize_Process_Event(string currentProcess, int totalSheetCount, int maximumValue)
        {
            if (Initialize_Process_Event != null)
            {
                Initialize_Process_Event(currentProcess, totalSheetCount, maximumValue);
            }
        }

        /// <summary>
        /// Raise_Set_Process_Name_Event method raises the event for setting Process Info to Background Worker.
        /// </summary>
        /// <param name="indicatorName"></param>
        /// <param name="unitName"></param>
        /// <param name="currentSheetNo"></param>
        internal static void Raise_Set_Process_Name_Event(string indicatorName, string unitName, int currentSheetNo)
        {
            if (Set_Process_Name_Event != null)
            {
                Set_Process_Name_Event(indicatorName, unitName);
            }
        }

        /// <summary>
        /// Raise_Notify_Progress_Event method raises the event for notifying progress of a process. 
        /// </summary>
        /// <param name="recordNo"></param>
        internal static void Raise_Notify_Progress_Event(int recordNo)
        {
            if (Notify_Progress_Event != null)
            {
                Notify_Progress_Event(recordNo);
            }
        }


        /// <summary>
        /// Raise_Notify_File_Name_Event method raises the event for notifying file name on SDMX-ML data file creation. 
        /// </summary>
        /// <param name="fileName"></param>
        internal static void Raise_Notify_File_Name_Event(string fileName)
        {
            if (Notify_File_Name_Event != null)
            {
                Notify_File_Name_Event(fileName);
            }
        }

        /// <summary>
        /// Raise_IUSSkipped_Event method raises event for notifying IUS skipped during import.
        /// </summary>
        /// <param name="indicatorFileNameWPath"></param>
        internal static void Raise_IUSSkipped_Event(string indicator, string unit, string subgroup)
        {
            if (IUSSkipped_Event != null)
            {
                IUSSkipped_Event(indicator, unit, subgroup);
            }
        }

        #endregion "Raise Event methods"

        /// <summary>
        /// Import DSD from Webservice to template/database
        /// </summary>
        /// <param name="schemaType"></param>
        /// <param name="dsdfileNameWPath"></param>
        /// <param name="format"></param>
        /// <param name="agencyId"></param>
        /// <param name="dbConnection"></param>
        /// <param name="dbQueries"></param>
        /// <returns></returns>
        public static bool Import_DSD(SDMXSchemaType schemaType, string dsdfileNameWPath, DataFormats format, string agencyId, DIConnection dbConnection, DIQueries dbQueries)
        {
            bool RetVal = false;

            RetVal = SDMXHelper.Import_DSD(schemaType, dsdfileNameWPath, format, agencyId, dbConnection, dbQueries, false);

            return RetVal;
        }

        /// <summary>
        /// Import DSD from Webservice to template/database
        /// </summary>
        /// <param name="schemaType"></param>
        /// <param name="dsdfileNameWPath"></param>
        /// <param name="format"></param>
        /// <param name="agencyId"></param>
        /// <param name="dbConnection"></param>
        /// <param name="dbQueries"></param>
        /// <param name="includeSource"></param>
        /// <returns></returns>
        public static bool Import_DSD(SDMXSchemaType schemaType, string dsdfileNameWPath, DataFormats format, string agencyId, DIConnection dbConnection, DIQueries dbQueries, bool includeSource)
        {
            bool RetVal = false;
            BaseSDMXHelper BaseSDMXHelpeObj = null;
            try
            {
                BaseSDMXHelpeObj = new BaseSDMXHelper(dbConnection, dbQueries);


                RetVal = BaseSDMXHelpeObj.Import_DSD(schemaType, dsdfileNameWPath, includeSource);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RetVal;
        }

        public static bool Import_Metadata(SDMXSchemaType schemaType, string tempxmlfileNameWPath, string sdmxWebServiceUrl, DIConnection dbConnection, DIQueries dbQueries)
        {
            bool RetVal = false;
            BaseSDMXHelper BaseSDMXHelpeObj = null;
            try
            {
                BaseSDMXHelpeObj = new BaseSDMXHelper(dbConnection, dbQueries);


                RetVal = BaseSDMXHelpeObj.GenerateMetadataXMLFromSDMXWebservice(sdmxWebServiceUrl, tempxmlfileNameWPath);
            }
            catch (Exception)
            {
            }

            return RetVal;
        }


        /// <summary>
        /// Genmerate DES File from 
        /// </summary>
        /// <param name="schemaType"></param>
        /// <param name="DESFileNameWPath"></param>
        /// <param name="format"></param>
        /// <param name="agencyId"></param>
        /// <param name="dbConnection"></param>
        /// <param name="dbQueries"></param>
        /// <returns></returns>
        public static bool Generate_DES(SDMXSchemaType schemaType, DataFormats format, string agencyId, string DESOutputFileNameWPath, string SDMXFileNameWithPath, DIConnection dbConnection, DIQueries dbQueries)
        {

            bool RetVal = false;
            BaseSDMXHelper BaseSDMXHelpeObj = null;
            try
            {
                BaseSDMXHelpeObj = new BaseSDMXHelper(dbConnection, dbQueries);


                RetVal = BaseSDMXHelpeObj.Generate_DES(schemaType, DESOutputFileNameWPath, SDMXFileNameWithPath);
            }
            catch (Exception)
            {
            }
            return RetVal;

        }

        /// <summary>
        /// Generate_DES
        /// </summary>
        /// <param name="schemaType"></param>
        /// <param name="format"></param>
        /// <param name="agencyId"></param>
        /// <param name="DESOutputFileNameWPath"></param>
        /// <param name="dataFileNameWithPath"></param>
        /// <param name="DSDFileNameWPath"></param>
        /// <returns></returns>
        public static bool Generate_DES(SDMXSchemaType schemaType, DataFormats format, string agencyId, string DESOutputFileNameWPath, string SDMXFileNameWithPath, string DSDFileNameWPath, string languageCode)
        {
            bool RetVal = false;
            BaseSDMXHelper BaseSDMXHelpeObj = null;
            try
            {
                BaseSDMXHelpeObj = new BaseSDMXHelper();//(dbConnection, dbQueries);


                RetVal = BaseSDMXHelpeObj.Generate_DES(schemaType, DESOutputFileNameWPath, SDMXFileNameWithPath, DSDFileNameWPath, languageCode);
            }
            catch (Exception)
            {
            }

            return RetVal;
        }

        /// <summary>
        /// Generate DES File from SDMX data files
        /// </summary>
        /// <param name="SDMXFileNameWithPath"></param>
        /// <param name="DSDFileNameWPath"></param>
        /// <param name="outputFileNameWPath"></param>
        /// <returns></returns>
        public static bool Generate_DES(string SDMXFileNameWithPath, string DSDFileNameWPath, string outputFileNameWPath, string languageCode)
        {
            bool RetVal = false;

            try
            {
                RetVal = Generate_DES(SDMXSchemaType.Two_One, DataFormats.StructureSpecificTS, "agency", outputFileNameWPath, SDMXFileNameWithPath, DSDFileNameWPath, languageCode);

            }
            catch (Exception)
            {
                RetVal = false;
            }

            return RetVal;
        }

        /// <summary>
        /// Import SDMX Data Files
        /// </summary>
        /// <param name="schemaType"></param>
        /// <param name="format"></param>
        /// <param name="agencyId"></param>
        /// <param name="SDMXFileNameWithPath"></param>
        /// <param name="dbConnection"></param>
        /// <param name="dbQueries"></param>
        /// <returns></returns>
        public static bool Import_SDMXML_Data(SDMXSchemaType schemaType, DataFormats format, string agencyId, string SDMXFileNameWithPath, DIConnection dbConnection, DIQueries dbQueries)
        {
            bool RetVal = false;

            BaseSDMXHelper BaseSDMXHelpeObj = null;
            try
            {
                BaseSDMXHelpeObj = new BaseSDMXHelper(dbConnection, dbQueries);


                RetVal = BaseSDMXHelpeObj.Import_SDMXML_Data(SDMXFileNameWithPath);
            }
            catch (Exception)
            {
            }


            return RetVal;
        }

        /// <summary>
        /// Generate SDMX Files from DES
        /// </summary>
        /// <param name="DESFileNameWithPath"></param>
        /// <param name="DSDFileNameWPath"></param>
        /// <param name="outputFolder"></param>
        /// <returns></returns>
        public static bool Generate_SDMXML_Data(string DESFileNameWithPath, string DSDFileNameWPath, string outputFolder)
        {
            bool RetVal = false;
            DIConnection DBConnection = null;
            DIQueries DBQueries = null;
            int count = 0;

            //  Utility.TemporaryFileNamePath
            DataTable Table = DevInfo.Lib.DI_LibBAL.Import.DAImport.DES.DataEntrySpreadsheets.GetDataTableForAllDESSheets(DESFileNameWithPath);

            string TempDatabaseName = string.Empty;
            string TempDatabaseNameCompact = string.Empty;
            TempDatabaseName = Path.Combine(Path.GetDirectoryName(DESFileNameWithPath), DICommon.GetValidFileName(DateTime.Now.Ticks.ToString() + DICommon.FileExtension.Template));

            Dictionary<string, int> IndicatorList = new Dictionary<string, int>();
            Dictionary<string, int> UnitList = new Dictionary<string, int>();
            Dictionary<string, int> AreaList = new Dictionary<string, int>();
            Dictionary<string, int> SgValList = new Dictionary<string, int>();
            Dictionary<string, int> TimepeirodList = new Dictionary<string, int>();
            Dictionary<string, int> SourceList = new Dictionary<string, int>();

            try
            {

                DevInfo.Lib.DI_LibDAL.Resources.Resource.GetBlankDevInfoDBFile(TempDatabaseName);

                DBConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, TempDatabaseName, string.Empty, string.Empty);
                DBQueries = new DIQueries(DBConnection.DIDataSetDefault(), DBConnection.DILanguageCodeDefault(DBConnection.DIDataSetDefault()));

                DIDataValueHelper.MergeTextualandNumericDataValueColumn(DBConnection, DBQueries);

                IndicatorBuilder IndBuilder = new IndicatorBuilder(DBConnection, DBQueries);
                UnitBuilder UBuilder = new UnitBuilder(DBConnection, DBQueries);
                DI6SubgroupValBuilder SGBuilder = new DI6SubgroupValBuilder(DBConnection, DBQueries);
                AreaBuilder AreaBuilderObj = new AreaBuilder(DBConnection, DBQueries);
                IndicatorClassificationBuilder ICBuilder = new IndicatorClassificationBuilder(DBConnection, DBQueries);
                TimeperiodBuilder TBuilder = new TimeperiodBuilder(DBConnection, DBQueries);
                SourceBuilder SrcBuilder = new SourceBuilder(DBConnection, DBQueries);
                DIDatabase DatabaseObj = new DIDatabase(DBConnection, DBQueries);
                IUSBuilder IUSBuilderObj = new IUSBuilder(DBConnection, DBQueries);


                foreach (DataRow Row in Table.Rows)
                {
                    int IndicatorNId = 0;
                    int UnitNId = 0;
                    int SGValNId = 0;
                    int AreaNId = 0;
                    int TimeperiodNId = 0;
                    int SourceNid = 0;
                    int IUSNID = 0;

                    //-- Import Indicator
                    if (IndicatorList.ContainsKey(Convert.ToString(Row[Indicator.IndicatorGId])))
                    {
                        IndicatorNId = IndicatorList[Convert.ToString(Row[Indicator.IndicatorGId])];
                    }
                    else
                    {
                        IndicatorNId = IndBuilder.ImportIndicator(Convert.ToString(Row[Indicator.IndicatorName]), Convert.ToString(Row[Indicator.IndicatorGId]), false);
                        IndicatorList.Add(Convert.ToString(Row[Indicator.IndicatorGId]), IndicatorNId);
                    }

                    //-- Import Unit
                    if (UnitList.ContainsKey(Convert.ToString(Row[Unit.UnitGId])))
                    {
                        UnitNId = UnitList[Convert.ToString(Row[Unit.UnitGId])];
                    }
                    else
                    {
                        UnitNId = UBuilder.ImportUnit(Convert.ToString(Row[Unit.UnitGId]), Convert.ToString(Row[Unit.UnitName]).Trim(), false);
                        UnitList.Add(Convert.ToString(Row[Unit.UnitGId]), UnitNId);
                    }

                    //-- Import SubgroupVal
                    if (!string.IsNullOrEmpty(Convert.ToString(Row[SubgroupVals.SubgroupVal])))
                    {
                        if (SgValList.ContainsKey(Convert.ToString(Row[SubgroupVals.SubgroupValGId])))
                        {
                            SGValNId = SgValList[Convert.ToString(Row[SubgroupVals.SubgroupValGId])];
                        }
                        else
                        {
                            SGValNId = SGBuilder.ImportSubgroupVal(Convert.ToString(Row[SubgroupVals.SubgroupVal]), Convert.ToString(Row[SubgroupVals.SubgroupValGId]), false);
                            SgValList.Add(Convert.ToString(Row[SubgroupVals.SubgroupValGId]), SGValNId);
                        }
                    }

                    //-- Import Area
                    if (AreaList.ContainsKey(Convert.ToString(Row[Area.AreaID])))
                    {
                        AreaNId = AreaList[Convert.ToString(Row[Area.AreaID])];
                    }
                    else
                    {
                        AreaNId = AreaBuilderObj.ImportArea(Convert.ToString(Row[Area.AreaName]), Convert.ToString(Row[Area.AreaID]), String.Empty, false);
                        AreaList.Add(Convert.ToString(Row[Area.AreaID]), AreaNId);
                    }

                    //-- Import Timeperiod
                    if (TimepeirodList.ContainsKey(Convert.ToString(Row[Timeperiods.TimePeriod])))
                    {
                        TimeperiodNId = TimepeirodList[Convert.ToString(Row[Timeperiods.TimePeriod])];
                    }
                    else
                    {
                        TimeperiodNId = TBuilder.CheckNCreateTimeperiod(Convert.ToString(Row[Timeperiods.TimePeriod]));
                        TimepeirodList.Add(Convert.ToString(Row[Timeperiods.TimePeriod]), TimeperiodNId);
                    }

                    //-- Import Source
                    if (SourceList.ContainsKey(Convert.ToString(Row[IndicatorClassifications.ICName])))
                    {
                        SourceNid = SourceList[Convert.ToString(Row[IndicatorClassifications.ICName])];
                    }
                    else
                    {
                        SourceNid = SrcBuilder.CheckNCreateSource(Convert.ToString(Row[IndicatorClassifications.ICName]));
                        SourceList.Add(Convert.ToString(Row[IndicatorClassifications.ICName]), SourceNid);
                    }


                    IUSNID = IUSBuilderObj.ImportIUS(IndicatorNId, UnitNId, SGValNId, 0, 0, DBQueries, DBConnection);

                    if (IUSNID > 0 && AreaNId > 0 && TimeperiodNId > 0 && !string.IsNullOrEmpty(Convert.ToString(Row[Data.DataValue])))
                    {
                        DatabaseObj.CheckNCreateData(AreaNId, IUSNID, SourceNid, TimeperiodNId, Convert.ToString(Row[Data.DataValue]));
                    }
                }

                DatabaseObj.UpdateIndicatorUnitSubgroupNIDsInData();

                //-- Compact Database into TempFile
                TempDatabaseNameCompact = Path.Combine(Path.GetDirectoryName(DESFileNameWithPath), DICommon.GetValidFileName(DateTime.Now.Ticks.ToString() + DICommon.FileExtension.Template));

                DIDatabase.CompactDataBase(ref DBConnection, DBQueries, TempDatabaseNameCompact, true);


                DBConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, TempDatabaseNameCompact, string.Empty, string.Empty);
                DBQueries = new DIQueries(DBConnection.DIDataSetDefault(), DBConnection.DILanguageCodeDefault(DBConnection.DIDataSetDefault()));

                if (!Directory.Exists(Path.Combine(outputFolder, DBQueries.LanguageCode.Trim('_'))))
                {
                    Directory.CreateDirectory(Path.Combine(outputFolder, DBQueries.LanguageCode.Trim('_')));
                }

                System.Xml.XmlDocument XmlDoc = SDMXUtility.Get_Query(SDMXSchemaType.Two_One, null, QueryFormats.StructureSpecificTS, DataReturnDetailTypes.Full, Guid.NewGuid().ToString().Replace("-", "").Replace("_", ""), DBConnection, DBQueries);

                RetVal = SDMXUtility.Generate_Data(SDMXSchemaType.Two_One, XmlDoc, DevInfo.Lib.DI_LibSDMX.DataFormats.StructureSpecificTS, DBConnection, DBQueries, outputFolder);


            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }

            return RetVal;


        }

    }
}
