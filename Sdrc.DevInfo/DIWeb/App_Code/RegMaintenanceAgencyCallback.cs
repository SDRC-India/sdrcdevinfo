using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Configuration;
using System.IO;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using SpreadsheetGear;
using DevInfo.Lib.DI_LibSDMX;
using DevInfo.Lib.DI_LibDAL.Connection;
using System.Data;
using SDMXObjectModel.Message;
using SDMXObjectModel.Structure;
using SDMXObjectModel.Common;
using DI7.Lib.BAL.HTMLGenerator;
using DevInfo.Lib.DI_LibDAL.Queries;
using SDMXObjectModel.Registry;
using SDMXObjectModel;

/// <summary>
/// Summary description for RegUploadDSDCallback
/// </summary>
public partial class Callback : System.Web.UI.Page
{
    #region "--Methods--"

    #region "--Private--"

    #region "--SDMX Artefacts Generation For Database--"

    private string GenerateSDMXArtefacts(int DbNid, string UseNId)
    {
        string RetVal = "false";
        string CategoryFile = string.Empty;
        string SDMXMLFiles = string.Empty;
        string SDMXMLDirectories = string.Empty;
        DIConnection DIConnection;
        DIQueries DIQueries;
        string OutputFolder, Language, AgencyId, AppSettingFile;
        List<ArtefactInfo> Artefacts;
        XmlDocument XmlDoc;
        AppSettingFile = string.Empty;

        DIConnection = Global.GetDbConnection(DbNid);
        DIQueries = new DIQueries(DIConnection.DIDataSetDefault(), DIConnection.DILanguageCodeDefault(DIConnection.DIDataSetDefault()));
        OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNid.ToString() + "\\sdmx");
        Language = DIConnection.DILanguageCodeDefault(DIConnection.DIDataSetDefault()).Substring(1);
        AgencyId = DevInfo.Lib.DI_LibSDMX.Constants.MaintenanceAgencyScheme.Prefix + UseNId;


        try
        {
            //Check for presence of AppSetting key Category Scheme
            //AppSettingFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.AppSettingFile]);
            //XmlDoc = new XmlDocument();
            //XmlDoc.Load(AppSettingFile);
            //Global.CheckAppSetting(XmlDoc, Constants.AppSettingKeys.CategoryScheme, "SC");
            //SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.IsSDMXHeaderCreated, "true");
            //XmlDoc.Save(AppSettingFile);
            //Global.GetAppSetting();
            //Check if  header file exist if not allow  deletion  

            Global.SetDBXmlAttributes(Convert.ToString(DbNid), Constants.XmlFile.Db.Tags.DatabaseAttributes.IsSDMXHeaderCreated, "true");
            if (!File.Exists(OutputFolder + "/" + DevInfo.Lib.DI_LibSDMX.Constants.Header.FileName))
            {
                this.Clean_SDMX_Folder_Structure(DbNid);

                this.Create_SDMX_Folder_Structure(DbNid);
            }
            //HeaderFilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNid.ToString() + "\\" + Constants.FolderName.SDMX.sdmx + DevInfo.Lib.DI_LibSDMX.Constants.Header.FileName);
            //XmlDocument UploadedHeaderXml = new XmlDocument();
            //SDMXObjectModel.Message.StructureType UploadedDSDStructure = new SDMXObjectModel.Message.StructureType();
            //SDMXObjectModel.Message.StructureHeaderType Header = new SDMXObjectModel.Message.StructureHeaderType();
            //SDMXObjectModel.Message.BasicHeaderType BHeader = new SDMXObjectModel.Message.BasicHeaderType();
            //if (File.Exists(HeaderFilePath))
            //{
            //    UploadedHeaderXml.Load(HeaderFilePath);
            //    UploadedDSDStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureType), UploadedHeaderXml);
            //    Header = UploadedDSDStructure.Header;

            //}



            Artefacts = SDMXUtility.Generate_ConceptScheme(SDMXSchemaType.Two_One, ConceptSchemeTypes.ALL, AgencyId, Language, null, Path.Combine(OutputFolder, "Concepts"), DIConnection, DIQueries);

            Artefacts.AddRange(SDMXUtility.Generate_DSD(SDMXSchemaType.Two_One, AgencyId, Language, null, OutputFolder, DIConnection, DIQueries));
            this.Set_DSD_Name(Artefacts[Artefacts.Count - 1], DbNid, Path.Combine(OutputFolder, DevInfo.Lib.DI_LibSDMX.Constants.DSD.FileName));

            Artefacts.AddRange(SDMXUtility.Generate_MSD(SDMXSchemaType.Two_One, MSDTypes.ALL, AgencyId, Language, null, Path.Combine(OutputFolder, "MSD"), DIConnection, DIQueries));

            Artefacts.AddRange(SDMXUtility.Generate_Codelist(SDMXSchemaType.Two_One, CodelistTypes.ALL, true, AgencyId, Language, null, Path.Combine(OutputFolder, "Codelists"), DIConnection, DIQueries));

            Artefacts.AddRange(SDMXUtility.Generate_CategoryScheme(SDMXSchemaType.Two_One, CategorySchemeTypes.ALL, AgencyId, Language, null, Path.Combine(OutputFolder, "Categories"), null, null, DIConnection, DIQueries));

            //  Artefacts.AddRange(SDMXUtility.Generate_Categorization(SDMXSchemaType.Two_One, AgencyId, Language, null, Path.Combine(OutputFolder, "Categorisations"), DIConnection, DIQueries,Global.CategoryScheme));

            Artefacts.AddRange(SDMXUtility.Generate_DFD(SDMXSchemaType.Two_One, DevInfo.Lib.DI_LibSDMX.Constants.DSD.Id, AgencyId, null, Path.Combine(OutputFolder, "Provisioning Metadata")));

            Artefacts.AddRange(SDMXUtility.Generate_MFD(SDMXSchemaType.Two_One, MSDTypes.ALL, AgencyId, null, Path.Combine(OutputFolder, "Provisioning Metadata")));

            Artefacts.AddRange(SDMXUtility.Generate_Complete_XML(SDMXSchemaType.Two_One, AgencyId, Language, null, OutputFolder, null, null, DIConnection, DIQueries, Global.CategoryScheme));//, Artefacts
            this.Set_DSD_Name(Artefacts[Artefacts.Count - 1], DbNid, Path.Combine(OutputFolder, DevInfo.Lib.DI_LibSDMX.Constants.Complete_XML.FileName));

            Artefacts.AddRange(SDMXUtility.Generate_Summary_XML(SDMXSchemaType.Two_One, AgencyId, Language, null, OutputFolder, DIConnection, DIQueries));
            this.Set_DSD_Name(Artefacts[Artefacts.Count - 1], DbNid, Path.Combine(OutputFolder, DevInfo.Lib.DI_LibSDMX.Constants.Summary_XML.FileName));

            Artefacts.AddRange(SDMXUtility.Generate_Report(SDMXSchemaType.Two_One, Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNid.ToString() + "\\sdmx\\" + DevInfo.Lib.DI_LibSDMX.Constants.Complete_XML.FileName), AgencyId, Language, OutputFolder));

