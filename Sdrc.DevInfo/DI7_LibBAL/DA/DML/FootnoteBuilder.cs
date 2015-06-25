using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;


namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    /// Build indicator according to indicator information and insert it into database.
    /// </summary>
    public   class FootnoteBuilder
    {
                #region "-- Private --"

        #region "-- Variables --"

        private DIConnection DBConnection;
        private DIQueries DBQueries;

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// To check existance of Footnote first into collection then into database
        /// </summary>
        /// <param name="footnoteInfo">object of FootnoteInfo</param>
        /// <returns>Footnote Nid</returns>
        public int CheckFootnoteExists(FootnoteInfo footnoteInfo)
        {
            int RetVal = 0;

            //Step 1: check source exists in source collection
            RetVal = this.CheckFootnoteInCollection(footnoteInfo.Name);

            //Step 2: check footnote exists in database.
            if (RetVal <= 0)
            {
                RetVal = this.GetFootnoteNid(footnoteInfo.GID, footnoteInfo.Name);
            }

            return RetVal;
        }

        /// <summary>
        ///To add footnoteInfo into collection 
        /// </summary>
        /// <param name="footnoteInfo">object of FootnoteInfo</param>
        private void AddFootnoteIntoCollection(FootnoteInfo footnoteInfo)
        {
            if (!this.FootnoteCollection.ContainsKey(footnoteInfo.Name))
            {
                this.FootnoteCollection.Add(footnoteInfo.Name, footnoteInfo);
            }

        }

        /// <summary>
        /// To  check Footnote exists in collection
        /// </summary>
        /// <param name="footnote"></param>
        /// <returns>Footnote Nid</returns>
        private int CheckFootnoteInCollection(string footnote)
        {
            int RetVal = 0;
            try
            {
                if (this.FootnoteCollection.ContainsKey(footnote))
                {
                    RetVal = this.FootnoteCollection[footnote].Nid;
                }
            }
            catch (Exception)
            {
                RetVal = 0;
            }
            return RetVal;
        }
              

        /// <summary>
        /// Returns footnote nid. 
        /// </summary>
        /// <param name="footnoteGid">Footnote GID </param>
        /// <param name="footnote">Footnote</param>
        /// <returns></returns>
        public int GetFootnoteNid(string footnoteGid, string footnote)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;

            //--step1:Get Nid by GID if GID is not empty
            if (!string.IsNullOrEmpty(footnoteGid))
            {
                RetVal = this.GetNidByGID(footnoteGid);
            }

            //--step2:Get Nid by Name if name is not empty
            if (RetVal <= 0)
            {
                if (!string.IsNullOrEmpty(footnote))
                {
                    RetVal = this.GetNidByName(footnote);
                }
            }

            return RetVal;
        }

        /// <summary>
        /// Retruns nid only if name exists in the database
        /// </summary>
        /// <returns></returns>
        private int GetNidByName(string name)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;
            try
            {
                SqlQuery = this.DBQueries.Footnote.GetFootnote(FilterFieldType.Name, "'" + DIQueries.RemoveQuotesForSqlQuery( name) + "'");
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
        private int GetNidByGID(string GID)
        {
            string SqlQuery = string.Empty;
            int RetVal = 0;
            try
            {
                SqlQuery = this.DBQueries.Footnote.GetFootnote(FilterFieldType.GId, "'" +  DIQueries.RemoveQuotesForSqlQuery(GID) + "'");
                RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(SqlQuery));
            }
            catch (Exception)
            {
                RetVal = 0;
            }
            return RetVal;
        }

        /// <summary>
        /// Insert Footnote record into database
        /// </summary>
        /// <param name="footnoteInfo">object of FootnoteInfo</param>
        /// <returns>Ture/False. Return true after successful insertion otherwise false</returns>
        private bool InsertIntoDatabase(FootnoteInfo footnoteInfo)
        {
            bool RetVal = false;
            string Footnote = footnoteInfo.Name;
            string FootnoteGId = Guid.NewGuid().ToString();
            string LanguageCode = string.Empty;
            string DefaultLanguageCode = string.Empty;
            string FootnoteForDatabase = string.Empty;

            try
            {
                DefaultLanguageCode = this.DBQueries.LanguageCode;

                //replace GID only if given gid is not empty or null.
                if (!string.IsNullOrEmpty(footnoteInfo.GID))
                {
                    FootnoteGId = footnoteInfo.GID;
                }

                foreach (DataRow languageRow in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                {

                    LanguageCode = languageRow[Language.LanguageCode].ToString();
                    if (LanguageCode == DefaultLanguageCode.Replace("_", String.Empty))
                    {
                        FootnoteForDatabase = Footnote;
                    }
                    else
                    {
                        FootnoteForDatabase = Constants.PrefixForNewValue + Footnote;

                    }
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Footnote.Insert.InsertFootnote(this.DBQueries.DataPrefix,"_" + LanguageCode,FootnoteForDatabase ,FootnoteGId));                   
                }

                RetVal = true;
            }
            catch (Exception)
            {
                RetVal = false;
            }

            return RetVal;

        }


        #endregion

        #endregion

        #region"--Internal--"
        #region "-- Variables & Propertipes --"

        /// <summary>
        /// Returns footnote colleciton in key,pair format. Key is footnote and value is Object of FootnoteInfo.
        /// </summary>
        internal Dictionary<string, FootnoteInfo> FootnoteCollection = new Dictionary<string, FootnoteInfo>();

        #endregion
        #endregion

        #region "-- Public --"
        #region "-- New/Dispose --"

        public FootnoteBuilder(DIConnection connection, DIQueries queries)
        {
            this.DBConnection = connection;
            this.DBQueries = queries;
        }

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Check existance of footnote record into database if false then create footnote record 
        /// </summary>
        /// <param name="footnote">Footnote</param>
        /// <returns>Footnote Nid</returns>
        public int CheckNCreateFoonote(string  footnote)
        {
            int RetVal = 0;
            FootnoteInfo FootnoteObj = new FootnoteInfo();
            FootnoteObj.Name = footnote;

            RetVal= this.CheckNCreateFoonote(FootnoteObj);    

            return RetVal;
        }

        /// <summary>
        /// Check existance of footnote record into database if false then create footnote record 
        /// </summary>
        /// <param name="footnoteInfo">object of FootnoteInfo</param>
        /// <returns>Footnote Nid</returns>
        public int CheckNCreateFoonote(FootnoteInfo footnoteInfo)
        {
            int RetVal = 0;
            
            try
            {
                // check footnote exists or not
                RetVal = this.CheckFootnoteExists(footnoteInfo);

                // if footnote does not exist then create it.
                if (RetVal <= 0)
                {
                     // insert footnote
                    if (this.InsertIntoDatabase(footnoteInfo))
                    {
                        RetVal = this.GetNidByName(footnoteInfo.Name);
                    }
                    
                }

                // add footnote information into collection
                footnoteInfo.Nid = RetVal;
                this.AddFootnoteIntoCollection(footnoteInfo);
            }
            catch (Exception)
            {
                RetVal = 0;
            }
                        
            return RetVal;
        }

        /// <summary>
        /// Deletes recrods from Footnote table
        /// </summary>
        /// <param name="footnoteNids"></param>
        public void DeleteFootnote(string footnoteNids)
        {
            DITables TableNames;
            string SqlQuery = string.Empty;

            try
            {

                foreach (DataRow Row in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                {
                    TableNames = new DITables(this.DBQueries.DataPrefix, "_" + Row[Language.LanguageCode]);
                    SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.Footnote.Delete.DeleteFootnote(TableNames.FootNote, footnoteNids);
                    this.DBConnection.ExecuteNonQuery(SqlQuery);

                   
                }

                //update data table
                if (!string.IsNullOrEmpty(footnoteNids))
                {
                    SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.Data.Update.RemoveFootnoteNId(this.DBQueries.DataPrefix, footnoteNids);
                    this.DBConnection.ExecuteNonQuery(SqlQuery);
                }
            }
            catch (Exception ex)
            {
                
                throw new ApplicationException(ex.ToString());
            }
			
        }


        /// <summary>
        ///Update FootNotes  
        /// </summary>
        /// <param name="notesClassInfo"></param>
        /// <returns >Count Of Records Updated</returns>
        public int UpdateFootNotes(FootnoteInfo footnoteInfo)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;
            try
            {
                SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.Footnote.Update.UpdateFootnote(this.DBQueries.TablesName.FootNote, footnoteInfo.Nid , footnoteInfo.Name );

                RetVal = this.DBConnection.ExecuteNonQuery(SqlQuery);

            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }


        /// <summary>
        /// Retruns name
        /// </summary>
        /// <returns></returns>
        public string GetNameByNid(int nid)
        {
            string RetVal = string.Empty;
            string SqlQuery = string.Empty;
            DataTable Table;
            try
            {
                SqlQuery = this.DBQueries.Footnote.GetFootnote(FilterFieldType.NId, "'" + nid + "'");
                Table = this.DBConnection.ExecuteDataTable(this.DBQueries.Footnote.GetFootnote(FilterFieldType.None, "'" + nid.ToString() +"'"));

                foreach (DataRow Row in Table.Rows)
                {
                    RetVal=Convert.ToString(Row[FootNotes.FootNote]);
                    break;
                }
            }
            catch (Exception)
            {
                RetVal = string.Empty;
            }
            return RetVal;
        }

        /// <summary>
        /// Imports footnotes from source database into current template or database
        /// </summary>
        /// <param name="footnoteInfo"></param>
        /// <param name="NidInSourceDB"></param>
        /// <param name="sourceQurey"></param>
        /// <param name="sourceDBConnection"></param>
        /// <returns></returns>
        public int ImportFootnote(FootnoteInfo footnoteInfo, int NidInSourceDB, DIQueries sourceQurey, DIConnection sourceDBConnection)
        {
            int RetVal = -1;            
            Dictionary<String, String> OldIconNId_NewIconNId = new Dictionary<string, string>();
            
            try
            {
                //check footnote already exists in database or not
                RetVal = this.GetFootnoteNid("", footnoteInfo.Name);

                if (RetVal > 0)
                {
                    // NA
                }
                else
                {
                    RetVal = this.CheckNCreateFoonote(footnoteInfo);
                }            
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }

            return RetVal;
        }        

        #endregion

        #endregion

    }
}