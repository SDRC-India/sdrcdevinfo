using System;
using System.Collections;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Map
{
    [Serializable()]
    public class Themes : CollectionBase , ICloneable
    {

        public Themes()
            : base()
        {
        }

        public void Add(Theme p_Theme)
        {
            if (p_Theme.Type == ThemeType.Color)
            {

                foreach (Theme _Theme in List)
                {
                    if (_Theme.Type == ThemeType.Color)
                    {
                        _Theme.Visible = false;
                    }
                }
                p_Theme.Visible = true;
                List.Insert(0, p_Theme);

                //*** Always Add New Color Theme at 0th index
            }
            else
            {
                List.Add(p_Theme);
            }
        }

        public void Remove(int p_Index)
        {
            List.RemoveAt(p_Index);
        }

        public void Insert(int p_Position, Theme p_Theme)
        {
            List.Insert(p_Position, p_Theme);
        }

        public Theme this[int p_Index]
        {
            get
            {
                if (p_Index < 0)
                    return null;
                return (Theme)List[p_Index];

            }
            set { List[p_Index] = value; }
        }

        public Theme this[string p_Key]
        {

            get
            {
                Theme RetVal = null;
                foreach (Theme _Theme in List)
                {
                    if (_Theme.ID == p_Key)
                    {
                        RetVal = _Theme;
                        break;
                    }
                }
                return RetVal;
            }
        }

        //Property "ItemIndex" is converted into Function : becoz c# do not allow to pass parameter to any property.
        public int ItemIndex(string p_Key)
        {
            int RetVal = -1;
            for (int i = 0; i <= List.Count - 1; i++)
            {
                if (((Theme)List[i]).ID == p_Key)
                {
                    RetVal = i;
                    break;
                }
            }
            return RetVal;

        }

        //*** Returns Active Color Theme
        public Theme GetActiveTheme()
        {
            Theme RetVal = null;

            foreach (Theme _Theme in List)
            {
                if (_Theme.Type == ThemeType.Color & _Theme.Visible == true)
                {
                    RetVal = _Theme;
                    break;
                }
            }
            return RetVal;
        }

        public void MoveToTop(int p_Index)
        {
            Move(p_Index, 0);
        }

        public void MoveToBottom(int p_Index)
        {
            Move(p_Index, List.Count - 1);
        }

        public void Move(int p_FromIndex, int p_ToIndex)
        {
            if (p_FromIndex < p_ToIndex)
            {
                if (p_ToIndex > 0)
                {
                    List.Insert(p_ToIndex + 1, List[p_FromIndex]);
                    List.RemoveAt(p_FromIndex);
                }
            }
            else
            {
                if (p_ToIndex < List.Count - 1)
                {
                    List.Insert(List.Count - 1, List[p_FromIndex]);
                    List.RemoveAt(p_FromIndex);
                }
            }
        }

        public object Clone()
        {
            object RetVal = null;
            try
            {
                //*** Serialization is one way to do deep cloning. It works only if the objects and its references are serializable
                //BinaryFormatter oBinaryFormatter = new BinaryFormatter();
                XmlSerializer oXmlSerializer = new XmlSerializer(typeof(Themes));
                MemoryStream oMemStream = new MemoryStream();
                oXmlSerializer.Serialize(oMemStream, this);
                oMemStream.Position = 0;
                //string text = new StreamReader(oMemStream).ReadToEnd();
                RetVal = (Themes)oXmlSerializer.Deserialize(oMemStream);
                oMemStream.Close();
                oMemStream.Dispose();
                oMemStream = null;
            }
            catch
            {

            }
            return (Themes)RetVal;
        }

    }
}