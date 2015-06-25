using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Drawing;

using DevInfo.Lib.DI_LibBAL.UI.Presentations;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.Common;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.Graph;

using DevInfo.Lib.DI_LibBAL.UI.Presentations.Table;
using SpreadsheetGear;
using SpreadsheetGear.Charts;


namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Common
{
    /// <summary>
    /// Class to create the different template style
    /// </summary>
    /// <remarks>
    /// Properties has been exposed for each Styling Element like Title, SubTitle .... etc
    /// Default values for these Properties are set internally. Earlier it was done through None.xml file
    /// </remarks>
    public class StyleTemplate : ICloneable
    {
        #region " -- Public -- "

        #region " -- New / Dispose -- "

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="presentationType">Presentation type to be used</param>
        /// <param name="showExcel"></param>
        public StyleTemplate(Presentation.PresentationType presentationType, bool showExcel)
        {
            FontSetting TempFontSetting;
            switch (presentationType)
            {
                case Presentation.PresentationType.Table:
                    TempFontSetting = new FontSetting("Arial", FontStyle.Bold, 11, Color.Black, Color.White, StringAlignment.Near, false, false, FootNoteDisplayStyle.Separate);
                    this._TitleSetting = new StyleTemplateSetting(TempFontSetting, true, string.Empty, LineDrawStyle.Solid, 0, TextOrientation.Horizontal, 0, false, LegendPosition.Left, StringAlignment.Center, 0, string.Empty, false);

                    TempFontSetting = new FontSetting("Arial", FontStyle.Bold, 8, Color.Black, Color.White, StringAlignment.Near, false, false, FootNoteDisplayStyle.Separate);
                    this._SubTitleSetting = new StyleTemplateSetting(TempFontSetting, true, string.Empty, LineDrawStyle.Solid, 0, TextOrientation.Horizontal, 0, false, LegendPosition.Left, StringAlignment.Center, 0, string.Empty, false);

                    // -- Legend settings
                    TempFontSetting = new FontSetting("Arial", FontStyle.Regular, 8, Color.Black, Color.White, StringAlignment.Center, true, false, FootNoteDisplayStyle.Inline);
                    this._Legends = new StyleTemplateSetting(TempFontSetting, true, string.Empty, LineDrawStyle.Solid, 0, TextOrientation.Horizontal, 0, false, LegendPosition.Right, StringAlignment.Center, 0, string.Empty, false);

                    break;

                case Presentation.PresentationType.Graph:
                    if (showExcel)
                    {
                        //-- Title Font Settings
                        TempFontSetting = new FontSetting("Arial", FontStyle.Bold, 14, Color.Black, Color.White, StringAlignment.Center);
                        //-- Set chart title
                        this._TitleSetting = new StyleTemplateSetting(TempFontSetting, true, "#00000", LineDrawStyle.Solid, 0, TextOrientation.Horizontal, 0, false, LegendPosition.Top, StringAlignment.Center, 0, "#111111", false);

                        //-- sub Title Font Settings
                        TempFontSetting = new FontSetting("Arial", FontStyle.Regular, 10, Color.Black, Color.White, StringAlignment.Center);
                        this._SubTitleSetting = new StyleTemplateSetting(TempFontSetting, true, string.Empty, LineDrawStyle.Solid, 0, TextOrientation.Horizontal, 0, false, LegendPosition.Left, StringAlignment.Center, 0, string.Empty, false);

                        //-- Set chart legends                        
                        TempFontSetting = new FontSetting("Arial", FontStyle.Regular, 10, Color.Black, Color.White, StringAlignment.Center, true, false, FootNoteDisplayStyle.Inline);
                        this._Legends = new StyleTemplateSetting(TempFontSetting, true, Color.Black.Name, LineDrawStyle.Solid, 2, TextOrientation.Horizontal, 0, false, LegendPosition.Right, StringAlignment.Center, 60, "#E3E3E3", false);
                    }
                    else
                    {
                        //-- Set chart title
                        TempFontSetting = new FontSetting("Arial", FontStyle.Bold, 14, Color.Black, Color.White, StringAlignment.Center);
                        this._TitleSetting = new StyleTemplateSetting(TempFontSetting, true, "#000000", LineDrawStyle.Solid, 0, TextOrientation.Horizontal, 0, false, LegendPosition.Top, StringAlignment.Center, 0, "#111111", false);

                        TempFontSetting = new FontSetting("Arial", FontStyle.Regular, 8, Color.Black, Color.White, StringAlignment.Center);
                        this._SubTitleSetting = new StyleTemplateSetting(TempFontSetting, true, string.Empty, LineDrawStyle.Solid, 0, TextOrientation.Horizontal, 0, false, LegendPosition.Left, StringAlignment.Center, 0, string.Empty, false);

                        TempFontSetting = new FontSetting("Arial", FontStyle.Regular, 8, Color.Black, Color.White, StringAlignment.Center, true, false, FootNoteDisplayStyle.Inline);
                        this._Legends = new StyleTemplateSetting(TempFontSetting, true, Color.Black.Name, LineDrawStyle.Solid, 2, TextOrientation.Horizontal, 0, false, LegendPosition.Right, StringAlignment.Center, 60, "#E3E3E3", false);

                    }
                    break;

                case Presentation.PresentationType.Map:
                    TempFontSetting = new FontSetting("Arial", FontStyle.Bold, 14, Color.Black, Color.White, StringAlignment.Center, false, false, FootNoteDisplayStyle.Separate);
                    this._TitleSetting = new StyleTemplateSetting(TempFontSetting, true, string.Empty, LineDrawStyle.Solid, 0, TextOrientation.Horizontal, 0, false, LegendPosition.Left, StringAlignment.Center, 0, string.Empty, false);

                    TempFontSetting = new FontSetting("Arial", FontStyle.Regular, 12, Color.Black, Color.White, StringAlignment.Center, false, false, FootNoteDisplayStyle.Separate);
                    this._SubTitleSetting = new StyleTemplateSetting(TempFontSetting, true, string.Empty, LineDrawStyle.Solid, 0, TextOrientation.Horizontal, 0, false, LegendPosition.Left, StringAlignment.Center, 0, string.Empty, false);

                    TempFontSetting = new FontSetting("Arial", FontStyle.Regular, 8, Color.Black, Color.White, StringAlignment.Center, false, false, FootNoteDisplayStyle.Separate);
                    this._Legends = new StyleTemplateSetting(TempFontSetting, false, "#ffffff", LineDrawStyle.Solid, 0, TextOrientation.Horizontal, 0, false, LegendPosition.Right, StringAlignment.Center, 60, "#E3E3E3", false);
                    break;
                case Presentation.PresentationType.None:
                    break;
                default:
                    break;
            }

            // -- Column Settings
            TempFontSetting = new FontSetting("Arial", FontStyle.Bold, 8, ColorTranslator.FromHtml("#000000"),true, ColorTranslator.FromHtml("#D7D7D7"), StringAlignment.Center, true, false, FootNoteDisplayStyle.Inline, false, ColorTranslator.ToHtml(Color.White), ColorTranslator.ToHtml(Color.White), true, 22, false);
            this._ColumnSetting = new StyleTemplateSetting(TempFontSetting, true, string.Empty, LineDrawStyle.Solid, 0, TextOrientation.Horizontal, 0, false, LegendPosition.Left, StringAlignment.Center,0, string.Empty, false);

            // -- Row settings
            TempFontSetting = new FontSetting("Arial", FontStyle.Regular, 8, ColorTranslator.FromHtml("#000000"), true, ColorTranslator.FromHtml("#F5F5F5"), StringAlignment.Near, true, false, FootNoteDisplayStyle.Inline, false, ColorTranslator.ToHtml(Color.White), ColorTranslator.ToHtml(Color.White), true, 10, false);
            this._RowSetting = new StyleTemplateSetting(TempFontSetting, true, string.Empty, LineDrawStyle.Solid, 0, TextOrientation.Horizontal, 0, false, LegendPosition.Left, StringAlignment.Center, 0, string.Empty, false);

            // -- Content settings
            TempFontSetting = new FontSetting("Arial", FontStyle.Regular, 8, Color.Black, true, Color.White, StringAlignment.Far, true, false, FootNoteDisplayStyle.Inline, false, ColorTranslator.ToHtml(Color.White), ColorTranslator.ToHtml(Color.White), true, 10, false);
            this._ContentSetting = new StyleTemplateSetting(TempFontSetting, true, string.Empty, LineDrawStyle.Solid, 0, TextOrientation.Horizontal, 0, false, LegendPosition.Left, StringAlignment.Center, 0, string.Empty, false);

            // -- Sub aggregate settings
            TempFontSetting = new FontSetting("Arial", FontStyle.Bold, 8, ColorTranslator.FromHtml("#804040"), ColorTranslator.FromHtml("#BBDDFF"), StringAlignment.Center);
            this._SubAggregateSetting = new StyleTemplateSetting(TempFontSetting, true, string.Empty, LineDrawStyle.Solid, 0, TextOrientation.Horizontal, 0, false, LegendPosition.Left, StringAlignment.Center, 0, string.Empty, false);

            // -- Group Aggregate settings
            TempFontSetting = new FontSetting("Arial", FontStyle.Bold, 8, Color.White, ColorTranslator.FromHtml("#0000A0"), StringAlignment.Center);
            this._GroupAggregateSetting = new StyleTemplateSetting(TempFontSetting, true, string.Empty, LineDrawStyle.Solid, 0, TextOrientation.Horizontal, 0, false, LegendPosition.Left, StringAlignment.Center, 0, string.Empty, false);

            // -- Group Header settings
            TempFontSetting = new FontSetting("Arial", FontStyle.Bold, 9, ColorTranslator.FromHtml("#400000"), ColorTranslator.FromHtml("#FFFF80"), StringAlignment.Center);
            this._GroupHeaderSetting = new StyleTemplateSetting(TempFontSetting, true, string.Empty, LineDrawStyle.Solid, 0, TextOrientation.Horizontal, 0, false, LegendPosition.Left, StringAlignment.Center, 0, string.Empty, false);

            // -- Footnotes settings
            TempFontSetting = new FontSetting("Arial", FontStyle.Regular, 7, ColorTranslator.FromHtml("#8080FF"), Color.White, StringAlignment.Center, true, false, FootNoteDisplayStyle.Separate);
            this._Footnotes = new StyleTemplateSetting(TempFontSetting, true, string.Empty, LineDrawStyle.Solid, 0, TextOrientation.Horizontal, 0, false, LegendPosition.Left, StringAlignment.Center, 0, string.Empty, false);

            // -- Comments settings
            TempFontSetting = new FontSetting("Arial", FontStyle.Regular, 7, ColorTranslator.FromHtml("#408080"), Color.White, StringAlignment.Center, true, false, FootNoteDisplayStyle.Separate);
            this._Comments = new StyleTemplateSetting(TempFontSetting, true, string.Empty, LineDrawStyle.Solid, 0, TextOrientation.Horizontal, 0, false, LegendPosition.Left, StringAlignment.Center, 0, string.Empty, false);

            // -- Denominator settings
            TempFontSetting = new FontSetting("Arial", FontStyle.Regular, 8, Color.Black, Color.White, StringAlignment.Center, false, false, FootNoteDisplayStyle.Separate);
            this._Denominator = new StyleTemplateSetting(TempFontSetting,false, string.Empty, LineDrawStyle.Solid, 0, TextOrientation.Horizontal, 0, false, LegendPosition.Left, StringAlignment.Center, 0, string.Empty, false);

            // -- Disclaimer font settings
            TempFontSetting = new FontSetting("Microsoft Sans Serif", FontStyle.Regular, 8, Color.Black, Color.White, StringAlignment.Center, false, false, FootNoteDisplayStyle.Separate);
            //TempFontSetting = new FontSetting("Microsoft Sans Serif", FontStyle.Regular, 8, Color.Black, Color.White, StringAlignment.Center, "Note: The boundaries and the names shown and the designations used on these maps do not imply official endorsement or acceptance by the United Nations.", false, false, FootNoteDisplayStyle.Separate);
            this._DisclaimerFont = new StyleTemplateSetting(TempFontSetting, true, string.Empty, LineDrawStyle.Solid, 0, TextOrientation.Horizontal, 0, false, LegendPosition.Left, StringAlignment.Center, 0, string.Empty, false);

            // -- Legend title settings
            TempFontSetting = new FontSetting("Microsoft Sans Serif", FontStyle.Bold, 10, Color.Black, Color.Black, StringAlignment.Center, false, false, FootNoteDisplayStyle.Separate);
            this._LegendTitle = new StyleTemplateSetting(TempFontSetting, true, string.Empty, LineDrawStyle.Solid, 0, TextOrientation.Horizontal, 0, false, LegendPosition.Right, StringAlignment.Center, 0, string.Empty, false);

            // -- Legend body settings
            TempFontSetting = new FontSetting("Microsoft Sans Serif", FontStyle.Regular, 8, Color.Black, Color.Black, StringAlignment.Center, false, false, FootNoteDisplayStyle.Separate);
            this._LegendBody = new StyleTemplateSetting(TempFontSetting, true, string.Empty, LineDrawStyle.Solid, 0, TextOrientation.Horizontal, 0, false, LegendPosition.Right, StringAlignment.Center, 0, string.Empty, false);

            // -- Legend body settings
            TempFontSetting = new FontSetting("Microsoft Sans Serif", FontStyle.Regular, 8, Color.Black, Color.Black, StringAlignment.Center, false, false, FootNoteDisplayStyle.Separate);
            this._LegendBody = new StyleTemplateSetting(TempFontSetting, true, string.Empty, LineDrawStyle.Solid, 0, TextOrientation.Horizontal, 0, false, LegendPosition.Left, StringAlignment.Center, 0, string.Empty, false);

            // -- Theme Label settings
            TempFontSetting = new FontSetting("Arial", FontStyle.Regular, 8, Color.Black, Color.Black, StringAlignment.Center, false, false, FootNoteDisplayStyle.Separate);
            this._ThemeLabel = new StyleTemplateSetting(TempFontSetting, true, string.Empty, LineDrawStyle.Solid, 0, TextOrientation.Horizontal, 0, false, LegendPosition.Left, StringAlignment.Center, 0, string.Empty, false);

            // -- Label Font settings
            TempFontSetting = new FontSetting("Arial", FontStyle.Regular, 8, Color.Black, Color.Black, StringAlignment.Center, false, false, FootNoteDisplayStyle.Separate);
            this._LabelFontSetting = new StyleTemplateSetting(TempFontSetting, true, string.Empty, LineDrawStyle.Solid, 0, TextOrientation.Horizontal, 0, false, LegendPosition.Left, StringAlignment.Center, 0, string.Empty, false);

            //-- Set chart Plot area
            FontSetting PlotFont = new FontSetting("Arial", FontStyle.Regular, 5, Color.Black, true, Color.White, StringAlignment.Center, true, false, FootNoteDisplayStyle.Inline, false, ColorTranslator.ToHtml(Color.White), ColorTranslator.ToHtml(Color.White), false, 21, false, FontSetting.CellBorderStyle.Fill, ColorTranslator.ToHtml(Color.White), false, 0);
            this._PlotArea = new StyleTemplateSetting(PlotFont, true, ColorTranslator.ToHtml(Color.White), LineDrawStyle.Solid, 0, TextOrientation.Custom, 45, false, LegendPosition.Right, StringAlignment.Center, 60, "#E3E3E3", false);

            //-- Set chart area
            FontSetting ChartFont = new FontSetting("Arial", FontStyle.Regular, 5, Color.Black, true, Color.White, StringAlignment.Center, true, false, FootNoteDisplayStyle.Inline, false, ColorTranslator.ToHtml(Color.White), ColorTranslator.ToHtml(Color.White), false, 21, false, FontSetting.CellBorderStyle.Fill, ColorTranslator.ToHtml(Color.White), false, 0);
            this._ChartArea = new StyleTemplateSetting(ChartFont, false, "#ffffff", LineDrawStyle.Solid, 0, TextOrientation.Horizontal, 0, false, LegendPosition.Right, StringAlignment.Center, 60, "#E3E3E3", false);

            if (showExcel && presentationType == Presentation.PresentationType.Graph)
            {
                //-- Set chart border
                FontSetting TitleFont = new FontSetting("Arial", FontStyle.Regular, 10, Color.Black, Color.White, StringAlignment.Center);
                this._Border = new StyleTemplateSetting(TitleFont, false, "#000000", LineDrawStyle.Solid, 0, TextOrientation.Horizontal, 0, false, LegendPosition.Top, StringAlignment.Center, 0, "#111111", false);

                //-- Set chart grid
                TitleFont = new FontSetting("Arial", FontStyle.Regular, 10, Color.Black, Color.White, StringAlignment.Center);
                this._Grid = new StyleTemplateSetting(TitleFont, true, "#E9E9E9", LineDrawStyle.Solid, 1, TextOrientation.Horizontal, 0, false, LegendPosition.Top, StringAlignment.Center, 0, "#111111", false);

                //-- Set chart xaxis
                FontSetting AxisFont = new FontSetting("Arial", FontStyle.Regular, 8, Color.Black, Color.White, StringAlignment.Center);
                this._XAxis = new StyleTemplateSetting(AxisFont, true, "#000000", LineDrawStyle.Solid, 1, TextOrientation.Custom, 45, false, LegendPosition.Top, StringAlignment.Center, 10, "#111111", true);

                //-- Set chart y axis
                AxisFont = new FontSetting("Arial", FontStyle.Regular, 8, Color.Black, Color.White, StringAlignment.Center);
                this._YAxis = new StyleTemplateSetting(AxisFont, true, "#000000", LineDrawStyle.Solid, 1, TextOrientation.Horizontal, 90, false, LegendPosition.Top, StringAlignment.Center, 60, "#111111", true);

                //-- Set chart xaxis series label
                AxisFont = new FontSetting("Arial", FontStyle.Regular, 8, Color.Black, Color.White, StringAlignment.Center, false, false, FootNoteDisplayStyle.Inline);
                this._XAxisSeriesLabel = new StyleTemplateSetting(AxisFont, true, "#ffffff", LineDrawStyle.Solid, 0, TextOrientation.Custom, 45, false, LegendPosition.Right, StringAlignment.Center, 60, "#E3E3E3", false);

                //-- Set chart yaxis series label
                AxisFont = new FontSetting("Arial", FontStyle.Regular, 8, Color.Black, Color.White, StringAlignment.Center, false, false, FootNoteDisplayStyle.Inline);
                this._YAxisSeriesLabel = new StyleTemplateSetting(AxisFont, false, "#ffffff", LineDrawStyle.Solid, 0, TextOrientation.Custom, 45, false, LegendPosition.Right, StringAlignment.Center, 60, "#E3E3E3", false);
            }
            else
            {
                //-- Set chart border
                FontSetting TitleFont = new FontSetting("Arial", FontStyle.Regular, 8, Color.Black, Color.White, StringAlignment.Center);
                this._Border = new StyleTemplateSetting(TitleFont, false, "#000000", LineDrawStyle.Solid, 0, TextOrientation.Horizontal, 0, false, LegendPosition.Top, StringAlignment.Center, 0, "#111111", false);

                //-- Set chart grid
                TitleFont = new FontSetting("Arial", FontStyle.Regular, 8, Color.Black, Color.White, StringAlignment.Center);
                this._Grid = new StyleTemplateSetting(TitleFont, false, "#E9E9E9", LineDrawStyle.Solid, 1, TextOrientation.Horizontal, 0, false, LegendPosition.Top, StringAlignment.Center, 0, "#111111", false);

                //-- Set chart xaxis
                FontSetting AxisFont = new FontSetting("Arial", FontStyle.Regular, 8, Color.Black, Color.White, StringAlignment.Center);
                this._XAxis = new StyleTemplateSetting(AxisFont, true, "#000000", LineDrawStyle.Solid, 1, TextOrientation.Custom, 45, false, LegendPosition.Top, StringAlignment.Center, 10, "#111111", true);

                //-- Set chart y axis
                AxisFont = new FontSetting("Arial", FontStyle.Regular, 8, Color.Black, Color.White, StringAlignment.Center);
                this._YAxis = new StyleTemplateSetting(AxisFont, true, "#000000", LineDrawStyle.Solid, 1, TextOrientation.Horizontal, 0, false, LegendPosition.Top, StringAlignment.Center, 60, "#111111", true);

                //-- Set chart xaxis series label
                AxisFont = new FontSetting("Arial", FontStyle.Regular, 5, Color.Black, Color.White, StringAlignment.Center, false, false, FootNoteDisplayStyle.Inline);
                this._XAxisSeriesLabel = new StyleTemplateSetting(AxisFont, true, "#ffffff", LineDrawStyle.Solid, 0, TextOrientation.Custom, 45, false, LegendPosition.Right, StringAlignment.Center, 60, "#E3E3E3", false);

                //-- Set chart yaxis series label
                AxisFont = new FontSetting("Arial", FontStyle.Regular, 5, Color.Black, Color.White, StringAlignment.Center, false, false, FootNoteDisplayStyle.Inline);
                this._YAxisSeriesLabel = new StyleTemplateSetting(AxisFont, false, "#ffffff", LineDrawStyle.Solid, 0, TextOrientation.Horizontal, 0, false, LegendPosition.Right, StringAlignment.Center, 60, "#E3E3E3", false);
            }
        }

        /// <summary>
        /// Constructor only for serilization purpose.
        /// </summary>
        public StyleTemplate()
            : this(Presentation.PresentationType.Table, true)
        {
            // Do nothing
        }

        #endregion

        #region " -- Properties -- "

        private StyleTemplateSetting _TitleSetting;
        /// <summary>
        /// Gets or sets the format style of title.
        /// </summary>
        public StyleTemplateSetting TitleSetting
        {
            get
            {
                return this._TitleSetting;
            }
            set
            {
                this._TitleSetting = value;
            }
        }

        private StyleTemplateSetting _SubTitleSetting;
        /// <summary>
        /// Gets or sets the format style of subtitle.
        /// </summary>
        public StyleTemplateSetting SubTitleSetting
        {
            get
            {
                return this._SubTitleSetting;
            }
            set
            {
                this._SubTitleSetting = value;
            }
        }

        private StyleTemplateSetting _ColumnSetting;
        /// <summary>
        /// Gets or sets the format style of columns.
        /// </summary>
        public StyleTemplateSetting ColumnSetting
        {
            get
            {
                return this._ColumnSetting;
            }
            set
            {
                this._ColumnSetting = value;
            }
        }

        private StyleTemplateSetting _RowSetting;
        /// <summary>
        /// Gets or sets the format style of rows.
        /// </summary>
        public StyleTemplateSetting RowSetting
        {
            get
            {
                return this._RowSetting;
            }
            set
            {
                this._RowSetting = value;
            }
        }

        private StyleTemplateSetting _ContentSetting;
        /// <summary>
        /// Gets or sets the format style of contents.
        /// </summary>
        public StyleTemplateSetting ContentSetting
        {
            get
            {
                return this._ContentSetting;
            }
            set
            {
                this._ContentSetting = value;
            }
        }

        private StyleTemplateSetting _SubAggregateSetting;
        /// <summary>
        /// Gets or sets the format style of sub aggregates.
        /// </summary>
        public StyleTemplateSetting SubAggregateSetting
        {
            get
            {
                return _SubAggregateSetting;
            }
            set
            {
                _SubAggregateSetting = value;
            }
        }

        private StyleTemplateSetting _GroupAggregateSetting;
        /// <summary>
        /// Gets or sets the format style of group aggregates.
        /// </summary>
        public StyleTemplateSetting GroupAggregateSetting
        {
            get
            {
                return this._GroupAggregateSetting;
            }
            set
            {
                this._GroupAggregateSetting = value;
            }
        }

        private StyleTemplateSetting _GroupHeaderSetting;
        /// <summary>
        /// Gets or sets the format style of group header settings.
        /// </summary>
        public StyleTemplateSetting GroupHeaderSetting
        {
            get
            {
                return this._GroupHeaderSetting;
            }
            set
            {
                this._GroupHeaderSetting = value;
            }
        }

        private StyleTemplateSetting _Footnotes;
        /// <summary>
        /// Gets or sets the format style of footnotes.
        /// </summary>
        public StyleTemplateSetting Footnotes
        {
            get
            {
                return this._Footnotes;
            }
            set
            {
                this._Footnotes = value;
            }
        }

        private StyleTemplateSetting _Comments;
        /// <summary>
        /// Gets or sets the format style of comments.
        /// </summary>
        public StyleTemplateSetting Comments
        {
            get
            {
                return _Comments;
            }
            set
            {
                _Comments = value;
            }
        }

        private StyleTemplateSetting _Denominator;
        /// <summary>
        /// Gets or sets the format style of Denominator.
        /// </summary>
        public StyleTemplateSetting Denominator
        {
            get
            {
                return this._Denominator;
            }
            set
            {
                this._Denominator = value;
            }
        }

        private StyleTemplateSetting _Legends;
        /// <summary>
        /// Gets or sets the format style of legends.
        /// </summary>
        public StyleTemplateSetting Legends
        {
            get
            {
                return this._Legends;
            }
            set
            {
                this._Legends = value;
            }
        }

        #region "-- Map -- "

        private StyleTemplateSetting _DisclaimerFont;
        /// <summary>
        /// Gets or sets Disclaimer GraphTemplateStyles.
        /// </summary>
        /// <remarks>This property is added for usage in Map class. (E.g. Map.Disclaimer)</remarks>
        public StyleTemplateSetting DisclaimerFont
        {
            get
            {
                return this._DisclaimerFont;
            }
            set
            {
                this._DisclaimerFont = value;
            }
        }

        private StyleTemplateSetting _LegendTitle;
        /// <summary>
        /// Gets or sets LegendTitle Font.
        /// </summary>
        /// <remarks>This property is added for usage in Map class.</remarks>
        public StyleTemplateSetting LegendTitle
        {
            get
            {
                return this._LegendTitle;
            }
            set
            {
                this._LegendTitle = value;
            }
        }

        private StyleTemplateSetting _LegendBody;
        /// <summary>
        /// Gets or sets LegendBody GraphTemplateStyles.
        /// </summary>
        /// <remarks>This property is added for usage in Map class. (E.g. Map.LegendBody)</remarks>
        public StyleTemplateSetting LegendBody
        {
            get
            {
                return this._LegendBody;
            }
            set
            {
                this._LegendBody = value;
            }
        }

        private StyleTemplateSetting _ThemeLabel;
        /// <summary>
        /// Gets or sets ThemeLabel GraphTemplateStyles.
        /// </summary>
        /// <remarks>This property is added for usage in Map class. (E.g. Map.ThemeLabel)</remarks>
        public StyleTemplateSetting ThemeLabel
        {
            get
            {
                return this._ThemeLabel;
            }
            set
            {
                this._ThemeLabel = value;
            }
        }

        private StyleTemplateSetting _LabelFontSetting;
        /// <summary>
        /// Gets or sets LabelFontSetting GraphTemplateStyles.
        /// </summary>
        /// <remarks>This property is added for usage in Map class. (E.g. Map.LabelFontSetting)</remarks>
        public StyleTemplateSetting LabelFontSetting
        {
            get
            {
                return this._LabelFontSetting;
            }
            set
            {
                this._LabelFontSetting = value;
            }
        }


        #endregion

        #region " -- Graph -- "

        private StyleTemplateSetting _Border;
        /// <summary>
        /// Gets or sets the Border settings
        /// </summary>
        public StyleTemplateSetting Border
        {
            get
            {
                return this._Border;
            }
            set
            {
                this._Border = value;
            }
        }

        private StyleTemplateSetting _Grid;
        /// <summary>
        /// Gets or sets the grid settings
        /// </summary>
        public StyleTemplateSetting Grid
        {
            get
            {
                return this._Grid;
            }
            set
            {
                this._Grid = value;
            }
        }

        private StyleTemplateSetting _YAxis;
        /// <summary>
        /// Gets or sets the y axis settings
        /// </summary>
        public StyleTemplateSetting YAxis
        {
            get
            {
                return this._YAxis;
            }
            set
            {
                this._YAxis = value;
            }
        }

        private StyleTemplateSetting _XAxis;
        /// <summary>
        /// Gets or sets the x axis settings
        /// </summary>
        public StyleTemplateSetting XAxis
        {
            get
            {
                return this._XAxis;
            }
            set
            {
                this._XAxis = value;
            }
        }

        private StyleTemplateSetting _XAxisSeriesLabel;
        /// <summary>
        /// Gets or sets the x axis seris label settings
        /// </summary>
        public StyleTemplateSetting XAxisSeriesLabel
        {
            get
            {
                return this._XAxisSeriesLabel;
            }
            set
            {
                this._XAxisSeriesLabel = value;
            }
        }


        private StyleTemplateSetting _YAxisSeriesLabel;
        /// <summary>
        /// Gets or sets the y axis series label settings
        /// </summary>
        public StyleTemplateSetting YAxisSeriesLabel
        {
            get
            {
                return this._YAxisSeriesLabel;
            }
            set
            {
                this._YAxisSeriesLabel = value;
            }
        }

        private StyleTemplateSetting _PlotArea;
        /// <summary>
        /// Gets or sets the chart plot area settings
        /// </summary>
        public StyleTemplateSetting PlotArea
        {
            get
            {
                return this._PlotArea;
            }
            set
            {
                this._PlotArea = value;
            }
        }

        private StyleTemplateSetting _ChartArea;
        /// <summary>
        /// Gets or sets the chart area settings
        /// </summary>
        public StyleTemplateSetting ChartArea
        {
            get
            {
                return this._ChartArea;
            }
            set
            {
                this._ChartArea = value;
            }
        }

        #endregion


        private bool _ShowBorderLines = true;
        /// <summary>
        /// Gets or sets the border lines
        /// <remarks> If it is true, border lines will appear in preview Step 6.</remarks>
        /// </summary>
        public bool ShowBorderLines
        {
            get
            {
                return this._ShowBorderLines;
            }
            set
            {
                this._ShowBorderLines = value;
            }
        }

        private string _BorderLineColor = ColorTranslator.ToHtml(Color.Black);
        /// <summary>
        /// Gets or sers the border line color
        /// </summary>
        public string BorderLineColor
        {
            get
            {
                return this._BorderLineColor;
            }
            set
            {
                this._BorderLineColor = value;
            }
        }

        private AreaLevelFormats _LevelFormat = new AreaLevelFormats();
        /// <summary>
        /// 
        /// </summary>
        public AreaLevelFormats LevelFormat
        {
            get 
            {
                return this._LevelFormat; 
            }
            set 
            {
                this._LevelFormat = value; 
            }
        }
	

        #endregion

        #region " -- Constants -- "

        /// <summary>
        /// None
        /// </summary>
        public const string DEFAULT_TEMPLATE_FILE_NAME = "None";

        #endregion

        #region " -- Methods -- "
						
        /// <summary>
        /// Save the template in form of XML file.
        /// </summary>
        /// <param name="fileNameWPath">file Name with path</param>
        /// <param name="styleTemplate">Object of StyleTemplate type</param>
        public void SaveStyleTemplate(string fileNameWPath, StyleTemplate styleTemplate)
        {
            try
            {
                XmlSerializer TemplateSerialize = new XmlSerializer(typeof(StyleTemplate));
                StreamWriter TemplateWriter = new StreamWriter(fileNameWPath);
                TemplateSerialize.Serialize(TemplateWriter, styleTemplate);
                TemplateWriter.Close();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Load the template XML file
        /// </summary>
        /// <param name="fileNameWPath">file Name with path</param>
        /// <returns>Object of StyleTemplate type</returns>
        public static StyleTemplate LoadStyleTemplate(string fileNameWPath)
        {
            StyleTemplate RetVal;
            try
            {
                XmlSerializer TemplateSerialize = new XmlSerializer(typeof(StyleTemplate));
                TextReader TemplateReader = new StreamReader(fileNameWPath);
                RetVal = (StyleTemplate)TemplateSerialize.Deserialize(TemplateReader);
                TemplateReader.Close();
            }
            catch (Exception)
            {
                RetVal = null;
            }
            return RetVal;
        }

        /// <summary>
        /// Delete the Template XML file
        /// </summary>
        /// <param name="fileNameWPath">file Name with path</param>
        /// <returns>True, if file is deleted. False in case of file is not deleted</returns>
        public static bool DeleteStyleTemplate(string fileNameWPath)
        {
            bool RetVal = false;
            try
            {
                if (File.Exists(fileNameWPath) == true)
                {
                    File.Delete(fileNameWPath);
                    RetVal = true;
                }
            }
            catch (Exception)
            {
                RetVal = false;
            }
            return RetVal;
        }

        #endregion

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            StyleTemplate RetVal;
            try
            {
                XmlSerializer XmlSerializer = new XmlSerializer(typeof(StyleTemplate));
                MemoryStream MemoryStream = new MemoryStream();
                XmlSerializer.Serialize(MemoryStream, this);
                MemoryStream.Position = 0;
                RetVal = (StyleTemplate)XmlSerializer.Deserialize(MemoryStream);
                MemoryStream.Close();
                MemoryStream.Dispose();
            }
            catch (Exception)
            {
                RetVal = null;
            }
            return RetVal;
        }

        #endregion
    }
}
