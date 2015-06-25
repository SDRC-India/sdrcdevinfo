using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml;
using System.IO;

namespace DevInfo.Lib.DI_LibBAL.DIBookBAL
{
    /// <summary>
    /// Provides information about diBook
    /// </summary>
    public class DIBookInfo
    {
        #region "-- Static --"

        internal static Dictionary<int, string> GetAllPages(string diBookPath)
        {
            Dictionary<int, string> RetVal = new Dictionary<int, string>();
            string PagesXmlFileNameWPath = string.Empty;
            XmlDocument XmlFile;
            XmlElement RootElement;
            XmlNodeList NodeList;
            string PageName = string.Empty;
            int PageNo = 1;

            try
            {
                PagesXmlFileNameWPath = diBookPath + "\\" + Constants.XmlFolderName + "\\" + Constants.XmlFileName;

                // check file exists or not
                if (File.Exists(PagesXmlFileNameWPath))
                {
                    // get pages information
                    XmlFile = new XmlDocument();
                    XmlFile.Load(PagesXmlFileNameWPath);

                    if (XmlFile.HasChildNodes)
                    {
                        RootElement = XmlFile.DocumentElement;
                        foreach (XmlNode Node in RootElement.ChildNodes)
                        {
                            if (Node.Name.ToLower() == Constants.PageNodeTag)
                            {
                                PageName = Node.Attributes[Constants.PageNodeAttribute].Value.ToString().ToLower();

                                // add pagename into page collection
                                if (!RetVal.ContainsValue(PageName))
                                {
                                    RetVal.Add(PageNo, PageName);
                                    PageNo++;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

            return RetVal;
        }

        

        #endregion

        #region "-- private --"

        #region "-- methods --"

        private Size GetImageSize(string fileNameWPath)
        {
            Size RetVal=new Size();
            Image ImageFile = null;

            try
            {
                if (File.Exists(fileNameWPath))
                {
                    ImageFile = Image.FromFile(fileNameWPath);
                    RetVal = ImageFile.Size;
                    ImageFile.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

            return RetVal;
        }

        #endregion

        #endregion

        #region "-- public --"

        #region "-- Variables / Properties --"



        private Dictionary<int, string> _Pages = new Dictionary<int, string>();
        /// <summary>
        /// Gets or sets collection of pages. Key is page no and value is page file path
        /// </summary>
        public Dictionary<int, string> Pages
        {
            get { return this._Pages; }
            set { this._Pages = value; }
        }

        private string _FolderPath = string.Empty;
        /// <summary>
        /// Gets or sets diBook's folder path
        /// </summary>
        public string FolderPath
        {
            get
            {
                return this._FolderPath;
            }
            set
            {
                this._FolderPath = value;
            }
        }


        private string _CoverPagePath = string.Empty;
        /// <summary>
        /// Gets cover page path
        /// </summary>
        public string CoverPagePath
        {
            get
            {
                if (this._Pages.Count > 0)
                {
                    this._CoverPagePath = this._Pages[1];
                }
                else
                {
                    this._CoverPagePath = string.Empty;
                }
                return this._CoverPagePath;
            }
        }

        #endregion


        #region "-- New/Dispose --"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="diBookPath"></param>
        /// <param name="pages"></param>
        public DIBookInfo(string diBookPath, Dictionary<int, string> pages)
        {
            this._FolderPath = diBookPath;
            this._Pages = pages;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="diBookPath"></param>
        public DIBookInfo(string diBookPath): this(diBookPath,DIBookInfo.GetAllPages(diBookPath))
        {
        }
        #endregion


        #region "-- Methods --"

        /// <summary>
        /// Returns the paper size
        /// </summary>
        /// <returns></returns>
        public Size GetPaperSize()
        {
            Size RetVal = new Size(500, 600);
            XmlDocument XmlDoc;
            XmlTextReader XmlFile;
            string PageXmlFilePath = string.Empty;
            int Height = 600;
            int Width = 500;

            try
            {
                PageXmlFilePath = this._FolderPath + "\\" + Constants.XmlFolderName + "\\" + Constants.XmlFileName;

                if (File.Exists(PageXmlFilePath))
                {
                    // load file
                    XmlDoc = new XmlDocument();
                    XmlDoc.Load(PageXmlFilePath);

                    // get size
                    foreach (XmlElement Element in XmlDoc.GetElementsByTagName(Constants.ContentNodeTag))
                    {
                        Width = Convert.ToInt32(Element.GetAttribute(Constants.WidthAttribute));
                        Height = Convert.ToInt32(Element.GetAttribute(Constants.HeightAttribute));
                        break;
                    }

                    // set size
                    RetVal.Width = Width;
                    RetVal.Height = Height;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
            
            return RetVal;
        }

        /// <summary>
        /// Gets pages and  images size.
        /// </summary>
        public Dictionary<int, Size> GetPagesImageSize()
        {
            Dictionary<int, Size> RetVal = new Dictionary<int, Size>();
            string ImageFileName = string.Empty;
            Size ImageSize;

            try
            {
                foreach (int PageNo in this._Pages.Keys)
                {
                    if (!string.IsNullOrEmpty(this._Pages[PageNo]))
                    {
                        ImageFileName = Path.Combine(this._FolderPath, this._Pages[PageNo]);
                        RetVal.Add(PageNo, this.GetImageSize(ImageFileName));
                    }
                }

            }
            catch (Exception ex)
            {                
                throw new ApplicationException(ex.ToString());
            }            

            return RetVal; 
        }

        
        #endregion

        #endregion

    }
}
