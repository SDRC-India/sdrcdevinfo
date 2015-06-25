using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;

using DevInfo.Lib.DI_LibBAL.UI.Presentations.Table;

namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Common.TableGraph
{

	/// <summary>
	/// defines the constants string related to field object.
	/// </summary>
	//public struct FieldId
	//{

	//    public const string INDICATOR = Indicator.IndicatorName;
	//    public const string AREAID = Area.AreaID;
	//    public const string AREANAME = Area.AreaName;
	//    public const string TIMEPERIOD = Timeperiods.TimePeriod;
	//    public const string UNIT = Unit.UnitName;
	//    public const string SOURCE = IndicatorClassifications.ICName;
	//    public const string SUBGROUP = SubgroupVals.SubgroupVal;
	//    public const string AGE = SubgroupSpecial.Subgroup_Name_Age;
	//    public const string SEX = SubgroupSpecial.Subgroup_Name_Sex;
	//    public const string LOCATION = SubgroupSpecial.Subgroup_Name_Location;
	//    public const string OTHERS = SubgroupSpecial.Subgroup_Name_Others;
	//    public const string DATA_VALUE = Data.DataValue;

	//    // Metadata Fileds to be added on the fly Convention MD_IND_1, MD_IND_3
	//    public const string METADATA_INDICATOR = TablePresentation.MetadataIndicator;
	//    public const string METADATA_AREA = TablePresentation.MetadataArea;
	//    public const string METADATA_SOURCE = TablePresentation.MetadataSource;

	//    // ICs Fileds to be added on the fly Convention CLS_GL_1...s
	//    public const string IC_GOAL = TablePresentation.ICGoal;
	//    public const string IC_SECTOR = TablePresentation.ICSector;
	//    public const string IC_SOURCE = TablePresentation.ICSource;
	//    public const string IC_THEME = TablePresentation.ICTheme;
	//    public const string IC_INSTITUTE = TablePresentation.ICInstitution;
	//    public const string IC_CONVENTION = TablePresentation.ICConvention;

	//    public const string DENOMINATOR = Data.DataDenominator;
	//    public const string NONE = "NONE";

	//    ////TODO: To replaced by DAL constant. Used only in pivot class
	//    //public const string SUBGROUP_AGE = "Subgroup_Age";              //SubgroupSpecial.Subgroup_Name_Age
	//    //public const string SUBGROUP_SEX = "Subgroup_Sex";
	//    //public const string SUBGROUP_LOCATION = "Subgroup_Location";
	//    //public const string SUBGROUP_OTHERS = "Subgroup_Others";

	//}

    /// <summary>
    /// Enum for Step 4 for field sorting type
    /// </summary>
    public enum OrderType
    {
        Asc=0,
        Desc=1
    }

    /// <summary>
    /// Defines the structure of field. ( e.g. Indicator,Area,TimePeriod)
    /// </summary>
    public class Field
    {

        #region "-- Private --"

        #region " -- New/Dispose -- "

        /// <summary>
        /// Private default constructor is needed to serailize and deserialize the class
        /// </summary>
        private Field()
        { 
        }

        #endregion


        #endregion

        #region "-- Internal / Public --"
        
        #region " -- New / Dispose -- "

        public Field(string FieldID, string Caption)//, FieldType FieldType)
		{
			this._FieldID = FieldID;
			this._Caption = Caption;
			//this._FieldType = FieldType;
        }

        #endregion

        #region "-- Variables --"

        /// <summary>
        /// Stores the field index in the collection
        /// </summary>
        internal int FieldIndex = 0;

        #endregion

        #region " -- Properties -- "

        #region " -- Step 1 --"

        private string _FieldID=string.Empty;
        /// <summary>
        /// Gets or sets the value for field id
        /// </summary>
        public string FieldID
        {
            get
            {
                return this._FieldID;
            }
            set
            {
                this._FieldID = value;
            }
        }

        private string _Caption = string.Empty;
        /// <summary>
        /// Gets or sets the value for language based caption
        /// </summary>
        /// <remarks>
        /// Language captions to be set by hosting application
        /// </remarks>
        public string Caption
        {
            get
            {
                return this._Caption;
            }
            set
            {
                this._Caption = value;
            }
        }

		//private FieldType _FieldType;
		///// <summary>
		///// Gets or sets the value for type
		///// </summary>
		//public FieldType FieldType
		//{
		//    get
		//    {
		//        return this._FieldType;
		//    }
		//    set
		//    {
		//        this._FieldType = value;
		//    }
		//}

        #endregion

        #region " -- Step 3 -- "

        private OrderType _SortType = OrderType.Asc;
        /// <summary>
        /// Get or sets the Sorting order of fields.
        /// <remarks>
        /// Order should be in the form of "ASC" or "DESC" 
        /// <para>Will be used for sorting items at step4 only</para>
        /// </remarks>
        /// </summary>
        public OrderType SortType
        {
            get 
            { 
                return this._SortType; 
            }
            set 
            {
                this._SortType = value; 
            }
        }
 

        #endregion

        #endregion

        #endregion
    }
}
