using System;
using System.Collections.Generic;
using System.Text;
using System.Data;


namespace DI7.Lib.BAL.HTMLGenerator
{
    public class HTMLTableGenerator
    {
        #region "-- Inner Class --"

        /// <summary>
        /// Defines the Display type of HTML table Row
        /// </summary>
        public enum DisplayType
        {
            RadioButtonType,
            CheckboxType
        }

        /// <summary>
        /// Defines the Css class name for Row, Cell , Table , etc.
        /// </summary>
        public class CssClassName
        {
            #region "-- Variables / Properties --"

            private string _Table = "DefaultTableStyle";
            /// <summary>
            /// Gets or sets table css class name
            /// </summary>
            public string Table
            {
                get { return this._Table; }
                set { this._Table = value; }
            }

            private string _DataRow = "DataRowStyle";
            /// <summary>
            /// Gets or sets css class name for data row
            /// </summary>
            public string DataRow
            {
                get { return this._DataRow; }
                set { this._DataRow = value; }
            }

            private string _CheckBoxDataColumn = "CheckBoxDataColumnStyle_crmpd";
            /// <summary>
            /// Gets or sets css class name for checkbox data column
            /// </summary>
            public string CheckBoxDataColumn
            {
                get { return this._CheckBoxDataColumn; }
                set { this._CheckBoxDataColumn = value; }
            }

            private string _FirstDataColumn = "DataColumnStyle_crmpd";
            /// <summary>
            /// Gets or sets css class name for first data column
            /// </summary>
            public string FirstDataColumn
            {
                get { return this._FirstDataColumn; }
                set { this._FirstDataColumn = value; }
            }


            private string _DataColumn = "DataColumnStyle_crmpd";
            /// <summary>
            /// Gets or sets css class name for data column
            /// </summary>
            public string DataColumn
            {
                get { return this._DataColumn; }
                set { this._DataColumn = value; }
            }

            private string _HeaderRow = "HeaderRowStyle";
            /// <summary>
            /// Gets or sets table header css class name
            /// </summary>
            public string HeaderRow
            {
                get { return this._HeaderRow; }
                set { this._HeaderRow = value; }
            }

            private string _HeaderColumn = "HeaderColumnStyle_crmpd";
            /// <summary>
            /// Gets or sets table header's column css class name
            /// </summary>
            public string HeaderColumn
            {
                get { return this._HeaderColumn; }
                set { this._HeaderColumn = value; }
            }

            private string _CheckBoxColumn = "CheckBoxColumnStyle_crmpd";
            /// <summary>
            /// Gets or sets css class name for check box column
            /// </summary>
            public string CheckBoxColumn
            {
                get { return this._CheckBoxColumn; }
                set { this._CheckBoxColumn = value; }
            }




            #endregion

        }

        #endregion

        #region "-- Variables / Properties --"

        private CssClassName _CssClass = new CssClassName();
        /// <summary>
        /// Gets or sets Css class for table
        /// </summary>
        public CssClassName CssClass
        {
            get { return this._CssClass; }
            set { this._CssClass = value; }
        }


        private int _Border = 0;
        /// <summary>
        /// Gets or sets table border. Default is 1.
        /// </summary>
        public int Border
        {
            get { return this._Border; }
            set { this._Border = value; }
        }

        private bool _ShowSorting = false;
        public bool ShowSorting
        {
            get { return this._ShowSorting; }
            set { this._ShowSorting = value; }
        } 

        private DisplayType _RowDisplayType = DisplayType.CheckboxType;
        /// <summary>
        /// Gets or set Row display type. By default row is checkbox type.
        /// </summary>
        public DisplayType RowDisplayType
        {
            get { return this._RowDisplayType; }
            set { this._RowDisplayType = value; }
        }

        #endregion

        #region "-- New/ Dispose--"

        public HTMLTableGenerator()
        {
            //do nothing
        }

        //public HTMLTableGenerator(string tableCssClassName, string rowCssClassName, string headerCssClassName, string columnCssClassName)
        //{

