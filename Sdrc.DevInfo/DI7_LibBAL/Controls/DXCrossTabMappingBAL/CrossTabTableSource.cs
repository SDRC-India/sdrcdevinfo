using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;
using DevInfo.Lib.DI_LibBAL.Controls.MappingControlsBAL;
using System.Xml.Serialization;
using System.IO;
using DevInfo.Lib.DI_LibBAL.Utility.MRU;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibBAL.LogFiles;
using DevInfo.Lib.DI_LibBAL.DES;


namespace DevInfo.Lib.DI_LibBAL.Controls.DXCrossTabMappingBAL
{
    /// <summary>
    /// Provides CrossTabTableControl source which helps in managing input files/tables.
    /// </summary>
    public class CrossTabTableSource
    {
        #region "-- Private  --"

        #region "-- Variables --"


        #endregion

        #region "-- New/Dispose --"


        #endregion

        #region "-- Methods  --"



        #endregion

        #endregion

        #region "-- Public --"

        #region "-- Variables/Properties --"

        //////private Dictionary<string,CrossTabInputFileInfo> _InputFiles=new Dictionary<string,CrossTabInputFileInfo>();
        ////// <summary>
        ////// Gets or sets input files. Key is filename without path and value is instance of CrossTabInputFileInfo
        ////// </summary>
        //////public Dictionary<string,CrossTabInputFileInfo> InputFiles
        //////{
        //////    get { return this._InputFiles; }
        //////    set { this._InputFiles = value; }
        //////}


        private List<CrossTabInputFileInfo> _InputFiles = new List<CrossTabInputFileInfo>();
        /// <summary>
        /// Gets or sets input files. 
        /// </summary>
        public List<CrossTabInputFileInfo> InputFiles
        {
            get { return this._InputFiles; }
            set { this._InputFiles = value; }
        }

        private Color _DenominatorColumnColor = Color.WhiteSmoke;
        /// <summary>
        /// Gets or sets denominator column's color
        /// </summary>
        public Color DenominatorColumnColor
        {
            get { return this._DenominatorColumnColor; }
            set { this._DenominatorColumnColor = value; }
        }

        private string _DXApplicationName = string.Empty;
        /// <summary>
        /// Gets or sets DXApplication name 
        /// </summary>
        public string DXApplicationName
        {
            get { return this._DXApplicationName; }
            set { this._DXApplicationName = value; }
        }

        private string _OutputFileNameForLogFile = string.Empty;
        /// <summary>
        /// Gets or sets output file name for log file.
        /// </summary>
        public string OutputFileNameForLogFile
        {
            get { return this._OutputFileNameForLogFile; }
            set { this._OutputFileNameForLogFile = value; }
        }
        

        private string _ZeroMaskValue=string.Empty;
        /// <summary>
        /// Gets or sets ZeroMaskValue. May be empty or null. Zero  will be inserted if the given zeroMaskValue is found while inserting or updating data rows.
        /// </summary>
        public string ZeroMaskValue
        {
            get { return this._ZeroMaskValue; }
            set { this._ZeroMaskValue = value; }
        }


        private string _MissingValueCharacter = ".";
        /// <summary>
        /// Gets or sets MissingValueCharacter. May be empty or null. If datavalue is equal to the given MissingValueCharacter then it will not be imported into data table.Default value is "."
        /// </summary>
        public string MissingValueCharacter
        {
            get { return this._MissingValueCharacter; }
            set { this._MissingValueCharacter = value; }
        }

        #endregion

        #region "-- New/Dispose --"

        /// <summary>
        /// Constructor
        /// </summary>
        public CrossTabTableSource()
        {
        }

        #endregion

        #region "-- Methods  --"

