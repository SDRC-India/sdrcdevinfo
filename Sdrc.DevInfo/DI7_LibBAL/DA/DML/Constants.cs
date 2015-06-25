using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{

    internal class Constants
    {
        internal const string DefaultPublisherName = "Global";
        internal const string SourceSeparator = "_";
        internal const string PrefixForNewValue = "#";
        internal const string BlankTemplateFileName = "TempTemplateForDML.tpl";
        internal const string DIMetedataRootNode = "Indicator_Info";
        /// <summary>
        /// Metadata_Category Constants
        /// </summary>
        internal class MetadataCategory
        {
            internal const string MaskFolderPath = "Bin\\Templates\\Metadata\\Mask";

            internal const string DIAdaptationsFileName = "DIAdaptations.xml";
            internal const string IndicatorMaskFileName = "IndMask.xml";
            internal const string AreaMapMaskFileName = "MapMask.xml";
            internal const string SourceMaskFileName = "SrcMask.xml";

            internal const string DIAdaptationElementName = "DIAdaptation";
            internal const string DIAdaptionFooderElementName = "AdaptationFolder";
            internal const string RootElementName = "root";
            internal const string LangAttributeName = "lang";
            internal const string CaptionElementName = "Caption";

            internal const string Metadata = "metadata";
            internal const string MetadataCategoryNodePath = "metadata/Category";
            internal const string Category = "Category";
            internal const string NameAttribute = "name";

            internal const string IndicatorCategoryType = "I";
            internal const string AreaCategoryType = "A";
            internal const string SourceCategoryType = "S";
        }

        internal class Data
        {
            internal const string Orginal_Textual_Data_valueColumn = "orgTextual_Data_value";
            internal const string Orginal_Data_valueColumn = "orgData_value";
        }        
    }   
}
