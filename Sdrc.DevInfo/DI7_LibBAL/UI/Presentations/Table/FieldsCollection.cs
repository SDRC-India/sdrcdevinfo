using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using DevInfo.Lib.DI_LibBAL.UI.Presentations.Common.TableGraph;

namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Table
{

	/// <summary>
	/// Contains the event which get raised when any field is get addfed in the collection.
	/// </summary>
	/// <param name="Source">The collection where the item is to be aded</param>
	/// <param name="ID">Field ID</param>
	public delegate void FieldAddedDelegate(FieldSource Source, string ID);

    public delegate void ColumnOrderChangeDelegate();
	
	/// <summary>
	/// Defines the collection for field objects.
	/// </summary>
	public class FieldsCollection : System.Collections.CollectionBase
	{
		
		#region "-- Private -"

		#region "-- Methods --"

		#region "-- RaiseEvent --"

		/// <summary>
		/// Defines when an event is raised.
		/// </summary>
		/// <param name="Source"></param>
		/// <param name="ID"></param>
		private void RaiseAfterAddedEvent(FieldSource Source, string ID)
		{
			if (this.AfterAdded != null)
			{
				this.AfterAdded(Source, ID);
			}
		}

        /// <summary>
        /// Raise event to update column arrangement when column fields order are changed
        /// </summary>
        private void RaiseColumnOrderChangeEvent()
        {
            if (this.ColumnOrderChangeEvent != null)
            {
                this.ColumnOrderChangeEvent();
            }
        }

		#endregion

		#endregion

		#endregion

		#region "-- Public --"

		#region "-- Properties and Variables--"

        [XmlIgnore()]
		private FieldSource _Source;
		/// <summary>
		///	 Get the destination of field
		/// </summary>
		public FieldSource Source
		{
			get
			{
				return this._Source;
			} 
		}

		
		#endregion

		#region "-- Constructor --"

		/// <summary>
		/// Constructor
		/// </summary>
		public FieldsCollection()
		{
			// dont implement this
		}
		
		/// <summary>
		/// Initializes _destination with the field destination
		/// </summary>
		/// <param name="Destination">Field destination of type FieldDestination</param>
		public FieldsCollection(FieldSource Source)
		{
			_Source = Source;
		}

		#endregion

		#region "-- Methods --"

		/// <summary>
		/// Add a Field to the fields collection.
		/// </summary>
		/// <param name="Field">Field object</param>
		public void Add(Field Field)
		{
			this.InnerList.Add(Field);		
			//raise the event	
			this.RaiseAfterAddedEvent(this._Source, Field.FieldID);
		}	

        /// <summary>
        /// Returns the field object based on field Id
        /// <remarks>Indexer is create with int parameter to serialize the class</remarks>
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Field object</returns>
        public Field this[int id]
        {
            get
            {
                Field RetVal = null;
                try
                {                    
                    RetVal = (Field)this.InnerList[id];
                    RetVal.FieldIndex = id;
                }
                catch (Exception ex)
                {
                    RetVal = null;
                }
                return RetVal;
            }
        }    

        /// <summary>
        /// Returns the field object based on field Id
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>Field object</returns>
        public Field this[string id]
        {
            get
            {
                Field RetVal = null;
                int Index = 0;
                try
                {
                    foreach (Field Field in InnerList)
                    {
                        if (Field.FieldID == id)
                        {
                            RetVal = Field;
                            RetVal.FieldIndex = Index;
                            break;
                        }
                        Index++;
                    }
                }
                catch (Exception)
                {
                    RetVal = null;
                }
                return RetVal;
            } 
        }


        internal bool Exists(string ID)
        {
            bool RetVal = false;
            try
			{
				foreach (Field Field in InnerList)
				{
					if (Field.FieldID == ID)
					{
						RetVal = true;
						break;
					}
				}
			}
            catch (Exception)
            {
                
            }

            return RetVal;
        }

		/// <summary>
		/// Get caption of field object on the basis of field ID
		/// </summary>
		/// <param name="ID"></param>
		/// <returns></returns>
		internal string GetCaption(string ID)
		{
			string RetVal = string.Empty;
			try
			{
				foreach (Field Field in InnerList)
				{
					if (Field.FieldID == ID)
					{
						RetVal = Field.Caption;
						break;
					}
				}
			}
			catch (Exception)
			{
				RetVal = string.Empty;
			}
			return RetVal;			
		}

        /// <summary>
        /// Insert the field in the collection at the index
        /// </summary>
        /// <param name="index">index</param>
        /// <param name="field">field</param>
        internal void Insert(int index,Field field)
        {
            this.InnerList.Insert(index, field);
        }

        /// <summary>
        /// Move the field up by 1 index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="field"></param>
        public void MoveUp(int index,Field field)
        {
            this.InnerList.Insert(index - 1, field);
            this.InnerList.RemoveAt(index + 1);
            if (this._Source == FieldSource.Column)
            {
                this.RaiseColumnOrderChangeEvent();
            }
        }

        /// <summary>
        /// Move the field down by 1 index
        /// </summary>
        /// <param name="index">Index of the field</param>
        /// <param name="field"></param>
        public void MoveDown(int index, Field field)
        {
            this.InnerList.Insert(index + 2, field);
            this.InnerList.RemoveAt(index);
            if (this._Source == FieldSource.Column)
            {
                this.RaiseColumnOrderChangeEvent();
            }
        }

        /// <summary>
        /// Move the field on the top of the collection.
        /// </summary>
        /// <param name="field"></param>
        public void MoveToTop(Field field)
        {
            this.InnerList.Remove(field);
            this.InnerList.Insert(0,field);
        }

        /// <summary>
        /// Move the field on the top of the collection.
        /// </summary>
        /// <param name="field"></param>
        public void MoveToEnd(Field field)
        {
            this.InnerList.Remove(field);
            this.InnerList.Insert(this.Count, field);
        }

        /// <summary>
        /// Delete the field object from the collection
        /// </summary>
        /// <param name="Field"></param>
        public void Remove(Field Field)
        {
            InnerList.Remove(Field);            
        }

        /// <summary>
        /// Remove the range from the collection.
        /// </summary>
        /// <param name="index">Strarting index range</param>
        /// <param name="count">Number of elements to be removed.</param>
        public void RemoveRange(int index, int count)
        {
            this.InnerList.RemoveRange(index, count);
        }

        /// <summary>
        /// Get Fieldid of field object on the basis of caption
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string GetFieldId(string caption)
        {
            string RetVal = string.Empty;
            try
            {
                foreach (Field Field in InnerList)
                {
                    if (Field.Caption.ToLower() == caption.ToLower())
                    {
                        RetVal = Field.FieldID;
                        break;
                    }
                }
            }
            catch (Exception)
            {
                RetVal = string.Empty;
            }
            return RetVal;
        }

		#endregion	
 
        #region " -- Events --"

        /// <summary>
        /// Event declaration
        /// </summary>
        public event FieldAddedDelegate AfterAdded;

        /// <summary>
        /// Declare event to update column arrangement when column fields order are changed
        /// </summary>
        public event ColumnOrderChangeDelegate ColumnOrderChangeEvent;

        #endregion

        #endregion

    }  
}
