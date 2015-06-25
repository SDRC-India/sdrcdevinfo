using System;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Microsoft.VisualBasic;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using DevInfo.Lib.DI_LibBAL.CommonDelegates;
using System.Diagnostics;

//******************Copy Right Notice*****************************
//Copyright(2007) <Company Name>.. This material is ‘proprietary to ‘<Company Name> and not part of this should be reproduced or reused ‘in any form or disclosed to third parties without the explicit written ‘Authorization of <Company Name>.         
//*******************************************************************
// Program Name:    RangeCheckReportGenerator        
// Developed By: DG9
// Creation date:    
// Program Comments: Brief description of the purpose of the program‘ being written 
// ********************************************************************
//********************Change history*********************************
// No. Mod: Date       Mod: By        Change Description      
// 1. 2009-03-17        DG9            Bug fixed for DataTable DataType
//  *****************************************************************


namespace DevInfo.Lib.DI_LibBAL.DA.Reports.RangeCheckReport
{
   public  class RangeCheckReportGenerator
    {
        #region -- Private --

    #region "-- Enum --"

    private enum RangeType
    { 
        DataBelow_MinVal,
        DataBelow_MaxVal,
        DataAbove_MinVal,
        DataAbove_MaxVal
    }

    #endregion

        #region Varibles and Properties

        private string ReportDestinationFilePath =string.Empty;
            private DIConnection  DBConnection=null;
            private DIQueries DBQueries= null;
            private Dictionary<DRCColumnsHeader, string> ColumnsHeader = new Dictionary<DRCColumnsHeader,string>();           

        #endregion

        #region -- New/ Dispose

          private RangeCheckReportGenerator()
           {
              //do nothing
           } 

        #endregion

        #region -- Methods --

       /// <summary>
       /// Connect To Database
       /// </summary>
            private void ConnectToDataBase()
            {
                string DatasetPrefix = String.Empty;
                string LanguageCode = String.Empty;

                //-- Open and Connect to database  If Database file is Selected
                if (!String.IsNullOrEmpty(this._SourceDatabaseNameWPath))
                {
                    //--Dispose Connection
                    if (this.DBConnection != null)
                    {
                        this.DBConnection.Dispose();
                        this.DBConnection = null;
                    }

                    this.DBConnection = new DIConnection(new DIConnectionDetails(DIServerType.MsAccess, String.Empty, String.Empty, this._SourceDatabaseNameWPath, String.Empty, String.Empty));
                    DatasetPrefix = this.DBConnection.DIDataSetDefault();
                    LanguageCode = this.DBConnection.DILanguageCodeDefault(DatasetPrefix);
                    this.DBQueries = new DIQueries(DatasetPrefix, LanguageCode);
                    
                }
            }

       /// <summary>
       /// Fill collection with Labguage string required for Excel WorkBook
       /// </summary>
            private void InitColumnHeading()
            {
               
                //-- add language based columns header
                this.ColumnsHeader.Add(DRCColumnsHeader.AreaId, DILanguage.GetLanguageString("AREA_ID"));
                this.ColumnsHeader.Add(DRCColumnsHeader.AreaName, DILanguage.GetLanguageString("AREANAME"));
                this.ColumnsHeader.Add(DRCColumnsHeader.DataRangeCheckReport, DILanguage.GetLanguageString("DATA") + " " + DILanguage.GetLanguageString("RANGECHECKREPORT"));
                this.ColumnsHeader.Add(DRCColumnsHeader.DataValue, DILanguage.GetLanguageString("DATAVALUE"));
                this.ColumnsHeader.Add(DRCColumnsHeader.DateandTime, DILanguage.GetLanguageString("DATE_TIME"));
                this.ColumnsHeader.Add(DRCColumnsHeader.Indicator, DILanguage.GetLanguageString("INDICATOR"));
                this.ColumnsHeader.Add(DRCColumnsHeader.Max, DILanguage.GetLanguageString("MAX"));
                this.ColumnsHeader.Add(DRCColumnsHeader.Min, DILanguage.GetLanguageString("MIN"));
                this.ColumnsHeader.Add(DRCColumnsHeader.RangeCheck, DILanguage.GetLanguageString("RANGECHECKREPORT"));
                this.ColumnsHeader.Add(DRCColumnsHeader.Source, DILanguage.GetLanguageString("SOURCE"));
                this.ColumnsHeader.Add(DRCColumnsHeader.Subgroup, DILanguage.GetLanguageString("SUBGROUP"));
                this.ColumnsHeader.Add(DRCColumnsHeader.Time, DILanguage.GetLanguageString("TIME"));
                this.ColumnsHeader.Add(DRCColumnsHeader.Unit, DILanguage.GetLanguageString("UNIT"));
            }

