using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using SDMXObjectModel.Structure;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using SDMXObjectModel.Common;
using SDMXObjectModel;
using System.Xml;
using System.IO;

namespace DevInfo.Lib.DI_LibSDMX
{
    internal class CategorizationUtility : ArtefactUtility
    {
        #region "--Properties--"

        #region "--Private--"

        private bool _multiLanguageHandlingRequired;

        private DIConnection _diConnection;

        private DIQueries _diQueries;

        #endregion "--Private--"

        #region "--Public--"

        internal bool MultiLanguageHandlingRequired
        {
            get
            {
                return this._multiLanguageHandlingRequired;
            }
            set
            {
                this._multiLanguageHandlingRequired = value;
            }
        }

        internal DIConnection DIConnection
        {
            get
            {
                return this._diConnection;
            }
            set
            {
                this._diConnection = value;
            }
        }

        internal DIQueries DIQueries
        {
            get
            {
                return this._diQueries;
            }
            set
            {
                this._diQueries = value;
            }
        }

        #endregion "--Public--"

        #endregion "--Properties--""

        #region "--Constructors--"

        #region "--Private--"

        #endregion "--Private--"

        #region "--Public--"

        internal CategorizationUtility(string agencyId, string language, Header header, string outputFolder, DIConnection DIConnection, DIQueries DIQueries)
            : base(agencyId, language, header, outputFolder)
        {
            this._diConnection = DIConnection;
            this._diQueries = DIQueries;

            if (string.IsNullOrEmpty(language))
            {
                this.Language = this._diQueries.LanguageCode.Substring(1);
                this._multiLanguageHandlingRequired = true;
            }
            else
            {
                if (this._diConnection.IsValidDILanguage(this._diQueries.DataPrefix, language))
                {
                    this.Language = language;
                    this._multiLanguageHandlingRequired = false;
                }
                else
                {
                    this.Language = this._diQueries.LanguageCode.Substring(1);
                    this._multiLanguageHandlingRequired = false;
                }
            }
        }

        #endregion "--Public--"

        #endregion "--Constructors--""

        #region "--Methods--"

        #region "--Private--"

