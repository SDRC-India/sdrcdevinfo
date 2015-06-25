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


namespace DevInfo.Lib.DI_LibBAL.Converter.Database
{
    /// <summary>
    /// Helps in converting Database into DevInfo 5.0 SP2.
    /// </summary>
    public class DI5SP2DBConverter : DBConverter
    {
        #region -- Private --

        #region -- Methods --

        private void CreateTables(bool forOnlineDB)
        {
            try
            {
                this.CreateAssistantTblsForAllLngs(forOnlineDB);
                this.CreateNotesTblsForAllLngs(forOnlineDB);
                this.CreateIconTbl(forOnlineDB);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        #region --  Assistant --

        private bool IsAssistantTblExists(bool forOnlineDB)
        {
            bool RetVal = false;

            try
            {
                if (forOnlineDB)
                {
                    //    sTablePrefix = oDBConnection.ExecuteScalar(oDA_Queries.Import_Prefix) + "_"; 
                }

                //-- Check the existence of all assistant table 
                // Step1: check  UT_Assistant_fr table
                if (this._DBConnection.ExecuteDataTable("SELECT count(*) FROM " + this._DBQueries.TablesName.Assistant + " WHERE 1=1") != null)

                    // Step2: check  UT_Assistant_Topic table
                    if (this._DBConnection.ExecuteDataTable("SELECT count(*) FROM " + this._DBQueries.TablesName.AssistantTopic + " WHERE 1=1") != null)


                        // Step3: check  UT_Assistant_eBook_fr table
                        if (this._DBConnection.ExecuteDataTable("SELECT count(*) FROM " + this._DBQueries.TablesName.AssistanteBook + " WHERE 1=1") != null)

                            RetVal = true;
            }

            catch (Exception)
            {
                RetVal = false;
                
            }

            return RetVal;
        }

        private void CreateAssistantTblsForAllLngs(bool forOnlineDB)
        {
            DITables TableNames;

            if (this.IsAssistantTblExists(forOnlineDB) == false)
            {
                try
                {
                    //-- Get all Languages from database 
                    foreach (DataRow Row in this._DBConnection.DILanguages(this._DBQueries.DataPrefix).Rows)
                    {
                        TableNames = new DITables(this._DBQueries.DataPrefix, Row[Language.LanguageCode].ToString());

                        //-- eBook table 
                        try
                        {
                            // drop table if already exists
                        this._DBConnection.DropTable(TableNames.AssistanteBook);
                    }
                    catch (Exception)
                    { }
                        this._DBConnection.ExecuteNonQuery(DALQueries.Assistant.Insert.CreateAssistantEBookTbl(TableNames.AssistanteBook, forOnlineDB));

                        //-- Assistant table 
                        try
                        {
                            // drop table if already exists
                        this._DBConnection.DropTable(TableNames.Assistant);
                        }
                        catch (Exception)
                        {}
                        this._DBConnection.ExecuteNonQuery(DALQueries.Assistant.Insert.CreateAssistantTbl(TableNames.Assistant, forOnlineDB));

                        //-- topic table 
                        try
                        {
                            // drop table if already exists
                            this._DBConnection.DropTable(TableNames.AssistantTopic);
                        }
                        catch (Exception)
                        {}
                        
                        this._DBConnection.ExecuteNonQuery(DALQueries.Assistant.Insert.CreateAssistantTopicTbl(TableNames.AssistantTopic, forOnlineDB));

                    }


                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ex.ToString());
                }
            }
        }

        #endregion

        #region --  Notes --

        private bool IsNotesTblExists(bool forOnlineDB)
        {
            bool RetVal = false;

            try
            {
                if (forOnlineDB)
                {
                    //    sTablePrefix = oDBConnection.ExecuteScalar(oDA_Queries.Import_Prefix) + "_"; 
                }

                //-- Check the existence of the table 

                if (this._DBConnection.ExecuteDataTable("SELECT count(*) FROM " + this._DBQueries.TablesName.NotesData + " WHERE 1=1") != null)
                    RetVal = true;

            }

            catch (Exception)
            {
                RetVal = false;
            }

            return RetVal;
        }

        private bool IsNotesApprovedColumnExists()
        {
            bool RetVal = false;

            try
            {
                if (this._DBConnection.ExecuteDataTable(this._DBQueries.Notes.GetNotes(string.Empty, "-1", string.Empty, string.Empty, CheckedStatus.True, FieldSelection.Light)) != null)
                    RetVal = true;

            }

            catch (Exception)
            {
                RetVal = false;
            }

            return RetVal;
        }

        private void CreateNotesTblsForAllLngs(bool forOnlineDB)
        {
            DITables TableNames;

            if (this.IsNotesTblExists(forOnlineDB) == false)
            {
                try
                {
                    //-- create common Notes table 
                    this._DBConnection.ExecuteNonQuery(DALQueries.Notes.Insert.CreateNotesData(forOnlineDB, this._DBQueries.DataPrefix, this._DBConnection.ConnectionStringParameters.ServerType));
                    this._DBConnection.ExecuteNonQuery(DALQueries.Notes.Insert.CreateProfile(forOnlineDB, this._DBQueries.DataPrefix, this._DBConnection.ConnectionStringParameters.ServerType));

                    //-- Get all Languages from database 
                    foreach (DataRow Row in this._DBConnection.DILanguages(this._DBQueries.DataPrefix).Rows)
                    {
                        TableNames = new DITables(this._DBQueries.DataPrefix, Row[Language.LanguageCode].ToString());
                        //-- Create LngBsd Notes tables : UT_Notes_en, UT_Notes_Classification_en 
                        this._DBConnection.ExecuteNonQuery(DALQueries.Notes.Insert.CreateNotes(forOnlineDB, TableNames.Notes, this._DBConnection.ConnectionStringParameters.ServerType));

                        this._DBConnection.ExecuteNonQuery(DALQueries.Notes.Insert.CreateClassification(forOnlineDB, TableNames.NotesClassification, this._DBConnection.ConnectionStringParameters.ServerType));

                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ex.ToString());
                }
            }
            else
            {
                //-- check notes_approved column exists in UT_Note_en table , if not then create it 
                if (!IsNotesApprovedColumnExists())
                {

                    //-- Get all Languages from database 
                    try
                    {
                        foreach (DataRow Row in this._DBConnection.DILanguages(this._DBQueries.DataPrefix).Rows)
                        {
                            try
                            {
                                TableNames = new DITables(this._DBQueries.DataPrefix, Row[Language.LanguageCode].ToString());


                                this._DBConnection.ExecuteNonQuery(DALQueries.Notes.Update.AlterTable(TableNames.Notes));
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }

                    catch (Exception ex)
                    {
                        throw new ApplicationException(ex.ToString());
                    }

                }
            }
        }

        #endregion

        #region --  ICONs --

        private bool IsIconTblExists()
        {
            bool RetVal = false;

            try
            {

                //-- Check the existence of the table 
                if (this._DBConnection.ExecuteDataTable("SELECT count(*) FROM " + this._DBQueries.TablesName.Icons + " WHERE 1=1") != null)
                    RetVal = true;

            }

            catch (Exception)
            {
                RetVal = false;
            }

            return RetVal;
        }

        private void CreateIconTbl(bool forOnlineDB)
        {
            if (this.IsIconTblExists() == false)
            {
                try
                {
                    this._DBConnection.ExecuteNonQuery(DALQueries.Icon.Insert.CreateIconsTbl(this._DBQueries.DataPrefix, forOnlineDB));

                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ex.ToString());
                }
            }
        }

        #endregion
        #endregion

        #endregion

        #region -- Public --

        #region -- Variables / Properties --

        #endregion

        #region -- New/Dispose --

        public DI5SP2DBConverter(DIConnection dbConnection, DIQueries dbQueries)
            : base(dbConnection, dbQueries)
        {
            //donothing
        }

        #endregion

        #region -- Methods --

        public override bool IsValidDB(bool forOnlineDB)
        {
            return this.IsAssistantTblExists(forOnlineDB);
        }

        public override bool DoConversion(bool forOnlineDB)
        {
            bool RetVal = false;

            //do the conversion only if database has different shcema
            try
            {
                this.CreateTables(forOnlineDB);
                RetVal = true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
                RetVal = false;
            }

            return RetVal;
        }

        #endregion

        #endregion
    }

}