       /// <summary>
       /// Initialize WorkSheet with Heading and Columns names and set font properties.
       /// </summary>
       /// <param name="RangeCheckSheet"></param>
       /// <param name="SheetNo"></param>
            private void SetWorkbookInitialValue(ref DIExcel RangeCheckSheet, int SheetNo)
            {

                //-- Set Report Heading 
                RangeCheckSheet.SetCellValue(SheetNo, RangeCheckFileRowsInfo.SheetHeaderRowIndex, RangeCheckFileRowsInfo.SourceHeaderColIndex, this.ColumnsHeader [DRCColumnsHeader.DataRangeCheckReport]);

                //-- Set Font and Size 
                    RangeCheckSheet.GetCellFont(SheetNo, RangeCheckFileRowsInfo.SheetHeaderRowIndex, RangeCheckFileRowsInfo.SheetHeaderColIndex).Bold = true;
                    RangeCheckSheet.GetCellFont(SheetNo, RangeCheckFileRowsInfo.SheetHeaderRowIndex, RangeCheckFileRowsInfo.SheetHeaderColIndex).Size = RangeCheckCustomizationInfo.HeaderFontSize;
                

                //-- Set Font Bold 
                RangeCheckSheet.GetRangeFont(SheetNo, RangeCheckFileRowsInfo.SourceHeaderRowIndex, RangeCheckFileRowsInfo.SheetHeaderColIndex, RangeCheckFileRowsInfo.DateTimeHeaderRowIndex, RangeCheckFileRowsInfo.SourceDataColIndex).Bold = true;

                //-- Display the database file name 
                RangeCheckSheet.SetCellValue(SheetNo, RangeCheckFileRowsInfo.SourceHeaderRowIndex, RangeCheckFileRowsInfo.SourceHeaderColIndex, this.ColumnsHeader[DRCColumnsHeader.Source ]);
                RangeCheckSheet.SetCellValue(SheetNo, RangeCheckFileRowsInfo.SourceHeaderRowIndex, RangeCheckFileRowsInfo.SourceDataColIndex, this._SourceDatabaseNameWPath  );

                //-- Set Date and Time Value 
                RangeCheckSheet.SetCellValue(SheetNo, RangeCheckFileRowsInfo.DateTimeHeaderRowIndex, RangeCheckFileRowsInfo.DateTimeHeaderColIndex, this.ColumnsHeader[DRCColumnsHeader.DateandTime]);
                string   CurDateTime;
                CurDateTime = DateTime.Now.ToString();
                RangeCheckSheet.SetCellValue(SheetNo, RangeCheckFileRowsInfo.DateTimeHeaderRowIndex, RangeCheckFileRowsInfo.DateTimeDataColIndex, CurDateTime);
            }

            /// <summary>
            /// Set Column Width and Alignment
            /// </summary>
            /// <param name="RangeCheckSheet"></param>
            /// <param name="SheetNo"></param>
            private void SetColumnWidth(ref DIExcel RangeCheckSheet, int SheetNo)
            {
                //-- Set Column Width 
                RangeCheckSheet.SetColumnWidth(SheetNo, RangeCheckCustomizationInfo.FirstColumnWidth, RangeCheckFileRowsInfo.SheetStartRowIndex, 0, RangeCheckCustomizationInfo.MAX_EXCEL_ROWS, RangeCheckFileRowsInfo.DateTimeDataColIndex);
                RangeCheckSheet.SetColumnWidth(SheetNo, RangeCheckCustomizationInfo.LastColumnWidth, RangeCheckFileRowsInfo.SheetStartRowIndex, RangeCheckFileRowsInfo.ColWidithStartIndex, RangeCheckCustomizationInfo.MAX_EXCEL_ROWS, RangeCheckFileRowsInfo.ColWidthLastIndex);

                RangeCheckSheet.SetHorizontalAlignment(SheetNo, RangeCheckFileRowsInfo.SheetStartRowIndex, RangeCheckFileRowsInfo.SheetHeaderColIndex, RangeCheckCustomizationInfo.MAX_EXCEL_ROWS, RangeCheckFileRowsInfo.ColWidthLastIndex, SpreadsheetGear.HAlign.Left);
            }

