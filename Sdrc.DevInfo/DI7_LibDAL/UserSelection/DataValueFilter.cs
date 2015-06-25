using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.UserSelection
{
    /// <summary>
    /// Represents the data value filters
    /// This class supports the DevInfo Framework infrastructure and is not intended to be used directly from your code
    /// </summary>
    public class DataValueFilter
    {
        #region " -- Private / Internal -- "    
        /// <summary>Internal Constructor</summary>
        /// <remarks>Allow Filter Class to instatntiate DataValueFilter and expose as property </remarks>
        public  DataValueFilter()
        {
        }

        #endregion

        #region " -- Public -- "

        #region " -- Properties -- "

        private double _FromDataValue;
        /// <summary>Gets or sets the first data value used with operator</summary>
        /// <remarks>Required for all OperatorType (except None)</remarks>
        public double FromDataValue
        {
            get { return _FromDataValue; }
            set { _FromDataValue = value; }
        }

        private OpertorType _OpertorType = OpertorType.None;
        /// <summary>Type of operator. None, EqualTo, Between, GreaterThan, LessThan</summary>
        /// <remarks>Enum value of OpertorType</remarks>
        public OpertorType OpertorType
        {
            get { return _OpertorType; }
            set { _OpertorType = value; }
        }

        private double _ToDataValue;
        /// <summary>Gets or sets the second data value used with operator</summary>
        /// <remarks>Required when OperatorType = Between</remarks>
        public double ToDataValue
        {
            get { return _ToDataValue; }
            set { _ToDataValue = value; }
        }

        #endregion

        #region " -- Methods -- "

        /// <summary>
        /// Clears DataValue filter. Set OpertorType to None, FromDataValue = 0, ToDataValue = 0
        /// </summary>
        public void Clear()
        {
            this._OpertorType = OpertorType.None;
            this._FromDataValue = 0;
            this._ToDataValue = 0;
        }

        internal virtual string SQL_GetDataValueFilter(DIServerType DIServerType)
        {
            string RetVal = String.Empty;
            switch (this._OpertorType)
            {
                case OpertorType.None:
                    break;
                case OpertorType.EqualTo:
                    RetVal = " AND (" + SQL_GetNumericDataValue(DIServerType) + " = " + this._FromDataValue + ")";
                    break;
                case OpertorType.Between:
                    RetVal = " AND (" + SQL_GetNumericDataValue(DIServerType) + " BETWEEN " + this._FromDataValue + " AND " + this._ToDataValue + ")";
                    break;
                case OpertorType.GreaterThan:
                    RetVal = " AND (" + SQL_GetNumericDataValue(DIServerType) + " > " + this._FromDataValue + ")";
                    break;
                case OpertorType.LessThan:
                    RetVal = " AND (" + SQL_GetNumericDataValue(DIServerType) + " < " + this._ToDataValue + ")";
                    break;
                default:
                    break;
            }
            return RetVal;
        }

        private string SQL_GetNumericDataValue(DIServerType DIServerType)
        {
            string RetVal = string.Empty;
            try
            {
                switch (DIServerType)
                {
                    case DIServerType.SqlServer:
                     case DIServerType.SqlServerExpress:
                       RetVal = "CASE WHEN (PATINDEX('%[^0-9.]%', D." + Data.DataValue + ") > 0) THEN (CAST(LEFT(D." + Data.DataValue + ", PATINDEX('%[^0-9.]%', D." + Data.DataValue + ") - 1) AS Real)) ELSE (CAST(D." + Data.DataValue + " AS Real)) END";
                        break;
                    case DIServerType.MsAccess:
                        RetVal = "val(D." + Data.DataValue + ")";
                        break;
                    case DIServerType.Oracle:
                    case DIServerType.MySql:
                        RetVal = "D." + Data.DataValue;
                        break;
                    case DIServerType.Excel:
                        RetVal = "D." + Data.DataValue;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                RetVal = string.Empty;
            }
            return RetVal;
        }

        #endregion

        #endregion
    }
}
