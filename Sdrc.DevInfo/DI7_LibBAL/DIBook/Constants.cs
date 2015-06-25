using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DevInfo.Lib.DI_LibBAL.DIBookBAL
{
    /// <summary>
    /// Provides  constants for DIBook 
    /// </summary>
    public class Constants
    {
        // folder and files
        public  const string diBooksFolderName = "diBooks";
        public  const string PagesFolderName = "pages";
        public  const string XmlFolderName = "xml";
        public  const string XmlFileName = "Pages.xml";
        public const string HtmlFileName = "Default.html";
        

        // page.xml
        public  const string PageNodeTag = "page";
        public  const string PageNodeAttribute = "src";
                public const string ContentNodeTag = "content";
        public const string HeightAttribute = "height";
        public const string WidthAttribute = "width";

        // page panel
        public  static Size PagePanelSize = new Size(52, 67);
        public  const int PagePanelDistance=3;
        public  static Color PagePanelBackColor = Color.White;
        public  static Point PageNoLabelLocation = new Point(17, 23);
        public  static Size PageNoLabelSize = new Size(16,18);

    }

}