           /// <summary>
           /// Set Line Borders of Sheet
           /// </summary>
           private void SetSheetBorder(ref DIExcel RangeCheckSheet, int SheetNo)
           {
                //-- Set Border Line 
                RangeCheckSheet.SetCellBorder(SheetNo, RangeCheckFileRowsInfo.SheetStartRowIndex, RangeCheckFileRowsInfo.SheetHeaderColIndex, SpreadsheetGear.LineStyle.Continuous, SpreadsheetGear.BorderWeight.Medium, Color.Black, SpreadsheetGear.BordersIndex.EdgeTop);
                RangeCheckSheet.SetCellBorder(SheetNo, RangeCheckFileRowsInfo.SheetStartRowIndex, RangeCheckFileRowsInfo.SheetHeaderColIndex, SpreadsheetGear.LineStyle.Continuous, SpreadsheetGear.BorderWeight.Medium, Color.Black, SpreadsheetGear.BordersIndex.EdgeBottom);
                RangeCheckSheet.SetCellBorder(SheetNo, RangeCheckFileRowsInfo.SheetStartRowIndex, RangeCheckFileRowsInfo.SheetHeaderColIndex, SpreadsheetGear.LineStyle.Continuous, SpreadsheetGear.BorderWeight.Medium, Color.Black, SpreadsheetGear.BordersIndex.EdgeLeft);
                RangeCheckSheet.SetCellBorder(SheetNo, RangeCheckFileRowsInfo.SheetStartRowIndex, RangeCheckFileRowsInfo.SheetHeaderColIndex, SpreadsheetGear.LineStyle.Continuous, SpreadsheetGear.BorderWeight.Medium, Color.Black, SpreadsheetGear.BordersIndex.InsideHorizontal);
                RangeCheckSheet.SetCellBorder(SheetNo, RangeCheckFileRowsInfo.SheetStartRowIndex, RangeCheckFileRowsInfo.SheetHeaderColIndex, SpreadsheetGear.LineStyle.Continuous, SpreadsheetGear.BorderWeight.Medium, Color.Black, SpreadsheetGear.BordersIndex.InsideVertical);

            }

       /// <summary>
            ///  Rename DataTable Column with Required Value
            /// </summary>
            /// <param name="Table"></param>
            private void RenameColumn(ref DataTable Table)
            {
                Table.BeginInit();
                Table.Columns[0].ColumnName = this.ColumnsHeader[DRCColumnsHeader.Time];
                Table.Columns[1].ColumnName = this.ColumnsHeader[DRCColumnsHeader.AreaId];
                Table.Columns[2].ColumnName = this.ColumnsHeader[DRCColumnsHeader.AreaName];
                Table.Columns[3].ColumnName = this.ColumnsHeader[DRCColumnsHeader.DataValue];
                Table.Columns[4].ColumnName = this.ColumnsHeader[DRCColumnsHeader.Source];
                Table.EndInit();
            }

       /// <summary>
       ///-- Add Column for Range Details 
       /// </summary>
       /// <returns></returns>
        private DataTable RangeHeaderDataTable()
        {
            DataTable RetVal = new DataTable();

            RetVal.Columns.Add( DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Timeperiods.TimePeriod);
            RetVal.Columns.Add(DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaID);
            RetVal.Columns.Add(DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaName);
            RetVal.Columns.Add(DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Data.DataValue, Type.GetType("System.Double"));
            RetVal.Columns.Add(DevInfo.Lib.DI_LibDAL.Queries.DIColumns.IndicatorClassifications.ICName);
            return RetVal;

        }

