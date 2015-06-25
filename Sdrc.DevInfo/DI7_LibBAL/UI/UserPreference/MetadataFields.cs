using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace DevInfo.Lib.DI_LibBAL.UI.UserPreference
{
    public class MetadataFields
    {
        #region " -- Enum -- "

        public enum MetadataFieldType
        {
            MD_IND,
            MD_AREA,
            MD_SRC
        }

        #endregion

        #region " -- Public -- "

        #region " -- New / Dispose "

        public MetadataFields(string maskFolderPath, string languageFileName)
        {
            this.MaskFolderPath = maskFolderPath;
            this.LanguageFileNameWithoutExtension = Path.GetFileNameWithoutExtension(languageFileName);
        }

        #endregion

        #region " -- Constants -- "

        /// <summary>
        /// IndMask.xml
        /// </summary>
        private const string INDICATOR_MASK_FILE = "IndMask.xml";

        /// <summary>
        /// MapMask.xml
        /// </summary>
        private const string AREA_MASK_FILE = "MapMask.xml";

        /// <summary>
        /// SrcMask.xml
        /// </summary>
        private const string SOURCE_MASK_FILE = "SrcMask.xml";

        #endregion

        #region " -- Properties -- "

        private MetadataFieldType _MetadataFieldType;
        /// <summary>
        /// Gets or sets the metadata field type.
        /// </summary>
        public MetadataFieldType FieldType
        {
            get 
            {
                return this._MetadataFieldType; 
            }
            set 
            {
                this._MetadataFieldType = value;
            }
        }      


        #endregion

        #region " -- Methods -- "

        /// <summary>
        /// Get the Metadata XML string
        /// </summary>
        /// <returns></returns>
        public string GetMetadataString()
        {
            string Retval = string.Empty;
            try
            {
                // -- Get the metadata file name
                string MetadataFileName = this.GetMetadataFile();

                if (!string.IsNullOrEmpty(MetadataFileName) && File.Exists(MetadataFileName))
                {

                    XmlDocument XMLDoc = new XmlDocument();
                    XMLDoc.Load(MetadataFileName);                    
                    //Retval = @"<treenode CheckBox='True' Checked='False' text='Metadata '>";
                    Retval = @"<treenode ID=' ' text='Metadata '>";
                    for (int i = 0; i < XMLDoc.DocumentElement.ChildNodes.Count; i++)
                    {
                        XmlNodeList XMLNodeList;
                        // -- Get the metadata id
                        XMLNodeList = XMLDoc.DocumentElement.ChildNodes[i].SelectNodes("ID");
                        Retval += " <treenode ID='" + XMLNodeList[0].InnerText + "' " + " text='";
                        // -- Get the metadata caption
                        XMLNodeList = XMLDoc.DocumentElement.ChildNodes[i].SelectNodes("Caption");
                        for (int j = 0; j <= XMLNodeList.Count - 1; j++)
                        {
                            if (XMLNodeList[j].Attributes.GetNamedItem("lang").Value.ToLower() == this.LanguageFileNameWithoutExtension.Substring(this.LanguageFileNameWithoutExtension.Length-3,2).ToLower())
                            {
                                Retval += XMLNodeList[j].InnerText + "'/>";
                                break;
                            }
                        }
                    }
                    Retval += "</treenode>";
                }
                
            }
            catch (Exception)
            {
                Retval = string.Empty;
            }
            return Retval;
        }

        #endregion

        #endregion

        #region " -- Private -- "

        #region " -- Variables -- "

        /// <summary>
        /// Mask folder path
        /// </summary>
        private string MaskFolderPath = string.Empty;

        // -- User pref object
        private string LanguageFileNameWithoutExtension = string.Empty;

        #endregion

        #region " -- Methods -- "

        /// <summary>
        /// Get the metadata file name
        /// </summary>
        /// <returns></returns>
        private string GetMetadataFile()
        {
            string Retval = string.Empty;
            try
            {
                switch (this._MetadataFieldType)
                {
                    case MetadataFieldType.MD_IND:
                        Retval = Path.Combine(this.MaskFolderPath, INDICATOR_MASK_FILE);
                        break;
                    case MetadataFieldType.MD_AREA:
                        Retval = Path.Combine(this.MaskFolderPath, AREA_MASK_FILE);
                        break;
                    case MetadataFieldType.MD_SRC:
                        Retval = Path.Combine(this.MaskFolderPath, SOURCE_MASK_FILE);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                Retval = string.Empty;
            }
            return Retval;
        }
        
        #endregion

        #endregion
    }
}
