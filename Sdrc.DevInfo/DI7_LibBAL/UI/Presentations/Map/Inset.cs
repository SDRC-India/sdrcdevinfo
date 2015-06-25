using System;
using System.Drawing;
using System.Collections;
using System.Drawing.Imaging;
using System.Xml.Serialization;
using System.IO;

namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Map
{
    [Serializable()]
    public class Inset
    {

        # region "Private Variables"
        private string m_Name = "";
        private RectangleF m_Extent;
        private bool m_Visible = true;
        # endregion

        # region "Properties"
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        public RectangleF Extent
        {
            get { return m_Extent; }
            set { m_Extent = value; }
        }
        public bool Visible
        {
            get { return m_Visible; }
            set { m_Visible = value; }
        }

        private Image _InsetImage = null;
        /// <summary>
        /// Gets or sets the Inset image object.
        /// </summary>
        [XmlIgnore()]
        public Image InsetImage
        {
            get { return _InsetImage; }
            set { _InsetImage = value; }
        }

        /// <summary>
        /// Gets or sets the Inset image in Base64 string format. 
        /// <para>This is used for Inset image to xml serialize and deserialize</para>
        /// </summary>
        public string InsetImageString
        {
            get 
            {
                if (this._InsetImage != null)
                {
                    MemoryStream ms = new MemoryStream();
                    this._InsetImage.Save(ms, ImageFormat.Png);
                    return Convert.ToBase64String(ms.ToArray());
                }
                else
                {
                    return string.Empty;
                }
            }
            set 
            {
                if (string.IsNullOrEmpty(value))
                {
                    this._InsetImage = null;
                }
                else
                {
                    this._InsetImage = Image.FromStream(new MemoryStream(Convert.FromBase64String(value)));
                }
            }
        }
	

        # endregion

        # region "Methods"

        public Size GetInsetName(string p_Path, string p_FileName)
        {
            string p_FileExt = "emf";
            return GetInsetName(p_Path, p_FileName, p_FileExt);
        }

        public Size GetInsetName(string p_Path, string p_FileName, string p_FileExt)
        {
            Size ImageSize;
            Font Fnt = new Font("Arial", 8);
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(1, 1);
            Graphics g = Graphics.FromImage(bmp);

            //*** Get maximum width of Title / Subtitle
            ImageSize = g.MeasureString(m_Name, Fnt).ToSize();

            ImageSize.Width = ImageSize.Width + 20;
            ImageSize.Height = ImageSize.Height + 20;

            bmp = new System.Drawing.Bitmap(ImageSize.Width, ImageSize.Height);
            g = Graphics.FromImage(bmp);
            switch (p_FileExt.ToLower())
            {
                case "emf":
                    IntPtr hRefDC = g.GetHdc();
                    Metafile m = new Metafile(p_Path + "\\" + p_FileName + ".emf", hRefDC);
                    g.ReleaseHdc(hRefDC);
                    Graphics gMeta = Graphics.FromImage(m);
                    gMeta.DrawString(m_Name, Fnt, Brushes.Black, 10, 10);
                    m.Dispose();
                    gMeta.Dispose();
                    break;
                case "png":
                    g.DrawString(m_Name, Fnt, Brushes.Black, 10, 10);
                    bmp.Save(p_Path + "\\" + p_FileName + ".png", ImageFormat.Png);
                    break;
                case "jpg":
                    g.Clear(Color.White);
                    g.DrawString(m_Name, Fnt, Brushes.Black, 10, 10);
                    bmp.Save(p_Path + "\\" + p_FileName + ".jpg", ImageFormat.Jpeg);
                    break;
                case "bmp":
                    g.Clear(Color.White);
                    g.DrawString(m_Name, Fnt, Brushes.Black, 10, 10);
                    bmp.Save(p_Path + "\\" + p_FileName + ".bmp", ImageFormat.Bmp);
                    break;
                case "gif":
                    g.Clear(Color.White);
                    g.DrawString(m_Name, Fnt, Brushes.Black, 10, 10);
                    bmp.Save(p_Path + "\\" + p_FileName + ".gif", ImageFormat.Gif);
                    break;
                case "tiff":
                    g.DrawString(m_Name, Fnt, Brushes.Black, 10, 10);
                    bmp.Save(p_Path + "\\" + p_FileName + ".tiff", ImageFormat.Tiff);
                    break;
                case "ico":
                    g.DrawString(m_Name, Fnt, Brushes.Black, 10, 10);
                    bmp.Save(p_Path + "\\" + p_FileName + ".ico", ImageFormat.Icon);
                    break;
            }

            bmp.Dispose();
            bmp = null;
            g.Dispose();

            Fnt.Dispose();
            return ImageSize;
        }


        # endregion

    }
}