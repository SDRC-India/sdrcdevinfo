using System;
using System.Text;
using System.Collections;

namespace DevInfo.Lib.DI_LibBAL.Utility
{
    public enum SortDirection
    {
        ASC = -1,
        DESC = 1
    }

    /// <summary>
    /// This is a simple collection class which both supports Generics as well as Sorting.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>Creating a Generic Collection Class with Sorting Support in .NET 2.0. http://kylefinley.net/archive/2006/02/08/41.aspx </remarks>
    public class GenericCollection<T> : System.Collections.CollectionBase
    {
        public virtual T this[int index]
        {
            get { return (T)this.List[index]; }
            set { this.List[index] = value; }
        }


        /// <summary>
        /// Get the BottomPanelButton on the basis of Button Id (case insensetive)
        /// </summary>
        /// <param name="ButtonId">Button Id. case Insensetive</param>
        /// <returns>BottomPanelButton</returns>
        public virtual T this[string IDField, string Id]
        {
            get
            {
                try
                {
                    for (int i = 0; i < this.List.Count; i++)
                    {
                        object ListItem = this.List[i];
                        object ItemId = ListItem.GetType().GetProperty(IDField).GetValue(ListItem, null);
                        if (string.Compare(Id, ItemId.ToString(), true) == 0)
                        {
                            return (T)this.List[i];
                        }
                    }

                    return default(T);

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.Message);
                    return default(T);
                }
            }
        }

        public virtual int IndexOf(T item)
        {
            return this.List.IndexOf(item);
        }

        public virtual int Add(T item)
        {
            return this.List.Add(item);
        }

        public virtual void Remove(T item)
        {
            this.List.Remove(item);
        }

        public virtual void CopyTo(Array array, int index)
        {
            this.List.CopyTo(array, index);
        }

        public virtual void AddRange(GenericCollection<T> collection)
        {
            this.InnerList.AddRange(collection);
        }

        public virtual void AddRange(T[] collection)
        {
            this.InnerList.AddRange(collection);
        }

        public virtual bool Contains(T item)
        {
            return this.List.Contains(item);
        }

        public virtual void Insert(int index, T item)
        {
            this.List.Insert(index, item);
        }

        public void Sort(string sortExpression, SortDirection sortDirection)
        {
            InnerList.Sort(new Comparer(sortExpression, sortDirection));
        }

        public void Sort(string sortExpression)
        {
            InnerList.Sort(new Comparer(sortExpression));
        }



    }

    /// <summary>
    /// The Comparer class provides the ability to specify which property will be used for sorting, as well as compare two objects based on that property value.
    /// Since the collection will be sorted based on object property values, reflection will used to provide access to the value of the property name specified by the caller
    /// </summary>
    public class Comparer   : IComparer
    {
        string m_SortPropertyName = string.Empty;
        SortDirection m_SortDirection = SortDirection.ASC;
        

        public Comparer(string sortPropertyName)
        {
            this.m_SortPropertyName = sortPropertyName;
            // default to ascending order
            this.m_SortDirection = SortDirection.ASC;            
        }

        public Comparer(string sortPropertyName, SortDirection sortDirection)
        {
            this.m_SortPropertyName = sortPropertyName;
            this.m_SortDirection = sortDirection;
        }

        public int Compare(object x, object y)
        {
            // Get the values of the relevant property on the x and y objects

            object valueOfX = x.GetType().GetProperty(m_SortPropertyName).GetValue(x, null);
            object valueOfY = y.GetType().GetProperty(m_SortPropertyName).GetValue(y, null);

            IComparable comp = valueOfY as IComparable;

            // Flip the value from whatever it was to the opposite.
            return Flip(comp.CompareTo(valueOfX));
        }

        private int Flip(int i)
        {
            return (i * (int)m_SortDirection);
        }
    }

}
