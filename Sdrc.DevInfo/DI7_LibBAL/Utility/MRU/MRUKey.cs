using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.Utility.MRU
{

    /// <summary>
    /// Specifies the type of most recent values.
    /// </summary>
    public enum MRUKey
    {
        /// <summary>
        /// For DevInfo5.0 database file selection
        /// </summary>
        MRU_DATABASES = 0,        

        /// <summary>
        /// For template file selection
        /// </summary>
        MRU_TEMPLATES = 1,

        /// <summary>
        /// For spreadsheet(excel) file selection
        /// </summary>
        MRU_SPREADSHEETS = 2,

        /// <summary>
        /// For DevInfo4.0 database file selection
        /// </summary>
        MRU_DI4_DATABASES = 3,

        /// <summary>
        /// For language file selection
        /// </summary>
        MRU_LANGUAGE = 4,

        /// <summary>
        /// For report types file selection
        /// </summary>
        MRU_REPORTS = 5,

        /// <summary>
        /// For SPSS file selection
        /// </summary>
        MRU_SPS = 6,

        /// <summary>
        /// For SPO file selection
        /// </summary>
        MRU_SPO = 7,

        /// <summary>
        /// For DX Language file selection
        /// </summary>
        MRU_DX_LANGUAGE = 8,

        /// <summary>
        /// For WDI file selection
        /// </summary>
        MRU_WDI = 9,

        /// <summary>
        /// For DX XLS Data Capture file selection
        /// </summary>
        MRU_DX_FREE_FORMAT = 10,


        /// <summary>
        /// For DX SMS Data Capture file selection
        /// </summary>
        MRU_DX_SMS = 11,

        /// <summary>
        /// For DX Desktop Data Capture file selection
        /// </summary>
        MRU_DX_DESKTOP_DATACAPTURE = 12,

        /// <summary>
        /// For Dx App Form Generator file selection
        /// </summary>
        MRU_PDA_FORMAT = 13,
        /// <summary>
        /// For DX STATA File Selection
        /// </summary>
        MRU_DX_STATA = 14,

        /// <summary>
        /// For Advance gallery
        /// </summary>
        MRU_PRESENTATIONS = 15,
        /// <summary>
        /// For Metadata xml files
        /// </summary>
        MRU_METADATAXML = 16,

        /// <summary>
        /// For metadata rtf files
        /// </summary>
        MRU_MetaDataRTF = 17,

        /// <summary>
        ///For Map Folder
        /// </summary>
        MRU_MAPS = 18,

        /// <summary>
        /// For CSPRO files
        /// </summary>
        MRU_CSPRO = 19,

        /// <summary>
        /// For HTML files
        /// </summary>
        MRU_HTML = 20,

        /// <summary>
        /// For xml files
        /// </summary>
        MRU_IMPORT_COMMENTS = 21,

        /// <summary>
        /// For *.cris files
        /// </summary>
        MRU_CRIS = 22,

        /// <summary>
        /// For *.smcl files
        /// </summary>
        MRU_STATA_SMCL = 23,

        /// <summary>
        /// For *.xml files
        /// </summary>
        MRU_STATA_DAT = 24,

        /// <summary>
        /// For *.xls files
        /// </summary>
        MRU_SAS_XLS = 25,

        /// <summary>
        /// For .xls & .xlsx files
        /// </summary>
        MRU_DIPROFILE = 26,

        /// <summary>
        /// For .csv files
        /// </summary>
        MRU_IMPORT_CSV_CENSUS = 27,

        /// <summary>
        /// For .tpl files
        /// </summary>
        MRU_NEW_TEMPLATES = 28,

        /// <summary>
        /// For .* files
        /// </summary>
        MRU_DIDataFiles = 29,

        /// <summary>
        /// SDMX web service url
        /// </summary>
        MRU_SDMXWebService = 30,

        /// <summary>
        /// For MS access accdb database
        /// </summary>
        MRU_ACCDB_DATABASES = 31
    }
}