       /// <summary>
       /// Imports Row In Temporary DataTable
       /// </summary>
       /// <param name="RangeRows"></param>
       /// <returns></returns>
        private DataTable RangeCheckDetails(DataRow[] RangeRows)
        {
            DataTable RetVal = RangeHeaderDataTable();
            foreach (DataRow row in RangeRows)
            {
                RetVal.ImportRow(row);
            }
            return RetVal;
        }

        #region "-- Change No: c1 --"
       
       private DataTable GetValidatedRangeDetails(DataTable reportTable,int lastIUSNID,RangeType rangeTypes)
       {
           DataTable RetVal =reportTable.Clone();
           // -- Get 
           DataRow[] iusRecords = reportTable.Select(Indicator_Unit_Subgroup.IUSNId + " = " + lastIUSNID);
           
           foreach (DataRow RowVal in iusRecords)
           {
               switch (rangeTypes)
               {
                   case RangeType.DataBelow_MinVal:
                       if (Convert.ToDouble(RowVal[Data.DataValue]) < Convert.ToDouble(RowVal[Indicator_Unit_Subgroup.MinValue]))
                       {
                           RetVal.ImportRow(RowVal);
                       }
                       break;
                   case RangeType.DataBelow_MaxVal:
                       if (Convert.ToDouble(RowVal[Data.DataValue]) < Convert.ToDouble(RowVal[Indicator_Unit_Subgroup.MaxValue]))
                       {
                           RetVal.ImportRow(RowVal);
                       }
                       break;
                   case RangeType.DataAbove_MinVal:
                       if (Convert.ToDouble(RowVal[Data.DataValue]) > Convert.ToDouble(RowVal[Indicator_Unit_Subgroup.MinValue]))
                       {
                           RetVal.ImportRow(RowVal);
                       }
                       break;
                   case RangeType.DataAbove_MaxVal:
                       if (Convert.ToDouble(RowVal[Data.DataValue]) > Convert.ToDouble(RowVal[Indicator_Unit_Subgroup.MaxValue]))
                       {
                           RetVal.ImportRow(RowVal);
                       }
                       break;
                  
               }
           }

           RetVal.AcceptChanges();
          
           return RetVal;

       }

        #endregion

        #endregion

    #endregion

        #region -- Public --

       #region "-- Events --"

       /// <summary>
            /// Fires when value of prgressbar is changed.
            /// </summary>
            public event IncrementProgressBar ProgressBar_Increment;

            /// <summary>
            /// Fires when process started to initialize progress bar.
            /// </summary>
            public event InitializeProgressBar ProgressBar_Initialize;

            /// <summary>
            /// Fireds when process stop.
            /// </summary>
            public event CloseProgressBar ProgressBar_Close;


            #endregion

       #region Varibles and Properties

       
       private string _SourceDatabaseNameWPath;
       /// <summary>
       /// Get Connected Database File path and Set Database Conection
       /// </summary>
            public string SourceDatabaseNameWPath
            {
                get
                {
                    return this._SourceDatabaseNameWPath;
                }
                set
                {
                    this._SourceDatabaseNameWPath = value;
                    if (!String.IsNullOrEmpty(value))
                    {
                        this.ConnectToDataBase();
                    }
                }
            }
        #endregion

       #region -- New/ Dispose
            /// <summary>
            ///Constructor
            /// </summary>
            /// <param name="SourceDataBasePath">Pass the Source Database Path</param>
            /// <param name="DestinationFilePath">Path of Report File Destination</param>
     
            public RangeCheckReportGenerator(string SourceDataBasePath,string DestinationFilePath )
            {
                this.SourceDatabaseNameWPath = SourceDataBasePath;
                this.ReportDestinationFilePath=DestinationFilePath ;
            }

       #endregion

       #region -- Methods --

       #region "-- Raise Event mehtods --"

