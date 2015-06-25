using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    /// stores categories and categoryinfo object
    /// </summary>
    public class MetadataInfo
    {
        private Dictionary<string, CategoryInfo> _Categories = new Dictionary<string, CategoryInfo>();
        /// <summary>
        /// Gets or sets Categories. Key is category name and value is object of categoryInfo
        /// </summary>
        public Dictionary<string, CategoryInfo> Categories
        {
            get { return this._Categories; }
            set { this._Categories = value; }
        }
    }
}