        //    //this.TableCssClassName = tableCssClassName;
        //    //this.RowCssClassName = rowCssClassName;
        //    //this.HeaderCssClassName = headerCssClassName;
        //    //this.ColumnCssClassName = columnCssClassName;

        //    this.CssClass.Table = tableCssClassName;
        //    this.CssClass.Row = rowCssClassName;
        //    this.CssClass.Header = headerCssClassName;
        //    this.CssClass.Column = columnCssClassName;
        //}

        #endregion

        #region "-- Methods --"
        /// <summary>
        /// Get Html Table
        /// </summary>
        /// <param name="table"></param>
        /// <param name="NIdColumnName"></param>
        /// <param name="IDPrefix"></param>
        /// <param name="addHeaderRow"></param>
        /// <returns></returns>
        public string GetTableHmtl(DataTable table, string NIdColumnName, string IDPrefix, bool addHeaderRow)
        {
            string RetVal = string.Empty;
            StringBuilder StrBuilder = new StringBuilder();

            // 1. add table tag
            StrBuilder.AppendLine("<TABLE border='" + this._Border + "' id='" + IDPrefix + "_Table'   cellspacing='0' cellpadding='0' style='width:100%;' >");
            StrBuilder.AppendLine("<TBODY id='tbody'>");

            // 2. add header row
            if (addHeaderRow)
            {
                StrBuilder.AppendLine(this.GetHeaderRow(table, NIdColumnName));
            }

            // 3. create and add each  row         
            foreach (DataRow Row in table.Rows)
            {
                StrBuilder.AppendLine(this.GetTableRow(Row, IDPrefix, NIdColumnName));
            }

            StrBuilder.AppendLine("</TBODY>");

            // 4. close table tag
            StrBuilder.AppendLine("</TABLE >");

            // 5. set return variable
            RetVal = StrBuilder.ToString();


            return RetVal;
        }

        /// <summary>
        /// Get Headers Row
        /// </summary>
        /// <param name="table"></param>
        /// <param name="NidColumnName"></param>
        /// <returns></returns>
        public string GetHeaderRow(DataTable table, string NidColumnName)
        {
            string RetVal = string.Empty;

            StringBuilder StrBuilder = new StringBuilder();

            // 1. add TR tag
            StrBuilder.Append("<TR class='" + this._CssClass.HeaderRow + "'>");

            // 2. add checkbox column
            StrBuilder.Append("<TD class='" + this._CssClass.CheckBoxColumn + "'>&nbsp;</TD>");


            // 3. add columns header except NID column
            foreach (DataColumn Column in table.Columns)
            {
                if (Column.ColumnName.ToUpper() != NidColumnName.ToUpper())
                {
                    StrBuilder.Append("<TD class='" + this._CssClass.HeaderColumn + "'>");
                    //StrBuilder.Append(Column.Caption);
                    StrBuilder.Append("<span class='flt_lft'>" + Column.Caption + "</span>");
                    if (_ShowSorting)
                    {
                        StrBuilder.Append("<div class='up_dwn_arrw_pos'>");
                        StrBuilder.Append("<div class='up_arrw'>");
                        StrBuilder.Append("<img onclick = \"Sort('" + "ASC[^^^^]_" + Column.Caption + "');\" id='ASC[^^^^]_" + Column.Caption + "' src='../../../stock/themes/default/images/spacer.gif' width='8' height='5' style='cursor:pointer;'>");

                        StrBuilder.Append("</div>");
                        StrBuilder.Append("<div class='down_arrw'>");
                        StrBuilder.Append("<img onclick = \"Sort('" + "DESC[^^^^]_" + Column.Caption + "');\" id='DESC[^^^^]_" + Column.Caption + "' src='../../../stock/themes/default/images/spacer.gif' width='10' height='5' style='cursor:pointer;'>");
                        StrBuilder.Append("</div>");
                        StrBuilder.Append("</div>");

                    }
                    StrBuilder.Append("</TD>");
                }
            }

            // 4. close TR tag
            StrBuilder.Append("</TR>");

            RetVal = StrBuilder.ToString();

            return RetVal;
        }

