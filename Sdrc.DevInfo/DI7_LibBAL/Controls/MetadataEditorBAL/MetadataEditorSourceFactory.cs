using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibBAL.Controls.MetadataEditorBAL
{
    /// <summary>
    /// Helps in creating instance of MetadataEditorSoruce. Based on factory design pattern.
    /// </summary>
    public static class MetadataEditorSourceFactory
    {
        /// <summary>
        /// Returns the instance of MetadataEditorSource for the given element type
        /// </summary>
        /// <param name="elementNid"></param>
        /// <param name="elementType"></param>
        /// <param name="dbConnection"></param>
        /// <param name="dbQueries"></param>
        /// <returns></returns>
        public static MetadataEditorSource CreateInstance(int elementNid, MetaDataType elementType, DIConnection dbConnection, DIQueries dbQueries)
        {
            MetadataEditorSource RetVal = null;

            switch (elementType)
            {
                case MetaDataType.Indicator:
                    RetVal = new IndicatorMetadataEditorSource();
                    RetVal._ImageElementType = "MI";
                    break;

                case MetaDataType.Map:
                    RetVal = new AreaMetadataEditorSource();
                    RetVal._ImageElementType = "MA";
                    break;

                case MetaDataType.Source:
                    RetVal = new DISourceMetadataEditorSource();
                    RetVal._ImageElementType = "MS"; 
                    break;
                                        
                default:
                    RetVal = new ICMetadataEditorSource();
                    RetVal._ImageElementType = string.Empty;
                    RetVal._IsRtfMetadata = true;
                    break;
            }

            // set properties
            if (RetVal != null)
            {
                RetVal.DBConnection = dbConnection;
                RetVal.DBQueries = dbQueries;                
            }

            return RetVal;
        }
    }
}
