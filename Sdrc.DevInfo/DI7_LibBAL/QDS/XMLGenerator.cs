using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.IO;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DI7_XMLGenerator_LibBAL.Classes;
using DI7_XMLGenerator_LibBAL.BAL;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL.QDS.DI7CacheNSP;
using DevInfo.Lib.DI_LibBAL.QDS.UI.Databases;

namespace DevInfo.Lib.DI_LibBAL.QDS
{
    public class XMLGenerator
    {
        #region "-- Private --"

        #region "-- variables --"

        DIConnection DBConnection = null;
        DIQueries DBQueries = null;
        string OutputFolderPath = string.Empty;
        string DataBasePath = string.Empty;
        
        #endregion
        
        #endregion

        #region "-- Public --"

        #region "--  New/Dispose --"

        public XMLGenerator(string outputFolderPath, DIConnection dbConnection, DIQueries dbQueries)
        {
            this.OutputFolderPath = outputFolderPath;
            this.DBConnection = dbConnection;
            this.DBQueries = dbQueries;
        }

        public XMLGenerator(string outputFolderPath, string dbPath)
        {
            this.OutputFolderPath = outputFolderPath;
            this.DataBasePath = dbPath;
            this.DBConnection = new DIConnection(DIServerType.MsAccess, "", "", dbPath, "", "");
        }

        #endregion

        #region "-- Events --"

        public event ProgressChangedDelegate ProgressChangedEvent;
        public event EventHandler DisplayProgressFormEvent;

        public void RaiseProgressChangedEvent(int value, string languageCode, string folderName, bool savingDatabase)
        {
            if (this.ProgressChangedEvent != null)
            {
                // todo: language handling for string
                if (!string.IsNullOrEmpty(languageCode))
                {
                    languageCode = "Generating xml files for:" + languageCode;
                }

                if (!string.IsNullOrEmpty(folderName))
                {
                    folderName = "XML Files:" + folderName;
                }
                if (savingDatabase)
                {
                    languageCode = "Saving database details...";
                }

                this.ProgressChangedEvent(value, "Optimizing database", languageCode, folderName, string.Empty);
            }
        }

