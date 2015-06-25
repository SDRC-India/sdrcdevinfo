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
    internal class CategorySchemeUtility : ArtefactUtility
    {
        #region "--Properties--"

        #region "--Private--"

        private bool _completeOrSummaryFlag;

        private bool _multiLanguageHandlingRequired;

        private DIConnection _diConnection;

        private DIQueries _diQueries;

        private CategorySchemeTypes _categorySchemeType;

        private Dictionary<string, string> _dictIndicator;

        private Dictionary<string, string> _dictIndicatorMapping;

        #endregion "--Private--"

        #region "--Public--"

        internal bool CompleteOrSummaryFlag
        {
            get
            {
                return this._completeOrSummaryFlag;
            }
            set
            {
                this._completeOrSummaryFlag = value;
            }
        }

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

        internal Dictionary<string, string> DictIndicator
        {
            get
            {
                return this._dictIndicator;
            }
            set
            {
                this._dictIndicator = value;
            }
        }

        internal Dictionary<string, string> DictIndicatorMapping
        {
            get
            {
                return this._dictIndicatorMapping;
            }
            set
            {
                this._dictIndicatorMapping = value;
            }
        }

        #endregion "--Public--"

        #endregion "--Properties--""

        #region "--Constructors--"

        #region "--Private--"

        #endregion "--Private--"

        #region "--Public--"

        internal CategorySchemeUtility(CategorySchemeTypes categorySchemeType, bool completeOrSummaryFlag, string agencyId, string language, Header header, string outputFolder, Dictionary<string, string> DictIndicator, Dictionary<string, string> DictIndicatorMapping, DIConnection DIConnection, DIQueries DIQueries)
            : base(agencyId, language, header, outputFolder)
        {
            this._categorySchemeType = categorySchemeType;
            this._completeOrSummaryFlag = completeOrSummaryFlag;
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

            this._dictIndicator = DictIndicator;
            this._dictIndicatorMapping = DictIndicatorMapping;
        }

        #endregion "--Public--"

        #endregion "--Constructors--""

        #region "--Methods--"

        #region "--Private--"

        private ArtefactInfo Generate_Sector_CategoryScheme()
        {
            ArtefactInfo RetVal;
            CategorySchemeType CategoryScheme;
            CategoryType Category;
            string ICGId, ICNId, ICName;
            string Query;
            DataTable DtICs;
            RetVal = null;

            try
            {

                CategoryScheme = new CategorySchemeType(Constants.CategoryScheme.Sector.Id, this.AgencyId, Constants.CategoryScheme.Sector.Version, Constants.CategoryScheme.Sector.Name, Constants.CategoryScheme.Sector.Description, Constants.DefaultLanguage, null);

                if (this._completeOrSummaryFlag == true)
                {
                    Query = this.Get_Language_Specific_Query(CategorySchemeTypes.Sector, FilterFieldType.None, string.Empty, this.Language);
                    DtICs = this.DIConnection.ExecuteDataTable(Query);

                    foreach (DataRow DrICs in DtICs.Select(IndicatorClassifications.ICParent_NId + Constants.EqualsTo + Constants.MinusOne))
                    {
                        ICNId = DrICs[IndicatorClassifications.ICNId].ToString();
                        ICGId = DrICs[IndicatorClassifications.ICGId].ToString();
                        ICName = DrICs[IndicatorClassifications.ICName].ToString();

                        Category = new CategoryType(ICGId, ICName, string.Empty, this.Language, null);
                        if (this.MultiLanguageHandlingRequired)
                        {
                            this.Handle_All_Languages(CategorySchemeTypes.Sector, Category, ICGId);
                        }

                        this.Add_Annotation(Category, Constants.Annotations.CategoryType, Constants.Annotations.IC);
                        this.Add_Children_Categories(CategorySchemeTypes.Sector, Category, ICNId, DtICs);
                        CategoryScheme.Items.Add(Category);
                    }

                    if (CategoryScheme.Items.Count > 0)
                    {
                        RetVal = this.Prepare_ArtefactInfo_From_CategoryScheme(CategoryScheme, this.Get_File_Name(CategorySchemeTypes.Sector));
                    }
                }
                else
                {
                    RetVal = this.Prepare_ArtefactInfo_From_CategoryScheme(CategoryScheme, this.Get_File_Name(CategorySchemeTypes.Sector));
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

        private ArtefactInfo Generate_Goal_CategoryScheme()
        {
            ArtefactInfo RetVal;
            CategorySchemeType CategoryScheme;
            CategoryType Category;
            string ICGId, ICNId, ICName;
            string Query;
            DataTable DtICs;
            RetVal = null;

            try
            {
                CategoryScheme = new CategorySchemeType(Constants.CategoryScheme.Goal.Id, this.AgencyId, Constants.CategoryScheme.Goal.Version, Constants.CategoryScheme.Goal.Name, Constants.CategoryScheme.Goal.Description, Constants.DefaultLanguage, null);

                if (this._completeOrSummaryFlag == true)
                {
                    Query = this.Get_Language_Specific_Query(CategorySchemeTypes.Goal, FilterFieldType.None, string.Empty, this.Language);
                    DtICs = this.DIConnection.ExecuteDataTable(Query);

                    foreach (DataRow DrICs in DtICs.Select(IndicatorClassifications.ICParent_NId + Constants.EqualsTo + Constants.MinusOne))
                    {
                        ICNId = DrICs[IndicatorClassifications.ICNId].ToString();
                        ICGId = DrICs[IndicatorClassifications.ICGId].ToString();
                        ICName = DrICs[IndicatorClassifications.ICName].ToString();

                        Category = new CategoryType(ICGId, ICName, string.Empty, this.Language, null);
                        if (this.MultiLanguageHandlingRequired)
                        {
                            this.Handle_All_Languages(CategorySchemeTypes.Goal, Category, ICGId);
                        }

                        this.Add_Children_Categories(CategorySchemeTypes.Goal, Category, ICNId, DtICs);
                        CategoryScheme.Items.Add(Category);
                    }

                    if (CategoryScheme.Items.Count > 0)
                    {
                        RetVal = this.Prepare_ArtefactInfo_From_CategoryScheme(CategoryScheme, this.Get_File_Name(CategorySchemeTypes.Goal));
                    }
                }
                else
                {
                    RetVal = this.Prepare_ArtefactInfo_From_CategoryScheme(CategoryScheme, this.Get_File_Name(CategorySchemeTypes.Goal));
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

        private ArtefactInfo Generate_Theme_CategoryScheme()
        {
            ArtefactInfo RetVal;
            CategorySchemeType CategoryScheme;
            CategoryType Category;
            string ICGId, ICNId, ICName;
            string Query;
            DataTable DtICs;
            RetVal = null;

            try
            {
                CategoryScheme = new CategorySchemeType(Constants.CategoryScheme.Theme.Id, this.AgencyId, Constants.CategoryScheme.Theme.Version, Constants.CategoryScheme.Theme.Name, Constants.CategoryScheme.Theme.Description, Constants.DefaultLanguage, null);

                if (this._completeOrSummaryFlag == true)
                {
                    Query = this.Get_Language_Specific_Query(CategorySchemeTypes.Theme, FilterFieldType.None, string.Empty, this.Language);
                    DtICs = this.DIConnection.ExecuteDataTable(Query);

                    foreach (DataRow DrICs in DtICs.Select(IndicatorClassifications.ICParent_NId + Constants.EqualsTo + Constants.MinusOne))
                    {
                        ICNId = DrICs[IndicatorClassifications.ICNId].ToString();
                        ICGId = DrICs[IndicatorClassifications.ICGId].ToString();
                        ICName = DrICs[IndicatorClassifications.ICName].ToString();

                        Category = new CategoryType(ICGId, ICName, string.Empty, this.Language, null);
                        if (this.MultiLanguageHandlingRequired)
                        {
                            this.Handle_All_Languages(CategorySchemeTypes.Theme, Category, ICGId);
                        }

                        this.Add_Children_Categories(CategorySchemeTypes.Theme, Category, ICNId, DtICs);
                        CategoryScheme.Items.Add(Category);
                    }

                    if (CategoryScheme.Items.Count > 0)
                    {
                        RetVal = this.Prepare_ArtefactInfo_From_CategoryScheme(CategoryScheme, this.Get_File_Name(CategorySchemeTypes.Theme));
                    }
                }
                else
                {
                    RetVal = this.Prepare_ArtefactInfo_From_CategoryScheme(CategoryScheme, this.Get_File_Name(CategorySchemeTypes.Theme));
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

        private ArtefactInfo Generate_Source_CategoryScheme()
        {
            ArtefactInfo RetVal;
            CategorySchemeType CategoryScheme;
            CategoryType Category;
            string ICGId, ICNId, ICName;
            string Query;
            DataTable DtICs;
            RetVal = null;

            try
            {
                CategoryScheme = new CategorySchemeType(Constants.CategoryScheme.Source.Id, this.AgencyId, Constants.CategoryScheme.Source.Version, Constants.CategoryScheme.Source.Name, Constants.CategoryScheme.Source.Description, Constants.DefaultLanguage, null);

                if (this._completeOrSummaryFlag == true)
                {
                    Query = this.Get_Language_Specific_Query(CategorySchemeTypes.Source, FilterFieldType.None, string.Empty, this.Language);
                    DtICs = this.DIConnection.ExecuteDataTable(Query);

                    foreach (DataRow DrICs in DtICs.Select(IndicatorClassifications.ICParent_NId + Constants.EqualsTo + Constants.MinusOne))
                    {
                        ICNId = DrICs[IndicatorClassifications.ICNId].ToString();
                        ICGId = DrICs[IndicatorClassifications.ICGId].ToString();
                        ICName = DrICs[IndicatorClassifications.ICName].ToString();

                        Category = new CategoryType(ICGId, ICName, string.Empty, this.Language, null);
                        if (this.MultiLanguageHandlingRequired)
                        {
                            this.Handle_All_Languages(CategorySchemeTypes.Source, Category, ICGId);
                        }

                        this.Add_Children_Categories(CategorySchemeTypes.Source, Category, ICNId, DtICs);
                        CategoryScheme.Items.Add(Category);
                    }
                    if (CategoryScheme.Items.Count > 0)
                    {
                        RetVal = this.Prepare_ArtefactInfo_From_CategoryScheme(CategoryScheme, this.Get_File_Name(CategorySchemeTypes.Source));
                    }
                }
                else
                {
                    RetVal = this.Prepare_ArtefactInfo_From_CategoryScheme(CategoryScheme, this.Get_File_Name(CategorySchemeTypes.Source));
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

        private ArtefactInfo Generate_Convention_CategoryScheme()
        {
            ArtefactInfo RetVal;
            CategorySchemeType CategoryScheme;
            CategoryType Category;
            string ICGId, ICNId, ICName;
            string Query;
            DataTable DtICs;
            RetVal = null;

            try
            {
                CategoryScheme = new CategorySchemeType(Constants.CategoryScheme.Convention.Id, this.AgencyId, Constants.CategoryScheme.Convention.Version, Constants.CategoryScheme.Convention.Name, Constants.CategoryScheme.Convention.Description, Constants.DefaultLanguage, null);

                if (this._completeOrSummaryFlag == true)
                {
                    Query = this.Get_Language_Specific_Query(CategorySchemeTypes.Convention, FilterFieldType.None, string.Empty, this.Language);
                    DtICs = this.DIConnection.ExecuteDataTable(Query);

                    foreach (DataRow DrICs in DtICs.Select(IndicatorClassifications.ICParent_NId + Constants.EqualsTo + Constants.MinusOne))
                    {
                        ICNId = DrICs[IndicatorClassifications.ICNId].ToString();
                        ICGId = DrICs[IndicatorClassifications.ICGId].ToString();
                        ICName = DrICs[IndicatorClassifications.ICName].ToString();

                        Category = new CategoryType(ICGId, ICName, string.Empty, this.Language, null);
                        if (this.MultiLanguageHandlingRequired)
                        {
                            this.Handle_All_Languages(CategorySchemeTypes.Convention, Category, ICGId);
                        }

                        this.Add_Children_Categories(CategorySchemeTypes.Convention, Category, ICNId, DtICs);
                        CategoryScheme.Items.Add(Category);
                    }
                    if (CategoryScheme.Items.Count > 0)
                    {
                        RetVal = this.Prepare_ArtefactInfo_From_CategoryScheme(CategoryScheme, this.Get_File_Name(CategorySchemeTypes.Convention));
                    }
                }
                else
                {
                    RetVal = this.Prepare_ArtefactInfo_From_CategoryScheme(CategoryScheme, this.Get_File_Name(CategorySchemeTypes.Convention));
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

        private ArtefactInfo Generate_Framework_CategoryScheme()
        {
            ArtefactInfo RetVal;
            CategorySchemeType CategoryScheme;
            CategoryType Category;
            string ICGId, ICNId, ICName;
            string Query;
            DataTable DtICs;
            RetVal = null;

            try
            {
                CategoryScheme = new CategorySchemeType(Constants.CategoryScheme.Framework.Id, this.AgencyId, Constants.CategoryScheme.Framework.Version, Constants.CategoryScheme.Framework.Name, Constants.CategoryScheme.Framework.Description, Constants.DefaultLanguage, null);

                if (this._completeOrSummaryFlag == true)
                {
                    Query = this.Get_Language_Specific_Query(CategorySchemeTypes.Framework, FilterFieldType.None, string.Empty, this.Language);
                    DtICs = this.DIConnection.ExecuteDataTable(Query);

                    foreach (DataRow DrICs in DtICs.Select(IndicatorClassifications.ICParent_NId + Constants.EqualsTo + Constants.MinusOne))
                    {
                        ICNId = DrICs[IndicatorClassifications.ICNId].ToString();
                        ICGId = DrICs[IndicatorClassifications.ICGId].ToString();
                        ICName = DrICs[IndicatorClassifications.ICName].ToString();

                        Category = new CategoryType(ICGId, ICName, string.Empty, this.Language, null);
                        if (this.MultiLanguageHandlingRequired)
                        {
                            this.Handle_All_Languages(CategorySchemeTypes.Framework, Category, ICGId);
                        }

                        this.Add_Children_Categories(CategorySchemeTypes.Framework, Category, ICNId, DtICs);
                        CategoryScheme.Items.Add(Category);
                    }
                    if (CategoryScheme.Items.Count > 0)
                    {
                        RetVal = this.Prepare_ArtefactInfo_From_CategoryScheme(CategoryScheme, this.Get_File_Name(CategorySchemeTypes.Framework));
                    }
                }
                else
                {
                    RetVal = this.Prepare_ArtefactInfo_From_CategoryScheme(CategoryScheme, this.Get_File_Name(CategorySchemeTypes.Framework));
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

        private ArtefactInfo Generate_Institution_CategoryScheme()
        {
            ArtefactInfo RetVal;
            CategorySchemeType CategoryScheme;
            CategoryType Category;
            string ICGId, ICNId, ICName;
            string Query;
            DataTable DtICs;
            RetVal = null;

            try
            {
                CategoryScheme = new CategorySchemeType(Constants.CategoryScheme.Institution.Id, this.AgencyId, Constants.CategoryScheme.Institution.Version, Constants.CategoryScheme.Institution.Name, Constants.CategoryScheme.Institution.Description, Constants.DefaultLanguage, null);

                if (this._completeOrSummaryFlag == true)
                {
                    Query = this.Get_Language_Specific_Query(CategorySchemeTypes.Institution, FilterFieldType.None, string.Empty, this.Language);
                    DtICs = this.DIConnection.ExecuteDataTable(Query);

                    foreach (DataRow DrICs in DtICs.Select(IndicatorClassifications.ICParent_NId + Constants.EqualsTo + Constants.MinusOne))
                    {
                        ICNId = DrICs[IndicatorClassifications.ICNId].ToString();
                        ICGId = DrICs[IndicatorClassifications.ICGId].ToString();
                        ICName = DrICs[IndicatorClassifications.ICName].ToString();

                        Category = new CategoryType(ICGId, ICName, string.Empty, this.Language, null);
                        if (this.MultiLanguageHandlingRequired)
                        {
                            this.Handle_All_Languages(CategorySchemeTypes.Institution, Category, ICGId);
                        }

                        this.Add_Children_Categories(CategorySchemeTypes.Institution, Category, ICNId, DtICs);
                        CategoryScheme.Items.Add(Category);
                    }
                    if (CategoryScheme.Items.Count > 0)
                    {
                        RetVal = this.Prepare_ArtefactInfo_From_CategoryScheme(CategoryScheme, this.Get_File_Name(CategorySchemeTypes.Institution));
                    }
                }
                else
                {
                    RetVal = this.Prepare_ArtefactInfo_From_CategoryScheme(CategoryScheme, this.Get_File_Name(CategorySchemeTypes.Institution));
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

        private void Add_Children_Categories(CategorySchemeTypes categorySchemeType, CategoryType ParentCategory, string ICParent_NId, DataTable DtICs)
        {
            CategoryType ChildCategory;
            string ICGId, ICNId, ICName, IndicatorNId, IndicatorGId, IndicatorName;
            DataTable DtIndicators;
            DataRow[] ICRows;

            ICRows = DtICs.Select(IndicatorClassifications.ICParent_NId + Constants.EqualsTo + ICParent_NId);

            if (ICRows.Length > 0)
            {
                foreach (DataRow DrICs in ICRows)
                {
                    ICNId = DrICs[IndicatorClassifications.ICNId].ToString();
                    ICGId = DrICs[IndicatorClassifications.ICGId].ToString();
                    ICName = DrICs[IndicatorClassifications.ICName].ToString();

                    ChildCategory = new CategoryType(ICGId, ICName, string.Empty, this.Language, null);
                    if (this.MultiLanguageHandlingRequired)
                    {
                        this.Handle_All_Languages(categorySchemeType, ChildCategory, ICGId);
                    }

                    this.Add_Annotation(ChildCategory, Constants.Annotations.CategoryType, Constants.Annotations.IC);
                    this.Add_Children_Categories(categorySchemeType, ChildCategory, ICNId, DtICs);
                    ParentCategory.Items.Add(ChildCategory);
                }
            }
            else
            {
                DtIndicators = this.DIConnection.ExecuteDataTable(this.Get_Language_Specific_Query_For_Indicators(categorySchemeType, ICParent_NId, this.Language));
                foreach (DataRow DrIndicators in DtIndicators.Rows)
                {
                    IndicatorNId = DrIndicators[Indicator.IndicatorNId].ToString();
                    IndicatorGId = DrIndicators[Indicator.IndicatorGId].ToString();
                    IndicatorName = DrIndicators[Indicator.IndicatorName].ToString();

                    if (this._dictIndicatorMapping == null || this._dictIndicatorMapping.Keys.Count == 0)
                    {
                        ChildCategory = new CategoryType(IndicatorGId, IndicatorName, string.Empty, this.Language, null);
                        if (this.MultiLanguageHandlingRequired)
                        {
                            this.Handle_All_Languages_For_Indicators(categorySchemeType, ChildCategory, IndicatorGId);
                        }

                        this.Add_Annotation(ChildCategory, Constants.Annotations.CategoryType, Constants.Annotations.Indicator);
                        ParentCategory.Items.Add(ChildCategory);
                    }
                    else
                    {
                        if (this._dictIndicatorMapping.ContainsKey(IndicatorGId))
                        {
                            ChildCategory = new CategoryType(this._dictIndicatorMapping[IndicatorGId], this._dictIndicator[this._dictIndicatorMapping[IndicatorGId]], string.Empty, Constants.DefaultLanguage, null);
                            this.Add_Annotation(ChildCategory, Constants.Annotations.CategoryType, Constants.Annotations.Indicator);
                            ParentCategory.Items.Add(ChildCategory);
                        }
                    }
                }
            }
        }

        private void Handle_All_Languages(CategorySchemeTypes categorySchemeType, CategoryType Category, string ICGId)
        {
            DataTable DtTable;
            string Query, Language;

            Query = string.Empty;

            if (this.MultiLanguageHandlingRequired)
            {
                foreach (DataRow LanguageRow in this.DIConnection.DILanguages(this.DIQueries.DataPrefix).Rows)
                {
                    Language = LanguageRow[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Language.LanguageCode].ToString();
                    if (Language != this.DIQueries.LanguageCode.Substring(1))
                    {
                        Query = this.Get_Language_Specific_Query(categorySchemeType, FilterFieldType.GId, Constants.Apostophe + ICGId + Constants.Apostophe, Language);
                        DtTable = this.DIConnection.ExecuteDataTable(Query);
                        if (DtTable.Rows.Count > 0)
                        {
                            Category.Name.Add(new TextType(Language, DtTable.Rows[0][IndicatorClassifications.ICName].ToString()));
                        }
                    }
                }
            }
        }

        private void Handle_All_Languages_For_Indicators(CategorySchemeTypes categorySchemeType, CategoryType Category, string IndicatorGId)
        {
            DataTable DtTable;
            DIQueries DIQueriesLanguage;
            string Query, Language;

            Query = string.Empty;

            if (this.MultiLanguageHandlingRequired)
            {
                foreach (DataRow LanguageRow in this.DIConnection.DILanguages(this.DIQueries.DataPrefix).Rows)
                {
                    Language = LanguageRow[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Language.LanguageCode].ToString();

                    if (Language != this.DIQueries.LanguageCode.Substring(1))
                    {
                        DIQueriesLanguage = new DIQueries(this.DIQueries.DataPrefix, Language);
                        Query = DIQueriesLanguage.Indicators.GetIndicator(FilterFieldType.GId, IndicatorGId, FieldSelection.Light);

                        DtTable = this.DIConnection.ExecuteDataTable(Query);
                        if (DtTable.Rows.Count > 0)
                        {
                            Category.Name.Add(new TextType(Language, DtTable.Rows[0][Indicator.IndicatorName].ToString()));
                        }
                    }
                }
            }
        }

        private string Get_Language_Specific_Query(CategorySchemeTypes categorySchemeType, FilterFieldType filterField, string filterText, string language)
        {
            string RetVal;
            DIQueries DIQueriesLanguage;

            RetVal = string.Empty;
            // 1. Creating language specific DIQueries object.
            DIQueriesLanguage = new DIQueries(this.DIQueries.DataPrefix, language);

            switch (categorySchemeType)
            {
                case CategorySchemeTypes.Sector:
                    RetVal = DIQueriesLanguage.IndicatorClassification.GetIC(filterField, filterText, ICType.Sector, FieldSelection.Light);
                    break;
                case CategorySchemeTypes.Goal:
                    RetVal = DIQueriesLanguage.IndicatorClassification.GetIC(filterField, filterText, ICType.Goal, FieldSelection.Light);
                    break;
                case CategorySchemeTypes.Theme:
                    RetVal = DIQueriesLanguage.IndicatorClassification.GetIC(filterField, filterText, ICType.Theme, FieldSelection.Light);
                    break;
                case CategorySchemeTypes.Source:
                    RetVal = DIQueriesLanguage.IndicatorClassification.GetIC(filterField, filterText, ICType.Source, FieldSelection.Light);
                    break;
                case CategorySchemeTypes.Convention:
                    RetVal = DIQueriesLanguage.IndicatorClassification.GetIC(filterField, filterText, ICType.Convention, FieldSelection.Light);
                    break;
                case CategorySchemeTypes.Framework:
                    RetVal = DIQueriesLanguage.IndicatorClassification.GetIC(filterField, filterText, ICType.CF, FieldSelection.Light);
                    break;
                case CategorySchemeTypes.Institution:
                    RetVal = DIQueriesLanguage.IndicatorClassification.GetIC(filterField, filterText, ICType.Institution, FieldSelection.Light);
                    break;
                default:
                    break;
            }

            return RetVal;
        }

        private string Get_Language_Specific_Query_For_Indicators(CategorySchemeTypes categorySchemeType, string filterText, string language)
        {
            string RetVal;
            DIQueries DIQueriesLanguage;

            RetVal = string.Empty;
            // 1. Creating language specific DIQueries object.
            DIQueriesLanguage = new DIQueries(this.DIQueries.DataPrefix, language);

            switch (categorySchemeType)
            {
                case CategorySchemeTypes.Sector:
                    RetVal = DIQueriesLanguage.Indicators.GetIndicatorByIC(ICType.Sector, filterText, FieldSelection.Light);
                    break;
                case CategorySchemeTypes.Goal:
                    RetVal = DIQueriesLanguage.Indicators.GetIndicatorByIC(ICType.Goal, filterText, FieldSelection.Light);
                    break;
                case CategorySchemeTypes.Theme:
                    RetVal = DIQueriesLanguage.Indicators.GetIndicatorByIC(ICType.Theme, filterText, FieldSelection.Light);
                    break;
                case CategorySchemeTypes.Source:
                    RetVal = DIQueriesLanguage.Indicators.GetIndicatorByIC(ICType.Source, filterText, FieldSelection.Light);
                    break;
                case CategorySchemeTypes.Convention:
                    RetVal = DIQueriesLanguage.Indicators.GetIndicatorByIC(ICType.Convention, filterText, FieldSelection.Light);
                    break;
                case CategorySchemeTypes.Framework:
                    RetVal = DIQueriesLanguage.Indicators.GetIndicatorByIC(ICType.CF, filterText, FieldSelection.Light);
                    break;
                case CategorySchemeTypes.Institution:
                    RetVal = DIQueriesLanguage.Indicators.GetIndicatorByIC(ICType.Institution, filterText, FieldSelection.Light);
                    break;
                default:
                    break;
            }

            return RetVal;
        }

        private void Add_Annotation(CategoryType Category, string title, string text)
        {
            if (Category.Annotations == null)
            {
                Category.Annotations = new List<AnnotationType>();
            }
            AnnotationType Annotation = new AnnotationType();
            Annotation.AnnotationTitle = title;
            Annotation.AnnotationText = new List<TextType>();
            Annotation.AnnotationText.Add(new TextType(null, text));
            Category.Annotations.Add(Annotation);
        }

        private ArtefactInfo Prepare_ArtefactInfo_From_CategoryScheme(CategorySchemeType CategoryScheme, string FileName)
        {
            ArtefactInfo RetVal;
            SDMXObjectModel.Message.StructureType Structure;
            XmlDocument XmlContent;

            RetVal = null;
            XmlContent = null;

            try
            {
                Structure = this.Get_Structure_Object(CategoryScheme);
                XmlContent = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.StructureType), Structure);
                RetVal = new ArtefactInfo(CategoryScheme.id, CategoryScheme.agencyID, CategoryScheme.version, string.Empty, ArtefactTypes.CategoryS, FileName, XmlContent);
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

        private string Get_File_Name(CategorySchemeTypes categorySchemeType)
        {
            string RetVal;

            RetVal = string.Empty;

            switch (categorySchemeType)
            {
                case CategorySchemeTypes.Sector:
                    RetVal = Constants.CategoryScheme.Sector.FileName;
                    break;
                case CategorySchemeTypes.Goal:
                    RetVal = Constants.CategoryScheme.Goal.FileName;
                    break;
                case CategorySchemeTypes.Theme:
                    RetVal = Constants.CategoryScheme.Theme.FileName;
                    break;
                case CategorySchemeTypes.Source:
                    RetVal = Constants.CategoryScheme.Source.FileName;
                    break;
                case CategorySchemeTypes.Convention:
                    RetVal = Constants.CategoryScheme.Convention.FileName;
                    break;
                case CategorySchemeTypes.Framework:
                    RetVal = Constants.CategoryScheme.Framework.FileName;
                    break;
                case CategorySchemeTypes.Institution:
                    RetVal = Constants.CategoryScheme.Institution.FileName;
                    break;
                default:
                    break;
            }

            return RetVal;
        }

        private SDMXObjectModel.Message.StructureType Get_Structure_Object(CategorySchemeType CategoryScheme)
        {
            SDMXObjectModel.Message.StructureType RetVal;

            RetVal = new SDMXObjectModel.Message.StructureType();
            RetVal.Header = this.Get_Appropriate_Header();
            RetVal.Structures = new StructuresType(null, null, null, null, null, null, null, null, null, null, null, CategoryScheme, null, null, null);
            RetVal.Footer = null;

            return RetVal;
        }

        #endregion "--Private--"

        #region "--Public--"

        public override List<ArtefactInfo> Generate_Artefact()
        {
            List<ArtefactInfo> RetVal;
            ArtefactInfo Artefact;

            RetVal = null;

            try
            {
                if ((this._categorySchemeType & CategorySchemeTypes.ALL) == CategorySchemeTypes.ALL)
                {
                    Artefact = this.Generate_Sector_CategoryScheme();
                    this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);

                    Artefact = this.Generate_Goal_CategoryScheme();
                    this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);

                    Artefact = this.Generate_Theme_CategoryScheme();
                    this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);

                    Artefact = this.Generate_Source_CategoryScheme();
                    this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);

                    Artefact = this.Generate_Convention_CategoryScheme();
                    this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);

                    Artefact = this.Generate_Framework_CategoryScheme();
                    this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);

                    Artefact = this.Generate_Institution_CategoryScheme();
                    this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);
                }
                else
                {
                    if ((this._categorySchemeType & CategorySchemeTypes.Sector) == CategorySchemeTypes.Sector)
                    {
                        Artefact = this.Generate_Sector_CategoryScheme();
                        this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);
                    }
                    if ((this._categorySchemeType & CategorySchemeTypes.Goal) == CategorySchemeTypes.Goal)
                    {
                        Artefact = this.Generate_Goal_CategoryScheme();
                        this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);
                    }
                    if ((this._categorySchemeType & CategorySchemeTypes.Theme) == CategorySchemeTypes.Theme)
                    {
                        Artefact = this.Generate_Theme_CategoryScheme();
                        this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);
                    }
                    if ((this._categorySchemeType & CategorySchemeTypes.Source) == CategorySchemeTypes.Source)
                    {
                        Artefact = this.Generate_Source_CategoryScheme();
                        this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);
                    }
                    if ((this._categorySchemeType & CategorySchemeTypes.Convention) == CategorySchemeTypes.Convention)
                    {
                        Artefact = this.Generate_Convention_CategoryScheme();
                        this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);
                    }
                    if ((this._categorySchemeType & CategorySchemeTypes.Framework) == CategorySchemeTypes.Framework)
                    {
                        Artefact = this.Generate_Framework_CategoryScheme();
                        this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);
                    }
                    if ((this._categorySchemeType & CategorySchemeTypes.Institution) == CategorySchemeTypes.Institution)
                    {
                        Artefact = this.Generate_Institution_CategoryScheme();
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

        #endregion "--Public--"

        #endregion "--Methods--""

    }
}
