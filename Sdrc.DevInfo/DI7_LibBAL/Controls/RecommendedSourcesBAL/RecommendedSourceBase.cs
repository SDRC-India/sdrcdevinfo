using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.UserSelection;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibDAL.ExceptionHandler;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;


namespace DevInfo.Lib.DI_LibBAL.Controls.RecommendedSourcesBAL
{
    public abstract class RecommendedSourceBase
    {

        private void UpdateRecommendedSourcesTable(string ICIUSLabel, int dataNId, int recommendedSourcesNId)
        {
            DataRow[] Rows;
            DataRow NewRow;

            try
            {
                Rows = this.RecommendedSourcesTable.Select(RecommendedSources.NId + "=" + recommendedSourcesNId);
                if (Rows.Length > 0)
                {
                    //update label
                    Rows[0][RecommendedSources.ICIUSLabel] = ICIUSLabel;
                }
                else
                {
                    // add new row
                    NewRow = this.RecommendedSourcesTable.NewRow();
                    NewRow[RecommendedSources.NId] = recommendedSourcesNId;
                    NewRow[RecommendedSources.ICIUSLabel] = ICIUSLabel;
                    NewRow[RecommendedSources.DataNId] = dataNId;
                    this.RecommendedSourcesTable.Rows.Add(NewRow);
                }

                // update changes
                this.RecommendedSourcesTable.AcceptChanges();
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        protected string _TagColumnName = string.Empty;

        /// <summary>
        /// Get Nid Column for Control Tag
        /// </summary>
        public string NIdColumnName
        {
            get { return this._TagColumnName; }
        }

        protected string _DisplayColumnName = string.Empty;
        /// <summary>
        /// Get Column Name To Display In Control Label
        /// </summary>
        public string DisplayColumnName
        {
            get { return this._DisplayColumnName; }
        }

        protected string _GlobalColumnName;
        /// <summary>
        /// Gets global column name
        /// </summary>
        public string GlobalColumnName
        {
            get { return this._GlobalColumnName; }
        }


        private ICType _SeletectedICType;
        /// <summary>
        /// Gets or sets selected ICType. Use this property only if ICNId's is -1 
        /// </summary>
        public ICType SeletectedICType
        {
            get { return this._SeletectedICType; }
            set { this._SeletectedICType = value; }
        }


        protected DIConnection _DBConnection;

        public DIConnection DBConnection
        {
            set { this._DBConnection = value; }
        }

        protected DIQueries _DBQueries;

        public DIQueries DBQueries
        {
            set { this._DBQueries = value; }
        }

        private UserSelection _UserSelections;
        /// <summary>
        /// Gets or Sets user selection
        /// </summary>
        public UserSelection UserSelections
        {
            get { return this._UserSelections; }
            set
            {
                this._UserSelections = value;
            }
        }

        private DataTable _RecommendedSourcesTable = null;
        /// <summary>
        /// Get RecommendedSource DataTable
        /// </summary>
        public DataTable RecommendedSourcesTable
        {
            get
            {                
                return this._RecommendedSourcesTable;
            }
            set
            {
                this._RecommendedSourcesTable = value;
            }

        }


        public abstract DataTable GetDefaultElementsTable();

        public abstract DataTable GetAllRecordsFrmData(int NId);

        public DataTable GetAllRecordsWRecommendedSource(int NId)
        {
            DataTable RetVal = null;

            DataTable DataRecordsTable = null;
            DataTable RecommendedSources = null;

            // step1: get records from data by user selection and for the given NId
            DataRecordsTable = this.GetAllRecordsFrmData(NId);
            DataRecordsTable.Columns.Add(DevInfo.Lib.DI_LibDAL.Queries.DIColumns.RecommendedSources.ICIUSLabel);


            // step2: get all recommended sources
            //RecommendedSources=this.GetAllRecommendedSources();

            //foreach (DataRow  row in DataRecordsByNids.Rows )
            //{
            //    DataRow[] rows = RecommendedSources.Select(DevInfo.Lib.DI_LibDAL.Queries.DIColumns.RecommendedSources.DataNId + "=" + row[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.RecommendedSources.DataNId]);
            //    if (rows.Length > 0)
            //    {
            //        row[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.RecommendedSources.ICIUSLabel] = rows[0][DevInfo.Lib.DI_LibDAL.Queries.DIColumns.RecommendedSources.ICIUSLabel];
            //    }

            //}

            //-- Add selected column and check the row on the basis of IUS_IC and data_NId
            DataRecordsTable = this.AddSelectRowColumnInTable(DataRecordsTable);

            DataRecordsTable.AcceptChanges();
            RetVal = DataRecordsTable;

            return RetVal;

        }

        /// <summary>
        /// Select rows on the basis of IUS_IC and DataNId.
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private DataTable AddSelectRowColumnInTable(DataTable table)
        {
            DataColumn Col = null;
            DataTable RetVal = new DataTable();
            try
            {
                string SelectColumn = "SELECTED";
                Col = new DataColumn(SelectColumn, Type.GetType("System.Boolean"));
                Col.Caption = string.Empty;
                Col.DefaultValue = true;
                RetVal.Columns.Add(Col);
                RetVal.Merge(table);

                if (!string.IsNullOrEmpty(UserSelections.DataViewFilters.DeletedSourceNIds))
                {
                    if (UserSelections.DataViewFilters.DeletedSourceNIds.Contains("_"))
                    {
                        DataRow[] Rows = new DataRow[0];

                        Rows = RetVal.Select(Indicator_Unit_Subgroup.IUSNId + " + '_' + " + Data.SourceNId + " IN (" + UserSelections.DataViewFilters.DeletedSourceNIds + ")");
                        foreach (DataRow Row in Rows)
                        {
                            Row[SelectColumn] = false;
                        }
                    }
                    else
                    {
                        DataRow[] Rows = new DataRow[0];

                        Rows = RetVal.Select(Data.DataNId + " IN (" + UserSelections.DataViewFilters.DeletedSourceNIds + ")");
                        foreach (DataRow Row in Rows)
                        {
                            Row[SelectColumn] = false;
                        }
                    }
                }
            }
            catch (Exception ex) { }//MessageBoxControl.ShowErrorMessage(ex); }
            return RetVal;
        }

        public DataTable GetAllRecommendedSources()
        {
            DataTable RetVal = null;

            string Query = this._DBQueries.RecommendedSources.GetRecommendedSources(string.Empty);
            RetVal = this._DBConnection.ExecuteDataTable(Query);

            return RetVal;
        }

        public bool SaveRecommendedSources(DataTable table)
        {
            bool RetVal = false;

            RecommendedSourcesBuilder RecBuilder = new RecommendedSourcesBuilder(this._DBConnection, this._DBQueries);
            try
            {

                foreach (DataRow row in table.Rows)
                {

                    RecBuilder.CheckNInsertRecommendedSource(Convert.ToInt32(row[Data.DataNId]), Convert.ToString(row[Data.ICIUSOrder]), Convert.ToString(row[RecommendedSources.ICIUSLabel]));

                }
                RetVal = true;
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }

        /// <summary>
        /// Saves ICIUSLabel into recommendedsources table
        /// </summary>
        /// <param name="ICIUSLabel"></param>
        /// <param name="ICIUSOrder"></param>
        /// <param name="dataNId"></param>
        /// <returns></returns>
        public int SaveICIUSLabel(string ICIUSLabel, string ICIUSOrder, string dataNId)
        {
            int RetVal = -1;
            DataRow[] Rows;
            RecommendedSourcesBuilder RecBuilder = new RecommendedSourcesBuilder(this._DBConnection, this._DBQueries);

            try
            {
                // check records is already exists or not. IF exists then get NID
                Rows = this._RecommendedSourcesTable.Select(RecommendedSources.DataNId + "=" + dataNId);
                if (Rows.Length > 0)
                {
                    RetVal =Convert.ToInt32( Rows[0][RecommendedSources.NId]);
                }


                if (RetVal <= 0)
                {
                    //insert into database               
                    RecBuilder.InsertIntoDatabase(Convert.ToInt32(dataNId), ICIUSLabel);
                    RetVal = this._DBConnection.GetNewId();
                }
                else
                {
                    // update into database
                    RecBuilder.UpdateRecommendedSources(Convert.ToInt32(dataNId), ICIUSLabel);
                }

                //update RecommendedSourcesTable
                this.UpdateRecommendedSourcesTable(ICIUSLabel, Convert.ToInt32(dataNId), RetVal);

            }
            catch (Exception ex)
            {
                RetVal = -1;
                ExceptionFacade.ThrowException(ex);
            }

            return RetVal;
        }

        /// <summary>
        /// Saves ICIUSOrder into Data Table
        /// </summary>
        /// <param name="ICIUSOrder"></param>
        /// <param name="dataNId"></param>
        /// <returns></returns>
        public bool SaveICIUSOrder(string ICIUSOrder, int dataNId)
        {
            bool RetVal = false;
            RecommendedSourcesBuilder RecBuilder = new RecommendedSourcesBuilder(this._DBConnection, this._DBQueries);

            try
            {
                RecBuilder.UpdateICIUSOrderInDataTable(dataNId, ICIUSOrder);
                RetVal = true;
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }

            return RetVal;
        }


    }
}
