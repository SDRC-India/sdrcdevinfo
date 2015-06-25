using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibDAL.UserSelection;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.Graph;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.Table;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.Map;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;


namespace DevInfo.Lib.DI_LibBAL.UI.Presentations
{

	/// <summary>
	/// Represents generic functionality related to presentations
	/// </summary>
	public class Presentation
    {
        #region " -- Public -- "

        #region "--Constants--"
        /// <summary>
        /// Name of Keyword worksheet.
        /// </summary>
        public const string KEYWORD_WORKSHEET_NAME = "Keywords";

        /// <summary>
        /// Name of Selections worksheet where xml serialised user preference are preserved
        /// </summary>
        public const string SELECTION_WORKSHEET_NAME = "Selections";

        /// <summary>
        /// Table
        /// </summary>
        public const string TABLE_FOLDER = "Table";

        /// <summary>
        /// Graph
        /// </summary>
        public const string GRAPH_FOLDER = "Graph";

        /// <summary>
        /// Map
        /// </summary>
        public const string MAP_FOLDER = "Map";

        /// <summary>
        /// Table.xls
        /// </summary>
        public const String Table_Suffix = "Table.xls";

        /// <summary>
        /// Graph.xls
        /// </summary>
        public const String Graph_Suffix = "Graph.xls";
        
        /// <summary>
        /// Map.xls
        /// </summary>
        public const String Map_Suffix = "Map.xls";


        #endregion

        #region "--Enums--"

        /// <summary>
        /// Presentation Types
        /// </summary>
        public enum PresentationType
        {
            Table,
            Graph,
            Map,
            FrequencyTable,
            None
        }

        /// <summary>
        /// Wizard Mode
        /// </summary>
        public enum WizardMode
        {
            Create,
            Report,
            DynamicPresentation
        }

        #endregion

        #region "--Methods--"

        /// <summary>
        /// Returns enum value for presentation type based on file name and extension
        /// </summary>
        /// <param name="presentationPath">Full path of the presentation</param>
        /// <returns></returns>
        public static PresentationType GetPresentationType(string presentationPath)
        {
            PresentationType RetVal = PresentationType.None;

            if (presentationPath.ToLower().EndsWith(Table_Suffix.ToLower()))
            {
                RetVal = PresentationType.Table;
            }
            else if (presentationPath.ToLower().EndsWith(Graph_Suffix.ToLower()))
            {
                RetVal = PresentationType.Graph;
            }
            else if (presentationPath.ToLower().EndsWith(Map_Suffix.ToLower()))
            {
                RetVal = PresentationType.Map;
            }

            return RetVal;
        }

