using System;
//using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Collections;

namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Map
{
    [Serializable()]
    public class Insets : CollectionBase
    {
        //$$$ -1 implies that no inset is currently active and Default map is to be shown
        private int m_SelIndex = -1;

        public int SelIndex
        {
            get { return m_SelIndex; }
            set { m_SelIndex = value; }
        }



        public Insets()
            : base()
        {
        }
        public void Add(Inset p_Inset)
        {
            List.Add(p_Inset);
            m_SelIndex = List.Count - 1;
        }

        public void Remove(Inset p_Inset)
        {
            if (List.IndexOf(p_Inset) == m_SelIndex)
                m_SelIndex = -1;
            List.Remove(p_Inset);
        }

        public new void RemoveAt(int p_Index)
        {
            if (p_Index == m_SelIndex)
            {
                m_SelIndex = -1;
                //*** If Currently active Inset is deleted then set the SelIndex to -1
            }
            else if (p_Index < m_SelIndex)
            {
                m_SelIndex = m_SelIndex - 1;
            }
            List.RemoveAt(p_Index);

            if (List.Count == 0)
                m_SelIndex = -1;
            //*** If all inset are deleted set the SelIndex to -1
        }

        public new int Count
        {
            get { return List.Count; }
        }

        public Inset this[int p_Index]
        {
            get { return (Inset)List[p_Index]; }
        }

        public Inset this[string p_Name]
        {
            get
            {
                foreach (Inset _Inset in List)
                {
                    if (_Inset.Name == p_Name)
                    {
                        return _Inset;
                    }
                }
                return null;
            }
        }

    }
}