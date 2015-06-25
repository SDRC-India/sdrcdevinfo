using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using SDMXObjectModel.Common;
using SDMXObjectModel.Registry;
using System.Data;

namespace DevInfo.Lib.DI_LibSDMX
{
    public static class SDMXUtility
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

        public static XmlDocument Get_Data(SDMXSchemaType schemaType, XmlDocument query, DataFormats format, DIConnection DIConnection, DIQueries DIQueries)
        {
            XmlDocument RetVal;
            BaseDataUtility BaseDataUtility;

            RetVal = null;
            BaseDataUtility = null;

            try
            {
                switch (format)
                {
                    case DataFormats.Generic:
                        BaseDataUtility = new GenericDataUtility(DIConnection, DIQueries, string.Empty);
                        break;
                    case DataFormats.GenericTS:
                        BaseDataUtility = new GenericTimeSeriesDataUtility(DIConnection, DIQueries, string.Empty);
                        break;
                    case DataFormats.StructureSpecific:
                        BaseDataUtility = new StructureSpecificDataUtility(DIConnection, DIQueries, string.Empty);
                        break;
                    case DataFormats.StructureSpecificTS:
                        BaseDataUtility = new StructureSpecificTimeSeriesDataUtility(DIConnection, DIQueries, string.Empty);
                        break;
                    default:
                        break;
                }

                RetVal = BaseDataUtility.Get_Data(query);
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains(Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message) &&
                    !ex.Message.Contains(Constants.SDMXWebServices.Exceptions.NoResults.Message) &&
                    !ex.Message.Contains(Constants.SDMXWebServices.Exceptions.NotImplemented.Message))
                {
                    throw new Exception(Constants.SDMXWebServices.Exceptions.ServerError.Message);
                }
                else
                {
                    throw ex;
                }
            }
            finally
            {
            }

