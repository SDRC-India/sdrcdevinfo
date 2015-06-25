using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DALQueries = DevInfo.Lib.DI_LibDAL.Queries;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL.Metadata;
using System.IO;
using System.Xml;
using System.Windows.Forms;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    /// Provide methods to Add,update,delete records in MetadataCategory table
    /// Update Metadata from mask Files
    /// </summary>
    public class DI7MetadataCategoryBuilder
    {

        #region "-- private --"

        #region "-- Variables --"

        private DIConnection DBConnection;
        private DIQueries DBQueries;

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Retruns nid only if name exists in the database
        /// </summary>
        /// <returns></returns>
        private int GetNidByName(string name, string categoryType, string parentNId)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;
            DataTable Table = null;
            try
            {
                SqlQuery = this.DBQueries.Metadata_Category.GetMetadataCategoriesByCategoryname(name, categoryType, parentNId);

                Table = this.DBConnection.ExecuteDataTable(SqlQuery);

                if (Table.Rows.Count > 0)
                {
                    RetVal = Convert.ToInt32(Table.Rows[0][Metadata_Category.CategoryNId]);
                }

            }
            catch (Exception)
            {
                RetVal = 0;
            }
            return RetVal;
        }

        /// <summary>
        /// Retruns nid only if name exists in the database
        /// </summary>
        /// <returns></returns>
        private int GetNidByName(string name, DIQueries dbQueries, string parentNId)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;
            try
            {
                SqlQuery = dbQueries.Metadata_Category.GetMetadataCategories(FilterFieldType.Name, "'" + DIQueries.RemoveQuotesForSqlQuery(name) + "'", parentNId);
                RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(SqlQuery));
            }
            catch (Exception)
            {
                RetVal = 0;
            }
            return RetVal;
        }


        /// <summary>
        /// Retruns nid only if GID exists in the database
        /// </summary>
        /// <returns></returns>
        private int GetNidByGID(string GId, string categoryType)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;
            DataTable Table = null;
            try
            {
                SqlQuery = this.DBQueries.Metadata_Category.GetMetadataCategoryNIdByGID(GId, categoryType);

                Table = this.DBConnection.ExecuteDataTable(SqlQuery);

                if (Table.Rows.Count > 0)
                {
                    RetVal = Convert.ToInt32(Table.Rows[0][Metadata_Category.CategoryNId]);
                }

            }
            catch (Exception)
            {
                RetVal = 0;
            }
            return RetVal;
        }


        #region "-- Convert Metadata --"


        #endregion

        private string GetCategoryTypeText(MetadataElementType categoryMaskType)
        {
            string RetVal = string.Empty;

            switch (categoryMaskType)
            {
                case MetadataElementType.Indicator:
                    RetVal = "I";
                    break;
                case MetadataElementType.Area:
                    RetVal = "A";
                    break;
                case MetadataElementType.Source:
                    RetVal = "S";
                    break;
            }


            return RetVal;

        }

        private static DI7MetadataCategoryInfo GetMetadataCategoryInfo(DataRow row, DIConnection dbConnection, DIQueries dbQueries)
        {
            DI7MetadataCategoryInfo RetVal = new DI7MetadataCategoryInfo();
            DI7MetadataCategoryInfo SubCategory;

            RetVal.CategoryNId = Convert.ToInt32(row[Metadata_Category.CategoryNId]);
            RetVal.CategoryGID = Convert.ToString(row[Metadata_Category.CategoryGId]);
            RetVal.CategoryName = Convert.ToString(row[Metadata_Category.CategoryName]);
            RetVal.Description = Convert.ToString(row[Metadata_Category.CategoryDescription]);
            RetVal.CategoryOrder = Convert.ToInt32(row[Metadata_Category.CategoryOrder]);
            RetVal.CategoryType = Convert.ToString(row[Metadata_Category.CategoryType]);
            RetVal.IsMandatory = Convert.ToBoolean(row[Metadata_Category.IsMandatory]);
            RetVal.IsPresentational = Convert.ToBoolean(row[Metadata_Category.IsPresentational]);
            RetVal.ParentNid = Convert.ToInt32(row[Metadata_Category.ParentCategoryNId]);

            // add subcategories (if any)
            RetVal.SubCategories = new Dictionary<string, DI7MetadataCategoryInfo>();

            foreach (DataRow SubCategoryRow in dbConnection.ExecuteDataTable(dbQueries.Metadata_Category.GetMetadataCategories(FilterFieldType.Type, "'" + RetVal.CategoryType + "'", RetVal.CategoryNId.ToString())).Rows)
            {

                if (!RetVal.SubCategories.ContainsKey(Convert.ToString(SubCategoryRow[Metadata_Category.CategoryGId])))
                {
                    RetVal.SubCategories.Add(Convert.ToString(SubCategoryRow[Metadata_Category.CategoryGId]), new DI7MetadataCategoryInfo());
                }

                // Add subcategory information
                SubCategory = RetVal.SubCategories[Convert.ToString(SubCategoryRow[Metadata_Category.CategoryGId])];
                SubCategory.CategoryNId = Convert.ToInt32(SubCategoryRow[Metadata_Category.CategoryNId]);
                SubCategory.CategoryGID = Convert.ToString(SubCategoryRow[Metadata_Category.CategoryGId]);
                SubCategory.CategoryName = Convert.ToString(SubCategoryRow[Metadata_Category.CategoryName]);
                SubCategory.Description = Convert.ToString(SubCategoryRow[Metadata_Category.CategoryDescription]);
                SubCategory.CategoryOrder = Convert.ToInt32(SubCategoryRow[Metadata_Category.CategoryOrder]);
                SubCategory.CategoryType = Convert.ToString(SubCategoryRow[Metadata_Category.CategoryType]);
                SubCategory.IsMandatory = Convert.ToBoolean(SubCategoryRow[Metadata_Category.IsMandatory]);
                SubCategory.IsPresentational = Convert.ToBoolean(SubCategoryRow[Metadata_Category.IsPresentational]);
                SubCategory.ParentNid = Convert.ToInt32(SubCategoryRow[Metadata_Category.ParentCategoryNId]);

            }


            return RetVal;
        }

        private DI7MetadataCategoryInfo GetMedataCategoryInfo(DataTable table)
        {
            DI7MetadataCategoryInfo RetVal = new DI7MetadataCategoryInfo();

            foreach (DataRow Row in table.Rows)
            {
                RetVal = DI7MetadataCategoryBuilder.GetMetadataCategoryInfo(Row, this.DBConnection, this.DBQueries);
                break;
            }

            return RetVal;
        }

        #endregion

        #endregion

        #region "-- public --"

        #region "-- Variables --"

        #endregion

        #region "-- New/Dispose --"
        /// <summary>
        /// Constructor for MetadataCategoryBuilder Class
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="queries"></param>
        public DI7MetadataCategoryBuilder(DIConnection connection, DIQueries queries)
        {
            this.DBConnection = connection;
            this.DBQueries = queries;
        }

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Returns metadata category info(with sub categories)
        /// </summary>
        /// <param name="categoryGid"></param>
        /// <returns></returns>
        public DI7MetadataCategoryInfo GetMedataCategoryInfo(string categoryGid)
        {
            DI7MetadataCategoryInfo RetVal = null;
            DataTable Table = null;

            try
            {
                Table = this.DBConnection.ExecuteDataTable(this.DBQueries.Metadata_Category.GetMetadataCategories(FilterFieldType.GId, categoryGid));
                RetVal = this.GetMedataCategoryInfo(Table);
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }

            return RetVal;
        }

        /// <summary>
        /// Returns metadata category info(with sub categories)
        /// </summary>
        /// <param name="categoryNid"></param>
        /// <returns></returns>
        public DI7MetadataCategoryInfo GetMedataCategoryInfo(int categoryNid)
        {
            DI7MetadataCategoryInfo RetVal = null;
            DataTable Table = null;

            try
            {
                Table = this.DBConnection.ExecuteDataTable(this.DBQueries.Metadata_Category.GetMetadataCategories(FilterFieldType.NId, Convert.ToString(categoryNid)));
                RetVal = this.GetMedataCategoryInfo(Table);
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }

            return RetVal;
        }

        /// <summary>
        /// Checks and insert metadata category into database.
        /// </summary>
        /// <param name="metadataCategory"></param>
        /// <returns></returns>
        public int CheckNInsertCategory(DI7MetadataCategoryInfo metadataCategory)
        {
            int RetVal = 0;

            if (!string.IsNullOrEmpty(metadataCategory.CategoryGID) && !string.IsNullOrEmpty(metadataCategory.CategoryName))
            {
                RetVal = this.GetNidByGID(metadataCategory.CategoryGID, metadataCategory.CategoryType);


                if (RetVal <= 0)
                {
                    RetVal = this.GetNidByName(metadataCategory.CategoryName, metadataCategory.CategoryType, metadataCategory.ParentNid.ToString());
                }

                if (RetVal > 0)
                {
                    // update category info
                    this.UpdateMetadataCategory(metadataCategory);
                }
                else
                {
                    // insert metadata category
                    RetVal = this.InsertIntoDatabase(metadataCategory);

                }
            }
            return RetVal;
        }



        /// <summary>
        /// Insert MetadataCategory record into database
        /// </summary>
        /// <param name="MetadataCategoryInfo">object of IndicatorInfo</param>
        /// <returns>Ture/False. Return true after successful insertion otherwise false</returns>
        public int InsertIntoDatabase(DI7MetadataCategoryInfo metadataCategory)
        {
            int RetVal = 0;
            string MetadataCategoryName = metadataCategory.CategoryName;
            string LanguageCode = string.Empty;
            string DefaultLanguageCode = string.Empty;
            string MetadataCategoryForDatabase = string.Empty;
            DITables TablesName;
            int LastOrder = 0;
            try
            {
                DefaultLanguageCode = this.DBQueries.LanguageCode;

                //get and set category order
                // get max category order from database/template
                try
                {
                    LastOrder = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(this.DBQueries.Metadata_Category.GetMaxMetadataCategoryOrder("'" + metadataCategory.CategoryType + "'", metadataCategory.ParentNid.ToString())));
                }
                catch (Exception)
                {
                }
                LastOrder += 1;
                metadataCategory.CategoryOrder = LastOrder;


                // insert metadata category into database/template
                foreach (DataRow languageRow in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                {

                    LanguageCode = languageRow[Language.LanguageCode].ToString();
                    TablesName = new DITables(this.DBQueries.DataPrefix, LanguageCode);

                    //if (LanguageCode == DefaultLanguageCode.Replace("_", String.Empty))
                    //{
                    MetadataCategoryForDatabase = MetadataCategoryName;
                    //}
                    //else
                    //{
                    //    MetadataCategoryForDatabase = Constants.PrefixForNewValue + MetadataCategoryName;
                    //}

                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.MetadataCategory.Insert.InsertMetadataCategory(TablesName.MetadataCategory, MetadataCategoryForDatabase, metadataCategory.CategoryGID,
                        metadataCategory.Description, metadataCategory.CategoryType, metadataCategory.CategoryOrder, metadataCategory.ParentNid.ToString(), metadataCategory.IsPresentational, metadataCategory.IsMandatory));

                }

                RetVal = this.DBConnection.GetNewId();
            }
            catch (Exception)
            {
                RetVal = 0;
            }

            return RetVal;

        }

        /// <summary>
        /// Updates Metadata category
        /// </summary>
        /// <param name="metadataCategoryInfo"></param>
        public void UpdateMetadataCategory(DI7MetadataCategoryInfo metadataCategoryInfo)
        {
            string SqlQuery = string.Empty;

            try
            {

                if (metadataCategoryInfo.CategoryNId > 0)
                {

                    SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.MetadataCategory.Update.UpdateMetadataCategory(this.DBQueries.TablesName.MetadataCategory, metadataCategoryInfo.CategoryNId, DICommon.RemoveQuotes(metadataCategoryInfo.CategoryName), metadataCategoryInfo.CategoryType, metadataCategoryInfo.CategoryGID, metadataCategoryInfo.Description, metadataCategoryInfo.ParentNid.ToString(), metadataCategoryInfo.IsMandatory, metadataCategoryInfo.IsPresentational);

                    this.DBConnection.ExecuteNonQuery(SqlQuery);
                }
            }
            catch (Exception)
            {

            }
        }


        /// <summary>
        /// /// Deletes metadata category records from metadata category and metadata report tables
        /// </summary>
        /// <param name="categoryNid"></param>
        public void DeleteMetadataCategory(int categoryNid)
        {
            string LanguageCode = string.Empty;
            DITables TablesName;
            string CategoryNids = categoryNid.ToString();
            DataTable SubCategoriesTable = null;
            try
            {
                // Get sub categories nid
                SubCategoriesTable = this.DBConnection.ExecuteDataTable(this.DBQueries.Metadata_Category.GetMetadataCategories(FilterFieldType.Search, Metadata_Category.ParentCategoryNId + "=" + categoryNid.ToString()));
                if (SubCategoriesTable.Rows.Count > 0)
                {
                    CategoryNids += "," + DIConnection.GetDelimitedValuesFromDataTable(SubCategoriesTable, Metadata_Category.CategoryNId);
                }

                foreach (DataRow languageRow in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                {

                    LanguageCode = languageRow[Language.LanguageCode].ToString();
                    TablesName = new DITables(this.DBQueries.DataPrefix, LanguageCode);


                    // Delete records from metadata category table
                    this.DBConnection.ExecuteNonQuery(DI_LibDAL.Queries.MetadataCategory.Delete.DeleteMetadataCategory(TablesName.MetadataCategory, CategoryNids));

                    // Delete records from metadata report table
                    this.DBConnection.ExecuteNonQuery(DI_LibDAL.Queries.MetadataReport.Delete.DeleteMetadataRecordsByCategories(TablesName.MetadataReport, CategoryNids));
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }

        }

        /// <summary>
        /// Returns true if given category already exists.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="categoryType"></param>
        /// <param name="currentNid">New Mode: '-1' , Edit mode: actual nid</param>
        /// <returns></returns>
        public bool IsAlreadyExists(string name, string categoryType, int currentNid, int parentNid)
        {
            bool RetVal = false;
            int FoundNid = 0;
            try
            {
                FoundNid = this.GetNidByName(name, categoryType, parentNid.ToString());
                if (FoundNid > 0 && FoundNid != currentNid)
                {
                    RetVal = true;
                }
            }
            catch (Exception)
            {
                RetVal = false;
            }

            return RetVal;
        }

        /// <summary>
        /// Returns true if given category already exists.
        /// </summary>
        /// <param name="GId"></param>
        /// <param name="NId"></param>
        /// <param name="categoryType"></param>
        /// <returns></returns>
        public bool IsGIDAlreadyExists(string GId, int NId, string categoryType)
        {
            bool RetVal = false;
            int FoundNid = 0;
            try
            {
                foreach (DataRow Row in this.DBConnection.ExecuteDataTable(this.DBQueries.Metadata_Category.GetMetadataCategories(FilterFieldType.GId, GId)).Rows)
                {
                    FoundNid = Convert.ToInt32(Row[Metadata_Category.CategoryNId]);
                    if (FoundNid != NId)
                    {
                        RetVal = true;
                    }
                }
            }
            catch (Exception)
            {
                RetVal = false;
            }

            return RetVal;
        }

        /// <summary>
        /// Imports all metadata categories from given source database to current database
        /// </summary>
        /// <param name="srcDBConnection"></param>
        /// <param name="srcDBQueries"></param>
        /// <param name="categoryType"></param>
        public void ImportAllMetadataCategories(DIConnection srcDBConnection, DIQueries srcDBQueries, MetadataElementType categoryType)
        {
            // import by GID. If GID exists then update existing category otherwise insert it into current database            // update process will update in current langauge but insert process will insert in all langauges

            DataTable SrcCategoryTable = null;
            DI7MetadataCategoryInfo SrcCategoryInfo = null;
            DataTable TrgCategoryTable = null;
            int TrgCategoryNid = 0;
            int TrgCategoryParentNid = -1;

            try
            {
                // get source category table
                SrcCategoryTable = srcDBConnection.ExecuteDataTable(srcDBQueries.Metadata_Category.GetMetadataCategories(FilterFieldType.Type, DIQueries.MetadataElementTypeText[categoryType]));

                // get target(current database) category table
                TrgCategoryTable = this.DBConnection.ExecuteDataTable(this.DBQueries.Metadata_Category.GetMetadataCategories(FilterFieldType.Type, DIQueries.MetadataElementTypeText[categoryType]));

                // import categories & sub categories from source database into current database
                foreach (DataRow SrcRow in SrcCategoryTable.Select(Metadata_Category.ParentCategoryNId + "=-1"))
                {
                    // get category from source database
                    SrcCategoryInfo = DI7MetadataCategoryBuilder.GetMetadataCategoryInfo(SrcRow, srcDBConnection, srcDBQueries);
                    // check src category gid exists in current(target) database of not
                    TrgCategoryNid = 0;
                    TrgCategoryParentNid = -1;

                    foreach (DataRow TrgRow in TrgCategoryTable.Select(Metadata_Category.CategoryGId + "='" + DIQueries.RemoveQuotesForSqlQuery(SrcCategoryInfo.CategoryGID) + "' OR " + Metadata_Category.CategoryName + "='" + DIQueries.RemoveQuotesForSqlQuery(SrcCategoryInfo.CategoryName) + "'"))
                    {
                        // if exists then get nid and parent nid
                        TrgCategoryNid = Convert.ToInt32(TrgRow[Metadata_Category.CategoryNId]);
                        TrgCategoryParentNid = Convert.ToInt32(TrgRow[Metadata_Category.ParentCategoryNId]);
                    }
                    // update nid & parent nid in  src category info
                    SrcCategoryInfo.CategoryNId = TrgCategoryNid;
                    SrcCategoryInfo.ParentNid = TrgCategoryParentNid;

                    if (TrgCategoryNid > 0)
                    {
                        // update category info
                        this.UpdateMetadataCategory(SrcCategoryInfo);
                    }
                    else
                    {
                        // insert category into current database
                        TrgCategoryNid = this.InsertIntoDatabase(SrcCategoryInfo);
                    }

                    #region -- insert/update sub categories into current database --

                    // insert/update only if target category parent nid is equal to -1 (means at level1)
                    if (TrgCategoryParentNid == -1)
                    {
                        foreach (DI7MetadataCategoryInfo SrcSubCategory in SrcCategoryInfo.SubCategories.Values)
                        {
                            SrcSubCategory.CategoryNId = 0;
                            SrcSubCategory.ParentNid = TrgCategoryNid;

                            // check sub category exists ( where gid=<src gid> and parent nid=<trg nid>
                            foreach (DataRow TrgSubCategoryRow in TrgCategoryTable.Select(Metadata_Category.CategoryGId + "='" + DIQueries.RemoveQuotesForSqlQuery(SrcSubCategory.CategoryGID) + "' AND " + Metadata_Category.ParentCategoryNId + "=" + TrgCategoryNid))
                            {
                                // if exists then get nid
                                SrcSubCategory.CategoryNId = Convert.ToInt32(TrgSubCategoryRow[Metadata_Category.CategoryNId]);
                            }

                            if (SrcSubCategory.CategoryNId > 0)
                            {
                                // update sub category into current database
                                this.UpdateMetadataCategory(SrcSubCategory);
                            }
                            else
                            {
                                // insert sub category into current database
                                this.InsertIntoDatabase(SrcSubCategory);
                            }
                        }
                    }

                    #endregion

                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
        }


        /// <summary>
        /// Update Metadata Category Order
        /// </summary>
        /// <param name="categoryNId"></param>
        /// <param name="categoryOrder"></param>
        public void UpdateMetadataCategoryOrder(int categoryNId, int categoryOrder)
        {
            try
            {
                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.MetadataCategory.Update.UpdateMetadataCategoryOrder(this.DBQueries.TablesName.MetadataCategory, categoryOrder, categoryNId));

            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
        }


        #endregion

        #endregion

        #region "-- Static --"

        /// <summary>
        /// Rearrange given category table by category order
        /// </summary>
        /// <param name="categoryTable"></param>
        /// <returns></returns>
        public static DataTable RearrangeCategoryTableByCategoryOrder(DataTable categoryTable)
        {
            DataTable RetVal = null;

            try
            {
                RetVal = categoryTable.Clone();

                // sort category table  
                categoryTable.DefaultView.Sort = Metadata_Category.CategoryOrder;
                categoryTable = categoryTable.DefaultView.ToTable();

                foreach (DataRow Row in categoryTable.Select(Metadata_Category.ParentCategoryNId + "=-1"))
                {
                    RetVal.Rows.Add(Row.ItemArray);

                    foreach (DataRow ChildRow in categoryTable.Select(Metadata_Category.ParentCategoryNId + "=" + Convert.ToString(Row[Metadata_Category.CategoryNId])))
                    {
                        RetVal.Rows.Add(ChildRow.ItemArray);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
                RetVal = categoryTable;
            }

            RetVal.AcceptChanges();

            return RetVal;
        }

        /// <summary>
        /// return Default Xsl string from Resource
        /// </summary>
        /// <returns></returns>
        public static string GetMetadataXslt()
        {
            string RetVal = string.Empty;
            // Get Xsl string from resource File.
            RetVal = Resource1.XSLTFile2;

            return RetVal;
        }

        /// <summary>
        ///  Updating XSLT from Resourse File into database.
        /// </summary>
        /// <param name="dataprefix"></param>
        public  void UpdateXSLT(string dataprefix)
        {
            string xsltString = DI7MetadataCategoryBuilder.GetMetadataXslt();
            string SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.Xslt.Update.UpdateXSLT(dataprefix,DICommon.RemoveQuotes(xsltString));
            this.DBConnection.ExecuteNonQuery(SqlQuery);
        }
        #endregion
    }
}