        private List<ArtefactInfo> Generate_Categorisations()
        {
            List<ArtefactInfo> RetVal;
            ArtefactInfo Artefact;
            CategorisationType Categorisation;
            DataTable DtIC, DtICIUS;
            ICType ICTypeEnum;
            string ICNId, ICGId, IUSGId, ParentCSId, ParentCSVersion;

            RetVal = new List<ArtefactInfo>();
            Artefact = null;
            Categorisation = null;
            ICTypeEnum = ICType.Sector;
            ICNId = string.Empty;
            ICGId = string.Empty;
            IUSGId = string.Empty;
            ParentCSId = string.Empty;
            ParentCSVersion = string.Empty;

            try
            {
                DtIC = this._diConnection.ExecuteDataTable(this._diQueries.IndicatorClassification.GetIC(FilterFieldType.None, string.Empty, FieldSelection.Light));

                foreach (DataRow DrIC in DtIC.Rows)
                {
                    switch (DrIC[IndicatorClassifications.ICType].ToString())
                    {
                        case "SC":
                            ICTypeEnum = ICType.Sector;
                            ParentCSId = Constants.CategoryScheme.Sector.Id;
                            ParentCSVersion = Constants.CategoryScheme.Sector.Version;
                            break;
                        case "GL":
                            ICTypeEnum = ICType.Goal;
                            ParentCSId = Constants.CategoryScheme.Goal.Id;
                            ParentCSVersion = Constants.CategoryScheme.Goal.Version;
                            break;
                        case "CF":
                            ICTypeEnum = ICType.CF;
                            ParentCSId = Constants.CategoryScheme.Framework.Id;
                            ParentCSVersion = Constants.CategoryScheme.Framework.Version;
                            break;
                        case "IT":
                            ICTypeEnum = ICType.Institution;
                            ParentCSId = Constants.CategoryScheme.Institution.Id;
                            ParentCSVersion = Constants.CategoryScheme.Institution.Version;
                            break;
                        case "TH":
                            ICTypeEnum = ICType.Theme;
                            ParentCSId = Constants.CategoryScheme.Theme.Id;
                            ParentCSVersion = Constants.CategoryScheme.Theme.Version;
                            break;
                        case "SR":
                            ICTypeEnum = ICType.Source;
                            ParentCSId = Constants.CategoryScheme.Source.Id;
                            ParentCSVersion = Constants.CategoryScheme.Source.Version;
                            break;
                        case "CV":
                            ICTypeEnum = ICType.Convention;
                            ParentCSId = Constants.CategoryScheme.Convention.Id;
                            ParentCSVersion = Constants.CategoryScheme.Convention.Version;
                            break;
                        default:
                            break;
                    }

                    ICNId = DrIC[IndicatorClassifications.ICNId].ToString();
                    ICGId = DrIC[IndicatorClassifications.ICGId].ToString();

                    DtICIUS = this._diConnection.ExecuteDataTable(this._diQueries.IUS.GetIUSByIC(ICTypeEnum, ICNId, FieldSelection.Light));

                    foreach (DataRow DrICIUS in DtICIUS.Rows)
                    {
                        IUSGId = DrICIUS[Indicator.IndicatorGId].ToString() + Constants.AtTheRate + DrICIUS[Unit.UnitGId].ToString() + Constants.AtTheRate + DrICIUS[SubgroupVals.SubgroupValGId].ToString();

                        Categorisation = new CategorisationType(Constants.Categorization.Prefix + ICGId + Constants.AtTheRate + IUSGId, this.AgencyId, Constants.Categorization.Version, Constants.Categorization.Name, Constants.Categorization.Description, Constants.DefaultLanguage, null);
                        Categorisation.Source = new ObjectReferenceType();
                        Categorisation.Source.Items = new List<object>();
                        Categorisation.Source.Items.Add(new ObjectRefType(IUSGId, this.AgencyId, null, Constants.CodeList.IUS.Id, Constants.CodeList.IUS.Version, ObjectTypeCodelistType.Code, true, PackageTypeCodelistType.codelist, true));

                        Categorisation.Target = new CategoryReferenceType();
                        Categorisation.Target.Items = new List<object>();
                        Categorisation.Target.Items.Add(new CategoryRefType(ICGId, this.AgencyId, ParentCSId, ParentCSVersion));

                        Artefact = this.Prepare_ArtefactInfo_From_Categorisation(Categorisation, Constants.Categorization.Prefix + ICGId + Constants.AtTheRate + IUSGId + Constants.XmlExtension);
                        this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);
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

        private List<ArtefactInfo> Generate_Categorisations(string IcType)
        {
            List<ArtefactInfo> RetVal;
            ArtefactInfo Artefact;
            CategorisationType Categorisation;
            DataTable DtIC, DtICIUS;
            ICType ICTypeEnum;
            ICType ICCategoryScheme;
            string ICNId, ICGId, IUSGId, ParentCSId, ParentCSVersion;

            RetVal = new List<ArtefactInfo>();
            Artefact = null;
            Categorisation = null;
            ICTypeEnum = ICType.Sector;
            ICCategoryScheme = ICType.Sector;
            ICNId = string.Empty;
            ICGId = string.Empty;
            IUSGId = string.Empty;
            ParentCSId = string.Empty;
            ParentCSVersion = string.Empty;

            try
            {
                switch (IcType)
                {
                    case "SC":
                        ICCategoryScheme = ICType.Sector;
                         break;
                    case "GL":
                        ICCategoryScheme = ICType.Goal;
                            break;
                    case "CF":
                        ICCategoryScheme = ICType.CF;
                        break;
                    case "IT":
                        ICCategoryScheme = ICType.Institution;
                        break;
                    case "TH":
                        ICCategoryScheme = ICType.Theme;
                       break;
                    case "SR":
                        ICCategoryScheme = ICType.Source;
                       break;
                    case "CV":
                        ICCategoryScheme = ICType.Convention;
                        break;
                      default:
                        break;
                }
                if (IcType == "ALL")
                {
                    DtIC = this._diConnection.ExecuteDataTable(this._diQueries.IndicatorClassification.GetIC(FilterFieldType.None, string.Empty, FieldSelection.Light));
                }
                else
                {
                    DtIC = this._diConnection.ExecuteDataTable(this._diQueries.IndicatorClassification.GetIC(FilterFieldType.None, string.Empty, ICCategoryScheme, FieldSelection.Light));
                }
                foreach (DataRow DrIC in DtIC.Rows)
                {
                    switch (DrIC[IndicatorClassifications.ICType].ToString())
                    {
                        case "SC":
                            ICTypeEnum = ICType.Sector;
                            ParentCSId = Constants.CategoryScheme.Sector.Id;
                            ParentCSVersion = Constants.CategoryScheme.Sector.Version;
                            break;
                        case "GL":
                            ICTypeEnum = ICType.Goal;
                            ParentCSId = Constants.CategoryScheme.Goal.Id;
                            ParentCSVersion = Constants.CategoryScheme.Goal.Version;
                            break;
                        case "CF":
                            ICTypeEnum = ICType.CF;
                            ParentCSId = Constants.CategoryScheme.Framework.Id;
                            ParentCSVersion = Constants.CategoryScheme.Framework.Version;
                            break;
                        case "IT":
                            ICTypeEnum = ICType.Institution;
                            ParentCSId = Constants.CategoryScheme.Institution.Id;
                            ParentCSVersion = Constants.CategoryScheme.Institution.Version;
                            break;
                        case "TH":
                            ICTypeEnum = ICType.Theme;
                            ParentCSId = Constants.CategoryScheme.Theme.Id;
                            ParentCSVersion = Constants.CategoryScheme.Theme.Version;
                            break;
                        case "SR":
                            ICTypeEnum = ICType.Source;
                            ParentCSId = Constants.CategoryScheme.Source.Id;
                            ParentCSVersion = Constants.CategoryScheme.Source.Version;
                            break;
                        case "CV":
                            ICTypeEnum = ICType.Convention;
                            ParentCSId = Constants.CategoryScheme.Convention.Id;
                            ParentCSVersion = Constants.CategoryScheme.Convention.Version;
                            break;
                        default:
                            break;
                    }

                    ICNId = DrIC[IndicatorClassifications.ICNId].ToString();
                    ICGId = DrIC[IndicatorClassifications.ICGId].ToString();

                    DtICIUS = this._diConnection.ExecuteDataTable(this._diQueries.IUS.GetIUSByIC(ICTypeEnum, ICNId, FieldSelection.Light));

                    foreach (DataRow DrICIUS in DtICIUS.Rows)
                    {
                        IUSGId = DrICIUS[Indicator.IndicatorGId].ToString() + Constants.AtTheRate + DrICIUS[Unit.UnitGId].ToString() + Constants.AtTheRate + DrICIUS[SubgroupVals.SubgroupValGId].ToString();

                        Categorisation = new CategorisationType(Constants.Categorization.Prefix + ICGId + Constants.AtTheRate + IUSGId, this.AgencyId, Constants.Categorization.Version, Constants.Categorization.Name, Constants.Categorization.Description, Constants.DefaultLanguage, null);
                        Categorisation.Source = new ObjectReferenceType();
                        Categorisation.Source.Items = new List<object>();
                        Categorisation.Source.Items.Add(new ObjectRefType(IUSGId, this.AgencyId, null, Constants.CodeList.IUS.Id, Constants.CodeList.IUS.Version, ObjectTypeCodelistType.Code, true, PackageTypeCodelistType.codelist, true));

                        Categorisation.Target = new CategoryReferenceType();
                        Categorisation.Target.Items = new List<object>();
                        Categorisation.Target.Items.Add(new CategoryRefType(ICGId, this.AgencyId, ParentCSId, ParentCSVersion));

                        Artefact = this.Prepare_ArtefactInfo_From_Categorisation(Categorisation, Constants.Categorization.Prefix + ICGId + Constants.AtTheRate + IUSGId + Constants.XmlExtension);
                        this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);
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

        private ArtefactInfo Prepare_ArtefactInfo_From_Categorisation(CategorisationType Categorisation, string FileName)
        {
            ArtefactInfo RetVal;
            SDMXObjectModel.Message.StructureType Structure;
            XmlDocument XmlContent;

            RetVal = null;
            XmlContent = null;

            try
            {
                Structure = this.Get_Structure_Object(Categorisation);
                XmlContent = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.StructureType), Structure);
                RetVal = new ArtefactInfo(Categorisation.id, Categorisation.agencyID, Categorisation.version, string.Empty, ArtefactTypes.Categorisation, FileName, XmlContent);
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

        private SDMXObjectModel.Message.StructureType Get_Structure_Object(CategorisationType Categorisation)
        {
            SDMXObjectModel.Message.StructureType RetVal;

            RetVal = new SDMXObjectModel.Message.StructureType();
            RetVal.Header = this.Get_Appropriate_Header();
            RetVal.Structures = new StructuresType(null, null, null, null, null, null, null, null, null, null, Categorisation, null, null, null, null);
            RetVal.Footer = null;

            return RetVal;
        }

        #endregion "--Private--"

        #region "--Public--"

        public override List<ArtefactInfo> Generate_Artefact()
        {
            List<ArtefactInfo> RetVal;

            RetVal = null;

            try
            {
                RetVal = this.Generate_Categorisations();
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

        public List<ArtefactInfo> Generate_CompleteArtefact(string IcType)
        {
            List<ArtefactInfo> RetVal;

            RetVal = null;

            try
            {
                RetVal = this.Generate_Categorisations(IcType);
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


        #endregion "--Public--"

        #endregion "--Methods--""

    }
}