            return RetVal;
        }

        public static string Get_SubgroupVal_GId(Dictionary<string, string> DictSubgroupBreakup, DataTable DtSubgroupBreakup)
        {
            string RetVal;
            bool IsSetFlag;
            string SubgroupValGId, SubgroupTypeGId, SubgroupGId;
            List<string> ProcessedSubgroupValGIds;
            DataRow[] SubgroupValRows;

            RetVal = string.Empty;
            IsSetFlag = false;
            SubgroupValGId = string.Empty;
            SubgroupTypeGId = string.Empty;
            SubgroupGId = string.Empty;
            ProcessedSubgroupValGIds = new List<string>();
            SubgroupValRows = null;

            try
            {
                foreach (DataRow DrSubgroupBreakup in DtSubgroupBreakup.Rows)
                {
                    SubgroupValGId = DrSubgroupBreakup[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGId].ToString();

                    if (!ProcessedSubgroupValGIds.Contains(SubgroupValGId))
                    {
                        ProcessedSubgroupValGIds.Add(SubgroupValGId);

                        SubgroupValRows = DtSubgroupBreakup.Select(DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGId + Constants.EqualsTo + Constants.Apostophe + SubgroupValGId + Constants.Apostophe);

                        if (SubgroupValRows.Length == DictSubgroupBreakup.Count)
                        {
                            foreach (DataRow DrSubgroupVal in SubgroupValRows)
                            {
                                SubgroupTypeGId = DrSubgroupVal[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupTypes.SubgroupTypeGID].ToString();
                                SubgroupGId = DrSubgroupVal[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Subgroup.SubgroupGId].ToString();

                                if (DictSubgroupBreakup.ContainsKey(SubgroupTypeGId) && DictSubgroupBreakup[SubgroupTypeGId] == SubgroupGId)
                                {
                                    IsSetFlag = true;
                                }
                                else
                                {
                                    IsSetFlag = false;
                                    break;
                                }
                            }

                            if (IsSetFlag == true)
                            {
                                RetVal = SubgroupValGId;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        public static bool Generate_Data(SDMXSchemaType schemaType, XmlDocument query, DataFormats format, DIConnection DIConnection, DIQueries DIQueries, string outputFolder, out int fileCount,out List<string> GeneratedFiles,SDMXObjectModel.Message.StructureHeaderType Header)
        {
            bool RetVal;
            BaseDataUtility BaseDataUtility;

            RetVal = false;
            BaseDataUtility = null;
            fileCount = 0;
            GeneratedFiles = new List<string>();
            try
            {
                switch (format)
                {
                    case DataFormats.Generic:
                        BaseDataUtility = new GenericDataUtility(DIConnection, DIQueries, string.Empty);
                        break;
                    case DataFormats.GenericTS:
                        BaseDataUtility = new GenericTimeSeriesDataUtility(DIConnection, DIQueries, string.Empty);
                        break;
                    case DataFormats.StructureSpecific:
                        BaseDataUtility = new StructureSpecificDataUtility(DIConnection, DIQueries, string.Empty);
                        break;
                    case DataFormats.StructureSpecificTS:
                        BaseDataUtility = new StructureSpecificTimeSeriesDataUtility(DIConnection, DIQueries, string.Empty);
                        break;
                    default:
                        break;
                }

                RetVal = BaseDataUtility.Generate_Data(query, outputFolder,out fileCount,out GeneratedFiles,Header);
            }
            catch (Exception ex)
            {
                RetVal = false;
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        public static bool Generate_Data(SDMXSchemaType schemaType, XmlDocument query, DataFormats format, DIConnection DIConnection, DIQueries DIQueries, string outputFolder)
        {
            bool RetVal;
            BaseDataUtility BaseDataUtility;

            RetVal = false;
            BaseDataUtility = null;
            
            try
            {
                switch (format)
                {
                    case DataFormats.Generic:
                        BaseDataUtility = new GenericDataUtility(DIConnection, DIQueries, string.Empty);
                        break;
                    case DataFormats.GenericTS:
                        BaseDataUtility = new GenericTimeSeriesDataUtility(DIConnection, DIQueries, string.Empty);
                        break;
                    case DataFormats.StructureSpecific:
                        BaseDataUtility = new StructureSpecificDataUtility(DIConnection, DIQueries, string.Empty);
                        break;
                    case DataFormats.StructureSpecificTS:
                        BaseDataUtility = new StructureSpecificTimeSeriesDataUtility(DIConnection, DIQueries, string.Empty);
                        break;
                    default:
                        break;
                }

                RetVal = BaseDataUtility.Generate_Data(query, outputFolder);
            }
            catch (Exception ex)
            {
                RetVal = false;
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }
       
        public static XmlDocument Get_MetadataReport(SDMXSchemaType schemaType, XmlDocument query, string language, Header header, DIConnection DIConnection, DIQueries DIQueries)
        {
            XmlDocument RetVal;
            MetadataReportUtility MetadataReportUtility;

            RetVal = null;
            MetadataReportUtility = null;

            try
            {
                MetadataReportUtility = new MetadataReportUtility(string.Empty, language, header, DIConnection, DIQueries);
                RetVal = MetadataReportUtility.Get_MetadataReport(query);
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains(Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message) &&
                    !ex.Message.Contains(Constants.SDMXWebServices.Exceptions.NoResults.Message) &&
                    !ex.Message.Contains(Constants.SDMXWebServices.Exceptions.NotImplemented.Message))
                {
                    throw new Exception(Constants.SDMXWebServices.Exceptions.ServerError.Message);
                }
                else
                {
                    throw ex;
                }
            }
            finally
            {
            }

            return RetVal;
        }

        public static XmlDocument Get_MetadataReport(SDMXSchemaType schemaType, string TargetObjectId, MetadataTypes metadataType, string agencyId, string language, DIConnection DIConnection, DIQueries DIQueries)
        {
            XmlDocument RetVal;
            MetadataReportUtility MetadataReportUtility;

            RetVal = null;
            MetadataReportUtility = null;

            try
            {
                MetadataReportUtility = new MetadataReportUtility(agencyId, language, null, DIConnection, DIQueries);
                RetVal = MetadataReportUtility.Get_MetadataReport(TargetObjectId, metadataType);
            }
            catch (Exception ex)
            {
                RetVal = null;
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        public static bool Generate_MetadataReport(SDMXSchemaType schemaType, MetadataTypes metadataType, string filterNIds, string agencyId, string language, Header header, DIConnection DIConnection, DIQueries DIQueries, string outputFolder, out List<string> GeneratedMetadataFiles, SDMXObjectModel.Message.StructureHeaderType Header)
        {
            bool RetVal;
            MetadataReportUtility MetadataReportUtility;

            RetVal = false;
            MetadataReportUtility = null;
            GeneratedMetadataFiles = new List<string>();
            try
            {
                MetadataReportUtility = new MetadataReportUtility(agencyId, language, header, DIConnection, DIQueries);
                RetVal = MetadataReportUtility.Generate_MetadataReport(metadataType, filterNIds, outputFolder,out GeneratedMetadataFiles,Header);
            }
            catch (Exception ex)
            {
                RetVal = false;
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        public static bool Generate_MetadataReport(SDMXSchemaType schemaType, MetadataTypes metadataType, string filterNIds, string agencyId, string language, Header header, DIConnection DIConnection, DIQueries DIQueries, string outputFolder)
        {
            bool RetVal;
            MetadataReportUtility MetadataReportUtility;

            RetVal = false;
            MetadataReportUtility = null;

            try
            {
                MetadataReportUtility = new MetadataReportUtility(agencyId, language, header, DIConnection, DIQueries);
                RetVal = MetadataReportUtility.Generate_MetadataReport(metadataType, filterNIds, outputFolder);
            }
            catch (Exception ex)
            {
                RetVal = false;
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        public static XmlDocument Get_Query(SDMXSchemaType schemaType, Dictionary<string, string> dictUserSelections, QueryFormats format, DataReturnDetailTypes dataReturnDetailType, string agencyId, DIConnection DIConnection, DIQueries DIQueries)
        {
            XmlDocument RetVal;
            BaseQueryUtility BaseQueryUtility;

            RetVal = null;
            BaseQueryUtility = null;

            try
            {
                switch (format)
                {
                    case QueryFormats.Generic:
                        BaseQueryUtility = new GenericQueryUtility(dictUserSelections, dataReturnDetailType, agencyId, DIConnection, DIQueries);
                        break;
                    case QueryFormats.GenericTS:
                        BaseQueryUtility = new GenericTimeSeriesQueryUtility(dictUserSelections, dataReturnDetailType, agencyId, DIConnection, DIQueries);
                        break;
                    case QueryFormats.StructureSpecific:
                        BaseQueryUtility = new StructureSpecificQueryUtility(dictUserSelections, dataReturnDetailType, agencyId, DIConnection, DIQueries);
                        break;
                    case QueryFormats.StructureSpecificTS:
                        BaseQueryUtility = new StructureSpecificTimeSeriesQueryUtility(dictUserSelections, dataReturnDetailType, agencyId, DIConnection, DIQueries);
                        break;
                    default:
                        break;
                }

                RetVal = BaseQueryUtility.Get_Query();
            }
            catch (Exception ex)
            {
                RetVal = null;
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        public static XmlDocument Get_MetadataQuery(SDMXSchemaType schemaType, MetadataTypes metadataType, string codeId, string agencyId)
        {
            XmlDocument RetVal;
            MetadataQueryUtility MetadataQueryUtility;

            RetVal = null;
            MetadataQueryUtility = null;

            try
            {
                MetadataQueryUtility = new MetadataQueryUtility(metadataType, codeId, agencyId);
                RetVal = MetadataQueryUtility.Get_Query();
            }
            catch (Exception ex)
            {
                RetVal = null;
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        public static List<ArtefactInfo> Generate_Codelist(SDMXSchemaType schemaType, CodelistTypes codelistType, bool completeOrSummaryFlag, string agencyId, string language, Header header, string outputFolder, DIConnection DIConnection, DIQueries DIQueries)
        {
            List<ArtefactInfo> RetVal;
            CodelistUtility CodelistUtility;
            RetVal = null;
            CodelistUtility = null;

            try
            {
                if (language.Contains("_"))
                {
                    language = language.Replace("_", "");
                }
                CodelistUtility = new CodelistUtility(codelistType, completeOrSummaryFlag, agencyId, language, header, outputFolder, DIConnection, DIQueries);
                RetVal = CodelistUtility.Generate_Artefact();
                CodelistUtility.Save_Artefacts(RetVal);
            }
            catch (Exception ex)
            {
                RetVal = null;
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        public static List<ArtefactInfo> Generate_ConceptScheme(SDMXSchemaType schemaType, ConceptSchemeTypes conceptSchemeType, string agencyId, string language, Header header, string outputFolder, DIConnection DIConnection, DIQueries DIQueries)
        {
            List<ArtefactInfo> RetVal;
            ConceptSchemeUtility ConceptSchemeUtility;

            RetVal = null;
            ConceptSchemeUtility = null;

            try
            {
                ConceptSchemeUtility = new ConceptSchemeUtility(conceptSchemeType, agencyId, language, header, outputFolder, DIConnection, DIQueries);
                RetVal = ConceptSchemeUtility.Generate_Artefact();
                ConceptSchemeUtility.Save_Artefacts(RetVal);
            }
            catch (Exception ex)
            {
                RetVal = null;
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        public static List<ArtefactInfo> Generate_DSD(SDMXSchemaType schemaType, string agencyId, string language, Header header, string outputFolder, DIConnection DIConnection, DIQueries DIQueries)
        {
            List<ArtefactInfo> RetVal;
            DSDUtility DSDUtility;

            RetVal = null;
            DSDUtility = null;

            try
            {
                DSDUtility = new DSDUtility(agencyId, language, header, outputFolder, DIConnection, DIQueries);
                RetVal = DSDUtility.Generate_Artefact();
                DSDUtility.Save_Artefacts(RetVal);
            }
            catch (Exception ex)
            {
                RetVal = null;
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        public static List<ArtefactInfo> Generate_MSD(SDMXSchemaType schemaType, MSDTypes msdType, string agencyId, string language, Header header, string outputFolder, DIConnection DIConnection, DIQueries DIQueries)
        {
            List<ArtefactInfo> RetVal;
            MSDUtility MSDUtility;

            RetVal = null;
            MSDUtility = null;

            try
            {
                MSDUtility = new MSDUtility(msdType, agencyId, language, header, outputFolder, DIConnection, DIQueries);
                RetVal = MSDUtility.Generate_Artefact();
                MSDUtility.Save_Artefacts(RetVal);
            }
            catch (Exception ex)
            {
                RetVal = null;
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        public static List<ArtefactInfo> Generate_CategoryScheme(SDMXSchemaType schemaType, CategorySchemeTypes categorySchemeType, string agencyId, string language, Header header, string outputFolder, Dictionary<string, string> DictIndicator, Dictionary<string, string> DictIndicatorMapping, DIConnection DIConnection, DIQueries DIQueries)
        {
            List<ArtefactInfo> RetVal;
            CategorySchemeUtility CategorySchemeUtility;

            RetVal = null;
            CategorySchemeUtility = null;

            try
            {
                CategorySchemeUtility = new CategorySchemeUtility(categorySchemeType, true, agencyId, language, header, outputFolder, DictIndicator, DictIndicatorMapping, DIConnection, DIQueries);
                RetVal = CategorySchemeUtility.Generate_Artefact();
                CategorySchemeUtility.Save_Artefacts(RetVal);
            }
            catch (Exception ex)
            {
                RetVal = null;
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        public static List<ArtefactInfo> Generate_Categorization(SDMXSchemaType schemaType, string agencyId, string language, Header header, string outputFolder, DIConnection DIConnection, DIQueries DIQueries,string IcType)
        {
            List<ArtefactInfo> RetVal;
            CategorizationUtility CategorizationUtility;

            RetVal = null;
            CategorizationUtility = null;

            try
            {
                CategorizationUtility = new CategorizationUtility(agencyId, language, header, outputFolder, DIConnection, DIQueries);
               // RetVal = CategorizationUtility.Generate_Artefact();
                RetVal = CategorizationUtility.Generate_CompleteArtefact(IcType);
                CategorizationUtility.Save_Artefacts(RetVal);
            }
            catch (Exception ex)
            {
                RetVal = null;
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        public static List<ArtefactInfo> Generate_DFD(SDMXSchemaType schemaType, string dsdId, string agencyId, Header header, string outputFolder)
        {
            List<ArtefactInfo> RetVal;
            DFDUtility DFDUtility;

            RetVal = null;
            DFDUtility = null;

            try
            {
                DFDUtility = new DFDUtility(dsdId, agencyId, header, outputFolder);
                RetVal = DFDUtility.Generate_Artefact();
                DFDUtility.Save_Artefacts(RetVal);
            }
            catch (Exception ex)
            {
                RetVal = null;
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        public static List<ArtefactInfo> Generate_MFD(SDMXSchemaType schemaType, MSDTypes msdType, string agencyId, Header header, string outputFolder)
        {
            List<ArtefactInfo> RetVal;
            MFDUtility MFDUtility;

            RetVal = null;
            MFDUtility = null;

            try
            {
                MFDUtility = new MFDUtility(msdType, agencyId, header, outputFolder);
                RetVal = MFDUtility.Generate_Artefact();
                MFDUtility.Save_Artefacts(RetVal);
            }
            catch (Exception ex)
            {
                RetVal = null;
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        public static List<ArtefactInfo> Generate_Complete_XML(SDMXSchemaType schemaType, string agencyId, string language, Header header, string outputFolder, Dictionary<string, string> DictIndicator, Dictionary<string, string> DictIndicatorMapping, DIConnection DIConnection, DIQueries DIQueries,string IcType)//, List<ArtefactInfo> Artefacts
        {
            List<ArtefactInfo> RetVal;
            CompositeUtility CompositeUtility;

            RetVal = null;
            CompositeUtility = null;

            try
            {
                if (language.Contains("_"))
                {
                    language = language.Replace("_","");
                }
                CompositeUtility = new CompositeUtility(true, agencyId, language, header, outputFolder, DictIndicator, DictIndicatorMapping, DIConnection, DIQueries);
                 RetVal = CompositeUtility.Generate_Artefact(IcType);
               // RetVal = CompositeUtility.Generate_CompleteArtefact(Artefacts);
                CompositeUtility.Save_Artefacts(RetVal);
            }
            catch (Exception ex)
            {
                RetVal = null;
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        public static List<ArtefactInfo> Generate_Summary_XML(SDMXSchemaType schemaType, string agencyId, string language, Header header, string outputFolder, DIConnection DIConnection, DIQueries DIQueries)
        {
            List<ArtefactInfo> RetVal;
            CompositeUtility CompositeUtility;

            RetVal = null;
            CompositeUtility = null;

            try
            {
                if (language.Contains("_"))
                {
                    language = language.Replace("_", "");
                }

                CompositeUtility = new CompositeUtility(false, agencyId, language, header, outputFolder, null, null, DIConnection, DIQueries);
                RetVal = CompositeUtility.Generate_Artefact();
                CompositeUtility.Save_Artefacts(RetVal);
            }
            catch (Exception ex)
            {
                RetVal = null;
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        public static List<ArtefactInfo> Generate_Report(SDMXSchemaType schemaType, string completeXmlFileNameWPath, string agencyId, string language, string outputFolder)
        {
            List<ArtefactInfo> RetVal;
            ReportUtility ReportUtility;

            RetVal = null;
            ReportUtility = null;

            try
            {
                ReportUtility = new ReportUtility(completeXmlFileNameWPath, agencyId, language, outputFolder);
                RetVal = ReportUtility.Generate_Artefact();
            }
            catch (Exception ex)
            {
                RetVal = null;
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        public static List<ArtefactInfo> Generate_Header(string agencyId, string language, Header header, string outputFolder)
        {
            List<ArtefactInfo> RetVal;

            HeaderUtility ArtifactHeader;
            RetVal = null;
            ArtifactHeader = null;

            try
            {
                if (language.Contains("_"))
                {
                    language = language.Replace("_", "");
                }
                ArtifactHeader = new HeaderUtility(agencyId, language, header, outputFolder);
                RetVal = ArtifactHeader.Generate_Artefact();
               
                ArtifactHeader.Save_Artefacts(RetVal);
            }
            catch (Exception ex)
            {
                RetVal = null;
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        public static List<ArtefactInfo> Register_User(SDMXSchemaType schemaType, string originalFileNameWPath, UserTypes userType, string id, string name, string language, string outputFolder)
        {
            List<ArtefactInfo> RetVal;
            UsersUtility UsersUtility;

            RetVal = null;
            UsersUtility = null;

            try
            {
                UsersUtility = new UsersUtility(originalFileNameWPath, userType, id, name, language, outputFolder, true);
                RetVal = UsersUtility.Generate_Artefact();
                UsersUtility.Save_Artefacts(RetVal);
            }
            catch (Exception ex)
            {
                RetVal = null;
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        public static List<ArtefactInfo> UnRegister_User(SDMXSchemaType schemaType, string originalFileNameWPath, UserTypes userType, string id)
        {
            List<ArtefactInfo> RetVal;
            UsersUtility UsersUtility;

            RetVal = null;
            UsersUtility = null;

            try
            {
                UsersUtility = new UsersUtility(originalFileNameWPath, userType, id, string.Empty, string.Empty, string.Empty, false);
                RetVal = UsersUtility.Generate_Artefact();
                UsersUtility.Save_Artefacts(RetVal);
            }
            catch (Exception ex)
            {
                RetVal = null;
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        public static List<ArtefactInfo> Generate_PA(SDMXSchemaType schemaType, bool dfdMfdFlag, string dfdMfdId, string providerId, string agencyId, Header header, string outputFolder)
        {
            List<ArtefactInfo> RetVal;
            PAUtility PAUtility;

            RetVal = null;
            PAUtility = null;

            try
            {
                PAUtility = new PAUtility(dfdMfdFlag, dfdMfdId, providerId, agencyId, header, outputFolder);
                RetVal = PAUtility.Generate_Artefact();
                PAUtility.Save_Artefacts(RetVal);
            }
            catch (Exception ex)
            {
                RetVal = null;
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        public static List<ArtefactInfo> Register_DataRepository(SDMXSchemaType schemaType, string id, string paId, string dataURL, bool isREST, string wadlURL, bool isSOAP, string wsdlURL, string fileURL, string agencyId, Header header, string outputFolder)
        {
            List<ArtefactInfo> RetVal;
            RegistrationUtility RegistrationUtility;

            RetVal = null;
            RegistrationUtility = null;

            try
            {
                RegistrationUtility = new RegistrationUtility(id, paId, dataURL, isREST, wadlURL, isSOAP, wsdlURL, fileURL, agencyId, header, outputFolder);
                RetVal = RegistrationUtility.Generate_Artefact();
                RegistrationUtility.Save_Artefacts(RetVal);
            }
            catch (Exception ex)
            {
                RetVal = null;
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        public static List<ArtefactInfo> Create_Constraint(SDMXSchemaType schemaType, XmlDocument sdmxMLFileXMLDocument, string registrationId, string simpleDataSourceUrl, string agencyId, Header header, string outputFolder)
        {
            List<ArtefactInfo> RetVal;
            ConstraintUtility ConstraintUtility;

            RetVal = null;
            ConstraintUtility = null;

            try
            {
                ConstraintUtility = new ConstraintUtility(sdmxMLFileXMLDocument, registrationId, simpleDataSourceUrl, agencyId, header, outputFolder);
                RetVal = ConstraintUtility.Generate_Artefact();
                ConstraintUtility.Save_Artefacts(RetVal);
            }
            catch (Exception ex)
            {
                RetVal = null;
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        public static List<ArtefactInfo> Subscribe_User(SDMXSchemaType schemaType, string id, string userId, UserTypes userType, List<bool> isSOAPMailIds, List<string> notificationMailIds, List<bool> isSOAPHTTPs, List<string> notificationHTTPs, string subscriberAssignedId, DateTime startDate, DateTime endDate, string EventSelector, Dictionary<string, string> dictCategories, string MFDId, string agencyId, Header header, string outputFolder)
        {
            List<ArtefactInfo> RetVal;
            SubscriptionUtility SubscriptionUtility;

            RetVal = null;
            SubscriptionUtility = null;

            try
            {
                SubscriptionUtility = new SubscriptionUtility(id, userId, userType, isSOAPMailIds, notificationMailIds, isSOAPHTTPs, notificationHTTPs, subscriberAssignedId, startDate, endDate, EventSelector, dictCategories, MFDId, agencyId, header, outputFolder);
                RetVal = SubscriptionUtility.Generate_Artefact();
                SubscriptionUtility.Save_Artefacts(RetVal);
            }
            catch (Exception ex)
            {
                RetVal = null;
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        public static List<ArtefactInfo> Get_Notification(SDMXSchemaType schemaType, DateTime eventTime, string registrationId, string subscriptionURN, ActionType action, RegistrationType registration, Header header)
        {
            List<ArtefactInfo> RetVal;
            NotificationUtility NotificationUtility;

            RetVal = null;
            NotificationUtility = null;

            try
            {
                NotificationUtility = new NotificationUtility(eventTime, registrationId, subscriptionURN, action, registration, header);
                RetVal = NotificationUtility.Generate_Artefact();
            }
            catch (Exception ex)
            {
                RetVal = null;
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        public static List<ArtefactInfo> Get_Notification(SDMXSchemaType schemaType, DateTime eventTime, List<ArtefactRef> artefacts, string subscriptionURN, ActionType action, Header header)
        {
            List<ArtefactInfo> RetVal;
            NotificationUtility NotificationUtility;

            RetVal = null;
            NotificationUtility = null;

            try
            {
                NotificationUtility = new NotificationUtility(eventTime, artefacts, subscriptionURN, action, header);
                RetVal = NotificationUtility.Generate_Artefact();
            }
            catch (Exception ex)
            {
                RetVal = null;
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        public static List<ArtefactInfo> Generate_Mapping(SDMXSchemaType schemaType, MappingType mappingType, Dictionary<string, string> dictMapping, string sourceId, string sourceAgencyId, string sourceVersion, string targetId, string targetAgencyId, string targetVersion, string agencyId, string language, Header header, string fileNameWPath, string outputFolder,string codelistName)
        {
            List<ArtefactInfo> RetVal;
            MappingUtility MappingUtility;

            RetVal = null;
            MappingUtility = null;

            try
            {
                MappingUtility = new MappingUtility(mappingType, dictMapping, sourceId, sourceAgencyId, sourceVersion, targetId, targetAgencyId, targetVersion, agencyId, language, header, fileNameWPath, outputFolder,codelistName);
                RetVal = MappingUtility.Generate_Artefact();
                MappingUtility.Save_Artefacts(RetVal);
            }
            catch (Exception ex)
            {
                RetVal = null;
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        public static Dictionary<string, string> Validate_SDMXML(SDMXSchemaType schemaType, string dataFileNameWPath, string completeFileNameWPath)
        {
            Dictionary<string, string> RetVal;
            BaseValidateUtility BaseValidateUtility;

            RetVal = new Dictionary<string, string>();
            BaseValidateUtility = null;

            try
            {
                BaseValidateUtility = new SDMXMLValidateUtility();
                RetVal = BaseValidateUtility.ValidateSdmxMlAgainstDSD(dataFileNameWPath, completeFileNameWPath);
                if (RetVal.Keys.Count == 0)
                {
                    RetVal.Add(SDMXValidationStatus.Valid.ToString(), string.Empty);
                }
            }
            catch (Exception ex)
            {
                RetVal = new Dictionary<string, string>();
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        public static Dictionary<string, string> Validate_DSDAgainstMasterDSD(SDMXSchemaType schemaType, string dsdFileNameWPath, string devinfodsdFileNameWPath)
        {
            Dictionary<string, string> RetVal;
            BaseValidateUtility BaseValidateUtility;

            RetVal = new Dictionary<string, string>();
            BaseValidateUtility = null;

            try
            {
                BaseValidateUtility = new DSDValidateUtility();
                RetVal = BaseValidateUtility.ValidateDSDAgainstDevInfoDSD(dsdFileNameWPath, devinfodsdFileNameWPath);
                if (RetVal.Keys.Count == 0)
                {
                    RetVal.Add(DSDValidationStatus.Valid.ToString(), string.Empty);
                }
            }
            catch (Exception ex)
            {
                RetVal = new Dictionary<string, string>();
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        public static Dictionary<string, string> Validate_MetadataReport(SDMXSchemaType schemaType, string metadataFileNameWPath, string completeFileNameWPath, string mfd_Id)
        {
            Dictionary<string, string> RetVal;
            BaseValidateUtility BaseValidateUtility;

            RetVal = new Dictionary<string, string>();
            BaseValidateUtility = null;

            try
            {
                BaseValidateUtility = new MetadataValidateUtility();
                RetVal = BaseValidateUtility.ValidateMetadataReportAgainstMSD(metadataFileNameWPath, completeFileNameWPath, mfd_Id);
                if (RetVal.Keys.Count == 0)
                {
                    RetVal.Add(MetadataValidationStatus.Valid.ToString(), string.Empty);
                }
            }
            catch (Exception ex)
            {
                RetVal = new Dictionary<string, string>();
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        public static bool Validate_DSDFile(SDMXSchemaType schemaType, string dsdFileNameWPath)
        {
            bool Retval;
            Dictionary<string, string> dictStatus;
            BaseValidateUtility BaseValidateUtility;
            string ValidationStatus;

            dictStatus = new Dictionary<string, string>();
            BaseValidateUtility = null;
            Retval = true;
            ValidationStatus = string.Empty;

            try
            {
                BaseValidateUtility = new DSDValidateUtility();
                dictStatus = BaseValidateUtility.ValidateDSD(dsdFileNameWPath);
                foreach (string key in dictStatus.Keys)
                {
                    ValidationStatus = key;
                }
                if ((ValidationStatus == DSDValidationStatus.Valid.ToString()) || (dictStatus.Keys.Count == 0))
                {
                    Retval = true;
                }
                else
                {
                    Retval = false;
                }
            }
            catch (Exception ex)
            {
                Retval = false;
                throw ex;
            }
            finally
            {
            }

            return Retval;
        }

        public static bool Validate_SDMXMLFile(SDMXSchemaType schemaType, string dataFileNameWPath)
        {

            bool Retval;
            Dictionary<string, string> dictStatus;
            BaseValidateUtility BaseValidateUtility;
            string ValidationStatus;

            dictStatus = new Dictionary<string, string>();
            BaseValidateUtility = null;
            Retval = true;
            ValidationStatus = string.Empty;

            try
            {
                BaseValidateUtility = new SDMXMLValidateUtility();
                dictStatus = BaseValidateUtility.ValidateSdmxML(dataFileNameWPath);
                foreach (string key in dictStatus.Keys)
                {
                    ValidationStatus = key;
                }
                if ((ValidationStatus == SDMXValidationStatus.Valid.ToString()) || (dictStatus.Keys.Count == 0))
                {
                    Retval = true;
                }
                else
                {
                    Retval = false;
                }
            }
            catch (Exception ex)
            {
                Retval = false;
                throw ex;
            }
            finally
            {
            }

            return Retval;

        }
    }
}
