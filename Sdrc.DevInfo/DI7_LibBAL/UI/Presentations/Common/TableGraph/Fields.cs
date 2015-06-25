using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using DevInfo.Lib.DI_LibBAL.UI.Presentations.Table;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;

namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Common.TableGraph
{
    /// <summary>
    /// Declare the delegate to update the sort fields (Step 4)
    /// </summary>
    public delegate void ChangeSortDelegate();

    /// <summary>
    /// Declare the delgate to update the column arrangement, if there is any change in Column collection
    /// </summary>
    public delegate void UpdateColumnAggangementDelegate();

    /// <summary>
    /// Declare the delegate to change the aggregate field. 
    /// True for Add new item and false for remove the item from aggregate fields
    /// </summary>
    /// <param name="FieldID"></param>
    /// <param name="OpType"></param>
    public delegate void ChangeAggregateFieldDelegate(string FieldID,bool OpType);


	/// <summary>
    /// Exposes collections for rows,columns,available and all fields
    /// </summary> 	
    public class Fields
    {       
        #region "-- Private --"

		#region "-- Method --"

        /// <summary>
        /// Remove the metadata from the collection
        /// </summary>
        /// <param name="collectionField"></param>
        /// <param name="field"></param>
        private void RemoveMetadata(FieldsCollection collectionField, Field field)
        {
            string[] DeletedField = new string[0];
            string FieldIds=string.Empty;
            foreach (Field MetadataField in collectionField)
            {
                // Indicator metadata
                if (field.FieldID == Indicator.IndicatorName)
                {
                    if (MetadataField.FieldID.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.MetadataIndicator))
                    {
                        FieldIds = MetadataField.FieldID + ",";
                    }
                }
                // Area metadata
                else if (field.FieldID== Area.AreaID || field.FieldID == Area.AreaName)
                {
                    if (MetadataField.FieldID.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.MetadataArea))
                    {
                        FieldIds = MetadataField.FieldID + ",";
                    }
                }
                // Source metadata
				else if (field.FieldID == IndicatorClassifications.ICName)
                {
                    if (MetadataField.FieldID.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.MetadataSource))
                    {
                        FieldIds = MetadataField.FieldID + ",";
                    }
                }
            }
            //  delete the fields from list
            DeletedField = FieldIds.Split(",".ToCharArray());            
            for (int i = 0; i < DeletedField.Length - 1; i++)
            {
                if (!string.IsNullOrEmpty(DeletedField[i].Trim()))
                {
                    collectionField.Remove(collectionField[DeletedField[i]]);
                }
            }
        }	

		#endregion	
	
	    #region "-- RaiseEvent --"

        /// <summary>
        /// Raise the event to change the aggregate field
        /// </summary>
        /// <param name="FieldID">FieldId</param>
        /// <param name="OpType">True for insertion, false to remove the item </param>
	    private void RaiseChangeAggregateFieldEvent(string FieldID,bool OpType)
	    {
		    if (this.ChangeAggregateFieldEvent != null)
		    {
			    this.ChangeAggregateFieldEvent(FieldID,OpType);
		    }
	    }

        /// <summary>
        /// Raise the event to update the column arrangement, if there is any change in Column collection
        /// </summary>
        private void RaiseUpdateColumnAggangementEvent()
        {
            if (this.UpdateColumnAggangementEvent!= null)
            {
                this.UpdateColumnAggangementEvent();
            }
        }

        /// <summary>
        /// Raise the event to update the sort fields (Step 4).
        /// </summary>
        public void RaiseChangeSortEvent()
        {
            if (this.ChangeSortEvent != null)
            {
                this.ChangeSortEvent();
            }
        }

  		#endregion

        #endregion

        #region "-- Public --"

        #region " -- Properties -- "        

        private FieldsCollection _Rows;
		/// <summary>
		/// Get or Set the FieldCol for rows
		/// </summary>
		public FieldsCollection Rows
		{
			get			
			{					
				return _Rows;
			}
			set
			{
				_Rows = value;
			}
		}		

		private FieldsCollection _Columns;
		/// <summary>
		/// Get or Set the FieldCol for columns
		/// </summary>
		public FieldsCollection Columns
		{
			get
			{
				return _Columns;
			}
			set
			{
				_Columns = value;
			}
		}

		private FieldsCollection _Available;
		/// <summary>
		/// Get or Set the FieldCol for available
		/// </summary>
		public FieldsCollection Available
		{
			get
			{					
				return _Available;
			}
			set
			{
				_Available = value;
			}
		}

		private FieldsCollection _All;
		/// <summary>
		/// Get or Set the FieldCol for All
		/// </summary>
		public FieldsCollection All
		{
			get
			{
				return _All;
			}
			set
			{
				_All = value;
			}
		}

        private FieldsCollection _Sort;
        /// <summary>
        /// Gets or sets the fields sort collection.
        /// </summary>
        public FieldsCollection Sort
        {
            get 
            { 
                return this._Sort; 
            }
            set 
            { 
                this._Sort = value; 
            }
        }

        #endregion


        #region "-- Constructor --"

        /// <summary>
		/// Initializes the objects for FieldSources and attaches the event for them.
		/// </summary>
		public Fields()
		{

            //Initialize object for _Rows
            _Rows = new FieldsCollection(FieldSource.Rows);
            //Initialize object for _Columns
            _Columns = new FieldsCollection(FieldSource.Column);
            //Initialize object for _Available
            _Available = new FieldsCollection(FieldSource.Available);
            //Initialize object for _All
            _All = new FieldsCollection(FieldSource.All);

            //Intialize the object with this._Rows
            this._Sort = new FieldsCollection(FieldSource.Rows);

            //Attach event to _Rows
            _Rows.AfterAdded += new FieldAddedDelegate(_Rows_AfterAdded);
            //Attach event to _Columns
            _Columns.AfterAdded += new FieldAddedDelegate(_Columns_AfterAdded);
            //Attach event to _Available
            _Available.AfterAdded += new FieldAddedDelegate(_Available_AfterAdded);

            this._Columns.ColumnOrderChangeEvent += new ColumnOrderChangeDelegate(_Columns_ColumnOrderChangeEvent);
		}

        void _Columns_ColumnOrderChangeEvent()
        {
            this.RaiseUpdateColumnAggangementEvent();
        }

		
		#endregion
       

		#region "-- Event Handelers --"

		/// <summary>
        /// Handles event when item is added to available and delete this item from rows or column
		/// </summary>
		/// <param name="Source">Field source</param>
		/// <param name="ID">Field Id</param>
		void _Available_AfterAdded(FieldSource Source, string ID)
		{            
			Field Field;
			Field = Rows[ID];
          
			if (Field != null)
			{
                this.RaiseChangeAggregateFieldEvent(ID, false);
                // Remove metadata, if parent column is removed from the collection (Indicator, Area, Source)
				if (Field.FieldID == Indicator.IndicatorName || Field.FieldID == Area.AreaID || Field.FieldID == Area.AreaName || Field.FieldID == IndicatorClassifications.ICName)
                {
                    this.RemoveMetadata(this.Rows, Field);
                }				
				this._Rows.Remove(Field);                
			}

            // -- Remove the field from the sort collection
            Field = this.Sort[ID];
            if (Field != null)
            {
                this._Sort.Remove(Field);                
            }

            // -- Update the UI in step 4
            this.RaiseChangeSortEvent();

			Field = null;
			Field = Columns[ID];
			if (Field != null)
			{
                // Remove metadata, if parent column is removed from the collection (Indicator, Area, Source)
				if (Field.FieldID == Indicator.IndicatorName || Field.FieldID == Area.AreaID || Field.FieldID == Area.AreaName || Field.FieldID == IndicatorClassifications.ICName)
                {
                    this.RemoveMetadata(this.Columns, Field);
                }
				this._Columns.Remove(Field);
                this.RaiseUpdateColumnAggangementEvent();
			}
		}
	

		/// <summary>
		/// Handles event when item is added to column and delete this item from rows or available
		/// </summary>
		/// <param name="Source">Field source</param>
		/// <param name="ID">Field Id</param>
		void _Columns_AfterAdded(FieldSource Source, string ID)
		{
            this.RaiseUpdateColumnAggangementEvent();
            Field Field;
			Field = Rows[ID];
			if (Field != null)
			{
                this.RaiseChangeAggregateFieldEvent(ID, false);
                // Remove metadata, if parent column is removed from the collection (Indicator, Area, Source)
				if (Field.FieldID == Indicator.IndicatorName || Field.FieldID == Area.AreaID || Field.FieldID == Area.AreaName || Field.FieldID == IndicatorClassifications.ICName)
                {
                    this.RemoveMetadata(this.Rows, Field);
                }                
				this._Rows.Remove(Field);                
			}

            // -- Remove the field from the sort collection
            Field = this.Sort[ID];
            if (Field != null)
            {                
                this._Sort.Remove(Field);
            }

            // -- Update the UI in step 4
            this.RaiseChangeSortEvent();

			Field = null;
			Field = Available[ID];
			if (Field != null)
			{
                this._Available.Remove(Field);
			}
		}
		

		/// <summary>
        /// Handles event when item is added to rows and delete this item from column or available
		/// </summary>
		/// <param name="Source">Field source</param>
		/// <param name="ID">Field Id</param>
		void _Rows_AfterAdded(FieldSource Source, string ID)
		{
			Field Field;
            
			Field = Columns[ID];

			if (Field != null)
			{
                // Remove metadata, if parent column is removed from the collection (Indicator, Area, Source)
				if (Field.FieldID == Indicator.IndicatorName || Field.FieldID == Area.AreaID || Field.FieldID == Area.AreaName || Field.FieldID == IndicatorClassifications.ICName)
                {
                    this.RemoveMetadata(this.Columns, Field);
                }
                this._Columns.Remove(Field);
                this.RaiseUpdateColumnAggangementEvent();
			}
			Field = null;
			Field = Available[ID];
			if (Field != null)
			{
                this._Available.Remove(Field);
			}
            this.RaiseChangeSortEvent();
            this.RaiseChangeAggregateFieldEvent(ID, true);
		}       


        #endregion

        #region " -- Events -- "
        
        /// <summary>
        /// Declare the event to update the column arrangement, if there is any change in Column collection
        /// </summary>
        public event UpdateColumnAggangementDelegate UpdateColumnAggangementEvent;

        /// <summary>
        /// Declare the event to update the sort fields (Step 4)
        /// </summary>
        public event ChangeSortDelegate ChangeSortEvent;

        /// <summary>
        /// Declare the event to change the aggregate field.
        /// </summary>
        public event ChangeAggregateFieldDelegate ChangeAggregateFieldEvent;

 
        #endregion

        #endregion
    }						
}
