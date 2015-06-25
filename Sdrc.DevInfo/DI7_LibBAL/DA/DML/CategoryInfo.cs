using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    /// stores category and metadata
    /// </summary>
    public class CategoryInfo : MetadataCategoryInfo
    {

        
        private string _MetadataText = string.Empty;
        /// <summary>
        /// Gets or sets Metadata Text.
        /// </summary>
        public string MetadataText
        {
            get { return this._MetadataText; }
            set { this._MetadataText = value; }
        }
    }
}
