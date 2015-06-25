using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibBAL.LogFiles;
using System.IO;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;

namespace DevInfo.Lib.DI_LibBAL.Converter.Database
{
    

    public class DBConverterLog
    {
        private string InputFileNameWPath = string.Empty;
        private string LogFileNameWPath = string.Empty;
        private DXLogFile LogFile;
        private bool ShowLogFile = false;
        private DIConnection DBConnection = null;
        private DIQueries DBQueries = null;

        public DBConverterLog(string inputFileNameWPath, DIConnection dbConnection,DIQueries dbQueries)
        {
            this.InputFileNameWPath = inputFileNameWPath;
            this.DBConnection = dbConnection;
            this.DBQueries = dbQueries;
        }


        /// <summary>
        /// Generates Log file
        /// </summary>
        /// <param name="showLogFile"></param>
        /// <param name="inputFileNameWPath"></param>
        public void GenerateLogFile(bool showLogFile)
        {

            this.GenerateLogFile(showLogFile,this.InputFileNameWPath);
        }


        /// <summary>
        /// Generates Log file
        /// </summary>
        /// <param name="showLogFile"></param>
        /// <param name="inputFileNameWPath"></param>
        public void GenerateLogFile(bool showLogFile,string sourceFileNameWPath)
        {

            List<string> InputFiles = new List<string>();
            string LogFileNameWPath = string.Empty;
            string CurrentDateTime = string.Empty;
            string FileName = string.Empty;

            try
            {
                this.LogFile = new DXLogFile();
                this.ShowLogFile = showLogFile;

                if (!string.IsNullOrEmpty(sourceFileNameWPath))
                {
                    CurrentDateTime = DateTime.Now.ToString().Replace("/", "");
                    CurrentDateTime = CurrentDateTime.Replace("-", "");
                    CurrentDateTime = CurrentDateTime.Replace(":", "").Trim();

                    FileName = LogFile.GenerateLogFileName("Conversion_LogFile_" + CurrentDateTime);
                    LogFile.ShowTime = false;
                                        
                    InputFiles.Add(sourceFileNameWPath);
                    LogFileNameWPath = Path.Combine(Path.GetDirectoryName(this.InputFileNameWPath), FileName);
                    this.LogFileNameWPath = LogFileNameWPath;

                    LogFile.Start(LogFileNameWPath, "Database conversion log file", InputFiles, this.InputFileNameWPath, DBConnection, DBQueries);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void CloseLogFile()
        {
            if (this.LogFile != null)
            {
                this.LogFile.Close();

                if (this.ShowLogFile && File.Exists(this.LogFileNameWPath))
                {
                    System.Diagnostics.Process.Start(this.LogFileNameWPath);
                }
            }
        }


        public void AddNewSubgroupInfoIntoLog(SortedDictionary<string, string> subgroupsAddedWNewSubgorup)
        {
            // "New subgrop added for:"
            if (subgroupsAddedWNewSubgorup.Count > 0)
            {
                DataTable Table = new DataTable();
                Table.Columns.Add("Subgroup");
                Table.Columns.Add("Subgroup dimension value");
                DataRow Row;

                foreach (string key in subgroupsAddedWNewSubgorup.Keys)
                {
                    Row = Table.NewRow();
                    Row[0] = key;
                    Row[1] = subgroupsAddedWNewSubgorup[key].ToString();
                    Table.Rows.Add(Row);
                }

                this.LogFile.AddDataTable(Table, "New subgrop added for:");
            }
        }

        public void AddMismatchSubgroupInfoIntoLog(List<string> mismatchSubgroups)
        {
            if (mismatchSubgroups.Count > 0)
            {
                // "Mismatch subgrop:"
                DataTable Table = new DataTable();
                DataRow Row;
                
                Table.Columns.Add("Subgroups");
                
                foreach (string NewSubgroup in mismatchSubgroups)
                {
                    Row = Table.NewRow();
                    Row[0] = NewSubgroup;
                    Table.Rows.Add(Row);
                }

                this.LogFile.AddDataTable(Table, "Subgroups created under others :");
            }
        }
        
        public void AddSubgroupInfoWhereTotalIsAddedIntoLog(List<string> subgroupsAddedWithTotal)
        {
            if (subgroupsAddedWithTotal.Count > 0)
            {
                // "Subgrop where relationship with total is added:"
                DataTable Table = new DataTable();
                DataRow Row;

                Table.Columns.Add("Subgroup");

                foreach (string SG in subgroupsAddedWithTotal)
                {
                    Row = Table.NewRow();
                    Row[0] = SG;
                    Table.Rows.Add(Row);
                }


                this.LogFile.AddDataTable(Table, "Subgrop where relationship with total is added:");
            }
        }

        public void AddWrongMetadataElementsInfo(SortedDictionary<MetadataElementType, List<string>> wrongMetadataElementList)
        {            
            this.AddWrongMetadataElement(wrongMetadataElementList, MetadataElementType.Area);
            this.AddWrongMetadataElement(wrongMetadataElementList, MetadataElementType.Indicator);
            this.AddWrongMetadataElement(wrongMetadataElementList, MetadataElementType.Source);
        }

        private void AddWrongMetadataElement(SortedDictionary<MetadataElementType, List<string>> wrongMetadataElementList, MetadataElementType metadataType)
        {
            DataTable Table = new DataTable();
            DataRow Row;

            Table.Columns.Add(metadataType.ToString() + " Name");

            // add indicators
            if (wrongMetadataElementList.ContainsKey(metadataType))
            {
                foreach (string ElementName in wrongMetadataElementList[metadataType])
                {
                    Row = Table.NewRow();
                    Row[0] = ElementName;
                    Table.Rows.Add(Row);
                }

                
                this.LogFile.AddDataTable(Table, metadataType.ToString() + "s where metadata exist in a wrong format:");
            }
        }

        public void AddMissingMetadataCategoryTables(List<string> missingMetadataCategoryTables)
        {
            DataTable Table = new DataTable();
            DataRow Row;

            Table.Columns.Add("Metdata Category Table");

            if (missingMetadataCategoryTables.Count > 0)
            {                
                
                foreach (string tbl in missingMetadataCategoryTables)
                {
                    Row = Table.NewRow();
                    Row[0] = tbl;
                    Table.Rows.Add(Row);
                }

                this.LogFile.AddDataTable(Table, "Missing metadata category tables :");
            }
        }
    }

}
