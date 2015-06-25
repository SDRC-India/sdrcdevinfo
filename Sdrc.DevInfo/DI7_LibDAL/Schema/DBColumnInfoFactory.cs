using System.Data.OleDb;
using System.Collections.Generic;
using DevInfo.Lib.DI_LibDAL.Connection;
using System.Data.Common;
using System.Data;
using System;
using DevInfo.Lib.DI_LibDAL.Schema;

namespace DevInfo.Lib.DI_LibDAL.Schema
{
/// <summary>
/// Provides all columns information like ColumnName, Size, etc./// 
/// </summary>
    public class DbColumnInfoFactory
    {
        #region -- Private --

        #region "-- Variables --"

        private const string Table = "TABLE";
        private const string TablesCollectionName = "Tables";
        private const string ColumnsCollectionName = "Columns";
        private const string TableName = "TABLE_NAME";
        private const string ColumnName = "COLUMN_NAME";
        private const String CharacterMaximumLength = "CHARACTER_MAXIMUM_LENGTH";

        #endregion

        #region -- New/Dispose --

        private DbColumnInfoFactory()
        {
        }

        #endregion

        #endregion

        #region -- Public --

        #region -- Variables / Properties --
        private Dictionary<string, DbColumnInfo> _DbColumns = new Dictionary<string, DbColumnInfo>();
        /// <summary>
        /// Gets the columns detail
        /// </summary>
        public Dictionary<string, DbColumnInfo> DbColumns
        {
            get { return this._DbColumns; }
        }

        #endregion

        #region -- New/Dispose --

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connection"></param>
        public DbColumnInfoFactory(DIConnection connection)
        {
            DbConnection DbConnection;
            DataTable SchemaTable;
            DataTable SchemaColumns;
            string[] ObjArrRestrict = new string[4];
            string ColumnName = string.Empty;
            long ColumnSize = 0;

            try
            {
                if (connection.GetConnection().State == ConnectionState.Open)
                {
                    DbConnection = connection.GetConnection();
                    ObjArrRestrict[0] = null;
                    ObjArrRestrict[1] = null;
                    ObjArrRestrict[2] = null;
                    ObjArrRestrict[3] = Table;

                    SchemaTable = DbConnection.GetSchema(TablesCollectionName);

                    foreach (DataRow row in SchemaTable.Rows)
                    {
                        ObjArrRestrict[2] = row[TableName].ToString();
                        ObjArrRestrict[3] = null;
                        SchemaColumns = DbConnection.GetSchema(ColumnsCollectionName, ObjArrRestrict);

                        foreach (DataRow schemaColumnsRow in SchemaColumns.Rows)
                        {
                            ColumnName = schemaColumnsRow[DbColumnInfoFactory.ColumnName].ToString();

                            if ((!Microsoft.VisualBasic.Information.IsDBNull(schemaColumnsRow[DbColumnInfoFactory.CharacterMaximumLength])) & (!(this._DbColumns.ContainsKey(ColumnName))))
                            {
                                ColumnSize = Convert.ToInt64(schemaColumnsRow[DbColumnInfoFactory.CharacterMaximumLength]);
                                this._DbColumns.Add(ColumnName, new DbColumnInfo(ColumnName, ColumnSize));
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
    }
}