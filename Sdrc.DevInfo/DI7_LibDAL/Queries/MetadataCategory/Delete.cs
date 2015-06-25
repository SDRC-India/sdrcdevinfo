using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.MetadataCategory
{
    public class Delete
    {

        #region "-- public --"
        
        #region "-- Methods --"

        /// <summary>
        /// Returns sql query to delete records from UT_Metadata_Category_en table for the given CategoryNId
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="categoryNIds"></param>
        /// <returns></returns>
        public static string DeleteMetadataCategory(string tableName, string categoryNIds)
        {
            string RetVal = string.Empty;

            RetVal = "Delete FROM " + tableName;

            if (!string.IsNullOrEmpty(categoryNIds))
            {
                RetVal += " WHERE " + DIColumns.Metadata_Category.CategoryNId + " IN(" + categoryNIds + ")";
            }

            return RetVal;
        }

        #endregion

        #endregion

    }
}
