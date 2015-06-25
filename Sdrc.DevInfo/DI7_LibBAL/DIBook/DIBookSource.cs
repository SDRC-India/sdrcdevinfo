using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Drawing;



namespace DevInfo.Lib.DI_LibBAL.DIBookBAL
{
    public class DIBookSource
    {
        #region "-- private --"

        #region "-- Methods --"

        private void LoadBooks(string diBooksFolderPath)
        {
            DirectoryInfo RootDir = new DirectoryInfo(diBooksFolderPath);
            string DIBookFolderPath = string.Empty;

            try
            {
                this._DIBooks.Clear();

                foreach (DirectoryInfo DIBookPath in RootDir.GetDirectories())
                {
                    DIBookFolderPath = DIBookPath.FullName.ToLower();

                    this.AddDIBook(DIBookFolderPath);
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        private void AddDIBook(string diBookFolderPath)
        {
            DIBookInfo NewDIBook;
            Dictionary<int, string> Pages;

            try
            {
                if (!this._DIBooks.ContainsKey(diBookFolderPath))
                {
                    Pages = this.GetAllPages(diBookFolderPath);
                    NewDIBook = new DIBookInfo(diBookFolderPath, Pages);

                    this._DIBooks.Add(diBookFolderPath, NewDIBook);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        private Dictionary<int, string> GetAllPages(string diBookPath)
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
                                if (!RetVal.ContainsKey(PageNo))
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

        #region "-- Page xml file--"

        private void UpdatePageXmlFile(DIBookInfo diBook, Size pageSize)
        {
            // update pages
            XmlTextWriter XmlFile;
            string PageXmlFilePath = string.Empty;
            try
            {
                PageXmlFilePath = diBook.FolderPath + "\\" + Constants.XmlFolderName + "\\" + Constants.XmlFileName;

                if (File.Exists(PageXmlFilePath))
                {
                    File.Delete(PageXmlFilePath);
                }

                XmlFile = new XmlTextWriter(PageXmlFilePath, null);
                XmlFile.WriteStartDocument();

                this.AddContentTag(XmlFile, pageSize);

                for (int Index = 1; Index <= diBook.Pages.Values.Count; Index++)
                {
                    this.AddPageTag(XmlFile, diBook.Pages[Index]);
                }

                // close content tag
                XmlFile.WriteEndElement();

                XmlFile.WriteEndDocument();

                XmlFile.Close();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        private void AddContentTag(XmlTextWriter xmlFile, Size pageSize)
        {
            string Width = "570";
            string Height = "754";

            if (pageSize != null)
            {
                Width = pageSize.Width.ToString();
                Height = pageSize.Height.ToString();
            }

            xmlFile.WriteStartElement("content");

            // width :" width="570" 
            xmlFile.WriteStartAttribute("width");
            xmlFile.WriteValue(Width);
            xmlFile.WriteEndAttribute();

            // height :height="754" 
            xmlFile.WriteStartAttribute("height");
            xmlFile.WriteValue(Height);
            xmlFile.WriteEndAttribute();

            // bgcolor :bgcolor="047391" 
            xmlFile.WriteStartAttribute("bgcolor");
            xmlFile.WriteValue("047391");
            xmlFile.WriteEndAttribute();

            // loadercolor :loadercolor="ffffff"
            xmlFile.WriteStartAttribute("loadercolor");
            xmlFile.WriteValue("ffffff");
            xmlFile.WriteEndAttribute();

            // panelcolor :panelcolor ="5d5d61"
            xmlFile.WriteStartAttribute("panelcolor");
            xmlFile.WriteValue("5d5d61");
            xmlFile.WriteEndAttribute();

            // buttoncolor :buttoncolor="5d5d61"
            xmlFile.WriteStartAttribute("buttoncolor");
            xmlFile.WriteValue("5d5d61");
            xmlFile.WriteEndAttribute();

            // textcolor :textcolor="FFFF00"
            xmlFile.WriteStartAttribute("textcolor");
            xmlFile.WriteValue("FFFF00");
            xmlFile.WriteEndAttribute();


        }

        private void AddPageTag(XmlTextWriter xmlFile, string srcFile)
        {
            //<page src="pages/pg1.jpg"/>

            xmlFile.WriteStartElement("page");
            xmlFile.WriteStartAttribute("src");
            xmlFile.WriteValue(srcFile.Replace("//", "/"));
            xmlFile.WriteEndAttribute();

            xmlFile.WriteEndElement();
        }

        #endregion

        #endregion

        #endregion

        #region "-- Public --"

        #region "-- Variables/Properties --"

        private Dictionary<string, DIBookInfo> _DIBooks = new Dictionary<string, DIBookInfo>();
        /// <summary>
        /// Gets or sets collection of diBook. Key is diBook name with path and value is instance of diBookInfo which provides information about dibook
        /// </summary>
        public Dictionary<string, DIBookInfo> DIBooks
        {
            get { return this._DIBooks; }
            set { this._DIBooks = value; }
        }

        #endregion

        #region "-- New/Dispose --"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="folderPath"></param>
        public DIBookSource(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                this.LoadBooks(folderPath);
            }
        }

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Returns the cover page filename with diBook folder path. Key : diBookFolder path and value is cover page file name.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetCoverPages()
        {
            Dictionary<string, string> RetVal = new Dictionary<string, string>();

            foreach (string DIBookPath in this.DIBooks.Keys)
            {
                if (!RetVal.ContainsKey(DIBookPath))
                {
                    if (File.Exists(DIBookPath + "\\" + this.DIBooks[DIBookPath].CoverPagePath))
                    {
                        RetVal.Add(DIBookPath, DIBookPath + "\\" + this.DIBooks[DIBookPath].CoverPagePath);
                    }
                }
            }

            return RetVal;
        }

        /// <summary>
        /// Returns new DIBook
        /// </summary>
        /// <param name="diBookPath"></param>
        /// <returns></returns>
        public DIBookInfo GetNewDIBook(string diBookPath)
        {
            DIBookInfo RetVal;
            Dictionary<int, string> Pages = new Dictionary<int, string>();
            Pages.Add(1, string.Empty);
            Pages.Add(2, string.Empty);
            Pages.Add(3, string.Empty);
            Pages.Add(4, string.Empty);
            RetVal = new DIBookInfo(diBookPath, Pages);

            return RetVal;
        }

        /// <summary>
        /// Saves the diBook
        /// </summary>
        /// <param name="diBook"></param>
        public void SaveDIBook(DIBookInfo diBook, Size pageSize)
        {
            // update pages.xml
            this.UpdatePageXmlFile(diBook, pageSize);

        }

        /// <summary>
        /// Returns true/false. Ture if all page path exists otherwise false
        /// </summary>
        /// <param name="diBook"></param>
        /// <returns></returns>
        public bool IsValidBook(DIBookInfo diBook)
        {
            bool RetVal = true;

            foreach (string PageFilePath in diBook.Pages.Values)
            {
                if (!File.Exists(PageFilePath))
                {
                    RetVal = false;
                    break;
                }
            }

            return RetVal;
        }

        #endregion

        #endregion

     

    }
}