        /// <summary>
        /// Check the existence of the selection worksheet in the presentation.
        /// </summary>
        /// <param name="fileNameWPath"></param>
        /// <returns></returns>
        public static bool CheckSelectionWorksheetExistence(string fileNameWPath)
        {
            bool RetVal = false;
            try
            {
                //  Open presentation using excel wrapper class (Spreadsheet gear) 
                DIExcel DIExcel = new DIExcel(fileNameWPath);

                // Identify Selection worksheet 
                int SelectionSheetIndex = DIExcel.GetSheetIndex(SELECTION_WORKSHEET_NAME);

                if (SelectionSheetIndex >= 0)
                {
                    //-- If selection worksheet exists in the presentation
                    RetVal = true;
                }
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        /// <summary>
        /// Insert Xml serialized presentation text in a worksheet and append it to presenatation workbook
        /// </summary>
        /// <param name="Presentation">Preesnataion Class Instance TablePresentation / Map</param>
        /// <param name="PresentationPath">Full path of presentation file</param>
        /// <param name="presentationType">PresentationType enum value. Table / Graph / Map</param>
        public static void InsertSelectionSheet(object Presentation, string PresentationPath, PresentationType presentationType)
        {
            string SerializedText = string.Empty;
            DIExcel DIExcel = new DIExcel(PresentationPath);
            DIExcel.InsertWorkSheet(SELECTION_WORKSHEET_NAME);
            int SelectionWorksheetIndex = DIExcel.GetSheetIndex(SELECTION_WORKSHEET_NAME);

            //-- Upadte the user selection GIds before inserting them in selections sheet 
            //-- Retrieve serialized text
            switch (presentationType)
            {
                case PresentationType.Table:
                    SerializedText = ((TablePresentation)Presentation).GetSerializedText(true);
                    break;
                case PresentationType.Graph:
                    if (((GraphPresentation)Presentation).TablePresentation.UserPreference.General.ShowExcel)
                    {
                        SelectionWorksheetIndex -= 1;
                    }
                    SerializedText = ((GraphPresentation)Presentation).GetSerializedText();
                    break;
                case PresentationType.Map:
                    SerializedText = ((Map.Map)Presentation).GetSerializedText(true);
                    break;
            }

            //Insert Serialized Text into worksheet
            // Single excel cell can hold at the most 32000 characters
            // If length of serialized text is greater than 32000 char then break them and place them in multiple cells
            //TODO Use this logic for Table and graph also and update GetSerializedPresentationText() function accordingly
            if (SerializedText.Length > 32000)
            {
                int i = 0;
                //Calculating and Iterating number of 32000 Characters slots in Text 

                //-- Set First column format type as "TEXT"
                DIExcel.SetColumnFormatType("A:A", SelectionWorksheetIndex, SpreadsheetGear.NumberFormatType.Text);

                while (i < Math.Floor((double)(SerializedText.Length / 32000)))
                {
                    //'Adding next 32000 Charcters each time at i row. 
                    if (i == 160)
                    {

                    }
                    DIExcel.SetCellValue(SelectionWorksheetIndex, i, 0, SerializedText.Substring(32000 * i, 32000));
                    i += 1;
                }
                DIExcel.SetCellValue(SelectionWorksheetIndex, i, 0, SerializedText.Substring(32000 * i));
            }
            else
            {
                DIExcel.SetCellValue(SelectionWorksheetIndex, 0, 0, SerializedText);
            }

            //-- Hide the selection sheet.
            DIExcel.HideWorksheet(SELECTION_WORKSHEET_NAME);

            DIExcel.ActiveSheetIndex = 0;
            DIExcel.Save();
            DIExcel.Close();
        }

        /// <summary>
        /// Extarct Serialized Xml text from Selection worksheet of presentation
        /// </summary>
        /// <param name="PresentationPath">Presentation file path</param>
        /// <param name="presentationType">Presentation Type</param>
        /// <returns></returns>Excel
        public static string GetSerializedPresentationText(string PresentationPath, PresentationType presentationType)
        {
            string RetVal = string.Empty;
            object SerializedText = null;

            //  Open presentation using excel wrapper class (Spreadsheet gear) 
            DIExcel DIExcel = new DIExcel(PresentationPath);

            // Identify Selection worksheet 
            int SelectionSheetIndex = DIExcel.GetSheetIndex(SELECTION_WORKSHEET_NAME);

            // Check for existence of selection tab 
            if (SelectionSheetIndex != -1)
            {
                // In case of graph chart sheet is not conidered as sheet so reduce index by 1
                if (presentationType == Presentation.PresentationType.Graph)
                {
                    SelectionSheetIndex -= 1;
                }

                //Assunmption - Xml serialized text will occupy at the most 20 cells.
                //May convert this logic based on max range used
                for (int i = 0; i < 500; i++)
                {
                    SerializedText = DIExcel.GetCellValue(SelectionSheetIndex, i, 0, i, 0);
                    if (!string.IsNullOrEmpty(SerializedText.ToString()))
                    {
                        RetVal += SerializedText.ToString();
                    }
                    else
                    {
                        break;
                    }
                }
            }
            DIExcel.Close();

            return RetVal;
        }

        /// <summary>
        /// Extarct Serialized Xml text from Selection worksheet of presentation
        /// </summary>
        /// <param name="PresentationPath">Presentation file path</param>
        /// <param name="presentationType">Presentation Type</param>
        /// <param name="showExcel">True, if hosting application use Excel, otherwise false</param>
        /// <returns></returns>Excel
        public static string GetSerializedPresentationText(string PresentationPath, PresentationType presentationType, bool showExcel)
        {
            string RetVal = string.Empty;
            object SerializedText = null;

            //  Open presentation using excel wrapper class (Spreadsheet gear) 
            DIExcel DIExcel = new DIExcel(PresentationPath);

            // Identify Selection worksheet 
            int SelectionSheetIndex = DIExcel.GetSheetIndex(SELECTION_WORKSHEET_NAME);

            // Check for existence of selection tab 
            if (SelectionSheetIndex != -1)
            {
                // In case of graph chart sheet is not conidered as sheet so reduce index by 1
                if (showExcel && presentationType == Presentation.PresentationType.Graph)
                {
                    SelectionSheetIndex -= 1;
                }

                //Assunmption - Xml serialized text will occupy at the most 20 cells.
                //May convert this logic based on max range used
                for (int i = 0; i < 500; i++)
                {
                    SerializedText = DIExcel.GetCellValue(SelectionSheetIndex, i, 0, i, 0);
                    if (!string.IsNullOrEmpty(SerializedText.ToString()))
                    {
                        RetVal += SerializedText.ToString();
                    }
                    else
                    {
                        break;
                    }
                }
            }
            DIExcel.Close();

            return RetVal;
        }


        /// <summary>
        /// Save the presentation in the sidebar
        /// </summary>
        /// <param name="sidebarFolderPath"></param>
        /// <param name="presentationPath"></param>
        /// <param name="presentationType"></param>
        public static void SaveToSidebar(string sidebarFolderPath, string presentationPath, string presentationFileName, PresentationType presentationType)
        {
            string FolderName = string.Empty;
            int EmptyFolderIndex = -1;

            //-- Identify the folder name to be used.
            switch (presentationType)
            {
                case PresentationType.Table:
                    FolderName = TABLE_FOLDER;
                    break;
                case PresentationType.Graph:
                    FolderName = GRAPH_FOLDER;
                    break;
                case PresentationType.Map:
                    FolderName = MAP_FOLDER;
                    break;
                case PresentationType.None:
                    break;
                default:
                    break;
            }

            //-- check for Empty folder
            EmptyFolderIndex = CheckForEmptyFolder(FolderName, sidebarFolderPath);

            if (EmptyFolderIndex == -1)
            {
                //-- Delete the fifth folder of the presentation, if none of them are empty.
                Directory.Delete(Path.Combine(sidebarFolderPath, FolderName + " 5"), true);

                //-- Rename the folders
                RenameFolder(FolderName, sidebarFolderPath);

                //-- Copy the presentation
                CopyPresentation(FolderName, 1, sidebarFolderPath, presentationPath, presentationFileName);
            }
            else
            {
                //-- Copy the presentation
                CopyPresentation(FolderName, EmptyFolderIndex, sidebarFolderPath, presentationPath, presentationFileName);
            }
        }

        /// <summary>
        /// Check the existing connection details with presentation database details
        /// </summary>
        /// <param name="workingDbConnection"></param>
        /// <param name="presentationPath"></param>
        /// <returns>False, if the working and presentation database connection details are different</returns>
        public static bool ValidateConnection(DIConnection workingDbConnection, string presentationPath, bool showExcel)
        {
            bool Retval = true;
            try
            {
                DIConnectionDetails PresentationDbDetails = new DIConnectionDetails();
                PresentationType PresentationType = GetPresentationType(presentationPath);
                string SerializedXML = GetSerializedPresentationText(presentationPath, PresentationType, showExcel);

                switch (PresentationType)
                {
                    case PresentationType.Table:
                        TablePresentation TablePresentation = TablePresentation.LoadFromSerializeText(SerializedXML);
                        PresentationDbDetails = TablePresentation.UserPreference.Database.SelectedConnectionDetail;
                        break;
                    case PresentationType.Graph:
                        GraphPresentation GraphPresentation = GraphPresentation.LoadFromSerializeText(SerializedXML);
                        PresentationDbDetails = GraphPresentation.TablePresentation.UserPreference.Database.SelectedConnectionDetail;
                        break;
                    case PresentationType.Map:
                        Map.Map MapPresentation = Map.Map.LoadFromSerializeText(SerializedXML);
                        PresentationDbDetails = MapPresentation.UserPreference.Database.SelectedConnectionDetail;
                        break;
                    case PresentationType.None:
                        break;
                    default:
                        break;
                }

                Retval = Presentation.ValidateConnection(workingDbConnection, PresentationDbDetails);

            }
            catch (Exception)
            {
                Retval = true;
            }
            return Retval;
        }

        /// <summary>
        /// Check the existing connection details with proposed database details
        /// </summary>
        /// <param name="workingDbConnection"></param>
        /// <param name="proposedDBConnectionDetail"></param>
        /// <returns>False, if the working and new proposed database connection details are different</returns>
        public static bool ValidateConnection(DIConnection workingDbConnection, DIConnectionDetails proposedDBConnectionDetail)
        {
            bool Retval = true;
            try
            {
                //-- ServerType
                if (workingDbConnection.ConnectionStringParameters.ServerType != proposedDBConnectionDetail.ServerType)
                {
                    Retval = false;
                }

                //-- Database name
				if (workingDbConnection.ConnectionStringParameters.DbName.Trim().ToLower() != proposedDBConnectionDetail.DbName.Trim().ToLower())
                {
                    Retval = false;
                }

                //-- user name
                if (workingDbConnection.ConnectionStringParameters.UserName.Trim() != proposedDBConnectionDetail.UserName.Trim())
                {
                    Retval = false;
                }

                //-- Password
                if (workingDbConnection.ConnectionStringParameters.Password.Trim() != proposedDBConnectionDetail.Password.Trim())
                {
                    Retval = false;
                }

                //-- Port number
                if (workingDbConnection.ConnectionStringParameters.PortNo.Trim() != proposedDBConnectionDetail.PortNo.Trim())
                {
                    Retval = false;
                }


                if (!Retval)
                {
                    //-- IF the existing connection is different from presentation database details
                    DIConnection TestConnection = new DIConnection(proposedDBConnectionDetail);
                    if (TestConnection == null)
                    {
                        Retval = true;
                    }
                }
            }
            catch (Exception)
            {
                Retval = true;
            }

            return Retval;
        }
        #endregion

        #endregion

        #region " -- Private -- "

        #region " -- Methods -- "

        /// <summary>
        /// Copy the presentation in their respective sidebar folder.
        /// </summary>
        /// <param name="folderPrefix"></param>
        /// <param name="folderIndex"></param>
        /// <param name="sidebarFolderPath"></param>
        /// <param name="presentationPath"></param>
        private static void CopyPresentation(string folderPrefix, int folderIndex, string sidebarFolderPath, string presentationPath, string outputFileName)
        {
            if (!Directory.Exists(Path.Combine(sidebarFolderPath, folderPrefix + " " + folderIndex.ToString())))
            {
                Directory.CreateDirectory(Path.Combine(sidebarFolderPath, folderPrefix + " " + folderIndex.ToString()));
            }

            string DestinationFolder = Path.Combine(sidebarFolderPath, folderPrefix + " " + folderIndex.ToString());

            //-- Clear files in folder
            foreach (string file in Directory.GetFiles(DestinationFolder))
            {
                try
                {
                    File.Delete(file);
                }
                catch 
                {
                    
                }
            }

            if (!string.IsNullOrEmpty(outputFileName))
            {
                File.Copy(presentationPath, Path.Combine(DestinationFolder, outputFileName));
            }
            else
            {
                File.Copy(presentationPath, Path.Combine(DestinationFolder, Path.GetFileName(presentationPath)));
            }
        }

        /// <summary>
        /// Rename the folder 
        /// </summary>
        /// <param name="folderPrefix"></param>
        /// <param name="sidebarFolderPath"></param>
        /// <remarks> Table 1 will be renamed as Table 2... </remarks>
        private static void RenameFolder(string folderPrefix, string sidebarFolderPath)
        {
            for (int FolderIndex = 4; FolderIndex >= 1; FolderIndex--)
            {
                //-- Rename the folder
                Directory.Move(Path.Combine(sidebarFolderPath, folderPrefix + " " + FolderIndex.ToString()), Path.Combine(sidebarFolderPath, folderPrefix + " " + (FolderIndex + 1).ToString()));
            }
        }

        /// <summary>
        /// Check for the empty folder.
        /// </summary>
        /// <param name="folderPrefix"></param>
        /// <param name="sidebarFolderPath"></param>
        /// <returns></returns>
        private static int CheckForEmptyFolder(string folderPrefix, string sidebarFolderPath)
        {
            int Retval = -1;
            try
            {
                for (int FolderIndex = 1; FolderIndex < 6; FolderIndex++)
                {
                    Retval = CheckForFile(folderPrefix, sidebarFolderPath, FolderIndex);
                    if (Retval > -1)
                    {
                        break;
                    }
                }
            }
            catch (Exception)
            {

            }
            return Retval;
        }

        /// <summary>
        /// Check for the file in the folder.
        /// </summary>
        /// <param name="folderPrefix"></param>
        /// <param name="sidebarFolderPath"></param>
        /// <param name="FolderIndex"></param>
        /// <returns></returns>
        private static int CheckForFile(string folderPrefix, string sidebarFolderPath, int FolderIndex)
        {
            int Retval = -1;
            try
            {
                if (!Directory.Exists(Path.Combine(sidebarFolderPath, folderPrefix + " " + FolderIndex.ToString())))
                {
                    Directory.CreateDirectory(Path.Combine(sidebarFolderPath, folderPrefix + " " + FolderIndex.ToString()));
                }
                System.IO.DirectoryInfo Dir = new DirectoryInfo(Path.Combine(sidebarFolderPath, folderPrefix + " " + FolderIndex.ToString()));
                FileInfo[] Files = Dir.GetFiles();
                if (Files.Length == 0)
                {
                    Retval = FolderIndex;
                }
            }
            catch (Exception)
            {
            }
            return Retval;
        }

        #endregion

        #endregion

    }
}