        /// <summary>
        /// Imports input files into database and returns log file name with path
        /// </summary>
        /// 
        public string ImportFilesIntoDB(Database dbDatabase)
        {
            string RetVal = string.Empty;

            CrossTabLogFile LogFile = new CrossTabLogFile();
            List<string> InputFileNames = new List<string>();
            int TableIndex = 1;
            try
            {
                RetVal = DICommon.DefaultFolder.DefaultSpreadSheetsFolder + LogFile.GenerateLogFileName(this._DXApplicationName);

                // Get fileNames            
                foreach (CrossTabInputFileInfo InputFile1 in this._InputFiles)
                {
                    if (string.IsNullOrEmpty(InputFile1.ActucalFileNameWPath))
                    {
                        InputFileNames.Add(InputFile1.FileNameWPath);
                    }
                    else
                    {
                        InputFileNames.Add(InputFile1.ActucalFileNameWPath);
                    }
                }

                // Start log file
                LogFile.Start(RetVal, this._DXApplicationName, InputFileNames, this._OutputFileNameForLogFile, dbDatabase.DBConnection, dbDatabase.DBQueries);

                foreach (CrossTabInputFileInfo InputFileInfo in this._InputFiles)
                {
                    TableIndex = 1;

                    // Write file name in log file
                    LogFile.StartInputFileLog(InputFileInfo.ActucalFileNameWithoutExtension);

                    foreach (CrossTabTableInfo TableInfo in InputFileInfo.Tables)
                    {

                        if (string.IsNullOrEmpty(TableInfo.Caption))
                        {
                            TableInfo.Caption = TableIndex.ToString();
                        }

                        TableInfo.ZeroMaskValue = this._ZeroMaskValue;
                        TableInfo.MissingValueCharacter = this._MissingValueCharacter;
                        TableInfo.ImportTableValuesIntoDB(dbDatabase, LogFile);

                        TableIndex++;
                    }

                    //end input file log
                    LogFile.EndInputFileLog();
                }

                // close log file
                LogFile.Close();

                //update I,U & S NIDs in data table            
                try
                {
                    dbDatabase.DBConnection.ExecuteNonQuery(DI_LibDAL.Queries.Data.Update.UpdateIndicatorUnitSubgroupVAlNids(dbDatabase.DBQueries.TablesName));
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ex.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
            return RetVal;
        }

        /// <summary>
        /// Imports input files into data entry spreadsheet and returns log file name with path
        /// </summary>

        public void ImportFilesIntoDES(Database dbDatabase)
        {

            List<string> InputFileNames = new List<string>();
            int TableIndex = 1;

            DESGenerator DESFile;
            DataEntrySpreadsheet DESSheet;
            string DESFileNameWPath = string.Empty;

            try
            {

                foreach (CrossTabInputFileInfo InputFileInfo in this._InputFiles)
                {
                    TableIndex = 1;

                    // create DES File instance
                    DESFile = new DESGenerator();

                    foreach (CrossTabTableInfo TableInfo in InputFileInfo.Tables)
                    {

                        if (string.IsNullOrEmpty(TableInfo.Caption))
                        {
                            TableInfo.Caption = TableIndex.ToString();
                        }

                        // Get sheet and insert it into DES File
                        TableInfo.ZeroMaskValue = this._ZeroMaskValue;
                        TableInfo.MissingValueCharacter = this._MissingValueCharacter;
                        DESSheet = TableInfo.GetTableValuesIntoDES(dbDatabase);

                        if (DESSheet != null)
                        {
                            DESFile.DESSheets.Add(DESSheet);
                        }

                        TableIndex++;
                    }

                    // create DES File
                    if (string.IsNullOrEmpty(InputFileInfo.ActucalFileNameWithoutExtension))
                    {
                        DESFileNameWPath = DICommon.DefaultFolder.DefaultSpreadSheetsFolder + "DATA_" + Path.GetFileNameWithoutExtension(InputFileInfo.FileNameWExtension) + DICommon.FileExtension.Excel;
                    }
                    else
                    {
                        DESFileNameWPath = DICommon.DefaultFolder.DefaultSpreadSheetsFolder + "DATA_" + Path.GetFileNameWithoutExtension( InputFileInfo.ActucalFileNameWithoutExtension )+ DICommon.FileExtension.Excel;
                    }

                    DESFile.GenerateDESFile(DESFileNameWPath);
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }


        #endregion

        #endregion

    }




}
