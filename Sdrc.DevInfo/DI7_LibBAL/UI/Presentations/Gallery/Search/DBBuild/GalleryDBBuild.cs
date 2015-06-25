using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibBAL.Utility;


namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Gallery.Search.DBBuild
{
    #region " -- Delegate -- "
    /// <summary>
    /// Delegate for New GalleryId Added Event
    /// </summary>
   // public delegate void NewGalleryAddedDelegate();
    #endregion

    /// <summary>
    /// This Class is for building Database from Presentation
    /// </summary>
    public class GalleryDBBuild:IDisposable
    {
        #region " -- Private --  "
        
        #region " -- Variable --  "

        private DIConnection DBConnection=null;
        private int Pres_Nid = 0;        // Hold Presention Nid for Presentation Used 
        private String FileNameSeperator = "{[]}";

        #endregion

        #region " -- Constants -- "

        private const String CELL_A1 = "A1";
        private const String CELL_A2 = "A2";
        private const String Pres_Type_M = "M";
        private const String Pres_Type_G = "G";
        private const String Pres_Type_T = "T";
        private const String DB_PWD = "unitednations2000";

        #endregion

        #region "-- enum -- "
        public enum PresentationType
	    {
            /// <summary>
            /// Presentation Type: MAP
            /// </summary>
	        M=0,
            /// <summary>
            ///  Presentation Type: GRAPH
            /// </summary>
            G=1,
            /// <summary>
            ///  Presentation Type: TABLE
            /// </summary>
             T=2

	    }

        #endregion

        #region " -- Methods --  "
        /// <summary>
        /// Validate is file Exist 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private bool ValidateFilePath(String filePath)
        {
            bool RetVal=false;
            if (File.Exists(filePath))
            {
                RetVal=true;
            }
            return RetVal;
        }

        /// <summary>
        /// Validate Database 
        /// </summary>
        /// <param name="databasePath"></param>
        /// <returns></returns>
        private bool ValidateDB(string databasePath)
        {
            bool RetVal = false;
            try
            {
                // Chk is Db exist
                RetVal = ValidateFilePath(databasePath);

                //If db exist. Check for required Tables
                if (RetVal == true)
                {
                    // Creating Database Connection using DIConnection               
                    this.DBConnection = new DIConnection(DIServerType.MsAccess, "", "", databasePath, "", DB_PWD);
                }
            }
            catch (Exception ex)
            {
                RetVal = false;
            }
            return RetVal;
        }

        /// <summary>
        /// Validate Database 
        /// </summary>
        /// <param name="databasePath"></param>
        /// <returns></returns>
        private bool ValidateDB()
        {
            bool RetVal = false;
            RetVal= this.ValidateDB(this._GalleryDatabase);
            return RetVal;
        }

        /// <summary>
        /// Read excel workbook and update Database with Keywords
        /// This will Get UserKeywords from Excel and Call overloaded GetKeywordAndUpdateDatabase function with
        /// two parameter presFile path and UKeywords
        /// </summary>
        /// <param name="presentationFile">Excel work book to read</param>        
        private Int32 GetKeywordAndUpdateDatabase(string presentationFile)
        {
            Int32 RetVal = 0;
            
            System.Data.DataTable UKeywordsDataTable;
            String Pres_FileName = string.Empty;
            String UKeyword = String.Empty;
            String Keywords = String.Empty;
            String FileNameForTitle = String.Empty;  // In Case of Graph and Table title will be title + subtitles and filename

            try
            {
                // Using Excel Control Library
                DIExcel DIExl = new DIExcel(presentationFile);
               
                // --- Get UserKeywords ---
                // Get Keywords Sheet into DataTable               
                UKeywordsDataTable = DIExl.GetDataTableFromSheet(ExcelSheetName.KEYWORDS);

                // Get UserKeyword
                UKeyword = this.GetUserKeyWords(UKeywordsDataTable);

                //close excel
                DIExl.Close();
                // Call Other overload
                RetVal = this.GetKeywordAndUpdateDatabase(presentationFile, UKeyword);

            }
            catch (Exception ex)
            {
              
            }
            return RetVal;
        }
        
        /// <summary>
        /// Read excel workbook and update Database with Keywords
        /// </summary>
        /// <param name="presentationFile">Excel work book to read</param>        
        private Int32 GetKeywordAndUpdateDatabase(string presentationFile, string userKeywords)
        {
            Int32 RetVal = 0;
            System.Data.DataTable ExcelSheetDataTable;
            System.Data.DataTable UKeywordsDataTable;
            String Pres_FileName = string.Empty;
            PresentationType Pres_Type = PresentationType.M;
            String Title = String.Empty;
            String UKeyword = String.Empty;
            String Keywords = String.Empty;

            String FileNameForTitle = String.Empty;  // In Case of Graph and Table title will be title + subtitles and filename
            int Map_Suffix_Lenght = 10;      //" - Map.xls"            

            try
            {
                // Using Excel Control Library
                DIExcel DIExl = new DIExcel(presentationFile);

                // Getting Pres fileName For Database Field
                Pres_FileName = Path.GetFileName(presentationFile);

                //Start  of getting Data from Excel Sheet
                // Check For Presention Type                                    
                if (presentationFile.Contains(Presentation.Map_Suffix)) // Presention is Map : Get Title and Data 
                {
                    // Get Presentation Type for Database table when presentation file is Map
                    //Pres_Type = Pres_Type_M;
                    Pres_Type = PresentationType.M;

                    // In case of Map Title Column will be name of the file minus " - Map.xls"
                    Title = Pres_FileName.Substring(0, Pres_FileName.Length - Map_Suffix_Lenght);

                    // Using Excel Control Function to get value from Sheet
                    ExcelSheetDataTable = DIExl.GetDataTableFromSheet(ExcelSheetName.DATA);
                }
                else //  Get Title and Data for Graph and Map Presentioan 
                {
                    // Get Presentation Type for Database table when presentation file is table or Graph                    
                    // Setting Presention Type On the basis of suffix
                    if (presentationFile.Contains(Presentation.Graph_Suffix) || presentationFile.ToLower().Contains(Presentation.Graph_Suffix.ToLower()))
                    {
                        Pres_Type = PresentationType.G;
                    }
                    else if (presentationFile.Contains(Presentation.Table_Suffix) || presentationFile.ToLower().Contains(Presentation.Table_Suffix.ToLower()))
                    {
                        Pres_Type = PresentationType.T;
                    }

                    // Get Presentation Title
                    Title = this.GetPresentationTitle(Pres_Type, Pres_FileName, DIExl);

                    // DataTable In case of Graph  or Table
                    ExcelSheetDataTable = DIExl.GetDataTableFromSheet(ExcelSheetName.DATA);
                }
                //End of getting Data from Excel Sheet

                //Getting Keywords
                Keywords = this.GetKeyWordsFromPresentationDataTable(ExcelSheetDataTable);

               
                // Update Database
                Pres_Nid = UpdateDBTables(presentationFile, Pres_Type, Title, userKeywords, Keywords);
                RetVal = Pres_Nid;
            }
            catch (Exception ex)
            {
                //Successs = false;
            }
            return RetVal;
        }

        /// <summary>
        /// Extract keyword from DataTable and update Database with Keywords
        /// </summary>
        /// <param name="presentationFile">Excel work book to read</param>        
        private int GetKeywordAndUpdateDatabase(Presentation.PresentationType presType, String presTitle, String userKeyword, DataTable presentationDataTable, string PresFile)
        {
            int RetVal = -1;
            // If File is an xls File then get presTitle,UKeyword  from Excel File

            System.Data.DataTable ExcelSheetDataTable;
            System.Data.DataTable UKeywordsDataTable;
            String Pres_FileName = string.Empty;
            PresentationType Pres_Type = PresentationType.M;
            String Title = String.Empty;
            String UKeyword = String.Empty;
            String Keywords = String.Empty;

            String FileNameForTitle = String.Empty;  // In Case of Graph and Table title will be title + subtitles and filename
            int Map_Suffix_Lenght = 10;      //" - Map.xls"            

            try
            {
                //If Excel File.
                if (PresFile.EndsWith(DICommon.FileExtension.Excel))
                {
                    // Using Excel Control Library
                    DIExcel DIExl = new DIExcel(PresFile);

                    // Getting Pres fileName For Database Field
                    Pres_FileName = Path.GetFileName(PresFile);

                    //  Get Presentation Type pres type abbreviation. Which is entered in Database.M:Map ,T:Table,G:Graph
                    Pres_Type = GetPresTypeForGalleryDB(presType);

                    // Get title from excel Presentaion
                    // In case of Map Title Column will be name of the file minus " - Map.xls"
                    if (presType==Presentation.PresentationType.Map)
                    {
                        // In case of Map Title Column will be name of the file minus " - Map.xls"
                        Title = this.GetPresentationTitle(Pres_Type, Pres_FileName, DIExl);
                        //Title =  Pres_FileName.Substring(0, Pres_FileName.Length - Map_Suffix_Lenght);
                    }
                    // Presentaion is table or Graph
                    else
                    {
                        // Get Presentation Title
                        Title = this.GetPresentationTitle(Pres_Type, Pres_FileName, DIExl);
                    }

                    // --- Get UserKeywords from Excel Presentaion ---
                    // Get Keywords Sheet into DataTable               
                    UKeywordsDataTable = DIExl.GetDataTableFromSheet(ExcelSheetName.KEYWORDS);

                    // Get UserKeyword
                    UKeyword = this.GetUserKeyWords(UKeywordsDataTable);
                }
                //In case of Web application title and Ukeyword will be same as passed by client application 
                else
                {
                    Title = presTitle;
                    UKeyword = userKeyword;
                    Pres_Type = GetPresTypeForGalleryDB(presType);
                }
         
                //Getting Keywords from presentation Data table
               Keywords = this.GetKeyWordsFromPresentationDataTable(presentationDataTable);
                
            // Update Database
            RetVal = UpdateDBTables(PresFile, Pres_Type, Title, UKeyword, Keywords);
            Pres_Nid = RetVal;
            }
            catch (Exception ex)
            {
                //Successs = false;
            }
            return RetVal;
        }
        /// <summary>
        /// Return pres type abbreviation for entry in Gallery database
        /// </summary>
        /// <param name="presType"></param>
        /// <returns></returns>
        private PresentationType GetPresTypeForGalleryDB(Presentation.PresentationType presType)
        {
            PresentationType RetVal=PresentationType.M;

            switch (presType)
            {
                case Presentation.PresentationType.Table:
                    RetVal = PresentationType.T;
                    break;
                case Presentation.PresentationType.Graph:
                    RetVal = PresentationType.G;
                    break;
                case Presentation.PresentationType.Map:
                    RetVal = PresentationType.M;
                    break;
            }
            return RetVal;
        }
        /// <summary>
        /// Get Title from Presentaion
        /// </summary>
        /// <param name="presentaionType"></param>
        /// <param name="presFileName"></param>
        /// <returns></returns>
        private String GetPresentationTitle(PresentationType presentaionType,String presFileName, DIExcel dIExl)
        {
            string RetVal = string.Empty;
            int Map_Suffix_Lenght = 10;      //" - Map.xls"
            int Graph_Suffix_Lenght = 12;    //" - Graph.xls"
            try
            {
                switch (presentaionType)
                {
                    //Map .    // In case of Map RetVal Column will be name of the file minus " - Map.xls"
                    case PresentationType.M:
                        RetVal = presFileName.Substring(0, presFileName.Length - Map_Suffix_Lenght);
                        break;

                    case PresentationType.G:
                        //Get FileName for RetVal Column. In case of table or Graph  this will be name of the file minus " - Table.xls" or  - Graph.xls so lenght is 1;
                        RetVal = presFileName.Substring(0, presFileName.Length - Graph_Suffix_Lenght);
                        break;

                    case PresentationType.T:

                        //Get RetVal and Subtitle Column Value  when Presentation is not Map

                        //Get FileName for RetVal Column. In case of table or Graph  this will be name of the file minus " - Table.xls" or  - Graph.xls so lenght is 1;
                        RetVal = presFileName.Substring(0, presFileName.Length - Graph_Suffix_Lenght);

                        // Value in First sheet A1 and A2 Column respectively will added in RetVal
                        // Getting  Value of Main RetVal from first cell and adding this to RetVal  
                        if (dIExl.GetCellValue(0, CELL_A1).Length > 0)
                        {
                            //Check RetVal already contain this word or not, If Not then add this to RetVal
                            if (RetVal.Contains(dIExl.GetCellValue(0, CELL_A1)) == false)
                            {
                                //Check column value contain RetVal filename or not
                                if ((dIExl.GetCellValue(0, CELL_A1)).Contains(RetVal) == false) // Add column value to RetVal
                                {
                                    RetVal += " " + dIExl.GetCellValue(0, CELL_A1);
                                }
                                else // Replace Existing Value of RetVal with this column value
                                {
                                    RetVal = dIExl.GetCellValue(0, CELL_A1);
                                }
                            }
                        }
                        // Getting SubTitle (If Available)
                        if (dIExl.GetCellValue(0, CELL_A2).Length > 0)
                        {
                            // Adding SubTitle in RetVal string with a blank space                             
                            RetVal += " " + dIExl.GetCellValue(0, CELL_A2);

                        }	

                     
                    
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {                
            }
            return RetVal;
        }

        /// <summary>
        /// Get Keyword from Presentation using datatable created from Presentaion data 
        /// </summary>
        /// <param name="presDataTable"></param>
        /// <returns></returns>
        private string GetKeyWordsFromPresentationDataTable(DataTable presDataTable)
        {
            string RetVal = string.Empty;
            try
            {
                //1. Remove columns not required for Keyword Creation
            // Start of Deleting Columns (DataValue and AreaId),Those are not required for keyword creation
            if (presDataTable.Columns[ExcelColumnsName.DATA] != null)
            {
                presDataTable.Columns.Remove(ExcelColumnsName.DATA);
            }
            else if (presDataTable.Columns[ExcelColumnsName.DATA_Value] != null)
            {
                presDataTable.Columns.Remove(ExcelColumnsName.DATA_Value);
            }
            if (presDataTable.Columns[ExcelColumnsName.Area_ID] != null)
            {
                presDataTable.Columns.Remove(ExcelColumnsName.Area_ID);
            }

            // End of Deleting Columns (DataValue and AreaId),Those are not required for keyword creation           

            // 2.Start of Keyword Creation
            // For each Row
            // Notify to client that  Keyword Creation has started
            //// NotifyProgress(-1, presDataTable.Rows.Count);

            for (int i = 0; i < presDataTable.Rows.Count; i++)
            {
                //Notify progress to client
                //// NotifyProgress(i+1, 0);
                // For Each column
                for (int j = 0; j < presDataTable.Columns.Count; j++)
                {
                    // If new Keyword is not already in the keyword list then add it 
                    if ((!RetVal.Contains(presDataTable.Rows[i][j].ToString())) && (presDataTable.Rows[i][j].ToString() != ""))
                    {
                        if (RetVal.Length > 0)
                        {
                            RetVal += " " + presDataTable.Rows[i][j].ToString();
                        }
                        else
                        {
                            RetVal = presDataTable.Rows[i][j].ToString();
                        }
                    }
                }
            }
            }
            catch (Exception ex)
            {   
            }
            return RetVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userkeywordDataTable"></param>
        /// <returns></returns>
        private string GetUserKeyWords(DataTable userKeywordDataTable)
        {
            string RetVal = string.Empty;
            const string Column1= "Column1";
            const string Column = "Column"; ;
            try
            {
                // if Data is present  in first row of Excel sheet then that data will appears as Column name 
                // If No data was found in excel sheet. still datatable shows only one column with value "Column1"
                if (userKeywordDataTable.Columns[0].ToString() != Column1)
                {
                    RetVal =userKeywordDataTable.Columns[0].ToString();
                    for (int k = 1; k <userKeywordDataTable.Columns.Count; k++)
                    {
                        if (userKeywordDataTable.Columns[k].ToString() != "")
                        {
                            //If column start with column (ex: Column1,Column2 ) then do not add in Ukeywords
                            if ((userKeywordDataTable.Columns[k].ToString()).StartsWith(Column) == false)
                            {
                                RetVal += " " +userKeywordDataTable.Columns[k].ToString();
                            }
                        }
                    }
                }
                // Check For existance of Table columns 
                if (userKeywordDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i <userKeywordDataTable.Rows.Count; i++)
                    {
                        for (int j = 0; j <userKeywordDataTable.Columns.Count; j++)
                        {
                            if (userKeywordDataTable.Rows[i][j].ToString() != "")
                            {
                                if (RetVal.Length > 0)
                                {
                                    RetVal += " " +userKeywordDataTable.Rows[i][j].ToString();
                                }
                                else
                                {
                                    RetVal =userKeywordDataTable.Rows[i][j].ToString();
                                }
                            }
                        }
                    }
                }
                // --- End of Get UserKeywords ---

            }
            catch (Exception ex)
            {   
            }
            return RetVal;
        }

        /// <summary>       
        /// Inserting Presention Details into Database 
        /// Return Presentation NId
        /// </summary>
        /// <param name="pres_FileName"> Presentation File Name for UT_PresMst</param>
        /// <param name="fileType"> Presentation Type for UT_PresMst</param>
        /// <param name="Title">Presentation Title for UT_PresKeyword</param>
        /// <param name="keywords">Pres_keywords for UT_PresKeyword</param>
        public int UpdateDBTables(String presentationFile, PresentationType pres_Type, string Title, String ukeyword, string keywords)
        {
            String sSql = String.Empty;
            string Pres_FileName = string.Empty;
            int PresNid = 0;
            int GalleryFolderNid = 0;
            object oPresNid = null;
            try
            {
                // Update DB only when Presentaion file exist
                if (File.Exists(presentationFile))
                {   
                    // Save FileName without extention in Database                    
                    // file name may be Côte d’Ivoire - AIDS deaths Total  Number - Table.xls. So do not remove quotes using
                    // DICommon remove Quotes as it replace "’", "''"
                    // Use Local remove quote that  will only replace "'", "''"
                    Pres_FileName =RemoveQuotes(Path.GetFileNameWithoutExtension(presentationFile));
                    // Clear PreSearch Table
                    this.ClearPreSearchTable();

                    #region " -- File already in Database.Delete it -- "   
                    ////////////// Ckeck File already in DataBase               
                    try
                    {
                        ////Check is this gallary folder already exist in GalleryMst.                                            
                        ////Get NId of Gallery Folder.                       
                        GalleryFolderNid = this.GetGalleryIdForPresentation(Path.GetDirectoryName(presentationFile));

                        // If gallery folder do not exist add it
                        if (GalleryFolderNid == 0)
                        {
                            // Add new Gallery in Gallery Mst
                            GalleryFolderNid = this.AddNewGalleryToGallMst(Path.GetDirectoryName(presentationFile));
                        }

                        // Check for Presentaion 
                        sSql = this.GetPresNIdQuery(Pres_FileName, GalleryFolderNid, pres_Type.ToString());
                        oPresNid = DBConnection.ExecuteScalarSqlQuery(sSql);
                        
                        // If presentation  was found in gallery db  then delete it.
                        if (oPresNid != null)
                        {
                            this.DeletePresFromGallery(Convert.ToInt32(oPresNid));
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    #endregion
                                        
                    //// If gallery folder do not exist add it
                    //if (GalleryFolderNid==0)
                    //{
                    //    // Add new Gallery in Gallery Mst
                    //    GalleryFolderNid = this.AddNewGalleryToGallMst(Path.GetDirectoryName(presentationFile));                       
                    //}

                    // Insert into  Master Table
                    sSql = "Insert into " + DBTable.UT_PresMst + " ( " + PresentationMaster.Pres_FileName + ","
                        + PresentationMaster.Pres_Type + "," + PresentationMaster.GalleryId + ") values ('" + Pres_FileName + "','" + pres_Type + "','" + GalleryFolderNid.ToString() + "')";

                    //Executing Query           
                    DBConnection.ExecuteNonQuery(sSql);

                    try
                    {
                        // Get New PresNid 
                        sSql = "Select max(" + PresentationMaster.Pres_NId + ") from " + DBTable.UT_PresMst;
                        PresNid = (int)DBConnection.ExecuteScalarSqlQuery(sSql);

                        // Insert new Record in Pres Table
                        sSql = " Insert into " + DBTable.UT_PresKeyword +
                          " (" + PresentationMaster.Pres_NId + "," + PresentationKeywords.Pres_Titles + "," +
                            PresentationMaster.Pres_Type + "," + PresentationKeywords.Pres_UKeywords + "," + PresentationKeywords.Pres_Keywords + ") values ("
                           + PresNid + ",'" + DICommon.RemoveQuotes(Title) + "','" + pres_Type + "','" +DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(ukeyword)) + "','" +DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(keywords)) + "') ";
                        DBConnection.ExecuteNonQuery(sSql);
                    }
                    catch (Exception ex)
                    {
                        // Delete Entry From PresMaster for this NID and Set Success to False                     
                        //Successs = false;
                        sSql = "DELETE * from UT_PresMst where " + PresentationMaster.Pres_NId + " = " + PresNid;
                        DBConnection.ExecuteNonQuery(sSql);
                        PresNid = 0;
                    }
            }
            }

            catch (Exception EX)
            {
                //Successs = false;
                PresNid = 0;
            }
            return PresNid;
        }

        /// <summary>
        /// Clear Presearch Tables 
        /// </summary>
        private void ClearPreSearchTable()
        {
            try
            {
                //Clear Presearch Tables 
                this.DBConnection.ExecuteNonQuery("SP_ClearPredefinedSearch", System.Data.CommandType.StoredProcedure, null);
            }
            catch (Exception ex)
            {                
            }            
        }

        /// <summary>
        /// Delete Presentation from GalleryDB presMaster and keywords Table
        /// </summary>
        /// <param name="sSql"></param>
        /// <param name="PresNid"></param>
        /// <param name="oPresNid"></param>
        /// <remarks>Delete From PresMst and PresKeywords Table, Used for updating pres details in gallery db</remarks>
        private void DeletePresFromGallery(int PresNIdForDelete)
        {
            String sSql = String.Empty;
            try
            {
                //DELETE FROM Pres keyword
                // sSql = "DELETE * from  " + DBTable.UT_PresKeyword + " where Pres_Nid = " + PresNid;
                sSql = "DELETE FROM " + DBTable.UT_PresKeyword + " WHERE " + PresentationMaster.Pres_NId + "=" + PresNIdForDelete;
                this.DBConnection.ExecuteNonQuery(sSql);

                //DELETE FROM Pres Mst
                //sSql = "DELETE * from " + DBTable.UT_PresMst + " where Pres_Nid= " + PresNid;
                sSql = "DELETE FROM " + DBTable.UT_PresMst + " WHERE " + PresentationMaster.Pres_NId + "=" + PresNIdForDelete;
                this.DBConnection.ExecuteNonQuery(sSql);
               
            }
            catch (Exception ex)
            {                
            }   
        }

        /// <summary>
        /// Delete Presentaions from all db tables using Presentaion Id
        /// </summary>
        /// <param name="PresNIdForDelete"></param>
        private void DeletePresentationsFromDBTables(int PresNIdForDelete)
        {
            string sSql = string.Empty;

           //Delete From Master and Keyword table
            this.DeletePresFromGallery(PresNIdForDelete);

            //Delete from presearch
            // Remove all presearch where =
            //sSql = "DELETE FROM  UT_PreSearches WHERE  PDS_Presentation_NIds LIKE 'PresNIdForDelete'";
            sSql = "DELETE FROM  UT_PreSearches WHERE  PDS_Presentation_NIds = '" + PresNIdForDelete +
                "' OR  PDS_Presentation_NIds LIKE '%," + PresNIdForDelete + "' OR PDS_Presentation_NIds LIKE '%," + PresNIdForDelete
                + ",%'  or  PDS_Presentation_Nids LIKE '" + PresNIdForDelete + ",%' ";

            //Execute query
            this.DBConnection.ExecuteNonQuery(sSql);
        }

        /// <summary>
        /// If DiBook Path is  C:\-- Projects --\DevInfo 6.0\User Interface - Desktop\bin\DI Book\diBooks\diBook1
        /// We Save Presentaion Name as  diBook1 {[]} C:\-- Projects --\DevInfo 6.0\User Interface - Desktop\bin\DI Book\diBooks
        /// </summary>
        /// <param name="dIBookPath"></param>
        /// <returns></returns>    
        private String GetDiBookFileNameForDB(String dIBookPath)
        {
            // Get DiBook File name  
            DirectoryInfo DirInfo = new DirectoryInfo(dIBookPath);
            string DIBookFolderPath = DirInfo.Parent.FullName;//Path.GetDirectoryName(dIBookPath);
            string DIBookName = DirInfo.Name;
            String PresNameTobeSavedInDB = DIBookName + this.FileNameSeperator + DIBookFolderPath;
            return PresNameTobeSavedInDB;
        }

        private String GetDiVideoFileNameForDB(String dIVideoPath)
        {
            // Get DiBook File name  
            DirectoryInfo DirInfo = new DirectoryInfo(dIVideoPath);
            string DIBookFolderPath = DirInfo.Parent.FullName;//Path.GetDirectoryName(dIBookPath);
            string DIBookName = DirInfo.Name;
            String PresNameTobeSavedInDB = DIBookName + this.FileNameSeperator + DIBookFolderPath;
            return PresNameTobeSavedInDB;
        }

        /// <summary>
        ///  Add Dibook to Gallery
        /// </summary>
        /// <param name="dIBookName"></param>
        /// <param name="GalleryPath"></param>
        /// <returns></returns>
        private int UpdateDBTablesForDIBook(String dIBookName, String GalleryPath,String Pres_Type)
        {
            String sSql = String.Empty;
            string Pres_FileName = string.Empty;
            int PresNid = 0;
            int GalleryFolderNId = 0;            
            string DIBookKeyword = string.Empty;
            try
            {
                // Get FileName saved in Database
                Pres_FileName = dIBookName;
                //DIBook name will be saved as Keyword in gallery database so search can be performed on it
                DIBookKeyword = Pres_FileName.Split(FileNameSeperator.ToCharArray())[0].ToString().Trim();
                //Clear  older Presearch Tables 
                this.ClearPreSearchTable();                

                #region " -- File already in Database.Delete it -- "
                // Ckeck File already in DataBase               
                try
                {
                    // -- Get Gallery Id .If Gallery already Exist                       
                    GalleryFolderNId = GetGalleryIdForPresentation(GalleryPath);

                    PresNid = GetPresNIdIfExistInDB(this.DBConnection, Pres_FileName, GalleryFolderNId, Pres_Type);

                    // Delete Presentation entry from database if already exist 
                    if (PresNid != 0)
                    {
                        this.DeletePresFromGallery(Pres_Nid);
                    }
                }
                catch (Exception ex)
                {
                }
                #endregion

                // If gallery folder do not exist add it
                if (GalleryFolderNId == 0)
                {
                    GalleryFolderNId = AddNewGalleryToGallMst(GalleryPath);
                }
                // Insert into  Master Table
                sSql = "Insert into " + DBTable.UT_PresMst + " ( " + PresentationMaster.Pres_FileName + ","
                    + PresentationMaster.Pres_Type + "," + PresentationMaster.GalleryId + ") values ('" + Pres_FileName + "','" + Pres_Type + "','" + GalleryFolderNId.ToString() + "')";

                //Executing Query           
                DBConnection.ExecuteNonQuery(sSql);

                try
                {
                    // Get New PresNid 
                    sSql = "Select max(" + PresentationMaster.Pres_NId + ") from " + DBTable.UT_PresMst;
                    PresNid = (int)DBConnection.ExecuteScalarSqlQuery(sSql);

                    // Insert new Record in Pres Table
                    sSql = " Insert into " + DBTable.UT_PresKeyword +
                      " (" + PresentationMaster.Pres_NId + "," +
                        PresentationMaster.Pres_Type + "," +
                        PresentationKeywords.Pres_Keywords + ") values ("
                       + PresNid + ",'B','" +DIBookKeyword +"') ";
                    DBConnection.ExecuteNonQuery(sSql);
                }
                catch (Exception ex)
                {
                    // Delete Entry From PresMaster for this NID and Set Success to False                     
                    //Successs = false;
                    sSql = "DELETE * from UT_PresMst where " + PresentationMaster.Pres_NId + " = " + PresNid;
                    DBConnection.ExecuteNonQuery(sSql);
                    PresNid = 0;
                }
            }
            catch (Exception EX)
            {
                //Successs = false;
                PresNid = 0;
            }
            return PresNid;
        }

        private  Int32 GetPresNIdIfExistInDB(DIConnection dBConnection, string Pres_FileName, int GalleryFolderNid, string pres_Type)
        {
            string sSql = String.Empty;
            Int32 RetVal = 0;
            try
            {
                sSql = GetPresNIdQuery(Pres_FileName, GalleryFolderNid, pres_Type);

                RetVal = Convert.ToInt32(dBConnection.ExecuteScalarSqlQuery(sSql));
            }
            catch (Exception ex)
            {
                RetVal = 0;
            }

            return RetVal;
        }

        /// <summary>
        /// Get  Query for Getting PresNId from PresMaster using PresType and Gallery FolderNId
        /// </summary>
        /// <param name="Pres_FileName"></param>
        /// <param name="GalleryFolderNid"> this  may be 0 .Indicating Gallery id not available </param>
        /// <param name="pres_Type"></param>
        /// <returns></returns>
        private string GetPresNIdQuery(string Pres_FileName, int GalleryFolderNid, string pres_Type)
        {
            string sSql = string.Empty;
            if (GalleryFolderNid != 0)
            {
                //-- Check  name of presentaion folder exist or not
                sSql = "SELECT " + PresentationMaster.Pres_NId + " FROM " + DBTable.UT_PresMst + " WHERE " + PresentationMaster.Pres_FileName + " ='" + Pres_FileName + "' and " + PresentationMaster.Pres_Type + "='" + pres_Type +
                    "' AND " + PresentationMaster.GalleryId + " = " + GalleryFolderNid;
            }
            else
            {
                //-- Check  name of presentaion folder exist or not
                sSql = "SELECT " + PresentationMaster.Pres_NId + " FROM " + DBTable.UT_PresMst + " WHERE " + PresentationMaster.Pres_FileName + " ='" + Pres_FileName + "' and " + PresentationMaster.Pres_Type + "='" + pres_Type + "' ";
            }
            return sSql;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="GalleryFolderNid"></param>
        /// <returns></returns>
        private Int32 GetGalleryIdForPresentation(string galleryFolderPath)
        {
            Int32 RetVal = 0;
            String sSql = String.Empty;
            try
            {
                //Get NId of Gallery Folder.
                sSql = "SELECT " + GalleryMaster.GalleryFolderNId + " FROM " +
                    DBTable.UT_GalleryMst + " WHERE " + GalleryMaster.GalleryFolder
                    + " = '" + DICommon.RemoveQuotes(galleryFolderPath) + "'";
                RetVal = Convert.ToInt32(DBConnection.ExecuteScalarSqlQuery(sSql));
            }
            catch (Exception ex)
            {
                RetVal = 0;
            }
            return RetVal;
        }

        /// <summary>
        /// Removes Quotes From the String
        /// Remove quotes in BAL under DICommon is also doing this RetVal.Replace("’", "''");
        /// Which is not required as name may  contain string Like -- cot^e d`Ivoire
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveQuotes(string value)
        {
            string RetVal = string.Empty;

            RetVal = value.Replace("'", "''");
           
            return RetVal;
        }
        #endregion

        #endregion

        #region " -- Public --  "

        #region " -- Properties -- "

        private string _GalleryDatabase=string.Empty;
        /// <summary>
        /// Get or set the path and the file name of the Gallery database
        /// </summary>
        public string GalleryDatabase
        {
            get
            {
                return this._GalleryDatabase;
            }
            set
            {
               this._GalleryDatabase = value; 
            }
        }
	
        #endregion

        #region " -- Methods --  "

        #region " -- Create keyword and update Database -- "                                        
                    //Step1: Create keywords and update DataBase (IF Fail Set Success variable to false)
                  
                    #endregion
        /// <summary>
        /// Add Presentation to gallery Database
        /// </summary>
        /// <param name="presentationPath">Excel Presentation Path</param>
        /// <param name="dBPath"></param>
        public Int32 AddPresentationToGalleryDatabase(string presentationPath, string dBPath)
        {
            Int32 RetVal =0;            
            bool ValidPresentationFile = false;
            bool ValidDatabase = false;
            
            // Validate Presentation Path

            ValidPresentationFile =this.ValidateFilePath(presentationPath);
            if (ValidPresentationFile)
            {
                //Validate DB Path and Chk DB have Required Tables            
                ValidDatabase = this.ValidateDB(dBPath);
                if (ValidDatabase)
                {
                    //Get KeyWords from Presentation . Using Spreadsheet
                    // Update Database
                     RetVal= GetKeywordAndUpdateDatabase(presentationPath);
                }
            }
            if (this.DBConnection!=null)
            {
                DBConnection.Dispose();
            }
            return RetVal;
        }

        /// <summary>
        /// Add Presentation to gallery Database ,when  UserKeywords are Provided
        /// </summary>
        /// <param name="presentationPath">Excel Presentation Path</param>
        /// <param name="dBPath"></param>
        public Int32 AddPresentationToGalleryDatabase(string presentationPath, string UKeywords, string dBPath)
        {
            Int32 RetVal = 0;
            bool ValidPresentationFile = false;
            bool ValidDatabase = false;

            // Validate Presentation Path

            ValidPresentationFile = this.ValidateFilePath(presentationPath);
            if (ValidPresentationFile)
            {
                //Validate DB Path and Chk DB have Required Tables            
                ValidDatabase = this.ValidateDB(dBPath);
                if (ValidDatabase)
                {
                    //Get KeyWords from Presentation . Using Spreadsheet
                    // Update Database
                    RetVal = GetKeywordAndUpdateDatabase(presentationPath,UKeywords);
                }
            }
            if (this.DBConnection != null)
            {
                DBConnection.Dispose();
            }
            return RetVal;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="presType"></param>
        /// <param name="PresTitle">Title of Presentaion</param>
        /// <param name="userKeyword"></param>
        /// <param name="presKeywordDataTable"></param>
        /// <param name="presentaionFilePath"></param>
        /// <remarks> User can pass Empty string for presTitle and userKeyword. In case of xls file 
        /// PresTitle and User keyword will be extracted from presentaion excel file </remarks>
        /// Example :AddPresentationToGalleryDatabase(Presentation.PresentationType.Graph, "", "", keywordDataTable, @"C:\-- Projects --\DevInfo 6.0\Gallery\Presentations\Adut literacy rate India - Graph.xls");
        public int  AddPresentationToGalleryDatabase(Presentation.PresentationType presType,string PresTitle,string userKeyword, DataTable presKeywordDataTable, string presentaionFilePath)
        {
            int RetVal = -1;
            bool ValidPresentation = false;
            bool ValidDatabase = false;
            DataTable CustomizedPresDataTable;
            DataView PresentationDataView = presKeywordDataTable.DefaultView;
            string[] PresDTColumns = new string[6];
            PresDTColumns[0] = DI_LibDAL.Queries.DIColumns.Timeperiods.TimePeriod;
            PresDTColumns[1] = DI_LibDAL.Queries.DIColumns.Area.AreaName;
            PresDTColumns[2] = DI_LibDAL.Queries.DIColumns.Indicator.IndicatorName;
            PresDTColumns[3] = DI_LibDAL.Queries.DIColumns.Unit.UnitName;
            PresDTColumns[4] = DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupVal;
            PresDTColumns[5] = DI_LibDAL.Queries.DIColumns.IndicatorClassifications.ICName;

            // Get presentaion data table with required column only
            CustomizedPresDataTable = PresentationDataView.ToTable(true, PresDTColumns);

            // Validate Presentation Path
            if (presKeywordDataTable.Rows.Count > 0)
            {
                //TODO : Validation for valid DataTable
                ValidPresentation = true;
            }

            if (ValidPresentation)
            {
                //Validate DB Path and Chk DB have Required Tables            
                ValidDatabase = this.ValidateDB();
                if (ValidDatabase)
                {
                    //Get KeyWords from Presentation . Using Spreadsheet
                    // Update Database                    
                    RetVal= this.GetKeywordAndUpdateDatabase(presType, PresTitle, userKeyword, CustomizedPresDataTable, presentaionFilePath);
                }
            }
            if (this.DBConnection != null)
            {
                DBConnection.Dispose();
            }
            return RetVal;
        }
            
        /// <summary>
        ///  Delete Presentaion from Gallery Database
        /// </summary>
        /// <param name="presentationPath"></param>
        /// <param name="dbPath"></param>
        public void DeletePresentationFromGalleryDatabase(String presentationPath, string dbPath)
        {
            this.GalleryDatabase = dbPath;
            this.DeletePresentationFromGalleryDatabase(presentationPath);
        }

        /// <summary>
        /// Delete Presentaion from Gallery Database
        /// </summary>
        /// <param name="presentationPath">Path of presentation to be deleted</param>
        public void DeletePresentationFromGalleryDatabase(String presentationPath)
        {            
            string PresFileNameForDelete = string.Empty;
            string PresGalleryPathForDelete = string.Empty;
            string sSql = string.Empty;
            string PDS_PresentationNIdTobeUpdated = string.Empty;

            //Get Presentaion filename and Gallery path
            PresGalleryPathForDelete = Path.GetDirectoryName(presentationPath);

            // Get file name from presentaion id which is found in database
            PresFileNameForDelete =Path.GetFileNameWithoutExtension(presentationPath);
                       
            if (this.ValidateDB(this._GalleryDatabase))
            {
                // Find Presention in PresMaster and get it's NId
                try
                {                  
                    // Check Gallery ID also
                    this.DeletePresFromGalleryDB(PresFileNameForDelete, PresGalleryPathForDelete);
                }
                catch (Exception ex)
                {
                }
            }
            if (this.DBConnection !=null)
            {
                this.DBConnection.Dispose();
                this.DBConnection = null;
            }
        }

        /// <summary>
        /// Delete Presentaion details from Gallery DB
        /// </summary>
        /// <param name="PresNIdForDelete"></param>
        /// <param name="PresFileNameForDelete"></param>
        /// <param name="PresGalleryPathForDelete"></param>
        private void DeletePresFromGalleryDB(string PresFileNameForDelete, string PresGalleryPathForDelete)
        {
            // Chk if pres Exist. If do not exist then method will return -1
            int PresNIdForDelete = GetPresNIdFromPresFileNameNGalleryPath(PresFileNameForDelete, PresGalleryPathForDelete);

            //If pres nid found then delete this pres records from tables
            if (PresNIdForDelete != -1)
            {
                this.DeletePresentationsFromDBTables(PresNIdForDelete);
            }
        }

        /// <summary>
        /// Get PresNId using PresName and galleryPath
        /// Used for chking whether pres exist or not
        /// </summary>
        /// <param name="PresFileName"></param>
        /// <param name="PresGalleryPath"></param>
        /// <returns></returns>
        private int GetPresNIdFromPresFileNameNGalleryPath(string PresFileName, string PresGalleryPath)
        {
            string sSql = string.Empty;
            int RetVal = -1;

            //Get NId of Gallery Folder.                   
            int GalleryFolderNid = this.GetGalleryIdForPresentation(DICommon.EscapeWildcardChar(PresGalleryPath));
            if (GalleryFolderNid != 0)
            {
                ////-- Check  name of presentaion folder exist or not
                sSql = "SELECT " + PresentationMaster.Pres_NId + " FROM " + DBTable.UT_PresMst + " WHERE " +
                PresentationMaster.Pres_FileName + "= '" + RemoveQuotes(PresFileName) + "' AND " + PresentationMaster.GalleryId + " = " + GalleryFolderNid;
            }
            else
            {
                sSql = "SELECT " + PresentationMaster.Pres_NId + " FROM " + DBTable.UT_PresMst + " WHERE " +
            PresentationMaster.Pres_FileName + "= '" + RemoveQuotes(PresFileName) + "'";
            }

            // Get  pres nid to be deleted
            RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(sSql));
            return RetVal;
        }       
            
        /// <summary>
        /// Delete DIBook Details from Gallery Database. 
        /// </summary>
        /// <param name="dIBookPath">DIBook Path</param>
        /// <param name="galleryPath">Path of Gallery Folder, With whom this  dibook presentation is associated in B</param>
        public void DeleteDiBookFromGalleryDatabase(String dIBookPath, String galleryPath)
        {            
            string PresFileNameForDelete = string.Empty;
            string PresGalleryPathForDelete = string.Empty;
            string sSql = string.Empty;
            string PDS_PresentationNIdTobeUpdated = string.Empty;

            // Get file name from presentaion id which is found in database
            PresFileNameForDelete = this.GetDiBookFileNameForDB(dIBookPath);

            //Get Presentaion filename and Gallery path
            PresGalleryPathForDelete = galleryPath;
                        
            if (this.ValidateDB(this._GalleryDatabase))
            {
                try
                {
                    //Delete Presentaion  details from database
                    this.DeletePresFromGalleryDB(PresFileNameForDelete, PresGalleryPathForDelete);
                }
                catch (Exception ex)
                {
                }
            }
            if (this.DBConnection != null)
            {
                this.DBConnection.Dispose();
                this.DBConnection = null;
            }

        }

        /// <summary>
        /// Delete DIBook Details from Gallery Database. 
        /// </summary>
        /// <param name="dIBookPath">DIBook Path</param>
        /// <param name="galleryPath">Path of Gallery Folder, With whom this  dibook presentation is associated in B</param>
        public void DeleteDiVideoFromGalleryDatabase(String dIVideoPath, String galleryPath)
        {
            string PresFileNameForDelete = string.Empty;
            string PresGalleryPathForDelete = string.Empty;
            string sSql = string.Empty;
            string PDS_PresentationNIdTobeUpdated = string.Empty;

            // Get file name from presentaion id which is found in database
            PresFileNameForDelete = this.GetDiVideoFileNameForDB(dIVideoPath);

            //Get Presentaion filename and Gallery path
            PresGalleryPathForDelete = galleryPath;

            if (this.ValidateDB(this._GalleryDatabase))
            {
                try
                {
                    //Delete Presentaion  details from database
                    this.DeletePresFromGalleryDB(PresFileNameForDelete, PresGalleryPathForDelete);
                }
                catch (Exception ex)
                {
                }
            }
            if (this.DBConnection != null)
            {
                this.DBConnection.Dispose();
                this.DBConnection = null;
            }

        }


        #region " -- User Keywords -- " 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="GalleryId"></param>
        /// <param name="galleryDatabase"></param>
        public void UpdateLastUsedGallery(int lastUsedGalleryId)
        {
           
            if (this.DBConnection==null || this.DBConnection.GetConnection().State==ConnectionState.Closed)
            {
                this.DBConnection = new DIConnection(DIServerType.MsAccess, "", "",this._GalleryDatabase, "", DB_PWD);           		                 
            }
            try
            {               
                // Uncheck all galleries
                String sSql = "UPDATE  " + DBTable.UT_GalleryMst + "  SET " + GalleryMaster.LastUsed + "= false";
                this.DBConnection.ExecuteNonQuery(sSql);

                if (lastUsedGalleryId !=-1)
                {
                    //  Check Last used gallery
                    sSql = "UPDATE  " +DBTable.UT_GalleryMst+ "  SET "+ GalleryMaster.LastUsed +"= true  WHERE " +
                        GalleryMaster.GalleryFolderNId + " = " +lastUsedGalleryId;
                    this.DBConnection.ExecuteNonQuery(sSql);
                }

             }
            catch (Exception ex)
            {
            }           
           
        }

        /// <summary>
        /// Update Ukeywords for this presentaion.
        /// </summary>
        /// <param name="presNid"></param>
        /// <param name="uKeywords"></param>
        public void UpdateUserKeywordInDatabase(String presentationNids , string uKeywords)
        {
            String sSql = string.Empty;
            string SplitCharacter_Comma = ",";
            string[] PresNIds = null;
            int PresNidTobeUpdated = 0;
            
            // Chk connection
            if (this.DBConnection == null || this.DBConnection.GetConnection().State == ConnectionState.Closed)
            {
                this.DBConnection = new DIConnection(DIServerType.MsAccess, "", "", this._GalleryDatabase, "", DB_PWD);
            }
            
            // Update 
            //Split presNids string on comma
            PresNIds = presentationNids.Split(SplitCharacter_Comma.ToCharArray());
            foreach (string PresNId in PresNIds)
            {
                // Get pres Nid to be updated
                PresNidTobeUpdated = Convert.ToInt32(PresNId);
                try
                {
                   // if (PresNidTobeUpdated > 0 && !string.IsNullOrEmpty(uKeywords))
                     if (PresNidTobeUpdated > 0 )
                    {                        
                        sSql = "UPDATE  " + DBTable.UT_PresKeyword + "  SET " + PresentationKeywords.Pres_UKeywords + "= '" +DICommon.RemoveQuotes(uKeywords) + "' WHERE " +
                               PresentationKeywords.Pres_NId + " = " + PresNidTobeUpdated;
                        this.DBConnection.ExecuteNonQuery(sSql);

                        //Clear  older Presearch Tables
                        this.ClearPreSearchTable();                        
                    }

                }
                catch (Exception ex)
                {
                }
            }
            this.DBConnection.Dispose();
        }

        /// <summary>
        /// Get UserKeywords for Presentation
        /// </summary>
        /// <param name="presNIdForKeyword"></param>
        /// <returns></returns>
        public String GetUserKeywordsFromDatabase(Int32 presNIdForKeyword)
        {
            String RetVal = string.Empty;
            String sSql = string.Empty;
            // Chk connection
            if (this.DBConnection == null || this.DBConnection.GetConnection().State == ConnectionState.Closed)
            {
                if (!string.IsNullOrEmpty(this._GalleryDatabase))
                {
                    this.DBConnection = new DIConnection(DIServerType.MsAccess, "", "", this._GalleryDatabase, "", DB_PWD);
                }             
            }
            try
            {
                // use Query to get UKeywords
                sSql = "SELECT  " + PresentationKeywords.Pres_UKeywords + "  FROM " + DBTable.UT_PresKeyword + " WHERE " +
                                PresentationKeywords.Pres_NId + " = " + presNIdForKeyword;
                RetVal= this.DBConnection.ExecuteScalarSqlQuery(sSql).ToString();
            }
            catch (Exception ex)
            {
                RetVal = string.Empty;   
            }

            return RetVal;
        }

        #endregion

        /// <summary>
        ///  Get Presentation details from Database Keywords Table
        /// this include Keywords ,UKeywords,Title,PresType
        /// </summary>
        public DataTable GetPresDetailsFromDbKeywordsTable(Int32 presNIdForKeyword)
        {
            DataTable Retval = null;
            string sSql = string.Empty;
            // Chk connection
            if (this.DBConnection == null || this.DBConnection.GetConnection().State == ConnectionState.Closed)
            {
                if (!string.IsNullOrEmpty(this._GalleryDatabase))
                {
                    this.DBConnection = new DIConnection(DIServerType.MsAccess, "", "", this._GalleryDatabase, "", DB_PWD);
                }
            }
            try
            {
                // use Query to get UKeywords
                sSql = "SELECT  " + PresentationKeywords.Pres_NId +"," + PresentationKeywords.Pres_Titles
                + "," +  PresentationKeywords.Pres_Type + "," +  PresentationKeywords.Pres_UKeywords+ "," + PresentationKeywords.Pres_Keywords + "  FROM " + DBTable.UT_PresKeyword + " WHERE " +
                                PresentationKeywords.Pres_NId + " = " + presNIdForKeyword;
                Retval = this.DBConnection.ExecuteDataTable(sSql);
            }
            catch (Exception ex)
            {   
            }
            return Retval;
        }

       /// <summary>
        /// Add DIBook To Gallery .
       /// </summary>
       /// <param name="dIBookPath">diBook Path</param>
       /// <param name="GalleryPath">Path of gallery To which this will be added</param>
       /// <returns></returns>
        public Int32 AddDIBookToGallery(String dIBookPath,String GalleryPath)
        {
            Int32 RetVal = 0;
            string PresTypeDIBook = "B";
            // Add dIBook Path as Presentation Name 
            //If DiBook Path is  C:\-- Projects --\DevInfo 6.0\User Interface - Desktop\bin\DI Book\diBooks\diBook1
            // We Save Presentaion Name as  diBook1 {[]} C:\-- Projects --\DevInfo 6.0\User Interface - Desktop\bin\DI Book\diBooks
            // In Case Of dIBook PresType will Be B
            // Adding Book We prompt Add  Keywords  to Gallery
            
            if (!string.IsNullOrEmpty(dIBookPath) && Directory.Exists(dIBookPath))
            {
                if (this.DBConnection==null && ! string.IsNullOrEmpty(this._GalleryDatabase))
                {
                    // Creating Database Connection using DIConnection                
                    this.DBConnection = new DIConnection(DIServerType.MsAccess, "", "", this._GalleryDatabase, "", DB_PWD);
                }
                try
                {
                    // Check If dIBook Path exist. If exist then add this to Gallery
                    if (Directory.Exists(dIBookPath))
                    {
                        String PresNameTobeSavedInDB = GetDiBookFileNameForDB(dIBookPath);

                        //Clear  older Presearch Tables 
                        this.ClearPreSearchTable();                      

                        // Update  Gallery database Tables for this di book record
                        RetVal = this.UpdateDBTablesForDIBook(PresNameTobeSavedInDB, GalleryPath, PresTypeDIBook);                        
                    }                    
                }
                catch (Exception ex)
                {
                }
            }
            return RetVal;
        }

        /// <summary>
        /// Add DIBook To Gallery .
        /// </summary>
        /// <param name="dIVideoPath">diVideo Path</param>
        /// <param name="GalleryPath">Path of gallery To which this will be added</param>
        /// <returns></returns>
        public Int32 AddDIVideoToGallery_old(String dIVideoPath, String GalleryPath)
        {
            Int32 RetVal = 0;
            String PresTypeDIVideo = "V";
            // Add dIBook Path as Presentation Name 
            //If DiBook Path is  C:\-- Projects --\DevInfo 6.0\User Interface - Desktop\bin\DI Book\diBooks\diBook1
            // We Save Presentaion Name as  diBook1 {[]} C:\-- Projects --\DevInfo 6.0\User Interface - Desktop\bin\DI Book\diBooks
            // In Case Of dIBook PresType will Be B
            // Adding Book We prompt Add  Keywords  to Gallery

            if (!string.IsNullOrEmpty(dIVideoPath) && Directory.Exists(dIVideoPath))
            {
                if (this.DBConnection == null && !string.IsNullOrEmpty(this._GalleryDatabase))
                {
                    // Creating Database Connection using DIConnection                
                    this.DBConnection = new DIConnection(DIServerType.MsAccess, "", "", this._GalleryDatabase, "", DB_PWD);
                }
                try
                {
                    // Check If dIBook Path exist. If exist then add this to Gallery
                    if (Directory.Exists(dIVideoPath))
                    {
                        String PresNameTobeSavedInDB = GetDiBookFileNameForDB(dIVideoPath);

                        //Clear  older Presearch Tables 
                        this.ClearPreSearchTable();

                        // Update  Gallery database Tables for this di book record
                        RetVal = this.UpdateDBTablesForDIBook(PresNameTobeSavedInDB, GalleryPath, PresTypeDIVideo);
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return RetVal;
        }

        public Int32 AddDIVideoToGallery(String dIVideoPath, String GalleryPath)
        {
            Int32 RetVal = 0;
            String PresTypeDIVideo = "V";
            // Add dIBook Path as Presentation Name 
            //If DiBook Path is  C:\-- Projects --\DevInfo 6.0\User Interface - Desktop\bin\DI Book\diBooks\diBook1
            // We Save Presentaion Name as  diBook1 {[]} C:\-- Projects --\DevInfo 6.0\User Interface - Desktop\bin\DI Book\diBooks
            // In Case Of dIBook PresType will Be B
            // Adding Book We prompt Add  Keywords  to Gallery
            if (this.DBConnection == null && !string.IsNullOrEmpty(this._GalleryDatabase))
            {
                // Creating Database Connection using DIConnection                
                this.DBConnection = new DIConnection(DIServerType.MsAccess, "", "", this._GalleryDatabase, "", DB_PWD);
            }

            if (!string.IsNullOrEmpty(dIVideoPath) && File.Exists(dIVideoPath))
            {
               
                try
                {
                    // Check If dIBook Path exist. If exist then add this to Gallery
                    if (File.Exists(dIVideoPath))
                    {
                        String PresNameTobeSavedInDB = GetDiBookFileNameForDB(dIVideoPath);

                        //Clear  older Presearch Tables 
                        this.ClearPreSearchTable();

                        // Update  Gallery database Tables for this di book record
                        RetVal = this.UpdateDBTablesForDIBook(PresNameTobeSavedInDB, GalleryPath, PresTypeDIVideo);
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return RetVal;
        }

        /// <summary>
        /// Get PresNID using PresPath
        /// </summary>
        /// <param name="PresPath"></param>
        /// <param name="presType"></param>
        /// <returns></returns>
        public Int32 GetPresNIdByPresPath(string PresPath, Presentation.PresentationType presType)
        {
            int RetVal=-1;
            string GalleryFolderPath = string.Empty;            
            string PresFileName = string.Empty;

            // Chk connection
            if (this.DBConnection == null || this.DBConnection.GetConnection().State == ConnectionState.Closed)
            {
                this.DBConnection = new DIConnection(DIServerType.MsAccess, "", "", this._GalleryDatabase, "", DB_PWD);
            }

            try
            {
                if (!string.IsNullOrEmpty(PresPath))
                {                 
                    GalleryFolderPath = Path.GetDirectoryName(PresPath);
                    PresFileName = Path.GetFileNameWithoutExtension(PresPath);
                    RetVal = this.GetPresNIdFromPresFileNameNGalleryPath(PresFileName, GalleryFolderPath);                 
                }
            }
            catch (Exception ex)
            {
                RetVal = -1;
            }
            return RetVal;
        }

        public PresentationType GetGalleryPresentationType(string filename)
        {
            PresentationType RetVal = PresentationType.G;
            try
            {
                switch (Presentations.Presentation.GetPresentationType(filename))
                {
                    case Presentation.PresentationType.Table:
                        RetVal = PresentationType.T;
                        break;
                    case Presentation.PresentationType.Graph:
                        RetVal = PresentationType.G;
                        break;
                    case Presentation.PresentationType.Map:
                        RetVal = PresentationType.M;
                        break;
                    case Presentation.PresentationType.None:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        public int UpdateDBTables(String presentationFile, PresentationType pres_Type, string Title, String ukeyword, string keywords, int GalleryFolderNid)
        {
            String sSql = String.Empty;
            string Pres_FileName = string.Empty;
            int PresNid = 0;
            object oPresNid = null;
            try
            {
                // Update DB only when Presentaion file exist
                if (File.Exists(presentationFile))
                {
                    // Save FileName without extention in Database                    
                    // file name may be Côte d’Ivoire - AIDS deaths Total  Number - Table.xls. So do not remove quotes using
                    // DICommon remove Quotes as it replace "’", "''"
                    // Use Local remove quote that  will only replace "'", "''"
                    Pres_FileName = RemoveQuotes(Path.GetFileNameWithoutExtension(presentationFile));
                    // Clear PreSearch Table
                    this.ClearPreSearchTable();

                    #region " -- File already in Database.Delete it -- "
                    ////////////// Ckeck File already in DataBase               
                    try
                    {
                        // If gallery folder do not exist add it
                        if (GalleryFolderNid > 0)
                        {
                            // Check for Presentaion 
                            sSql = this.GetPresNIdQuery(Pres_FileName, GalleryFolderNid, pres_Type.ToString());
                            oPresNid = DBConnection.ExecuteScalarSqlQuery(sSql);

                            // If presentation  was found in gallery db  then delete it.
                            if (oPresNid != null)
                            {
                                this.DeletePresFromGallery(Convert.ToInt32(oPresNid));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    #endregion

                    //// If gallery folder do not exist add it
                    //if (GalleryFolderNid==0)
                    //{
                    //    // Add new Gallery in Gallery Mst
                    //    GalleryFolderNid = this.AddNewGalleryToGallMst(Path.GetDirectoryName(presentationFile));                       
                    //}

                    // Insert into  Master Table
                    sSql = "Insert into " + DBTable.UT_PresMst + " ( " + PresentationMaster.Pres_FileName + ","
                        + PresentationMaster.Pres_Type + "," + PresentationMaster.GalleryId + ") values ('" + Pres_FileName + "','" + pres_Type + "','" + GalleryFolderNid.ToString() + "')";

                    //Executing Query           
                    DBConnection.ExecuteNonQuery(sSql);

                    try
                    {
                        // Get New PresNid 
                        sSql = "Select max(" + PresentationMaster.Pres_NId + ") from " + DBTable.UT_PresMst;
                        PresNid = (int)DBConnection.ExecuteScalarSqlQuery(sSql);

                        // Insert new Record in Pres Table
                        sSql = " Insert into " + DBTable.UT_PresKeyword +
                          " (" + PresentationMaster.Pres_NId + "," + PresentationKeywords.Pres_Titles + "," +
                            PresentationMaster.Pres_Type + "," + PresentationKeywords.Pres_UKeywords + "," + PresentationKeywords.Pres_Keywords + ") values ("
                           + PresNid + ",'" + DICommon.RemoveQuotes(Title) + "','" + pres_Type + "','" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(ukeyword)) + "','" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(keywords)) + "') ";
                        DBConnection.ExecuteNonQuery(sSql);
                    }
                    catch (Exception ex)
                    {
                        // Delete Entry From PresMaster for this NID and Set Success to False                     
                        //Successs = false;
                        sSql = "DELETE * from UT_PresMst where " + PresentationMaster.Pres_NId + " = " + PresNid;
                        DBConnection.ExecuteNonQuery(sSql);
                        PresNid = 0;
                    }
                }
            }

            catch (Exception EX)
            {
                //Successs = false;
                PresNid = 0;
            }
            return PresNid;
        }

        /// <summary>
        /// check and create the folder
        /// </summary>
        /// <param name="presentationFile"></param>
        /// <returns></returns>
        public Int32 CheckNCreateGalleryFolder(String galleryPath)
        {
            Int32 RetVal = 0;
            String sSql = String.Empty;
            int GalleryCount = 0;
            IDataReader FolderReader;

            if (this.DBConnection == null || this.DBConnection.GetConnection().State == ConnectionState.Closed)
            {
                this.DBConnection = new DIConnection(DIServerType.MsAccess, "", "", this._GalleryDatabase, "", DB_PWD);
            }

            sSql = "Select " + GalleryMaster.GalleryFolderNId + " FROM " + DBTable.UT_GalleryMst + " WHERE " + GalleryMaster.GalleryFolder + " = '" + DICommon.EscapeWildcardChar(galleryPath) + "'";
            FolderReader= this.DBConnection.ExecuteReader(sSql);

            if (!FolderReader.Read())
            {
                FolderReader.Close();
                // Check If any Gallery Already exist
                sSql = "SELECT count(*)  FROM " + DBTable.UT_GalleryMst;

                GalleryCount = Convert.ToInt32(DBConnection.ExecuteScalarSqlQuery(sSql));
                if (GalleryCount == 0)
                {
                    sSql = "Insert INTO " + DBTable.UT_GalleryMst + " ( " + GalleryMaster.GalleryFolder + "," + GalleryMaster.LastUsed + ") values ('"
                                            + DICommon.EscapeWildcardChar(galleryPath) + "'," + "true" + ")";
                }
                else
                {
                    sSql = "Insert INTO " + DBTable.UT_GalleryMst + " ( " + GalleryMaster.GalleryFolder + "," + GalleryMaster.LastUsed + ") values ('"
                                            + DICommon.EscapeWildcardChar(galleryPath) + "'," + "false" + ")";
                }

                //Executing Query           
                DBConnection.ExecuteNonQuery(sSql);
                RetVal = Convert.ToInt32(DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));
            }
            else
            {
                RetVal = Convert.ToInt32(FolderReader[GalleryMaster.GalleryFolderNId]);
                FolderReader.Close();
            }
            return RetVal;
        }


        #endregion

        #region " -- Constructor -- "
        /// <summary>
        /// Default  Constructor
        /// </summary>
        public  GalleryDBBuild()
        {
        }

        /// <summary>
        /// Constructor with GalleryDatabasePath as parameter
        /// </summary>
        /// <param name="galleryDatabasePath">Full Path of GalleryDatabase </param>
        public  GalleryDBBuild(string galleryDatabasePath)
        {
            //Set Gallery Database Path
            this._GalleryDatabase = galleryDatabasePath;
        }

        /// <summary>
        ///  Dispose Connection
        /// </summary>
        public void Dispose()
        {
            try
            {
                // If db connection exist then Dispose it
                if (this.DBConnection != null)
                {
                    this.DBConnection.Dispose();
                    this.DBConnection = null;
                }
            }
            catch (Exception ex)
            {                
                throw;
            }            
        }

        #endregion

        #region " -- Events--  "
      //  public event NewGalleryAddedDelegate NewGalleryAdded;
        #endregion

        #endregion

        #region " -- To Delete -- "
        /// <summary>
        /// Delete Presentaion from Gallery Database
        /// </summary>
        /// <param name="presentationPath"></param>
        /// <param name="dbPath"></param>
        /// <remarks>To be deleted</remarks> 
        public void DeletePresentationFromGalleryDatabase_old(String presentationPath, string dbPath)
        {
            int PresNIdForDelete = 0;
            string PresFileNameForDelete = string.Empty;
            string PresGalleryPathForDelete = string.Empty;
            string sSql = string.Empty;
            string PDS_PresentationNIdTobeUpdated = string.Empty;

            //Get Presentaion filename and Gallery path
            PresGalleryPathForDelete = Path.GetDirectoryName(presentationPath);
            PresFileNameForDelete = Path.GetFileName(presentationPath);

            if (this.ValidateDB(dbPath))
            {

                // Find Presention in PresMaster and get it's NId
                try
                {
                    sSql = "SELECT " + PresentationMaster.Pres_NId + " FROM " + DBTable.UT_PresMst + " WHERE " +
                    PresentationMaster.Pres_FileName + "= '" + PresFileNameForDelete + "'";

                    // Get  pres nid to be deleted
                    PresNIdForDelete = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(sSql));

                    //If pres nid found then delete this pres records from tables
                    if (PresNIdForDelete != 0)
                    {
                        //DELETE FROM Pres keyword
                        sSql = "DELETE FROM " + DBTable.UT_PresKeyword + " WHERE " + PresentationMaster.Pres_NId + "=" + PresNIdForDelete;
                        this.DBConnection.ExecuteNonQuery(sSql);

                        //DELETE FROM Pres Mst
                        sSql = "DELETE FROM " + DBTable.UT_PresMst + " WHERE " + PresentationMaster.Pres_NId + "=" + PresNIdForDelete;
                        this.DBConnection.ExecuteNonQuery(sSql);

                        //Delete from presearch                      
                        sSql = "DELETE FROM  UT_PreSearches WHERE  PDS_Presentation_NIds=" + PresNIdForDelete;
                        this.DBConnection.ExecuteNonQuery(sSql);


                        string ConnString = this.DBConnection.GetConnection().ConnectionString;
                        DIServerType TempserverType = DBConnection.ConnectionStringParameters.ServerType;
                        this.DBConnection.Dispose();

                        DIConnection TempDBConn = new DIConnection(ConnString, TempserverType);

                        System.Data.Common.DbDataAdapter Adapter = TempDBConn.CreateDBDataAdapter();
                        System.Data.Common.DbCommand cmd = TempDBConn.GetCurrentDBProvider().CreateCommand();
                        cmd.CommandText = "Select * from " + DBTable.UT_PreSearches;
                        cmd.Connection = TempDBConn.GetConnection();
                        Adapter.SelectCommand = cmd;

                        System.Data.Common.DbCommandBuilder CmdBuilder = TempDBConn.GetCurrentDBProvider().CreateCommandBuilder();
                        CmdBuilder.DataAdapter = Adapter;

                        DataSet TargetFileDataset = new System.Data.DataSet();

                        Adapter.Fill(TargetFileDataset, DBTable.UT_PreSearches);

                        //  Update                         
                        foreach (DataRow DRow in TargetFileDataset.Tables[0].Rows)
                        {
                            if (DRow[PreSearches.PDS_Presentation_NIds].ToString().Contains(PresNIdForDelete.ToString()))
                            {
                                if (DRow[PreSearches.PDS_Presentation_NIds].ToString().EndsWith(PresNIdForDelete.ToString()))
                                {
                                    DRow[PreSearches.PDS_Presentation_NIds] = DRow[PreSearches.PDS_Presentation_NIds].ToString().Replace(PresNIdForDelete.ToString(), " ").Trim();
                                }
                                else
                                {
                                    DRow[PreSearches.PDS_Presentation_NIds] = DRow[PreSearches.PDS_Presentation_NIds].ToString().Replace(PresNIdForDelete.ToString() + ",", "").Trim();
                                }
                            }
                        }
                        TargetFileDataset.AcceptChanges();

                        //update TempDataTable into target database
                        Adapter.Update(TargetFileDataset, DBTable.UT_PreSearches);
                        System.Threading.Thread.Sleep(1000);
                        TempDBConn.Dispose();
                        this.DBConnection = new DIConnection(ConnString, TempserverType);
                    }
                }
                catch (Exception ex)
                {
                }

            }

        }

        public void DeleteDiBookFromGalleryDatabase_old(String dIBookPath, String galleryPath)
        {
            int PresNIdForDelete = 0;
            string PresFileNameForDelete = string.Empty;
            string PresGalleryPathForDelete = string.Empty;
            string sSql = string.Empty;
            string PDS_PresentationNIdTobeUpdated = string.Empty;

            // Get file name from presentaion id which is found in database
            PresFileNameForDelete = this.GetDiBookFileNameForDB(dIBookPath);

            //Get Presentaion filename and Gallery path
            PresGalleryPathForDelete = galleryPath;

            // Find Presention in PresMaster and get it's NId
            if (this.ValidateDB(this._GalleryDatabase))
            {
                try
                {
                    // Check Gallery ID also
                    //Get NId of Gallery Folder.
                    sSql = "SELECT " + GalleryMaster.GalleryFolderNId + " FROM " +
                        DBTable.UT_GalleryMst + " WHERE " + GalleryMaster.GalleryFolder
                        + " = '" + DICommon.EscapeWildcardChar(PresGalleryPathForDelete) + "'";
                    int GalleryFolderNid = Convert.ToInt32(DBConnection.ExecuteScalarSqlQuery(sSql));
                    if (GalleryFolderNid != 0)
                    {
                        ////-- Check  name of presentaion folder exist or not
                        sSql = "SELECT " + PresentationMaster.Pres_NId + " FROM " + DBTable.UT_PresMst + " WHERE " +
                  PresentationMaster.Pres_FileName + "= '" + PresFileNameForDelete + "' AND " + PresentationMaster.GalleryId + " = " + GalleryFolderNid;

                    }
                    else
                    {
                        sSql = "SELECT " + PresentationMaster.Pres_NId + " FROM " + DBTable.UT_PresMst + " WHERE " +
                    PresentationMaster.Pres_FileName + "= '" + PresFileNameForDelete + "'";
                    }

                    // Get  pres nid to be deleted
                    PresNIdForDelete = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(sSql));

                    //If pres nid found then delete this pres records from tables
                    if (PresNIdForDelete != 0)
                    {
                        //DELETE FROM Pres keyword
                        sSql = "DELETE FROM " + DBTable.UT_PresKeyword + " WHERE " + PresentationMaster.Pres_NId + "=" + PresNIdForDelete;
                        this.DBConnection.ExecuteNonQuery(sSql);

                        //DELETE FROM Pres Mst
                        sSql = "DELETE FROM " + DBTable.UT_PresMst + " WHERE " + PresentationMaster.Pres_NId + "=" + PresNIdForDelete;
                        this.DBConnection.ExecuteNonQuery(sSql);

                        //Delete from presearch
                        // Remove all presearch where =
                        //sSql = "DELETE FROM  UT_PreSearches WHERE  PDS_Presentation_NIds LIKE 'PresNIdForDelete'";
                        sSql = "DELETE FROM  UT_PreSearches WHERE  PDS_Presentation_NIds = '" + PresNIdForDelete +
                            "' OR  PDS_Presentation_NIds LIKE '%," + PresNIdForDelete + "' OR PDS_Presentation_NIds LIKE '%," + PresNIdForDelete
                            + ",%'  or  PDS_Presentation_Nids LIKE '" + PresNIdForDelete + ",%' ";

                        //Execute query
                        this.DBConnection.ExecuteNonQuery(sSql);
                    }
                }
                catch (Exception ex)
                {
                }
            }
            if (this.DBConnection != null)
            {
                this.DBConnection.Dispose();
                this.DBConnection = null;
            }

        }


        /// <summary>
        /// Read excel workbook and update Database with Keywords
        /// </summary>
        /// <param name="presentationFile">Excel work book to read</param>        
        private Int32 GetKeywordAndUpdateDatabase_Old(string presentationFile)
        {
            Int32 RetVal = 0;
            System.Data.DataTable ExcelSheetDataTable;
            System.Data.DataTable UKeywordsDataTable;
            String Pres_FileName = string.Empty;
            PresentationType Pres_Type = PresentationType.M;
            String Title = String.Empty;
            String UKeyword = String.Empty;
            String Keywords = String.Empty;

            String FileNameForTitle = String.Empty;  // In Case of Graph and Table title will be title + subtitles and filename
            int Map_Suffix_Lenght = 10;      //" - Map.xls"            

            try
            {
                // Using Excel Control Library
                DIExcel DIExl = new DIExcel(presentationFile);

                // Getting Pres fileName For Database Field
                Pres_FileName = Path.GetFileName(presentationFile);

                //Start  of getting Data from Excel Sheet
                // Check For Presention Type                                    
                if (presentationFile.Contains(Presentation.Map_Suffix)) // Presention is Map : Get Title and Data 
                {
                    // Get Presentation Type for Database table when presentation file is Map
                    //Pres_Type = Pres_Type_M;
                    Pres_Type = PresentationType.M;

                    // In case of Map Title Column will be name of the file minus " - Map.xls"
                    Title = Pres_FileName.Substring(0, Pres_FileName.Length - Map_Suffix_Lenght);

                    // Using Excel Control Function to get value from Sheet
                    ExcelSheetDataTable = DIExl.GetDataTableFromSheet(ExcelSheetName.MAPDATA);
                }
                else //  Get Title and Data for Graph and Map Presentioan 
                {
                    // Get Presentation Type for Database table when presentation file is table or Graph                    
                    // Setting Presention Type On the basis of suffix
                    if (presentationFile.Contains(Presentation.Graph_Suffix) || presentationFile.ToLower().Contains(Presentation.Graph_Suffix.ToLower()))
                    {
                        Pres_Type = PresentationType.G;
                    }
                    else if (presentationFile.Contains(Presentation.Table_Suffix) || presentationFile.ToLower().Contains(Presentation.Table_Suffix.ToLower()))
                    {
                        Pres_Type = PresentationType.T;
                    }

                    // Get Presentation Title
                    Title = this.GetPresentationTitle(Pres_Type, Pres_FileName, DIExl);

                    // DataTable In case of Graph  or Table
                    ExcelSheetDataTable = DIExl.GetDataTableFromSheet(ExcelSheetName.DATA);
                }
                //End of getting Data from Excel Sheet

                //Getting Keywords
                Keywords = this.GetKeyWordsFromPresentationDataTable(ExcelSheetDataTable);

                // --- Get UserKeywords ---
                // Get Keywords Sheet into DataTable               
                UKeywordsDataTable = DIExl.GetDataTableFromSheet(ExcelSheetName.KEYWORDS);

                // Get UserKeyword
                UKeyword = this.GetUserKeyWords(UKeywordsDataTable);

                // Update Database
                Pres_Nid = UpdateDBTables(presentationFile, Pres_Type, Title, UKeyword, Keywords);
                RetVal = Pres_Nid;
            }
            catch (Exception ex)
            {
                //Successs = false;
            }
            return RetVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="presentationFile"></param>
        /// <returns></returns>
        private Int32 AddNewGalleryToGallMst(String galleryPath)
        {
            Int32 RetVal = 0;
            String sSql = String.Empty;
            int GalleryCount = 0;

            // Check If any Gallery Already exist
            sSql = "SELECT count(*)  FROM " + DBTable.UT_GalleryMst;

            GalleryCount = Convert.ToInt32(DBConnection.ExecuteScalarSqlQuery(sSql));
            if (GalleryCount == 0)
            {
                sSql = "Insert INTO " + DBTable.UT_GalleryMst + " ( " + GalleryMaster.GalleryFolder + "," + GalleryMaster.LastUsed + ") values ('"
                                        + DICommon.EscapeWildcardChar(galleryPath) + "'," + "true" + ")";
            }
            else
            {
                sSql = "Insert INTO " + DBTable.UT_GalleryMst + " ( " + GalleryMaster.GalleryFolder + "," + GalleryMaster.LastUsed + ") values ('"
                                        + DICommon.EscapeWildcardChar(galleryPath) + "'," + "false" + ")";
            }

            //Executing Query           
            DBConnection.ExecuteNonQuery(sSql);
            RetVal = Convert.ToInt32(DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));
            return RetVal;
        }  

        #endregion
    }

    #region "-- Internal Classes Holding Constants-- " 
    
    /// <summary>
    /// Class Holding Constant related to ExcelSheet Name
    /// </summary>
    internal class ExcelSheetName
    {
        internal static String DATA = DILanguage.GetLanguageString("DATA");
        internal static String MAPDATA =DILanguage.GetLanguageString("MAPDATA");
        internal static String KEYWORDS = DILanguage.GetLanguageString("KEYWORDS");
    }
    /// <summary>
    /// /// Class Holding Constant related to ExcelSheet Column name
    /// </summary>
    internal class ExcelColumnsName
    {
        internal static String DATA = DILanguage.GetLanguageString("DATA");
        internal static String DATA_Value = DILanguage.GetLanguageString("DATAVALUE");
        internal static String Area_ID = DILanguage.GetLanguageString("AREAID");
    }
    /// <summary>
    ///  Class Holding Constants related to Database
    /// </summary>
    internal class DBTable
    {
        // Table Names
        internal const String UT_PresMst = "UT_PresMst";
        internal const String UT_PreSearches = "UT_PreSearches";
        internal const String UT_PresKeyword = "UT_PresKeyword";
        internal const String UT_GalleryMst = "UT_GalleryMst";

        // Table Column Names
       
          
       

    }

    /// <summary>
    ///  Table Column Names of Pres_Mst table
    /// </summary>
    internal class PresentationMaster
    {
        internal const String Pres_Type = "Pres_Type";
        internal const String Pres_NId = "Pres_NId";
        internal const String Pres_FileName = "Pres_FileName";
        internal const String GalleryId = "GalleryId";        
    }


    /// <summary>
    ///  Table Column Names of Pres_Keywords table
    /// </summary>
    internal class PresentationKeywords
    {
        internal const String Pres_NId = "Pres_NId";
        internal const String Pres_Type = "Pres_Type";
        internal const String Pres_Titles = "Pres_Titles";
        internal const String Pres_UKeywords = "Pres_UKeywords";
        internal const String Pres_Keywords = "Pres_Keywords"; 
    }


    /// <summary>
    ///  Table Column Names of Gallery_Mst table
    /// </summary>
    internal class GalleryMaster
    {
        internal const String GalleryFolderNId = "GalleryFolderNId";
        internal const String GalleryFolder = "GalleryFolder";
        internal const String LastUsed = "LastUsed";
    }

    internal class PreSearches
    {
        internal const String PDS_NId = "PDS_NId";
        internal const String PDS_Keyword = "PDS_Keyword";
        internal const String PDS_Condition = "PDS_Condition";
        internal const String PDS_Presentation_NIds = "PDS_Presentation_NIds";
        internal const String PDS_GalleryId = "PDS_GalleryId";

    }

    #endregion


}