        /// <summary>
        /// Get Table Row Details
        /// </summary>
        /// <param name="row"></param>
        /// <param name="IDPrefix"></param>
        /// <param name="NIdColumnName"></param>
        /// <returns></returns>
        public string GetTableRow(DataRow row, string IDPrefix, string NIdColumnName)
        {
            string RetVal = string.Empty;
            StringBuilder StrBuilder = new StringBuilder();
            bool IsFirstColumnInserted = false;
            string CssName = string.Empty;

            // 1. add TR tag
            StrBuilder.Append("<TR class='" + this._CssClass.DataRow + "'>");

            // 2. checkbox  / radio button column
            if (this._RowDisplayType == DisplayType.CheckboxType)
            {
                // check box
                StrBuilder.Append(this.GetCheckBoxHTML(row, IDPrefix, NIdColumnName));
            }
            else
            {
                // radio button
                StrBuilder.Append(this.GetRadioButton(row, IDPrefix, NIdColumnName));
            }

            // 3. add  all columns except NId column
            foreach (DataColumn Column in row.Table.Columns)
            {
                if (Column.ColumnName.ToUpper() != NIdColumnName.ToUpper())
                {
                    if (!IsFirstColumnInserted)
                    {
                        // change css class name for only first column
                        CssName = this._CssClass.FirstDataColumn;
                        IsFirstColumnInserted = true;
                    }
                    else
                    {
                        // css class name for all columns except first column
                        CssName = this._CssClass.DataColumn;
                    }

                    StrBuilder.AppendLine(this.GetTableRowColumn(row, Column.ColumnName, CssName));
                }
            }

            // 4. close TR tag
            StrBuilder.Append("</TR>");

            RetVal = StrBuilder.ToString();

            return RetVal;
        }

        /// <summary>
        /// Get Row Html
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <param name="cssName"></param>
        /// <returns></returns>
        public string GetTableRowColumn(DataRow row, string columnName, string cssName)
        {
            string RetVal = string.Empty;
            string ColumnValue = string.Empty;
            StringBuilder StrBuilder = new StringBuilder();

            ColumnValue = row[columnName].ToString().Replace("\n", "<BR>");
            if (string.IsNullOrEmpty(ColumnValue))
            {
                ColumnValue = "&nbsp;";
            }

            StrBuilder.Append("<TD class='" + cssName + "' > ");
            StrBuilder.Append(ColumnValue.Replace("<", "&lt;").Replace(">", "&gt;"));
            StrBuilder.Append("</TD>");

            RetVal = StrBuilder.ToString();

            return RetVal;
        }

        /// <summary>
        /// Get Checkbox Html string
        /// </summary>
        /// <param name="row"></param>
        /// <param name="IDPrefix"></param>
        /// <param name="NIdColumnName"></param>
        /// <returns></returns>
        public string GetCheckBoxHTML(DataRow row, string IDPrefix, string NIdColumnName)
        {
            string RetVal = string.Empty;
            StringBuilder StrBuilder = new StringBuilder();

            StrBuilder.Append("<TD class='" + this._CssClass.CheckBoxDataColumn + "'>");

            StrBuilder.Append("<input type='CheckBox' id='" + IDPrefix + "_" + row[NIdColumnName].ToString() + "' />");

            StrBuilder.Append("</TD>");


            RetVal = StrBuilder.ToString();

            return RetVal;
        }

        /// <summary>
        /// Get Radio Button Html string
        /// </summary>
        /// <param name="row"></param>
        /// <param name="IDPrefix"></param>
        /// <param name="NIdColumnName"></param>
        /// <returns></returns>
        public string GetRadioButton(DataRow row, string IDPrefix, string NIdColumnName)
        {
            string RetVal = string.Empty;
            StringBuilder StrBuilder = new StringBuilder();

            StrBuilder.Append("<TD class='" + this._CssClass.CheckBoxDataColumn + "'>");

            StrBuilder.Append("<input type='Radio' id='" + IDPrefix + "_" + row[NIdColumnName].ToString() + "'  name='group1' />");

            StrBuilder.Append("</TD>");


            RetVal = StrBuilder.ToString();

            return RetVal;
        }

        #endregion

    }
}
