using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using DevInfo.Lib.DI_LibDAL;
using DALQueries = DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibBAL.Converter.Database;
using DevInfo.Lib.DI_LibBAL.Utility;
using System.Web.UI;
using DevInfo.Lib.DI_LibBAL.LogFiles;

namespace DevInfo.Lib.DI_LibBAL.Converter.Database
{
    /// <summary>
    /// Helps in converting DevInfo database into DevInfo 6.0
    /// </summary>
    public class DI6DBConverter : DI5SP2DBConverter
    {

        #region --  Private --

        DI6SubgroupTypeInfo LocationTypeInfo = null;
        DI6SubgroupTypeInfo OthersTypeInfo = null;


        #region --  Methods --

        #region -- version --



        private void CreateDBVersionTable(bool forOnlineDB)
        {
            try
            {

                this._DBConnection.ExecuteNonQuery(DALQueries.DBVersion.Insert.CreateTable(forOnlineDB, this._DBConnection.ConnectionStringParameters.ServerType));

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

        }

        private void InsertVersionInfo()
        {
            string VersionNumber = Constants.Versions.DI6_0_0_1;
            string VersionChangeDate = Constants.VersionsChangedDates.DI6_0_0_1;
            string VersionComments = Constants.VersionComments.DI6_0_0_1;
            DBVersionBuilder VersionBuilder;

            try
            {
                VersionBuilder = new DBVersionBuilder(this._DBConnection, this._DBQueries);
                VersionBuilder.InsertVersionInfo(VersionNumber, VersionChangeDate, VersionComments);

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        #endregion

        #region -- SubgroupVal --

        private void CreateSubgroupTypeTable(bool forOnlineDB)
        {
            DITables TableNames;


            try
            {
                //-- Get all Languages from database 
                foreach (DataRow Row in this._DBConnection.DILanguages(this._DBQueries.DataPrefix).Rows)
                {
                    TableNames = new DITables(this._DBQueries.DataPrefix, Row[Language.LanguageCode].ToString());
                    this._DBConnection.ExecuteNonQuery(DALQueries.SubgroupTypes.Insert.CreateTable(forOnlineDB, TableNames.SubgroupType, this._DBConnection.ConnectionStringParameters.ServerType));

                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

        }

        private void CreateSubgroupValSubgroupTable(bool forOnlineDB)
        {
            try
            {

                this._DBConnection.ExecuteNonQuery(DALQueries.SubgroupValSubgroup.Insert.CreateTable(forOnlineDB, this._DBQueries.DataPrefix, this._DBConnection.ConnectionStringParameters.ServerType));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

        }

        private void ConvertSubgroup()
        {
            // Step1: create 4 subgroup type :1-Sex, 2-Location, 3-Age, 4-Others,
            // Order TOTAL +( Location & Sex &  Age & other)
            //this.InsertSubgroupType(Constants.SubgroupType.Total, string.Empty, Constants.SubgroupType.Order.Total, false);


            this.InsertSubgroupType(Constants.SubgroupType.Sex, string.Empty, 2, false);
            this.InsertSubgroupType(Constants.SubgroupType.Location, string.Empty, 1, false);
            this.InsertSubgroupType(Constants.SubgroupType.Age, string.Empty, 3, false);
            this.InsertSubgroupType(Constants.SubgroupType.Other, string.Empty, 4, false);


            // Step2: Get all records from subgroup_val table
            foreach (DataRow SubgroupValRow in this._DBConnection.ExecuteDataTable(this._DBQueries.SubgroupVals.GetSubgroupVals()).Rows)
            {
                // Step3: Create relationship of subgroups and subgroup_val into subgroup_val_subgroup table
                this.CreateSbgroupValRelations(SubgroupValRow);
            }


            // get and set location type info
            this.GetNSetLocationTypeInfo();

            // get and set others type info
            this.GetNSetOthersTypeInfo();
        }



        private void CreateSbgroupValRelations(DataRow subgroupValRow)
        {
            try
            {
                int SubgroupValNid = Convert.ToInt32(subgroupValRow[SubgroupVals.SubgroupValNId]);
                int LocationNid = Convert.ToInt32(subgroupValRow[SubgroupValRemovedColumns.SubgroupValLocation]);
                int SexNid = Convert.ToInt32(subgroupValRow[SubgroupValRemovedColumns.SubgroupValSex]);
                int AgeNid = Convert.ToInt32(subgroupValRow[SubgroupValRemovedColumns.SubgroupValAge]);
                int OtherNid = Convert.ToInt32(subgroupValRow[SubgroupValRemovedColumns.SubgroupValOthers]);

                this.InsertSubgroupValRelations(SubgroupValNid, AgeNid);
                this.InsertSubgroupValRelations(SubgroupValNid, SexNid);
                this.InsertSubgroupValRelations(SubgroupValNid, LocationNid);
                this.InsertSubgroupValRelations(SubgroupValNid, OtherNid);
            }
            catch (Exception ex)
            {
            }

        }

        private int InsertSubgroupValRelations(int subgroupValNId, int subgroupNId)
        {
            int RetVal = 0;
            if (subgroupNId > 0 & subgroupValNId > 0)
            {
                try
                {
                    this._DBConnection.ExecuteNonQuery(DALQueries.SubgroupValSubgroup.Insert.InsertSubgroupValRelation(this._DBQueries.DataPrefix, subgroupValNId, subgroupNId));
                    RetVal = Convert.ToInt32(this._DBConnection.ExecuteDataTable(this._DBQueries.SubgroupValSubgroup.GetSubgroupValsSubgroup(subgroupValNId.ToString(), subgroupNId.ToString())).Rows[0][SubgroupValsSubgroup.SubgroupValSubgroupNId]);
                    //RetVal = this._DBConnection.ExecuteNonQuery("SELECT @@IDENTITY");

                }
                catch (Exception ex)
                {
                    RetVal = 0;
                    throw new ApplicationException(ex.ToString());
                }
            }
            return RetVal;
        }

        private int InsertSubgroupType(string subgroupTypeName, string subgroupTypeGID, int order, bool isGlobal)
        {
            int RetVal = 0;
            DITables TableNames;
            string SubgroupTypeNameForDatabase = string.Empty;
            string SubgroupTypeGID = Guid.NewGuid().ToString();

            try
            {
                // if the given gid is not null then use it.
                if (!string.IsNullOrEmpty(subgroupTypeGID))
                {
                    SubgroupTypeGID = subgroupTypeGID;
                }

                //-- Get all Languages from database and insert subgroup type
                foreach (DataRow Row in this._DBConnection.DILanguages(this._DBQueries.DataPrefix).Rows)
                {

                    TableNames = new DITables(this._DBQueries.DataPrefix, "_" + Row[Language.LanguageCode].ToString());
                    if ("_" + Row[Language.LanguageCode].ToString().ToUpper() == this._DBQueries.LanguageCode.ToUpper())
                    {
                        SubgroupTypeNameForDatabase = subgroupTypeName;
                    }
                    else
                    {
                        SubgroupTypeNameForDatabase = Constants.PrefixForNewValue + subgroupTypeName;
                    }


                    this._DBConnection.ExecuteNonQuery(DALQueries.SubgroupTypes.Insert.InsertSubgroupType(TableNames.SubgroupType, SubgroupTypeNameForDatabase, SubgroupTypeGID, order, isGlobal));

                    RetVal = this._DBConnection.ExecuteNonQuery("SELECT @@IDENTITY");
                }
            }
            catch (Exception ex)
            {
                RetVal = 0;
                throw new ApplicationException(ex.ToString());
            }
            return RetVal;
        }

        private void DeleteSubgroupValColumns()
        {
            DITables TableNames;

            try
            {
                //-- Get all Languages from database and delete Age,Sex,Location and others columns 
                foreach (DataRow Row in this._DBConnection.DILanguages(this._DBQueries.DataPrefix).Rows)
                {
                    TableNames = new DITables(this._DBQueries.DataPrefix, Row[Language.LanguageCode].ToString());
                    this._DBConnection.ExecuteNonQuery(DALQueries.Subgroup.Delete.DeleteColumnForDI6(TableNames.SubgroupVals, SubgroupType.Age));
                    this._DBConnection.ExecuteNonQuery(DALQueries.Subgroup.Delete.DeleteColumnForDI6(TableNames.SubgroupVals, SubgroupType.Location));
                    this._DBConnection.ExecuteNonQuery(DALQueries.Subgroup.Delete.DeleteColumnForDI6(TableNames.SubgroupVals, SubgroupType.Others));
                    this._DBConnection.ExecuteNonQuery(DALQueries.Subgroup.Delete.DeleteColumnForDI6(TableNames.SubgroupVals, SubgroupType.Sex));

                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

        }

        private void UpdateSubgroupTypeColumnDataType(bool forOnlineDB)
        {
            DITables TablesName;

            try
            {
                //-- Get all Languages from database 
                foreach (DataRow Row in this._DBConnection.DILanguages(this._DBQueries.DataPrefix).Rows)
                {
                    TablesName = new DITables(this._DBQueries.DataPrefix, Row[Language.LanguageCode].ToString());

                    this._DBConnection.ExecuteNonQuery(DALQueries.Subgroup.Update.UpdateSubgroupTypeDataType(TablesName.Subgroup, forOnlineDB, this._DBConnection.ConnectionStringParameters.ServerType));

                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

        }

        #region "-- Subgroup Val Text Updation --"

        private void UpdateSubgroupValText()
        {
            string SqlQuery = string.Empty;
            DI6SubgroupValBuilder SGValBuilder;
            DIQueries TempQueries;

            try
            {
                // get all subgroup val
                SqlQuery = this._DBQueries.SubgroupVals.GetSubgroupVals();
                SGValBuilder = new DI6SubgroupValBuilder(this._DBConnection, this._DBQueries);

                // run this process (update subgroup val text) only for default langauge 
                foreach (DataRow Row in this._DBConnection.ExecuteDataTable(SqlQuery).Rows)
                {
                    // run this process (update subgroup val text) only for default langauge 


                    //    // for available language update subgroup val text
                    //    foreach (DataRow LangaugeRow in this._DBConnection.DILanguages(this._DBQueries.DataPrefix).Rows)
                    //    {
                    // create subgroup val builder instance
                    //TempQueries = new DIQueries(this._DBQueries.DataPrefix, "_" + LangaugeRow[Language.LanguageCode].ToString());

                    // SGValBuilder = new DI6SubgroupValBuilder(this._DBConnection, TempQueries);

                    //        this.CheckNUpdateSubgroupValText(Convert.ToInt32(Row[SubgroupVals.SubgroupValNId]), SGValBuilder, TempQueries);
                    //    }


                    this.CheckNUpdateSubgroupValText(Convert.ToInt32(Row[SubgroupVals.SubgroupValNId]), SGValBuilder, this._DBQueries);
                }


            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        public string GetTotalTextString(string lngCode)
        {
            string RetVal = "Total";

            lngCode = lngCode.Replace("_", "");
            switch (lngCode.ToLower())
            {
                case "en":
                    RetVal = Resource1.TotalInEnglish;
                    break;

                case "fr":
                    RetVal = Resource1.TotalInFrench;
                    break;

                case "es":
                    RetVal = Resource1.TotalInSpanish;
                    break;

                case "zh":
                    RetVal = Resource1.TotalInChinese;
                    break;

                case "ar":
                    RetVal = Resource1.TotalInArabic;
                    break;

                default:
                    break;
            }

            return RetVal;
        }

        /// <summary>
        /// Checks and updates the SubgroupVal text for the given SubgroupValNIds
        /// </summary>
        /// <param name="SubgroupValNIds"></param>
        public void CheckNUpdateSubgroupValText(int SubgroupValNId, DI6SubgroupValBuilder SGValBuilder, DIQueries TempQueries)
        {
            string SqlQuery = string.Empty;
            string SubgroupNids = string.Empty;
            string DI5SubgroupValText = string.Empty;
            string NewSubgroupValText = string.Empty;
            string NewSubgroupValTextWTotal = string.Empty;
            string NewSubgroupValTextWTotalNPrefix = string.Empty;

            string TotalTextValue = string.Empty;

            int TotalNId = 0;
            bool IsTotalSGFound = false;
            bool IsSubgroupValInvalid = true;

            DI6SubgroupValInfo SGValInfo;

            DI6SubgroupBuilder SGBuilder;
            DI6SubgroupInfo SGInfo = new DI6SubgroupInfo();




            bool IsLocationSubgroupExistsInSubgroup = false;
            string LocationSubgroupName = string.Empty;
            int LocationSubgroupNId = 0;
            int NewSubgroupNId = 0;

            try
            {

                // get total text value for the curent langauge                
                TotalTextValue = this.GetTotalTextString(TempQueries.LanguageCode);


                SGBuilder = new DI6SubgroupBuilder(this.DBConnection, TempQueries);

                // Step1: get SubgroupVal info
                SGValInfo = SGValBuilder.GetSubgroupValInfo(FilterFieldType.NId, SubgroupValNId.ToString());
                //NewSubgroupValText

                if (SGValInfo != null)
                {
                    DI5SubgroupValText = DICommon.RemoveQuotes(Convert.ToString(SGValInfo.Name));


                    // Get subgroup Nids and check location subgroup exists in the template/databse.
                    foreach (DI6SubgroupInfo DimensionValue in SGValInfo.Dimensions)
                    {
                        if (!string.IsNullOrEmpty(SubgroupNids))
                        {
                            SubgroupNids += ",";
                        }
                        SubgroupNids += DimensionValue.Nid;

                        if (DimensionValue.Nid == 0 || DimensionValue.DISubgroupType == null)
                        {
                            // subgroup missing in DI5 but relationship of that subgroup exists in Subgroup_VAls table
                        }
                        else
                        {
                            if (DimensionValue.DISubgroupType.Order == 1) // 1 order is for location in DI5 DB
                            {
                                IsLocationSubgroupExistsInSubgroup = true;
                                LocationSubgroupName = DimensionValue.Name;
                                LocationSubgroupNId = DimensionValue.Nid;
                            }
                        }
                    }
                }
                // Step3: Check and update SubgroupVal text 
                if (!string.IsNullOrEmpty(SubgroupNids))
                {
                    // Step 3.1: Get New subgroup val text
                    NewSubgroupValText = DICommon.RemoveQuotes(SGValBuilder.CreateSubgroupValTextBySubgroupNids(SubgroupNids));
                }
                // step 3.2: compare New subgroup Val text with DI5SubgroupVal text 
                // step 3.2.1: compare Normal text value
                if (NewSubgroupValText.ToLower() != DI5SubgroupValText.ToLower())
                {
                    // step 3.2.2: if text doesnot match then compare the subgroup val with Total
                    NewSubgroupValTextWTotal = TotalTextValue + " " + NewSubgroupValText;

                    if (NewSubgroupValTextWTotal.ToLower() == DI5SubgroupValText.ToLower())
                    {
                        IsTotalSGFound = true;
                    }
                    else
                    {
                        // step 3.2.3: if text still doesnot match then compare the subgroup val with Total and prefix (#)
                        NewSubgroupValTextWTotalNPrefix = Constants.PrefixForNewValue + TotalTextValue + " " + NewSubgroupValText;

                        if (NewSubgroupValTextWTotalNPrefix.ToLower() == DI5SubgroupValText.ToLower())
                        {
                            TotalTextValue = Constants.PrefixForNewValue + TotalTextValue;
                            IsTotalSGFound = true;
                        }
                    }




                    // if New subgroup text matched with DI5 subgroup text
                    if (IsTotalSGFound)
                    {
                        // Check New DI6 subgrop text has Location subgroup
                        //      If location subgroup dimension exists,
                        if (IsLocationSubgroupExistsInSubgroup)
                        {
                            //          Delete the relationship of subgroup with the location subgroup 
                            SGValBuilder.DeleteSubgroupValRelations(SubgroupValNId, LocationSubgroupNId.ToString());

                            //          Create new subgroup under location where subgroup value is equal to "Total" + available value of location subgroup dimension
                            SGInfo = new DI6SubgroupInfo();
                            SGInfo.DISubgroupType = this.LocationTypeInfo;
                            SGInfo.Type = this.LocationTypeInfo.Nid;
                            SGInfo.Name = TotalTextValue + " " + LocationSubgroupName;

                            NewSubgroupNId = SGBuilder.CheckNCreateSubgroup(SGInfo);

                            //          Create relationship with new subgoup 
                            SGValBuilder.InsertSubgroupValRelations(SubgroupValNId, NewSubgroupNId);

                        }
                        else
                        {
                            //      And If not
                            //          Check TOTAL is available under location or not
                            //              If not then create it under location
                            TotalNId = this.CheckNCreateTotalSubgroupUnderLocation(TotalTextValue);
                            //          Create relationship with Total
                            SGValBuilder.InsertSubgroupValRelations(SubgroupValNId, TotalNId);
                        }

                        IsSubgroupValInvalid = false;
                    }

                }
                else
                {
                    IsSubgroupValInvalid = false;
                }

                // Step 4.1: Get New subgroup val text
                NewSubgroupValText = DICommon.RemoveQuotes(SGValBuilder.CreateSubgroupValTextBySubgroupNids(string.Join(",", SGValBuilder.GetAssocaitedSubgroupsNId(SubgroupValNId).ToArray())));

                // Step 4.2: Update Subgroup Val text
                // SGValBuilder.UpdateSubgroupVals(SubgroupValNId, NewSubgroupValText, SGValInfo.Global, SGValInfo.GID);

                if (IsSubgroupValInvalid)
                {

                    // 4.2.1 create new subgorup under others dimension
                    SGInfo = new DI6SubgroupInfo();

                    // if subgorup val is equal to "total" only and no relationship found, then create total subgroup under location otherwise create it under others

                    if (string.IsNullOrEmpty(NewSubgroupValText) && (DI5SubgroupValText.Trim().ToLower() == TotalTextValue.Trim().ToLower() || DI5SubgroupValText.Trim().ToLower() == Constants.PrefixForNewValue.Trim().ToLower() + TotalTextValue.Trim().ToLower()))
                    {
                        SGInfo.DISubgroupType = this.LocationTypeInfo;
                        SGInfo.Type = this.LocationTypeInfo.Nid;
                    }
                    else
                    {
                        SGInfo.DISubgroupType = this.OthersTypeInfo;
                        SGInfo.Type = this.OthersTypeInfo.Nid;
                    }

                    SGInfo.Name = DI5SubgroupValText;

                    // check it is already exist or not. If not then only show it under log file
                    NewSubgroupNId = SGBuilder.GetSubgroupNid(string.Empty, DI5SubgroupValText);

                    if (NewSubgroupNId <= 0)
                    {
                        NewSubgroupNId = SGBuilder.CheckNCreateSubgroup(SGInfo);

                        if (SGInfo.Type != this.LocationTypeInfo.Nid)
                        {
                            // add subgorup into mismatch list
                            this.MismatchSubgroups.Add(DI5SubgroupValText);
                        }
                    }


                    // 4.2.2 delete relationship of subgroup val nid from subgroup_val_subgroup table
                    SGValBuilder.DeleteSubgroupValRelations(SubgroupValNId);

                    // 4.2.3 create subgroupval relationship with new subgroup
                    SGValBuilder.InsertSubgroupValRelations(SubgroupValNId, NewSubgroupNId);


                }
                else
                {
                    if (IsTotalSGFound)
                    {
                        if (IsLocationSubgroupExistsInSubgroup)
                        {
                            this.SubgroupsAddedWNewSubgorup.Add(NewSubgroupValText, SGInfo.Name);
                        }
                        else
                        {
                            this.SubgroupsAddedWithTotal.Add(NewSubgroupValText);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

        }



        private void GetNSetLocationTypeInfo()
        {
            int LocationSGTypeNid = 0;
            DI6SubgroupTypeBuilder SGTypeBuilder = new DI6SubgroupTypeBuilder(this.DBConnection, this.DBQueries);

            LocationSGTypeNid = SGTypeBuilder.GetSubgroupTypeNid(string.Empty, Constants.SubgroupType.Location);
            if (LocationSGTypeNid <= 0)
            {
                LocationSGTypeNid = SGTypeBuilder.GetSubgroupTypeNid(string.Empty, Constants.PrefixForNewValue + Constants.SubgroupType.Location);
            }

            this.LocationTypeInfo = SGTypeBuilder.GetSubgroupTypeInfoByNid(LocationSGTypeNid);
        }

        private void GetNSetOthersTypeInfo()
        {
            int OthersSGTypeNid = 0;
            DI6SubgroupTypeBuilder SGTypeBuilder = new DI6SubgroupTypeBuilder(this.DBConnection, this.DBQueries);

            OthersSGTypeNid = SGTypeBuilder.GetSubgroupTypeNid(string.Empty, Constants.SubgroupType.Other);

            if (OthersSGTypeNid <= 0)
            {
                OthersSGTypeNid = SGTypeBuilder.GetSubgroupTypeNid(string.Empty, Constants.PrefixForNewValue + Constants.SubgroupType.Other);
            }

            this.OthersTypeInfo = SGTypeBuilder.GetSubgroupTypeInfoByNid(OthersSGTypeNid);
        }


        private int CheckNCreateTotalSubgroupUnderLocation(string TotalSGText)
        {
            int RetVal = 0;

            DI6SubgroupInfo SGInfo = new DI6SubgroupInfo();

            DI6SubgroupBuilder SGBuilder = new DI6SubgroupBuilder(this.DBConnection, this.DBQueries);
            DI6SubgroupTypeBuilder SGTypeBuilder = new DI6SubgroupTypeBuilder(this.DBConnection, this.DBQueries);

            SGInfo.Name = TotalSGText;
            SGInfo.Type = this.LocationTypeInfo.Nid;
            SGInfo.DISubgroupType = this.LocationTypeInfo;

            RetVal = SGBuilder.CheckNCreateSubgroup(SGInfo);

            return RetVal;
        }

        ////private void UpdateSubgroupForTotal()
        ////{

        ////    // Check New DI6 subgrop text has Location subgroup
        ////    //      If location subgroup dimension exists,
        ////    //          Create new subgroup under location where subgroup value is equal to "Total" + available value of location subgroup dimension
        ////    //          Delete the relationship of subgroup with the location subgroup 
        ////    //          Create relationship with new subgoup 
        ////    //      And If not
        ////    //          Check TOTAL is available under location or not
        ////    //              If not then create it under location
        ////    //          Create relationship with Total


        ////}

        #endregion


        #endregion

        #region -- Indicator_Classifications_IUS --

        private void UpdateIndicatorClassificationsIUS(bool forOnlineDB)
        {
            //insert columns into indicatorClassificationIUS 
            this.InsertColumnsInIndicatorClassificationsIUS(forOnlineDB);

            //update IC_IUS_Order 
            this.UpdateICIUSOrder();
        }

        private void InsertColumnsInIndicatorClassificationsIUS(bool forOnlineDB)
        {
            try
            {
                //order column
                this._DBConnection.ExecuteNonQuery(DALQueries.IndicatorClassification.Insert.InsertIC_IUS_OrderColumn(this._DBQueries.DataPrefix, forOnlineDB, this._DBConnection.ConnectionStringParameters.ServerType));

                //label column
                this._DBConnection.ExecuteNonQuery(DALQueries.IndicatorClassification.Insert.InsertIC_IUS_LabelColumn(this._DBQueries.DataPrefix, forOnlineDB, this._DBConnection.ConnectionStringParameters.ServerType));

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

        }

        private void UpdateICIUSOrder()
        {
            try
            {
                this._DBConnection.ExecuteNonQuery(DALQueries.IndicatorClassification.Update.UdpateICIUSOrderValues(this._DBQueries.DataPrefix));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        #endregion

        #region -- UT_Data Table --

        private void UpdateDataTable(bool forOnlineDB)
        {
            //insert columns into UT_Data
            this.InsertColumnsInToDataTable(forOnlineDB);


            this.UpdateIndicatorUnitSubgroupNids();
        }

        private void UpdateIndicatorUnitSubgroupNids()
        {
            try
            {
                this._DBConnection.ExecuteNonQuery(DALQueries.Data.Update.UpdateIndicatorUnitSubgroupVAlNids(this._DBQueries.TablesName));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        private void InsertColumnsInToDataTable(bool forOnlineDB)
        {
            try
            {
                this._DBConnection.ExecuteNonQuery(DALQueries.Data.Insert.InsertIndicatorNidColumn(this._DBQueries.DataPrefix, forOnlineDB, this._DBConnection.ConnectionStringParameters.ServerType));

                this._DBConnection.ExecuteNonQuery(DALQueries.Data.Insert.InsertUnitNidColumn(this._DBQueries.DataPrefix, forOnlineDB, this._DBConnection.ConnectionStringParameters.ServerType));

                this._DBConnection.ExecuteNonQuery(DALQueries.Data.Insert.InsertSubgroupVAlNidColumn(this._DBQueries.DataPrefix, forOnlineDB, this._DBConnection.ConnectionStringParameters.ServerType));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

        }


        #endregion



        #endregion

        #endregion

        #region -- internal --

        #region -- Variables / Properties --

        #endregion

        #region -- New/Dispose --

        public DI6DBConverter(DIConnection dbConnection, DIQueries dbQueries)
            : base(dbConnection, dbQueries)
        {
            //donothing
        }

        #endregion


        #region -- Methods --
        /// <summary>
        /// Returns true/false. True if Database is in valid format otherwise false.
        /// </summary>
        /// <param name="forOnlineDB"></param>
        /// <returns></returns>
        public override bool IsValidDB(bool forOnlineDB)
        {
            bool RetVal = false;

            DBVersionBuilder VersionBuilder = new DBVersionBuilder(this._DBConnection, this._DBQueries);
            RetVal = VersionBuilder.IsVersionTableExists();
                      

            return RetVal;
        }

        /// <summary>
        /// Converts DevInfo Database into DevInfo6.0 database
        /// </summary>
        /// <param name="forOnlineDB"></param>
        /// <returns></returns>
        public override bool DoConversion(bool forOnlineDB)
        {
            bool RetVal = false;
            int TotalSteps = 9;

            //do the conversion only if database has different shcema
            try
            {
                if (!this.IsValidDB(forOnlineDB))
                {

                    RetVal = base.DoConversion(forOnlineDB);

                    if (RetVal)
                    {
                        this.RaiseProcessStartedEvent(TotalSteps);

                        // Step1: create DB_version table
                        this.CreateDBVersionTable(forOnlineDB);
                        this.RaiseProcessInfoEvent(1);

                        // Step2: insert Version Information
                        this.InsertVersionInfo();
                        this.RaiseProcessInfoEvent(2);

                        // Step3: create UT_Subgroup_Vals_Subgroup table
                        this.CreateSubgroupValSubgroupTable(forOnlineDB);
                        this.RaiseProcessInfoEvent(3);

                        // Step4: create UT_Subgroup_Type_en table
                        this.CreateSubgroupTypeTable(forOnlineDB);
                        this.RaiseProcessInfoEvent(4);

                        // Step5: Convert the subgroups into new format
                        this.UpdateSubgroupTypeColumnDataType(forOnlineDB);
                        this.ConvertSubgroup();
                        this.RaiseProcessInfoEvent(5);

                        // Step6: Delete Age,Sex,Others and Location from Subgroup_val table
                        this.DeleteSubgroupValColumns();
                        this.RaiseProcessInfoEvent(6);

                        // Step7: update subgroup_val text in subgroup_Val table
                        this.UpdateSubgroupValText();
                        this.RaiseProcessInfoEvent(7);

                        // Step8: Update UT_Indicator_Classifications_IUS table
                        this.UpdateIndicatorClassificationsIUS(forOnlineDB);
                        this.RaiseProcessInfoEvent(8);

                        // Step9: Update UT_Data table
                        this.UpdateDataTable(forOnlineDB);
                        this.RaiseProcessInfoEvent(9);


                        // Step10: Update filed size of Element_Type column in UT_Icon table
                        try
                        {
                            this._DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Icon.Update.UpdateElementTypeColumnFiledSize(this._DBQueries.TablesName.Icons));
                        }
                        catch (Exception ex)
                        {
                            throw new ApplicationException(ex.ToString());
                        }

                        RetVal = true;

                    }
                }
                else
                {
                    RetVal = true;
                }
            }
            catch (Exception ex)
            {
                RetVal = false;
                throw new ApplicationException(ex.ToString());
            }

            //this.GenerateLogFile();
            return RetVal;
        }


        #endregion

        #endregion
    }

}
