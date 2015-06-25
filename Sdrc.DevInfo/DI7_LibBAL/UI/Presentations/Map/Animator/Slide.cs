using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Map.Animator
{
    /// <summary>
    /// Store Details about an Individual Slide of Swf File
    /// </summary>
    public class Slide
    {
        #region " -- Public -- "
        #region " -- Properties -- "

        private string _TitleImagePath;
        /// <summary>
        /// Gets or sets the path of image displayed at top of the Slide
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Title and Subtitle</remarks>
        public string TitleImagePath
        {
            get { return _TitleImagePath; }
            set { _TitleImagePath = value; }
        }
     
        private string _MapImagePath;
        /// <summary>
        /// Gets or sets the MapImage path 
        /// </summary>
        /// <value>File name with extension</value>
        /// <returns></returns>
        /// <remarks>Main Map Image
        /// A caption with this file name shall be displayed at the top
        /// </remarks>
        public string MapImagePath
        {
            get { return _MapImagePath; }
            set { _MapImagePath = value; }
        }

        private string _LegendImagePath;
        /// <summary>
        ///  Get/Set the path of Legend image 
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string LegendImagePath
        {
            get { return _LegendImagePath; }
            set { _LegendImagePath = value; }
        }
        
        private string[] _Source;
        /// <summary>
        ///  Gets or sets the Source name array
        /// </summary>
        public string[] Source
        {
            get { return _Source; }
            set { _Source = value; }
        }

        private string _DisclaimerImagePath;
        /// <summary>
        /// Gets or sets the path of disclaimer image 
        /// </summary>
        public string DisclaimerImagePath
        {
            get { return _DisclaimerImagePath; }
            set { _DisclaimerImagePath = value; }
        }

        private string _MapYear;
        /// <summary>
        /// Gets or sets the Year for the Image 
        /// </summary>
        public string MapYear
        {
            get { return _MapYear; }
            set { _MapYear = value; }
        }

        #endregion

        #region " -- Constructor -- "
        /// <summary>
        /// Slide Constructor - Set each detail for a Slide 
        /// </summary>
        /// <param name="mapYear">Year corresponding to the Map</param>
        /// <param name="titleImageFile">Title image file name with extention</param>
        /// <param name="mapImageFile">Map image file name with extention</param>
        /// <param name="legendImageFile">Legend image file name with extention</param>
        /// <param name="source">List containing name of the sources</param>
        /// <param name="disclaimerImageFile">Disclaimer image file name with extention</param>
        public Slide(string mapYear, string titleImageFile, string mapImageFile, string legendImageFile, string[] source, string disclaimerImageFile)
        {
            _MapYear = mapYear;
            _TitleImagePath = titleImageFile;
            _MapImagePath = mapImageFile;
            _LegendImagePath = legendImageFile;
            _Source = source;
            _DisclaimerImagePath = disclaimerImageFile;
        }

        #endregion

        #endregion
    }
}