        public void RaiseDisplayProgressFormEvent()
        {
            if (this.DisplayProgressFormEvent != null)
            {
                this.DisplayProgressFormEvent(this, null);
            }
        }

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Generate all xml files of mdb database
        /// </summary>
        /// <param name="dataFolderName"></param>
        /// <param name="areaOrderBy"></param>
        /// <param name="quickSelectionType"></param>
        /// <returns></returns>
        public bool GenerateDefaultXmlFiles(string dataFolderName, string areaOrderBy, string quickSelectionType)
        {
            bool RetVal = false;
            string LanguageBasedOutputFolder = string.Empty;
            SDMXMLGenerator SDMXMLFileGenerator = null;
            AreaQuickSelectionType QuickSelectionType = AreaQuickSelectionType.Immediate;
            int ProgressCount = 8;

            try
            {
                //-- raise event to display progress form
                this.RaiseDisplayProgressFormEvent();

                // increment progress bar value
                this.RaiseProgressChangedEvent(ProgressCount++, string.Empty, string.Empty, false);

                SDMXMLFileGenerator = new SDMXMLGenerator(this.DBConnection.ConnectionStringParameters.GetConnectionString(), Convert.ToInt32(this.DBConnection.ConnectionStringParameters.ServerType));

                LanguageBasedOutputFolder = Path.Combine(this.OutputFolderPath, dataFolderName);

                // increment progress bar value
                this.RaiseProgressChangedEvent(ProgressCount++, string.Empty, "Area", false);

                #region "-- Generate Area -- "

                switch (areaOrderBy)
                {
                    case "AreaName":
                        SDMXMLFileGenerator.SortByAreaName = true;
                        break;
                    case "AreaId":
                        SDMXMLFileGenerator.SortByAreaName = false;
                        break;
                    default:
                        break;
                }

                SDMXMLFileGenerator.AreaLevel = "_";
                SDMXMLFileGenerator.GenerateXmlCodeList(XMLCodeListType.DI7Area, LanguageBasedOutputFolder, QDSConstants.FolderName.Codelists.Area, true);

                // increment progress bar value
                this.RaiseProgressChangedEvent(ProgressCount++, string.Empty, "Area level", false);

                //-- Generate Area level            
                SDMXMLFileGenerator.GenerateXmlCodeList(XMLCodeListType.Arealevel, LanguageBasedOutputFolder, QDSConstants.FolderName.Codelists.Area, true);
                #endregion

                // increment progress bar value
                this.RaiseProgressChangedEvent(ProgressCount++, string.Empty, "Quick Search", false);

                #region "-- Quick Search -- "

                switch (quickSelectionType)
                {
                    case "All":
                        QuickSelectionType = AreaQuickSelectionType.All;
                        break;
                    case "Immediate":
                        QuickSelectionType = AreaQuickSelectionType.Immediate;
                        break;
                    case "None":
                        QuickSelectionType = AreaQuickSelectionType.None;
                        break;
                    default:
                        break;
                }

                SDMXMLFileGenerator.SelectedAreaQuickSelectionType = QuickSelectionType;

                //-- Generate Area search            
                SDMXMLFileGenerator.GenerateXmlCodeList(XMLCodeListType.AreaSearch, LanguageBasedOutputFolder, QDSConstants.FolderName.Codelists.Area, true);

                //-- Generate Quick search                   
                SDMXMLFileGenerator.GenerateXmlCodeList(XMLCodeListType.QuickSearch, LanguageBasedOutputFolder, QDSConstants.FolderName.Codelists.Area, true);


                #endregion

                // increment progress bar value
                this.RaiseProgressChangedEvent(ProgressCount++, string.Empty, "Footnote", false);


                //-- Generate Footnotes            
                SDMXMLFileGenerator.GenerateXmlCodeList(XMLCodeListType.Footnotes, LanguageBasedOutputFolder, QDSConstants.FolderName.Codelists.Footnotes, true);

                // increment progress bar value
                this.RaiseProgressChangedEvent(ProgressCount++, string.Empty, "IC", false);

                //-- Generate ic xml files e.g avl_ic_types.xml,sc.xml,sc_l1.xml,sc_l1_icNId.xml and for other ictypes etc.
                SDMXMLFileGenerator.GenerateXmlCodeList(XMLCodeListType.IC, LanguageBasedOutputFolder, QDSConstants.FolderName.Codelists.IC, true);

                // increment progress bar value
                this.RaiseProgressChangedEvent(ProgressCount++, string.Empty, "IC and IUS", false);

                //-- Generate ic-ius files iu_ICNId.xml            
               SDMXMLFileGenerator.GenerateXmlCodeList(XMLCodeListType.ic_ius, LanguageBasedOutputFolder, QDSConstants.FolderName.Codelists.IC_IUS, true);

               // increment progress bar value
               this.RaiseProgressChangedEvent(ProgressCount++, string.Empty, "IUS", false);

                //-- Generate IUS codelist e.g. _iu_.xml, ius_IndicatorNID_UnitNID.xml and IUSSearch xml files            
                SDMXMLFileGenerator.GenerateXmlCodeList(XMLCodeListType.ius, LanguageBasedOutputFolder, QDSConstants.FolderName.Codelists.IUS, true);

                // increment progress bar value
                this.RaiseProgressChangedEvent(ProgressCount++, string.Empty, "Metadata", false);

                //-- Generate Metadata            
                SDMXMLFileGenerator.GenerateXmlCodeList(XMLCodeListType.Metadata, LanguageBasedOutputFolder, QDSConstants.FolderName.Codelists.Metadata, true)
;
                // increment progress bar value
                this.RaiseProgressChangedEvent(ProgressCount++, string.Empty, "Timeperiod", false);
                
                //-- Generate Time Period            
                SDMXMLFileGenerator.GenerateXmlCodeList(XMLCodeListType.tp, LanguageBasedOutputFolder, QDSConstants.FolderName.Codelists.TP, true);
                                

                RetVal = true;
            }
            catch (Exception)
            {
                throw;
            }

            return RetVal;
        }        

        #endregion

        #endregion
    }
}
