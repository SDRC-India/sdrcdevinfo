using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL.Controls.LookInWindowBAL;
using DevInfo.Lib.DI_LibBAL.Utility.MRU;

namespace DevInfo.Lib.DI_LibBAL.Controls.LookInWindowBAL
{
    /// <summary>
    /// Based on Factory Design pattern. Helps in creating an instance of BaseLookInSource for LookInWindowControl
    /// </summary>
    public class ListViewFactory
    {
        /// <summary>
        /// This method is based on Factory design pattern
        /// </summary>
        /// <param name="elementImportType"></param>
        public static BaseLookInSource CreateInstance(ElementImportType elementImportType)
        {
            
            BaseLookInSource RetVal = null;
            string FileSelectionFilterString = string.Empty;
            MRUKey DefaultFileType = MRUKey.MRU_TEMPLATES;
            FileSelectionFilterString = DICommon.FilterType.MS_TEMPLATE_TPL + "|" + DICommon.FilterType.Database ;

            

            switch (elementImportType)
            {
                case ElementImportType.Indicator:
                    RetVal = new IndicatorLookInSource();
                    RetVal.ImageIndex = 0;
                    RetVal.ImportFileType = DataSourceType.Excel;

                    FileSelectionFilterString += "|" + DICommon.FilterType.MS_EXCEL_XLS + "|" + DICommon.FilterType.MS_EXCEL_XLSX + "|" + DICommon.FilterType.MS_XML_XML;
                    RetVal.ShowDESButton = true;
                    break;

                case ElementImportType.Unit:
                    RetVal = new UnitLookInSource();
                    RetVal.ImageIndex = 1;
                    break;

                case ElementImportType.SubgroupType:
                    RetVal = new DISubgroupTypeLookInSource();
                    RetVal.ImageIndex = -1;
                    break;

                case ElementImportType.Subgroups:
                    RetVal = new DISubgroupLookInSource();
                    RetVal.ImageIndex = -1;
                    break;
                                   
                case ElementImportType.SubgroupVal:
                    RetVal = new SubgroupValLookInSource();
                    RetVal.ImageIndex = 2;
                    break;
                
                case ElementImportType.IUS:
                    RetVal = new IUSLookInSource();
                    RetVal.ImageIndex = 0;
                    break;
                case ElementImportType.Area:
                    RetVal = new AreaLookInSource(false);
                    RetVal.ImportFileType = DataSourceType.Excel;
                    RetVal.ImageIndex = 0;

                    FileSelectionFilterString += "|" + DICommon.FilterType.MS_EXCEL_XLS + "|" + DICommon.FilterType.MS_EXCEL_XLSX + "|" + DICommon.FilterType.MS_XML_XML;
                    break;

                case ElementImportType.AreaMaps:
                    RetVal = new AreaLookInSource(true);
                    RetVal.ImageIndex = 0;
                    FileSelectionFilterString += "|" + DICommon.FilterType.MS_XML_XML;
                    break;
                case ElementImportType.Framework:
                    RetVal = new ICLookInSource(ICType.CF);
                    RetVal.ImageIndex = 0;
                    FileSelectionFilterString += "|" + DICommon.FilterType.MS_XML_XML;
                    break;
                case ElementImportType.Sector:
                    RetVal = new ICLookInSource(ICType.Sector);
                    RetVal.ImageIndex = 0;
                    FileSelectionFilterString += "|" + DICommon.FilterType.MS_XML_XML;
                    break;
                case ElementImportType.Goal:
                    RetVal = new ICLookInSource(ICType.Goal);
                    RetVal.ImageIndex = 0;
                    FileSelectionFilterString += "|" + DICommon.FilterType.MS_XML_XML;
                    break;
                case ElementImportType.Institution:
                    RetVal = new ICLookInSource(ICType.Institution);
                    RetVal.ImageIndex = 0;
                    FileSelectionFilterString += "|" + DICommon.FilterType.MS_XML_XML;
                    break;
                case ElementImportType.Convention:
                    RetVal = new ICLookInSource(ICType.Convention);
                    RetVal.ImageIndex = 0;
                    FileSelectionFilterString += "|" + DICommon.FilterType.MS_XML_XML;
                    break;
                case ElementImportType.Theme:
                    RetVal = new ICLookInSource(ICType.Theme);
                    RetVal.ImageIndex = 0;
                    FileSelectionFilterString += "|" + DICommon.FilterType.MS_XML_XML;
                    break;

                case ElementImportType.Source:
                    RetVal = new DISourceLookInSource();
                    RetVal.ImageIndex = 0;
                    FileSelectionFilterString = DICommon.FilterType.Database;
                    DefaultFileType = MRUKey.MRU_DATABASES;
                    break;

                case ElementImportType.Timeperiod:
                    RetVal = new TimeperiodLookInSource();
                    RetVal.ImageIndex = 0;
                    FileSelectionFilterString = DICommon.FilterType.Database;
                    DefaultFileType = MRUKey.MRU_DATABASES;
                    break;

                case ElementImportType.MetaDataConvention:
                    RetVal = new MetaDataICLookInSource(ICType.Convention);
                    RetVal.ImageIndex = 0;
                    RetVal.ImportFileType = DataSourceType.RTF;
                    RetVal.IsEnableSearching = true;
                    break;

                case ElementImportType.MetaDataFramework:
                    RetVal = new MetaDataICLookInSource(ICType.CF);
                    RetVal.ImageIndex = 0;
                    RetVal.ImportFileType = DataSourceType.RTF;
                    RetVal.IsEnableSearching = true;
                    break;

                case ElementImportType.MetaDataGoal:
                    RetVal = new MetaDataICLookInSource(ICType.Goal);
                    RetVal.ImageIndex = 0;
                    RetVal.ImportFileType = DataSourceType.RTF;
                    RetVal.IsEnableSearching = true;
                    break;

                case ElementImportType.MetaDataIndicator:
                    RetVal = new MetaDataIndicatorLookInSource();
                    RetVal.ImageIndex = 0;
                    RetVal.ImportFileType = DataSourceType.XML_EXCEL;
                    RetVal.ShowDESButton = true;
                    RetVal.IsEnableSearching = true;
                    break;

                case ElementImportType.MetaDataInstitution:
                    RetVal = new MetaDataICLookInSource(ICType.Institution);
                    RetVal.ImageIndex = 0;
                    RetVal.ImportFileType = DataSourceType.RTF;
                    RetVal.IsEnableSearching = true;
                    break;

                case ElementImportType.MetaDataMap:
                    RetVal = new MetaDataMapLookInSource();
                    RetVal.ImageIndex = 0;
                    RetVal.ImportFileType = DataSourceType.XML_EXCEL;
                    RetVal.ShowDESButton = true;
                    RetVal.IsEnableSearching = true;
                    break;

                case ElementImportType.MetaDataSector:
                    RetVal = new MetaDataICLookInSource(ICType.Sector);
                    RetVal.ImageIndex = 0;
                    RetVal.ImportFileType = DataSourceType.RTF;
                    RetVal.IsEnableSearching = true;
                    break;

                case ElementImportType.MetaDataSource:
                    RetVal = new MetaDataICLookInSource(ICType.Source);
                    RetVal.ImageIndex = 0;
                    RetVal.ImportFileType = DataSourceType.XML_EXCEL;
                    RetVal.IsEnableSearching = true;
                    break;

                case ElementImportType.MetaDataTheme:
                    RetVal = new MetaDataICLookInSource( ICType.Theme);
                    RetVal.ImageIndex = 0;
                    RetVal.ImportFileType = DataSourceType.RTF;                    
                    RetVal.IsEnableSearching = true;
                    break;
                case ElementImportType.MetaDataCategoryIndicator:
                    RetVal = new MetaDataCategoryIndicatorLookInSource();
                    RetVal.ImageIndex = 0;
                    RetVal.ShowDESButton = false;
                    RetVal.IsEnableSearching = true;
                    break;
                case ElementImportType.MetaDataCategoryMap:
                    RetVal = new MetaDataCategoryAreaLookInSource();
                    RetVal.ImageIndex = 0;
                    RetVal.IsEnableSearching = true;
                    break;
                case ElementImportType.MetaDataCategorySource:
                    RetVal = new MetaDataCategorySourceLookInSource();
                    RetVal.ImageIndex = 0;
                    RetVal.IsEnableSearching = true;
                    break;
                case ElementImportType.Footnote:
                    RetVal = new FootnoteLookInSource();
                    RetVal.ImageIndex = 0;
                    FileSelectionFilterString = DICommon.FilterType.Database;
                    DefaultFileType = MRUKey.MRU_DATABASES;
                    break;
                default:
                    break;
            }

            RetVal.FileSelectionFilterString = FileSelectionFilterString;
            RetVal.DefaultFileType = DefaultFileType;
            
            return RetVal;
        }

    }
}