            Artefacts.AddRange(this.Create_PAs_For_Database_Per_Provider(DbNid.ToString(), AgencyId, Path.Combine(OutputFolder, "Provisioning Metadata\\PAs"), Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users + DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.FileName), false));

            Artefacts.AddRange(SDMXUtility.Generate_Categorization(SDMXSchemaType.Two_One, AgencyId, Language, null, Path.Combine(OutputFolder, "Categorisations"), DIConnection, DIQueries, Global.CategoryScheme));
            if (!File.Exists(OutputFolder + "/" + DevInfo.Lib.DI_LibSDMX.Constants.Header.FileName))
            {
                Artefacts.AddRange(SDMXUtility.Generate_Header(string.Empty, string.Empty, null, OutputFolder));
            }
            this.Delete_SDMXArtefacts_Details_In_Database(DbNid.ToString(), UseNId);

            this.Save_Artefacts_Details_In_Database(Artefacts, DbNid);

            this.Send_Notifications_For_Subscriptions_For_Structural_Metadata_Changes(DbNid.ToString());

            RetVal = "true";
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog(ex.StackTrace);
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;
    }

    private void Clean_SDMX_Folder_Structure(int DbNid)
    {
        string FolderName;
        string AppPhysicalPath, DbFolder;

        AppPhysicalPath = string.Empty;
        DbFolder = string.Empty;

        try
        {
            AppPhysicalPath = HttpContext.Current.Request.PhysicalApplicationPath;
            DbFolder = Constants.FolderName.Data + DbNid.ToString() + "\\";
            FolderName = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.sdmx);

            if (Directory.Exists(FolderName))
            {
                foreach (String Folder in Directory.GetDirectories(FolderName))
                {
                    if (Folder.Contains("Subscriptions") || Folder.Contains("Registrations") || Folder.Contains("Constraints"))
                    {
                        continue;
                    }
                    else
                    {
                        if (Directory.GetDirectories(Folder).Length > 0)
                        {
                            foreach (String SubFolder in Directory.GetDirectories(Folder))
                            {
                                Global.DeleteDirectory(SubFolder);
                            }
                        }
                        else
                        {
                            Global.DeleteDirectory(Folder);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
        }
    }

    private void Create_SDMX_Folder_Structure(int DbNid)
    {
        string FolderName;
        string AppPhysicalPath, DbFolder, language, ConsumerFileName, ProviderFileName;
        SDMXObjectModel.Message.StructureType Structure;
        DIConnection DIConnection;
        DIQueries DIQueries;

        AppPhysicalPath = string.Empty;
        DbFolder = string.Empty;
        language = string.Empty;
        ConsumerFileName = string.Empty;
        ProviderFileName = string.Empty;
        Structure = null;
        DIConnection = null;
        DIQueries = null;

        try
        {
            AppPhysicalPath = HttpContext.Current.Request.PhysicalApplicationPath;
            DbFolder = Constants.FolderName.Data + DbNid.ToString() + "\\";

            FolderName = Path.Combine(AppPhysicalPath, Constants.FolderName.Users);
            this.Create_Directory_If_Not_Exists(FolderName);

            FolderName = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.sdmx);
            this.Create_Directory_If_Not_Exists(FolderName);

            FolderName = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.Categories);
            this.Create_Directory_If_Not_Exists(FolderName);

            FolderName = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.Categorisations);
            this.Create_Directory_If_Not_Exists(FolderName);

            FolderName = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.Codelists);
            this.Create_Directory_If_Not_Exists(FolderName);

            FolderName = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.Concepts);
            this.Create_Directory_If_Not_Exists(FolderName);

            FolderName = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.Metadata);
            this.Create_Directory_If_Not_Exists(FolderName);

            FolderName = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.AreaMetadata);
            this.Create_Directory_If_Not_Exists(FolderName);

            FolderName = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.IndicatorMetadata);
            this.Create_Directory_If_Not_Exists(FolderName);

            FolderName = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.SourceMetadata);
            this.Create_Directory_If_Not_Exists(FolderName);

            FolderName = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.MSD);
            this.Create_Directory_If_Not_Exists(FolderName);

            FolderName = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.ProvisioningMetadata);
            this.Create_Directory_If_Not_Exists(FolderName);

            FolderName = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.Constraints);
            this.Create_Directory_If_Not_Exists(FolderName);

            FolderName = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.PAs);
            this.Create_Directory_If_Not_Exists(FolderName);

            FolderName = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.Registrations);
            this.Create_Directory_If_Not_Exists(FolderName);

            FolderName = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.Subscriptions);
            this.Create_Directory_If_Not_Exists(FolderName);

            FolderName = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.SDMX_ML);
            this.Create_Directory_If_Not_Exists(FolderName);

            DIConnection = Global.GetDbConnection(DbNid);
            DIQueries = new DIQueries(DIConnection.DIDataSetDefault(), DIConnection.DILanguageCodeDefault(DIConnection.DIDataSetDefault()));

            foreach (DataRow LanguageRow in DIConnection.DILanguages(DIQueries.DataPrefix).Rows)
            {
                language = LanguageRow[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Language.LanguageCode].ToString();

                FolderName = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.SDMX_ML + "\\" + language);
                this.Create_Directory_If_Not_Exists(FolderName);
            }

            ConsumerFileName = Path.Combine(AppPhysicalPath, Constants.FolderName.Users + DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.FileName);

            if (File.Exists(ConsumerFileName))
            {
                Structure = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), ConsumerFileName);

                if (Structure != null && Structure.Structures != null && Structure.Structures.OrganisationSchemes != null && Structure.Structures.OrganisationSchemes.Count > 0 && Structure.Structures.OrganisationSchemes[0] is SDMXObjectModel.Structure.DataConsumerSchemeType &&
                    Structure.Structures.OrganisationSchemes[0].Organisation != null && Structure.Structures.OrganisationSchemes[0].Organisation.Count > 0)
                {
                    foreach (DataConsumerType DataConsumer in Structure.Structures.OrganisationSchemes[0].Organisation)
                    {
                        FolderName = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.Subscriptions + DataConsumer.id.Replace(DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.Prefix, string.Empty));
                        this.Create_Directory_If_Not_Exists(FolderName);
                    }
                }
            }

            ProviderFileName = Path.Combine(AppPhysicalPath, Constants.FolderName.Users + DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.FileName);

            if (File.Exists(ProviderFileName))
            {
                Structure = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), ProviderFileName);

                if (Structure != null && Structure.Structures != null && Structure.Structures.OrganisationSchemes != null && Structure.Structures.OrganisationSchemes.Count > 0 && Structure.Structures.OrganisationSchemes[0] is SDMXObjectModel.Structure.DataProviderSchemeType &&
                    Structure.Structures.OrganisationSchemes[0].Organisation != null && Structure.Structures.OrganisationSchemes[0].Organisation.Count > 0)
                {
                    foreach (DataProviderType DataProvider in Structure.Structures.OrganisationSchemes[0].Organisation)
                    {
                        FolderName = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.Subscriptions + DataProvider.id.Replace(DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix, string.Empty));
                        this.Create_Directory_If_Not_Exists(FolderName);

                        FolderName = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.Registrations + DataProvider.id.Replace(DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix, string.Empty));
                        this.Create_Directory_If_Not_Exists(FolderName);

                        FolderName = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.Constraints + DataProvider.id.Replace(DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix, string.Empty));
                        this.Create_Directory_If_Not_Exists(FolderName);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }
    }

    private void Set_DSD_Name(ArtefactInfo Artefact, int DbNId, string FileNameWPath)
    {
        SDMXObjectModel.Message.StructureType Structure;

        Structure = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureType), Artefact.Content);

        if (Structure.Structures != null && Structure.Structures.DataStructures != null && Structure.Structures.DataStructures.Count > 0)
        {
            if (Structure.Structures.DataStructures[0].Name != null && Structure.Structures.DataStructures[0].Name.Count > 0)
            {
                foreach (TextType Name in Structure.Structures.DataStructures[0].Name)
                {
                    Name.Value = Global.GetDbConnectionDetails(DbNId.ToString())[0];
                }
            }
        }

        this.Remove_Extra_Annotations(Structure);
        this.Remove_Reduntant_Artefact_Tags(Structure);

        Serializer.SerializeToFile(typeof(SDMXObjectModel.Message.StructureType), Structure, FileNameWPath);
    }

    private void Remove_Extra_Annotations(SDMXObjectModel.Message.StructureType Structure)
    {
        if (Structure.Structures.Codelists != null)
        {
            foreach (CodelistType Codelist in Structure.Structures.Codelists)
            {
                Codelist.Annotations = null;
            }
        }

        if (Structure.Structures.CategorySchemes != null)
        {
            foreach (CategorySchemeType CategoryScheme in Structure.Structures.CategorySchemes)
            {
                CategoryScheme.Annotations = null;

                foreach (CategoryType Category in CategoryScheme.Items)
                {
                    Category.Annotations = null;
                    this.Remove_Extra_Annotations_From_Child_Categories(Category);
                }
            }
        }

        if (Structure.Structures.Categorisations != null)
        {
            foreach (CategorisationType Categorisation in Structure.Structures.Categorisations)
            {
                Categorisation.Annotations = null;

                ((ObjectRefType)Categorisation.Source.Items[0]).version = null;
            }
        }

        // Removing annotation at ConceptScheme level
        if (Structure.Structures.Concepts != null)
        {
            foreach (ConceptSchemeType ConceptScheme in Structure.Structures.Concepts)
            {
                ConceptScheme.Annotations = null;

                foreach (ConceptType Concept in ConceptScheme.Items)
                {
                    Concept.Annotations = null;
                }
            }
        }

        // Removing annotation at DataStructure level
        if (Structure.Structures.DataStructures != null)
        {
            if (Structure.Structures.DataStructures.Count > 0)
            {
                Structure.Structures.DataStructures[0].Annotations = null;

                // Removing annotation at Dimensionlist level
                ((DataStructureComponentsType)Structure.Structures.DataStructures[0].Item).DimensionList = null;
                Structure.Structures.DataStructures[0].Item.Items[0].Annotations = null;
                foreach (BaseDimensionType BaseDimension in Structure.Structures.DataStructures[0].Item.Items[0].Items)
                {
                    BaseDimension.Annotations = null;
                }

                // Removing annotation at Attributelist level
                ((DataStructureComponentsType)Structure.Structures.DataStructures[0].Item).AttributeList = null;
                Structure.Structures.DataStructures[0].Item.Items[1].Annotations = null;
                foreach (AttributeType Attribute in Structure.Structures.DataStructures[0].Item.Items[1].Items)
                {
                    Attribute.Annotations = null;
                }

                // Removing annotation at Measurelist level
                ((DataStructureComponentsType)Structure.Structures.DataStructures[0].Item).MeasureList = null;
                Structure.Structures.DataStructures[0].Item.Items[2].Annotations = null;
                foreach (PrimaryMeasureType PrimaryMeasure in Structure.Structures.DataStructures[0].Item.Items[2].Items)
                {
                    PrimaryMeasure.Annotations = null;
                }
            }
        }

        // Removing annotation at MetadataStructure level
        if (Structure.Structures.MetadataStructures != null)
        {
            foreach (SDMXObjectModel.Structure.MetadataStructureType MetadataStructure in Structure.Structures.MetadataStructures)
            {
                MetadataStructure.Annotations = null;

                // Removing annotation at MetadataTarget level
                MetadataStructure.Item.Items[0].Annotations = null;

                // Removing annotation at IdentifiableObjectTarget level
                MetadataStructure.Item.Items[0].Items[0].Annotations = null;

                // Removing annotation at ReportStructure level
                MetadataStructure.Item.Items[1].Annotations = null;

                // Removing annotation at MetadataAttribute level
                foreach (MetadataAttributeType MetadataAttribute in MetadataStructure.Item.Items[1].Items)
                {
                    MetadataAttribute.Annotations = null;
                }
            }
        }

        // Removing annotation at DataFlow level
        if (Structure.Structures.Dataflows != null)
        {
            foreach (DataflowType DataFlow in Structure.Structures.Dataflows)
            {
                DataFlow.Annotations = null;
            }
        }

        // Removing annotation at MetadataFlow level
        if (Structure.Structures.Metadataflows != null)
        {
            foreach (MetadataflowType MetadataFlow in Structure.Structures.Metadataflows)
            {
                MetadataFlow.Annotations = null;
            }
        }
    }

    private void Remove_Extra_Annotations_From_Child_Categories(CategoryType ParentCategory)
    {
        foreach (CategoryType ChildCategory in ParentCategory.Items)
        {
            ChildCategory.Annotations = null;
            this.Remove_Extra_Annotations_From_Child_Categories(ChildCategory);
        }
    }

    private void Remove_Reduntant_Artefact_Tags(SDMXObjectModel.Message.StructureType Structure)
    {
        if (Structure.Structures.Categorisations != null && Structure.Structures.Categorisations.Count == 0)
        {
            Structure.Structures.Categorisations = null;
        }

        if (Structure.Structures.CategorySchemes != null && Structure.Structures.CategorySchemes.Count == 0)
        {
            Structure.Structures.CategorySchemes = null;
        }

        if (Structure.Structures.Codelists != null && Structure.Structures.Codelists.Count == 0)
        {
            Structure.Structures.Codelists = null;
        }

        if (Structure.Structures.Concepts != null && Structure.Structures.Concepts.Count == 0)
        {
            Structure.Structures.Concepts = null;
        }

        if (Structure.Structures.Constraints != null && Structure.Structures.Constraints.Count == 0)
        {
            Structure.Structures.Constraints = null;
        }

        if (Structure.Structures.Dataflows != null && Structure.Structures.Dataflows.Count == 0)
        {
            Structure.Structures.Dataflows = null;
        }

        if (Structure.Structures.DataStructures != null && Structure.Structures.DataStructures.Count == 0)
        {
            Structure.Structures.DataStructures = null;
        }

        if (Structure.Structures.HierarchicalCodelists != null && Structure.Structures.HierarchicalCodelists.Count == 0)
        {
            Structure.Structures.HierarchicalCodelists = null;
        }

        if (Structure.Structures.Metadataflows != null && Structure.Structures.Metadataflows.Count == 0)
        {
            Structure.Structures.Metadataflows = null;
        }

        if (Structure.Structures.MetadataStructures != null && Structure.Structures.MetadataStructures.Count == 0)
        {
            Structure.Structures.MetadataStructures = null;
        }

        if (Structure.Structures.OrganisationSchemes != null && Structure.Structures.OrganisationSchemes.Count == 0)
        {
            Structure.Structures.OrganisationSchemes = null;
        }

        if (Structure.Structures.Processes != null && Structure.Structures.Processes.Count == 0)
        {
            Structure.Structures.Processes = null;
        }

        if (Structure.Structures.ProvisionAgreements != null && Structure.Structures.ProvisionAgreements.Count == 0)
        {
            Structure.Structures.ProvisionAgreements = null;
        }

        if (Structure.Structures.ReportingTaxonomies != null && Structure.Structures.ReportingTaxonomies.Count == 0)
        {
            Structure.Structures.ReportingTaxonomies = null;
        }

        if (Structure.Structures.StructureSets != null && Structure.Structures.StructureSets.Count == 0)
        {
            Structure.Structures.StructureSets = null;
        }

        Structure.Footer = null;
    }

    private List<ArtefactInfo> Create_PAs_For_Database_Per_Provider(string DbNId, string AgencyId, string OutputFolder, string ProviderFileName, bool UploadedDSDFlag)
    {
        List<ArtefactInfo> RetVal;
        DataTable DtMFDs;
        DIConnection DIConnection;
        SDMXObjectModel.Message.StructureType Structure;
        string ProviderId;
        string MFDId;
        string Query;

        RetVal = new List<ArtefactInfo>();
        DIConnection = null;
        Structure = null;
        ProviderId = string.Empty;
        MFDId = string.Empty;
        Query = string.Empty;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                          string.Empty, string.Empty);

            Query = "SELECT * FROM Artefacts WHERE Type=" + Convert.ToInt32(ArtefactTypes.MFD).ToString() + " And DbNId = " + DbNId + ";";
            DtMFDs = DIConnection.ExecuteDataTable(Query);

            if (File.Exists(ProviderFileName))
            {
                Structure = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), ProviderFileName);

                if (Structure != null && Structure.Structures != null && Structure.Structures.OrganisationSchemes != null && Structure.Structures.OrganisationSchemes.Count > 0 && Structure.Structures.OrganisationSchemes[0] is SDMXObjectModel.Structure.DataProviderSchemeType &&
                    Structure.Structures.OrganisationSchemes[0].Organisation != null && Structure.Structures.OrganisationSchemes[0].Organisation.Count > 0)
                {
                    foreach (DataProviderType DataProvider in Structure.Structures.OrganisationSchemes[0].Organisation)
                    {
                        ProviderId = DataProvider.id;
                        RetVal.AddRange(SDMXUtility.Generate_PA(SDMXSchemaType.Two_One, true, DevInfo.Lib.DI_LibSDMX.Constants.DFD.Id, ProviderId, AgencyId, null, OutputFolder));

                        if (UploadedDSDFlag == false)
                        {
                            RetVal.AddRange(SDMXUtility.Generate_PA(SDMXSchemaType.Two_One, false, DevInfo.Lib.DI_LibSDMX.Constants.MFD.Area.Id, ProviderId, AgencyId, null, OutputFolder));
                            RetVal.AddRange(SDMXUtility.Generate_PA(SDMXSchemaType.Two_One, false, DevInfo.Lib.DI_LibSDMX.Constants.MFD.Indicator.Id, ProviderId, AgencyId, null, OutputFolder));
                            RetVal.AddRange(SDMXUtility.Generate_PA(SDMXSchemaType.Two_One, false, DevInfo.Lib.DI_LibSDMX.Constants.MFD.Source.Id, ProviderId, AgencyId, null, OutputFolder));
                        }
                        else
                        {
                            foreach (DataRow DrMFDs in DtMFDs.Rows)
                            {
                                MFDId = DrMFDs["Id"].ToString();
                                RetVal.AddRange(SDMXUtility.Generate_PA(SDMXSchemaType.Two_One, false, MFDId, ProviderId, AgencyId, null, OutputFolder));
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;
    }

    private void Delete_SDMXArtefacts_Details_In_Database(string DbNId, string AdminNId)
    {
        string DeleteQuery;
        DIConnection DIConnection;
        DataTable DtTable;
        string RegistrationFolder = string.Empty;
        DeleteQuery = string.Empty;
        DIConnection = null;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                           string.Empty, string.Empty);

            DtTable = DIConnection.ExecuteDataTable("SELECT * FROM Artefacts WHERE DBNId = " + DbNId + " AND Type = " + Convert.ToInt32(ArtefactTypes.Registration).ToString() + ";");
            foreach (DataRow DrTable in DtTable.Rows)
            {
                if (DrTable["FileLocation"].ToString().Contains(@"\" + DbNId + @"\" + Constants.FolderName.SDMX.Registrations + AdminNId + @"\"))
                {
                    Global.Delete_Registration_Artefact(DbNId, AdminNId, DrTable["Id"].ToString());
                    Global.Delete_Constraint_Artefact(DbNId, AdminNId, DrTable["Id"].ToString());
                }
            }

            DeleteQuery = "DELETE FROM Artefacts WHERE DBNId = " + DbNId + " AND (Type NOT IN (" + Convert.ToInt32(ArtefactTypes.Subscription).ToString() + "," + Convert.ToInt32(ArtefactTypes.Registration).ToString() + "," + Convert.ToInt32(ArtefactTypes.Constraint).ToString() + "," + Convert.ToInt32(ArtefactTypes.Mapping).ToString() + "));";
            DIConnection.ExecuteDataTable(DeleteQuery);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }
    }

    private void Save_Artefacts_Details_In_Database(List<ArtefactInfo> Artefacts, int DbNid)
    {
        string InsertQuery;
        DIConnection DIConnection;
        System.Data.Common.DbParameter DbParam;
        List<System.Data.Common.DbParameter> DbParams;

        InsertQuery = string.Empty;
        DIConnection = null;

        string AppPhysicalPath = string.Empty;
        string DbFolder = string.Empty;

        try
        {
            AppPhysicalPath = HttpContext.Current.Request.PhysicalApplicationPath;
            DbFolder = Constants.FolderName.Data + DbNid.ToString() + "\\";

            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                           string.Empty, string.Empty);
            DbParams = new List<System.Data.Common.DbParameter>();

            foreach (ArtefactInfo Artefact in Artefacts)
            {
                InsertQuery = "INSERT INTO Artefacts (DBNId, Id, AgencyId, Version, URN, Type, FileLocation)" +
                              " VALUES(@DBNId, @Id, @AgencyId, @Version, @URN, @Type, @FileLocation);";

                DbParams = new List<System.Data.Common.DbParameter>();


                DbParam = DIConnection.CreateDBParameter();
                DbParam.ParameterName = "DBNId";
                DbParam.DbType = DbType.Int32;
                DbParam.Value = DbNid;

                DbParams.Add(DbParam);

                DbParam = DIConnection.CreateDBParameter();
                DbParam.ParameterName = "Id";
                DbParam.DbType = DbType.String;
                DbParam.Value = Artefact.Id;
                DbParams.Add(DbParam);

                DbParam = DIConnection.CreateDBParameter();
                DbParam.ParameterName = "AgencyId";
                DbParam.DbType = DbType.String;
                DbParam.Value = Artefact.AgencyId;
                DbParams.Add(DbParam);

                DbParam = DIConnection.CreateDBParameter();
                DbParam.ParameterName = "Version";
                DbParam.DbType = DbType.String;
                DbParam.Value = Artefact.Version;
                DbParams.Add(DbParam);

                DbParam = DIConnection.CreateDBParameter();
                DbParam.ParameterName = "URN";
                DbParam.DbType = DbType.String;
                DbParam.Value = Artefact.URN;
                DbParams.Add(DbParam);

                DbParam = DIConnection.CreateDBParameter();
                DbParam.ParameterName = "Type";
                DbParam.DbType = DbType.Int32;
                DbParam.Value = Convert.ToInt32(Artefact.Type);
                DbParams.Add(DbParam);

                DbParam = DIConnection.CreateDBParameter();
                DbParam.ParameterName = "FileLocation";
                DbParam.DbType = DbType.String;

                switch (Artefact.Type)
                {
                    case ArtefactTypes.ConceptS:
                        DbParam.Value = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.Concepts + Artefact.FileName);
                        break;
                    case ArtefactTypes.DSD:
                        DbParam.Value = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.sdmx + Artefact.FileName);
                        break;
                    case ArtefactTypes.CL:
                        DbParam.Value = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.Codelists + Artefact.FileName);
                        break;
                    case ArtefactTypes.CategoryS:
                        DbParam.Value = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.Categories + Artefact.FileName);
                        break;
                    //case ArtefactTypes.Categorisation:
                    //    DbParam.Value = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.Categorisations + Artefact.FileName);
                    //    break;
                    case ArtefactTypes.DFD:
                        DbParam.Value = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.ProvisioningMetadata + Artefact.FileName);
                        break;
                    case ArtefactTypes.Complete:
                        DbParam.Value = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.sdmx + Artefact.FileName);
                        break;
                    case ArtefactTypes.Summary:
                        DbParam.Value = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.sdmx + Artefact.FileName);
                        break;
                    case ArtefactTypes.Report:
                        DbParam.Value = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.sdmx + Artefact.FileName);
                        break;
                    case ArtefactTypes.PA:
                        DbParam.Value = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.PAs + Artefact.FileName);
                        break;
                    case ArtefactTypes.MSD:
                        DbParam.Value = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.MSD + Artefact.FileName);
                        break;
                    case ArtefactTypes.MFD:
                        DbParam.Value = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.ProvisioningMetadata + Artefact.FileName);
                        break;
                    case ArtefactTypes.Categorisation:
                        DbParam.Value = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.Categorisations + Artefact.FileName);
                        break;
                    case ArtefactTypes.Header:
                        DbParam.Value = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.sdmx + Artefact.FileName);
                        break;
                    default:
                        break;
                }

                DbParams.Add(DbParam);

                DIConnection.ExecuteDataTable(InsertQuery, CommandType.Text, DbParams);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }
    }
    /// <summary>
    /// Todo: try catch block for both complete
    /// </summary>
    /// <param name="Header"></param>
    /// <param name="DbNId"></param>
    /// <param name="UserNId"></param>
    /// <param name="sdmxFolderPath"></param>
    /// <returns></returns>
    private bool UpdateCompleteWithHeaderForDB(SDMXObjectModel.Message.StructureHeaderType Header, string DbNId, string UserNId, string sdmxFolderPath)
    {
        bool RetVal = false;
        SDMXObjectModel.Message.StructureType Complete;

        string completePath = sdmxFolderPath + "\\" + DevInfo.Lib.DI_LibSDMX.Constants.Complete_XML.FileName;
        Complete = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), completePath);
        Complete.Header = Header;


        Complete.Structures.HierarchicalCodelists = null;
        Complete.Structures.ProvisionAgreements = null;
        Complete.Structures.ReportingTaxonomies = null;
        Complete.Structures.StructureSets = null;
        Complete.Structures.OrganisationSchemes = null;
        Complete.Structures.Constraints = null;
        Complete.Structures.Processes = null;
        Complete.Footer = null;

        Remove_Extra_Annotations(Complete);
        SDMXObjectModel.Serializer.SerializeToFile(typeof(SDMXObjectModel.Message.StructureType), Complete, sdmxFolderPath + "\\" + DevInfo.Lib.DI_LibSDMX.Constants.Complete_XML.FileName);
        RetVal = true;
        return RetVal;
    }

    private bool UpdateCodelistsForDBWithHeader(SDMXObjectModel.Message.StructureHeaderType Header, string DbNId, string UserNId, string CodelistsFolderPath)
    {
        bool RetVal;
        SDMXObjectModel.Message.StructureType CodelistStructure;
        RetVal = true;
        try
        {

            string[] Files = Directory.GetFiles(CodelistsFolderPath);
            foreach (string codilistFile in Files)
            {
                CodelistStructure = new SDMXObjectModel.Message.StructureType();
                CodelistStructure = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), codilistFile);
                CodelistStructure.Header = Header;

                CodelistStructure.Structures.Categorisations = null;
                CodelistStructure.Structures.OrganisationSchemes = null;
                CodelistStructure.Structures.HierarchicalCodelists = null;
                CodelistStructure.Structures.Concepts = null;
                CodelistStructure.Structures.Metadataflows = null;
                CodelistStructure.Structures.MetadataStructures = null;
                CodelistStructure.Structures.Processes = null;
                CodelistStructure.Structures.ReportingTaxonomies = null;
                CodelistStructure.Structures.StructureSets = null;
                CodelistStructure.Structures.CategorySchemes = null;
                CodelistStructure.Structures.Constraints = null;
                CodelistStructure.Structures.Dataflows = null;
                CodelistStructure.Footer = null;
                Remove_Extra_Annotations(CodelistStructure);
                SDMXObjectModel.Serializer.SerializeToFile(typeof(SDMXObjectModel.Message.StructureType), CodelistStructure, codilistFile);
            }



        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("Creating Codelists For Uploaded DSD from Admin");
            //Global.WriteErrorsInLog(ex.StackTrace);
            //Global.WriteErrorsInLog(ex.Message);
            RetVal = false;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {

        }

        return RetVal;
    }

    private bool UpdateCategorySchemeForDBWithHeader(string DbNId, string UserNId, string DataBaseNId)
    {
        bool RetVal;
        SDMXObjectModel.Message.StructureType CategorySchemeStructure;
        SDMXObjectModel.Message.StructureType ProvisioningMetadata;
        SDMXObjectModel.Message.StructureType ProvisionAgreement;
        RetVal = true;
        string UploadedHeaderFileWPath = string.Empty;
        string UploadedHeaderFolderPath = Server.MapPath("../../stock/data");
        string UploadedHeaderName = string.Empty;
        string CategorySchemeFolderPath = string.Empty;
        string ProvisonMetadataFolderPath = string.Empty;
        string ProvisionAgreementFolderPath = string.Empty;
        XmlDocument UploadedHeaderXml = new XmlDocument();
        string[] Files = null;
        string[] PAs = null;
        string[] PMetadata = null;
        FileInfo[] ProvisionMetadataFiles = null;
        string filename = string.Empty;
        try
        {
            UploadedHeaderFileWPath = UploadedHeaderFolderPath + "/" + DataBaseNId + "/" + "sdmx" + "/" + DevInfo.Lib.DI_LibSDMX.Constants.Header.FileName;
            CategorySchemeFolderPath = UploadedHeaderFolderPath + "/" + DbNId + "/" + "sdmx" + "/" + "Categories";
            ProvisonMetadataFolderPath = UploadedHeaderFolderPath + "/" + DbNId + "/" + "sdmx" + "/" + "Provisioning Metadata";
            ProvisionAgreementFolderPath = UploadedHeaderFolderPath + "/" + DbNId + "/" + "sdmx" + "/" + "Provisioning Metadata" + "/" + "PAs";
            UploadedHeaderXml.Load(UploadedHeaderFileWPath);
            SDMXObjectModel.Message.StructureType UploadedDSDStructure = new SDMXObjectModel.Message.StructureType();
            SDMXObjectModel.Message.StructureHeaderType Header = new SDMXObjectModel.Message.StructureHeaderType();
            UploadedDSDStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureType), UploadedHeaderXml);
            Header = UploadedDSDStructure.Header;

            Files = Directory.GetFiles(CategorySchemeFolderPath);
            foreach (string categorySchemeFile in Files)
            {
                CategorySchemeStructure = new SDMXObjectModel.Message.StructureType();
                CategorySchemeStructure = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), categorySchemeFile);
                CategorySchemeStructure.Header = Header;
                CategorySchemeStructure.Structures.Categorisations = null;
                CategorySchemeStructure.Structures.Codelists = null;
                CategorySchemeStructure.Structures.HierarchicalCodelists = null;
                CategorySchemeStructure.Structures.Concepts = null;
                CategorySchemeStructure.Structures.Metadataflows = null;
                CategorySchemeStructure.Structures.MetadataStructures = null;
                CategorySchemeStructure.Structures.ProvisionAgreements = null;
                CategorySchemeStructure.Structures.ReportingTaxonomies = null;
                CategorySchemeStructure.Structures.StructureSets = null;
                CategorySchemeStructure.Structures.OrganisationSchemes = null;
                CategorySchemeStructure.Structures.Dataflows = null;
                CategorySchemeStructure.Structures.Constraints = null;
                CategorySchemeStructure.Structures.DataStructures = null;
                CategorySchemeStructure.Structures.Processes = null;
                CategorySchemeStructure.Footer = null;
                Remove_Extra_Annotations(CategorySchemeStructure);
                SDMXObjectModel.Serializer.SerializeToFile(typeof(SDMXObjectModel.Message.StructureType), CategorySchemeStructure, categorySchemeFile);
            }

            PAs = Directory.GetFiles(ProvisionAgreementFolderPath);
            foreach (string paFile in PAs)
            {
                ProvisionAgreement = new SDMXObjectModel.Message.StructureType();
                ProvisionAgreement = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), paFile);
                ProvisionAgreement.Header = Header;
                ProvisionAgreement.Structures.Categorisations = null;
                ProvisionAgreement.Structures.Codelists = null;
                ProvisionAgreement.Structures.HierarchicalCodelists = null;
                ProvisionAgreement.Structures.Concepts = null;
                ProvisionAgreement.Structures.Metadataflows = null;
                ProvisionAgreement.Structures.MetadataStructures = null;
                ProvisionAgreement.Structures.ReportingTaxonomies = null;
                ProvisionAgreement.Structures.StructureSets = null;
                ProvisionAgreement.Structures.OrganisationSchemes = null;
                ProvisionAgreement.Structures.Dataflows = null;
                ProvisionAgreement.Structures.Constraints = null;
                ProvisionAgreement.Structures.DataStructures = null;
                ProvisionAgreement.Structures.Processes = null;
                ProvisionAgreement.Structures.CategorySchemes = null;
                ProvisionAgreement.Footer = null;
                Remove_Extra_Annotations(ProvisionAgreement);
                SDMXObjectModel.Serializer.SerializeToFile(typeof(SDMXObjectModel.Message.StructureType), ProvisionAgreement, paFile);
            }

            PMetadata = Directory.GetFiles(ProvisonMetadataFolderPath);

            foreach (string pmetadataFile in PMetadata)
            {
                filename = string.Empty;
                ProvisioningMetadata = new SDMXObjectModel.Message.StructureType();
                ProvisioningMetadata = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), pmetadataFile);
                ProvisioningMetadata.Header = Header;
                //  PAStructure.Structures.ProvisionAgreements[0].id.Contains("DF_")
                filename = ExtractFilename(pmetadataFile);
                if (filename.Contains("DF"))
                {
                    ProvisioningMetadata.Structures.Metadataflows = null;
                }
                else
                {
                    ProvisioningMetadata.Structures.Dataflows = null;
                }
                ProvisioningMetadata.Structures.Categorisations = null;
                ProvisioningMetadata.Structures.Codelists = null;
                ProvisioningMetadata.Structures.HierarchicalCodelists = null;
                ProvisioningMetadata.Structures.Concepts = null;
                ProvisioningMetadata.Structures.MetadataStructures = null;
                ProvisioningMetadata.Structures.ProvisionAgreements = null;
                ProvisioningMetadata.Structures.ReportingTaxonomies = null;
                ProvisioningMetadata.Structures.StructureSets = null;
                ProvisioningMetadata.Structures.OrganisationSchemes = null;
                ProvisioningMetadata.Structures.Constraints = null;
                ProvisioningMetadata.Structures.DataStructures = null;
                ProvisioningMetadata.Structures.Processes = null;
                ProvisioningMetadata.Structures.CategorySchemes = null;
                ProvisioningMetadata.Footer = null;
                Remove_Extra_Annotations(ProvisioningMetadata);
                SDMXObjectModel.Serializer.SerializeToFile(typeof(SDMXObjectModel.Message.StructureType), ProvisioningMetadata, pmetadataFile);
            }
        }
        catch (Exception ex)
        {
            RetVal = false;
            Global.CreateExceptionString(ex, null);
            //Global.WriteErrorsInLog("Creating CategoryScheme For Uploaded DSD From Admin");
            //Global.WriteErrorsInLog(ex.StackTrace);
            //Global.WriteErrorsInLog(ex.Message);
        }
        finally
        {

        }

        return RetVal;
    }

    private bool UpdateArtefactsForDBWithHeader(SDMXObjectModel.Message.StructureHeaderType Header, string DbNId, string UserNId, string sdmxFolderPath, string DatabaseNIDVersion2_1)
    {
        bool RetVal;
        SDMXObjectModel.Message.StructureType DSD;
        SDMXObjectModel.Message.StructureType MSD;
        SDMXObjectModel.Message.StructureType MSDConcepts;
        SDMXObjectModel.Message.StructureType Summary;
        string AgencyId, AgencyName, Language, MFDDFolderPathInMetadataFolder;
        string AppPhysicalPath;
        string SummaryFile;
        string DSDFile;
        string DbFolder;
        string ConsumerFileName, ProviderFileName;
        string ConceptSchemeFolder;
        string MSDFolder;

        string[] conceptSchemes;
        string[] MSDS;
        RetVal = true;
        AgencyId = string.Empty;
        AgencyName = string.Empty;
        Language = string.Empty;
        MFDDFolderPathInMetadataFolder = string.Empty;
        ConsumerFileName = string.Empty;
        ProviderFileName = string.Empty;
        ConceptSchemeFolder = string.Empty;
        MSDFolder = string.Empty;

        try
        {

            AgencyId = DevInfo.Lib.DI_LibSDMX.Constants.MaintenanceAgencyScheme.Prefix + UserNId;
            AgencyName = Global.GetDbDetails(DbNId.ToString(), Language)[0];
            AppPhysicalPath = HttpContext.Current.Request.PhysicalApplicationPath;
            DbFolder = Constants.FolderName.Data + DbNId.ToString() + "\\";


            DSD = new SDMXObjectModel.Message.StructureType();
            DSDFile = sdmxFolderPath + "\\" + DevInfo.Lib.DI_LibSDMX.Constants.DSD.FileName;
            DSD = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), DSDFile);
            DSD.Header = Header;
            DSD.Structures.OrganisationSchemes = null;
            DSD.Structures.HierarchicalCodelists = null;
            DSD.Structures.Concepts = null;
            DSD.Structures.Metadataflows = null;
            DSD.Structures.MetadataStructures = null;
            DSD.Structures.Processes = null;
            DSD.Structures.ReportingTaxonomies = null;
            DSD.Structures.StructureSets = null;
            DSD.Structures.CategorySchemes = null;
            DSD.Structures.Codelists = null;
            DSD.Structures.Dataflows = null;
            DSD.Structures.Constraints = null;
            DSD.Structures.ProvisionAgreements = null;
            DSD.Footer = null;
            //Creating DSD and saving its information in the Database.mdb
            SDMXObjectModel.Serializer.SerializeToFile(typeof(SDMXObjectModel.Message.StructureType), DSD, sdmxFolderPath + "\\" + DevInfo.Lib.DI_LibSDMX.Constants.DSD.FileName);



            SummaryFile = sdmxFolderPath + "\\" + DevInfo.Lib.DI_LibSDMX.Constants.Summary_XML.FileName;
            Summary = new SDMXObjectModel.Message.StructureType();
            Summary = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), SummaryFile);

            ConceptSchemeFolder = sdmxFolderPath + "/" + "Concepts";
            MSDFolder = sdmxFolderPath + "/" + "MSD";

            Summary.Header = Header;
            Summary.Structures.OrganisationSchemes = null;
            Summary.Structures.HierarchicalCodelists = null;
            Summary.Structures.Processes = null;
            Summary.Structures.ReportingTaxonomies = null;
            Summary.Structures.StructureSets = null;
            Summary.Structures.Constraints = null;
            Summary.Structures.ProvisionAgreements = null;
            Summary.Structures.Categorisations = null;
            Summary.Footer = null;


            //Creating Summary file and saving its information in the Database.mdb
            SDMXObjectModel.Serializer.SerializeToFile(typeof(SDMXObjectModel.Message.StructureType), Summary, sdmxFolderPath + "\\" + DevInfo.Lib.DI_LibSDMX.Constants.Summary_XML.FileName);

            MSDS = Directory.GetFiles(MSDFolder);
            foreach (string msdfile in MSDS)
            {
                MSD = new SDMXObjectModel.Message.StructureType();
                MSD = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), msdfile);
                MSD.Header = Header;
                MSD.Structures.OrganisationSchemes = null;
                MSD.Structures.HierarchicalCodelists = null;
                MSD.Structures.Concepts = null;
                MSD.Structures.Metadataflows = null;
                MSD.Structures.DataStructures = null;
                MSD.Structures.Processes = null;
                MSD.Structures.ReportingTaxonomies = null;
                MSD.Structures.StructureSets = null;
                MSD.Structures.CategorySchemes = null;
                MSD.Structures.Codelists = null;
                MSD.Structures.Dataflows = null;
                MSD.Structures.Codelists = null;
                MSD.Structures.Categorisations = null;
                MSD.Structures.ProvisionAgreements = null;
                MSD.Footer = null;
                //Creating MSD and saving its information in the Database.mdb

                SDMXObjectModel.Serializer.SerializeToFile(typeof(SDMXObjectModel.Message.StructureType), MSD, msdfile);//sdmxFolderPath + "\\MSD\\" +
            }

            conceptSchemes = Directory.GetFiles(ConceptSchemeFolder);
            foreach (string conceptSchemeFile in conceptSchemes)
            {
                MSDConcepts = new SDMXObjectModel.Message.StructureType();
                MSDConcepts = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), conceptSchemeFile);
                MSDConcepts.Header = Header;
                MSDConcepts.Structures.MetadataStructures = null;
                MSDConcepts.Structures.OrganisationSchemes = null;
                MSDConcepts.Structures.HierarchicalCodelists = null;
                MSDConcepts.Structures.Metadataflows = null;
                MSDConcepts.Structures.DataStructures = null;
                MSDConcepts.Structures.Processes = null;
                MSDConcepts.Structures.ReportingTaxonomies = null;
                MSDConcepts.Structures.StructureSets = null;
                MSDConcepts.Structures.CategorySchemes = null;
                MSDConcepts.Structures.Codelists = null;
                MSDConcepts.Structures.Dataflows = null;
                MSDConcepts.Structures.Categorisations = null;
                MSDConcepts.Structures.ProvisionAgreements = null;
                MSDConcepts.Structures.Constraints = null;
                MSDConcepts.Footer = null;

                //Creating MSDConcepts and saving its information in the Database.mdb
                SDMXObjectModel.Serializer.SerializeToFile(typeof(SDMXObjectModel.Message.StructureType), MSDConcepts, conceptSchemeFile);//sdmxFolderPath + "\\Concepts\\" + 

            }
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("Creating Artefacts For Uploaded DSD From Admin");
            //Global.WriteErrorsInLog(ex.StackTrace);
            //Global.WriteErrorsInLog(ex.Message);
            RetVal = false;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {

        }

        return RetVal;
    }

    #endregion "--SDMX Artefacts Generation For Database--"

    #region "--SDMX Artefacts Generation For Uploaded DSD--"

    private bool CreateFolderStructureForUploadedDSD(string UploadedDSDFolderPath)
    {
        bool RetVal;
        string sdmxFolderPath = string.Empty;
        string CodelistsFolderPath = string.Empty;
        string ConceptsFolderPath = string.Empty;
        string CategoriesFolderPath = string.Empty;
        string MetadataFolderPath = string.Empty;
        string MSDFolderPath = string.Empty;
        string ProvisioningMetadataFolderPath = string.Empty;
        string RegistrationFolderPath = string.Empty;
        string SDMXMLFolderPath = string.Empty;
        string SubscriptionFolderPath = string.Empty;
        string MappingFolderPath = string.Empty;

        RetVal = true;
        try
        {
            //----------------------Creation of a folder for uploaded DSD as per the New Id alloted to Uploaded DSD in db.xml------------
            if (!(Directory.Exists(UploadedDSDFolderPath)))
            {
                Directory.CreateDirectory(UploadedDSDFolderPath);
            }
            //-----------------------------------------------------------------------------------------------------------------
            //----------------------Creation of a sdmx folder for uploaded DSD ------------------------------------------------
            sdmxFolderPath = UploadedDSDFolderPath + "\\" + "sdmx";
            if (!(Directory.Exists(sdmxFolderPath)))
            {
                Directory.CreateDirectory(sdmxFolderPath);
            }
            //-----------------------------------------------------------------------------------------------------------------
            //----------------------Creation of a Codelists folder for uploaded DSD ----------------------------------------------
            CodelistsFolderPath = sdmxFolderPath + "\\" + "Codelists";
            if (!(Directory.Exists(CodelistsFolderPath)))
            {
                Directory.CreateDirectory(CodelistsFolderPath);
            }
            //-----------------------------------------------------------------------------------------------------------------
            //----------------------Creation of a Concepts folder for uploaded DSD ----------------------------------------------
            ConceptsFolderPath = sdmxFolderPath + "\\" + "Concepts";
            if (!(Directory.Exists(ConceptsFolderPath)))
            {
                Directory.CreateDirectory(ConceptsFolderPath);
            }

            //----------------------Creation of a Categories folder for uploaded DSD ----------------------------------------------
            CategoriesFolderPath = sdmxFolderPath + "\\" + "Categories";
            if (!(Directory.Exists(CategoriesFolderPath)))
            {
                Directory.CreateDirectory(CategoriesFolderPath);
            }
            //-----------------------------------------------------------------------------------------------------------------
            //----------------------Creation of a Metadata folder for uploaded DSD -----------------------------------------------
            MetadataFolderPath = sdmxFolderPath + "\\" + "Metadata";
            if (!(Directory.Exists(MetadataFolderPath)))
            {
                Directory.CreateDirectory(MetadataFolderPath);
            }
            //-----------------------------------------------------------------------------------------------------------------
            //----------------------Creation of a MSD folder for uploaded DSD -----------------------------------------------
            MSDFolderPath = sdmxFolderPath + "\\" + "MSD";
            if (!(Directory.Exists(MSDFolderPath)))
            {
                Directory.CreateDirectory(MSDFolderPath);
            }
            //----------------------Creation of a Provisioning Metadata and PAs folder for uploaded DSD --------------------------
            ProvisioningMetadataFolderPath = sdmxFolderPath + "\\" + "Provisioning Metadata";
            if (!(Directory.Exists(ProvisioningMetadataFolderPath)))
            {
                Directory.CreateDirectory(ProvisioningMetadataFolderPath);
            }

            ProvisioningMetadataFolderPath = ProvisioningMetadataFolderPath + "\\" + "PAs";
            if (!(Directory.Exists(ProvisioningMetadataFolderPath)))
            {
                Directory.CreateDirectory(ProvisioningMetadataFolderPath);
            }
            //-----------------------------------------------------------------------------------------------------------------
            //----------------------Creation of a Registrations folder for uploaded DSD --------------------------------------------
            RegistrationFolderPath = sdmxFolderPath + "\\" + "Registrations";
            if (!(Directory.Exists(RegistrationFolderPath)))
            {
                Directory.CreateDirectory(RegistrationFolderPath);
            }
            else
            {
                Directory.Delete(RegistrationFolderPath, true);
                Directory.CreateDirectory(RegistrationFolderPath);
            }
            //-----------------------------------------------------------------------------------------------------------------
            //----------------------Creation of a SDMX-ML folder for uploaded DSD --------------------------------------------
            SDMXMLFolderPath = sdmxFolderPath + "\\" + "SDMX-ML";
            if (!(Directory.Exists(SDMXMLFolderPath)))
            {
                Directory.CreateDirectory(SDMXMLFolderPath);
            }
            //-----------------------------------------------------------------------------------------------------------------
            //----------------------Creation of a Subscriptions folder for uploaded DSD --------------------------------------------
            SubscriptionFolderPath = sdmxFolderPath + "\\" + "Subscriptions";
            if (!(Directory.Exists(SubscriptionFolderPath)))
            {
                Directory.CreateDirectory(SubscriptionFolderPath);
            }
            //-----------------------------------------------------------------------------------------------------------------
            //-----------------------------------------------------------------------------------------------------------------
            //----------------------Creation of a Mapping folder for uploaded DSD --------------------------------------------
            MappingFolderPath = sdmxFolderPath + "\\" + "Mapping";
            if (!(Directory.Exists(MappingFolderPath)))
            {
                Directory.CreateDirectory(MappingFolderPath);
            }
            //-----------------------------------------------------------------------------------------------------------------


        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("Uploading DSD from Admin");
            //Global.WriteErrorsInLog(ex.StackTrace);
            //Global.WriteErrorsInLog(ex.Message);
            Global.CreateExceptionString(ex, null);
            RetVal = false;
        }
        finally
        {

        }

        return RetVal;
    }

    private void Save_Artefacts_Details_For_Uploaded_DSD_In_Database(int DbNId, string Id, string AgencyId, string Version, string URN, ArtefactTypes Type, string FileLocation, string FileName)
    {
        string InsertQuery;
        DIConnection DIConnection;
        System.Data.Common.DbParameter DbParam;
        List<System.Data.Common.DbParameter> DbParams;

        InsertQuery = string.Empty;
        DIConnection = null;

        string AppPhysicalPath = string.Empty;
        string DbFolder = string.Empty;

        try
        {
            AppPhysicalPath = HttpContext.Current.Request.PhysicalApplicationPath;
            DbFolder = Constants.FolderName.Data + DbNId.ToString() + "\\";

            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                           string.Empty, string.Empty);
            DbParams = new List<System.Data.Common.DbParameter>();


            InsertQuery = "INSERT INTO Artefacts (DBNId, Id, AgencyId, Version, URN, Type, FileLocation)" +
                          " VALUES(@DBNId, @Id, @AgencyId, @Version, @URN, @Type, @FileLocation);";

            DbParams = new List<System.Data.Common.DbParameter>();


            DbParam = DIConnection.CreateDBParameter();
            DbParam.ParameterName = "DBNId";
            DbParam.DbType = DbType.Int32;
            DbParam.Value = DbNId;

            DbParams.Add(DbParam);

            DbParam = DIConnection.CreateDBParameter();
            DbParam.ParameterName = "Id";
            DbParam.DbType = DbType.String;
            DbParam.Value = Id;
            DbParams.Add(DbParam);

            DbParam = DIConnection.CreateDBParameter();
            DbParam.ParameterName = "AgencyId";
            DbParam.DbType = DbType.String;
            DbParam.Value = AgencyId;
            DbParams.Add(DbParam);

            DbParam = DIConnection.CreateDBParameter();
            DbParam.ParameterName = "Version";
            DbParam.DbType = DbType.String;
            DbParam.Value = Version;
            DbParams.Add(DbParam);

            DbParam = DIConnection.CreateDBParameter();
            DbParam.ParameterName = "URN";
            DbParam.DbType = DbType.String;
            DbParam.Value = URN;
            DbParams.Add(DbParam);

            DbParam = DIConnection.CreateDBParameter();
            DbParam.ParameterName = "Type";
            DbParam.DbType = DbType.Int32;
            DbParam.Value = Convert.ToInt32(Type);
            DbParams.Add(DbParam);

            DbParam = DIConnection.CreateDBParameter();
            DbParam.ParameterName = "FileLocation";
            DbParam.DbType = DbType.String;

            switch (Type)
            {
                case ArtefactTypes.ConceptS:
                    DbParam.Value = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.Concepts + FileName);
                    break;
                case ArtefactTypes.DSD:
                    DbParam.Value = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.sdmx + FileName);
                    break;
                case ArtefactTypes.MSD:
                    DbParam.Value = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.MSD + FileName);
                    break;
                case ArtefactTypes.CL:
                    DbParam.Value = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.Codelists + FileName);
                    break;
                case ArtefactTypes.CategoryS:
                    DbParam.Value = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.Categories + FileName);
                    break;
                case ArtefactTypes.DFD:
                    DbParam.Value = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.ProvisioningMetadata + FileName);
                    break;
                case ArtefactTypes.MFD:
                    DbParam.Value = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.ProvisioningMetadata + FileName);
                    break;
                case ArtefactTypes.Complete:
                    DbParam.Value = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.sdmx + FileName);
                    break;
                case ArtefactTypes.Summary:
                    DbParam.Value = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.sdmx + FileName);
                    break;
                case ArtefactTypes.Report:
                    DbParam.Value = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.sdmx + FileName);
                    break;
                case ArtefactTypes.PA:
                    DbParam.Value = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.PAs + FileName);
                    break;
                case ArtefactTypes.Mapping:
                    DbParam.Value = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.sdmx + FileName);
                    break;
                case ArtefactTypes.Header:
                    DbParam.Value = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.sdmx + FileName);
                    break;
                default:
                    break;
            }

            DbParams.Add(DbParam);

            DIConnection.ExecuteDataTable(InsertQuery, CommandType.Text, DbParams);

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }
    }

    private bool CreateCodelistsForUploadedDSD(SDMXApi_2_0.Message.StructureType UploadedDSDStructure, string CodelistsFolderPath, int DbNId)
    {
        bool RetVal;
        SDMXApi_2_0.Message.StructureType CodelistStructure;
        int CodelistIndex;
        string CodelistName;
        string CodelistId;
        string CodelistAgencyId;
        string CodelistVersion;

        RetVal = true;
        CodelistIndex = 0;
        CodelistName = string.Empty;
        CodelistId = string.Empty;
        CodelistAgencyId = string.Empty;
        CodelistVersion = string.Empty;
        try
        {
            foreach (SDMXApi_2_0.Structure.CodeListType CodeList in UploadedDSDStructure.CodeLists)
            {
                CodelistStructure = new SDMXApi_2_0.Message.StructureType();
                CodelistStructure.CodeLists = new List<SDMXApi_2_0.Structure.CodeListType>();
                CodelistStructure.CodeLists.Add(UploadedDSDStructure.CodeLists[CodelistIndex]);
                CodelistId = UploadedDSDStructure.CodeLists[CodelistIndex].id;
                CodelistAgencyId = UploadedDSDStructure.CodeLists[CodelistIndex].agencyID;
                CodelistVersion = UploadedDSDStructure.CodeLists[CodelistIndex].version;
                CodelistName = UploadedDSDStructure.CodeLists[CodelistIndex].Name[0].Value.ToString();

                CodelistStructure.Header = UploadedDSDStructure.Header;
                CodelistStructure.OrganisationSchemes = null;
                CodelistStructure.HierarchicalCodelists = null;
                CodelistStructure.Concepts = null;
                CodelistStructure.Metadataflows = null;
                CodelistStructure.MetadataStructureDefinitions = null;
                CodelistStructure.Processes = null;
                CodelistStructure.ReportingTaxonomies = null;
                CodelistStructure.StructureSets = null;
                CodelistStructure.CategorySchemes = null;
                CodelistStructure.KeyFamilies = null;
                CodelistStructure.Dataflows = null;

                SDMXApi_2_0.Serializer.SerializeToFile(typeof(SDMXApi_2_0.Message.StructureType), CodelistStructure, CodelistsFolderPath + "\\" + CodelistName + ".xml");
                this.Save_Artefacts_Details_For_Uploaded_DSD_In_Database(DbNId, CodelistId, CodelistAgencyId, CodelistVersion, string.Empty, ArtefactTypes.CL, CodelistsFolderPath + "\\" + CodelistName + ".xml", CodelistName + ".xml");

                CodelistIndex = CodelistIndex + 1;

            }

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("Creating Codelists For Uploaded DSD from Admin");
            //Global.WriteErrorsInLog(ex.StackTrace);
            //Global.WriteErrorsInLog(ex.Message);
            RetVal = false;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {

        }

        return RetVal;
    }

    private bool CreateReportForUploadedDSD(SDMXApi_2_0.Message.StructureType UploadedDSDStructure, string sdmxFolderPath, int DbNId, string UserNId)
    {
        bool RetVal;
        RetVal = true;
        string AgencyId;
        AgencyId = string.Empty;

        try
        {
            AgencyId = DevInfo.Lib.DI_LibSDMX.Constants.MaintenanceAgencyScheme.Prefix + UserNId;
            DIExcel ReportExcel = new DIExcel();
            ReportExcel = this.GenerateDSDWorksheet(ReportExcel, 0, UploadedDSDStructure);
            ReportExcel = this.GenerateCodelistWorksheets(ReportExcel, 0, UploadedDSDStructure);

            ReportExcel.ActiveSheetIndex = 0;
            ReportExcel.SaveAs(sdmxFolderPath + "\\" + DevInfo.Lib.DI_LibSDMX.Constants.Report.FileName);
            this.Save_Artefacts_Details_For_Uploaded_DSD_In_Database(DbNId, DevInfo.Lib.DI_LibSDMX.Constants.Report.Id, AgencyId, DevInfo.Lib.DI_LibSDMX.Constants.Report.Version, string.Empty, ArtefactTypes.Report, sdmxFolderPath + "\\" + DevInfo.Lib.DI_LibSDMX.Constants.Report.FileName, DevInfo.Lib.DI_LibSDMX.Constants.Report.FileName);


        }
        catch (Exception ex)
        {
            RetVal = false;
            Global.CreateExceptionString(ex, null);
            //Global.WriteErrorsInLog("Creating Report For Uploaded DSD From Admin");
            //Global.WriteErrorsInLog(ex.StackTrace);
            //Global.WriteErrorsInLog(ex.Message);

        }
        finally
        {

        }

        return RetVal;
    }

    private bool CreateCategorySchemeForUploadedDSD(SDMXApi_2_0.Message.StructureType UploadedDSDStructure, string sdmxFolderPath, int DbNId, int AssociatedDbNId, string UserNId)
    {
        bool RetVal;
        RetVal = true;
        string AgencyId, Language, OutputFolder;
        DIConnection DIConnection;
        DIQueries DIQueries;
        List<ArtefactInfo> Artefacts;
        Dictionary<string, string> DictIndicator, DictIndicatorMapping;

        DIConnection = Global.GetDbConnection(AssociatedDbNId);
        DIQueries = new DIQueries(DIConnection.DIDataSetDefault(), DIConnection.DILanguageCodeDefault(DIConnection.DIDataSetDefault()));
        OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId.ToString() + "\\sdmx");
        Language = DIConnection.DILanguageCodeDefault(DIConnection.DIDataSetDefault()).Substring(1);
        AgencyId = DevInfo.Lib.DI_LibSDMX.Constants.MaintenanceAgencyScheme.Prefix + UserNId;

        try
        {
            DictIndicator = RegTwoZeroFunctionality.Get_Indicator_GIds(Path.Combine(OutputFolder, "Complete.xml"));
            DictIndicatorMapping = RegTwoZeroFunctionality.Get_Indicator_Mapping_Dict(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId.ToString() + Constants.FolderName.SDMX.Mapping + DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.FileName), Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId.ToString() + Constants.FolderName.SDMX.Mapping + "IUSMapping.xml"));

            Artefacts = SDMXUtility.Generate_CategoryScheme(SDMXSchemaType.Two_One, CategorySchemeTypes.ALL, AgencyId, Language, null, Path.Combine(OutputFolder, "Categories"), DictIndicator, DictIndicatorMapping, DIConnection, DIQueries);

            this.Save_Artefacts_Details_In_Database(Artefacts, DbNId);
        }
        catch (Exception ex)
        {
            RetVal = false;
            Global.CreateExceptionString(ex, null);
            //Global.WriteErrorsInLog("Creating CategoryScheme For Uploaded DSD From Admin");
            //Global.WriteErrorsInLog(ex.StackTrace);
            //Global.WriteErrorsInLog(ex.Message);
        }
        finally
        {

        }

        return RetVal;
    }

    private SDMXObjectModel.Message.StructureHeaderType Get_Appropriate_Header_Of_StructureHeaderType()
    {
        SDMXObjectModel.Message.StructureHeaderType RetVal;
        SenderType Sender;
        PartyType Receiver;

        Sender = new SenderType(DevInfo.Lib.DI_LibSDMX.Constants.Header.SenderId, DevInfo.Lib.DI_LibSDMX.Constants.Header.SenderName, DevInfo.Lib.DI_LibSDMX.Constants.DefaultLanguage, new SDMXObjectModel.Message.ContactType(DevInfo.Lib.DI_LibSDMX.Constants.Header.Sender, DevInfo.Lib.DI_LibSDMX.Constants.Header.SenderDepartment, DevInfo.Lib.DI_LibSDMX.Constants.Header.SenderRole, DevInfo.Lib.DI_LibSDMX.Constants.DefaultLanguage));
        Sender.Contact[0].Items = new string[] { DevInfo.Lib.DI_LibSDMX.Constants.Header.SenderTelephone, DevInfo.Lib.DI_LibSDMX.Constants.Header.SenderEmail, DevInfo.Lib.DI_LibSDMX.Constants.Header.SenderFax };
        Sender.Contact[0].ItemsElementName = new SDMXObjectModel.Message.ContactChoiceType[] { SDMXObjectModel.Message.ContactChoiceType.Telephone, SDMXObjectModel.Message.ContactChoiceType.Email, SDMXObjectModel.Message.ContactChoiceType.Fax };

        Receiver = new PartyType(DevInfo.Lib.DI_LibSDMX.Constants.Header.ReceiverId, DevInfo.Lib.DI_LibSDMX.Constants.Header.ReceiverName, DevInfo.Lib.DI_LibSDMX.Constants.DefaultLanguage, new SDMXObjectModel.Message.ContactType(DevInfo.Lib.DI_LibSDMX.Constants.Header.Receiver, DevInfo.Lib.DI_LibSDMX.Constants.Header.ReceiverDepartment, DevInfo.Lib.DI_LibSDMX.Constants.Header.ReceiverRole, DevInfo.Lib.DI_LibSDMX.Constants.DefaultLanguage));
        Receiver.Contact[0].Items = new string[] { DevInfo.Lib.DI_LibSDMX.Constants.Header.ReceiverTelephone, DevInfo.Lib.DI_LibSDMX.Constants.Header.ReceiverEmail, DevInfo.Lib.DI_LibSDMX.Constants.Header.ReceiverFax };
        Receiver.Contact[0].ItemsElementName = new SDMXObjectModel.Message.ContactChoiceType[] { SDMXObjectModel.Message.ContactChoiceType.Telephone, SDMXObjectModel.Message.ContactChoiceType.Email, SDMXObjectModel.Message.ContactChoiceType.Fax };

        RetVal = new StructureHeaderType(DevInfo.Lib.DI_LibSDMX.Constants.Header.Id, true, DateTime.Now, Sender, Receiver);
        return RetVal;
    }

    private bool CreateCompleteFileForUploadedDSD(SDMXApi_2_0.Message.StructureType UploadedDSDStructure, string sdmxFolderPath, int DbNId, string UserNId)
    {
        bool RetVal;
        SDMXApi_2_0.Message.StructureType Complete;
        string AgencyId;
        string AppPhysicalPath;
        string DbFolder;

        RetVal = true;
        AgencyId = string.Empty;
        try
        {
            AgencyId = DevInfo.Lib.DI_LibSDMX.Constants.MaintenanceAgencyScheme.Prefix + UserNId;
            AppPhysicalPath = HttpContext.Current.Request.PhysicalApplicationPath;
            DbFolder = Constants.FolderName.Data + DbNId.ToString() + "\\";

            Complete = new SDMXApi_2_0.Message.StructureType();

            Complete.Header = UploadedDSDStructure.Header;

            Complete.Concepts = UploadedDSDStructure.Concepts;
            Complete.KeyFamilies = UploadedDSDStructure.KeyFamilies;
            Complete.CodeLists = UploadedDSDStructure.CodeLists;
            Complete.MetadataStructureDefinitions = UploadedDSDStructure.MetadataStructureDefinitions;
            Complete.OrganisationSchemes = null;
            Complete.HierarchicalCodelists = null;
            Complete.Metadataflows = null;
            Complete.Processes = null;
            Complete.ReportingTaxonomies = null;
            Complete.StructureSets = null;
            Complete.CategorySchemes = null;
            Complete.Dataflows = null;

            foreach (SDMXApi_2_0.Structure.CodeListType CodeList in Complete.CodeLists)
            {
                foreach (SDMXApi_2_0.Structure.CodeType Code in CodeList.Code)
                {
                    Code.Annotations = null;
                }
            }

            //Creating Complete file and saving its information in the Database.mdb
            SDMXApi_2_0.Serializer.SerializeToFile(typeof(SDMXApi_2_0.Message.StructureType), Complete, sdmxFolderPath + "\\" + DevInfo.Lib.DI_LibSDMX.Constants.Complete_XML.FileName);
            this.Save_Artefacts_Details_For_Uploaded_DSD_In_Database(DbNId, DevInfo.Lib.DI_LibSDMX.Constants.Complete_XML.Id, AgencyId, DevInfo.Lib.DI_LibSDMX.Constants.Complete_XML.Version, string.Empty, ArtefactTypes.Complete, sdmxFolderPath + "\\" + DevInfo.Lib.DI_LibSDMX.Constants.Complete_XML.FileName, DevInfo.Lib.DI_LibSDMX.Constants.Complete_XML.FileName);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("Creating Artefacts For Uploaded DSD From Admin");
            //Global.WriteErrorsInLog(ex.StackTrace);
            //Global.WriteErrorsInLog(ex.Message);
            RetVal = false;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {

        }

        return RetVal;
    }

    public bool CreateHeaderFileForUploadedDSD(SDMXApi_2_0.Message.StructureType UploadedDSDStructure, string sdmxFolderPath, int DbNId, string UserNId)
    {
        bool RetVal;

        SDMXApi_2_0.Message.StructureType Header;
        //  SDMXApi_2_0.Message.HeaderType Header;
        string AgencyId;
        string AppPhysicalPath;
        string DbFolder;

        RetVal = true;
        AgencyId = string.Empty;
        try
        {
            AgencyId = DevInfo.Lib.DI_LibSDMX.Constants.MaintenanceAgencyScheme.Prefix + UserNId;
            AppPhysicalPath = HttpContext.Current.Request.PhysicalApplicationPath;
            DbFolder = Constants.FolderName.Data + DbNId.ToString() + "\\";

            Header = new SDMXApi_2_0.Message.StructureType();
            //Header.ID = UploadedDSDStructure.Header.ID;
            //Header.Name = UploadedDSDStructure.Header.na;
            //Header.Test = UploadedDSDStructure.Header.Sender;
            //Header.Prepared = UploadedDSDStructure.Header.Sender;
            //Header.Sender = UploadedDSDStructure.Header.Sender;
            //Header.Receiver = UploadedDSDStructure.Header.Receiver;

            Header.Header = UploadedDSDStructure.Header;

            Header.Concepts = null;
            Header.KeyFamilies = null;
            Header.CodeLists = null;
            Header.MetadataStructureDefinitions = null;
            Header.OrganisationSchemes = null;
            Header.HierarchicalCodelists = null;
            Header.Metadataflows = null;
            Header.Processes = null;
            Header.ReportingTaxonomies = null;
            Header.StructureSets = null;
            Header.CategorySchemes = null;
            Header.Dataflows = null;
            Remove_Extra_Annotations(Header);
            //Creating Header file and saving its information in the Database.mdb
            SDMXApi_2_0.Serializer.SerializeToFile(typeof(SDMXApi_2_0.Message.StructureType), Header, sdmxFolderPath + "\\" + DevInfo.Lib.DI_LibSDMX.Constants.Header.FileName);
            this.Save_Artefacts_Details_For_Uploaded_DSD_In_Database(DbNId, DevInfo.Lib.DI_LibSDMX.Constants.Header.Id, string.Empty, string.Empty, string.Empty, ArtefactTypes.Header, sdmxFolderPath + "\\" + DevInfo.Lib.DI_LibSDMX.Constants.Header.FileName, DevInfo.Lib.DI_LibSDMX.Constants.Header.FileName);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("Creating Artefacts For Uploaded DSD From Admin");
            //Global.WriteErrorsInLog(ex.StackTrace);
            //Global.WriteErrorsInLog(ex.Message);
            RetVal = false;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {

        }

        return RetVal;
    }

    private bool CreateArtefactsForUploadedDSD(SDMXApi_2_0.Message.StructureType UploadedDSDStructure, string sdmxFolderPath, int DbNId, string UserNId)
    {
        bool RetVal;
        SDMXApi_2_0.Message.StructureType Concepts;
        SDMXApi_2_0.Message.StructureType DSD;
        SDMXApi_2_0.Message.StructureType MSD;
        SDMXApi_2_0.Message.StructureType MSDConcepts;
        SDMXApi_2_0.Message.StructureType Summary;
        List<ArtefactInfo> Artefacts, PAArtefacts;
        string AgencyId, AgencyName, Language, MFDDFolderPathInMetadataFolder;
        string AppPhysicalPath;
        string DbFolder;
        string FolderName;
        string ConsumerFileName, ProviderFileName;
        SDMXObjectModel.Message.StructureType Structure;

        RetVal = true;
        AgencyId = string.Empty;
        AgencyName = string.Empty;
        Language = string.Empty;
        MFDDFolderPathInMetadataFolder = string.Empty;
        ConsumerFileName = string.Empty;
        ProviderFileName = string.Empty;
        Structure = null;

        try
        {
            //Language = UploadedDSDStructure.Concepts.Concept[0].Name[0].lang;
            Language = UploadedDSDStructure.CodeLists[0].Name[0].lang;

            AgencyId = DevInfo.Lib.DI_LibSDMX.Constants.MaintenanceAgencyScheme.Prefix + UserNId;
            AgencyName = Global.GetDbDetails(DbNId.ToString(), Language)[0];
            AppPhysicalPath = HttpContext.Current.Request.PhysicalApplicationPath;
            DbFolder = Constants.FolderName.Data + DbNId.ToString() + "\\";

            Concepts = new SDMXApi_2_0.Message.StructureType();
            DSD = new SDMXApi_2_0.Message.StructureType();

            Summary = new SDMXApi_2_0.Message.StructureType();

            if (UploadedDSDStructure.Concepts != null)
            {
                Concepts.Header = UploadedDSDStructure.Header;
                Concepts.Concepts = UploadedDSDStructure.Concepts;
                Summary.Concepts = UploadedDSDStructure.Concepts;
            }
            DSD.Header = UploadedDSDStructure.Header;
            Summary.Header = UploadedDSDStructure.Header;


            DSD.KeyFamilies = UploadedDSDStructure.KeyFamilies;



            Summary.KeyFamilies = UploadedDSDStructure.KeyFamilies;
            Summary.MetadataStructureDefinitions = UploadedDSDStructure.MetadataStructureDefinitions;
            Summary.CodeLists = UploadedDSDStructure.CodeLists;

            foreach (SDMXApi_2_0.Structure.CodeListType CodeList in Summary.CodeLists)
            {
                CodeList.Code = null;
            }

            Summary.OrganisationSchemes = null;
            Summary.HierarchicalCodelists = null;
            Summary.Metadataflows = null;
            Summary.Processes = null;
            Summary.ReportingTaxonomies = null;
            Summary.StructureSets = null;
            Summary.CategorySchemes = null;
            Summary.Dataflows = null;

            //Creating Summary file and saving its information in the Database.mdb
            SDMXApi_2_0.Serializer.SerializeToFile(typeof(SDMXApi_2_0.Message.StructureType), Summary, sdmxFolderPath + "\\" + DevInfo.Lib.DI_LibSDMX.Constants.Summary_XML.FileName);
            this.Save_Artefacts_Details_For_Uploaded_DSD_In_Database(DbNId, DevInfo.Lib.DI_LibSDMX.Constants.Summary_XML.Id, AgencyId, DevInfo.Lib.DI_LibSDMX.Constants.Summary_XML.Version, string.Empty, ArtefactTypes.Summary, sdmxFolderPath + "\\" + DevInfo.Lib.DI_LibSDMX.Constants.Summary_XML.FileName, DevInfo.Lib.DI_LibSDMX.Constants.Summary_XML.FileName);


            DSD.OrganisationSchemes = null;
            DSD.HierarchicalCodelists = null;
            DSD.Concepts = null;
            DSD.Metadataflows = null;
            DSD.MetadataStructureDefinitions = null;
            DSD.Processes = null;
            DSD.ReportingTaxonomies = null;
            DSD.StructureSets = null;
            DSD.CategorySchemes = null;
            DSD.CodeLists = null;
            DSD.Dataflows = null;

            if (UploadedDSDStructure.MetadataStructureDefinitions != null)
            {
                if (UploadedDSDStructure.MetadataStructureDefinitions.Count > 0)
                {
                    foreach (SDMXApi_2_0.Structure.MetadataStructureDefinitionType MetadataStructureDefinition in UploadedDSDStructure.MetadataStructureDefinitions)
                    {
                        MSD = new SDMXApi_2_0.Message.StructureType();
                        MSD.Header = UploadedDSDStructure.Header;
                        MSD.MetadataStructureDefinitions = new List<SDMXApi_2_0.Structure.MetadataStructureDefinitionType>();
                        MSD.MetadataStructureDefinitions.Add(new SDMXApi_2_0.Structure.MetadataStructureDefinitionType());
                        MSD.MetadataStructureDefinitions[0] = MetadataStructureDefinition;
                        MSD.OrganisationSchemes = null;
                        MSD.HierarchicalCodelists = null;
                        MSD.Concepts = null;
                        MSD.Metadataflows = null;
                        MSD.KeyFamilies = null;
                        MSD.Processes = null;
                        MSD.ReportingTaxonomies = null;
                        MSD.StructureSets = null;
                        MSD.CategorySchemes = null;
                        MSD.CodeLists = null;
                        MSD.Dataflows = null;
                        //Creating MSD and saving its information in the Database.mdb
                        SDMXApi_2_0.Serializer.SerializeToFile(typeof(SDMXApi_2_0.Message.StructureType), MSD, sdmxFolderPath + "\\MSD\\" + MSD.MetadataStructureDefinitions[0].id + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension);
                        this.Save_Artefacts_Details_For_Uploaded_DSD_In_Database(DbNId, MSD.MetadataStructureDefinitions[0].id, MSD.MetadataStructureDefinitions[0].agencyID, MSD.MetadataStructureDefinitions[0].version, string.Empty, ArtefactTypes.MSD, sdmxFolderPath + "\\" + MSD.MetadataStructureDefinitions[0].id + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension, MSD.MetadataStructureDefinitions[0].id + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension);

                        //Creating MFD and saving its information in the Database.mdb
                        this.Generate_MFD("MF_" + MetadataStructureDefinition.id, AgencyId, DevInfo.Lib.DI_LibSDMX.Constants.MFD.Area.Version, "DevInfo Metadata Flow for " + GetLangSpecificValueFor_Version_2_0(MetadataStructureDefinition.Name, Language), DevInfo.Lib.DI_LibSDMX.Constants.MFD.Area.Description, Language, MetadataStructureDefinition.id, MetadataStructureDefinition.agencyID, MetadataStructureDefinition.version, sdmxFolderPath, DbNId);

                        //----------------------Creation of a MSD folder for uploaded DSD -----------------------------------------------
                        MFDDFolderPathInMetadataFolder = sdmxFolderPath + "\\" + "Metadata" + "\\" + "MF_" + MetadataStructureDefinition.id;
                        if (!(Directory.Exists(MFDDFolderPathInMetadataFolder)))
                        {
                            Directory.CreateDirectory(MFDDFolderPathInMetadataFolder);
                        }
                    }
                }
            }

            if (UploadedDSDStructure.Concepts.ConceptScheme != null)
            {
                if (UploadedDSDStructure.Concepts.ConceptScheme.Count > 0)
                {
                    foreach (SDMXApi_2_0.Structure.ConceptSchemeType ConceptScheme in UploadedDSDStructure.Concepts.ConceptScheme)
                    {
                        MSDConcepts = new SDMXApi_2_0.Message.StructureType();
                        MSDConcepts.Header = UploadedDSDStructure.Header;
                        MSDConcepts.Concepts.ConceptScheme = new List<SDMXApi_2_0.Structure.ConceptSchemeType>();
                        MSDConcepts.Concepts.ConceptScheme.Add(new SDMXApi_2_0.Structure.ConceptSchemeType());
                        MSDConcepts.Concepts.ConceptScheme[0] = ConceptScheme;
                        MSDConcepts.Concepts.Concept = null;
                        MSDConcepts.MetadataStructureDefinitions = null;
                        MSDConcepts.OrganisationSchemes = null;
                        MSDConcepts.HierarchicalCodelists = null;
                        MSDConcepts.Metadataflows = null;
                        MSDConcepts.KeyFamilies = null;
                        MSDConcepts.Processes = null;
                        MSDConcepts.ReportingTaxonomies = null;
                        MSDConcepts.StructureSets = null;
                        MSDConcepts.CategorySchemes = null;
                        MSDConcepts.CodeLists = null;
                        MSDConcepts.Dataflows = null;

                        //Creating MSDConcepts and saving its information in the Database.mdb
                        SDMXApi_2_0.Serializer.SerializeToFile(typeof(SDMXApi_2_0.Message.StructureType), MSDConcepts, sdmxFolderPath + "\\Concepts\\" + MSDConcepts.Concepts.ConceptScheme[0].id + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension);
                        this.Save_Artefacts_Details_For_Uploaded_DSD_In_Database(DbNId, string.Empty, MSDConcepts.Concepts.ConceptScheme[0].agencyID, MSDConcepts.Concepts.ConceptScheme[0].version, string.Empty, ArtefactTypes.ConceptS, sdmxFolderPath + "\\Concepts\\" + MSDConcepts.Concepts.ConceptScheme[0].id + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension, MSDConcepts.Concepts.ConceptScheme[0].id + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension);

                    }

                }
            }
            if (UploadedDSDStructure.Concepts != null)
            {
                if (UploadedDSDStructure.Concepts.Concept.Count > 0)
                {
                    Concepts.OrganisationSchemes = null;
                    Concepts.HierarchicalCodelists = null;
                    Concepts.KeyFamilies = null;
                    Concepts.Metadataflows = null;
                    Concepts.MetadataStructureDefinitions = null;
                    Concepts.Processes = null;
                    Concepts.ReportingTaxonomies = null;
                    Concepts.StructureSets = null;
                    Concepts.CategorySchemes = null;
                    Concepts.CodeLists = null;
                    Concepts.Dataflows = null;
                    Concepts.Concepts.ConceptScheme = null;

                    //Creating Concepts and saving its information in the Database.mdb
                    SDMXApi_2_0.Serializer.SerializeToFile(typeof(SDMXApi_2_0.Message.StructureType), Concepts, sdmxFolderPath + "\\Concepts\\" + DevInfo.Lib.DI_LibSDMX.Constants.ConceptScheme.DSD.FileName);
                    this.Save_Artefacts_Details_For_Uploaded_DSD_In_Database(DbNId, string.Empty, Concepts.Concepts.Concept[0].agencyID, Concepts.Concepts.Concept[0].version, string.Empty, ArtefactTypes.ConceptS, sdmxFolderPath + "\\Concepts\\" + DevInfo.Lib.DI_LibSDMX.Constants.ConceptScheme.DSD.FileName, DevInfo.Lib.DI_LibSDMX.Constants.ConceptScheme.DSD.FileName);
                }
            }
            //Creating DSD and saving its information in the Database.mdb
            SDMXApi_2_0.Serializer.SerializeToFile(typeof(SDMXApi_2_0.Message.StructureType), DSD, sdmxFolderPath + "\\" + DevInfo.Lib.DI_LibSDMX.Constants.DSD.FileName);
            this.Save_Artefacts_Details_For_Uploaded_DSD_In_Database(DbNId, DSD.KeyFamilies[0].id, DSD.KeyFamilies[0].agencyID, DSD.KeyFamilies[0].version, string.Empty, ArtefactTypes.DSD, sdmxFolderPath + "\\" + DevInfo.Lib.DI_LibSDMX.Constants.DSD.FileName, DevInfo.Lib.DI_LibSDMX.Constants.DSD.FileName);





            //Creating DFD and saving its information in the Database.mdb
            Artefacts = SDMXUtility.Generate_DFD(SDMXSchemaType.Two_One, UploadedDSDStructure.KeyFamilies[0].id, AgencyId, null, sdmxFolderPath + "\\Provisioning Metadata");

            //Creating PA per user and saving its information in the Database.mdb
            PAArtefacts = this.Create_PAs_For_Database_Per_Provider(DbNId.ToString(), AgencyId, sdmxFolderPath + "\\Provisioning Metadata\\PAs", Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users + DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.FileName), true);
            if (PAArtefacts != null && PAArtefacts.Count > 0)
            {
                Artefacts.AddRange(PAArtefacts);
            }

            this.Save_Artefacts_Details_In_Database(Artefacts, DbNId);

            ConsumerFileName = Path.Combine(AppPhysicalPath, Constants.FolderName.Users + DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.FileName);

            if (File.Exists(ConsumerFileName))
            {
                Structure = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), ConsumerFileName);

                if (Structure != null && Structure.Structures != null && Structure.Structures.OrganisationSchemes != null && Structure.Structures.OrganisationSchemes.Count > 0 && Structure.Structures.OrganisationSchemes[0] is SDMXObjectModel.Structure.DataConsumerSchemeType &&
                    Structure.Structures.OrganisationSchemes[0].Organisation != null && Structure.Structures.OrganisationSchemes[0].Organisation.Count > 0)
                {
                    foreach (DataConsumerType DataConsumer in Structure.Structures.OrganisationSchemes[0].Organisation)
                    {
                        FolderName = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.Subscriptions + DataConsumer.id.Replace(DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.Prefix, string.Empty));
                        this.Create_Directory_If_Not_Exists(FolderName);
                    }
                }
            }

            ProviderFileName = Path.Combine(AppPhysicalPath, Constants.FolderName.Users + DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.FileName);

            if (File.Exists(ProviderFileName))
            {
                Structure = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), ProviderFileName);

                if (Structure != null && Structure.Structures != null && Structure.Structures.OrganisationSchemes != null && Structure.Structures.OrganisationSchemes.Count > 0 && Structure.Structures.OrganisationSchemes[0] is SDMXObjectModel.Structure.DataProviderSchemeType &&
                    Structure.Structures.OrganisationSchemes[0].Organisation != null && Structure.Structures.OrganisationSchemes[0].Organisation.Count > 0)
                {
                    foreach (DataProviderType DataProvider in Structure.Structures.OrganisationSchemes[0].Organisation)
                    {
                        FolderName = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.Subscriptions + DataProvider.id.Replace(DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix, string.Empty));
                        this.Create_Directory_If_Not_Exists(FolderName);

                        FolderName = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.Registrations + DataProvider.id.Replace(DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix, string.Empty));
                        this.Create_Directory_If_Not_Exists(FolderName);

                        FolderName = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.Constraints + DataProvider.id.Replace(DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix, string.Empty));
                        this.Create_Directory_If_Not_Exists(FolderName);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("Creating Artefacts For Uploaded DSD From Admin");
            //Global.WriteErrorsInLog(ex.StackTrace);
            //Global.WriteErrorsInLog(ex.Message);
            RetVal = false;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {

        }

        return RetVal;
    }

    private void Generate_MFD(string MFDId, string MFDAgencyId, string MFDVersion, string MFDName, string MFDDescription, string MFDLanguage, string DSDId, string DSDAgencyId, string DSDVersion, string sdmxFolderPath, int DbNId)
    {
        SDMXObjectModel.Message.StructureType MFD;
        SDMXObjectModel.Structure.MetadataflowType Metadataflow;

        Metadataflow = null;

        try
        {
            MFD = new SDMXObjectModel.Message.StructureType();
            MFD.Header = Get_Appropriate_Header_Of_StructureHeaderType();
            Metadataflow = new SDMXObjectModel.Structure.MetadataflowType(MFDId, MFDAgencyId, MFDVersion, MFDName, MFDDescription, MFDLanguage, null);

            Metadataflow.Structure = new MetadataStructureReferenceType();
            Metadataflow.Structure.Items.Add(new MetadataStructureRefType(DSDId, DSDAgencyId, DSDVersion));
            MFD.Structures.Metadataflows = new List<MetadataflowType>();
            MFD.Structures.Metadataflows.Add(Metadataflow);
            SDMXObjectModel.Serializer.SerializeToFile(typeof(SDMXObjectModel.Message.StructureType), MFD, sdmxFolderPath + "\\Provisioning Metadata\\" + MFDId + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension);
            this.Save_Artefacts_Details_For_Uploaded_DSD_In_Database(DbNId, MFDId, MFDAgencyId, MFDVersion, string.Empty, ArtefactTypes.MFD, sdmxFolderPath + "\\Provisioning Metadata\\" + MFDId + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension, MFDId + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
        }
    }

    private void Generate_Header(SDMXSchemaType schemaType, string agencyId, string language, Header header, string outputFolder, string sdmxFolderPath, int DbNId)
    {
        SDMXObjectModel.Message.StructureType Header;


        try
        {
            Header = new SDMXObjectModel.Message.StructureType();
            Header.Header = Get_Appropriate_Header_Of_StructureHeaderType();
            Header.Structures = null;
            Header.Footer = null;
            SDMXObjectModel.Serializer.SerializeToFile(typeof(SDMXObjectModel.Message.StructureType), Header, sdmxFolderPath + "\\" + DevInfo.Lib.DI_LibSDMX.Constants.Header.FileName);
            this.Save_Artefacts_Details_For_Uploaded_DSD_In_Database(DbNId, DevInfo.Lib.DI_LibSDMX.Constants.Header.Id, string.Empty, string.Empty, string.Empty, ArtefactTypes.Header, sdmxFolderPath + "\\" + DevInfo.Lib.DI_LibSDMX.Constants.Header.FileName, DevInfo.Lib.DI_LibSDMX.Constants.Header.FileName);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
        }
    }

    private DIExcel GenerateDSDWorksheet(DIExcel ReportExcel, int SheetIndex, SDMXApi_2_0.Message.StructureType CompleteStructure)
    {

        int i, j;
        IWorksheet DSDWorkSheet = null;
        int rowindex = 0;
        string Language, AttributeImportance;
        Language = string.Empty;
        AttributeImportance = string.Empty;


        try
        {
            // Language = CompleteStructure.Concepts.Concept[0].Name[0].lang;
            if (CompleteStructure.Concepts.Concept.Count > 0)
            {
                Language = CompleteStructure.Concepts.Concept[0].Name[0].lang;
            }
            else if (CompleteStructure.Concepts.ConceptScheme.Count > 0)
            {
                Language = CompleteStructure.Concepts.ConceptScheme[0].Name[0].lang;
            }

            DSDWorkSheet = ReportExcel.GetWorksheet(0);
            ReportExcel.RenameWorkSheet(0, "DSD");
            rowindex = rowindex + 1;
            this.WriteValueInCell(ReportExcel, "Data Structure Definition", rowindex, 1, 14, true, 30, 0, 0);
            rowindex = rowindex + 2;

            //Binding Dimensions  
            this.WriteValueInCell(ReportExcel, "Dimensions", rowindex, 1, 12, true, 30, 0, 0);
            rowindex = rowindex + 2;
            if (CompleteStructure.Concepts.Concept.Count > 0)
            {
                for (i = 0; i < CompleteStructure.KeyFamilies[0].Components.Dimension.Count; i++)
                {
                    for (j = 0; j < CompleteStructure.Concepts.Concept.Count; j++)
                    {
                        if (CompleteStructure.Concepts.Concept[j].id == CompleteStructure.KeyFamilies[0].Components.Dimension[i].conceptRef)
                        {
                            this.WriteValueInCell(ReportExcel, GetLangSpecificValueFor_Version_2_0(CompleteStructure.Concepts.Concept[j].Name, Language), rowindex, 1, 10, false, 30, 0, 0);
                            this.WriteValueInCell(ReportExcel, GetLangSpecificValueFor_Version_2_0(CompleteStructure.Concepts.Concept[j].Description, Language), rowindex, 2, 10, false, 250, 0, 0);
                            rowindex = rowindex + 1;
                            break;
                        }
                    }

                }
            }
            else if (CompleteStructure.Concepts.ConceptScheme.Count > 0)
            {
                for (i = 0; i < CompleteStructure.KeyFamilies[0].Components.Dimension.Count; i++)
                {
                    for (j = 0; j < CompleteStructure.Concepts.ConceptScheme.Count; j++)
                    {
                        for (int k = 0; k < CompleteStructure.Concepts.ConceptScheme[j].Concept.Count; k++)
                        {
                            if (CompleteStructure.Concepts.ConceptScheme[j].Concept[k].id == CompleteStructure.KeyFamilies[0].Components.Dimension[i].conceptRef)
                            {
                                this.WriteValueInCell(ReportExcel, GetLangSpecificValueFor_Version_2_0(CompleteStructure.Concepts.ConceptScheme[j].Concept[k].Name, Language), rowindex, 1, 10, false, 30, 0, 0);
                                this.WriteValueInCell(ReportExcel, GetLangSpecificValueFor_Version_2_0(CompleteStructure.Concepts.ConceptScheme[j].Concept[k].Description, Language), rowindex, 2, 10, false, 250, 0, 0);
                                rowindex = rowindex + 1;
                                break;
                            }
                        }
                     
                    }

                }
            }

            //Binding Time Dimension  

            if (CompleteStructure.Concepts.Concept.Count > 0)
            {
                for (j = 0; j < CompleteStructure.Concepts.Concept.Count; j++)
                {
                    if (CompleteStructure.Concepts.Concept[j].id == CompleteStructure.KeyFamilies[0].Components.TimeDimension.conceptRef)
                    {
                        this.WriteValueInCell(ReportExcel, GetLangSpecificValueFor_Version_2_0(CompleteStructure.Concepts.Concept[j].Name, Language), rowindex, 1, 10, false, 30, 0, 0);
                        this.WriteValueInCell(ReportExcel, GetLangSpecificValueFor_Version_2_0(CompleteStructure.Concepts.Concept[j].Description, Language), rowindex, 2, 10, false, 250, 0, 0);
                        rowindex = rowindex + 1;
                        break;
                    }
                }
            }

            else if (CompleteStructure.Concepts.ConceptScheme.Count > 0)
            {
                for (j = 0; j < CompleteStructure.Concepts.ConceptScheme.Count; j++)
                {
                    for (int k = 0; k < CompleteStructure.Concepts.ConceptScheme[j].Concept.Count; k++)
                    {
                        if (CompleteStructure.Concepts.ConceptScheme[j].Concept[k].id == CompleteStructure.KeyFamilies[0].Components.TimeDimension.conceptRef)
                        {
                            this.WriteValueInCell(ReportExcel, GetLangSpecificValueFor_Version_2_0(CompleteStructure.Concepts.ConceptScheme[j].Concept[k].Name, Language), rowindex, 1, 10, false, 30, 0, 0);
                            this.WriteValueInCell(ReportExcel, GetLangSpecificValueFor_Version_2_0(CompleteStructure.Concepts.ConceptScheme[j].Concept[k].Description, Language), rowindex, 2, 10, false, 250, 0, 0);
                            rowindex = rowindex + 1;
                            break;
                        }
                    }
                }
            }

            rowindex = rowindex + 2;

            //Binding Attributes  

            this.WriteValueInCell(ReportExcel, "Attributes", rowindex, 1, 12, true, 30, 0, 0);
            rowindex = rowindex + 2;

            if (CompleteStructure.Concepts.Concept.Count > 0)
            {
                for (i = 0; i < CompleteStructure.KeyFamilies[0].Components.Attribute.Count; i++)
                {
                    for (j = 0; j < CompleteStructure.Concepts.Concept.Count; j++)
                    {
                        if (CompleteStructure.Concepts.Concept[j].id == CompleteStructure.KeyFamilies[0].Components.Attribute[i].conceptRef)
                        {
                            this.WriteValueInCell(ReportExcel, GetLangSpecificValueFor_Version_2_0(CompleteStructure.Concepts.Concept[j].Name, Language), rowindex, 1, 10, false, 30, 0, 0);
                            this.WriteValueInCell(ReportExcel, GetLangSpecificValueFor_Version_2_0(CompleteStructure.Concepts.Concept[j].Description, Language), rowindex, 2, 10, false, 250, 0, 0);
                            rowindex = rowindex + 1;
                            this.WriteValueInCell(ReportExcel, "Attachment Level : " + CompleteStructure.KeyFamilies[0].Components.Attribute[i].attachmentLevel, rowindex, 1, 10, false, 30, 0, 0);

                            if (CompleteStructure.KeyFamilies[0].Components.Attribute[i].assignmentStatus == SDMXApi_2_0.Structure.AssignmentStatusType.Mandatory)
                            {
                                AttributeImportance = "Mandatory : " + "Yes";
                            }
                            else
                            {
                                AttributeImportance = "Mandatory : " + "No";
                            }
                            this.WriteValueInCell(ReportExcel, AttributeImportance, rowindex, 2, 10, false, 30, 0, 0);
                            rowindex = rowindex + 2;

                            break;
                        }
                    }

                }
            }
            else if (CompleteStructure.Concepts.ConceptScheme.Count > 0)
            {
                for (i = 0; i < CompleteStructure.KeyFamilies[0].Components.Attribute.Count; i++)
                {
                    for (j = 0; j < CompleteStructure.Concepts.ConceptScheme.Count; j++)
                    {
                        for (int k = 0; k < CompleteStructure.Concepts.ConceptScheme[j].Concept.Count; k++)
                        {
                            if (CompleteStructure.Concepts.ConceptScheme[j].Concept[k].id == CompleteStructure.KeyFamilies[0].Components.Attribute[i].conceptRef)
                            {
                                this.WriteValueInCell(ReportExcel, GetLangSpecificValueFor_Version_2_0(CompleteStructure.Concepts.ConceptScheme[j].Concept[k].Name, Language), rowindex, 1, 10, false, 30, 0, 0);
                                this.WriteValueInCell(ReportExcel, GetLangSpecificValueFor_Version_2_0(CompleteStructure.Concepts.ConceptScheme[j].Concept[k].Description, Language), rowindex, 2, 10, false, 250, 0, 0);
                                rowindex = rowindex + 1;
                                this.WriteValueInCell(ReportExcel, "Attachment Level : " + CompleteStructure.KeyFamilies[0].Components.Attribute[i].attachmentLevel, rowindex, 1, 10, false, 30, 0, 0);

                                if (CompleteStructure.KeyFamilies[0].Components.Attribute[i].assignmentStatus == SDMXApi_2_0.Structure.AssignmentStatusType.Mandatory)
                                {
                                    AttributeImportance = "Mandatory : " + "Yes";
                                }
                                else
                                {
                                    AttributeImportance = "Mandatory : " + "No";
                                }
                                this.WriteValueInCell(ReportExcel, AttributeImportance, rowindex, 2, 10, false, 30, 0, 0);
                                rowindex = rowindex + 2;

                                break;
                            }
                        }

                    }

                }
            }

            rowindex = rowindex + 1;

            //Binding Measure  
            this.WriteValueInCell(ReportExcel, "Measure", rowindex, 1, 12, true, 30, 0, 0);

            rowindex = rowindex + 1;
            if (CompleteStructure.Concepts.Concept.Count > 0)
            {
                for (j = 0; j < CompleteStructure.Concepts.Concept.Count; j++)
                {
                    if (CompleteStructure.Concepts.Concept[j].id == CompleteStructure.KeyFamilies[0].Components.PrimaryMeasure.conceptRef)
                    {
                        this.WriteValueInCell(ReportExcel, GetLangSpecificValueFor_Version_2_0(CompleteStructure.Concepts.Concept[j].Name, Language), rowindex, 1, 10, false, 30, 0, 0);
                        this.WriteValueInCell(ReportExcel, GetLangSpecificValueFor_Version_2_0(CompleteStructure.Concepts.Concept[j].Description, Language), rowindex, 2, 10, false, 250, 0, 0);
                        rowindex = rowindex + 1;
                        break;
                    }
                }
            }
            else if (CompleteStructure.Concepts.ConceptScheme.Count > 0)
            {
                for (j = 0; j < CompleteStructure.Concepts.ConceptScheme.Count; j++)
                {
                    if (CompleteStructure.Concepts.ConceptScheme[j].id == CompleteStructure.KeyFamilies[0].Components.PrimaryMeasure.conceptSchemeRef)
                    {
                        for (int k = 0; k < CompleteStructure.Concepts.ConceptScheme[j].Concept.Count; k++)
                        {
                            if (CompleteStructure.Concepts.ConceptScheme[j].Concept[k].id == CompleteStructure.KeyFamilies[0].Components.PrimaryMeasure.conceptRef)
                            {
                                this.WriteValueInCell(ReportExcel, GetLangSpecificValueFor_Version_2_0(CompleteStructure.Concepts.ConceptScheme[j].Concept[k].Name, Language), rowindex, 1, 10, false, 30, 0, 0);
                                this.WriteValueInCell(ReportExcel, GetLangSpecificValueFor_Version_2_0(CompleteStructure.Concepts.ConceptScheme[j].Concept[k].Description, Language), rowindex, 2, 10, false, 250, 0, 0);
                                rowindex = rowindex + 1;
                                break;
                            }
                        }
                    }
                }
            }



        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
        }

        return ReportExcel;
    }

    private DIExcel GenerateCodelistWorksheets(DIExcel ReportExcel, int SheetIndex, SDMXApi_2_0.Message.StructureType CompleteStructure)
    {

        int i, j;
        IWorksheet CodelistWorkSheet = null;
        SDMXApi_2_0.Structure.CodeListType Codelist;
        SDMXApi_2_0.Structure.CodeType Code;
        string CodelistName = string.Empty;
        string CodeValue = string.Empty;
        string CodeDescription = string.Empty;

        int rowindex = 0;
        string Language = string.Empty;


        try
        {
            if (CompleteStructure.Concepts.Concept.Count > 0)
            {
                Language = CompleteStructure.Concepts.Concept[0].Name[0].lang;
            }
            else if (CompleteStructure.Concepts.ConceptScheme.Count > 0)
            {
                Language = CompleteStructure.Concepts.ConceptScheme[0].Name[0].lang;
            }


            for (i = 1; i <= CompleteStructure.CodeLists.Count; i++)
            {

                Codelist = CompleteStructure.CodeLists[i - 1];
                CodelistName = GetLangSpecificValueFor_Version_2_0(Codelist.Name, Language);
                ReportExcel.InsertWorkSheet(CodelistName);
                CodelistWorkSheet = ReportExcel.GetWorksheet(i);
                rowindex = 1;
                this.WriteValueInCell(ReportExcel, CodelistName, rowindex, 1, 12, true, 30, 0, i);
                rowindex = rowindex + 2;

                this.WriteValueInCell(ReportExcel, "Code", rowindex, 1, 10, true, 60, 0, i);
                this.WriteValueInCell(ReportExcel, "Description", rowindex, 2, 10, true, 250, 0, i);

                rowindex = rowindex + 2;
                //Binding Codelist  

                for (j = 0; j < Codelist.Code.Count; j++)
                {
                    Code = new SDMXApi_2_0.Structure.CodeType();
                    Code = Codelist.Code[j];
                    CodeValue = Code.value;
                    CodeDescription = GetLangSpecificValueFor_Version_2_0(Code.Description, Language);
                    if ((CodeValue.Length + 1) <= 30)
                    {
                        this.WriteValueInCell(ReportExcel, CodeValue, rowindex, 1, 10, false, 30, 0, i);
                    }
                    else
                    {
                        this.WriteValueInCell(ReportExcel, CodeValue, rowindex, 1, 10, false, CodeValue.Length + 1, 0, i);
                    }

                    this.WriteValueInCell(ReportExcel, CodeDescription, rowindex, 2, 10, false, 250, 0, i);
                    rowindex = rowindex + 1;

                }
            }

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
        }

        return ReportExcel;
    }

    private string GetLangSpecificValueFor_Version_2_0(List<SDMXApi_2_0.Common.TextType> ListOfValues, string LangCode)
    {
        string Retval = string.Empty;
        if ((ListOfValues != null) && (ListOfValues.Count > 0))
        {
            foreach (SDMXApi_2_0.Common.TextType ObjectValue in ListOfValues)
            {
                if (ObjectValue.lang != null)
                {
                    if (ObjectValue.lang.ToString() == LangCode)
                    {
                        Retval = ObjectValue.Value.ToString();
                        break;
                    }
                }

            }
            if (Retval == string.Empty)
            {
                Retval = ListOfValues[0].Value.ToString();
            }
        }

        return Retval;

    }

    //updating header on generated artifacts for dsd
    //private void Update_MFD_WithHeader(string MFDId, string MFDAgencyId, string MFDVersion, string MFDName, string MFDDescription, string MFDLanguage, string DSDId, string DSDAgencyId, string DSDVersion, string sdmxFolderPath, int DbNId)
    //{
    //    SDMXObjectModel.Message.StructureType MFD;
    //    SDMXObjectModel.Structure.MetadataflowType Metadataflow;

    //    Metadataflow = null;

    //    try
    //    {
    //        MFD = new SDMXObjectModel.Message.StructureType();
    //        //MFD.Header = Header;
    //        SDMXObjectModel.Serializer.SerializeToFile(typeof(SDMXObjectModel.Message.StructureType), MFD, sdmxFolderPath + "\\Provisioning Metadata\\" + MFDId + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension);

    //    }
    //    catch (Exception ex)
    //    {
    //        Global.CreateExceptionString(ex, null);
    //        throw ex;
    //    }
    //    finally
    //    {
    //    }
    //}

    private bool UpdateCompleteWithHeader(SDMXApi_2_0.Message.HeaderType Header, string DbNId, string UserNId, string sdmxFolderPath)
    {
        bool RetVal = false;
        SDMXApi_2_0.Message.StructureType Complete;

        string completePath = sdmxFolderPath + "\\" + DevInfo.Lib.DI_LibSDMX.Constants.Complete_XML.FileName;
        Complete = (SDMXApi_2_0.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXApi_2_0.Message.StructureType), completePath);
        Complete.Header = Header;
        Complete.OrganisationSchemes = null;
        Complete.HierarchicalCodelists = null;
        Complete.Metadataflows = null;
        Complete.Processes = null;
        Complete.ReportingTaxonomies = null;
        Complete.StructureSets = null;
        Complete.CategorySchemes = null;
        Complete.Dataflows = null;

        foreach (SDMXApi_2_0.Structure.CodeListType CodeList in Complete.CodeLists)
        {
            foreach (SDMXApi_2_0.Structure.CodeType Code in CodeList.Code)
            {
                Code.Annotations = null;
            }
        }

        SDMXApi_2_0.Serializer.SerializeToFile(typeof(SDMXApi_2_0.Message.StructureType), Complete, sdmxFolderPath + "\\" + DevInfo.Lib.DI_LibSDMX.Constants.Complete_XML.FileName);
        RetVal = true;
        return RetVal;
    }

    private bool UpdateCodelistsForUploadedDSDWithHeader(SDMXApi_2_0.Message.HeaderType Header, string DbNId, string UserNId, string CodelistsFolderPath)
    {
        bool RetVal;
        SDMXApi_2_0.Message.StructureType CodelistStructure;
        RetVal = true;
        try
        {

            string[] Files = Directory.GetFiles(CodelistsFolderPath);
            foreach (string codilistFile in Files)
            {
                CodelistStructure = new SDMXApi_2_0.Message.StructureType();
                CodelistStructure = (SDMXApi_2_0.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXApi_2_0.Message.StructureType), codilistFile);
                CodelistStructure.Header = Header;
                CodelistStructure.OrganisationSchemes = null;
                CodelistStructure.HierarchicalCodelists = null;
                CodelistStructure.Concepts = null;
                CodelistStructure.Metadataflows = null;
                CodelistStructure.MetadataStructureDefinitions = null;
                CodelistStructure.Processes = null;
                CodelistStructure.ReportingTaxonomies = null;
                CodelistStructure.StructureSets = null;
                CodelistStructure.CategorySchemes = null;
                CodelistStructure.KeyFamilies = null;
                CodelistStructure.Dataflows = null;
                // Global.deleteFiles(codilistFile);
                Remove_Extra_Annotations(CodelistStructure);
                SDMXApi_2_0.Serializer.SerializeToFile(typeof(SDMXApi_2_0.Message.StructureType), CodelistStructure, codilistFile);
            }



        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("Creating Codelists For Uploaded DSD from Admin");
            //Global.WriteErrorsInLog(ex.StackTrace);
            //Global.WriteErrorsInLog(ex.Message);
            RetVal = false;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {

        }

        return RetVal;
    }

    private bool UpdateCategorySchemeForUploadedDSDWithHeader(string DbNId, string UserNId, string DataBaseNId)
    {
        bool RetVal;
        SDMXObjectModel.Message.StructureType CategorySchemeStructure;
        SDMXObjectModel.Message.StructureType ProvisioningMetadata;
        SDMXObjectModel.Message.StructureType ProvisionAgreement;
        RetVal = true;
        string UploadedHeaderFileWPath = string.Empty;
        string UploadedHeaderFolderPath = Server.MapPath("../../stock/data");
        string UploadedHeaderName = string.Empty;
        string CategorySchemeFolderPath = string.Empty;
        string ProvisonMetadataFolderPath = string.Empty;
        string ProvisionAgreementFolderPath = string.Empty;
        XmlDocument UploadedHeaderXml = new XmlDocument();
        string[] Files = null;
        string[] PAs = null;
        string[] PMetadata = null;
        string filename = string.Empty;
        try
        {
            UploadedHeaderFileWPath = UploadedHeaderFolderPath + "/" + DataBaseNId + "/" + "sdmx" + "/" + DevInfo.Lib.DI_LibSDMX.Constants.Header.FileName;
            CategorySchemeFolderPath = UploadedHeaderFolderPath + "/" + DbNId + "/" + "sdmx" + "/" + "Categories";
            ProvisonMetadataFolderPath = UploadedHeaderFolderPath + "/" + DbNId + "/" + "sdmx" + "/" + "Provisioning Metadata";
            ProvisionAgreementFolderPath = UploadedHeaderFolderPath + "/" + DbNId + "/" + "sdmx" + "/" + "Provisioning Metadata" + "/" + "PAs";
            UploadedHeaderXml.Load(UploadedHeaderFileWPath);
            SDMXObjectModel.Message.StructureType UploadedDSDStructure = new SDMXObjectModel.Message.StructureType();
            SDMXObjectModel.Message.StructureHeaderType Header = new SDMXObjectModel.Message.StructureHeaderType();
            UploadedDSDStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureType), UploadedHeaderXml);
            Header = UploadedDSDStructure.Header;

            Files = Directory.GetFiles(CategorySchemeFolderPath);
            foreach (string categorySchemeFile in Files)
            {
                CategorySchemeStructure = new SDMXObjectModel.Message.StructureType();
                CategorySchemeStructure = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), categorySchemeFile);
                CategorySchemeStructure.Header = Header;
                CategorySchemeStructure.Structures.Categorisations = null;
                CategorySchemeStructure.Structures.Codelists = null;
                CategorySchemeStructure.Structures.HierarchicalCodelists = null;
                CategorySchemeStructure.Structures.Concepts = null;
                CategorySchemeStructure.Structures.Metadataflows = null;
                CategorySchemeStructure.Structures.MetadataStructures = null;
                CategorySchemeStructure.Structures.ProvisionAgreements = null;
                CategorySchemeStructure.Structures.ReportingTaxonomies = null;
                CategorySchemeStructure.Structures.StructureSets = null;
                CategorySchemeStructure.Structures.OrganisationSchemes = null;
                CategorySchemeStructure.Structures.Dataflows = null;
                CategorySchemeStructure.Structures.Constraints = null;
                CategorySchemeStructure.Structures.DataStructures = null;
                CategorySchemeStructure.Structures.Processes = null;
                CategorySchemeStructure.Footer = null;
                Remove_Extra_Annotations(CategorySchemeStructure);
                SDMXObjectModel.Serializer.SerializeToFile(typeof(SDMXObjectModel.Message.StructureType), CategorySchemeStructure, categorySchemeFile);
            }

            PAs = Directory.GetFiles(ProvisionAgreementFolderPath);
            foreach (string paFile in PAs)
            {
                ProvisionAgreement = new SDMXObjectModel.Message.StructureType();
                ProvisionAgreement = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), paFile);
                ProvisionAgreement.Header = Header;
                ProvisionAgreement.Structures.Categorisations = null;
                ProvisionAgreement.Structures.Codelists = null;
                ProvisionAgreement.Structures.HierarchicalCodelists = null;
                ProvisionAgreement.Structures.Concepts = null;
                ProvisionAgreement.Structures.Metadataflows = null;
                ProvisionAgreement.Structures.MetadataStructures = null;
                ProvisionAgreement.Structures.ReportingTaxonomies = null;
                ProvisionAgreement.Structures.StructureSets = null;
                ProvisionAgreement.Structures.OrganisationSchemes = null;
                ProvisionAgreement.Structures.Dataflows = null;
                ProvisionAgreement.Structures.Constraints = null;
                ProvisionAgreement.Structures.DataStructures = null;
                ProvisionAgreement.Structures.Processes = null;
                ProvisionAgreement.Structures.CategorySchemes = null;
                ProvisionAgreement.Footer = null;
                Remove_Extra_Annotations(ProvisionAgreement);
                SDMXObjectModel.Serializer.SerializeToFile(typeof(SDMXObjectModel.Message.StructureType), ProvisionAgreement, paFile);
            }

            PMetadata = Directory.GetFiles(ProvisonMetadataFolderPath);
            foreach (string pmetadataFile in PMetadata)
            {
                filename = string.Empty;
                ProvisioningMetadata = new SDMXObjectModel.Message.StructureType();
                ProvisioningMetadata = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), pmetadataFile);
                ProvisioningMetadata.Header = Header;
                filename = ExtractFilename(pmetadataFile);
                if (filename.Contains("DF"))
                {
                    ProvisioningMetadata.Structures.Metadataflows = null;
                }
                else
                {
                    ProvisioningMetadata.Structures.Dataflows = null;
                }
                ProvisioningMetadata.Structures.Categorisations = null;
                ProvisioningMetadata.Structures.Codelists = null;
                ProvisioningMetadata.Structures.HierarchicalCodelists = null;
                ProvisioningMetadata.Structures.Concepts = null;
                ProvisioningMetadata.Structures.MetadataStructures = null;
                ProvisioningMetadata.Structures.ProvisionAgreements = null;
                ProvisioningMetadata.Structures.ReportingTaxonomies = null;
                ProvisioningMetadata.Structures.StructureSets = null;
                ProvisioningMetadata.Structures.OrganisationSchemes = null;
                ProvisioningMetadata.Structures.Constraints = null;
                ProvisioningMetadata.Structures.DataStructures = null;
                ProvisioningMetadata.Structures.Processes = null;
                ProvisioningMetadata.Structures.CategorySchemes = null;
                ProvisioningMetadata.Footer = null;
                Remove_Extra_Annotations(ProvisioningMetadata);
                SDMXObjectModel.Serializer.SerializeToFile(typeof(SDMXObjectModel.Message.StructureType), ProvisioningMetadata, pmetadataFile);
            }
        }
        catch (Exception ex)
        {
            RetVal = false;
            Global.CreateExceptionString(ex, null);
            //Global.WriteErrorsInLog("Creating CategoryScheme For Uploaded DSD From Admin");
            //Global.WriteErrorsInLog(ex.StackTrace);
            //Global.WriteErrorsInLog(ex.Message);
        }
        finally
        {

        }

        return RetVal;
    }

    private bool UpdateArtefactsForUploadedDSDWithHeader(SDMXApi_2_0.Message.HeaderType Header, string DbNId, string UserNId, string sdmxFolderPath, string DatabaseNIDVersion2_1)
    {
        bool RetVal;
        SDMXApi_2_0.Message.StructureType DSD;
        SDMXApi_2_0.Message.StructureType MSD;
        SDMXApi_2_0.Message.StructureType MSDConcepts;
        SDMXApi_2_0.Message.StructureType Summary;
        string AgencyId, AgencyName, Language, MFDDFolderPathInMetadataFolder;
        string AppPhysicalPath;
        string SummaryFile;
        string DSDFile;
        string DbFolder;
        string ConsumerFileName, ProviderFileName;
        string ConceptSchemeFolder;
        string MSDFolder;

        string[] conceptSchemes;
        string[] MSDS;
        RetVal = true;
        AgencyId = string.Empty;
        AgencyName = string.Empty;
        Language = string.Empty;
        MFDDFolderPathInMetadataFolder = string.Empty;
        ConsumerFileName = string.Empty;
        ProviderFileName = string.Empty;
        ConceptSchemeFolder = string.Empty;
        MSDFolder = string.Empty;

        try
        {

            AgencyId = DevInfo.Lib.DI_LibSDMX.Constants.MaintenanceAgencyScheme.Prefix + UserNId;
            AgencyName = Global.GetDbDetails(DbNId.ToString(), Language)[0];
            AppPhysicalPath = HttpContext.Current.Request.PhysicalApplicationPath;
            DbFolder = Constants.FolderName.Data + DbNId.ToString() + "\\";


            DSD = new SDMXApi_2_0.Message.StructureType();
            DSDFile = sdmxFolderPath + "\\" + DevInfo.Lib.DI_LibSDMX.Constants.DSD.FileName;
            DSD = (SDMXApi_2_0.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXApi_2_0.Message.StructureType), DSDFile);
            DSD.Header = Header;
            DSD.OrganisationSchemes = null;
            DSD.HierarchicalCodelists = null;
            DSD.Concepts = null;
            DSD.Metadataflows = null;
            DSD.MetadataStructureDefinitions = null;
            DSD.Processes = null;
            DSD.ReportingTaxonomies = null;
            DSD.StructureSets = null;
            DSD.CategorySchemes = null;
            DSD.CodeLists = null;
            DSD.Dataflows = null;

            Remove_Extra_Annotations(DSD);
            //Creating DSD and saving its information in the Database.mdb
            SDMXApi_2_0.Serializer.SerializeToFile(typeof(SDMXApi_2_0.Message.StructureType), DSD, sdmxFolderPath + "\\" + DevInfo.Lib.DI_LibSDMX.Constants.DSD.FileName);



            SummaryFile = sdmxFolderPath + "\\" + DevInfo.Lib.DI_LibSDMX.Constants.Summary_XML.FileName;
            Summary = new SDMXApi_2_0.Message.StructureType();
            Summary = (SDMXApi_2_0.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXApi_2_0.Message.StructureType), SummaryFile);

            ConceptSchemeFolder = sdmxFolderPath + "/" + "Concepts";
            MSDFolder = sdmxFolderPath + "/" + "MSD";
            Summary.Header = Header;
            Summary.OrganisationSchemes = null;
            Summary.HierarchicalCodelists = null;
            Summary.Metadataflows = null;
            Summary.Processes = null;
            Summary.ReportingTaxonomies = null;
            Summary.StructureSets = null;
            Summary.CategorySchemes = null;
            Summary.Dataflows = null;

            //Creating Summary file and saving its information in the Database.mdb
            SDMXApi_2_0.Serializer.SerializeToFile(typeof(SDMXApi_2_0.Message.StructureType), Summary, sdmxFolderPath + "\\" + DevInfo.Lib.DI_LibSDMX.Constants.Summary_XML.FileName);

            MSDS = Directory.GetFiles(MSDFolder);
            foreach (string msdfile in MSDS)
            {
                MSD = new SDMXApi_2_0.Message.StructureType();
                MSD = (SDMXApi_2_0.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXApi_2_0.Message.StructureType), msdfile);
                MSD.Header = Header;
                MSD.OrganisationSchemes = null;
                MSD.HierarchicalCodelists = null;
                MSD.Concepts = null;
                MSD.Metadataflows = null;
                MSD.KeyFamilies = null;
                MSD.Processes = null;
                MSD.ReportingTaxonomies = null;
                MSD.StructureSets = null;
                MSD.CategorySchemes = null;
                MSD.CodeLists = null;
                MSD.Dataflows = null;
                //Creating MSD and saving its information in the Database.mdb

                SDMXApi_2_0.Serializer.SerializeToFile(typeof(SDMXApi_2_0.Message.StructureType), MSD, msdfile);//sdmxFolderPath + "\\MSD\\"
            }

            conceptSchemes = Directory.GetFiles(ConceptSchemeFolder);
            foreach (string conceptSchemeFile in conceptSchemes)
            {
                MSDConcepts = new SDMXApi_2_0.Message.StructureType();
                MSDConcepts = (SDMXApi_2_0.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXApi_2_0.Message.StructureType), conceptSchemeFile);
                MSDConcepts.Header = Header;
                MSDConcepts.MetadataStructureDefinitions = null;
                MSDConcepts.OrganisationSchemes = null;
                MSDConcepts.HierarchicalCodelists = null;
                MSDConcepts.Metadataflows = null;
                MSDConcepts.KeyFamilies = null;
                MSDConcepts.Processes = null;
                MSDConcepts.ReportingTaxonomies = null;
                MSDConcepts.StructureSets = null;
                MSDConcepts.CategorySchemes = null;
                MSDConcepts.CodeLists = null;
                MSDConcepts.Dataflows = null;

                //Creating MSDConcepts and saving its information in the Database.mdb
                SDMXApi_2_0.Serializer.SerializeToFile(typeof(SDMXApi_2_0.Message.StructureType), MSDConcepts, conceptSchemeFile);//sdmxFolderPath + "\\Concepts\\" +

            }
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("Creating Artefacts For Uploaded DSD From Admin");
            //Global.WriteErrorsInLog(ex.StackTrace);
            //Global.WriteErrorsInLog(ex.Message);
            RetVal = false;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {

        }

        return RetVal;
    }

    private bool UpdateNonMAForUploadedDBWithHeader(string DbNId, string UserNId, string DataBaseNId)
    {
        bool RetVal;
        SDMXObjectModel.Message.RegistryInterfaceType Registrations;
        SDMXObjectModel.Message.GenericMetadataType MetadataFiles;
        //SDMXObjectModel.Message.GenericDataType DataFiles;
        SDMXObjectModel.Message.StructureSpecificTimeSeriesDataType DataFiles;

        SDMXObjectModel.Message.RegistryInterfaceType Subscriptions;
        RetVal = true;
        string UploadedHeaderFileWPath = string.Empty;
        string UploadedHeaderFolderPath = Server.MapPath("../../stock/data");
        string UploadedHeaderName = string.Empty;
        string SubscriptionsFolderPath = string.Empty;
        string MetadataFolderPath = string.Empty;
        string RegistrationsFolderPath = string.Empty;
        string SDMXMLFolderPath = string.Empty;
        string MappingFolderPath = string.Empty;
        XmlDocument UploadedHeaderXml = new XmlDocument();
        FileInfo[] Files = null;

        DirectoryInfo dirRegs = null;
        DirectoryInfo dirMetadata = null;
        DirectoryInfo dirSubscriptions = null;
        DirectoryInfo dirSDMXML = null;


        try
        {
            UploadedHeaderFileWPath = UploadedHeaderFolderPath + "/" + DataBaseNId + "/" + "sdmx" + "/" + DevInfo.Lib.DI_LibSDMX.Constants.Header.FileName;
            dirRegs = new DirectoryInfo(UploadedHeaderFolderPath + "\\" + DbNId + "\\" + "sdmx" + "\\" + "Registrations");
            dirMetadata = new DirectoryInfo(UploadedHeaderFolderPath + "\\" + DbNId + "\\" + "sdmx" + "\\" + "Metadata");
            dirSDMXML = new DirectoryInfo(UploadedHeaderFolderPath + "\\" + DbNId + "\\" + "sdmx" + "\\" + "SDMX-ML");
            dirSubscriptions = new DirectoryInfo(UploadedHeaderFolderPath + "\\" + DbNId + "\\" + "sdmx" + "\\" + "Subscriptions");
            MappingFolderPath = UploadedHeaderFolderPath + "/" + DbNId + "/" + "sdmx" + "/" + "Mappings";
            UploadedHeaderXml.Load(UploadedHeaderFileWPath);
            SDMXObjectModel.Message.StructureType UploadedDSDStructure = new SDMXObjectModel.Message.StructureType();
            SDMXObjectModel.Message.StructureHeaderType Header = new SDMXObjectModel.Message.StructureHeaderType();
            SDMXObjectModel.Message.BasicHeaderType BHeader = new SDMXObjectModel.Message.BasicHeaderType();

            UploadedDSDStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureType), UploadedHeaderXml);
            Header = UploadedDSDStructure.Header;
            //  BHeader = (SDMXObjectModel.Message.BasicHeaderType)UploadedDSDStructure.Header;
            foreach (DirectoryInfo dirReg in dirRegs.GetDirectories())
            {
                Files = dirReg.GetFiles();
                foreach (FileInfo regfile in Files)
                {
                    Registrations = new SDMXObjectModel.Message.RegistryInterfaceType();

                    Registrations = (SDMXObjectModel.Message.RegistryInterfaceType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.RegistryInterfaceType), UploadedHeaderFolderPath + "\\" + DbNId + "\\" + "sdmx" + "\\" + "Registrations" + "\\" + dirReg.Name + "\\" + regfile.ToString());

                    Registrations.Header.ID = Header.ID.ToString();
                    Registrations.Header.Prepared = Header.Prepared.ToString();
                    foreach (PartyType receiver in Header.Receiver)
                    {
                        Registrations.Header.Receiver.Contact = receiver.Contact;
                        Registrations.Header.Receiver.id = receiver.id;
                        Registrations.Header.Receiver.Name = receiver.Name;
                    }
                    Registrations.Header.Sender = (SDMXObjectModel.Message.SenderType)Header.Sender;
                    Registrations.Header.Test = Header.Test;

                    Registrations.Footer = null;
                    SDMXObjectModel.Serializer.SerializeToFile(typeof(SDMXObjectModel.Message.RegistryInterfaceType), Registrations, UploadedHeaderFolderPath + "\\" + DbNId + "\\" + "sdmx" + "\\" + "Registrations" + "\\" + dirReg.Name + "\\" + regfile.ToString());
                }
            }

            foreach (DirectoryInfo dirMeta in dirMetadata.GetDirectories())
            {
                Files = null;
                Files = dirMeta.GetFiles();
                foreach (FileInfo metafile in Files)
                {
                    MetadataFiles = new SDMXObjectModel.Message.GenericMetadataType();

                    MetadataFiles = (SDMXObjectModel.Message.GenericMetadataType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.GenericMetadataType), UploadedHeaderFolderPath + "\\" + DbNId + "\\" + "sdmx" + "\\" + "Metadata" + "\\" + dirMeta.Name + "\\" + metafile.ToString());

                    MetadataFiles.Header.ID = Header.ID.ToString();
                    MetadataFiles.Header.Prepared = Header.Prepared.ToString();
                    MetadataFiles.Header.Receiver = Header.Receiver;
                    MetadataFiles.Header.Sender = (SDMXObjectModel.Message.SenderType)Header.Sender;
                    MetadataFiles.Header.Test = Header.Test;

                    MetadataFiles.Footer = null;
                    SDMXObjectModel.Serializer.SerializeToFile(typeof(SDMXObjectModel.Message.GenericMetadataType), MetadataFiles, UploadedHeaderFolderPath + "\\" + DbNId + "\\" + "sdmx" + "\\" + "Metadata" + "\\" + dirMeta.Name + "\\" + metafile.ToString());
                }
            }

            foreach (DirectoryInfo dirSDMX in dirSDMXML.GetDirectories())
            {
                Files = null;
                Files = dirSDMX.GetFiles();
                foreach (FileInfo sdmxMlFile in Files)
                {
                    DataFiles = new SDMXObjectModel.Message.StructureSpecificTimeSeriesDataType();

                    DataFiles = (SDMXObjectModel.Message.StructureSpecificTimeSeriesDataType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureSpecificTimeSeriesDataType), UploadedHeaderFolderPath + "\\" + DbNId + "\\" + "sdmx" + "\\" + "SDMX-ML" + "\\" + dirSDMX.Name + "\\" + sdmxMlFile.ToString());

                    DataFiles.Header.ID = Header.ID.ToString();
                    DataFiles.Header.Prepared = Header.Prepared.ToString();
                    DataFiles.Header.Receiver = Header.Receiver;
                    DataFiles.Header.Sender = (SDMXObjectModel.Message.SenderType)Header.Sender;
                    DataFiles.Header.Test = Header.Test;

                    DataFiles.Footer = null;
                    SDMXObjectModel.Serializer.SerializeToFile(typeof(SDMXObjectModel.Message.StructureSpecificTimeSeriesDataType), DataFiles, UploadedHeaderFolderPath + "\\" + DbNId + "\\" + "sdmx" + "\\" + "SDMX-ML" + "\\" + dirSDMX.Name + "\\" + sdmxMlFile.ToString());
                }
            }

            foreach (DirectoryInfo dirSubs in dirSubscriptions.GetDirectories())
            {
                Files = null;
                Files = dirSubs.GetFiles();
                foreach (FileInfo subsFile in Files)
                {
                    Subscriptions = new SDMXObjectModel.Message.RegistryInterfaceType();

                    Subscriptions = (SDMXObjectModel.Message.RegistryInterfaceType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.RegistryInterfaceType), UploadedHeaderFolderPath + "\\" + DbNId + "\\" + "sdmx" + "\\" + "Subscriptions" + "\\" + dirSubs.Name + "\\" + subsFile.ToString());

                    Subscriptions.Header.ID = Header.ID.ToString();
                    Subscriptions.Header.Prepared = Header.Prepared.ToString();
                    foreach (PartyType receiver in Header.Receiver)
                    {
                        Subscriptions.Header.Receiver.Contact = receiver.Contact;
                        Subscriptions.Header.Receiver.id = receiver.id;
                        Subscriptions.Header.Receiver.Name = receiver.Name;
                    }
                    Subscriptions.Header.Sender = (SDMXObjectModel.Message.SenderType)Header.Sender;
                    Subscriptions.Header.Test = Header.Test;

                    Subscriptions.Footer = null;
                    SDMXObjectModel.Serializer.SerializeToFile(typeof(SDMXObjectModel.Message.RegistryInterfaceType), Subscriptions, UploadedHeaderFolderPath + "\\" + DbNId + "\\" + "sdmx" + "\\" + "Subscriptions" + "\\" + dirSubs.Name + "\\" + subsFile.ToString());
                }
            }


        }
        catch (Exception ex)
        {
            RetVal = false;
            Global.CreateExceptionString(ex, null);
            //Global.WriteErrorsInLog("Creating CategoryScheme For Uploaded DSD From Admin");
            //Global.WriteErrorsInLog(ex.StackTrace);
            //Global.WriteErrorsInLog(ex.Message);
        }
        finally
        {

        }

        return RetVal;
    }


    private bool UpdateNonMAForUploadedDSDWithHeader(string DbNId, string UserNId, string DataBaseNId)
    {
        bool RetVal;
        SDMXObjectModel.Message.RegistryInterfaceType Registrations;
        SDMXApi_2_0.Message.GenericMetadataType MetadataFiles;
        //SDMXObjectModel.Message.GenericDataType DataFiles;

        SDMXApi_2_0.Message.CompactDataType DataFiles;
        SDMXObjectModel.Message.RegistryInterfaceType Subscriptions;
        RetVal = true;
        string UploadedHeaderFileWPath = string.Empty;
        string UploadedHeaderFolderPath = Server.MapPath("../../stock/data");
        string UploadedDSDHeaderFilePath = string.Empty;
        string UploadedHeaderName = string.Empty;
        string SubscriptionsFolderPath = string.Empty;
        string MetadataFolderPath = string.Empty;
        string RegistrationsFolderPath = string.Empty;
        string SDMXMLFolderPath = string.Empty;
        string MappingFolderPath = string.Empty;
        XmlDocument UploadedHeaderXml = new XmlDocument();
        XmlDocument UploadedHeaderXmlFor2_0 = new XmlDocument();
        FileInfo[] Files = null;

        DirectoryInfo dirRegs = null;
        DirectoryInfo dirMetadata = null;
        DirectoryInfo dirSubscriptions = null;
        DirectoryInfo dirSDMXML = null;


        try
        {
            UploadedHeaderFileWPath = UploadedHeaderFolderPath + "/" + DataBaseNId + "/" + "sdmx" + "/" + DevInfo.Lib.DI_LibSDMX.Constants.Header.FileName;
            UploadedDSDHeaderFilePath = UploadedHeaderFolderPath + "/" + DbNId + "/" + "sdmx" + "/" + DevInfo.Lib.DI_LibSDMX.Constants.Header.FileName;
            dirRegs = new DirectoryInfo(UploadedHeaderFolderPath + "\\" + DbNId + "\\" + "sdmx" + "\\" + "Registrations");
            dirMetadata = new DirectoryInfo(UploadedHeaderFolderPath + "\\" + DbNId + "\\" + "sdmx" + "\\" + "Metadata");
            dirSDMXML = new DirectoryInfo(UploadedHeaderFolderPath + "\\" + DbNId + "\\" + "sdmx" + "\\" + "SDMX-ML");
            dirSubscriptions = new DirectoryInfo(UploadedHeaderFolderPath + "\\" + DbNId + "\\" + "sdmx" + "\\" + "Subscriptions");
            MappingFolderPath = UploadedHeaderFolderPath + "/" + DbNId + "/" + "sdmx" + "/" + "Mappings";
            UploadedHeaderXml.Load(UploadedHeaderFileWPath);
            UploadedHeaderXmlFor2_0.Load(UploadedDSDHeaderFilePath);
            SDMXObjectModel.Message.StructureType UploadedDSDStructure = new SDMXObjectModel.Message.StructureType();
            SDMXObjectModel.Message.StructureHeaderType Header = new SDMXObjectModel.Message.StructureHeaderType();

            SDMXApi_2_0.Message.StructureType UploadedDSDStruct20 = new SDMXApi_2_0.Message.StructureType();
            SDMXApi_2_0.Message.HeaderType DSDHeader = new SDMXApi_2_0.Message.HeaderType();

            UploadedDSDStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureType), UploadedHeaderXml);
            Header = UploadedDSDStructure.Header;
            UploadedDSDStruct20 = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromXmlDocument(typeof(SDMXApi_2_0.Message.StructureType), UploadedHeaderXmlFor2_0);
            DSDHeader = UploadedDSDStruct20.Header;

            foreach (DirectoryInfo dirReg in dirRegs.GetDirectories())
            {
                Files = dirReg.GetFiles();
                foreach (FileInfo regfile in Files)
                {
                    Registrations = new SDMXObjectModel.Message.RegistryInterfaceType();

                    Registrations = (SDMXObjectModel.Message.RegistryInterfaceType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.RegistryInterfaceType), UploadedHeaderFolderPath + "\\" + DbNId + "\\" + "sdmx" + "\\" + "Registrations" + "\\" + dirReg.Name + "\\" + regfile.ToString());

                    Registrations.Header.ID = Header.ID.ToString();
                    Registrations.Header.Prepared = Header.Prepared.ToString();
                    foreach (PartyType receiver in Header.Receiver)
                    {
                        Registrations.Header.Receiver.Contact = receiver.Contact;
                        Registrations.Header.Receiver.id = receiver.id;
                        Registrations.Header.Receiver.Name = receiver.Name;
                    }
                    Registrations.Header.Sender = (SDMXObjectModel.Message.SenderType)Header.Sender;
                    Registrations.Header.Test = Header.Test;

                    Registrations.Footer = null;
                    SDMXObjectModel.Serializer.SerializeToFile(typeof(SDMXObjectModel.Message.RegistryInterfaceType), Registrations, UploadedHeaderFolderPath + "\\" + DbNId + "\\" + "sdmx" + "\\" + "Registrations" + "\\" + dirReg.Name + "\\" + regfile.ToString());
                }
            }

            foreach (DirectoryInfo dirMeta in dirMetadata.GetDirectories())
            {
                Files = null;
                Files = dirMeta.GetFiles();
                foreach (FileInfo metafile in Files)
                {
                    MetadataFiles = new SDMXApi_2_0.Message.GenericMetadataType();

                    MetadataFiles = (SDMXApi_2_0.Message.GenericMetadataType)Deserializer.LoadFromFile(typeof(SDMXApi_2_0.Message.GenericMetadataType), UploadedHeaderFolderPath + "\\" + DbNId + "\\" + "sdmx" + "\\" + "Metadata" + "\\" + dirMeta.Name + "\\" + metafile.ToString());

                    MetadataFiles.Header.ID = DSDHeader.ID.ToString();
                    MetadataFiles.Header.Prepared = DSDHeader.Prepared.ToString();
                    MetadataFiles.Header.Receiver = DSDHeader.Receiver;
                    MetadataFiles.Header.Sender = DSDHeader.Sender;
                    MetadataFiles.Header.Test = DSDHeader.Test;

                    SDMXApi_2_0.Serializer.SerializeToFile(typeof(SDMXApi_2_0.Message.GenericMetadataType), MetadataFiles, UploadedHeaderFolderPath + "\\" + DbNId + "\\" + "sdmx" + "\\" + "Metadata" + "\\" + dirMeta.Name + "\\" + metafile.ToString());
                }
            }

            foreach (DirectoryInfo dirSDMX in dirSDMXML.GetDirectories())
            {
                Files = null;
                Files = dirSDMX.GetFiles();
                foreach (FileInfo sdmxMlFile in Files)
                {
                    DataFiles = new SDMXApi_2_0.Message.CompactDataType();

                    DataFiles = (SDMXApi_2_0.Message.CompactDataType)Deserializer.LoadFromFile(typeof(SDMXApi_2_0.Message.CompactDataType), UploadedHeaderFolderPath + "\\" + DbNId + "\\" + "sdmx" + "\\" + "SDMX-ML" + "\\" + dirSDMX.Name + "\\" + sdmxMlFile.ToString());

                    DataFiles.Header.ID = DSDHeader.ID.ToString();
                    DataFiles.Header.Prepared = Header.Prepared.ToString();
                    DataFiles.Header.Receiver = DSDHeader.Receiver;
                    DataFiles.Header.Sender = DSDHeader.Sender;
                    DataFiles.Header.Test = DSDHeader.Test;

                    SDMXApi_2_0.Serializer.SerializeToFile(typeof(SDMXApi_2_0.Message.CompactDataType), DataFiles, UploadedHeaderFolderPath + "\\" + DbNId + "\\" + "sdmx" + "\\" + "SDMX-ML" + "\\" + dirSDMX.Name + "\\" + sdmxMlFile.ToString());
                }
            }

            foreach (DirectoryInfo dirSubs in dirSubscriptions.GetDirectories())
            {
                Files = null;
                Files = dirSubs.GetFiles();
                foreach (FileInfo subsFile in Files)
                {
                    Subscriptions = new SDMXObjectModel.Message.RegistryInterfaceType();

                    Subscriptions = (SDMXObjectModel.Message.RegistryInterfaceType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.RegistryInterfaceType), UploadedHeaderFolderPath + "\\" + DbNId + "\\" + "sdmx" + "\\" + "Subscriptions" + "\\" + dirSubs.Name + "\\" + subsFile.ToString());

                    Subscriptions.Header.ID = Header.ID.ToString();
                    Subscriptions.Header.Prepared = Header.Prepared.ToString();
                    foreach (PartyType receiver in Header.Receiver)
                    {
                        Subscriptions.Header.Receiver.Contact = receiver.Contact;
                        Subscriptions.Header.Receiver.id = receiver.id;
                        Subscriptions.Header.Receiver.Name = receiver.Name;
                    }
                    Subscriptions.Header.Sender = (SDMXObjectModel.Message.SenderType)Header.Sender;
                    Subscriptions.Header.Test = Header.Test;

                    Subscriptions.Footer = null;
                    SDMXObjectModel.Serializer.SerializeToFile(typeof(SDMXObjectModel.Message.RegistryInterfaceType), Subscriptions, UploadedHeaderFolderPath + "\\" + DbNId + "\\" + "sdmx" + "\\" + "Subscriptions" + "\\" + dirSubs.Name + "\\" + subsFile.ToString());
                }
            }


        }
        catch (Exception ex)
        {
            RetVal = false;
            Global.CreateExceptionString(ex, null);
            //Global.WriteErrorsInLog("Creating CategoryScheme For Uploaded DSD From Admin");
            //Global.WriteErrorsInLog(ex.StackTrace);
            //Global.WriteErrorsInLog(ex.Message);
        }
        finally
        {

        }

        return RetVal;
    }


    private void Remove_Extra_Annotations(SDMXApi_2_0.Message.StructureType Structure)
    {
        if (Structure.CodeLists != null)
        {
            foreach (SDMXApi_2_0.Structure.CodeListType Codelist in Structure.CodeLists)
            {
                foreach (SDMXApi_2_0.Structure.CodeType Code in Codelist.Code)
                {
                    Code.Annotations = null;
                }
            }
        }

        if (Structure.CategorySchemes != null)
        {
            foreach (SDMXApi_2_0.Structure.CategorySchemeType CategoryScheme in Structure.CategorySchemes)
            {
                CategoryScheme.Annotations = null;

                foreach (SDMXApi_2_0.Structure.CategoryType Category in CategoryScheme.Category)
                {
                    Category.Annotations = null;
                    this.Remove_Extra_Annotations_From_Child_Categories(Category);
                }
            }
        }

        if (Structure.KeyFamilies != null)
        {
            foreach (SDMXApi_2_0.Structure.KeyFamilyType Kef in Structure.KeyFamilies)
            {
                Kef.Components.Dimension[0].Annotations = null;


            }
        }

        // Removing annotation at ConceptScheme level
        if (Structure.Concepts != null)
        {
            //SDMXApi_2_0.Structure.ConceptSchemeType ConceptScheme in Structure.Concepts
            foreach (SDMXApi_2_0.Structure.ConceptType ConceptScheme in Structure.Concepts.Concept)
            {
                ConceptScheme.Annotations = null;


            }
        }

        // Removing annotation at MetadataStructure level
        //if (Structure.MetadataStructureDefinitions != null)
        //{
        //    foreach (SDMXApi_2_0.Structure.MetadataStructureDefinitionsType MetadataStructure in Structure.MetadataStructureDefinitions.)
        //    {
        //        MetadataStructure.MetadataStructureDefinition[0].Annotations = null;

        //        // Removing annotation at MetadataTarget level
        //        MetadataStructure.Item.Items[0].Annotations = null;

        //        // Removing annotation at IdentifiableObjectTarget level
        //        MetadataStructure.Item.Items[0].Items[0].Annotations = null;

        //        // Removing annotation at ReportStructure level
        //        MetadataStructure.Item.Items[1].Annotations = null;

        //        // Removing annotation at MetadataAttribute level
        //        foreach (MetadataAttributeType MetadataAttribute in MetadataStructure.Item.Items[1].Items)
        //        {
        //            MetadataAttribute.Annotations = null;
        //        }
        //    }
        //}


    }

    private void Remove_Extra_Annotations_From_Child_Categories(SDMXApi_2_0.Structure.CategoryType ParentCategory)
    {
        foreach (SDMXApi_2_0.Structure.CategoryType ChildCategory in ParentCategory.Category)
        {
            ChildCategory.Annotations = null;
            this.Remove_Extra_Annotations_From_Child_Categories(ChildCategory);
        }
    }

    #endregion "--SDMX Artefacts Generation For Uploaded DSD--"

    #region "--Displaying All Connections with SDMX Artefact Publish Details--"

    private string GetAllDbConnectionsWithPublishDetails(string CategoryName, string UseNId)
    {
        string RetVal = string.Empty;
        string DBFile = string.Empty;
        string DbNId = string.Empty;
        string ConnectionName = string.Empty;
        string DatabaseType = string.Empty;
        string CreatedOn = string.Empty;
        string Publisher = string.Empty;

        DIConnection DIConnection = null;
        string Query = string.Empty;
        DataTable DtDFD;

        XmlDocument XmlDoc;
        XmlNode xmlNode;
        int i = 0;
        string DefDbNId = string.Empty;
        string ConnStr = string.Empty;
        string DbType = string.Empty;


        try
        {
            DefDbNId = Global.GetDefaultDbNId();

            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                     string.Empty, string.Empty);

            DBFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DBFile);

            xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "[@" + Constants.XmlFile.Db.Tags.CategoryAttributes.Name + "='" + CategoryName + "']");

            RetVal = "<table style=\"width:100%; \" border=\"0\" cellSpacing=\"0\" cellSpacing=\"0\">";
            RetVal += "<tr class=\"HeaderRowStyle \">";
            RetVal += "<td class=\"HeaderColumnStyle \"></td>";
            RetVal += "<td class=\"HeaderColumnStyle \"><span id=\"lang_Connection_Name\"></span></td>";
            RetVal += "<td class=\"HeaderColumnStyle \"><span  id=\"lang_Database_Type\"></span></td>";
            RetVal += "<td class=\"HeaderColumnStyle \"><span  id=\"lang_Created_On\"></span></td>";
            RetVal += "<td class=\"HeaderColumnStyle \"><span  id=\"lang_Publisher\"></span></td>";
            RetVal += "</tr>";

            for (i = 0; i < xmlNode.ChildNodes.Count; i++)
            {
                if (xmlNode.ChildNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.SDMXDb].Value.ToLower() == "false")
                {
                    RetVal = RetVal + "<tr class=\"DataRowStyle \">";
                    DbNId = xmlNode.ChildNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Id].Value;
                    ConnectionName = xmlNode.ChildNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Name].Value;

                    ConnStr = xmlNode.ChildNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DatabaseConnection].Value;

                    if (!string.IsNullOrEmpty(ConnStr))
                    {
                        DbType = Global.SplitString(ConnStr, "||")[0];
                    }

                    if (DbType == "0")
                    {
                        DatabaseType = "Sql Server";
                    }
                    else if (DbType == "3")
                    {
                        DatabaseType = "My Sql";
                    }
                    else if (DbType == "8")
                    {
                        DatabaseType = "Firebird";
                    }
                    else
                    {
                        DatabaseType = "";
                    }

                    CreatedOn = xmlNode.ChildNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.LastModified].Value;

                    Query = "SELECT * FROM Artefacts WHERE Type=" + Convert.ToInt32(ArtefactTypes.DFD).ToString() + " And DbNId = " + xmlNode.ChildNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Id].Value + ";";

                    DtDFD = DIConnection.ExecuteDataTable(Query);
                    if (DtDFD.Rows.Count > 0)
                    {
                        Publisher = DtDFD.Rows[0]["AgencyId"].ToString();
                        //  if (Global.SplitString(Publisher, "_")[1] == UseNId)
                        ///   {
                        RetVal += "<td class=\"CheckBoxDataColumnStyle\"><input type=\"Radio\" id=\"db_" + DbNId + "\"  name=\"group1\" style=\"display:none\" /></td>";
                        //  }
                        //    else
                        //  {
                        //     RetVal += "<td class=\"CheckBoxDataColumnStyle\"></td>";
                        //  }
                    }
                    else
                    {
                        Publisher = "";
                        RetVal += "<td class=\"CheckBoxDataColumnStyle\"><input type=\"Radio\" id=\"db_" + DbNId + "\"  name=\"group1\" style=\"display:none\" /></td>";
                    }

                    RetVal += "<td class=\"DataColumnStyle\">" + ConnectionName + "</td>";
                    RetVal += "<td class=\"DataColumnStyle\">" + DatabaseType + "</td>";
                    RetVal += "<td class=\"DataColumnStyle\">" + CreatedOn + "</td>";
                    RetVal += "<td class=\"DataColumnStyle\">" + Publisher + "</td>";

                    RetVal = RetVal + "</tr>";

                }
            }
            RetVal = RetVal + "</table>";

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("Getting all database connection with Publish Details.");
            //Global.WriteErrorsInLog(ex.StackTrace);
            //Global.WriteErrorsInLog(ex.Message);
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    #endregion "--Displaying All Connections with SDMX Artefact Publish Details--"

    #region "--Send Notifications For Structral Metadata Changes--"

    private void Send_Notifications_For_Subscriptions_For_Structural_Metadata_Changes(string DbNId)
    {
        Dictionary<string, SubscriptionType> DictSubscriptions;
        List<ArtefactRef> ListOfArtefactRefs;
        bool IsAdminUploadedDSD;
        string ArtefactLanguage, ArtefactName, ArtefactDescription;

        ListOfArtefactRefs = new List<ArtefactRef>();
        ArtefactLanguage = string.Empty;
        ArtefactName = string.Empty;
        ArtefactDescription = string.Empty;

        try
        {

            IsAdminUploadedDSD = Global.IsDSDUploadedFromAdmin(Convert.ToInt32(DbNId));
            if (IsAdminUploadedDSD)
            {
                SDMXApi_2_0.Message.StructureType Summary;
                Summary = new SDMXApi_2_0.Message.StructureType();
                Summary = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromFile(typeof(SDMXApi_2_0.Message.StructureType), Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId + "\\sdmx\\Summary" + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension));

                foreach (SDMXApi_2_0.Structure.KeyFamilyType DSD in Summary.KeyFamilies)
                {
                    if ((DSD.Name != null) && (DSD.Name.Count > 0))
                    {
                        ArtefactLanguage = DSD.Name[0].lang;
                        ArtefactName = DSD.Name[0].Value;
                    }
                    if ((DSD.Description != null) && (DSD.Description.Count > 0))
                    {
                        ArtefactDescription = DSD.Description[0].Value;
                    }

                    ListOfArtefactRefs.Add(new ArtefactRef(DSD.id, DSD.agencyID, DSD.version, ArtefactLanguage, ArtefactName, ArtefactDescription, ArtefactTypes.DSD));
                }

                foreach (SDMXApi_2_0.Structure.MetadataStructureDefinitionType MSD in Summary.MetadataStructureDefinitions)
                {
                    if ((MSD.Name != null) && (MSD.Name.Count > 0))
                    {
                        ArtefactLanguage = MSD.Name[0].lang;
                        ArtefactName = MSD.Name[0].Value;
                    }
                    if ((MSD.Description != null) && (MSD.Description.Count > 0))
                    {
                        ArtefactDescription = MSD.Description[0].Value;
                    }
                    ListOfArtefactRefs.Add(new ArtefactRef(MSD.id, MSD.agencyID, MSD.version, ArtefactLanguage, ArtefactName, ArtefactDescription, ArtefactTypes.MSD));
                }
            }
            else
            {
                SDMXObjectModel.Message.StructureType Summary;
                Summary = new SDMXObjectModel.Message.StructureType();
                Summary = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId + "\\sdmx\\Summary" + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension));

                foreach (SDMXObjectModel.Structure.DataStructureType DSD in Summary.Structures.DataStructures)
                {
                    if ((DSD.Name != null) && (DSD.Name.Count > 0))
                    {
                        ArtefactLanguage = DSD.Name[0].lang;
                        ArtefactName = DSD.Name[0].Value;
                    }
                    if ((DSD.Description != null) && (DSD.Description.Count > 0))
                    {
                        ArtefactDescription = DSD.Description[0].Value;
                    }

                    ListOfArtefactRefs.Add(new ArtefactRef(DSD.id, DSD.agencyID, DSD.version, ArtefactLanguage, ArtefactName, ArtefactDescription, ArtefactTypes.DSD));
                }

                foreach (SDMXObjectModel.Structure.MetadataStructureType MSD in Summary.Structures.MetadataStructures)
                {
                    if ((MSD.Name != null) && (MSD.Name.Count > 0))
                    {
                        ArtefactLanguage = MSD.Name[0].lang;
                        ArtefactName = MSD.Name[0].Value;
                    }
                    if ((MSD.Description != null) && (MSD.Description.Count > 0))
                    {
                        ArtefactDescription = MSD.Description[0].Value;
                    }
                    ListOfArtefactRefs.Add(new ArtefactRef(MSD.id, MSD.agencyID, MSD.version, ArtefactLanguage, ArtefactName, ArtefactDescription, ArtefactTypes.MSD));
                }
            }

            DictSubscriptions = this.Get_Subscriptions_Dictionary_For_Structural_Metadata_Changes(DbNId);

            foreach (string RegistryURN in DictSubscriptions.Keys)
            {
                this.Send_Notifications_For_Current_Subscription_For_Structural_Metadata_Changes(DictSubscriptions[RegistryURN], ListOfArtefactRefs);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {

        }
    }

    private Dictionary<string, SubscriptionType> Get_Subscriptions_Dictionary_For_Structural_Metadata_Changes(string DbNId)
    {
        Dictionary<string, SubscriptionType> RetVal;
        string Query, FileNameWPath;
        DIConnection DIConnection;
        DataTable DtSubscriptions;
        SDMXObjectModel.Message.RegistryInterfaceType RegistryInterface;
        SDMXObjectModel.Registry.SubscriptionType Subscription;

        RetVal = new Dictionary<string, SubscriptionType>();
        Query = string.Empty;
        FileNameWPath = string.Empty;
        DIConnection = null;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                           string.Empty, string.Empty);
            Query = "SELECT * FROM Artefacts WHERE DBNId = " + DbNId + " AND Type = " + Convert.ToInt32(ArtefactTypes.Subscription).ToString() + ";";
            DtSubscriptions = DIConnection.ExecuteDataTable(Query);
            foreach (DataRow DrSubscriptions in DtSubscriptions.Rows)
            {
                FileNameWPath = DrSubscriptions["FileLocation"].ToString();
                RegistryInterface = (SDMXObjectModel.Message.RegistryInterfaceType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.RegistryInterfaceType), FileNameWPath);
                Subscription = ((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)RegistryInterface.Item).SubscriptionRequest[0].Subscription;
                if (Subscription.EventSelector[0] is StructuralRepositoryEventsType)
                {
                    RetVal.Add(Subscription.RegistryURN, Subscription);
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;
    }

    private void Send_Notifications_For_Current_Subscription_For_Structural_Metadata_Changes(SubscriptionType Subscription, List<ArtefactRef> ListOfArtefactRefs)
    {
        string MessageContent;
        List<ArtefactInfo> Artefacts;

        DateTime CurrentDate;
        DateTime ValidityPeriodStartDate;
        DateTime ValidityPeriodEndDate;


        try
        {
            CurrentDate = DateTime.Now;
            ValidityPeriodStartDate = Subscription.ValidityPeriod.StartDate;
            ValidityPeriodEndDate = Subscription.ValidityPeriod.EndDate;
            if ((CurrentDate >= ValidityPeriodStartDate) && (CurrentDate <= ValidityPeriodEndDate))
            {
                Artefacts = SDMXUtility.Get_Notification(SDMXSchemaType.Two_One, DateTime.Now, ListOfArtefactRefs, Subscription.RegistryURN, ActionType.Append, null);

                foreach (NotificationURLType NotificationMailId in Subscription.NotificationMailTo)
                {
                    if (NotificationMailId.isSOAP == true)
                    {
                        MessageContent = Global.GetSoapWrappedXml(Artefacts[0].Content.OuterXml);
                    }
                    else
                    {
                        MessageContent = Artefacts[0].Content.OuterXml;
                    }

                    Global.Send_Notification_By_Email(MessageContent, NotificationMailId.Value);
                }

                foreach (NotificationURLType NotificationHTTP in Subscription.NotificationHTTP)
                {
                    if (NotificationHTTP.isSOAP == true)
                    {
                        MessageContent = Global.GetSoapWrappedXml(Artefacts[0].Content.OuterXml);
                    }
                    else
                    {
                        MessageContent = Artefacts[0].Content.OuterXml;
                    }

                    Global.Send_Notification_By_HTTP_Post(MessageContent, NotificationHTTP.Value);
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
        }
    }

    #endregion "--Send Notifications For Structral Metadata Changes--"


    private static string ExtractFilename(string filepath)
    {
        // If path ends with a "\", it's a path only so return String.Empty.
        if (filepath.Trim().EndsWith(@"\"))
            return String.Empty;

        // Determine where last backslash is. 
        int position = filepath.LastIndexOf('\\');
        // If there is no backslash, assume that this is a filename. 
        if (position == -1)
        {
            // Determine whether file exists in the current directory. 
            if (File.Exists(Environment.CurrentDirectory + Path.DirectorySeparatorChar + filepath))
                return filepath;
            else
                return String.Empty;
        }
        else
        {
            // Determine whether file exists using filepath. 
            if (File.Exists(filepath))
                // Return filename without file path. 
                return filepath.Substring(position + 1);
            else
                return String.Empty;
        }
    }

    #endregion "--Private--"

    #region "--Public--"

    public string AdminGetAllTheDatabasesTable(string requestParam)
    {
        string RetVal = string.Empty;
        string DefaultCategory = string.Empty;
        string UseNId = string.Empty;

        try
        {
            UseNId = requestParam;
            DefaultCategory = Global.GetSelectedCategoryName(Global.GetDefaultDbNId()); ;
            RetVal = this.GetAllDbConnectionsWithPublishDetails(DefaultCategory, UseNId);

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
        return RetVal;
    }

    public string AdminGetAllTheDatabaseList()
    {
        string RetVal = string.Empty;
        string DBFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNodeList XmlNodes;
        int i = 0;

        try
        {
            DBFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DBFile);

            XmlNodes = XmlDoc.GetElementsByTagName("db");

            for (i = 0; i < XmlNodes.Count; i++)
            {
                if ((XmlNodes[i].Attributes["sdmxdb"].Value == "false") && (XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Id] != null))
                {
                    RetVal = RetVal + XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Name].Value + Constants.Delimiters.ValuesDelimiter + XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Id].Value + Constants.Delimiters.Comma;
                }
            }
            if (RetVal.Length > 0)
            {
                RetVal = RetVal.Remove(RetVal.Length - 1);
            }

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string AdminSDMXArtefactsGeneration(string requestParam)
    {
        string RetVal = string.Empty;
        int DbNId = -1;
        string UserNId = string.Empty;
        string[] Params;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DbNId = int.Parse(Params[0]);
            UserNId = Params[1];
            //UserNId = Session[Constants.SessionNames.LoggedAdminUserNId].ToString();
            RetVal = GenerateSDMXArtefacts(DbNId, UserNId);
        }
        catch (Exception ex)
        {
            RetVal = ex.Message;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string AdminRegisterDatabaseForUploadedDSD(string requestParam)
    {
        string RetVal = string.Empty;
        string AssosciatedDb = string.Empty;
        string[] Params;
        string DBConnectionsFile = string.Empty;
        XmlDocument UploadedDSDXml = new XmlDocument();
        XmlDocument XmlDoc;
        XmlNode xmlNode;
        string UploadedDSDFileWPath = string.Empty;
        string UploadedDSDFolderPath = Server.MapPath("../../stock/data");
        string UploadedDSDName = string.Empty;
        string UserNId = string.Empty;
        bool DefaultDB = false;

        XmlElement NewNode;
        int NewId = 0;

        XmlNodeList ObjXmlNodeList;
        XmlNodeList CategoryNodeList;
        int CategoryId = 0;
        string CategoryName = string.Empty;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);

            UploadedDSDFileWPath = Params[0];
            AssosciatedDb = Params[1];
            UserNId = Params[2];

            if (Params.Length > 3)
            {
                if (!bool.TryParse(Params[3], out DefaultDB))
                {
                    DefaultDB = false;
                }
            }

            UploadedDSDXml.Load(UploadedDSDFileWPath);
            SDMXApi_2_0.Message.StructureType UploadedDSDStructure = new SDMXApi_2_0.Message.StructureType();
            UploadedDSDStructure = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromXmlDocument(typeof(SDMXApi_2_0.Message.StructureType), UploadedDSDXml);
            if ((UploadedDSDStructure.CodeLists.Count > 0) && (UploadedDSDStructure.KeyFamilies.Count > 0))
            {
                //&& (UploadedDSDStructure.Concepts.Concept.Count > 0)
                //------------------------ Updating db.xml by creating new entry for Uploaded DSD--------------------------------------
                DBConnectionsFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
                XmlDoc = new XmlDocument();
                XmlDoc.Load(DBConnectionsFile);

                CategoryNodeList = XmlDoc.GetElementsByTagName(Constants.XmlFile.Db.Tags.Category);
                CategoryName = CategoryNodeList[0].Attributes[Constants.XmlFile.Db.Tags.CategoryAttributes.Name].Value;
                xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "[@" + Constants.XmlFile.Db.Tags.CategoryAttributes.Name + "='" + CategoryName + "']");

                // Get old higher id                   
                ObjXmlNodeList = XmlDoc.SelectNodes("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "/" + "child::node()");
                foreach (XmlNode data in ObjXmlNodeList)
                {
                    CategoryId = int.Parse(data.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Id].Value);
                    if (NewId < CategoryId)
                    {
                        NewId = CategoryId;
                    }
                }

                // Increase 1 for new id
                NewId++;
                if (UploadedDSDStructure.KeyFamilies[0].Name.Count > 0)
                {
                    UploadedDSDName = GetLangSpecificValueFor_Version_2_0(UploadedDSDStructure.KeyFamilies[0].Name, "en");
                    if (string.IsNullOrEmpty(UploadedDSDName))
                    {
                        UploadedDSDName = "DSD";
                    }
                }
                else
                {
                    UploadedDSDName = "DSD";
                }

                //Create new element node and set its attributes
                NewNode = XmlDoc.CreateElement(Constants.XmlFile.Db.Tags.Database);
                NewNode.SetAttribute(Constants.XmlFile.Db.Tags.DatabaseAttributes.Id, NewId.ToString());
                NewNode.SetAttribute(Constants.XmlFile.Db.Tags.DatabaseAttributes.Name, UploadedDSDName);
                NewNode.SetAttribute(Constants.XmlFile.Db.Tags.DatabaseAttributes.SDMXDb, "true");
                NewNode.SetAttribute(Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDId, UploadedDSDStructure.KeyFamilies[0].id);
                NewNode.SetAttribute(Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDAgencyId, UploadedDSDStructure.KeyFamilies[0].agencyID);
                NewNode.SetAttribute(Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDVersion, UploadedDSDStructure.KeyFamilies[0].version);
                NewNode.SetAttribute(Constants.XmlFile.Db.Tags.DatabaseAttributes.AssosciatedDb, AssosciatedDb);
                NewNode.SetAttribute(Constants.XmlFile.Db.Tags.DatabaseAttributes.Count, "");
                NewNode.SetAttribute(Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultLanguage, "en");
                NewNode.SetAttribute(Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultArea, "");
                NewNode.SetAttribute(Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultAreaJSON, "");
                NewNode.SetAttribute(Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultAreaCount, "");
                NewNode.SetAttribute(Constants.XmlFile.Db.Tags.DatabaseAttributes.DatabaseConnection, "");
                NewNode.SetAttribute(Constants.XmlFile.Db.Tags.DatabaseAttributes.AvailableLanguage, "en");
                NewNode.SetAttribute(Constants.XmlFile.Db.Tags.DatabaseAttributes.LastModified, string.Format("{0:yyyy-MM-dd}", DateTime.Today.Date));
                NewNode.SetAttribute(Constants.XmlFile.Db.Tags.DatabaseAttributes.DescriptionEnglish, UploadedDSDName);
                NewNode.SetAttribute(Constants.XmlFile.Db.Tags.DatabaseAttributes.IsSDMXHeaderCreated, "true");
                xmlNode.AppendChild(NewNode);


                //Now the defDSD attribute of Category will  consume default dsd id.
                if (DefaultDB == true)
                {
                    if (XmlDoc.GetElementsByTagName(Constants.XmlFile.Db.Tags.Root)[0].Attributes[Constants.XmlFile.Db.Tags.RootAttributes.DefaultDSD] == null)
                    {
                        XmlElement root = XmlDoc.DocumentElement;

                        // Add a new attribute.
                        root.SetAttribute(Constants.XmlFile.Db.Tags.RootAttributes.DefaultDSD, NewId.ToString());

                    }
                    else
                    {
                        XmlDoc.GetElementsByTagName(Constants.XmlFile.Db.Tags.Root)[0].Attributes[Constants.XmlFile.Db.Tags.RootAttributes.DefaultDSD].Value = NewId.ToString();
                    }
                }
                //else
                //{
                //    if (XmlDoc.GetElementsByTagName(Constants.XmlFile.Db.Tags.Root)[0].Attributes[Constants.XmlFile.Db.Tags.RootAttributes.DefaultDSD] != null)
                //    {
                //        XmlDoc.GetElementsByTagName(Constants.XmlFile.Db.Tags.Root)[0].Attributes[Constants.XmlFile.Db.Tags.RootAttributes.DefaultDSD].Value = "";


                //    }

                //}


                File.SetAttributes(DBConnectionsFile, FileAttributes.Normal);
                XmlDoc.Save(DBConnectionsFile);

                //-----------------------------------------------------------------------------------------------------------------

                //----------------------Creation of folder structure for uploaded DSD as per the New Id alloted to it in db.xml----
                UploadedDSDFolderPath = UploadedDSDFolderPath + "/" + NewId.ToString();

                RetVal = CreateFolderStructureForUploadedDSD(UploadedDSDFolderPath).ToString();

                //----------------------Creation of Complete File for uploaded DSD as per the New Id alloted to it in db.xml---------
                RetVal = CreateCompleteFileForUploadedDSD(UploadedDSDStructure, UploadedDSDFolderPath + "/" + "sdmx", NewId, UserNId).ToString();
                //----------------------Creation of Header File for uploaded DSD as per the New Id alloted to it in db.xml---------
                RetVal = CreateHeaderFileForUploadedDSD(UploadedDSDStructure, UploadedDSDFolderPath + "/" + "sdmx", NewId, UserNId).ToString();

                //----------------------Creation of Codelists for uploaded DSD as per the New Id alloted to it in db.xml---------
                RetVal = CreateCodelistsForUploadedDSD(UploadedDSDStructure, UploadedDSDFolderPath + "/" + "sdmx" + "/" + "Codelists", NewId).ToString();

                //----------------------Creation of a Category Scheme for uploaded DSD as per the New Id alloted to it in db.xml---------
                RetVal = CreateCategorySchemeForUploadedDSD(UploadedDSDStructure, UploadedDSDFolderPath + "/" + "sdmx", NewId, Convert.ToInt32(AssosciatedDb), UserNId).ToString();

                //----------------------Creation of a Report for uploaded DSD as per the New Id alloted to it in db.xml---------
                RetVal = CreateReportForUploadedDSD(UploadedDSDStructure, UploadedDSDFolderPath + "/" + "sdmx", NewId, UserNId).ToString();

                //----------------------Creation of Other Artefacts for uploaded DSD as per the New Id alloted to it in db.xml-----------
                RetVal = CreateArtefactsForUploadedDSD(UploadedDSDStructure, UploadedDSDFolderPath + "/" + "sdmx", NewId, UserNId).ToString();
                Session["hdbnid"] = Convert.ToString(NewId);
            }
            else
            {
                RetVal = "DSD does not contains codelists or concepts of key family listing.";
            }
            if (RetVal == "true")
            {
                File.Delete(UploadedDSDFileWPath);
            }
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("Uploading DSD from Admin");
            //Global.WriteErrorsInLog(ex.StackTrace);
            //Global.WriteErrorsInLog(ex.Message);
            RetVal = ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {

        }

        return RetVal;
    }

    public string AdminUpdateDatabaseForUploadedDSD(string requestParam)
    {
        string RetVal = string.Empty;
        string[] Params;
        string DBConnectionsFile = string.Empty;
        XmlDocument UploadedDSDXml = new XmlDocument();
        XmlDocument XmlDoc;
        XmlNode xmlNode;
        string UploadedDSDFileWPath = string.Empty;
        string DbNId = string.Empty;
        string AssosciatedDb = string.Empty;
        string UploadedDSDFolderPath = Server.MapPath("../../stock/data");
        string UploadedDSDName = string.Empty;
        string UserNId = string.Empty;
        bool DefaultDB = false;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);

            UploadedDSDFileWPath = Params[0];
            DbNId = Params[1];
            AssosciatedDb = Params[2];
            UserNId = Params[3];

            if (Params.Length > 4)
            {
                if (!bool.TryParse(Params[4], out DefaultDB))
                {
                    DefaultDB = false;
                }
            }


            // && (UploadedDSDStructure.Concepts.Concept.Count > 0) 
            //------------------------ Updating db.xml by creating new entry for Uploaded DSD--------------------------------------
            DBConnectionsFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DBConnectionsFile);

            xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "/" + Constants.XmlFile.Db.Tags.Database + "[@" + Constants.XmlFile.Db.Tags.DatabaseAttributes.Id + "=" + DbNId + "]");

            //xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Name].Value = UploadedDSDName;
            //xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDId].Value = UploadedDSDStructure.KeyFamilies[0].id;
            //xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDAgencyId].Value = UploadedDSDStructure.KeyFamilies[0].agencyID;
            //xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDVersion].Value = UploadedDSDStructure.KeyFamilies[0].version;
            //xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.AssosciatedDb].Value = AssosciatedDb;
            //xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.LastModified].Value = string.Format("{0:yyyy-MM-dd}", DateTime.Today.Date);
            //xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DescriptionEnglish].Value = UploadedDSDName;

            if (DefaultDB == true)
            {
                // XmlDoc.GetElementsByTagName(Constants.XmlFile.Db.Tags.Root)[0].Attributes[Constants.XmlFile.Db.Tags.RootAttributes.Default].Value = DbNId.ToString();
                if (XmlDoc.GetElementsByTagName(Constants.XmlFile.Db.Tags.Root)[0].Attributes[Constants.XmlFile.Db.Tags.RootAttributes.DefaultDSD] == null)
                {
                    XmlElement root = XmlDoc.DocumentElement;

                    // Add a new attribute.
                    root.SetAttribute(Constants.XmlFile.Db.Tags.RootAttributes.DefaultDSD, DbNId.ToString());

                }
                else
                {
                    XmlDoc.GetElementsByTagName(Constants.XmlFile.Db.Tags.Root)[0].Attributes[Constants.XmlFile.Db.Tags.RootAttributes.DefaultDSD].Value = DbNId.ToString();
                    Session["hdbnid"] = Convert.ToString(DbNId);
                }
            }
            else
            {
                if (XmlDoc.GetElementsByTagName(Constants.XmlFile.Db.Tags.Root)[0].Attributes[Constants.XmlFile.Db.Tags.RootAttributes.DefaultDSD] != null)
                {
                    XmlDoc.GetElementsByTagName(Constants.XmlFile.Db.Tags.Root)[0].Attributes[Constants.XmlFile.Db.Tags.RootAttributes.DefaultDSD].Value = "";
                }

            }

            File.SetAttributes(DBConnectionsFile, FileAttributes.Normal);
            XmlDoc.Save(DBConnectionsFile);
            RetVal = "True";

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("Uploading DSD from Admin");
            //Global.WriteErrorsInLog(ex.StackTrace);
            //Global.WriteErrorsInLog(ex.Message);
            RetVal = ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {

        }

        return RetVal;
    }



    //public string AdminUpdateDatabaseForUploadedDSD(string requestParam)
    //{
    //    string RetVal = string.Empty;
    //    string[] Params;
    //    string DBConnectionsFile = string.Empty;
    //    XmlDocument UploadedDSDXml = new XmlDocument();
    //    XmlDocument XmlDoc;
    //    XmlNode xmlNode;
    //    string UploadedDSDFileWPath = string.Empty;
    //    string DbNId = string.Empty;
    //    string AssosciatedDb = string.Empty;
    //    string UploadedDSDFolderPath = Server.MapPath("../../stock/data");
    //    string UploadedDSDName = string.Empty;
    //    string UserNId = string.Empty;
    //    bool DefaultDB = false;

    //    try
    //    {
    //        Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);

    //        UploadedDSDFileWPath = Params[0];
    //        DbNId = Params[1];
    //        AssosciatedDb = Params[2];
    //        UserNId = Params[3];

    //        if (Params.Length > 4)
    //        {
    //            if (!bool.TryParse(Params[4], out DefaultDB))
    //            {
    //                DefaultDB = false;
    //            }
    //        }

    //        UploadedDSDXml.Load(UploadedDSDFileWPath);
    //        SDMXApi_2_0.Message.StructureType UploadedDSDStructure = new SDMXApi_2_0.Message.StructureType();
    //        UploadedDSDStructure = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromXmlDocument(typeof(SDMXApi_2_0.Message.StructureType), UploadedDSDXml);
    //        if ((UploadedDSDStructure.CodeLists.Count > 0) && (UploadedDSDStructure.KeyFamilies.Count > 0))
    //        {
    //            // && (UploadedDSDStructure.Concepts.Concept.Count > 0) 
    //            //------------------------ Updating db.xml by creating new entry for Uploaded DSD--------------------------------------
    //            DBConnectionsFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
    //            XmlDoc = new XmlDocument();
    //            XmlDoc.Load(DBConnectionsFile);

    //            xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "/" + Constants.XmlFile.Db.Tags.Database + "[@" + Constants.XmlFile.Db.Tags.DatabaseAttributes.Id + "=" + DbNId + "]");
    //            if (UploadedDSDStructure.KeyFamilies[0].Name.Count > 0)
    //            {
    //                UploadedDSDName = GetLangSpecificValueFor_Version_2_0(UploadedDSDStructure.KeyFamilies[0].Name, "en");
    //                if (string.IsNullOrEmpty(UploadedDSDName))
    //                {
    //                    UploadedDSDName = "DSD";
    //                }
    //            }
    //            else
    //            {
    //                UploadedDSDName = "DSD";
    //            }
    //            xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Name].Value = UploadedDSDName;
    //            xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDId].Value = UploadedDSDStructure.KeyFamilies[0].id;
    //            xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDAgencyId].Value = UploadedDSDStructure.KeyFamilies[0].agencyID;
    //            xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDVersion].Value = UploadedDSDStructure.KeyFamilies[0].version;
    //            xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.AssosciatedDb].Value = AssosciatedDb;
    //            xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.LastModified].Value = string.Format("{0:yyyy-MM-dd}", DateTime.Today.Date);
    //            xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DescriptionEnglish].Value = UploadedDSDName;

    //            if (DefaultDB == true)
    //            {
    //                // XmlDoc.GetElementsByTagName(Constants.XmlFile.Db.Tags.Root)[0].Attributes[Constants.XmlFile.Db.Tags.RootAttributes.Default].Value = DbNId.ToString();
    //                if (XmlDoc.GetElementsByTagName(Constants.XmlFile.Db.Tags.Root)[0].Attributes[Constants.XmlFile.Db.Tags.RootAttributes.DefaultDSD] == null)
    //                {
    //                    XmlElement root = XmlDoc.DocumentElement;

    //                    // Add a new attribute.
    //                    root.SetAttribute(Constants.XmlFile.Db.Tags.RootAttributes.DefaultDSD, DbNId.ToString());

    //                }
    //                else
    //                {
    //                    XmlDoc.GetElementsByTagName(Constants.XmlFile.Db.Tags.Root)[0].Attributes[Constants.XmlFile.Db.Tags.RootAttributes.DefaultDSD].Value = DbNId.ToString();
    //                    Session["hdbnid"] = Convert.ToString(DbNId);
    //                }
    //            }
    //            else
    //            {
    //                if (XmlDoc.GetElementsByTagName(Constants.XmlFile.Db.Tags.Root)[0].Attributes[Constants.XmlFile.Db.Tags.RootAttributes.DefaultDSD] != null)
    //                {
    //                    XmlDoc.GetElementsByTagName(Constants.XmlFile.Db.Tags.Root)[0].Attributes[Constants.XmlFile.Db.Tags.RootAttributes.DefaultDSD].Value = "";
    //                }

    //            }
    //            File.SetAttributes(DBConnectionsFile, FileAttributes.Normal);
    //            XmlDoc.Save(DBConnectionsFile);

    //            this.Clean_SDMX_Folder_Structure(Convert.ToInt32(DbNId));
    //            //-----------------------------------------------------------------------------------------------------------------

    //            //----------------------Updation of folder structure for uploaded DSD as per the DbNId alloted to Uploaded DSD in db.xml----
    //            UploadedDSDFolderPath = UploadedDSDFolderPath + "/" + DbNId;

    //            RetVal = CreateFolderStructureForUploadedDSD(UploadedDSDFolderPath).ToString();

    //            this.Delete_SDMXArtefacts_Details_In_Database(DbNId, UserNId);//commented

    //            //----------------------Updation of Complete File for uploaded DSD as per the DbNId alloted to Uploaded DSD alloted to it in db.xml---------
    //            RetVal = CreateCompleteFileForUploadedDSD(UploadedDSDStructure, UploadedDSDFolderPath + "/" + "sdmx", Convert.ToInt32(DbNId), UserNId).ToString();

    //            //----------------------Updation of Complete File for uploaded DSD as per the DbNId alloted to Uploaded DSD alloted to it in db.xml---------
    //            RetVal = CreateHeaderFileForUploadedDSD(UploadedDSDStructure, UploadedDSDFolderPath + "/" + "sdmx", Convert.ToInt32(DbNId), UserNId).ToString();

    //            //----------------------Updation of Codelists for uploaded DSD as per the DbNId alloted to Uploaded DSD alloted to it in db.xml---------
    //            RetVal = CreateCodelistsForUploadedDSD(UploadedDSDStructure, UploadedDSDFolderPath + "/" + "sdmx" + "/" + "Codelists", Convert.ToInt32(DbNId)).ToString();

    //            //----------------------Updation of the Category Scheme for uploaded DSD as per the DbNId alloted to Uploaded DSD alloted to it in db.xml---------
    //            RetVal = CreateCategorySchemeForUploadedDSD(UploadedDSDStructure, UploadedDSDFolderPath + "/" + "sdmx", Convert.ToInt32(DbNId), Convert.ToInt32(AssosciatedDb), UserNId).ToString();

    //            //----------------------Updation of the Report for uploaded DSD as per the DbNId alloted to Uploaded DSD alloted to it in db.xml---------
    //            RetVal = CreateReportForUploadedDSD(UploadedDSDStructure, UploadedDSDFolderPath + "/" + "sdmx", Convert.ToInt32(DbNId), UserNId).ToString();

    //            //----------------------Updation of Other Artefacts for uploaded DSD as per the DbNId alloted to Uploaded DSD alloted to it in db.xml-----------
    //            RetVal = CreateArtefactsForUploadedDSD(UploadedDSDStructure, UploadedDSDFolderPath + "/" + "sdmx", Convert.ToInt32(DbNId), UserNId).ToString();

    //            this.Send_Notifications_For_Subscriptions_For_Structural_Metadata_Changes(DbNId);
    //            //AdminUpdateArtifactsWithHeaderForUploadedDSD(requestParam);
    //        }
    //        else
    //        {
    //            RetVal = "DSD does not contains codelists or concepts of key family listing.";
    //        }
    //        if (RetVal == "true")
    //        {
    //            File.Delete(UploadedDSDFileWPath);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        //Global.WriteErrorsInLog("Uploading DSD from Admin");
    //        //Global.WriteErrorsInLog(ex.StackTrace);
    //        //Global.WriteErrorsInLog(ex.Message);
    //        RetVal = ex.Message;
    //        Global.CreateExceptionString(ex, null);
    //    }
    //    finally
    //    {

    //    }

    //    return RetVal;
    //}

    public string AdminGetAllTheUploadedDSDsTable(string requestParam)
    {
        string RetVal = string.Empty;
        string DBFile = string.Empty;
        string DSDPath = string.Empty;
        string DSDViewPath = string.Empty;
        string AssociatedDb = string.Empty;
        string Publisher = string.Empty;
        string UserNId = string.Empty;

        XmlDocument XmlDoc;
        XmlNodeList XmlNodes;
        DIConnection DIConnection = null;
        string Query = string.Empty;
        DataTable DtDFD;
        int i = 0;
        int DSDCount = 0;
        string HeaderFilePath = string.Empty;
        string DbNid = string.Empty;
        try
        {
            UserNId = requestParam;
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                  string.Empty, string.Empty);
            DBFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DBFile);

            XmlNodes = XmlDoc.GetElementsByTagName("db");
            if (XmlNodes.Count > 0)
            {
                RetVal = "<table style=\"width:100%; \" border=\"0\" cellSpacing=\"0\" cellSpacing=\"0\">";
                RetVal += "<tr class=\"HeaderRowStyle \">";
                RetVal += "<td class=\"HeaderColumnStyle \"><span  id=\"lang_SNo\"></span></td>";
                RetVal += "<td class=\"HeaderColumnStyle \"><span  id=\"lang_Id\"></span></td>";
                RetVal += "<td class=\"HeaderColumnStyle \"><span  id=\"lang_AgencyId\"></span></td>";
                RetVal += "<td class=\"HeaderColumnStyle \"><span  id=\"lang_Version\"></span></td>";
                RetVal += "<td class=\"HeaderColumnStyle \"><span  id=\"lang_Assosciated_Database\"></span></td>";
                RetVal += "<td class=\"HeaderColumnStyle \"><span  id=\"lang_Publisher_DSD\"></span></td>";
                //   RetVal += "<td class=\"HeaderColumnStyle \"><span  id=\"lang_Details\"></span></td>";
                RetVal += "<td class=\"HeaderColumnStyle \"><span  id=\"lang_Action\"></span></td>";
                RetVal += "<td class=\"HeaderColumnStyle \"><span  id=\"lang_Header\"></span></td>";
                RetVal += "</tr>";
                for (i = 0; i < XmlNodes.Count; i++)
                {
                    if ((XmlNodes[i].Attributes["sdmxdb"].Value == "true") && (XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDId] != null) && (XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDVersion] != null))
                    {
                        DSDCount = DSDCount + 1;
                        RetVal = RetVal + "<tr class=\"DataRowStyle \">";
                        RetVal += "<td class=\"DataColumnStyle \">" + DSDCount + "</td>";
                        RetVal += "<td class=\"DataColumnStyle \">" + XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDId].Value + "</td>";
                        if (XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDAgencyId] != null)
                        {
                            RetVal += "<td class=\"DataColumnStyle \">" + XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDAgencyId].Value + "</td>";
                        }
                        else
                        {
                            RetVal += "<td class=\"DataColumnStyle \"></td>";
                        }
                        RetVal += "<td class=\"DataColumnStyle \">" + XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDVersion].Value + "</td>";
                        if (XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.AssosciatedDb] != null)
                        {
                            AssociatedDb = XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.AssosciatedDb].Value;
                            RetVal += "<td class=\"DataColumnStyle \">" + Global.GetDbConnectionDetails(AssociatedDb)[0] + "</td>";
                        }
                        else
                        {
                            RetVal += "<td class=\"DataColumnStyle \"></td>";
                        }
                        DSDPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Id].Value + "\\sdmx\\DSD" + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension);
                        DSDViewPath = "../../" + Constants.FolderName.Data + XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Id].Value + "/sdmx/DSD" + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension;
                        Query = "SELECT * FROM Artefacts WHERE Type=" + Convert.ToInt32(ArtefactTypes.DFD).ToString() + " And DbNId = " + XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Id].Value + ";";

                        DtDFD = DIConnection.ExecuteDataTable(Query);
                        if (DtDFD.Rows.Count > 0)
                        {
                            Publisher = DtDFD.Rows[0]["AgencyId"].ToString();
                            RetVal += "<td class=\"DataColumnStyle \">" + Publisher + "</td>";


                        }
                        else
                        {
                            RetVal += "<td class=\"DataColumnStyle \"></td>";
                        }

                        // RetVal += "<td class=\"DataColumnStyle \">" + "<a style=\"cursor:pointer;\" href=\"" + DSDViewPath.Replace("\\", "/") + "\"  target=\"_blank\" name=\"lang_View\"></a> | <a style=\"cursor:pointer;\" href='Download.aspx?fileId=" + DSDPath + "'  name=\"lang_Download\"></a>" + "</td>";
                        if (string.IsNullOrEmpty(Publisher))
                        {
                            RetVal += "<td class=\"DataColumnStyle \"></td>";
                        }
                        else
                        {
                            if (this.isUserAdmin(UserNId) == true)//Global.SplitString(Publisher, "_")[1] == UserNId
                            {

                                RetVal += "<td class=\"DataColumnStyle \">" + "<a style=\"cursor:pointer;\" href=\"javascript:void(0);\" onclick=\"OpenUploadDSDPopup('U','" + XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Id].Value + "','" + AssociatedDb + "');\"  name=\"lang_Edit\"></a> | <a style=\"cursor:pointer;\" href=\"javascript:void(0);\" onclick=\"OpenUploadDSDPopup('D','" + XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Id].Value + "','" + AssociatedDb + "');\" name=\"lang_Delete\"></a>" + "</td>";
                            }
                            else
                            {
                                RetVal += "<td class=\"DataColumnStyle \"></td>";
                            }
                        }
                        DbNid = XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Id].Value;
                        HeaderFilePath = "../../" + Constants.FolderName.Data + DbNid + "\\" + Constants.FolderName.SDMX.sdmx + Constants.FileName.HeaderFileName;
                        RetVal += "<td class=\"DataColumnStyle \">" + "<a style=\"cursor:pointer;\" href=\"" + HeaderFilePath.Replace("\\", "/") + "\"  target=\"_blank\" name=\"lang_View\"></a> | <a style=\"cursor:pointer;\" href=\"javascript:void(0);\" onclick=\"OpenHeaderUpdatePopup(false,'" + DbNid + "');\"  name=\"lang_Edit\"></a>" + "</td>";

                        RetVal = RetVal + "</tr>";
                    }
                }
                RetVal = RetVal + "</table>";
            }

            if (RetVal.Length > 0)
            {
                RetVal = RetVal.Remove(RetVal.Length - 1);
            }

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string AdminGetAllTheUploadedDSDsList()
    {
        string RetVal = string.Empty;
        string DBFile = string.Empty;
        string AgencyId = string.Empty;
        XmlDocument XmlDoc;
        XmlNodeList XmlNodes;
        int i = 0;

        try
        {
            DBFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DBFile);

            XmlNodes = XmlDoc.GetElementsByTagName("db");

            for (i = 0; i < XmlNodes.Count; i++)
            {
                if ((XmlNodes[i].Attributes["sdmxdb"].Value == "true") && (XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDId] != null) && (XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDVersion] != null))
                {
                    if (XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDAgencyId] != null)
                    {
                        AgencyId = XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDAgencyId].Value;
                    }

                    RetVal = RetVal + XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDId].Value + "-" + AgencyId + "-" + XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDVersion].Value + Constants.Delimiters.ValuesDelimiter + XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Id].Value + Constants.Delimiters.Comma;
                }
            }
            if (RetVal.Length > 0)
            {
                RetVal = RetVal.Remove(RetVal.Length - 1);
            }

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string AdminGetAllTheUploadedMSDsTable()
    {
        string RetVal = string.Empty;
        string DBFile = string.Empty;
        string CompleteXmlPath = string.Empty;
        string MSDPath = string.Empty;
        string MSDViewPath = string.Empty;
        string AgencyId = string.Empty;

        XmlDocument XmlDoc;
        XmlNodeList XmlNodes;
        int i = 0;
        int MSDCount = 0;
        int j = 0;
        SDMXApi_2_0.Message.StructureType CompleteStructure = new SDMXApi_2_0.Message.StructureType();


        try
        {
            DBFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DBFile);

            XmlNodes = XmlDoc.GetElementsByTagName("db");
            if (XmlNodes.Count > 0)
            {
                RetVal = "<table style=\"width:100%; \" border=\"0\" cellSpacing=\"0\" cellSpacing=\"0\">";
                RetVal += "<tr class=\"HeaderRowStyle \">";
                RetVal += "<td class=\"HeaderColumnStyle \"><span  id=\"lang_SNo_MSD\"></span></td>";
                RetVal += "<td class=\"HeaderColumnStyle \"><span  id=\"lang_Id_MSD\"></span></td>";
                RetVal += "<td class=\"HeaderColumnStyle  \"><span  id=\"lang_Agency_Id_MSD\"></span></td>";
                RetVal += "<td class=\"HeaderColumnStyle  \"><span  id=\"lang_Version_MSD\"></span></td>";
                RetVal += "<td class=\"HeaderColumnStyle  \"><span  id=\"lang_Assosciated_DSD_MSD\"></span></td>";
                //  RetVal += "<td class=\"HeaderColumnStyle  \"><span  id=\"lang_Details_MSD\"></span></td>";
                RetVal += "</tr>";
                for (i = 0; i < XmlNodes.Count; i++)
                {
                    if ((XmlNodes[i].Attributes["sdmxdb"].Value == "true") && (XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDId] != null) && (XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDVersion] != null))
                    {
                        CompleteXmlPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Id].Value + "\\sdmx\\Complete" + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension);
                        CompleteStructure = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromFile(typeof(SDMXApi_2_0.Message.StructureType), CompleteXmlPath);

                        for (j = 0; j < CompleteStructure.MetadataStructureDefinitions.Count; j++)
                        {
                            MSDCount = MSDCount + 1;
                            MSDPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Id].Value + "\\sdmx\\MSD\\" + CompleteStructure.MetadataStructureDefinitions[j].id + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension);
                            MSDViewPath = "../../" + Constants.FolderName.Data + XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Id].Value + "/sdmx/MSD/" + CompleteStructure.MetadataStructureDefinitions[j].id + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension;
                            RetVal = RetVal + "<tr class=\"DataRowStyle \">";
                            RetVal += "<td class=\"DataRowStyle \">" + MSDCount + "</td>";
                            RetVal += "<td class=\"DataRowStyle \">" + CompleteStructure.MetadataStructureDefinitions[j].id + "</td>";
                            RetVal += "<td class=\"DataRowStyle \">" + CompleteStructure.MetadataStructureDefinitions[j].agencyID + "</td>";
                            RetVal += "<td class=\"DataRowStyle \">" + CompleteStructure.MetadataStructureDefinitions[j].version + "</td>";
                            RetVal += "<td class=\"DataRowStyle \">" + XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDId].Value + "-" + XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDAgencyId].Value + "-" + XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDVersion].Value + "</td>";
                            ///RetVal += "<td class=\"DataColumnStyle \">" + "<a style=\"cursor:pointer;\" href=\"" + MSDViewPath.Replace("\\", "/") + "\"  target=\"_blank\" name=\"lang_View\"></a> | <a style=\"cursor:pointer;\" href='Download.aspx?fileId=" + MSDPath + "' name=\"lang_Download\"></a>" + "</td>";

                            RetVal = RetVal + "</tr>";
                        }

                    }
                }
                RetVal = RetVal + "</table>";
            }

            if (RetVal.Length > 0)
            {
                RetVal = RetVal.Remove(RetVal.Length - 1);
            }

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string AdminCheckIfDSDAlreadyUploaded(string requestParam)
    {
        string RetVal = string.Empty;
        string DBFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNodeList XmlNodes;
        int i = 0;
        bool ValidDSD = true;
        XmlDocument UploadedDSDXml = new XmlDocument();
        string UploadedDSDFileWPath = string.Empty;
        string UploadedDSDId = string.Empty;
        string UploadedDSDAgencyId = string.Empty;
        string UploadedDSDVersion = string.Empty;
        string DbNId = string.Empty;
        string[] Params;
        string ErrorMessage = string.Empty;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            UploadedDSDFileWPath = Params[0];
            DbNId = Params[1];

            UploadedDSDXml.Load(UploadedDSDFileWPath);
            SDMXApi_2_0.Message.StructureType UploadedDSDStructure = new SDMXApi_2_0.Message.StructureType();
            UploadedDSDStructure = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromXmlDocument(typeof(SDMXApi_2_0.Message.StructureType), UploadedDSDXml);

            if (UploadedDSDStructure.KeyFamilies.Count > 0)
            {
                UploadedDSDId = UploadedDSDStructure.KeyFamilies[0].id;
                UploadedDSDAgencyId = UploadedDSDStructure.KeyFamilies[0].agencyID;
                UploadedDSDVersion = UploadedDSDStructure.KeyFamilies[0].version;

                DBFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
                XmlDoc = new XmlDocument();
                XmlDoc.Load(DBFile);

                XmlNodes = XmlDoc.GetElementsByTagName("db");

                for (i = 0; i < XmlNodes.Count; i++)
                {
                    if (XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Id].Value != DbNId)
                    {
                        if ((XmlNodes[i].Attributes["sdmxdb"].Value == "true") && (XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDId] != null) && (XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDAgencyId] != null) && (XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDVersion] != null))
                        {
                            if ((UploadedDSDId == XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDId].Value) && (UploadedDSDAgencyId == XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDAgencyId].Value) && (UploadedDSDVersion == XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDVersion].Value))
                            {
                                ValidDSD = false;
                                ErrorMessage = "A DSD with same Id, Agency Id, Version has been uploaded.Please upload different DSD.";
                            }
                        }
                    }

                }

            }
            else
            {
                ValidDSD = false;
                ErrorMessage = "Invalid SDMX Artifact";
            }
            RetVal = ValidDSD.ToString()+Constants.Delimiters.ParamDelimiter+ErrorMessage;

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            RetVal = "false";
        }

        return RetVal;
    }

    public string GetDefaultDb()
    {
        string RetVal;

        RetVal = string.Empty;

        try
        {
            // RetVal = Global.GetDefaultDbNId();
            RetVal = Global.GetDefaultDSDNId();
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
        }

        return RetVal;
    }

    public bool AdminUpdateArtifactsWithHeaderForUploadedDSD(string requestParam)
    {
        bool RetVal = false;
        string DbNId = string.Empty;
        string[] Params;
        string DBConnectionsFile = string.Empty;
        string DBFile = string.Empty;
        XmlDocument UploadedHeaderXml = new XmlDocument();
        XmlDocument XmlDoc;
        XmlNodeList XmlNodes;
        string UploadedHeaderFileWPath = string.Empty;
        string UploadedHeaderFolderPath = Server.MapPath("../../stock/data");
        string UploadedHeaderName = string.Empty;
        string UserNId = string.Empty;
        string DatabaseNIdfor2_1 = string.Empty;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DbNId = Params[0];
            UserNId = Params[1];
            UploadedHeaderFileWPath = UploadedHeaderFolderPath + "/" + DbNId + "/" + "sdmx" + "/" + DevInfo.Lib.DI_LibSDMX.Constants.Header.FileName;


            DBFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DBFile);
            XmlNodes = XmlDoc.GetElementsByTagName("db");
            for (int i = 0; i < XmlNodes.Count; i++)
            {
                if ((XmlNodes[i].Attributes["sdmxdb"].Value == "false") && (XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Id] != null))
                {
                    DatabaseNIdfor2_1 = XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Id].Value;
                }
            }


            UploadedHeaderXml.Load(UploadedHeaderFileWPath);
            SDMXApi_2_0.Message.StructureType UploadedDSDStructure = new SDMXApi_2_0.Message.StructureType();
            SDMXApi_2_0.Message.HeaderType Header = new SDMXApi_2_0.Message.HeaderType();
            UploadedDSDStructure = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromXmlDocument(typeof(SDMXApi_2_0.Message.StructureType), UploadedHeaderXml);
            Header = UploadedDSDStructure.Header;
            UploadedHeaderFolderPath = UploadedHeaderFolderPath + "/" + DbNId;
            RetVal = UpdateCompleteWithHeader(Header, DbNId, UserNId, UploadedHeaderFolderPath + "/" + "sdmx");
            RetVal = UpdateCodelistsForUploadedDSDWithHeader(Header, DbNId, UserNId, UploadedHeaderFolderPath + "/" + "sdmx" + "/" + "Codelists");
            RetVal = UpdateCategorySchemeForUploadedDSDWithHeader(DbNId, UserNId, DatabaseNIdfor2_1);
            RetVal = UpdateArtefactsForUploadedDSDWithHeader(Header, DbNId, UserNId, UploadedHeaderFolderPath + "/" + "sdmx", DatabaseNIdfor2_1);
            RetVal = UpdateNonMAForUploadedDSDWithHeader(DbNId, UserNId, DatabaseNIdfor2_1);

        }
        catch (Exception ex)
        {
            RetVal = false;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {

        }


        return RetVal;
    }

    public bool AdminUpdateArtifactsWithHeaderForDatabase(string requestParam)
    {
        bool RetVal = false;
        string DbNId = string.Empty;
        string[] Params;
        string DBConnectionsFile = string.Empty;
        string DBFile = string.Empty;
        XmlDocument UploadedHeaderXml = new XmlDocument();
        XmlDocument XmlDoc;
        XmlNodeList XmlNodes;
        string UploadedHeaderFileWPath = string.Empty;
        string UploadedHeaderFolderPath = Server.MapPath("../../stock/data");
        string UploadedHeaderName = string.Empty;
        string UserNId = string.Empty;

        string DatabaseNIdfor2_1 = string.Empty;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DbNId = Params[0];
            UserNId = Params[1];
            UploadedHeaderFileWPath = UploadedHeaderFolderPath + "/" + DbNId + "/" + "sdmx" + "/" + DevInfo.Lib.DI_LibSDMX.Constants.Header.FileName;


            DBFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DBFile);
            XmlNodes = XmlDoc.GetElementsByTagName("db");
            for (int i = 0; i < XmlNodes.Count; i++)
            {
                if ((XmlNodes[i].Attributes["sdmxdb"].Value == "false") && (XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Id] != null))
                {
                    DatabaseNIdfor2_1 = XmlNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Id].Value;
                }
            }


            UploadedHeaderXml.Load(UploadedHeaderFileWPath);

            SDMXObjectModel.Message.StructureType UploadedDSDStructure = new SDMXObjectModel.Message.StructureType();
            SDMXObjectModel.Message.StructureHeaderType Header = new SDMXObjectModel.Message.StructureHeaderType();
            UploadedDSDStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureType), UploadedHeaderXml);
            Header = UploadedDSDStructure.Header;
            UploadedHeaderFolderPath = UploadedHeaderFolderPath + "/" + DbNId;
            RetVal = UpdateCompleteWithHeaderForDB(Header, DbNId, UserNId, UploadedHeaderFolderPath + "/" + "sdmx");
            RetVal = UpdateCodelistsForDBWithHeader(Header, DbNId, UserNId, UploadedHeaderFolderPath + "/" + "sdmx" + "/" + "Codelists");
            RetVal = UpdateCategorySchemeForDBWithHeader(DbNId, UserNId, DatabaseNIdfor2_1);
            RetVal = UpdateArtefactsForDBWithHeader(Header, DbNId, UserNId, UploadedHeaderFolderPath + "/" + "sdmx", DatabaseNIdfor2_1);
            RetVal = UpdateNonMAForUploadedDBWithHeader(DbNId, UserNId, DatabaseNIdfor2_1);

        }
        catch (Exception ex)
        {
            RetVal = false;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {

        }


        return RetVal;
    }

    #endregion "--Public--"

    #endregion "--Methods--"
}
