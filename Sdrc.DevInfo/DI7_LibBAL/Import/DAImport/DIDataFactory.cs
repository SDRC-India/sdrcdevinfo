using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibBAL.Import.DAImport.DES;
using DevInfo.Lib.DI_LibBAL.Import.DAImport.Database;
using DevInfo.Lib.DI_LibBAL.Import.DAImport.Common;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.UserSelection;


namespace DevInfo.Lib.DI_LibBAL.Import.DAImport
{
    /// <summary>
    /// Helps to create instance of DIData on the basis of source file type
    /// </summary>
    public static class DIDataFactory
    {
        #region "-- Public --"

        #region "-- Methods --"

        /// <summary>
        /// Returns the instance of DIData based on the importFile type
        /// </summary>
        /// <param name="importFileType"></param>
        /// <param name="sourceFileNamesWPath">List of source files with path</param>
        /// <param name="targetFileNamesWPath">Target file name with path</param>
        /// <param name="tempFolderPath">Temproray folder path</param>
        /// <param name="htmlLogOutputPath">Spreadsheet folder path</param>
        /// <returns></returns>
        public static DIData CreateInstance(DIImportFileType importFileType, List<string> sourceFileNamesWPath, string targetFileNamesWPath, string tempFolderPath, string htmlLogOutputPath)
        {
            DIData RetVal = null;
            switch (importFileType)
            {
                case DIImportFileType.DataEntrySpreasheet:
                    RetVal = new DataEntrySpreadsheets(sourceFileNamesWPath, targetFileNamesWPath, tempFolderPath, htmlLogOutputPath);
                    break;
                case DIImportFileType.Database:
                    RetVal = new Database.Database(sourceFileNamesWPath, targetFileNamesWPath, tempFolderPath, htmlLogOutputPath);
                    break;
                case DIImportFileType.Template:
                    break;
                case DIImportFileType.SDMXXml:
                    RetVal = new SDMX.SDMXXml(sourceFileNamesWPath, targetFileNamesWPath, tempFolderPath, htmlLogOutputPath);
                    break;
                case DIImportFileType.CSV:
                    RetVal = new CSV.CSVImporter(sourceFileNamesWPath, targetFileNamesWPath, tempFolderPath, htmlLogOutputPath);
                    break;
                default:
                    break;
            }

            if (RetVal != null)
            {
                RetVal.ImportFileType = importFileType;
            }
            return RetVal;
        }

        public static DIData CreateInstance(DIImportFileType importFileType, List<string> sourceFileNamesWPath,
            string tempFolderPath, DIConnection dbConnection, DIQueries dbQueries)
        {
            DIData RetVal = null;
            switch (importFileType)
            {
                case DIImportFileType.DataEntrySpreasheet:
                    break;
                case DIImportFileType.Database:
                    break;
                case DIImportFileType.Template:
                    break;
                case DIImportFileType.SDMXXml:
                    RetVal = new SDMX.SDMXXml(sourceFileNamesWPath, tempFolderPath, dbConnection, dbQueries);
                    break;
                default:
                    break;
            }

            if (RetVal != null)
            {
                RetVal.ImportFileType = importFileType;
            }
            return RetVal;
        }

        public static DIData CreateInstance(DIImportFileType importFileType, List<string> sourceFileNamesWPath,
                  string tempFolderPath, DIConnection dbConnection, string datasetPrefix, string languageCode)
        {
            DIData RetVal = null;
            switch (importFileType)
            {
                case DIImportFileType.DataEntrySpreasheet:
                    break;
                case DIImportFileType.Database:
                    break;
                case DIImportFileType.Template:
                    break;
                case DIImportFileType.SDMXXml:
                    RetVal = new SDMX.SDMXXml(sourceFileNamesWPath, tempFolderPath, dbConnection, datasetPrefix, languageCode);
                    break;
                default:
                    break;
            }

            if (RetVal != null)
            {
                RetVal.ImportFileType = importFileType;
            }
            return RetVal;
        }

        public static DIData CreateInstance(DIImportFileType importFileType, string sourceFileNamesWPath,
                  string tempFolderPath, DIConnection dbConnection, DIQueries dbQueries)
        {
            DIData RetVal = null;
            switch (importFileType)
            {
                case DIImportFileType.DataEntrySpreasheet:
                    RetVal = new DataEntrySpreadsheets(sourceFileNamesWPath, tempFolderPath, dbConnection, dbQueries);
                    break;
                case DIImportFileType.Database:
                    break;
                case DIImportFileType.Template:
                    break;
                case DIImportFileType.SDMXXml:
                    break;
                default:
                    break;
            }

            if (RetVal != null)
            {
                RetVal.ImportFileType = importFileType;
            }
            return RetVal;
        }


        public static DIData CreateInstance(DIImportFileType importFileType,UserSelection userSelections, DIConnection sourceDBConnection, DIQueries sourceDBQueries, DIConnection targetDBConnection, DIQueries targetDBQueries)
        {
            DIData RetVal = null;
            switch (importFileType)
            {
                case DIImportFileType.DataEntrySpreasheet:
                    break;
                case DIImportFileType.Database:
                    RetVal = new DAImport.Database.Database(userSelections, sourceDBConnection, sourceDBQueries, targetDBConnection, targetDBQueries);
                    break;
                case DIImportFileType.Template:
                    break;
                case DIImportFileType.SDMXXml:
                    break;
                default:
                    break;
            }

            if (RetVal != null)
            {
                RetVal.ImportFileType = importFileType;
            }
            return RetVal;
        }

        #endregion

        #endregion

    }
}
