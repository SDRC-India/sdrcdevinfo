using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace DevInfo.Lib.DI_LibBAL.Controls.DXCrossTabMappingBAL
{
    /// <summary>
    ///  Provides information of cross tab input file which can be of SPSS, SPO *.htm , etc file type .
    /// </summary>
    public class CrossTabInputFileInfo
    {
        #region "-- Private  --"

        #region "-- Variables --"


        #endregion

        #region "-- New/Dispose --"


        #endregion

        #region "-- Methods  --"

        /// <summary>
        /// Sets tables name 
        /// </summary>
        private void SetAllTablesName()
        {
            foreach (CrossTabTableInfo TableInfo in this.Tables)
            {
                TableInfo.SetAllDataTablesName();
            }
        }

        private void ImportMappingValues(CrossTabInputFileInfo srcInputFileInfo, bool importDefaultValues, bool importRowsMapping, bool importColsMapping, int startingTableIndex)
        {
            int TrgTableIndex;
            try
            {
                if (this._Tables != null && this._Tables.Count > 0 && srcInputFileInfo.Tables != null && srcInputFileInfo.Tables.Count > 0)
                {
                    // import mapping information from srcInputFileInfo only if table is exists
                    TrgTableIndex = startingTableIndex;

                    for (int SrcTableIndex = 0; TrgTableIndex < this._Tables.Count; TrgTableIndex++, SrcTableIndex++)
                    {
                        // check table exists in source input file
                        if (srcInputFileInfo.Tables.Count <= SrcTableIndex)
                        {
                            break;
                        }


                        if (srcInputFileInfo.Tables[TrgTableIndex] != null)
                        {
                            // import columns mapping by checking column header
                            if (importColsMapping)
                            {
                                this._Tables[TrgTableIndex].ImportColumnsMapping(srcInputFileInfo.Tables[SrcTableIndex]);
                            }

                            // import rows mapping by checking rows header
                            if (importRowsMapping)
                            {
                                this._Tables[TrgTableIndex].ImportRowsMapping(srcInputFileInfo.Tables[SrcTableIndex]);
                            }

                            // import default mapping 
                            if (importDefaultValues)
                            {
                                this._Tables[TrgTableIndex].ImportDefaultMapping(srcInputFileInfo.Tables[SrcTableIndex]);
                                this._Tables[TrgTableIndex].DecimalValue = srcInputFileInfo.Tables[SrcTableIndex].DefaultMapping.DefaultDecimalValue;
                            }


                            // import denominator table 
                            // only if src table caption is same as target table caption and columns count is same
                            string TrgTableCaption=this._Tables[TrgTableIndex].Caption.ToLower();
                            string SrcTableCaption=srcInputFileInfo.Tables[SrcTableIndex].Caption.ToLower();


                            if ( TrgTableIndex == SrcTableIndex )
                            {
                                if (this._Tables[TrgTableIndex].ColumnsHeaderTable.Columns.Count ==
srcInputFileInfo.Tables[SrcTableIndex].ColumnsHeaderTable.Columns.Count)
                                {
                                    this._Tables[TrgTableIndex].DenominatorTable = srcInputFileInfo.Tables[SrcTableIndex].DenominatorTable;
                                }

                            }

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }





        #endregion

        #endregion

        #region "-- Public --"

        #region "-- Variables/Properties --"

        private string _ActucalFileNameWithoutExtension = string.Empty;
        /// <summary>
        /// Gets or sets Actucal filename without extension. This name will be used while creating dataentry spreadsheets
        /// </summary>
        public string ActucalFileNameWithoutExtension
        {
            get { return this._ActucalFileNameWithoutExtension; }
            set { this._ActucalFileNameWithoutExtension = value; }
        }

        private string _ActucalFileNameWPath = string.Empty;
        /// <summary>
        /// Gets or sets Actucal filename with path. 
        /// </summary>
        public string ActucalFileNameWPath
        {
            get { return this._ActucalFileNameWPath; }
            set { this._ActucalFileNameWPath = value; }
        }

        private string _FileNameWExtension = string.Empty;
        /// <summary>
        /// Gets  FileName with extension
        /// </summary>
        public string FileNameWExtension
        {
            get { return this._FileNameWExtension; }
        }

        private string _FileNameWPath = string.Empty;
        /// <summary>
        /// Gets filename with path
        /// </summary>
        public string FileNameWPath
        {
            get { return this._FileNameWPath; }
        }

        private List<CrossTabTableInfo> _Tables = new List<CrossTabTableInfo>();
        /// <summary>
        /// Gets or sets CrossTab tables
        /// </summary>
        public List<CrossTabTableInfo> Tables
        {
            get { return this._Tables; }
            set { this._Tables = value; }
        }


        #endregion

        #region "-- New/Dispose --"

        /// <summary>
        /// constructor
        /// </summary>
        public CrossTabInputFileInfo()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fileNameWPath"></param>
        public CrossTabInputFileInfo(string fileNameWPath)
        {
            this._FileNameWPath = fileNameWPath;
            this._FileNameWExtension = Path.GetFileName(this._FileNameWPath);
        }

        #endregion

        #region "-- Methods  --"

        /// <summary>
        /// Exports mappingValues 
        /// </summary>
        public void ExportMappingValues(string fileNameWPath)
        {
            XmlSerializer SerializedObject;
            StreamWriter Writer;
            try
            {
                this.SetAllTablesName();

                SerializedObject = new XmlSerializer(typeof(CrossTabInputFileInfo));
                Writer = new StreamWriter(fileNameWPath);
                SerializedObject.Serialize(Writer, this);
                Writer.Close();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        /// <summary>
        /// Imports mappingValues  
        /// </summary>
        /// <param name="fileNameWPath"></param>
        /// <param name="importDefaultValues"></param>
        /// <param name="importRowsMapping"></param>
        /// <param name="importColsMapping"></param>
        public void ImportFrmFile(string fileNameWPath, bool importDefaultValues, bool importRowsMapping, bool importColsMapping, int startingTableIndex)
        {
            XmlSerializer SerializedObject;
            StreamReader Reader;
            CrossTabInputFileInfo SrcInputFileInfo = null;
            try
            {
                if (File.Exists(fileNameWPath))
                {
                    SerializedObject = new XmlSerializer(typeof(CrossTabInputFileInfo));
                    Reader = new StreamReader(fileNameWPath);
                    SrcInputFileInfo = (CrossTabInputFileInfo)SerializedObject.Deserialize(Reader);
                    Reader.Close();

                    if (SrcInputFileInfo != null)
                    {
                        this.ImportMappingValues(SrcInputFileInfo, importDefaultValues, importRowsMapping, importColsMapping,startingTableIndex);
                    }
                }
            }
            catch (Exception)
            {
                SrcInputFileInfo = DXSFileConveter.GetInputFileInfo(fileNameWPath);
                if (SrcInputFileInfo != null)
                {
                    this.ImportMappingValues(SrcInputFileInfo, importDefaultValues, importRowsMapping, importColsMapping, startingTableIndex);
                }
            }
        }



        #endregion

        #endregion

    }

}