            /// <summary>
            /// To raise ProgressBar_Increment
            /// </summary>
            protected void RaiseProgressBarIncrement(int value)
            {
                if (this.ProgressBar_Increment != null)
                    this.ProgressBar_Increment(value);
            }

            /// <summary>
            /// To raise ProgressBar_Initialize
            /// </summary>
            protected void RaiseProgressBarInitialize(int maximumValue)
            {
                if (this.ProgressBar_Initialize != null)
                    this.ProgressBar_Initialize(maximumValue);
            }

            /// <summary>
            /// To raise ProgressBar_Close
            /// </summary>
            protected void RaiseProgressBarClose()
            {
                if (this.ProgressBar_Close != null)
                    this.ProgressBar_Close();
            }

       #endregion

       #region "-- Generate Excel Report --"
            
       /// <summary>
       /// Generate Range Check Report
       /// </summary>
       /// <returns></returns>
        public bool GenerateExcelReport() 
        {
            bool RetVal = false;
            DataTable ReportTable; 
            int SheetNo = 0;
            int ProgressBarValue=0;

            //-- Initialize Collection
            this.InitColumnHeading();

            //-- Check Selected File Selected 
            if (this.ReportDestinationFilePath.Length > 0) 
            { 
                DIExcel RangeCheckSheet = new DIExcel(); 
                
                string NumDecSeparator = System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator; 
                //-- Problem with Different Locale of Office and Regional(Setting) 
                System.Threading.Thread ThisThread = System.Threading.Thread.CurrentThread ; 
                System.Globalization.CultureInfo OriginalCulture = ThisThread.CurrentCulture ; 
                
                //-- Create Worksheet named "Range Check" 
                RangeCheckSheet.CreateWorksheet(this.ColumnsHeader[DRCColumnsHeader.RangeCheck]); 
                SheetNo = RangeCheckSheet.GetSheetIndex(this.ColumnsHeader[DRCColumnsHeader.RangeCheck]); 
                
                this.SetWorkbookInitialValue( ref RangeCheckSheet,  SheetNo); 
                
                ThisThread.CurrentCulture = OriginalCulture; 
                
                //-- Fill DataTable 
                ReportTable = this.DBConnection.ExecuteDataTable(DBQueries.Data.GetValuesRangeCheck());
                
               // TempTable.Merge(ReportTable,false);
                int maxValue = ReportTable.Rows.Count; 
                try 
                { 
                    ThisThread.CurrentCulture = new System.Globalization.CultureInfo("en-US"); 
                    //-- Proceed If datatable has Records 
                    if (ReportTable.Rows.Count > 0) 
                    { 
                        // Initialize progress bar
                        this.RaiseProgressBarInitialize(maxValue + 1);
                       
                        int RowNum = RangeCheckFileRowsInfo.DataStartingRowIndex; 
                        int LastIUSNID = 0; 
                        
                        //-- Fill Record Indicator wise 
                        foreach (DataRow rowval in ReportTable.Rows) 
                        {
                            // -- Avoid Process if IUSNID is Last IUSNID 
                            if (Convert.ToInt32(rowval[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator_Unit_Subgroup.IUSNId]) != LastIUSNID) 
                            {
                                LastIUSNID = Convert.ToInt32(rowval[ DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator_Unit_Subgroup.IUSNId]); 
                                
                                //-- Set Data Column Header Bold 
                                RangeCheckSheet.GetRangeFont(SheetNo, RowNum, RangeCheckFileRowsInfo.SheetHeaderColIndex, RowNum + RangeCheckFileRowsInfo.HeaderRowCount, RangeCheckFileRowsInfo.SheetHeaderColIndex).Bold = true;
                                
                                RowNum += 1; 
                                RangeCheckSheet.SetCellValue(SheetNo, RowNum, RangeCheckFileRowsInfo.DetailHeaderColIndex, this.ColumnsHeader[DRCColumnsHeader.Indicator]);
                                RangeCheckSheet.SetCellValue(SheetNo, RowNum, RangeCheckFileRowsInfo.DetailDataColIndex, Convert.ToString( rowval[ DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorName] )); 
                                RowNum += 1; 
                                RangeCheckSheet.SetCellValue(SheetNo, RowNum, RangeCheckFileRowsInfo.DetailHeaderColIndex, this.ColumnsHeader[DRCColumnsHeader.Unit]);
                                RangeCheckSheet.SetCellValue(SheetNo, RowNum, RangeCheckFileRowsInfo.DetailDataColIndex, Convert.ToString( rowval[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitName] )); 
                                RowNum += 1; 
                                RangeCheckSheet.SetCellValue(SheetNo, RowNum, RangeCheckFileRowsInfo.DetailHeaderColIndex, this.ColumnsHeader[DRCColumnsHeader.Subgroup]);
                                RangeCheckSheet.SetCellValue(SheetNo, RowNum, RangeCheckFileRowsInfo.DetailDataColIndex, Convert.ToString(rowval[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupVal])); 
                                RowNum += 1; 
                                RangeCheckSheet.SetCellValue(SheetNo, RowNum, RangeCheckFileRowsInfo.DetailHeaderColIndex, this.ColumnsHeader[DRCColumnsHeader.Min]);
                                // -- Get Minimum Value 
                                double MinValue = (Information.IsDBNull(rowval[Indicator_Unit_Subgroup.MinValue])? Convert.ToDouble("0.0") :Convert.ToDouble( rowval[Indicator_Unit_Subgroup.MinValue])) ;
                                RangeCheckSheet.SetCellValue(SheetNo, RowNum, RangeCheckFileRowsInfo.DetailDataColIndex, MinValue); 
                                RowNum += 1; 
                                RangeCheckSheet.SetCellValue(SheetNo, RowNum, RangeCheckFileRowsInfo.DetailHeaderColIndex, this.ColumnsHeader[DRCColumnsHeader.Max]);
                                double MaxValue = (Information.IsDBNull(rowval[Indicator_Unit_Subgroup.MaxValue]) ? Convert.ToDouble("0.0") : Convert.ToDouble(rowval[Indicator_Unit_Subgroup.MaxValue]));
                                RangeCheckSheet.SetCellValue(SheetNo, RowNum, RangeCheckFileRowsInfo.DetailDataColIndex, MaxValue); 
                                RowNum += 1; 

                                DataTable Table=null;
                                DataTable OtherTable=null;
                                // -- insert timeperiod, areaid,area name,datavalue,source 
                                
                                #region "-- Change No: c1 --"
                                
                                if (MaxValue > MinValue)
                                {
                                    Table = this.RangeCheckDetails(this.GetValidatedRangeDetails(ReportTable, LastIUSNID,RangeType.DataBelow_MinVal ).Select()); 
                                    OtherTable = this.RangeCheckDetails(this.GetValidatedRangeDetails(ReportTable, LastIUSNID, RangeType.DataAbove_MaxVal).Select());
                                }
                                else if (MaxValue == 0.0 && MinValue == 0.0) { }
                                else
                                {
                                    Table = this.RangeCheckDetails(this.GetValidatedRangeDetails(ReportTable, LastIUSNID, RangeType.DataAbove_MinVal).Select());
                                    OtherTable = this.RangeCheckDetails(this.GetValidatedRangeDetails(ReportTable, LastIUSNID, RangeType.DataBelow_MaxVal).Select());
                                }

                                #endregion

                                ////if (MaxValue > MinValue)
                                ////{
                                ////    Table = RangeCheckDetails(ReportTable.Select(Indicator_Unit_Subgroup.IUSNId + " = " + LastIUSNID )); //+ " AND " + Data.DataValue + " < " + Indicator_Unit_Subgroup.MinValue  ));
                                ////    OtherTable = RangeCheckDetails(ReportTable.Select(Indicator_Unit_Subgroup.IUSNId + " = " + LastIUSNID ));//+ " AND " + Data.DataValue + " > " + Indicator_Unit_Subgroup.MaxValue));
                                ////}
                                ////else if (MaxValue == 0.0 && MinValue == 0.0) { }
                                ////else
                                ////{
                                ////    Table = RangeCheckDetails(ReportTable.Select(DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator_Unit_Subgroup.IUSNId + " = " + LastIUSNID + " AND " + Data.DataValue + " > " + Indicator_Unit_Subgroup.MinValue));
                                ////    OtherTable = RangeCheckDetails(ReportTable.Select(DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator_Unit_Subgroup.IUSNId + " = " + LastIUSNID + " AND " + Data.DataValue + " < " + Indicator_Unit_Subgroup.MaxValue));
                                ////}
                                if (Table != null)
                                {
                                    // -- Merge Both Table
                                    Table.Merge(OtherTable, true);

                                    //-- Check if Records Exist in DataTable Then Export to Excel Workbook 
                                    if (Table.Rows.Count > 0)
                                    {
                                        //-- Set Data Column Header Font 
                                        RangeCheckSheet.GetRangeFont(SheetNo, RowNum, RangeCheckFileRowsInfo.SheetHeaderColIndex, RowNum, RangeCheckFileRowsInfo.ColWidthLastIndex).Italic = true;
                                        //-- Set Column Backgroud Color TO Grey 
                                        RangeCheckSheet.SetRangeColor(SheetNo, RowNum, RangeCheckFileRowsInfo.SheetHeaderColIndex, RowNum, RangeCheckFileRowsInfo.ColWidthLastIndex, Color.Black, System.Drawing.Color.FromArgb(Color.Gray.R, Color.Gray.G, Color.Gray.B));

                                        //-- Raname DataTable Column Name As Per Excel Sheet 
                                        this.RenameColumn(ref Table);
                                        //-- Load DataTAble Into Excel 
                                        RangeCheckSheet.LoadDataTableIntoSheet(RowNum, RangeCheckFileRowsInfo.DetailHeaderColIndex, Table, SheetNo, false);
                                    }
                                    RowNum += Table.Rows.Count + 1;
                                }
                                this.RaiseProgressBarIncrement(ProgressBarValue);   //raise Progressbar_Increment event
                            } //End Of Next

                            //-- Increase ProgressBar Value 
                            ProgressBarValue++;

                          //-- IF numbers of record in dataview is morethan the max rows i.e 50,000 available in excel 
                            if (RowNum > RangeCheckCustomizationInfo.MAX_EXCEL_ROWS )
                            { 
                                break; 
                            } 
                        } 
                        //-- Set WorkSheet Border Line width
                        this.SetColumnWidth(ref RangeCheckSheet, SheetNo);
                        this.SetSheetBorder(ref RangeCheckSheet, SheetNo);
                    } //End OF IF

                    //-- Make Progressbar Value to Maximum 
                    this.RaiseProgressBarIncrement(maxValue );

                    try 
                        { 
                        // -- Save the Workbook If file exist then delete and then Save 
                        if (System.IO.File.Exists(this.ReportDestinationFilePath))
                        {
                            System.IO.File.SetAttributes(this.ReportDestinationFilePath, FileAttributes.Normal);
                            System.IO.File.Delete(this.ReportDestinationFilePath); 
                        }
                        RangeCheckSheet.SaveAs(this.ReportDestinationFilePath);

                        this.RaiseProgressBarClose();
                        RetVal = true;
                        } 
                        catch
                        {
                            RetVal = false;
                        }
                } 
                catch (Exception ex) 
                {
                   this.RaiseProgressBarClose();
                   throw new ApplicationException(ex.Message);
                }
            }//End OF IF
            return RetVal;
        }

       public void OpenSavedReport()
       {
           try
           {
               if(File.Exists(this.ReportDestinationFilePath))
               {
                Process ReportProcess = new Process();
                ProcessStartInfo StartInfo = new ProcessStartInfo(this.ReportDestinationFilePath);
                StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                ReportProcess.StartInfo = StartInfo;
                ReportProcess.Start();
               }
           }
           catch { }
       }

      #endregion

       /// <summary>
       /// Dispose Database Connection.
       /// </summary>
       public void DisposeConnection()
       {
           if (this.DBConnection != null)
           {
               this.DBConnection.Dispose();
           }
           if (this.DBQueries != null)
           {
               this.DBQueries.Dispose();
           }
       }

       #endregion
       
    #endregion
        
       
       


      
    }
}
